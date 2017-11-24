
/****** Object:  StoredProcedure [dbo].[ObtenerInfoSummarySpoolShop]    Script Date: 10/24/2014 6:04:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerInfoSummarySpoolShop
	Funcion:	Obtiene la info para el resumen de spool por numero de control
	Parametros:	@SpoolIDs nvarchar(max)
	Autor:		SCB
	Modificado:	
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerInfoSummarySpoolShop]
(
	@SpoolID NVARCHAR(MAX),
	@ProyectoID INT = NULL
)
AS 
BEGIN

	SET NOCOUNT ON;

	DECLARE @SumPeqs INT,
			@SumJuntas INT,
			@CantMaterialesPendientes INT,
			@CantDespachosPendientes INT,
			@CantArmadosPendientes INT,
			@CantSoldadurasPendientes INT,
			@CantInsVisualPendientes INT,			
			@CantPWHTPendientes INT,
			@CantRequierePWHT INT, 
			@PendientesPND INT, 
			@PendienteDimensional BIT,
			@ReportePruebaHidrostatica VARCHAR(100),
			@ReportePruebaNeumatica VARCHAR(100)

	CREATE TABLE #Spool
	(
		SpoolID INT NOT NULL, 
		ProyectoID INT, 
		FechaLiberacionDimensional DATETIME, 		
		LiberacionDimensional VARCHAR(50), 
		FechaPrimario DATETIME,
		ReportePrimario VARCHAR(50),		
		FechaAcabadoVisual DATETIME,
		ReporteAcabadoVisual VARCHAR(50),
		Sistema VARCHAR(50),
		Cuadrante VARCHAR(50)	,
		Spool VARCHAR(50),
		RequierePWHT VARCHAR(50),
		NumeroEmbarque VARCHAR(50),
		PorcentajePND INT		
	)
	
	CREATE TABLE #JuntaSpool(
		JuntaSpoolID INT, 
		SpoolID INT,		
		EtiquetaJunta NVARCHAR(50),
		TipoJuntaID INT,
		TipoJunta VARCHAR(50),
		FechaSoldadura DATETIME,
		FechaInspeccionVisual DATETIME,
		RequierePWHT VARCHAR(50),		
		ClasifPND VARCHAR(50),
		ReporteTt VARCHAR(50),
		TtTipoPruebaID INT,
		FechaReporteTt VARCHAR(50),	
		RequiTt VARCHAR(50),
		ReportePnd VARCHAR(50),
		PndTipoPruebaID INT,
		FechaReportePND DATETIME,
		RequiPND VARCHAR(50),
		JuntaWorkstatusID INT
		
	)
	
	CREATE TABLE #NumeroUnico(
		NumeroUnicoID INT
	)

	CREATE TABLE #Materiales
	(
		NumeroUnicoID INT,
		SpoolID INT,	
		Etiqueta VARCHAR(50),		
		NumeroUnico VARCHAR(50),
		Certificado VARCHAR(50),
		ItemCode VARCHAR(150),		
		Descripcion VARCHAR(500),
		FabricanteId INT,
		Fabricante VARCHAR(150),
		Pedimento1 VARCHAR(50),
		Pedimento2 VARCHAR(50),
		Factura VARCHAR(50)
	)
	
				

	INSERT INTO #Spool
		SELECT	s.SpoolID, 
				s.ProyectoID,
				CASE WHEN rd.FechaReporte IS NOT NULL THEN rd.FechaReporte
					ELSE NULL END,			 -- fecha lib dim				
				CASE WHEN rd.NumeroReporte IS NOT NULL THEN rd.NumeroReporte
					ELSE '' END,			 -- lib dim
				ps.FechaPrimarios,
				ps.ReportePrimarios,
				ps.FechaAcabadoVisual,
				ps.ReporteAcabadoVisual,
				s.SistemaPintura,
				c.Nombre,
				s.Nombre,
				CASE WHEN s.RequierePwht = 0 THEN 'No'
					 WHEN s.RequierePwht > 0 THEN 'Si' 
					 END,
				e.NumeroEmbarque,
			s.PorcentajePnd		
			FROM Spool  s			
			LEFT JOIN OrdenTrabajoSpool ots
				ON ots.SpoolID = s.spoolID
			LEFT JOIN WorkstatusSpool wss
				ON wss.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID		
			LEFT JOIN ReporteDimensionalDetalle rdd
				ON rdd.WorkstatusSpoolID = wss.WorkstatusSpoolID				
			LEFT JOIN ReporteDimensional rd
				ON rdd.ReporteDimensionalID = rd.ReporteDimensionalID	
				AND rdd.Aprobado = 1 --solo el reporte aprobado
				AND rd.TipoReporteDimensionalID = 1 --Réporte tipo Liberacion Dimensional			L
			LEFT JOIN PinturaSpool ps 
				ON wss.WorkstatusSpoolID = ps.WorkstatusSpoolID
			LEFT JOIN Cuadrante c
			On c.CuadranteID = s.CuadranteID
			LEFT JOIN EmbarqueSpool es
			ON es.WorkstatusSpoolID = wss.WorkstatusSpoolID
			LEFT JOIN Embarque e
			ON e.EmbarqueID = es.EmbarqueID	
			WHERE s.ProyectoID = @ProyectoID
				AND s.SpoolID = @SpoolID		
	

	INSERT INTO #JuntaSpool	
		SELECT  js.JuntaSpoolID,
				js.SpoolID,
				CASE WHEN jws.EtiquetaJunta IS NULL THEN js.Etiqueta
					ELSE jws.EtiquetaJunta END AS EtiquetaJunta,
				js.TipoJuntaID,
				tj.Codigo,
				jSold.FechaSoldadura,
				null,--fechains visual
				CASE WHEN js.RequierePwht IS NULL THEN '' 
					  WHEN js.RequierePwht = 0 THEN 'No'
					  WHEN js.RequierePwht = 1 THEN 'Si'
					  END,				
				apj.ClasificacionPND, --clasif pnd
				'', --reporte tt
				NULL,--tt tipo prueba
				NULL,-- fecha tt
				'',
				'', -- reporte pnd
				NULL,-- pnd tipoPruebaId
				NULL, --Fechapnd
				'',
				jws.JuntaWorkstatusID										
		FROM JuntaSpool js 
		LEFT JOIN JuntaWorkstatus jws
			ON jws.JuntaSpoolID = js.JuntaSpoolID		
		LEFT JOIN JuntaSoldadura jSold
			ON jsold.JuntaWorkstatusID = jws.JuntaWorkstatusID		
		LEFT JOIN TipoJunta tj
			ON tj.TipoJuntaID = js.TipoJuntaID	
		LEFT JOIN AgrupadoresPorJunta apj
		ON apj.JuntaSpoolID = js.JuntaSpoolID
		WHERE js.SpoolID IN (SELECT SpoolID FROM #Spool)		
		order BY js.Etiqueta 

	
	UPDATE #JuntaSpool 
		SET FechaInspeccionVisual = jiv.FechaInspeccion		
		FROM JuntaInspeccionVisual jiv
		LEFT JOIN JuntaWorkstatus jws
			ON jiv.JuntaWorkstatusID = jws.JuntaWorkstatusID 	
			AND jiv.JuntaInspeccionVisualID = jws.JuntaInspeccionVisualID
		LEFT JOIN InspeccionVisual iv 
			ON iv.InspeccionVisualID = jiv.InspeccionVisualID
		WHERE iv.ProyectoID = ProyectoID
		AND jws.JuntaSpoolID = #JuntaSpool.JuntaSpoolID
	
	UPDATE #JuntaSpool 
	SET ReporteTt = rtt.NumeroReporte,
			TtTipoPruebaID = rtt.TipoPruebaID,
			FechaReporteTt = rtt.FechaReporte,			
			RequiTt = r.NumeroRequisicion
	FROM ReporteTt rTT
		LEFT JOIN JuntaReporteTt jrTT
		ON  rTT.ReporteTtID = jrTT.ReporteTtID
		LEFT JOIN TipoPrueba tp
		ON tp.TipoPruebaID = rTT.TipoPruebaID
		LEFT JOIN JuntaRequisicion jr
		on jrTT.JuntaWorkstatusID = jr.JuntaWorkstatusID
		LEFT JOIN Requisicion r
		on r.RequisicionID = jr.RequisicionID
		WHERE	jrTT.JuntaWorkstatusID = #JuntaSpool.JuntaWorkstatusID	

	UPDATE #JuntaSpool 
		SET ReportePnd = rpnd.NumeroReporte,
			PndTipoPruebaID = rpnd.TipoPruebaID,
			FechaReportePND = rpnd.FechaReporte,		
			RequiPND = r.NumeroRequisicion
	FROM ReportePnd rPND
		LEFT JOIN JuntaReportePnd jrPND
		ON rPND.ReportePndID = jrPND.ReportePndID
		LEFT JOIN TipoPrueba tp
		on tp.TipoPruebaID = rpnd.TipoPruebaID
		LEFT JOIN JuntaRequisicion jr
		on jrpnd.JuntaWorkstatusID = jr.JuntaWorkstatusID
		LEFT JOIN Requisicion r
		on r.RequisicionID = jr.RequisicionID
		WHERE jrPND.JuntaWorkstatusID = #JuntaSpool.JuntaWorkstatusID
	

	INSERT INTO #NumeroUnico
		SELECT DISTINCT NumeroUnico1ID 
		FROM JuntaArmado ja 
			WHERE ja.JuntaArmadoID IN (SELECT JuntaArmadoID FROM #JuntaSpool)
		UNION
		SELECT DISTINCT NumeroUnico2ID 
		FROM JuntaArmado ja 
			WHERE ja.JuntaArmadoID IN (SELECT JuntaArmadoID FROM #JuntaSpool) 
		UNION
		  SELECT      DISTINCT NumeroUnicoDespachadoID
		  FROM        OrdenTrabajoMaterial odtm
		  INNER JOIN  OrdenTrabajoSpool odts ON odtm.OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID
		  WHERE       odts.SpoolID in
		  (
				select SpoolID FROM #Spool
		  )
		  AND
		  (
				select COUNT(OrdenTrabajoJuntaID)
				FROM OrdenTrabajoJunta odtj
				WHERE odtj.OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID
		  ) = 0

	INSERT INTO #Materiales
		SELECT nu.NumeroUnicoId, 
			ms.SpoolID,	
			ms.Etiqueta,
			nu.Codigo,			
			c.NumeroCertificado,
			ic.Codigo,		
			ic.DescripcionEspanol,			
			c.FabricanteID,
			f.Nombre, 
			nu.OrdenDeCompra,
			r.CampoLibre2,
			nu.Factura			
			FROM MaterialSpool ms
			LEFT JOIN ItemCode ic
				ON ic.ItemCodeID = ms.ItemCodeID
			LEFT JOIN NumeroUnico nu
				ON nu.ItemCodeID =	 ms.ItemCodeID
			LEFT JOIN Colada c
				ON c.ColadaID = nu.ColadaID
			LEFT JOIN Fabricante f
				ON f.FabricanteID = c.FabricanteID		
			LEFT JOIN RecepcionNumeroUnico rnu
			ON rnu.NumeroUnicoID = nu.NumeroUnicoID
			LEFT JOIN Recepcion r
			ON r.RecepcionID = rnu.RecepcionID
			 WHERE SpoolID IN (SELECT SPOOLID FROM #Spool)
			 AND  nu.NumeroUnicoID IN (select * FROM #NumeroUnico )
			 ORDER BY ms.Etiqueta ASC

	SELECT * FROM #Spool
	SELECT * FROM #JuntaSpool 
	SELECT * FROM #Materiales
		

	DROP TABLE #Spool
	DROP TABLE #JuntaSpool
	DROP TABLE #Materiales
	DROP TABLE #NumeroUnico

	
	SET NOCOUNT OFF;
	
END


