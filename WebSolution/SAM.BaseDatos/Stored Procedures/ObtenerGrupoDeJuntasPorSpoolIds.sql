IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerGrupoDeJuntasPorSpoolIds]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerGrupoDeJuntasPorSpoolIds]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerGrupoDeJuntasPorSpoolIds
	Funcion:	Obtiene la cuanta de las juntas para una serie de spools
	Parametros:	@SpoolIDs nvarchar(max)
	Autor:		IHM
	Modificado:	28/10/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerGrupoDeJuntasPorSpoolIds]
(
	 @SpoolIDs	NVARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	 [SpoolID]
			,COUNT(JuntaSpoolID) [Cuenta]
			,CAST(0 as int) [Suma]
	FROM [JuntaSpool]
	WHERE [SpoolID] IN
	(
		SELECT CAST([Value] AS INT)
		FROM dbo.SplitCVSToTable(@SpoolIDs,',')
	)
	GROUP BY [SpoolID]



	SET NOCOUNT OFF;

END
GO

