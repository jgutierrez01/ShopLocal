/****** Sección para la creación de las tablas, relaciones y procedimientos almacenados necesarios ******/

USE [Sam]
GO

/****** Object:  Table [dbo].[OrdenTrabajo]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrdenTrabajo]') AND type in (N'U'))
BEGIN
ALTER TABLE [dbo].[OrdenTrabajo] ADD [VersionOrden] INT NULL 
UPDATE [dbo].[OrdenTrabajo] SET [VersionOrden] = 0 WHERE [VersionOrden] IS NULL 
END
GO


/****** Object:  Table [dbo].[SpoolHistorico]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpoolHistorico]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SpoolHistorico](
	[SpoolHistoricoID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[FamiliaAcero1ID] [int] NOT NULL,
	[FamiliaAcero2ID] [int] NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Dibujo] [nvarchar](50) NOT NULL,
	[Especificacion] [nvarchar](15) NULL,
	[Cedula] [nvarchar](10) NULL,
	[Pdis] [decimal](10, 4) NULL,
	[DiametroPlano] [decimal](10, 4) NULL,
	[Peso] [decimal](7, 2) NULL,
	[Area] [decimal](7, 2) NULL,
	[PorcentajePnd] [int] NULL,
	[RequierePwht] [bit] NOT NULL,
	[PendienteDocumental] [bit] NOT NULL,
	[AprobadoParaCruce] [bit] NOT NULL,
	[Prioridad] [int] NULL,
	[Revision] [nvarchar](10) NULL,
	[RevisionCliente] [nvarchar](10) NULL,
	[Segmento1] [nvarchar](20) NULL,
	[Segmento2] [nvarchar](20) NULL,
	[Segmento3] [nvarchar](20) NULL,
	[Segmento4] [nvarchar](20) NULL,
	[Segmento5] [nvarchar](20) NULL,
	[Segmento6] [nvarchar](20) NULL,
	[Segmento7] [nvarchar](20) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[SistemaPintura] [nvarchar](50) NULL,
	[ColorPintura] [nvarchar](50) NULL,
	[CodigoPintura] [nvarchar](50) NULL,
	[ProyectoNombre] [nvarchar](100) NOT NULL,
	[FamiliaAcero1Nombre] [nvarchar](50) NOT NULL,
	[FamiliaAcero2Nombre] [nvarchar](50) NULL,
	[TieneHoldIngenieria] [bit] NULL,
	[TieneHoldCalidad] [bit] NULL,
	[Confinado] [bit] NULL,
 CONSTRAINT [PK_SpoolHistorico] PRIMARY KEY CLUSTERED 
(
	[SpoolHistoricoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DTSExecLog]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DTSExecLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DTSExecLog](
	[DTSExecLogID] [int] IDENTITY(1,1) NOT NULL,
	[DTSExecStartTime] [datetime] NULL,
	[DTSExecEndTime] [datetime] NULL,
	[DTSExecCommandString] [varchar](4000) NULL,
	[DTSExecReturnCode] [int] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DTSExecLog] PRIMARY KEY CLUSTERED 
(
	[DTSExecLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MaterialSpoolHistorico]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MaterialSpoolHistorico]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MaterialSpoolHistorico](
	[MaterialSpoolHistoricoID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolHistoricoID] [int] NOT NULL,
	[MaterialSpoolID] [int] NOT NULL,
	[SpoolID] [int] NOT NULL,
	[ItemCodeID] [int] NOT NULL,
	[Diametro1] [decimal](7, 4) NOT NULL,
	[Diametro2] [decimal](7, 4) NOT NULL,
	[Etiqueta] [nvarchar](10) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Peso] [decimal](7, 2) NULL,
	[Area] [decimal](7, 2) NULL,
	[Especificacion] [nvarchar](10) NULL,
	[Grupo] [nvarchar](150) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[SpoolNombre] [nvarchar](50) NOT NULL,
	[ItemCodeDescripcionEspanol] [nvarchar](150) NOT NULL,
	[ItemCodeDescripcionIngles] [nvarchar](150) NULL,
	[ItemCodeCodigo] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MaterialSpoolHistorico] PRIMARY KEY CLUSTERED 
(
	[MaterialSpoolHistoricoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[JuntaSpoolHistorico]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JuntaSpoolHistorico]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[JuntaSpoolHistorico](
	[JuntaSpoolHistoricoID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolHistoricoID] [int] NOT NULL,
	[JuntaSpoolID] [int] NOT NULL,
	[SpoolID] [int] NOT NULL,
	[TipoJuntaID] [int] NOT NULL,
	[FabAreaID] [int] NOT NULL,
	[Etiqueta] [nvarchar](10) NULL,
	[EtiquetaMaterial1] [nvarchar](10) NULL,
	[EtiquetaMaterial2] [nvarchar](10) NULL,
	[Cedula] [nvarchar](10) NULL,
	[FamiliaAceroMaterial1ID] [int] NOT NULL,
	[FamiliaAceroMaterial2ID] [int] NULL,
	[Diametro] [decimal](7, 4) NOT NULL,
	[Espesor] [decimal](10, 4) NULL,
	[KgTeoricos] [decimal](12, 4) NULL,
	[Peqs] [decimal](10, 4) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[SpoolNombre] [nvarchar](50) NOT NULL,
	[TipoJuntaCodigo] [nvarchar](10) NOT NULL,
	[FabAreaCodigo] [nvarchar](20) NOT NULL,
	[FamiliaAceroMaterial1Nombre] [nvarchar](50) NOT NULL,
	[FamiliaAceroMaterial2Nombre] [varchar](50) NULL,
 CONSTRAINT [PK_JuntaSpoolHistorico] PRIMARY KEY CLUSTERED 
(
	[JuntaSpoolHistoricoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CorteSpoolHistorico]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CorteSpoolHistorico]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CorteSpoolHistorico](
	[CorteSpoolHistoricoID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolHistoricoID] [int] NOT NULL,
	[CorteSpoolID] [int] NOT NULL,
	[SpoolID] [int] NOT NULL,
	[ItemCodeID] [int] NOT NULL,
	[TipoCorte1ID] [int] NOT NULL,
	[TipoCorte2ID] [int] NOT NULL,
	[EtiquetaMaterial] [nvarchar](10) NULL,
	[EtiquetaSeccion] [nvarchar](5) NULL,
	[Diametro] [decimal](7, 4) NOT NULL,
	[InicioFin] [nvarchar](20) NULL,
	[Cantidad] [int] NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[SpoolNombre] [nvarchar](50) NOT NULL,
	[ItemCodeCodigo] [nvarchar](50) NOT NULL,
	[ItemCodeDescripcionEspanol] [nvarchar](150) NOT NULL,
	[ItemCodeDescripcionIngles] [nvarchar](150) NULL,
	[TipoCorte1Codigo] [nvarchar](10) NOT NULL,
	[TipoCorte2Codigo] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_CorteSpoolHistorico] PRIMARY KEY CLUSTERED 
(
	[CorteSpoolHistoricoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [dbo].[EjecutaDTSCargaIngenieria]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EjecutaDTSCargaIngenieria]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[EjecutaDTSCargaIngenieria]
	-- Add the parameters for the stored procedure here
	@CommandString AS VARCHAR(4000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	 
	/*
	@ReturnCode recibe el resultado de la ejecución del DTEXEC
	Value  Description  
	0  The package executed successfully.  
	1  The package failed.  
	3  The package was cancelled by the user.  
	4  The utility was unable to locate the requested package. The package could not be found.  
	5  The utility was unable to load the requested package. The package could not be loaded.  
	6  The utility encountered an internal error of syntactic or semantic errors in the command line. 	
	*/

	DECLARE		@DTSExecStartTime	DATETIME
	DECLARE		@DTSExecEndTime		DATETIME 
	DECLARE		@ReturnCode			INT
	
	SET			@DTSExecStartTime = GETDATE() 
	
	EXEC		@ReturnCode = XP_CMDSHELL @CommandString,NO_OUTPUT


	SET			@DTSExecEndTime = GETDATE() 	

	INSERT INTO	dbo.DTSExecLog 
				(DTSExecStartTime,DTSExecEndTime,DTSExecCommandString,DTSExecReturnCode,UsuarioModifica,FechaModificacion) 
	VALUES		(@DTSExecStartTime,@DTSExecEndTime,@CommandString,@ReturnCode,NULL,GETDATE())

	SELECT		@ReturnCode

END
' 
END
GO
/****** Object:  Table [dbo].[DTSSummaryLog]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DTSSummaryLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DTSSummaryLog](
	[DTSSummaryLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[SAMWebSessionID] [nvarchar](50) NOT NULL,
	[SAMWebProjectID] [int] NOT NULL,
	[Category] [smallint] NULL,
	[CountByCategory] [smallint] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DTSSummaryLog] PRIMARY KEY CLUSTERED 
(
	[DTSSummaryLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DTSErrorLog]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DTSErrorLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DTSErrorLog](
	[DTSErrorLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[SAMWebSessionID] [nvarchar](50) NOT NULL,
	[Desripcion] [nvarchar](250) NULL,
	[Origen] [nvarchar](250) NULL,
	[Proceso] [nvarchar](250) NULL,
	[Tipo] [nvarchar](50) NULL,
	[Accion] [nvarchar](150) NULL,
	[Archivo] [nvarchar](50) NULL,
	[Fila] [int] NULL,
	[SpoolNombre] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DTSErrorLog] PRIMARY KEY CLUSTERED 
(
	[DTSErrorLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[SpoolPendiente]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SpoolPendiente](
	[SpoolPendienteID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[FamiliaAcero1ID] [int] NOT NULL,
	[FamiliaAcero2ID] [int] NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Dibujo] [nvarchar](50) NOT NULL,
	[Especificacion] [nvarchar](15) NULL,
	[Cedula] [nvarchar](10) NULL,
	[Pdis] [decimal](10, 4) NULL,
	[DiametroPlano] [decimal](10, 4) NULL,
	[Peso] [decimal](7, 2) NULL,
	[Area] [decimal](7, 2) NULL,
	[PorcentajePnd] [int] NULL,
	[RequierePwht] [bit] NOT NULL,
	[PendienteDocumental] [bit] NOT NULL,
	[AprobadoParaCruce] [bit] NOT NULL,
	[Prioridad] [int] NULL,
	[Revision] [nvarchar](10) NULL,
	[RevisionCliente] [nvarchar](10) NULL,
	[Segmento1] [nvarchar](20) NULL,
	[Segmento2] [nvarchar](20) NULL,
	[Segmento3] [nvarchar](20) NULL,
	[Segmento4] [nvarchar](20) NULL,
	[Segmento5] [nvarchar](20) NULL,
	[Segmento6] [nvarchar](20) NULL,
	[Segmento7] [nvarchar](20) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[SistemaPintura] [nvarchar](50) NULL,
	[ColorPintura] [nvarchar](50) NULL,
	[CodigoPintura] [nvarchar](50) NULL,
 CONSTRAINT [PK_SpoolPendiente] PRIMARY KEY CLUSTERED 
(
	[SpoolPendienteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_SpoolPendiente_Nombre_Proyecto] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[Nombre] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CorteSpoolPendiente]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CorteSpoolPendiente](
	[CorteSpoolPendienteID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolPendienteID] [int] NOT NULL,
	[ItemCodeID] [int] NOT NULL,
	[TipoCorte1ID] [int] NOT NULL,
	[TipoCorte2ID] [int] NOT NULL,
	[EtiquetaMaterial] [nvarchar](10) NULL,
	[EtiquetaSeccion] [nvarchar](5) NULL,
	[Diametro] [decimal](7, 4) NOT NULL,
	[InicioFin] [nvarchar](20) NULL,
	[Cantidad] [int] NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_CorteSpoolPendiente] PRIMARY KEY CLUSTERED 
(
	[CorteSpoolPendienteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[JuntaSpoolPendiente]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[JuntaSpoolPendiente](
	[JuntaSpoolPendienteID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolPendienteID] [int] NOT NULL,
	[TipoJuntaID] [int] NOT NULL,
	[FabAreaID] [int] NOT NULL,
	[Etiqueta] [nvarchar](10) NULL,
	[EtiquetaMaterial1] [nvarchar](10) NULL,
	[EtiquetaMaterial2] [nvarchar](10) NULL,
	[Cedula] [nvarchar](10) NULL,
	[FamiliaAceroMaterial1ID] [int] NOT NULL,
	[FamiliaAceroMaterial2ID] [int] NULL,
	[Diametro] [decimal](7, 4) NOT NULL,
	[Espesor] [decimal](10, 4) NULL,
	[KgTeoricos] [decimal](12, 4) NULL,
	[Peqs] [decimal](10, 4) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaSpoolPendiente] PRIMARY KEY CLUSTERED 
(
	[JuntaSpoolPendienteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_JuntaSpoolPendiente_SpoolPendienteID_Etiqueta] UNIQUE NONCLUSTERED 
(
	[SpoolPendienteID] ASC,
	[Etiqueta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [dbo].[InsertaSpool]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaSpool]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
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
	,@Peso					DECIMAL(10,7) 
	,@Area					DECIMAL(10,7) 
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
    IF @Tabla = ''SPOOL''
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
		
    IF @Tabla = ''SPOOLPENDIENTE''
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
' 
END
GO
/****** Object:  Table [dbo].[MaterialSpoolPendiente]    Script Date: 01/10/2012 09:59:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MaterialSpoolPendiente](
	[MaterialSpoolPendienteID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolPendienteID] [int] NOT NULL,
	[ItemCodeID] [int] NOT NULL,
	[Diametro1] [decimal](7, 4) NOT NULL,
	[Diametro2] [decimal](7, 4) NOT NULL,
	[Etiqueta] [nvarchar](10) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Peso] [decimal](7, 2) NULL,
	[Area] [decimal](7, 2) NULL,
	[Especificacion] [nvarchar](10) NULL,
	[Grupo] [nvarchar](150) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_MaterialSpoolPendiente] PRIMARY KEY CLUSTERED 
(
	[MaterialSpoolPendienteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_MaterialSpoolPendiente_SpoolPendienteID_Etiqueta] UNIQUE NONCLUSTERED 
(
	[SpoolPendienteID] ASC,
	[Etiqueta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [dbo].[ModificaValoresSpool]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ModificaValoresSpool]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ModificaValoresSpool]
	-- Add the parameters for the stored procedure here
	@Tabla					NVARCHAR(50)
	,@Campo					NVARCHAR(50)
	,@SpoolID				INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @Tabla = ''SPOOL''
		BEGIN
			IF @Campo = ''CEDULA''
				BEGIN
					UPDATE		A
					SET			A.Cedula = B.Cedula
					FROM		dbo.Spool  A
					INNER JOIN	(
								SELECT		TOP 1 
											BA.SpoolID,BA.Cedula
								FROM		dbo.JuntaSpool BA
								WHERE		BA.SpoolID = @SpoolID
								GROUP BY	BA.SpoolID,Cedula
								ORDER BY	MAX(BA.Diametro) DESC
								) B ON A.SpoolID = B.SpoolID					
				END 
		END 
		
    IF @Tabla = ''SPOOLPENDIENTE''
		BEGIN
			IF @Campo = ''CEDULA''
				BEGIN
					UPDATE		A
					SET			A.Cedula = B.Cedula
					FROM		dbo.SpoolPendiente  A
					INNER JOIN	(
								SELECT		TOP 1 
											BA.SpoolPendienteID,BA.Cedula
								FROM		dbo.JuntaSpoolPendiente BA
								WHERE		BA.SpoolPendienteID = @SpoolID
								GROUP BY	BA.SpoolPendienteID,BA.Cedula
								ORDER BY	MAX(BA.Diametro) DESC
								) B ON A.SpoolPendienteID = B.SpoolPendienteID
				END 					
		END		
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertaSpoolHistorico]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaSpoolHistorico]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertaSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@SpoolHistoricoID		INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	INSERT INTO	dbo.SpoolHistorico 
					(
					SpoolID
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
					,A.Area
					,A.PorcentajePnd
					,A.RequierePwht
					,A.PendienteDocumental
					,A.AprobadoParaCruce
					,A.Prioridad
					,A.Revision
					,A.RevisionCliente
					,A.Segmento1
					,A.Segmento2
					,A.Segmento3
					,A.Segmento4
					,A.Segmento5
					,A.Segmento6
					,A.Segmento7
					,A.UsuarioModifica
					,A.FechaModificacion
					,A.SistemaPintura
					,A.ColorPintura
					,A.CodigoPintura
					,ProyectoNombre
					,FamiliaAcero1Nombre
					,FamiliaAcero2Nombre 
					,TieneHoldIngenieria
					,TieneHoldCalidad
					,Confinado				
					)
	SELECT 
					A.SpoolID
					,A.ProyectoID
					,A.FamiliaAcero1ID
					,A.FamiliaAcero2ID
					,A.Nombre
					,A.Dibujo
					,A.Especificacion
					,A.Cedula
					,A.Pdis
					,A.DiametroPlano
					,A.Peso
					,A.Area
					,A.PorcentajePnd
					,A.RequierePwht
					,A.PendienteDocumental
					,A.AprobadoParaCruce
					,A.Prioridad
					,A.Revision
					,A.RevisionCliente
					,A.Segmento1
					,A.Segmento2
					,A.Segmento3
					,A.Segmento4
					,A.Segmento5
					,A.Segmento6
					,A.Segmento7
					,A.UsuarioModifica
					,A.FechaModificacion
					,A.SistemaPintura
					,A.ColorPintura
					,A.CodigoPintura
					,B.Nombre as ProyectoNombre
					,C.Nombre as FamiliaAcero1Nombre
					,D.Nombre as FamiliaAcero2Nombre
					,F.TieneHoldIngenieria
					,F.TieneHoldCalidad
					,F.Confinado 
	FROM			dbo.Spool A
	INNER JOIN		dbo.Proyecto B ON A.ProyectoID = B.ProyectoID
	LEFT JOIN		dbo.FamiliaAcero C ON A.FamiliaAcero1ID = C.FamiliaAceroID
	LEFT JOIN		dbo.FamiliaAcero D ON A.FamiliaAcero2ID = D.FamiliaAceroID
	LEFT JOIN		dbo.aspnet_Users E ON A.UsuarioModifica = E.UserId
	LEFT JOIN		dbo.SpoolHold F ON A.SpoolID = F.SpoolID
	WHERE			A.SpoolID = @SpoolID
	
	SET @SpoolHistoricoID = SCOPE_IDENTITY() 

END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertaMaterialSpoolHistorico]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaMaterialSpoolHistorico]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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
' 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertaMaterialSpool]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaMaterialSpool]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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

    -- Insert statements for procedure here
    IF @Tabla = ''SPOOL''
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
			)
			
			SET @MaterialSpoolID = SCOPE_IDENTITY() 
		END

    IF @Tabla = ''SPOOLPENDIENTE''
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
			)
			SET @MaterialSpoolID = SCOPE_IDENTITY() 					
		END 
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertaJuntaSpoolHistorico]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaJuntaSpoolHistorico]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertaJuntaSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@SpoolHistoricoID		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO		dbo.JuntaSpoolHistorico 
					(
					SpoolHistoricoID
					,JuntaSpoolID			 
					,SpoolID
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
					,SpoolNombre
					,TipoJuntaCodigo
					,FabAreaCodigo
					,FamiliaAceroMaterial1Nombre
					,FamiliaAceroMaterial2Nombre
					)
	SELECT	
					@SpoolHistoricoID
					,A.JuntaSpoolID
					,A.SpoolID
					,A.TipoJuntaID
					,A.FabAreaID
					,A.Etiqueta
					,A.EtiquetaMaterial1
					,A.EtiquetaMaterial2
					,A.Cedula
					,A.FamiliaAceroMaterial1ID
					,A.FamiliaAceroMaterial2ID
					,A.Diametro
					,A.Espesor
					,A.KgTeoricos
					,A.Peqs
					,A.UsuarioModifica
					,A.FechaModificacion
					,B.Nombre as SpoolNombre 
					,C.Codigo as TipoJuntaCodigo
					,D.Codigo as FabAreaCodigo
					,E.Nombre as FamiliaAceroMaterial1Nombre 
					,F.Nombre as FamiliaAceroMaterial2Nombre 
	FROM			dbo.JuntaSpool A 
	INNER JOIN		dbo.Spool B ON A.SpoolID = B.SpoolID
	INNER JOIN		dbo.TipoJunta C ON A.TipoJuntaID = C.TipoJuntaID
	INNER JOIN		dbo.FabArea D ON A.FabAreaID = D.FabAreaID
	INNER JOIN		dbo.FamiliaAcero E ON A.FamiliaAceroMaterial1ID = E.FamiliaAceroID
	LEFT JOIN		dbo.FamiliaAcero F ON A.FamiliaAceroMaterial2ID = F.FamiliaAceroID
	LEFT JOIN		dbo.aspnet_Users G ON A.UsuarioModifica = G.UserId
	WHERE			A.SpoolID = @SpoolID
	
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertaJuntaSpool]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaJuntaSpool]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertaJuntaSpool]
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

    -- Insert statements for procedure here
    IF @Tabla = ''SPOOL''
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
			)
			
			SET @JuntaSpoolID = SCOPE_IDENTITY() 
		END

    IF @Tabla = ''SPOOLPENDIENTE''
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
			)
			
			SET @JuntaSpoolID = SCOPE_IDENTITY() 
		END	
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertaCorteSpoolHistorico]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaCorteSpoolHistorico]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertaCorteSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@SpoolHistoricoID		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	INSERT INTO		dbo.CorteSpoolHistorico
					(
					SpoolHistoricoID
					,CorteSpoolID
					,SpoolID
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
					,SpoolNombre
					,ItemCodeCodigo
					,ItemCodeDescripcionEspanol
					,ItemCodeDescripcionIngles
					,TipoCorte1Codigo
					,TipoCorte2Codigo
					)
	SELECT			@SpoolHistoricoID
					,A.CorteSpoolID
					,A.SpoolID
					,A.ItemCodeID
					,A.TipoCorte1ID
					,A.TipoCorte2ID
					,A.EtiquetaMaterial
					,A.EtiquetaSeccion
					,A.Diametro
					,A.InicioFin
					,A.Cantidad
					,A.Observaciones
					,A.UsuarioModifica
					,A.FechaModificacion
					,B.Nombre as SpoolNombre
					,C.Codigo as ItemCodeCodigo
					,C.DescripcionEspanol as ItemCodeDescripcionEspanol
					,C.DescripcionIngles as ItemCodeDescripcionIngles
					,D.Codigo as TipoCorte1Codigo
					,E.Codigo as TipoCorte2Codigo
	FROM			dbo.CorteSpool A 
	INNER JOIN		dbo.Spool B ON A.SpoolID = B.SpoolID
	INNER JOIN		dbo.ItemCode C ON A.ItemCodeID = C.ItemCodeID
	INNER JOIN		dbo.TipoCorte D ON A.TipoCorte1ID = D.TipoCorteID
	INNER JOIN		dbo.TipoCorte E ON A.TipoCorte2ID = E.TipoCorteID
	LEFT JOIN		dbo.aspnet_Users F ON A.UsuarioModifica = F.UserId
	WHERE			A.SpoolID = @SpoolID

END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertaCorteSpool]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaCorteSpool]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
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
    IF @Tabla = ''SPOOL''
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

    IF @Tabla = ''SPOOLPENDIENTE''
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
' 
END
GO
/****** Object:  StoredProcedure [dbo].[EliminaSpool]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EliminaSpool]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[EliminaSpool]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@Tabla					NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF @Tabla = ''SPOOL''  
		BEGIN
		
			/*
			TIPOS DE ACCESORIOS 
			Tubo = 1, Accessorio = 2 
			*/
		
			DECLARE	@SpoolHistoricoID INT

			EXEC dbo.InsertaSpoolHistorico @SpoolID,@SpoolHistoricoID = @SpoolHistoricoID OUTPUT
			EXEC dbo.InsertaMaterialSpoolHistorico @SpoolID, @SpoolHistoricoID
			EXEC dbo.InsertaJuntaSpoolHistorico @SpoolID, @SpoolHistoricoID
			EXEC dbo.InsertaCorteSpoolHistorico @SpoolID, @SpoolHistoricoID
			
			DELETE FROM dbo.CorteSpool WHERE SpoolID = @SpoolID
			DELETE FROM dbo.JuntaSpool WHERE SpoolID = @SpoolID
			
			UPDATE		A 
			SET			A.InventarioDisponibleCruce = A.InventarioBuenEstado - (A.InventarioCongelado - B.Cantidad)
						,A.InventarioCongelado = A.InventarioCongelado - B.Cantidad 
			FROM		dbo.NumeroUnicoInventario A
			INNER JOIN	(
							SELECT		BA.NumeroUnicoCongeladoID,SUM(BC.Cantidad) AS Cantidad
							FROM		dbo.CongeladoParcial BA
							INNER JOIN	dbo.MaterialSpool BC ON BA.MaterialSpoolID = BC.MaterialSpoolID AND BC.SpoolID = @SpoolID
							INNER JOIN	dbo.ItemCode BD ON BC.ItemCodeID = BD.ItemCodeID 
							GROUP BY	BA.NumeroUnicoCongeladoID
						) B ON A.NumeroUnicoID = B.NumeroUnicoCongeladoID
			
			UPDATE		A 
			SET			A.InventarioDisponibleCruce = A.InventarioBuenEstado - (A.InventarioCongelado - B.Cantidad)
						,A.InventarioCongelado = A.InventarioCongelado - B.Cantidad 
			FROM		dbo.NumeroUnicoSegmento A 
			INNER JOIN	(
							SELECT		BA.NumeroUnicoCongeladoID,BA.SegmentoCongelado,SUM(BC.Cantidad) AS Cantidad			
							FROM		dbo.CongeladoParcial BA
							INNER JOIN	dbo.MaterialSpool BC ON BA.MaterialSpoolID = BC.MaterialSpoolID AND BC.SpoolID = @SpoolID
							INNER JOIN	dbo.ItemCode BD ON BC.ItemCodeID = BD.ItemCodeID AND BD.TipoMaterialID = 1 -- APLICA SOLO PARA TUBOS
							GROUP BY	BA.NumeroUnicoCongeladoID, BA.SegmentoCongelado
						) B ON A.NumeroUnicoID = B.NumeroUnicoCongeladoID AND A.Segmento = B.SegmentoCongelado
			
			DELETE		A
			FROM		dbo.CongeladoParcial A 
			INNER JOIN	dbo.MaterialSpool B ON A.MaterialSpoolID = B.MaterialSpoolID AND B.SpoolID = @SpoolID
			
			DELETE FROM dbo.MaterialSpool WHERE SpoolID = @SpoolID
			DELETE FROM dbo.Spool WHERE SpoolID = @SpoolID

		END
	IF @Tabla = ''SPOOLPENDIENTE''
		BEGIN
		
			DELETE FROM dbo.CorteSpoolPendiente WHERE SpoolPendienteID = @SpoolID
			DELETE FROM dbo.JuntaSpoolPendiente WHERE SpoolPendienteID = @SpoolID
			DELETE FROM dbo.MaterialSpoolPendiente WHERE SpoolPendienteID = @SpoolID
			DELETE FROM dbo.SpoolPendiente WHERE SpoolPendienteID = @SpoolID
		
		END 
	
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CreaSpoolHistorico]    Script Date: 01/10/2012 09:59:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreaSpoolHistorico]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[CreaSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	DECLARE	@SpoolHistoricoID INT

	EXEC dbo.InsertaSpoolHistorico @SpoolID,@SpoolHistoricoID = @SpoolHistoricoID OUTPUT
	EXEC dbo.InsertaMaterialSpoolHistorico @SpoolID, @SpoolHistoricoID
	EXEC dbo.InsertaJuntaSpoolHistorico @SpoolID, @SpoolHistoricoID
	EXEC dbo.InsertaCorteSpoolHistorico @SpoolID, @SpoolHistoricoID
	
	SELECT 	@SpoolHistoricoID
	
END



' 
END
GO
/****** Object:  Default [DF_SpoolPendiente_PendienteDocumental]    Script Date: 01/10/2012 09:59:14 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_SpoolPendiente_PendienteDocumental]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SpoolPendiente_PendienteDocumental]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SpoolPendiente] ADD  CONSTRAINT [DF_SpoolPendiente_PendienteDocumental]  DEFAULT ((0)) FOR [PendienteDocumental]
END


End
GO
/****** Object:  Check [CK_CorteSpoolPendiente_Diametro]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_CorteSpoolPendiente_Diametro]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [CK_CorteSpoolPendiente_Diametro] CHECK  (([Diametro]>(0)))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_CorteSpoolPendiente_Diametro]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente] CHECK CONSTRAINT [CK_CorteSpoolPendiente_Diametro]
GO
/****** Object:  Check [CK_JuntaSpoolPendiente_Diametro]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_JuntaSpoolPendiente_Diametro]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [CK_JuntaSpoolPendiente_Diametro] CHECK  (([Diametro]>(0)))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_JuntaSpoolPendiente_Diametro]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente] CHECK CONSTRAINT [CK_JuntaSpoolPendiente_Diametro]
GO
/****** Object:  Check [CK_MaterialSpoolPendiente_Diametro1]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_MaterialSpoolPendiente_Diametro1]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]'))
ALTER TABLE [dbo].[MaterialSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [CK_MaterialSpoolPendiente_Diametro1] CHECK  (([Diametro1]>(0)))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_MaterialSpoolPendiente_Diametro1]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]'))
ALTER TABLE [dbo].[MaterialSpoolPendiente] CHECK CONSTRAINT [CK_MaterialSpoolPendiente_Diametro1]
GO
/****** Object:  ForeignKey [FK_CorteSpoolHistorico_SpoolHistorico]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolHistorico_SpoolHistorico]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolHistorico]'))
ALTER TABLE [dbo].[CorteSpoolHistorico]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpoolHistorico_SpoolHistorico] FOREIGN KEY([SpoolHistoricoID])
REFERENCES [dbo].[SpoolHistorico] ([SpoolHistoricoID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolHistorico_SpoolHistorico]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolHistorico]'))
ALTER TABLE [dbo].[CorteSpoolHistorico] CHECK CONSTRAINT [FK_CorteSpoolHistorico_SpoolHistorico]
GO
/****** Object:  ForeignKey [FK_CorteSpoolPendiente_aspnet_Users]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpoolPendiente_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente] CHECK CONSTRAINT [FK_CorteSpoolPendiente_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_CorteSpoolPendiente_ItemCode]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_ItemCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpoolPendiente_ItemCode] FOREIGN KEY([ItemCodeID])
REFERENCES [dbo].[ItemCode] ([ItemCodeID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_ItemCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente] CHECK CONSTRAINT [FK_CorteSpoolPendiente_ItemCode]
GO
/****** Object:  ForeignKey [FK_CorteSpoolPendiente_SpoolPendiente]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_SpoolPendiente]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpoolPendiente_SpoolPendiente] FOREIGN KEY([SpoolPendienteID])
REFERENCES [dbo].[SpoolPendiente] ([SpoolPendienteID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_SpoolPendiente]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente] CHECK CONSTRAINT [FK_CorteSpoolPendiente_SpoolPendiente]
GO
/****** Object:  ForeignKey [FK_CorteSpoolPendiente_TipoCorte_Material1]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_TipoCorte_Material1]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpoolPendiente_TipoCorte_Material1] FOREIGN KEY([TipoCorte1ID])
REFERENCES [dbo].[TipoCorte] ([TipoCorteID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_TipoCorte_Material1]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente] CHECK CONSTRAINT [FK_CorteSpoolPendiente_TipoCorte_Material1]
GO
/****** Object:  ForeignKey [FK_CorteSpoolPendiente_TipoCorte_Material2]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_TipoCorte_Material2]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpoolPendiente_TipoCorte_Material2] FOREIGN KEY([TipoCorte2ID])
REFERENCES [dbo].[TipoCorte] ([TipoCorteID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CorteSpoolPendiente_TipoCorte_Material2]') AND parent_object_id = OBJECT_ID(N'[dbo].[CorteSpoolPendiente]'))
ALTER TABLE [dbo].[CorteSpoolPendiente] CHECK CONSTRAINT [FK_CorteSpoolPendiente_TipoCorte_Material2]
GO
/****** Object:  ForeignKey [FK_DTSErrorLog_aspnet_Users]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DTSErrorLog_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[DTSErrorLog]'))
ALTER TABLE [dbo].[DTSErrorLog]  WITH CHECK ADD  CONSTRAINT [FK_DTSErrorLog_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DTSErrorLog_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[DTSErrorLog]'))
ALTER TABLE [dbo].[DTSErrorLog] CHECK CONSTRAINT [FK_DTSErrorLog_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_DTSSummaryLog_aspnet_Users]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DTSSummaryLog_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[DTSSummaryLog]'))
ALTER TABLE [dbo].[DTSSummaryLog]  WITH CHECK ADD  CONSTRAINT [FK_DTSSummaryLog_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DTSSummaryLog_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[DTSSummaryLog]'))
ALTER TABLE [dbo].[DTSSummaryLog] CHECK CONSTRAINT [FK_DTSSummaryLog_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaSpoolHistorico_SpoolHistorico]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolHistorico_SpoolHistorico]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolHistorico]'))
ALTER TABLE [dbo].[JuntaSpoolHistorico]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpoolHistorico_SpoolHistorico] FOREIGN KEY([SpoolHistoricoID])
REFERENCES [dbo].[SpoolHistorico] ([SpoolHistoricoID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolHistorico_SpoolHistorico]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolHistorico]'))
ALTER TABLE [dbo].[JuntaSpoolHistorico] CHECK CONSTRAINT [FK_JuntaSpoolHistorico_SpoolHistorico]
GO
/****** Object:  ForeignKey [FK_JuntaSpoolPendiente_aspnet_Users]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpoolPendiente_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente] CHECK CONSTRAINT [FK_JuntaSpoolPendiente_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaSpoolPendiente_FabArea]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_FabArea]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpoolPendiente_FabArea] FOREIGN KEY([FabAreaID])
REFERENCES [dbo].[FabArea] ([FabAreaID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_FabArea]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente] CHECK CONSTRAINT [FK_JuntaSpoolPendiente_FabArea]
GO
/****** Object:  ForeignKey [FK_JuntaSpoolPendiente_FamiliaAcero_1]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_FamiliaAcero_1]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpoolPendiente_FamiliaAcero_1] FOREIGN KEY([FamiliaAceroMaterial1ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_FamiliaAcero_1]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente] CHECK CONSTRAINT [FK_JuntaSpoolPendiente_FamiliaAcero_1]
GO
/****** Object:  ForeignKey [FK_JuntaSpoolPendiente_FamiliaAcero_2]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_FamiliaAcero_2]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpoolPendiente_FamiliaAcero_2] FOREIGN KEY([FamiliaAceroMaterial2ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_FamiliaAcero_2]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente] CHECK CONSTRAINT [FK_JuntaSpoolPendiente_FamiliaAcero_2]
GO
/****** Object:  ForeignKey [FK_JuntaSpoolPendiente_SpoolPendiente]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_SpoolPendiente]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpoolPendiente_SpoolPendiente] FOREIGN KEY([SpoolPendienteID])
REFERENCES [dbo].[SpoolPendiente] ([SpoolPendienteID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_SpoolPendiente]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente] CHECK CONSTRAINT [FK_JuntaSpoolPendiente_SpoolPendiente]
GO
/****** Object:  ForeignKey [FK_JuntaSpoolPendiente_TipoJunta]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_TipoJunta]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpoolPendiente_TipoJunta] FOREIGN KEY([TipoJuntaID])
REFERENCES [dbo].[TipoJunta] ([TipoJuntaID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_JuntaSpoolPendiente_TipoJunta]') AND parent_object_id = OBJECT_ID(N'[dbo].[JuntaSpoolPendiente]'))
ALTER TABLE [dbo].[JuntaSpoolPendiente] CHECK CONSTRAINT [FK_JuntaSpoolPendiente_TipoJunta]
GO
/****** Object:  ForeignKey [FK_MaterialSpoolHistorico_SpoolHistorico]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MaterialSpoolHistorico_SpoolHistorico]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolHistorico]'))
ALTER TABLE [dbo].[MaterialSpoolHistorico]  WITH CHECK ADD  CONSTRAINT [FK_MaterialSpoolHistorico_SpoolHistorico] FOREIGN KEY([SpoolHistoricoID])
REFERENCES [dbo].[SpoolHistorico] ([SpoolHistoricoID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MaterialSpoolHistorico_SpoolHistorico]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolHistorico]'))
ALTER TABLE [dbo].[MaterialSpoolHistorico] CHECK CONSTRAINT [FK_MaterialSpoolHistorico_SpoolHistorico]
GO
/****** Object:  ForeignKey [FK_MaterialSpoolPendiente_aspnet_Users]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MaterialSpoolPendiente_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]'))
ALTER TABLE [dbo].[MaterialSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_MaterialSpoolPendiente_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MaterialSpoolPendiente_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]'))
ALTER TABLE [dbo].[MaterialSpoolPendiente] CHECK CONSTRAINT [FK_MaterialSpoolPendiente_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_MaterialSpoolPendiente_ItemCode]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MaterialSpoolPendiente_ItemCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]'))
ALTER TABLE [dbo].[MaterialSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_MaterialSpoolPendiente_ItemCode] FOREIGN KEY([ItemCodeID])
REFERENCES [dbo].[ItemCode] ([ItemCodeID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MaterialSpoolPendiente_ItemCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]'))
ALTER TABLE [dbo].[MaterialSpoolPendiente] CHECK CONSTRAINT [FK_MaterialSpoolPendiente_ItemCode]
GO
/****** Object:  ForeignKey [FK_MaterialSpoolPendiente_SpoolPendiente]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MaterialSpoolPendiente_SpoolPendiente]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]'))
ALTER TABLE [dbo].[MaterialSpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_MaterialSpoolPendiente_SpoolPendiente] FOREIGN KEY([SpoolPendienteID])
REFERENCES [dbo].[SpoolPendiente] ([SpoolPendienteID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MaterialSpoolPendiente_SpoolPendiente]') AND parent_object_id = OBJECT_ID(N'[dbo].[MaterialSpoolPendiente]'))
ALTER TABLE [dbo].[MaterialSpoolPendiente] CHECK CONSTRAINT [FK_MaterialSpoolPendiente_SpoolPendiente]
GO
/****** Object:  ForeignKey [FK_SpoolPendiente_aspnet_Users]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SpoolPendiente_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
ALTER TABLE [dbo].[SpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_SpoolPendiente_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SpoolPendiente_aspnet_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
ALTER TABLE [dbo].[SpoolPendiente] CHECK CONSTRAINT [FK_SpoolPendiente_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_SpoolPendiente_FamiliaAcero_1]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SpoolPendiente_FamiliaAcero_1]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
ALTER TABLE [dbo].[SpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_SpoolPendiente_FamiliaAcero_1] FOREIGN KEY([FamiliaAcero1ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SpoolPendiente_FamiliaAcero_1]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
ALTER TABLE [dbo].[SpoolPendiente] CHECK CONSTRAINT [FK_SpoolPendiente_FamiliaAcero_1]
GO
/****** Object:  ForeignKey [FK_SpoolPendiente_FamiliaAcero_2]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SpoolPendiente_FamiliaAcero_2]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
ALTER TABLE [dbo].[SpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_SpoolPendiente_FamiliaAcero_2] FOREIGN KEY([FamiliaAcero2ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SpoolPendiente_FamiliaAcero_2]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
ALTER TABLE [dbo].[SpoolPendiente] CHECK CONSTRAINT [FK_SpoolPendiente_FamiliaAcero_2]
GO
/****** Object:  ForeignKey [FK_SpoolPendiente_Proyecto]    Script Date: 01/10/2012 09:59:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SpoolPendiente_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
ALTER TABLE [dbo].[SpoolPendiente]  WITH CHECK ADD  CONSTRAINT [FK_SpoolPendiente_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SpoolPendiente_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[SpoolPendiente]'))
ALTER TABLE [dbo].[SpoolPendiente] CHECK CONSTRAINT [FK_SpoolPendiente_Proyecto]
GO
