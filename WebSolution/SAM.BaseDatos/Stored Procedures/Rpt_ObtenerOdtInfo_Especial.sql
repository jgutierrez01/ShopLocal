IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerOdtInfo_Especial]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Rpt_ObtenerOdtInfo_Especial]
GO

/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerOdtInfo_Especial]    Script Date: 4/3/2014 4:16:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--	Modificaciones
--	JHT		03/04/2014		Cambiar Orden trabajo po Orden trabajo especial
--
CREATE PROCEDURE [dbo].[Rpt_ObtenerOdtInfo_Especial]
(
	@OrdenTrabajoEspecialID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
		SELECT
		   p.Nombre AS [ProyectoNombre]
		  ,ot.OrdenTrabajoEspecialID
		  ,ot.NumeroOrden
		  ,ot.FechaOrden
		  ,u.UserId
		  ,u.Nombre AS [Nombre]
		  ,u.ApPaterno
		  ,t.Nombre AS [TallerNombre]
		  ,t.TallerID AS [TallerID]
		  ,COUNT(DISTINCT ots.OrdenTrabajoEspecialSpoolID) AS [TotalSpools]
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
		  INNER JOIN Proyecto p
			ON ot.ProyectoID = p.ProyectoID
		  INNER JOIN Usuario u
			ON ot.UsuarioModifica = u.UserId
		WHERE
		  ot.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID
	    GROUP BY
			p.Nombre
			,ot.OrdenTrabajoEspecialID
			,ot.NumeroOrden
			,ot.FechaOrden
			,u.UserId
			,u.Nombre
			,u.ApPaterno
			,t.Nombre
		    ,t.TallerID

END

GO


