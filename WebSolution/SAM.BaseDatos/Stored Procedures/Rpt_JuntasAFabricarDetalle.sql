-- =============================================================
-- Author:		David Emmanuel Zúñiga Herrera
-- Create date: 27/Mayo/2013
-- Description:	Para el reporte de Detalle de Juntas  a Fabricar
-- =============================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_JuntasAFabricarDetalle]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_JuntasAFabricarDetalle]
GO

CREATE PROCEDURE [dbo].[Rpt_JuntasAFabricarDetalle]
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
		
	SELECT	CASE WHEN (ISNULL(bs.LetraBaston,'MAN')) = 'MAN' THEN 'MAN' ELSE bs.LetraBaston END [LetraBaston]
			,s.SpoolID
			,js.JuntaSpoolID
			,ots.Partida
			,ots.NumeroControl
			,s.Nombre AS [NombreSpool]
			,js.Etiqueta	
			,(js.EtiquetaMaterial1 + '-' + js.EtiquetaMaterial2) [Etiquetas]		
			,(CASE (ISNULL(bs.LetraBaston,'MAN')) 
				WHEN 'MAN' 
					THEN 
						ISNULL(
								CAST((	SELECT TOP 1 LetraBaston 
										FROM JuntaSpool js1
										INNER JOIN TipoJunta tj1 
											ON tj1.TipoJuntaID = js1.TipoJuntaID
										INNER JOIN OrdenTrabajoSpool ots1 
											ON ots1.SpoolID = js1.SpoolID
										INNER JOIN Spool s1 
											ON s1.SpoolID = js1.SpoolID
										LEFT JOIN BastonSpoolJunta bsj1 
											ON js1.JuntaSpoolID = bsj1.JuntaSpoolID
										LEFT JOIN BastonSpool bs1 
											ON bs1.BastonSpoolID = bsj1.BastonSpoolID
										INNER JOIN OrdenTrabajo ot1 
											ON ot1.OrdenTrabajoID = ots1.OrdenTrabajoID
										WHERE ot1.OrdenTrabajoID = ot.OrdenTrabajoID 
										AND js1.FabAreaID = js.FabAreaID 
										AND s1.SpoolID = s.SpoolID									
										AND ISNULL(bs1.LetraBaston,'MAN') <> 'MAN'
										AND (js1.EtiquetaMaterial1 = js.EtiquetaMaterial1 
										OR js1.EtiquetaMaterial2 = js.EtiquetaMaterial1)
										ORDER BY bs1.LetraBaston
									) 
								AS NVARCHAR(10))
					   ,js.EtiquetaMaterial1)  
				  ELSE 
					  js.EtiquetaMaterial1
			END) [EtiquetaMaterial1]
			,(CASE (ISNULL(bs.LetraBaston,'MAN')) 
				WHEN 'MAN' 
					THEN 
						ISNULL(
								CAST((	SELECT TOP 1 LetraBaston 
										FROM JuntaSpool js1
										INNER JOIN TipoJunta tj1 
											ON tj1.TipoJuntaID = js1.TipoJuntaID
										INNER JOIN OrdenTrabajoSpool ots1 
											ON ots1.SpoolID = js1.SpoolID
										INNER JOIN Spool s1 
											ON s1.SpoolID =  js1.SpoolID
										LEFT JOIN BastonSpoolJunta bsj1 
											ON js1.JuntaSpoolID = bsj1.JuntaSpoolID
										LEFT JOIN BastonSpool bs1 
											ON bs1.BastonSpoolID = bsj1.BastonSpoolID
										INNER JOIN OrdenTrabajo ot1 
											ON ot1.OrdenTrabajoID = ots1.OrdenTrabajoID
										WHERE ot1.OrdenTrabajoID = ot.OrdenTrabajoID 
										AND js1.FabAreaID = js.FabAreaID 
										AND s1.SpoolID = s.SpoolID									
										AND ISNULL(bs1.LetraBaston,'MAN') <> 'MAN'
										AND (js1.EtiquetaMaterial1 = js.EtiquetaMaterial2
										OR js1.EtiquetaMaterial2 = js.EtiquetaMaterial2)
										ORDER BY bs1.LetraBaston
									) 
								AS NVARCHAR(10))
						,js.EtiquetaMaterial2)  
					ELSE 
						js.EtiquetaMaterial2
			END) [EtiquetaMaterial2]
			,js.Diametro
			,tj.Codigo AS [CodigoJunta]
			,js.Cedula
			,(SELECT TOP 1 fa1.Nombre FROM FamiliaAcero fa1 WHERE fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID) [FamiliaAceroMaterial1]
			,(SELECT TOP 1 fa2.Nombre FROM FamiliaAcero fa2 WHERE fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID) [FamiliaAceroMaterial2]
			,(t.Nombre + e.Nombre) [Estacion]
			,CASE
				WHEN 
					(ISNULL(bs.LetraBaston,'MAN')) <> 'MAN'
				THEN	
					(ISNULL((SELECT (t1.Nombre + e1.Nombre) FROM Estacion e1 INNER JOIN BastonSpool bs1 ON
						     e1.EstacionID = bs1.EstacionID  INNER JOIN Taller t1 
						     ON t1.TallerID = bs1.TallerID WHERE bs1.SpoolID = bs.SpoolID AND  bs1.LetraBaston Like 'MAN'
						     )
						,'')
					)	
			END [Segunda]
	FROM JuntaSpool js
	INNER JOIN TipoJunta tj 
		ON tj.TipoJuntaID = js.TipoJuntaID
	INNER JOIN OrdenTrabajoSpool ots 
		ON ots.SpoolID = js.SpoolID
	INNER JOIN Spool s 
		ON s.SpoolID = js.SpoolID
	LEFT JOIN BastonSpoolJunta bsj 
		ON js.JuntaSpoolID = bsj.JuntaSpoolID
	LEFT JOIN BastonSpool bs 
		ON bs.BastonSpoolID = bsj.BastonSpoolID
	LEFT JOIN Estacion e
		ON e.EstacionID = bs.EstacionID
	LEFT JOIN Taller t
		ON t.TallerID = bs.TallerID
	INNER JOIN OrdenTrabajo ot 
		ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
	WHERE ot.OrdenTrabajoID = @OrdenTrabajoID AND js.FabAreaID = @ShopFabAreaID

		
END


/*

exec Rpt_JuntasAFabricarDetalle 35

*/