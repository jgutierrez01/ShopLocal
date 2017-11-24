USE [SAM]
GO
/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerOdtInfoConEstado]    Script Date: 06/21/2013 17:52:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para el header de los reportes de ODT
-- =============================================
ALTER PROCEDURE [dbo].[Rpt_ObtenerOdtInfoConEstado]
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
