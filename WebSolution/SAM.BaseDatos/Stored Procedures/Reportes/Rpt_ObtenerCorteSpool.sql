IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerCorteSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerCorteSpool
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerCorteSpool]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON; 
		
		SELECT	ot.OrdenTrabajoID
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
	   INNER JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID
	   INNER JOIN OrdenTrabajo ot ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
	   WHERE ot.OrdenTrabajoID = @OrdenTrabajoID
END

/*
	exec Rpt_ObtenerCorteSpool 41
*/


GO


