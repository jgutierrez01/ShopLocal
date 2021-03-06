IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_JuntasAFabricar_Especial]') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[Rpt_JuntasAFabricar_Especial]
GO

/****** Object:  StoredProcedure [dbo].[Rpt_JuntasAFabricar_Especial]    Script Date: 4/9/2014 12:31:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 10/Octubre/2010
-- Description:	Para obtener informacion de lista de corte
-- MODIFICACIONES
-- JHT		03/04/2014		Tomar en cuenta solo las juntas que tengan soldadura y armado
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_JuntasAFabricar_Especial]
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
			,(SELECT CASE WHEN (jws.JuntaArmadoID <> '' OR jws.JuntaArmadoID <> NULL) THEN '1' ELSE '0' END) as TieneArmado
			,(SELECT CASE WHEN (jws.JuntaSoldaduraID <> '' OR jws.JuntaSoldaduraID <> NULL) THEN '1' ELSE '0' END) as TieneSoldadura
	FROM JuntaSpool js
	INNER JOIN TipoJunta tj ON tj.TipoJuntaID = js.TipoJuntaID
	INNER JOIN OrdenTrabajoEspecialSpool ots ON ots.SpoolID = js.SpoolID
	INNER JOIN Spool s ON s.SpoolID =  js.SpoolID
	INNER JOIN OrdenTrabajoEspecial ot ON ot.OrdenTrabajoEspecialID = ots.OrdenTrabajoEspecialID
	INNER JOIN JuntaWorkstatus jws ON jws.JuntaSpoolID = js.JuntaSpoolID
	WHERE ot.OrdenTrabajoEspecialID = @OrdenTrabajoEspecialID AND js.FabAreaID = @ShopFabAreaID
		
END

/*
	exec Rpt_JuntasAFabricar 41
*/



