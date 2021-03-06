
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaJuntaSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertaJuntaSpool]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[InsertaJuntaSpool]
	Funcion:	Genera un nuevo registro en la tabla JuntaSpool
	Parametros:	@Tabla				NVARCHAR(50)	
				@JuntaSpoolID		INT OUTPUT
				@SpoolID			INT
				@TipoJuntaID		INT
				@FabAreaID			INT
				@Etiqueta			NVARCHAR(10)
				@EtiquetaMaterial1	NVARCHAR(10)
				@EtiquetaMaterial2	NVARCHAR(10)
				@Cedula			NVARCHAR(10)
				@FamiliaAceroMaterial1ID INT
				@FamiliaAceroMaterial2ID INT
				@Diametro			DECIMAL(74)
				@Espesor			DECIMAL(104)
				@KgTeoricos		DECIMAL(124)
				@Peqs				DECIMAL(104)
				@UsuarioModifica	UNIQUEIDENTIFIER
				@FechaModificacion	DATETIME			
	Autor:		HL
	Modificado:	05/06/2014 GTG se agrego el campo RequierePwht
*****************************************************************************************/

CREATE PROCEDURE InsertaJuntaSpool
	-- Add the parameters for the stored procedure here
	@Tabla				NVARCHAR(50)	
	,@JuntaSpoolID		INT OUTPUT
	,@SpoolID			INT
	,@TipoJuntaID		INT
	,@FabAreaID			INT
	,@Etiqueta			NVARCHAR(10)
	,@EtiquetaMaterial1	NVARCHAR(10)
	,@EtiquetaMaterial2	NVARCHAR(10)
	,@Cedula			NVARCHAR(10)
	,@FamiliaAceroMaterial1ID INT
	,@FamiliaAceroMaterial2ID INT
	,@Diametro			DECIMAL(7,4)
	,@Espesor			DECIMAL(10,4)
	,@KgTeoricos		DECIMAL(12,4)
	,@Peqs				DECIMAL(10,4)
	,@UsuarioModifica	UNIQUEIDENTIFIER
	,@FechaModificacion	DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @RequierePwht BIT

	SELECT @RequierePwht = RequierePwht 
			FROM dbo.Spool 
			WHERE SpoolID = @SpoolID

    -- Insert statements for procedure here
    IF @Tabla = 'SPOOL'
		BEGIN    		
		
			INSERT INTO dbo.JuntaSpool (
					SpoolID
					,TipoJuntaID
					,FabAreaID
					,Etiqueta
					,EtiquetaMaterial1
					,EtiquetaMaterial2
					,Cedula
					,FamiliaAceroMaterial1ID
					,FamiliaAceroMaterial2ID
					,Diametro
					,Espesor
					,KgTeoricos
					,Peqs
					,UsuarioModifica
					,FechaModificacion
					,EsManual
					,RequierePwht
			)
			VALUES (
					@SpoolID
					,@TipoJuntaID
					,@FabAreaID
					,@Etiqueta
					,@EtiquetaMaterial1
					,@EtiquetaMaterial2
					,@Cedula
					,@FamiliaAceroMaterial1ID
					,@FamiliaAceroMaterial2ID
					,@Diametro
					,@Espesor
					,@KgTeoricos
					,@Peqs
					,@UsuarioModifica
					,@FechaModificacion
					,1
					,@RequierePwht
			)
			
			SET @JuntaSpoolID = SCOPE_IDENTITY() 
		END

    IF @Tabla = 'SPOOLPENDIENTE'
		BEGIN
			INSERT INTO dbo.JuntaSpoolPendiente (
					SpoolPendienteID
					,TipoJuntaID
					,FabAreaID
					,Etiqueta
					,EtiquetaMaterial1
					,EtiquetaMaterial2
					,Cedula
					,FamiliaAceroMaterial1ID
					,FamiliaAceroMaterial2ID
					,Diametro
					,Espesor
					,KgTeoricos
					,Peqs
					,UsuarioModifica
					,FechaModificacion
					,RequierePwht
			)
			VALUES (
					@SpoolID
					,@TipoJuntaID
					,@FabAreaID
					,@Etiqueta
					,@EtiquetaMaterial1
					,@EtiquetaMaterial2
					,@Cedula
					,@FamiliaAceroMaterial1ID
					,@FamiliaAceroMaterial2ID
					,@Diametro
					,@Espesor
					,@KgTeoricos
					,@Peqs
					,@UsuarioModifica
					,@FechaModificacion
					,@RequierePwht
			)
			
			SET @JuntaSpoolID = SCOPE_IDENTITY() 
		END	
END
