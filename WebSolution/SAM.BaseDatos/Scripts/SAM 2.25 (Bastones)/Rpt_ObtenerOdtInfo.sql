USE [SAM]
GO
/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerOdtInfo]    Script Date: 06/21/2013 17:52:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para el header de los reportes de ODT
-- =============================================
ALTER PROCEDURE [dbo].[Rpt_ObtenerOdtInfo]
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
		  ,COUNT(CASE (ISNULL(bs.LetraBaston,'MAN')) WHEN 'MAN' THEN NULL ELSE 1 END) [Total]
		FROM
		  OrdenTrabajo ot
		  INNER JOIN OrdenTrabajoSpool ots
			ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
		  INNER JOIN Spool s 
			ON ots.SpoolID = s.SpoolID
		  INNER JOIN JuntaSpool js 
			ON js.SpoolID = s.SpoolID
	      LEFT JOIN BastonSpoolJunta bsj 
		    ON js.JuntaSpoolID = bsj.JuntaSpoolID
	      LEFT JOIN BastonSpool bs 
		    ON bs.BastonSpoolID = bsj.BastonSpoolID	
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
