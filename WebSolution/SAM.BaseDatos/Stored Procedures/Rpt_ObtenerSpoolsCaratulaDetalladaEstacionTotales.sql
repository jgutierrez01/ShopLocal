-- ==================================================================
-- Author:		David Emmanuel Zúñiga Herrera
-- Create date: 05/Junio/2013
-- Description:	Para el reporte de carátula Detallada (Totales) de ODT
-- ===================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerSpoolsCaratulaDetalladaEstacionTotales]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_ObtenerSpoolsCaratulaDetalladaEstacionTotales]
GO

CREATE PROCEDURE [dbo].[Rpt_ObtenerSpoolsCaratulaDetalladaEstacionTotales]
(
	@OrdenTrabajoID int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @ShopFabAreaID INT
	
	SELECT @ShopFabAreaID = fa.FabAreaID
	FROM FabArea fa
	WHERE fa.Codigo = 'SHOP'
		
	SELECT	s.Area,			
			s.Pdis,
			s.Peso,			
			(	SELECT	SUM(isnull(js.Peqs,0))
				FROM OrdenTrabajoJunta otj
				INNER JOIN JuntaSpool js on otj.JuntaSpoolID = js.JuntaSpoolID
				WHERE	otj.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
				AND js.FabAreaID = @ShopFabAreaID
			) [Peqs]
	FROM	OrdenTrabajoSpool ots
	INNER JOIN Spool s 
		ON ots.SpoolID = s.SpoolID
	WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
	ORDER BY ots.NumeroControl
END

/*
	exec Rpt_ObtenerSpoolsCaratulaDetalladaEstacionTotales 27
*/
