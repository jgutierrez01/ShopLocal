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
				11/08/2011 SACB
				22/11/2011 LMG -- correccion de query para tomar en cuenta item codes que no tengan numero unico aun
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
		DescripcionEspanol nvarchar(MAX),
		DescripcionIngles nvarchar(MAX)
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
			 UNION
	SELECT 	ic.ItemCodeID ,
			ms.Diametro1,
			ms.Diametro2,
			0 [CantidadRecibida],
			0 [CantidadDanada],
			0 [InventarioFisico],
			0 [InventarioBuenEstado],
			0 [InventarioCongelado],
			0 [InventarioDisponibleCruce],
			0 [InventarioTransferenciaCorte],
			0 [TotalEntradaOtrosProcesos],			
			0 [TotalDespachado],
			0 [TotalDespachadoParaICE],
			0 [TotalCorte],
			0 [TotalMerma],
			0 [TotalOtrasSalidas],
			0 [TotalSalidasTemporales],
			0 [TotalCondicionada],
			0 [TotalRechazada],
			0 [TotalEnTransferenciaCorteICE],
			0 [TotalCortadoICE]
			from ItemCode ic 
			inner join MaterialSpool ms on ms.ItemCodeID = ic.ItemCodeID			
			where ic.proyectoid = @ProyectoID 
			and (select COUNT(*) from NumeroUnico nu where nu.ItemCodeID=ic.ItemCodeID and nu.Diametro1 = ms.Diametro1 and nu.Diametro2 = ms.Diametro2) = 0
			
	
	
	
	
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
		   --isnull(odt.Cantidad,0) + isnull(nu.TotalEnTransferenciaCorteICE, 0) + isnull(nu.TotalCortadoICE,0) - isnull(nu.InventarioTransferenciaCorte,0) -
		   --case when (isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0)- isnull(nu.TotalDespachadoParaICE,0))<0 then 0
		   --else isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0)- isnull(nu.TotalDespachadoParaICE,0) end - isnull(nu.TotalDespachado,0) - isnull(de.CantidadDespachadaEquivalente,0) [TotalPorDespachar],
		   isnull(odt.Cantidad,0) - isnull(nu.TotalDespachado,0) [TotalPorDespachar],
		   isnull(nu.InventarioBuenEstado,0) [InventarioFisico],
		   isnull(de.CantidadCongeladaEquivalente,0) - isnull(nu.TotalEnTransferenciaCorteICE, 0) - isnull(nu.TotalCortadoICE, 0) [CantidadCongeladaEquivalente],
		   isnull(nu.InventarioCongelado,0) - 
		   case when (isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0) - isnull(nu.TotalDespachadoParaICE,0))<0 then 0
		   else isnull(nu.TotalCorte,0) - isnull(nu.TotalDespachado,0) - isnull(nu.TotalDespachadoParaICE,0) end  [InventarioCongelado],
		   case when isnull(nu.InventarioDisponibleCruce,0) < 0 then 0
		   else ISNULL(nu.InventarioDisponibleCruce,0) end  - isnull(nu.TotalCondicionada,0) - isnull(nu.TotalRechazada,0)  [InventarioDisponibleCruce],
		   case when isnull(iceq.InventarioDisponibleCruceEquivalente,0) < 0 then 0
		   else isnull(iceq.InventarioDisponibleCruceEquivalente,0) end [InventarioDisponibleCruceEquivalente],
		   case when isnull(nu.InventarioDisponibleCruce,0) < 0 then 0
		   else ISNULL(nu.InventarioDisponibleCruce,0) end +
		   case when isnull(iceq.InventarioDisponibleCruceEquivalente,0) < 0 then 0
		   else isnull(iceq.InventarioDisponibleCruceEquivalente,0)  end - isnull(nu.TotalCondicionada,0) -  isnull(nu.TotalRechazada,0) [TotalDisponibleCruce]		   	  
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
				 isnull(SUM(nui.InventarioDisponibleCruce),0) - isnull(SUM(nui.TotalCondicionada),0) - isnull(SUM(nui.TotalRechazada),0)  [InventarioDisponibleCruceEquivalente],
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

