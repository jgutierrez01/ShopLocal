IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_ObtenerSpoolsCaratulaOdtEspecial]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Rpt_ObtenerSpoolsCaratulaOdtEspecial]
GO
/****** Object:  StoredProcedure [dbo].[Rpt_ObtenerSpoolsCaratulaOdt]    Script Date: 4/14/2014 2:40:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Ivan Hernandez Marchand
-- Create date: 08/Octubre/2010
-- Description:	Para el reporte de carátula de ODT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_ObtenerSpoolsCaratulaOdtEspecial]
(
	@OrdenTrabajoEspecialID int
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
			ots.OrdenTrabajoEspecialSpoolID,
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
				WHERE	otj.OrdenTrabajoSpoolID IN (
													SELECT OrdenTrabajoSpoolID FROM OrdenTrabajoSpool tots
													INNER JOIN Spool s ON tots.SpoolID = s.SpoolID
													INNER JOIN OrdenTrabajoEspecialSpool totes ON s.SpoolID = totes.SpoolID
													WHERE totes.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID
												  )
						AND js.FabAreaID = @ShopFabAreaID
			) [Peqs],
			(
				SELECT	CASE	WHEN COUNT(odtm.CongeladoEsEquivalente) > 0 THEN CAST (1 as bit)
								ELSE CAST(0 as bit) END
				FROM	OrdenTrabajoMaterial odtm
				WHERE	odtm.OrdenTrabajoSpoolID IN (
													SELECT OrdenTrabajoSpoolID FROM OrdenTrabajoSpool tots
													INNER JOIN Spool s ON tots.SpoolID = s.SpoolID
													INNER JOIN OrdenTrabajoEspecialSpool totes ON s.SpoolID = totes.SpoolID
													WHERE totes.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID
												  )       --ots.OrdenTrabajoSpoolID
						AND odtm.CongeladoEsEquivalente = 1
			) [TieneEquivalencia]
	FROM	OrdenTrabajoEspecialSpool ots
	INNER JOIN Spool s on ots.SpoolID = s.SpoolID
	WHERE ots.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID
	ORDER BY
	ots.NumeroControl
	
END

/*
	exec Rpt_ObtenerSpoolsCaratulaOdt 41
*/


GO


