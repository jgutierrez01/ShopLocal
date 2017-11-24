IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerProyectoCaratulaOdt]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerProyectoCaratulaOdt
GO
-- =============================================
-- Author:		Ivan Hernandez Marchand
-- Create date: 08/Octubre/2010
-- Description:	Para el reporte de carátula de ODT
-- =============================================
CREATE PROCEDURE Rpt_ObtenerProyectoCaratulaOdt
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT	odt.NumeroOrden,
			odt.FechaOrden,
			(SELECT TOP 1 Nombre FROM Taller t WHERE t.TallerID = odt.TallerID) [Taller],
			(SELECT TOP 1 Nombre FROM Proyecto proy WHERE proy.ProyectoID = odt.ProyectoID) [Proyecto]
	FROM	OrdenTrabajo odt
	WHERE	odt.OrdenTrabajoID = @OrdenTrabajoID

END
GO

