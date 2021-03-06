IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_MaterialesAFabricar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_MaterialesAFabricar
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_MaterialesAFabricar]
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
		
		
		SELECT	 ots.Partida
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
				,(SELECT c.Consecutivo FROM @Congelados c WHERE c.NumeroUnicoCongeladoID = otm.NumeroUnicoCongeladoID) [Consecutivo]
		FROM	MaterialSpool ms
		INNER JOIN ItemCode ic ON ic.ItemCodeID = ms.ItemCodeID
		INNER JOIN Spool s ON s.SpoolID = ms.SpoolID
		INNER JOIN OrdenTrabajoMaterial otm ON otm.MaterialSpoolID = ms.MaterialSpoolID
		INNER JOIN OrdenTrabajoSpool ots ON ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID
		WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
		
END

/*
	exec [Rpt_MaterialesAFabricar] 41
	
*/


GO

