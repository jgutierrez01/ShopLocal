IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerResumenMateriales]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerResumenMateriales
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 12/Octubre/2010
-- Description:	Para obtener informacion de resumen de materiales
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerResumenMateriales]
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
	FROM	MaterialSpool ms
	INNER JOIN ItemCode ic on ic.ItemCodeID = ms.ItemCodeID
	INNER JOIN OrdenTrabajoMaterial otm on otm.MaterialSpoolID = ms.MaterialSpoolID
	INNER JOIN OrdenTrabajoSpool ots ON ots.OrdenTrabajoSpoolID = otm.OrdenTrabajoSpoolID
	WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
	
	
END

/*
	exec [Rpt_ObtenerResumenMateriales] 41
	update OrdenTrabajoMaterial set CongeladoEsEquivalente = 1 where OrdenTrabajoMaterialID in (3580,3581,3554)
	--15955
	update OrdenTrabajoMaterial set CongeladoEsEquivalente = 1, NumeroUnicoCongeladoID = 21765 where OrdenTrabajoMaterialID in (3551)
*/


GO

