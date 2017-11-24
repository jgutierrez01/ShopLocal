IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculaHojaParaReporte]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[CalculaHojaParaReporte]
GO
/****************************************************************************************
	Nombre:		CalculaHojaParaReporte
	Funcion:	Calcula las hojas que cada reporte debe incluir y actualiza los valores
				en la tabla del reporte especificado
	Parametros:	@TipoReporte INT
				@ProyectoID INT
				@NumeroReporte NVARCHAR(50)
				@IDs VARCHAR(MAX)
	Autor:		LMG
	Modificado:	02/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[CalculaHojaParaReporte]
(	
	@TipoReporte INT,
	@ProyectoID INT,
	@NumeroReporte NVARCHAR(50),
	@IDs VARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT ON;
	
	CREATE TABLE #TempDibujos
	(	
		DibujoID INT IDENTITY,
		Dibujo NVARCHAR(50),
		SpoolID INT
	)
	
	CREATE TABLE #TempJuntas
	(	
		JuntaSpoolID INT,
		JuntaWorkstatusID INT
	)
	
	CREATE TABLE #TempSpools
	(	
		SpoolID INT,
		WorkstatusSpoolID INT
	)
	-- Se obtienen los ids de las juntas que son parte del reporte
		
	-- Inspeccion Visual
	IF(@TipoReporte = 0)
	BEGIN 
		INSERT INTO #TempJuntas
		SELECT 	jw.JuntaSpoolID, 
				jw.JuntaWorkstatusID
		FROM	JuntaWorkstatus jw
		INNER JOIN JuntaInspeccionVisual jiv on jw.JuntaWorkstatusID = jiv.JuntaWorkstatusID
		INNER JOIN InspeccionVisual iv on jiv.InspeccionVisualID = iv.InspeccionVisualID
		WHERE	jw.JuntaWorkstatusID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (iv.NumeroReporte = @NumeroReporte  AND iv.ProyectoID = @ProyectoID)
	END
	-- Reporte TT
	ELSE IF(@TipoReporte = 1) 
	BEGIN 
		INSERT INTO #TempJuntas
		SELECT 	jw.JuntaSpoolID, 
				jw.JuntaWorkstatusID
		FROM	JuntaWorkstatus jw
		INNER JOIN JuntaReporteTt jtt on jw.JuntaWorkstatusID = jtt.JuntaWorkstatusID
		INNER JOIN ReporteTt tt on jtt.ReporteTtID = tt.ReporteTtID
		WHERE	jw.JuntaWorkstatusID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (tt.NumeroReporte = @NumeroReporte  AND tt.ProyectoID = @ProyectoID)
	END
	-- Reporte PND
	ELSE IF(@TipoReporte = 2) 
	BEGIN 
		INSERT INTO #TempJuntas
		SELECT 	jw.JuntaSpoolID, 
				jw.JuntaWorkstatusID
		FROM	JuntaWorkstatus jw
		INNER JOIN JuntaReportePnd jpnd on jw.JuntaWorkstatusID = jpnd.JuntaWorkstatusID
		INNER JOIN ReportePnd pnd on jpnd.ReportePndID = pnd.ReportePndID
		WHERE	jw.JuntaWorkstatusID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (pnd.NumeroReporte = @NumeroReporte  AND pnd.ProyectoID = @ProyectoID)
	END
	-- Reporte Dimensional
	ELSE IF(@TipoReporte = 3) 
	BEGIN 
		INSERT INTO #TempSpools
		SELECT 	ots.SpoolID, 
				ws.WorkstatusSpoolID
		FROM	WorkstatusSpool ws
		INNER JOIN OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		INNER JOIN ReporteDimensionalDetalle rdd on rdd.WorkstatusSpoolID = ws.WorkstatusSpoolID
		INNER JOIN ReporteDimensional rd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID
		WHERE	ws.WorkstatusSpoolID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (rd.NumeroReporte = @NumeroReporte  AND rd.ProyectoID = @ProyectoID)
	END
	-- Reporte Spool PND
	ELSE IF(@TipoReporte = 21)
	BEGIN 
		INSERT INTO #TempSpools
		SELECT 	ots.SpoolID, 
				ws.WorkstatusSpoolID
		FROM	WorkstatusSpool ws
		INNER JOIN OrdenTrabajoSpool ots on ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
		INNER JOIN SpoolReportePnd spnd on ws.WorkstatusSpoolID = spnd.WorkstatusSpoolID
		INNER JOIN ReporteSpoolPnd pnd on spnd.ReporteSpoolPndID = pnd.ReporteSpoolPndID
		WHERE	ws.WorkstatusSpoolID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,',')) OR (pnd.NumeroReporte = @NumeroReporte  AND pnd.ProyectoID = @ProyectoID)
	END
	
								
	----Obtengo los Spools
	IF(@TipoReporte <> 3 and @TipoReporte <> 21)
	BEGIN
		INSERT INTO #TempDibujos
		SELECT DISTINCT s.Dibujo, s.SpoolID
		FROM JuntaSpool js
		INNER JOIN Spool s on js.SpoolID = s.SpoolID
		WHERE js.JuntaSpoolID in (SELECT JuntaSpoolID FROM #TempJuntas)
		ORDER BY s.Dibujo
	END
	ELSE	
	BEGIN
		INSERT INTO #TempDibujos
		SELECT DISTINCT s.Dibujo, s.SpoolID
		FROM Spool s 
		WHERE s.SpoolID in (SELECT SpoolID FROM #TempSpools)
		ORDER BY s.Dibujo
	END
	
	
	-- Inspeccion Visual
	IF(@TipoReporte = 0)
	BEGIN 
		UPDATE JuntaInspeccionVisual 
		SET JuntaInspeccionVisual.Hoja = (SELECT DibujoID 
										  FROM #TempDibujos 
										  WHERE SpoolID = (	SELECT j.SpoolID 
															FROM JuntaSpool j
															INNER JOIN JuntaWorkstatus jw on j.JuntaSpoolID = jw.JuntaSpoolID
															WHERE jw.JuntaWorkstatusID = JuntaInspeccionVisual.JuntaWorkstatusID))
		WHERE JuntaInspeccionVisual.InspeccionVisualID in (	SELECT InspeccionVisualID 
															FROM InspeccionVisual 
															WHERE NumeroReporte = @NumeroReporte AND ProyectoID = @ProyectoID)
	END
	-- Reporte TT
	ELSE IF(@TipoReporte = 1)
	BEGIN 
		UPDATE JuntaReporteTt 
		SET JuntaReporteTt.Hoja = (	SELECT DibujoID 
									FROM #TempDibujos 
									WHERE SpoolID = (SELECT j.SpoolID 
													 FROM JuntaSpool j
													 INNER JOIN JuntaWorkstatus jw on j.JuntaSpoolID = jw.JuntaSpoolID
													 WHERE jw.JuntaWorkstatusID = JuntaReporteTt.JuntaWorkstatusID))
		WHERE JuntaReporteTt.ReporteTtID in (SELECT ReporteTtID 
											 FROM ReporteTt
											 WHERE NumeroReporte = @NumeroReporte AND ProyectoID = @ProyectoID)
	END
	-- Reporte PND
	ELSE IF(@TipoReporte = 2)
	BEGIN 
		UPDATE JuntaReportePnd 
		SET JuntaReportePnd.Hoja = (SELECT DibujoID 
									FROM #TempDibujos 
									WHERE SpoolID = (SELECT j.SpoolID 
													 FROM JuntaSpool j
													 INNER JOIN JuntaWorkstatus jw on j.JuntaSpoolID = jw.JuntaSpoolID
													 WHERE jw.JuntaWorkstatusID = JuntaReportePnd.JuntaWorkstatusID))
		WHERE JuntaReportePnd.ReportePndID in (	SELECT ReportePndID 
												FROM ReportePnd
												WHERE NumeroReporte = @NumeroReporte AND ProyectoID = @ProyectoID)
	END
	-- Reporte Dimensional
	ELSE IF(@TipoReporte = 3)
	BEGIN 
		UPDATE ReporteDimensionalDetalle 
		SET ReporteDimensionalDetalle.Hoja = (	SELECT DibujoID 
												FROM #TempDibujos 
												WHERE SpoolID = (SELECT s.SpoolID 
																 FROM OrdenTrabajoSpool s
																 INNER JOIN WorkstatusSpool ws on s.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
																 WHERE ws.WorkstatusSpoolID = ReporteDimensionalDetalle.WorkstatusSpoolID))
		WHERE ReporteDimensionalDetalle.WorkstatusSpoolID in (SELECT Value FROM dbo.SplitCVSToTable(@IDs,','))
	END
	-- Reporte Spool PND
	ELSE IF(@TipoReporte = 21)
	BEGIN 
		UPDATE SpoolReportePnd
		SET SpoolReportePnd.Hoja = (SELECT DibujoID 
									FROM #TempDibujos 
									WHERE SpoolID = (SELECT s.SpoolID
													 FROM OrdenTrabajoSpool s
													 INNER JOIN WorkstatusSpool ws on s.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
													 WHERE ws.WorkstatusSpoolID = SpoolReportePnd.WorkstatusSpoolID))
		WHERE SpoolReportePnd.ReporteSpoolPndID in (SELECT ReporteSpoolPndID 
													FROM ReporteSpoolPnd
													WHERE NumeroReporte = @NumeroReporte AND ProyectoID = @ProyectoID)
	END

	DROP TABLE #TempDibujos
	DROP TABLE #TempJuntas	
		
	SELECT CAST(1 as bit)
		
	SET NOCOUNT OFF;

END


GO
