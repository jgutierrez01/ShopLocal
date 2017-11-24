IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerIcEquivalentesPorOdt]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerIcEquivalentesPorOdt
GO
-- =============================================
-- Author:		Ivan Hernandez
-- Create date: 11/Nov/2010
-- Description:	Obtener los I.C. equivalentes en caso que haya que sugerirlos en la ODT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerIcEquivalentesPorOdt]
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

	SELECT	c.NumeroUnicoCongeladoID,
			c.Consecutivo,
			nu.Diametro1,
			nu.Diametro2,
			ic.Codigo,
			ic.DescripcionEspanol,
			ic.DescripcionIngles,
			ic.ItemCodeID
	FROM	@Congelados c
	INNER JOIN NumeroUnico nu on c.NumeroUnicoCongeladoID = nu.NumeroUnicoID
	INNER JOIN ItemCode ic on ic.ItemCodeID = nu.ItemCodeID
	
	
END

/*
	exec [Rpt_ObtenerIcEquivalentesPorOdt] 41
*/

GO

