IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerCorteSpool_Especial]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Rpt_ObtenerCorteSpool_Especial]
GO

/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerCorteSpoolEspecial]    Script Date: 4/3/2014 6:00:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerCorteSpool_Especial]
(
	@OrdenTrabajoEspecialID int
)
AS
BEGIN
	SET NOCOUNT ON; 
		
		SELECT	DISTINCT(ot.OrdenTrabajoEspecialID)
			   ,ots.SpoolID
			   ,ots.NumeroControl
			   ,ic.ItemCodeID
			   ,ic.DescripcionEspanol
			   ,ic.Codigo
			   ,cs.Diametro
			   ,cs.TipoCorte1ID
			   ,cs.Cantidad
			   ,cs.TipoCorte2ID
			   ,cs.EtiquetaMaterial
			   ,cs.EtiquetaSeccion
			   ,cs.Observaciones
			   ,tc1.Nombre AS [TipoCorteNombre1]
			   ,tc2.Nombre AS [TipoCorteNombre2]
			   ,s.Nombre AS NombreSpool
	   FROM ItemCode ic
	   INNER JOIN CorteSpool cs ON cs.ItemCodeID = ic.ItemCodeID
	   INNER JOIN Spool s ON s.SpoolID = cs.SpoolID
	   INNER JOIN TipoCorte tc1 ON tc1.TipoCorteID = cs.TipoCorte1ID
	   INNER JOIN TipoCorte tc2 ON tc2.TipoCorteID = cs.TipoCorte2ID
	   INNER JOIN OrdenTrabajoEspecialSpool ots ON ots.SpoolID = s.SpoolID
	   INNER JOIN OrdenTrabajoEspecial ot ON ot.OrdenTrabajoEspecialID = ots.OrdenTrabajoEspecialID
	   INNER JOIN OrdenTrabajoSpool odtSpool ON odtSpool.SpoolID = ots.SpoolID
	   INNER JOIN OrdenTrabajoMaterial otm ON otm.OrdenTrabajoSpoolID = odtSpool.OrdenTrabajoSpoolID
	   WHERE ot.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID 
	   AND (otm.TieneCorte = 0 OR otm.TieneCorte IS NULL)
END

/*
	exec Rpt_ObtenerCorteSpool 41
*/



GO


