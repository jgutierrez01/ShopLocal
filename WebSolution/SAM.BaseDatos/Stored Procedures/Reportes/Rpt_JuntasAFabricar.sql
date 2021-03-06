IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_JuntasAFabricar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_JuntasAFabricar
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_JuntasAFabricar]
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
		
	SELECT	 ots.Partida
			,ots.NumeroControl
			,s.Nombre AS [NombreSpool]
			,js.Etiqueta
			,js.EtiquetaMaterial1
			,js.EtiquetaMaterial2
			,js.Diametro
			,tj.Codigo AS [CodigoJunta]
			,js.Cedula
			,(SELECT TOP 1 fa1.Nombre FROM FamiliaAcero fa1 WHERE fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID) [FamiliaAceroMaterial1]
			,(SELECT TOP 1 fa2.Nombre FROM FamiliaAcero fa2 WHERE fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID) [FamiliaAceroMaterial2]
	FROM JuntaSpool js
	INNER JOIN TipoJunta tj ON tj.TipoJuntaID = js.TipoJuntaID
	INNER JOIN OrdenTrabajoSpool ots ON ots.SpoolID = js.SpoolID
	INNER JOIN Spool s ON s.SpoolID =  js.SpoolID
	INNER JOIN OrdenTrabajo ot ON ot.OrdenTrabajoID = ots.OrdenTrabajoID
	WHERE ot.OrdenTrabajoID = @OrdenTrabajoID AND js.FabAreaID = @ShopFabAreaID
		
END

/*
	exec Rpt_JuntasAFabricar 41
*/


GO

