IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaCorteSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertaCorteSpool]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[[InsertaCorteSpool]]
	Funcion:	Genera un nuevo registro en la tabla spool o spoolhistorico
	Parametros:	@Tabla				NVARCHAR(50)	
				@CorteSpoolID		INT OUTPUT
				@SpoolID			INT
				@ItemCodeID	INT
				@TipoCorte1ID		INT
				@TipoCorte2ID		INT
				@EtiquetaMaterial	NVARCHAR(10)
				@EtiquetaSeccion	NVARCHAR(5)
				@Diametro			DECIMAL(7,4)
				@InicioFin			NVARCHAR(20)
				@Cantidad			INT
				@Observaciones		NVARCHAR(500)
				@UsuarioModifica	UNIQUEIDENTIFIER
				@FechaModificacion	DATETIME				
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[InsertaCorteSpool]

	-- Add the parameters for the stored procedure here
	@Tabla				NVARCHAR(50)	
	,@CorteSpoolID		INT OUTPUT
	,@SpoolID			INT
	,@ItemCodeID		INT
	,@TipoCorte1ID		INT
	,@TipoCorte2ID		INT
	,@EtiquetaMaterial	NVARCHAR(10)
	,@EtiquetaSeccion	NVARCHAR(5)
	,@Diametro			DECIMAL(7,4)
	,@InicioFin			NVARCHAR(20)
	,@Cantidad			INT
	,@Observaciones		NVARCHAR(500)
	,@UsuarioModifica	UNIQUEIDENTIFIER
	,@FechaModificacion	DATETIME	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @Tabla = 'SPOOL'
		BEGIN
    
			INSERT INTO dbo.CorteSpool (
					SpoolID
					,ItemCodeID
					,TipoCorte1ID
					,TipoCorte2ID
					,EtiquetaMaterial
					,EtiquetaSeccion
					,Diametro
					,InicioFin
					,Cantidad
					,Observaciones
					,UsuarioModifica
					,FechaModificacion
			)
			VALUES (
					@SpoolID
					,@ItemCodeID
					,@TipoCorte1ID
					,@TipoCorte2ID
					,@EtiquetaMaterial
					,@EtiquetaSeccion
					,@Diametro
					,@InicioFin
					,@Cantidad
					,@Observaciones
					,@UsuarioModifica
					,@FechaModificacion
			)
			
			SET @CorteSpoolID = SCOPE_IDENTITY() 
		END

    IF @Tabla = 'SPOOLPENDIENTE'
		BEGIN
			INSERT INTO dbo.CorteSpoolPendiente (
					SpoolPendienteID
					,ItemCodeID
					,TipoCorte1ID
					,TipoCorte2ID
					,EtiquetaMaterial
					,EtiquetaSeccion
					,Diametro
					,InicioFin
					,Cantidad
					,Observaciones
					,UsuarioModifica
					,FechaModificacion
			)
			VALUES (
					@SpoolID
					,@ItemCodeID
					,@TipoCorte1ID
					,@TipoCorte2ID
					,@EtiquetaMaterial
					,@EtiquetaSeccion
					,@Diametro
					,@InicioFin
					,@Cantidad
					,@Observaciones
					,@UsuarioModifica
					,@FechaModificacion
			)
			
			SET @CorteSpoolID = SCOPE_IDENTITY() 
		END
	
END
