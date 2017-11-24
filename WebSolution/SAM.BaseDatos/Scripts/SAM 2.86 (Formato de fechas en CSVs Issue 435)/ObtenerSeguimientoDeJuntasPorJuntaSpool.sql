IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSeguimientoDeJuntasPorJuntaSpool]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorJuntaSpool]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[ObtenerSeguimientoDeJuntasPorJuntaSpool]
	Funcion:	Trae toda la informacion necesaria para el seguimiento de jutnas
	Parametros:	@JuntaSpoolID INT				
	Autor:		SCB
	Modificado:	08/16/2011 IHM
				31/08/2011 PEGV
				06/09/2011 PEGV
				27/03/2013 BGG
				24/01/2014 JHT -- Agrego campo DiametroMayor
				07/02/2014 JHT -- Agrego campo CuadranteID
				11/03/2014 JHT -- Liberación calidad
				18/03/2014 GTG -- Agrego Fecha Emision de OT
				10/04/2014 GTG -- Vigencia Aduana
                                Bernardo Garza, 13/05/2014 -- cambios en cálculo de % armado y soldadura
				05/06/2014 GTG -- agregar campo RequierePwht
				06/06/2014 JHT -- Nuevo campo Liberacion Materiales /// Nuevo campo Liberado Pintura
				13/06/2014 JHT -- Nuevo Campo Ok PND
				01/07/2014 GTG -- Modificacion de insert into #TempInspeccionDimensional
				07/08/2014 GTG -- Nuevo Modulo PruebaNeumatica
				07/07/2014 GTG -- Nuevos campos PruebaHidrostatica y pruebaNeumatica
				13/05/2014 JHT -- Nuevo campo "Presion" ReporteSpoolPND
				GTG 23/07/2014 -- Nuevos campos en modulo Agrupadores
				05/08/2014 JHT -- Nuevos cambios para los agrupadores por Junta
				25/07/2014 JHT -- Cambios al campo de requiere prueba hidrostatica
				GTG 23/07/2014 -- Nuevos campos en modulo Agrupadores
				GTG 28/07/2014 -- nuevo campo modulo general FechaLocalizacion
				GTG 15/08/2014 -- cambio consulta pintura spool
				JME 13/10/2014 -- cambio formato fecha
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerSeguimientoDeJuntasPorJuntaSpool]
(
	@JuntaSpoolID INT
)
AS
BEGIN



	SET NOCOUNT ON; 

	declare @TH int
	select @TH = TipoJuntaID 
	from TipoJunta where Codigo = 'TH'

	declare @TW int
	select @TW = TipoJuntaID 
	from TipoJunta where Codigo = 'TW'

	declare @SHOP int
	select @SHOP = FabAreaID 
	from FabArea where Codigo = 'SHOP'

	declare @FIELD int
	select @FIELD = FabAreaID 
	from FabArea where Codigo = 'FIELD'

	-- Tabla de avance
	CREATE TABLE #TempAvance
	(
		SpoolID int,
		TotalJuntasShop int,
		TotalJuntasSoldables int,
		TotalJuntasArmadas int,
		TotalJuntasSoldadas int,
		PorcArmado decimal(10,2),
		PorcSoldado decimal(10,2)
	)			

	--TABLA PROYECTO
	CREATE TABLE #TempProyecto
	(	
		ProyectoID INT,
		Patio INT,
		Nombre NVARCHAR(100)
	)

	--TABLA SPOOL TRAMO RECTO
	CREATE TABLE #SpoolTramoRecto
	(
		SpoolID INT,
		TieneDespacho BIT,
		TieneOT BIT
	)

	--TABLA ORDEN DE TRABAJO
	CREATE TABLE #TempOrdenTrabajo
	(	
		OrdenTrabajoID INT,
		NumeroOrden NVARCHAR(50),
		FechaOrden DATETIME
	)

	--TABLA ORDEN TRABAJO SPOOL
	CREATE TABLE #TempOrdenTrabajoSpool
	(	
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoID INT,
		NumeroControl NVARCHAR(50),
		SpoolID INT,
		FechaLiberacionCalidad DATETIME,
		UsuarioliberacionCalidad VARCHAR(100),
		FechaLiberacionMaterial DATETIME,
		UsuarioLiberacionMaterial VARCHAR(100),
		FechaOkPnd DATETIME NULL,
		UsuarioOkPnd VARCHAR(100) NULL
	)

	--TABLA ORDEN TRABAJO MATERIAL
	CREATE TABLE #TempOrdenTrabajoMaterial
	(
		OrdenTrabajoMaterialID INT,
		OrdenTrabajoSpoolID INT,
		MaterialSpoolID INT,
		DespachoID INT,
		TieneInventarioCongelado BIT,
		TieneCorte BIT,
		TieneDespacho BIT,
		EsAsignado BIT,
		Cortador NVARCHAR(100),
		Despachador NVARCHAR(100)
	)

	-------------------TABLAS DE AGRUPADORES ---------------------------

	CREATE TABLE #TempAgrupadoresPorJunta(
		AgrupadorID INT NULL,
		JuntaSpoolID INT NULL,
		AgrupadoresPorJuntaClasifPND VARCHAR(500) NULL,
		AgrupadoresPorJuntaClasifReparacion VARCHAR(500) NULL,
		AgrupadoresPorJuntaClasifSoporte VARCHAR(500) NULL,
		AgrupadoresPorJuntaGrupo1 VARCHAR(500) NULL,
		AgrupadoresPorJuntaGrupo2 VARCHAR(500) NULL,
		AgrupadoresPorJuntaGrupo3 VARCHAR(500) NULL,
		AgrupadoresPorJuntaGrupo4 VARCHAR(500) NULL,
		AgrupadoresPorJuntaGrupo5 VARCHAR(500) NULL
	)

	-------------------- AGRUPADORES --------------------------------------
	--TABLA SPOOL
	CREATE TABLE #TempSpool 
	(	
		SpoolID INT,
		Nombre NVARCHAR(50),
	    PorcentajePnd INT,
	    Especificacion VARCHAR(15),
	    Prioridad INT,
	    RevisionStg nvarchar(10),
	    RevisionCte nvarchar(10),
	    Isometrico nvarchar(50),
		SistemaPintura nvarchar(100),
	    ColorPintura nvarchar(100),
	    CodigoPintura nvarchar(100)	 ,
	    DiametroPlano DECIMAL(10,4),
		DiametroMayor DECIMAL(10,4),
	    FechaEtiquetaEmbarque DATETIME,
	    NumeroEtiquetaEmbarque NVARCHAR(20),
	    RequierePwht BIT,		
		UltimaLocalizacion NVARCHAR(100),
	    Campo1 NVARCHAR(100),
	    Campo2 NVARCHAR(100),
	    Campo3 NVARCHAR(100),
	    Campo4 NVARCHAR(100),
	    Campo5 NVARCHAR(100),
		RequierePruebaHidrostatica VARCHAR(10),
		UltimoProceso NVARCHAR(500) NULL,
		AreaGrupo NVARCHAR(500) NULL,
		KgsGrupo NVARCHAR(500) NULL,
		DiamGrupo NVARCHAR(500) NULL,
		PeqGrupo NVARCHAR(500) NULL,
		SistemaPinturaFinal NVARCHAR(500) NULL,
		Paint NVARCHAR(500) NULL,		
		DiametroPromedio NVARCHAR(500) NULL,		
		PaintLine NVARCHAR(500) NULL,
		AreaEq NVARCHAR(500) NULL,
		Inox NVARCHAR(500) NULL,
		ClasifInox NVARCHAR(500) NULL,
		FechaLocalizacion DATETIME 
	)

	--TABLA JUNTA SPOOL
	CREATE TABLE #TempJuntaSpool 
	(	
		EtiquetaJunta NVARCHAR(10),
		JuntaSpoolID INT,
		MaterialSpool1ID INT,
		MaterialSpool2ID INT,
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
		ValorNumericoEtiqueta1 decimal(9,3),
		ValorNumericoEtiqueta2 decimal(9,3),
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(150),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(150),
		FabAreaID INT,
		FamiliaAcero1 VARCHAR(50),
		FamiliaMaterial1 VARCHAR(50),
		FamiliaAcero2 VARCHAR(50),
		FamiliaMaterial2 VARCHAR(50),
		DiamMat1 DECIMAL(7,4),
		EspMat1 VARCHAR(10),
		DiamMat2 DECIMAL(7,4),
		EspMat2 VARCHAR(10),
		CodigoFabArea nvarchar(20),
		EstacionID int null,
		EsManual bit null,
		FabClas NVARCHAR(150) null,
		CampoJunta2 NVARCHAR(150) null,
		CampoJunta3 NVARCHAR(150) null,
		CampoJunta4 NVARCHAR(150) null,
		CampoJunta5 NVARCHAR(150) null,
		RequierePwht bit null,
		RequierePruebaNeumatica bit null
	)

	--TABLA TempJuntaSpoolB
	CREATE TABLE #TempJuntaSpoolB 
	(	
		EtiquetaJunta NVARCHAR(10),
		JuntaSpoolID INT,
		MaterialSpool1ID INT,
		MaterialSpool2ID INT,
		SpoolID INT,
		TipoJunta NVARCHAR(50),
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
		ValorNumericoEtiqueta1 decimal(9,3),
		ValorNumericoEtiqueta2 decimal(9,3),
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(MAX),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(MAX),
		FabAreaID INT,
		FamiliaAcero1 VARCHAR(50),
		FamiliaMaterial1 VARCHAR(50),
		FamiliaAcero2 VARCHAR(50),
		FamiliaMaterial2 VARCHAR(50),
		DiamMat1 DECIMAL(7,4),
		EspMat1 VARCHAR(10),
		DiamMat2 DECIMAL(7,4),
		EspMat2 VARCHAR(10),
		CodigoFabArea nvarchar(20),
		EstacionID int null,
		EsManual bit null,
		FabClas NVARCHAR(150) null,
		CampoJunta2 NVARCHAR(150) null,
		CampoJunta3 NVARCHAR(150) null,
		CampoJunta4 NVARCHAR(150) null,
		CampoJunta5 NVARCHAR(150) null,
		RequierePwht bit null,
		RequierePruebaNeumatica bit null
	)

	--TABLA MATERIAL SPOOL
	CREATE TABLE #TempMaterialSpool
	(	
		MaterialSpoolID INT,
		SpoolID INT,
		ItemCodeID INT,
		Etiqueta NVARCHAR(10),
		CodigoItemCode NVARCHAR(50),
		DescripcionItemCode NVARCHAR(MAX),
		EtiquetaEsNumero BIT,
		ValorNumericoEtiqueta decimal(9,3),
		Diametro1 DECIMAL(7,4),
		Especificacion VARCHAR(10),
	)

	--TABLA JUNTA WORKSTATUS
	CREATE TABLE #TempJuntaWorkstatus 
	(	
		JuntaWorkStatusID INT,
		JuntaCampo BIT,
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
		SoldaduraJuntaCampo BIT,
		SoldaduraFecha DATETIME,
		SoldaduraFechaReporte DATETIME,
		SoldaduraTaller NVARCHAR(50),
		SoldaduraWPS NVARCHAR(50),
		SoldaduraWPSRelleno NVARCHAR(50),
		SoldaduraProcesoRelleno NVARCHAR(50),
		SoldaduraConsumiblesRelleno NVARCHAR(500),
		SoldaduraProcesoRaiz NVARCHAR(50),
		SoldaduraSoldadorRaiz NVARCHAR(50),
		SoldaduraSoldadorRelleno NVARCHAR(50),
		SoldaduraMaterialBase1 NVARCHAR(50),
		SoldaduraMaterialBase2 NVARCHAR(50),
		ObservacionesSoldadura NVARCHAR(500)
	)

	--TABLA JUNTA REPORTE TT
	CREATE TABLE #TempJuntaReporteTt 
	(	
		JuntaReporteTtID INT,
		JuntaWorkstatusID INT,
		JuntaCampo BIT,
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
		JuntaCampo BIT,
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
		FechaPreparacion DATETIME,
		LiberadoPintura BIT NULL
	)

	--TABLA JUNTA INSPECCION VISUAL
	CREATE TABLE #TempJuntaInspeccionVisual 
	(	
		InspeccionVisualJuntaInspeccionVisualID INT,
		InspeccionVisualJuntaWorkstatusID INT,
		InspeccionVisualJuntaCampo BIT,
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
		JuntaCampo bit,
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
		PruebaRTJuntaCampo BIT,
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
		FechaModificacion DATETIME,
		PruebaRTSector NVARCHAR(MAX),
		PruebaRTCuadrante NVARCHAR(MAX)
	)

	--TABLA PRUEBA PT
	CREATE TABLE #TempPruebaPT 
	(	
		PruebaPTJuntaReportePndID INT,
		PruebaPTJuntaWorkstatusID INT,
		PruebaPTJuntaCampo BIT,
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
		PruebaRTPostTTJuntaCampo BIT,
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

	--TABLA PRUEBA PMI
	CREATE TABLE #TempPruebaPMI 
	(	
		PruebaPMIJuntaReportePndID INT,
		PruebaPMIJuntaWorkstatusID INT,
		PruebaPMIJuntaCampo BIT,
		PruebaPMIFechaRequisicion DATETIME,
		PruebaPMINumeroRequisicion NVARCHAR(50),
		PruebaPMICodigoRequisicion NVARCHAR(50),
		PruebaPMIFechaPrueba DATETIME,
		PruebaPMIFechaReporte DATETIME,
		PruebaPMINumeroReporte NVARCHAR(50),
		PruebaPMIHoja INT,
		PruebaPMIResultado BIT,
		PruebaPMIDefecto NVARCHAR(MAX),
		PruebaPMIObservacionesReporte NVARCHAR(500),
		PruebaPMIObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)

	--TABLA PRUEBA PT(PostTT)
	CREATE TABLE #TempPruebaPTPostTT 
	(	
		PruebaPTPostTTJuntaReportePndID INT,
		PruebaPTPostTTJuntaWorkstatusID INT,
		PruebaPTPostTTJuntaCampo BIT,
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
		PruebaUTJuntaCampo BIT,
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

	--TABLA PRUEBA Neumatica
	CREATE TABLE #TempPruebaNeumatica
	(	
		PruebaNeumaticaJuntaReportePndID INT,
		PruebaNeumaticaJuntaWorkstatusID INT,
		PruebaNeumaticaJuntaCampo BIT,
		PruebaNeumaticaFechaRequisicion DATETIME,
		PruebaNeumaticaNumeroRequisicion NVARCHAR(50),
		PruebaNeumaticaCodigoRequisicion NVARCHAR(50),
		PruebaNeumaticaFechaPrueba DATETIME,
		PruebaNeumaticaFechaReporte DATETIME,
		PruebaNeumaticaNumeroReporte NVARCHAR(50),
		PruebaNeumaticaHoja INT,
		PruebaNeumaticaResultado BIT,
		PruebaNeumaticaDefecto NVARCHAR(MAX),
		PruebaNeumaticaSector NVARCHAR(MAX),
		PruebaNeumaticaCuadrante NVARCHAR(MAX),
		PruebaNeumaticaObservacionesReporte NVARCHAR(500),
		PruebaNeumaticaObservacionesRequisicion NVARCHAR(500),
		FechaModificacion DATETIME
	)

	--TABLA TRATAMIENTO PWHT
	CREATE TABLE #TempTratamientoPwht 
	(	
		TratamientoPwhtJuntaReporteTtID INT,
		TratamientoPwhtJuntaWorkstatusID INT,
		TratamientoPwhtJuntaRequisicionID INT,
		TratamientoPwhtJuntaCampo BIT,
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
		TratamientoDurezasJuntaCampo BIT,
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
		TratamientoPreheatJuntaCampo BIT,
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
		PinturaReportePullOff NVARCHAR(50),
		PinturaLiberado BIT NULL
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
		EmbarqueFechaEstimada DATETIME,
		EmbarqueNumeroEmbarque NVARCHAR(50),
		Nota1 NVARCHAR(50),
		Nota2 NVARCHAR(50),
		Nota3 NVARCHAR(50),
		Nota4 NVARCHAR(50),
		Nota5 NVARCHAR(50),
		EmbarqueVigenciaAduana DATETIME
	)

	--TABLA SPOOL REPORTE PND
	CREATE TABLE #TempSpoolReportePnd 
	(	
		SpoolReportePndID INT,
		WorkstatusSpoolID INT,
		TipoPruebaSpoolID INT,
		FechaReporte DATETIME,
		NumeroReporte NVARCHAR(50),
		SpoolRequisicionID INT,		
		FechaPrueba DATETIME,
		Hoja INT,
		Aprobado BIT,
		Observaciones NVARCHAR(500),
		FechaModificacion DATETIME,
		Presion VARCHAR(150)
	)

	--TABLA PRUEBA HIDROSTÁTICA
	CREATE TABLE #TempPruebaHidrostatica
	(	
		PruebaHidrostaticaSpoolReportePndID INT,
		PruebaHidrostaticaWorkstatusSpoolID INT,
		PruebaHidrostaticaFechaRequisicion DATETIME,
		PruebaHidrostaticaNumeroRequisicion NVARCHAR(50),
		PruebaHidrostaticaFechaPrueba DATETIME,
		PruebaHidrostaticaFechaReporte DATETIME,
		PruebaHidrostaticaNumeroReporte NVARCHAR(50),
		PruebaHidrostaticaHoja INT,
		PruebaHidrostaticaAprobado BIT,
		PruebaHidroPresion VARCHAR(150)
	)

	--TABLA GENERAL
	CREATE TABLE #TempGeneral 
	(
		GeneralJuntaWorkstatusID INT,
		GeneralJuntaCampo BIT,
		GeneralJuntaSpoolID INT,
		GeneralJuntaRequierePwht NVARCHAR(2),
		GeneralJuntaRequierePruebaNeumatica NVARCHAR(2),
		GeneralProyecto NVARCHAR(50),
		GeneralOrdenDeTrabajo NVARCHAR(50),
		GeneralFechaEmision DATETIME,
		GeneralNumeroDeControl NVARCHAR(50),
		GeneralSpool NVARCHAR(50),
		GeneralBaston NVARCHAR(50),
		GeneralEstacion NVARCHAR(50),
		GeneralSegundaFabricacion NVARCHAR(50),
		GeneralJunta NVARCHAR(50),
		GeneralTipoJunta NVARCHAR(50),
		GeneralDiametro DECIMAL(7,4),
		GeneralDiametroMayor DECIMAL(10,4),
		GeneralCedula NVARCHAR(10),
		GeneralEspesor DECIMAL(10,4),
		GeneralLocalizacion NVARCHAR(50),
		GeneralUltimaLocalizacion NVARCHAR(100),
		GeneralUltimoProceso NVARCHAR(50),
		GeneralTieneHold BIT,
		Campo1 NVARCHAR(100),
	    Campo2 NVARCHAR(100),
	    Campo3 NVARCHAR(100),
	    Campo4 NVARCHAR(100),
	    Campo5 NVARCHAR(100),
		GeneralPeqs DECIMAL(10,4),
		GeneralKgTeoricos DECIMAL(12,4),
		GeneralFabArea nvarchar(20),
		OrdenTrabajoSpoolID INT,
		ItemCodeIDMaterial1 INT,
		CodigoItemCodeMaterial1 NVARCHAR(50),
		DescripcionItemCodeMaterial1 NVARCHAR(MAX),
		ItemCodeIDMaterial2 INT,
		CodigoItemCodeMaterial2 NVARCHAR(50),
		DescripcionItemCodeMaterial2 NVARCHAR(MAX),
		GeneralFamiliaAcero1 VARCHAR(50),
		GeneralFamiliaMaterial1 VARCHAR(50),
		GeneralFamiliaAcero2 VARCHAR(50),
		GeneralFamiliaMaterial2 VARCHAR(50),
		GeneralPorcentajePnd INT,
		GeneralEspecificacion VARCHAR(15),
		GeneralDiamMat1 DECIMAL(7,4),
		GeneralEspMat1 VARCHAR(10),
		GeneralDiamMat2 DECIMAL(7,4),
		GeneralEspMat2 VARCHAR(10),
		PorcentajeArmado decimal(5,2),
		PorcentajeSoldado decimal(5,2),
		GeneralPrioridad int,
		GeneralIsometrico nvarchar(50),
		GeneralRevisionSteelgo nvarchar(10),
	    GeneralRevisionCliente nvarchar(10),
	    PinturaSistema VARCHAR(100),
		PinturaColor VARCHAR(100),
		PinturaCodigo VARCHAR(100),
		ObservacionesHold NVARCHAR(MAX),
		FechaHold DATETIME 		,
		GeneralDiametroPlano DECIMAL(10,4),		
		GeneralFechaEtiquetaEmbarque DATETIME,
		GeneralEtiquetaEmbarque NVARCHAR(20),
		GeneralMaterialPendienteSpool BIT,
		GeneralMaterialPendienteJunta BIT,
		GeneralDespachado1 NVARCHAR(50),
		GeneralDespachado2 NVARCHAR(50),
		SpoolID INT,
		GeneralPWHT BIT,
		GeneralRequierePruebaHidrostatica BIT NULL,
		FabClas NVARCHAR(150) null,
		CampoJunta2 NVARCHAR(150) null,
		CampoJunta3 NVARCHAR(150) null,
		CampoJunta4 NVARCHAR(150) null,
		CampoJunta5 NVARCHAR(150) null,
		GeneralFechaLiberacionCalidad DATETIME NULL,
		GeneralUsuarioLiberacionCalidad VARCHAR(100) NULL,
		GeneralFechaLiberacionMaterial DATETIME NULL,
		GeneralUsuarioLiberacionMaterial NVARCHAR(100) NULL,
		GeneralFechaOkPnd DATETIME NULL,
		GeneralUsuarioOkPnd VARCHAR(100) NULL,
		AgrupadoresSpoolUltimoProceso  VARCHAR(500) NULL,
		AgrupadoresAreaGrupo VARCHAR(500) NULL,
		AgrupadoresKgsGrupo VARCHAR(500) NULL,
		AgrupadoresDiamGrupo VARCHAR(500) NULL,
		AgrupadoresPeqGrupo VARCHAR(500) NULL,
		AgrupadoresSistemaPinturaFinal VARCHAR(500) NULL,
		AgrupadoresPaintNoPaint VARCHAR(500) NULL,
		AgrupadoresDiametroPromedio VARCHAR(500) NULL,
		AgrupadoresPaintLine VARCHAR(500) NULL,
		AgrupadoresAreaEQ VARCHAR(500) NULL,
		AgrupadoresInoxNoInox VARCHAR(500) NULL,
		AgrupadoresClasifInox VARCHAR(500) NULL,
		GeneralFechaLocalizacion DATETIME NULL
	)

	CREATE TABLE #TempTransferenciaSpool(
		SpoolID INT,
		NumeroTransferencia NVARCHAR(200),
		PreparacionTransferencia DATETIME,
		Transferencia DATETIME
	)

	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_JuntaWorkstatusID ON #TempJuntaWorkstatus(JuntaWorkStatusID, JuntaCampo)
	CREATE NONCLUSTERED INDEX IX_TempJuntaWorkstatus_OrdenTrabajoSpoolID ON #TempJuntaWorkstatus(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempJuntaSoldadura_SoldaduraJuntaWorkstatusID ON #TempJuntaSoldadura(SoldaduraJuntaWorkstatusID, SoldaduraJuntaCampo)
	CREATE NONCLUSTERED INDEX IX_TempJuntaInspeccionVisual_InspeccionVisualJuntaWorkstatusID ON #TempJuntaInspeccionVisual(InspeccionVisualJuntaWorkstatusID, InspeccionVisualJuntaCampo)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_OrdenTrabajoSpoolID ON #TempWorkstatusSpool(OrdenTrabajoSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempWorkstatusSpool_WorkstatusSpoolID ON #TempWorkstatusSpool(WorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempPintura_PinturaWorkstatusSpoolID ON #TempPintura(PinturaWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempEmbarque_EmbarqueWorkstatusSpoolID ON #TempEmbarque(EmbarqueWorkstatusSpoolID)
	CREATE NONCLUSTERED INDEX IX_TempGeneral_GeneralJuntaWorkstatusID ON #TempGeneral(GeneralJuntaWorkstatusID, GeneralJuntaCampo)

	--Comienzan los inserts a las tablas temporales con filtros si los tienen.
	DECLARE @OrdenTrabajoSpoolID INT

	--SELECT @OrdenTrabajoSpoolID = OrdenTrabajoSpoolID
	--FROM OrdenTrabajoJunta 
	--WHERE JuntaSpoolID = @JuntaSpoolID

	SELECT  @OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID from JuntaSpool js
	INNER JOIN OrdenTrabajoSpool ots on js.SpoolID = ots.SpoolID 
	WHERE JuntaSpoolID = @JuntaSpoolID


	--INSERT PROYECTO
	INSERT INTO #TempProyecto
		SELECT p.ProyectoID,
			   p.PatioID,
			   p.Nombre
		FROM JuntaSpool js 
		INNER JOIN Spool s ON s.SpoolID = js.SpoolID
		INNER JOIN Proyecto p ON s.ProyectoID = p.ProyectoID
		WHERE JuntaSpoolID = @JuntaSpoolID

	--INSERT SPOOL TRAMO RECTO
	INSERT INTO #SpoolTramoRecto
	SELECT s.SpoolID,
		   case when EXISTS
					 (
						/* verificamos si tiene despacho cancelado */
						select *
						from Despacho d
						where d.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID and Cancelado = 1
					 )
					 then 0
				 when EXISTS
					 (
						/* verificamos que este despachado */
						select *
						from Despacho d
						where d.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID and Cancelado = 0
					 )
					 then 1
				 else 0
			end,
			case when ots.OrdenTrabajoSpoolID IS NULL then 0 else 1 end
	FROM Spool s
	LEFT JOIN OrdenTrabajoSpool ots on s.SpoolID = ots.SpoolID
	WHERE NOT EXISTS
		  ( /* verificamos que sea un solo tubo, aquellos spools que no tengan juntas shop */
		   select *
		   from JuntaSpool js
		   where js.SpoolID = s.SpoolID and js.FabAreaID = @SHOP
		  )

	-- INSERT A ODT	
	INSERT INTO #TempOrdenTrabajo
		SELECT OrdenTrabajoID,
			   ot.NumeroOrden, 
			   ot.FechaOrden
		FROM OrdenTrabajo ot
		WHERE OrdenTrabajoID IN
		(
			SELECT odts.OrdenTrabajoID
			FROM	OrdenTrabajoSpool odts
			WHERE	odts.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID
		)


	--INSERT ORDEN TRABAJO SPOOL
	INSERT INTO #TempOrdenTrabajoSpool
		SELECT ots.OrdenTrabajoSpoolID,
			   ots.OrdenTrabajoID,
			   ots.NumeroControl,
			   ots.SpoolID,
			   ws.FechaLiberacionCalidad,
			   (SELECT Nombre + ' ' + ApPaterno + ' ' + ApMaterno FROM Usuario Where UserId = ws.UsuarioLiberacionCalidad),
			   ws.FechaLiberacionMateriales,
			   (SELECT Nombre + ' ' + ApPaterno + ' ' + ApMaterno FROM Usuario Where UserId = ws.UsuarioLiberacionMateriales),
			   ws.FechaOkPnd,
			   (SELECT Nombre + ' ' + ApPaterno + ' ' + ApMaterno FROM Usuario Where UserId = ws.UsuarioOkPnd)
		FROM OrdenTrabajoSpool ots
		LEFT JOIN WorkstatusSpool ws on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		WHERE ots.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID

		--INSERT ORDEN TRABAJO MATERIAL
		INSERT INTO #TempOrdenTrabajoMaterial
		SELECT otm.OrdenTrabajoMaterialID,
			   otm.OrdenTrabajoSpoolID,
			   otm.MaterialSpoolID,
			   otm.DespachoID,
			   otm.TieneInventarioCongelado,
			   otm.TieneCorte,
			   otm.TieneDespacho,
			   otm.EsAsignado,
			   cr.NumeroEmpleado,
			   dr.NumeroEmpleado			   
				FROM OrdenTrabajoMaterial otm
				INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID
				INNER JOIN CorteDetalle cd on cd.CorteDetalleID = otm.CorteDetalleID	
				INNER JOIN Corte c on c.CorteID = cd.CorteID
				INNER JOIN Cortador cr on cr.CortadorID = c.CortadorID
				INNER JOIN Despacho d on d.DespachoID = otm.DespachoID
				INNER JOIN Despachador dr on dr.DespachadorID = d.DespachadorID

	--------------------------------TABLAS DE AGRUPADORES -------------------------------------------
	--INSERT TABLA #TempAgrupadoresPorJunta
	INSERT INTO #TempAgrupadoresPorJunta		
		SELECT AgrupadorJuntaID,
				JuntaSpoolID,
				ClasificacionPND,
				ClasificacionReparacion,
				ClasificaiconSoporte,
				Grupo1,
				Grupo2,
				Grupo3,
				Grupo4,
				Grupo5
		FROM AgrupadoresPorJunta
		WHERE JuntaSpoolID = @JuntaSpoolID
	--------------------------------     AGRUPADORES      -------------------------------------------

	--INSERT SPOOL
	INSERT INTO #TempSpool
		SELECT s.SpoolID,
			   s.Nombre,
			   PorcentajePnd,
			   Especificacion,
			   Prioridad,
			   Revision,
			   RevisionCliente,
			   Dibujo,
			   SistemaPintura,
			   ColorPintura,
			   CodigoPintura,
			   DiametroPlano,
			   DiametroMayor,
			   FechaEtiqueta,
			   NumeroEtiqueta,
			   RequierePwht,			  				
			   c.Nombre as UltimaLocalizacion,
			   Campo1,
               Campo2,
               Campo3,
               Campo4,
               Campo5,
			   RequierePruebaHidrostatica	 ,
				   UltimoProceso,
					AreaGrupo,
					KgsGrupo ,
					DiamGrupo,
					PeqGrupo ,
					SistemaPinturaFinal ,
					Paint,		
					DiametroPromedio,		
					PaintLine,
					AreaEq,
					Inox,
					ClasifInox,
					FechaLocalizacion
		FROM Spool s
		LEFT OUTER JOIN Cuadrante c
		on s.CuadranteID = c.CuadranteID
		WHERE s.SpoolID IN
		(
			SELECT SpoolID FROM JuntaSpool WHERE JuntaSpoolId = @JuntaSpoolID
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
		Diametro1,
		Especificacion,
		EtiquetaEsNumero, 
		ValorNumericoEtiqueta
	)
	SELECT	ms.SpoolID,
			ms.MaterialSpoolID,
			ms.ItemCodeID,
			ic.Codigo,
			ic.DescripcionEspanol,
			ms.Etiqueta,
			ms.Diametro1,
			ms.Especificacion,
			CAST(ISNUMERIC(ms.Etiqueta) AS BIT),
			CASE WHEN ISNUMERIC(ms.Etiqueta) = 1 THEN CAST(ms.Etiqueta AS decimal(9,3)) ELSE NULL END
	FROM MaterialSpool ms
	INNER JOIN ItemCode ic on ms.ItemCodeID = ic.ItemCodeID

	--INSERT JUNTA SPOOL
INSERT INTO #TempJuntaSpool
	(
		EtiquetaJunta,
		JuntaSpoolID,
		MaterialSpool1ID,
		MaterialSpool2ID,
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
		FamiliaAcero1,
		FamiliaMaterial1,
		FamiliaAcero2,
		FamiliaMaterial2,
		DiamMat1,
		EspMat1,
		DiamMat2,
		EspMat2,
		CodigoFabArea,
		EstacionID,
		EsManual,
		FabClas,
		CampoJunta2,
		CampoJunta3,
		CampoJunta4,
		CampoJunta5, 
		RequierePwht,
		RequierePruebaNeumatica
	)
	SELECT DISTINCT js.Etiqueta,
		  js.JuntaSpoolID,
		  NULL,
		  NULL,
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
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial1) = 1 THEN CAST(js.EtiquetaMaterial1 AS decimal(9,3)) ELSE NULL END,
		  CASE WHEN ISNUMERIC(js.EtiquetaMaterial2) = 1 THEN CAST(js.EtiquetaMaterial2 AS decimal(9,3)) ELSE NULL END,
		  NULL,
		  NULL,
		  NULL,
		  NULL,
		  NULL,
		  NULL,
		  fa1.Nombre [FamiliaAcero1],
		  fm1.Nombre [FamiliaMaterial1],
		  fa2.Nombre [FamiliaAcero2],
		  fm2.Nombre [FamiliaMaterial2],
		  NULL,
		  NULL,
		  NULL,
		  NULL,
		  fab.Codigo,
		  js.EstacionID,
		  js.EsManual,
		  js.FabClas,
		  js.Campo2,
		  js.Campo3,
		  js.Campo4,
		  js.Campo5,
		  js.RequierePwht,
		  js.RequierePruebaNeumatica
	FROM JuntaSpool js
	INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
	LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
	LEFT JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
	LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
	INNER JOIN FabArea fab on js.FabAreaID = fab.FabAreaID
	WHERE js.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)


		INSERT INTO #TempJuntaSpoolB
	(
		EtiquetaJunta,
		JuntaSpoolID,
		MaterialSpool1ID,
		MaterialSpool2ID,
		SpoolID,
		TipoJunta,
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
		FamiliaAcero1,
		FamiliaMaterial1,
		FamiliaAcero2,
		FamiliaMaterial2,
		DiamMat1,
		EspMat1,
		DiamMat2,
		EspMat2,
		CodigoFabArea,
		EstacionID,
		EsManual,
		FabClas,
		CampoJunta2,
		CampoJunta3,
		CampoJunta4,
		CampoJunta5,
		RequierePwht,
		RequierePruebaNeumatica
	)
	SELECT DISTINCT js.EtiquetaJunta,
		  js.JuntaSpoolID,
		  ms1.MaterialSpoolID,
		  ms2.MaterialSpoolID,
		  js.SpoolID,
		  tj.Codigo,
		  js.Diametro,
		  js.Cedula,
		  js.Espesor,
		  js.EtiquetaMaterial1,
		  js.EtiquetaMaterial2,
		  js.FamiliaAceroMaterial1ID,
		  js.FamiliaAceroMaterial2ID,
		  js.Peqs,
		  js.KgTeoricos,
		  js.Etiqueta1EsNumero,
		  js.Etiqueta2EsNumero,
		  js.ValorNumericoEtiqueta1,
		  js.ValorNumericoEtiqueta2,
		  ms1.ItemCodeID,
		  ms1.CodigoItemCode,
		 ms1.DescripcionItemCode,
		  ms2.ItemCodeID,
		  ms2.CodigoItemCode,
		  ms2.DescripcionItemCode,
		  js.FamiliaAcero1,
		  js.FamiliaMaterial1,
		  js.FamiliaAcero2,
		  js.FamiliaMaterial2,
		  ms1.Diametro1 [DiamMat1],
		 ms1.Especificacion [EspMat1],
		  ms2.Diametro1 [DiamMat2],
		  ms2.Especificacion [EspMat2],
		  js.CodigoFabArea,
		  js.EstacionID,
		  js.EsManual,
		  js.FabClas,
		  js.CampoJunta2,
		  js.CampoJunta3,
		  js.CampoJunta4,
		  js.CampoJunta5,
		  js.RequierePwht,
		  js.RequierePruebaNeumatica
	FROM #TempJuntaSpool js
	INNER JOIN TipoJunta tj on js.TipoJuntaID = tj.TipoJuntaID
	LEFT JOIN JuntaCampo jc on js.JuntaSpoolID = jc.JuntaSpoolID
	LEFT JOIN JuntaCampoArmado jca on jc.JuntaCampoID = jca.JuntaCampoID
	LEFT JOIN #TempMaterialSpool ms1 on ms1.SpoolID = CASE WHEN js.Etiqueta1EsNumero = 1 THEN js.SpoolID ELSE jca.Spool1ID END and (ms1.Etiqueta = js.EtiquetaMaterial1 or ms1.ValorNumericoEtiqueta = (CASE WHEN js.Etiqueta1EsNumero = 1 THEN  js.ValorNumericoEtiqueta1 ELSE jca.EtiquetaMaterial1 END))
	LEFT JOIN #TempMaterialSpool ms2 on ms2.SpoolID = CASE WHEN js.Etiqueta2EsNumero = 1 THEN js.SpoolID ELSE jca.Spool2ID END and (ms2.Etiqueta = js.EtiquetaMaterial2 or ms2.ValorNumericoEtiqueta = (CASE WHEN js.Etiqueta2EsNumero = 1 THEN  js.ValorNumericoEtiqueta2 ELSE jca.EtiquetaMaterial2 END))	
	WHERE js.SpoolID IN
	(
		SELECT SpoolID FROM #TempSpool
	)


	--INSERT JUNTA WORKSTATUS
	INSERT INTO #TempJuntaWorkstatus
		SELECT jws.JuntaWorkstatusID,
			   CAST(0 as bit),
			   jws.JuntaSpoolID,
			   jws.JuntaArmadoID,
			   jws.JuntaSoldaduraID,
			   jws.JuntaInspeccionVisualID,
			   jws.OrdenTrabajoSpoolID,
			   jws.UltimoProcesoID,
			   jws.EtiquetaJunta,
			   jws.JuntaFinal
		FROM JuntaWorkstatus jws 
		WHERE jws.JuntaSpoolID = @JuntaSpoolID
			  AND JuntaFinal = 1
		UNION
		SELECT	jc.JuntaCampoID,
				CAST(1 as bit),
				jc.JuntaSpoolID,
				jc.JuntaCampoArmadoID,
				jc.JuntaCampoSoldaduraID,
				jc.JuntaCampoInspeccionVisualID,
				jc.OrdenTrabajoSpoolID,
				jc.UltimoProcesoID,
				jc.EtiquetaJunta,
				jc.JuntaFinal
		FROM JuntaCampo jc
		WHERE jc.JuntaSpoolID = @JuntaSpoolID
			  AND jc.JuntaFinal = 1		  

	--INSERT JUNTA SOLDADURA
	INSERT INTO #TempJuntaSoldadura
		SELECT DISTINCT js.JuntaSoldaduraID,
						js.JuntaWorkstatusID,
						CAST(0 as bit),
						FechaSoldadura,
						FechaReporte,
						ta.Nombre [Teller],
						Wps.Nombre [Wps],
						Wps1.Nombre [WpsRelleno],
						pr.Nombre [ProcesoRelleno],
						cr.ConsumiblesRelleno,
						pra.Nombre [ProcesoRaiz],
						sra.SoldadorRaiz,
						sr.SoldadorRelleno,			   
						fm1.Nombre,
						fm2.Nombre,
						js.Observaciones  
		FROM JuntaSoldadura js
		INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = js.JuntaWorkstatusID
		INNER JOIN Taller ta on ta.TallerID = js.TallerID
		LEFT JOIN Wps wps on wps.WpsID =  js.WpsID
		LEFT JOIN Wps wps1 on wps1.WpsID =  js.WpsRellenoID
		LEFT JOIN ProcesoRelleno pr on pr.ProcesoRellenoID = js.ProcesoRellenoID
		LEFT JOIN ProcesoRaiz pra on pra.ProcesoRaizID = js.ProcesoRaizID
		INNER JOIN #TempJuntaSpoolB jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
		INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = jsp.FamiliaAceroMaterial1ID
		INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
		LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = jsp.FamiliaAceroMaterial2ID
		LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT DISTINCT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaSoldaduraDetalle jsd1
			INNER JOIN Soldador s on s.SoldadorID = jsd1.SoldadorID
			WHERE (jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID) and jsd1.TecnicaSoldadorID  = 2
			FOR XML PATH (''))),' ',', ') AS SoldadorRelleno
			FROM JuntaSoldaduraDetalle jsd
			GROUP BY jsd.JuntaSoldaduraID
		) sr on sr.JuntaSoldaduraID = js.JuntaSoldaduraID
		LEFT JOIN(
			SELECT jsd.JuntaSoldaduraID, 
				   REPLACE(RTRIM((SELECT DISTINCT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
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
		WHERE jws.JuntaCampo = 0
		UNION
		SELECT DISTINCT jcs.JuntaCampoSoldaduraID,
						jcs.JuntaCampoID,
						CAST(1 as bit),
						FechaSoldadura,
						FechaReporte,
						'' [Teller],
						Wps.Nombre [Wps],
						Wps1.Nombre [WpsRelleno],
						pr.Nombre [ProcesoRelleno],
						cr.ConsumiblesRelleno,
						pra.Nombre [ProcesoRaiz],
						sra.SoldadorRaiz,
						sr.SoldadorRelleno,			   
						fm1.Nombre,
						fm2.Nombre,
						jcs.Observaciones
		FROM JuntaCampoSoldadura jcs
		INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = jcs.JuntaCampoID 
		LEFT JOIN Wps wps on wps.WpsID =  jcs.WpsRaizID
		LEFT JOIN Wps wps1 on wps1.WpsID =  jcs.WpsRellenoID
		LEFT JOIN ProcesoRelleno pr on pr.ProcesoRellenoID = jcs.ProcesoRellenoID
		LEFT JOIN ProcesoRaiz pra on pra.ProcesoRaizID = jcs.ProcesoRaizID
		INNER JOIN #TempJuntaSpoolB jsp on jsp.JuntaSpoolID = jws.JuntaSpoolID
		INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = jsp.FamiliaAceroMaterial1ID
		INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
		LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = jsp.FamiliaAceroMaterial2ID
		LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
		LEFT JOIN(
			SELECT jcsd.JuntaCampoSoldaduraID, 
				   REPLACE(RTRIM((SELECT DISTINCT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaCampoSoldaduraDetalle jcsd1
			INNER JOIN Soldador s on s.SoldadorID = jcsd1.SoldadorID
			WHERE (jcsd1.JuntaCampoSoldaduraID = jcsd.JuntaCampoSoldaduraID) and jcsd1.TecnicaSoldadorID  = 2
			FOR XML PATH (''))),' ',', ') AS SoldadorRelleno
			FROM JuntaCampoSoldaduraDetalle jcsd
			GROUP BY jcsd.JuntaCampoSoldaduraID
		) sr on sr.JuntaCampoSoldaduraID = jcs.JuntaCampoSoldaduraID
		LEFT JOIN(
			SELECT jcsd.JuntaCampoSoldaduraID, 
				   REPLACE(RTRIM((SELECT DISTINCT s.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaCampoSoldaduraDetalle jcsd1
			INNER JOIN Soldador s on s.SoldadorID = jcsd1.SoldadorID
			WHERE (jcsd1.JuntaCampoSoldaduraID = jcsd.JuntaCampoSoldaduraID) and jcsd1.TecnicaSoldadorID  = 1
			FOR XML PATH (''))),' ',', ') AS SoldadorRaiz
			FROM JuntaCampoSoldaduraDetalle jcsd
			GROUP BY jcsd.JuntaCampoSoldaduraID
		) sra on sra.JuntaCampoSoldaduraID = jcs.JuntaCampoSoldaduraID
		LEFT JOIN(
			SELECT jcsd.JuntaCampoSoldaduraID, 
				   REPLACE(RTRIM((SELECT co.Codigo + '' + CAST('' AS NVARCHAR(MAX)) + ' ' 
			FROM JuntaCampoSoldaduraDetalle jcsd1
			INNER JOIN Consumible co on co.ConsumibleID = jcsd1.ConsumibleID
			WHERE (jcsd1.JuntaCampoSoldaduraID = jcsd.JuntaCampoSoldaduraID) 
			FOR XML PATH (''))),' ',', ')  AS ConsumiblesRelleno
			FROM JuntaCampoSoldaduraDetalle jcsd
			INNER JOIN Consumible c on c.ConsumibleID = jcsd.ConsumibleID
			GROUP BY jcsd.JuntaCampoSoldaduraID
		) cr on cr.JuntaCampoSoldaduraID = jcs.JuntaCampoSoldaduraID	
		WHERE jws.JuntaCampo = 1		

	--INSERT JUNTA INSPECCION VISUAL
	insert into #TempJuntaInspeccionVisualDefecto	
 	SELECT DISTINCT	jiv.JuntaInspeccionVisualID,
					jiv.JuntaWorkstatusID,
					CAST(0 as bit),
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
	WHERE jws.JuntaCampo = 0
	UNION
	SELECT DISTINCT	jciv.JuntaCampoInspeccionVisualID,
					jciv.JuntaCampoID,
					CAST(1 as bit),
					jciv.FechaInspeccion [Fecha],
					iv.FechaReporte,
					iv.NumeroReporte,
					null,
					jciv.Aprobado [Resultado],
					substring(d.Defecto,0,LEN(d.defecto)) as Defecto,
					jciv.Observaciones,
					jciv.FechaModificacion			   			   
	FROM JuntaCampoInspeccionVisual jciv
	INNER JOIN #TempJuntaWorkstatus jws on jws.JuntaWorkStatusID = jciv.JuntaCampoID 
	INNER JOIN InspeccionVisualCampo iv on iv.InspeccionVisualCampoID = jciv.InspeccionVisualCampoID
	LEFT JOIN(
		SELECT jcivd.JuntaCampoInspeccionVisualID, 
			   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
		FROM JuntaCampoInspeccionVisualDefecto jcivd1
		INNER JOIN Defecto d on d.DefectoID = jcivd1.DefectoID
		WHERE (jcivd1.JuntaCampoInspeccionVisualID = jcivd.JuntaCampoInspeccionVisualID) 
		FOR XML PATH (''))) AS Defecto
		FROM JuntaCampoInspeccionVisualDefecto jcivd
		INNER JOIN Defecto d on d.DefectoID = jcivd.DefectoID
		GROUP BY jcivd.JuntaCampoInspeccionVisualID
	) d on d.JuntaCampoInspeccionVisualID = jciv.JuntaCampoInspeccionVisualID	
	WHERE jws.JuntaCampo = 1


	INSERT INTO #TempJuntaInspeccionVisual
		SELECT JuntaInspeccionVisualID,
			   jiv.JuntaWorkstatusID,
			   CAST(0 as bit),
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
			AND ISNULL(e.FechaModificacion, 0) = ISNULL(jiv.FechaModificacion, 0)
		WHERE jiv.JuntaCampo = 0
		UNION
		SELECT JuntaInspeccionVisualID,
			   jiv.JuntaWorkstatusID,
			   CAST(1 as bit),
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
			AND ISNULL(e.FechaModificacion, 0) = ISNULL(jiv.FechaModificacion, 0)
		WHERE jiv.JuntaCampo = 1

	--INSERT WORKSTATUS SPOOL
	INSERT INTO #TempWorkstatusSpool
		SELECT DISTINCT ws.WorkstatusSpoolID,
			   ws.OrdenTrabajoSpoolID,
			   s.SistemaPintura,
			   s.ColorPintura,
			   s.CodigoPintura,
			   s.NumeroEtiquetaEmbarque,
			   s.FechaEtiquetaEmbarque,
			   ws.FechaPreparacion,
			   ws.LiberadoPintura	  		   
		FROM WorkstatusSpool ws
		INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID	
		INNER JOIN #TempSpool s ON s.SpoolID = ots.SpoolID		

	--INSERT REPORTE DIMENSIONAL
	INSERT INTO #TempReporteDimensional
		SELECT DISTINCT rd.ReporteDimensionalID,
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
		INNER JOIN #TempReporteDimensional rd 
		on rd.ReporteDimensionalID = rdd.ReporteDimensionalID		
		right JOIN(
				select MAX(ReporteDimensionalDetalleID) as ReporteDimensionalDetalleID,				   
				   Sum(Case Aprobado When 0 Then 1 Else 0 End) As numerorechazos,
				   WorkstatusSpoolID				  
		    from ReporteDimensionalDetalle		 
		    group by WorkstatusSpoolID
		) nr 
		on nr.ReporteDimensionalDetalleID = rdd.ReporteDimensionalDetalleID
		and nr.WorkstatusSpoolID = rdd.WorkstatusSpoolID
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
			   CAST(0 as bit),
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
		where	jw.JuntaCampo = 0
		UNION
		SELECT DISTINCT jcrpnd.JuntaCampoReportePndID,
			   jcrpnd.JuntaCampoID,
			   CAST(1 as bit),
			   rcpnd.TipoPruebaID,
			   rcpnd.FechaReporte,
			   rcpnd.NumeroReporte,
			   jcrpnd.JuntaCampoRequisicionID,
			   jcrpnd.FechaPrueba,
			   null,
			   jcrpnd.Aprobado,
			   jcrpnd.Observaciones,
			   jcrpnd.FechaModificacion
		FROM JuntaCampoReportePnd jcrpnd
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jcrpnd.JuntaCampoID
		INNER JOIN ReporteCampoPND rcpnd on rcpnd.ReporteCampoPNDID = jcrpnd.ReporteCampoPNDID
		where	jw.JuntaCampo = 1


	--INSERT JUNTA REPORTE TT
	INSERT INTO #TempJuntaReporteTt
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jrtt.JuntaWorkstatusID,
			   CAST(0 as bit),
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
		where	jw.JuntaCampo = 0
		UNION
		SELECT DISTINCT jrctt.JuntaCampoReporteTtID,
			   jrctt.JuntaCampoID,
			   CAST(1 as bit),
			   rtt.TipoPruebaID,
			   rtt.FechaReporte,
			   rtt.NumeroReporte,
			   jrctt.JuntaCampoRequisicionID,
			   jrctt.Aprobado,
			   jrctt.NumeroGrafica,
			   null,
			   jrctt.FechaTratamiento,
			   jrctt.Observaciones,
			   jrctt.FechaModificacion
		FROM JuntaCampoReporteTT jrctt
		INNER JOIN #TempJuntaWorkstatus jw on jw.JuntaWorkStatusID = jrctt.JuntaCampoID 
		INNER JOIN ReporteCampoTt rtt on rtt.ReporteCampoTTID = jrctt.ReporteCampoTTID
		where	jw.JuntaCampo = 1


	--INSERT PRUEBA RT
	INSERT INTO #TempPruebaRT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
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
			   jrpnd.FechaModificacion,
			   SUBSTRING(rd.Sector,0,LEN(rd.Sector)) as Sector,
			   SUBSTRING(rd.Cuadrante,0,LEN(rd.Cuadrante)) as Cuadrante
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
							FROM JuntaReportePndSector jrpnds1
							INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
							WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
							FOR XML PATH (''))) AS Defecto,
					RTRIM((SELECT '(' + jrpnds1.Sector + ', de ' + jrpnds1.SectorInicio + ' a ' + jrpnds1.SectorFin + ')' + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
							FROM JuntaReportePndSector jrpnds1
							INNER JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
							WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
							FOR XML PATH (''))) AS Sector,
					null AS Cuadrante
			FROM JuntaReportePndSector jrpnds
			INNER JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			GROUP BY jrpnds.JuntaReportePndID
			UNION
			SELECT jrpndc.JuntaReportePndID, 
							   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
										FROM JuntaReportePndCuadrante jrpndc1
										INNER JOIN Defecto d on d.DefectoID = jrpndc1.DefectoID
										WHERE (jrpndc1.JuntaReportePndID = jrpndc.JuntaReportePndID) 
										FOR XML PATH (''))) AS Defecto,
								null AS Sector,
								RTRIM((SELECT '(' + jrpndc1.Cuadrante + ', ' + jrpndc1.Placa + ')' + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
										FROM JuntaReportePndCuadrante jrpndc1
										INNER JOIN Defecto d on d.DefectoID = jrpndc1.DefectoID
										WHERE (jrpndc1.JuntaReportePndID = jrpndc.JuntaReportePndID) 
										FOR XML PATH (''))) AS Cuadrante
						FROM JuntaReportePndCuadrante jrpndc
						INNER JOIN Defecto d on d.DefectoID = jrpndc.DefectoID
						GROUP BY jrpndc.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 1
				and (ISNULL(jrpnd.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		UNION
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaCampoID,
			   CAST(1 as bit),
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
			   null,
			   jrpnd.FechaModificacion,
			   SUBSTRING(rd.Sector,0,LEN(rd.Sector)) as Sector,
			   SUBSTRING(rd.Cuadrante,0,LEN(rd.Cuadrante)) as Cuadrante
		FROM JuntaCampoRequisicion jr 		
		INNER JOIN RequisicionCampo r on r.RequisicionCampoID = jr.RequisicionCampoID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaCampoRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN(
			SELECT jcrpnds.JuntaCampoReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
							FROM JuntaCampoReportePndSector jcrpnds1
							INNER JOIN Defecto d on d.DefectoID = jcrpnds1.DefectoID
							WHERE (jcrpnds1.JuntaCampoReportePndID = jcrpnds.JuntaCampoReportePndID) 
							FOR XML PATH (''))) AS Defecto,
					RTRIM((SELECT '(' + jcrpnds1.Sector + ', de ' + jcrpnds1.SectorInicio + ' a ' + jcrpnds1.SectorFin + ')' + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
							FROM JuntaCampoReportePndSector jcrpnds1
							INNER JOIN Defecto d on d.DefectoID = jcrpnds1.DefectoID
							WHERE (jcrpnds1.JuntaCampoReportePndID = jcrpnds.JuntaCampoReportePndID) 
							FOR XML PATH (''))) AS Sector,
					null AS Cuadrante
			FROM JuntaCampoReportePndSector jcrpnds
			INNER JOIN Defecto d on d.DefectoID = jcrpnds.DefectoID
			GROUP BY jcrpnds.JuntaCampoReportePndID
			UNION
			SELECT jcrpndc.JuntaCampoReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
							FROM JuntaCampoReportePndCuadrante jcrpndc1
							INNER JOIN Defecto d on d.DefectoID = jcrpndc1.DefectoID
							WHERE (jcrpndc1.JuntaCampoReportePndID = jcrpndc.JuntaCampoReportePndID) 
							FOR XML PATH (''))) AS Defecto,
					null AS Sector,
					RTRIM((SELECT '(' + jcrpndc1.Cuadrante + ', ' + jcrpndc1.Placa + ')' + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
							FROM JuntaCampoReportePndCuadrante jcrpndc1
							INNER JOIN Defecto d on d.DefectoID = jcrpndc1.DefectoID
							WHERE (jcrpndc1.JuntaCampoReportePndID = jcrpndc.JuntaCampoReportePndID) 
							FOR XML PATH (''))) AS Cuadrante
			FROM JuntaCampoReportePndCuadrante jcrpndc
			INNER JOIN Defecto d on d.DefectoID = jcrpndc.DefectoID
			GROUP BY jcrpndc.JuntaCampoReportePndID
		) rd on rd.JuntaCampoReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 1
				and ISNULL(jrpnd.JuntaCampo, 1) = 1

	--INSERT PRUEBA PT
	INSERT INTO #TempPruebaPT 
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
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
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
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
		WHERE	r.TipoPruebaID = 2
				and (ISNULL(jrpnd.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		UNION
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaCampoID,
			   CAST(1 as bit),
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
			   null,
			   jrpnd.FechaModificacion
		FROM JuntaCampoRequisicion jr 		
		INNER JOIN RequisicionCampo r on r.RequisicionCampoID = jr.RequisicionCampoID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaCampoRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN(
			SELECT jcrpnds.JuntaCampoReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaCampoReportePndSector jcrpnds1
			INNER JOIN Defecto d on d.DefectoID = jcrpnds1.DefectoID
			WHERE (jcrpnds1.JuntaCampoReportePndID = jcrpnds.JuntaCampoReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaCampoReportePndSector jcrpnds
			INNER JOIN Defecto d on d.DefectoID = jcrpnds.DefectoID
			GROUP BY jcrpnds.JuntaCampoReportePndID
		) rd on rd.JuntaCampoReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 2
				and ISNULL(jrpnd.JuntaCampo, 1) = 1

	--INSERT PRUEBA RT(PostTT)
	INSERT INTO #TempPruebaRTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
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
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
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
		WHERE	r.TipoPruebaID = 5
				and (ISNULL(jrpnd.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		UNION
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaCampoID,
			   CAST(1 as bit),
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
			   null,
			   jrpnd.FechaModificacion
		FROM JuntaCampoRequisicion jr 		
		INNER JOIN RequisicionCampo r on r.RequisicionCampoID = jr.RequisicionCampoID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaCampoRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN(
			SELECT jcrpnds.JuntaCampoReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaCampoReportePndSector jcrpnds1
			INNER JOIN Defecto d on d.DefectoID = jcrpnds1.DefectoID
			WHERE (jcrpnds1.JuntaCampoReportePndID = jcrpnds.JuntaCampoReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaCampoReportePndSector jcrpnds
			INNER JOIN Defecto d on d.DefectoID = jcrpnds.DefectoID
			GROUP BY jcrpnds.JuntaCampoReportePndID
		) rd on rd.JuntaCampoReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 5
				and ISNULL(jrpnd.JuntaCampo, 1) = 1

	--INSERT PRUEBA PMI
	INSERT INTO #TempPruebaPMI
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
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
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
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
		WHERE	r.TipoPruebaID = 10				
				and (ISNULL(jrpnd.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		UNION
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaCampoID,
			   CAST(1 as bit),
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
			   null,
			   jrpnd.FechaModificacion
		FROM JuntaCampoRequisicion jr 		
		INNER JOIN RequisicionCampo r on r.RequisicionCampoID = jr.RequisicionCampoID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaCampoRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN(
			SELECT jcrpnds.JuntaCampoReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaCampoReportePndSector jcrpnds1
			INNER JOIN Defecto d on d.DefectoID = jcrpnds1.DefectoID
			WHERE (jcrpnds1.JuntaCampoReportePndID = jcrpnds.JuntaCampoReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaCampoReportePndSector jcrpnds
			INNER JOIN Defecto d on d.DefectoID = jcrpnds.DefectoID
			GROUP BY jcrpnds.JuntaCampoReportePndID
		) rd on rd.JuntaCampoReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 10
				and ISNULL(jrpnd.JuntaCampo, 1) = 1

	--INSERT PRUEBA PT(PostTT)
	INSERT INTO #TempPruebaPTPostTT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
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
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
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
		WHERE	r.TipoPruebaID = 6
				and (ISNULL(jrpnd.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		UNION
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaCampoID,
			   CAST(1 as bit),
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
			   null,
			   jrpnd.FechaModificacion
		FROM JuntaCampoRequisicion jr 		
		INNER JOIN RequisicionCampo r on r.RequisicionCampoID = jr.RequisicionCampoID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaCampoRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN(
			SELECT jcrpnds.JuntaCampoReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaCampoReportePndSector jcrpnds1
			INNER JOIN Defecto d on d.DefectoID = jcrpnds1.DefectoID
			WHERE (jcrpnds1.JuntaCampoReportePndID = jcrpnds.JuntaCampoReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaCampoReportePndSector jcrpnds
			INNER JOIN Defecto d on d.DefectoID = jcrpnds.DefectoID
			GROUP BY jcrpnds.JuntaCampoReportePndID
		) rd on rd.JuntaCampoReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 6
				and ISNULL(jrpnd.JuntaCampo, 1) = 1

	--INSERT PRUEBA UT
	INSERT INTO #TempPruebaUT
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
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
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
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
		WHERE	r.TipoPruebaID = 8				
				and (ISNULL(jrpnd.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		UNION
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaCampoID,
			   CAST(1 as bit),
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
			   null,
			   jrpnd.FechaModificacion
		FROM JuntaCampoRequisicion jr 		
		INNER JOIN RequisicionCampo r on r.RequisicionCampoID = jr.RequisicionCampoID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaCampoRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN(
			SELECT jcrpnds.JuntaCampoReportePndID, 
				   RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
			FROM JuntaCampoReportePndSector jcrpnds1
			INNER JOIN Defecto d on d.DefectoID = jcrpnds1.DefectoID
			WHERE (jcrpnds1.JuntaCampoReportePndID = jcrpnds.JuntaCampoReportePndID) 
			FOR XML PATH (''))) AS Defecto
			FROM JuntaCampoReportePndSector jcrpnds
			INNER JOIN Defecto d on d.DefectoID = jcrpnds.DefectoID
			GROUP BY jcrpnds.JuntaCampoReportePndID
		) rd on rd.JuntaCampoReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 8
				and ISNULL(jrpnd.JuntaCampo, 1) = 1

    --INSERT PRUEBA Neumatica
	INSERT INTO #TempPruebaNeumatica
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   SUBSTRING(rd.Sector,0,LEN(rd.Sector)) as Sector,
			   SUBSTRING(rc.Cuadrante,0,LEN(rc.Cuadrante)) as Cuadrante,
			   jrpnd.Observaciones,
			   r.Observaciones,
			   jrpnd.FechaModificacion
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
						FROM JuntaReportePndSector jrpnds1
						left JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
						WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
						FOR XML PATH (''))) AS Defecto,
				RTRIM((SELECT '(' + jrpnds1.Sector + ', de ' + jrpnds1.SectorInicio + ' a ' + jrpnds1.SectorFin + ')' + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
						FROM JuntaReportePndSector jrpnds1
						left JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID	
						where jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID			
						FOR XML PATH (''))) AS Sector
			FROM JuntaReportePndSector jrpnds
			LEFT JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			LEFT Join JuntaReportePndCuadrante jrpndc on d.DefectoID = jrpndc.DefectoID
			GROUP BY jrpnds.JuntaReportePndID, jrpndc.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		LEFT JOIN(
		select jrpndc.JuntaReportePndId,
			RTRIM((SELECT '(' + jrpndc1.Cuadrante + ', ' + jrpndc1.Placa + ')' + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
						FROM JuntaReportePndCuadrante jrpndc1
						left JOIN Defecto d on d.DefectoID = jrpndc1.DefectoID							
						where jrpndc1.JuntaReportePndID = jrpndc.JuntaReportePndID						
						FOR XML PATH (''))) AS Cuadrante
					from JuntaReportePndCuadrante jrpndc
					left join Defecto d
					on d.DefectoID = jrpndc.DefectoID
					Group by jrpndc.JuntaReportePndID
		)rc on rc.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 11				
				and (ISNULL(jrpnd.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		UNION
		SELECT DISTINCT jrpnd.JuntaReportePndID,
			   jr.JuntaCampoID,
			   CAST(1 as bit),
			   r.FechaRequisicion,
			   r.NumeroRequisicion,
			   r.CodigoAsme,
			   jrpnd.FechaPrueba,
			   jrpnd.FechaReporte,
			   jrpnd.NumeroReporte,
			   jrpnd.Hoja,
			   jrpnd.Aprobado,
			   substring(rd.Defecto,0,LEN(rd.Defecto)) as Defecto,
			   SUBSTRING(rd.Sector,0,LEN(rd.Sector)) as Sector,
			   SUBSTRING(rc.Cuadrante,0,LEN(rc.Cuadrante)) as Cuadrante,
			   jrpnd.Observaciones,
			   null,
			   jrpnd.FechaModificacion
		FROM JuntaCampoRequisicion jr 		
		INNER JOIN RequisicionCampo r on r.RequisicionCampoID = jr.RequisicionCampoID
		LEFT JOIN #TempJuntaReportePnd jrpnd on jr.JuntaCampoRequisicionID = jrpnd.JuntaRequisicionID
		LEFT JOIN(
			SELECT jrpnds.JuntaReportePndID, 
				RTRIM((SELECT d.Nombre + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
						FROM JuntaReportePndSector jrpnds1
						left JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID
						WHERE (jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID) 
						FOR XML PATH (''))) AS Defecto,
				RTRIM((SELECT '(' + jrpnds1.Sector + ', de ' + jrpnds1.SectorInicio + ' a ' + jrpnds1.SectorFin + ')' + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
						FROM JuntaReportePndSector jrpnds1
						left JOIN Defecto d on d.DefectoID = jrpnds1.DefectoID	
						where jrpnds1.JuntaReportePndID = jrpnds.JuntaReportePndID			
						FOR XML PATH (''))) AS Sector
			FROM JuntaReportePndSector jrpnds
			LEFT JOIN Defecto d on d.DefectoID = jrpnds.DefectoID
			LEFT Join JuntaReportePndCuadrante jrpndc on d.DefectoID = jrpndc.DefectoID
			GROUP BY jrpnds.JuntaReportePndID, jrpndc.JuntaReportePndID
		) rd on rd.JuntaReportePndID = jrpnd.JuntaReportePndID
		LEFT JOIN(
		select jrpndc.JuntaReportePndId,
			RTRIM((SELECT '(' + jrpndc1.Cuadrante + ', ' + jrpndc1.Placa + ')' + '' + CAST('' AS NVARCHAR(MAX)) + ',' 
						FROM JuntaReportePndCuadrante jrpndc1
						left JOIN Defecto d on d.DefectoID = jrpndc1.DefectoID							
						where jrpndc1.JuntaReportePndID = jrpndc.JuntaReportePndID						
						FOR XML PATH (''))) AS Cuadrante
					from JuntaReportePndCuadrante jrpndc
					left join Defecto d
					on d.DefectoID = jrpndc.DefectoID
					Group by jrpndc.JuntaReportePndID
		)rc on rc.JuntaReportePndID = jrpnd.JuntaReportePndID
		WHERE	r.TipoPruebaID = 11
				and ISNULL(jrpnd.JuntaCampo, 1) = 1


	--INSERT TRATAMIENTO PWHT
	INSERT INTO #TempTratamientoPwht
		SELECT DISTINCT
			   jrtt.JuntaReporteTtID,
			   jr.JuntaWorkstatusID,
			   jr.JuntaRequisicionID,
			   CAST(0 as bit) as TipoJunta,
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
			   r.Observaciones as ObservacionesRequisicion,
			   jrtt.FechaModificacion
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
		LEFT JOIN #TempJuntaReporteTt jrtt on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		WHERE	r.TipoPruebaID = 3
				and (ISNULL(jrtt.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)						
		union
		SELECT DISTINCT
			   jrtt.JuntaReporteTtID,
			   jcr.JuntaCampoID,
			   jcr.JuntaCampoRequisicionID,
			   CAST(1 as bit),
			   rc.FechaRequisicion,
			   rc.NumeroRequisicion,
			   rc.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   null,
			   jrtt.FechaModificacion			   
		FROM JuntaCampoRequisicion jcr 		
		INNER JOIN RequisicionCampo rc on rc.RequisicionCampoID = jcr.RequisicionCampoID
		LEFT JOIN #TempJuntaReporteTt jrtt on jcr.JuntaCampoRequisicionID = jrtt.JuntaRequisicionID
		WHERE	rc.TipoPruebaID = 3
				and ISNULL(jrtt.JuntaCampo, 1) = 1


	--INSERT TRATAMIENTO DUREZAS
	INSERT INTO #TempTratamientoDurezas
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
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
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
		LEFT JOIN #TempJuntaReporteTt jrtt on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		WHERE	r.TipoPruebaID = 4
				and (ISNULL(jrtt.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		union
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jcr.JuntaCampoID,
			   CAST(1 as bit),
			   rc.FechaRequisicion,
			   rc.NumeroRequisicion,
			   rc.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   null,
			   jrtt.FechaModificacion			   
		FROM JuntaCampoRequisicion jcr 		
		INNER JOIN RequisicionCampo rc on rc.RequisicionCampoID = jcr.RequisicionCampoID
		LEFT JOIN #TempJuntaReporteTt jrtt on jcr.JuntaCampoRequisicionID = jrtt.JuntaRequisicionID
		WHERE	rc.TipoPruebaID = 4
				and ISNULL(jrtt.JuntaCampo, 1) = 1

	--INSERT TRATAMIENTO PREHEAT
	INSERT INTO #TempTratamientoPreheat
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jr.JuntaWorkstatusID,
			   CAST(0 as bit),
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
		FROM JuntaRequisicion jr 		
		INNER JOIN Requisicion r on r.RequisicionID = jr.RequisicionID
		LEFT JOIN #TempJuntaWorkstatus tjws on jr.JuntaWorkstatusID = tjws.JuntaWorkStatusID
		LEFT JOIN #TempJuntaReporteTt jrtt on jr.JuntaRequisicionID = jrtt.JuntaRequisicionID
		WHERE	r.TipoPruebaID = 7
				and (ISNULL(jrtt.JuntaCampo, 0) = 0 or ISNULL(tjws.JuntaCampo, 0) = 0)
		union
		SELECT DISTINCT jrtt.JuntaReporteTtID,
			   jcr.JuntaCampoID,
			   CAST(0 as bit),
			   rc.FechaRequisicion,
			   rc.NumeroRequisicion,
			   rc.CodigoAsme,
			   jrtt.FechaTratamiento,
			   jrtt.FechaReporte,
			   jrtt.NumeroReporte,
			   jrtt.Hoja,
			   jrtt.NumeroGrafica,
			   jrtt.Aprobado,
			   jrtt.Observaciones,
			   null,
			   jrtt.FechaModificacion			   
		FROM JuntaCampoRequisicion jcr 		
		INNER JOIN RequisicionCampo rc on rc.RequisicionCampoID = jcr.RequisicionCampoID
		LEFT JOIN #TempJuntaReporteTt jrtt on jcr.JuntaCampoRequisicionID = jrtt.JuntaRequisicionID
		WHERE	rc.TipoPruebaID = 7
				and ISNULL(jrtt.JuntaCampo, 1) = 1

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
			   ps.ReportePullOff,
			   ws.LiberadoPintura
		FROM #TempWorkstatusSpool ws		
		LEFT JOIN RequisicionPinturaDetalle rpd on ws.WorkstatusSpoolID = rpd.WorkstatusSpoolID
		LEFT JOIN RequisicionPintura rp on rp.RequisicionPinturaID = rpd.RequisicionPinturaID
		LEFT JOIN PinturaSpool ps on ps.WorkstatusSpoolID = ws.WorkstatusSpoolID

	--INSERT EMBARQUE
	INSERT INTO #TempEmbarque
			SELECT DISTINCT es.EmbarqueSpoolID,
				   ws.WorkstatusSpoolID,
				   ws.EmbarqueEtiqueta,
				   ws.FechaEtiqueta,
				   ws.FechaPreparacion,
				   e.FechaEmbarque,
				   e.FechaEstimada,
				   e.NumeroEmbarque,
				   e.Nota1,
				   e.Nota2,
				   e.Nota3,
				   e.Nota4,
				   e.Nota5,
				   e.VigenciaAduana
			FROM #TempWorkstatusSpool ws
			LEFT JOIN EmbarqueSpool es on es.WorkstatusSpoolID = ws.WorkstatusSpoolID
			LEFT JOIN Embarque e on e.EmbarqueID = es.EmbarqueID

	-- Insert Avance
	INSERT INTO #TempAvance
		select	avance.SpoolID,
				avance.TotalJuntasShopSpool,
				avance.TotalJuntasSoldables,
				avance.JuntasArmadas,
				avance.JuntasSoldadas,
				cast((cast(avance.JuntasArmadas as decimal(5,2)) / cast(case when avance.TotalJuntasShopSpool = 0 then 1 else avance.TotalJuntasShopSpool end as decimal(5,2))) * 100.0 as decimal(10,2)) [PorcentajeArmado],
				cast((cast(avance.JuntasSoldadas as decimal(5,2)) / cast(case when avance.TotalJuntasSoldables = 0 then 1 else avance.TotalJuntasSoldables end as decimal(5,2))) * 100.0 as decimal(10,2)) [PorcentajeSoldado]
		from
		(
			select	js.SpoolID,
					COUNT(js.JuntaSpoolID) [TotalJuntasShopSpool],
					COUNT(js.JuntaSpoolID) [TotalJuntasSoldables],
					SUM(
						CASE	WHEN jw.JuntaArmadoID is not null and jw.ArmadoAprobado = 1 THEN 1
								ELSE 0 END
					) [JuntasArmadas],
					SUM(
						CASE	WHEN (js.TipoJuntaID = @TW and jw.JuntaArmadoID is not null and jw.ArmadoAprobado = 1) or (jw.JuntaSoldaduraID is not null and jw.SoldaduraAprobada = 1) THEN 1
								ELSE 0 END
					) [JuntasSoldadas],
					s.Pdis
			from	JuntaSpool js
			left join JuntaWorkstatus jw on js.JuntaSpoolID = jw.JuntaSpoolID and jw.VersionJunta = 1
			left join Spool s on s.SpoolID = js.SpoolID
			left join WorkstatusSpool wss on jw.OrdenTrabajoSpoolID = wss.OrdenTrabajoSpoolID
			where	js.FabAreaID != @FIELD and -- Excluimos juntas field para armado y soldadura
					js.TipoJuntaID != @TH      -- Excluuimos juntas TH para armado y soldadura
					and exists
					(
						select 1
						from #TempJuntaSpoolB tjs
						where	tjs.SpoolID = js.SpoolID
					)
			group by js.SpoolID, s.Pdis
		) avance				


	--INSERT SPOOL REPORTE PND
	INSERT INTO #TempSpoolReportePnd
	SELECT DISTINCT srpnd.SpoolReportePndID,
			srpnd.WorkstatusSpoolID,
			rpnd.TipoPruebaSpoolID,
			rpnd.FechaReporte,
			rpnd.NumeroReporte,
			srpnd.SpoolRequisicionID,
			srpnd.FechaPrueba,
			srpnd.Hoja,
			srpnd.Aprobado,
			srpnd.Observaciones,
			srpnd.FechaModificacion,
			rpnd.Presion
	FROM SpoolReportePnd srpnd
	INNER JOIN #TempWorkstatusSpool ws on ws.WorkstatusSpoolID = srpnd.WorkstatusSpoolID 
	INNER JOIN ReporteSpoolPnd rpnd on rpnd.ReporteSpoolPndID = srpnd.ReporteSpoolPndID

	--INSERT PRUEBA HIDROSTÁTICA
	INSERT INTO #TempPruebaHidrostatica 
	SELECT DISTINCT srpnd.SpoolReportePndID,
					sr.WorkstatusSpoolID,
					rs.FechaRequisicion,
					rs.NumeroRequisicion,
					srpnd.FechaPrueba,
					srpnd.FechaReporte,
					srpnd.NumeroReporte,
					srpnd.Hoja,
					srpnd.Aprobado,
					srpnd.Presion
	FROM SpoolRequisicion sr 		
	INNER JOIN RequisicionSpool rs on rs.RequisicionSpoolID = sr.RequisicionSpoolID
	LEFT JOIN #TempSpoolReportePnd srpnd on sr.SpoolRequisicionID = srpnd.SpoolRequisicionID
	LEFT JOIN #TempWorkstatusSpool ws on sr.WorkstatusSpoolID = ws.WorkstatusSpoolID
	WHERE rs.TipoPruebaSpoolID = 1


	INSERT INTO #TempTransferenciaSpool
	SELECT A.SpoolID,B.NumeroTransferencia,C.FechaPreparacion,B.FechaTransferencia
	 FROM(
			SELECT
				a.SpoolID,
				max(a.TransferenciaSpoolID) AS TransferenciaSpoolID
			from transferenciaSpool a		
			inner join transferencia b on a.transferenciaSpoolid=b.transferenciaSpoolid
			inner join #TempOrdenTrabajoSpool c on c.SpoolID=a.SpoolID
			group by a.spoolid
	) A
	INNER JOIN Transferencia B ON A.TransferenciaSpoolID=B.TransferenciaSpoolID
	INNER JOIN TransferenciaSpool C ON A.TransferenciaSpoolID=C.TransferenciaSpoolID

	--INSERT GENERAL	
	INSERT INTO #TempGeneral
		SELECT jws.JuntaWorkStatusID,
			   jws.JuntaCampo,
			   js.JuntaSpoolID,
			   [RequierePwht] = case
								 when js.RequierePwht = 0 then 'No'
								 when js.RequierePwht = 1 then 'Si'
								end,
			   [RequierePruebaNeumatica]	= case							
								 when js.RequierePruebaNeumatica = 1 then 'Si'
								 else 'No'
								end,								
			   p.Nombre,
			   ot.NumeroOrden,
			   ot.FechaOrden,
			   ots.NumeroControl,
			   s.Nombre,
			   b.LetraBaston,
			   b.Estacion,
	           [SegundaFabricacion] = case 
	                                   when b.LetraBaston <> 'MAN' then sf.SegundaFabricacion
	                                   when b.LetraBaston = 'MAN' then ''
	                                  end,
			   ISNULL(jws.EtiquetaJunta,js.EtiquetaJunta) ,
			   js.TipoJunta,
			   js.Diametro,
			   s.DiametroMayor,
			   js.Cedula,
			   js.Espesor,
			   js.EtiquetaMaterial1 + ' - ' + js.EtiquetaMaterial2,
			   s.UltimaLocalizacion,
			   up.Nombre,
			   ISNULL((select '1'
				where sh.TieneHoldCalidad = 1 or
					  sh.TieneHoldIngenieria = 1),0),
			    s.Campo1,
			    s.Campo2,
			    s.Campo3,
			    s.Campo4,
			    s.Campo5,
				js.peqs,
				js.KgTeoricos,
				js.CodigoFabArea,
				ots.OrdenTrabajoSpoolID,
				js.ItemCodeIDMaterial1,
				js.CodigoItemCodeMaterial1,
				js.DescripcionItemCodeMaterial1,
				js.ItemCodeIDMaterial2,
				js.CodigoItemCodeMaterial2,
				js.DescripcionItemCodeMaterial2,
				fa1.Nombre,
				fm1.Nombre,
				fa2.Nombre,
				fm2.Nombre,
				s.PorcentajePnd,
				s.Especificacion,
				js.DiamMat1,
			    js.EspMat1,
			    js.DiamMat2,
			    js.EspMat2,
			    case when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = s.SpoolID and tr.TieneOT = 0) then 0 when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = s.SpoolID and tr.TieneDespacho = 1) then 100 when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = s.SpoolID and tr.TieneDespacho = 0) then 0 else isnull(grupo.PorcArmado,0) end,
				case when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = s.SpoolID and tr.TieneOT = 0) then 0 when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = s.SpoolID and tr.TieneDespacho = 1) then 100 when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = s.SpoolID and tr.TieneDespacho = 0) then 0 else isnull(grupo.PorcSoldado,0) end,
				s.Prioridad,
			    s.Isometrico,
			    s.RevisionStg,
			    s.RevisionCte,
			    s.SistemaPintura,
				s.ColorPintura,
				s.CodigoPintura,
				SpoolHoldHistorial.Observaciones,
				SpoolHoldHistorial.FechaHold,
				s.DiametroPlano,
				s.FechaEtiquetaEmbarque,
				s.NumeroEtiquetaEmbarque,
				case when totm.MaterialPendiente > 0 then '1'
				else '0' end,
				case when (mat1.TieneInventarioCongelado = 0 and mat1.TieneCorte = 0 and mat1.TieneDespacho = 0 and mat1.EsAsignado = 0)
					or (mat2.TieneInventarioCongelado = 0 and mat2.TieneCorte = 0 and mat2.TieneDespacho = 0 and mat2.EsAsignado = 0) then '1'
				else '0' end,
				nu1.Codigo,
				nu2.Codigo,
				s.SpoolID,
				s.RequierePwht,		
				CASE WHEN s.RequierePruebaHidrostatica IS NOT NULL 
					 THEN
						CASE WHEN s.RequierePruebaHidrostatica = 1 
							 THEN 
								1 
							 ELSE 
								0 
						END
					 ELSE
						NULL
				END,	
				js.FabClas,
				js.CampoJunta2,
				js.CampoJunta3,
				js.CampoJunta4,
				js.CampoJunta5,
				ots.FechaLiberacionCalidad,
				ots.UsuarioliberacionCalidad,
				ots.FechaLiberacionMaterial,
				ots.UsuarioLiberacionMaterial,
				ots.FechaOkPnd,
				ots.UsuarioOkPnd,
				s.UltimoProceso,
				s.AreaGrupo,
				s.KgsGrupo,
				s.DiamGrupo,
				s.PeqGrupo,
				s.SistemaPinturaFinal,
				s.Paint,
				s.DiametroPromedio,
				s.PaintLine,
				s.AreaEQ,
				s.Inox,
				s.ClasifInox,
				s.FechaLocalizacion
			FROM #TempJuntaSpoolB js
			LEFT JOIN
			(
				select [LetraBaston] = ISNULL(bs.LetraBaston, ''),
					   [Estacion] = ISNULL(e.Nombre, ''),
					   js.JuntaSpoolID
				from #TempJuntaSpoolB js
				left join BastonSpoolJunta bsj on js.JuntaSpoolID = bsj.JuntaSpoolID
				inner join BastonSpool bs on bsj.BastonSpoolID = bs.BastonSpoolID
				left join Estacion e on bs.EstacionID = e.EstacionID
			) b on js.JuntaSpoolID = b.JuntaSpoolID
			LEFT JOIN #TempJuntaWorkstatus jws on js.JuntaSpoolID = jws.JuntaSpoolID
			INNER JOIN #TempProyecto p on 1 = 1
			LEFT JOIN #TempOrdenTrabajoSpool ots on ots.SpoolID = js.SpoolID
			LEFT JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
			LEFT JOIN 
				(
					select	OrdenTrabajoSpoolID,
							count(ordentrabajoSpoolid) as [MaterialPendiente]
					from #TempOrdenTrabajoMaterial otm
					where otm.TieneInventarioCongelado = 0 and otm.TieneCorte = 0 and otm.TieneDespacho = 0 and otm.EsAsignado = 0
					group by otm.OrdenTrabajoSpoolID
				) totm on ots.OrdenTrabajoSpoolID = totm.OrdenTrabajoSpoolID
			LEFT JOIN #TempOrdenTrabajoMaterial mat1 on mat1.MaterialSpoolID = js.MaterialSpool1ID
			LEFT JOIN Despacho desp1 on mat1.DespachoID = desp1.DespachoID and desp1.Cancelado = 0
			LEFT JOIN NumeroUnico nu1 on desp1.NumeroUnicoID = nu1.NumeroUnicoID
			LEFT JOIN #TempOrdenTrabajoMaterial mat2 on mat2.MaterialSpoolID = js.MaterialSpool2ID
			LEFT JOIN Despacho desp2 on mat2.DespachoID = desp2.DespachoID and desp2.Cancelado = 0
			LEFT JOIN NumeroUnico nu2 on desp2.NumeroUnicoID = nu2.NumeroUnicoID
			INNER JOIN #TempSpool s on s.SpoolID = js.SpoolID
			left join
			(
				select [SegundaFabricacion] = ISNULL(e.Nombre, ''), s.SpoolID
				from #TempSpool s
				left join BastonSpool bs on s.SpoolID = bs.SpoolID
				left join Estacion e on bs.EstacionID = e.EstacionID
				where bs.LetraBaston = 'MAN'
			) sf on s.SpoolID = sf.SpoolID
			LEFT JOIN UltimoProceso up on up.UltimoProcesoID = jws.UltimoProcesoID
			LEFT JOIN SpoolHold sh on sh.SpoolID = s.SpoolID
			LEFT JOIN 
			(
				SELECT ts.SpoolID,MAX(SpoolHoldHistorial.SpoolHoldHistorialID) AS SpoolHoldHistorialID
				FROM #Tempspool ts INNER JOIN SpoolHoldHistorial ON ts.SpoolID = SpoolHoldHistorial.SpoolID
				GROUP BY ts.SpoolID
			) tHoldHistorial ON tHoldHistorial.SpoolID = s.SpoolID
			LEFT JOIN SpoolHoldHistorial ON SpoolHoldHistorial.SpoolHoldHistorialID = tHoldHistorial.SpoolHoldHistorialID
			INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
			INNER JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
			LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
			LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
			LEFT JOIN #TempAvance grupo on js.SpoolID = grupo.SpoolID

	--DESPLEGAR TABLA

	select g.GeneralJuntaWorkstatusID,
		   g.GeneralJuntaCampo,
		   g.GeneralJuntaSpoolID,
		   g.GeneralJuntaRequierePwht,
		   g.GeneralJuntaRequierePruebaNeumatica,
		   g.GeneralProyecto,
		   g.GeneralOrdenDeTrabajo,
		   g.GeneralFechaEmision,
		   g.GeneralNumeroDeControl,
		   g.GeneralSpool,
		   g.GeneralBaston,
		   g.GeneralEstacion,
		   g.GeneralSegundaFabricacion,
		   g.GeneralJunta,
		   g.GeneralTipoJunta,
		   g.GeneralDiametro,
		   g.GeneralDiametroMayor,
		   g.GeneralCedula,
		   g.GeneralEspesor,
		   g.GeneralLocalizacion,
		   g.GeneralUltimaLocalizacion,
		   g.GeneralUltimoProceso,
		   g.GeneralTieneHold,
		   g.Campo1,
		   g.Campo2,
		   g.Campo3,
		   g.Campo4,
		   g.Campo5,
		   g.CodigoItemCodeMaterial1,
		   g.DescripcionItemCodeMaterial1,
		   g.CodigoItemCodeMaterial2,
		   g.DescripcionItemCodeMaterial2,
		   g.GeneralPeqs,
		   g.GeneralKgTeoricos,
		   g.GeneralFabArea,
		   g.PorcentajeArmado,
		   g.PorcentajeSoldado,
		   g.GeneralPrioridad,
		   g.GeneralIsometrico,
		   g.GeneralRevisionSteelgo,
		   g.GeneralRevisionCliente,
		   g.GeneralMaterialPendienteSpool,
		   g.GeneralMaterialPendienteJunta,
		   g.GeneralDespachado1,
		   g.GeneralDespachado2,
		   g.FabClas,
		   g.CampoJunta2,
		   g.CampoJunta3,
		   g.CampoJunta4,
		   g.CampoJunta5,
		   g.GeneralFechaLiberacionCalidad,
		   g.GeneralUsuarioLiberacionCalidad,
		   g.GeneralFechaLiberacionMaterial,
		   g.GeneralUsuarioLiberacionMaterial,
		   g.GeneralFechaOkPnd,
		   g.GeneralUsuarioOkPnd,
		   ja.FechaArmado [ArmadoFecha],
		   ja.FechaReporte [ArmadoFechaReporte],
		   ja.Taller [ArmadoTaller],
		   ja.Tubero [ArmadoTubero],
		   ja.NumeroUnico1 [ArmadoNumeroUnico1],
		   ja.NumeroUnico2 [ArmadoNumeroUnico2],
		   [NumeroUnico1Pendiente] = case when ja.NumeroUnico1 is null then CONVERT(bit, 1) when ja.NumeroUnico1 is not null then CONVERT(bit, 0) end,
		   [NumeroUnico2Pendiente] = case when ja.NumeroUnico2 is null then CONVERT(bit, 1) when ja.NumeroUnico2 is not null then CONVERT(bit, 0) end,
		   ja.EtiquetaMaterial1 [ArmadoEtiquetaMaterial1],
		   ja.EtiquetaMaterial2 [ArmadoEtiquetaMaterial2],
		   ja.Spool1 [ArmadoSpool1],
		   ja.Spool2 [ArmadoSpool2],
		   ja.ObservacionesArmado,
		   js.SoldaduraFecha,
		   js.SoldaduraFechaReporte,
		   js.SoldaduraTaller,
		   js.SoldaduraWPS,
		   js.SoldaduraWPSRelleno,
		   js.SoldaduraProcesoRelleno,
		   js.SoldaduraConsumiblesRelleno,
		   js.SoldaduraProcesoRaiz,
		   js.SoldaduraSoldadorRaiz,
		   js.SoldaduraSoldadorRelleno,
		   js.SoldaduraMaterialBase1,
		   js.SoldaduraMaterialBase2,
		   js.ObservacionesSoldadura,
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
		   prt.PruebaRTSector,
		   prt.PruebaRTCuadrante,
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
		   ppmi.PruebaPMIFechaRequisicion,
		   ppmi.PruebaPMINumeroRequisicion,
		   ppmi.PruebaPMICodigoRequisicion,
		   ppmi.PruebaPMIFechaPrueba,
		   ppmi.PruebaPMIFechaReporte,
		   ppmi.PruebaPMINumeroReporte,
		   ppmi.PruebaPMIHoja,
		   ppmi.PruebaPMIResultado,
		   ppmi.PruebaPMIDefecto,
		   ppmi.PruebaPMIObservacionesReporte,
		   ppmi.PruebaPMIObservacionesRequisicion,		   
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
		   pn.PruebaNeumaticaFechaRequisicion,
		   pn.PruebaNeumaticaNumeroRequisicion,
		   pn.PruebaNeumaticaCodigoRequisicion,
		   pn.PruebaNeumaticaFechaPrueba,
		   pn.PruebaNeumaticaFechaReporte,
		   pn.PruebaNeumaticaNumeroReporte,
		   pn.PruebaNeumaticaHoja,
		   pn.PruebaNeumaticaResultado,
		   pn.PruebaNeumaticaDefecto,		   
		   pn.PruebaNeumaticaSector,
		   pn.PruebaNeumaticaCuadrante,
		   pn.PruebaNeumaticaObservacionesReporte,
		   pn.PruebaNeumaticaObservacionesRequisicion,
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
		   g.PinturaSistema,
		   g.PinturaColor,
		   g.PinturaCodigo,
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
		   p.PinturaLiberado,
		   g.GeneralEtiquetaEmbarque as EmbarqueEtiqueta,
		   g.GeneralFechaEtiquetaEmbarque as EmbarqueFechaEtiqueta,
		   e.EmbarqueFechaPreparacion,
		   e.EmbarqueFechaEmbarque,
		   e.EmbarqueFechaEstimada,
		   e.EmbarqueNumeroEmbarque,
		   e.Nota1,
		   e.Nota2,
		   e.Nota3,
		   e.Nota4,
		   e.Nota5,
		   e.EmbarqueVigenciaAduana,
		   g.GeneralFamiliaAcero1,
		   g.GeneralFamiliaMaterial1,
		   g.GeneralFamiliaAcero2,
		   g.GeneralFamiliaMaterial2,
		   g.GeneralPorcentajePnd,
		   g.GeneralEspecificacion,
		   g.GeneralDiamMat1,
		   g.GeneralEspMat1,
		   g.GeneralDiamMat2,
		   g.GeneralEspMat2,
		   ja.ArmadoCedMat1, 
		   ja.ArmadoCedMat2,
		   g.ObservacionesHold,
		   g.FechaHold AS FechaHold,
		   g.GeneralDiametroPlano AS DiametroPlano,
		   g.GeneralPWHT,
		   g.GeneralRequierePruebaHidrostatica,
		   tmph.PruebaHidrostaticaFechaRequisicion as PruebaHidroFechaRequisicion,
		   tmph.PruebaHidrostaticaNumeroRequisicion as PruebaHidroNumeroRequisicion,
		   tmph.PruebaHidrostaticaFechaPrueba as PruebaHidroFechaPrueba,
		   tmph.PruebaHidrostaticaFechaReporte as PruebaHidroFechaReporte,
		   tmph.PruebaHidrostaticaNumeroReporte as PruebaHidroNumeroReporte,
	       tmph.PruebaHidrostaticaHoja as PruebaHidroHoja,
		   tmph.PruebaHidrostaticaAprobado as PruebaHidroAprobado,
		   tmph.PruebaHidroPresion,
		   tom.Cortador [GeneralCortador],
		   tom.Despachador [GeneralDespachador],	   
		   g.AgrupadoresSpoolUltimoProceso,
			g.AgrupadoresAreaGrupo,
			g.AgrupadoresKgsGrupo,
			g.AgrupadoresDiamGrupo,
			g.AgrupadoresPeqGrupo,
			g.AgrupadoresSistemaPinturaFinal,
			g.AgrupadoresPaintNoPaint,
			g.AgrupadoresDiametroPromedio,
			g.AgrupadoresPaintLine,
			g.AgrupadoresAreaEQ,
			g.AgrupadoresInoxNoInox,
			g.AgrupadoresClasifInox,
			g.GeneralFechaLocalizacion AS GeneralFechaLocalizacion,
			agxj.AgrupadoresPorJuntaClasifPND,
			agxj.AgrupadoresPorJuntaClasifReparacion,
			agxj.AgrupadoresPorJuntaClasifSoporte,
			agxj.AgrupadoresPorJuntaGrupo1,
			agxj.AgrupadoresPorJuntaGrupo2,
			agxj.AgrupadoresPorJuntaGrupo3,
			agxj.AgrupadoresPorJuntaGrupo4,
			agxj.AgrupadoresPorJuntaGrupo5,
			tts.NumeroTransferencia,
			tts.PreparacionTransferencia,
			tts.Transferencia
	from #TempGeneral g 
	LEFT JOIN #TempJuntaWorkstatus jw on g.GeneralJuntaWorkstatusID = jw.JuntaWorkStatusID and g.GeneralJuntaCampo = jw.JuntaCampo
	LEFT JOIN
	(
		SELECT	jarm.JuntaArmadoID [JuntaArmadoID],
				jarm.JuntaWorkstatusID [JuntaWorkstatusID],
				CAST(0 as bit) [JuntaCampo],
				FechaArmado,
				FechaReporte,
				ta.Nombre [Taller],
				tu.Codigo [Tubero],
				NumeroUnico1ID,
				NumeroUnico2ID,
				nu1.Codigo [NumeroUnico1],
				nu2.Codigo [NumeroUnico2],
				jarm.TuberoID,
				nu1.Cedula [ArmadoCedMat1],
				nu2.Cedula [ArmadoCedMat2],
				null [EtiquetaMaterial1],
				null [EtiquetaMaterial2],
				null [Spool1],
				null [Spool2],
				jarm.Observaciones [ObservacionesArmado]
		FROM JuntaArmado jarm
		INNER JOIN Taller ta on ta.TallerID = jarm.TallerID 
		INNER JOIN Tubero tu on tu.TuberoID = jarm.TuberoID
		LEFT JOIN NumeroUnico nu1 on nu1.NumeroUnicoID = jarm.NumeroUnico1ID
		LEFT JOIN NumeroUnico nu2 on nu2.NumeroUnicoID = jarm.NumeroUnico2ID
		WHERE jarm.JuntaWorkstatusID IN
		(
			SELECT JuntaWorkstatusID FROM #TempJuntaWorkstatus WHERE JuntaCampo = 0
		)
		union
		SELECT	jarm.JuntaCampoArmadoID [JuntaArmadoID],
				jarm.JuntaCampoID [JuntaWorkstatusID],
				CAST(1 as bit) [JuntaCampo],
				FechaArmado,
				FechaReporte,
				null [Taller],
				tu.Codigo [Tubero],
				NumeroUnico1ID,
				NumeroUnico2ID,
				nu1.Codigo [NumeroUnico1],
				nu2.Codigo [NumeroUnico2],
				jarm.TuberoID,
				nu1.Cedula [ArmadoCedMat1],
				nu2.Cedula [ArmadoCedMat2],
				jarm.EtiquetaMaterial1,
				jarm.EtiquetaMaterial2,
				(select Nombre from Spool where SpoolID = jarm.Spool1ID) [Spool1],
				(select Nombre from Spool where SpoolID = jarm.Spool2ID) [Spool2],
				jarm.Observaciones [ObservacionesArmado]
		FROM JuntaCampoArmado jarm
		INNER JOIN Tubero tu on tu.TuberoID = jarm.TuberoID
		LEFT JOIN NumeroUnico nu1 on nu1.NumeroUnicoID = jarm.NumeroUnico1ID
		LEFT JOIN NumeroUnico nu2 on nu2.NumeroUnicoID = jarm.NumeroUnico2ID
		WHERE jarm.JuntaCampoID IN
		(
			SELECT JuntaWorkstatusID FROM #TempJuntaWorkstatus WHERE JuntaCampo = 1
		)		
	) ja on ja.JuntaWorkstatusID = jw.JuntaWorkStatusID and ja.JuntaCampo = jw.JuntaCampo
	LEFT JOIN #TempJuntaSoldadura js on js.SoldaduraJuntaWorkstatusID = jw.JuntaWorkStatusID and js.SoldaduraJuntaCampo = jw.JuntaCampo
	LEFT JOIN #TempJuntaInspeccionVisual iv on iv.InspeccionVisualJuntaWorkstatusID = jw.JuntaWorkStatusID and iv.InspeccionVisualJuntaCampo = jw.JuntaCampo
	LEFT JOIN #TempOrdenTrabajoSpool ots on g.SpoolID = ots.SpoolID
	LEFT JOIN #TempTransferenciaSpool tts on tts.SpoolID=ots.SpoolID
	LEFT JOIN #TempWorkstatusSpool ws on ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
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
				tmp.PruebaRTJuntaWorkstatusID,
				tmp.PruebaRTJuntaCampo,
				tmp.PruebaRTSector,
				tmp.PruebaRTCuadrante
		from #TempPruebaRT tmp
		left join(
			select	PruebaRTJuntaWorkstatusID, 
					MAX(PruebaRTFechaPrueba) as PruebaRTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaRT
			where PruebaRTJuntaCampo = 0
			group by PruebaRTJuntaWorkstatusID
		) A on	A.PruebaRTJuntaWorkstatusID = tmp.PruebaRTJuntaWorkstatusID
				and A.PruebaRTFechaPrueba = tmp.PruebaRTFechaPrueba
				AND A.FechaModificacion = tmp.FechaModificacion
		where tmp.PruebaRTJuntaCampo = 0
		union
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
				tmp.PruebaRTJuntaWorkstatusID,
				tmp.PruebaRTJuntaCampo,
				tmp.PruebaRTSector,
				tmp.PruebaRTCuadrante
		from #TempPruebaRT tmp
		left join(
			select	PruebaRTJuntaWorkstatusID, 
					MAX(PruebaRTFechaPrueba) as PruebaRTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaRT
			where PruebaRTJuntaCampo = 1
			group by PruebaRTJuntaWorkstatusID
		) A on	A.PruebaRTJuntaWorkstatusID = tmp.PruebaRTJuntaWorkstatusID
				and A.PruebaRTFechaPrueba = tmp.PruebaRTFechaPrueba
				AND A.FechaModificacion = tmp.FechaModificacion
		where tmp.PruebaRTJuntaCampo = 1
	)  prt on prt.PruebaRTJuntaWorkstatusID = jw.JuntaWorkStatusID and prt.PruebaRTJuntaCampo = jw.JuntaCampo
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
				tmp.PruebaPTJuntaWorkstatusID,
				tmp.PruebaPTJuntaCampo
		from #TempPruebaPT tmp
		left join(
			select	PruebaPTJuntaWorkstatusID, 
					MAX(PruebaPTFechaPrueba) as PruebaPTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion						
			from #TempPruebaPT
			where PruebaPTJuntaCampo = 0
			group by PruebaPTJuntaWorkstatusID
		) A on A.PruebaPTJuntaWorkstatusID = tmp.PruebaPTJuntaWorkstatusID
		and A.PruebaPTFechaPrueba = tmp.PruebaPTFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where tmp.PruebaPTJuntaCampo = 0
		union
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
				tmp.PruebaPTJuntaWorkstatusID,
				tmp.PruebaPTJuntaCampo
		from #TempPruebaPT tmp
		left join(
			select	PruebaPTJuntaWorkstatusID, 
					MAX(PruebaPTFechaPrueba) as PruebaPTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion						
			from #TempPruebaPT
			where PruebaPTJuntaCampo = 1
			group by PruebaPTJuntaWorkstatusID
		) A on A.PruebaPTJuntaWorkstatusID = tmp.PruebaPTJuntaWorkstatusID
		and A.PruebaPTFechaPrueba = tmp.PruebaPTFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where tmp.PruebaPTJuntaCampo = 1		
	)  ppt on ppt.PruebaPTJuntaWorkstatusID = jw.JuntaWorkStatusID and ppt.PruebaPTJuntaCampo = jw.JuntaCampo
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
				tmp.PruebaRTPostTTJuntaWorkstatusID,
				tmp.PruebaRTPostTTJuntaCampo
		from #TempPruebaRTPostTT tmp
		left join(
			select	PruebaRTPostTTJuntaWorkstatusID, 
					MAX(PruebaRTPostTTFechaPrueba) as PruebaRTPostTTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaRTPostTT
			where PruebaRTPostTTJuntaCampo = 0
			group by PruebaRTPostTTJuntaWorkstatusID
		) A on A.PruebaRTPostTTJuntaWorkstatusID = tmp.PruebaRTPostTTJuntaWorkstatusID
		and A.PruebaRTPostTTFechaPrueba = tmp.PruebaRTPostTTFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaRTPostTTJuntaCampo = 0
		union
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
				tmp.PruebaRTPostTTJuntaWorkstatusID,
				tmp.PruebaRTPostTTJuntaCampo
		from #TempPruebaRTPostTT tmp
		left join(
			select	PruebaRTPostTTJuntaWorkstatusID, 
					MAX(PruebaRTPostTTFechaPrueba) as PruebaRTPostTTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaRTPostTT
			where PruebaRTPostTTJuntaCampo = 1
			group by PruebaRTPostTTJuntaWorkstatusID
		) A on A.PruebaRTPostTTJuntaWorkstatusID = tmp.PruebaRTPostTTJuntaWorkstatusID
		and A.PruebaRTPostTTFechaPrueba = tmp.PruebaRTPostTTFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaRTPostTTJuntaCampo = 1		
	)  prtptt on prtptt.PruebaRTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID and prtptt.PruebaRTPostTTJuntaCampo = jw.JuntaCampo
	LEFT JOIN
	(
		select	tmp.PruebaPMIFechaRequisicion,
				tmp.PruebaPMINumeroRequisicion,
				tmp.PruebaPMICodigoRequisicion,
				tmp.PruebaPMIFechaPrueba,
				tmp.PruebaPMIFechaReporte,
				tmp.PruebaPMINumeroReporte,
				tmp.PruebaPMIHoja,
				tmp.PruebaPMIResultado,
				tmp.PruebaPMIDefecto,
				tmp.PruebaPMIObservacionesReporte,
				tmp.PruebaPMIObservacionesRequisicion, 
				tmp.PruebaPMIJuntaWorkstatusID,
				tmp.PruebaPMIJuntaCampo
		from #TempPruebaPMI tmp
		left join(
			select	PruebaPMIJuntaWorkstatusID, 
					MAX(PruebaPMIFechaPrueba) as PruebaPMIFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaPMI
			where PruebaPMIJuntaCampo = 0
			group by PruebaPMIJuntaWorkstatusID
		) A on A.PruebaPMIJuntaWorkstatusID = tmp.PruebaPMIJuntaWorkstatusID
		and A.PruebaPMIFechaPrueba = tmp.PruebaPMIFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaPMIJuntaCampo = 0
		union
		select	tmp.PruebaPMIFechaRequisicion,
				tmp.PruebaPMINumeroRequisicion,
				tmp.PruebaPMICodigoRequisicion,
				tmp.PruebaPMIFechaPrueba,
				tmp.PruebaPMIFechaReporte,
				tmp.PruebaPMINumeroReporte,
				tmp.PruebaPMIHoja,
				tmp.PruebaPMIResultado,
				tmp.PruebaPMIDefecto,
				tmp.PruebaPMIObservacionesReporte,
				tmp.PruebaPMIObservacionesRequisicion, 
				tmp.PruebaPMIJuntaWorkstatusID,
				tmp.PruebaPMIJuntaCampo
		from #TempPruebaPMI tmp
		left join(
			select	PruebaPMIJuntaWorkstatusID, 
					MAX(PruebaPMIFechaPrueba) as PruebaPMIFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaPMI
			where PruebaPMIJuntaCampo = 1
			group by PruebaPMIJuntaWorkstatusID
		) A on A.PruebaPMIJuntaWorkstatusID = tmp.PruebaPMIJuntaWorkstatusID
		and A.PruebaPMIFechaPrueba = tmp.PruebaPMIFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaPMIJuntaCampo = 1		
	)  ppmi on ppmi.PruebaPMIJuntaWorkstatusID = jw.JuntaWorkStatusID and ppmi.PruebaPMIJuntaCampo = jw.JuntaCampo
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
				tmp.PruebaPTPostTTJuntaWorkstatusID,
				tmp.PruebaPTPostTTJuntaCampo
		from #TempPruebaPTPostTT tmp
		left join(
			select	PruebaPTPostTTJuntaWorkstatusID, 
					MAX(PruebaPTPostTTFechaPrueba) as PruebaPTPostTTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaPTPostTT
			where PruebaPTPostTTJuntaCampo = 0
			group by PruebaPTPostTTJuntaWorkstatusID
		) A on	A.PruebaPTPostTTJuntaWorkstatusID = tmp.PruebaPTPostTTJuntaWorkstatusID
				and A.PruebaPTPostTTFechaPrueba = tmp.PruebaPTPostTTFechaPrueba
				AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaPTPostTTJuntaCampo = 0
		union
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
				tmp.PruebaPTPostTTJuntaWorkstatusID,
				tmp.PruebaPTPostTTJuntaCampo
		from #TempPruebaPTPostTT tmp
		left join(
			select	PruebaPTPostTTJuntaWorkstatusID, 
					MAX(PruebaPTPostTTFechaPrueba) as PruebaPTPostTTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaPTPostTT
			where PruebaPTPostTTJuntaCampo = 1
			group by PruebaPTPostTTJuntaWorkstatusID
		) A on	A.PruebaPTPostTTJuntaWorkstatusID = tmp.PruebaPTPostTTJuntaWorkstatusID
				and A.PruebaPTPostTTFechaPrueba = tmp.PruebaPTPostTTFechaPrueba
				AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaPTPostTTJuntaCampo = 1		
	)  pptptt on pptptt.PruebaPTPostTTJuntaWorkstatusID = jw.JuntaWorkStatusID and pptptt.PruebaPTPostTTJuntaCampo = jw.JuntaCampo
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
				tmp.PruebaUTJuntaWorkstatusID,
				tmp.PruebaUTJuntaCampo
		from #TempPruebaUT tmp
		left join(
			select	PruebaUTJuntaWorkstatusID, 
					MAX(PruebaUTFechaPrueba) as PruebaUTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaUT
			where PruebaUTJuntaCampo = 0
			group by PruebaUTJuntaWorkstatusID
		) A on A.PruebaUTJuntaWorkstatusID = tmp.PruebaUTJuntaWorkstatusID
		and A.PruebaUTFechaPrueba = tmp.PruebaUTFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaUTJuntaCampo = 0
		union
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
				tmp.PruebaUTJuntaWorkstatusID,
				tmp.PruebaUTJuntaCampo
		from #TempPruebaUT tmp
		left join(
			select	PruebaUTJuntaWorkstatusID, 
					MAX(PruebaUTFechaPrueba) as PruebaUTFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaUT
			where PruebaUTJuntaCampo = 1
			group by PruebaUTJuntaWorkstatusID
		) A on A.PruebaUTJuntaWorkstatusID = tmp.PruebaUTJuntaWorkstatusID
		and A.PruebaUTFechaPrueba = tmp.PruebaUTFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaUTJuntaCampo = 1
	)  put on put.PruebaUTJuntaWorkstatusID = jw.JuntaWorkStatusID and put.PruebaUTJuntaCampo = jw.JuntaCampo
	LEFT JOIN
	( 
		select	tmp.PruebaNeumaticaFechaRequisicion,
				tmp.PruebaNeumaticaNumeroRequisicion,
				tmp.PruebaNeumaticaCodigoRequisicion,
				tmp.PruebaNeumaticaFechaPrueba,
				tmp.PruebaNeumaticaFechaReporte,
				tmp.PruebaNeumaticaNumeroReporte,
				tmp.PruebaNeumaticaHoja,
				tmp.PruebaNeumaticaResultado,
				tmp.PruebaNeumaticaDefecto,				
				tmp.PruebaNeumaticaSector,
				tmp.PruebaNeumaticaCuadrante,
				tmp.PruebaNeumaticaObservacionesReporte,
				tmp.PruebaNeumaticaObservacionesRequisicion,
				tmp.PruebaNeumaticaJuntaWorkstatusID,
				tmp.PruebaNeumaticaJuntaCampo
		from #TempPruebaNeumatica tmp
		left join(
			select	PruebaNeumaticaJuntaWorkstatusID, 
					MAX(PruebaNeumaticaFechaPrueba) as PruebaNeumaticaFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaNeumatica
			where PruebaNeumaticaJuntaCampo = 0
			group by PruebaNeumaticaJuntaWorkstatusID
		) A on A.PruebaNeumaticaJuntaWorkstatusID = tmp.PruebaNeumaticaJuntaWorkstatusID
		and A.PruebaNeumaticaFechaPrueba = tmp.PruebaNeumaticaFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaNeumaticaJuntaCampo = 0
		union
		select	tmp.PruebaNeumaticaFechaRequisicion,
				tmp.PruebaNeumaticaNumeroRequisicion,
				tmp.PruebaNeumaticaCodigoRequisicion,
				tmp.PruebaNeumaticaFechaPrueba,
				tmp.PruebaNeumaticaFechaReporte,
				tmp.PruebaNeumaticaNumeroReporte,
				tmp.PruebaNeumaticaHoja,
				tmp.PruebaNeumaticaResultado,
				tmp.PruebaNeumaticaDefecto,				
				tmp.PruebaNeumaticaSector,
				tmp.PruebaNeumaticaCuadrante,
				tmp.PruebaNeumaticaObservacionesReporte,
				tmp.PruebaNeumaticaObservacionesRequisicion,
				tmp.PruebaNeumaticaJuntaWorkstatusID,
				tmp.PruebaNeumaticaJuntaCampo
		from #TempPruebaNeumatica tmp
		left join(
			select	PruebaNeumaticaJuntaWorkstatusID, 
					MAX(PruebaNeumaticaFechaPrueba) as PruebaNeumaticaFechaPrueba,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempPruebaNeumatica
			where PruebaNeumaticaJuntaCampo = 1
			group by PruebaNeumaticaJuntaWorkstatusID
		) A on A.PruebaNeumaticaJuntaWorkstatusID = tmp.PruebaNeumaticaJuntaWorkstatusID
		and A.PruebaNeumaticaFechaPrueba = tmp.PruebaNeumaticaFechaPrueba
		AND A.FechaModificacion = tmp.FechaModificacion
		where PruebaNeumaticaJuntaCampo = 1
	)  pn on pn.PruebaNeumaticaJuntaWorkstatusID = jw.JuntaWorkStatusID and pn.PruebaNeumaticaJuntaCampo = jw.JuntaCampo	
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
				tmp.TratamientoPwhtJuntaWorkstatusID,
				tmp.TratamientoPwhtJuntaCampo
		from #TempTratamientoPwht tmp
		left join(
			select	TratamientoPwhtJuntaWorkstatusID,					 
					MAX(TratamientoPwhtFechaRequisicion) as TratamientoPwhtFechaRequisicion,
					MAX(FechaModificacion) as FechaModificacion
			from #TempTratamientoPwht
			where TratamientoPwhtJuntaCampo = 0
			group by TratamientoPwhtJuntaWorkstatusID
		) A on A.TratamientoPwhtJuntaWorkstatusID = tmp.TratamientoPwhtJuntaWorkstatusID
		and A.TratamientoPwhtFechaRequisicion = tmp.TratamientoPwhtFechaRequisicion
		and A.FechaModificacion = tmp.FechaModificacion
		where TratamientoPwhtJuntaCampo = 0
		union
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
				tmp.TratamientoPwhtJuntaWorkstatusID,
				tmp.TratamientoPwhtJuntaCampo
		from #TempTratamientoPwht tmp
		left join(
			select	TratamientoPwhtJuntaWorkstatusID, 
					MAX(TratamientoPwhtFechaTratamiento) as TratamientoPwhtFechaTratamiento,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempTratamientoPwht
			where TratamientoPwhtJuntaCampo = 1
			group by TratamientoPwhtJuntaWorkstatusID
		) A on A.TratamientoPwhtJuntaWorkstatusID = tmp.TratamientoPwhtJuntaWorkstatusID
		and A.TratamientoPwhtFechaTratamiento = tmp.TratamientoPwhtFechaTratamiento
		AND A.FechaModificacion = tmp.FechaModificacion
		where TratamientoPwhtJuntaCampo = 1
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
				tmp.TratamientoDurezasJuntaWorkstatusID,
				tmp.TratamientoDurezasJuntaCampo
		from #TempTratamientoDurezas tmp
		left join(
			select	TratamientoDurezasJuntaWorkstatusID,
					MAX(TratamientoDurezasFechaRequisicion) as TratamientoDurezasFechaRequisicion
			from #TempTratamientoDurezas
			where TratamientoDurezasJuntaCampo = 0
			group by TratamientoDurezasJuntaWorkstatusID
		) A on A.TratamientoDurezasJuntaWorkstatusID = tmp.TratamientoDurezasJuntaWorkstatusID
		and A.TratamientoDurezasFechaRequisicion = tmp.TratamientoDurezasFechaRequisicion	
		where TratamientoDurezasJuntaCampo = 0
		union	
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
				tmp.TratamientoDurezasJuntaWorkstatusID,
				tmp.TratamientoDurezasJuntaCampo
		from #TempTratamientoDurezas tmp
		left join(
			select	TratamientoDurezasJuntaWorkstatusID,
					MAX(TratamientoDurezasFechaTratamiento) as TratamientoDurezasFechaTratamiento,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempTratamientoDurezas
			where TratamientoDurezasJuntaCampo = 1
			group by TratamientoDurezasJuntaWorkstatusID
		) A on A.TratamientoDurezasJuntaWorkstatusID = tmp.TratamientoDurezasJuntaWorkstatusID
		and A.TratamientoDurezasFechaTratamiento = tmp.TratamientoDurezasFechaTratamiento			
		AND A.FechaModificacion = tmp.FechaModificacion	
		where TratamientoDurezasJuntaCampo = 1	
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
				tmp.TratamientoPreheatJuntaWorkstatusID,
				tmp.TratamientoPreheatJuntaCampo
		from #TempTratamientoPreheat tmp
		left join(
			select	TratamientoPreheatJuntaWorkstatusID,
					MAX(TratamientoPreheatFechaTratamiento) as TratamientoPreheatFechaTratamiento,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempTratamientoPreheat
			where TratamientoPreheatJuntaCampo = 0
			group by TratamientoPreheatJuntaWorkstatusID
		) A on A.TratamientoPreheatJuntaWorkstatusID = tmp.TratamientoPreheatJuntaWorkstatusID
			AND A.TratamientoPreheatFechaTratamiento = tmp.TratamientoPreheatFechaTratamiento
			AND A.FechaModificacion = tmp.FechaModificacion	
		where TratamientoPreheatJuntaCampo = 0
		union
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
				tmp.TratamientoPreheatJuntaWorkstatusID,
				tmp.TratamientoPreheatJuntaCampo
		from #TempTratamientoPreheat tmp
		left join(
			select	TratamientoPreheatJuntaWorkstatusID,
					MAX(TratamientoPreheatFechaTratamiento) as TratamientoPreheatFechaTratamiento,
					MAX(FechaModificacion) AS FechaModificacion
			from #TempTratamientoPreheat
			where TratamientoPreheatJuntaCampo = 1
			group by TratamientoPreheatJuntaWorkstatusID
		) A on A.TratamientoPreheatJuntaWorkstatusID = tmp.TratamientoPreheatJuntaWorkstatusID
			AND A.TratamientoPreheatFechaTratamiento = tmp.TratamientoPreheatFechaTratamiento
			AND A.FechaModificacion = tmp.FechaModificacion	
		where TratamientoPreheatJuntaCampo = 1				
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
					MAX(InspeccionDimensionalFechaLiberacion) AS InspeccionDimensionalFechaLiberacion
			FROM #TempInspeccionDimensional
			GROUP BY InspeccionDimensionalWorkstatusSpoolID
		) A ON A.InspeccionDimensionalWorkstatusSpoolID = tmp.InspeccionDimensionalWorkstatusSpoolID
			AND A.InspeccionDimensionalFechaLiberacion = tmp.InspeccionDimensionalFechaLiberacion			
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
	LEFT JOIN #TempPruebaHidrostatica tmph on tmph.PruebaHidrostaticaWorkstatusSpoolID = ws.WorkstatusSpoolID
	LEFT JOIN #TempOrdenTrabajoMaterial tom on tom.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
	LEFT JOIN #TempAgrupadoresPorJunta agxj on agxj.JuntaSpoolID = g.GeneralJuntaSpoolID
	where GeneralJuntaSpoolID = @JuntaSpoolID


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
	DROP TABLE #TempPruebaPMI 
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
	DROP TABLE #TempJuntaSpoolB 
	DROP TABLE #TempSpool 
	DROP TABLE #TempOrdenTrabajoSpool
	DROP TABLE #TempPruebaPT
	DROP TABLE #TempMaterialSpool
	DROP TABLE #TempJuntaInspeccionVisualDefecto


	SET NOCOUNT OFF;

END


GO
