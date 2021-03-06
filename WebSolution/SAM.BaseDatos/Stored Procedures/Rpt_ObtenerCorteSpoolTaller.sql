
-- ==================================================================
-- Author:		David Zúñiga
-- Create date: 19/Junio/2013
-- Description:	Para obtener informacion de lista de corte por Taller
-- ==================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerCorteSpoolTaller]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_ObtenerCorteSpoolTaller]
GO

CREATE  PROCEDURE [dbo].[Rpt_ObtenerCorteSpoolTaller]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON; 
		
		SELECT	ot.OrdenTrabajoID
			   ,ots.SpoolID
			   ,ots.NumeroControl
			   ,ic.ItemCodeID
			   ,ic.DescripcionEspanol
			   ,ic.Codigo
			   ,cs.Diametro
			   ,cs.TipoCorte1ID
			   ,cs.Cantidad
			   ,cs.TipoCorte2ID
			   ,cs.EtiquetaMaterial
			   ,cs.EtiquetaSeccion
			   ,cs.Observaciones
			   ,tc1.Nombre AS [TipoCorteNombre1]
			   ,tc2.Nombre AS [TipoCorteNombre2]
			   ,s.Nombre AS NombreSpool
			   ,(CASE 
					WHEN cs.EtiquetaMaterial <> '' 
					THEN 
						ISNULL(								
							(	SELECT TOP 1 ISNULL(t1.Nombre,'')
								FROM JuntaSpool js1 
								INNER JOIN BastonSpoolJunta  bsj1 
									ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
								INNER JOIN BastonSpool bs1 
									ON bs1.BastonSpoolID = bsj1.BastonSpoolID
								INNER JOIN Taller t1 
									ON t1.TallerID = bs1.TallerID
								WHERE ((	CASE LEN(cs.EtiquetaMaterial) 
												WHEN '1' 
													THEN '0' + cs.EtiquetaMaterial 
													ELSE cs.EtiquetaMaterial
													END
										) 
										= 
										(	SELECT TOP 1 CASE  LEN(js1.EtiquetaMaterial1) 
															WHEN '1' 
																THEN '0' + js1.EtiquetaMaterial1 
																ELSE js1.EtiquetaMaterial1 
																END
										)
									OR (	CASE LEN(cs.EtiquetaMaterial) 
												WHEN '1'
													THEN '0' +  cs.EtiquetaMaterial 
													ELSE cs.EtiquetaMaterial 
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
						,ISNULL(								
							(	
								SELECT TOP 1 ISNULL(t1.Nombre,'')
								FROM JuntaSpool js1 
								INNER JOIN BastonSpoolJunta  bsj1 
									ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
								INNER JOIN BastonSpool bs1 
									ON bs1.BastonSpoolID = bsj1.BastonSpoolID
								INNER JOIN Taller t1 
									ON t1.TallerID = bs1.TallerID
								WHERE js1.SpoolID = s.SpoolID 
								AND js1.EsManual = 1
							)								
						,'')) 						
				    END 
		    ) [Taller]
	   FROM ItemCode ic
	   INNER JOIN CorteSpool cs ON cs.ItemCodeID = ic.ItemCodeID
	   INNER JOIN Spool s ON s.SpoolID = cs.SpoolID
	   INNER JOIN TipoCorte tc1 ON tc1.TipoCorteID = cs.TipoCorte1ID
	   INNER JOIN TipoCorte tc2 ON tc2.TipoCorteID = cs.TipoCorte2ID
	   INNER JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID
	   INNER JOIN OrdenTrabajo ot ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
	   WHERE ot.OrdenTrabajoID = @OrdenTrabajoID
END