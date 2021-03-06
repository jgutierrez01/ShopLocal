IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerListaCorte]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerListaCorte
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerListaCorte]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
		SELECT
		cs.EtiquetaMaterial
		,tc1.Nombre AS [TipoCorteNombre1]
		,tc2.Nombre AS [TipoCorteNombre2]
		,cs.TipoCorte1ID
		,cs.TipoCorte2ID
		,s.Dibujo
		,s.Nombre
		,cs.EtiquetaSeccion
		FROM CorteSpool cs
		INNER JOIN TipoCorte tc1
		ON tc1.TipoCorteID = cs.TipoCorte1ID
		INNER JOIN TipoCorte tc2
		ON tc2.TipoCorteID = cs.TipoCorte2ID
		INNER JOIN Spool s
		ON s.SpoolID = cs.SpoolID
		INNER JOIN OrdenTrabajoSpool ots
		ON ots.SpoolID =  cs.SpoolID
		INNER JOIN OrdenTrabajo ot
		ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
		WHERE ot.OrdenTrabajoID = @OrdenTrabajoID
END
GO

