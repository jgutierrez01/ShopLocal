IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaMaterialSpoolHistorico]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertaMaterialSpoolHistorico]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		InsertaMaterialSpoolHistorico
	Funcion:	Genera un nuevo registro en la tabla MaterialSpool
	Parametros:	@SpoolID				INT
				@SpoolHistoricoID		INT	
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[InsertaMaterialSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@SpoolHistoricoID		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	INSERT INTO		dbo.MaterialSpoolHistorico
					(
					SpoolHistoricoID
					,MaterialSpoolID
					,SpoolID
					,ItemCodeID
					,Diametro1
					,Diametro2
					,Etiqueta
					,Cantidad
					,Peso
					,Area
					,Especificacion
					,Grupo
					,UsuarioModifica
					,FechaModificacion
					,SpoolNombre
					,ItemCodeDescripcionEspanol
					,ItemCodeDescripcionIngles
					,ItemCodeCodigo
					)
	SELECT	
					@SpoolHistoricoID
					,A.MaterialSpoolID
					,A.SpoolID
					,A.ItemCodeID
					,A.Diametro1
					,A.Diametro2
					,A.Etiqueta
					,A.Cantidad
					,A.Peso
					,A.Area
					,A.Especificacion
					,A.Grupo
					,A.UsuarioModifica
					,A.FechaModificacion
					,B.Nombre as SpoolNombre
					,C.DescripcionEspanol as ItemCodeDescripcionEspanol
					,C.DescripcionIngles as ItemCodeDescripcionIngles
					,C.Codigo as ItemCodeDescripcionIngles
	FROM			dbo.MaterialSpool A
	INNER JOIN		dbo.Spool B ON A.SpoolID = B.SpoolID
	INNER JOIN		dbo.ItemCode C ON A.ItemCodeID = C.ItemCodeID
	LEFT JOIN		dbo.aspnet_Users D ON A.UsuarioModifica = D.UserId
	WHERE			A.SpoolID = @SpoolID	
END
