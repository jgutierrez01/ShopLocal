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
	Modificado:	12/03/2011
*****************************************************************************************/

CREATE PROCEDURE [dbo].[ListadoCertificacion] 
(@proyectoID INT)
AS 
BEGIN

	CREATE TABLE #Spools(
		SpoolID INT,
		OrdenTrabajoSpoolID INT,
		Nombre NVARCHAR(50),		
	    NumeroControl NVARCHAR(50),
	    Segmento1 NVARCHAR (20),
	    Segmento2 NVARCHAR (20),
	    Segmento3 NVARCHAR (20),
	    Segmento4 NVARCHAR (20),
	    Segmento5 NVARCHAR (20),
	    Segmento6 NVARCHAR (20),
	    Segmento7 NVARCHAR (20)
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
		ReporteSandblast NVARCHAR(50),
		Embarcado BIT,
		NumeroEmbarque NVARCHAR(50)
	)
	
	
	--traigo los spools del proyecto
	INSERT INTO #Spools
	SELECT	s.SpoolID, 
			OrdenTrabajoSpoolID, 
			Nombre, 
			NumeroControl,
			Segmento1,
			Segmento2,
			Segmento3,
			Segmento4,
			Segmento5,
			Segmento6,
			Segmento7
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
			ISNULL(ReporteSandblast,'') AS ReporteSandblast,
			Embarcado,
			NumeroEmbarque
	FROM #Spools s 
	LEFT JOIN WorkstatusSpool ws
		ON s.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
	LEFT JOIN ReporteDimensionalDetalle rdd
		ON rdd.WorkstatusSpoolID = ws.WorkstatusSpoolID
	LEFT JOIN ReporteDimensional rd	
		ON rd.ReporteDimensionalID = rdd.ReporteDimensionalID	
	LEFT JOIN PinturaSpool ps	
		ON ps.WorkstatusSpoolID = ws.WorkstatusSpoolID
	LEFT JOIN EmbarqueSpool embs 
		ON embs.WorkstatusSpoolID = ws.WorkstatusSpoolID	
	LEFT JOIN Embarque emb
		ON emb.EmbarqueID = embs.EmbarqueID
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

	SELECT Nombre,ProyectoID FROM Proyecto WHERE ProyectoID = @proyectoID
	SELECT * FROM #Spools
	SELECT * FROM #WorkstatusSpool
	SELECT * FROM #JuntaReportes
	SELECT * FROM #JuntaWorkStatus
	SELECT Nombre 
		FROM Wps w 
			INNER JOIN WpsProyecto wpp 
				ON wpp.WpsID = w.WpsID
				WHERE wpp.ProyectoID = @proyectoID	

	DROP TABLE #Spools
	DROP TABLE #JuntaSpools
	DROP TABLE #WorkstatusSpool
	DROP TABLE #JuntaReportes
	DROP TABLE #JuntaWorkStatus
	

END

GO