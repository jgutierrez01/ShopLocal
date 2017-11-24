CREATE NONCLUSTERED INDEX [IX_JuntaSoldaduraDetalle_CombinadoConsumible] ON [dbo].[JuntaSoldaduraDetalle] ([JuntaSoldaduraID], [ConsumibleID])
GO

CREATE NONCLUSTERED INDEX [IX_JuntaSoldaduraDetalle_CombinadoSoldadura] ON [dbo].[JuntaSoldaduraDetalle] ([JuntaSoldaduraID], [SoldadorID], [TecnicaSoldadorID])
GO

DROP PROCEDURE ObtenerSeguimientoDeJuntas
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


/****************************************************************************************
	Nombre:		ObtenerSeguimientoDeJuntasPorProyecto
	Funcion:	Trae toda la informacion necesaria para el seguimiento de jutnas
	Parametros:	@ProyectoID INT
				@HistorialRep BIT
	Autor:		MMG
	Modificado:	28/01/2011 IHM
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
			INNER JOIN #TempProyecto p on p.ProyectoID = @ProyectoID
			INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
			INNER JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
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


DECLARE @ModuloSeguimientoJuntaID int

SELECT	@ModuloSeguimientoJuntaID = ModuloSeguimientoJuntaID
FROM	ModuloSeguimientoJunta
WHERE	[Nombre] = 'General'

INSERT INTO [dbo].[CampoSeguimientoJunta] ([ModuloSeguimientoJuntaID], [Nombre], [NombreIngles], [OrdenUI], [NombreControlUI], [NombreColumnaSp], [DataFormat], [CssColumnaUI], [UsuarioModifica], [FechaModificacion], [AnchoUI]) VALUES (@ModuloSeguimientoJuntaID, N'IC Mat1', N'IC Mat1', 13, N'', N'CodigoItemCodeMaterial1', N'', N'', NULL, NULL, 125)
INSERT INTO [dbo].[CampoSeguimientoJunta] ([ModuloSeguimientoJuntaID], [Nombre], [NombreIngles], [OrdenUI], [NombreControlUI], [NombreColumnaSp], [DataFormat], [CssColumnaUI], [UsuarioModifica], [FechaModificacion], [AnchoUI]) VALUES (@ModuloSeguimientoJuntaID, N'Desc Mat 1', N'Desc Mat 1', 13, N'', N'DescripcionItemCodeMaterial1', N'', N'', NULL, NULL, 375)
INSERT INTO [dbo].[CampoSeguimientoJunta] ([ModuloSeguimientoJuntaID], [Nombre], [NombreIngles], [OrdenUI], [NombreControlUI], [NombreColumnaSp], [DataFormat], [CssColumnaUI], [UsuarioModifica], [FechaModificacion], [AnchoUI]) VALUES (@ModuloSeguimientoJuntaID, N'IC Mat2', N'IC Mat2', 13, N'', N'CodigoItemCodeMaterial2', N'', N'', NULL, NULL, 125)
INSERT INTO [dbo].[CampoSeguimientoJunta] ([ModuloSeguimientoJuntaID], [Nombre], [NombreIngles], [OrdenUI], [NombreControlUI], [NombreColumnaSp], [DataFormat], [CssColumnaUI], [UsuarioModifica], [FechaModificacion], [AnchoUI]) VALUES (@ModuloSeguimientoJuntaID, N'Desc Mat 2', N'Desc Mat 2', 13, N'', N'DescripcionItemCodeMaterial2', N'', N'', NULL, NULL, 375)

UPDATE [dbo].[CampoSeguimientoJunta] SET [DataFormat]=N'0#.###' WHERE [Nombre] = 'Diámetro'
