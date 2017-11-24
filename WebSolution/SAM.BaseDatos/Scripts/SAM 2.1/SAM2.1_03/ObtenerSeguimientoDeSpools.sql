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
	Modificado:	11/03/2011 MMG
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
	    Segmento7 VARCHAR(20),
	    PorcentajePnd INT
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
		KgTeoricos DECIMAL(12,4),
		FamiliaAcero1 VARCHAR(50),
		FamiliaMaterial1 VARCHAR(50),
		FamiliaAcero2 VARCHAR(50),
		FamiliaMaterial2 VARCHAR(50)
		
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
	
	CREATE TABLE #TempCanridadJuntas
	(
		SpoolID INT,
		FamiliaAcero1 VARCHAR(50),
		FamiliaMaterial1 VARCHAR(50),
		FamiliaAcero2 VARCHAR(50),
		FamiliaMaterial2 VARCHAR(50),
		NumeroJuntas INT
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
		GeneralPeqs DECIMAL(10,4),
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
		OrdenTrabajoSpoolID INT,
		GeneralFamiliaAcero1 VARCHAR(50),
		GeneralFamiliaMaterial1 VARCHAR(50),
		GeneralFamiliaAcero2 VARCHAR(50),
		GeneralFamiliaMaterial2 VARCHAR(50),
		GeneralPorcentajePnd INT
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
				   Segmento7,
				   PorcentajePnd
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
				   Segmento7,
				   PorcentajePnd
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
				  js.KgTeoricos,
				  fa1.Nombre [FamiliaAcero1],
				  fm1.Nombre [FamiliaMaterial1],
				  fa2.Nombre [FamiliaAcero2],
				  fm2.Nombre [FamiliaMaterial2]
			FROM JuntaSpool js
			INNER JOIN #TempSpool sp on sp.SpoolID = js.SpoolID
			LEFT JOIN FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
			LEFT JOIN FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
			LEFT JOIN FamiliaMaterial fm1 on fm1.FamiliaMaterialID = fa1.FamiliaMaterialID
			LEFT JOIN FamiliaMaterial fm2 on fm2.FamiliaMaterialID = fa2.FamiliaMaterialID
			
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
				
	--INSERT CANTIDAD JUNTAS
	INSERT INTO #TempCanridadJuntas
	SELECT  js.SpoolID,
			js.FamiliaAcero1,
			FamiliaMaterial1,
			max(FamiliaAcero2),
			max(FamiliaMaterial2),
			js.cantidad
			from(
					SELECT tjs.SpoolID,
						   FamiliaAcero1,
						   FamiliaMaterial1,
						   FamiliaAcero2,
						   FamiliaMaterial2,
						   COUNT(SpoolID) [cantidad]
					from #tempJuntaSpool tjs
					group by FamiliaAcero1,FamiliaMaterial1,SpoolID,FamiliaAcero2,FamiliaMaterial2
			 ) js
			INNER JOIN(
				SELECT SpoolID, MAX(NumeroJuntas) AS cantidad
				from(
						SELECT tjs.SpoolID,
							   FamiliaAcero1,
							   FamiliaMaterial1,
							   FamiliaAcero2,
							   FamiliaMaterial2,
							   COUNT(SpoolID) [NumeroJuntas]
						from #tempJuntaSpool tjs
						group by FamiliaAcero1,FamiliaMaterial1,SpoolID,FamiliaAcero2,FamiliaMaterial2
					) js
					GROUP BY SpoolID
			) A ON A.SpoolID = js.SpoolID
				AND A.cantidad = js.Cantidad
				GROUP BY JS.SpoolID,JS.cantidad,js.FamiliaAcero1,FamiliaMaterial1
		
				
	--INSERT GENERAL	
	INSERT INTO #TempGeneral
		SELECT S.SpoolID,
			   wss.WorkStatusSpoolID,
			   p.Nombre,
			   ot.NumeroOrden,
			   ots.NumeroControl,
			   s.Nombre,
			   nj.NumeroJuntas,
			   nj.peqs,
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
				ots.OrdenTrabajoSpoolID,
				Fam.FamiliaAcero1,
				Fam.FamiliaMaterial1,
				Fam.FamiliaAcero2,
				Fam.FamiliaMaterial2,
				s.PorcentajePnd
			FROM #Tempspool s
			LEFT JOIN #TempOrdenTrabajoSpool ots on ots.SpoolID = s.SpoolID
			LEFT JOIN #TempWorkstatusSpool wss on wss.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
			INNER JOIN #TempProyecto p on p.ProyectoID = @ProyectoID
			LEFT JOIN #TempOrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
			LEFT JOIN SpoolHold sh on sh.SpoolID = s.SpoolID
			LEFT JOIN(
				SELECT SpoolID,
					   peqs,
					   COUNT(SpoolID) [NumeroJuntas]
				from #tempJuntaSpool
				group by SpoolID,peqs
			) nj on nj.SpoolID = s.SpoolID
			LEFT JOIN #TempCanridadJuntas Fam on Fam.SpoolID = s.SpoolID
			
	--DESPLEGAR TABLA

		select g.GeneralSpoolID,
		   g.GeneralProyecto,
		   g.GeneralOrdenDeTrabajo,
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
		   e.EmbarqueNumeroEmbarque,
		   g.GeneralPorcentajePnd
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


