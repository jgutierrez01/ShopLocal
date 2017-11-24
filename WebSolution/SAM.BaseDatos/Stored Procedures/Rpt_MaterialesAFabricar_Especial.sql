IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_MaterialesAFabricar_Especial]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Rpt_MaterialesAFabricar_Especial]
GO

/****** Object:  StoredProcedure [dbo].[Rpt_MaterialesAFabricarEspecial]    Script Date: 4/4/2014 1:57:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_MaterialesAFabricar_Especial]
(
	@OrdenTrabajoEspecialID int
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
				from OrdenTrabajoSpool ot INNER JOIN OrdenTrabajoEspecialSpool otes
				ON ot.SpoolID = otes.SpoolID
				INNER JOIN OrdenTrabajoEspecial ote 
				ON ote.OrdenTrabajoEspecialID = otes.OrdenTrabajoEspecialID
				where ote.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID
			)
			and otm.CongeladoEsEquivalente = 1
		) t
		
		
		SELECT	 ots.Partida
				,ots.OrdenTrabajoEspecialSpoolID
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
				,(SELECT c.Consecutivo FROM @Congelados c 
					WHERE c.NumeroUnicoCongeladoID = otm.NumeroUnicoCongeladoID) [Consecutivo]
				,(SELECT CASE WHEN (otm.TieneDespacho = NULL OR otm.TieneDespacho = '') 
					THEN '0' ELSE '1' END ) as FueDespachado
		FROM	MaterialSpool ms
		INNER JOIN ItemCode ic ON ic.ItemCodeID = ms.ItemCodeID
		INNER JOIN Spool s ON s.SpoolID = ms.SpoolID
		INNER JOIN OrdenTrabajoMaterial otm ON otm.MaterialSpoolID = ms.MaterialSpoolID
		INNER JOIN OrdenTrabajoEspecialSpool ots ON ots.SpoolID = s.SpoolID
		WHERE ots.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID
		
END

/*
	exec [Rpt_MaterialesAFabricar] 41
	
*/



GO


