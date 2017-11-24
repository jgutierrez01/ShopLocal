IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerOdtInfoConEstado_Especial]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Rpt_ObtenerOdtInfoConEstado_Especial]
GO

/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerOdtInfoConEstado_Especial]    Script Date: 4/3/2014 4:43:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [dbo].[Rpt_ObtenerOdtInfoConEstado_Especial]
(
	@OrdenTrabajoEspecialID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
		SELECT ot.OrdenTrabajoEspecialID
		,ot.FechaOrden
		,ot.NumeroOrden
		,t.Nombre AS [TallerNombre]
		,eo.Nombre AS [EstatusNombre]
		,COUNT(DISTINCT ots.OrdenTrabajoEspecialSpoolID) as [TotalSpools]
		,COUNT(CASE (ISNULL(bs.LetraBaston,'MAN')) WHEN 'MAN' THEN NULL ELSE 1 END) [Total]
		FROM
		OrdenTrabajoEspecial ot
		INNER JOIN OrdenTrabajoEspecialSpool ots
		ON ot.OrdenTrabajoEspecialID = ots.OrdenTrabajoEspecialID
		INNER JOIN Spool s 
		ON ots.SpoolID = s.SpoolID
		LEFT JOIN JuntaSpool js 
		ON js.SpoolID = s.SpoolID
	    LEFT JOIN BastonSpoolJunta bsj 
		ON js.JuntaSpoolID = bsj.JuntaSpoolID
	    LEFT JOIN BastonSpool bs 
		ON bs.BastonSpoolID = bsj.BastonSpoolID
		INNER JOIN Taller t
		ON t.TallerID = ot.TallerID
		INNER JOIN EstatusOrden eo
		ON eo.EstatusOrdenID = ot.EstatusOrdenID
		WHERE ot.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID
		GROUP BY
			ot.OrdenTrabajoEspecialID
			,ot.FechaOrden
			,ot.NumeroOrden
			,t.Nombre
			,eo.Nombre

END

GO


