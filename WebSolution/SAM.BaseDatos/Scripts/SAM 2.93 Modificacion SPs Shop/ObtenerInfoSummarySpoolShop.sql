USE [SAM]
GO
/****** Object:  StoredProcedure [dbo].[ObtenerInfoSummarySpoolShop]    Script Date: 11/4/2014 3:36:13 PM ******/
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
ALTER PROCEDURE [dbo].[ObtenerInfoSummarySpoolShop]
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
		Hold VARCHAR(50),
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
		JuntaWorkstatusID INT,
		Diametro DECIMAL
		
	)
	
	CREATE TABLE #ReqRepPNDTT
	(
		SpoolId INT,
		JuntaWorkstatusId INT,
		RequiPTId INT NULL,
		NumeroRequiPT VARCHAR(50) NULL,
		NumeroReportePT VARCHAR(50) NULL,
		FechaRequiPT DATETIME NULL,
		AprobadoPT BIT NULL,
		RequiRTId INT NULL,
		NumeroReporteRT VARCHAR(50) NULL,
		NumeroRequiRT VARCHAR(50) NULL,
		FechaRequiRT DATETIME NULL,
		AprobadoRT BIT NULL,
		NumeroReportePWHT VARCHAR(50) NULL,
		AprobadoPWHT BIT NULL
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
		Factura VARCHAR(50),
		Cantidad INT 
	)			

	INSERT INTO #Spool
		SELECT	s.SpoolID, 
				s.ProyectoID,
				CASE WHEN sh.TieneHoldCalidad = 1 THEN 'SI'
					WHEN sh.TieneHoldIngenieria = 1 THEN 'SI'
					WHEN sh.SpoolID is null THEN 'NO'
					ELSE 'NO' END,
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
				CASE WHEN s.RequierePwht = 0 THEN 'NO'
					 WHEN s.RequierePwht > 0 THEN 'SI' 
					 END,
				e.NumeroEmbarque,
			s.PorcentajePnd		
			FROM Spool  s	
			LEFT JOIN SpoolHold sh
				ON s.SpoolID = sh.SpoolID		
			LEFT JOIN OrdenTrabajoSpool ots
				ON ots.SpoolID = s.spoolID
			LEFT JOIN WorkstatusSpool wss
				ON wss.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID		
			LEFT JOIN ReporteDimensionalDetalle rdd
				ON rdd.WorkstatusSpoolID = wss.WorkstatusSpoolID				
			LEFT JOIN ReporteDimensional rd
				ON rdd.ReporteDimensionalID = rd.ReporteDimensionalID	
				AND rdd.Aprobado = 1 --solo el reporte aprobado
				AND rd.TipoReporteDimensionalID = 1 --Réporte tipo Liberacion Dimensional			
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
					  WHEN js.RequierePwht = 0 THEN 'NO'
					  WHEN js.RequierePwht = 1 THEN 'SI'
					  END,				
				apj.ClasificacionPND, --clasif pnd				
				jws.JuntaWorkstatusID,
				js.Diametro										
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
		AND js.FabAreaID = (SELECT FabAreaID FROM FabArea where Codigo LIKE 'SHOP')		
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
		AND jws.JuntaWorkstatusID = #JuntaSpool.JuntaWorkstatusID

	INSERT INTO #ReqRepPNDTT
	SELECT @SpoolId,js.JuntaWorkstatusId, NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL, NULL, NULL,NULL, NULL
	FROM #JuntaSpool js

	Update #ReqRepPNDTT
		SET NumeroRequiPT = r.NumeroRequisicion,
			FechaRequiPt = r.FechaRequisicion,
			RequiPTId = r.RequisicionId,
			NumeroReportePT = rpnd.numeroReporte,
			AprobadoPT = jrpnd.Aprobado
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				left join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'Reporte de PT' and Categoria like 'PND')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		LEFT JOIN JuntareportePND jrpnd
			ON jrpnd.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrpnd.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		LEFT JOIN ReportePND rpnd 
			ON rpnd.ReportePNDId = jrpnd.ReportePNDId			
	where  sqry.JuntaWorkstatusID = #ReqRepPNDTT.JuntaWorkstatusId	
	
	
	Update #ReqRepPNDTT
		SET NumeroRequiRT = r.NumeroRequisicion,
			FechaRequiRt = r.FechaRequisicion,
			RequiRTId = r.RequisicionId,
			NumeroReporteRT = rpnd.numeroReporte,
			AprobadoRT = jrpnd.Aprobado
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				left join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'Reporte de RT' and Categoria like 'PND')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		LEFT JOIN JuntareportePND jrpnd
			ON jrpnd.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrpnd.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		LEFT JOIN ReportePND rpnd 
			ON rpnd.ReportePNDId = jrpnd.ReportePNDId			
	where  sqry.JuntaWorkstatusID = #ReqRepPNDTT.JuntaWorkstatusId	
	
	
		Update #ReqRepPNDTT
		SET NumeroReportePWHT = rtt.NumeroReporte,			
			AprobadoPWHT = jrTt.Aprobado
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				left join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'PWHT' and Categoria like 'TT')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		LEFT JOIN JuntareporteTT jrTT
			ON jrTT.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrTT.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		LEFT JOIN ReporteTt rtt
			ON rtt.ReporteTtID = jrTt.ReporteTtID			
	where  sqry.JuntaWorkstatusID = #ReqRepPNDTT.JuntaWorkstatusId	
	

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
			nu.Factura,
			ms.Cantidad			
			FROM MaterialSpool ms
			INNER JOIN #Spool s 		
			on s.SpoolID = ms.SpoolID	
			left join OrdenTrabajoMaterial otm
			on ms.MaterialSpoolID = otm.MaterialSpoolID
			LEFT JOIN ItemCode ic
				ON ic.ItemCodeID = ms.ItemCodeID							
			LEFT JOIN NumeroUnico nu
				ON nu.ItemCodeID =	 ms.ItemCodeID	
			and otm.NumeroUnicoDespachadoID = nu.NumeroUnicoID
			left join Despacho d
			on d.DespachoID = otm.DespachoID
			LEFT JOIN RecepcionNumeroUnico rnu
			ON rnu.NumeroUnicoID = nu.NumeroUnicoID
			LEFT JOIN Recepcion r
			ON r.RecepcionID = rnu.RecepcionID
			LEFT JOIN Colada c
				ON c.ColadaID = nu.ColadaID
				and c.ProyectoID= s.ProyectoId
				LEFT JOIN Fabricante f
				ON f.FabricanteID = c.FabricanteID
		
			 ORDER BY ms.Etiqueta ASC

	SELECT * FROM #Spool
	SELECT * FROM #JuntaSpool 
	SELECT * FROM #Materiales
	SELECT * FROM #ReqRepPNDTT
		

	DROP TABLE #Spool
	DROP TABLE #JuntaSpool
	DROP TABLE #Materiales
	DROP TABLE #NumeroUnico
	DROP TABLE #ReqRepPNDTT
	

SET NOCOUNT OFF;

END


GO
