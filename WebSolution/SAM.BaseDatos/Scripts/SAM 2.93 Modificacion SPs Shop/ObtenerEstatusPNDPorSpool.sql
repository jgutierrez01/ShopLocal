
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerEstatusPNDPorSpool') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[ObtenerSeguimientoDeSpools]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


/****************************************************************************************
	Nombre:		[ObtenerEstatusPNDPorSpool]
	Funcion:	obtiene estatus de reportes pnd por spool
	Parametros:	@JuntaSpoolID INT				
	Autor:		GTG
	Modificado:			

	Nota: Porcentaje pnd = 100%; Si todas las juntas tienen al menos un reporte pnd aprobado = completo, si no Incompleto
		  Porcentaje pnd != 100%; 
					CASO 1. Contar cuantas juntas tienen clasificado reporte PND y cuantas  tienen aprobado reporte PND,
								  cantidades iguales completo, si no incompleto
					CASO 2. Todas las Juntas tienen Clasificado PND = NO entonces No aplica
					CASO 3. Todas las juntas tienen Clasificado PND = NULL entonces ? "nose "		
*****************************************************************************************/

CREATE PROCEDURE [dbo].[ObtenerEstatusPNDPorSpool] 
	@SpoolID NVARCHAR(MAX),
	@ProyectoID INT = NULL,
	@EstatusPND INT out,
	@PorcentajePND INT
AS
BEGIN	
	
	SET NOCOUNT ON;

	-- Variables
	DECLARE	@CantJuntas INT,
			@CantAlMenosUno INT,
			@CantJuntasClasifNull INT,
			@CantJuntasNo INT,
			@CantJuntasReqPND INT

	-- Constantes
		DECLARE @NOAPLICA INT,
				@COMPLETO INT,
				@NOSE INT, -- cuando las juntas no tienen clasificacion PND
				@INCOMPPLETO INT

		SET @COMPLETO = 1
		SET @INCOMPPLETO = 0
		SET @NOAPLICA = 2
		SET @NOSE = 3

	CREATE TABLE #JuntasPND
	(	JuntaSpoolId INT,
		JuntaWorkStatusId INT,
		ClasifPND VARCHAR(50),
		RT VARCHAR(50),		
		PT VARCHAR(50),	
		RTPostTT VARCHAR(50),		
		PTPostTT VARCHAR(50),	
		UT VARCHAR(50),
		PMI VARCHAR(50),
		Neumatic VARCHAR(50),
		AlMenosUno BIT
	)


	INSERT INTO #JuntasPND
		SELECT js.JuntaSpoolId, jws.JuntaWorkstatusID, apj.ClasificacionPND, NULL,NULL,NULL,NULL,NULL,NULL,NULL, 0
			FROM JuntaSpool js
		LEFT JOIN AgrupadoresPorJunta apj
			ON apj.JuntaSpoolID = js.JuntaSpoolID
		LEFT JOIN JuntaWorkstatus jws
			ON jws.JuntaSpoolID = js.JuntaSpoolID
		WHERE js.SpoolID = @SpoolID
		AND js.FabAreaID = (SELECT FabAreaID FROM FabArea where Codigo LIKE 'SHOP')	
		and js.TipoJuntaID not in (SELECT TipoJuntaID FROM TipoJunta WHERE  Nombre IN ('Threaded','Tack Weld'))
			
	UPDATE #JuntasPND
	SET RT = rpnd.NumeroReporte										
			FROM	JuntaReportePnd jrpnd 
		LEFT join ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID		
		LEFT JOIN (
			SELECT JuntaWorkstatusID, MAX(jrpnd1.FechaPrueba) AS FechaPrueba, MAX(jrpnd1.FechaModificacion) AS FechaModificacion
							FROM JuntaReportePnd jrpnd1	
				INNER JOIN ReportePnd r1 ON r1.ReportePndID = jrpnd1.ReportePndID			
				AND r1.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'Reporte de RT') 	
				AND jrpnd1.Aprobado = 1			
				where JuntaWorkstatusID = jrpnd1.JuntaWorkstatusID
				group by JuntaWorkstatusID) AS a 
				ON jrpnd.JuntaWorkstatusID = a.JuntaWorkstatusID 
				AND jrpnd.FechaPrueba = a.FechaPrueba 
				AND jrpnd.FechaModificacion = a.FechaModificacion				
				AND jrpnd.Aprobado = 1		
				AND rpnd.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'Reporte de RT') 		
				where #JuntasPND.JuntaWorkStatusId = a.JuntaWorkstatusID

	UPDATE #JuntasPND
	SET PT = rpnd.NumeroReporte										
			FROM	JuntaReportePnd jrpnd 
		LEFT join ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID		
		LEFT JOIN (
			SELECT JuntaWorkstatusID, MAX(jrpnd1.FechaPrueba) AS FechaPrueba, MAX(jrpnd1.FechaModificacion) AS FechaModificacion 
							FROM JuntaReportePnd jrpnd1	
				INNER JOIN ReportePnd r1 ON r1.ReportePndID = jrpnd1.ReportePndID									
				where JuntaWorkstatusID = jrpnd1.JuntaWorkstatusID
				AND jrpnd1.Aprobado = 1			
				AND r1.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'Reporte de PT') 	
				group by JuntaWorkstatusID) AS a 
				ON jrpnd.JuntaWorkstatusID = a.JuntaWorkstatusID 
				AND jrpnd.FechaPrueba = a.FechaPrueba 
				AND jrpnd.FechaModificacion = a.FechaModificacion
				AND jrpnd.Aprobado = 1
				AND rpnd.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'Reporte de PT') 	
				where #JuntasPND.JuntaWorkStatusId = a.JuntaWorkstatusID

	UPDATE #JuntasPND
	SET RTPostTT = rpnd.NumeroReporte										
			FROM	JuntaReportePnd jrpnd 
		LEFT join ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID		
		LEFT JOIN (
			SELECT JuntaWorkstatusID, MAX(jrpnd1.FechaPrueba) AS FechaPrueba, MAX(jrpnd1.FechaModificacion) AS FechaModificacion
							FROM JuntaReportePnd jrpnd1	
				INNER JOIN ReportePnd r1 ON r1.ReportePndID = jrpnd1.ReportePndID					
				where JuntaWorkstatusID = jrpnd1.JuntaWorkstatusID	
				AND jrpnd1.Aprobado = 1					
				AND r1.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'RT (POST-TT)') 
				group by JuntaWorkstatusID) AS a 
				ON jrpnd.JuntaWorkstatusID = a.JuntaWorkstatusID 
				AND jrpnd.FechaPrueba = a.FechaPrueba 
				AND jrpnd.FechaModificacion = a.FechaModificacion				
				AND jrpnd.Aprobado = 1		
				AND rpnd.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'RT (POST-TT)') 		
				where #JuntasPND.JuntaWorkStatusId = a.JuntaWorkstatusID

	UPDATE #JuntasPND
	SET PTPostTT = rpnd.NumeroReporte										
			FROM	JuntaReportePnd jrpnd 
		LEFT join ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID		
		LEFT JOIN (
			SELECT JuntaWorkstatusID, MAX(jrpnd1.FechaPrueba) AS FechaPrueba, MAX(jrpnd1.FechaModificacion) AS FechaModificacion
							FROM JuntaReportePnd jrpnd1	
				INNER JOIN ReportePnd r1 ON r1.ReportePndID = jrpnd1.ReportePndID	
				where JuntaWorkstatusID = jrpnd1.JuntaWorkstatusID
				AND jrpnd1.Aprobado = 1		
				AND r1.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'PT (POST-TT)') 
				group by JuntaWorkstatusID) AS a 
				ON jrpnd.JuntaWorkstatusID = a.JuntaWorkstatusID 
				AND jrpnd.FechaPrueba = a.FechaPrueba 
				AND jrpnd.FechaModificacion = a.FechaModificacion
				AND jrpnd.Aprobado = 1
				AND rpnd.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'PT (POST-TT)') 		
				where #JuntasPND.JuntaWorkStatusId = a.JuntaWorkstatusID

	UPDATE #JuntasPND
	SET  UT = rpnd.NumeroReporte										
			FROM	JuntaReportePnd jrpnd 
		LEFT join ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID		
		LEFT JOIN (
			SELECT JuntaWorkstatusID, MAX(jrpnd1.FechaPrueba) AS FechaPrueba, MAX(jrpnd1.FechaModificacion) AS FechaModificacion 
							FROM JuntaReportePnd jrpnd1	
				INNER JOIN ReportePnd r1 ON r1.ReportePndID = jrpnd1.ReportePndID
				where JuntaWorkstatusID = jrpnd1.JuntaWorkstatusID
				AND jrpnd1.Aprobado = 1		
				AND r1.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'Reporte de UT') 	
				group by JuntaWorkstatusID) AS a 
				ON jrpnd.JuntaWorkstatusID = a.JuntaWorkstatusID 
				AND jrpnd.FechaPrueba = a.FechaPrueba 
				AND jrpnd.FechaModificacion = a.FechaModificacion
				AND jrpnd.Aprobado = 1
				AND rpnd.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'Reporte de UT') 		 		
				where #JuntasPND.JuntaWorkStatusId = a.JuntaWorkstatusID

	UPDATE #JuntasPND
	SET PMI = rpnd.NumeroReporte										
			FROM	JuntaReportePnd jrpnd 
		LEFT join ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID		
		LEFT JOIN (
			SELECT JuntaWorkstatusID, MAX(jrpnd1.FechaPrueba) AS FechaPrueba, MAX(jrpnd1.FechaModificacion) AS FechaModificacion 
							FROM JuntaReportePnd jrpnd1	
				INNER JOIN ReportePnd r1 ON r1.ReportePndID = jrpnd1.ReportePndID				
				where JuntaWorkstatusID = jrpnd1.JuntaWorkstatusID			
				AND jrpnd1.Aprobado = 1
				AND r1.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'Reporte PMI') 	
				group by JuntaWorkstatusID) AS a 
				ON jrpnd.JuntaWorkstatusID = a.JuntaWorkstatusID 
				AND jrpnd.FechaPrueba = a.FechaPrueba 
				AND jrpnd.FechaModificacion = a.FechaModificacion
				AND jrpnd.Aprobado = 1
				AND rpnd.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like  'Reporte PMI') 	
				where #JuntasPND.JuntaWorkStatusId = a.JuntaWorkstatusID

	UPDATE #JuntasPND
	SET Neumatic = rpnd.NumeroReporte										
			FROM	JuntaReportePnd jrpnd 
		LEFT join ReportePnd rpnd on rpnd.ReportePndID = jrpnd.ReportePndID		
		LEFT JOIN (
			SELECT JuntaWorkstatusID, MAX(jrpnd1.FechaPrueba) AS FechaPrueba, MAX(jrpnd1.FechaModificacion) AS FechaModificacion 
							FROM JuntaReportePnd jrpnd1	
				INNER JOIN ReportePnd r1 ON r1.ReportePndID = jrpnd1.ReportePndID	
				where JuntaWorkstatusID = jrpnd1.JuntaWorkstatusID
				AND jrpnd1.Aprobado = 1
				AND r1.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like 'Neumática') 	
				group by JuntaWorkstatusID) AS a 
				ON jrpnd.JuntaWorkstatusID = a.JuntaWorkstatusID 
				AND jrpnd.FechaPrueba = a.FechaPrueba 
				AND jrpnd.FechaModificacion = a.FechaModificacion
				AND jrpnd.Aprobado = 1
				AND rpnd.TipoPruebaID in (SELECT TipoPruebaID FROM TipoPrueba where Nombre like  'Neumática') 		
				where #JuntasPND.JuntaWorkStatusId = a.JuntaWorkstatusID

	UPDATE #JuntasPND
	SET AlMenosUno = Case WHEN (RT IS NOT NULL  AND RT <> '')
								  OR (PT IS NOT NULL AND PT <> '')
									OR (RTPostTT IS NOT NULL AND RTPostTT <> '')
										OR (PTPostTT IS NOT NULL AND PTPostTT <> '')
											OR (UT IS NOT NULL AND UT <> '')
												OR (PMI IS NOT NULL AND PMI <> '')
													OR (Neumatic IS NOT NULL AND Neumatic <> '') THEN 1 
							ELSE 0 END

	SELECT @CantJuntas = COUNT(JuntaSpoolId) FROM #JuntasPND

	IF @PorcentajePND = 0
		BEGIN
		
			SELECT @EstatusPND = @NOAPLICA
			Print '0%'
		END
	ELSE IF @PorcentajePND = 100
		BEGIN
		print '100%'
			SELECT @CantAlMenosUno = COUNT(JuntaSpoolId) FROM #JuntasPND
			WHERE AlMenosUno = 1

			IF @CantAlMenosUno = @CantJuntas
				BEGIN
					SELECT @EstatusPND = @COMPLETO
					print '100% completo' 
				END
			
			ELSE
				SELECT @EstatusPND = @INCOMPPLETO
				print '100% incompleto' 
		END
	ELSE
		BEGIN
	
			SELECT @CantJuntasClasifNull = COUNT(JuntaSpoolId) FROM #JuntasPND
			WHERE ClasifPND IS NULL
			
			SELECT @CantJuntasNo = COUNT(JuntaSpoolId) FROM #JuntasPND
			WHERE ClasifPND LIKE 'NO'			
		
		
			IF @CantJuntasClasifNull = @CantJuntas
				BEGIN
					SELECT @EstatusPND = @NOSE
					print '>0 nose'
					
 				END
			ELSE IF @CantJuntasNo = @CantJuntas
				BEGIN
					SELECT @EstatusPND = @NOAPLICA
					print '>0 no aplica'
				END
			ELSE
				BEGIN
				
					SELECT @CantJuntasReqPND = COUNT(JuntaSpoolId) FROM #JuntasPND
					WHERE ClasifPND IS NOT NULL AND ClasifPND <> '' AND ClasifPND <> 'No'					

					IF(@CantAlMenosUno = @CantJuntasReqPND)
						SELECT @EstatusPND = @COMPLETO 										
					ELSE
						SELECT @EstatusPND = @INCOMPPLETO									
				END
		END						

	--Select 	@EstatusPND
	--SELECT * from #JuntasPND
	DROP TABLE #JuntasPND

SET NOCOUNT OFF;

END


GO


