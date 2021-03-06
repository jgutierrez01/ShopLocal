USE [SAM]
GO
/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerSpoolsCaratulaDetalladaEstacion]    Script Date: 06/27/2013 15:23:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Rpt_ObtenerSpoolsCaratulaDetalladaEstacion]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @ShopFabAreaID INT
	
	SELECT @ShopFabAreaID = fa.FabAreaID
	FROM FabArea fa
	WHERE fa.Codigo = 'SHOP'
		
	DECLARE @bastones table
	(
		Peso DECIMAL(7,2),
		Baston VARCHAR(30),
		SpoolID INT
	);

	INSERT INTO @bastones

	SELECT (	SELECT ISNULL(SUM(ms1.Peso),0) FROM MaterialSPool ms1 
				WHERE 
					(	CASE LEN(ms1.Etiqueta) 
						WHEN '1' 
							THEN '0' + ms1.Etiqueta 
							ELSE ms1.Etiqueta 
							END
					) 
					IN			
					(	SELECT CASE  LEN(js1.EtiquetaMaterial1) 
								WHEN '1' 
									THEN '0' + js1.EtiquetaMaterial1 
									ELSE js1.EtiquetaMaterial1 
									END 
						FROM	OrdenTrabajoSpool ots1
						INNER JOIN Spool s1
							ON ots1.SpoolID = s1.SpoolID
						INNER JOIN JuntaSpool js1 
							ON js1.SpoolID = s1.SpoolID
						INNER JOIN BastonSpoolJunta bsj1 
							ON js1.JuntaSpoolID = bsj1.JuntaSpoolID
						INNER JOIN BastonSpool bs1 
							ON bs1.BastonSpoolID = bsj1.BastonSpoolID
						WHERE ots1.OrdenTrabajoID = @OrdenTrabajoID 
						AND ISNULL(bs.LetraBaston,'MAN') = ISNULL(bs1.LetraBaston,'MAN')
						AND s1.SpoolID = s.SpoolID
						
						UNION
	
						SELECT CASE  LEN(js1.EtiquetaMaterial2)  
								WHEN '1' 
									THEN '0' + js1.EtiquetaMaterial2
									ELSE js1.EtiquetaMaterial2
									END 
						FROM	OrdenTrabajoSpool ots1
						INNER JOIN Spool s1 
							ON ots1.SpoolID = s1.SpoolID
						INNER JOIN JuntaSpool js1 
							ON js1.SpoolID = s1.SpoolID
						INNER JOIN BastonSpoolJunta bsj1 
							ON js1.JuntaSpoolID = bsj1.JuntaSpoolID
						INNER JOIN BastonSpool bs1 
							ON bs1.BastonSpoolID = bsj1.BastonSpoolID
						WHERE ots1.OrdenTrabajoID = @OrdenTrabajoID 
						AND ISNULL(bs.LetraBaston,'MAN') = ISNULL(bs1.LetraBaston,'MAN')
						AND s1.SpoolID = s.SpoolID)
					    AND ms1.SpoolID = s.SpoolID
			 )
			 ,ISNULL(bs.LetraBaston,'MAN')			
			 ,s.SpoolID

	FROM	OrdenTrabajoSpool ots
	INNER JOIN Spool s 
		ON ots.SpoolID = s.SpoolID
	INNER JOIN JuntaSpool js 
		ON js.SpoolID = s.SpoolID
	LEFT JOIN BastonSpoolJunta bsj 
		ON js.JuntaSpoolID = bsj.JuntaSpoolID
	LEFT JOIN BastonSpool bs 
		ON bs.BastonSpoolID = bsj.BastonSpoolID
	LEFT JOIN Estacion e 
		ON e.EstacionID = bs.EstacionID 
	WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
	GROUP BY s.SpoolID, bs.LetraBaston
		
	SELECT  ots.NumeroControl
			,ots.Partida
			,ots.OrdenTrabajoSpoolID
			,s.SpoolID
			,s.Nombre
			,s.Dibujo
			,s.Area
			,s.Especificacion
			,s.Pdis
			,s.Peso
			,s.RevisionCliente
			,s.Revision
			,ISNULL(
					(	SELECT SUM(ISNULL(js1.Diametro,0)) 
						FROM OrdenTrabajoJunta ot1
						INNER JOIN JuntaSpool js1 
							ON ot1.JuntaSpoolID = js1.JuntaSpoolID
						LEFT JOIN BastonSpoolJunta bsj1 
							ON js1.JuntaSpoolID = bsj1.JuntaSpoolID
						LEFT JOIN BastonSpool bs1 
							ON bs1.BastonSpoolID = bsj1.BastonSpoolID
						WHERE ot1.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
						AND ISNULL(bs.LetraBaston,'MAN') = ISNULL(bs1.LetraBaston,'MAN')													
						AND js1.FabAreaID = @ShopFabAreaID			
					)
			 ,0) [Diametro]
			,CASE WHEN (ISNULL(bs.LetraBaston,'M')) = 'M' THEN 'MAN' ELSE bs.LetraBaston END [LetraBaston]
			,ISNULL(
					(	SELECT SUM(ISNULL(js1.Peqs,0))
						FROM OrdenTrabajoJunta ot1
						INNER JOIN JuntaSpool js1 
							ON ot1.JuntaSpoolID = js1.JuntaSpoolID
						LEFT JOIN BastonSpoolJunta bsj1 
							ON js1.JuntaSpoolID = bsj1.JuntaSpoolID
						LEFT JOIN BastonSpool bs1 
							ON bs1.BastonSpoolID = bsj1.BastonSpoolID
						WHERE ot1.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
						AND ISNULL(bs.LetraBaston,'MAN') = ISNULL(bs1.LetraBaston,'MAN')													
						AND js1.FabAreaID = @ShopFabAreaID													
					)
			 ,0) [Peqs]	
			,(ISNULL((SELECT t1.Nombre FROM Taller t1 WHERE t1.TallerID = bs.TallerID),'')
			  +
			  ISNULL((SELECT e1.Nombre FROM Estacion e1 WHERE e1.EstacionID = bs.EstacionID),'')			
			 ) [Estacion]
			,(	SELECT	SUM(ISNULL(js.Peqs,0))
				FROM OrdenTrabajoJunta otj
				INNER JOIN JuntaSpool js 
					ON otj.JuntaSpoolID = js.JuntaSpoolID
				WHERE	otj.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
				AND js.FabAreaID = @ShopFabAreaID
			 ) [PeqsHead]
			,(	SELECT	
					CASE	
						WHEN COUNT(odtm.CongeladoEsEquivalente) > 0 THEN CAST (1 as bit)
								ELSE CAST(0 as bit) 
						END
				FROM	OrdenTrabajoMaterial odtm
				WHERE	odtm.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
						AND odtm.CongeladoEsEquivalente = 1
			 ) [TieneEquivalencia]
			 ,(CASE WHEN (ISNULL(bs.LetraBaston,'MAN')) = 'MAN' 
					THEN 
						CASE 
							WHEN (	s.Peso - (	
												SELECT  ISNULL(SUM(b.Peso),0) 
												FROM @bastones b 
												WHERE  ISNULL(b.Baston,'MAN') <> 'MAN'
												AND b.SpoolID = s.SpoolID
											  )
								  ) < 0 
							THEN '0' 
							ELSE 
								s.Peso - (	
											SELECT  ISNULL(SUM(b.Peso),0) 
											FROM @bastones b 
											WHERE  ISNULL(b.Baston,'MAN') <> 'MAN'
											AND b.SpoolID = s.SpoolID
										  )
						END
				    ELSE 
						(	
							SELECT  b.Peso 
							FROM @bastones b 
							WHERE  b.Baston = bs.LetraBaston
							AND b.SpoolID = s.SpoolID
						) 
					END 
			 ) [Kilogramos]				
	FROM	OrdenTrabajoSpool ots
	INNER JOIN Spool s 
		ON ots.SpoolID = s.SpoolID
	INNER JOIN JuntaSpool js 
		ON js.SpoolID = s.SpoolID
	LEFT JOIN BastonSpoolJunta bsj 
		ON js.JuntaSpoolID = bsj.JuntaSpoolID
	LEFT JOIN BastonSpool bs 
		ON bs.BastonSpoolID = bsj.BastonSpoolID
	LEFT JOIN Estacion e 
		ON e.EstacionID = bs.EstacionID 
	WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
	and js.FabAreaID = @ShopFabAreaID
	GROUP BY ots.NumeroControl, ots.Partida, ots.OrdenTrabajoSpoolID, s.SpoolID, s.Nombre, s.Dibujo,
			s.Area, s.Especificacion, s.Pdis, s.Peso, s.RevisionCliente, s.Revision, bs.LetraBaston, 
			js.EstacionID,  e.Nombre, js.Esmanual, e.EstacionID, bs.BastonSpoolID, ots.OrdenTrabajoID, 
			bs.TallerID, bs.EstacionID
	ORDER BY ots.NumeroControl, LetraBaston	
END


