USE [SAM]
GO
/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerResumenMaterialesTaller]    Script Date: 06/17/2013 15:25:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ===========================================================================
-- Author:		David Emmanuel Zúñiga Herrera
-- Create date: 02/Junio/2013
-- Description:	Para el reporte de Resumen de Detalle de Materiales por Taller
-- ===========================================================================
ALTER PROCEDURE [dbo].[Rpt_ObtenerResumenMaterialesTaller]
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

	SELECT	ic.Codigo
		   ,ic.DescripcionEspanol
		   ,ic.TipoMaterialID
		   ,ms.Diametro1
		   ,ms.Diametro2
		   ,ms.Grupo
		   ,ms.Cantidad
		   ,otm.OrdenTrabajoMaterialID
		   ,otm.CongeladoEsEquivalente
		   ,otm.NumeroUnicoCongeladoID
		   ,otm.NumeroUnicoSugeridoID
		   ,otm.SegmentoSugerido		  
		   ,(SELECT c.Consecutivo FROM @Congelados c WHERE c.NumeroUnicoCongeladoID = otm.NumeroUnicoCongeladoID) [Consecutivo]
		   ,(CASE 
				WHEN ms.Etiqueta <> '' 
					THEN 
						ISNULL(								
							(	SELECT TOP 1 ISNULL(t1.Nombre,'')
								FROM JuntaSpool js1 
								INNER JOIN BastonSpoolJunta  bsj1 
									ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
								INNER JOIN BastonSpool bs1 
									ON bs1.BastonSpoolID = bsj1.BastonSpoolID
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
						,ISNULL(								
							(	
								SELECT TOP 1 ISNULL(t1.Nombre,'')
								FROM JuntaSpool js1 
								INNER JOIN BastonSpoolJunta  bsj1 
									ON bsj1.JuntaSpoolID = js1.JuntaSpoolID 
								INNER JOIN BastonSpool bs1 
									ON bs1.BastonSpoolID = bsj1.BastonSpoolID
								LEFT JOIN Taller t1 
									ON t1.TallerID = bs1.TallerID
								WHERE js1.SpoolID = s.SpoolID 
								AND js1.EsManual = 1
							)								
						,'')) 						
				    END 
		    ) [Taller]
	FROM	MaterialSpool ms
	INNER JOIN ItemCode ic on ic.ItemCodeID = ms.ItemCodeID
	INNER JOIN Spool s ON s.SpoolID = ms.SpoolID
	INNER JOIN OrdenTrabajoMaterial otm on otm.MaterialSpoolID = ms.MaterialSpoolID
	INNER JOIN OrdenTrabajoSpool ots ON ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID
	WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
	
	
END
