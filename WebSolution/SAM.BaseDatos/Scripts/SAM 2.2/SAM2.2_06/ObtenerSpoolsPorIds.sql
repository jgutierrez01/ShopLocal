IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerSpoolsPorIds]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerSpoolsPorIds]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerSpoolsPorIds
	Funcion:	Obtiene los spools de la tabla Spool por un arreglo de Ids enviado
	Parametros:	@SpoolIDs nvarchar(max)
	Autor:		IHM
	Modificado:	28/10/2010, 02/09/2011 PEGV
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerSpoolsPorIds]
(
	 @SpoolIDs	NVARCHAR(MAX)
)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	 [SpoolID]
			,[ProyectoID]
			,[FamiliaAcero1ID]
			,[FamiliaAcero2ID]
			,[Nombre]
			,[Dibujo]
			,[Especificacion]
			,[Cedula]
			,[Pdis]
			,[DiametroPlano]
			,[Peso]
			,[Area]
			,[PorcentajePnd]
			,[RequierePwht]
			,[PendienteDocumental]
			,[AprobadoParaCruce]
			,[Prioridad]
			,[Revision]
			,[RevisionCliente]
			,[Segmento1]
			,[Segmento2]
			,[Segmento3]
			,[Segmento4]
			,[Segmento5]
			,[Segmento6]
			,[Segmento7]
			,[UsuarioModifica]
			,[FechaModificacion]
			,[VersionRegistro]
			,[SistemaPintura]
			,[ColorPintura]
			,[CodigoPintura]
	FROM [Spool]
	WHERE [SpoolID] IN
	(
		SELECT CAST([Value] AS INT)
		FROM dbo.SplitCVSToTable(@SpoolIDs,',')
	)



	SET NOCOUNT OFF;

END
GO

