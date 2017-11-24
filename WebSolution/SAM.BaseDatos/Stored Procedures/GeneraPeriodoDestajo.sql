IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GeneraPeriodoDestajo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GeneraPeriodoDestajo]
GO
/****************************************************************************************
	Nombre:		GeneraPeriodoDestajo
	Funcion:	Genera el periodo de destajo especificado.  Lleva a cabo bulk inserts
				para los tuberos y soldadores
	Parametros:	@Semana nvarchar(5)
				@Anio int
				@FechaInicio datetime
				@FechaFin datetime
				@CantidadDiasFestivos int
				@Tuberos xml
				@Soldadores xml
				@UsuarioModifica uniqueidentifier
	Autor:		IHM
	Modificado:	18/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[GeneraPeriodoDestajo]
(
	@Semana nvarchar(5),
	@Anio int,
	@FechaInicio datetime,
	@FechaFin datetime,
	@CantidadDiasFestivos int,
	@Tuberos xml,
	@Soldadores xml,
	@UsuarioModifica uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT ON;
	
	declare @PeriodoDestajoID int
	
	insert into PeriodoDestajo
	(
		Semana,
		Anio,
		FechaInicio,
		FechaFin,
		CantidadDiasFestivos,
		Aprobado,
		UsuarioModifica,
		FechaModificacion
	)
	values
	(
		@Semana,
		@Anio,
		@FechaInicio,
		@FechaFin,
		@CantidadDiasFestivos,
		cast(0 as bit),
		@UsuarioModifica,
		GETDATE()
	)
	
	set @PeriodoDestajoID = SCOPE_IDENTITY()
	
	CREATE TABLE #TmpTuberos
	(
		DestajoTuberoID int,
		TuberoID int
	)
	
	CREATE TABLE #TmpSoldadores
	(
		DestajoSoldadorID int,
		SoldadorID int
	)
	
	-- Por algún motivo desconocido hacer un bulk insert directo de XML es tardadísimo, así que
	-- primero los vaciamos a tablas temporales
	select	 destajo.value('TuberoID[1]','int') [TuberoID]
			,destajo.value('ReferenciaCuadro[1]','money') [ReferenciaCuadro]
			,destajo.value('CostoDiaFestivo[1]','money') [CostoDiaFestivo]
			,destajo.value('TotalDestajo[1]','money') [TotalDestajo]
			,destajo.value('TotalCuadro[1]','money') [TotalCuadro]
			,destajo.value('TotalDiasFestivos[1]','money') [TotalDiasFestivos]
			,destajo.value('TotalOtros[1]','money') [TotalOtros]
			,destajo.value('TotalAjuste[1]','money') [TotalAjuste]
			,destajo.value('GranTotal[1]','money') [GranTotal]
	into #TuberosXml
	from @Tuberos.nodes('/ArrayOfDestajoArmadoGrupo[1]/DestajoArmadoGrupo') as DestajoTubero(destajo)

	select	 jta.value('JuntaWorkstatusID[1]','int') [JuntaWorkstatusID]
			,jta.value('ProyectoID[1]','int') [ProyectoID]
			,jta.value('Diametro[1]','decimal(7,4)') [PDIs]
			,jta.value('CostoUnitario[1]','money') [CostoUnitario]
			,jta.value('Destajo[1]','money') [Destajo]
			,jta.value('ProrrateoCuadro[1]','money') [ProrrateoCuadro]
			,jta.value('ProrrateoDiasFestivos[1]','money') [ProrrateoDiasFestivos]
			,jta.value('ProrrateoOtros[1]','money') [ProrrateoOtros]
			,jta.value('Total[1]','money') [Total]
			,jta.value('EsDePeriodoAnterior[1]','bit') [EsDePeriodoAnterior]
			,jta.value('CostoDestajoVacio[1]','bit') [CostoDestajoVacio]
			,jta.value('TuberoID[1]','int') [TuberoID]
	into #TmpJuntasTuberosXml
	from @Tuberos.nodes('/ArrayOfDestajoArmadoGrupo[1]/DestajoArmadoGrupo/juntas/junta') as Juntas(jta)	

	select	 destajo.value('SoldadorID[1]','int') [SoldadorID]
			,destajo.value('ReferenciaCuadro[1]','money') [ReferenciaCuadro]
			,destajo.value('CostoDiaFestivo[1]','money') [CostoDiaFestivo]
			,destajo.value('TotalDestajoRaiz[1]','money') [TotalDestajoRaiz]
			,destajo.value('TotalDestajoRelleno[1]','money') [TotalDestajoRelleno]
			,destajo.value('TotalCuadro[1]','money') [TotalCuadro]
			,destajo.value('TotalDiasFestivos[1]','money') [TotalDiasFestivos]
			,destajo.value('TotalOtros[1]','money') [TotalOtros]
			,destajo.value('TotalAjuste[1]','money') [TotalAjuste]
			,destajo.value('GranTotal[1]','money') [GranTotal]
	into #SoldadoresXml
	from @Soldadores.nodes('/ArrayOfDestajoSoldaduraGrupo[1]/DestajoSoldaduraGrupo') as DestajoSoldador(destajo)
	
	select	 jta.value('JuntaWorkstatusID[1]','int') [JuntaWorkstatusID]
			,jta.value('ProyectoID[1]','int') [ProyectoID]
			,jta.value('Diametro[1]','decimal(7,4)') [PDIs]
			,jta.value('CostoUnitarioRaiz[1]','money') [CostoUnitarioRaiz]
			,jta.value('CostoUnitarioRelleno[1]','money') [CostoUnitarioRelleno]
			,jta.value('RaizDividida[1]','bit') [RaizDividida]
			,jta.value('RellenoDividido[1]','bit') [RellenoDividido]
			,jta.value('NumeroFondeadores[1]','int') [NumeroFondeadores]
			,jta.value('NumeroRellenadores[1]','int') [NumeroRellenadores]
			,jta.value('DestajoRaiz[1]','money') [DestajoRaiz]
			,jta.value('DestajoRelleno[1]','money') [DestajoRelleno]
			,jta.value('ProrrateoCuadro[1]','money') [ProrrateoCuadro]
			,jta.value('ProrrateoDiasFestivos[1]','money') [ProrrateoDiasFestivos]
			,jta.value('ProrrateoOtros[1]','money') [ProrrateoOtros]
			,jta.value('Total[1]','money') [Total]
			,jta.value('EsDePeriodoAnterior[1]','bit') [EsDePeriodoAnterior]
			,jta.value('CostoRaizVacio[1]','bit') [CostoRaizVacio]
			,jta.value('CostoRellenoVacio[1]','bit') [CostoRellenoVacio]
			,jta.value('SoldadorID[1]','int') [SoldadorID]
	into #TmpJuntasSoldadoresXml
	from @Soldadores.nodes('/ArrayOfDestajoSoldaduraGrupo[1]/DestajoSoldaduraGrupo/juntas/junta') as Juntas(jta)	
	
	insert into DestajoTubero
	(
		TuberoID,
		PeriodoDestajoID,
		ReferenciaCuadro,
		CantidadDiasFestivos,
		CostoDiaFestivo,
		TotalDestajo,
		TotalCuadro,
		TotalDiasFestivos,
		TotalOtros,
		TotalAjuste,
		GranTotal,
		Aprobado,
		UsuarioModifica,
		FechaModificacion
	)
	output inserted.DestajoTuberoID, inserted.TuberoID into #TmpTuberos
	select	TuberoID,
			@PeriodoDestajoID,
			ReferenciaCuadro,
			@CantidadDiasFestivos,
			CostoDiaFestivo,
			TotalDestajo,
			TotalCuadro,
			TotalDiasFestivos,
			TotalOtros,
			TotalAjuste,
			GranTotal,
			CAST(0 as bit),
			@UsuarioModifica,
			GETDATE()
	from #TuberosXml

	insert into DestajoTuberoDetalle
	(
		DestajoTuberoID,
		JuntaWorkstatusID,
		ProyectoID,
		PDIs,
		CostoUnitario,
		Destajo,
		ProrrateoCuadro,
		ProrrateoDiasFestivos,
		ProrrateoOtros,
		Ajuste,
		Total,
		EsDePeriodoAnterior,
		CostoDestajoVacio,
		UsuarioModifica,
		FechaModificacion
	)
	select	t.DestajoTuberoID,
			JuntaWorkstatusID,
			ProyectoID,
			PDIs,
			CostoUnitario,
			Destajo,
			ProrrateoCuadro,
			ProrrateoDiasFestivos,
			ProrrateoOtros,
			CAST(0 as int),
			Total,
			EsDePeriodoAnterior,
			CostoDestajoVacio,
			@UsuarioModifica,
			GETDATE()
	from #TmpJuntasTuberosXml jtas
	inner join #TmpTuberos t on jtas.TuberoID = t.TuberoID
	
	insert into DestajoSoldador
	(
		SoldadorID,
		PeriodoDestajoID,
		ReferenciaCuadro,
		CantidadDiasFestivos,
		CostoDiaFestivo,
		TotalDestajoRaiz,
		TotalDestajoRelleno,
		TotalCuadro,
		TotalDiasFestivos,
		TotalOtros,
		TotalAjuste,
		GranTotal,
		Aprobado,
		UsuarioModifica,
		FechaModificacion
	)
	output inserted.DestajoSoldadorID, inserted.SoldadorID into #TmpSoldadores
	select	SoldadorID,
			@PeriodoDestajoID,
			ReferenciaCuadro,
			@CantidadDiasFestivos,
			CostoDiaFestivo,
			TotalDestajoRaiz,
			TotalDestajoRelleno,
			TotalCuadro,
			TotalDiasFestivos,
			TotalOtros,
			TotalAjuste,
			GranTotal,
			CAST(0 as bit),
			@UsuarioModifica,
			GETDATE()
	from #SoldadoresXml
	
	insert into DestajoSoldadorDetalle
	(
		DestajoSoldadorID,
		JuntaWorkstatusID,
		ProyectoID,
		PDIs,
		CostoUnitarioRaiz,
		CostoUnitarioRelleno,
		RaizDividida,
		RellenoDividido,
		NumeroFondeadores,
		NumeroRellenadores,
		DestajoRaiz,
		DestajoRelleno,
		ProrrateoCuadro,
		ProrrateoDiasFestivos,
		ProrrateoOtros,
		Ajuste,
		Total,
		EsDePeriodoAnterior,
		CostoRaizVacio,
		CostoRellenoVacio,
		UsuarioModifica,
		FechaModificacion
	)
	select	t.DestajoSoldadorID,
			JuntaWorkstatusID,
			ProyectoID,
			PDIs,
			CostoUnitarioRaiz,
			CostoUnitarioRelleno,
			RaizDividida,
			RellenoDividido,
			NumeroFondeadores,
			NumeroRellenadores,
			DestajoRaiz,
			DestajoRelleno,
			ProrrateoCuadro,
			ProrrateoDiasFestivos,
			ProrrateoOtros,
			CAST(0 as int),
			Total,
			EsDePeriodoAnterior,
			CostoRaizVacio,
			CostoRellenoVacio,
			@UsuarioModifica,
			GETDATE()
	from #TmpJuntasSoldadoresXml jtas
	inner join #TmpSoldadores t on t.SoldadorID = jtas.SoldadorID
	
	drop table #TmpTuberos
	drop table #TmpSoldadores
	drop table #TuberosXml
	drop table #TmpJuntasTuberosXml
	drop table #SoldadoresXml
	drop table #TmpJuntasSoldadoresXml

	-- Regresar el ID del periodo insertado
	SELECT @PeriodoDestajoID
	
		
	SET NOCOUNT OFF;

END

/*
	delete from DestajoTuberoDetalle
	delete from DestajoTubero
	delete from DestajoSoldadorDetalle
	delete from DestajoSoldador
	delete from PeriodoDestajo
	exec GeneraPeriodoDestajo '10', 2010, '20101101', '20101130', 0, '<?xml version="1.0" encoding="utf-8"?><ArrayOfDestajoArmadoGrupo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><DestajoArmadoGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><ReferenciaCuadro>0</ReferenciaCuadro><TotalCuadro>0</TotalCuadro><GranTotal>0</GranTotal><TotalDestajo>0</TotalDestajo><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><TotalAjuste>0</TotalAjuste><TuberoID>3</TuberoID><ID>-1</ID><juntas><junta><JuntaWorkstatusID>20</JuntaWorkstatusID><TuberoID>3</TuberoID><TipoJuntaID>1</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>8.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-1</ID><IDPadre>-1</IDPadre></junta><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><TuberoID>3</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-2</ID><IDPadre>-1</IDPadre></junta></juntas></DestajoArmadoGrupo><DestajoArmadoGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><ReferenciaCuadro>0</ReferenciaCuadro><TotalCuadro>0</TotalCuadro><GranTotal>0</GranTotal><TotalDestajo>0</TotalDestajo><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><TotalAjuste>0</TotalAjuste><TuberoID>4</TuberoID><ID>-2</ID><juntas><junta><JuntaWorkstatusID>39</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-1</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>40</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-2</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>41</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-3</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>42</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-4</ID><IDPadre>-2</IDPadre></junta></juntas></DestajoArmadoGrupo></ArrayOfDestajoArmadoGrupo>', '<?xml version="1.0" encoding="utf-8"?><ArrayOfDestajoSoldaduraGrupo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><DestajoSoldaduraGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><GranTotal>0</GranTotal><ReferenciaCuadro>0</ReferenciaCuadro><SoldadorID>3</SoldadorID><TotalAjuste>0</TotalAjuste><TotalCuadro>0</TotalCuadro><TotalDestajoRaiz>0</TotalDestajoRaiz><TotalDestajoRelleno>0</TotalDestajoRelleno><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><ID>-1</ID><juntas><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><SoldadorID>3</SoldadorID><TecnicaSoldadorID>0</TecnicaSoldadorID><ProcesoRaizID>0</ProcesoRaizID><ProcesoRellenoID>0</ProcesoRellenoID><TipoJuntaID>0</TipoJuntaID><FamiliaAceroID>0</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><CostoUnitarioRaiz>0</CostoUnitarioRaiz><CostoUnitarioRelleno>0</CostoUnitarioRelleno><RaizDividida>true</RaizDividida><RellenoDividido>false</RellenoDividido><NumeroFondeadores>2</NumeroFondeadores><NumeroRellenadores>0</NumeroRellenadores><DestajoRaiz>0</DestajoRaiz><DestajoRelleno>0</DestajoRelleno><ProrrateoCuadro>0</ProrrateoCuadro><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><Ajuste>0</Ajuste><Total>0</Total><ID>-1</ID><IDPadre>-1</IDPadre></junta></juntas></DestajoSoldaduraGrupo><DestajoSoldaduraGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><GranTotal>0</GranTotal><ReferenciaCuadro>0</ReferenciaCuadro><SoldadorID>4</SoldadorID><TotalAjuste>0</TotalAjuste><TotalCuadro>0</TotalCuadro><TotalDestajoRaiz>0</TotalDestajoRaiz><TotalDestajoRelleno>0</TotalDestajoRelleno><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><ID>-2</ID><juntas><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><SoldadorID>4</SoldadorID><TecnicaSoldadorID>0</TecnicaSoldadorID><ProcesoRaizID>0</ProcesoRaizID><ProcesoRellenoID>0</ProcesoRellenoID><TipoJuntaID>0</TipoJuntaID><FamiliaAceroID>0</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><CostoUnitarioRaiz>0</CostoUnitarioRaiz><CostoUnitarioRelleno>0</CostoUnitarioRelleno><RaizDividida>true</RaizDividida><RellenoDividido>false</RellenoDividido><NumeroFondeadores>2</NumeroFondeadores><NumeroRellenadores>0</NumeroRellenadores><DestajoRaiz>0</DestajoRaiz><DestajoRelleno>0</DestajoRelleno><ProrrateoCuadro>0</ProrrateoCuadro><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><Ajuste>0</Ajuste><Total>0</Total><ID>-2</ID><IDPadre>-2</IDPadre></junta></juntas></DestajoSoldaduraGrupo></ArrayOfDestajoSoldaduraGrupo>','D6A113B4-464E-496F-B15D-4456CB0AE55B'
*/


GO


