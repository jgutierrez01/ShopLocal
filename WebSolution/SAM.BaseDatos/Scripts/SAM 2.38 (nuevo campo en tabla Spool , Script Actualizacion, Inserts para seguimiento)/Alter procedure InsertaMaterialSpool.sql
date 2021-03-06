IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaMaterialSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertaMaterialSpool]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[InsertaMaterialSpool]
	Funcion:	Genera un nuevo registro en la tabla MaterialSpool
	Parametros:	@Tabla				NVARCHAR(50)
				@MaterialSpoolID	INT OUTPUT
				@SpoolID			INT
				@ItemCodeID		INT
				@Diametro1			DECIMAL(74)
				@Diametro2			DECIMAL(74)
				@Etiqueta			NVARCHAR(10)
				@Cantidad			INT
				@Peso				DECIMAL(72)
				@Area				DECIMAL(72)
				@Especificacion	NVARCHAR(10)
				@Grupo				NVARCHAR(150)
				@UsuarioModifica	UNIQUEIDENTIFIER
				@FechaModificacion	DATETIME		
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[InsertaMaterialSpool]

	-- Add the parameters for the stored procedure here
	@Tabla				NVARCHAR(50)
	,@MaterialSpoolID	INT OUTPUT
	,@SpoolID			INT
	,@ItemCodeID		INT
	,@Diametro1			DECIMAL(7,4)
	,@Diametro2			DECIMAL(7,4)
	,@Etiqueta			NVARCHAR(10)
	,@Cantidad			INT
	,@Peso				DECIMAL(7,2)
	,@Area				DECIMAL(7,2)
	,@Especificacion	NVARCHAR(10)
	,@Grupo				NVARCHAR(150)
	,@UsuarioModifica	UNIQUEIDENTIFIER
	,@FechaModificacion	DATETIME
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @DescripcionItemCode NVARCHAR(150)
	SELECT @DescripcionItemCode=DescripcionEspanol FROM ItemCode WHERE ItemCodeID = @ItemCodeID

    -- Insert statements for procedure here
    IF @Tabla = 'SPOOL'
		BEGIN
			INSERT INTO dbo.MaterialSpool (
					SpoolID
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
					,DescripcionMaterial
			)
			VALUES (
					@SpoolID
					,@ItemCodeID
					,@Diametro1
					,@Diametro2
					,@Etiqueta
					,@Cantidad
					,@Peso
					,@Area
					,@Especificacion
					,@Grupo
					,@UsuarioModifica
					,@FechaModificacion
					,@DescripcionItemCode
			)

			IF (SELECT DiametroMayor from Spool where SpoolID = @SpoolID) < @Diametro1
				BEGIN
					UPDATE Spool SET DiametroMayor = @Diametro1
				END
			
			SET @MaterialSpoolID = SCOPE_IDENTITY() 
		END

    IF @Tabla = 'SPOOLPENDIENTE'
		BEGIN
			INSERT INTO dbo.MaterialSpoolPendiente (
					SpoolPendienteID
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
					,DescripcionMaterial
			)
			VALUES (
					@SpoolID
					,@ItemCodeID
					,@Diametro1
					,@Diametro2
					,@Etiqueta
					,@Cantidad
					,@Peso
					,@Area
					,@Especificacion
					,@Grupo
					,@UsuarioModifica
					,@FechaModificacion
					,@DescripcionItemCode
			)
			SET @MaterialSpoolID = SCOPE_IDENTITY() 					
		END 
END
