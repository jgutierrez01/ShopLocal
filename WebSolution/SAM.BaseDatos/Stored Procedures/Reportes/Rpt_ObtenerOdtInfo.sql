IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerOdtInfo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerOdtInfo
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para el header de los reportes de ODT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerOdtInfo]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
		SELECT
		  p.Nombre AS [ProyectoNombre]
		  ,ot.OrdenTrabajoID
		  ,ot.NumeroOrden
		  ,ot.FechaOrden
		  ,u.UserId
		  ,u.Nombre AS [Nombre]
		  ,u.ApPaterno
		  ,t.Nombre AS [TallerNombre]
		  ,t.TallerID AS [TallerID]
		  ,COUNT(DISTINCT ots.OrdenTrabajoSpoolID) AS [TotalSpools]
		FROM
		  OrdenTrabajo ot
		  INNER JOIN OrdenTrabajoSpool ots
			ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
		  INNER JOIN Taller t
			ON t.TallerID = ot.TallerID
		  INNER JOIN Proyecto p
			ON ot.ProyectoID = p.ProyectoID
		  INNER JOIN Usuario u
			ON ot.UsuarioModifica = u.UserId
		WHERE
		  ot.OrdenTrabajoID = @OrdenTrabajoID
	    GROUP BY
			p.Nombre
			,ot.OrdenTrabajoID
			,ot.NumeroOrden
			,ot.FechaOrden
			,u.UserId
			,u.Nombre
			,u.ApPaterno
			,t.Nombre
		    ,t.TallerID

END
GO

