
/****** Object:  StoredProcedure [dbo].[ObtenerSpoolsPorIds]    Script Date: 3/24/2014 4:42:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerSpoolsPorIds
	Funcion:	Obtiene los spools de la tabla Spool por un arreglo de Ids enviado
	Parametros:	@SpoolIDs nvarchar(max)
	Autor:		IHM
	Modificado:	28/10/2010, 02/09/2011 PEGV
				24/03/2014, JHT Agregar campo EsRevision
*****************************************************************************************/
ALTER PROCEDURE [dbo].[ObtenerSpoolsPorIds]
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
			,[FechaEtiqueta]
			,[NumeroEtiqueta]
			,[Campo1]
			,[Campo2]
			,[Campo3]
			,[Campo4]
			,[Campo5]
			,[CuadranteId]
			,[DiametroMayor]
			,[EsRevision]
			,[ConteoRevisiones]
			,[UltimaOrdenTrabajoEspecial]
	FROM [Spool]
	WHERE [SpoolID] IN
	(
		SELECT CAST([Value] AS INT)
		FROM dbo.SplitCVSToTable(@SpoolIDs,',')
	)



	SET NOCOUNT OFF;

END
