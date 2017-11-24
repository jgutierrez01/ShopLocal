
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSeguimientoDeSpools]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[ObtenerSeguimientoDeSpools]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


/****************************************************************************************
	Nombre:		ObtenerSeguimientoDeSpools
	Funcion:	Trae toda la informacion necesaria para el seguimiento de spools
	Parametros:	@ProyectoID INT
				@HistorialRep BIT
	Autor:		MMG
	Modificado:	JHT 23/01/2014 -- Agregar campo DiametroMayor
				JHT 10/02/2014 -- Agregar Ultima Localización a tabla general
				JHT 11/03/2014 -- Liberacion Calidad
				GTG 18/03/2014 -- Se agrego campo FechaEmision de OT
				GTG 10/04/2014 -- Vigencia Aduana
                                Bernardo Garza, 13/05/2014 -- cambios en cálculo de % armado y soldadura
				JHT 06/06/2014 -- Nuevo Campo Fecha Liberacion Material /// Nuevo campo liberado pintura
				JHT 13/06/2014 -- Nuevo campo fecha Ok Pnd
				01/07/2014 GTG -- Modificacion de insert into #TempInspeccionDimensional
				07/07/2014 GTG -- Nuevos campos RequierePruebaHidrostactica 
				JHT 14/05/2014 -- Nuevo campo Presion para Pruebas hidrostaticas
				GTG 23/07/2014 -- Nuevos campos en modulo Agrupadores
				GTG 28/07/2014 -- nuevo campo modulo general FechaLocalizacion
				GTG 15/08/2014 -- cambio consulta pintura spool
				JME 13/10/2014 -- cambio formato fechas
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

	declare @TH int
	select @TH = TipoJuntaID 
	from TipoJunta where Codigo = 'TH'

	declare @TW int
	select @TW = TipoJuntaID 
	from TipoJunta where Codigo = 'TW'


	declare @ShopFabAreaID int

	select @ShopFabAreaID = FabAreaID
	from FabArea
	where Codigo = 'SHOP'

	declare @FIELD int
	select @FIELD = FabAreaID 
	from FabArea where Codigo = 'FIELD'

	--Se crean las tablas temporales

	--TABLA SPOOL TRAMO RECTO
	CREATE TABLE #SpoolTramoRecto
	(
		SpoolID INT,
		TieneDespacho BIT,
		TieneOT BIT
	)

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
		NumeroOrden VARCHAR(50),		
		FechaOrden DATETIME
	)

	--TABLA ORDEN TRABAJO SPOOL
	CREATE TABLE #TempOrdenTrabajoSpool
	(	
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoID INT,
		NumeroControl VARCHAR(50),
		SpoolID INT,
		FechaLiberacionCalidad DATETIME,
		UsuarioliberacionCalidad VARCHAR(100),
		FechaLiberacionMaterial DATETIME,
		UsuarioLiberacionMaterial NVARCHAR(100),
		FechaOkPnd DATETIME NULL,
		UsuarioOkPnd VARCHAR(100) NULL
	)

	--TABLA ORDEN TRABAJO MATERIAL
	CREATE TABLE #TempOrdenTrabajoMaterial
	(
		OrdenTrabajoMaterialID INT,
		OrdenTrabajoSpoolID INT,
		TieneInventarioCongelado BIT,
		TieneCorte BIT,
		TieneDespacho BIT,
		EsAsignado BIT
	)

	--TABLA SPOOL
	CREATE TABLE #TempSpool
	(	
		SpoolID INT,
		Nombre VARCHAR(50),
		Pdi DECIMAL(10,4),
		Peso DECIMAL(7,2),
		Area DECIMAL(8,4),
		Especificacion VARCHAR(15),
		Prioridad INT,
	    Segmento1 VARCHAR(20),
	    Segmento2 VARCHAR(20),
	    Segmento3 VARCHAR(20),
	    Segmento4 VARCHAR(20),
	    Segmento5 VARCHAR(20),
	    Segmento6 VARCHAR(20),
	    Segmento7 VARCHAR(20),
	    PorcentajePnd INT,
	    Isometrico nvarchar(50),
	    RevisionCte nvarchar(10),
	    RevisionStgo nvarchar(10),
	    FamiliaAcero1ID int,
	    FamiliaAcero2ID int,
	    SistemaPintura nvarchar(100),
	    ColorPintura nvarchar(100),
	    CodigoPintura nvarchar(100),
	    DiametroPlano DECIMAL(10,4),
		DiametroMayor DECIMAL(10,4),
	    FechaEtiqueta DATETIME,
	    NumeroEtiqueta NVARCHAR(20),
	    RequierePWHT BIT,		
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
		FechaLocalizacion DATETIME NULL
	)

	--TABLA CORTESPOOL
	CREATE TABLE #TempCorteSpool
	(
		CorteSpoolID INT,
		SpoolID INT,
		EtiquetaSeccion NVARCHAR(5),
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
		FechaPreparacion DATETIME,
		TieneLiberacionDimensional BIT,
		LiberadoPintura BIT NULL
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
		PinturaReportePullOff VARCHAR(50),
		PinturaLiberado BIT NULL
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

	--TABLA EMBARQUE
	CREATE TABLE #TempEmbarque
	(
		EmbarqueEmbarqueSpoolID INT,
		EmbarqueWorkstatusSpoolID INT,
		EmbarqueEtiqueta VARCHAR(20),
		EmbarqueFechaEtiqueta DATETIME,
		EmbarqueFechaPreparacion DATETIME,
		EmbarqueFechaEmbarque DATETIME,
		EmbarqueFechaEstimada DATETIME,
		EmbarqueNumeroEmbarque VARCHAR(50),
		Nota1 NVARCHAR(50),
		Nota2 NVARCHAR(50),
		Nota3 NVARCHAR(50),
		Nota4 NVARCHAR(50),
		Nota5 NVARCHAR(50),
		EmbarqueVigenciaAduana DATETIME
	)

	CREATE TABLE #TempCantidadJuntas
	(
		SpoolID INT,
		FamiliaAcero1ID int,
		FamiliaAcero2ID int,
		TotalJuntas int,
		TotalJuntasShop int,
		TotalKgTeoricos decimal(16,4),
		TotalPeqs decimal (16,4),
		RowNum int
	)
	--TABLA GENERAL
	CREATE TABLE #TempGeneral
	(
		GeneralSpoolID INT,
		GeneralJuntaWorkstatusID INT,
		GeneralProyecto VARCHAR(50),
		GeneralOrdenDeTrabajo VARCHAR(50),
		GeneralFechaEmision DATETIME, --Fecha emision orden de trabajo
		GeneralNumeroDeControl VARCHAR(50),
		GeneralSpool VARCHAR(50),
		GeneralNumeroJuntas INT,
		GeneralPeqs DECIMAL(16,4),
		GeneralKgsTeoricos DECIMAL(16,4),
		GeneralPrioridad INT,
		GeneralPdi DECIMAL(10,4),
		GeneralPeso DECIMAL(7,2),
		GeneralArea DECIMAL(7,2),
		GeneralEspecificacion VARCHAR(15),
		GeneralTieneHold BIT,
		GeneralPWHT BIT,
		GeneralRequierePruebaHidrostatica BIT NULL,
		GeneralUltimaLocalizacion NVARCHAR(100),
		Segmento1 VARCHAR(20),
		Segmento2 VARCHAR(20),
		Segmento3 VARCHAR(20),
		Segmento4 VARCHAR(20),
		Segmento5 VARCHAR(20),
		Segmento6 VARCHAR(20),
		Segmento7 VARCHAR(20),
		Campo1 NVARCHAR(100),
		Campo2 NVARCHAR(100),
		Campo3 NVARCHAR(100),
		Campo4 NVARCHAR(100),
		Campo5 NVARCHAR(100),
		Isometrico nvarchar(50),
		RevisionCte nvarchar(10),
	    RevisionStgo nvarchar(10),
		OrdenTrabajoSpoolID INT,
		OrdenTrabajoID INT,
		GeneralFamiliaAcero1 VARCHAR(50),
		GeneralFamiliaMaterial1 VARCHAR(50),
		GeneralFamiliaAcero2 VARCHAR(50),
		GeneralFamiliaMaterial2 VARCHAR(50),
		GeneralPorcentajePnd INT,
		PinturaSistema VARCHAR(100),
		PinturaColor VARCHAR(100),
		PinturaCodigo VARCHAR(100),
		ObservacionesHold NVARCHAR(MAX),
		FechaHold DATETIME ,
		GeneralDiametroPlano DECIMAL(10,4),
		GeneralDiametroMayor DECIMAL(10,4),
		GeneralFechaEtiqueta DATETIME,
		GeneralNumeroEtiqueta NVARCHAR(20),
		GeneralMaterialPendiente bit,
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

	--Comienzan los inserts a las tablas temporales con filtros si los tienen.

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
		   where js.SpoolID = s.SpoolID and js.FabAreaID = @ShopFabAreaID
		  )

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
				   ot.NumeroOrden,
				   ot.FechaOrden
			FROM OrdenTrabajo ot
			WHERE ProyectoID = @ProyectoID

	END
	ELSE 
	BEGIN

		INSERT INTO #TempOrdenTrabajo
			SELECT OrdenTrabajoID,
				   ot.NumeroOrden,
				   ot.FechaOrden
			FROM OrdenTrabajo ot
			WHERE ProyectoID = @ProyectoID AND
				  OrdenTrabajoID = @OrdenTrabajoID

	END

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
		INNER JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
		LEFT JOIN WorkstatusSpool ws on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		

	--INSERT ORDEN TRABAJO MATERIAL
	INSERT INTO #TempOrdenTrabajoMaterial
		SELECT otm.OrdenTrabajoMaterialID,
			   otm.OrdenTrabajoSpoolID,
			   otm.TieneInventarioCongelado,
			   otm.TieneCorte,
			   otm.TieneDespacho,
			   otm.EsAsignado
	FROM OrdenTrabajoMaterial otm
	INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID

	--INSERT SPOOL
	IF(@SpoolID IS NULL)
	BEGIN
		INSERT INTO #TempSpool
			SELECT s.SpoolID,
				   s.Nombre,
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
				   Segmento7,
				   PorcentajePnd,
				   Dibujo,
				   RevisionCliente,
				   Revision,
				   FamiliaAcero1ID,
				   FamiliaAcero2ID,
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
				   RequierePruebaHidrostatica,
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
			ON s.CuadranteID = c.CuadranteID
			WHERE s.ProyectoID = @ProyectoID

	END
	ELSE
	BEGIN

		INSERT INTO #TempSpool
			SELECT s.SpoolID,
				   s.Nombre,
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
				   Segmento7,
				   PorcentajePnd,
				   Dibujo,
				   RevisionCliente,
				   Revision,
				   FamiliaAcero1ID,
				   FamiliaAcero2ID,
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
				    RequierePruebaHidrostatica,
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
			ON s.CuadranteID = c.CuadranteID
			WHERE s.ProyectoID = @ProyectoID

	END

	--INSERT CORTESPOOL
	IF(@SpoolID IS NULL)
	BEGIN
		INSERT INTO #TempCorteSpool
			SELECT CorteSpoolID,
				   SpoolID,
				   EtiquetaSeccion
			FROM CorteSpool
	END
	ELSE
	BEGIN

		INSERT INTO #TempCorteSpool
			SELECT CorteSpoolID,
				   SpoolID,
				   EtiquetaSeccion
			FROM CorteSpool
			WHERE SpoolID = @SpoolID

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
			   nr.numerorechazos
		FROM ReporteDimensionalDetalle rdd
		INNER JOIN #TempReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID	
		LEFT JOIN(
			select WorkstatusSpoolID,
				   COUNT(Aprobado) numerorechazos
		    from ReporteDimensionalDetalle
		    where Aprobado = 0
		    group by WorkstatusSpoolID
		) nr on nr.WorkstatusSpoolID = rdd.WorkstatusSpoolID
		where rd.TipoReporteDimensionalID = 2
		order by rdd.FechaLiberacion desc

	--INSERT WORKSTATUS SPOOL
	INSERT INTO #TempWorkstatusSpool
		SELECT DISTINCT ws.WorkstatusSpoolID,
			   ws.OrdenTrabajoSpoolID,			   
			   s.SistemaPintura,
			   s.ColorPintura,
			   s.CodigoPintura,
			   s.NumeroEtiqueta,
			   s.FechaEtiqueta,
			   ws.FechaPreparacion,
			   ws.TieneLiberacionDimensional,
			   (CASE WHEN ws.LiberadoPintura IS NOT NULL THEN ws.LiberadoPintura ELSE 0 END)	  		   
		FROM WorkstatusSpool ws
		INNER JOIN #TempOrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID	
		INNER JOIN #TempSpool s ON s.SpoolID = ots.SpoolID

	
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
			   (CASE WHEN ws.LiberadoPintura IS NOT NULL THEN ws.LiberadoPintura ELSE 0 END)
		FROM #TempOrdenTrabajoSpool ots
		INNER JOIN Spool s
		on s.SpoolID = ots.SpoolID
		LEFT join #TempWorkstatusSpool ws	on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		LEFT join PinturaSpool ps on ps.WorkstatusSpoolID = ws.WorkstatusSpoolID	
		LEFT JOIN RequisicionPinturaDetalle rpd on rpd.WorkstatusSpoolID = ws.WorkstatusSpoolID
		LEFT JOIN RequisicionPintura rp on rp.RequisicionPinturaID = rpd.RequisicionPinturaID				
		where s.ProyectoID = @ProyectoID

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
		SELECT S.SpoolID,
			   wss.WorkStatusSpoolID,
			   p.Nombre,
			   ot.NumeroOrden,
			   ot.FechaOrden,
			   ots.NumeroControl,
			   s.Nombre,
			   tJuntas.TotalJuntas,
			   tJuntas.TotalPeqs,
			   tJuntas.TotalKgTeoricos,
			   s.Prioridad,
			   s.pdi,
			   s.Peso,
			   s.Area,
			   s.Especificacion,
			   ISNULL((select '1'
				where sh.TieneHoldCalidad = 1 or
					  sh.TieneHoldIngenieria = 1 or
					  sh.Confinado = 1),0),
				s.RequierePWHT,
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
				s.UltimaLocalizacion,
				s.Segmento1,
				s.Segmento2,
				s.Segmento3,
				s.Segmento4,
				s.Segmento5,
				s.Segmento6,
				s.Segmento7,
				s.Campo1,
				s.Campo2,
				s.Campo3,
				s.Campo4,
				s.Campo5,
				s.Isometrico,
				s.RevisionCte,
				s.RevisionStgo,
				ots.OrdenTrabajoSpoolID,
				ot.OrdenTrabajoID,
				fa1.Nombre,
				fm1.Nombre,
				fa2.Nombre,
				fm2.Nombre,
				s.PorcentajePnd,
				s.SistemaPintura,
				s.ColorPintura,
				s.CodigoPintura,
				SpoolHoldHistorial.Observaciones,
				SpoolHoldHistorial.FechaHold,
				s.DiametroPlano,
				s.DiametroMayor,
				s.FechaEtiqueta,
				s.NumeroEtiqueta,
				case when totm.MaterialPendiente > 0 then '1'
				else '0' end,
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
			FROM #Tempspool s
			left join
			(
				select	js.SpoolID,
						COUNT(js.JuntaSpoolID) [TotalJuntas],
						SUM(isnull(js.KgTeoricos,0)) [TotalKgTeoricos],
						SUM(isnull(js.Peqs,0)) [TotalPeqs]
						from JuntaSpool js
						where FabAreaID = @ShopFabAreaID
						group by js.SpoolID
			) tJuntas on tJuntas.SpoolID = s.SpoolID
			INNER JOIN #TempProyecto p on p.ProyectoID = @ProyectoID
			INNER JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = s.FamiliaAcero1ID
			inner join FamiliaMaterial fm1 on fa1.FamiliaMaterialID = fm1.FamiliaMaterialID
			left join FamiliaAcero fa2 on fa2.FamiliaAceroID = s.FamiliaAcero2ID
			left join FamiliaMaterial fm2 on fa2.FamiliaMaterialID = fm2.FamiliaMaterialID
			LEFT JOIN #TempOrdenTrabajoSpool ots on ots.SpoolID = s.SpoolID
			LEFT JOIN 
				(
					select	OrdenTrabajoSpoolID,
							count(ordentrabajoSpoolid) as [MaterialPendiente]
					from #TempOrdenTrabajoMaterial otm
					where otm.TieneInventarioCongelado = 0 and otm.TieneCorte = 0 and otm.TieneDespacho = 0 and otm.EsAsignado = 0
					group by otm.OrdenTrabajoSpoolID
				) totm on ots.OrdenTrabajoSpoolID = totm.OrdenTrabajoSpoolID
			LEFT JOIN #TempWorkstatusSpool wss on wss.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
			LEFT JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
			LEFT JOIN SpoolHold sh on sh.SpoolID = s.SpoolID
			LEFT JOIN 
			(
				SELECT ts.SpoolID,MAX(SpoolHoldHistorial.SpoolHoldHistorialID) AS SpoolHoldHistorialID
				FROM #Tempspool ts INNER JOIN SpoolHoldHistorial ON ts.SpoolID = SpoolHoldHistorial.SpoolID
				GROUP BY ts.SpoolID
			) tHoldHistorial ON tHoldHistorial.SpoolID = s.SpoolID
			LEFT JOIN SpoolHoldHistorial ON SpoolHoldHistorial.SpoolHoldHistorialID = tHoldHistorial.SpoolHoldHistorialID
	--DESPLEGAR TABLA

		select g.GeneralSpoolID,
		   g.GeneralProyecto,
		   g.GeneralOrdenDeTrabajo,
		   g.GeneralFechaEmision,
		   g.GeneralNumeroDeControl,
		   g.GeneralSpool,
		   g.GeneralNumeroJuntas,
		   g.GeneralFamiliaAcero1,
		   g.GeneralFamiliaMaterial1,
		   g.GeneralFamiliaAcero2,
		   g.GeneralFamiliaMaterial2,
		   g.GeneralPrioridad,
		   g.GeneralPdi,
		   g.GeneralPeso,
		   g.GeneralArea,
		   g.GeneralEspecificacion,
		   g.GeneralPeqs,
		   g.GeneralKgsTeoricos,
		   g.GeneralTieneHold,
		   g.GeneralPWHT,
		   g.GeneralRequierePruebaHidrostatica,
		   g.GeneralUltimaLocalizacion,
		   g.Segmento1,
		   g.Segmento2,
		   g.Segmento3,
		   g.Segmento4,
		   g.Segmento5,
		   g.Segmento6,
		   g.Segmento7,
		   g.Campo1,
		   g.Campo2,
		   g.Campo3,
		   g.Campo4,
		   g.Campo5,
		   g.Isometrico,
		   g.RevisionCte,
		   g.RevisionStgo,
		   g.GeneralMaterialPendiente,
		   tcs.GeneralEtiquetaSegmentos,
		   id.InspeccionDimensionalFecha,
		   id.InspeccionDimensionalFechaReporte,
		   id.InspeccionDimensionalNumeroReporte,
		   id.InspeccionDimensionalHoja,
		   id.InspeccionDimensionalResultado,
		   id.InspeccionDimensionalObservaciones,
		   ISNULL(id.InspeccionDimensionalNumeroRechazos,0) [InspeccionDimensionalNumeroRechazos],
		   ie.InspeccionEspesoresFecha,
		   ie.InspeccionEspesoresFechaReporte,
		   ie.InspeccionEspesoresNumeroReporte,
		   ie.InspeccionEspesoresHoja,
		   ie.InspeccionEspesoresResultado,
		   ie.InspeccionEspesoresObservaciones,
		   ISNULL(ie.InspeccionEspesoresNumeroRechazos,0) [InspeccionEspesoresNumeroRechazos],
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
		   g.GeneralNumeroEtiqueta as EmbarqueEtiqueta,
		   g.GeneralFechaEtiqueta as EmbarqueFechaEtiqueta,
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
		   g.GeneralPorcentajePnd,
		   case when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = g.GeneralSpoolID and tr.TieneOT = 0) then 0 when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = g.GeneralSpoolID and tr.TieneDespacho = 1) then 100 when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = g.GeneralSpoolID and tr.TieneDespacho = 0) then 0 else isnull(porcentajes.PorcArmado, 0) end [PorcentajeArmado],
		   case when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = g.GeneralSpoolID and tr.TieneOT = 0) then 0 when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = g.GeneralSpoolID and tr.TieneDespacho = 1) then 100 when EXISTS(select * from #SpoolTramoRecto tr where tr.SpoolID = g.GeneralSpoolID and tr.TieneDespacho = 0) then 0 else isnull(porcentajes.PorcSoldado, 0) end [PorcentajeSoldado],
		   g.ObservacionesHold,
		   g.FechaHold AS FechaHold,
		   g.GeneralDiametroPlano AS DiametroPlano,
		   g.GeneralDiametroMayor,
		   g.GeneralFechaLiberacionCalidad,
		   g.GeneralUsuarioLiberacionCalidad,
		   g.GeneralFechaLiberacionMaterial,
		   g.GeneralUsuarioLiberacionMaterial,
		   g.GeneralFechaOkPnd,
		   g.GeneralUsuarioOkPnd,	
		   tmph.PruebaHidrostaticaFechaRequisicion as PruebaHidroFechaRequisicion,
		   tmph.PruebaHidrostaticaNumeroRequisicion as PruebaHidroNumeroRequisicion,
		   tmph.PruebaHidrostaticaFechaPrueba as PruebaHidroFechaPrueba,
		   tmph.PruebaHidrostaticaFechaReporte as PruebaHidroFechaReporte,
		   tmph.PruebaHidrostaticaNumeroReporte as PruebaHidroNumeroReporte,
	       tmph.PruebaHidrostaticaHoja as PruebaHidroHoja,
		   tmph.PruebaHidrostaticaAprobado as PruebaHidroAprobado,
		   tmph.PruebaHidroPresion,
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
			tts.NumeroTransferencia,
			tts.PreparacionTransferencia,
			tts.Transferencia		
	from #TempGeneral g
	left join
	(
		select distinct substring((select ', '+ EtiquetaSeccion  AS [text()]

								   from #TempCorteSpool
					               where SpoolID = tg.GeneralSpoolID
                                   for XML PATH ('')),2, 1000) [GeneralEtiquetaSegmentos], tg.GeneralSpoolID
		from #TempGeneral tg
	) tcs on g.GeneralSpoolID = tcs.GeneralSpoolID
	LEFT JOIN
	(
		SELECT	grp1.SpoolID,
				(cast(grp1.Armadas as decimal(5,2)) / cast(grp1.TotalJuntas as decimal(5,2))) * 100.0 [PorcArmado],
				(cast(grp1.Soldadas as decimal(5,2)) / cast(case when grp1.TotalJuntasParaSoldar = 0 then 1 else grp1.TotalJuntasParaSoldar end as decimal(5,2))) * 100.0 [PorcSoldado]
		FROM
		(
			SELECT	tjs.SpoolID,
					COUNT(tjs.JuntaSpoolID) [TotalJuntas],
					COUNT(tjs.JuntaSpoolID) [TotalJuntasParaSoldar],
					SUM(
						CASE	WHEN (tjs.TipoJuntaID = @TW and tjws.JuntaArmadoID is not null and tjws.ArmadoAprobado = 1) or (tjws.JuntaSoldaduraID is not null and tjws.SoldaduraAprobada = 1) THEN 1
								ELSE 0 END
					) [Soldadas],
					SUM(
						CASE	WHEN tjws.JuntaArmadoID is not null and tjws.ArmadoAprobado = 1 THEN 1
								ELSE 0 END
					) [Armadas]
			FROM JuntaSpool tjs
			LEFT JOIN JuntaWorkstatus tjws on tjs.JuntaSpoolID = tjws.JuntaSpoolID and tjws.VersionJunta = 1
			WHERE	tjs.FabAreaID != @FIELD and -- Excluimos juntas field para armado y soldadura
					tjs.TipoJuntaID != @TH      -- Excluuimos juntas TH para armado y soldadura
					AND tjs.SpoolID IN
					(
						SELECT SpoolID FROM #TempSpool
					)
			GROUP BY tjs.SpoolID
		) grp1
	) porcentajes on porcentajes.SpoolID = g.GeneralSpoolID
	LEFT JOIN #TempOrdenTrabajoSpool ots on ots.SpoolID = g.GeneralSpoolID
	LEFT JOIN #TempTransferenciaSpool tts on tts.SpoolID=ots.SpoolID
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
	LEFT JOIN #TempPruebaHidrostatica tmph on tmph.PruebaHidrostaticaWorkstatusSpoolID = ws.WorkstatusSpoolID
	WHERE (@OrdenTrabajoSpoolID IS NULL OR  G.OrdenTrabajoSpoolID = @OrdenTrabajoSpoolID)
	and (@OrdenTrabajoID IS null or g.OrdenTrabajoID = @OrdenTrabajoID)

	SET NOCOUNT OFF;

END


GO
