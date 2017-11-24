if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SplitCVSToTable]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[SplitCVSToTable]
GO 
create function dbo.SplitCVSToTable
(
 @String nvarchar(MAX),
 @Delimiter nvarchar (10)
 )
returns @ValueTable table ([Value] nvarchar(MAX))

begin

 declare @NextString nvarchar(MAX)
 declare @Pos int
 declare @NextPos int
 declare @CommaCheck nvarchar(1)

if (@String != '' AND @String is not null)
BEGIN

 --Initialize
 set @NextString = ''
 set @CommaCheck = right(@String,1) 
 
 --Check for trailing Comma, if not exists, INSERT
 --if (@CommaCheck <> @Delimiter )
 set @String = @String + @Delimiter
 
 --Get position of first Comma
 set @Pos = charindex(@Delimiter,@String)
 set @NextPos = 1
 
 --Loop while there is still a comma in the String of levels
 while (@pos <>  0)  
 begin
  set @NextString = substring(@String,1,@Pos - 1)
 
  insert into @ValueTable ( [Value]) Values (@NextString)
 
  set @String = substring(@String,@pos +1,len(@String))
  
  set @NextPos = @Pos
  set @pos  = charindex(@Delimiter,@String)
 end
 
END

 return
end

GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActualizaUsername]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ActualizaUsername]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ActualizaUsername
	Funcion:	Cambia el username en la tabla del membership provider				
	Parametros:	@UserID				UNIQUEIDENTIFIER
				@UserName			NVARCHAR(256)
	Autor:		IHM
	Modificado:	16/09/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ActualizaUsername]
(
	 @UserID			UNIQUEIDENTIFIER
	,@UserName			NVARCHAR(256)
)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE	aspnet_Users
	SET		 UserName = @UserName
			,LoweredUserName = LOWER(@UserName)
	WHERE	UserId = @UserID
	
	SELECT CAST(1 as bit)

	SET NOCOUNT OFF;

END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BorraFamiliaMaterial]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[BorraFamiliaMaterial]
GO

Create PROCEDURE [dbo].[BorraFamiliaMaterial]
(	
	@FamiliaMaterialID int,
	@VersionRegistro timestamp
)
AS
BEGIN
	
	DELETE FROM FamiliaMaterial
	WHERE	FamiliaMaterialID = @FamiliaMaterialID
			AND
			VersionRegistro = @VersionRegistro

END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BorraPeriodoDestajo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[BorraPeriodoDestajo]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		BorraPeriodoDestajo
	Funcion:	Elimina un periodo de destajo y todas sus relaciones
	Parametros:	@PeriodoDestajoID	INT
	Autor:		IHM
	Modificado:	23/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[BorraPeriodoDestajo]
(
	@PeriodoDestajoID	INT
)
AS
BEGIN

	SET NOCOUNT ON;
	
	delete from DestajoSoldadorDetalle
	where DestajoSoldadorID in
	(
		select DestajoSoldadorID
		from DestajoSoldador
		where PeriodoDestajoID = @PeriodoDestajoID
	)

	delete from DestajoTuberoDetalle
	where DestajoTuberoID in
	(
		select DestajoTuberoID
		from DestajoTubero
		where PeriodoDestajoID = @PeriodoDestajoID
	)
	
	delete from DestajoSoldador where PeriodoDestajoID = @PeriodoDestajoID
	delete from DestajoTubero where PeriodoDestajoID = @PeriodoDestajoID
	
	delete from PeriodoDestajo where PeriodoDestajoID = @PeriodoDestajoID

	select CAST(1 as bit) [Borrado]

	SET NOCOUNT OFF;

END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BorraRecepcion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[BorraRecepcion]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		BorraRecepcion
	Funcion:	Elimina la recepción y todas sus relaciones			
	Parametros:	@RecepcionID	INT
	Autor:		LMG
	Modificado:	01/10/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[BorraRecepcion]
(
	@RecepcionID	INT
)
AS
BEGIN

	SET NOCOUNT ON;
	
	CREATE TABLE #NumUnico
	( 
		NumeroUnicoID INT
	)
	
	INSERT INTO #NumUnico
	SELECT rnu.NumeroUnicoID 	
	FROM RecepcionNumeroUnico rnu		
	WHERE rnu.RecepcionID = @RecepcionID

    DELETE FROM RecepcionNumeroUnico WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
	
	DELETE FROM NumeroUnicoInventario WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
	
	DELETE FROM NumeroUnicoMovimiento WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
	
	DELETE FROM NumeroUnicoSegmento WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
		
	DELETE FROM NumeroUnico WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
	
	DELETE FROM Recepcion WHERE
	RecepcionID = @RecepcionID
	
	DROP TABLE #NumUnico
	
	SELECT CAST(1 as bit)	

	SET NOCOUNT OFF;

END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculaHojaParaReporte]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[CalculaHojaParaReporte]
GO
/****************************************************************************************
	Nombre:		CalculaHojaParaReporte
	Funcion:	Calcula las hojas que cada reporte debe incluir y actualiza los valores
				en la tabla del reporte especificado
	Parametros:	@TipoReporte INT
				@ProyectoID INT
				@NumeroReporte NVARCHAR(50)
				@IDs VARCHAR(MAX)
	Autor:		LMG
	Modificado:	02/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[CalculaHojaParaReporte]
(	
	@TipoReporte INT,
	@ProyectoID INT,
	@NumeroReporte NVARCHAR(50),
	@IDs VARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT ON;
	
	CREATE TABLE #TempDibujos
	(	
		DibujoID INT IDENTITY,
		Dibujo NVARCHAR(50),
		SpoolID INT
	)
	
	CREATE TABLE #TempJuntas
	(	
		JuntaSpoolID INT,
		JuntaWorkstatusID INT
	)
	
	CREATE TABLE #TempSpools
	(	
		SpoolID INT,
		WorkstatusSpoolID INT
	)
	-- Se obtienen los ids de las juntas que son parte del reporte
		
	-- Inspeccion Visual
	IF(@TipoReporte = 0)
	BEGIN 
		INSERT INTO #TempJuntas
		SELECT 	jw.JuntaSpoolID, 
				jw.JuntaWorkstatusID
		FROM	JuntaWorkstatus jw
		INNER JOIN JuntaInspeccionVisual jiv on jw.JuntaWorkstatusID = jiv.JuntaWorkstatusID
		INNER JOIN InspeccionVisual iv on jiv.InspeccionVisualID = iv.InspeccionVisualID
		WHERE	jw.JuntaWorkstatusID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (iv.NumeroReporte = @NumeroReporte  AND iv.ProyectoID = @ProyectoID)
	END
	-- Reporte TT
	ELSE IF(@TipoReporte = 1) 
	BEGIN 
		INSERT INTO #TempJuntas
		SELECT 	jw.JuntaSpoolID, 
				jw.JuntaWorkstatusID
		FROM	JuntaWorkstatus jw
		INNER JOIN JuntaReporteTt jtt on jw.JuntaWorkstatusID = jtt.JuntaWorkstatusID
		INNER JOIN ReporteTt tt on jtt.ReporteTtID = tt.ReporteTtID
		WHERE	jw.JuntaWorkstatusID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (tt.NumeroReporte = @NumeroReporte  AND tt.ProyectoID = @ProyectoID)
	END
	-- Reporte PND
	ELSE IF(@TipoReporte = 2) 
	BEGIN 
		INSERT INTO #TempJuntas
		SELECT 	jw.JuntaSpoolID, 
				jw.JuntaWorkstatusID
		FROM	JuntaWorkstatus jw
		INNER JOIN JuntaReportePnd jpnd on jw.JuntaWorkstatusID = jpnd.JuntaWorkstatusID
		INNER JOIN ReportePnd pnd on jpnd.ReportePndID = pnd.ReportePndID
		WHERE	jw.JuntaWorkstatusID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (pnd.NumeroReporte = @NumeroReporte  AND pnd.ProyectoID = @ProyectoID)
	END
	-- Reporte Dimensional
	ELSE IF(@TipoReporte = 3) 
	BEGIN 
		INSERT INTO #TempSpools
		SELECT 	ots.SpoolID, 
				ws.WorkstatusSpoolID
		FROM	WorkstatusSpool ws
		INNER JOIN OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		INNER JOIN ReporteDimensionalDetalle rdd on rdd.WorkstatusSpoolID = ws.WorkstatusSpoolID
		INNER JOIN ReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID
		WHERE	ws.WorkstatusSpoolID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (rd.NumeroReporte = @NumeroReporte  AND rd.ProyectoID = @ProyectoID)
	END
	
								
	----Obtengo los Spools
	IF(@TipoReporte <> 3)
	BEGIN
		INSERT INTO #TempDibujos
		SELECT DISTINCT s.Dibujo, s.SpoolID
		FROM JuntaSpool js
		INNER JOIN Spool s on js.SpoolID = s.SpoolID
		WHERE js.JuntaSpoolID in (SELECT JuntaSpoolID FROM #TempJuntas)
		ORDER BY s.Dibujo
	END
	ELSE	
	BEGIN
		INSERT INTO #TempDibujos
		SELECT DISTINCT s.Dibujo, s.SpoolID
		FROM Spool s 
		WHERE s.SpoolID in (SELECT SpoolID FROM #TempSpools)
		ORDER BY s.Dibujo
	END
	
	
	-- Inspeccion Visual
	IF(@TipoReporte = 0)
	BEGIN 
		UPDATE JuntaInspeccionVisual 
		SET JuntaInspeccionVisual.Hoja = (SELECT DibujoID 
										  FROM #TempDibujos 
										  WHERE SpoolID = (	SELECT j.SpoolID 
															FROM JuntaSpool j
															INNER JOIN JuntaWorkstatus jw on j.JuntaSpoolID = jw.JuntaSpoolID
															WHERE jw.JuntaWorkstatusID = JuntaInspeccionVisual.JuntaWorkstatusID))
		WHERE JuntaInspeccionVisual.InspeccionVisualID in (	SELECT InspeccionVisualID 
															FROM InspeccionVisual 
															WHERE NumeroReporte = @NumeroReporte AND ProyectoID = @ProyectoID)
	END
	-- Reporte TT
	ELSE IF(@TipoReporte = 1)
	BEGIN 
		UPDATE JuntaReporteTt 
		SET JuntaReporteTt.Hoja = (	SELECT DibujoID 
									FROM #TempDibujos 
									WHERE SpoolID = (SELECT j.SpoolID 
													 FROM JuntaSpool j
													 INNER JOIN JuntaWorkstatus jw on j.JuntaSpoolID = jw.JuntaSpoolID
													 WHERE jw.JuntaWorkstatusID = JuntaReporteTt.JuntaWorkstatusID))
		WHERE JuntaReporteTt.ReporteTtID in (SELECT ReporteTtID 
											 FROM ReporteTt
											 WHERE NumeroReporte = @NumeroReporte AND ProyectoID = @ProyectoID)
	END
	-- Reporte PND
	ELSE IF(@TipoReporte = 2)
	BEGIN 
		UPDATE JuntaReportePnd 
		SET JuntaReportePnd.Hoja = (SELECT DibujoID 
									FROM #TempDibujos 
									WHERE SpoolID = (SELECT j.SpoolID 
													 FROM JuntaSpool j
													 INNER JOIN JuntaWorkstatus jw on j.JuntaSpoolID = jw.JuntaSpoolID
													 WHERE jw.JuntaWorkstatusID = JuntaReportePnd.JuntaWorkstatusID))
		WHERE JuntaReportePnd.ReportePndID in (	SELECT ReportePndID 
												FROM ReportePnd
												WHERE NumeroReporte = @NumeroReporte AND ProyectoID = @ProyectoID)
	END
	-- Reporte Dimensional
	ELSE IF(@TipoReporte = 3)
	BEGIN 
		UPDATE ReporteDimensionalDetalle 
		SET ReporteDimensionalDetalle.Hoja = (	SELECT DibujoID 
												FROM #TempDibujos 
												WHERE SpoolID = (SELECT s.SpoolID 
																 FROM OrdenTrabajoSpool s
																 INNER JOIN WorkstatusSpool ws on s.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
																 WHERE ws.WorkstatusSpoolID = ReporteDimensionalDetalle.WorkstatusSpoolID))
		WHERE ReporteDimensionalDetalle.WorkstatusSpoolID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,','))
	END
	
	DROP TABLE #TempDibujos
	DROP TABLE #TempJuntas	
		
	SELECT CAST(1 as bit)
		
	SET NOCOUNT OFF;

END

/*
exec CalculaHojaParaReporte 0,29,'22','20,21,22,26,27'
select * from juntaworkstatus

SELECT Value FROM dbo.SplitCVSToTable('20,21,22,26,27',',')
*/


GO

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


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GuardaFamiliaMaterial]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GuardaFamiliaMaterial]
GO

Create PROCEDURE [dbo].[GuardaFamiliaMaterial]
(	
	@FamiliaMaterialID int,
	@Nombre nvarchar(50),
	@Descripcion nvarchar(500),
	@UsuarioModifica uniqueidentifier,
	@VersionRegistro timestamp
)
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Tabla TABLE
	(
		ID int,
		VersionRegistro varbinary(8)
	);
	
	IF ISNULL(@FamiliaMaterialID,-1) <= 0
	BEGIN

		INSERT INTO [FamiliaMaterial]
			([Nombre]
			,[Descripcion]
			,[UsuarioModifica]
			,[FechaModificacion])
		OUTPUT	INSERTED.FamiliaMaterialID, INSERTED.VersionRegistro
		INTO @Tabla
		VALUES
			(@Nombre
			,@Descripcion
			,@UsuarioModifica
			,GETDATE())

	END
	ELSE
	BEGIN

		UPDATE [FamiliaMaterial]
		SET	[Nombre] = @Nombre
			,[Descripcion] = @Descripcion
			,[UsuarioModifica] = @UsuarioModifica
			,[FechaModificacion] = GETDATE()
		OUTPUT	INSERTED.FamiliaMaterialID, INSERTED.VersionRegistro
		INTO @Tabla
		WHERE	[FamiliaMaterialID] = @FamiliaMaterialID
				AND
				[VersionRegistro] = @VersionRegistro

	END
	
	-- Regresar los valores insertados/actualizados
	SELECT ID, VersionRegistro FROM @Tabla

END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ListadoCertificacion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ListadoCertificacion] 
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[ListadoCertificacion] 
	Funcion:	Obtiene las tablas necesarias para contruir el listado
			de certificacion				
	Parametros:	@proyectoID INT
	Autor:		SCB
	Modificado:	12/13/2010
*****************************************************************************************/

CREATE PROCEDURE [dbo].[ListadoCertificacion] 
(@proyectoID INT)
AS 
BEGIN

	CREATE TABLE #Spools(
		SpoolID INT,
		OrdenTrabajoSpoolID INT,
		Nombre NVARCHAR(50),		
	    NumeroControl NVARCHAR(50)
	)
	
	CREATE TABLE #JuntaSpools(
		JuntaSpoolID INT
	)
	
	CREATE TABLE #JuntaWorkstatus(
		SpoolID INT,
		JuntaSpoolID INT,
		JuntaWorkStatusID INT,
		InspeccionVisualAprobada BIT, 
		NumeroReporte NVARCHAR(50),
		JuntaSoldaduraID INT,
		TipoJuntaID INT,
		JuntaArmadoID INT,
		FabAreaID INT
	)
	
	CREATE TABLE #JuntaReportes(
		JuntaWorkstatusID INT, 		
		Aprobado BIT,
		NumeroReporte NVARCHAR(50),
		TipoPruebaID INT
	)
	
	CREATE TABLE #WorkstatusSpool(
		SpoolID INT,
		OrdenTrabajoSpoolID INT,
		TieneLiberacionDimensional BIT, 
		LiberadoPintura BIT,		
		NumeroReporte NVARCHAR(50),
	    TipoReporteDimensionalID INT,
	    Aprobado BIT,
	    WorkstatusSpoolID INT,
	    ReporteAcabadoVisual NVARCHAR(50),
		ReporteAdherencia NVARCHAR(50),
		ReporteIntermedios NVARCHAR(50),
		ReportePrimarios NVARCHAR(50),
		ReportePullOff NVARCHAR(50),
		ReporteSandblast NVARCHAR(50)
	)
	
	
	--traigo los spools del proyecto
	INSERT INTO #Spools
	SELECT	s.SpoolID, 
			OrdenTrabajoSpoolID, 
			Nombre, 
			NumeroControl
	FROM Spool s
	INNER JOIN OrdenTrabajoSpool ots
		ON ots.SpoolID = s.SpoolID
	WHERE ProyectoID = @proyectoID
	ORDER BY s.SpoolID
	

	--traigo las juntas de esos spools
	INSERT INTO #JuntaSpools
	SELECT JuntaSpoolID
	FROM JuntaSpool 
	WHERE SpoolID IN (SELECT SpoolID FROM #Spools)
	
	--para inspeccion visual y soldadura
	INSERT INTO #JuntaWorkStatus
	SELECT	js.SpoolID, 
			js.JuntaSpoolID, 
			jws.JuntaWorkstatusID, 
			InspeccionVisualAprobada, 
			iv.NumeroReporte, 
			JuntaSoldaduraID, 
			TipoJuntaID, 
			JuntaArmadoID, 
			FabAreaID
	FROM Spool s
	INNER JOIN JuntaSpool js
		ON s.SpoolID = js.SpoolID
	LEFT JOIN JuntaWorkstatus jws
		ON jws.JuntaSpoolID = js.JuntaSpoolID	
	LEFT JOIN JuntaInspeccionVisual jiv
		ON jws.JuntaWorkstatusID = jiv.JuntaWorkstatusID
	LEFT JOIN InspeccionVisual iv
		ON iv.InspeccionVisualID = jiv.InspeccionVisualID 		  		
	WHERE js.JuntaSpoolID IN(SELECT JuntaSpoolID FROM #JuntaSpools)
	AND (JuntaFinal = 1 )--OR jws.JuntaWorkstatusID is null
	
	--para inspeccion dimensional y espesores	
	INSERT INTO #WorkstatusSpool
	
	SELECT	s.SpoolID, 
			ws.OrdenTrabajoSpoolID, 
			TieneLiberacionDimensional, 
			LiberadoPintura, 
			NumeroReporte, 
			TipoReporteDimensionalID , 
			Aprobado, 
			ws.WorkstatusSpoolID,
			ISNULL(ReporteAcabadoVisual,'') AS ReporteAcabadoVisual,
			ISNULL(ReporteAdherencia,'') AS ReporteAdherencia,
			ISNULL(ReporteIntermedios,'') AS ReporteIntermedios,
			ISNULL(ReportePrimarios,'') AS ReportePrimarios,
			ISNULL(ReportePullOff,'') AS ReportePullOff,
			ISNULL(ReporteSandblast,'') AS ReporteSandblast
	FROM #Spools s 
	LEFT JOIN WorkstatusSpool ws
		ON s.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
	LEFT JOIN ReporteDimensionalDetalle rdd
		ON rdd.WorkstatusSpoolID = ws.WorkstatusSpoolID
	LEFT JOIN ReporteDimensional rd	
		ON rd.ReporteDimensionalID = rdd.ReporteDimensionalID	
	LEFT JOIN PinturaSpool ps	
		ON ps.WorkstatusSpoolID = ws.WorkstatusSpoolID
	WHERE rd.ProyectoID = @proyectoID or rd.ProyectoID is null
	
	
	--para Reportes Pnds
	INSERT INTO #JuntaReportes
	SELECT	JuntaWorkstatusID, 
			Aprobado, 
			NumeroReporte, 
			TipoPruebaID
	FROM JuntaReportePnd jrPnd
	INNER JOIN ReportePnd rPnd
		ON rPnd.ReportePndID = jrPnd.ReportePndID
	WHERE JuntaWorkstatusID IN(SELECT JuntaWorkstatusID FROM #JuntaWorkstatus)
	
	
	--para Reportes TT
	INSERT INTO #JuntaReportes
	SELECT	jr.JuntaWorkstatusID, 
			Aprobado, 
			NumeroReporte, 
			r.TipoPruebaID
	FROM JuntaRequisicion jr 
	INNER JOIN Requisicion r 
		ON r.RequisicionID = jr.RequisicionID
	LEFT JOIN JuntaReporteTT jrTT
		ON jrTT.JuntaRequisicionID = jr.JuntaRequisicionID
	INNER JOIN ReporteTT rTT
		ON rTT.ReporteTtID = jrTT.ReporteTtID		
	WHERE jr.JuntaWorkstatusID IN(SELECT JuntaWorkstatusID FROM #JuntaWorkstatus)

	SELECT Nombre FROM Proyecto WHERE ProyectoID = @proyectoID
	SELECT * FROM #Spools
	SELECT * FROM #WorkstatusSpool
	SELECT * FROM #JuntaReportes
	SELECT * FROM #JuntaWorkStatus
	

	DROP TABLE #Spools
	DROP TABLE #JuntaSpools
	DROP TABLE #WorkstatusSpool
	DROP TABLE #JuntaReportes
	DROP TABLE #JuntaWorkStatus
	

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerConsultaDeItemCode]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerConsultaDeItemCode]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerConsultaDeItemCode
	Funcion:	Documentar
	Parametros:	@ProyectoID int
	Autor:		MMG
	Modificado:	25/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerConsultaDeItemCode]
(
	@ProyectoID INT
)
AS
BEGIN

	SET NOCOUNT ON; 

	SET NOCOUNT ON;
	
	CREATE TABLE #TempItemCode
	(	
		ItemCodeID INT,
		TipoMaterialID INT,
		Codigo NVARCHAR(50),
		DescripcionEspanol nvarchar(150),
		DescripcionIngles nvarchar(150)
	)
	
	CREATE TABLE #TempNumerosUnicosInventario
	(	
		ItemCodeID INT,
		Diametro1 DECIMAL(7, 4),
		Diametro2 DECIMAL(7, 4),
		CantidadRecibida INT,
		CantidadDanada INT,
		InventarioFisico INT,
		InventarioBuenEstado INT,
		InventarioCongelado INT,
		InventarioDisponibleCruce INT,
		InventarioTransferenciaCorte INT,
		TotalEntradaOtrosProcesos INT,
		TotalDespachado INT,
		TotalDespachadoParaICE INT,
		TotalCorte INT,
		TotalMerma INT,
		TotalOtrasSalidas INT,
		TotalSalidasTemporales INT,
		TotalCondicionada INT,
		TotalRechazada INT,
		TotalEnTransferenciaCorteICE INT,
		TotalCortadoICE INT
	)
	
	CREATE TABLE #TempMaterialSpool
	(	
		ItemCodeID INT,
		Diametro1 DECIMAL(7, 4),
		Diametro2 DECIMAL(7, 4),
		Cantidad INT
	)
	
	CREATE TABLE #TempODT
	(	
		ItemCodeID INT,
		Diametro1 DECIMAL(7, 4),
		Diametro2 DECIMAL(7, 4),
		Cantidad INT
	)
	
	CREATE TABLE #TempItemCodeIntegrado
	(	
		ItemCodeID INT,
		Diametro1 DECIMAL(7, 4),
		Diametro2 DECIMAL(7, 4)
	)
	
	-- Tabla que traerá la relación entre los números únicos y las ordenes de trabajo en la que aparecen
	CREATE TABLE #TempODTItemCodes
	(
		ItemCodeID INT,
		Diametro1 DECIMAL (7,4),
		Diametro2 DECIMAL (7,4),
		OrdenTrabajoID INT,
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoMaterialID INT,
		MaterialSpoolID INT,
		TieneDespacho BIT
	)
	
	CREATE TABLE #TempSumaNumerosUnicos
	(	
		ItemCodeID INT,
		Diametro1 DECIMAL(7, 4),
		Diametro2 DECIMAL(7, 4),
		TotalEntradaOtrosProcesos INT,
		TotalDespachado INT,
		TotalDespachadoParaICE INT,
		TotalCorte INT,
		TotalMerma INT,
		TotalOtrasSalidas INT,
		TotalSalidasTemporales INT,
		TotalEntradasTemporales INT,
		TotalCondicionada INT,
		TotalRechazada INT,
		TotalEnTransferenciaCorteICE INT,
		TotalCortadoICE INT
	)
	
	CREATE TABLE #TempSumaPorIC
	(	
		ItemCodeID INT,
		Diametro1 DECIMAL(7, 4),
		Diametro2 DECIMAL(7, 4),
		TotalEntradaOtrosProcesos INT,
		TotalDespachado INT,
		TotalDespachadoParaICE INT,
		TotalCorte INT,
		TotalMerma INT,
		TotalOtrasSalidas INT,
		TotalSalidasTemporales INT,
		TotalCondicionada INT,
		TotalRechazada INT,
		TotalEnTransferenciaCorteICE INT,
		TotalCortadoICE INT
	)
	
	CREATE TABLE #TempDespachadaEquivalente
	(	
		ItemCodeID INT,
		Diametro1 DECIMAL(7, 4),
		Diametro2 DECIMAL(7, 4),
		--TotalEnTransferenciaDesdeICE INT,
		--TotalCortadoDesdeICE INT,
		CantidadDespachadaEquivalente INT,
		CantidadCongeladaEquivalente INT
	)
	-- Se agregan los registros a las tablas temporales
		
	-- IItemCode
	INSERT INTO #TempItemCode
	SELECT 	ic.ItemCodeID ,
			ic.TipoMaterialID,
			ic.Codigo,
			ic.DescripcionEspanol,
			ic.DescripcionIngles
	FROM	ItemCode ic
	WHERE ic.ProyectoID = @ProyectoID
	
	--ItemCodeIntegrado
	INSERT INTO #TempItemCodeIntegrado
	select	distinct nu.ItemCodeID,
			nu.Diametro1,
			nu.Diametro2
	from NumeroUnico nu
	where nu.ProyectoID = @ProyectoID	
	union	
	select	distinct ms.ItemCodeID,
			ms.Diametro1,
			ms.Diametro2
	from MaterialSpool ms
	where exists
	(
		select 1 from Spool s where ProyectoID = @ProyectoID and ms.SpoolID = s.SpoolID
	)
	
	INSERT INTO #TempODTItemCodes
	SELECT  distinct 
			ms.ItemCodeID, 
			ms.Diametro1,
			ms.Diametro2,
			ot.OrdenTrabajoID,
			ots.OrdenTrabajoSpoolID,
			om.OrdenTrabajoMaterialID,
			om.MaterialSpoolID,
			om.TieneDespacho
	FROM    OrdenTrabajo ot
	INNER JOIN OrdenTrabajoSpool ots on ot.OrdenTrabajoID = ots.OrdenTrabajoID
	INNER JOIN OrdenTrabajoMaterial om on om.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	INNER JOIN MaterialSpool ms on ms.MaterialSpoolID = om.MaterialSpoolID	
	where	ot.ProyectoID = @ProyectoID
	
	INSERT INTO #TempSumaNumerosUnicos
	select  nu.ItemCodeID,
					nu.Diametro1,
					nu.Diametro2,
		   (
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (11)
						and num.Estatus = 'A'
			) [TotalEntradaOtrosProcesos],	
			(
				select isnull(SUM(d.Cantidad),0)
				from Despacho d 
				where d.NumeroUnicoID = nu.NumeroUnicoID
				and d.Cancelado = 0 and d.EsEquivalente = 0
			)	[TotalDespachado],
			(
				select isnull(SUM(d.Cantidad),0)
				from Despacho d 
				where d.NumeroUnicoID = nu.NumeroUnicoID
				and d.Cancelado = 0 and d.EsEquivalente = 1
			)	[TotalDespachadoParaICE],
			(				
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (18)
						and num.Estatus = 'A'
			) [TotalCorte],
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (5,9)
						and num.Estatus = 'A'
			) [TotalMerma],
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID not in (2,18,5,9,6)
						and num.Estatus = 'A'
						and exists
						(
							select 1
							from TipoMovimiento tp
							where tp.EsEntrada = 0
								  and tp.EsTransferenciaProcesos = 0
								  and tp.TipoMovimientoID = num.TipoMovimientoID
						)
			) [TotalOtrasSalidas],
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num				
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (6) --Salida a Pintura
						and num.Estatus = 'A'						
			) [TotalSalidasTemporales],
			(
				select isnull(SUM(num.Cantidad),0) 
				from NumeroUnicoMovimiento num				
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (7) --Entrada de Pintura
						and num.Estatus = 'A'
			) [TotalEntradasTemporales],
			nuc.TotalCondicionada,
			nur.TotalRechazada,
			(select isnull(SUM(nucte.Longitud),0)
					from NumeroUnicoCorte nucte
					where nucte.TieneCorte = 0
					and nucte.NumeroUnicoID = nu.NumeroUnicoID					
					and	nucte.OrdenTrabajoID not in 
					(select odtic.OrdenTrabajoID from #TempODTItemCodes odtic
					where odtic.ItemCodeID = nu.ItemCodeID
					and odtic.Diametro1 = nu.Diametro1
					and odtic.Diametro2 = nu.Diametro2)
			) [TotalEnTransferenciaCorteICE],
			(select isnull(SUM(cd.Cantidad),0)
					from CorteDetalle cd		
					inner join Corte c on c.CorteID = cd.CorteID
					inner join NumeroUnicoCorte nucte on nucte.NumeroUnicoCorteID = c.NumeroUnicoCorteID					
					where nucte.NumeroUnicoID = nu.NumeroUnicoID
					and cd.Cancelado = 0
					and c.Cancelado = 0
					and cd.MaterialSpoolID not in 
						(select odtic.MaterialSpoolID from #TempODTItemCodes odtic
							where (odtic.ItemCodeID = nu.ItemCodeID
							and odtic.Diametro1 = nu.Diametro1
							and odtic.Diametro2 = nu.Diametro2)
							or odtic.TieneDespacho = 1)					
			) [TotalCortadoICE]			
			from NumeroUnico nu
			LEFT JOIN
			(
				SELECT isnull(SUM(CantidadRecibida),0) [TotalCondicionada],
					   nu.NumeroUnicoID
				FROM NumeroUnico nu
				inner join NumeroUnicoInventario nui on nui.NumeroUnicoID = nu.NumeroUnicoID 
				where nu.Estatus = 'C'
				group by nu.NumeroUnicoID
			) nuc ON nuc.NumeroUnicoID = nu.NumeroUnicoID 
			LEFT JOIN
			(
				SELECT isnull(SUM(CantidadRecibida),0) [TotalRechazada],
					   nu.NumeroUnicoID
				FROM NumeroUnico nu
				inner join NumeroUnicoInventario nui on nui.NumeroUnicoID = nu.NumeroUnicoID 
				where nu.Estatus = 'R'
				group by nu.NumeroUnicoID
			) nur ON nur.NumeroUnicoID = nu.NumeroUnicoID 
			where nu.ProyectoID = @ProyectoID
			
	INSERT INTO #TempSumaPorIC
	SELECT	ic.ItemCodeID,
			ic.Diametro1,
			ic.Diametro2,
			sum(isnull(ic.TotalEntradaOtrosProcesos,0)) [TotalEntradaOtrosProcesos],
			sum(isnull(ic.TotalDespachado,0)) [TotalDespachado],
			sum(isnull(ic.TotalDespachadoParaICE,0)) [TotalDespachadoParaICE],
			sum(isnull(ic.TotalCorte,0)) [TotalCorte],
			sum(isnull(ic.TotalMerma,0)) [TotalMerma],
			sum(isnull(ic.TotalOtrasSalidas,0)) [TotalOtrasSalidas],
			sum(isnull(ic.TotalSalidasTemporales,0)) - sum(isnull(ic.TotalEntradasTemporales,0)) [TotalSalidasTemporales],			
			sum(isnull(ic.TotalCondicionada,0)) [TotalCondicionada],
			sum(isnull(ic.TotalRechazada,0)) [TotalRechazada],
			sum(isnull(ic.TotalEnTransferenciaCorteICE,0)) [TotalEnTransferenciaCorteICE],
			sum(isnull(ic.TotalCortadoICE,0)) [TotalCortadoICE]
			from #TempSumaNumerosUnicos ic
			group by ic.ItemCodeID, ic.Diametro1, ic.Diametro2
				
	-- Numero Unico y Numero Unico Inventario
	
	INSERT INTO #TempNumerosUnicosInventario
	SELECT 	nu.ItemCodeID ,
			nu.Diametro1,
			nu.Diametro2,
			SUM(nui.CantidadRecibida) [CantidadRecibida],
			SUM(nui.CantidadDanada) [CantidadDanada],
			SUM(nui.InventarioFisico) [InventarioFisico],
			SUM(nui.InventarioBuenEstado) [InventarioBuenEstado],
			SUM(nui.InventarioCongelado) [InventarioCongelado],
			SUM(nui.InventarioDisponibleCruce) [InventarioDisponibleCruce],
			SUM(nui.InventarioTransferenciaCorte) [InventarioTransferenciaCorte],
			tnu.TotalEntradaOtrosProcesos [TotalEntradaOtrosProcesos],			
			tnu.TotalDespachado [TotalDespachado],
			tnu.TotalDespachadoParaICE [TotalDespachadoParaICE],
			tnu.TotalCorte [TotalCorte],
			tnu.TotalMerma [TotalMerma],
			tnu.TotalOtrasSalidas [TotalOtrasSalidas],
			tnu.TotalSalidasTemporales [TotalSalidasTemporales],
			tnu.TotalCondicionada [TotalCondicionada],
			TotalRechazada [TotalRechazada],
			tnu.TotalEnTransferenciaCorteICE [TotalEnTransferenciaCorteICE],
			tnu.TotalCortadoICE [TotalCortadoICE]
	FROM	NumeroUnico nu
	INNER JOIN NumeroUnicoInventario nui on nu.NumeroUnicoID = nui.NumeroUnicoID
	LEFT JOIN #TempSumaPorIC tnu on nu.ItemCodeID = tnu.ItemCodeID and nu.Diametro1 = tnu.Diametro1 and nu.Diametro2 = tnu.Diametro2	
	WHERE nu.ProyectoID = @ProyectoID
	group by nu.ItemCodeID,
			 nu.Diametro1,
			 nu.Diametro2,
			 tnu.TotalEntradaOtrosProcesos,
			 tnu.TotalDespachado,
			 tnu.TotalDespachadoParaICE,
			 tnu.TotalCorte,
			 tnu.TotalMerma,
			 tnu.TotalOtrasSalidas,
			 tnu.TotalSalidasTemporales,
			 tnu.TotalCondicionada,
			 tnu.TotalRechazada,
			 tnu.TotalEnTransferenciaCorteICE,
			 tnu.TotalCortadoICE
	
	
	
	--MaterialSpool
	INSERT INTO #TempMaterialSpool
	select	ms.ItemCodeID,
			ms.Diametro1,
			ms.Diametro2,
			sum(ms.Cantidad) [Cantidad]
	from MaterialSpool ms
	where exists
	(
		select 1 from Spool s where ProyectoID = @ProyectoID and ms.SpoolID = s.SpoolID
	)
	group by ms.ItemCodeID,
			 ms.Diametro1,
			 ms.Diametro2
			 
			 --ODT
	INSERT INTO #TempODT
	SELECT	ms.ItemCodeID,
			ms.Diametro1,
			ms.Diametro2,
			sum(ms.Cantidad) [Cantidad]
	FROM	MaterialSpool ms 
	WHERE	MaterialSpoolID in
			(	SELECT MaterialSpoolID from OrdenTrabajoMaterial om
				INNER JOIN OrdenTrabajoSpool os on om.OrdenTrabajoSpoolID = os.OrdenTrabajoSpoolID
				INNER JOIN OrdenTrabajo o on o.OrdenTrabajoID = os.OrdenTrabajoID
				WHERE o.ProyectoID = @ProyectoID )
	group by ms.ItemCodeID,
			 ms.Diametro1,
			 ms.Diametro2
		
	INSERT INTO #TempDespachadaEquivalente
	Select 
	d.ItemCodeID,
	d.Diametro1,
	d.Diametro2,
	--SUM(ISNULL(d.TotalEnTransferenciaDesdeICE,0)) [TotalEnTransferenciaDesdeICE],
	--SUM(ISNULL(d.TotalCortadoDesdeICE,0)) [TotalCortadoDesdeICE],
	SUM(ISNULL(d.CantidadDespachadaEquivalente,0)) [CantidadDespachadaEquivalente],
	SUM(ISNULL(d.CantidadCongeladaEquivalente,0)) [CantidadCongeladaEquivalente]
	from (select  
			iceq.ItemCodeID,
			iceq.Diametro1,
			iceq.Diametro2,
			--(select isnull(SUM(nucte.Longitud),0)
			--		from NumeroUnicoCorte nucte
			--		where nucte.TieneCorte = 0
			--		and nucte.NumeroUnicoID = nu.NumeroUnicoID					
			--		and	nucte.OrdenTrabajoID in 
			--		(select odtic.OrdenTrabajoID from #TempODTItemCodes odtic
			--		where odtic.ItemCodeID = iceq.ItemCodeID
			--		and odtic.Diametro1 = iceq.Diametro1
			--		and odtic.Diametro2 = iceq.Diametro2)
			--) [TotalEnTransferenciaDesdeICE],
			--(select isnull(SUM(cd.Cantidad),0)
			--		from CorteDetalle cd		
			--		inner join Corte c on c.CorteID = cd.CorteID
			--		inner join NumeroUnicoCorte nucte on nucte.NumeroUnicoCorteID = c.NumeroUnicoCorteID					
			--		where nucte.NumeroUnicoID = nu.NumeroUnicoID
			--		and cd.Cancelado = 0
			--		and c.Cancelado = 0
			--		and cd.MaterialSpoolID in 
			--			(select odtic.MaterialSpoolID from #TempODTItemCodes odtic
			--				where odtic.ItemCodeID = iceq.ItemCodeID
			--				and odtic.Diametro1 = iceq.Diametro1
			--				and odtic.Diametro2 = iceq.Diametro2
			--				and odtic.TieneDespacho = 0)				    	
			--) [TotalCortadoDesdeICE],	
			(select isnull(SUM(desp.Cantidad),0)
					from Despacho desp
					inner join OrdenTrabajoMaterial om on om.DespachoID = desp.DespachoID
					inner join MaterialSpool m on m.MaterialSpoolID = om.MaterialSpoolID
					where	desp.NumeroUnicoID = nu.NumeroUnicoID
							and desp.Cancelado = 0
							and om.DespachoEsEquivalente = 1
							and m.ItemCodeID = iceq.ItemCodeID
							and m.Diametro1 = iceq.Diametro1
							and m.Diametro2 = iceq.Diametro2
			) [CantidadDespachadaEquivalente],
			(select isnull(SUM(om.CantidadCongelada),0)
					from OrdenTrabajoMaterial om  
					inner join MaterialSpool m on m.MaterialSpoolID = om.MaterialSpoolID
					where	om.NumeroUnicoCongeladoID = nu.NumeroUnicoID							
							and om.CongeladoEsEquivalente = 1
							and m.ItemCodeID = iceq.ItemCodeID
							and m.Diametro1 = iceq.Diametro1
							and m.Diametro2 = iceq.Diametro2
			) [CantidadCongeladaEquivalente]		
		from NumeroUnico nu
		inner join ItemCodeEquivalente iceq on nu.ItemCodeID = iceq.ItemEquivalenteID and nu.Diametro1 = iceq.DiametroEquivalente1 and nu.Diametro2 = iceq.DiametroEquivalente2 
		where	 nu.ProyectoID = @ProyectoID ) d
group by d.ItemCodeID, d.Diametro1, d.Diametro2
			 				  
	SELECT distinct ic.ItemCodeID,
		   ic.Codigo [CodigoItemCode], 
		   tm.Nombre [TipoMaterialNombreEspañol],
		   tm.NombreIngles [TipoMaterialNombreIngles],
		   ic.DescripcionEspanol,
		   ic.DescripcionIngles,
		   ici.Diametro1,
		   ici.Diametro2,
		   isnull(ms.Cantidad,0) [CantidadIngenieria],
		   isnull(nu.CantidadRecibida,0) [CantidadRecibida],
		   isnull(nu.TotalEntradaOtrosProcesos,0) [TotalEntradaOtrosProcesos],
		   isnull(nu.TotalCondicionada,0) [TotalCondicionada],
		   isnull(nu.TotalRechazada,0) [TotalRechazada],
		   isnull(nu.CantidadDanada,0) [CantidadDanada],
		   isnull(nu.CantidadRecibida,0) + isnull(nu.TotalEntradaOtrosProcesos,0) - isnull(nu.TotalCondicionada,0) - isnull(nu.TotalRechazada,0) - isnull(nu.CantidadDanada,0) [RecibidoNeto],
		   isnull(nu.TotalSalidasTemporales,0) [TotalSalidasTemporales],
		   isnull(nu.TotalOtrasSalidas,0) [TotalOtrasSalidas],
		   isnull(nu.TotalMerma,0) [TotalMerma],
		   isnull(odt.Cantidad,0) [CantidadOrdenTrabajo],
		   isnull(nu.TotalEnTransferenciaCorteICE, 0) [CantidadEnPreparacionEquivalente],
		   isnull(nu.TotalCortadoICE,0) [CantidadCortadaICE],
		   --isnull(de.TotalEnTransferenciaDesdeICE,0) [CantidadEnPreparacionDesdeICE],
		   --isnull(de.TotalCortadoDesdeICE,0) [CantidadCortadoDesdeICE],
		   isnull(nu.InventarioTransferenciaCorte,0) [InventarioTransferenciaCorte],
		   case when (isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0) - isnull(nu.TotalDespachadoParaICE,0))<0 then 0
		   else isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0) - isnull(nu.TotalDespachadoParaICE,0) end [TotalCorteSinDespacho],
		   isnull(de.CantidadDespachadaEquivalente,0) [CantidadDespachadaEquivalente],
		   isnull(nu.TotalDespachado,0) [TotalDespachado],
		   isnull(nu.TotalDespachadoParaICE,0) [TotalDespachadoParaICE],
		   isnull(odt.Cantidad,0) + isnull(nu.TotalEnTransferenciaCorteICE, 0) + isnull(nu.TotalCortadoICE,0) - isnull(nu.InventarioTransferenciaCorte,0) -
		   case when (isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0)- isnull(nu.TotalDespachadoParaICE,0))<0 then 0
		   else isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0)- isnull(nu.TotalDespachadoParaICE,0) end - isnull(nu.TotalDespachado,0) - isnull(de.CantidadDespachadaEquivalente,0) [TotalPorDespachar],
		   isnull(nu.InventarioBuenEstado,0) [InventarioFisico],
		   isnull(de.CantidadCongeladaEquivalente,0) - isnull(nu.TotalEnTransferenciaCorteICE, 0) - isnull(nu.TotalCortadoICE, 0) [CantidadCongeladaEquivalente],
		   isnull(nu.InventarioCongelado,0) - isnull(nu.InventarioTransferenciaCorte,0) -
		   case when (isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0) - isnull(nu.TotalDespachadoParaICE,0))<0 then 0
		   else isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0) - isnull(nu.TotalDespachadoParaICE,0) end  [InventarioCongelado],
		   case when isnull(nu.InventarioDisponibleCruce,0) < 0 then 0
		   else ISNULL(nu.InventarioDisponibleCruce,0) end [InventarioDisponibleCruce],
		   case when isnull(iceq.InventarioDisponibleCruceEquivalente,0) < 0 then 0
		   else isnull(iceq.InventarioDisponibleCruceEquivalente,0) end [InventarioDisponibleCruceEquivalente],
		   case when isnull(nu.InventarioDisponibleCruce,0) < 0 then 0
		   else ISNULL(nu.InventarioDisponibleCruce,0) end +
		   case when isnull(iceq.InventarioDisponibleCruceEquivalente,0) < 0 then 0
		   else isnull(iceq.InventarioDisponibleCruceEquivalente,0) end [TotalDisponibleCruce]		   	  
    FROM #TempItemCodeIntegrado ici
    INNER JOIN #TempItemCode ic on ici.ItemCodeID = ic.ItemCodeID
	LEFT JOIN #TempNumerosUnicosInventario nu on ici.ItemCodeID = nu.ItemCodeID and ici.Diametro1 = nu.Diametro1 and ici.Diametro2 = nu.Diametro2
	LEFT JOIN TipoMaterial tm on tm.TipoMaterialID = ic.TipoMaterialID
	LEFT JOIN #TempMaterialSpool ms on ici.ItemCodeID = ms.ItemCodeID  and ici.Diametro1 = ms.Diametro1 
		  and ici.Diametro2 = ms.Diametro2
	LEFT JOIN #TempODT odt on ici.ItemCodeID = odt.ItemCodeID  and ici.Diametro1 = odt.Diametro1 
		  and ici.Diametro2 = odt.Diametro2
	left JOIN #TempDespachadaEquivalente de on ici.ItemCodeID = de.ItemCodeID and ici.Diametro1 = de.Diametro1 
		  and ici.Diametro2 = de.Diametro2
	LEFT JOIN
	(
		SELECT	 ice.ItemCodeID,
				 isnull(SUM(nui.InventarioDisponibleCruce),0) [InventarioDisponibleCruceEquivalente],
				 isnull(SUM(nui.InventarioBuenEstado),0) [InventarioBuenEstadoEquivalente],
				 ice.Diametro1,
				 ice.Diametro2
		FROM #TempItemCodeIntegrado icod
		inner join ItemCodeEquivalente ice on icod.ItemCodeID = ice.ItemCodeID and ice.Diametro1 = icod.Diametro1 and ice.Diametro2 = icod.Diametro2
		inner join #TempNumerosUnicosInventario nui on nui.ItemCodeID = ice.ItemEquivalenteID and nui.Diametro1 = ice.DiametroEquivalente1 and nui.Diametro2 = ice.DiametroEquivalente2
		group by ice.ItemCodeID,
				 ice.Diametro1,
				 ice.Diametro2
	) iceq ON ici.ItemCodeID = iceq.ItemCodeID 
		   and nu.Diametro1 = iceq.Diametro1 
		   and nu.Diametro2 = iceq.Diametro2 


	SET NOCOUNT OFF;

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerConsultaDeNumerosUnicos]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerConsultaDeNumerosUnicos]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerConsultaDeNumerosUnicos
	Funcion:	Obtiene la cuanta de las juntas para una serie de spools
	Parametros:	@ProyectoID int
	Autor:		IHM
	Modificado:	29/10/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerConsultaDeNumerosUnicos]
(
	 @ProyectoID int,
	 @NumeroColada nvarchar(10),
	 @CodigoItemCode nvarchar(50),
	 @CodigoNumeroUnicoInicial nvarchar(20),
	 @CodigoNumeroUnicoFinal nvarchar(20)
)
AS
BEGIN

	SET NOCOUNT ON; 

	select	nu.NumeroUnicoID,
			nu.Codigo,
			nu.Diametro1,
			nu.Diametro2,
			nu.Factura,
			nu.PartidaFactura,
			nu.OrdenDeCompra,
			nu.PartidaOrdenDeCompra,
			nu.Cedula,
			nu.FabricanteID,
			nu.ProveedorID,
			nu.MarcadoAsme,
			nu.MarcadoGolpe,
			nu.MarcadoPintura,
			nu.PruebasHidrostaticas,
			nu.Estatus, 
			nu.TipoCorte1ID,
			nu.TipoCorte2ID,
			ic.ItemCodeID,
			ic.DescripcionEspanol,
			ic.Codigo [CodigoItemCode],
			ic.TipoMaterialID,
			c.NumeroColada,
			c.NumeroCertificado,
			c.AceroID,
			nui.CantidadDanada,
			nui.CantidadRecibida,
			nui.InventarioBuenEstado,
			nui.InventarioCongelado,
			nui.InventarioDisponibleCruce,
			nui.InventarioFisico,		
			nui.InventarioTransferenciaCorte,		
			r.TransportistaID,		
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (11)
						and num.Estatus = 'A'
			) [TotalEntradaOtrosProcesos],	
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (6)
						and num.Estatus = 'A'
			) [TotalSalidasTemporales],
			(
			select isnull(SUM(num.Cantidad),0) 
				from NumeroUnicoMovimiento num				
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (7) --Entrada de Pintura
						and num.Estatus = 'A'	
			) [TotalEntradasTemporales],
			(
				select isnull(SUM(d.Cantidad),0)
				from Despacho d 
				where d.NumeroUnicoID = nu.NumeroUnicoID
				and d.Cancelado = 0
			)	[TotalDespachado],
			(
				select isnull(SUM(d.Cantidad),0)
				from Despacho d 
				where d.NumeroUnicoID = nu.NumeroUnicoID
				and d.Cancelado = 0 and d.EsEquivalente = 1
			)	[TotalDespachadoParaICE],
			(				
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (18)
						and num.Estatus = 'A'
			) [TotalCorte],
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (5,9)
						and num.Estatus = 'A'
			) [TotalMerma],
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID not in (2,18,5,9,6)
						and num.Estatus = 'A'
						and exists
						(
							select 1
							from TipoMovimiento tp
							where tp.EsEntrada = 0
								  and tp.EsTransferenciaProcesos = 0
								  and tp.TipoMovimientoID = num.TipoMovimientoID
						)
			) [TotalOtrasSalidas]		
	from NumeroUnico nu
	inner join NumeroUnicoInventario nui on nu.NumeroUnicoID = nui.NumeroUnicoID
	inner join RecepcionNumeroUnico rnu on rnu.NumeroUnicoID = nu.NumeroUnicoID
	inner join Recepcion r on rnu.RecepcionID = r.RecepcionID
	left join Colada c on nu.ColadaID = c.ColadaID
	left join ItemCode ic on nu.ItemCodeID = ic.ItemCodeID
	where nu.ProyectoID = @ProyectoID
		  and (ISNULL(@NumeroColada,'') = '' or c.NumeroColada = @NumeroColada)
		  and (ISNULL(@CodigoItemCode,'') = '' or ic.Codigo = @CodigoItemCode)
		  and (ISNULL(@CodigoNumeroUnicoInicial,'') = '' or nu.Codigo >= @CodigoNumeroUnicoInicial)
		  and (ISNULL(@CodigoNumeroUnicoFinal,'') = '' or nu.Codigo <= @CodigoNumeroUnicoFinal)
	

	SET NOCOUNT OFF;

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerEstimacionJuntas]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerEstimacionJuntas]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


/****************************************************************************************
	Nombre:		ObtenerEstimacionJuntas
	Funcion:	Obtiene un listado con la estimación de las juntas y posibilidades para estimar
	Parametros:	@ProyectoID int
	Autor:		IHM
	Modificado:	10/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerEstimacionJuntas]
(
	 @ProyectoID int
)
AS
BEGIN

	SET NOCOUNT ON; 

	SELECT	 jw.[JuntaWorkstatusID]
			,jw.[OrdenTrabajoSpoolID]
			,jw.[JuntaSpoolID]
			,jw.[EtiquetaJunta]
			,jw.[ArmadoAprobado]
			,jw.[SoldaduraAprobada]
			,jw.[InspeccionVisualAprobada]
			,js.Diametro
			,js.FamiliaAceroMaterial1ID
			,js.FamiliaAceroMaterial2ID
			,js.TipoJuntaID
			,ots.NumeroControl
			,s.Nombre [NombreSpool]
			,tEst.EstimacionJuntaID
			,tEst.EstimacionID
			,tEst.ConceptoEstimacionID
			,tEst.NumeroEstimacion
			,tRepPnd.JuntaReportePndID
			,tRepPnd.TipoPruebaID [TipoPruebaPndID]
			,tRepPnd.Aprobado [AprobadoPnd]
			,tRepTt.JuntaReporteTtID
			,tRepTt.TipoPruebaID [TipoPruebaTtID]
			,tRepTt.Aprobado [AprobadoTt]
			,tRepD.ReporteDimensionalDetalleID
			,tRepD.ReporteDimensionalID
			,tRepD.Aprobado [AprobadoReporteDimensional]
	FROM [JuntaWorkstatus] jw
	INNER JOIN [OrdenTrabajoSpool] ots on jw.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	INNER JOIN [JuntaSpool] js on js.JuntaSpoolID = jw.JuntaSpoolID
	INNER JOIN
	(
		SELECT SpoolID, Nombre
		FROM Spool
		WHERE ProyectoID = @ProyectoID
	) s on s.SpoolID = ots.SpoolID
	LEFT JOIN
	(
		SELECT	 esj.[EstimacionJuntaID]
				,esj.[ConceptoEstimacionID]
				,esj.[JuntaWorkstatusID]
				,est.[EstimacionID]
				,est.[NumeroEstimacion]
				,est.[FechaEstimacion]
		FROM [EstimacionJunta] esj
		INNER JOIN [Estimacion] est on esj.EstimacionID = est.EstimacionID
		WHERE est.ProyectoID = @ProyectoID	
	) tEst ON jw.JuntaWorkstatusID = tEst.JuntaWorkstatusID
	LEFT JOIN
	(
		SELECT	 jrpnd.[JuntaReportePndID]
				,jrpnd.[JuntaWorkstatusID]
				,jrpnd.Aprobado
				,rpnd.TipoPruebaID
		FROM [JuntaReportePnd] jrpnd
		INNER JOIN [ReportePnd] rpnd on jrpnd.ReportePndID = rpnd.ReportePndID
		WHERE rpnd.ProyectoID = @ProyectoID
	) tRepPnd ON jw.JuntaWorkstatusID = tRepPnd.JuntaWorkstatusID
	LEFT JOIN
	(
		SELECT	 jrtt.[JuntaReporteTtID]
				,jrtt.[JuntaWorkstatusID]
				,jrtt.Aprobado
				,rtt.TipoPruebaID
		FROM [JuntaReporteTt] jrtt
		INNER JOIN [ReporteTt] rtt on jrtt.ReporteTtID = rtt.ReporteTtID
		WHERE rtt.ProyectoID = @ProyectoID
	) tRepTt ON jw.JuntaWorkstatusID = tRepTt.JuntaWorkstatusID
	LEFT JOIN
	(
		SELECT	 rdd.ReporteDimensionalDetalleID
				,rdd.ReporteDimensionalID
				,rdd.Aprobado
				,ws.OrdenTrabajoSpoolID
		FROM [ReporteDimensionalDetalle] rdd
		INNER JOIN [ReporteDimensional] rd ON rdd.ReporteDimensionalID = rd.ReporteDimensionalID
		INNER JOIN [WorkstatusSpool] ws ON rdd.WorkstatusSpoolID = ws.WorkstatusSpoolID
		WHERE	rd.ProyectoID = @ProyectoID
				AND rd.TipoReporteDimensionalID = 1
				AND ws.TieneLiberacionDimensional = 1
	) tRepD ON jw.OrdenTrabajoSpoolID = tRepD.OrdenTrabajoSpoolID
	WHERE EXISTS
	(
		SELECT 1
		FROM OrdenTrabajo ot
		WHERE	ot.ProyectoID = @ProyectoID
				AND ots.OrdenTrabajoID = ot.OrdenTrabajoID
	)
	
	SET NOCOUNT OFF;

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerEstimacionSpools]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerEstimacionSpools]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


/****************************************************************************************
	Nombre:		ObtenerEstimacionSpools
	Funcion:	Obtiene un listado con la estimación de los spools y posibilidades para estimar
	Parametros:	@ProyectoID int
	Autor:		IHM
	Modificado:	10/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerEstimacionSpools]
(
	 @ProyectoID int
)
AS
BEGIN

	SET NOCOUNT ON; 

	SELECT	 ws.[WorkstatusSpoolID]
			,ws.[OrdenTrabajoSpoolID]
			,ws.[Embarcado]
			,ws.[TieneLiberacionDimensional]
			,ws.[LiberadoPintura]
			,sp.Pdis
			,sp.Nombre
			,ots.NumeroControl
			,tEst.EstimacionSpoolID
			,tEst.EstimacionID
			,tEst.ConceptoEstimacionID
			,tEst.NumeroEstimacion
	FROM WorkstatusSpool ws
	INNER JOIN [OrdenTrabajoSpool] ots on ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	INNER JOIN [Spool] sp on sp.SpoolID = ots.SpoolID
	LEFT JOIN
	(
		SELECT	 ess.[EstimacionSpoolID]
				,ess.[ConceptoEstimacionID]
				,ess.[WorkstatusSpoolID]
				,est.[EstimacionID]
				,est.[NumeroEstimacion]
				,est.[FechaEstimacion]
		FROM [EstimacionSpool] ess
		INNER JOIN [Estimacion] est on ess.EstimacionID = est.EstimacionID
		WHERE est.ProyectoID = @ProyectoID	
	) tEst ON ws.WorkstatusSpoolID = tEst.WorkstatusSpoolID
	WHERE EXISTS
	(
		SELECT 1
		FROM OrdenTrabajo ot
		WHERE	ot.ProyectoID = @ProyectoID
				AND ots.OrdenTrabajoID = ot.OrdenTrabajoID
	)
	
	SET NOCOUNT OFF;

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerGrupoDeJuntasPorSpoolIds]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerGrupoDeJuntasPorSpoolIds]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerGrupoDeJuntasPorSpoolIds
	Funcion:	Obtiene la cuanta de las juntas para una serie de spools
	Parametros:	@SpoolIDs nvarchar(max)
	Autor:		IHM
	Modificado:	28/10/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerGrupoDeJuntasPorSpoolIds]
(
	 @SpoolIDs	NVARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	 [SpoolID]
			,COUNT(JuntaSpoolID) [Cuenta]
			,CAST(0 as int) [Suma]
	FROM [JuntaSpool]
	WHERE [SpoolID] IN
	(
		SELECT CAST([Value] AS INT)
		FROM dbo.SplitCVSToTable(@SpoolIDs,',')
	)
	GROUP BY [SpoolID]



	SET NOCOUNT OFF;

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerInfoCaratulaSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerInfoCaratulaSpool]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
/****************************************************************************************
	Nombre:		ObtenerInfoCaratulaSpool
	Funcion:	Obtiene la info para la caratula del los spools enviados
	Parametros:	@SpoolIDs nvarchar(max)
	Autor:		SCB
	Modificado:	21/12/2010
*****************************************************************************************/
CREATE PROCEDURE ObtenerInfoCaratulaSpool
(
	@SpoolIDs NVARCHAR(MAX),
	@ProyectoID INT = NULL
)
AS 
BEGIN

	SET NOCOUNT ON;


	CREATE TABLE #OrdenTrabajoSpool
	(
		OrdenTrabajoSpoolID INT, 
		SpoolID INT,
		NumeroControl NVARCHAR(50),
		Nombre NVARCHAR(50),
	)

	CREATE TABLE #WorkstatusSpool
	(
		OrdenTrabajoSpoolID INT, 	
		WorkstatusSpoolID INT	
	)


	CREATE TABLE #SpoolIDs
	(
		SpoolID INT
	)

	CREATE TABLE #JuntaSpoolIDs
	(
		JuntaSpoolID INT
	)

	CREATE TABLE #JuntaWorkstatus(
		JuntaWorkstatusID INT,
		EtiquetaJunta NVARCHAR(50), 
		JuntaSpoolID INT, 
		JuntaArmadoID INT, 
		JuntaSoldaduraID INT 
	)

	CREATE TABLE #NumeroUnico(
		NumeroUnicoID INT
	)

	IF(@ProyectoID IS NULL)
		BEGIN

			INSERT INTO #SpoolIDs
			SELECT CAST([Value] AS INT)
			FROM dbo.SplitCVSToTable(@SpoolIDs,',')
			
		END
	ELSE 
		BEGIN
			INSERT INTO #SpoolIDs
			SELECT SpoolID
			FROM Spool 
			WHERE ProyectoID = @ProyectoID
		END	
	

	INSERT INTO #OrdenTrabajoSpool 
	SELECT OrdenTrabajoSpoolID, s.SpoolID, NumeroControl, Nombre
	FROM OrdenTrabajoSpool ots
	INNER JOIN Spool s 
		ON s.SpoolID = ots.SpoolID
	WHERE s.[SpoolID] IN
		(
			SELECT SpoolID FROM #SpoolIDs
		)

	INSERT INTO #WorkstatusSpool
	SELECT OrdenTrabajoSpoolID, WorkstatusSpoolID 
	FROM WorkstatusSpool 
	WHERE OrdenTrabajoSpoolID IN(
		SELECT OrdenTrabajoSpoolID FROM #OrdenTrabajoSpool
	)
	
	SELECT	s.SpoolID,
			s.Dibujo, 
			s.RevisionCliente, 
			ots.OrdenTrabajoSpoolID, 
			ws.WorkstatusSpoolID,
			ots.Nombre,
			ots.NumeroControl
	FROM Spool s
	LEFT JOIN #OrdenTrabajoSpool ots 
		ON ots.SpoolID = s.SpoolID
	LEFT JOIN WorkstatusSpool ws 
		ON ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	WHERE s.[SpoolID] IN
	(
		SELECT SpoolID FROM #SpoolIDs
	)

	
	SELECT	WorkstatusSpoolID, 
			ReportePrimarios, 
			ps.ReporteAdherencia, 
			ps.ReportePullOff, 
			ps.ReporteIntermedios, 
			ps.ReporteAcabadoVisual
	FROM PinturaSpool ps
	WHERE WorkstatusSpoolID IN(
		SELECT WorkstatusSpoolID FROM #WorkstatusSpool
	)
	
	SELECT	NumeroReporte, 
			TipoReporteDimensionalID, 
			WorkstatusSpoolID 
	FROM ReporteDimensionalDetalle rdd
	INNER JOIN ReporteDimensional rd 
		ON rd.ReporteDimensionalID = rdd.ReporteDimensionalID
	WHERE Aprobado =1 AND WorkstatusSpoolID IN(
		SELECT WorkstatusSpoolID FROM #WorkstatusSpool
	)
	ORDER BY FechaLiberacion DESC
	
	
	INSERT INTO #JuntaSpoolIDs
	SELECT JuntaSpoolID 
	FROM JuntaSpool 
	WHERE SpoolID IN 
		(
			SELECT SpoolID FROM #SpoolIDs
		)
		
		
	INSERT INTO #JuntaWorkstatus	
	SELECT  JuntaWorkstatusID,
			EtiquetaJunta, 
			JuntaSpoolID , 
			JuntaArmadoID, 
			JuntaSoldaduraID 
	FROM JuntaWorkstatus 
	WHERE JuntaSpoolID IN(SELECT JuntaSpoolID FROM #JuntaSpoolIDs)	
		
	INSERT INTO #NumeroUnico
	SELECT DISTINCT NumeroUnico1ID 
	FROM JuntaArmado ja 
		WHERE ja.JuntaArmadoID IN (SELECT JuntaArmadoID FROM #JuntaWorkstatus)
	UNION
	SELECT DISTINCT NumeroUnico2ID 
	FROM JuntaArmado ja 
		WHERE ja.JuntaArmadoID IN (SELECT JuntaArmadoID FROM #JuntaWorkstatus) 
	
	SELECT	nu.NumeroUnicoID, 
			ic.DescripcionEspanol, 
			ic.DescripcionIngles, 
			ic.Codigo as CodigoMaterial, 
			c.NumeroColada, 
			c.NumeroCertificado as Certificado
	FROM NumeroUnico nu
	INNER JOIN Colada c 
		ON nu.ColadaID = c.ColadaID
	INNER JOIN ItemCode ic 
		ON ic.ItemCodeID = nu.ItemCodeID
	WHERE nu.NumeroUnicoID IN(SELECT NumeroUnicoID FROM #NumeroUnico)
		
	SELECT	js.SpoolID,
			jws.EtiquetaJunta, 
			tj.Codigo as TipoJunta, 
			js.Diametro, 
			js.Cedula, 
			ja.FechaArmado,
			jsold.FechaSoldadura, 
			Wps.Nombre as WPS, 
			jsoldDet.TecnicaSoldadorID, 
			jsoldDet.SoldadorID, 
			sold.Codigo as CodigoSoldador, 
			rPND.TipoPruebaID as PndTipoPruebaID, 
			rPND.NumeroReporte as ReportePnd, 
			rTT.TipoPruebaID as TtTipoPruebaID, 
			rTT.NumeroReporte as ReporteTt, 			
			ja.NumeroUnico1ID,
			ja.NumeroUnico2ID
	FROM #JuntaWorkstatus jws
	INNER JOIN JuntaSpool js
		ON js.JuntaSpoolID = jws.JuntaSpoolID
	INNER JOIN TipoJunta tj
		ON tj.TipoJuntaID = js.TipoJuntaID	
	LEFT JOIN JuntaArmado ja
		ON ja.JuntaArmadoID = jws.JuntaArmadoID
	LEFT JOIN JuntaSoldadura jsold
		ON jws.JuntaSoldaduraID = jsold.JuntaSoldaduraID
	LEFT JOIN JuntaSoldaduraDetalle jsoldDet
		ON jsoldDet.JuntaSoldaduraID = jsold.JuntaSoldaduraID
	LEFT JOIN Wps 
		ON Wps.WpsID = jsold.WpsID
	LEFT JOIN Soldador sold 
		ON jsoldDet.SoldadorID = sold.SoldadorID
	LEFT JOIN JuntaReporteTt jrTT
		ON jrTT.JuntaWorkstatusID = jws.JuntaWorkstatusID
	LEFT JOIN ReporteTt rTT
		ON rTT.ReporteTtID = jrTT.ReporteTtID
	LEFT JOIN JuntaReportePnd jrPND
		ON jrPND.JuntaWorkstatusID = jws.JuntaWorkstatusID
	LEFT JOIN ReportePnd rPND
		ON rPND.ReportePndID = jrPND.ReportePndID
	LEFT JOIN Consumible c
		ON c.ConsumibleID = jsoldDet.ConsumibleID		
	
	
	DROP TABLE #JuntaSpoolIDs
	DROP TABLE #SpoolIDs
	DROP TABLE #OrdenTrabajoSpool
	DROP TABLE #WorkstatusSpool
	DROP TABLE #NumeroUnico
	DROP TABLE #JuntaWorkstatus
	
	SET NOCOUNT OFF;
	
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerInfoPorcPnd]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerInfoPorcPnd]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerInfoPorcPnd
	Funcion:	Obtiene la infop relevante a numeros de juntas Rt aplicar,etc
	Parametros:	@SpoolID INT
	Autor:		SAC
	Modificado:	12/23/2010
*****************************************************************************************/
CREATE PROCEDURE dbo.ObtenerInfoPorcPnd
(@SpoolID INT) AS
BEGIN	
	
	DECLARE @Dibujo NVARCHAR(50) = ''
	DECLARE @ProyectoID INT
	DECLARE @PorcPND INT = 0
	DECLARE @BW INT
	DECLARE @OLET INT
	DECLARE @NumJuntas INT
	DECLARE @NumJuntasAplicarRT INT
	DECLARE @NumJuntasAplicarPT INT
	DECLARE @NumJuntasRTAprobado INT
	DECLARE @NumJuntasPTAprobado INT
	DECLARE @TipoPruebaPTID INT = 2
	DECLARE @TipoPruebaRTID INT = 1
	DECLARE @FaltantesRT INT	
	DECLARE @FaltantesPT INT
	DECLARE @FaltantesTotales INT
	
	SELECT @Dibujo = Dibujo, @PorcPND = PorcentajePnd, @ProyectoID = ProyectoID  FROM Spool WHERE SpoolID = @SpoolID
	SELECT @BW = TipoJuntaID  FROM TipoJunta WHERE Codigo = 'BW'
	SELECT @OLET = TipoJuntaID  FROM TipoJunta WHERE Codigo = 'LET'
	
	
	SELECT JuntaSpoolID, TipoJuntaID
		INTO #JuntaSpoolIDS 
	FROM JuntaSpool 
	WHERE  (TipoJuntaID = @BW OR TipoJuntaID = @OLET) AND
		   SpoolID IN(
					SELECT SpoolID 
					FROM Spool 
					WHERE Dibujo = @Dibujo AND ProyectoID = @ProyectoID
					)
	
	
	IF @PorcPND < 100
		BEGIN
			SELECT @NumJuntas = COUNT(*) 
			FROM #JuntaSpoolIDS
			WHERE TipoJuntaID = @BW
			
			SELECT @NumJuntasAplicarRT = CEILING((@PorcPND * @NumJuntas)/100.0)
			SELECT @NumJuntasRTAprobado = COUNT(jr.JuntaReportePndID)  
			FROM JuntaReportePnd jr 
			INNER JOIN ReportePnd pnd 
				ON jr.ReportePndID = pnd.ReportePndID
			INNER JOIN JuntaWorkstatus jw 
				ON jw.JuntaWorkstatusID = jr.JuntaWorkstatusID
			WHERE TipoPruebaID = @TipoPruebaRTID 
				AND Aprobado = 1 
				AND JuntaSpoolID IN (SELECT JuntaSpoolID FROM #JuntaSpoolIDS)
		END
	ELSE
		BEGIN			
			
			SELECT @NumJuntasAplicarRT = COUNT(*) 
			FROM #JuntaSpoolIDS
			WHERE TipoJuntaID = @BW
			
			SELECT @NumJuntasAplicarPT = COUNT(*) 
			FROM #JuntaSpoolIDS
			WHERE TipoJuntaID = @OLET
			
			SELECT @NumJuntasRTAprobado = COUNT(jr.JuntaReportePndID)  
			FROM JuntaReportePnd jr 
			INNER JOIN ReportePnd pnd 
				ON jr.ReportePndID = pnd.ReportePndID
			INNER JOIN JuntaWorkstatus jw 
				ON jw.JuntaWorkstatusID = jr.JuntaWorkstatusID
			WHERE TipoPruebaID = @TipoPruebaRTID 
				AND Aprobado = 1
				AND JuntaSpoolID IN (SELECT JuntaSpoolID FROM #JuntaSpoolIDS)
			
			SELECT @NumJuntasPTAprobado = COUNT(jr.JuntaReportePndID)  
			FROM JuntaReportePnd jr 
			INNER JOIN ReportePnd pnd 
				ON jr.ReportePndID = pnd.ReportePndID
			INNER JOIN JuntaWorkstatus jw 
				ON jw.JuntaWorkstatusID = jr.JuntaWorkstatusID
			WHERE TipoPruebaID = @TipoPruebaPTID 
				AND Aprobado = 1
				AND JuntaSpoolID IN (SELECT JuntaSpoolID FROM #JuntaSpoolIDS)
		END
		
	SELECT @FaltantesPT = ISNULL(@NumJuntasAplicarPT,0) - ISNULL(@NumJuntasPTAprobado,0)
	IF @FaltantesPT < 0
		SELECT @FaltantesPT = 0
		
	SELECT @FaltantesRT = ISNULL(@NumJuntasAplicarRT,0) - ISNULL(@NumJuntasRTAprobado,0)
	IF @FaltantesRT < 0
		SELECT @FaltantesRT = 0
	
	SELECT @FaltantesTotales = @FaltantesPT + @FaltantesRT	
	
	SELECT  @PorcPND AS PorcantajePnd, 
			@NumJuntas AS JuntasTotales,
			ISNULL(@NumJuntasAplicarPT,0) AS NumJuntasAplicarPT,
			ISNULL(@NumJuntasPTAprobado,0) AS NumJuntasPTAprobado, 
			@FaltantesPT AS FaltantesPT,
			@NumJuntasAplicarRT AS NumJuntasAplicarRT,
			@NumJuntasRTAprobado AS NumJuntasRTAprobado,
			@FaltantesRT AS FaltantesRT,
			@FaltantesTotales AS FaltantesTotales
			
			 
	DROP TABLE #JuntaSpoolIDS
END			  

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSeguimientoDeJuntasPorJuntaWorkstatus]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorJuntaWorkstatus]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerSeguimientoDeJuntasPorJuntaWorkstatus
	Funcion:	Trae toda la informacion necesaria para el seguimiento de jutnas
	Parametros:	@JuntaWorkstatusID INT
				@HistorialRep BIT
	Autor:		MMG
	Modificado:	28/01/2011 IHM, 02/01/2011 SCB
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorJuntaWorkstatus]
(
	@JuntaWorkstatusID INT
)
AS
BEGIN
	
	SET NOCOUNT ON; 

	--TABLA PROYECTO
	CREATE TABLE #TempProyecto
	(	
		ProyectoID INT,
		Patio INT,
		Nombre NVARCHAR(100)
	)
	
	--TABLA ORDEN DE TRABAJO
	CREATE TABLE #TempOrdenTrabajo
	(	
		OrdenTrabajoID INT,
		NumeroOrden NVARCHAR(50)
	)
	
	--TABLA ORDEN TRABAJO SPOOL
	CREATE TABLE #TempOrdenTrabajoSpool
	(	
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoID INT,
		NumeroControl NVARCHAR(50),
		SpoolID INT
	)
	
	--TABLA SPOOL
	CREATE TABLE #TempSpool 
	(	
		SpoolID INT,
		Nombre NVARCHAR(50)
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempJuntaSpool 
	(	
		JuntaSpoolID INT,
		SpoolID INT,
		TipoJuntaID INT,
		Diametro DECIMAL(7,4),
		Cedula NVARCHAR(10),
		Espesor DECIMAL(10,4),
		EtiquetaMaterial1 NVARCHAR(10),
		EtiquetaMaterial2 NVARCHAR(10),
		FamiliaAceroMaterial1ID INT,
		FamiliaAceroMaterial2ID INT,
		peqs DECIMAL(10,4),
		KgTeoricos DECIMAL(12,4),
		Etiqueta1EsNumero BIT,
		Etiqueta2EsNumero BIT,
		ValorNumericoEtiqueta1 TINYINT,
		ValorNumericoEtiqueta2 TINYINT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150),
		FabAreaID INT	
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempMaterialSpool
	(	
		MaterialSpoolID INT,
		SpoolID INT,
		ItemCodeID INT,
		Etiqueta NVARCHAR(10),
		CodigoItemCode NVARCHAR(50),
		DescripcionItemCode NVARCHAR(150),
		EtiquetaEsNumero BIT,
		ValorNumericoEtiqueta TINYINT
	)
	
	--TABLA JUNTA WORKSTATUS
	CREATE TABLE #TempJuntaWorkstatus 
	(	
		JuntaWorkStatusID INT,
		JuntaSpoolID INT,
		JuntaArmadoID INT,
		JuntaSoldaduraID INT,
		JuntaInspeccionVisualID INT,
		OrdenTrabajoSpoolID INT,
		UltimoProcesoID INT,
		EtiquetaJunta NVARCHAR(50),
		JuntaFinal BIT
	)
	
	--TABLA JUNTA SOLDADURA
	CREATE TABLE #TempJuntaSoldadura 
	(	
		SoldaduraJuntaSoldaduraID INT,
		SoldaduraJuntaWorkstatusID INT,
		SoldaduraFecha DATETIME,
		SoldaduraFechaReporte DATETIME,
		SoldaduraTaller NVARCHAR(50),
		SoldaduraWPS NVARCHAR(50),
		SoldaduraProcesoRelleno NVARCHAR(50),
		SoldaduraConsumiblesRelleno NVARCHAR(50),
		SoldaduraProcesoRaiz NVARCHAR(50),
		SoldaduraSoldadorRaiz NVARCHAR(50),
		SoldaduraSoldadorRelleno NVARCHAR(50),
		SoldaduraMaterialBase1 NVARCHAR(50),
		SoldaduraMaterialBase2 NVARCHAR(50)
	)
	
	--TABLA JUNTA REPORTE TT
	CREATE TABLE #TempJuntaReporteTt 
	(	
		JuntaReporteTtID INT,
		JuntaWorkstatusID INT,
		TipoPruebaID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		JuntaRequisicionID INT,
		Aprobado BIT,
		NumeroGrafica  NVARCHAR(20),
		Hoja INT,
		FechaTratamiento DATETIME,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA JUNTA REPORTE PND
	CREATE TABLE #TempJuntaReportePnd 
	(	
		JuntaReportePndID INT,
		JuntaWorkstatusID INT,
		TipoPruebaID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		JuntaRequisicionID INT,		
		FechaPrueba DATETIME,
		Hoja INT,
		Aprobado BIT,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME
	)
			
	--TABLA REPORTE DIMENSIONAL
	CREATE TABLE #TempReporteDimensional 
	(	
		ReporteDimensionalID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		TipoReporteDimensionalID INT,
		FechaModificacion DATETIME
	)
	
	--TABLA INSPECCION DIMENSIONAL
	CREATE TABLE #TempInspeccionDimensional 
	(	
		InspeccionDimensionalReporteDimensionalDetalleID INT,
		InspeccionDimensionalWorkstatusSpoolID INT,
		InspeccionDimensionalFecha DATETIME,
		InspeccionDimensionalFechaReporte DATETIME,
		InspeccionDimensionalNumeroReporte NVARCHAR(50),
		InspeccionDimensionalHoja INT,
		InspeccionDimensionalResultado BIT,
		InspeccionDimensionalObservaciones NVARCHAR(500),
		InspeccionDimensionalFechaLiberacion DATETIME,		
		FechaModificacion DATETIME
	)
	
	--TABLA INSPECCION ESPESORES
	CREATE TABLE #TempInspeccionEspesores 
	(	
		InspeccionEspesoresReporteDimensionalDetalleID INT,
		InspeccionEspesoresWorkstatusSpoolID INT,
		InspeccionEspesoresFecha DATETIME,
		InspeccionEspesoresFechaReporte DATETIME,
		InspeccionEspesoresNumeroReporte NVARCHAR(50),
		InspeccionEspesoresHoja INT,
		InspeccionEspesoresResultado BIT,
		InspeccionEspesoresObservaciones NVARCHAR(500),
		InspeccionEspesoresFechaLiberacion DATETIME,
		FechaModificacion DATETIME
	)	
	
	--TABLA WORKSTATUS SPOOL
	CREATE TABLE #TempWorkstatusSpool 
	(	
		WorkstatusSpoolID INT,
		OrdenTrabajoSpoolID INT,
		PinturaSistema NVARCHAR(50),
		PinturaColor NVARCHAR(50),
		PinturaCodigo NVARCHAR(50),
		EmbarqueEtiqueta NVARCHAR(20),
		FechaEtiqueta DATETIME,
		FechaPreparacion DATETIME		
	)
		
	--TABLA JUNTA INSPECCION VISUAL
	CREATE TABLE #TempJuntaInspeccionVisual 
	(	
		InspeccionVisualJuntaInspeccionVisualID INT,
		InspeccionVisualJuntaWorkstatusID INT,
		InspeccionVisualFecha DATETIME,
		InspeccionVisualFechaReporte DATETIME,
		InspeccionVisualNumeroReporte NVARCHAR(50),
		InspeccionVisualHoja INT,
		InspeccionVisualResultado BIT,
		InspeccionVisualDefecto NVARCHAR(MAX),
		InspeccionVisualObservaciones NVARCHAR(500)		
	)
	
	CREATE TABLE #TempJuntaInspeccionVisualDefecto
	(
		JuntaInspeccionVisualID int,
		JuntaWorkstatusID int,
		Fecha datetime,
		FechaReporte datetime,
		NumeroReporte nvarchar(50),
		Hoja int,
		Resultado bit,
		Defecto nvarchar(100),
		Observaciones nvarchar(500),
		FechaModificacion datetime
	)
	
	--TABLA PRUEBA RT
	CREATE TABLE #TempPruebaRT 
	(	
		PruebaRTJuntaReportePndID INT,
		PruebaRTJuntaWorkstatusID INT,
		PruebaRTFechaRequisicion DATETIME,
		PruebaRTNumeroRequisicion NVARCHAR(50),
		PruebaRTCodigoRequisicion NVARCHAR(50),
		PruebaRTFechaPrueba DATETIME,
		PruebaRTFechaReporte DATETIME,
		PruebaRTNumeroReporte NVARCHAR(50),
		PruebaRTHoja INT,
		PruebaRTResultado BIT,
		PruebaRTDefecto NVARCHAR(MAX),
		PruebaRTObservacionesReporte NVARCHAR(500),
		PruebaRTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA PT
	CREATE TABLE #TempPruebaPT 
	(	
		PruebaPTJuntaReportePndID INT,
		PruebaPTJuntaWorkstatusID INT,
		PruebaPTFechaRequisicion DATETIME,
		PruebaPTNumeroRequisicion NVARCHAR(50),
		PruebaPTCodigoRequisicion NVARCHAR(50),
		PruebaPTFechaPrueba DATETIME,
		PruebaPTFechaReporte DATETIME,
		PruebaPTNumeroReporte NVARCHAR(50),
		PruebaPTHoja INT,
		PruebaPTResultado BIT,
		PruebaPTDefecto NVARCHAR(MAX),
		PruebaPTObservacionesReporte NVARCHAR(500),
		PruebaPTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA RT(PostTT)
	CREATE TABLE #TempPruebaRTPostTT 
	(	
		PruebaRTPostTTJuntaReportePndID INT,
		PruebaRTPostTTJuntaWorkstatusID INT,
		PruebaRTPostTTFechaRequisicion DATETIME,
		PruebaRTPostTTNumeroRequisicion NVARCHAR(50),
		PruebaRTPostTTCodigoRequisicion NVARCHAR(50),
		PruebaRTPostTTFechaPrueba DATETIME,
		PruebaRTPostTTFechaReporte DATETIME,
		PruebaRTPostTTNumeroReporte NVARCHAR(50),
		PruebaRTPostTTHoja INT,
		PruebaRTPostTTResultado BIT,
		PruebaRTPostTTDefecto NVARCHAR(MAX),
		PruebaRTPostTTObservacionesReporte NVARCHAR(500),
		PruebaRTPostTTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA PT(PostTT)
	CREATE TABLE #TempPruebaPTPostTT 
	(	
		PruebaPTPostTTJuntaReportePndID INT,
		PruebaPTPostTTJuntaWorkstatusID INT,
		PruebaPTPostTTFechaRequisicion DATETIME,
		PruebaPTPostTTNumeroRequisicion NVARCHAR(50),
		PruebaPTPostTTCodigoRequisicion NVARCHAR(50),
		PruebaPTPostTTFechaPrueba DATETIME,
		PruebaPTPostTTFechaReporte DATETIME,
		PruebaPTPostTTNumeroReporte NVARCHAR(50),
		PruebaPTPostTTHoja INT,
		PruebaPTPostTTResultado BIT,
		PruebaPTPostTTDefecto NVARCHAR(MAX),
		PruebaPTPostTTObservacionesReporte NVARCHAR(500),
		PruebaPTPostTTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA UT
	CREATE TABLE #TempPruebaUT 
	(	
		PruebaUTJuntaReportePndID INT,
		PruebaUTJuntaWorkstatusID INT,
		PruebaUTFechaRequisicion DATETIME,
		PruebaUTNumeroRequisicion NVARCHAR(50),
		PruebaUTCodigoRequisicion NVARCHAR(50),
		PruebaUTFechaPrueba DATETIME,
		PruebaUTFechaReporte DATETIME,
		PruebaUTNumeroReporte NVARCHAR(50),
		PruebaUTHoja INT,
		PruebaUTResultado BIT,
		PruebaUTDefecto NVARCHAR(MAX),
		PruebaUTObservacionesReporte NVARCHAR(500),
		PruebaUTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA TRATAMIENTO PWHT
	CREATE TABLE #TempTratamientoPwht 
	(	
		TratamientoPwhtJuntaReporteTtID INT,
		TratamientoPwhtJuntaWorkstatusID INT,
		TratamientoPwhtFechaRequisicion DATETIME,
		TratamientoPwhtNumeroRequisicion NVARCHAR(50),
		TratamientoPwhtCodigoRequisicion NVARCHAR(50),
		TratamientoPwhtFechaTratamiento DATETIME,
		TratamientoPwhtFechaReporte DATETIME,
		TratamientoPwhtNumeroReporte NVARCHAR(50),
		TratamientoPwhtHoja INT,
		TratamientoPwhtGrafica NVARCHAR(20),
		TratamientoPwhtResultado BIT,
		TratamientoPwhtObservacionesReporte NVARCHAR(500),
		TratamientoPwhtObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME		
	)
	
	--TABLA TRATAMIENTO DUREZAS
	CREATE TABLE #TempTratamientoDurezas 
	(	
		TratamientoDurezasJuntaReporteTtID INT,
		TratamientoDurezasJuntaWorkstatusID INT,
		TratamientoDurezasFechaRequisicion DATETIME,
		TratamientoDurezasNumeroRequisicion NVARCHAR(50),
		TratamientoDurezasCodigoRequisicion NVARCHAR(50),
		TratamientoDurezasFechaTratamiento DATETIME,
		TratamientoDurezasFechaReporte DATETIME,
		TratamientoDurezasNumeroReporte NVARCHAR(20),
		TratamientoDurezasHoja INT,
		TratamientoDurezasGrafica NVARCHAR(20),
		TratamientoDurezasResultado BIT,
		TratamientoDurezasObservacionesReporte NVARCHAR(500),
		TratamientoDurezasObservacionesRequisicion NVARCHAR(500),		
		FechaModificacion DATETIME
	)
	
	--TABLA TRATAMIENTO PREHEAT
	CREATE TABLE #TempTratamientoPreheat 
	(	
		TratamientoPreheatJuntaReporteTtID INT,
		TratamientoPreheatJuntaWorkstatusID INT,
		TratamientoPreheatFechaRequisicion DATETIME,
		TratamientoPreheatNumeroRequisicion NVARCHAR(50),
		TratamientoPreheatCodigoRequisicion NVARCHAR(50),
		TratamientoPreheatFechaTratamiento DATETIME,
		TratamientoPreheatFechaReporte DATETIME,
		TratamientoPreheatNumeroReporte NVARCHAR(50),
		TratamientoPreheatHoja INT,
		TratamientoPreheatGrafica NVARCHAR(20),
		TratamientoPreheatResultado BIT,
		TratamientoPreheatObservacionesReporte NVARCHAR(500),
		TratamientoPreheatObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PINTURA
	CREATE TABLE #TempPintura 
	(	
		PinturaPinturaSpoolID INT,
		PinturaWorkstatusSpoolID INT,
		PinturaFechaRequisicion DATETIME,
		PinturaNumeroRequisicion NVARCHAR(50),
		PinturaSistema NVARCHAR(50),
		PinturaColor NVARCHAR(50),
		PinturaCodigo NVARCHAR(50),
		PinturaFechaSendBlast DATETIME,
		PinturaReporteSendBlast NVARCHAR(50),
		PinturaFechaPrimarios DATETIME,
		PinturaReportePrimarios NVARCHAR(50),
		PinturaFechaIntermedios DATETIME,
		PinturaReporteIntermedios NVARCHAR(50),
		PinturaFechaAcabadoVisual DATETIME,
		PinturaReporteAcabadoVisual NVARCHAR(50),
		PinturaFechaAdherencia DATETIME,
		PinturaReporteAdherencia NVARCHAR(50),
		PinturaFechaPullOff DATETIME,
		PinturaReportePullOff NVARCHAR(50)
	)
	
	--TABLA EMBARQUE
	CREATE TABLE #TempEmbarque 
	(
		EmbarqueEmbarqueSpoolID INT,
		EmbarqueWorkstatusSpoolID INT,
		EmbarqueEtiqueta NVARCHAR(20),
		EmbarqueFechaEtiqueta DATETIME,
		EmbarqueFechaPreparacion DATETIME,
		EmbarqueFechaEmbarque DATETIME,
		EmbarqueNumeroEmbarque NVARCHAR(50)
	)
	
	--TABLA GENERAL
	CREATE TABLE #TempGeneral 
	(
		GeneralJuntaWorkstatusID INT,
		GeneralProyecto NVARCHAR(50),
		GeneralOrdenDeTrabajo NVARCHAR(50),
		GeneralNumeroDeControl NVARCHAR(50),
		GeneralSpool NVARCHAR(50),
		GeneralJunta NVARCHAR(50),
		GeneralTipoJunta NVARCHAR(50),
		GeneralDiametro DECIMAL(7,4),
		GeneralCedula NVARCHAR(10),
		GeneralEspesor DECIMAL(10,4),
		GeneralLocalizacion NVARCHAR(50),
		GeneralUltimoProceso NVARCHAR(50),
		GeneralTieneHold BIT,
		GeneralFamiliaAceroMaterial1ID INT,
		GeneralFamiliaAceroMaterial2ID INT,
		GeneralFamAcero1 NVARCHAR(50),
		GeneralFamAcero2 NVARCHAR(50),
		GeneralPeqs DECIMAL(10,4),
		GeneralKgTeoricos DECIMAL(12,4),
		OrdenTrabajoSpoolID INT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150),
		GeneralFabArea NVARCHAR(20)	
	)
	
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_JuntaWorkstatusID ON #TempJuntaWorkstatus(JuntaWorkStatusID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_OrdenTrabajoSpoolID ON #TempJuntaWorkstatus(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaSoldadura_SoldaduraJuntaWorkstatusID ON #TempJuntaSoldadura(SoldaduraJuntaWorkstatusID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaInspeccionVisual_InspeccionVisualJuntaWorkstatusID ON #TempJuntaInspeccionVisual(InspeccionVisualJuntaWorkstatusID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_OrdenTrabajoSpoolID ON #TempWorkstatusSpool(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_WorkstatusSpoolID ON #TempWorkstatusSpool(WorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempPintura_PinturaWorkstatusSpoolID ON #TempPintura(PinturaWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempEmbarque_EmbarqueWorkstatusSpoolID ON #TempEmbarque(EmbarqueWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempGeneral_GeneralJuntaWorkstatusID ON #TempGeneral(GeneralJuntaWorkstatusID)

	--Comienzan los inserts a las tablas temporales con filtros si los tienen.
	DECLARE @OrdenTrabajoSpoolID INT
	
	SELECT @OrdenTrabajoSpoolID = OrdenTrabajoSpoolID
	FROM JuntaWorkstatus jw
	WHERE jw.JuntaWorkstatusID = @JuntaWorkstatusID
	
	--INSERT PROYECTO
	INSERT INTO #TempProyecto
		SELECT ProyectoID,
			   PatioID,
			   Nombre
		FROM Proyecto
		WHERE ProyectoID IN
		(
			SELECT	odt.ProyectoID
			FROM	OrdenTrabajo odt
			WHERE EXISTS
			(
				SELECT 1
				FROM	OrdenTrabajoSpool odts
				WHERE	odts.OrdenTrabajoID = odt.OrdenTrabajoID
						AND odts.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID
			)
		)

	-- INSERT A ODT	
	INSERT INTO #TempOrdenTrabajo
		SELECT OrdenTrabajoID,
			   ot.NumeroOrden
		FROM OrdenTrabajo ot
		WHERE OrdenTrabajoID IN
		(
			SELECT odts.OrdenTrabajoID
			FROM	OrdenTrabajoSpool odts
			WHERE	odts.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID
		)

	
	--INSERT ORDEN TRABAJO SPOOL
	INSERT INTO #TempOrdenTrabajoSpool
		SELECT OrdenTrabajoSpoolID,
			   ots.OrdenTrabajoID,
			   ots.NumeroControl,
			   ots.SpoolID
		FROM OrdenTrabajoSpool ots
		WHERE ots.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID
	
	--INSERT SPOOL
	INSERT INTO #TempSpool
		SELECT SpoolID,
			   Nombre
		FROM Spool
		WHERE SpoolID IN
		(
			SELECT SpoolID FROM #TempOrdenTrabajoSpool
		)
	
	--INSERT Material Spool
	INSERT INTO #TempMaterialSpool
	(
		SpoolID, 
		MaterialSpoolID, 
		ItemCodeID, 
		CodigoItemCode, 
		DescripcionItemCode, 
		Etiqueta, 
		EtiquetaEsNumero, 
		ValorNumericoEtiqueta
	)
	SELECT	ms.SpoolID,
			ms.MaterialSpoolID,
			ms.ItemCodeID,
			ic.Codigo,
			ic.DescripcionEspanol,
			ms.Etiqueta,
			CAST(ISNUMERIC(ms.Etiqueta) AS BIT),
			CASE WHEN ISNUMERIC(ms.Etiqueta) = 1 THEN CAST(ms.Etiqueta AS TINYINT) ELSE NULL END
	FROM MaterialSpool ms
	INNER JOIN ItemCode ic on ms.ItemCodeID = ic.ItemCodeID
	WHERE ms.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)
	
	--INSERT JUNTA SPOOL
	INSERT INTO #TempJuntaSpool
	(
		JuntaSpoolID,
		SpoolID,
		TipoJuntaID,
		Diametro,
		Cedula,
		Espesor,
		EtiquetaMaterial1,
		EtiquetaMaterial2,
		FamiliaAceroMaterial1ID,
		FamiliaAceroMaterial2ID,
		peqs,
		KgTeoricos,
		Etiqueta1EsNumero,
		Etiqueta2EsNumero,
		ValorNumericoEtiqueta1,
		ValorNumericoEtiqueta2,
		ItemCodeIDMaterial1,
		CodigoItemCodeMaterial1,
		DescripcionItemCodeMaterial1,
		ItemCodeIDMaterial2,
		CodigoItemCodeMaterial2,
		DescripcionItemCodeMaterial2,
		FabAreaID
	)
	SELECT JuntaSpoolID,
		  js.SpoolID,
		  js.TipoJuntaID,
		  js.Diametro,
		  js.Cedula,
		  js.Espesor,
		  js.EtiquetaMaterial1,
		  js.EtiquetaMaterial2,
		  js.FamiliaAceroMaterial1ID,
		  js.FamiliaAceroMaterial2ID,
		  js.Peqs,
		  js.KgTeoricos,
		  CAST(ISNUMERIC(js.EtiquetaMaterial1) AS BIT),
		  CAST(ISNUMERIC(js.EtiquetaMaterial2) AS BIT),
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS TINYINT) ELSE NULL END,
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS TINYINT) ELSE NULL END,
		  ms1.ItemCodeID,
		  ms1.CodigoItemCode,
		  ms1.DescripcionItemCode,
		  ms2.ItemCodeID,
		  ms2.CodigoItemCode,
		  ms2.DescripcionItemCode,
		  js.FabAreaID
	FROM JuntaSpool js
	LEFT JOIN #TempMaterialSpool ms1 on js.SpoolID = ms1.SpoolID and (ms1.Etiqueta = js.EtiquetaMaterial1 or ms1.ValorNumericoEtiqueta = (CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS TINYINT) ELSE NULL END))
	LEFT JOIN #TempMaterialSpool ms2 on js.SpoolID = ms2.SpoolID and (ms2.Etiqueta = js.EtiquetaMaterial2 or ms2.ValorNumericoEtiqueta = (CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS TINYINT) ELSE NULL END))
	WHERE js.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)
	
	--INSERT JUNTA WORKSTATUS
	INSERT INTO #TempJuntaWorkstatus
		SELECT jws.JuntaWorkstatusID,
			   jws.JuntaSpoolID,
			   jws.JuntaArmadoID,
			   jws.JuntaSoldaduraID,
			   jws.JuntaInspeccionVisualID,
			   jws.OrdenTrabajoSpoolID,
			   jws.UltimoProcesoID,
			   jws.EtiquetaJunta,
			   jws.JuntaFinal
		FROM JuntaWorkstatus jws
		WHERE jws.JuntaWorkstatusID = @JuntaWorkstatusID
	
	--INSERT JUNTA SOLDADURA
	INSERT INTO #TempJuntaSoldadura
		SELECT DISTINCT js.JuntaSoldaduraID,
						js.JuntaWorkstatusID,
						FechaSoldadura,
						FechaReporte,
						ta.Nombre [Teller],
						Wps.Nombre [Wps],
						pr.Nombre [ProcesoRelleno],
						cr.ConsumiblesRelleno,
						pra.Nombre [ProcesoRaiz],
						sra.SoldadorRaiz,
						sr.SoldadorRelleno,			   
						fm1.Nombre,
						fm2.Nombre	   
		FROM JuntaSoldadura js
		INNER JOIN #TempJuntaWorkstatus jws ON js.JuntaWorkstatusID = jws.JuntaWorkStatusID
		INNER JOIN Taller ta on ta.TallerID = js.TallerID
		INNER JOIN Wps wps on wps.WpsID =  js.WpsID
		INNER JOIN ProcesoRelleno pr on pr.ProcesoRellenoID = js.ProcesoRellenoID
		INNER JOIN ProcesoRaiz pra on pra.ProcesoRaizID = js.ProcesoRaizID
		INNER JOIN #TempJuntaSpool jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
		INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = jsp.FamiliaAceroMaterial1ID
		INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
		LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = jsp.FamiliaAceroMaterial2ID
		LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 2
			FOR XML PATH (''))),' ',', ') AS SoldadorRelleno
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sr on sr.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 1
			FOR XML PATH (''))),' ',', ') AS SoldadorRaiz
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sra on sra.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT co.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Consumible co on co.ConsumibleID = jsd1.ConsumibleID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) 
			FOR XML PATH (''))),' ',', ')  AS ConsumiblesRelleno
			FROM JuntaSoldaduraDetalle jsd
			INNER JOIN Consumible c on c.ConsumibleID = jsd.ConsumibleID
			GROUP BY jsd.JuntaSoldaduraID
		) cr on cr.JuntaSoldaduraID = js.JuntaSoldaduraID
				 
	--INSERT JUNTA INSPECCION VISUAL
	insert into #TempJuntaInspeccionVisualDefecto	
 	SELECT DISTINCT	jiv.JuntaInspeccionVisualID,
					jiv.JuntaWorkstatusID,
					jiv.FechaInspeccion [Fecha],
					iv.FechaReporte,
					iv.NumeroReporte,
					jiv.Hoja,
					jiv.Aprobado [Resultado],
					substring(d.Defecto,0,LEN(d.defecto)) as Defecto,
					jiv.Observaciones,
					jiv.FechaModificacion			   			   
	FROM JuntaInspeccionVisual jiv
	INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = jiv.JuntaWorkstatusID 
	INNER JOIN InspeccionVisual iv on iv.InspeccionVisualID = jiv.InspeccionVisualID
	LEFT JOIN(
		SELECT jivd.JuntaInspeccionVisualID, 
			   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
		FROM JuntaInspeccionVisualDefecto jivd1
		INNER JOIN Defecto d on d.DefectoID = jivd1.DefectoID
		WHERE (jivd1.JuntaInspeccionVisualID = jivd.JuntaInspeccionVisualID) 
		FOR XML PATH (''))) AS Defecto
		FROM JuntaInspeccionVisualDefecto jivd
		INNER JOIN Defecto d on d.DefectoID = jivd.DefectoID
		GROUP BY jivd.JuntaInspeccionVisualID
	) d on d.JuntaInspeccionVisualID = jiv.JuntaInspeccionVisualID
		

	INSERT INTO #TempJuntaInspeccionVisual
		SELECT JuntaInspeccionVisualID,
			   jiv.JuntaWorkstatusID,
			   Fecha,
			   FechaReporte,
			   NumeroReporte,
			   Hoja,
			   Resultado,
			   Defecto,
			   Observaciones			   
		FROM #TempJuntaInspeccionVisualDefecto jiv		
		INNER JOIN(
			SELECT MAX(FechaModificacion)as FechaModificacion,
				   MAX(Fecha) as FechaMaxima, 
				   JuntaWorkstatusID 
			FROM #TempJuntaInspeccionVisualDefecto
			GROUP BY JuntaWorkstatusID
		) e on  e.JuntaWorkstatusID = jiv.JuntaWorkstatusID 
			AND e.FechaMaxima = jiv.Fecha
			AND e.FechaModificacion = jiv.FechaModificacion
	
	--INSERT WORKSTATUS SPOOL
	INSERT INTO #TempWorkstatusSpool
		SELECT DISTINCT ws.WorkstatusSpoolID,
			   ws.OrdenTrabajoSpoolID,
			   ws.SistemaPintura,
			   ws.ColorPintura,
			   ws.CodigoPintura,
			   ws.NumeroEtiqueta,
			   ws.FechaEtiqueta,
			   ws.FechaPreparacion		  		   
		FROM WorkstatusSpool ws
		INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID	
	
	--INSERT REPORTE DIMENSIONAL
	INSERT INTO #TempReporteDimensional
		SELECT	rd.ReporteDimensionalID,
				FechaReporte,
				NumeroReporte,
				TipoReporteDimensionalID,
				FechaModificacion
		FROM ReporteDimensional rd
		WHERE EXISTS
		(
			SELECT 1 FROM ReporteDimensionalDetalle rdd
			WHERE EXISTS
			(
				SELECT 1 FROM #TempWorkstatusSpool tws
				WHERE tws.WorkstatusSpoolID = rdd.WorkstatusSpoolID
			)
			AND rdd.ReporteDimensionalID = rd.ReporteDimensionalID
		)
				
	--INSERT INSPECCION DIMENSIONAL
	INSERT INTO #TempInspeccionDimensional
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   rdd.FechaLiberacion,
			   rd.FechaModificacion
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID
		where rd.TipoReporteDimensionalID = 1
		order by rdd.FechaLiberacion desc
		
	--INSERT INSPECCION ESPESORES
	INSERT INTO #TempInspeccionEspesores
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   rdd.FechaLiberacion,
			   rdd.FechaModificacion
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID			
		where rd.TipoReporteDimensionalID = 2
		order by rdd.FechaLiberacion desc
		
	--INSERT JUNTA REPORTE PND
	INSERT INTO #TempJuntaReportePnd
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   rpnd.TipoPruebaID,
			   rpnd.FechaReporte,
			   rpnd.NumeroReporte,
			   jrpnd.JuntaRequisicionID,
			   jrpnd.FechaPrueba,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   jrpnd.Observaciones,
			   jrpnd.FechaModificacion
		FROM JuntaReportePnd jrpnd
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrpnd.JuntaWorkstatusID 
		INNER JOIN ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID
		
		
	--INSERT JUNTA REPORTE TT
	INSERT INTO #TempJuntaReporteTt
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   rtt.TipoPruebaID,
			   rtt.FechaReporte,
			   rtt.NumeroReporte,
			   jrtt.JuntaRequisicionID,
			   jrtt.Aprobado,
			   jrtt.NumeroGrafica,
			   jrtt.Hoja,
			   jrtt.FechaTratamiento,
			   jrtt.Observaciones,
			   jrtt.FechaModificacion
		FROM JuntaReporteTt jrtt
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrtt.JuntaWorkstatusID 
		INNER JOIN ReporteTt rtt on rtt.ReporteTtID = jrtt.ReporteTtID
		
		
	--INSERT PRUEBA RT
	INSERT INTO #TempPruebaRT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 1
		
	--INSERT PRUEBA PT
	INSERT INTO #TempPruebaPT 
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 2
		
	--INSERT PRUEBA RT(PostTT)
	INSERT INTO #TempPruebaRTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 5
		
	--INSERT PRUEBA PT(PostTT)
	INSERT INTO #TempPruebaPTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 6
		
	--INSERT PRUEBA UT
	INSERT INTO #TempPruebaUT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd 
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 8
		
	--INSERT TRATAMIENTO PWHT
	INSERT INTO #TempTratamientoPwht
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion			   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 3
		
	--INSERT TRATAMIENTO DUREZAS
	INSERT INTO #TempTratamientoDurezas
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion		   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 4
		
	--INSERT TRATAMIENTO PREHEAT
	INSERT INTO #TempTratamientoPreheat
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion		   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 7
					
	--INSERT PINTURA
	INSERT INTO #TempPintura
		SELECT DISTINCT ps.PinturaSpoolID,
			   ws.WorkstatusSpoolID,
			   rp.FechaRequisicion,
			   rp.NumeroRequisicion,
			   ws.PinturaSistema,
			   ws.PinturaColor,
			   ws.PinturaCodigo,
			   ps.FechaSandblast,
			   ps.ReporteSandblast,
			   ps.FechaPrimarios,
			   ps.ReportePrimarios,
			   ps.FechaIntermedios,
			   ps.ReporteIntermedios,
			   ps.FechaAcabadoVisual,
			   ps.ReporteAcabadoVisual,
			   ps.FechaAdherencia,
			   ps.ReporteAdherencia,
			   ps.FechaPullOff,
			   ps.ReportePullOff
		FROM PinturaSpool ps
		INNER JOIN #TempWorkstatusSpool ws on ps.WorkstatusSpoolID = ws.WorkstatusSpoolID
		INNER JOIN RequisicionPinturaDetalle rpd on rpd.RequisicionPinturaDetalleID = ps.RequisicionPinturaDetalleID
		INNER JOIN RequisicionPintura rp on rp.RequisicionPinturaID = rpd.RequisicionPinturaID
								
	--INSERT EMBARQUE
	INSERT INTO #TempEmbarque
			SELECT DISTINCT es.EmbarqueSpoolID,
				   ws.WorkstatusSpoolID,
				   ws.EmbarqueEtiqueta,
				   ws.FechaEtiqueta,
				   ws.FechaPreparacion,
				   e.FechaEmbarque,
				   e.NumeroEmbarque  		   
			FROM EmbarqueSpool es
			LEFT JOIN #TempWorkstatusSpool ws on es.WorkstatusSpoolID = ws.WorkstatusSpoolID
			LEFT JOIN Embarque e on e.EmbarqueID = es.EmbarqueID
			
	--INSERT GENERAL	
	INSERT INTO #TempGeneral
		SELECT jws.JuntaWorkStatusID,
			   p.Nombre,
			   ot.NumeroOrden,
			   ots.NumeroControl,
			   s.Nombre,
			   jws.EtiquetaJunta,
			   tj.Codigo,
			   js.Diametro,
			   js.Cedula,
			   js.Espesor,
			   js.EtiquetaMaterial1 + ' - ' + js.EtiquetaMaterial2,
			   up.Nombre,
			   ISNULL((select '1'
				where sh.TieneHoldCalidad = 1 or
					  sh.TieneHoldIngenieria = 1),0),
				js.FamiliaAceroMaterial1ID,
				js.FamiliaAceroMaterial2ID,
				fm1.Nombre,
				fm2.Nombre,
				js.peqs,
				js.KgTeoricos,
				ots.OrdenTrabajoSpoolID,
				js.ItemCodeIDMaterial1,
				js.CodigoItemCodeMaterial1,
				js.DescripcionItemCodeMaterial1,
				js.ItemCodeIDMaterial2,
				js.CodigoItemCodeMaterial2,
				js.DescripcionItemCodeMaterial2,
				fab.Codigo
			FROM #TempJuntaWorkstatus jws
			INNER JOIN #TempJuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
			INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
			INNER JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
			INNER JOIN #TempProyecto p on 1 = 1
			INNER JOIN #TempSpool s on s.SpoolID = js.SpoolID
			INNER JOIN FabArea fab on fab.FabAreaID = js.FabAreaID
			LEFT JOIN UltimoProceso up on up.UltimoProcesoID = jws.UltimoProcesoID
			INNER JOIN TipoJunta tj on tj.TipoJuntaID = js.TipoJuntaID
			LEFT JOIN SpoolHold sh on sh.SpoolID = s.SpoolID
			INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
			INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
			LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
			LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
			
			
	--DESPLEGAR TABLA
	
		select g.GeneralJuntaWorkstatusID,
			   g.GeneralProyecto,
			   g.GeneralOrdenDeTrabajo,
			   g.GeneralNumeroDeControl,
			   g.GeneralSpool,
			   g.GeneralJunta,
			   g.GeneralTipoJunta,
			   g.GeneralDiametro,
			   g.GeneralCedula,
			   g.GeneralEspesor,
			   g.GeneralLocalizacion,
			   g.GeneralUltimoProceso,
			   g.GeneralTieneHold,
			   g.GeneralFamAcero1,
			   g.GeneralFamAcero2,
			   g.CodigoItemCodeMaterial1,
			   g.DescripcionItemCodeMaterial1,
			   g.CodigoItemCodeMaterial2,
			   g.DescripcionItemCodeMaterial2,
			   g.GeneralPeqs,
			   g.GeneralKgTeoricos,
			   g.GeneralFabArea,
			   ja.FechaArmado [ArmadoFecha],
			   ja.FechaReporte [ArmadoFechaReporte],
			   ja.Taller [ArmadoTaller],
			   ja.Tubero [ArmadoTubero],
			   ja.NumeroUnico1 [ArmadoNumeroUnico1],
			   ja.NumeroUnico2 [ArmadoNumeroUnico2],
			   js.SoldaduraFecha,
			   js.SoldaduraFechaReporte,
			   js.SoldaduraTaller,
			   js.SoldaduraWPS,
			   js.SoldaduraProcesoRelleno,
			   js.SoldaduraConsumiblesRelleno,
			   js.SoldaduraProcesoRaiz,
			   js.SoldaduraSoldadorRaiz,
			   js.SoldaduraSoldadorRelleno,
			   js.SoldaduraMaterialBase1,
			   js.SoldaduraMaterialBase2,
			   iv.InspeccionVisualFecha,
			   iv.InspeccionVisualFechaReporte,
			   iv.InspeccionVisualNumeroReporte,   
			   iv.InspeccionVisualHoja,
			   iv.InspeccionVisualResultado,
			   iv.InspeccionVisualDefecto,
			   iv.InspeccionVisualObservaciones,
			   id.InspeccionDimensionalFecha,
			   id.InspeccionDimensionalFechaReporte,
			   id.InspeccionDimensionalNumeroReporte,
			   id.InspeccionDimensionalHoja,
			   id.InspeccionDimensionalResultado,		   
			   ie.InspeccionEspesoresFecha,
			   ie.InspeccionEspesoresFechaReporte,
			   ie.InspeccionEspesoresNumeroReporte,
			   ie.InspeccionEspesoresHoja,
			   ie.InspeccionEspesoresResultado,
			   ie.InspeccionEspesoresObservaciones,
			   prt.PruebaRTFechaRequisicion,
			   prt.PruebaRTNumeroRequisicion,
			   prt.PruebaRTCodigoRequisicion,
			   prt.PruebaRTFechaPrueba,
			   prt.PruebaRTFechaReporte,
			   prt.PruebaRTNumeroReporte,
			   prt.PruebaRTHoja,
			   prt.PruebaRTResultado,
			   prt.PruebaRTDefecto,
			   prt.PruebaRTObservacionesReporte,
			   prt.PruebaRTObservacionesRequisicion,
			   ppt.PruebaPTFechaRequisicion,
			   ppt.PruebaPTNumeroRequisicion,
			   ppt.PruebaPTCodigoRequisicion,
			   ppt.PruebaPTFechaPrueba,
			   ppt.PruebaPTFechaReporte,
			   ppt.PruebaPTNumeroReporte,
			   ppt.PruebaPTHoja,
			   ppt.PruebaPTResultado,
			   ppt.PruebaPTDefecto,
			   ppt.PruebaPTObservacionesReporte,
			   ppt.PruebaPTObservacionesRequisicion,
			   prtptt.PruebaRTPostTTFechaRequisicion,
			   prtptt.PruebaRTPostTTNumeroRequisicion,
			   prtptt.PruebaRTPostTTCodigoRequisicion,
			   prtptt.PruebaRTPostTTFechaPrueba,
			   prtptt.PruebaRTPostTTFechaReporte,
			   prtptt.PruebaRTPostTTNumeroReporte,
			   prtptt.PruebaRTPostTTHoja,
			   prtptt.PruebaRTPostTTResultado,
			   prtptt.PruebaRTPostTTDefecto,
			   prtptt.PruebaRTPostTTObservacionesReporte,
			   prtptt.PruebaRTPostTTObservacionesRequisicion,
			   pptptt.PruebaPTPostTTFechaRequisicion,
			   pptptt.PruebaPTPostTTNumeroRequisicion,
			   pptptt.PruebaPTPostTTCodigoRequisicion,
			   pptptt.PruebaPTPostTTFechaPrueba,
			   pptptt.PruebaPTPostTTFechaReporte,
			   pptptt.PruebaPTPostTTNumeroReporte,
			   pptptt.PruebaPTPostTTHoja,
			   pptptt.PruebaPTPostTTResultado,
			   pptptt.PruebaPTPostTTDefecto,
			   pptptt.PruebaPTPostTTObservacionesReporte,
			   pptptt.PruebaPTPostTTObservacionesRequisicion,
			   put.PruebaUTFechaRequisicion,
			   put.PruebaUTNumeroRequisicion,
			   put.PruebaUTCodigoRequisicion,
			   put.PruebaUTFechaPrueba,
			   put.PruebaUTFechaReporte,
			   put.PruebaUTNumeroReporte,
			   put.PruebaUTHoja,
			   put.PruebaUTResultado,
			   put.PruebaUTDefecto,
			   put.PruebaUTObservacionesReporte,
			   put.PruebaUTObservacionesRequisicion,
			   tpwht.TratamientoPwhtFechaRequisicion,
			   tpwht.TratamientoPwhtNumeroRequisicion,
			   tpwht.TratamientoPwhtCodigoRequisicion,
			   tpwht.TratamientoPwhtFechaTratamiento,
			   tpwht.TratamientoPwhtFechaReporte,
			   tpwht.TratamientoPwhtNumeroReporte,
			   tpwht.TratamientoPwhtHoja,
			   tpwht.TratamientoPwhtGrafica,
			   tpwht.TratamientoPwhtResultado,
			   tpwht.TratamientoPwhtObservacionesReporte,
			   tpwht.TratamientoPwhtObservacionesRequisicion,
			   td.TratamientoDurezasFechaRequisicion,
			   td.TratamientoDurezasNumeroRequisicion,
			   td.TratamientoDurezasCodigoRequisicion,
			   td.TratamientoDurezasFechaTratamiento,
			   td.TratamientoDurezasFechaReporte,
			   td.TratamientoDurezasNumeroReporte,
			   td.TratamientoDurezasHoja,
			   td.TratamientoDurezasGrafica,
			   td.TratamientoDurezasResultado,
			   td.TratamientoDurezasObservacionesReporte,
			   td.TratamientoDurezasObservacionesRequisicion,
			   tp.TratamientopreheatFechaRequisicion,
			   tp.TratamientopreheatNumeroRequisicion,
			   tp.TratamientopreheatCodigoRequisicion,
			   tp.TratamientopreheatFechaTratamiento,
			   tp.TratamientopreheatFechaReporte,
			   tp.TratamientopreheatNumeroReporte,
			   tp.TratamientopreheatHoja,
			   tp.TratamientopreheatGrafica,
			   tp.TratamientopreheatResultado,
			   tp.TratamientopreheatObservacionesReporte,
			   tp.TratamientopreheatObservacionesRequisicion,
			   p.PinturaFechaRequisicion,
			   p.PinturaNumeroRequisicion,
			   p.PinturaSistema,
			   p.PinturaColor,
			   p.PinturaCodigo,
			   p.PinturaFechaSendBlast,
			   p.PinturaReporteSendBlast,
			   p.PinturaFechaPrimarios,
			   p.PinturaReportePrimarios,
			   p.PinturaFechaIntermedios,
			   p.PinturaReporteIntermedios,
			   p.PinturaFechaAcabadoVisual,
			   p.PinturaReporteAcabadoVisual,
			   p.PinturaFechaAdherencia,
			   p.PinturaReporteAdherencia,
			   p.PinturaFechaPullOff,
			   p.PinturaReportePullOff,
			   e.EmbarqueEtiqueta,
			   e.EmbarqueFechaEtiqueta,
			   e.EmbarqueFechaPreparacion,
			   e.EmbarqueFechaEmbarque,
			   e.EmbarqueNumeroEmbarque
		from #TempJuntaWorkstatus jw
		INNER JOIN #TempGeneral g on g.GeneralJuntaWorkstatusID = jw.JuntaWorkStatusID
		INNER JOIN
		(
			SELECT	jarm.JuntaArmadoID,
					jarm.JuntaWorkstatusID,
					FechaArmado,
					FechaReporte,
					ta.Nombre [Taller],
					tu.Codigo [Tubero],
					NumeroUnico1ID,
					NumeroUnico2ID,
					nu1.Codigo [NumeroUnico1],
					nu2.Codigo [NumeroUnico2],
					jarm.TuberoID
			FROM JuntaArmado jarm
			INNER JOIN Taller ta on ta.TallerID = jarm.TallerID 
			INNER JOIN Tubero tu on tu.TuberoID = jarm.TuberoID
			INNER JOIN NumeroUnico nu1 on nu1.NumeroUnicoID = jarm.NumeroUnico1ID
			INNER JOIN NumeroUnico nu2 on nu2.NumeroUnicoID = jarm.NumeroUnico2ID
			WHERE jarm.JuntaWorkstatusID IN
			(
				SELECT JuntaWorkstatusID FROM #TempJuntaWorkstatus
			)
		) ja on ja.JuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempJuntaSoldadura js on js.SoldaduraJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempJuntaInspeccionVisual iv on iv.InspeccionVisualJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempWorkstatusSpool ws on ws.OrdenTrabajoSpoolID = jw.OrdenTrabajoSpoolID
		LEFT JOIN #TempPintura p on p.PinturaWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN #TempEmbarque e on e.EmbarqueWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN
		(			
			select	tmp.PruebaRTFechaRequisicion,
					tmp.PruebaRTNumeroRequisicion,
					tmp.PruebaRTCodigoRequisicion,
					tmp.PruebaRTFechaPrueba,
					tmp.PruebaRTFechaReporte,
					tmp.PruebaRTNumeroReporte,
					tmp.PruebaRTHoja,
					tmp.PruebaRTResultado,
					tmp.PruebaRTDefecto,
					tmp.PruebaRTObservacionesReporte,
					tmp.PruebaRTObservacionesRequisicion,
					tmp.PruebaRTJuntaWorkstatusID
			from #TempPruebaRT tmp
			inner join(
				select	PruebaRTJuntaWorkstatusID, 
						MAX(PruebaRTFechaPrueba) as PruebaRTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaRT
				group by PruebaRTJuntaWorkstatusID
			) A on	A.PruebaRTJuntaWorkstatusID = tmp.PruebaRTJuntaWorkstatusID
					and A.PruebaRTFechaPrueba = tmp.PruebaRTFechaPrueba
					AND A.FechaModificacion = tmp.FechaModificacion
		)  prt on prt.PruebaRTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.PruebaPTFechaRequisicion,
					tmp.PruebaPTNumeroRequisicion,
					tmp.PruebaPTCodigoRequisicion,
					tmp.PruebaPTFechaPrueba,
					tmp.PruebaPTFechaReporte,
					tmp.PruebaPTNumeroReporte,
					tmp.PruebaPTHoja,
					tmp.PruebaPTResultado,
					tmp.PruebaPTDefecto,
					tmp.PruebaPTObservacionesReporte,
					tmp.PruebaPTObservacionesRequisicion,
					tmp.PruebaPTJuntaWorkstatusID
			from #TempPruebaPT tmp
			inner join(
				select	PruebaPTJuntaWorkstatusID, 
						MAX(PruebaPTFechaPrueba) as PruebaPTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion						
				from #TempPruebaPT
				group by PruebaPTJuntaWorkstatusID
			) A on A.PruebaPTJuntaWorkstatusID = tmp.PruebaPTJuntaWorkstatusID
			and A.PruebaPTFechaPrueba = tmp.PruebaPTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
			
		)  ppt on ppt.PruebaPTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			select	tmp.PruebaRTPostTTFechaRequisicion,
					tmp.PruebaRTPostTTNumeroRequisicion,
					tmp.PruebaRTPostTTCodigoRequisicion,
					tmp.PruebaRTPostTTFechaPrueba,
					tmp.PruebaRTPostTTFechaReporte,
					tmp.PruebaRTPostTTNumeroReporte,
					tmp.PruebaRTPostTTHoja,
					tmp.PruebaRTPostTTResultado,
					tmp.PruebaRTPostTTDefecto,
					tmp.PruebaRTPostTTObservacionesReporte,
					tmp.PruebaRTPostTTObservacionesRequisicion, 
					tmp.PruebaRTPostTTJuntaWorkstatusID
			from #TempPruebaRTPostTT tmp
			inner join(
				select	PruebaRTPostTTJuntaWorkstatusID, 
						MAX(PruebaRTPostTTFechaPrueba) as PruebaRTPostTTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaRTPostTT
				group by PruebaRTPostTTJuntaWorkstatusID
			) A on A.PruebaRTPostTTJuntaWorkstatusID = tmp.PruebaRTPostTTJuntaWorkstatusID
			and A.PruebaRTPostTTFechaPrueba = tmp.PruebaRTPostTTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
		)  prtptt on prtptt.PruebaRTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(	
			select	tmp.PruebaPTPostTTFechaRequisicion,
					tmp.PruebaPTPostTTNumeroRequisicion,
					tmp.PruebaPTPostTTCodigoRequisicion,
					tmp.PruebaPTPostTTFechaPrueba,
					tmp.PruebaPTPostTTFechaReporte,
					tmp.PruebaPTPostTTNumeroReporte,
					tmp.PruebaPTPostTTHoja,
					tmp.PruebaPTPostTTResultado,
					tmp.PruebaPTPostTTDefecto,
					tmp.PruebaPTPostTTObservacionesReporte,
					tmp.PruebaPTPostTTObservacionesRequisicion,
					tmp.PruebaPTPostTTJuntaWorkstatusID
			from #TempPruebaPTPostTT tmp
			inner join(
				select	PruebaPTPostTTJuntaWorkstatusID, 
						MAX(PruebaPTPostTTFechaPrueba) as PruebaPTPostTTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaPTPostTT
				group by PruebaPTPostTTJuntaWorkstatusID
			) A on	A.PruebaPTPostTTJuntaWorkstatusID = tmp.PruebaPTPostTTJuntaWorkstatusID
					and A.PruebaPTPostTTFechaPrueba = tmp.PruebaPTPostTTFechaPrueba
					AND A.FechaModificacion = tmp.FechaModificacion
		)  pptptt on pptptt.PruebaPTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		( 
			select	tmp.PruebaUTFechaRequisicion,
					tmp.PruebaUTNumeroRequisicion,
					tmp.PruebaUTCodigoRequisicion,
					tmp.PruebaUTFechaPrueba,
					tmp.PruebaUTFechaReporte,
					tmp.PruebaUTNumeroReporte,
					tmp.PruebaUTHoja,
					tmp.PruebaUTResultado,
					tmp.PruebaUTDefecto,
					tmp.PruebaUTObservacionesReporte,
					tmp.PruebaUTObservacionesRequisicion,
					tmp.PruebaUTJuntaWorkstatusID
			from #TempPruebaUT tmp
			inner join(
				select	PruebaUTJuntaWorkstatusID, 
						MAX(PruebaUTFechaPrueba) as PruebaUTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaUT
				group by PruebaUTJuntaWorkstatusID
			) A on A.PruebaUTJuntaWorkstatusID = tmp.PruebaUTJuntaWorkstatusID
			and A.PruebaUTFechaPrueba = tmp.PruebaUTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
		)  put on put.PruebaUTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.TratamientoPwhtFechaRequisicion,
					tmp.TratamientoPwhtNumeroRequisicion,
					tmp.TratamientoPwhtCodigoRequisicion,
					tmp.TratamientoPwhtFechaTratamiento,
					tmp.TratamientoPwhtFechaReporte,
					tmp.TratamientoPwhtNumeroReporte,
					tmp.TratamientoPwhtHoja,
					tmp.TratamientoPwhtGrafica,
					tmp.TratamientoPwhtResultado,
					tmp.TratamientoPwhtObservacionesReporte,
					tmp.TratamientoPwhtObservacionesRequisicion,
					tmp.TratamientoPwhtJuntaWorkstatusID
			from #TempTratamientoPwht tmp
			inner join(
				select	TratamientoPwhtJuntaWorkstatusID, 
						MAX(TratamientoPwhtFechaTratamiento) as TratamientoPwhtFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoPwht
				group by TratamientoPwhtJuntaWorkstatusID
			) A on A.TratamientoPwhtJuntaWorkstatusID = tmp.TratamientoPwhtJuntaWorkstatusID
			and A.TratamientoPwhtFechaTratamiento = tmp.TratamientoPwhtFechaTratamiento
			AND A.FechaModificacion = tmp.FechaModificacion
		)  tpwht on tpwht.TratamientoPwhtJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.TratamientoDurezasFechaRequisicion,
					tmp.TratamientoDurezasNumeroRequisicion,
					tmp.TratamientoDurezasCodigoRequisicion,
					tmp.TratamientoDurezasFechaTratamiento,
					tmp.TratamientoDurezasFechaReporte,
					tmp.TratamientoDurezasNumeroReporte,
					tmp.TratamientoDurezasHoja,
					tmp.TratamientoDurezasGrafica,
					tmp.TratamientoDurezasResultado,
					tmp.TratamientoDurezasObservacionesReporte,
					tmp.TratamientoDurezasObservacionesRequisicion, 
					tmp.TratamientoDurezasJuntaWorkstatusID
			from #TempTratamientoDurezas tmp
			inner join(
				select	TratamientoDurezasJuntaWorkstatusID,
						MAX(TratamientoDurezasFechaTratamiento) as TratamientoDurezasFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoDurezas
				group by TratamientoDurezasJuntaWorkstatusID
			) A on A.TratamientoDurezasJuntaWorkstatusID = tmp.TratamientoDurezasJuntaWorkstatusID
			and A.TratamientoDurezasFechaTratamiento = tmp.TratamientoDurezasFechaTratamiento			
			AND A.FechaModificacion = tmp.FechaModificacion			
		) td on td.TratamientoDurezasJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			select	tmp.TratamientopreheatFechaRequisicion,
					tmp.TratamientopreheatNumeroRequisicion,
					tmp.TratamientopreheatCodigoRequisicion,
					tmp.TratamientopreheatFechaTratamiento,
					tmp.TratamientopreheatFechaReporte,
					tmp.TratamientopreheatNumeroReporte,
					tmp.TratamientopreheatHoja,
					tmp.TratamientopreheatGrafica,
					tmp.TratamientopreheatResultado,
					tmp.TratamientopreheatObservacionesReporte,
					tmp.TratamientopreheatObservacionesRequisicion,
					tmp.TratamientoPreheatJuntaWorkstatusID
			from #TempTratamientoPreheat tmp
			inner join(
				select	TratamientoPreheatJuntaWorkstatusID,
						MAX(TratamientoPreheatFechaTratamiento) as TratamientoPreheatFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoPreheat
				group by TratamientoPreheatJuntaWorkstatusID
			) A on A.TratamientoPreheatJuntaWorkstatusID = tmp.TratamientoPreheatJuntaWorkstatusID
				AND A.TratamientoPreheatFechaTratamiento = tmp.TratamientoPreheatFechaTratamiento
				AND A.FechaModificacion = tmp.FechaModificacion			
		) tp on tp.TratamientopreheatJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			SELECT	tmp.InspeccionDimensionalFecha,
					tmp.InspeccionDimensionalFechaReporte,
					tmp.InspeccionDimensionalNumeroReporte,
					tmp.InspeccionDimensionalHoja,
					tmp.InspeccionDimensionalResultado,
					tmp.InspeccionDimensionalWorkstatusSpoolID
			FROM #TempInspeccionDimensional tmp
			INNER JOIN(
				SELECT	InspeccionDimensionalWorkstatusSpoolID, 
						MAX(InspeccionDimensionalFechaLiberacion) AS InspeccionDimensionalFechaLiberacion,
						MAX(FechaModificacion) AS FechaModificacion
				FROM #TempInspeccionDimensional
				GROUP BY InspeccionDimensionalWorkstatusSpoolID
			) A ON A.InspeccionDimensionalWorkstatusSpoolID = tmp.InspeccionDimensionalWorkstatusSpoolID
				AND A.InspeccionDimensionalFechaLiberacion = tmp.InspeccionDimensionalFechaLiberacion
				AND A.FechaModificacion = tmp.FechaModificacion			
		) id on id.InspeccionDimensionalWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN
		(
			SELECT	tmp.InspeccionEspesoresFecha,
					tmp.InspeccionEspesoresFechaReporte,
					tmp.InspeccionEspesoresNumeroReporte,
					tmp.InspeccionEspesoresHoja,
					tmp.InspeccionEspesoresResultado,
					tmp.InspeccionEspesoresObservaciones,
					tmp.InspeccionEspesoresWorkstatusSpoolID
			FROM #TempInspeccionEspesores tmp
			INNER JOIN(
					SELECT InspeccionEspesoresWorkstatusSpoolID, 
						   MAX(InspeccionEspesoresFechaLiberacion) AS InspeccionEspesoresFechaLiberacion,
						   MAX(FechaModificacion) AS FechaModificacion
					FROM #TempInspeccionEspesores
					GROUP BY InspeccionEspesoresWorkstatusSpoolID
				) A ON A.InspeccionEspesoresWorkstatusSpoolID = tmp.InspeccionEspesoresWorkstatusSpoolID
					AND A.InspeccionEspesoresFechaLiberacion = tmp.InspeccionEspesoresFechaLiberacion
		) ie on ie.InspeccionEspesoresWorkstatusSpoolID = ws.WorkstatusSpoolID
		
	DROP TABLE #TempProyecto
	DROP TABLE #TempOrdenTrabajo
	DROP TABLE #TempGeneral 
	DROP TABLE #TempEmbarque 
	DROP TABLE #TempPintura 
	DROP TABLE #TempTratamientoPreheat 
	DROP TABLE #TempTratamientoDurezas 
	DROP TABLE #TempTratamientoPwht 
	DROP TABLE #TempPruebaUT 
	DROP TABLE #TempPruebaPTPostTT 
	DROP TABLE #TempPruebaRTPostTT 
	DROP TABLE #TempPruebaRT 	
	DROP TABLE #TempJuntaInspeccionVisual 
	DROP TABLE #TempWorkstatusSpool 
	DROP TABLE #TempInspeccionEspesores 
	DROP TABLE #TempInspeccionDimensional 
	DROP TABLE #TempReporteDimensional 
	DROP TABLE #TempJuntaReportePnd 	
	DROP TABLE #TempJuntaReporteTt 
	DROP TABLE #TempJuntaSoldadura 	
	DROP TABLE #TempJuntaWorkstatus 
	DROP TABLE #TempJuntaSpool 
	DROP TABLE #TempSpool 
	DROP TABLE #TempOrdenTrabajoSpool
	DROP TABLE #TempPruebaPT
	DROP TABLE #TempMaterialSpool
	DROP TABLE #TempJuntaInspeccionVisualDefecto

		
	SET NOCOUNT OFF;

END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSeguimientoDeJuntasPorOrdenTrabajo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorOrdenTrabajo]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerSeguimientoDeJuntasPorOrdenTrabajo
	Funcion:	Trae toda la informacion necesaria para el seguimiento de jutnas
	Parametros:	@OrdenTrabajoID INT
				@HistorialRep BIT
	Autor:		MMG
	Modificado:	28/01/2011 IHM
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorOrdenTrabajo]
(
	@OrdenTrabajoID INT,
	@HistorialRep BIT
)
AS
BEGIN
	
	SET NOCOUNT ON; 

	--TABLA PROYECTO
	CREATE TABLE #TempProyecto
	(	
		ProyectoID INT,
		Patio INT,
		Nombre NVARCHAR(100)
	)
	
	--TABLA ORDEN DE TRABAJO
	CREATE TABLE #TempOrdenTrabajo
	(	
		OrdenTrabajoID INT,
		NumeroOrden NVARCHAR(50)
	)
	
	--TABLA ORDEN TRABAJO SPOOL
	CREATE TABLE #TempOrdenTrabajoSpool
	(	
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoID INT,
		NumeroControl NVARCHAR(50),
		SpoolID INT
	)
	
	--TABLA SPOOL
	CREATE TABLE #TempSpool 
	(	
		SpoolID INT,
		Nombre NVARCHAR(50)
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempJuntaSpool 
	(	
		JuntaSpoolID INT,
		SpoolID INT,
		TipoJuntaID INT,
		Diametro DECIMAL(7,4),
		Cedula NVARCHAR(10),
		Espesor DECIMAL(10,4),
		EtiquetaMaterial1 NVARCHAR(10),
		EtiquetaMaterial2 NVARCHAR(10),
		FamiliaAceroMaterial1ID INT,
		FamiliaAceroMaterial2ID INT,
		peqs DECIMAL(10,4),
		KgTeoricos DECIMAL(12,4),
		Etiqueta1EsNumero BIT,
		Etiqueta2EsNumero BIT,
		ValorNumericoEtiqueta1 TINYINT,
		ValorNumericoEtiqueta2 TINYINT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150)	
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempMaterialSpool
	(	
		MaterialSpoolID INT,
		SpoolID INT,
		ItemCodeID INT,
		Etiqueta NVARCHAR(10),
		CodigoItemCode NVARCHAR(50),
		DescripcionItemCode NVARCHAR(150),
		EtiquetaEsNumero BIT,
		ValorNumericoEtiqueta TINYINT
	)
	
	--TABLA JUNTA WORKSTATUS
	CREATE TABLE #TempJuntaWorkstatus 
	(	
		JuntaWorkStatusID INT,
		JuntaSpoolID INT,
		JuntaArmadoID INT,
		JuntaSoldaduraID INT,
		JuntaInspeccionVisualID INT,
		OrdenTrabajoSpoolID INT,
		UltimoProcesoID INT,
		EtiquetaJunta NVARCHAR(50),
		JuntaFinal BIT
	)
	
	--TABLA JUNTA SOLDADURA
	CREATE TABLE #TempJuntaSoldadura 
	(	
		SoldaduraJuntaSoldaduraID INT,
		SoldaduraJuntaWorkstatusID INT,
		SoldaduraFecha DATETIME,
		SoldaduraFechaReporte DATETIME,
		SoldaduraTaller NVARCHAR(50),
		SoldaduraWPS NVARCHAR(50),
		SoldaduraProcesoRelleno NVARCHAR(50),
		SoldaduraConsumiblesRelleno NVARCHAR(50),
		SoldaduraProcesoRaiz NVARCHAR(50),
		SoldaduraSoldadorRaiz NVARCHAR(50),
		SoldaduraSoldadorRelleno NVARCHAR(50),
		SoldaduraMaterialBase1 NVARCHAR(50),
		SoldaduraMaterialBase2 NVARCHAR(50)
	)
	
	--TABLA JUNTA REPORTE TT
	CREATE TABLE #TempJuntaReporteTt 
	(	
		JuntaReporteTtID INT,
		JuntaWorkstatusID INT,
		TipoPruebaID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		JuntaRequisicionID INT,
		Aprobado BIT,
		NumeroGrafica  NVARCHAR(20),
		Hoja INT,
		FechaTratamiento DATETIME,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA JUNTA REPORTE PND
	CREATE TABLE #TempJuntaReportePnd 
	(	
		JuntaReportePndID INT,
		JuntaWorkstatusID INT,
		TipoPruebaID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		JuntaRequisicionID INT,		
		FechaPrueba DATETIME,
		Hoja INT,
		Aprobado BIT,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME
	)
			
	--TABLA REPORTE DIMENSIONAL
	CREATE TABLE #TempReporteDimensional 
	(	
		ReporteDimensionalID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		TipoReporteDimensionalID INT,
		FechaModificacion DATETIME
	)
	
	--TABLA INSPECCION DIMENSIONAL
	CREATE TABLE #TempInspeccionDimensional 
	(	
		InspeccionDimensionalReporteDimensionalDetalleID INT,
		InspeccionDimensionalWorkstatusSpoolID INT,
		InspeccionDimensionalFecha DATETIME,
		InspeccionDimensionalFechaReporte DATETIME,
		InspeccionDimensionalNumeroReporte NVARCHAR(50),
		InspeccionDimensionalHoja INT,
		InspeccionDimensionalResultado BIT,
		InspeccionDimensionalObservaciones NVARCHAR(500),
		InspeccionDimensionalFechaLiberacion DATETIME,		
		FechaModificacion DATETIME
	)
	
	--TABLA INSPECCION ESPESORES
	CREATE TABLE #TempInspeccionEspesores 
	(	
		InspeccionEspesoresReporteDimensionalDetalleID INT,
		InspeccionEspesoresWorkstatusSpoolID INT,
		InspeccionEspesoresFecha DATETIME,
		InspeccionEspesoresFechaReporte DATETIME,
		InspeccionEspesoresNumeroReporte NVARCHAR(50),
		InspeccionEspesoresHoja INT,
		InspeccionEspesoresResultado BIT,
		InspeccionEspesoresObservaciones NVARCHAR(500),
		InspeccionEspesoresFechaLiberacion DATETIME,
		FechaModificacion DATETIME
	)	
	
	--TABLA WORKSTATUS SPOOL
	CREATE TABLE #TempWorkstatusSpool 
	(	
		WorkstatusSpoolID INT,
		OrdenTrabajoSpoolID INT,
		PinturaSistema NVARCHAR(50),
		PinturaColor NVARCHAR(50),
		PinturaCodigo NVARCHAR(50),
		EmbarqueEtiqueta NVARCHAR(20),
		FechaEtiqueta DATETIME,
		FechaPreparacion DATETIME		
	)
		
	--TABLA JUNTA INSPECCION VISUAL
	CREATE TABLE #TempJuntaInspeccionVisual 
	(	
		InspeccionVisualJuntaInspeccionVisualID INT,
		InspeccionVisualJuntaWorkstatusID INT,
		InspeccionVisualFecha DATETIME,
		InspeccionVisualFechaReporte DATETIME,
		InspeccionVisualNumeroReporte NVARCHAR(50),
		InspeccionVisualHoja INT,
		InspeccionVisualResultado BIT,
		InspeccionVisualDefecto NVARCHAR(MAX),
		InspeccionVisualObservaciones NVARCHAR(500)		
	)
	
	CREATE TABLE #TempJuntaInspeccionVisualDefecto
	(
		JuntaInspeccionVisualID int,
		JuntaWorkstatusID int,
		Fecha datetime,
		FechaReporte datetime,
		NumeroReporte nvarchar(50),
		Hoja int,
		Resultado bit,
		Defecto nvarchar(100),
		Observaciones nvarchar(500),
		FechaModificacion datetime
	)
	
	--TABLA PRUEBA RT
	CREATE TABLE #TempPruebaRT 
	(	
		PruebaRTJuntaReportePndID INT,
		PruebaRTJuntaWorkstatusID INT,
		PruebaRTFechaRequisicion DATETIME,
		PruebaRTNumeroRequisicion NVARCHAR(50),
		PruebaRTCodigoRequisicion NVARCHAR(50),
		PruebaRTFechaPrueba DATETIME,
		PruebaRTFechaReporte DATETIME,
		PruebaRTNumeroReporte NVARCHAR(50),
		PruebaRTHoja INT,
		PruebaRTResultado BIT,
		PruebaRTDefecto NVARCHAR(MAX),
		PruebaRTObservacionesReporte NVARCHAR(500),
		PruebaRTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA PT
	CREATE TABLE #TempPruebaPT 
	(	
		PruebaPTJuntaReportePndID INT,
		PruebaPTJuntaWorkstatusID INT,
		PruebaPTFechaRequisicion DATETIME,
		PruebaPTNumeroRequisicion NVARCHAR(50),
		PruebaPTCodigoRequisicion NVARCHAR(50),
		PruebaPTFechaPrueba DATETIME,
		PruebaPTFechaReporte DATETIME,
		PruebaPTNumeroReporte NVARCHAR(50),
		PruebaPTHoja INT,
		PruebaPTResultado BIT,
		PruebaPTDefecto NVARCHAR(MAX),
		PruebaPTObservacionesReporte NVARCHAR(500),
		PruebaPTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA RT(PostTT)
	CREATE TABLE #TempPruebaRTPostTT 
	(	
		PruebaRTPostTTJuntaReportePndID INT,
		PruebaRTPostTTJuntaWorkstatusID INT,
		PruebaRTPostTTFechaRequisicion DATETIME,
		PruebaRTPostTTNumeroRequisicion NVARCHAR(50),
		PruebaRTPostTTCodigoRequisicion NVARCHAR(50),
		PruebaRTPostTTFechaPrueba DATETIME,
		PruebaRTPostTTFechaReporte DATETIME,
		PruebaRTPostTTNumeroReporte NVARCHAR(50),
		PruebaRTPostTTHoja INT,
		PruebaRTPostTTResultado BIT,
		PruebaRTPostTTDefecto NVARCHAR(MAX),
		PruebaRTPostTTObservacionesReporte NVARCHAR(500),
		PruebaRTPostTTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA PT(PostTT)
	CREATE TABLE #TempPruebaPTPostTT 
	(	
		PruebaPTPostTTJuntaReportePndID INT,
		PruebaPTPostTTJuntaWorkstatusID INT,
		PruebaPTPostTTFechaRequisicion DATETIME,
		PruebaPTPostTTNumeroRequisicion NVARCHAR(50),
		PruebaPTPostTTCodigoRequisicion NVARCHAR(50),
		PruebaPTPostTTFechaPrueba DATETIME,
		PruebaPTPostTTFechaReporte DATETIME,
		PruebaPTPostTTNumeroReporte NVARCHAR(50),
		PruebaPTPostTTHoja INT,
		PruebaPTPostTTResultado BIT,
		PruebaPTPostTTDefecto NVARCHAR(MAX),
		PruebaPTPostTTObservacionesReporte NVARCHAR(500),
		PruebaPTPostTTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA UT
	CREATE TABLE #TempPruebaUT 
	(	
		PruebaUTJuntaReportePndID INT,
		PruebaUTJuntaWorkstatusID INT,
		PruebaUTFechaRequisicion DATETIME,
		PruebaUTNumeroRequisicion NVARCHAR(50),
		PruebaUTCodigoRequisicion NVARCHAR(50),
		PruebaUTFechaPrueba DATETIME,
		PruebaUTFechaReporte DATETIME,
		PruebaUTNumeroReporte NVARCHAR(50),
		PruebaUTHoja INT,
		PruebaUTResultado BIT,
		PruebaUTDefecto NVARCHAR(MAX),
		PruebaUTObservacionesReporte NVARCHAR(500),
		PruebaUTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA TRATAMIENTO PWHT
	CREATE TABLE #TempTratamientoPwht 
	(	
		TratamientoPwhtJuntaReporteTtID INT,
		TratamientoPwhtJuntaWorkstatusID INT,
		TratamientoPwhtFechaRequisicion DATETIME,
		TratamientoPwhtNumeroRequisicion NVARCHAR(50),
		TratamientoPwhtCodigoRequisicion NVARCHAR(50),
		TratamientoPwhtFechaTratamiento DATETIME,
		TratamientoPwhtFechaReporte DATETIME,
		TratamientoPwhtNumeroReporte NVARCHAR(50),
		TratamientoPwhtHoja INT,
		TratamientoPwhtGrafica NVARCHAR(20),
		TratamientoPwhtResultado BIT,
		TratamientoPwhtObservacionesReporte NVARCHAR(500),
		TratamientoPwhtObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME		
	)
	
	--TABLA TRATAMIENTO DUREZAS
	CREATE TABLE #TempTratamientoDurezas 
	(	
		TratamientoDurezasJuntaReporteTtID INT,
		TratamientoDurezasJuntaWorkstatusID INT,
		TratamientoDurezasFechaRequisicion DATETIME,
		TratamientoDurezasNumeroRequisicion NVARCHAR(50),
		TratamientoDurezasCodigoRequisicion NVARCHAR(50),
		TratamientoDurezasFechaTratamiento DATETIME,
		TratamientoDurezasFechaReporte DATETIME,
		TratamientoDurezasNumeroReporte NVARCHAR(20),
		TratamientoDurezasHoja INT,
		TratamientoDurezasGrafica NVARCHAR(20),
		TratamientoDurezasResultado BIT,
		TratamientoDurezasObservacionesReporte NVARCHAR(500),
		TratamientoDurezasObservacionesRequisicion NVARCHAR(500),		
		FechaModificacion DATETIME
	)
	
	--TABLA TRATAMIENTO PREHEAT
	CREATE TABLE #TempTratamientoPreheat 
	(	
		TratamientoPreheatJuntaReporteTtID INT,
		TratamientoPreheatJuntaWorkstatusID INT,
		TratamientoPreheatFechaRequisicion DATETIME,
		TratamientoPreheatNumeroRequisicion NVARCHAR(50),
		TratamientoPreheatCodigoRequisicion NVARCHAR(50),
		TratamientoPreheatFechaTratamiento DATETIME,
		TratamientoPreheatFechaReporte DATETIME,
		TratamientoPreheatNumeroReporte NVARCHAR(50),
		TratamientoPreheatHoja INT,
		TratamientoPreheatGrafica NVARCHAR(20),
		TratamientoPreheatResultado BIT,
		TratamientoPreheatObservacionesReporte NVARCHAR(500),
		TratamientoPreheatObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PINTURA
	CREATE TABLE #TempPintura 
	(	
		PinturaPinturaSpoolID INT,
		PinturaWorkstatusSpoolID INT,
		PinturaFechaRequisicion DATETIME,
		PinturaNumeroRequisicion NVARCHAR(50),
		PinturaSistema NVARCHAR(50),
		PinturaColor NVARCHAR(50),
		PinturaCodigo NVARCHAR(50),
		PinturaFechaSendBlast DATETIME,
		PinturaReporteSendBlast NVARCHAR(50),
		PinturaFechaPrimarios DATETIME,
		PinturaReportePrimarios NVARCHAR(50),
		PinturaFechaIntermedios DATETIME,
		PinturaReporteIntermedios NVARCHAR(50),
		PinturaFechaAcabadoVisual DATETIME,
		PinturaReporteAcabadoVisual NVARCHAR(50),
		PinturaFechaAdherencia DATETIME,
		PinturaReporteAdherencia NVARCHAR(50),
		PinturaFechaPullOff DATETIME,
		PinturaReportePullOff NVARCHAR(50)
	)
	
	--TABLA EMBARQUE
	CREATE TABLE #TempEmbarque 
	(
		EmbarqueEmbarqueSpoolID INT,
		EmbarqueWorkstatusSpoolID INT,
		EmbarqueEtiqueta NVARCHAR(20),
		EmbarqueFechaEtiqueta DATETIME,
		EmbarqueFechaPreparacion DATETIME,
		EmbarqueFechaEmbarque DATETIME,
		EmbarqueNumeroEmbarque NVARCHAR(50)
	)
	
	--TABLA GENERAL
	CREATE TABLE #TempGeneral 
	(
		GeneralJuntaWorkstatusID INT,
		GeneralProyecto NVARCHAR(50),
		GeneralOrdenDeTrabajo NVARCHAR(50),
		GeneralNumeroDeControl NVARCHAR(50),
		GeneralSpool NVARCHAR(50),
		GeneralJunta NVARCHAR(50),
		GeneralTipoJunta NVARCHAR(50),
		GeneralDiametro DECIMAL(7,4),
		GeneralCedula NVARCHAR(10),
		GeneralEspesor DECIMAL(10,4),
		GeneralLocalizacion NVARCHAR(50),
		GeneralUltimoProceso NVARCHAR(50),
		GeneralTieneHold BIT,
		GeneralFamiliaAceroMaterial1ID INT,
		GeneralFamiliaAceroMaterial2ID INT,
		GeneralFamAcero1 NVARCHAR(50),
		GeneralFamAcero2 NVARCHAR(50),
		GeneralPeqs DECIMAL(10,4),
		GeneralKgTeoricos DECIMAL(12,4),
		OrdenTrabajoSpoolID INT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150)	
	)
	
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_JuntaWorkstatusID ON #TempJuntaWorkstatus(JuntaWorkStatusID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_OrdenTrabajoSpoolID ON #TempJuntaWorkstatus(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaSoldadura_SoldaduraJuntaWorkstatusID ON #TempJuntaSoldadura(SoldaduraJuntaWorkstatusID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaInspeccionVisual_InspeccionVisualJuntaWorkstatusID ON #TempJuntaInspeccionVisual(InspeccionVisualJuntaWorkstatusID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_OrdenTrabajoSpoolID ON #TempWorkstatusSpool(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_WorkstatusSpoolID ON #TempWorkstatusSpool(WorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempPintura_PinturaWorkstatusSpoolID ON #TempPintura(PinturaWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempEmbarque_EmbarqueWorkstatusSpoolID ON #TempEmbarque(EmbarqueWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempGeneral_GeneralJuntaWorkstatusID ON #TempGeneral(GeneralJuntaWorkstatusID)

	--Comienzan los inserts a las tablas temporales con filtros si los tienen.
	
	--INSERT PROYECTO
	INSERT INTO #TempProyecto
		SELECT ProyectoID,
			   PatioID,
			   Nombre
		FROM Proyecto
		WHERE ProyectoID IN
		(
			SELECT ProyectoID FROM OrdenTrabajo WHERE OrdenTrabajoID = @OrdenTrabajoID
		)

	-- INSERT A ODT	
	INSERT INTO #TempOrdenTrabajo
		SELECT OrdenTrabajoID,
			   ot.NumeroOrden
		FROM OrdenTrabajo ot
		WHERE OrdenTrabajoID = @OrdenTrabajoID

	
	--INSERT ORDEN TRABAJO SPOOL
	INSERT INTO #TempOrdenTrabajoSpool
		SELECT OrdenTrabajoSpoolID,
			   ots.OrdenTrabajoID,
			   ots.NumeroControl,
			   ots.SpoolID
		FROM OrdenTrabajoSpool ots
		WHERE ots.OrdenTrabajoID IN
		(
			SELECT OrdenTrabajoID FROM #TempOrdenTrabajo
		)
	
	--INSERT SPOOL
	INSERT INTO #TempSpool
		SELECT SpoolID,
			   Nombre
		FROM Spool
		WHERE SpoolID IN
		(
			SELECT SpoolID FROM #TempOrdenTrabajoSpool
		)
	
	--INSERT Material Spool
	INSERT INTO #TempMaterialSpool
	(
		SpoolID, 
		MaterialSpoolID, 
		ItemCodeID, 
		CodigoItemCode, 
		DescripcionItemCode, 
		Etiqueta, 
		EtiquetaEsNumero, 
		ValorNumericoEtiqueta
	)
	SELECT	ms.SpoolID,
			ms.MaterialSpoolID,
			ms.ItemCodeID,
			ic.Codigo,
			ic.DescripcionEspanol,
			ms.Etiqueta,
			CAST(ISNUMERIC(ms.Etiqueta) AS BIT),
			CASE WHEN ISNUMERIC(ms.Etiqueta) = 1 THEN CAST(ms.Etiqueta AS TINYINT) ELSE NULL END
	FROM MaterialSpool ms
	INNER JOIN ItemCode ic on ms.ItemCodeID = ic.ItemCodeID
	WHERE ms.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)
	
	--INSERT JUNTA SPOOL
	INSERT INTO #TempJuntaSpool
	(
		JuntaSpoolID,
		SpoolID,
		TipoJuntaID,
		Diametro,
		Cedula,
		Espesor,
		EtiquetaMaterial1,
		EtiquetaMaterial2,
		FamiliaAceroMaterial1ID,
		FamiliaAceroMaterial2ID,
		peqs,
		KgTeoricos,
		Etiqueta1EsNumero,
		Etiqueta2EsNumero,
		ValorNumericoEtiqueta1,
		ValorNumericoEtiqueta2,
		ItemCodeIDMaterial1,
		CodigoItemCodeMaterial1,
		DescripcionItemCodeMaterial1,
		ItemCodeIDMaterial2,
		CodigoItemCodeMaterial2,
		DescripcionItemCodeMaterial2
	)
	SELECT JuntaSpoolID,
		  js.SpoolID,
		  js.TipoJuntaID,
		  js.Diametro,
		  js.Cedula,
		  js.Espesor,
		  js.EtiquetaMaterial1,
		  js.EtiquetaMaterial2,
		  js.FamiliaAceroMaterial1ID,
		  js.FamiliaAceroMaterial2ID,
		  js.Peqs,
		  js.KgTeoricos,
		  CAST(ISNUMERIC(js.EtiquetaMaterial1) AS BIT),
		  CAST(ISNUMERIC(js.EtiquetaMaterial2) AS BIT),
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS TINYINT) ELSE NULL END,
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS TINYINT) ELSE NULL END,
		  ms1.ItemCodeID,
		  ms1.CodigoItemCode,
		  ms1.DescripcionItemCode,
		  ms2.ItemCodeID,
		  ms2.CodigoItemCode,
		  ms2.DescripcionItemCode
	FROM JuntaSpool js
	LEFT JOIN #TempMaterialSpool ms1 on js.SpoolID = ms1.SpoolID and (ms1.Etiqueta = js.EtiquetaMaterial1 or ms1.ValorNumericoEtiqueta = (CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS TINYINT) ELSE NULL END))
	LEFT JOIN #TempMaterialSpool ms2 on js.SpoolID = ms2.SpoolID and (ms2.Etiqueta = js.EtiquetaMaterial2 or ms2.ValorNumericoEtiqueta = (CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS TINYINT) ELSE NULL END))
	WHERE js.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)
	
	--INSERT JUNTA WORKSTATUS
	INSERT INTO #TempJuntaWorkstatus
		SELECT jws.JuntaWorkstatusID,
			   jws.JuntaSpoolID,
			   jws.JuntaArmadoID,
			   jws.JuntaSoldaduraID,
			   jws.JuntaInspeccionVisualID,
			   jws.OrdenTrabajoSpoolID,
			   jws.UltimoProcesoID,
			   jws.EtiquetaJunta,
			   jws.JuntaFinal
		FROM JuntaWorkstatus jws
		WHERE jws.JuntaSpoolID IN
		(
			SELECT JuntaSpoolID FROM #TempJuntaSpool
		)
	
	-- Borrar las que no sean finales a menos que se quiera el historial
	DELETE FROM #TempJuntaWorkstatus WHERE JuntaFinal = 0 AND @HistorialRep = 0
				
	--INSERT JUNTA SOLDADURA
	INSERT INTO #TempJuntaSoldadura
		SELECT DISTINCT js.JuntaSoldaduraID,
						js.JuntaWorkstatusID,
						FechaSoldadura,
						FechaReporte,
						ta.Nombre [Teller],
						Wps.Nombre [Wps],
						pr.Nombre [ProcesoRelleno],
						cr.ConsumiblesRelleno,
						pra.Nombre [ProcesoRaiz],
						sra.SoldadorRaiz,
						sr.SoldadorRelleno,			   
						fm1.Nombre,
						fm2.Nombre	   
		FROM JuntaSoldadura js
		INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = js.JuntaWorkstatusID
		INNER JOIN Taller ta on ta.TallerID = js.TallerID
		INNER JOIN Wps wps on wps.WpsID =  js.WpsID
		INNER JOIN ProcesoRelleno pr on pr.ProcesoRellenoID = js.ProcesoRellenoID
		INNER JOIN ProcesoRaiz pra on pra.ProcesoRaizID = js.ProcesoRaizID
		INNER JOIN #TempJuntaSpool jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
		INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = jsp.FamiliaAceroMaterial1ID
		INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
		LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = jsp.FamiliaAceroMaterial2ID
		LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 2
			FOR XML PATH (''))),' ',', ') AS SoldadorRelleno
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sr on sr.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 1
			FOR XML PATH (''))),' ',', ') AS SoldadorRaiz
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sra on sra.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT co.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Consumible co on co.ConsumibleID = jsd1.ConsumibleID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) 
			FOR XML PATH (''))),' ',', ')  AS ConsumiblesRelleno
			FROM JuntaSoldaduraDetalle jsd
			INNER JOIN Consumible c on c.ConsumibleID = jsd.ConsumibleID
			GROUP BY jsd.JuntaSoldaduraID
		) cr on cr.JuntaSoldaduraID = js.JuntaSoldaduraID
				 
	--INSERT JUNTA INSPECCION VISUAL
	insert into #TempJuntaInspeccionVisualDefecto	
 	SELECT DISTINCT	jiv.JuntaInspeccionVisualID,
					jiv.JuntaWorkstatusID,
					jiv.FechaInspeccion [Fecha],
					iv.FechaReporte,
					iv.NumeroReporte,
					jiv.Hoja,
					jiv.Aprobado [Resultado],
					substring(d.Defecto,0,LEN(d.defecto)) as Defecto,
					jiv.Observaciones,
					jiv.FechaModificacion			   			   
	FROM JuntaInspeccionVisual jiv
	INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = jiv.JuntaWorkstatusID 
	INNER JOIN InspeccionVisual iv on iv.InspeccionVisualID = jiv.InspeccionVisualID
	LEFT JOIN(
		SELECT jivd.JuntaInspeccionVisualID, 
			   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
		FROM JuntaInspeccionVisualDefecto jivd1
		INNER JOIN Defecto d on d.DefectoID = jivd1.DefectoID
		WHERE (jivd1.JuntaInspeccionVisualID = jivd.JuntaInspeccionVisualID) 
		FOR XML PATH (''))) AS Defecto
		FROM JuntaInspeccionVisualDefecto jivd
		INNER JOIN Defecto d on d.DefectoID = jivd.DefectoID
		GROUP BY jivd.JuntaInspeccionVisualID
	) d on d.JuntaInspeccionVisualID = jiv.JuntaInspeccionVisualID
		

	INSERT INTO #TempJuntaInspeccionVisual
		SELECT JuntaInspeccionVisualID,
			   jiv.JuntaWorkstatusID,
			   Fecha,
			   FechaReporte,
			   NumeroReporte,
			   Hoja,
			   Resultado,
			   Defecto,
			   Observaciones			   
		FROM #TempJuntaInspeccionVisualDefecto jiv		
		INNER JOIN(
			SELECT MAX(FechaModificacion)as FechaModificacion,
				   MAX(Fecha) as FechaMaxima, 
				   JuntaWorkstatusID 
			FROM #TempJuntaInspeccionVisualDefecto
			GROUP BY JuntaWorkstatusID
		) e on  e.JuntaWorkstatusID = jiv.JuntaWorkstatusID 
			AND e.FechaMaxima = jiv.Fecha
			AND e.FechaModificacion = jiv.FechaModificacion
	
	--INSERT WORKSTATUS SPOOL
	INSERT INTO #TempWorkstatusSpool
		SELECT DISTINCT ws.WorkstatusSpoolID,
			   ws.OrdenTrabajoSpoolID,
			   ws.SistemaPintura,
			   ws.ColorPintura,
			   ws.CodigoPintura,
			   ws.NumeroEtiqueta,
			   ws.FechaEtiqueta,
			   ws.FechaPreparacion		  		   
		FROM WorkstatusSpool ws
		INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID	
	
	--INSERT REPORTE DIMENSIONAL
	INSERT INTO #TempReporteDimensional
		SELECT	rd.ReporteDimensionalID,
				FechaReporte,
				NumeroReporte,
				TipoReporteDimensionalID,
				FechaModificacion
		FROM ReporteDimensional rd
		WHERE EXISTS
		(
			SELECT 1 FROM ReporteDimensionalDetalle rdd
			WHERE EXISTS
			(
				SELECT 1 FROM #TempWorkstatusSpool tws
				WHERE tws.WorkstatusSpoolID = rdd.WorkstatusSpoolID
			)
			AND rdd.ReporteDimensionalID = rd.ReporteDimensionalID
		)
				
	--INSERT INSPECCION DIMENSIONAL
	INSERT INTO #TempInspeccionDimensional
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   rdd.FechaLiberacion,
			   rd.FechaModificacion
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID
		where rd.TipoReporteDimensionalID = 1
		order by rdd.FechaLiberacion desc
		
	--INSERT INSPECCION ESPESORES
	INSERT INTO #TempInspeccionEspesores
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   rdd.FechaLiberacion,
			   rdd.FechaModificacion
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID			
		where rd.TipoReporteDimensionalID = 2
		order by rdd.FechaLiberacion desc
		
	--INSERT JUNTA REPORTE PND
	INSERT INTO #TempJuntaReportePnd
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   rpnd.TipoPruebaID,
			   rpnd.FechaReporte,
			   rpnd.NumeroReporte,
			   jrpnd.JuntaRequisicionID,
			   jrpnd.FechaPrueba,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   jrpnd.Observaciones,
			   jrpnd.FechaModificacion
		FROM JuntaReportePnd jrpnd
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrpnd.JuntaWorkstatusID 
		INNER JOIN ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID
		
		
	--INSERT JUNTA REPORTE TT
	INSERT INTO #TempJuntaReporteTt
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   rtt.TipoPruebaID,
			   rtt.FechaReporte,
			   rtt.NumeroReporte,
			   jrtt.JuntaRequisicionID,
			   jrtt.Aprobado,
			   jrtt.NumeroGrafica,
			   jrtt.Hoja,
			   jrtt.FechaTratamiento,
			   jrtt.Observaciones,
			   jrtt.FechaModificacion
		FROM JuntaReporteTt jrtt
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrtt.JuntaWorkstatusID 
		INNER JOIN ReporteTt rtt on rtt.ReporteTtID = jrtt.ReporteTtID
		
		
	--INSERT PRUEBA RT
	INSERT INTO #TempPruebaRT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 1
		
	--INSERT PRUEBA PT
	INSERT INTO #TempPruebaPT 
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 2
		
	--INSERT PRUEBA RT(PostTT)
	INSERT INTO #TempPruebaRTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 5
		
	--INSERT PRUEBA PT(PostTT)
	INSERT INTO #TempPruebaPTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 6
		
	--INSERT PRUEBA UT
	INSERT INTO #TempPruebaUT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd 
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 8
		
	--INSERT TRATAMIENTO PWHT
	INSERT INTO #TempTratamientoPwht
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion			   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 3
		
	--INSERT TRATAMIENTO DUREZAS
	INSERT INTO #TempTratamientoDurezas
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion		   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 4
		
	--INSERT TRATAMIENTO PREHEAT
	INSERT INTO #TempTratamientoPreheat
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion		   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 7
					
	--INSERT PINTURA
	INSERT INTO #TempPintura
		SELECT DISTINCT ps.PinturaSpoolID,
			   ws.WorkstatusSpoolID,
			   rp.FechaRequisicion,
			   rp.NumeroRequisicion,
			   ws.PinturaSistema,
			   ws.PinturaColor,
			   ws.PinturaCodigo,
			   ps.FechaSandblast,
			   ps.ReporteSandblast,
			   ps.FechaPrimarios,
			   ps.ReportePrimarios,
			   ps.FechaIntermedios,
			   ps.ReporteIntermedios,
			   ps.FechaAcabadoVisual,
			   ps.ReporteAcabadoVisual,
			   ps.FechaAdherencia,
			   ps.ReporteAdherencia,
			   ps.FechaPullOff,
			   ps.ReportePullOff
		FROM PinturaSpool ps
		INNER JOIN #TempWorkstatusSpool ws on ps.WorkstatusSpoolID = ws.WorkstatusSpoolID
		INNER JOIN RequisicionPinturaDetalle rpd on rpd.RequisicionPinturaDetalleID = ps.RequisicionPinturaDetalleID
		INNER JOIN RequisicionPintura rp on rp.RequisicionPinturaID = rpd.RequisicionPinturaID
								
	--INSERT EMBARQUE
	INSERT INTO #TempEmbarque
			SELECT DISTINCT es.EmbarqueSpoolID,
				   ws.WorkstatusSpoolID,
				   ws.EmbarqueEtiqueta,
				   ws.FechaEtiqueta,
				   ws.FechaPreparacion,
				   e.FechaEmbarque,
				   e.NumeroEmbarque  		   
			FROM EmbarqueSpool es
			LEFT JOIN #TempWorkstatusSpool ws on es.WorkstatusSpoolID = ws.WorkstatusSpoolID
			LEFT JOIN Embarque e on e.EmbarqueID = es.EmbarqueID
			
	--INSERT GENERAL	
	INSERT INTO #TempGeneral
		SELECT jws.JuntaWorkStatusID,
			   p.Nombre,
			   ot.NumeroOrden,
			   ots.NumeroControl,
			   s.Nombre,
			   jws.EtiquetaJunta,
			   tj.Codigo,
			   js.Diametro,
			   js.Cedula,
			   js.Espesor,
			   js.EtiquetaMaterial1 + ' - ' + js.EtiquetaMaterial2,
			   up.Nombre,
			   ISNULL((select '1'
				where sh.TieneHoldCalidad = 1 or
					  sh.TieneHoldIngenieria = 1),0),
				js.FamiliaAceroMaterial1ID,
				js.FamiliaAceroMaterial2ID,
				fm1.Nombre,
				fm2.Nombre,
				js.peqs,
				js.KgTeoricos,
				ots.OrdenTrabajoSpoolID,
				js.ItemCodeIDMaterial1,
				js.CodigoItemCodeMaterial1,
				js.DescripcionItemCodeMaterial1,
				js.ItemCodeIDMaterial2,
				js.CodigoItemCodeMaterial2,
				js.DescripcionItemCodeMaterial2
			FROM #TempJuntaWorkstatus jws
			INNER JOIN #TempJuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
			INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
			INNER JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
			INNER JOIN #TempProyecto p on 1 = 1
			INNER JOIN #TempSpool s on s.SpoolID = js.SpoolID
			LEFT JOIN UltimoProceso up on up.UltimoProcesoID = jws.UltimoProcesoID
			INNER JOIN TipoJunta tj on tj.TipoJuntaID = js.TipoJuntaID
			LEFT JOIN SpoolHold sh on sh.SpoolID = s.SpoolID
			INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
			INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
			LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
			LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
			
	--DESPLEGAR TABLA
	
		select g.GeneralJuntaWorkstatusID,
			   g.GeneralProyecto,
			   g.GeneralOrdenDeTrabajo,
			   g.GeneralNumeroDeControl,
			   g.GeneralSpool,
			   g.GeneralJunta,
			   g.GeneralTipoJunta,
			   g.GeneralDiametro,
			   g.GeneralCedula,
			   g.GeneralEspesor,
			   g.GeneralLocalizacion,
			   g.GeneralUltimoProceso,
			   g.GeneralTieneHold,
			   g.GeneralFamAcero1,
			   g.GeneralFamAcero2,
			   g.CodigoItemCodeMaterial1,
			   g.DescripcionItemCodeMaterial1,
			   g.CodigoItemCodeMaterial2,
			   g.DescripcionItemCodeMaterial2,
			   g.GeneralPeqs,
			   g.GeneralKgTeoricos,
			   ja.FechaArmado [ArmadoFecha],
			   ja.FechaReporte [ArmadoFechaReporte],
			   ja.Taller [ArmadoTaller],
			   ja.Tubero [ArmadoTubero],
			   ja.NumeroUnico1 [ArmadoNumeroUnico1],
			   ja.NumeroUnico2 [ArmadoNumeroUnico2],
			   js.SoldaduraFecha,
			   js.SoldaduraFechaReporte,
			   js.SoldaduraTaller,
			   js.SoldaduraWPS,
			   js.SoldaduraProcesoRelleno,
			   js.SoldaduraConsumiblesRelleno,
			   js.SoldaduraProcesoRaiz,
			   js.SoldaduraSoldadorRaiz,
			   js.SoldaduraSoldadorRelleno,
			   js.SoldaduraMaterialBase1,
			   js.SoldaduraMaterialBase2,
			   iv.InspeccionVisualFecha,
			   iv.InspeccionVisualFechaReporte,
			   iv.InspeccionVisualNumeroReporte,   
			   iv.InspeccionVisualHoja,
			   iv.InspeccionVisualResultado,
			   iv.InspeccionVisualDefecto,
			   iv.InspeccionVisualObservaciones,
			   id.InspeccionDimensionalFecha,
			   id.InspeccionDimensionalFechaReporte,
			   id.InspeccionDimensionalNumeroReporte,
			   id.InspeccionDimensionalHoja,
			   id.InspeccionDimensionalResultado,		   
			   ie.InspeccionEspesoresFecha,
			   ie.InspeccionEspesoresFechaReporte,
			   ie.InspeccionEspesoresNumeroReporte,
			   ie.InspeccionEspesoresHoja,
			   ie.InspeccionEspesoresResultado,
			   ie.InspeccionEspesoresObservaciones,
			   prt.PruebaRTFechaRequisicion,
			   prt.PruebaRTNumeroRequisicion,
			   prt.PruebaRTCodigoRequisicion,
			   prt.PruebaRTFechaPrueba,
			   prt.PruebaRTFechaReporte,
			   prt.PruebaRTNumeroReporte,
			   prt.PruebaRTHoja,
			   prt.PruebaRTResultado,
			   prt.PruebaRTDefecto,
			   prt.PruebaRTObservacionesReporte,
			   prt.PruebaRTObservacionesRequisicion,
			   ppt.PruebaPTFechaRequisicion,
			   ppt.PruebaPTNumeroRequisicion,
			   ppt.PruebaPTCodigoRequisicion,
			   ppt.PruebaPTFechaPrueba,
			   ppt.PruebaPTFechaReporte,
			   ppt.PruebaPTNumeroReporte,
			   ppt.PruebaPTHoja,
			   ppt.PruebaPTResultado,
			   ppt.PruebaPTDefecto,
			   ppt.PruebaPTObservacionesReporte,
			   ppt.PruebaPTObservacionesRequisicion,
			   prtptt.PruebaRTPostTTFechaRequisicion,
			   prtptt.PruebaRTPostTTNumeroRequisicion,
			   prtptt.PruebaRTPostTTCodigoRequisicion,
			   prtptt.PruebaRTPostTTFechaPrueba,
			   prtptt.PruebaRTPostTTFechaReporte,
			   prtptt.PruebaRTPostTTNumeroReporte,
			   prtptt.PruebaRTPostTTHoja,
			   prtptt.PruebaRTPostTTResultado,
			   prtptt.PruebaRTPostTTDefecto,
			   prtptt.PruebaRTPostTTObservacionesReporte,
			   prtptt.PruebaRTPostTTObservacionesRequisicion,
			   pptptt.PruebaPTPostTTFechaRequisicion,
			   pptptt.PruebaPTPostTTNumeroRequisicion,
			   pptptt.PruebaPTPostTTCodigoRequisicion,
			   pptptt.PruebaPTPostTTFechaPrueba,
			   pptptt.PruebaPTPostTTFechaReporte,
			   pptptt.PruebaPTPostTTNumeroReporte,
			   pptptt.PruebaPTPostTTHoja,
			   pptptt.PruebaPTPostTTResultado,
			   pptptt.PruebaPTPostTTDefecto,
			   pptptt.PruebaPTPostTTObservacionesReporte,
			   pptptt.PruebaPTPostTTObservacionesRequisicion,
			   put.PruebaUTFechaRequisicion,
			   put.PruebaUTNumeroRequisicion,
			   put.PruebaUTCodigoRequisicion,
			   put.PruebaUTFechaPrueba,
			   put.PruebaUTFechaReporte,
			   put.PruebaUTNumeroReporte,
			   put.PruebaUTHoja,
			   put.PruebaUTResultado,
			   put.PruebaUTDefecto,
			   put.PruebaUTObservacionesReporte,
			   put.PruebaUTObservacionesRequisicion,
			   tpwht.TratamientoPwhtFechaRequisicion,
			   tpwht.TratamientoPwhtNumeroRequisicion,
			   tpwht.TratamientoPwhtCodigoRequisicion,
			   tpwht.TratamientoPwhtFechaTratamiento,
			   tpwht.TratamientoPwhtFechaReporte,
			   tpwht.TratamientoPwhtNumeroReporte,
			   tpwht.TratamientoPwhtHoja,
			   tpwht.TratamientoPwhtGrafica,
			   tpwht.TratamientoPwhtResultado,
			   tpwht.TratamientoPwhtObservacionesReporte,
			   tpwht.TratamientoPwhtObservacionesRequisicion,
			   td.TratamientoDurezasFechaRequisicion,
			   td.TratamientoDurezasNumeroRequisicion,
			   td.TratamientoDurezasCodigoRequisicion,
			   td.TratamientoDurezasFechaTratamiento,
			   td.TratamientoDurezasFechaReporte,
			   td.TratamientoDurezasNumeroReporte,
			   td.TratamientoDurezasHoja,
			   td.TratamientoDurezasGrafica,
			   td.TratamientoDurezasResultado,
			   td.TratamientoDurezasObservacionesReporte,
			   td.TratamientoDurezasObservacionesRequisicion,
			   tp.TratamientopreheatFechaRequisicion,
			   tp.TratamientopreheatNumeroRequisicion,
			   tp.TratamientopreheatCodigoRequisicion,
			   tp.TratamientopreheatFechaTratamiento,
			   tp.TratamientopreheatFechaReporte,
			   tp.TratamientopreheatNumeroReporte,
			   tp.TratamientopreheatHoja,
			   tp.TratamientopreheatGrafica,
			   tp.TratamientopreheatResultado,
			   tp.TratamientopreheatObservacionesReporte,
			   tp.TratamientopreheatObservacionesRequisicion,
			   p.PinturaFechaRequisicion,
			   p.PinturaNumeroRequisicion,
			   p.PinturaSistema,
			   p.PinturaColor,
			   p.PinturaCodigo,
			   p.PinturaFechaSendBlast,
			   p.PinturaReporteSendBlast,
			   p.PinturaFechaPrimarios,
			   p.PinturaReportePrimarios,
			   p.PinturaFechaIntermedios,
			   p.PinturaReporteIntermedios,
			   p.PinturaFechaAcabadoVisual,
			   p.PinturaReporteAcabadoVisual,
			   p.PinturaFechaAdherencia,
			   p.PinturaReporteAdherencia,
			   p.PinturaFechaPullOff,
			   p.PinturaReportePullOff,
			   e.EmbarqueEtiqueta,
			   e.EmbarqueFechaEtiqueta,
			   e.EmbarqueFechaPreparacion,
			   e.EmbarqueFechaEmbarque,
			   e.EmbarqueNumeroEmbarque
		from #TempJuntaWorkstatus jw
		INNER JOIN #TempGeneral g on g.GeneralJuntaWorkstatusID = jw.JuntaWorkStatusID
		INNER JOIN
		(
			SELECT	jarm.JuntaArmadoID,
					jarm.JuntaWorkstatusID,
					FechaArmado,
					FechaReporte,
					ta.Nombre [Taller],
					tu.Codigo [Tubero],
					NumeroUnico1ID,
					NumeroUnico2ID,
					nu1.Codigo [NumeroUnico1],
					nu2.Codigo [NumeroUnico2],
					jarm.TuberoID
			FROM JuntaArmado jarm
			INNER JOIN Taller ta on ta.TallerID = jarm.TallerID 
			INNER JOIN Tubero tu on tu.TuberoID = jarm.TuberoID
			INNER JOIN NumeroUnico nu1 on nu1.NumeroUnicoID = jarm.NumeroUnico1ID
			INNER JOIN NumeroUnico nu2 on nu2.NumeroUnicoID = jarm.NumeroUnico2ID
			WHERE jarm.JuntaWorkstatusID IN
			(
				SELECT JuntaWorkstatusID FROM #TempJuntaWorkstatus
			)
		) ja on ja.JuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempJuntaSoldadura js on js.SoldaduraJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempJuntaInspeccionVisual iv on iv.InspeccionVisualJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempWorkstatusSpool ws on ws.OrdenTrabajoSpoolID = jw.OrdenTrabajoSpoolID
		LEFT JOIN #TempPintura p on p.PinturaWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN #TempEmbarque e on e.EmbarqueWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN
		(			
			select	tmp.PruebaRTFechaRequisicion,
					tmp.PruebaRTNumeroRequisicion,
					tmp.PruebaRTCodigoRequisicion,
					tmp.PruebaRTFechaPrueba,
					tmp.PruebaRTFechaReporte,
					tmp.PruebaRTNumeroReporte,
					tmp.PruebaRTHoja,
					tmp.PruebaRTResultado,
					tmp.PruebaRTDefecto,
					tmp.PruebaRTObservacionesReporte,
					tmp.PruebaRTObservacionesRequisicion,
					tmp.PruebaRTJuntaWorkstatusID
			from #TempPruebaRT tmp
			inner join(
				select	PruebaRTJuntaWorkstatusID, 
						MAX(PruebaRTFechaPrueba) as PruebaRTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaRT
				group by PruebaRTJuntaWorkstatusID
			) A on	A.PruebaRTJuntaWorkstatusID = tmp.PruebaRTJuntaWorkstatusID
					and A.PruebaRTFechaPrueba = tmp.PruebaRTFechaPrueba
					AND A.FechaModificacion = tmp.FechaModificacion
		)  prt on prt.PruebaRTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.PruebaPTFechaRequisicion,
					tmp.PruebaPTNumeroRequisicion,
					tmp.PruebaPTCodigoRequisicion,
					tmp.PruebaPTFechaPrueba,
					tmp.PruebaPTFechaReporte,
					tmp.PruebaPTNumeroReporte,
					tmp.PruebaPTHoja,
					tmp.PruebaPTResultado,
					tmp.PruebaPTDefecto,
					tmp.PruebaPTObservacionesReporte,
					tmp.PruebaPTObservacionesRequisicion,
					tmp.PruebaPTJuntaWorkstatusID
			from #TempPruebaPT tmp
			inner join(
				select	PruebaPTJuntaWorkstatusID, 
						MAX(PruebaPTFechaPrueba) as PruebaPTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion						
				from #TempPruebaPT
				group by PruebaPTJuntaWorkstatusID
			) A on A.PruebaPTJuntaWorkstatusID = tmp.PruebaPTJuntaWorkstatusID
			and A.PruebaPTFechaPrueba = tmp.PruebaPTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
			
		)  ppt on ppt.PruebaPTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			select	tmp.PruebaRTPostTTFechaRequisicion,
					tmp.PruebaRTPostTTNumeroRequisicion,
					tmp.PruebaRTPostTTCodigoRequisicion,
					tmp.PruebaRTPostTTFechaPrueba,
					tmp.PruebaRTPostTTFechaReporte,
					tmp.PruebaRTPostTTNumeroReporte,
					tmp.PruebaRTPostTTHoja,
					tmp.PruebaRTPostTTResultado,
					tmp.PruebaRTPostTTDefecto,
					tmp.PruebaRTPostTTObservacionesReporte,
					tmp.PruebaRTPostTTObservacionesRequisicion, 
					tmp.PruebaRTPostTTJuntaWorkstatusID
			from #TempPruebaRTPostTT tmp
			inner join(
				select	PruebaRTPostTTJuntaWorkstatusID, 
						MAX(PruebaRTPostTTFechaPrueba) as PruebaRTPostTTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaRTPostTT
				group by PruebaRTPostTTJuntaWorkstatusID
			) A on A.PruebaRTPostTTJuntaWorkstatusID = tmp.PruebaRTPostTTJuntaWorkstatusID
			and A.PruebaRTPostTTFechaPrueba = tmp.PruebaRTPostTTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
		)  prtptt on prtptt.PruebaRTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(	
			select	tmp.PruebaPTPostTTFechaRequisicion,
					tmp.PruebaPTPostTTNumeroRequisicion,
					tmp.PruebaPTPostTTCodigoRequisicion,
					tmp.PruebaPTPostTTFechaPrueba,
					tmp.PruebaPTPostTTFechaReporte,
					tmp.PruebaPTPostTTNumeroReporte,
					tmp.PruebaPTPostTTHoja,
					tmp.PruebaPTPostTTResultado,
					tmp.PruebaPTPostTTDefecto,
					tmp.PruebaPTPostTTObservacionesReporte,
					tmp.PruebaPTPostTTObservacionesRequisicion,
					tmp.PruebaPTPostTTJuntaWorkstatusID
			from #TempPruebaPTPostTT tmp
			inner join(
				select	PruebaPTPostTTJuntaWorkstatusID, 
						MAX(PruebaPTPostTTFechaPrueba) as PruebaPTPostTTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaPTPostTT
				group by PruebaPTPostTTJuntaWorkstatusID
			) A on	A.PruebaPTPostTTJuntaWorkstatusID = tmp.PruebaPTPostTTJuntaWorkstatusID
					and A.PruebaPTPostTTFechaPrueba = tmp.PruebaPTPostTTFechaPrueba
					AND A.FechaModificacion = tmp.FechaModificacion
		)  pptptt on pptptt.PruebaPTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		( 
			select	tmp.PruebaUTFechaRequisicion,
					tmp.PruebaUTNumeroRequisicion,
					tmp.PruebaUTCodigoRequisicion,
					tmp.PruebaUTFechaPrueba,
					tmp.PruebaUTFechaReporte,
					tmp.PruebaUTNumeroReporte,
					tmp.PruebaUTHoja,
					tmp.PruebaUTResultado,
					tmp.PruebaUTDefecto,
					tmp.PruebaUTObservacionesReporte,
					tmp.PruebaUTObservacionesRequisicion,
					tmp.PruebaUTJuntaWorkstatusID
			from #TempPruebaUT tmp
			inner join(
				select	PruebaUTJuntaWorkstatusID, 
						MAX(PruebaUTFechaPrueba) as PruebaUTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaUT
				group by PruebaUTJuntaWorkstatusID
			) A on A.PruebaUTJuntaWorkstatusID = tmp.PruebaUTJuntaWorkstatusID
			and A.PruebaUTFechaPrueba = tmp.PruebaUTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
		)  put on put.PruebaUTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.TratamientoPwhtFechaRequisicion,
					tmp.TratamientoPwhtNumeroRequisicion,
					tmp.TratamientoPwhtCodigoRequisicion,
					tmp.TratamientoPwhtFechaTratamiento,
					tmp.TratamientoPwhtFechaReporte,
					tmp.TratamientoPwhtNumeroReporte,
					tmp.TratamientoPwhtHoja,
					tmp.TratamientoPwhtGrafica,
					tmp.TratamientoPwhtResultado,
					tmp.TratamientoPwhtObservacionesReporte,
					tmp.TratamientoPwhtObservacionesRequisicion,
					tmp.TratamientoPwhtJuntaWorkstatusID
			from #TempTratamientoPwht tmp
			inner join(
				select	TratamientoPwhtJuntaWorkstatusID, 
						MAX(TratamientoPwhtFechaTratamiento) as TratamientoPwhtFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoPwht
				group by TratamientoPwhtJuntaWorkstatusID
			) A on A.TratamientoPwhtJuntaWorkstatusID = tmp.TratamientoPwhtJuntaWorkstatusID
			and A.TratamientoPwhtFechaTratamiento = tmp.TratamientoPwhtFechaTratamiento
			AND A.FechaModificacion = tmp.FechaModificacion
		)  tpwht on tpwht.TratamientoPwhtJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.TratamientoDurezasFechaRequisicion,
					tmp.TratamientoDurezasNumeroRequisicion,
					tmp.TratamientoDurezasCodigoRequisicion,
					tmp.TratamientoDurezasFechaTratamiento,
					tmp.TratamientoDurezasFechaReporte,
					tmp.TratamientoDurezasNumeroReporte,
					tmp.TratamientoDurezasHoja,
					tmp.TratamientoDurezasGrafica,
					tmp.TratamientoDurezasResultado,
					tmp.TratamientoDurezasObservacionesReporte,
					tmp.TratamientoDurezasObservacionesRequisicion, 
					tmp.TratamientoDurezasJuntaWorkstatusID
			from #TempTratamientoDurezas tmp
			inner join(
				select	TratamientoDurezasJuntaWorkstatusID,
						MAX(TratamientoDurezasFechaTratamiento) as TratamientoDurezasFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoDurezas
				group by TratamientoDurezasJuntaWorkstatusID
			) A on A.TratamientoDurezasJuntaWorkstatusID = tmp.TratamientoDurezasJuntaWorkstatusID
			and A.TratamientoDurezasFechaTratamiento = tmp.TratamientoDurezasFechaTratamiento			
			AND A.FechaModificacion = tmp.FechaModificacion			
		) td on td.TratamientoDurezasJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			select	tmp.TratamientopreheatFechaRequisicion,
					tmp.TratamientopreheatNumeroRequisicion,
					tmp.TratamientopreheatCodigoRequisicion,
					tmp.TratamientopreheatFechaTratamiento,
					tmp.TratamientopreheatFechaReporte,
					tmp.TratamientopreheatNumeroReporte,
					tmp.TratamientopreheatHoja,
					tmp.TratamientopreheatGrafica,
					tmp.TratamientopreheatResultado,
					tmp.TratamientopreheatObservacionesReporte,
					tmp.TratamientopreheatObservacionesRequisicion,
					tmp.TratamientoPreheatJuntaWorkstatusID
			from #TempTratamientoPreheat tmp
			inner join(
				select	TratamientoPreheatJuntaWorkstatusID,
						MAX(TratamientoPreheatFechaTratamiento) as TratamientoPreheatFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoPreheat
				group by TratamientoPreheatJuntaWorkstatusID
			) A on A.TratamientoPreheatJuntaWorkstatusID = tmp.TratamientoPreheatJuntaWorkstatusID
				AND A.TratamientoPreheatFechaTratamiento = tmp.TratamientoPreheatFechaTratamiento
				AND A.FechaModificacion = tmp.FechaModificacion			
		) tp on tp.TratamientopreheatJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			SELECT	tmp.InspeccionDimensionalFecha,
					tmp.InspeccionDimensionalFechaReporte,
					tmp.InspeccionDimensionalNumeroReporte,
					tmp.InspeccionDimensionalHoja,
					tmp.InspeccionDimensionalResultado,
					tmp.InspeccionDimensionalWorkstatusSpoolID
			FROM #TempInspeccionDimensional tmp
			INNER JOIN(
				SELECT	InspeccionDimensionalWorkstatusSpoolID, 
						MAX(InspeccionDimensionalFechaLiberacion) AS InspeccionDimensionalFechaLiberacion,
						MAX(FechaModificacion) AS FechaModificacion
				FROM #TempInspeccionDimensional
				GROUP BY InspeccionDimensionalWorkstatusSpoolID
			) A ON A.InspeccionDimensionalWorkstatusSpoolID = tmp.InspeccionDimensionalWorkstatusSpoolID
				AND A.InspeccionDimensionalFechaLiberacion = tmp.InspeccionDimensionalFechaLiberacion
				AND A.FechaModificacion = tmp.FechaModificacion			
		) id on id.InspeccionDimensionalWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN
		(
			SELECT	tmp.InspeccionEspesoresFecha,
					tmp.InspeccionEspesoresFechaReporte,
					tmp.InspeccionEspesoresNumeroReporte,
					tmp.InspeccionEspesoresHoja,
					tmp.InspeccionEspesoresResultado,
					tmp.InspeccionEspesoresObservaciones,
					tmp.InspeccionEspesoresWorkstatusSpoolID
			FROM #TempInspeccionEspesores tmp
			INNER JOIN(
					SELECT InspeccionEspesoresWorkstatusSpoolID, 
						   MAX(InspeccionEspesoresFechaLiberacion) AS InspeccionEspesoresFechaLiberacion,
						   MAX(FechaModificacion) AS FechaModificacion
					FROM #TempInspeccionEspesores
					GROUP BY InspeccionEspesoresWorkstatusSpoolID
				) A ON A.InspeccionEspesoresWorkstatusSpoolID = tmp.InspeccionEspesoresWorkstatusSpoolID
					AND A.InspeccionEspesoresFechaLiberacion = tmp.InspeccionEspesoresFechaLiberacion
		) ie on ie.InspeccionEspesoresWorkstatusSpoolID = ws.WorkstatusSpoolID
		
	DROP TABLE #TempProyecto
	DROP TABLE #TempOrdenTrabajo
	DROP TABLE #TempGeneral 
	DROP TABLE #TempEmbarque 
	DROP TABLE #TempPintura 
	DROP TABLE #TempTratamientoPreheat 
	DROP TABLE #TempTratamientoDurezas 
	DROP TABLE #TempTratamientoPwht 
	DROP TABLE #TempPruebaUT 
	DROP TABLE #TempPruebaPTPostTT 
	DROP TABLE #TempPruebaRTPostTT 
	DROP TABLE #TempPruebaRT 	
	DROP TABLE #TempJuntaInspeccionVisual 
	DROP TABLE #TempWorkstatusSpool 
	DROP TABLE #TempInspeccionEspesores 
	DROP TABLE #TempInspeccionDimensional 
	DROP TABLE #TempReporteDimensional 
	DROP TABLE #TempJuntaReportePnd 	
	DROP TABLE #TempJuntaReporteTt 
	DROP TABLE #TempJuntaSoldadura 	
	DROP TABLE #TempJuntaWorkstatus 
	DROP TABLE #TempJuntaSpool 
	DROP TABLE #TempSpool 
	DROP TABLE #TempOrdenTrabajoSpool
	DROP TABLE #TempPruebaPT
	DROP TABLE #TempMaterialSpool
	DROP TABLE #TempJuntaInspeccionVisualDefecto

		
	SET NOCOUNT OFF;

END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSeguimientoDeJuntasPorOrdenTrabajoSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorOrdenTrabajoSpool]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerSeguimientoDeJuntasPorOrdenTrabajoSpool
	Funcion:	Trae toda la informacion necesaria para el seguimiento de jutnas
	Parametros:	@OrdenTrabajoSpoolID INT
				@HistorialRep BIT
	Autor:		MMG
	Modificado:	28/01/2011 IHM
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorOrdenTrabajoSpool]
(
	@OrdenTrabajoSpoolID INT,
	@HistorialRep BIT
)
AS
BEGIN
	
	SET NOCOUNT ON; 

	--TABLA PROYECTO
	CREATE TABLE #TempProyecto
	(	
		ProyectoID INT,
		Patio INT,
		Nombre NVARCHAR(100)
	)
	
	--TABLA ORDEN DE TRABAJO
	CREATE TABLE #TempOrdenTrabajo
	(	
		OrdenTrabajoID INT,
		NumeroOrden NVARCHAR(50)
	)
	
	--TABLA ORDEN TRABAJO SPOOL
	CREATE TABLE #TempOrdenTrabajoSpool
	(	
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoID INT,
		NumeroControl NVARCHAR(50),
		SpoolID INT
	)
	
	--TABLA SPOOL
	CREATE TABLE #TempSpool 
	(	
		SpoolID INT,
		Nombre NVARCHAR(50)
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempJuntaSpool 
	(	
		JuntaSpoolID INT,
		SpoolID INT,
		TipoJuntaID INT,
		Diametro DECIMAL(7,4),
		Cedula NVARCHAR(10),
		Espesor DECIMAL(10,4),
		EtiquetaMaterial1 NVARCHAR(10),
		EtiquetaMaterial2 NVARCHAR(10),
		FamiliaAceroMaterial1ID INT,
		FamiliaAceroMaterial2ID INT,
		peqs DECIMAL(10,4),
		KgTeoricos DECIMAL(12,4),
		Etiqueta1EsNumero BIT,
		Etiqueta2EsNumero BIT,
		ValorNumericoEtiqueta1 TINYINT,
		ValorNumericoEtiqueta2 TINYINT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150)	
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempMaterialSpool
	(	
		MaterialSpoolID INT,
		SpoolID INT,
		ItemCodeID INT,
		Etiqueta NVARCHAR(10),
		CodigoItemCode NVARCHAR(50),
		DescripcionItemCode NVARCHAR(150),
		EtiquetaEsNumero BIT,
		ValorNumericoEtiqueta TINYINT
	)
	
	--TABLA JUNTA WORKSTATUS
	CREATE TABLE #TempJuntaWorkstatus 
	(	
		JuntaWorkStatusID INT,
		JuntaSpoolID INT,
		JuntaArmadoID INT,
		JuntaSoldaduraID INT,
		JuntaInspeccionVisualID INT,
		OrdenTrabajoSpoolID INT,
		UltimoProcesoID INT,
		EtiquetaJunta NVARCHAR(50),
		JuntaFinal BIT
	)
	
	--TABLA JUNTA SOLDADURA
	CREATE TABLE #TempJuntaSoldadura 
	(	
		SoldaduraJuntaSoldaduraID INT,
		SoldaduraJuntaWorkstatusID INT,
		SoldaduraFecha DATETIME,
		SoldaduraFechaReporte DATETIME,
		SoldaduraTaller NVARCHAR(50),
		SoldaduraWPS NVARCHAR(50),
		SoldaduraProcesoRelleno NVARCHAR(50),
		SoldaduraConsumiblesRelleno NVARCHAR(50),
		SoldaduraProcesoRaiz NVARCHAR(50),
		SoldaduraSoldadorRaiz NVARCHAR(50),
		SoldaduraSoldadorRelleno NVARCHAR(50),
		SoldaduraMaterialBase1 NVARCHAR(50),
		SoldaduraMaterialBase2 NVARCHAR(50)
	)
	
	--TABLA JUNTA REPORTE TT
	CREATE TABLE #TempJuntaReporteTt 
	(	
		JuntaReporteTtID INT,
		JuntaWorkstatusID INT,
		TipoPruebaID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		JuntaRequisicionID INT,
		Aprobado BIT,
		NumeroGrafica  NVARCHAR(20),
		Hoja INT,
		FechaTratamiento DATETIME,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA JUNTA REPORTE PND
	CREATE TABLE #TempJuntaReportePnd 
	(	
		JuntaReportePndID INT,
		JuntaWorkstatusID INT,
		TipoPruebaID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		JuntaRequisicionID INT,		
		FechaPrueba DATETIME,
		Hoja INT,
		Aprobado BIT,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME
	)
			
	--TABLA REPORTE DIMENSIONAL
	CREATE TABLE #TempReporteDimensional 
	(	
		ReporteDimensionalID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		TipoReporteDimensionalID INT,
		FechaModificacion DATETIME
	)
	
	--TABLA INSPECCION DIMENSIONAL
	CREATE TABLE #TempInspeccionDimensional 
	(	
		InspeccionDimensionalReporteDimensionalDetalleID INT,
		InspeccionDimensionalWorkstatusSpoolID INT,
		InspeccionDimensionalFecha DATETIME,
		InspeccionDimensionalFechaReporte DATETIME,
		InspeccionDimensionalNumeroReporte NVARCHAR(50),
		InspeccionDimensionalHoja INT,
		InspeccionDimensionalResultado BIT,
		InspeccionDimensionalObservaciones NVARCHAR(500),
		InspeccionDimensionalFechaLiberacion DATETIME,		
		FechaModificacion DATETIME
	)
	
	--TABLA INSPECCION ESPESORES
	CREATE TABLE #TempInspeccionEspesores 
	(	
		InspeccionEspesoresReporteDimensionalDetalleID INT,
		InspeccionEspesoresWorkstatusSpoolID INT,
		InspeccionEspesoresFecha DATETIME,
		InspeccionEspesoresFechaReporte DATETIME,
		InspeccionEspesoresNumeroReporte NVARCHAR(50),
		InspeccionEspesoresHoja INT,
		InspeccionEspesoresResultado BIT,
		InspeccionEspesoresObservaciones NVARCHAR(500),
		InspeccionEspesoresFechaLiberacion DATETIME,
		FechaModificacion DATETIME
	)	
	
	--TABLA WORKSTATUS SPOOL
	CREATE TABLE #TempWorkstatusSpool 
	(	
		WorkstatusSpoolID INT,
		OrdenTrabajoSpoolID INT,
		PinturaSistema NVARCHAR(50),
		PinturaColor NVARCHAR(50),
		PinturaCodigo NVARCHAR(50),
		EmbarqueEtiqueta NVARCHAR(20),
		FechaEtiqueta DATETIME,
		FechaPreparacion DATETIME		
	)
		
	--TABLA JUNTA INSPECCION VISUAL
	CREATE TABLE #TempJuntaInspeccionVisual 
	(	
		InspeccionVisualJuntaInspeccionVisualID INT,
		InspeccionVisualJuntaWorkstatusID INT,
		InspeccionVisualFecha DATETIME,
		InspeccionVisualFechaReporte DATETIME,
		InspeccionVisualNumeroReporte NVARCHAR(50),
		InspeccionVisualHoja INT,
		InspeccionVisualResultado BIT,
		InspeccionVisualDefecto NVARCHAR(MAX),
		InspeccionVisualObservaciones NVARCHAR(500)		
	)
	
	CREATE TABLE #TempJuntaInspeccionVisualDefecto
	(
		JuntaInspeccionVisualID int,
		JuntaWorkstatusID int,
		Fecha datetime,
		FechaReporte datetime,
		NumeroReporte nvarchar(50),
		Hoja int,
		Resultado bit,
		Defecto nvarchar(100),
		Observaciones nvarchar(500),
		FechaModificacion datetime
	)
	
	--TABLA PRUEBA RT
	CREATE TABLE #TempPruebaRT 
	(	
		PruebaRTJuntaReportePndID INT,
		PruebaRTJuntaWorkstatusID INT,
		PruebaRTFechaRequisicion DATETIME,
		PruebaRTNumeroRequisicion NVARCHAR(50),
		PruebaRTCodigoRequisicion NVARCHAR(50),
		PruebaRTFechaPrueba DATETIME,
		PruebaRTFechaReporte DATETIME,
		PruebaRTNumeroReporte NVARCHAR(50),
		PruebaRTHoja INT,
		PruebaRTResultado BIT,
		PruebaRTDefecto NVARCHAR(MAX),
		PruebaRTObservacionesReporte NVARCHAR(500),
		PruebaRTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA PT
	CREATE TABLE #TempPruebaPT 
	(	
		PruebaPTJuntaReportePndID INT,
		PruebaPTJuntaWorkstatusID INT,
		PruebaPTFechaRequisicion DATETIME,
		PruebaPTNumeroRequisicion NVARCHAR(50),
		PruebaPTCodigoRequisicion NVARCHAR(50),
		PruebaPTFechaPrueba DATETIME,
		PruebaPTFechaReporte DATETIME,
		PruebaPTNumeroReporte NVARCHAR(50),
		PruebaPTHoja INT,
		PruebaPTResultado BIT,
		PruebaPTDefecto NVARCHAR(MAX),
		PruebaPTObservacionesReporte NVARCHAR(500),
		PruebaPTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA RT(PostTT)
	CREATE TABLE #TempPruebaRTPostTT 
	(	
		PruebaRTPostTTJuntaReportePndID INT,
		PruebaRTPostTTJuntaWorkstatusID INT,
		PruebaRTPostTTFechaRequisicion DATETIME,
		PruebaRTPostTTNumeroRequisicion NVARCHAR(50),
		PruebaRTPostTTCodigoRequisicion NVARCHAR(50),
		PruebaRTPostTTFechaPrueba DATETIME,
		PruebaRTPostTTFechaReporte DATETIME,
		PruebaRTPostTTNumeroReporte NVARCHAR(50),
		PruebaRTPostTTHoja INT,
		PruebaRTPostTTResultado BIT,
		PruebaRTPostTTDefecto NVARCHAR(MAX),
		PruebaRTPostTTObservacionesReporte NVARCHAR(500),
		PruebaRTPostTTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA PT(PostTT)
	CREATE TABLE #TempPruebaPTPostTT 
	(	
		PruebaPTPostTTJuntaReportePndID INT,
		PruebaPTPostTTJuntaWorkstatusID INT,
		PruebaPTPostTTFechaRequisicion DATETIME,
		PruebaPTPostTTNumeroRequisicion NVARCHAR(50),
		PruebaPTPostTTCodigoRequisicion NVARCHAR(50),
		PruebaPTPostTTFechaPrueba DATETIME,
		PruebaPTPostTTFechaReporte DATETIME,
		PruebaPTPostTTNumeroReporte NVARCHAR(50),
		PruebaPTPostTTHoja INT,
		PruebaPTPostTTResultado BIT,
		PruebaPTPostTTDefecto NVARCHAR(MAX),
		PruebaPTPostTTObservacionesReporte NVARCHAR(500),
		PruebaPTPostTTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA UT
	CREATE TABLE #TempPruebaUT 
	(	
		PruebaUTJuntaReportePndID INT,
		PruebaUTJuntaWorkstatusID INT,
		PruebaUTFechaRequisicion DATETIME,
		PruebaUTNumeroRequisicion NVARCHAR(50),
		PruebaUTCodigoRequisicion NVARCHAR(50),
		PruebaUTFechaPrueba DATETIME,
		PruebaUTFechaReporte DATETIME,
		PruebaUTNumeroReporte NVARCHAR(50),
		PruebaUTHoja INT,
		PruebaUTResultado BIT,
		PruebaUTDefecto NVARCHAR(MAX),
		PruebaUTObservacionesReporte NVARCHAR(500),
		PruebaUTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA TRATAMIENTO PWHT
	CREATE TABLE #TempTratamientoPwht 
	(	
		TratamientoPwhtJuntaReporteTtID INT,
		TratamientoPwhtJuntaWorkstatusID INT,
		TratamientoPwhtFechaRequisicion DATETIME,
		TratamientoPwhtNumeroRequisicion NVARCHAR(50),
		TratamientoPwhtCodigoRequisicion NVARCHAR(50),
		TratamientoPwhtFechaTratamiento DATETIME,
		TratamientoPwhtFechaReporte DATETIME,
		TratamientoPwhtNumeroReporte NVARCHAR(50),
		TratamientoPwhtHoja INT,
		TratamientoPwhtGrafica NVARCHAR(20),
		TratamientoPwhtResultado BIT,
		TratamientoPwhtObservacionesReporte NVARCHAR(500),
		TratamientoPwhtObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME		
	)
	
	--TABLA TRATAMIENTO DUREZAS
	CREATE TABLE #TempTratamientoDurezas 
	(	
		TratamientoDurezasJuntaReporteTtID INT,
		TratamientoDurezasJuntaWorkstatusID INT,
		TratamientoDurezasFechaRequisicion DATETIME,
		TratamientoDurezasNumeroRequisicion NVARCHAR(50),
		TratamientoDurezasCodigoRequisicion NVARCHAR(50),
		TratamientoDurezasFechaTratamiento DATETIME,
		TratamientoDurezasFechaReporte DATETIME,
		TratamientoDurezasNumeroReporte NVARCHAR(20),
		TratamientoDurezasHoja INT,
		TratamientoDurezasGrafica NVARCHAR(20),
		TratamientoDurezasResultado BIT,
		TratamientoDurezasObservacionesReporte NVARCHAR(500),
		TratamientoDurezasObservacionesRequisicion NVARCHAR(500),		
		FechaModificacion DATETIME
	)
	
	--TABLA TRATAMIENTO PREHEAT
	CREATE TABLE #TempTratamientoPreheat 
	(	
		TratamientoPreheatJuntaReporteTtID INT,
		TratamientoPreheatJuntaWorkstatusID INT,
		TratamientoPreheatFechaRequisicion DATETIME,
		TratamientoPreheatNumeroRequisicion NVARCHAR(50),
		TratamientoPreheatCodigoRequisicion NVARCHAR(50),
		TratamientoPreheatFechaTratamiento DATETIME,
		TratamientoPreheatFechaReporte DATETIME,
		TratamientoPreheatNumeroReporte NVARCHAR(50),
		TratamientoPreheatHoja INT,
		TratamientoPreheatGrafica NVARCHAR(20),
		TratamientoPreheatResultado BIT,
		TratamientoPreheatObservacionesReporte NVARCHAR(500),
		TratamientoPreheatObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PINTURA
	CREATE TABLE #TempPintura 
	(	
		PinturaPinturaSpoolID INT,
		PinturaWorkstatusSpoolID INT,
		PinturaFechaRequisicion DATETIME,
		PinturaNumeroRequisicion NVARCHAR(50),
		PinturaSistema NVARCHAR(50),
		PinturaColor NVARCHAR(50),
		PinturaCodigo NVARCHAR(50),
		PinturaFechaSendBlast DATETIME,
		PinturaReporteSendBlast NVARCHAR(50),
		PinturaFechaPrimarios DATETIME,
		PinturaReportePrimarios NVARCHAR(50),
		PinturaFechaIntermedios DATETIME,
		PinturaReporteIntermedios NVARCHAR(50),
		PinturaFechaAcabadoVisual DATETIME,
		PinturaReporteAcabadoVisual NVARCHAR(50),
		PinturaFechaAdherencia DATETIME,
		PinturaReporteAdherencia NVARCHAR(50),
		PinturaFechaPullOff DATETIME,
		PinturaReportePullOff NVARCHAR(50)
	)
	
	--TABLA EMBARQUE
	CREATE TABLE #TempEmbarque 
	(
		EmbarqueEmbarqueSpoolID INT,
		EmbarqueWorkstatusSpoolID INT,
		EmbarqueEtiqueta NVARCHAR(20),
		EmbarqueFechaEtiqueta DATETIME,
		EmbarqueFechaPreparacion DATETIME,
		EmbarqueFechaEmbarque DATETIME,
		EmbarqueNumeroEmbarque NVARCHAR(50)
	)
	
	--TABLA GENERAL
	CREATE TABLE #TempGeneral 
	(
		GeneralJuntaWorkstatusID INT,
		GeneralProyecto NVARCHAR(50),
		GeneralOrdenDeTrabajo NVARCHAR(50),
		GeneralNumeroDeControl NVARCHAR(50),
		GeneralSpool NVARCHAR(50),
		GeneralJunta NVARCHAR(50),
		GeneralTipoJunta NVARCHAR(50),
		GeneralDiametro DECIMAL(7,4),
		GeneralCedula NVARCHAR(10),
		GeneralEspesor DECIMAL(10,4),
		GeneralLocalizacion NVARCHAR(50),
		GeneralUltimoProceso NVARCHAR(50),
		GeneralTieneHold BIT,
		GeneralFamiliaAceroMaterial1ID INT,
		GeneralFamiliaAceroMaterial2ID INT,
		GeneralFamAcero1 NVARCHAR(50),
		GeneralFamAcero2 NVARCHAR(50),
		GeneralPeqs DECIMAL(10,4),
		GeneralKgTeoricos DECIMAL(12,4),
		OrdenTrabajoSpoolID INT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150)	
	)
	
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_JuntaWorkstatusID ON #TempJuntaWorkstatus(JuntaWorkStatusID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_OrdenTrabajoSpoolID ON #TempJuntaWorkstatus(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaSoldadura_SoldaduraJuntaWorkstatusID ON #TempJuntaSoldadura(SoldaduraJuntaWorkstatusID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaInspeccionVisual_InspeccionVisualJuntaWorkstatusID ON #TempJuntaInspeccionVisual(InspeccionVisualJuntaWorkstatusID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_OrdenTrabajoSpoolID ON #TempWorkstatusSpool(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_WorkstatusSpoolID ON #TempWorkstatusSpool(WorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempPintura_PinturaWorkstatusSpoolID ON #TempPintura(PinturaWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempEmbarque_EmbarqueWorkstatusSpoolID ON #TempEmbarque(EmbarqueWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempGeneral_GeneralJuntaWorkstatusID ON #TempGeneral(GeneralJuntaWorkstatusID)

	--Comienzan los inserts a las tablas temporales con filtros si los tienen.
	
	--INSERT PROYECTO
	INSERT INTO #TempProyecto
		SELECT ProyectoID,
			   PatioID,
			   Nombre
		FROM Proyecto
		WHERE ProyectoID IN
		(
			SELECT	odt.ProyectoID
			FROM	OrdenTrabajo odt
			WHERE EXISTS
			(
				SELECT 1
				FROM	OrdenTrabajoSpool odts
				WHERE	odts.OrdenTrabajoID = odt.OrdenTrabajoID
						AND odts.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID
			)
		)

	-- INSERT A ODT	
	INSERT INTO #TempOrdenTrabajo
		SELECT OrdenTrabajoID,
			   ot.NumeroOrden
		FROM OrdenTrabajo ot
		WHERE OrdenTrabajoID IN
		(
			SELECT odts.OrdenTrabajoID
			FROM	OrdenTrabajoSpool odts
			WHERE	odts.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID
		)

	
	--INSERT ORDEN TRABAJO SPOOL
	INSERT INTO #TempOrdenTrabajoSpool
		SELECT OrdenTrabajoSpoolID,
			   ots.OrdenTrabajoID,
			   ots.NumeroControl,
			   ots.SpoolID
		FROM OrdenTrabajoSpool ots
		WHERE ots.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID
	
	--INSERT SPOOL
	INSERT INTO #TempSpool
		SELECT SpoolID,
			   Nombre
		FROM Spool
		WHERE SpoolID IN
		(
			SELECT SpoolID FROM #TempOrdenTrabajoSpool
		)
	
	--INSERT Material Spool
	INSERT INTO #TempMaterialSpool
	(
		SpoolID, 
		MaterialSpoolID, 
		ItemCodeID, 
		CodigoItemCode, 
		DescripcionItemCode, 
		Etiqueta, 
		EtiquetaEsNumero, 
		ValorNumericoEtiqueta
	)
	SELECT	ms.SpoolID,
			ms.MaterialSpoolID,
			ms.ItemCodeID,
			ic.Codigo,
			ic.DescripcionEspanol,
			ms.Etiqueta,
			CAST(ISNUMERIC(ms.Etiqueta) AS BIT),
			CASE WHEN ISNUMERIC(ms.Etiqueta) = 1 THEN CAST(ms.Etiqueta AS TINYINT) ELSE NULL END
	FROM MaterialSpool ms
	INNER JOIN ItemCode ic on ms.ItemCodeID = ic.ItemCodeID
	WHERE ms.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)
	
	--INSERT JUNTA SPOOL
	INSERT INTO #TempJuntaSpool
	(
		JuntaSpoolID,
		SpoolID,
		TipoJuntaID,
		Diametro,
		Cedula,
		Espesor,
		EtiquetaMaterial1,
		EtiquetaMaterial2,
		FamiliaAceroMaterial1ID,
		FamiliaAceroMaterial2ID,
		peqs,
		KgTeoricos,
		Etiqueta1EsNumero,
		Etiqueta2EsNumero,
		ValorNumericoEtiqueta1,
		ValorNumericoEtiqueta2,
		ItemCodeIDMaterial1,
		CodigoItemCodeMaterial1,
		DescripcionItemCodeMaterial1,
		ItemCodeIDMaterial2,
		CodigoItemCodeMaterial2,
		DescripcionItemCodeMaterial2
	)
	SELECT JuntaSpoolID,
		  js.SpoolID,
		  js.TipoJuntaID,
		  js.Diametro,
		  js.Cedula,
		  js.Espesor,
		  js.EtiquetaMaterial1,
		  js.EtiquetaMaterial2,
		  js.FamiliaAceroMaterial1ID,
		  js.FamiliaAceroMaterial2ID,
		  js.Peqs,
		  js.KgTeoricos,
		  CAST(ISNUMERIC(js.EtiquetaMaterial1) AS BIT),
		  CAST(ISNUMERIC(js.EtiquetaMaterial2) AS BIT),
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS TINYINT) ELSE NULL END,
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS TINYINT) ELSE NULL END,
		  ms1.ItemCodeID,
		  ms1.CodigoItemCode,
		  ms1.DescripcionItemCode,
		  ms2.ItemCodeID,
		  ms2.CodigoItemCode,
		  ms2.DescripcionItemCode
	FROM JuntaSpool js
	LEFT JOIN #TempMaterialSpool ms1 on js.SpoolID = ms1.SpoolID and (ms1.Etiqueta = js.EtiquetaMaterial1 or ms1.ValorNumericoEtiqueta = (CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS TINYINT) ELSE NULL END))
	LEFT JOIN #TempMaterialSpool ms2 on js.SpoolID = ms2.SpoolID and (ms2.Etiqueta = js.EtiquetaMaterial2 or ms2.ValorNumericoEtiqueta = (CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS TINYINT) ELSE NULL END))
	WHERE js.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)
	
	--INSERT JUNTA WORKSTATUS
	INSERT INTO #TempJuntaWorkstatus
		SELECT jws.JuntaWorkstatusID,
			   jws.JuntaSpoolID,
			   jws.JuntaArmadoID,
			   jws.JuntaSoldaduraID,
			   jws.JuntaInspeccionVisualID,
			   jws.OrdenTrabajoSpoolID,
			   jws.UltimoProcesoID,
			   jws.EtiquetaJunta,
			   jws.JuntaFinal
		FROM JuntaWorkstatus jws
		WHERE jws.JuntaSpoolID IN
		(
			SELECT JuntaSpoolID FROM #TempJuntaSpool
		)
	
	-- Borrar las que no sean finales a menos que se quiera el historial
	DELETE FROM #TempJuntaWorkstatus WHERE JuntaFinal = 0 AND @HistorialRep = 0
				
	--INSERT JUNTA SOLDADURA
	INSERT INTO #TempJuntaSoldadura
		SELECT DISTINCT js.JuntaSoldaduraID,
						js.JuntaWorkstatusID,
						FechaSoldadura,
						FechaReporte,
						ta.Nombre [Teller],
						Wps.Nombre [Wps],
						pr.Nombre [ProcesoRelleno],
						cr.ConsumiblesRelleno,
						pra.Nombre [ProcesoRaiz],
						sra.SoldadorRaiz,
						sr.SoldadorRelleno,			   
						fm1.Nombre,
						fm2.Nombre	   
		FROM JuntaSoldadura js
		INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = js.JuntaWorkstatusID
		INNER JOIN Taller ta on ta.TallerID = js.TallerID
		INNER JOIN Wps wps on wps.WpsID =  js.WpsID
		INNER JOIN ProcesoRelleno pr on pr.ProcesoRellenoID = js.ProcesoRellenoID
		INNER JOIN ProcesoRaiz pra on pra.ProcesoRaizID = js.ProcesoRaizID
		INNER JOIN #TempJuntaSpool jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
		INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = jsp.FamiliaAceroMaterial1ID
		INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
		LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = jsp.FamiliaAceroMaterial2ID
		LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 2
			FOR XML PATH (''))),' ',', ') AS SoldadorRelleno
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sr on sr.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 1
			FOR XML PATH (''))),' ',', ') AS SoldadorRaiz
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sra on sra.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT co.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Consumible co on co.ConsumibleID = jsd1.ConsumibleID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) 
			FOR XML PATH (''))),' ',', ')  AS ConsumiblesRelleno
			FROM JuntaSoldaduraDetalle jsd
			INNER JOIN Consumible c on c.ConsumibleID = jsd.ConsumibleID
			GROUP BY jsd.JuntaSoldaduraID
		) cr on cr.JuntaSoldaduraID = js.JuntaSoldaduraID
				 
	--INSERT JUNTA INSPECCION VISUAL
	insert into #TempJuntaInspeccionVisualDefecto	
 	SELECT DISTINCT	jiv.JuntaInspeccionVisualID,
					jiv.JuntaWorkstatusID,
					jiv.FechaInspeccion [Fecha],
					iv.FechaReporte,
					iv.NumeroReporte,
					jiv.Hoja,
					jiv.Aprobado [Resultado],
					substring(d.Defecto,0,LEN(d.defecto)) as Defecto,
					jiv.Observaciones,
					jiv.FechaModificacion			   			   
	FROM JuntaInspeccionVisual jiv
	INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = jiv.JuntaWorkstatusID 
	INNER JOIN InspeccionVisual iv on iv.InspeccionVisualID = jiv.InspeccionVisualID
	LEFT JOIN(
		SELECT jivd.JuntaInspeccionVisualID, 
			   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
		FROM JuntaInspeccionVisualDefecto jivd1
		INNER JOIN Defecto d on d.DefectoID = jivd1.DefectoID
		WHERE (jivd1.JuntaInspeccionVisualID = jivd.JuntaInspeccionVisualID) 
		FOR XML PATH (''))) AS Defecto
		FROM JuntaInspeccionVisualDefecto jivd
		INNER JOIN Defecto d on d.DefectoID = jivd.DefectoID
		GROUP BY jivd.JuntaInspeccionVisualID
	) d on d.JuntaInspeccionVisualID = jiv.JuntaInspeccionVisualID
		

	INSERT INTO #TempJuntaInspeccionVisual
		SELECT JuntaInspeccionVisualID,
			   jiv.JuntaWorkstatusID,
			   Fecha,
			   FechaReporte,
			   NumeroReporte,
			   Hoja,
			   Resultado,
			   Defecto,
			   Observaciones			   
		FROM #TempJuntaInspeccionVisualDefecto jiv		
		INNER JOIN(
			SELECT MAX(FechaModificacion)as FechaModificacion,
				   MAX(Fecha) as FechaMaxima, 
				   JuntaWorkstatusID 
			FROM #TempJuntaInspeccionVisualDefecto
			GROUP BY JuntaWorkstatusID
		) e on  e.JuntaWorkstatusID = jiv.JuntaWorkstatusID 
			AND e.FechaMaxima = jiv.Fecha
			AND e.FechaModificacion = jiv.FechaModificacion
	
	--INSERT WORKSTATUS SPOOL
	INSERT INTO #TempWorkstatusSpool
		SELECT DISTINCT ws.WorkstatusSpoolID,
			   ws.OrdenTrabajoSpoolID,
			   ws.SistemaPintura,
			   ws.ColorPintura,
			   ws.CodigoPintura,
			   ws.NumeroEtiqueta,
			   ws.FechaEtiqueta,
			   ws.FechaPreparacion		  		   
		FROM WorkstatusSpool ws
		INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID	
	
	--INSERT REPORTE DIMENSIONAL
	INSERT INTO #TempReporteDimensional
		SELECT	rd.ReporteDimensionalID,
				FechaReporte,
				NumeroReporte,
				TipoReporteDimensionalID,
				FechaModificacion
		FROM ReporteDimensional rd
		WHERE EXISTS
		(
			SELECT 1 FROM ReporteDimensionalDetalle rdd
			WHERE EXISTS
			(
				SELECT 1 FROM #TempWorkstatusSpool tws
				WHERE tws.WorkstatusSpoolID = rdd.WorkstatusSpoolID
			)
			AND rdd.ReporteDimensionalID = rd.ReporteDimensionalID
		)
				
	--INSERT INSPECCION DIMENSIONAL
	INSERT INTO #TempInspeccionDimensional
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   rdd.FechaLiberacion,
			   rd.FechaModificacion
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID
		where rd.TipoReporteDimensionalID = 1
		order by rdd.FechaLiberacion desc
		
	--INSERT INSPECCION ESPESORES
	INSERT INTO #TempInspeccionEspesores
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   rdd.FechaLiberacion,
			   rdd.FechaModificacion
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID			
		where rd.TipoReporteDimensionalID = 2
		order by rdd.FechaLiberacion desc
		
	--INSERT JUNTA REPORTE PND
	INSERT INTO #TempJuntaReportePnd
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   rpnd.TipoPruebaID,
			   rpnd.FechaReporte,
			   rpnd.NumeroReporte,
			   jrpnd.JuntaRequisicionID,
			   jrpnd.FechaPrueba,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   jrpnd.Observaciones,
			   jrpnd.FechaModificacion
		FROM JuntaReportePnd jrpnd
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrpnd.JuntaWorkstatusID 
		INNER JOIN ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID
		
		
	--INSERT JUNTA REPORTE TT
	INSERT INTO #TempJuntaReporteTt
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   rtt.TipoPruebaID,
			   rtt.FechaReporte,
			   rtt.NumeroReporte,
			   jrtt.JuntaRequisicionID,
			   jrtt.Aprobado,
			   jrtt.NumeroGrafica,
			   jrtt.Hoja,
			   jrtt.FechaTratamiento,
			   jrtt.Observaciones,
			   jrtt.FechaModificacion
		FROM JuntaReporteTt jrtt
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrtt.JuntaWorkstatusID 
		INNER JOIN ReporteTt rtt on rtt.ReporteTtID = jrtt.ReporteTtID
		
		
	--INSERT PRUEBA RT
	INSERT INTO #TempPruebaRT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 1
		
	--INSERT PRUEBA PT
	INSERT INTO #TempPruebaPT 
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 2
		
	--INSERT PRUEBA RT(PostTT)
	INSERT INTO #TempPruebaRTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 5
		
	--INSERT PRUEBA PT(PostTT)
	INSERT INTO #TempPruebaPTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 6
		
	--INSERT PRUEBA UT
	INSERT INTO #TempPruebaUT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd 
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 8
		
	--INSERT TRATAMIENTO PWHT
	INSERT INTO #TempTratamientoPwht
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion			   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 3
		
	--INSERT TRATAMIENTO DUREZAS
	INSERT INTO #TempTratamientoDurezas
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion		   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 4
		
	--INSERT TRATAMIENTO PREHEAT
	INSERT INTO #TempTratamientoPreheat
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion		   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 7
					
	--INSERT PINTURA
	INSERT INTO #TempPintura
		SELECT DISTINCT ps.PinturaSpoolID,
			   ws.WorkstatusSpoolID,
			   rp.FechaRequisicion,
			   rp.NumeroRequisicion,
			   ws.PinturaSistema,
			   ws.PinturaColor,
			   ws.PinturaCodigo,
			   ps.FechaSandblast,
			   ps.ReporteSandblast,
			   ps.FechaPrimarios,
			   ps.ReportePrimarios,
			   ps.FechaIntermedios,
			   ps.ReporteIntermedios,
			   ps.FechaAcabadoVisual,
			   ps.ReporteAcabadoVisual,
			   ps.FechaAdherencia,
			   ps.ReporteAdherencia,
			   ps.FechaPullOff,
			   ps.ReportePullOff
		FROM PinturaSpool ps
		INNER JOIN #TempWorkstatusSpool ws on ps.WorkstatusSpoolID = ws.WorkstatusSpoolID
		INNER JOIN RequisicionPinturaDetalle rpd on rpd.RequisicionPinturaDetalleID = ps.RequisicionPinturaDetalleID
		INNER JOIN RequisicionPintura rp on rp.RequisicionPinturaID = rpd.RequisicionPinturaID
								
	--INSERT EMBARQUE
	INSERT INTO #TempEmbarque
			SELECT DISTINCT es.EmbarqueSpoolID,
				   ws.WorkstatusSpoolID,
				   ws.EmbarqueEtiqueta,
				   ws.FechaEtiqueta,
				   ws.FechaPreparacion,
				   e.FechaEmbarque,
				   e.NumeroEmbarque  		   
			FROM EmbarqueSpool es
			LEFT JOIN #TempWorkstatusSpool ws on es.WorkstatusSpoolID = ws.WorkstatusSpoolID
			LEFT JOIN Embarque e on e.EmbarqueID = es.EmbarqueID
			
	--INSERT GENERAL	
	INSERT INTO #TempGeneral
		SELECT jws.JuntaWorkStatusID,
			   p.Nombre,
			   ot.NumeroOrden,
			   ots.NumeroControl,
			   s.Nombre,
			   jws.EtiquetaJunta,
			   tj.Codigo,
			   js.Diametro,
			   js.Cedula,
			   js.Espesor,
			   js.EtiquetaMaterial1 + ' - ' + js.EtiquetaMaterial2,
			   up.Nombre,
			   ISNULL((select '1'
				where sh.TieneHoldCalidad = 1 or
					  sh.TieneHoldIngenieria = 1),0),
				js.FamiliaAceroMaterial1ID,
				js.FamiliaAceroMaterial2ID,
				fm1.Nombre,
				fm2.Nombre,
				js.peqs,
				js.KgTeoricos,
				ots.OrdenTrabajoSpoolID,
				js.ItemCodeIDMaterial1,
				js.CodigoItemCodeMaterial1,
				js.DescripcionItemCodeMaterial1,
				js.ItemCodeIDMaterial2,
				js.CodigoItemCodeMaterial2,
				js.DescripcionItemCodeMaterial2
			FROM #TempJuntaWorkstatus jws
			INNER JOIN #TempJuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
			INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
			INNER JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
			INNER JOIN #TempProyecto p on 1 = 1
			INNER JOIN #TempSpool s on s.SpoolID = js.SpoolID
			LEFT JOIN UltimoProceso up on up.UltimoProcesoID = jws.UltimoProcesoID
			INNER JOIN TipoJunta tj on tj.TipoJuntaID = js.TipoJuntaID
			LEFT JOIN SpoolHold sh on sh.SpoolID = s.SpoolID
			INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
			INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
			LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
			LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
			
	--DESPLEGAR TABLA
	
		select g.GeneralJuntaWorkstatusID,
			   g.GeneralProyecto,
			   g.GeneralOrdenDeTrabajo,
			   g.GeneralNumeroDeControl,
			   g.GeneralSpool,
			   g.GeneralJunta,
			   g.GeneralTipoJunta,
			   g.GeneralDiametro,
			   g.GeneralCedula,
			   g.GeneralEspesor,
			   g.GeneralLocalizacion,
			   g.GeneralUltimoProceso,
			   g.GeneralTieneHold,
			   g.GeneralFamAcero1,
			   g.GeneralFamAcero2,
			   g.CodigoItemCodeMaterial1,
			   g.DescripcionItemCodeMaterial1,
			   g.CodigoItemCodeMaterial2,
			   g.DescripcionItemCodeMaterial2,
			   g.GeneralPeqs,
			   g.GeneralKgTeoricos,
			   ja.FechaArmado [ArmadoFecha],
			   ja.FechaReporte [ArmadoFechaReporte],
			   ja.Taller [ArmadoTaller],
			   ja.Tubero [ArmadoTubero],
			   ja.NumeroUnico1 [ArmadoNumeroUnico1],
			   ja.NumeroUnico2 [ArmadoNumeroUnico2],
			   js.SoldaduraFecha,
			   js.SoldaduraFechaReporte,
			   js.SoldaduraTaller,
			   js.SoldaduraWPS,
			   js.SoldaduraProcesoRelleno,
			   js.SoldaduraConsumiblesRelleno,
			   js.SoldaduraProcesoRaiz,
			   js.SoldaduraSoldadorRaiz,
			   js.SoldaduraSoldadorRelleno,
			   js.SoldaduraMaterialBase1,
			   js.SoldaduraMaterialBase2,
			   iv.InspeccionVisualFecha,
			   iv.InspeccionVisualFechaReporte,
			   iv.InspeccionVisualNumeroReporte,   
			   iv.InspeccionVisualHoja,
			   iv.InspeccionVisualResultado,
			   iv.InspeccionVisualDefecto,
			   iv.InspeccionVisualObservaciones,
			   id.InspeccionDimensionalFecha,
			   id.InspeccionDimensionalFechaReporte,
			   id.InspeccionDimensionalNumeroReporte,
			   id.InspeccionDimensionalHoja,
			   id.InspeccionDimensionalResultado,		   
			   ie.InspeccionEspesoresFecha,
			   ie.InspeccionEspesoresFechaReporte,
			   ie.InspeccionEspesoresNumeroReporte,
			   ie.InspeccionEspesoresHoja,
			   ie.InspeccionEspesoresResultado,
			   ie.InspeccionEspesoresObservaciones,
			   prt.PruebaRTFechaRequisicion,
			   prt.PruebaRTNumeroRequisicion,
			   prt.PruebaRTCodigoRequisicion,
			   prt.PruebaRTFechaPrueba,
			   prt.PruebaRTFechaReporte,
			   prt.PruebaRTNumeroReporte,
			   prt.PruebaRTHoja,
			   prt.PruebaRTResultado,
			   prt.PruebaRTDefecto,
			   prt.PruebaRTObservacionesReporte,
			   prt.PruebaRTObservacionesRequisicion,
			   ppt.PruebaPTFechaRequisicion,
			   ppt.PruebaPTNumeroRequisicion,
			   ppt.PruebaPTCodigoRequisicion,
			   ppt.PruebaPTFechaPrueba,
			   ppt.PruebaPTFechaReporte,
			   ppt.PruebaPTNumeroReporte,
			   ppt.PruebaPTHoja,
			   ppt.PruebaPTResultado,
			   ppt.PruebaPTDefecto,
			   ppt.PruebaPTObservacionesReporte,
			   ppt.PruebaPTObservacionesRequisicion,
			   prtptt.PruebaRTPostTTFechaRequisicion,
			   prtptt.PruebaRTPostTTNumeroRequisicion,
			   prtptt.PruebaRTPostTTCodigoRequisicion,
			   prtptt.PruebaRTPostTTFechaPrueba,
			   prtptt.PruebaRTPostTTFechaReporte,
			   prtptt.PruebaRTPostTTNumeroReporte,
			   prtptt.PruebaRTPostTTHoja,
			   prtptt.PruebaRTPostTTResultado,
			   prtptt.PruebaRTPostTTDefecto,
			   prtptt.PruebaRTPostTTObservacionesReporte,
			   prtptt.PruebaRTPostTTObservacionesRequisicion,
			   pptptt.PruebaPTPostTTFechaRequisicion,
			   pptptt.PruebaPTPostTTNumeroRequisicion,
			   pptptt.PruebaPTPostTTCodigoRequisicion,
			   pptptt.PruebaPTPostTTFechaPrueba,
			   pptptt.PruebaPTPostTTFechaReporte,
			   pptptt.PruebaPTPostTTNumeroReporte,
			   pptptt.PruebaPTPostTTHoja,
			   pptptt.PruebaPTPostTTResultado,
			   pptptt.PruebaPTPostTTDefecto,
			   pptptt.PruebaPTPostTTObservacionesReporte,
			   pptptt.PruebaPTPostTTObservacionesRequisicion,
			   put.PruebaUTFechaRequisicion,
			   put.PruebaUTNumeroRequisicion,
			   put.PruebaUTCodigoRequisicion,
			   put.PruebaUTFechaPrueba,
			   put.PruebaUTFechaReporte,
			   put.PruebaUTNumeroReporte,
			   put.PruebaUTHoja,
			   put.PruebaUTResultado,
			   put.PruebaUTDefecto,
			   put.PruebaUTObservacionesReporte,
			   put.PruebaUTObservacionesRequisicion,
			   tpwht.TratamientoPwhtFechaRequisicion,
			   tpwht.TratamientoPwhtNumeroRequisicion,
			   tpwht.TratamientoPwhtCodigoRequisicion,
			   tpwht.TratamientoPwhtFechaTratamiento,
			   tpwht.TratamientoPwhtFechaReporte,
			   tpwht.TratamientoPwhtNumeroReporte,
			   tpwht.TratamientoPwhtHoja,
			   tpwht.TratamientoPwhtGrafica,
			   tpwht.TratamientoPwhtResultado,
			   tpwht.TratamientoPwhtObservacionesReporte,
			   tpwht.TratamientoPwhtObservacionesRequisicion,
			   td.TratamientoDurezasFechaRequisicion,
			   td.TratamientoDurezasNumeroRequisicion,
			   td.TratamientoDurezasCodigoRequisicion,
			   td.TratamientoDurezasFechaTratamiento,
			   td.TratamientoDurezasFechaReporte,
			   td.TratamientoDurezasNumeroReporte,
			   td.TratamientoDurezasHoja,
			   td.TratamientoDurezasGrafica,
			   td.TratamientoDurezasResultado,
			   td.TratamientoDurezasObservacionesReporte,
			   td.TratamientoDurezasObservacionesRequisicion,
			   tp.TratamientopreheatFechaRequisicion,
			   tp.TratamientopreheatNumeroRequisicion,
			   tp.TratamientopreheatCodigoRequisicion,
			   tp.TratamientopreheatFechaTratamiento,
			   tp.TratamientopreheatFechaReporte,
			   tp.TratamientopreheatNumeroReporte,
			   tp.TratamientopreheatHoja,
			   tp.TratamientopreheatGrafica,
			   tp.TratamientopreheatResultado,
			   tp.TratamientopreheatObservacionesReporte,
			   tp.TratamientopreheatObservacionesRequisicion,
			   p.PinturaFechaRequisicion,
			   p.PinturaNumeroRequisicion,
			   p.PinturaSistema,
			   p.PinturaColor,
			   p.PinturaCodigo,
			   p.PinturaFechaSendBlast,
			   p.PinturaReporteSendBlast,
			   p.PinturaFechaPrimarios,
			   p.PinturaReportePrimarios,
			   p.PinturaFechaIntermedios,
			   p.PinturaReporteIntermedios,
			   p.PinturaFechaAcabadoVisual,
			   p.PinturaReporteAcabadoVisual,
			   p.PinturaFechaAdherencia,
			   p.PinturaReporteAdherencia,
			   p.PinturaFechaPullOff,
			   p.PinturaReportePullOff,
			   e.EmbarqueEtiqueta,
			   e.EmbarqueFechaEtiqueta,
			   e.EmbarqueFechaPreparacion,
			   e.EmbarqueFechaEmbarque,
			   e.EmbarqueNumeroEmbarque
		from #TempJuntaWorkstatus jw
		INNER JOIN #TempGeneral g on g.GeneralJuntaWorkstatusID = jw.JuntaWorkStatusID
		INNER JOIN
		(
			SELECT	jarm.JuntaArmadoID,
					jarm.JuntaWorkstatusID,
					FechaArmado,
					FechaReporte,
					ta.Nombre [Taller],
					tu.Codigo [Tubero],
					NumeroUnico1ID,
					NumeroUnico2ID,
					nu1.Codigo [NumeroUnico1],
					nu2.Codigo [NumeroUnico2],
					jarm.TuberoID
			FROM JuntaArmado jarm
			INNER JOIN Taller ta on ta.TallerID = jarm.TallerID 
			INNER JOIN Tubero tu on tu.TuberoID = jarm.TuberoID
			INNER JOIN NumeroUnico nu1 on nu1.NumeroUnicoID = jarm.NumeroUnico1ID
			INNER JOIN NumeroUnico nu2 on nu2.NumeroUnicoID = jarm.NumeroUnico2ID
			WHERE jarm.JuntaWorkstatusID IN
			(
				SELECT JuntaWorkstatusID FROM #TempJuntaWorkstatus
			)
		) ja on ja.JuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempJuntaSoldadura js on js.SoldaduraJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempJuntaInspeccionVisual iv on iv.InspeccionVisualJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempWorkstatusSpool ws on ws.OrdenTrabajoSpoolID = jw.OrdenTrabajoSpoolID
		LEFT JOIN #TempPintura p on p.PinturaWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN #TempEmbarque e on e.EmbarqueWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN
		(			
			select	tmp.PruebaRTFechaRequisicion,
					tmp.PruebaRTNumeroRequisicion,
					tmp.PruebaRTCodigoRequisicion,
					tmp.PruebaRTFechaPrueba,
					tmp.PruebaRTFechaReporte,
					tmp.PruebaRTNumeroReporte,
					tmp.PruebaRTHoja,
					tmp.PruebaRTResultado,
					tmp.PruebaRTDefecto,
					tmp.PruebaRTObservacionesReporte,
					tmp.PruebaRTObservacionesRequisicion,
					tmp.PruebaRTJuntaWorkstatusID
			from #TempPruebaRT tmp
			inner join(
				select	PruebaRTJuntaWorkstatusID, 
						MAX(PruebaRTFechaPrueba) as PruebaRTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaRT
				group by PruebaRTJuntaWorkstatusID
			) A on	A.PruebaRTJuntaWorkstatusID = tmp.PruebaRTJuntaWorkstatusID
					and A.PruebaRTFechaPrueba = tmp.PruebaRTFechaPrueba
					AND A.FechaModificacion = tmp.FechaModificacion
		)  prt on prt.PruebaRTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.PruebaPTFechaRequisicion,
					tmp.PruebaPTNumeroRequisicion,
					tmp.PruebaPTCodigoRequisicion,
					tmp.PruebaPTFechaPrueba,
					tmp.PruebaPTFechaReporte,
					tmp.PruebaPTNumeroReporte,
					tmp.PruebaPTHoja,
					tmp.PruebaPTResultado,
					tmp.PruebaPTDefecto,
					tmp.PruebaPTObservacionesReporte,
					tmp.PruebaPTObservacionesRequisicion,
					tmp.PruebaPTJuntaWorkstatusID
			from #TempPruebaPT tmp
			inner join(
				select	PruebaPTJuntaWorkstatusID, 
						MAX(PruebaPTFechaPrueba) as PruebaPTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion						
				from #TempPruebaPT
				group by PruebaPTJuntaWorkstatusID
			) A on A.PruebaPTJuntaWorkstatusID = tmp.PruebaPTJuntaWorkstatusID
			and A.PruebaPTFechaPrueba = tmp.PruebaPTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
			
		)  ppt on ppt.PruebaPTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			select	tmp.PruebaRTPostTTFechaRequisicion,
					tmp.PruebaRTPostTTNumeroRequisicion,
					tmp.PruebaRTPostTTCodigoRequisicion,
					tmp.PruebaRTPostTTFechaPrueba,
					tmp.PruebaRTPostTTFechaReporte,
					tmp.PruebaRTPostTTNumeroReporte,
					tmp.PruebaRTPostTTHoja,
					tmp.PruebaRTPostTTResultado,
					tmp.PruebaRTPostTTDefecto,
					tmp.PruebaRTPostTTObservacionesReporte,
					tmp.PruebaRTPostTTObservacionesRequisicion, 
					tmp.PruebaRTPostTTJuntaWorkstatusID
			from #TempPruebaRTPostTT tmp
			inner join(
				select	PruebaRTPostTTJuntaWorkstatusID, 
						MAX(PruebaRTPostTTFechaPrueba) as PruebaRTPostTTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaRTPostTT
				group by PruebaRTPostTTJuntaWorkstatusID
			) A on A.PruebaRTPostTTJuntaWorkstatusID = tmp.PruebaRTPostTTJuntaWorkstatusID
			and A.PruebaRTPostTTFechaPrueba = tmp.PruebaRTPostTTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
		)  prtptt on prtptt.PruebaRTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(	
			select	tmp.PruebaPTPostTTFechaRequisicion,
					tmp.PruebaPTPostTTNumeroRequisicion,
					tmp.PruebaPTPostTTCodigoRequisicion,
					tmp.PruebaPTPostTTFechaPrueba,
					tmp.PruebaPTPostTTFechaReporte,
					tmp.PruebaPTPostTTNumeroReporte,
					tmp.PruebaPTPostTTHoja,
					tmp.PruebaPTPostTTResultado,
					tmp.PruebaPTPostTTDefecto,
					tmp.PruebaPTPostTTObservacionesReporte,
					tmp.PruebaPTPostTTObservacionesRequisicion,
					tmp.PruebaPTPostTTJuntaWorkstatusID
			from #TempPruebaPTPostTT tmp
			inner join(
				select	PruebaPTPostTTJuntaWorkstatusID, 
						MAX(PruebaPTPostTTFechaPrueba) as PruebaPTPostTTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaPTPostTT
				group by PruebaPTPostTTJuntaWorkstatusID
			) A on	A.PruebaPTPostTTJuntaWorkstatusID = tmp.PruebaPTPostTTJuntaWorkstatusID
					and A.PruebaPTPostTTFechaPrueba = tmp.PruebaPTPostTTFechaPrueba
					AND A.FechaModificacion = tmp.FechaModificacion
		)  pptptt on pptptt.PruebaPTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		( 
			select	tmp.PruebaUTFechaRequisicion,
					tmp.PruebaUTNumeroRequisicion,
					tmp.PruebaUTCodigoRequisicion,
					tmp.PruebaUTFechaPrueba,
					tmp.PruebaUTFechaReporte,
					tmp.PruebaUTNumeroReporte,
					tmp.PruebaUTHoja,
					tmp.PruebaUTResultado,
					tmp.PruebaUTDefecto,
					tmp.PruebaUTObservacionesReporte,
					tmp.PruebaUTObservacionesRequisicion,
					tmp.PruebaUTJuntaWorkstatusID
			from #TempPruebaUT tmp
			inner join(
				select	PruebaUTJuntaWorkstatusID, 
						MAX(PruebaUTFechaPrueba) as PruebaUTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaUT
				group by PruebaUTJuntaWorkstatusID
			) A on A.PruebaUTJuntaWorkstatusID = tmp.PruebaUTJuntaWorkstatusID
			and A.PruebaUTFechaPrueba = tmp.PruebaUTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
		)  put on put.PruebaUTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.TratamientoPwhtFechaRequisicion,
					tmp.TratamientoPwhtNumeroRequisicion,
					tmp.TratamientoPwhtCodigoRequisicion,
					tmp.TratamientoPwhtFechaTratamiento,
					tmp.TratamientoPwhtFechaReporte,
					tmp.TratamientoPwhtNumeroReporte,
					tmp.TratamientoPwhtHoja,
					tmp.TratamientoPwhtGrafica,
					tmp.TratamientoPwhtResultado,
					tmp.TratamientoPwhtObservacionesReporte,
					tmp.TratamientoPwhtObservacionesRequisicion,
					tmp.TratamientoPwhtJuntaWorkstatusID
			from #TempTratamientoPwht tmp
			inner join(
				select	TratamientoPwhtJuntaWorkstatusID, 
						MAX(TratamientoPwhtFechaTratamiento) as TratamientoPwhtFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoPwht
				group by TratamientoPwhtJuntaWorkstatusID
			) A on A.TratamientoPwhtJuntaWorkstatusID = tmp.TratamientoPwhtJuntaWorkstatusID
			and A.TratamientoPwhtFechaTratamiento = tmp.TratamientoPwhtFechaTratamiento
			AND A.FechaModificacion = tmp.FechaModificacion
		)  tpwht on tpwht.TratamientoPwhtJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.TratamientoDurezasFechaRequisicion,
					tmp.TratamientoDurezasNumeroRequisicion,
					tmp.TratamientoDurezasCodigoRequisicion,
					tmp.TratamientoDurezasFechaTratamiento,
					tmp.TratamientoDurezasFechaReporte,
					tmp.TratamientoDurezasNumeroReporte,
					tmp.TratamientoDurezasHoja,
					tmp.TratamientoDurezasGrafica,
					tmp.TratamientoDurezasResultado,
					tmp.TratamientoDurezasObservacionesReporte,
					tmp.TratamientoDurezasObservacionesRequisicion, 
					tmp.TratamientoDurezasJuntaWorkstatusID
			from #TempTratamientoDurezas tmp
			inner join(
				select	TratamientoDurezasJuntaWorkstatusID,
						MAX(TratamientoDurezasFechaTratamiento) as TratamientoDurezasFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoDurezas
				group by TratamientoDurezasJuntaWorkstatusID
			) A on A.TratamientoDurezasJuntaWorkstatusID = tmp.TratamientoDurezasJuntaWorkstatusID
			and A.TratamientoDurezasFechaTratamiento = tmp.TratamientoDurezasFechaTratamiento			
			AND A.FechaModificacion = tmp.FechaModificacion			
		) td on td.TratamientoDurezasJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			select	tmp.TratamientopreheatFechaRequisicion,
					tmp.TratamientopreheatNumeroRequisicion,
					tmp.TratamientopreheatCodigoRequisicion,
					tmp.TratamientopreheatFechaTratamiento,
					tmp.TratamientopreheatFechaReporte,
					tmp.TratamientopreheatNumeroReporte,
					tmp.TratamientopreheatHoja,
					tmp.TratamientopreheatGrafica,
					tmp.TratamientopreheatResultado,
					tmp.TratamientopreheatObservacionesReporte,
					tmp.TratamientopreheatObservacionesRequisicion,
					tmp.TratamientoPreheatJuntaWorkstatusID
			from #TempTratamientoPreheat tmp
			inner join(
				select	TratamientoPreheatJuntaWorkstatusID,
						MAX(TratamientoPreheatFechaTratamiento) as TratamientoPreheatFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoPreheat
				group by TratamientoPreheatJuntaWorkstatusID
			) A on A.TratamientoPreheatJuntaWorkstatusID = tmp.TratamientoPreheatJuntaWorkstatusID
				AND A.TratamientoPreheatFechaTratamiento = tmp.TratamientoPreheatFechaTratamiento
				AND A.FechaModificacion = tmp.FechaModificacion			
		) tp on tp.TratamientopreheatJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			SELECT	tmp.InspeccionDimensionalFecha,
					tmp.InspeccionDimensionalFechaReporte,
					tmp.InspeccionDimensionalNumeroReporte,
					tmp.InspeccionDimensionalHoja,
					tmp.InspeccionDimensionalResultado,
					tmp.InspeccionDimensionalWorkstatusSpoolID
			FROM #TempInspeccionDimensional tmp
			INNER JOIN(
				SELECT	InspeccionDimensionalWorkstatusSpoolID, 
						MAX(InspeccionDimensionalFechaLiberacion) AS InspeccionDimensionalFechaLiberacion,
						MAX(FechaModificacion) AS FechaModificacion
				FROM #TempInspeccionDimensional
				GROUP BY InspeccionDimensionalWorkstatusSpoolID
			) A ON A.InspeccionDimensionalWorkstatusSpoolID = tmp.InspeccionDimensionalWorkstatusSpoolID
				AND A.InspeccionDimensionalFechaLiberacion = tmp.InspeccionDimensionalFechaLiberacion
				AND A.FechaModificacion = tmp.FechaModificacion			
		) id on id.InspeccionDimensionalWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN
		(
			SELECT	tmp.InspeccionEspesoresFecha,
					tmp.InspeccionEspesoresFechaReporte,
					tmp.InspeccionEspesoresNumeroReporte,
					tmp.InspeccionEspesoresHoja,
					tmp.InspeccionEspesoresResultado,
					tmp.InspeccionEspesoresObservaciones,
					tmp.InspeccionEspesoresWorkstatusSpoolID
			FROM #TempInspeccionEspesores tmp
			INNER JOIN(
					SELECT InspeccionEspesoresWorkstatusSpoolID, 
						   MAX(InspeccionEspesoresFechaLiberacion) AS InspeccionEspesoresFechaLiberacion,
						   MAX(FechaModificacion) AS FechaModificacion
					FROM #TempInspeccionEspesores
					GROUP BY InspeccionEspesoresWorkstatusSpoolID
				) A ON A.InspeccionEspesoresWorkstatusSpoolID = tmp.InspeccionEspesoresWorkstatusSpoolID
					AND A.InspeccionEspesoresFechaLiberacion = tmp.InspeccionEspesoresFechaLiberacion
		) ie on ie.InspeccionEspesoresWorkstatusSpoolID = ws.WorkstatusSpoolID
		
	DROP TABLE #TempProyecto
	DROP TABLE #TempOrdenTrabajo
	DROP TABLE #TempGeneral 
	DROP TABLE #TempEmbarque 
	DROP TABLE #TempPintura 
	DROP TABLE #TempTratamientoPreheat 
	DROP TABLE #TempTratamientoDurezas 
	DROP TABLE #TempTratamientoPwht 
	DROP TABLE #TempPruebaUT 
	DROP TABLE #TempPruebaPTPostTT 
	DROP TABLE #TempPruebaRTPostTT 
	DROP TABLE #TempPruebaRT 	
	DROP TABLE #TempJuntaInspeccionVisual 
	DROP TABLE #TempWorkstatusSpool 
	DROP TABLE #TempInspeccionEspesores 
	DROP TABLE #TempInspeccionDimensional 
	DROP TABLE #TempReporteDimensional 
	DROP TABLE #TempJuntaReportePnd 	
	DROP TABLE #TempJuntaReporteTt 
	DROP TABLE #TempJuntaSoldadura 	
	DROP TABLE #TempJuntaWorkstatus 
	DROP TABLE #TempJuntaSpool 
	DROP TABLE #TempSpool 
	DROP TABLE #TempOrdenTrabajoSpool
	DROP TABLE #TempPruebaPT
	DROP TABLE #TempMaterialSpool
	DROP TABLE #TempJuntaInspeccionVisualDefecto

		
	SET NOCOUNT OFF;

END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSeguimientoDeJuntasPorProyecto]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorProyecto]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerSeguimientoDeJuntasPorProyecto
	Funcion:	Trae toda la informacion necesaria para el seguimiento de jutnas
	Parametros:	@ProyectoID INT
				@HistorialRep BIT
	Autor:		MMG
	Modificado:	03/07/2011 SACB
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorProyecto]
(
	@ProyectoID INT,
	@HistorialRep BIT
)
AS
BEGIN
	
	SET NOCOUNT ON; 

	--TABLA PROYECTO
	CREATE TABLE #TempProyecto
	(	
		ProyectoID INT,
		Patio INT,
		Nombre NVARCHAR(100)
	)
	
	--TABLA ORDEN DE TRABAJO
	CREATE TABLE #TempOrdenTrabajo
	(	
		OrdenTrabajoID INT,
		NumeroOrden NVARCHAR(50)
	)
	
	--TABLA ORDEN TRABAJO SPOOL
	CREATE TABLE #TempOrdenTrabajoSpool
	(	
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoID INT,
		NumeroControl NVARCHAR(50),
		SpoolID INT
	)
	
	--TABLA SPOOL
	CREATE TABLE #TempSpool 
	(	
		SpoolID INT,
		Nombre NVARCHAR(50)
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempJuntaSpool 
	(	
		Etiqueta NVARCHAR(10),
		JuntaSpoolID INT,
		SpoolID INT,
		TipoJuntaID INT,
		Diametro DECIMAL(7,4),
		Cedula NVARCHAR(10),
		Espesor DECIMAL(10,4),
		EtiquetaMaterial1 NVARCHAR(10),
		EtiquetaMaterial2 NVARCHAR(10),
		FamiliaAceroMaterial1ID INT,
		FamiliaAceroMaterial2ID INT,
		peqs DECIMAL(10,4),
		KgTeoricos DECIMAL(12,4),
		Etiqueta1EsNumero BIT,
		Etiqueta2EsNumero BIT,
		ValorNumericoEtiqueta1 TINYINT,
		ValorNumericoEtiqueta2 TINYINT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150)	
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempMaterialSpool
	(	
		MaterialSpoolID INT,
		SpoolID INT,
		ItemCodeID INT,
		Etiqueta NVARCHAR(10),
		CodigoItemCode NVARCHAR(50),
		DescripcionItemCode NVARCHAR(150),
		EtiquetaEsNumero BIT,
		ValorNumericoEtiqueta TINYINT
	)
	
	--TABLA JUNTA WORKSTATUS
	CREATE TABLE #TempJuntaWorkstatus 
	(	
		JuntaWorkStatusID INT,
		JuntaSpoolID INT,
		JuntaArmadoID INT,
		JuntaSoldaduraID INT,
		JuntaInspeccionVisualID INT,
		OrdenTrabajoSpoolID INT,
		UltimoProcesoID INT,
		EtiquetaJunta NVARCHAR(50),
		JuntaFinal BIT
	)
	
	--TABLA JUNTA SOLDADURA
	CREATE TABLE #TempJuntaSoldadura 
	(	
		SoldaduraJuntaSoldaduraID INT,
		SoldaduraJuntaWorkstatusID INT,
		SoldaduraFecha DATETIME,
		SoldaduraFechaReporte DATETIME,
		SoldaduraTaller NVARCHAR(50),
		SoldaduraWPS NVARCHAR(50),
		SoldaduraProcesoRelleno NVARCHAR(50),
		SoldaduraConsumiblesRelleno NVARCHAR(50),
		SoldaduraProcesoRaiz NVARCHAR(50),
		SoldaduraSoldadorRaiz NVARCHAR(50),
		SoldaduraSoldadorRelleno NVARCHAR(50),
		SoldaduraMaterialBase1 NVARCHAR(50),
		SoldaduraMaterialBase2 NVARCHAR(50)
	)
	
	--TABLA JUNTA REPORTE TT
	CREATE TABLE #TempJuntaReporteTt 
	(	
		JuntaReporteTtID INT,
		JuntaWorkstatusID INT,
		TipoPruebaID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		JuntaRequisicionID INT,
		Aprobado BIT,
		NumeroGrafica  NVARCHAR(20),
		Hoja INT,
		FechaTratamiento DATETIME,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA JUNTA REPORTE PND
	CREATE TABLE #TempJuntaReportePnd 
	(	
		JuntaReportePndID INT,
		JuntaWorkstatusID INT,
		TipoPruebaID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		JuntaRequisicionID INT,		
		FechaPrueba DATETIME,
		Hoja INT,
		Aprobado BIT,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME
	)
			
	--TABLA REPORTE DIMENSIONAL
	CREATE TABLE #TempReporteDimensional 
	(	
		ReporteDimensionalID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		TipoReporteDimensionalID INT,
		FechaModificacion DATETIME
	)
	
	--TABLA INSPECCION DIMENSIONAL
	CREATE TABLE #TempInspeccionDimensional 
	(	
		InspeccionDimensionalReporteDimensionalDetalleID INT,
		InspeccionDimensionalWorkstatusSpoolID INT,
		InspeccionDimensionalFecha DATETIME,
		InspeccionDimensionalFechaReporte DATETIME,
		InspeccionDimensionalNumeroReporte NVARCHAR(50),
		InspeccionDimensionalHoja INT,
		InspeccionDimensionalResultado BIT,
		InspeccionDimensionalObservaciones NVARCHAR(500),
		InspeccionDimensionalFechaLiberacion DATETIME,		
		FechaModificacion DATETIME
	)
	
	--TABLA INSPECCION ESPESORES
	CREATE TABLE #TempInspeccionEspesores 
	(	
		InspeccionEspesoresReporteDimensionalDetalleID INT,
		InspeccionEspesoresWorkstatusSpoolID INT,
		InspeccionEspesoresFecha DATETIME,
		InspeccionEspesoresFechaReporte DATETIME,
		InspeccionEspesoresNumeroReporte NVARCHAR(50),
		InspeccionEspesoresHoja INT,
		InspeccionEspesoresResultado BIT,
		InspeccionEspesoresObservaciones NVARCHAR(500),
		InspeccionEspesoresFechaLiberacion DATETIME,
		FechaModificacion DATETIME
	)	
	
	--TABLA WORKSTATUS SPOOL
	CREATE TABLE #TempWorkstatusSpool 
	(	
		WorkstatusSpoolID INT,
		OrdenTrabajoSpoolID INT,
		PinturaSistema NVARCHAR(50),
		PinturaColor NVARCHAR(50),
		PinturaCodigo NVARCHAR(50),
		EmbarqueEtiqueta NVARCHAR(20),
		FechaEtiqueta DATETIME,
		FechaPreparacion DATETIME		
	)
		
	--TABLA JUNTA INSPECCION VISUAL
	CREATE TABLE #TempJuntaInspeccionVisual 
	(	
		InspeccionVisualJuntaInspeccionVisualID INT,
		InspeccionVisualJuntaWorkstatusID INT,
		InspeccionVisualFecha DATETIME,
		InspeccionVisualFechaReporte DATETIME,
		InspeccionVisualNumeroReporte NVARCHAR(50),
		InspeccionVisualHoja INT,
		InspeccionVisualResultado BIT,
		InspeccionVisualDefecto NVARCHAR(MAX),
		InspeccionVisualObservaciones NVARCHAR(500)		
	)
	
	CREATE TABLE #TempJuntaInspeccionVisualDefecto
	(
		JuntaInspeccionVisualID int,
		JuntaWorkstatusID int,
		Fecha datetime,
		FechaReporte datetime,
		NumeroReporte nvarchar(50),
		Hoja int,
		Resultado bit,
		Defecto nvarchar(100),
		Observaciones nvarchar(500),
		FechaModificacion datetime
	)
	
	--TABLA PRUEBA RT
	CREATE TABLE #TempPruebaRT 
	(	
		PruebaRTJuntaReportePndID INT,
		PruebaRTJuntaWorkstatusID INT,
		PruebaRTFechaRequisicion DATETIME,
		PruebaRTNumeroRequisicion NVARCHAR(50),
		PruebaRTCodigoRequisicion NVARCHAR(50),
		PruebaRTFechaPrueba DATETIME,
		PruebaRTFechaReporte DATETIME,
		PruebaRTNumeroReporte NVARCHAR(50),
		PruebaRTHoja INT,
		PruebaRTResultado BIT,
		PruebaRTDefecto NVARCHAR(MAX),
		PruebaRTObservacionesReporte NVARCHAR(500),
		PruebaRTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA PT
	CREATE TABLE #TempPruebaPT 
	(	
		PruebaPTJuntaReportePndID INT,
		PruebaPTJuntaWorkstatusID INT,
		PruebaPTFechaRequisicion DATETIME,
		PruebaPTNumeroRequisicion NVARCHAR(50),
		PruebaPTCodigoRequisicion NVARCHAR(50),
		PruebaPTFechaPrueba DATETIME,
		PruebaPTFechaReporte DATETIME,
		PruebaPTNumeroReporte NVARCHAR(50),
		PruebaPTHoja INT,
		PruebaPTResultado BIT,
		PruebaPTDefecto NVARCHAR(MAX),
		PruebaPTObservacionesReporte NVARCHAR(500),
		PruebaPTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA RT(PostTT)
	CREATE TABLE #TempPruebaRTPostTT 
	(	
		PruebaRTPostTTJuntaReportePndID INT,
		PruebaRTPostTTJuntaWorkstatusID INT,
		PruebaRTPostTTFechaRequisicion DATETIME,
		PruebaRTPostTTNumeroRequisicion NVARCHAR(50),
		PruebaRTPostTTCodigoRequisicion NVARCHAR(50),
		PruebaRTPostTTFechaPrueba DATETIME,
		PruebaRTPostTTFechaReporte DATETIME,
		PruebaRTPostTTNumeroReporte NVARCHAR(50),
		PruebaRTPostTTHoja INT,
		PruebaRTPostTTResultado BIT,
		PruebaRTPostTTDefecto NVARCHAR(MAX),
		PruebaRTPostTTObservacionesReporte NVARCHAR(500),
		PruebaRTPostTTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA PT(PostTT)
	CREATE TABLE #TempPruebaPTPostTT 
	(	
		PruebaPTPostTTJuntaReportePndID INT,
		PruebaPTPostTTJuntaWorkstatusID INT,
		PruebaPTPostTTFechaRequisicion DATETIME,
		PruebaPTPostTTNumeroRequisicion NVARCHAR(50),
		PruebaPTPostTTCodigoRequisicion NVARCHAR(50),
		PruebaPTPostTTFechaPrueba DATETIME,
		PruebaPTPostTTFechaReporte DATETIME,
		PruebaPTPostTTNumeroReporte NVARCHAR(50),
		PruebaPTPostTTHoja INT,
		PruebaPTPostTTResultado BIT,
		PruebaPTPostTTDefecto NVARCHAR(MAX),
		PruebaPTPostTTObservacionesReporte NVARCHAR(500),
		PruebaPTPostTTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PRUEBA UT
	CREATE TABLE #TempPruebaUT 
	(	
		PruebaUTJuntaReportePndID INT,
		PruebaUTJuntaWorkstatusID INT,
		PruebaUTFechaRequisicion DATETIME,
		PruebaUTNumeroRequisicion NVARCHAR(50),
		PruebaUTCodigoRequisicion NVARCHAR(50),
		PruebaUTFechaPrueba DATETIME,
		PruebaUTFechaReporte DATETIME,
		PruebaUTNumeroReporte NVARCHAR(50),
		PruebaUTHoja INT,
		PruebaUTResultado BIT,
		PruebaUTDefecto NVARCHAR(MAX),
		PruebaUTObservacionesReporte NVARCHAR(500),
		PruebaUTObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA TRATAMIENTO PWHT
	CREATE TABLE #TempTratamientoPwht 
	(	
		TratamientoPwhtJuntaReporteTtID INT,
		TratamientoPwhtJuntaWorkstatusID INT,
		TratamientoPwhtFechaRequisicion DATETIME,
		TratamientoPwhtNumeroRequisicion NVARCHAR(50),
		TratamientoPwhtCodigoRequisicion NVARCHAR(50),
		TratamientoPwhtFechaTratamiento DATETIME,
		TratamientoPwhtFechaReporte DATETIME,
		TratamientoPwhtNumeroReporte NVARCHAR(50),
		TratamientoPwhtHoja INT,
		TratamientoPwhtGrafica NVARCHAR(20),
		TratamientoPwhtResultado BIT,
		TratamientoPwhtObservacionesReporte NVARCHAR(500),
		TratamientoPwhtObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME		
	)
	
	--TABLA TRATAMIENTO DUREZAS
	CREATE TABLE #TempTratamientoDurezas 
	(	
		TratamientoDurezasJuntaReporteTtID INT,
		TratamientoDurezasJuntaWorkstatusID INT,
		TratamientoDurezasFechaRequisicion DATETIME,
		TratamientoDurezasNumeroRequisicion NVARCHAR(50),
		TratamientoDurezasCodigoRequisicion NVARCHAR(50),
		TratamientoDurezasFechaTratamiento DATETIME,
		TratamientoDurezasFechaReporte DATETIME,
		TratamientoDurezasNumeroReporte NVARCHAR(20),
		TratamientoDurezasHoja INT,
		TratamientoDurezasGrafica NVARCHAR(20),
		TratamientoDurezasResultado BIT,
		TratamientoDurezasObservacionesReporte NVARCHAR(500),
		TratamientoDurezasObservacionesRequisicion NVARCHAR(500),		
		FechaModificacion DATETIME
	)
	
	--TABLA TRATAMIENTO PREHEAT
	CREATE TABLE #TempTratamientoPreheat 
	(	
		TratamientoPreheatJuntaReporteTtID INT,
		TratamientoPreheatJuntaWorkstatusID INT,
		TratamientoPreheatFechaRequisicion DATETIME,
		TratamientoPreheatNumeroRequisicion NVARCHAR(50),
		TratamientoPreheatCodigoRequisicion NVARCHAR(50),
		TratamientoPreheatFechaTratamiento DATETIME,
		TratamientoPreheatFechaReporte DATETIME,
		TratamientoPreheatNumeroReporte NVARCHAR(50),
		TratamientoPreheatHoja INT,
		TratamientoPreheatGrafica NVARCHAR(20),
		TratamientoPreheatResultado BIT,
		TratamientoPreheatObservacionesReporte NVARCHAR(500),
		TratamientoPreheatObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)
	
	--TABLA PINTURA
	CREATE TABLE #TempPintura 
	(	
		PinturaPinturaSpoolID INT,
		PinturaWorkstatusSpoolID INT,
		PinturaFechaRequisicion DATETIME,
		PinturaNumeroRequisicion NVARCHAR(50),
		PinturaSistema NVARCHAR(50),
		PinturaColor NVARCHAR(50),
		PinturaCodigo NVARCHAR(50),
		PinturaFechaSendBlast DATETIME,
		PinturaReporteSendBlast NVARCHAR(50),
		PinturaFechaPrimarios DATETIME,
		PinturaReportePrimarios NVARCHAR(50),
		PinturaFechaIntermedios DATETIME,
		PinturaReporteIntermedios NVARCHAR(50),
		PinturaFechaAcabadoVisual DATETIME,
		PinturaReporteAcabadoVisual NVARCHAR(50),
		PinturaFechaAdherencia DATETIME,
		PinturaReporteAdherencia NVARCHAR(50),
		PinturaFechaPullOff DATETIME,
		PinturaReportePullOff NVARCHAR(50)
	)
	
	--TABLA EMBARQUE
	CREATE TABLE #TempEmbarque 
	(
		EmbarqueEmbarqueSpoolID INT,
		EmbarqueWorkstatusSpoolID INT,
		EmbarqueEtiqueta NVARCHAR(20),
		EmbarqueFechaEtiqueta DATETIME,
		EmbarqueFechaPreparacion DATETIME,
		EmbarqueFechaEmbarque DATETIME,
		EmbarqueNumeroEmbarque NVARCHAR(50)
	)
	
	--TABLA GENERAL
	CREATE TABLE #TempGeneral 
	(
		GeneralJuntaWorkstatusID INT,
		GeneralProyecto NVARCHAR(50),
		GeneralOrdenDeTrabajo NVARCHAR(50),
		GeneralNumeroDeControl NVARCHAR(50),
		GeneralSpool NVARCHAR(50),
		GeneralJunta NVARCHAR(50),
		GeneralTipoJunta NVARCHAR(50),
		GeneralDiametro DECIMAL(7,4),
		GeneralCedula NVARCHAR(10),
		GeneralEspesor DECIMAL(10,4),
		GeneralLocalizacion NVARCHAR(50),
		GeneralUltimoProceso NVARCHAR(50),
		GeneralTieneHold BIT,
		GeneralFamiliaAceroMaterial1ID INT,
		GeneralFamiliaAceroMaterial2ID INT,
		GeneralFamAcero1 NVARCHAR(50),
		GeneralFamAcero2 NVARCHAR(50),
		GeneralPeqs DECIMAL(10,4),
		GeneralKgTeoricos DECIMAL(12,4),
		OrdenTrabajoSpoolID INT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150)	
	)
	
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_JuntaWorkstatusID ON #TempJuntaWorkstatus(JuntaWorkStatusID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_OrdenTrabajoSpoolID ON #TempJuntaWorkstatus(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaSoldadura_SoldaduraJuntaWorkstatusID ON #TempJuntaSoldadura(SoldaduraJuntaWorkstatusID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaInspeccionVisual_InspeccionVisualJuntaWorkstatusID ON #TempJuntaInspeccionVisual(InspeccionVisualJuntaWorkstatusID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_OrdenTrabajoSpoolID ON #TempWorkstatusSpool(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_WorkstatusSpoolID ON #TempWorkstatusSpool(WorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempPintura_PinturaWorkstatusSpoolID ON #TempPintura(PinturaWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempEmbarque_EmbarqueWorkstatusSpoolID ON #TempEmbarque(EmbarqueWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempGeneral_GeneralJuntaWorkstatusID ON #TempGeneral(GeneralJuntaWorkstatusID)

	--Comienzan los inserts a las tablas temporales con filtros si los tienen.
	
	--INSERT PROYECTO
	INSERT INTO #TempProyecto
		SELECT ProyectoID,
			   PatioID,
			   Nombre
		FROM Proyecto
		WHERE ProyectoID = @ProyectoID

	-- INSERT A ODT	
	INSERT INTO #TempOrdenTrabajo
		SELECT OrdenTrabajoID,
			   ot.NumeroOrden
		FROM OrdenTrabajo ot
		WHERE ProyectoID = @ProyectoID

	
	--INSERT ORDEN TRABAJO SPOOL
	INSERT INTO #TempOrdenTrabajoSpool
		SELECT OrdenTrabajoSpoolID,
			   ots.OrdenTrabajoID,
			   ots.NumeroControl,
			   ots.SpoolID
		FROM OrdenTrabajoSpool ots
		WHERE ots.OrdenTrabajoID IN
		(
			SELECT OrdenTrabajoID FROM #TempOrdenTrabajo
		)
	
	--INSERT SPOOL
	INSERT INTO #TempSpool
		SELECT SpoolID,
			   Nombre
		FROM Spool
		WHERE SpoolID IN
		(
			SELECT SpoolID FROM #TempOrdenTrabajoSpool
		)
	
	--INSERT Material Spool
	INSERT INTO #TempMaterialSpool
	(
		SpoolID, 
		MaterialSpoolID, 
		ItemCodeID, 
		CodigoItemCode, 
		DescripcionItemCode, 
		Etiqueta, 
		EtiquetaEsNumero, 
		ValorNumericoEtiqueta
	)
	SELECT	ms.SpoolID,
			ms.MaterialSpoolID,
			ms.ItemCodeID,
			ic.Codigo,
			ic.DescripcionEspanol,
			ms.Etiqueta,
			CAST(ISNUMERIC(ms.Etiqueta) AS BIT),
			CASE WHEN ISNUMERIC(ms.Etiqueta) = 1 THEN CAST(ms.Etiqueta AS TINYINT) ELSE NULL END
	FROM MaterialSpool ms
	INNER JOIN ItemCode ic on ms.ItemCodeID = ic.ItemCodeID
	WHERE ms.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)
	
	--INSERT JUNTA SPOOL
	INSERT INTO #TempJuntaSpool
	(
		Etiqueta,
		JuntaSpoolID,
		SpoolID,
		TipoJuntaID,
		Diametro,
		Cedula,
		Espesor,
		EtiquetaMaterial1,
		EtiquetaMaterial2,
		FamiliaAceroMaterial1ID,
		FamiliaAceroMaterial2ID,
		peqs,
		KgTeoricos,
		Etiqueta1EsNumero,
		Etiqueta2EsNumero,
		ValorNumericoEtiqueta1,
		ValorNumericoEtiqueta2,
		ItemCodeIDMaterial1,
		CodigoItemCodeMaterial1,
		DescripcionItemCodeMaterial1,
		ItemCodeIDMaterial2,
		CodigoItemCodeMaterial2,
		DescripcionItemCodeMaterial2
	)
	SELECT js.Etiqueta,
		  JuntaSpoolID,
		  js.SpoolID,
		  js.TipoJuntaID,
		  js.Diametro,
		  js.Cedula,
		  js.Espesor,
		  js.EtiquetaMaterial1,
		  js.EtiquetaMaterial2,
		  js.FamiliaAceroMaterial1ID,
		  js.FamiliaAceroMaterial2ID,
		  js.Peqs,
		  js.KgTeoricos,
		  CAST(ISNUMERIC(js.EtiquetaMaterial1) AS BIT),
		  CAST(ISNUMERIC(js.EtiquetaMaterial2) AS BIT),
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS TINYINT) ELSE NULL END,
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS TINYINT) ELSE NULL END,
		  ms1.ItemCodeID,
		  ms1.CodigoItemCode,
		  ms1.DescripcionItemCode,
		  ms2.ItemCodeID,
		  ms2.CodigoItemCode,
		  ms2.DescripcionItemCode
	FROM JuntaSpool js
	LEFT JOIN #TempMaterialSpool ms1 on js.SpoolID = ms1.SpoolID and (ms1.Etiqueta = js.EtiquetaMaterial1 or ms1.ValorNumericoEtiqueta = (CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS TINYINT) ELSE NULL END))
	LEFT JOIN #TempMaterialSpool ms2 on js.SpoolID = ms2.SpoolID and (ms2.Etiqueta = js.EtiquetaMaterial2 or ms2.ValorNumericoEtiqueta = (CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS TINYINT) ELSE NULL END))
	WHERE js.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)
	
	--INSERT JUNTA WORKSTATUS
	INSERT INTO #TempJuntaWorkstatus
		SELECT jws.JuntaWorkstatusID,
			   jws.JuntaSpoolID,
			   jws.JuntaArmadoID,
			   jws.JuntaSoldaduraID,
			   jws.JuntaInspeccionVisualID,
			   jws.OrdenTrabajoSpoolID,
			   jws.UltimoProcesoID,
			   jws.EtiquetaJunta,
			   jws.JuntaFinal
		FROM JuntaWorkstatus jws
		WHERE jws.JuntaSpoolID IN
		(
			SELECT JuntaSpoolID FROM #TempJuntaSpool
		)
	
	-- Borrar las que no sean finales a menos que se quiera el historial
	DELETE FROM #TempJuntaWorkstatus WHERE JuntaFinal = 0 AND @HistorialRep = 0
				
	--INSERT JUNTA SOLDADURA
	INSERT INTO #TempJuntaSoldadura
		SELECT DISTINCT js.JuntaSoldaduraID,
						js.JuntaWorkstatusID,
						FechaSoldadura,
						FechaReporte,
						ta.Nombre [Teller],
						Wps.Nombre [Wps],
						pr.Nombre [ProcesoRelleno],
						cr.ConsumiblesRelleno,
						pra.Nombre [ProcesoRaiz],
						sra.SoldadorRaiz,
						sr.SoldadorRelleno,			   
						fm1.Nombre,
						fm2.Nombre	   
		FROM JuntaSoldadura js
		INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = js.JuntaWorkstatusID
		INNER JOIN Taller ta on ta.TallerID = js.TallerID
		INNER JOIN Wps wps on wps.WpsID =  js.WpsID
		INNER JOIN ProcesoRelleno pr on pr.ProcesoRellenoID = js.ProcesoRellenoID
		INNER JOIN ProcesoRaiz pra on pra.ProcesoRaizID = js.ProcesoRaizID
		INNER JOIN #TempJuntaSpool jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
		INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = jsp.FamiliaAceroMaterial1ID
		INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
		LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = jsp.FamiliaAceroMaterial2ID
		LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 2
			FOR XML PATH (''))),' ',', ') AS SoldadorRelleno
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sr on sr.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 1
			FOR XML PATH (''))),' ',', ') AS SoldadorRaiz
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sra on sra.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT co.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Consumible co on co.ConsumibleID = jsd1.ConsumibleID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) 
			FOR XML PATH (''))),' ',', ')  AS ConsumiblesRelleno
			FROM JuntaSoldaduraDetalle jsd
			INNER JOIN Consumible c on c.ConsumibleID = jsd.ConsumibleID
			GROUP BY jsd.JuntaSoldaduraID
		) cr on cr.JuntaSoldaduraID = js.JuntaSoldaduraID
				 
	--INSERT JUNTA INSPECCION VISUAL
	insert into #TempJuntaInspeccionVisualDefecto	
 	SELECT DISTINCT	jiv.JuntaInspeccionVisualID,
					jiv.JuntaWorkstatusID,
					jiv.FechaInspeccion [Fecha],
					iv.FechaReporte,
					iv.NumeroReporte,
					jiv.Hoja,
					jiv.Aprobado [Resultado],
					substring(d.Defecto,0,LEN(d.defecto)) as Defecto,
					jiv.Observaciones,
					jiv.FechaModificacion			   			   
	FROM JuntaInspeccionVisual jiv
	INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = jiv.JuntaWorkstatusID 
	INNER JOIN InspeccionVisual iv on iv.InspeccionVisualID = jiv.InspeccionVisualID
	LEFT JOIN(
		SELECT jivd.JuntaInspeccionVisualID, 
			   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
		FROM JuntaInspeccionVisualDefecto jivd1
		INNER JOIN Defecto d on d.DefectoID = jivd1.DefectoID
		WHERE (jivd1.JuntaInspeccionVisualID = jivd.JuntaInspeccionVisualID) 
		FOR XML PATH (''))) AS Defecto
		FROM JuntaInspeccionVisualDefecto jivd
		INNER JOIN Defecto d on d.DefectoID = jivd.DefectoID
		GROUP BY jivd.JuntaInspeccionVisualID
	) d on d.JuntaInspeccionVisualID = jiv.JuntaInspeccionVisualID
		

	INSERT INTO #TempJuntaInspeccionVisual
		SELECT JuntaInspeccionVisualID,
			   jiv.JuntaWorkstatusID,
			   Fecha,
			   FechaReporte,
			   NumeroReporte,
			   Hoja,
			   Resultado,
			   Defecto,
			   Observaciones			   
		FROM #TempJuntaInspeccionVisualDefecto jiv		
		INNER JOIN(
			SELECT MAX(FechaModificacion)as FechaModificacion,
				   MAX(Fecha) as FechaMaxima, 
				   JuntaWorkstatusID 
			FROM #TempJuntaInspeccionVisualDefecto
			GROUP BY JuntaWorkstatusID
		) e on  e.JuntaWorkstatusID = jiv.JuntaWorkstatusID 
			AND e.FechaMaxima = jiv.Fecha
			AND e.FechaModificacion = jiv.FechaModificacion
	
	--INSERT REPORTE DIMENSIONAL
	INSERT INTO #TempReporteDimensional
		SELECT DISTINCT ReporteDimensionalID,
			   FechaReporte,
			   NumeroReporte,
			   TipoReporteDimensionalID,
			   FechaModificacion
		FROM ReporteDimensional
		WHERE ProyectoID = @ProyectoID 
				
	--INSERT INSPECCION DIMENSIONAL
	INSERT INTO #TempInspeccionDimensional
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   rdd.FechaLiberacion,
			   rd.FechaModificacion
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID
		where rd.TipoReporteDimensionalID = 1
		order by rdd.FechaLiberacion desc
		
	--INSERT INSPECCION ESPESORES
	INSERT INTO #TempInspeccionEspesores
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   rdd.FechaLiberacion,
			   rdd.FechaModificacion
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID			
		where rd.TipoReporteDimensionalID = 2
		order by rdd.FechaLiberacion desc
		
	--INSERT JUNTA REPORTE PND
	INSERT INTO #TempJuntaReportePnd
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   rpnd.TipoPruebaID,
			   rpnd.FechaReporte,
			   rpnd.NumeroReporte,
			   jrpnd.JuntaRequisicionID,
			   jrpnd.FechaPrueba,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   jrpnd.Observaciones,
			   jrpnd.FechaModificacion
		FROM JuntaReportePnd jrpnd
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrpnd.JuntaWorkstatusID 
		INNER JOIN ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID
		where rpnd.ProyectoID = @ProyectoID
		
	--INSERT JUNTA REPORTE TT
	INSERT INTO #TempJuntaReporteTt
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   rtt.TipoPruebaID,
			   rtt.FechaReporte,
			   rtt.NumeroReporte,
			   jrtt.JuntaRequisicionID,
			   jrtt.Aprobado,
			   jrtt.NumeroGrafica,
			   jrtt.Hoja,
			   jrtt.FechaTratamiento,
			   jrtt.Observaciones,
			   jrtt.FechaModificacion
		FROM JuntaReporteTt jrtt
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrtt.JuntaWorkstatusID 
		INNER JOIN ReporteTt rtt on rtt.ReporteTtID = jrtt.ReporteTtID
		where rtt.ProyectoID = @ProyectoID
		
	--INSERT PRUEBA RT
	INSERT INTO #TempPruebaRT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 1
		
	--INSERT PRUEBA PT
	INSERT INTO #TempPruebaPT 
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 2
		
	--INSERT PRUEBA RT(PostTT)
	INSERT INTO #TempPruebaRTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 5
		
	--INSERT PRUEBA PT(PostTT)
	INSERT INTO #TempPruebaPTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 6
		
	--INSERT PRUEBA UT
	INSERT INTO #TempPruebaUT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jrpnd.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM #TempJuntaReportePnd jrpnd 
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaReportePndSector jrpnds1
			INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
			WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE jrpnd.TipoPruebaID = 8
		
	--INSERT TRATAMIENTO PWHT
	INSERT INTO #TempTratamientoPwht
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion			   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 3
		
	--INSERT TRATAMIENTO DUREZAS
	INSERT INTO #TempTratamientoDurezas
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion		   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 4
		
	--INSERT TRATAMIENTO PREHEAT
	INSERT INTO #TempTratamientoPreheat
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   r.Observaciones,
			   jrtt.FechaModificacion		   
		FROM #TempJuntaReporteTt jrtt
		INNER JOIN JuntaRequisicion jr on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		WHERE jrtt.TipoPruebaID = 7
					
	--INSERT WORKSTATUS SPOOL
	INSERT INTO #TempWorkstatusSpool
		SELECT DISTINCT ws.WorkstatusSpoolID,
			   ws.OrdenTrabajoSpoolID,
			   ws.SistemaPintura,
			   ws.ColorPintura,
			   ws.CodigoPintura,
			   ws.NumeroEtiqueta,
			   ws.FechaEtiqueta,
			   ws.FechaPreparacion		  		   
		FROM WorkstatusSpool ws
		INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID	
	
	--INSERT PINTURA
	INSERT INTO #TempPintura
		SELECT DISTINCT ps.PinturaSpoolID,
			   ws.WorkstatusSpoolID,
			   rp.FechaRequisicion,
			   rp.NumeroRequisicion,
			   ws.PinturaSistema,
			   ws.PinturaColor,
			   ws.PinturaCodigo,
			   ps.FechaSandblast,
			   ps.ReporteSandblast,
			   ps.FechaPrimarios,
			   ps.ReportePrimarios,
			   ps.FechaIntermedios,
			   ps.ReporteIntermedios,
			   ps.FechaAcabadoVisual,
			   ps.ReporteAcabadoVisual,
			   ps.FechaAdherencia,
			   ps.ReporteAdherencia,
			   ps.FechaPullOff,
			   ps.ReportePullOff
		FROM PinturaSpool ps
		INNER JOIN #TempWorkstatusSpool ws on ps.WorkstatusSpoolID = ws.WorkstatusSpoolID
		INNER JOIN RequisicionPinturaDetalle rpd on rpd.RequisicionPinturaDetalleID = ps.RequisicionPinturaDetalleID
		INNER JOIN RequisicionPintura rp on rp.RequisicionPinturaID = rpd.RequisicionPinturaID
								
	--INSERT EMBARQUE
	INSERT INTO #TempEmbarque
			SELECT DISTINCT es.EmbarqueSpoolID,
				   ws.WorkstatusSpoolID,
				   ws.EmbarqueEtiqueta,
				   ws.FechaEtiqueta,
				   ws.FechaPreparacion,
				   e.FechaEmbarque,
				   e.NumeroEmbarque  		   
			FROM EmbarqueSpool es
			LEFT JOIN #TempWorkstatusSpool ws on es.WorkstatusSpoolID = ws.WorkstatusSpoolID
			LEFT JOIN Embarque e on e.EmbarqueID = es.EmbarqueID
			
	--INSERT GENERAL	
	INSERT INTO #TempGeneral
		SELECT jws.JuntaWorkStatusID,
			   p.Nombre,
			   ot.NumeroOrden,
			   ots.NumeroControl,
			   s.Nombre,
			   ISNULL(jws.EtiquetaJunta,js.Etiqueta) ,
			   tj.Codigo,
			   js.Diametro,
			   js.Cedula,
			   js.Espesor,
			   js.EtiquetaMaterial1 + ' - ' + js.EtiquetaMaterial2,
			   up.Nombre,
			   ISNULL((select '1'
				where sh.TieneHoldCalidad = 1 or
					  sh.TieneHoldIngenieria = 1),0),
				js.FamiliaAceroMaterial1ID,
				js.FamiliaAceroMaterial2ID,
				fm1.Nombre,
				fm2.Nombre,
				js.peqs,
				js.KgTeoricos,
				ots.OrdenTrabajoSpoolID,
				js.ItemCodeIDMaterial1,
				js.CodigoItemCodeMaterial1,
				js.DescripcionItemCodeMaterial1,
				js.ItemCodeIDMaterial2,
				js.CodigoItemCodeMaterial2,
				js.DescripcionItemCodeMaterial2
			FROM #TempJuntaSpool js
			LEFT JOIN #TempJuntaWorkstatus jws on js.JuntaSpoolID = jws.JuntaSpoolID
			INNER JOIN #TempProyecto p on p.ProyectoID = @ProyectoID
			LEFT JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
			LEFT JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
			INNER JOIN #TempSpool s on s.SpoolID = js.SpoolID
			LEFT JOIN UltimoProceso up on up.UltimoProcesoID = jws.UltimoProcesoID
			INNER JOIN TipoJunta tj on tj.TipoJuntaID = js.TipoJuntaID
			LEFT JOIN SpoolHold sh on sh.SpoolID = s.SpoolID
			INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
			INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
			LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
			LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
			
	--DESPLEGAR TABLA
	
		select g.GeneralJuntaWorkstatusID,
			   g.GeneralProyecto,
			   g.GeneralOrdenDeTrabajo,
			   g.GeneralNumeroDeControl,
			   g.GeneralSpool,
			   g.GeneralJunta,
			   g.GeneralTipoJunta,
			   g.GeneralDiametro,
			   g.GeneralCedula,
			   g.GeneralEspesor,
			   g.GeneralLocalizacion,
			   g.GeneralUltimoProceso,
			   g.GeneralTieneHold,
			   g.GeneralFamAcero1,
			   g.GeneralFamAcero2,
			   g.CodigoItemCodeMaterial1,
			   g.DescripcionItemCodeMaterial1,
			   g.CodigoItemCodeMaterial2,
			   g.DescripcionItemCodeMaterial2,
			   g.GeneralPeqs,
			   g.GeneralKgTeoricos,
			   ja.FechaArmado [ArmadoFecha],
			   ja.FechaReporte [ArmadoFechaReporte],
			   ja.Taller [ArmadoTaller],
			   ja.Tubero [ArmadoTubero],
			   ja.NumeroUnico1 [ArmadoNumeroUnico1],
			   ja.NumeroUnico2 [ArmadoNumeroUnico2],
			   js.SoldaduraFecha,
			   js.SoldaduraFechaReporte,
			   js.SoldaduraTaller,
			   js.SoldaduraWPS,
			   js.SoldaduraProcesoRelleno,
			   js.SoldaduraConsumiblesRelleno,
			   js.SoldaduraProcesoRaiz,
			   js.SoldaduraSoldadorRaiz,
			   js.SoldaduraSoldadorRelleno,
			   js.SoldaduraMaterialBase1,
			   js.SoldaduraMaterialBase2,
			   iv.InspeccionVisualFecha,
			   iv.InspeccionVisualFechaReporte,
			   iv.InspeccionVisualNumeroReporte,   
			   iv.InspeccionVisualHoja,
			   iv.InspeccionVisualResultado,
			   iv.InspeccionVisualDefecto,
			   iv.InspeccionVisualObservaciones,
			   id.InspeccionDimensionalFecha,
			   id.InspeccionDimensionalFechaReporte,
			   id.InspeccionDimensionalNumeroReporte,
			   id.InspeccionDimensionalHoja,
			   id.InspeccionDimensionalResultado,		   
			   ie.InspeccionEspesoresFecha,
			   ie.InspeccionEspesoresFechaReporte,
			   ie.InspeccionEspesoresNumeroReporte,
			   ie.InspeccionEspesoresHoja,
			   ie.InspeccionEspesoresResultado,
			   ie.InspeccionEspesoresObservaciones,
			   prt.PruebaRTFechaRequisicion,
			   prt.PruebaRTNumeroRequisicion,
			   prt.PruebaRTCodigoRequisicion,
			   prt.PruebaRTFechaPrueba,
			   prt.PruebaRTFechaReporte,
			   prt.PruebaRTNumeroReporte,
			   prt.PruebaRTHoja,
			   prt.PruebaRTResultado,
			   prt.PruebaRTDefecto,
			   prt.PruebaRTObservacionesReporte,
			   prt.PruebaRTObservacionesRequisicion,
			   ppt.PruebaPTFechaRequisicion,
			   ppt.PruebaPTNumeroRequisicion,
			   ppt.PruebaPTCodigoRequisicion,
			   ppt.PruebaPTFechaPrueba,
			   ppt.PruebaPTFechaReporte,
			   ppt.PruebaPTNumeroReporte,
			   ppt.PruebaPTHoja,
			   ppt.PruebaPTResultado,
			   ppt.PruebaPTDefecto,
			   ppt.PruebaPTObservacionesReporte,
			   ppt.PruebaPTObservacionesRequisicion,
			   prtptt.PruebaRTPostTTFechaRequisicion,
			   prtptt.PruebaRTPostTTNumeroRequisicion,
			   prtptt.PruebaRTPostTTCodigoRequisicion,
			   prtptt.PruebaRTPostTTFechaPrueba,
			   prtptt.PruebaRTPostTTFechaReporte,
			   prtptt.PruebaRTPostTTNumeroReporte,
			   prtptt.PruebaRTPostTTHoja,
			   prtptt.PruebaRTPostTTResultado,
			   prtptt.PruebaRTPostTTDefecto,
			   prtptt.PruebaRTPostTTObservacionesReporte,
			   prtptt.PruebaRTPostTTObservacionesRequisicion,
			   pptptt.PruebaPTPostTTFechaRequisicion,
			   pptptt.PruebaPTPostTTNumeroRequisicion,
			   pptptt.PruebaPTPostTTCodigoRequisicion,
			   pptptt.PruebaPTPostTTFechaPrueba,
			   pptptt.PruebaPTPostTTFechaReporte,
			   pptptt.PruebaPTPostTTNumeroReporte,
			   pptptt.PruebaPTPostTTHoja,
			   pptptt.PruebaPTPostTTResultado,
			   pptptt.PruebaPTPostTTDefecto,
			   pptptt.PruebaPTPostTTObservacionesReporte,
			   pptptt.PruebaPTPostTTObservacionesRequisicion,
			   put.PruebaUTFechaRequisicion,
			   put.PruebaUTNumeroRequisicion,
			   put.PruebaUTCodigoRequisicion,
			   put.PruebaUTFechaPrueba,
			   put.PruebaUTFechaReporte,
			   put.PruebaUTNumeroReporte,
			   put.PruebaUTHoja,
			   put.PruebaUTResultado,
			   put.PruebaUTDefecto,
			   put.PruebaUTObservacionesReporte,
			   put.PruebaUTObservacionesRequisicion,
			   tpwht.TratamientoPwhtFechaRequisicion,
			   tpwht.TratamientoPwhtNumeroRequisicion,
			   tpwht.TratamientoPwhtCodigoRequisicion,
			   tpwht.TratamientoPwhtFechaTratamiento,
			   tpwht.TratamientoPwhtFechaReporte,
			   tpwht.TratamientoPwhtNumeroReporte,
			   tpwht.TratamientoPwhtHoja,
			   tpwht.TratamientoPwhtGrafica,
			   tpwht.TratamientoPwhtResultado,
			   tpwht.TratamientoPwhtObservacionesReporte,
			   tpwht.TratamientoPwhtObservacionesRequisicion,
			   td.TratamientoDurezasFechaRequisicion,
			   td.TratamientoDurezasNumeroRequisicion,
			   td.TratamientoDurezasCodigoRequisicion,
			   td.TratamientoDurezasFechaTratamiento,
			   td.TratamientoDurezasFechaReporte,
			   td.TratamientoDurezasNumeroReporte,
			   td.TratamientoDurezasHoja,
			   td.TratamientoDurezasGrafica,
			   td.TratamientoDurezasResultado,
			   td.TratamientoDurezasObservacionesReporte,
			   td.TratamientoDurezasObservacionesRequisicion,
			   tp.TratamientopreheatFechaRequisicion,
			   tp.TratamientopreheatNumeroRequisicion,
			   tp.TratamientopreheatCodigoRequisicion,
			   tp.TratamientopreheatFechaTratamiento,
			   tp.TratamientopreheatFechaReporte,
			   tp.TratamientopreheatNumeroReporte,
			   tp.TratamientopreheatHoja,
			   tp.TratamientopreheatGrafica,
			   tp.TratamientopreheatResultado,
			   tp.TratamientopreheatObservacionesReporte,
			   tp.TratamientopreheatObservacionesRequisicion,
			   p.PinturaFechaRequisicion,
			   p.PinturaNumeroRequisicion,
			   p.PinturaSistema,
			   p.PinturaColor,
			   p.PinturaCodigo,
			   p.PinturaFechaSendBlast,
			   p.PinturaReporteSendBlast,
			   p.PinturaFechaPrimarios,
			   p.PinturaReportePrimarios,
			   p.PinturaFechaIntermedios,
			   p.PinturaReporteIntermedios,
			   p.PinturaFechaAcabadoVisual,
			   p.PinturaReporteAcabadoVisual,
			   p.PinturaFechaAdherencia,
			   p.PinturaReporteAdherencia,
			   p.PinturaFechaPullOff,
			   p.PinturaReportePullOff,
			   e.EmbarqueEtiqueta,
			   e.EmbarqueFechaEtiqueta,
			   e.EmbarqueFechaPreparacion,
			   e.EmbarqueFechaEmbarque,
			   e.EmbarqueNumeroEmbarque
		from #TempGeneral g 
		LEFT JOIN #TempJuntaWorkstatus jw on g.GeneralJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			SELECT	jarm.JuntaArmadoID,
					jarm.JuntaWorkstatusID,
					FechaArmado,
					FechaReporte,
					ta.Nombre [Taller],
					tu.Codigo [Tubero],
					NumeroUnico1ID,
					NumeroUnico2ID,
					nu1.Codigo [NumeroUnico1],
					nu2.Codigo [NumeroUnico2],
					jarm.TuberoID
			FROM JuntaArmado jarm
			INNER JOIN Taller ta on ta.TallerID = jarm.TallerID 
			INNER JOIN Tubero tu on tu.TuberoID = jarm.TuberoID
			INNER JOIN NumeroUnico nu1 on nu1.NumeroUnicoID = jarm.NumeroUnico1ID
			INNER JOIN NumeroUnico nu2 on nu2.NumeroUnicoID = jarm.NumeroUnico2ID
			WHERE jarm.JuntaWorkstatusID IN
			(
				SELECT JuntaWorkstatusID FROM #TempJuntaWorkstatus
			)
		) ja on ja.JuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempJuntaSoldadura js on js.SoldaduraJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempJuntaInspeccionVisual iv on iv.InspeccionVisualJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN #TempWorkstatusSpool ws on ws.OrdenTrabajoSpoolID = jw.OrdenTrabajoSpoolID
		LEFT JOIN #TempPintura p on p.PinturaWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN #TempEmbarque e on e.EmbarqueWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN
		(			
			select	tmp.PruebaRTFechaRequisicion,
					tmp.PruebaRTNumeroRequisicion,
					tmp.PruebaRTCodigoRequisicion,
					tmp.PruebaRTFechaPrueba,
					tmp.PruebaRTFechaReporte,
					tmp.PruebaRTNumeroReporte,
					tmp.PruebaRTHoja,
					tmp.PruebaRTResultado,
					tmp.PruebaRTDefecto,
					tmp.PruebaRTObservacionesReporte,
					tmp.PruebaRTObservacionesRequisicion,
					tmp.PruebaRTJuntaWorkstatusID
			from #TempPruebaRT tmp
			inner join(
				select	PruebaRTJuntaWorkstatusID, 
						MAX(PruebaRTFechaPrueba) as PruebaRTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaRT
				group by PruebaRTJuntaWorkstatusID
			) A on	A.PruebaRTJuntaWorkstatusID = tmp.PruebaRTJuntaWorkstatusID
					and A.PruebaRTFechaPrueba = tmp.PruebaRTFechaPrueba
					AND A.FechaModificacion = tmp.FechaModificacion
		)  prt on prt.PruebaRTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.PruebaPTFechaRequisicion,
					tmp.PruebaPTNumeroRequisicion,
					tmp.PruebaPTCodigoRequisicion,
					tmp.PruebaPTFechaPrueba,
					tmp.PruebaPTFechaReporte,
					tmp.PruebaPTNumeroReporte,
					tmp.PruebaPTHoja,
					tmp.PruebaPTResultado,
					tmp.PruebaPTDefecto,
					tmp.PruebaPTObservacionesReporte,
					tmp.PruebaPTObservacionesRequisicion,
					tmp.PruebaPTJuntaWorkstatusID
			from #TempPruebaPT tmp
			inner join(
				select	PruebaPTJuntaWorkstatusID, 
						MAX(PruebaPTFechaPrueba) as PruebaPTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion						
				from #TempPruebaPT
				group by PruebaPTJuntaWorkstatusID
			) A on A.PruebaPTJuntaWorkstatusID = tmp.PruebaPTJuntaWorkstatusID
			and A.PruebaPTFechaPrueba = tmp.PruebaPTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
			
		)  ppt on ppt.PruebaPTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			select	tmp.PruebaRTPostTTFechaRequisicion,
					tmp.PruebaRTPostTTNumeroRequisicion,
					tmp.PruebaRTPostTTCodigoRequisicion,
					tmp.PruebaRTPostTTFechaPrueba,
					tmp.PruebaRTPostTTFechaReporte,
					tmp.PruebaRTPostTTNumeroReporte,
					tmp.PruebaRTPostTTHoja,
					tmp.PruebaRTPostTTResultado,
					tmp.PruebaRTPostTTDefecto,
					tmp.PruebaRTPostTTObservacionesReporte,
					tmp.PruebaRTPostTTObservacionesRequisicion, 
					tmp.PruebaRTPostTTJuntaWorkstatusID
			from #TempPruebaRTPostTT tmp
			inner join(
				select	PruebaRTPostTTJuntaWorkstatusID, 
						MAX(PruebaRTPostTTFechaPrueba) as PruebaRTPostTTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaRTPostTT
				group by PruebaRTPostTTJuntaWorkstatusID
			) A on A.PruebaRTPostTTJuntaWorkstatusID = tmp.PruebaRTPostTTJuntaWorkstatusID
			and A.PruebaRTPostTTFechaPrueba = tmp.PruebaRTPostTTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
		)  prtptt on prtptt.PruebaRTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(	
			select	tmp.PruebaPTPostTTFechaRequisicion,
					tmp.PruebaPTPostTTNumeroRequisicion,
					tmp.PruebaPTPostTTCodigoRequisicion,
					tmp.PruebaPTPostTTFechaPrueba,
					tmp.PruebaPTPostTTFechaReporte,
					tmp.PruebaPTPostTTNumeroReporte,
					tmp.PruebaPTPostTTHoja,
					tmp.PruebaPTPostTTResultado,
					tmp.PruebaPTPostTTDefecto,
					tmp.PruebaPTPostTTObservacionesReporte,
					tmp.PruebaPTPostTTObservacionesRequisicion,
					tmp.PruebaPTPostTTJuntaWorkstatusID
			from #TempPruebaPTPostTT tmp
			inner join(
				select	PruebaPTPostTTJuntaWorkstatusID, 
						MAX(PruebaPTPostTTFechaPrueba) as PruebaPTPostTTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaPTPostTT
				group by PruebaPTPostTTJuntaWorkstatusID
			) A on	A.PruebaPTPostTTJuntaWorkstatusID = tmp.PruebaPTPostTTJuntaWorkstatusID
					and A.PruebaPTPostTTFechaPrueba = tmp.PruebaPTPostTTFechaPrueba
					AND A.FechaModificacion = tmp.FechaModificacion
		)  pptptt on pptptt.PruebaPTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		( 
			select	tmp.PruebaUTFechaRequisicion,
					tmp.PruebaUTNumeroRequisicion,
					tmp.PruebaUTCodigoRequisicion,
					tmp.PruebaUTFechaPrueba,
					tmp.PruebaUTFechaReporte,
					tmp.PruebaUTNumeroReporte,
					tmp.PruebaUTHoja,
					tmp.PruebaUTResultado,
					tmp.PruebaUTDefecto,
					tmp.PruebaUTObservacionesReporte,
					tmp.PruebaUTObservacionesRequisicion,
					tmp.PruebaUTJuntaWorkstatusID
			from #TempPruebaUT tmp
			inner join(
				select	PruebaUTJuntaWorkstatusID, 
						MAX(PruebaUTFechaPrueba) as PruebaUTFechaPrueba,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempPruebaUT
				group by PruebaUTJuntaWorkstatusID
			) A on A.PruebaUTJuntaWorkstatusID = tmp.PruebaUTJuntaWorkstatusID
			and A.PruebaUTFechaPrueba = tmp.PruebaUTFechaPrueba
			AND A.FechaModificacion = tmp.FechaModificacion
		)  put on put.PruebaUTJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.TratamientoPwhtFechaRequisicion,
					tmp.TratamientoPwhtNumeroRequisicion,
					tmp.TratamientoPwhtCodigoRequisicion,
					tmp.TratamientoPwhtFechaTratamiento,
					tmp.TratamientoPwhtFechaReporte,
					tmp.TratamientoPwhtNumeroReporte,
					tmp.TratamientoPwhtHoja,
					tmp.TratamientoPwhtGrafica,
					tmp.TratamientoPwhtResultado,
					tmp.TratamientoPwhtObservacionesReporte,
					tmp.TratamientoPwhtObservacionesRequisicion,
					tmp.TratamientoPwhtJuntaWorkstatusID
			from #TempTratamientoPwht tmp
			inner join(
				select	TratamientoPwhtJuntaWorkstatusID, 
						MAX(TratamientoPwhtFechaTratamiento) as TratamientoPwhtFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoPwht
				group by TratamientoPwhtJuntaWorkstatusID
			) A on A.TratamientoPwhtJuntaWorkstatusID = tmp.TratamientoPwhtJuntaWorkstatusID
			and A.TratamientoPwhtFechaTratamiento = tmp.TratamientoPwhtFechaTratamiento
			AND A.FechaModificacion = tmp.FechaModificacion
		)  tpwht on tpwht.TratamientoPwhtJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(			
			select	tmp.TratamientoDurezasFechaRequisicion,
					tmp.TratamientoDurezasNumeroRequisicion,
					tmp.TratamientoDurezasCodigoRequisicion,
					tmp.TratamientoDurezasFechaTratamiento,
					tmp.TratamientoDurezasFechaReporte,
					tmp.TratamientoDurezasNumeroReporte,
					tmp.TratamientoDurezasHoja,
					tmp.TratamientoDurezasGrafica,
					tmp.TratamientoDurezasResultado,
					tmp.TratamientoDurezasObservacionesReporte,
					tmp.TratamientoDurezasObservacionesRequisicion, 
					tmp.TratamientoDurezasJuntaWorkstatusID
			from #TempTratamientoDurezas tmp
			inner join(
				select	TratamientoDurezasJuntaWorkstatusID,
						MAX(TratamientoDurezasFechaTratamiento) as TratamientoDurezasFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoDurezas
				group by TratamientoDurezasJuntaWorkstatusID
			) A on A.TratamientoDurezasJuntaWorkstatusID = tmp.TratamientoDurezasJuntaWorkstatusID
			and A.TratamientoDurezasFechaTratamiento = tmp.TratamientoDurezasFechaTratamiento			
			AND A.FechaModificacion = tmp.FechaModificacion			
		) td on td.TratamientoDurezasJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			select	tmp.TratamientopreheatFechaRequisicion,
					tmp.TratamientopreheatNumeroRequisicion,
					tmp.TratamientopreheatCodigoRequisicion,
					tmp.TratamientopreheatFechaTratamiento,
					tmp.TratamientopreheatFechaReporte,
					tmp.TratamientopreheatNumeroReporte,
					tmp.TratamientopreheatHoja,
					tmp.TratamientopreheatGrafica,
					tmp.TratamientopreheatResultado,
					tmp.TratamientopreheatObservacionesReporte,
					tmp.TratamientopreheatObservacionesRequisicion,
					tmp.TratamientoPreheatJuntaWorkstatusID
			from #TempTratamientoPreheat tmp
			inner join(
				select	TratamientoPreheatJuntaWorkstatusID,
						MAX(TratamientoPreheatFechaTratamiento) as TratamientoPreheatFechaTratamiento,
						MAX(FechaModificacion) AS FechaModificacion
				from #TempTratamientoPreheat
				group by TratamientoPreheatJuntaWorkstatusID
			) A on A.TratamientoPreheatJuntaWorkstatusID = tmp.TratamientoPreheatJuntaWorkstatusID
				AND A.TratamientoPreheatFechaTratamiento = tmp.TratamientoPreheatFechaTratamiento
				AND A.FechaModificacion = tmp.FechaModificacion			
		) tp on tp.TratamientopreheatJuntaWorkstatusID = jw.JuntaWorkStatusID
		LEFT JOIN
		(
			SELECT	tmp.InspeccionDimensionalFecha,
					tmp.InspeccionDimensionalFechaReporte,
					tmp.InspeccionDimensionalNumeroReporte,
					tmp.InspeccionDimensionalHoja,
					tmp.InspeccionDimensionalResultado,
					tmp.InspeccionDimensionalWorkstatusSpoolID
			FROM #TempInspeccionDimensional tmp
			INNER JOIN(
				SELECT	InspeccionDimensionalWorkstatusSpoolID, 
						MAX(InspeccionDimensionalFechaLiberacion) AS InspeccionDimensionalFechaLiberacion,
						MAX(FechaModificacion) AS FechaModificacion
				FROM #TempInspeccionDimensional
				GROUP BY InspeccionDimensionalWorkstatusSpoolID
			) A ON A.InspeccionDimensionalWorkstatusSpoolID = tmp.InspeccionDimensionalWorkstatusSpoolID
				AND A.InspeccionDimensionalFechaLiberacion = tmp.InspeccionDimensionalFechaLiberacion
				AND A.FechaModificacion = tmp.FechaModificacion			
		) id on id.InspeccionDimensionalWorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN
		(
			SELECT	tmp.InspeccionEspesoresFecha,
					tmp.InspeccionEspesoresFechaReporte,
					tmp.InspeccionEspesoresNumeroReporte,
					tmp.InspeccionEspesoresHoja,
					tmp.InspeccionEspesoresResultado,
					tmp.InspeccionEspesoresObservaciones,
					tmp.InspeccionEspesoresWorkstatusSpoolID
			FROM #TempInspeccionEspesores tmp
			INNER JOIN(
					SELECT InspeccionEspesoresWorkstatusSpoolID, 
						   MAX(InspeccionEspesoresFechaLiberacion) AS InspeccionEspesoresFechaLiberacion,
						   MAX(FechaModificacion) AS FechaModificacion
					FROM #TempInspeccionEspesores
					GROUP BY InspeccionEspesoresWorkstatusSpoolID
				) A ON A.InspeccionEspesoresWorkstatusSpoolID = tmp.InspeccionEspesoresWorkstatusSpoolID
					AND A.InspeccionEspesoresFechaLiberacion = tmp.InspeccionEspesoresFechaLiberacion
		) ie on ie.InspeccionEspesoresWorkstatusSpoolID = ws.WorkstatusSpoolID
		
	
	DROP TABLE #TempProyecto
	DROP TABLE #TempOrdenTrabajo
	DROP TABLE #TempGeneral 
	DROP TABLE #TempEmbarque 
	DROP TABLE #TempPintura 
	DROP TABLE #TempTratamientoPreheat 
	DROP TABLE #TempTratamientoDurezas 
	DROP TABLE #TempTratamientoPwht 
	DROP TABLE #TempPruebaUT 
	DROP TABLE #TempPruebaPTPostTT 
	DROP TABLE #TempPruebaRTPostTT 
	DROP TABLE #TempPruebaRT 	
	DROP TABLE #TempJuntaInspeccionVisual 
	DROP TABLE #TempWorkstatusSpool 
	DROP TABLE #TempInspeccionEspesores 
	DROP TABLE #TempInspeccionDimensional 
	DROP TABLE #TempReporteDimensional 
	DROP TABLE #TempJuntaReportePnd 	
	DROP TABLE #TempJuntaReporteTt 
	DROP TABLE #TempJuntaSoldadura 	
	DROP TABLE #TempJuntaWorkstatus 
	DROP TABLE #TempJuntaSpool 
	DROP TABLE #TempSpool 
	DROP TABLE #TempOrdenTrabajoSpool
	DROP TABLE #TempPruebaPT
	DROP TABLE #TempMaterialSpool
	DROP TABLE #TempJuntaInspeccionVisualDefecto

		
	SET NOCOUNT OFF;

END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSeguimientoDeSpools]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerSeguimientoDeSpools]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerSeguimientoDeSpools
	Funcion:	Trae toda la informacion necesaria para el seguimiento de Spools
	Parametros:	@ProyectoID INT
				@NumeroOrden VarChar()
				@NumeroControl VarChar()
				@SpoolID INT
	Autor:		MMG
	Modificado:	26/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerSeguimientoDeSpools]
(
	@ProyectoID INT,
	@OrdenTrabajoID INT,
	@OrdenTrabajoSpoolID INT,
	@SpoolID INT
)
AS
BEGIN

	SET NOCOUNT ON; 

	--Se crean las tablas temporales
	
	--TABLA PROYECTO
	CREATE TABLE #TempProyecto
	(	
		ProyectoID INT,
		Patio INT,
		Nombre VARCHAR(100)
	)
	
	--TABLA ORDEN DE TRABAJO
	CREATE TABLE #TempOrdenTrabajo
	(	
		OrdenTrabajoID INT,
		NumeroOrden VARCHAR(50)
	)
	
	--TABLA ORDEN TRABAJO SPOOL
	CREATE TABLE #TempOrdenTrabajoSpool
	(	
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoID INT,
		NumeroControl VARCHAR(50),
		SpoolID INT
	)
	
	--TABLA SPOOL
	CREATE TABLE #TempSpool
	(	
		SpoolID INT,
		Nombre VARCHAR(50),
		Pdi DECIMAL(10,4),
		Peso DECIMAL(7,2),
		Area DECIMAL(7,4),
		Especificacion VARCHAR(15),
		Prioridad INT,
	    Segmento1 VARCHAR(20),
	    Segmento2 VARCHAR(20),
	    Segmento3 VARCHAR(20),
	    Segmento4 VARCHAR(20),
	    Segmento5 VARCHAR(20),
	    Segmento6 VARCHAR(20),
	    Segmento7 VARCHAR(20)
	)
	
	--TABLA JUNTA SPOOL
	CREATE TABLE #TempjuntaSpool
	(	
		JuntaSpoolID INT,
		SpoolID INT,
		TipoJuntaID INT,
		Diametro DECIMAL(7,4),
		Cedula VARCHAR(10),
		Espesor DECIMAL(10,4),
		EtiquetaMaterial1 VARCHAR(10),
		EtiquetaMaterial2 VARCHAR(10),
		FamiliaAceroMaterial1ID INT,
		FamiliaAceroMaterial2ID INT,
		peqs DECIMAL(10,4),
		KgTeoricos DECIMAL(12,4)
		
	)
	
	--TABLA JUNTA WORKSTATUS
	CREATE TABLE #TempJuntaWorkstatus
	(	
		JuntaWorkStatusID INT,
		JuntaSpoolID INT,
		JuntaArmadoID INT,
		JuntaSoldaduraID INT,
		JuntaInspeccionVisualID INT,
		OrdenTrabajoSpoolID INT,
		UltimoProcesoID INT,
		EtiquetaJunta VARCHAR(50)
	)	
			
	--TABLA REPORTE DIMENSIONAL
	CREATE TABLE #TempReporteDimensional
	(	
		ReporteDimensionalID INT,
		FechaReporte DATETIME,
		NumeroReporte VARCHAR(50),
		TipoReporteDimensionalID INT
	)
	
	--TABLA INSPECCION DIMENSIONAL
	CREATE TABLE #TempInspeccionDimensional
	(	
		InspeccionDimensionalReporteDimensionalDetalleID INT,
		InspeccionDimensionalWorkstatusSpoolID INT,
		InspeccionDimensionalFecha DATETIME,
		InspeccionDimensionalFechaReporte DATETIME,
		InspeccionDimensionalNumeroReporte VARCHAR(50),
		InspeccionDimensionalHoja INT,
		InspeccionDimensionalResultado BIT,
		InspeccionDimensionalObservaciones VARCHAR(500),
		InspeccionDimensionalNumeroRechazos INT
	)
	
	--TABLA INSPECCION ESPESORES
	CREATE TABLE #TempInspeccionEspesores
	(	
		InspeccionEspesoresReporteDimensionalDetalleID INT,
		InspeccionEspesoresWorkstatusSpoolID INT,
		InspeccionEspesoresFecha DATETIME,
		InspeccionEspesoresFechaReporte DATETIME,
		InspeccionEspesoresNumeroReporte VARCHAR(50),
		InspeccionEspesoresHoja INT,
		InspeccionEspesoresResultado BIT,
		InspeccionEspesoresObservaciones VARCHAR(500),
		InspeccionEspesoresNumeroRechazos INT
	)	
	
	--TABLA WORKSTATUS SPOOL
	CREATE TABLE #TempWorkstatusSpool
	(	
		WorkstatusSpoolID INT,
		OrdenTrabajoSpoolID INT,
		PinturaSistema VARCHAR(50),
		PinturaColor VARCHAR(50),
		PinturaCodigo VARCHAR(50),
		EmbarqueEtiqueta VARCHAR(20),
		FechaEtiqueta DATETIME,
		FechaPreparacion DATETIME		
	)
			
	--TABLA PINTURA
	CREATE TABLE #TempPintura
	(	
		PinturaPinturaSpoolID INT,
		PinturaWorkstatusSpoolID INT,
		PinturaFechaRequisicion DATETIME,
		PinturaNumeroRequisicion VARCHAR(50),
		PinturaSistema VARCHAR(50),
		PinturaColor VARCHAR(50),
		PinturaCodigo VARCHAR(50),
		PinturaFechaSendBlast DATETIME,
		PinturaReporteSendBlast VARCHAR(50),
		PinturaFechaPrimarios DATETIME,
		PinturaReportePrimarios VARCHAR(50),
		PinturaFechaIntermedios DATETIME,
		PinturaReporteIntermedios VARCHAR(50),
		PinturaFechaAcabadoVisual DATETIME,
		PinturaReporteAcabadoVisual VARCHAR(50),
		PinturaFechaAdherencia DATETIME,
		PinturaReporteAdherencia VARCHAR(50),
		PinturaFechaPullOff DATETIME,
		PinturaReportePullOff VARCHAR(50)
	)
	
	--TABLA EMBARQUE
	CREATE TABLE #TempEmbarque
	(
		EmbarqueEmbarqueSpoolID INT,
		EmbarqueWorkstatusSpoolID INT,
		EmbarqueEtiqueta VARCHAR(20),
		EmbarqueFechaEtiqueta DATETIME,
		EmbarqueFechaPreparacion DATETIME,
		EmbarqueFechaEmbarque DATETIME,
		EmbarqueNumeroEmbarque VARCHAR(50)
	)
	
	--TABLA GENERAL
	CREATE TABLE #TempGeneral
	(
		GeneralSpoolID INT,
		GeneralJuntaWorkstatusID INT,
		GeneralProyecto VARCHAR(50),
		GeneralOrdenDeTrabajo VARCHAR(50),
		GeneralNumeroDeControl VARCHAR(50),
		GeneralSpool VARCHAR(50),
		GeneralNumeroJuntas INT,
		GeneralPrioridad INT,
		GeneralPdi DECIMAL(10,4),
		GeneralPeso DECIMAL(7,2),
		GeneralArea DECIMAL(7,2),
		GeneralEspecificacion VARCHAR(15),
		GeneralTieneHold BIT,
		Segmento1 VARCHAR(20),
		Segmento2 VARCHAR(20),
		Segmento3 VARCHAR(20),
		Segmento4 VARCHAR(20),
		Segmento5 VARCHAR(20),
		Segmento6 VARCHAR(20),
		Segmento7 VARCHAR(20),
		OrdenTrabajoSpoolID INT
	)
	
	--Comienzan los inserts a las tablas temporales con filtros si los tienen.
	
	--INSERT PROYECTO
	INSERT INTO #TempProyecto
		SELECT ProyectoID,
			   PatioID,
			   Nombre
		FROM Proyecto
		WHERE ProyectoID = @ProyectoID
	
	--INSERT ORDEN DE TRABAJO
	if(@OrdenTrabajoID is NULL)
	BEGIN
	
		INSERT INTO #TempOrdenTrabajo
			SELECT OrdenTrabajoID,
				   ot.NumeroOrden
			FROM OrdenTrabajo ot
			WHERE ProyectoID = @ProyectoID
			
	END
	ELSE 
	BEGIN
	
		INSERT INTO #TempOrdenTrabajo
			SELECT OrdenTrabajoID,
				   ot.NumeroOrden
			FROM OrdenTrabajo ot
			WHERE ProyectoID = @ProyectoID AND
				  OrdenTrabajoID = @OrdenTrabajoID
				  
	END
	
	--INSERT ORDEN TRABAJO SPOOL
	INSERT INTO #TempOrdenTrabajoSpool
		SELECT OrdenTrabajoSpoolID,
			   ots.OrdenTrabajoID,
			   ots.NumeroControl,
			   ots.SpoolID
		FROM OrdenTrabajoSpool ots
		INNER JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
	
	--INSERT SPOOL
	IF(@SpoolID IS NULL)
	BEGIN
		INSERT INTO #TempSpool
			SELECT SpoolID,
				   Nombre,
				   Pdis,
				   Peso,
				   Area,
				   Especificacion,
				   Prioridad,
				   Segmento1,
				   Segmento2,
				   Segmento3,
				   Segmento4,
				   Segmento5,
				   Segmento6,
				   Segmento7
			FROM Spool
			WHERE ProyectoID = @ProyectoID
	
	END
	ELSE
	BEGIN
	
		INSERT INTO #TempSpool
			SELECT SpoolID,
				   Nombre,
				   Pdis,
				   Peso,
				   Area,
				   Especificacion,
				   Prioridad,
				   Segmento1,
				   Segmento2,
				   Segmento3,
				   Segmento4,
				   Segmento5,
				   Segmento6,
				   Segmento7
			FROM Spool
			WHERE ProyectoID = @ProyectoID and
			SpoolID = @SpoolID
		
	END
	
	--INSERT JUNTA SPOOL
	INSERT INTO #TempjuntaSpool
			SELECT JuntaSpoolID,
				  js.SpoolID,
				  js.TipoJuntaID,
				  js.Diametro,
				  js.Cedula,
				  js.Espesor,
				  js.EtiquetaMaterial1,
				  js.EtiquetaMaterial2,
				  js.FamiliaAceroMaterial1ID,
				  js.FamiliaAceroMaterial2ID,
				  js.Peqs,
				  js.KgTeoricos
			FROM JuntaSpool js
			INNER JOIN #TempSpool sp on sp.SpoolID = js.SpoolID
		
	--INSERT JUNTA WORKSTATUS
	IF(@OrdenTrabajoID IS NULL)
	BEGIN
			INSERT INTO #TempJuntaWorkstatus
				SELECT jws.JuntaWorkstatusID,
					   jws.JuntaSpoolID,
					   jws.JuntaArmadoID,
					   jws.JuntaSoldaduraID,
					   jws.JuntaInspeccionVisualID,
					   jws.OrdenTrabajoSpoolID,
					   jws.UltimoProcesoID,
					   jws.EtiquetaJunta
				FROM JuntaWorkstatus jws
				INNER JOIN #TempjuntaSpool jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
						
	END	
	ELSE
	BEGIN
			
		IF(@SpoolID IS NULL)
		BEGIN
		
			INSERT INTO #TempJuntaWorkstatus
			SELECT JuntaWorkstatusID,
				   jws.JuntaSpoolID,
				   jws.JuntaArmadoID,
				   jws.JuntaSoldaduraID,
				   jws.JuntaInspeccionVisualID,
				   jws.OrdenTrabajoSpoolID,
				   jws.UltimoProcesoID,
				   jws.EtiquetaJunta
			FROM JuntaWorkstatus jws
			INNER JOIN #TempjuntaSpool jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
			INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
						
		END
		ELSE
		BEGIN
		
			INSERT INTO #TempJuntaWorkstatus
				SELECT JuntaWorkstatusID,
					   jws.JuntaSpoolID,
					   jws.JuntaArmadoID,
					   jws.JuntaSoldaduraID,
					   jws.JuntaInspeccionVisualID,
					   jws.OrdenTrabajoSpoolID,
					   jws.UltimoProcesoID,
					   jws.EtiquetaJunta
				FROM JuntaWorkstatus jws
				INNER JOIN #TempjuntaSpool jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
				INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
				WHERE jws.JuntaWorkstatusID = @SpoolID
				   
		END
	END
	
	--INSERT REPORTE DIMENSIONAL
	INSERT INTO #TempReporteDimensional
		SELECT DISTINCT ReporteDimensionalID,
			   FechaReporte,
			   NumeroReporte,
			   TipoReporteDimensionalID
		FROM ReporteDimensional
		WHERE ProyectoID = @ProyectoID 
				
	--INSERT INSPECCION DIMENSIONAL
	INSERT INTO #TempInspeccionDimensional
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   nr.numerorechazos
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID
		LEFT JOIN(
			select ReporteDimensionalID,
				   COUNT(Aprobado) numerorechazos
		    from ReporteDimensionalDetalle
		    where Aprobado = 0
		    group by ReporteDimensionalID
		) nr on nr.ReporteDimensionalID = rdd.ReporteDimensionalID
		where rd.TipoReporteDimensionalID = 1
		order by rdd.FechaLiberacion desc
		
	--INSERT INSPECCION ESPESORES
	INSERT INTO #TempInspeccionEspesores
		SELECT DISTINCT rdd.ReporteDimensionalDetalleID,
			   rdd.WorkstatusSpoolID,
			   rdd.FechaLiberacion,
			   rd.FechaReporte,
			   rd.NumeroReporte,
			   rdd.Hoja,
			   rdd.Aprobado,
			   rdd.Observaciones,
			   nr.numerorechazos
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID	
		LEFT JOIN(
			select ReporteDimensionalID,
				   COUNT(Aprobado) numerorechazos
		    from ReporteDimensionalDetalle
		    where Aprobado = 0
		    group by ReporteDimensionalID
		) nr on nr.ReporteDimensionalID = rdd.ReporteDimensionalID
		where rd.TipoReporteDimensionalID = 2
		order by rdd.FechaLiberacion desc
								
	--INSERT WORKSTATUS SPOOL
	INSERT INTO #TempWorkstatusSpool
		SELECT DISTINCT ws.WorkstatusSpoolID,
			   ws.OrdenTrabajoSpoolID,
			   ws.SistemaPintura,
			   ws.ColorPintura,
			   ws.CodigoPintura,
			   ws.NumeroEtiqueta,
			   ws.FechaEtiqueta,
			   ws.FechaPreparacion		  		   
		FROM WorkstatusSpool ws
		INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID	
	
	--INSERT PINTURA
	INSERT INTO #TempPintura
		SELECT DISTINCT ps.PinturaSpoolID,
			   ws.WorkstatusSpoolID,
			   rp.FechaRequisicion,
			   rp.NumeroRequisicion,
			   ws.PinturaSistema,
			   ws.PinturaColor,
			   ws.PinturaCodigo,
			   ps.FechaSandblast,
			   ps.ReporteSandblast,
			   ps.FechaPrimarios,
			   ps.ReportePrimarios,
			   ps.FechaIntermedios,
			   ps.ReporteIntermedios,
			   ps.FechaAcabadoVisual,
			   ps.ReporteAcabadoVisual,
			   ps.FechaAdherencia,
			   ps.ReporteAdherencia,
			   ps.FechaPullOff,
			   ps.ReportePullOff
		FROM PinturaSpool ps
		INNER JOIN #TempWorkstatusSpool ws on ps.WorkstatusSpoolID = ws.WorkstatusSpoolID
		INNER JOIN RequisicionPinturaDetalle rpd on rpd.RequisicionPinturaDetalleID = ps.RequisicionPinturaDetalleID
		INNER JOIN RequisicionPintura rp on rp.RequisicionPinturaID = rpd.RequisicionPinturaID
		where ps.ProyectoID = @ProyectoID
									
	--INSERT EMBARQUE
	INSERT INTO #TempEmbarque
			SELECT DISTINCT es.EmbarqueSpoolID,
				   ws.WorkstatusSpoolID,
				   ws.EmbarqueEtiqueta,
				   ws.FechaEtiqueta,
				   ws.FechaPreparacion,
				   e.FechaEmbarque,
				   e.NumeroEmbarque  		   
			FROM EmbarqueSpool es
			LEFT JOIN #TempWorkstatusSpool ws on es.WorkstatusSpoolID = ws.WorkstatusSpoolID
			LEFT JOIN Embarque e on e.EmbarqueID = es.EmbarqueID
			where e.ProyectoID = @ProyectoID
				
	--INSERT GENERAL	
	INSERT INTO #TempGeneral
		SELECT S.SpoolID,
			   wss.WorkStatusSpoolID,
			   p.Nombre,
			   ot.NumeroOrden,
			   ots.NumeroControl,
			   s.Nombre,
			   nj.NumeroJuntas,
			   s.Prioridad,
			   s.pdi,
			   s.Peso,
			   s.Area,
			   s.Especificacion,
			   ISNULL((select '1'
				where sh.TieneHoldCalidad = 1 or
					  sh.TieneHoldIngenieria = 1 or
					  sh.Confinado = 1),0),
				s.Segmento1,
				s.Segmento2,
				s.Segmento3,
				s.Segmento4,
				s.Segmento5,
				s.Segmento6,
				s.Segmento7,
				ots.OrdenTrabajoSpoolID
			FROM #Tempspool s
			LEFT JOIN #TempOrdenTrabajoSpool ots on ots.SpoolID = s.SpoolID
			LEFT JOIN #TempWorkstatusSpool wss on wss.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
			INNER JOIN #TempProyecto p on p.ProyectoID = @ProyectoID
			LEFT JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
			LEFT JOIN SpoolHold sh on sh.SpoolID = s.SpoolID
			LEFT JOIN(
				SELECT SpoolID,
					   COUNT(SpoolID) [NumeroJuntas]
				from #tempJuntaSpool
				group by SpoolID
			) nj on nj.SpoolID = s.SpoolID
			
	--DESPLEGAR TABLA

		select g.GeneralSpoolID,
		   g.GeneralProyecto,
		   g.GeneralOrdenDeTrabajo,
		   g.GeneralNumeroDeControl,
		   g.GeneralSpool,
		   g.GeneralNumeroJuntas,
		   g.GeneralPrioridad,
		   g.GeneralPdi,
		   g.GeneralPeso,
		   g.GeneralArea,
		   g.GeneralEspecificacion,
		   g.GeneralTieneHold,
		   g.Segmento1,
		   g.Segmento2,
		   g.Segmento3,
		   g.Segmento4,
		   g.Segmento5,
		   g.Segmento6,
		   g.Segmento7,
		   id.InspeccionDimensionalFecha,
		   id.InspeccionDimensionalFechaReporte,
		   id.InspeccionDimensionalNumeroReporte,
		   id.InspeccionDimensionalHoja,
		   id.InspeccionDimensionalResultado,
		   id.InspeccionDimensionalObservaciones,
		   id.InspeccionDimensionalNumeroRechazos,
		   ie.InspeccionEspesoresFecha,
		   ie.InspeccionEspesoresFechaReporte,
		   ie.InspeccionEspesoresNumeroReporte,
		   ie.InspeccionEspesoresHoja,
		   ie.InspeccionEspesoresResultado,
		   ie.InspeccionEspesoresObservaciones,
		   ie.InspeccionEspesoresNumeroRechazos,
		   p.PinturaFechaRequisicion,
		   p.PinturaNumeroRequisicion,
		   p.PinturaSistema,
		   p.PinturaColor,
		   p.PinturaCodigo,
		   p.PinturaFechaSendBlast,
		   p.PinturaReporteSendBlast,
		   p.PinturaFechaPrimarios,
		   p.PinturaReportePrimarios,
		   p.PinturaFechaIntermedios,
		   p.PinturaReporteIntermedios,
		   p.PinturaFechaAcabadoVisual,
		   p.PinturaReporteAcabadoVisual,
		   p.PinturaFechaAdherencia,
		   p.PinturaReporteAdherencia,
		   p.PinturaFechaPullOff,
		   p.PinturaReportePullOff,
		   e.EmbarqueEtiqueta,
		   e.EmbarqueFechaEtiqueta,
		   e.EmbarqueFechaPreparacion,
		   e.EmbarqueFechaEmbarque,
		   e.EmbarqueNumeroEmbarque
	from #TempGeneral g
	LEFT JOIN #TempOrdenTrabajoSpool ots on ots.SpoolID = g.GeneralSpoolID
	LEFT JOIN #TempWorkstatusSpool ws on ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	LEFT JOIN #TempPintura p on p.PinturaWorkstatusSpoolID = ws.WorkstatusSpoolID
	LEFT JOIN #TempEmbarque e on e.EmbarqueWorkstatusSpoolID = ws.WorkstatusSpoolID
	LEFT JOIN
	(
		SELECT tmp.* 
			FROM #TempInspeccionDimensional tmp
			INNER JOIN(
				SELECT InspeccionDimensionalWorkstatusSpoolID, MAX(InspeccionDimensionalFecha) AS InspeccionDimensionalFechaLiberacion
				FROM #TempInspeccionDimensional
				GROUP BY InspeccionDimensionalWorkstatusSpoolID
			) A ON A.InspeccionDimensionalWorkstatusSpoolID = tmp.InspeccionDimensionalWorkstatusSpoolID
				AND A.InspeccionDimensionalFechaLiberacion = tmp.InspeccionDimensionalFecha
		
	) id on id.InspeccionDimensionalWorkstatusSpoolID = ws.WorkstatusSpoolID
	LEFT JOIN
	(
		SELECT tmp.* 
		FROM #TempInspeccionEspesores tmp
		INNER JOIN(
				SELECT InspeccionEspesoresWorkstatusSpoolID, MAX(InspeccionEspesoresFecha) AS InspeccionEspesoresFechaLiberacion
				FROM #TempInspeccionEspesores
				GROUP BY InspeccionEspesoresWorkstatusSpoolID
			) A ON A.InspeccionEspesoresWorkstatusSpoolID = tmp.InspeccionEspesoresWorkstatusSpoolID
				AND A.InspeccionEspesoresFechaLiberacion = tmp.InspeccionEspesoresFecha
			
	) ie on ie.InspeccionEspesoresWorkstatusSpoolID = ws.WorkstatusSpoolID
	WHERE @OrdenTrabajoSpoolID IS NULL OR  G.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID
	
	SET NOCOUNT OFF;

END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSpoolsPorIds]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerSpoolsPorIds]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerSpoolsPorIds
	Funcion:	Obtiene los spools de la tabla Spool por un arreglo de Ids enviado
	Parametros:	@SpoolIDs nvarchar(max)
	Autor:		IHM
	Modificado:	28/10/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerSpoolsPorIds]
(
	 @SpoolIDs	NVARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	 [SpoolID]
			,[ProyectoID]
			,[FamiliaAcero1ID]
			,[FamiliaAcero2ID]
			,[Nombre]
			,[Dibujo]
			,[Especificacion]
			,[Cedula]
			,[Pdis]
			,[DiametroPlano]
			,[Peso]
			,[Area]
			,[PorcentajePnd]
			,[RequierePwht]
			,[PendienteDocumental]
			,[AprobadoParaCruce]
			,[Prioridad]
			,[Revision]
			,[RevisionCliente]
			,[Segmento1]
			,[Segmento2]
			,[Segmento3]
			,[Segmento4]
			,[Segmento5]
			,[Segmento6]
			,[Segmento7]
			,[UsuarioModifica]
			,[FechaModificacion]
			,[VersionRegistro]
	FROM [Spool]
	WHERE [SpoolID] IN
	(
		SELECT CAST([Value] AS INT)
		FROM dbo.SplitCVSToTable(@SpoolIDs,',')
	)



	SET NOCOUNT OFF;

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegeneraDestajoSoldador]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RegeneraDestajoSoldador]
GO
/****************************************************************************************
	Nombre:		RegeneraDestajoSoldador
	Funcion:	Vuelve a generar el destajo de un soldador en particular eliminando
				el anterior previamente asi como los de sus soldadores "hermanos"
	Parametros:	@PeriodoDestajoID int,
				@CantidadDiasFestivos int,
				@Soldadores xml,
				@UsuarioModifica uniqueidentifier
	Autor:		IHM
	Modificado:	23/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[RegeneraDestajoSoldador]
(
	@PeriodoDestajoID int,
	@CantidadDiasFestivos int,
	@Soldadores xml,
	@UsuarioModifica uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT ON;
	
	declare @TblSoldadores table
	(
		SoldadorID int
	);
	
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

	-- Son pocos, entonces utilizamos memoria
	insert into @TblSoldadores
	select distinct destajo.value('SoldadorID[1]','int') [SoldadorID]
	from @Soldadores.nodes('/ArrayOfDestajoSoldaduraGrupo[1]/DestajoSoldaduraGrupo') as DestajoSoldador(destajo)
	
	--Borrar los registros que ya existen para el destajo en particular
	delete from DestajoSoldadorDetalle
	where DestajoSoldadorID in
	(
		select DestajoSoldadorID
		from DestajoSoldador
		where	PeriodoDestajoID = @PeriodoDestajoID
				and SoldadorID in
				(
					select SoldadorID
					from @TblSoldadores
				)
	)
	
	--Borrar los registros que ya existen para el destajo en particular
	delete from DestajoSoldador
	where	PeriodoDestajoID = @PeriodoDestajoID
			and SoldadorID in
			(
				select SoldadorID
				from @TblSoldadores
			)
	
	CREATE TABLE #TmpSoldadores
	(
		DestajoSoldadorID int,
		SoldadorID int
	)
	
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

	drop table #TmpSoldadores
	drop table #SoldadoresXml
	drop table #TmpJuntasSoldadoresXml
	
	-- Regresar el ID del periodo actualizado
	SELECT @PeriodoDestajoID
	
		
	SET NOCOUNT OFF;

END

/*
	exec RegeneraDestajoSoldador '10', 2010, '20101101', '20101130', 0, '<?xml version="1.0" encoding="utf-8"?><ArrayOfDestajoArmadoGrupo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><DestajoArmadoGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><ReferenciaCuadro>0</ReferenciaCuadro><TotalCuadro>0</TotalCuadro><GranTotal>0</GranTotal><TotalDestajo>0</TotalDestajo><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><TotalAjuste>0</TotalAjuste><TuberoID>3</TuberoID><ID>-1</ID><juntas><junta><JuntaWorkstatusID>20</JuntaWorkstatusID><TuberoID>3</TuberoID><TipoJuntaID>1</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>8.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-1</ID><IDPadre>-1</IDPadre></junta><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><TuberoID>3</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-2</ID><IDPadre>-1</IDPadre></junta></juntas></DestajoArmadoGrupo><DestajoArmadoGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><ReferenciaCuadro>0</ReferenciaCuadro><TotalCuadro>0</TotalCuadro><GranTotal>0</GranTotal><TotalDestajo>0</TotalDestajo><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><TotalAjuste>0</TotalAjuste><TuberoID>4</TuberoID><ID>-2</ID><juntas><junta><JuntaWorkstatusID>39</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-1</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>40</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-2</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>41</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-3</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>42</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-4</ID><IDPadre>-2</IDPadre></junta></juntas></DestajoArmadoGrupo></ArrayOfDestajoArmadoGrupo>', '<?xml version="1.0" encoding="utf-8"?><ArrayOfDestajoSoldaduraGrupo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><DestajoSoldaduraGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><GranTotal>0</GranTotal><ReferenciaCuadro>0</ReferenciaCuadro><SoldadorID>3</SoldadorID><TotalAjuste>0</TotalAjuste><TotalCuadro>0</TotalCuadro><TotalDestajoRaiz>0</TotalDestajoRaiz><TotalDestajoRelleno>0</TotalDestajoRelleno><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><ID>-1</ID><juntas><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><SoldadorID>3</SoldadorID><TecnicaSoldadorID>0</TecnicaSoldadorID><ProcesoRaizID>0</ProcesoRaizID><ProcesoRellenoID>0</ProcesoRellenoID><TipoJuntaID>0</TipoJuntaID><FamiliaAceroID>0</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><CostoUnitarioRaiz>0</CostoUnitarioRaiz><CostoUnitarioRelleno>0</CostoUnitarioRelleno><RaizDividida>true</RaizDividida><RellenoDividido>false</RellenoDividido><NumeroFondeadores>2</NumeroFondeadores><NumeroRellenadores>0</NumeroRellenadores><DestajoRaiz>0</DestajoRaiz><DestajoRelleno>0</DestajoRelleno><ProrrateoCuadro>0</ProrrateoCuadro><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><Ajuste>0</Ajuste><Total>0</Total><ID>-1</ID><IDPadre>-1</IDPadre></junta></juntas></DestajoSoldaduraGrupo><DestajoSoldaduraGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><GranTotal>0</GranTotal><ReferenciaCuadro>0</ReferenciaCuadro><SoldadorID>4</SoldadorID><TotalAjuste>0</TotalAjuste><TotalCuadro>0</TotalCuadro><TotalDestajoRaiz>0</TotalDestajoRaiz><TotalDestajoRelleno>0</TotalDestajoRelleno><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><ID>-2</ID><juntas><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><SoldadorID>4</SoldadorID><TecnicaSoldadorID>0</TecnicaSoldadorID><ProcesoRaizID>0</ProcesoRaizID><ProcesoRellenoID>0</ProcesoRellenoID><TipoJuntaID>0</TipoJuntaID><FamiliaAceroID>0</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><CostoUnitarioRaiz>0</CostoUnitarioRaiz><CostoUnitarioRelleno>0</CostoUnitarioRelleno><RaizDividida>true</RaizDividida><RellenoDividido>false</RellenoDividido><NumeroFondeadores>2</NumeroFondeadores><NumeroRellenadores>0</NumeroRellenadores><DestajoRaiz>0</DestajoRaiz><DestajoRelleno>0</DestajoRelleno><ProrrateoCuadro>0</ProrrateoCuadro><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><Ajuste>0</Ajuste><Total>0</Total><ID>-2</ID><IDPadre>-2</IDPadre></junta></juntas></DestajoSoldaduraGrupo></ArrayOfDestajoSoldaduraGrupo>','D6A113B4-464E-496F-B15D-4456CB0AE55B'
*/

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegeneraDestajoTubero]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RegeneraDestajoTubero]
GO
/****************************************************************************************
	Nombre:		RegeneraDestajoTubero
	Funcion:	Vuelve a generar el destajo de un tubero en particular eliminando
				el anterior previamente
	Parametros:	@OldDestajoTuberoID int,
				@PeriodoDestajoID int,
				@Tuberos xml,
				@UsuarioModifica uniqueidentifier
	Autor:		IHM
	Modificado:	23/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[RegeneraDestajoTubero]
(
	@OldDestajoTuberoID int,
	@PeriodoDestajoID int,
	@CantidadDiasFestivos int,
	@Tuberos xml,
	@UsuarioModifica uniqueidentifier
)
AS
BEGIN

	SET NOCOUNT ON;
	
	declare @DestajoTuberoID int
	
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
	where destajo.value('ID[1]', 'int') = -1

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
	where jta.value('IDPadre[1]', 'int') = -1
	
	--Primero borrar el registro de la persona ya que lo vamos a regenerar
	delete from DestajoTuberoDetalle where DestajoTuberoID = @OldDestajoTuberoID
	delete from DestajoTubero where DestajoTuberoID = @OldDestajoTuberoID
	
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
	
	set @DestajoTuberoID = SCOPE_IDENTITY()
		
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
	select	@DestajoTuberoID,
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
	
	drop table #TuberosXml
	drop table #TmpJuntasTuberosXml

	-- Regresar el ID del destajo insertado
	SELECT @DestajoTuberoID
	
		
	SET NOCOUNT OFF;

END

/*
	exec RegeneraDestajoTubero '10', 2010, '20101101', '20101130', 0, '<?xml version="1.0" encoding="utf-8"?><ArrayOfDestajoArmadoGrupo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><DestajoArmadoGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><ReferenciaCuadro>0</ReferenciaCuadro><TotalCuadro>0</TotalCuadro><GranTotal>0</GranTotal><TotalDestajo>0</TotalDestajo><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><TotalAjuste>0</TotalAjuste><TuberoID>3</TuberoID><ID>-1</ID><juntas><junta><JuntaWorkstatusID>20</JuntaWorkstatusID><TuberoID>3</TuberoID><TipoJuntaID>1</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>8.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-1</ID><IDPadre>-1</IDPadre></junta><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><TuberoID>3</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-2</ID><IDPadre>-1</IDPadre></junta></juntas></DestajoArmadoGrupo><DestajoArmadoGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><ReferenciaCuadro>0</ReferenciaCuadro><TotalCuadro>0</TotalCuadro><GranTotal>0</GranTotal><TotalDestajo>0</TotalDestajo><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><TotalAjuste>0</TotalAjuste><TuberoID>4</TuberoID><ID>-2</ID><juntas><junta><JuntaWorkstatusID>39</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-1</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>40</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-2</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>41</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-3</ID><IDPadre>-2</IDPadre></junta><junta><JuntaWorkstatusID>42</JuntaWorkstatusID><TuberoID>4</TuberoID><TipoJuntaID>4</TipoJuntaID><FamiliaAceroID>62</FamiliaAceroID><ProyectoID>46</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><Ajuste>0</Ajuste><CostoUnitario>0</CostoUnitario><Destajo>0</Destajo><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><ProrrateoCuadro>0</ProrrateoCuadro><Total>0</Total><ID>-4</ID><IDPadre>-2</IDPadre></junta></juntas></DestajoArmadoGrupo></ArrayOfDestajoArmadoGrupo>', '<?xml version="1.0" encoding="utf-8"?><ArrayOfDestajoSoldaduraGrupo xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><DestajoSoldaduraGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><GranTotal>0</GranTotal><ReferenciaCuadro>0</ReferenciaCuadro><SoldadorID>3</SoldadorID><TotalAjuste>0</TotalAjuste><TotalCuadro>0</TotalCuadro><TotalDestajoRaiz>0</TotalDestajoRaiz><TotalDestajoRelleno>0</TotalDestajoRelleno><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><ID>-1</ID><juntas><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><SoldadorID>3</SoldadorID><TecnicaSoldadorID>0</TecnicaSoldadorID><ProcesoRaizID>0</ProcesoRaizID><ProcesoRellenoID>0</ProcesoRellenoID><TipoJuntaID>0</TipoJuntaID><FamiliaAceroID>0</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><CostoUnitarioRaiz>0</CostoUnitarioRaiz><CostoUnitarioRelleno>0</CostoUnitarioRelleno><RaizDividida>true</RaizDividida><RellenoDividido>false</RellenoDividido><NumeroFondeadores>2</NumeroFondeadores><NumeroRellenadores>0</NumeroRellenadores><DestajoRaiz>0</DestajoRaiz><DestajoRelleno>0</DestajoRelleno><ProrrateoCuadro>0</ProrrateoCuadro><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><Ajuste>0</Ajuste><Total>0</Total><ID>-1</ID><IDPadre>-1</IDPadre></junta></juntas></DestajoSoldaduraGrupo><DestajoSoldaduraGrupo><Aprobado>false</Aprobado><CantidadDiasFestivos>0</CantidadDiasFestivos><CostoDiaFestivo>0</CostoDiaFestivo><GranTotal>0</GranTotal><ReferenciaCuadro>0</ReferenciaCuadro><SoldadorID>4</SoldadorID><TotalAjuste>0</TotalAjuste><TotalCuadro>0</TotalCuadro><TotalDestajoRaiz>0</TotalDestajoRaiz><TotalDestajoRelleno>0</TotalDestajoRelleno><TotalDiasFestivos>0</TotalDiasFestivos><TotalOtros>0</TotalOtros><ID>-2</ID><juntas><junta><JuntaWorkstatusID>38</JuntaWorkstatusID><SoldadorID>4</SoldadorID><TecnicaSoldadorID>0</TecnicaSoldadorID><ProcesoRaizID>0</ProcesoRaizID><ProcesoRellenoID>0</ProcesoRellenoID><TipoJuntaID>0</TipoJuntaID><FamiliaAceroID>0</FamiliaAceroID><ProyectoID>29</ProyectoID><Diametro>2.0000</Diametro><EsDePeriodoAnterior>false</EsDePeriodoAnterior><CostoUnitarioRaiz>0</CostoUnitarioRaiz><CostoUnitarioRelleno>0</CostoUnitarioRelleno><RaizDividida>true</RaizDividida><RellenoDividido>false</RellenoDividido><NumeroFondeadores>2</NumeroFondeadores><NumeroRellenadores>0</NumeroRellenadores><DestajoRaiz>0</DestajoRaiz><DestajoRelleno>0</DestajoRelleno><ProrrateoCuadro>0</ProrrateoCuadro><ProrrateoDiasFestivos>0</ProrrateoDiasFestivos><ProrrateoOtros>0</ProrrateoOtros><Ajuste>0</Ajuste><Total>0</Total><ID>-2</ID><IDPadre>-2</IDPadre></junta></juntas></DestajoSoldaduraGrupo></ArrayOfDestajoSoldaduraGrupo>','D6A113B4-464E-496F-B15D-4456CB0AE55B'
*/

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Destajos]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_Destajos]
GO
-- =============================================
-- Author:		Cesar Velazquez
-- Create date: 10/Enero/2011
-- Description:	Obtiene los datos de pagos de destajos de un proyecto
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Destajos]
@ProyectoID INT,
@Idioma INT
AS
BEGIN
	--Se crea la tabla temporal de Spool
	
	CREATE TABLE #Spool(
		SpoolID INT,
		ProyectoID INT,
		OrdenTrabajoSpoolID INT,
		Nombre NVARCHAR(50),		
	    NumeroControl NVARCHAR(50),
	    NumeroOrden NVARCHAR(50)
	)
	
	--Se obtienen los spools del proyecto
	INSERT INTO #Spool
	
	SELECT	s.SpoolID,
			s.ProyectoID, 
			ots.OrdenTrabajoSpoolID, 
			s.Nombre, 
			ots.NumeroControl,
			ot.NumeroOrden
	FROM Spool s
	
	INNER JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID
	INNER JOIN OrdenTrabajo ot ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
	WHERE s.ProyectoID = @ProyectoID
	ORDER BY s.SpoolID
	
		
	--Se crea la tabla temporal JuntaSpool
	CREATE TABLE #JuntaSpool(
		JuntaSpoolID INT,
		SpoolID INT,
		TipoJuntaID INT,
		Etiqueta NVARCHAR(10),
		Diametro DECIMAL(7,4),
		Cedula NVARCHAR(10),
		Espesor DECIMAL(10,4),
		EtiquetaMaterial1 NVARCHAR(10),
		EtiquetaMaterial2 NVARCHAR(10) 
	)
	
	--Se obtienen las juntas de los Spools
	INSERT INTO #JuntaSpool
	SELECT  js.JuntaSpoolID,
			js.SpoolID,
			js.TipoJuntaID,
			js.Etiqueta,
			js.Diametro,
			js.Cedula,
			js.Espesor,
			js.EtiquetaMaterial1,
			js.EtiquetaMaterial2
			
	FROM JuntaSpool js
	WHERE js.SpoolID IN (SELECT SpoolID FROM #Spool)
	
	--Se crea la tabla JuntaWorkstatus
	CREATE TABLE #JuntaWorkstatus(
		JuntaWorkstatusID INT,
		JuntaSpoolID INT,
		UltimoProcesoID INT
		)

	--Se obtienen los datos de JuntaWorkstatus que pertenecen a las Juntas
	INSERT INTO #JuntaWorkstatus
	SELECT  jw.JuntaWorkstatusID,
			jw.JuntaSpoolID,
			jw.UltimoProcesoID
	FROM JuntaWorkstatus jw
	WHERE JuntaSpoolID IN (SELECT JuntaSpoolID FROM #JuntaSpool)		
	
	--Se crea la tabla temporal Armado
	CREATE TABLE #Armado(
		JuntaWorkstatusID INT,
		DestajoArmado INT,
		TotalArmado Money
	)
	
	INSERT INTO #Armado
		SELECT  dtd.JuntaWorkstatusID,
				pd.Semana,
				dtd.Total
		FROM DestajoTuberoDetalle dtd
		INNER JOIN #JuntaWorkstatus jw
		ON (dtd.JuntaWorkstatusID = jw.JuntaWorkstatusID)
		INNER JOIN DestajoTubero dt
		ON (dt.DestajoTuberoID = dtd.DestajoTuberoID)
		INNER JOIN PeriodoDestajo pd
		ON (pd.PeriodoDestajoID = dt.PeriodoDestajoID)
		
	--Se crea la tabla temporal Fondeo
	CREATE TABLE #Fondeo(
		JuntaWorkstatusID INT,
		DestajoFondeo INT,
		TotalFondeo Money
	)
	
	INSERT INTO #Fondeo
		SELECT  dsd.JuntaWorkstatusID,
				pd.Semana,
				SUM(dsd.DestajoRaiz)
		FROM DestajoSoldadorDetalle dsd
		INNER JOIN #JuntaWorkstatus jw
		ON (dsd.JuntaWorkstatusID = jw.JuntaWorkstatusID)
		INNER JOIN DestajoSoldador ds
		ON (ds.DestajoSoldadorID = dsd.DestajoSoldadorID)
		INNER JOIN PeriodoDestajo pd
		ON (pd.PeriodoDestajoID = ds.PeriodoDestajoID)
		GROUP BY dsd.JuntaWorkstatusID,pd.Semana
	
	--Se crea la tabla temporal Relleno
	CREATE TABLE #Relleno(
		JuntaWorkstatusID INT,
		DestajoRelleno INT,
		TotalRelleno Money
	)
	
	INSERT INTO #Relleno
		SELECT  dsd.JuntaWorkstatusID,
				pd.Semana,
				SUM(dsd.DestajoRelleno)
		FROM DestajoSoldadorDetalle dsd
		INNER JOIN #JuntaWorkstatus jw
		ON (dsd.JuntaWorkstatusID = jw.JuntaWorkstatusID)
		INNER JOIN DestajoSoldador ds
		ON (ds.DestajoSoldadorID = dsd.DestajoSoldadorID)
		INNER JOIN PeriodoDestajo pd
		ON (pd.PeriodoDestajoID = ds.PeriodoDestajoID)
		GROUP BY dsd.JuntaWorkstatusID,pd.Semana

	SELECT 
	
		p.Nombre AS [NombreProyecto],
		s.NumeroOrden AS [OrdenTrabajo],
		s.NumeroControl,
		s.Nombre AS [Spool],
		js.Etiqueta AS [Junta],
		tj.Nombre AS [TipoJunta],
		js.Diametro,
		js.Cedula,
		ISNULL(js.Espesor,0) AS [Espesor],
		js.EtiquetaMaterial1 + ' - ' + js.EtiquetaMaterial2 AS [Localizacion],
		[UltimoProceso] =
						CASE 
							WHEN @Idioma = 0 THEN up.Nombre
							ELSE up.NombreIngles
						END,
		[¨Hold?] = 
				CASE 
					WHEN ISNULL(sh.SpoolID,0) = 0 THEN 'No'
					WHEN sh.TieneHoldIngenieria = 1 AND @Idioma = 0 THEN 'S¡'
					WHEN sh.TieneHoldCalidad = 1 AND @Idioma = 0 THEN 'S¡'
					WHEN sh.Confinado = 1 AND @Idioma = 0 THEN 'S¡'
					WHEN sh.TieneHoldIngenieria = 1 AND @Idioma = 1 THEN 'Yes'
					WHEN sh.TieneHoldCalidad = 1 AND @Idioma = 1 THEN 'Yes'
					WHEN sh.Confinado = 1 AND @Idioma = 1 THEN 'Yes'
					ELSE 'No'
				END,
		ISNULL(a.DestajoArmado,0) AS [DestajoArmado],
		ISNULL(f.DestajoFondeo,0) AS [DestajoFondeo],
		ISNULL(r.DestajoRelleno,0) AS [DestajoRelleno],
		ISNULL(a.TotalArmado,0) AS [TotalArmado],
		ISNULL(f.TotalFondeo,0) AS [TotalFondeo],
		ISNULL(r.TotalRelleno,0) AS [TotalRelleno]
		
	FROM Proyecto p
	INNER JOIN #Spool s
	ON (s.ProyectoID = p.ProyectoID)
	
	INNER JOIN #JuntaSpool js
	ON (js.SpoolID = s.SpoolID)
	
	INNER JOIN TipoJunta tj
	ON (tj.TipoJuntaID = js.TipoJuntaID)
	
	INNER JOIN #JuntaWorkstatus jw
	ON (jw.JuntaSpoolID = js.JuntaSpoolID)
	
	INNER JOIN UltimoProceso up
	ON (up.UltimoProcesoID = jw.UltimoProcesoID)
	
	LEFT JOIN SpoolHold sh
	ON (js.SpoolID = sh.SpoolID)
	
	LEFT JOIN #Armado a
	ON (jw.JuntaWorkstatusID = a.JuntaWorkstatusID)
	
	LEFT JOIN #Fondeo f
	ON (jw.JuntaWorkstatusID = f.JuntaWorkstatusID)
	
	LEFT JOIN #Relleno r
	ON (jw.JuntaWorkstatusID = r.JuntaWorkstatusID)
	
	WHERE p.ProyectoID = @ProyectoID
	ORDER BY s.NumeroControl
END

GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_JuntasAFabricar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_JuntasAFabricar
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_JuntasAFabricar]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON; 
		
	SELECT	 ots.Partida
			,ots.NumeroControl
			,s.Nombre AS [NombreSpool]
			,js.Etiqueta
			,js.EtiquetaMaterial1
			,js.EtiquetaMaterial2
			,js.Diametro
			,tj.Codigo AS [CodigoJunta]
			,js.Cedula
			,(SELECT TOP 1 fa1.Nombre FROM FamiliaAcero fa1 WHERE fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID) [FamiliaAceroMaterial1]
			,(SELECT TOP 1 fa2.Nombre FROM FamiliaAcero fa2 WHERE fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID) [FamiliaAceroMaterial2]
	FROM JuntaSpool js
	INNER JOIN TipoJunta tj ON tj.TipoJuntaID = js.TipoJuntaID
	INNER JOIN OrdenTrabajoSpool ots ON ots.SpoolID = js.SpoolID
	INNER JOIN Spool s ON s.SpoolID =  js.SpoolID
	INNER JOIN OrdenTrabajo ot ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
	WHERE ot.OrdenTrabajoID = @OrdenTrabajoID
		
END

/*
	exec Rpt_JuntasAFabricar 41
*/


GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_MaterialesAFabricar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_MaterialesAFabricar
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_MaterialesAFabricar]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON; 

		declare @Congelados table
		(
			NumeroUnicoCongeladoID int,
			Consecutivo int
		);
		
		insert into @Congelados
		select	t.NumeroUnicoCongeladoID,
				row_number() over(order by t.NumeroUnicoCongeladoID)
		from
		(
			select distinct otm.NumeroUnicoCongeladoID
			from OrdenTrabajoMaterial otm
			where otm.OrdenTrabajoSpoolID in
			(
				select OrdenTrabajoSpoolID
				from OrdenTrabajoSpool
				where OrdenTrabajoID = @OrdenTrabajoID
			)
			and otm.CongeladoEsEquivalente = 1
		) t
		
		
		SELECT	 ots.Partida
				,ots.OrdenTrabajoSpoolID
				,ots.NumeroControl
				,s.Nombre
				,ms.Etiqueta
				,ms.Diametro1
				,ms.Diametro2
				,ms.Cantidad
				,ic.Codigo
				,ic.DescripcionEspanol
				,ic.TipoMaterialID
				,otm.OrdenTrabajoMaterialID
				,otm.CongeladoEsEquivalente
				,otm.NumeroUnicoCongeladoID
				,otm.NumeroUnicoSugeridoID
				,otm.SegmentoSugerido
				,(SELECT c.Consecutivo FROM @Congelados c WHERE c.NumeroUnicoCongeladoID = otm.NumeroUnicoCongeladoID) [Consecutivo]
		FROM	MaterialSpool ms
		INNER JOIN ItemCode ic ON ic.ItemCodeID = ms.ItemCodeID
		INNER JOIN Spool s ON s.SpoolID = ms.SpoolID
		INNER JOIN OrdenTrabajoMaterial otm ON otm.MaterialSpoolID = ms.MaterialSpoolID
		INNER JOIN OrdenTrabajoSpool ots ON ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID
		WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
		
END

/*
	exec [Rpt_MaterialesAFabricar] 41
	
*/


GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerCorteSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerCorteSpool
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerCorteSpool]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON; 
		
		SELECT	ot.OrdenTrabajoID
			   ,ots.SpoolID
			   ,ots.NumeroControl
			   ,ic.ItemCodeID
			   ,ic.DescripcionEspanol
			   ,ic.Codigo
			   ,cs.Diametro
			   ,cs.TipoCorte1ID
			   ,cs.Cantidad
			   ,cs.TipoCorte2ID
			   ,cs.EtiquetaMaterial
			   ,cs.EtiquetaSeccion
			   ,cs.Observaciones
			   ,tc1.Nombre AS [TipoCorteNombre1]
			   ,tc2.Nombre AS [TipoCorteNombre2]
			   ,s.Nombre AS NombreSpool
	   FROM ItemCode ic
	   INNER JOIN CorteSpool cs ON cs.ItemCodeID = ic.ItemCodeID
	   INNER JOIN Spool s ON s.SpoolID = cs.SpoolID
	   INNER JOIN TipoCorte tc1 ON tc1.TipoCorteID = cs.TipoCorte1ID
	   INNER JOIN TipoCorte tc2 ON tc2.TipoCorteID = cs.TipoCorte2ID
	   INNER JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID
	   INNER JOIN OrdenTrabajo ot ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
	   WHERE ot.OrdenTrabajoID = @OrdenTrabajoID
END

/*
	exec Rpt_ObtenerCorteSpool 41
*/


GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerIcEquivalentesPorOdt]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerIcEquivalentesPorOdt
GO
-- =============================================
-- Author:		Ivan Hernandez
-- Create date: 11/Nov/2010
-- Description:	Obtener los I.C. equivalentes en caso que haya que sugerirlos en la ODT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerIcEquivalentesPorOdt]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @Congelados table
	(
		NumeroUnicoCongeladoID int,
		Consecutivo int
	);

	insert into @Congelados
	select	t.NumeroUnicoCongeladoID,
			row_number() over(order by t.NumeroUnicoCongeladoID)
	from
	(
		select distinct otm.NumeroUnicoCongeladoID
		from OrdenTrabajoMaterial otm
		where otm.OrdenTrabajoSpoolID in
		(
			select OrdenTrabajoSpoolID
			from OrdenTrabajoSpool
			where OrdenTrabajoID = @OrdenTrabajoID
		)
		and otm.CongeladoEsEquivalente = 1
	) t	

	SELECT	c.NumeroUnicoCongeladoID,
			c.Consecutivo,
			nu.Diametro1,
			nu.Diametro2,
			ic.Codigo,
			ic.DescripcionEspanol,
			ic.DescripcionIngles,
			ic.ItemCodeID
	FROM	@Congelados c
	INNER JOIN NumeroUnico nu on c.NumeroUnicoCongeladoID = nu.NumeroUnicoID
	INNER JOIN ItemCode ic on ic.ItemCodeID = nu.ItemCodeID
	
	
END

/*
	exec [Rpt_ObtenerIcEquivalentesPorOdt] 41
*/

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerListaCorte]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerListaCorte
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerListaCorte]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
		SELECT
		cs.EtiquetaMaterial
		,tc1.Nombre AS [TipoCorteNombre1]
		,tc2.Nombre AS [TipoCorteNombre2]
		,cs.TipoCorte1ID
		,cs.TipoCorte2ID
		,s.Dibujo
		,s.Nombre
		,cs.EtiquetaSeccion
		FROM CorteSpool cs
		INNER JOIN TipoCorte tc1
		ON tc1.TipoCorteID = cs.TipoCorte1ID
		INNER JOIN TipoCorte tc2
		ON tc2.TipoCorteID = cs.TipoCorte2ID
		INNER JOIN Spool s
		ON s.SpoolID = cs.SpoolID
		INNER JOIN OrdenTrabajoSpool ots
		ON ots.SpoolID =  cs.SpoolID
		INNER JOIN OrdenTrabajo ot
		ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
		WHERE ot.OrdenTrabajoID = @OrdenTrabajoID
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerOdtInfo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerOdtInfo
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para el header de los reportes de ODT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerOdtInfo]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
		SELECT
		  p.Nombre AS [ProyectoNombre]
		  ,ot.OrdenTrabajoID
		  ,ot.NumeroOrden
		  ,ot.FechaOrden
		  ,u.UserId
		  ,u.Nombre AS [Nombre]
		  ,u.ApPaterno
		  ,t.Nombre AS [TallerNombre]
		  ,t.TallerID AS [TallerID]
		  ,COUNT(DISTINCT ots.OrdenTrabajoSpoolID) AS [TotalSpools]
		FROM
		  OrdenTrabajo ot
		  INNER JOIN OrdenTrabajoSpool ots
			ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
		  INNER JOIN Taller t
			ON t.TallerID = ot.TallerID
		  INNER JOIN Proyecto p
			ON ot.ProyectoID = p.ProyectoID
		  INNER JOIN Usuario u
			ON ot.UsuarioModifica = u.UserId
		WHERE
		  ot.OrdenTrabajoID = @OrdenTrabajoID
	    GROUP BY
			p.Nombre
			,ot.OrdenTrabajoID
			,ot.NumeroOrden
			,ot.FechaOrden
			,u.UserId
			,u.Nombre
			,u.ApPaterno
			,t.Nombre
		    ,t.TallerID

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerOdtInfoConEstado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerOdtInfoConEstado
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para el header de los reportes de ODT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerOdtInfoConEstado]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
		SELECT ot.OrdenTrabajoID
		,ot.FechaOrden
		,ot.NumeroOrden
		,t.Nombre AS [TallerNombre]
		,eo.Nombre AS [EstatusNombre]
		,COUNT(DISTINCT ots.OrdenTrabajoSpoolID) as [TotalSpools]
		FROM
		OrdenTrabajo ot
		INNER JOIN OrdenTrabajoSpool ots
		ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
		INNER JOIN Taller t
		ON t.TallerID = ot.TallerID
		INNER JOIN EstatusOrden eo
		ON eo.EstatusOrdenID = ot.EstatusOrdenID
		WHERE ot.OrdenTrabajoID = @OrdenTrabajoID
		GROUP BY
			ot.OrdenTrabajoID
			,ot.FechaOrden
			,ot.NumeroOrden
			,t.Nombre
			,eo.Nombre

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerProyectoCaratulaOdt]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerProyectoCaratulaOdt
GO
-- =============================================
-- Author:		Ivan Hernandez Marchand
-- Create date: 08/Octubre/2010
-- Description:	Para el reporte de carátula de ODT
-- =============================================
CREATE PROCEDURE Rpt_ObtenerProyectoCaratulaOdt
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	odt.NumeroOrden,
			odt.FechaOrden,
			(SELECT TOP 1 Nombre FROM Taller t WHERE t.TallerID = odt.TallerID) [Taller],
			(SELECT TOP 1 Nombre FROM Proyecto proy WHERE proy.ProyectoID = odt.ProyectoID) [Proyecto]
	FROM	OrdenTrabajo odt
	WHERE	odt.OrdenTrabajoID = @OrdenTrabajoID

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerResumenMateriales]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerResumenMateriales
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 12/Octubre/2010
-- Description:	Para obtener informacion de resumen de materiales
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerResumenMateriales]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @Congelados table
	(
		NumeroUnicoCongeladoID int,
		Consecutivo int
	);

	insert into @Congelados
	select	t.NumeroUnicoCongeladoID,
			row_number() over(order by t.NumeroUnicoCongeladoID)
	from
	(
		select distinct otm.NumeroUnicoCongeladoID
		from OrdenTrabajoMaterial otm
		where otm.OrdenTrabajoSpoolID in
		(
			select OrdenTrabajoSpoolID
			from OrdenTrabajoSpool
			where OrdenTrabajoID = @OrdenTrabajoID
		)
		and otm.CongeladoEsEquivalente = 1
	) t	

	SELECT	ic.Codigo
		   ,ic.DescripcionEspanol
		   ,ic.TipoMaterialID
		   ,ms.Diametro1
		   ,ms.Diametro2
		   ,ms.Grupo
		   ,ms.Cantidad
		   ,otm.OrdenTrabajoMaterialID
		   ,otm.CongeladoEsEquivalente
		   ,otm.NumeroUnicoCongeladoID
		   ,otm.NumeroUnicoSugeridoID
		   ,otm.SegmentoSugerido
		   ,(SELECT c.Consecutivo FROM @Congelados c WHERE c.NumeroUnicoCongeladoID = otm.NumeroUnicoCongeladoID) [Consecutivo]
	FROM	MaterialSpool ms
	INNER JOIN ItemCode ic on ic.ItemCodeID = ms.ItemCodeID
	INNER JOIN OrdenTrabajoMaterial otm on otm.MaterialSpoolID = ms.MaterialSpoolID
	INNER JOIN OrdenTrabajoSpool ots ON ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID
	WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
	
	
END

/*
	exec [Rpt_ObtenerResumenMateriales] 41
	update OrdenTrabajoMaterial set CongeladoEsEquivalente = 1 where OrdenTrabajoMaterialID in (3580,3581,3554)
	--15955
	update OrdenTrabajoMaterial set CongeladoEsEquivalente = 1, NumeroUnicoCongeladoID = 21765 where OrdenTrabajoMaterialID in (3551)
*/


GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerSpoolsCaratulaOdt]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerSpoolsCaratulaOdt
GO
-- =============================================
-- Author:		Ivan Hernandez Marchand
-- Create date: 08/Octubre/2010
-- Description:	Para el reporte de carátula de ODT
-- =============================================
CREATE PROCEDURE Rpt_ObtenerSpoolsCaratulaOdt
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	ots.NumeroControl,
			ots.Partida,
			ots.OrdenTrabajoSpoolID,
			s.SpoolID,
			s.Nombre,
			s.Dibujo,
			s.Area,
			s.Especificacion,
			s.Pdis,
			s.Peso,
			s.RevisionCliente,
			s.Revision,
			(	SELECT	SUM(isnull(js.Peqs,0))
				FROM OrdenTrabajoJunta otj
				INNER JOIN JuntaSpool js on otj.JuntaSpoolID = js.JuntaSpoolID
				WHERE otj.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
			) [Peqs],
			(
				SELECT	CASE	WHEN COUNT(odtm.CongeladoEsEquivalente) > 0 THEN CAST (1 as bit)
								ELSE CAST(0 as bit) END
				FROM	OrdenTrabajoMaterial odtm
				WHERE	odtm.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
						AND odtm.CongeladoEsEquivalente = 1
			) [TieneEquivalencia]
	FROM	OrdenTrabajoSpool ots
	INNER JOIN Spool s on ots.SpoolID = s.SpoolID
	WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
	
END

/*
	exec Rpt_ObtenerSpoolsCaratulaOdt 41
*/

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Armado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Armado
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 29/Noviembre/2010
-- DescriptiON:	información del armado.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Armado]
(
	@ProyectoID int,
	@FechaInicial datetime,
	@FechaFinal datetime
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	SELECT s.Dibujo AS [Isometrico]
			,s.Revision
			,odt.NumeroOrden AS [OrdenTrabajo]
			,jws.EtiquetaJunta
			,p.Nombre AS [NombreProyecto]
			,p.FechaInicio
			,js.Diametro
			,js.Cedula
			,ja.FechaArmado
			,ja.Observaciones
			,tub.Codigo
			,tub.Nombre AS [NombreTubero]
			,tub.ApPaterno AS [ApPatTubero]
			,tub.ApMaterno AS [ApMatTubero]
			,t.Nombre AS [TallerNombre]
			,nu1.Codigo AS [NumeroUnico1]
			,nu2.Codigo AS [NumeroUnico2]
			FROM JuntaWorkstatus jws
			INNER JOIN OrdenTrabajoSpool ots
			ON ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
			INNER JOIN JuntaSpool js
			ON js.JuntaSpoolID = jws.JuntaSpoolID
			INNER JOIN Spool s
			ON s.SpoolID = ots.SpoolID
			INNER JOIN OrdenTrabajo odt
			ON odt.OrdenTrabajoID = ots.OrdenTrabajoID
			INNER JOIN Proyecto p 	
			ON p.ProyectoID = odt.ProyectoID
			INNER JOIN JuntaArmado ja
			ON ja.JuntaArmadoID = jws.JuntaArmadoID
			INNER JOIN Taller t
			ON t.TallerID = ja.TallerID
			INNER JOIN Tubero tub
			ON tub.TuberoID = ja.TuberoID
			INNER JOIN NumeroUnico nu1
			ON nu1.NumeroUnicoID = ja.NumeroUnico1ID
			INNER JOIN NumeroUnico nu2
			ON nu2.NumeroUnicoID = ja.NumeroUnico2ID
			where p.ProyectoID = @ProyectoID			
			AND jws.ArmadoAprobado = 1
			AND jws.JuntaFinal = 1
			AND ja.FechaArmado >= @FechaInicial AND ja.FechaArmado <= @FechaFinal

END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Durezas]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Durezas
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 03 / 12 / 2010
-- DescriptiON:	información del durezas de spool
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Durezas]
(
	@ProyectoID int,
	@NumeroReporte varchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select  rt.ProyectoID
		,rt.FechaReporte
		,p.Nombre as [NombreProyecto]
		,p.Descripcion as [DescripcionProyecto]
		,rt.NumeroReporte
		,jrt.Aprobado
		,jrt.Hoja
		,jrt.Observaciones
		,jrt.FechaTratamiento
		,jws.EtiquetaJunta
		,js.Diametro
		,js.Cedula
		,js.Espesor
		,fa1.Nombre as [FamiliaMaterial1]
		,fa2.Nombre as [FamiliaMaterial2]
		,s.Dibujo as [Isometrico]		
		from  ReporteTt rt
		inner join Proyecto p on p.ProyectoID = rt.ProyectoID		
		inner join JuntaReporteTt jrt on jrt.ReporteTtID = rt.ReporteTtID
		inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jrt.JuntaWorkstatusID
		inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
		inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
		left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
		inner join Spool s on s.SpoolID = ots.SpoolID
		where rt.ProyectoID = @ProyectoID
		and rt.NumeroReporte = @NumeroReporte
		and rt.TipoPruebaID = 4
	
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Embarque]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Embarque
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 06 / 12 / 2010
-- DescriptiON:	información del embarque
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Embarque]
(
	@ProyectoID int,
	@NumeroEmbarque nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.Nombre as [ProyectoNombre]
		,e.NumeroEmbarque
		,e.FechaEmbarque
		,ots.NumeroControl
		,s.Peso
		,s.Area
		,s.Revision
		,s.Nombre
		,s.Dibujo
		from Embarque e
		inner join Proyecto p on p.ProyectoID = e.ProyectoID
		inner join EmbarqueSpool es on es.EmbarqueID = e.EmbarqueID
		inner join WorkstatusSpool ws on ws.WorkstatusSpoolID = es.WorkstatusSpoolID
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		inner join Spool s on s.SpoolID = ots.SpoolID
		where e.NumeroEmbarque = @NumeroEmbarque
		and e.ProyectoID = @ProyectoID
	
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Espesores]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Espesores
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 1 / Diciembre / 2010
-- DescriptiON:	información de la liberacion dimensional de espesores.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Espesores]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	select p.ProyectoID
		,p.Nombre as [NombreProyecto]
		,rd.NumeroReporte
		,rd.FechaReporte
		,rdd.Observaciones
		,rdd.Aprobado
		,rdd.Hoja
		,ots.NumeroControl
		,s.Dibujo
		,s.Nombre as [NombreSpool]
		,s.Especificacion
		from ReporteDimensional rd
		inner join Proyecto p on p.ProyectoID = rd.ProyectoID
		inner join ReporteDimensionalDetalle rdd	
		on rdd.ReporteDimensionalID = rdd.ReporteDimensionalID
		inner join WorkstatusSpool ws
		on ws.WorkstatusSpoolID = rdd.WorkstatusSpoolID
		inner join OrdenTrabajoSpool ots
		on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		inner join Spool s
		on s.SpoolID = ots.SpoolID
		where rd.ProyectoID = @ProyectoID
		and rd.NumeroReporte = @NumeroReporte
		and rd.TipoReporteDimensionalID = 2
	
	END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_InspeccionVisual]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_InspeccionVisual
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 30/Noviembre/2010
-- DescriptiON:	información de la inspección visual.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_InspeccionVisual]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	select p.ProyectoID
	,p.Nombre as [NombreProyecto]
	,iv.NumeroReporte 
	,iv.FechaReporte
	,jiv.Aprobado
	,jiv.Hoja
	,jiv.FechaInspeccion
	,jiv.Observaciones
	,jws.EtiquetaJunta
	,ots.NumeroControl
	,s.Nombre as [NombreSpool]
	,s.Dibujo as [Isometrico]
	,s.Segmento1
	,s.Segmento2
	,s.Segmento3
	,s.Segmento4
	,s.Segmento5
	,s.Segmento6
	,s.Segmento7
	,s.Revision
	,js.Diametro
	,js.Espesor
	,fa1.Nombre as [FamiliaMaterial1]
	,fa2.Nombre as [FamiliaMaterial2]
	,prai.Codigo as [ProcesoRaiz]
	,prel.Codigo as [ProcesoRel]
	from InspeccionVisual iv
	inner join JuntaInspeccionVisual jiv	
	on jiv.InspeccionVisualID = iv.InspeccionVisualID
	inner join Proyecto p
	on p.ProyectoID = iv.ProyectoID
	inner join JuntaWorkstatus jws 
	on jws.JuntaWorkstatusID = jiv.JuntaWorkstatusID
	inner join OrdenTrabajoSpool ots	
	on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
	inner join Spool s
	on s.SpoolID = ots.SpoolID
	inner join JuntaSpool js
	on js.JuntaSpoolID = jws.JuntaSpoolID
	inner join FamiliaAcero fa1
	on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
	left join FamiliaAcero fa2
	on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
	inner join JuntaSoldadura jsol
	on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
	inner join ProcesoRaiz prai
	on prai.ProcesoRaizID = jsol.ProcesoRaizID
	inner join ProcesoRelleno prel
	on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
	where iv.ProyectoID = @ProyectoID
	and iv.NumeroReporte = @NumeroReporte
	
	
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_LiberacionDimensional]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_LiberacionDimensional
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 1 / Diciembre / 2010
-- DescriptiON:	información del reporte de liberación Dimensional.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_LiberacionDimensional]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	select rd.ProyectoID
		,rd.FechaReporte
		,p.Nombre as [ProyectoNombre]
		,rd.NumeroReporte
		,rdd.Aprobado
		,rdd.FechaLiberacion
		,rdd.Hoja
		,rdd.Observaciones
		,ots.NumeroControl
		,s.Dibujo
		,s.Especificacion
		,s.Nombre
		from ReporteDimensional rd
		inner join Proyecto p on p.ProyectoID = rd.ProyectoID
		inner join ReporteDimensionalDetalle rdd
		on rdd.ReporteDimensionalID = rd.ReporteDimensionalID
		inner join WorkstatusSpool ws
		on ws.WorkstatusSpoolID = rdd.WorkstatusSpoolID
		inner join OrdenTrabajoSpool ots
		on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		inner join Spool s
		on s.SpoolID = ots.SpoolID
		where rd.ProyectoID = @ProyectoID
		and rd.NumeroReporte = @NumeroReporte
		and rd.TipoReporteDimensionalID = 1
		
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_PruebaPT]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_PruebaPT
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 2 / Diciembre / 2010
-- DescriptiON:	información del reporte prueba PT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_PruebaPT]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select distinct p.ProyectoID
	,p.Nombre as [NombreProyecto]
	,rp.NumeroReporte
	,rp.FechaReporte
	,jrp.Aprobado
	,jrp.Hoja
	,jrp.FechaPrueba
	,jrp.Observaciones
	,jws.EtiquetaJunta
	,js.Espesor
	,js.Cedula
	,js.Diametro
	,fa1.Nombre as [FamiliaMaterial1]
	,fa2.Nombre as [FamiliaMaterial2]
	,s.Nombre as [SpoolNombre]
	,s.Dibujo
	,s.Revision
	,ots.NumeroControl
	,ot.NumeroOrden
	--,prai.Codigo as [ProcesoRaiz]
	--,prel.Codigo as [ProcesoRelleno]
	--,nS1.CodigoSoldadorRelleno
	--,nS.CodigoSoldadorRaiz
	from ReportePnd rp
	inner join Proyecto p on p.ProyectoID = rp.ProyectoID
	inner join JuntaReportePnd jrp on jrp.ReportePndID = rp.ReportePndID
	inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jrp.JuntaWorkstatusID
	inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
	inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
	left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
	inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
	inner join OrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
	inner join Spool s on s.SpoolID = ots.SpoolID
	inner join JuntaSoldadura jsol on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
	inner join JuntaSoldaduraDetalle jsd on jsd.JuntaSoldaduraID = jsol.JuntaSoldaduraID
	inner join ProcesoRaiz prai on prai.ProcesoRaizID = jsol.ProcesoRaizID
	inner join ProcesoRelleno prel on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
	LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(s1.Codigo,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				on s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				and jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS CodigoSoldadorRaiz
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID		
			) nS on nS.JuntaSoldaduraID = jsol.JuntaSoldaduraID	
	LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(s1.Codigo,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				on s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				and jsd.TecnicaSoldadorID = 2
				FOR XML PATH ('')),1,2,'') AS CodigoSoldadorRelleno
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID		
			) nS1 on nS1.JuntaSoldaduraID = jsol.JuntaSoldaduraID
	where rp.ProyectoID = @ProyectoID
	and rp.NumeroReporte = @NumeroReporte
	and rp.TipoPruebaID = 2

	
	END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_PruebaRT]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_PruebaRT
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 2 / Diciembre / 2010
-- DescriptiON:	información del reporte prueba RT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_PruebaRT]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.ProyectoID
		,p.Nombre as [NombreProyecto]
		,rpnd.NumeroReporte
		,rpnd.FechaReporte
		,jrp.Hoja
		,jrp.Observaciones
		,jrp.FechaPrueba
		,jrp.Aprobado
		,jws.EtiquetaJunta
		,js.Diametro
		,js.Cedula
		,js.Espesor
		,fa1.Nombre as [FamiliaMaterial1]
		,fa2.Nombre as [FamiliaMaterial2]
		,ots.NumeroControl
		,ot.NumeroOrden
		,s.Nombre as [NombreSpool]
		,s.Dibujo as [Isometrico]
		,s.Revision
		,prai.Codigo as [ProcesoRaiz]
		,prel.Codigo as [ProcesoRelleno]
		from ReportePnd rpnd
		inner join Proyecto p on p.ProyectoID = rpnd.ProyectoID
		inner join JuntaReportePnd jrp on jrp.ReportePndID = rpnd.ReportePndID
		inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jrp.JuntaWorkstatusID
		inner join JuntaSpool js
		on js.JuntaSpoolID = jws.JuntaSpoolID
		inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
		left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
		inner join JuntaSoldadura jsol
		on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
		inner join ProcesoRaiz prai on prai.ProcesoRaizID = jsol.ProcesoRaizID
		inner join ProcesoRelleno prel
		on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
		inner join OrdenTrabajoSpool ots
		on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
		inner join OrdenTrabajo ot
		on ot.OrdenTrabajoID = ots.OrdenTrabajoID
		inner join Spool s
		on s.SpoolID = ots.SpoolID
		where rpnd.ProyectoID = @ProyectoID
		and rpnd.NumeroReporte = @NumeroReporte
		and rpnd.TipoPruebaID = 1
	
	END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Requisicion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Requisicion
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 29/Noviembre/2010
-- DescriptiON:	información del armado.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Requisicion]
(
	@ProyectoID int,
	@NumeroRequisicion nvarchar(50),
	@TipoPrueba int
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select  distinct p.ProyectoID as [ProyectoID]
		,p.Nombre as [nombreProyecto]
		,r.CodigoAsme
		,r.NumeroRequisicion
		,s.Dibujo as [Isometrico]
		,s.Revision 
		,s.Segmento1
		,s.Segmento2
		,s.Segmento3
		,s.Segmento4
		,s.Segmento5
		,s.Segmento6
		,s.Segmento7
		,jws.EtiquetaJunta
		,js.Diametro
		,js.Espesor
		,r.Observaciones
		,fa1.Nombre as [FamiliaMaterial1]
		,fa2.Nombre as [FamiliaMaterial2]
		,prai.Codigo as [ProcesoRaiz]
		,prel.Codigo as [ProcesoRel]
		,nS.NombreSoldadoresRaiz
		,nS1.NombreSoldadoresRelleno
		from Requisicion r
		inner join JuntaRequisicion jr on jr.RequisicionID = r.RequisicionID
		inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jr.JuntaWorkstatusID
		inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID 
		inner join Spool s on s.SpoolID = js.SpoolID
		inner join Proyecto p on p.ProyectoID = s.ProyectoID
		inner join FamiliaAcero fa1 on js.FamiliaAceroMaterial1ID = fa1.FamiliaAceroID
		left join FamiliaAcero fa2 on js.FamiliaAceroMaterial2ID = fa2.FamiliaAceroID
		inner join JuntaSoldadura jsol on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
		inner join JuntaSoldaduraDetalle jsd on jsd.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		inner join Soldador sol on sol.SoldadorID = jsd.SoldadorID
		inner join ProcesoRaiz prai on prai.ProcesoRaizID = jsol.ProcesoRaizID
		inner join ProcesoRelleno prel on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(s1.Nombre + ' ','') 
								   + COALESCE(s1.ApPaterno + ' ','') 
								   + COALESCE(s1.ApMaterno,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				on s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				and jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS NombreSoldadoresRaiz
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID		
			) nS on nS.JuntaSoldaduraID = jsol.JuntaSoldaduraID	
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(s1.Nombre + ' ','') 
								   + COALESCE(s1.ApPaterno + ' ','') 
								   + COALESCE(s1.ApMaterno,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				on s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				and jsd.TecnicaSoldadorID = 2
				FOR XML PATH ('')),1,2,'') AS NombreSoldadoresRelleno
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID		
			) nS1 on nS1.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		WHERE r.ProyectoID = @ProyectoID
		and r.NumeroRequisicion = @NumeroRequisicion
		and r.TipoPruebaID = @TipoPrueba
		
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Soldadura]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Soldadura
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 29/Noviembre/2010
-- DescriptiON:	información del armado.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Soldadura]
(
	@ProyectoID int,
	@FechaInicial datetime,
	@FechaFinal datetime
)
AS 
BEGIN
	SET NOCOUNT ON;
	select  p.Nombre as [NombreProyecto]
		,s.Dibujo as [Isometrico]
		,s.Revision as [RevisionSpool]
		,ot.NumeroOrden as [OrdenTrabajo]
		,jws.EtiquetaJunta
		,t.Nombre [NombreTaller]
		,js.Diametro
		,js.Cedula
		,fa1.Nombre [FamiliaMaterial1]
		,fa2.Nombre [FamiliaMaterial2]
		,jsol.FechaSoldadura 
		,jsol.Observaciones
		,pr.Codigo as [ProcesoRaiz]
		,prel.Codigo as [ProcesoRelleno]
		,nS.NombreSoldadoresRaiz
		,nS1.NombreSoldadoresRelleno
		,nS2.CodigosConsumibles as [ConsumiblesRaiz]
		,nS3.CodigosConsumibles as [ConsumiblesRelleno]
		from JuntaWorkstatus jws
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
		inner join JuntaArmado ja on ja.JuntaWorkstatusID = jws.JuntaWorkstatusID
		inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
		inner join JuntaSoldadura jsol on jsol.JuntaSoldaduraID = jws.JuntaWorkstatusID
		inner join Spool s on s.SpoolID = ots.SpoolID
		inner join OrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
		inner join Proyecto p on p.ProyectoID = s.ProyectoID
		inner join Taller t on t.TallerID = ja.TallerID
		inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
		left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
		inner join ProcesoRaiz pr on pr.ProcesoRaizID = jsol.ProcesoRaizID
		inner join ProcesoRelleno prel on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
		inner join JuntaSoldaduraDetalle jsd on jsd.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(c1.Codigo + ' ','')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Consumible c1 
				ON c1.ConsumibleID = jsd.ConsumibleID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				AND jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS CodigosConsumibles
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Consumible c
				ON c.ConsumibleID = jsd1.ConsumibleID
				GROUP BY jsd1.JuntaSoldaduraID
			) nS2 on nS2.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(c1.Codigo + ' ','')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Consumible c1 
				ON c1.ConsumibleID = jsd.ConsumibleID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				AND jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS CodigosConsumibles
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Consumible c
				ON c.ConsumibleID = jsd1.ConsumibleID
				GROUP BY jsd1.JuntaSoldaduraID
			) nS3 on nS3.JuntaSoldaduraID = jsol.JuntaSoldaduraID		
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(s1.Nombre + ' ','') 
								   + COALESCE(s1.ApPaterno + ' ','') 
								   + COALESCE(s1.ApMaterno,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				ON s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				AND jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS NombreSoldadoresRaiz
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID
			) nS on nS.JuntaSoldaduraID = jsol.JuntaSoldaduraID	
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(s1.Nombre + ' ','') 
								   + COALESCE(s1.ApPaterno + ' ','') 
								   + COALESCE(s1.ApMaterno,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				ON s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				AND jsd.TecnicaSoldadorID = 2
				FOR XML PATH ('')),1,2,'') AS NombreSoldadoresRelleno
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID
			) nS1 on nS1.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		where p.ProyectoID = @ProyectoID
		and jsol.FechaSoldadura >= @FechaInicial 
		and jsol.FechaSoldadura <= @FechaFinal
		and jws.JuntaFinal = 1
		and jws.SoldaduraAprobada = 1
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_TT]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_TT
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 03 / 12 / 2010
-- DescriptiON:	información del tratamiento térmico - PWHT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_TT]
(
	@ProyectoID int,
	@NumeroTratamiento nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.ProyectoID
	,p.Nombre as [NombreProyecto]
	,rt.NumeroReporte
	,rt.FechaReporte
	,jrt.Hoja
	,jrt.Aprobado
	,jrt.FechaTratamiento
	,jrt.Observaciones
	,jws.EtiquetaJunta
	,js.Cedula
	,js.Diametro
	,js.Espesor
	,s.Nombre as [NombreSpool]
	,s.Dibujo
	,s.Revision
	,s.Especificacion
	,fa1.Nombre as [FamiliaMaterial1]
	,fa2.Nombre as [FamiliaMaterial2]
	,ots.NumeroControl
	from ReporteTt rt
	inner join Proyecto p on p.ProyectoID = rt.ProyectoID
	inner join JuntaReporteTt jrt on jrt.ReporteTtID = rt.ReporteTtID
	inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jrt.JuntaWorkstatusID
	inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
	inner join Spool s on s.SpoolID = js.SpoolID
	inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
	left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
	inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
	where rt.ProyectoID = @ProyectoID
	and rt.NumeroReporte = @NumeroTratamiento
	and rt.TipoPruebaID = 3

END


GO


