IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertaSpool]
GO
/****** Object:  StoredProcedure [dbo].[InsertaSpool]    Script Date: 02/24/2012 17:48:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertaSpool]
	-- Add the parameters for the stored procedure here
	@Tabla					NVARCHAR(50)
	,@SpoolID				INT OUTPUT 
	,@ProyectoID			INT
	,@FamiliaAcero1ID		INT
	,@FamiliaAcero2ID		INT
	,@Nombre				NVARCHAR(50)
	,@Dibujo				NVARCHAR(50)
	,@Especificacion		NVARCHAR(15) 
	,@Cedula				NVARCHAR(10) 
	,@Pdis					DECIMAL(10,4) 
	,@DiametroPlano			DECIMAL(10,4) 
	,@Peso					DECIMAL(7,2) 
	,@Area					DECIMAL(7,2) 
	,@PorcentajePnd			INT 
	,@RequierePwht			BIT 
	,@PendienteDocumental	BIT
	,@AprobadoParaCruce		BIT 
	,@Prioridad				INT 
	,@Revision				NVARCHAR(10) 
	,@RevisionCliente		NVARCHAR(10)
	,@Segmento1				NVARCHAR(20) 
	,@Segmento2				NVARCHAR(20) 
	,@Segmento3				NVARCHAR(20)  
	,@Segmento4				NVARCHAR(20) 
	,@Segmento5				NVARCHAR(20) 
	,@Segmento6				NVARCHAR(20) 
	,@Segmento7				NVARCHAR(20) 
	,@UsuarioModifica		UNIQUEIDENTIFIER
	,@FechaModificacion		DATETIME
	,@SistemaPintura		NVARCHAR(50) 
	,@ColorPintura			NVARCHAR(50)
	,@CodigoPintura			NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @Tabla = 'SPOOL'
		BEGIN
			INSERT INTO dbo.Spool (
					ProyectoID
					,FamiliaAcero1ID
					,FamiliaAcero2ID
					,Nombre
					,Dibujo
					,Especificacion
					,Cedula
					,Pdis
					,DiametroPlano
					,Peso
					,Area
					,PorcentajePnd
					,RequierePwht
					,PendienteDocumental
					,AprobadoParaCruce
					,Prioridad
					,Revision
					,RevisionCliente
					,Segmento1
					,Segmento2
					,Segmento3
					,Segmento4
					,Segmento5
					,Segmento6
					,Segmento7
					,UsuarioModifica
					,FechaModificacion
					,SistemaPintura
					,ColorPintura
					,CodigoPintura
			)
			VALUES (
					@ProyectoID
					,@FamiliaAcero1ID
					,@FamiliaAcero2ID
					,@Nombre
					,@Dibujo
					,@Especificacion
					,@Cedula
					,@Pdis
					,@DiametroPlano
					,@Peso
					,@Area
					,@PorcentajePnd
					,@RequierePwht
					,@PendienteDocumental
					,@AprobadoParaCruce
					,@Prioridad
					,@Revision
					,@RevisionCliente
					,@Segmento1
					,@Segmento2
					,@Segmento3
					,@Segmento4
					,@Segmento5
					,@Segmento6
					,@Segmento7
					,@UsuarioModifica
					,@FechaModificacion
					,@SistemaPintura
					,@ColorPintura
					,@CodigoPintura
			)
			
			SET @SpoolID = SCOPE_IDENTITY() 
		END 
		
    IF @Tabla = 'SPOOLPENDIENTE'
		BEGIN
		
			INSERT INTO dbo.SpoolPendiente (
					SpoolPendienteID
					,ProyectoID
					,FamiliaAcero1ID
					,FamiliaAcero2ID
					,Nombre
					,Dibujo
					,Especificacion
					,Cedula
					,Pdis
					,DiametroPlano
					,Peso
					,Area
					,PorcentajePnd
					,RequierePwht
					,PendienteDocumental
					,AprobadoParaCruce
					,Prioridad
					,Revision
					,RevisionCliente
					,Segmento1
					,Segmento2
					,Segmento3
					,Segmento4
					,Segmento5
					,Segmento6
					,Segmento7
					,UsuarioModifica
					,FechaModificacion
					,SistemaPintura
					,ColorPintura
					,CodigoPintura
			)
			VALUES (
					@SpoolID
					,@ProyectoID
					,@FamiliaAcero1ID
					,@FamiliaAcero2ID
					,@Nombre
					,@Dibujo
					,@Especificacion
					,@Cedula
					,@Pdis
					,@DiametroPlano
					,@Peso
					,@Area
					,@PorcentajePnd
					,@RequierePwht
					,@PendienteDocumental
					,@AprobadoParaCruce
					,@Prioridad
					,@Revision
					,@RevisionCliente
					,@Segmento1
					,@Segmento2
					,@Segmento3
					,@Segmento4
					,@Segmento5
					,@Segmento6
					,@Segmento7
					,@UsuarioModifica
					,@FechaModificacion
					,@SistemaPintura
					,@ColorPintura
					,@CodigoPintura
			)
			
			SET @SpoolID = @SpoolID
			
		END		
END
