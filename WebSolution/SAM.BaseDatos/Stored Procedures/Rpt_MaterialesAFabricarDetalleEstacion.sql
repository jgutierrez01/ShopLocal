
-- ==========================================================================
-- Author:		David Emmanuel Zúñiga Herrera
-- Create date: 14/Junio/2013
-- Description:	Para el reporte de Detalles de Materiales por Estación de ODT
-- ==========================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_MaterialesAFabricarDetalleEstacion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_MaterialesAFabricarDetalleEstacion]
GO

CREATE PROCEDURE [dbo].[Rpt_MaterialesAFabricarDetalleEstacion]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON; 

		declare @Congelados table
		(
			NumeroUnicoCongeladoID int,
			Consecutivo int
		);
		
		insert into @Congelados
		select	t.NumeroUnicoCongeladoID,
				row_number() over(order by t.NumeroUnicoCongeladoID)
		from
		(
			select distinct otm.NumeroUnicoCongeladoID
			from OrdenTrabajoMaterial otm
			where otm.OrdenTrabajoSpoolID in
			(
				select OrdenTrabajoSpoolID
				from OrdenTrabajoSpool
				where OrdenTrabajoID = @OrdenTrabajoID
			)
			and otm.CongeladoEsEquivalente = 1
		) t		
				
				SELECT  ots.Partida
				,ots.OrdenTrabajoSpoolID
				,ots.NumeroControl
				,s.Nombre
				,ms.Etiqueta
				,ms.Diametro1
				,ms.Diametro2
				,ms.Cantidad
				,ic.Codigo
				,ic.DescripcionEspanol
				,ic.TipoMaterialID
				,otm.OrdenTrabajoMaterialID
				,otm.CongeladoEsEquivalente
				,otm.NumeroUnicoCongeladoID
				,otm.NumeroUnicoSugeridoID
				,otm.SegmentoSugerido
				,s.SpoolID	
				,(SELECT c.Consecutivo FROM @Congelados c WHERE c.NumeroUnicoCongeladoID = otm.NumeroUnicoCongeladoID) [Consecutivo]		
				,CASE 
					WHEN ms.Etiqueta <> '' 
						THEN 
							ISNULL(CAST((	SELECT TOP 1 bs1.LetraBaston 
											FROM JuntaSpool js1 
											INNER JOIN BastonSpoolJunta  bsj1 
												ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
											INNER JOIN BastonSpool bs1 
												ON bs1.BastonSpoolID = bsj1.BastonSpoolID
											WHERE ((	CASE LEN(ms.Etiqueta) 
															WHEN '1' 
																THEN '0' + ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial1 
																			ELSE js1.EtiquetaMaterial1 
																			END
													)
												OR (	CASE LEN(ms.Etiqueta) 
															WHEN '1'
																THEN '0' +  ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial2 
																			ELSE js1.EtiquetaMaterial2 
																			END
												   ))
											AND js1.SpoolID = s.SpoolID 
											AND js1.EsManual = 0 
										)
								  AS NVARCHAR(50))
							,'MAN')
						ELSE 
							'MAN'
				END [LetraBaston]
				,CASE 
					WHEN ms.Etiqueta <> '' 
						THEN 
							ISNULL((	SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
										FROM JuntaSpool js1 
										INNER JOIN BastonSpoolJunta  bsj1 
											ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
										INNER JOIN BastonSpool bs1 
											ON bs1.BastonSpoolID = bsj1.BastonSpoolID
										LEFT JOIN Estacion e1 
											ON e1.EstacionID = bs1.EstacionID
										LEFT JOIN Taller t1
											ON t1.TallerID = bs1.TallerID
										WHERE ((	CASE LEN(ms.Etiqueta) 
														WHEN '1' 
															THEN '0' + ms.Etiqueta 
															ELSE ms.Etiqueta 
															END
												) 
												= 
												(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
																	WHEN '1' 
																		THEN '0' + js1.EtiquetaMaterial1 
																		ELSE js1.EtiquetaMaterial1 
																		END
												)
											OR (	CASE LEN(ms.Etiqueta) 
														WHEN '1'
															THEN '0' +  ms.Etiqueta 
															ELSE ms.Etiqueta 
															END
												) 
												= 
												(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
																	WHEN '1' 
																		THEN '0' + js1.EtiquetaMaterial2 
																		ELSE js1.EtiquetaMaterial2 
																		END
												  ))
										AND js1.SpoolID = s.SpoolID 
										AND js1.EsManual = 0
									)										
							,ISNULL((	
										SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
										FROM JuntaSpool js1 
										INNER JOIN BastonSpoolJunta  bsj1 
											ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
										INNER JOIN BastonSpool bs1 
											ON bs1.BastonSpoolID = bsj1.BastonSpoolID
										LEFT JOIN Estacion e1 
											ON e1.EstacionID = bs1.EstacionID
										LEFT JOIN Taller t1
											ON t1.TallerID = bs1.TallerID
										WHERE js1.SpoolID = s.SpoolID 
										AND js1.EsManual = 1								
									)										
							,'')) 						
					END [Estacion]
				,CASE
					WHEN 
						(ISNULL  (CAST((	SELECT TOP 1 bs1.LetraBaston 
											FROM JuntaSpool js1 
											INNER JOIN BastonSpoolJunta  bsj1 
												ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
											INNER JOIN BastonSpool bs1 
												ON bs1.BastonSpoolID = bsj1.BastonSpoolID
											WHERE ((	CASE LEN(ms.Etiqueta) 
															WHEN '1' 
																THEN '0' + ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial1 
																			ELSE js1.EtiquetaMaterial1 
																			END
													)
												OR (	CASE LEN(ms.Etiqueta) 
															WHEN '1'
																THEN '0' +  ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial2 
																			ELSE js1.EtiquetaMaterial2 
																			END
												   ))
											AND js1.SpoolID = s.SpoolID 
											AND js1.EsManual = 0 
										)
									AS NVARCHAR(50))
						,'MAN') <> 'MAN')
					THEN
						ISNULL((	SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
									FROM JuntaSpool js1 
									INNER JOIN BastonSpoolJunta  bsj1 
										ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
									INNER JOIN BastonSpool bs1 
										ON bs1.BastonSpoolID = bsj1.BastonSpoolID
									LEFT JOIN Estacion e1 
										ON e1.EstacionID = bs1.EstacionID
									LEFT JOIN Taller t1
										ON t1.TallerID = bs1.TallerID
									WHERE js1.SpoolID = s.SpoolID 
									AND js1.EsManual = 1
								
								)									
						,'')
				END	[Segunda]	
				,CASE 
					WHEN ms.Etiqueta <> '' 
						THEN 
							ISNULL((	SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
										FROM JuntaSpool js1 
										INNER JOIN BastonSpoolJunta  bsj1 
											ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
										INNER JOIN BastonSpool bs1 
											ON bs1.BastonSpoolID = bsj1.BastonSpoolID
										LEFT JOIN Estacion e1 
											ON e1.EstacionID = bs1.EstacionID
										LEFT JOIN Taller t1
											ON t1.TallerID = bs1.TallerID
										WHERE ((	CASE LEN(ms.Etiqueta) 
														WHEN '1' 
															THEN '0' + ms.Etiqueta 
															ELSE ms.Etiqueta 
															END
												) 
												= 
												(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
																	WHEN '1' 
																		THEN '0' + js1.EtiquetaMaterial1 
																		ELSE js1.EtiquetaMaterial1 
																		END
												)
											OR (	CASE LEN(ms.Etiqueta) 
														WHEN '1'
															THEN '0' +  ms.Etiqueta 
															ELSE ms.Etiqueta 
															END
												) 
												= 
												(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
																	WHEN '1' 
																		THEN '0' + js1.EtiquetaMaterial2 
																		ELSE js1.EtiquetaMaterial2 
																		END
												  ))
										AND js1.SpoolID = s.SpoolID 
										AND js1.EsManual = 0
									)										
							,ISNULL((	
										SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
										FROM JuntaSpool js1 
										INNER JOIN BastonSpoolJunta  bsj1 
											ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
										INNER JOIN BastonSpool bs1 
											ON bs1.BastonSpoolID = bsj1.BastonSpoolID
										LEFT JOIN Estacion e1 
											ON e1.EstacionID = bs1.EstacionID
										LEFT JOIN Taller t1
											ON t1.TallerID = bs1.TallerID
										WHERE js1.SpoolID = s.SpoolID 
										AND js1.EsManual = 1
								
									)										
							,'')) 						
				END[EstacionFinal]			
		FROM	MaterialSpool ms
		INNER JOIN ItemCode ic 
			ON ic.ItemCodeID = ms.ItemCodeID
		INNER JOIN Spool s 
			ON s.SpoolID = ms.SpoolID
		INNER JOIN OrdenTrabajoMaterial otm 
			ON otm.MaterialSpoolID = ms.MaterialSpoolID
		INNER JOIN OrdenTrabajoSpool ots 
			ON ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID
		WHERE ots.OrdenTrabajoID = @OrdenTrabajoID			
		
		UNION ALL		
		
		SELECT  ots.Partida
				,ots.OrdenTrabajoSpoolID
				,ots.NumeroControl
				,s.Nombre
				,ms.Etiqueta
				,ms.Diametro1
				,ms.Diametro2
				,ms.Cantidad
				,ic.Codigo
				,ic.DescripcionEspanol
				,ic.TipoMaterialID
				,otm.OrdenTrabajoMaterialID
				,otm.CongeladoEsEquivalente
				,otm.NumeroUnicoCongeladoID
				,otm.NumeroUnicoSugeridoID
				,otm.SegmentoSugerido
				,s.SpoolID	
				,(SELECT c.Consecutivo FROM @Congelados c WHERE c.NumeroUnicoCongeladoID = otm.NumeroUnicoCongeladoID) [Consecutivo]		
				,CASE 
					WHEN ms.Etiqueta <> '' 
						THEN 
							ISNULL(CAST((	SELECT TOP 1 bs1.LetraBaston 
											FROM JuntaSpool js1 
											INNER JOIN BastonSpoolJunta  bsj1 
												ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
											INNER JOIN BastonSpool bs1 
												ON bs1.BastonSpoolID = bsj1.BastonSpoolID
											WHERE ((	CASE LEN(ms.Etiqueta) 
															WHEN '1' 
																THEN '0' + ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial1 
																			ELSE js1.EtiquetaMaterial1 
																			END
													)
												OR (	CASE LEN(ms.Etiqueta) 
															WHEN '1'
																THEN '0' +  ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial2 
																			ELSE js1.EtiquetaMaterial2 
																			END
												   ))
											AND js1.SpoolID = s.SpoolID 
											AND js1.EsManual = 0 
										)
								  AS NVARCHAR(50))
							,'MAN')
						ELSE 
							'MAN'
				END [LetraBaston]
				,CASE 
					WHEN ms.Etiqueta <> '' 
						THEN 
							ISNULL((	SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
										FROM JuntaSpool js1 
										INNER JOIN BastonSpoolJunta  bsj1 
											ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
										INNER JOIN BastonSpool bs1 
											ON bs1.BastonSpoolID = bsj1.BastonSpoolID
										LEFT JOIN Estacion e1 
											ON e1.EstacionID = bs1.EstacionID
										LEFT JOIN Taller t1
											ON t1.TallerID = bs1.TallerID
										WHERE ((	CASE LEN(ms.Etiqueta) 
														WHEN '1' 
															THEN '0' + ms.Etiqueta 
															ELSE ms.Etiqueta 
															END
												) 
												= 
												(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
																	WHEN '1' 
																		THEN '0' + js1.EtiquetaMaterial1 
																		ELSE js1.EtiquetaMaterial1 
																		END
												)
											OR (	CASE LEN(ms.Etiqueta) 
														WHEN '1'
															THEN '0' +  ms.Etiqueta 
															ELSE ms.Etiqueta 
															END
												) 
												= 
												(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
																	WHEN '1' 
																		THEN '0' + js1.EtiquetaMaterial2 
																		ELSE js1.EtiquetaMaterial2 
																		END
												  ))
										AND js1.SpoolID = s.SpoolID 
										AND js1.EsManual = 0
									)										
							,ISNULL((	
										SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
										FROM JuntaSpool js1 
										INNER JOIN BastonSpoolJunta  bsj1 
											ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
										INNER JOIN BastonSpool bs1 
											ON bs1.BastonSpoolID = bsj1.BastonSpoolID
										LEFT JOIN Estacion e1 
											ON e1.EstacionID = bs1.EstacionID
										LEFT JOIN Taller t1
											ON t1.TallerID = bs1.TallerID
										WHERE js1.SpoolID = s.SpoolID 
										AND js1.EsManual = 1
								
									)										
							,'')) 						
					END [Estacion]
				,CASE
					WHEN 
						(ISNULL  (CAST((	SELECT TOP 1 bs1.LetraBaston 
											FROM JuntaSpool js1 
											INNER JOIN BastonSpoolJunta  bsj1 
												ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
											INNER JOIN BastonSpool bs1 
												ON bs1.BastonSpoolID = bsj1.BastonSpoolID
											WHERE ((	CASE LEN(ms.Etiqueta) 
															WHEN '1' 
																THEN '0' + ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial1 
																			ELSE js1.EtiquetaMaterial1 
																			END
													)
												OR (	CASE LEN(ms.Etiqueta) 
															WHEN '1'
																THEN '0' +  ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial2 
																			ELSE js1.EtiquetaMaterial2 
																			END
												   ))
											AND js1.SpoolID = s.SpoolID 
											AND js1.EsManual = 0 
										)
									AS NVARCHAR(50))
						,'MAN') <> 'MAN')
					THEN
						ISNULL((	SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
									FROM JuntaSpool js1 
									INNER JOIN BastonSpoolJunta  bsj1 
										ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
									INNER JOIN BastonSpool bs1 
										ON bs1.BastonSpoolID = bsj1.BastonSpoolID
									LEFT JOIN Estacion e1 
										ON e1.EstacionID = bs1.EstacionID
									LEFT JOIN Taller t1
										ON t1.TallerID = bs1.TallerID
									WHERE js1.SpoolID = s.SpoolID 
									AND js1.EsManual = 1								
								)									
						,'')
				END	[Segunda]
				,CASE
					WHEN 
						(ISNULL  (CAST((	SELECT TOP 1 bs1.LetraBaston 
											FROM JuntaSpool js1 
											INNER JOIN BastonSpoolJunta  bsj1 
												ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
											INNER JOIN BastonSpool bs1 
												ON bs1.BastonSpoolID = bsj1.BastonSpoolID
											WHERE ((	CASE LEN(ms.Etiqueta) 
															WHEN '1' 
																THEN '0' + ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial1 
																			ELSE js1.EtiquetaMaterial1 
																			END
													)
												OR (	CASE LEN(ms.Etiqueta) 
															WHEN '1'
																THEN '0' +  ms.Etiqueta 
																ELSE ms.Etiqueta 
																END
													) 
													= 
													(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
																		WHEN '1' 
																			THEN '0' + js1.EtiquetaMaterial2 
																			ELSE js1.EtiquetaMaterial2 
																			END
												   ))
											AND js1.SpoolID = s.SpoolID 
											AND js1.EsManual = 0 
										)
									AS NVARCHAR(50))
						,'MAN') <> 'MAN')
					THEN
						ISNULL((	SELECT TOP 1 (ISNULL(t1.Nombre,'') + ISNULL(e1.Nombre,''))
									FROM JuntaSpool js1 
									INNER JOIN BastonSpoolJunta  bsj1 
										ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
									INNER JOIN BastonSpool bs1 
										ON bs1.BastonSpoolID = bsj1.BastonSpoolID
									LEFT JOIN Estacion e1 
										ON e1.EstacionID = bs1.EstacionID
									LEFT JOIN Taller t1
										ON t1.TallerID = bs1.TallerID
									WHERE js1.SpoolID = s.SpoolID 
									AND js1.EsManual = 1								
								)									
						,'')
				END	[EstacionFinal]			
		FROM	MaterialSpool ms
		INNER JOIN ItemCode ic 
			ON ic.ItemCodeID = ms.ItemCodeID
		INNER JOIN Spool s 
			ON s.SpoolID = ms.SpoolID
		INNER JOIN OrdenTrabajoMaterial otm 
			ON otm.MaterialSpoolID = ms.MaterialSpoolID
		INNER JOIN OrdenTrabajoSpool ots 
			ON ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID
		WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
		AND
		(	SELECT TOP 1 bs1.LetraBaston 
			FROM JuntaSpool js1 
			INNER JOIN BastonSpoolJunta  bsj1 
				ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
			INNER JOIN BastonSpool bs1 
				ON bs1.BastonSpoolID = bsj1.BastonSpoolID
			WHERE ((	CASE LEN(ms.Etiqueta) 
							WHEN '1' 
								THEN '0' + ms.Etiqueta 
								ELSE ms.Etiqueta 
								END
					) 
					= 
					(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
										WHEN '1' 
											THEN '0' + js1.EtiquetaMaterial1 
											ELSE js1.EtiquetaMaterial1 
											END
					)
			OR (	CASE LEN(ms.Etiqueta) 
						WHEN '1'
							THEN '0' +  ms.Etiqueta 
							ELSE ms.Etiqueta 
							END
				) 
				= 
				(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial2) 
									WHEN '1' 
										THEN '0' + js1.EtiquetaMaterial2 
										ELSE js1.EtiquetaMaterial2 
										END
			))
			AND js1.SpoolID = s.SpoolID 
			AND js1.EsManual = 0) <> 'MAN'
END

