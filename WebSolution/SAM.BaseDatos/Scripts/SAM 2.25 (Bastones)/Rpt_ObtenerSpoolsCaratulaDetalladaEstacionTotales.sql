USE [SAM]
GO
/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerSpoolsCaratulaDetalladaEstacionTotales]    Script Date: 06/17/2013 15:26:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================================================================
-- Author:		David Emmanuel Zúñiga Herrera
-- Create date: 05/Junio/2013
-- Description:	Para el reporte de carátula Detallada (Totales) de ODT
-- ===================================================================
ALTER PROCEDURE [dbo].[Rpt_ObtenerSpoolsCaratulaDetalladaEstacionTotales]
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
