IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerOdtInfoConEstado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerOdtInfoConEstado
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para el header de los reportes de ODT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerOdtInfoConEstado]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
		SELECT ot.OrdenTrabajoID
		,ot.FechaOrden
		,ot.NumeroOrden
		,t.Nombre AS [TallerNombre]
		,eo.Nombre AS [EstatusNombre]
		,COUNT(DISTINCT ots.OrdenTrabajoSpoolID) as [TotalSpools]
		FROM
		OrdenTrabajo ot
		INNER JOIN OrdenTrabajoSpool ots
		ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
		INNER JOIN Taller t
		ON t.TallerID = ot.TallerID
		INNER JOIN EstatusOrden eo
		ON eo.EstatusOrdenID = ot.EstatusOrdenID
		WHERE ot.OrdenTrabajoID = @OrdenTrabajoID
		GROUP BY
			ot.OrdenTrabajoID
			,ot.FechaOrden
			,ot.NumeroOrden
			,t.Nombre
			,eo.Nombre

END
GO

