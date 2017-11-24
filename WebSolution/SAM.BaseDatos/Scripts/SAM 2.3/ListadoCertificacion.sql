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
				14/11/2011
*****************************************************************************************/

CREATE PROCEDURE [dbo].[ListadoCertificacion] 
(
	 @proyectoID INT
	,@ordenTrabajo INT
	,@numeroControl INT
	,@embarque NVARCHAR(20)
	,@segmento1 NVARCHAR(20)
	,@segmento2 NVARCHAR(20)
	,@segmento3 NVARCHAR(20)
	,@segmento4 NVARCHAR(20)
	,@segmento5 NVARCHAR(20)
	,@segmento6 NVARCHAR(20)
	,@segmento7 NVARCHAR(20)
)
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
		ReporteSandblast NVARCHAR(50),
		Embarcado BIT,
		NumeroEmbarque NVARCHAR(50)
	)
	
	CREATE TABLE #WPSSpool(
		SpoolID INT,
		WpsID INT,
		Nombre NVARCHAR(50)
	)
	
CREATE TABLE #MTR(
		OrdenTrabajoMaterialID INT,
		SpoolID INT,
		NumCertificado NVARCHAR(50)
	)
	
	--traigo los spools del proyecto
	INSERT INTO #Spools
	SELECT	s.SpoolID, 
			ots.OrdenTrabajoSpoolID, 
			Nombre, 
			NumeroControl
	FROM Spool s
	INNER JOIN OrdenTrabajoSpool ots
		ON ots.SpoolID = s.SpoolID
	LEFT JOIN WorkstatusSpool wss
		on	ots.OrdenTrabajoID = wss.OrdenTrabajoSpoolID
	LEFT JOIN EmbarqueSpool es
		on es.WorkstatusSpoolID = wss.WorkstatusSpoolID
	LEFT JOIN Embarque em
		on es.EmbarqueID = em.EmbarqueID
	WHERE s.ProyectoID = @proyectoID
	AND (@ordenTrabajo is null OR @ordenTrabajo = -1 OR OrdenTrabajoID = @ordenTrabajo)
	AND (@numeroControl is null OR @numeroControl = -1 OR ots.OrdenTrabajoSpoolID = @numeroControl)	
	AND (@embarque is null OR @embarque = '' OR em.NumeroEmbarque like '%'+@embarque+'%')
	AND (@segmento1 is null OR @segmento1 = '' OR s.Segmento1 like '%'+@segmento1+'%')
	AND (@segmento2 is null OR @segmento2 = '' OR s.Segmento2 like '%'+@segmento2+'%')
	AND (@segmento3 is null OR @segmento3 = '' OR s.Segmento3 like '%'+@segmento3+'%')
	AND (@segmento4 is null OR @segmento4 = '' OR s.Segmento4 like '%'+@segmento4+'%')
	AND (@segmento5 is null OR @segmento5 = '' OR s.Segmento5 like '%'+@segmento5+'%')
	AND (@segmento6 is null OR @segmento6 = '' OR s.Segmento6 like '%'+@segmento6+'%')
	AND (@segmento7 is null OR @segmento7 = '' OR s.Segmento7 like '%'+@segmento7+'%')
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
	
	--WPS
	INSERT INTO #WPSSpool
	SELECT DISTINCT  j.SpoolID
					,js.WpsID
					,w.Nombre
	FROM	#JuntaWorkstatus j 
	INNER JOIN JuntaSoldadura js on js.JuntaWorkstatusID = j.JuntaWorkStatusID
	INNER JOIN Wps w on w.WpsID = js.WpsID
	
	--MTR
	INSERT INTO #MTR
select otm.OrdenTrabajoMaterialID
,query.SpoolID
,c.NumeroCertificado
from OrdenTrabajoMaterial otm
join (SELECT s.SpoolID, ots.OrdenTrabajoSpoolID
	FROM #Spools s
	INNER JOIN OrdenTrabajoSpool ots on s.SpoolID = ots.SpoolID) query on otm.OrdenTrabajoSpoolID = query.OrdenTrabajoSpoolID
	INNER JOIN NumeroUnico nu on otm.NumeroUnicoDespachadoID = nu.NumeroUnicoID
	INNER JOIN Colada c on nu.ColadaID = c.ColadaID 

	SELECT Nombre,ProyectoID FROM Proyecto WHERE ProyectoID = @proyectoID
	SELECT * FROM #Spools
	SELECT * FROM #WorkstatusSpool
	SELECT * FROM #JuntaReportes
	SELECT * FROM #JuntaWorkStatus
	SELECT * FROM #WPSSpool
	SELECT * FROM #MTR

	DROP TABLE #Spools
	DROP TABLE #JuntaSpools
	DROP TABLE #WorkstatusSpool
	DROP TABLE #JuntaReportes
	DROP TABLE #JuntaWorkStatus
	DROP TABLE #WPSSpool
	DROP TABLE #MTR

END

GO

/*
exec ListadoCertificacion 46,'','','','','','',''
*/


