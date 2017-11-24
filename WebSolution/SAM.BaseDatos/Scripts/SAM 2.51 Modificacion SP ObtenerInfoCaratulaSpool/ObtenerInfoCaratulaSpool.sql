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
	Modificado:	05/04/2011
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerInfoCaratulaSpool]
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
		Nombre NVARCHAR(50)
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

	CREATE TABLE #JuntaSpool(
		JuntaSpoolID INT, 
		SpoolID INT,		
		EtiquetaJunta NVARCHAR(50),
		TipoJuntaID INT,
		Diametro DECIMAL(7, 4),
		Cedula NVARCHAR(10)
	)

	INSERT INTO #WorkstatusSpool
	SELECT OrdenTrabajoSpoolID, ws.WorkstatusSpoolID
	FROM WorkstatusSpool  ws	
	WHERE OrdenTrabajoSpoolID IN(
		SELECT OrdenTrabajoSpoolID FROM #OrdenTrabajoSpool
	)
	
	SELECT	s.SpoolID,
			s.Dibujo, 
			s.RevisionCliente, 
			ots.OrdenTrabajoSpoolID, 
			ws.WorkstatusSpoolID,
			ots.Nombre,
			ots.NumeroControl,
			ProyectoID
	FROM Spool s
	LEFT JOIN #OrdenTrabajoSpool ots 
		ON ots.SpoolID = s.SpoolID
	LEFT JOIN WorkstatusSpool ws 
		ON ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	WHERE s.[SpoolID] IN
	(
		SELECT SpoolID FROM #SpoolIDs
	)

	
	SELECT	wks.WorkstatusSpoolID, 
			ps.ReportePrimarios, 
			ps.ReporteAdherencia, 
			ps.ReportePullOff, 
			ps.ReporteIntermedios, 
			ps.ReporteAcabadoVisual, 
			NumeroEmbarque	
	FROM WorkstatusSpool wks
	LEFT JOIN PinturaSpool ps
		ON wks.WorkstatusSpoolID = ps.WorkstatusSpoolID
	LEFT JOIN EmbarqueSpool embs 
		ON embs.WorkstatusSpoolID = wks.WorkstatusSpoolID
	LEFT JOIN Embarque emb
		ON emb.EmbarqueID = embs.EmbarqueID
	WHERE wks.WorkstatusSpoolID IN(
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
		
	INSERT INTO #JuntaSpool	
	SELECT  JuntaSpoolID,
			SpoolID,
			Etiqueta as EtiquetaJunta, 
			TipoJuntaID,
			Diametro,
			Cedula
	FROM JuntaSpool
	WHERE JuntaSpoolID IN(SELECT JuntaSpoolID FROM #JuntaSpoolIDs)	
		
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
	UNION
      SELECT      DISTINCT NumeroUnicoDespachadoID
      from        OrdenTrabajoMaterial odtm
      inner join  OrdenTrabajoSpool odts on odtm.OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID
      where       odts.SpoolID in
      (
            select SpoolID from #SpoolIDs
      )
      and
      (
            select COUNT(OrdenTrabajoJuntaID)
            from OrdenTrabajoJunta odtj
            where odtj.OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID
      ) = 0
      
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
			CASE WHEN jws.EtiquetaJunta IS NULL THEN js.EtiquetaJunta
			ELSE jws.EtiquetaJunta END as EtiquetaJunta, 
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
			ja.NumeroUnico2ID,
			iv.NumeroReporte as ReporteIV			
	FROM  #JuntaSpool js
	LEFT JOIN #JuntaWorkstatus jws
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
	LEFT JOIN JuntaInspeccionVisual jiv
		ON jiv.JuntaWorkstatusID = jws.JuntaWorkstatusID and jiv.Aprobado = 1
	LEFT JOIN InspeccionVisual iv 
		ON iv.InspeccionVisualID = jiv.InspeccionVisualID
	
	
	DROP TABLE #JuntaSpoolIDs
	DROP TABLE #SpoolIDs
	DROP TABLE #OrdenTrabajoSpool
	DROP TABLE #JuntaSpool
	DROP TABLE #WorkstatusSpool
	DROP TABLE #NumeroUnico
	DROP TABLE #JuntaWorkstatus
	
	SET NOCOUNT OFF;
	
END


GO

