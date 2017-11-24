IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerSpoolsCaratulaOdt]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_ObtenerSpoolsCaratulaOdt
GO
-- =============================================
-- Author:		Ivan Hernandez Marchand
-- Create date: 08/Octubre/2010
-- Description:	Para el reporte de carátula de ODT
-- =============================================
CREATE PROCEDURE Rpt_ObtenerSpoolsCaratulaOdt
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
		
	SELECT	ots.NumeroControl,
			ots.Partida,
			ots.OrdenTrabajoSpoolID,
			s.SpoolID,
			s.Nombre,
			s.Dibujo,
			s.Area,
			s.Especificacion,
			s.Pdis,
			s.Peso,
			s.RevisionCliente,
			s.Revision,
			(	SELECT	SUM(isnull(js.Peqs,0))
				FROM OrdenTrabajoJunta otj
				INNER JOIN JuntaSpool js on otj.JuntaSpoolID = js.JuntaSpoolID
				WHERE	otj.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
						AND js.FabAreaID = @ShopFabAreaID
			) [Peqs],
			(
				SELECT	CASE	WHEN COUNT(odtm.CongeladoEsEquivalente) > 0 THEN CAST (1 as bit)
								ELSE CAST(0 as bit) END
				FROM	OrdenTrabajoMaterial odtm
				WHERE	odtm.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
						AND odtm.CongeladoEsEquivalente = 1
			) [TieneEquivalencia]
	FROM	OrdenTrabajoSpool ots
	INNER JOIN Spool s on ots.SpoolID = s.SpoolID
	WHERE ots.OrdenTrabajoID = @OrdenTrabajoID
	ORDER BY
	ots.NumeroControl
	
END

/*
	exec Rpt_ObtenerSpoolsCaratulaOdt 41
*/

GO

