IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerInfoPorcPnd]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerInfoPorcPnd]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerInfoPorcPnd
	Funcion:	Obtiene la infop relevante a numeros de juntas Rt aplicar,etc
	Parametros:	@SpoolID INT
	Autor:		SAC
	Modificado:	12/23/2010
*****************************************************************************************/
CREATE PROCEDURE dbo.ObtenerInfoPorcPnd
(@SpoolID INT) AS
BEGIN	
	
	DECLARE @Dibujo NVARCHAR(50) = ''
	DECLARE @ProyectoID INT
	DECLARE @PorcPND INT = 0
	DECLARE @BW INT
	DECLARE @OLET INT
	DECLARE @NumJuntas INT
	DECLARE @NumJuntasAplicarRT INT
	DECLARE @NumJuntasAplicarPT INT
	DECLARE @NumJuntasRTAprobado INT
	DECLARE @NumJuntasPTAprobado INT
	DECLARE @TipoPruebaPTID INT = 2
	DECLARE @TipoPruebaRTID INT = 1
	DECLARE @FaltantesRT INT	
	DECLARE @FaltantesPT INT
	DECLARE @FaltantesTotales INT
	
	SELECT @Dibujo = Dibujo, @PorcPND = PorcentajePnd, @ProyectoID = ProyectoID  FROM Spool WHERE SpoolID = @SpoolID
	SELECT @BW = TipoJuntaID  FROM TipoJunta WHERE Codigo = 'BW'
	SELECT @OLET = TipoJuntaID  FROM TipoJunta WHERE Codigo = 'LET'
	
	
	SELECT JuntaSpoolID, TipoJuntaID
		INTO #JuntaSpoolIDS 
	FROM JuntaSpool 
	WHERE  (TipoJuntaID = @BW OR TipoJuntaID = @OLET) AND
		   SpoolID IN(
					SELECT SpoolID 
					FROM Spool 
					WHERE Dibujo = @Dibujo AND ProyectoID = @ProyectoID
					)
	
	
	IF @PorcPND < 100
		BEGIN
			SELECT @NumJuntas = COUNT(*) 
			FROM #JuntaSpoolIDS
			WHERE TipoJuntaID = @BW
			
			SELECT @NumJuntasAplicarRT = CEILING((@PorcPND * @NumJuntas)/100.0)
			SELECT @NumJuntasRTAprobado = COUNT(jr.JuntaReportePndID)  
			FROM JuntaReportePnd jr 
			INNER JOIN ReportePnd pnd 
				ON jr.ReportePndID = pnd.ReportePndID
			INNER JOIN JuntaWorkstatus jw 
				ON jw.JuntaWorkstatusID = jr.JuntaWorkstatusID
			WHERE TipoPruebaID = @TipoPruebaRTID 
				AND Aprobado = 1 
				AND JuntaSpoolID IN (SELECT JuntaSpoolID FROM #JuntaSpoolIDS)
		END
	ELSE
		BEGIN			
			
			SELECT @NumJuntasAplicarRT = COUNT(*) 
			FROM #JuntaSpoolIDS
			WHERE TipoJuntaID = @BW
			
			SELECT @NumJuntasAplicarPT = COUNT(*) 
			FROM #JuntaSpoolIDS
			WHERE TipoJuntaID = @OLET
			
			SELECT @NumJuntasRTAprobado = COUNT(jr.JuntaReportePndID)  
			FROM JuntaReportePnd jr 
			INNER JOIN ReportePnd pnd 
				ON jr.ReportePndID = pnd.ReportePndID
			INNER JOIN JuntaWorkstatus jw 
				ON jw.JuntaWorkstatusID = jr.JuntaWorkstatusID
			WHERE TipoPruebaID = @TipoPruebaRTID 
				AND Aprobado = 1
				AND JuntaSpoolID IN (SELECT JuntaSpoolID FROM #JuntaSpoolIDS)
			
			SELECT @NumJuntasPTAprobado = COUNT(jr.JuntaReportePndID)  
			FROM JuntaReportePnd jr 
			INNER JOIN ReportePnd pnd 
				ON jr.ReportePndID = pnd.ReportePndID
			INNER JOIN JuntaWorkstatus jw 
				ON jw.JuntaWorkstatusID = jr.JuntaWorkstatusID
			WHERE TipoPruebaID = @TipoPruebaPTID 
				AND Aprobado = 1
				AND JuntaSpoolID IN (SELECT JuntaSpoolID FROM #JuntaSpoolIDS)
		END
		
	SELECT @FaltantesPT = ISNULL(@NumJuntasAplicarPT,0) - ISNULL(@NumJuntasPTAprobado,0)
	IF @FaltantesPT < 0
		SELECT @FaltantesPT = 0
		
	SELECT @FaltantesRT = ISNULL(@NumJuntasAplicarRT,0) - ISNULL(@NumJuntasRTAprobado,0)
	IF @FaltantesRT < 0
		SELECT @FaltantesRT = 0
	
	SELECT @FaltantesTotales = @FaltantesPT + @FaltantesRT	
	
	SELECT  @PorcPND AS PorcantajePnd, 
			@NumJuntas AS JuntasTotales,
			ISNULL(@NumJuntasAplicarPT,0) AS NumJuntasAplicarPT,
			ISNULL(@NumJuntasPTAprobado,0) AS NumJuntasPTAprobado, 
			@FaltantesPT AS FaltantesPT,
			@NumJuntasAplicarRT AS NumJuntasAplicarRT,
			@NumJuntasRTAprobado AS NumJuntasRTAprobado,
			@FaltantesRT AS FaltantesRT,
			@FaltantesTotales AS FaltantesTotales
			
			 
	DROP TABLE #JuntaSpoolIDS
END			  

GO


