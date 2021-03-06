IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Destajos]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_Destajos]
GO
-- =============================================
-- Author:		Cesar Velazquez
-- Create date: 10/Enero/2011
-- Description:	Obtiene los datos de pagos de destajos de un proyecto
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Destajos]
@ProyectoID INT,
@Idioma INT
AS
BEGIN
	--Se crea la tabla temporal de Spool
	
	CREATE TABLE #Spool(
		SpoolID INT,
		ProyectoID INT,
		OrdenTrabajoSpoolID INT,
		Nombre NVARCHAR(50),		
	    NumeroControl NVARCHAR(50),
	    NumeroOrden NVARCHAR(50)
	)
	
	--Se obtienen los spools del proyecto
	INSERT INTO #Spool
	
	SELECT	s.SpoolID,
			s.ProyectoID, 
			ots.OrdenTrabajoSpoolID, 
			s.Nombre, 
			ots.NumeroControl,
			ot.NumeroOrden
	FROM Spool s
	
	INNER JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID
	INNER JOIN OrdenTrabajo ot ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
	WHERE s.ProyectoID = @ProyectoID
	ORDER BY s.SpoolID
	
		
	--Se crea la tabla temporal JuntaSpool
	CREATE TABLE #JuntaSpool(
		JuntaSpoolID INT,
		SpoolID INT,
		TipoJuntaID INT,
		Etiqueta NVARCHAR(10),
		Diametro DECIMAL(7,4),
		Cedula NVARCHAR(10),
		Espesor DECIMAL(10,4),
		EtiquetaMaterial1 NVARCHAR(10),
		EtiquetaMaterial2 NVARCHAR(10) 
	)
	
	--Se obtienen las juntas de los Spools
	INSERT INTO #JuntaSpool
	SELECT  js.JuntaSpoolID,
			js.SpoolID,
			js.TipoJuntaID,
			js.Etiqueta,
			js.Diametro,
			js.Cedula,
			js.Espesor,
			js.EtiquetaMaterial1,
			js.EtiquetaMaterial2
			
	FROM JuntaSpool js
	WHERE js.SpoolID IN (SELECT SpoolID FROM #Spool)
	
	--Se crea la tabla JuntaWorkstatus
	CREATE TABLE #JuntaWorkstatus(
		JuntaWorkstatusID INT,
		JuntaSpoolID INT,
		UltimoProcesoID INT
		)

	--Se obtienen los datos de JuntaWorkstatus que pertenecen a las Juntas
	INSERT INTO #JuntaWorkstatus
	SELECT  jw.JuntaWorkstatusID,
			jw.JuntaSpoolID,
			jw.UltimoProcesoID
	FROM JuntaWorkstatus jw
	WHERE JuntaSpoolID IN (SELECT JuntaSpoolID FROM #JuntaSpool)		
	
	--Se crea la tabla temporal Armado
	CREATE TABLE #Armado(
		JuntaWorkstatusID INT,
		DestajoArmado INT,
		TotalArmado Money
	)
	
	INSERT INTO #Armado
		SELECT  dtd.JuntaWorkstatusID,
				pd.Semana,
				dtd.Total
		FROM DestajoTuberoDetalle dtd
		INNER JOIN #JuntaWorkstatus jw
		ON (dtd.JuntaWorkstatusID = jw.JuntaWorkstatusID)
		INNER JOIN DestajoTubero dt
		ON (dt.DestajoTuberoID = dtd.DestajoTuberoID)
		INNER JOIN PeriodoDestajo pd
		ON (pd.PeriodoDestajoID = dt.PeriodoDestajoID)
		
	--Se crea la tabla temporal Fondeo
	CREATE TABLE #Fondeo(
		JuntaWorkstatusID INT,
		DestajoFondeo INT,
		TotalFondeo Money
	)
	
	INSERT INTO #Fondeo
		SELECT  dsd.JuntaWorkstatusID,
				pd.Semana,
				SUM(dsd.DestajoRaiz)
		FROM DestajoSoldadorDetalle dsd
		INNER JOIN #JuntaWorkstatus jw
		ON (dsd.JuntaWorkstatusID = jw.JuntaWorkstatusID)
		INNER JOIN DestajoSoldador ds
		ON (ds.DestajoSoldadorID = dsd.DestajoSoldadorID)
		INNER JOIN PeriodoDestajo pd
		ON (pd.PeriodoDestajoID = ds.PeriodoDestajoID)
		GROUP BY dsd.JuntaWorkstatusID,pd.Semana
	
	--Se crea la tabla temporal Relleno
	CREATE TABLE #Relleno(
		JuntaWorkstatusID INT,
		DestajoRelleno INT,
		TotalRelleno Money
	)
	
	INSERT INTO #Relleno
		SELECT  dsd.JuntaWorkstatusID,
				pd.Semana,
				SUM(dsd.DestajoRelleno)
		FROM DestajoSoldadorDetalle dsd
		INNER JOIN #JuntaWorkstatus jw
		ON (dsd.JuntaWorkstatusID = jw.JuntaWorkstatusID)
		INNER JOIN DestajoSoldador ds
		ON (ds.DestajoSoldadorID = dsd.DestajoSoldadorID)
		INNER JOIN PeriodoDestajo pd
		ON (pd.PeriodoDestajoID = ds.PeriodoDestajoID)
		GROUP BY dsd.JuntaWorkstatusID,pd.Semana

	SELECT 
	
		p.Nombre AS [NombreProyecto],
		s.NumeroOrden AS [OrdenTrabajo],
		s.NumeroControl,
		s.Nombre AS [Spool],
		js.Etiqueta AS [Junta],
		tj.Nombre AS [TipoJunta],
		js.Diametro,
		js.Cedula,
		ISNULL(js.Espesor,0) AS [Espesor],
		js.EtiquetaMaterial1 + ' - ' + js.EtiquetaMaterial2 AS [Localizacion],
		[UltimoProceso] =
						CASE 
							WHEN @Idioma = 0 THEN up.Nombre
							ELSE up.NombreIngles
						END,
		[¿Hold?] = 
				CASE 
					WHEN ISNULL(sh.SpoolID,0) = 0 THEN 'No'
					WHEN sh.TieneHoldIngenieria = 1 AND @Idioma = 0 THEN 'Sí'
					WHEN sh.TieneHoldCalidad = 1 AND @Idioma = 0 THEN 'Sí'
					WHEN sh.Confinado = 1 AND @Idioma = 0 THEN 'Sí'
					WHEN sh.TieneHoldIngenieria = 1 AND @Idioma = 1 THEN 'Yes'
					WHEN sh.TieneHoldCalidad = 1 AND @Idioma = 1 THEN 'Yes'
					WHEN sh.Confinado = 1 AND @Idioma = 1 THEN 'Yes'
					ELSE 'No'
				END,
		ISNULL(a.DestajoArmado,0) AS [DestajoArmado],
		ISNULL(f.DestajoFondeo,0) AS [DestajoFondeo],
		ISNULL(r.DestajoRelleno,0) AS [DestajoRelleno],
		ISNULL(a.TotalArmado,0) AS [TotalArmado],
		ISNULL(f.TotalFondeo,0) AS [TotalFondeo],
		ISNULL(r.TotalRelleno,0) AS [TotalRelleno]
		
	FROM Proyecto p
	INNER JOIN #Spool s
	ON (s.ProyectoID = p.ProyectoID)
	
	INNER JOIN #JuntaSpool js
	ON (js.SpoolID = s.SpoolID)
	
	INNER JOIN TipoJunta tj
	ON (tj.TipoJuntaID = js.TipoJuntaID)
	
	INNER JOIN #JuntaWorkstatus jw
	ON (jw.JuntaSpoolID = js.JuntaSpoolID)
	
	INNER JOIN UltimoProceso up
	ON (up.UltimoProcesoID = jw.UltimoProcesoID)
	
	LEFT JOIN SpoolHold sh
	ON (js.SpoolID = sh.SpoolID)
	
	LEFT JOIN #Armado a
	ON (jw.JuntaWorkstatusID = a.JuntaWorkstatusID)
	
	LEFT JOIN #Fondeo f
	ON (jw.JuntaWorkstatusID = f.JuntaWorkstatusID)
	
	LEFT JOIN #Relleno r
	ON (jw.JuntaWorkstatusID = r.JuntaWorkstatusID)
	
	WHERE p.ProyectoID = @ProyectoID
	ORDER BY s.NumeroControl
END

GO



