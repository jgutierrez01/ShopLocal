IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GuardaFamiliaMaterial]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GuardaFamiliaMaterial]
GO

Create PROCEDURE [dbo].[GuardaFamiliaMaterial]
(	
	@FamiliaMaterialID int,
	@Nombre nvarchar(50),
	@Descripcion nvarchar(500),
	@UsuarioModifica uniqueidentifier,
	@VersionRegistro timestamp
)
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Tabla TABLE
	(
		ID int,
		VersionRegistro varbinary(8)
	);
	
	IF ISNULL(@FamiliaMaterialID,-1) <= 0
	BEGIN

		INSERT INTO [FamiliaMaterial]
			([Nombre]
			,[Descripcion]
			,[UsuarioModifica]
			,[FechaModificacion])
		OUTPUT	INSERTED.FamiliaMaterialID, INSERTED.VersionRegistro
		INTO @Tabla
		VALUES
			(@Nombre
			,@Descripcion
			,@UsuarioModifica
			,GETDATE())

	END
	ELSE
	BEGIN

		UPDATE [FamiliaMaterial]
		SET	[Nombre] = @Nombre
			,[Descripcion] = @Descripcion
			,[UsuarioModifica] = @UsuarioModifica
			,[FechaModificacion] = GETDATE()
		OUTPUT	INSERTED.FamiliaMaterialID, INSERTED.VersionRegistro
		INTO @Tabla
		WHERE	[FamiliaMaterialID] = @FamiliaMaterialID
				AND
				[VersionRegistro] = @VersionRegistro

	END
	
	-- Regresar los valores insertados/actualizados
	SELECT ID, VersionRegistro FROM @Tabla

END

GO

