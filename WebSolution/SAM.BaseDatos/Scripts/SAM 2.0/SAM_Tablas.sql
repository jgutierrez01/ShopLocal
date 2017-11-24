/****** Object:  Table [dbo].[TecnicaSoldador]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TecnicaSoldador](
	[TecnicaSoldadorID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TecnicaSoldador] PRIMARY KEY CLUSTERED 
(
	[TecnicaSoldadorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuloSeguimientoSpool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuloSeguimientoSpool](
	[ModuloSeguimientoSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[NombreIngles] [nvarchar](100) NOT NULL,
	[OrdenUI] [tinyint] NOT NULL,
	[NombreTemplateColumn] [nvarchar](50) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ModuloSeguimientoSpool] PRIMARY KEY CLUSTERED 
(
	[ModuloSeguimientoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InspeccionVisualPatioDefecto]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InspeccionVisualPatioDefecto](
	[InspeccionVisualPatioDefecto] [int] IDENTITY(1,1) NOT NULL,
	[InspeccionVisualPatioID] [int] NOT NULL,
	[DefectoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_InspeccionVisualPatioDefecto] PRIMARY KEY CLUSTERED 
(
	[InspeccionVisualPatioDefecto] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InspeccionVisualPatio]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InspeccionVisualPatio](
	[InspeccionVisualPatioID] [int] IDENTITY(1,1) NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[FechaInspeccion] [datetime] NOT NULL,
	[Aprobado] [bit] NOT NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_InspeccionVisualPatio] PRIMARY KEY CLUSTERED 
(
	[InspeccionVisualPatioID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CampoSeguimientoSpool]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CampoSeguimientoSpool](
	[CampoSeguimientoSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[ModuloSeguimientoSpoolID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[NombreIngles] [nvarchar](100) NOT NULL,
	[OrdenUI] [tinyint] NOT NULL,
	[NombreControlUI] [nvarchar](50) NOT NULL,
	[NombreColumnaSp] [nvarchar](50) NOT NULL,
	[DataFormat] [nvarchar](15) NOT NULL,
	[CssColumnaUI] [nvarchar](15) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[AnchoUI] [int] NULL,
 CONSTRAINT [PK_CampoSeguimientoSpool] PRIMARY KEY CLUSTERED 
(
	[CampoSeguimientoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcesoRelleno]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcesoRelleno](
	[ProcesoRellenoID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProcesoRelleno] PRIMARY KEY CLUSTERED 
(
	[ProcesoRellenoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_ProcesoRelleno_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcesoRaiz]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcesoRaiz](
	[ProcesoRaizID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProcesoRaiz] PRIMARY KEY CLUSTERED 
(
	[ProcesoRaizID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_ProcesoRaiz_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoReporteProyecto]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoReporteProyecto](
	[TipoReporteProyectoID] [int] NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[NombreIngles] [nvarchar](150) NULL,
	[OrdenUI] [tinyint] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TipoReporteProyecto] PRIMARY KEY CLUSTERED 
(
	[TipoReporteProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoReporteDimensional]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoReporteDimensional](
	[TipoReporteDimensionalID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[NombreIngles] [nvarchar](100) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TipoReporteDimensional] PRIMARY KEY CLUSTERED 
(
	[TipoReporteDimensionalID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoRechazo]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoRechazo](
	[TipoRechazoID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[NombreIngles] [nvarchar](100) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TipoRechazo] PRIMARY KEY CLUSTERED 
(
	[TipoRechazoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoPrueba]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoPrueba](
	[TipoPruebaID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[NombreIngles] [nvarchar](100) NULL,
	[Categoria] [nvarchar](10) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TipoPrueba] PRIMARY KEY CLUSTERED 
(
	[TipoPruebaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoMovimiento]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoMovimiento](
	[TipoMovimientoID] [int] NOT NULL,
	[EsEntrada] [bit] NOT NULL,
	[EsTransferenciaProcesos] [bit] NOT NULL,
	[ApareceEnSaldos] [bit] NOT NULL,
	[DisponibleMovimientosUI] [bit] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TipoMovimiento] PRIMARY KEY CLUSTERED 
(
	[TipoMovimientoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoMaterial]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoMaterial](
	[TipoMaterialID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ClasificacionMaterial] PRIMARY KEY CLUSTERED 
(
	[TipoMaterialID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoJunta]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoJunta](
	[TipoJuntaID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](10) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[VerificadoPorCalidad] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TipoJunta] PRIMARY KEY CLUSTERED 
(
	[TipoJuntaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_TipoJunta_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoCorte]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoCorte](
	[TipoCorteID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](10) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[VerificadoPorCalidad] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TipoCorte] PRIMARY KEY CLUSTERED 
(
	[TipoCorteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UltimoProceso]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltimoProceso](
	[UltimoProcesoID] [int] NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[NombreIngles] [nvarchar](150) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_UltimoProceso] PRIMARY KEY CLUSTERED 
(
	[UltimoProcesoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patio]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patio](
	[PatioID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[Propietario] [nvarchar](500) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Patio] PRIMARY KEY CLUSTERED 
(
	[PatioID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Perfil]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perfil](
	[PerfilID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Perfil] PRIMARY KEY CLUSTERED 
(
	[PerfilID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeriodoDestajo]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeriodoDestajo](
	[PeriodoDestajoID] [int] IDENTITY(1,1) NOT NULL,
	[Semana] [nvarchar](5) NOT NULL,
	[Anio] [int] NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NOT NULL,
	[CantidadDiasFestivos] [int] NOT NULL,
	[Aprobado] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_PeriodoDestajo] PRIMARY KEY CLUSTERED 
(
	[PeriodoDestajoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuloSeguimientoJunta]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuloSeguimientoJunta](
	[ModuloSeguimientoJuntaID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[NombreIngles] [nvarchar](100) NOT NULL,
	[OrdenUI] [tinyint] NOT NULL,
	[NombreTemplateColumn] [nvarchar](50) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ModuloSeguimientoJunta] PRIMARY KEY CLUSTERED 
(
	[ModuloSeguimientoJuntaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modulo]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulo](
	[ModuloID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Modulo] PRIMARY KEY CLUSTERED 
(
	[ModuloID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Modulo_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FabArea]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FabArea](
	[FabAreaID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[Nombre] [nvarchar](100) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_FabArea] PRIMARY KEY CLUSTERED 
(
	[FabAreaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_FabArea_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Diametro]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diametro](
	[DiametroID] [int] IDENTITY(1,1) NOT NULL,
	[Valor] [decimal](7, 4) NOT NULL,
	[VerificadoPorCalidad] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Diametro] PRIMARY KEY CLUSTERED 
(
	[DiametroID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Diametro_Valor] UNIQUE NONCLUSTERED 
(
	[Valor] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FamiliaMaterial]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FamiliaMaterial](
	[FamiliaMaterialID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_FamiliaMaterial] PRIMARY KEY CLUSTERED 
(
	[FamiliaMaterialID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_FamiliaMaterial_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstatusOrden]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstatusOrden](
	[EstatusOrdenID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_EstatusOrden] PRIMARY KEY CLUSTERED 
(
	[EstatusOrdenID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contacto]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacto](
	[ContactoID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[ApPaterno] [nvarchar](50) NOT NULL,
	[ApMaterno] [nvarchar](50) NULL,
	[CorreoElectronico] [nvarchar](255) NULL,
	[TelefonoOficina] [nvarchar](20) NULL,
	[TelefonoParticular] [nvarchar](20) NULL,
	[TelefonoCelular] [nvarchar](20) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Contacto] PRIMARY KEY CLUSTERED 
(
	[ContactoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[ClienteID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[Direccion] [nvarchar](max) NULL,
	[Ciudad] [nvarchar](50) NULL,
	[Estado] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[ClienteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cedula]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cedula](
	[CedulaID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[Orden] [int] NULL,
	[VerificadoPorCalidad] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Cedula] PRIMARY KEY CLUSTERED 
(
	[CedulaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Cedula_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConceptoEstimacion]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConceptoEstimacion](
	[ConceptoEstimacionID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NULL,
	[NombreIngles] [nvarchar](100) NULL,
	[Orden] [int] NULL,
	[AplicaParaJunta] [bit] NOT NULL,
	[AplicaParaSpool] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ConceptoEstimacion] PRIMARY KEY CLUSTERED 
(
	[ConceptoEstimacionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Color]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Color](
	[ColorID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[CodigoHexadecimal] [nvarchar](7) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Color] PRIMARY KEY CLUSTERED 
(
	[ColorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CampoSeguimientoJunta]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CampoSeguimientoJunta](
	[CampoSeguimientoJuntaID] [int] IDENTITY(1,1) NOT NULL,
	[ModuloSeguimientoJuntaID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[NombreIngles] [nvarchar](100) NOT NULL,
	[OrdenUI] [tinyint] NOT NULL,
	[NombreControlUI] [nvarchar](50) NOT NULL,
	[NombreColumnaSp] [nvarchar](50) NOT NULL,
	[DataFormat] [nvarchar](15) NOT NULL,
	[CssColumnaUI] [nvarchar](15) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[AnchoUI] [int] NULL,
 CONSTRAINT [PK_CampoSeguimientoJunta] PRIMARY KEY CLUSTERED 
(
	[CampoSeguimientoJuntaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaReportePndSector]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaReportePndSector](
	[JuntaReportePndSector] [int] IDENTITY(1,1) NOT NULL,
	[JuntaReportePndID] [int] NOT NULL,
	[Sector] [nvarchar](10) NOT NULL,
	[SectorInicio] [nvarchar](10) NOT NULL,
	[SectorFin] [nvarchar](10) NOT NULL,
	[DefectoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaReportePndSector] PRIMARY KEY CLUSTERED 
(
	[JuntaReportePndSector] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaReportePndCuadrante]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaReportePndCuadrante](
	[JuntaReportePndCuadranteID] [int] IDENTITY(1,1) NOT NULL,
	[JuntaReportePndID] [int] NOT NULL,
	[Cuadrante] [nvarchar](50) NULL,
	[Placa] [nvarchar](50) NULL,
	[DefectoID] [int] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaReportePndCuadrante] PRIMARY KEY CLUSTERED 
(
	[JuntaReportePndCuadranteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaInspeccionVisualDefecto]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaInspeccionVisualDefecto](
	[JuntaInspeccionVisualDefectoID] [int] IDENTITY(1,1) NOT NULL,
	[JuntaInspeccionVisualID] [int] NOT NULL,
	[DefectoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaInspeccionVisualDefecto] PRIMARY KEY CLUSTERED 
(
	[JuntaInspeccionVisualDefectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Defecto]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Defecto](
	[DefectoID] [int] IDENTITY(1,1) NOT NULL,
	[TipoPruebaID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[NombreIngles] [nvarchar](100) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Defecto] PRIMARY KEY CLUSTERED 
(
	[DefectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consumible]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consumible](
	[ConsumibleID] [int] IDENTITY(1,1) NOT NULL,
	[PatioID] [int] NOT NULL,
	[Codigo] [nvarchar](50) NOT NULL,
	[Kilogramos] [int] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Consumible] PRIMARY KEY CLUSTERED 
(
	[ConsumibleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Espesor]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Espesor](
	[EspesorID] [int] IDENTITY(1,1) NOT NULL,
	[DiametroID] [int] NOT NULL,
	[CedulaID] [int] NOT NULL,
	[Valor] [decimal](10, 4) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Espesor] PRIMARY KEY CLUSTERED 
(
	[EspesorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Espesor_DiametroID_CedulaID] UNIQUE NONCLUSTERED 
(
	[DiametroID] ASC,
	[CedulaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FamiliaAcero]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FamiliaAcero](
	[FamiliaAceroID] [int] IDENTITY(1,1) NOT NULL,
	[FamiliaMaterialID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](500) NULL,
	[VerificadoPorCalidad] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_FamiliaAcero] PRIMARY KEY CLUSTERED 
(
	[FamiliaAceroID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_FamiliaAcero_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fabricante]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fabricante](
	[FabricanteID] [int] IDENTITY(1,1) NOT NULL,
	[ContactoID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](50) NULL,
	[Direccion] [nvarchar](max) NULL,
	[Telefono] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Fabricante] PRIMARY KEY CLUSTERED 
(
	[FabricanteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Maquina]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Maquina](
	[MaquinaID] [int] IDENTITY(1,1) NOT NULL,
	[PatioID] [int] NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Maquina] PRIMARY KEY CLUSTERED 
(
	[MaquinaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KgTeorico]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KgTeorico](
	[KgTeoricoID] [int] IDENTITY(1,1) NOT NULL,
	[DiametroID] [int] NOT NULL,
	[CedulaID] [int] NOT NULL,
	[Valor] [decimal](12, 4) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_KgTeorico] PRIMARY KEY CLUSTERED 
(
	[KgTeoricoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permiso]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permiso](
	[PermisoID] [int] IDENTITY(1,1) NOT NULL,
	[ModuloID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[NombreIngles] [nvarchar](50) NULL,
	[Descripcion] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Permiso] PRIMARY KEY CLUSTERED 
(
	[PermisoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Permiso_ModuloID_Nombre] UNIQUE NONCLUSTERED 
(
	[ModuloID] ASC,
	[Nombre] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactoCliente]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactoCliente](
	[ContactoClienteID] [int] IDENTITY(1,1) NOT NULL,
	[ClienteID] [int] NOT NULL,
	[Puesto] [nvarchar](100) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[ApPaterno] [nvarchar](50) NOT NULL,
	[ApMaterno] [nvarchar](50) NULL,
	[CorreoElectronico] [nvarchar](255) NULL,
	[TelefonoOficina] [nvarchar](20) NULL,
	[TelefonoParticular] [nvarchar](20) NULL,
	[TelefonoCelular] [nvarchar](20) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ContactoCliente] PRIMARY KEY CLUSTERED 
(
	[ContactoClienteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proyecto](
	[ProyectoID] [int] IDENTITY(1,1) NOT NULL,
	[PatioID] [int] NOT NULL,
	[ClienteID] [int] NOT NULL,
	[ContactoID] [int] NOT NULL,
	[ColorID] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](500) NULL,
	[FechaInicio] [datetime] NULL,
	[Activo] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Proyecto] PRIMARY KEY CLUSTERED 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Taller]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Taller](
	[TallerID] [int] IDENTITY(1,1) NOT NULL,
	[PatioID] [int] NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Taller] PRIMARY KEY CLUSTERED 
(
	[TallerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UbicacionFisica]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UbicacionFisica](
	[UbicacionFisicaID] [int] IDENTITY(1,1) NOT NULL,
	[PatioID] [int] NOT NULL,
	[Nombre] [nvarchar](200) NOT NULL,
	[EsAreaCorte] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_UbicacionFisica] PRIMARY KEY CLUSTERED 
(
	[UbicacionFisicaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tubero]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tubero](
	[TuberoID] [int] IDENTITY(1,1) NOT NULL,
	[PatioID] [int] NOT NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[NumeroEmpleado] [nvarchar](20) NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[ApPaterno] [nvarchar](50) NOT NULL,
	[ApMaterno] [nvarchar](50) NULL,
	[Activo] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Tubero] PRIMARY KEY CLUSTERED 
(
	[TuberoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaSoldaduraDetalle]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaSoldaduraDetalle](
	[JuntaSoldaduraDetalleID] [int] IDENTITY(1,1) NOT NULL,
	[JuntaSoldaduraID] [int] NOT NULL,
	[SoldadorID] [int] NOT NULL,
	[ConsumibleID] [int] NOT NULL,
	[TecnicaSoldadorID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaSoldaduraDetalle] PRIMARY KEY CLUSTERED 
(
	[JuntaSoldaduraDetalleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_JuntaSoldaduraDetalle_CombinadoConsumible] ON [dbo].[JuntaSoldaduraDetalle] 
(
	[JuntaSoldaduraID] ASC,
	[ConsumibleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_JuntaSoldaduraDetalle_CombinadoSoldadura] ON [dbo].[JuntaSoldaduraDetalle] 
(
	[JuntaSoldaduraID] ASC,
	[SoldadorID] ASC,
	[TecnicaSoldadorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Soldador]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Soldador](
	[SoldadorID] [int] IDENTITY(1,1) NOT NULL,
	[PatioID] [int] NOT NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[NumeroEmpleado] [nvarchar](20) NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[ApPaterno] [nvarchar](50) NOT NULL,
	[ApMaterno] [nvarchar](50) NULL,
	[Activo] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Soldador] PRIMARY KEY CLUSTERED 
(
	[SoldadorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedor]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedor](
	[ProveedorID] [int] IDENTITY(1,1) NOT NULL,
	[ContactoID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](50) NULL,
	[Direccion] [nvarchar](max) NULL,
	[Telefono] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Proveedor] PRIMARY KEY CLUSTERED 
(
	[ProveedorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transportista]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transportista](
	[TransportistaID] [int] IDENTITY(1,1) NOT NULL,
	[ContactoID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](50) NULL,
	[Direccion] [nvarchar](max) NULL,
	[Telefono] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Transportista] PRIMARY KEY CLUSTERED 
(
	[TransportistaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[UserId] [uniqueidentifier] NOT NULL,
	[PerfilID] [int] NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[ApPaterno] [nvarchar](50) NOT NULL,
	[ApMaterno] [nvarchar](50) NULL,
	[Idioma] [nvarchar](5) NOT NULL,
	[BloqueadoPorAdministrador] [bit] NOT NULL,
	[EsAdministradorSistema] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Spool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Spool](
	[SpoolID] [int] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_Spool] PRIMARY KEY CLUSTERED 
(
	[SpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Spool_Nombre_Proyecto] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[Nombre] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Spool_ProyectoID] ON [dbo].[Spool] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaSoldadura]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaSoldadura](
	[JuntaSoldaduraID] [int] IDENTITY(1,1) NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[FechaSoldadura] [datetime] NOT NULL,
	[FechaReporte] [datetime] NOT NULL,
	[TallerID] [int] NOT NULL,
	[ProcesoRellenoID] [int] NULL,
	[ProcesoRaizID] [int] NULL,
	[WpsID] [int] NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaSoldadura] PRIMARY KEY CLUSTERED 
(
	[JuntaSoldaduraID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_JuntaSoldadura_JuntaWorkstatusID] UNIQUE NONCLUSTERED 
(
	[JuntaWorkstatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wps]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wps](
	[WpsID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[MaterialBase1ID] [int] NOT NULL,
	[MaterialBase2ID] [int] NULL,
	[ProcesoRaizID] [int] NOT NULL,
	[ProcesoRellenoID] [int] NOT NULL,
	[EspesorRaizMaximo] [int] NULL,
	[EspesorRellenoMaximo] [int] NULL,
	[RequierePwht] [bit] NOT NULL,
	[RequierePreheat] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Wps] PRIMARY KEY CLUSTERED 
(
	[WpsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioProyecto]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioProyecto](
	[UsuarioProyectoID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_UsuarioProyecto] PRIMARY KEY CLUSTERED 
(
	[UsuarioProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recepcion]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recepcion](
	[RecepcionID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TransportistaID] [int] NOT NULL,
	[FechaRecepcion] [datetime] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Recepcion] PRIMARY KEY CLUSTERED 
(
	[RecepcionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProyectoReporte]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProyectoReporte](
	[ProyectoReporteID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TipoReporteProyectoID] [int] NOT NULL,
	[RutaEspaniol] [nvarchar](max) NOT NULL,
	[RutaIngles] [nvarchar](max) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProyectoReporte] PRIMARY KEY CLUSTERED 
(
	[ProyectoReporteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProyectoNomenclaturaSpool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProyectoNomenclaturaSpool](
	[ProyectoID] [int] NOT NULL,
	[CantidadSegmentosSpool] [int] NOT NULL,
	[SegmentoSpool1] [nvarchar](20) NULL,
	[SegmentoSpool2] [nvarchar](20) NULL,
	[SegmentoSpool3] [nvarchar](20) NULL,
	[SegmentoSpool4] [nvarchar](20) NULL,
	[SegmentoSpool5] [nvarchar](20) NULL,
	[SegmentoSpool6] [nvarchar](20) NULL,
	[SegmentoSpool7] [nvarchar](20) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProyectoNomenclaturaSpool] PRIMARY KEY CLUSTERED 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProyectoDossier]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProyectoDossier](
	[ProyectoID] [int] NOT NULL,
	[ReporteInspeccionVisual] [bit] NOT NULL,
	[ReporteLiberacionDimensional] [bit] NOT NULL,
	[ReporteEspesores] [bit] NOT NULL,
	[ReporteRT] [bit] NOT NULL,
	[ReportePT] [bit] NOT NULL,
	[ReportePwht] [bit] NOT NULL,
	[ReporteDurezas] [bit] NOT NULL,
	[ReporteRTPostTT] [bit] NOT NULL,
	[ReportePTPostTT] [bit] NOT NULL,
	[ReportePreheat] [bit] NOT NULL,
	[ReporteUT] [bit] NOT NULL,
	[ReportesPintura] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProyectoDossier] PRIMARY KEY CLUSTERED 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProyectoConsecutivo]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProyectoConsecutivo](
	[ProyectoID] [int] NOT NULL,
	[ConsecutivoODT] [int] NOT NULL,
	[ConsecutivoNumeroUnico] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProyectoConsecutivo] PRIMARY KEY CLUSTERED 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProyectoConfiguracion]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProyectoConfiguracion](
	[ProyectoID] [int] NOT NULL,
	[PrefijoNumeroUnico] [nvarchar](10) NOT NULL,
	[PrefijoOrdenTrabajo] [nvarchar](10) NOT NULL,
	[DigitosNumeroUnico] [tinyint] NOT NULL,
	[DigitosOrdenTrabajo] [tinyint] NOT NULL,
	[ToleranciaCortes] [int] NULL,
	[AnguloBisel] [nvarchar](20) NULL,
	[CuadroTubero] [money] NOT NULL,
	[CuadroRaiz] [money] NOT NULL,
	[CuadroRelleno] [money] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProyectoConfiguracion] PRIMARY KEY CLUSTERED 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReporteDimensional]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReporteDimensional](
	[ReporteDimensionalID] [int] IDENTITY(1,1) NOT NULL,
	[TipoReporteDimensionalID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[FechaReporte] [datetime] NOT NULL,
	[NumeroReporte] [nvarchar](50) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ReporteDimensional] PRIMARY KEY CLUSTERED 
(
	[ReporteDimensionalID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_ReporteDimensional_ProyectoNumeroTipo] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[NumeroReporte] ASC,
	[TipoReporteDimensionalID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisicionNumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisicionNumeroUnico](
	[RequisicionNumeroUnicoID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[NumeroRequisicion] [nvarchar](50) NULL,
	[FechaRequisicion] [datetime] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_RequisicionNumeroUnico] PRIMARY KEY CLUSTERED 
(
	[RequisicionNumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RequisicionNumeroUnico_Proyecto] ON [dbo].[RequisicionNumeroUnico] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaRequisicion]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaRequisicion](
	[JuntaRequisicionID] [int] IDENTITY(1,1) NOT NULL,
	[RequisicionID] [int] NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaRequisicion] PRIMARY KEY CLUSTERED 
(
	[JuntaRequisicionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Requisicion]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requisicion](
	[RequisicionID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TipoPruebaID] [int] NULL,
	[FechaRequisicion] [datetime] NOT NULL,
	[NumeroRequisicion] [nvarchar](50) NOT NULL,
	[CodigoAsme] [nvarchar](50) NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Requisicion] PRIMARY KEY CLUSTERED 
(
	[RequisicionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Requisicion_ProyectoReporteTipoPrueba] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[NumeroRequisicion] ASC,
	[TipoPruebaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaReporteTt]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaReporteTt](
	[JuntaReporteTtID] [int] IDENTITY(1,1) NOT NULL,
	[ReporteTtID] [int] NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[JuntaRequisicionID] [int] NOT NULL,
	[FechaTratamiento] [datetime] NOT NULL,
	[NumeroGrafica] [nvarchar](20) NULL,
	[Hoja] [int] NULL,
	[Aprobado] [bit] NOT NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaReporteTt] PRIMARY KEY CLUSTERED 
(
	[JuntaReporteTtID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReporteTt]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReporteTt](
	[ReporteTtID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TipoPruebaID] [int] NOT NULL,
	[FechaReporte] [datetime] NOT NULL,
	[NumeroReporte] [nvarchar](50) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ReporteTt] PRIMARY KEY CLUSTERED 
(
	[ReporteTtID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_ReporteTt_ProyectoReporteTipoPrueba] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[TipoPruebaID] ASC,
	[NumeroReporte] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaReportePnd]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaReportePnd](
	[JuntaReportePndID] [int] IDENTITY(1,1) NOT NULL,
	[ReportePndID] [int] NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[JuntaRequisicionID] [int] NOT NULL,
	[TipoRechazoID] [int] NULL,
	[FechaPrueba] [datetime] NOT NULL,
	[Hoja] [int] NULL,
	[Aprobado] [bit] NOT NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaReportePnd] PRIMARY KEY CLUSTERED 
(
	[JuntaReportePndID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportePnd]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportePnd](
	[ReportePndID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TipoPruebaID] [int] NOT NULL,
	[NumeroReporte] [nvarchar](50) NOT NULL,
	[FechaReporte] [datetime] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ReportePnd] PRIMARY KEY CLUSTERED 
(
	[ReportePndID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_ReportePnd_ProyectoReporteTipoPrueba] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[NumeroReporte] ASC,
	[TipoPruebaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisicionPintura]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisicionPintura](
	[RequisicionPinturaID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[FechaRequisicion] [datetime] NOT NULL,
	[NumeroRequisicion] [nvarchar](50) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_RequisicionPintura] PRIMARY KEY CLUSTERED 
(
	[RequisicionPinturaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_RequisicionPintura_ProyectoRequisicion] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[NumeroRequisicion] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransportistaProyecto]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransportistaProyecto](
	[TransportistaProyectoID] [int] IDENTITY(1,1) NOT NULL,
	[TransportistaID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_TransportistaProyecto] PRIMARY KEY CLUSTERED 
(
	[TransportistaProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_TransportistaID_ProyectoID] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[TransportistaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProveedorProyecto]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProveedorProyecto](
	[ProveedorProyectoID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[ProveedorID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProveedorProyecto] PRIMARY KEY CLUSTERED 
(
	[ProveedorProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_ProveedorID_ProyectoID] UNIQUE NONCLUSTERED 
(
	[ProveedorID] ASC,
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PerfilPermiso]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerfilPermiso](
	[PerfilPermisoID] [int] IDENTITY(1,1) NOT NULL,
	[PerfilID] [int] NOT NULL,
	[PermisoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_PerfilPermiso] PRIMARY KEY CLUSTERED 
(
	[PerfilPermisoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_PerfilID_PermisoID] UNIQUE NONCLUSTERED 
(
	[PerfilPermisoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Peq]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Peq](
	[PeqID] [int] IDENTITY(1,1) NOT NULL,
	[FamiliaAceroID] [int] NOT NULL,
	[CedulaID] [int] NOT NULL,
	[DiametroID] [int] NOT NULL,
	[TipoJuntaID] [int] NOT NULL,
	[Equivalencia] [decimal](10, 4) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Peq] PRIMARY KEY CLUSTERED 
(
	[PeqID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonalizacionSeguimientoSpool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonalizacionSeguimientoSpool](
	[PersonalizacionSeguimientoSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](200) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_PersonalizacionSeguimientoSpool] PRIMARY KEY CLUSTERED 
(
	[PersonalizacionSeguimientoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonalizacionSeguimientoJunta]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonalizacionSeguimientoJunta](
	[PersonalizacionSeguimientoJuntaID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](200) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_PersonalizacionSeguimientoJunta] PRIMARY KEY CLUSTERED 
(
	[PersonalizacionSeguimientoJuntaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pagina]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pagina](
	[PaginaID] [int] IDENTITY(1,1) NOT NULL,
	[PermisoID] [int] NOT NULL,
	[Url] [nvarchar](500) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Pagina] PRIMARY KEY CLUSTERED 
(
	[PaginaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenTrabajo]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenTrabajo](
	[OrdenTrabajoID] [int] IDENTITY(1,1) NOT NULL,
	[EstatusOrdenID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TallerID] [int] NOT NULL,
	[NumeroOrden] [nvarchar](50) NULL,
	[FechaOrden] [datetime] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_OrdenTrabajo] PRIMARY KEY CLUSTERED 
(
	[OrdenTrabajoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemCode]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemCode](
	[ItemCodeID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TipoMaterialID] [int] NOT NULL,
	[Codigo] [nvarchar](50) NOT NULL,
	[ItemCodeCliente] [nvarchar](50) NULL,
	[DescripcionEspanol] [nvarchar](150) NOT NULL,
	[DescripcionIngles] [nvarchar](150) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ItemCode] PRIMARY KEY CLUSTERED 
(
	[ItemCodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_ItemCode_ProyectoID_Codigo] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[Codigo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaInspeccionVisual]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaInspeccionVisual](
	[JuntaInspeccionVisualID] [int] IDENTITY(1,1) NOT NULL,
	[InspeccionVisualID] [int] NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[FechaInspeccion] [datetime] NULL,
	[Aprobado] [bit] NOT NULL,
	[Hoja] [int] NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaInspeccionVisual] PRIMARY KEY CLUSTERED 
(
	[JuntaInspeccionVisualID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InspeccionVisual]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InspeccionVisual](
	[InspeccionVisualID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[NumeroReporte] [nvarchar](50) NOT NULL,
	[FechaReporte] [datetime] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_InspeccionVisual] PRIMARY KEY CLUSTERED 
(
	[InspeccionVisualID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_InspeccionVisual_ProyectoID_NumeroReporte] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[NumeroReporte] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FabricanteProyecto]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FabricanteProyecto](
	[FabricanteProyectoID] [int] IDENTITY(1,1) NOT NULL,
	[FabricanteID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_FabricanteProyecto] PRIMARY KEY CLUSTERED 
(
	[FabricanteProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_FabricanteID_ProyectoID] UNIQUE NONCLUSTERED 
(
	[FabricanteID] ASC,
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstimacionJunta]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstimacionJunta](
	[EstimacionJuntaID] [int] IDENTITY(1,1) NOT NULL,
	[EstimacionID] [int] NOT NULL,
	[ConceptoEstimacionID] [int] NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[Valor] [money] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_EstimacionJunta] PRIMARY KEY CLUSTERED 
(
	[EstimacionJuntaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_EstimacionJunta_ConceptoEstimacionID_JuntaWorkstatusID] UNIQUE NONCLUSTERED 
(
	[ConceptoEstimacionID] ASC,
	[JuntaWorkstatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estimacion]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estimacion](
	[EstimacionID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[NumeroEstimacion] [nvarchar](10) NOT NULL,
	[FechaEstimacion] [datetime] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Estimacion] PRIMARY KEY CLUSTERED 
(
	[EstimacionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Estimacion_ProyectoID_NumeroEstimacion] UNIQUE NONCLUSTERED 
(
	[NumeroEstimacion] ASC,
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Estimacion_Proyecto] ON [dbo].[Estimacion] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Embarque]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Embarque](
	[EmbarqueID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[FechaEmbarque] [datetime] NOT NULL,
	[NumeroEmbarque] [nvarchar](50) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Embarque] PRIMARY KEY CLUSTERED 
(
	[EmbarqueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DestajoTuberoDetalle]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DestajoTuberoDetalle](
	[DestajoTuberoDetalleID] [int] IDENTITY(1,1) NOT NULL,
	[DestajoTuberoID] [int] NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[PDIs] [decimal](7, 4) NOT NULL,
	[CostoUnitario] [money] NOT NULL,
	[Destajo] [money] NOT NULL,
	[ProrrateoCuadro] [money] NOT NULL,
	[ProrrateoDiasFestivos] [money] NOT NULL,
	[ProrrateoOtros] [money] NOT NULL,
	[Ajuste] [money] NOT NULL,
	[Total] [money] NOT NULL,
	[Comentarios] [nvarchar](255) NULL,
	[EsDePeriodoAnterior] [bit] NOT NULL,
	[CostoDestajoVacio] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DestajoTuberoDetalle] PRIMARY KEY CLUSTERED 
(
	[DestajoTuberoDetalleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_DestajoTuberoDetalle_JuntaWorkstatusID] UNIQUE NONCLUSTERED 
(
	[JuntaWorkstatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DestajoTubero]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DestajoTubero](
	[DestajoTuberoID] [int] IDENTITY(1,1) NOT NULL,
	[TuberoID] [int] NOT NULL,
	[PeriodoDestajoID] [int] NOT NULL,
	[ReferenciaCuadro] [money] NOT NULL,
	[CantidadDiasFestivos] [int] NOT NULL,
	[CostoDiaFestivo] [money] NOT NULL,
	[TotalDestajo] [money] NOT NULL,
	[TotalCuadro] [money] NOT NULL,
	[TotalDiasFestivos] [money] NOT NULL,
	[TotalOtros] [money] NOT NULL,
	[TotalAjuste] [money] NOT NULL,
	[GranTotal] [money] NOT NULL,
	[Aprobado] [bit] NOT NULL,
	[Comentarios] [nvarchar](255) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DestajoTubero] PRIMARY KEY CLUSTERED 
(
	[DestajoTuberoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DestajoSoldadorDetalle]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DestajoSoldadorDetalle](
	[DestajoSoldadorDetalleID] [int] IDENTITY(1,1) NOT NULL,
	[DestajoSoldadorID] [int] NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[PDIs] [decimal](7, 4) NOT NULL,
	[CostoUnitarioRaiz] [money] NOT NULL,
	[CostoUnitarioRelleno] [money] NOT NULL,
	[RaizDividida] [bit] NOT NULL,
	[RellenoDividido] [bit] NOT NULL,
	[NumeroFondeadores] [tinyint] NOT NULL,
	[NumeroRellenadores] [tinyint] NOT NULL,
	[DestajoRaiz] [money] NOT NULL,
	[DestajoRelleno] [money] NOT NULL,
	[ProrrateoCuadro] [money] NOT NULL,
	[ProrrateoDiasFestivos] [money] NOT NULL,
	[ProrrateoOtros] [money] NOT NULL,
	[Ajuste] [money] NOT NULL,
	[Total] [money] NOT NULL,
	[Comentarios] [nvarchar](255) NULL,
	[EsDePeriodoAnterior] [bit] NOT NULL,
	[CostoRaizVacio] [bit] NOT NULL,
	[CostoRellenoVacio] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DestajoSoldadorDetalle] PRIMARY KEY CLUSTERED 
(
	[DestajoSoldadorDetalleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DestajoSoldador]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DestajoSoldador](
	[DestajoSoldadorID] [int] IDENTITY(1,1) NOT NULL,
	[SoldadorID] [int] NOT NULL,
	[PeriodoDestajoID] [int] NOT NULL,
	[ReferenciaCuadro] [money] NOT NULL,
	[CantidadDiasFestivos] [int] NOT NULL,
	[CostoDiaFestivo] [money] NOT NULL,
	[TotalDestajoRaiz] [money] NOT NULL,
	[TotalDestajoRelleno] [money] NOT NULL,
	[TotalCuadro] [money] NOT NULL,
	[TotalDiasFestivos] [money] NOT NULL,
	[TotalOtros] [money] NOT NULL,
	[TotalAjuste] [money] NOT NULL,
	[GranTotal] [money] NOT NULL,
	[Aprobado] [bit] NOT NULL,
	[Comentarios] [nvarchar](255) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DestajoSoldador] PRIMARY KEY CLUSTERED 
(
	[DestajoSoldadorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Acero]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acero](
	[AceroID] [int] IDENTITY(1,1) NOT NULL,
	[FamiliaAceroID] [int] NOT NULL,
	[Nomenclatura] [nvarchar](50) NOT NULL,
	[VerificadoPorCalidad] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Acero] PRIMARY KEY CLUSTERED 
(
	[AceroID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Acero_Nomenclatura] UNIQUE NONCLUSTERED 
(
	[Nomenclatura] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CostoProcesoRelleno]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CostoProcesoRelleno](
	[CostoProcesoRellenoID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[FamiliaAceroID] [int] NOT NULL,
	[TipoJuntaID] [int] NOT NULL,
	[ProcesoRellenoID] [int] NOT NULL,
	[DiametroID] [int] NOT NULL,
	[CedulaID] [int] NOT NULL,
	[Costo] [money] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_CostoProcesoRelleno] PRIMARY KEY CLUSTERED 
(
	[CostoProcesoRellenoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CostoProcesoRaiz]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CostoProcesoRaiz](
	[CostoProcesoRaizID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[FamiliaAceroID] [int] NOT NULL,
	[TipoJuntaID] [int] NOT NULL,
	[ProcesoRaizID] [int] NOT NULL,
	[DiametroID] [int] NOT NULL,
	[CedulaID] [int] NOT NULL,
	[Costo] [money] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_CostoProcesoRaiz] PRIMARY KEY CLUSTERED 
(
	[CostoProcesoRaizID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CostoArmado]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CostoArmado](
	[CostoArmadoID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[FamiliaAceroID] [int] NOT NULL,
	[TipoJuntaID] [int] NOT NULL,
	[DiametroID] [int] NOT NULL,
	[CedulaID] [int] NOT NULL,
	[Costo] [money] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_CostoArmado] PRIMARY KEY CLUSTERED 
(
	[CostoArmadoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Colada]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Colada](
	[ColadaID] [int] IDENTITY(1,1) NOT NULL,
	[FabricanteID] [int] NULL,
	[AceroID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[NumeroColada] [nvarchar](10) NOT NULL,
	[NumeroCertificado] [nvarchar](20) NULL,
	[HoldCalidad] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Colada] PRIMARY KEY CLUSTERED 
(
	[ColadaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [IX_Colada_Compuesto] UNIQUE NONCLUSTERED 
(
	[FabricanteID] ASC,
	[NumeroCertificado] ASC,
	[NumeroColada] ASC,
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CorteSpool]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CorteSpool](
	[CorteSpoolID] [int] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_CorteSpool] PRIMARY KEY CLUSTERED 
(
	[CorteSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CorteSpool_SpoolID] ON [dbo].[CorteSpool] 
(
	[SpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetallePersonalizacionSeguimientoSpool]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetallePersonalizacionSeguimientoSpool](
	[DetallePersonalizacionSeguimientoSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[PersonalizacionSeguimientoSpoolID] [int] NOT NULL,
	[CampoSeguimientoSpoolID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DetallePersonalizacionSeguimientoSpool] PRIMARY KEY CLUSTERED 
(
	[DetallePersonalizacionSeguimientoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetallePersonalizacionSeguimientoJunta]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetallePersonalizacionSeguimientoJunta](
	[DetallePersonalizacionSeguimientoJuntaID] [int] IDENTITY(1,1) NOT NULL,
	[PersonalizacionSeguimientoJuntaID] [int] NOT NULL,
	[CampoSeguimientoJuntaID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_DetallePersonalizacionSeguimientoJunta] PRIMARY KEY CLUSTERED 
(
	[DetallePersonalizacionSeguimientoJuntaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemCodeEquivalente]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemCodeEquivalente](
	[ItemCodeEquivalenteID] [int] IDENTITY(1,1) NOT NULL,
	[ItemCodeID] [int] NOT NULL,
	[Diametro1] [decimal](7, 4) NOT NULL,
	[Diametro2] [decimal](7, 4) NOT NULL,
	[ItemEquivalenteID] [int] NOT NULL,
	[DiametroEquivalente1] [decimal](7, 4) NOT NULL,
	[DiametroEquivalente2] [decimal](7, 4) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ItemCodeEquivalente] PRIMARY KEY CLUSTERED 
(
	[ItemCodeEquivalenteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_ItemCodeEquivalente_Combinacion] UNIQUE NONCLUSTERED 
(
	[ItemCodeID] ASC,
	[Diametro1] ASC,
	[Diametro2] ASC,
	[ItemEquivalenteID] ASC,
	[DiametroEquivalente1] ASC,
	[DiametroEquivalente2] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaSpool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaSpool](
	[JuntaSpoolID] [int] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_JuntaSpool] PRIMARY KEY CLUSTERED 
(
	[JuntaSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_JuntaSpool_SpoolID_Etiqueta] UNIQUE NONCLUSTERED 
(
	[SpoolID] ASC,
	[Etiqueta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_JuntaSpool_SpoolID] ON [dbo].[JuntaSpool] 
(
	[SpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialSpool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialSpool](
	[MaterialSpoolID] [int] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_MaterialSpool] PRIMARY KEY CLUSTERED 
(
	[MaterialSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_MaterialSpool_SpoolID_Etiqueta] UNIQUE NONCLUSTERED 
(
	[SpoolID] ASC,
	[Etiqueta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_MaterialSpool_SpoolID] ON [dbo].[MaterialSpool] 
(
	[SpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JuntaWorkstatus]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaWorkstatus](
	[JuntaWorkstatusID] [int] IDENTITY(1,1) NOT NULL,
	[OrdenTrabajoSpoolID] [int] NOT NULL,
	[JuntaSpoolID] [int] NOT NULL,
	[EtiquetaJunta] [nvarchar](50) NOT NULL,
	[ArmadoAprobado] [bit] NOT NULL,
	[SoldaduraAprobada] [bit] NOT NULL,
	[InspeccionVisualAprobada] [bit] NOT NULL,
	[JuntaArmadoID] [int] NULL,
	[JuntaSoldaduraID] [int] NULL,
	[JuntaInspeccionVisualID] [int] NULL,
	[VersionJunta] [int] NOT NULL,
	[JuntaWorkstatusAnteriorID] [int] NULL,
	[JuntaFinal] [bit] NOT NULL,
	[UltimoProcesoID] [int] NULL,
	[ArmadoPagado] [bit] NOT NULL,
	[SoldaduraPagada] [bit] NOT NULL,
	[TotalPagadoArmado] [money] NULL,
	[TotalPagadoSoldadura] [money] NULL,
	[TotalPagado] [money] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_JuntaWorkstatus] PRIMARY KEY CLUSTERED 
(
	[JuntaWorkstatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenTrabajoSpool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenTrabajoSpool](
	[OrdenTrabajoSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[OrdenTrabajoID] [int] NOT NULL,
	[SpoolID] [int] NOT NULL,
	[Partida] [int] NOT NULL,
	[NumeroControl] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_OrdenTrabajoSpool] PRIMARY KEY CLUSTERED 
(
	[OrdenTrabajoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_OrdenTrabajoSpool_SpoolID] UNIQUE NONCLUSTERED 
(
	[SpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajoSpool_NumeroControl] ON [dbo].[OrdenTrabajoSpool] 
(
	[NumeroControl] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajoSpool_OrdenTrabajoID] ON [dbo].[OrdenTrabajoSpool] 
(
	[OrdenTrabajoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Solo permite que el spool esté en una ODT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrdenTrabajoSpool', @level2type=N'CONSTRAINT',@level2name=N'UQ_OrdenTrabajoSpool_SpoolID'
GO
/****** Object:  Table [dbo].[SpoolHoldHistorial]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpoolHoldHistorial](
	[SpoolHoldHistorialID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolID] [int] NOT NULL,
	[TipoHold] [nchar](3) NOT NULL,
	[FechaHold] [datetime] NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_SpoolHoldHistorial] PRIMARY KEY CLUSTERED 
(
	[SpoolHoldHistorialID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpoolHold]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpoolHold](
	[SpoolID] [int] NOT NULL,
	[TieneHoldIngenieria] [bit] NOT NULL,
	[TieneHoldCalidad] [bit] NOT NULL,
	[Confinado] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_SpoolHold] PRIMARY KEY CLUSTERED 
(
	[SpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WpsProyecto]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WpsProyecto](
	[WpsProyectoID] [int] IDENTITY(1,1) NOT NULL,
	[WpsID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_WpsProyecto] PRIMARY KEY CLUSTERED 
(
	[WpsProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_WpsID_ProyectoID] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[WpsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wpq]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wpq](
	[WpqID] [int] IDENTITY(1,1) NOT NULL,
	[WpsID] [int] NOT NULL,
	[SoldadorID] [int] NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaVigencia] [datetime] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Wpq] PRIMARY KEY CLUSTERED 
(
	[WpqID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_SoldadorID_WpsID] UNIQUE NONCLUSTERED 
(
	[SoldadorID] ASC,
	[WpsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkstatusSpool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkstatusSpool](
	[WorkstatusSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[OrdenTrabajoSpoolID] [int] NOT NULL,
	[UltimoProcesoID] [int] NULL,
	[TieneLiberacionDimensional] [bit] NOT NULL,
	[TieneRequisicionPintura] [bit] NOT NULL,
	[TienePintura] [bit] NOT NULL,
	[LiberadoPintura] [bit] NOT NULL,
	[Preparado] [bit] NOT NULL,
	[Embarcado] [bit] NOT NULL,
	[Certificado] [bit] NOT NULL,
	[NumeroEtiqueta] [nvarchar](20) NULL,
	[FechaEtiqueta] [datetime] NULL,
	[FechaPreparacion] [datetime] NULL,
	[FechaCertificacion] [datetime] NULL,
	[SistemaPintura] [nvarchar](50) NULL,
	[ColorPintura] [nvarchar](50) NULL,
	[CodigoPintura] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_WorkstatusSpool_1] PRIMARY KEY CLUSTERED 
(
	[WorkstatusSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_WorkstatusSpool_OrdenTrabajoSpoolID] UNIQUE NONCLUSTERED 
(
	[OrdenTrabajoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenTrabajoJunta]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenTrabajoJunta](
	[OrdenTrabajoJuntaID] [int] IDENTITY(1,1) NOT NULL,
	[OrdenTrabajoSpoolID] [int] NOT NULL,
	[JuntaSpoolID] [int] NOT NULL,
	[FueReingenieria] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_OrdenTrabajoJunta] PRIMARY KEY CLUSTERED 
(
	[OrdenTrabajoJuntaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_OrdenTrabajoJunta_JuntaSpoolID] UNIQUE NONCLUSTERED 
(
	[JuntaSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajoJunta_OrdenTrabajoSpoolID] ON [dbo].[OrdenTrabajoJunta] 
(
	[OrdenTrabajoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Solo permite que una junta esté una vez en la tabla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrdenTrabajoJunta', @level2type=N'CONSTRAINT',@level2name=N'UQ_OrdenTrabajoJunta_JuntaSpoolID'
GO
/****** Object:  Table [dbo].[JuntaArmado]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JuntaArmado](
	[JuntaArmadoID] [int] IDENTITY(1,1) NOT NULL,
	[JuntaWorkstatusID] [int] NOT NULL,
	[NumeroUnico1ID] [int] NOT NULL,
	[NumeroUnico2ID] [int] NOT NULL,
	[TallerID] [int] NOT NULL,
	[TuberoID] [int] NOT NULL,
	[FechaArmado] [datetime] NOT NULL,
	[FechaReporte] [datetime] NOT NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NULL,
 CONSTRAINT [PK_JuntaArmado] PRIMARY KEY CLUSTERED 
(
	[JuntaArmadoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_JuntaArmado_JuntaWorkstatusID] UNIQUE NONCLUSTERED 
(
	[JuntaWorkstatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NumeroUnico](
	[NumeroUnicoID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[ItemCodeID] [int] NULL,
	[ColadaID] [int] NULL,
	[ProveedorID] [int] NULL,
	[FabricanteID] [int] NULL,
	[TipoCorte1ID] [int] NULL,
	[TipoCorte2ID] [int] NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[Estatus] [nchar](1) NULL,
	[Factura] [nvarchar](20) NULL,
	[PartidaFactura] [nvarchar](10) NULL,
	[OrdenDeCompra] [nvarchar](20) NULL,
	[PartidaOrdenDeCompra] [nvarchar](10) NULL,
	[Diametro1] [decimal](7, 4) NOT NULL,
	[Diametro2] [decimal](7, 4) NOT NULL,
	[Cedula] [nvarchar](10) NULL,
	[NumeroUnicoCliente] [nvarchar](50) NULL,
	[MarcadoAsme] [bit] NOT NULL,
	[MarcadoGolpe] [bit] NOT NULL,
	[MarcadoPintura] [bit] NOT NULL,
	[PruebasHidrostaticas] [nvarchar](100) NULL,
	[TieneDano] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_NumeroUnico] PRIMARY KEY CLUSTERED 
(
	[NumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_Codigo_ProyectoID] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[Codigo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProyectoID] ON [dbo].[NumeroUnico] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmbarqueSpool]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmbarqueSpool](
	[EmbarqueSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[EmbarqueID] [int] NOT NULL,
	[WorkstatusSpoolID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_EmbarqueSpool] PRIMARY KEY CLUSTERED 
(
	[EmbarqueSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InspeccionDimensionalPatio]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InspeccionDimensionalPatio](
	[InspeccionDimensionalPatioID] [int] IDENTITY(1,1) NOT NULL,
	[WorkstatusSpoolID] [int] NOT NULL,
	[FechaInspeccion] [datetime] NOT NULL,
	[Aprobado] [bit] NOT NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_InspeccionDimensionalPatio] PRIMARY KEY CLUSTERED 
(
	[InspeccionDimensionalPatioID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstimacionSpool]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstimacionSpool](
	[EstimacionSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[EstimacionID] [int] NOT NULL,
	[ConceptoEstimacionID] [int] NOT NULL,
	[WorkstatusSpoolID] [int] NOT NULL,
	[Valor] [money] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_EstimacionSpool] PRIMARY KEY CLUSTERED 
(
	[EstimacionSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_EstimacionSpool_ConceptoEstimacionID_WorkstatusSpoolID] UNIQUE NONCLUSTERED 
(
	[ConceptoEstimacionID] ASC,
	[WorkstatusSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NumeroUnicoSegmento]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NumeroUnicoSegmento](
	[NumeroUnicoSegmentoID] [int] IDENTITY(1,1) NOT NULL,
	[NumeroUnicoID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[Segmento] [nchar](1) NOT NULL,
	[CantidadDanada] [int] NOT NULL,
	[InventarioFisico] [int] NOT NULL,
	[InventarioBuenEstado] [int] NOT NULL,
	[InventarioCongelado] [int] NOT NULL,
	[InventarioTransferenciaCorte] [int] NOT NULL,
	[InventarioDisponibleCruce] [int] NOT NULL,
	[Rack] [int] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_NumeroUnicoSegmento] PRIMARY KEY CLUSTERED 
(
	[NumeroUnicoSegmentoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_NumeroUnicoSegmento_ProyectoID_NumeroUnicoID_Segmento] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC,
	[NumeroUnicoID] ASC,
	[Segmento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_NumeroUnicoSegmento_NumeroUnicoID] ON [dbo].[NumeroUnicoSegmento] 
(
	[NumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_NumeroUnicoSegmento_ProyectoID] ON [dbo].[NumeroUnicoSegmento] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NumeroUnicoMovimiento]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NumeroUnicoMovimiento](
	[NumeroUnicoMovimientoID] [int] IDENTITY(1,1) NOT NULL,
	[NumeroUnicoID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TipoMovimientoID] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Segmento] [nchar](1) NULL,
	[FechaMovimiento] [datetime] NOT NULL,
	[Referencia] [nvarchar](150) NULL,
	[Estatus] [nchar](1) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_NumeroUnicoMovimiento] PRIMARY KEY CLUSTERED 
(
	[NumeroUnicoMovimientoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_NumeroUnicoMovimiento_NumeroUnicoID] ON [dbo].[NumeroUnicoMovimiento] 
(
	[NumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_NumeroUnicoMovimiento_ProyectoID] ON [dbo].[NumeroUnicoMovimiento] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NumeroUnicoInventario]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NumeroUnicoInventario](
	[NumeroUnicoID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[CantidadRecibida] [int] NOT NULL,
	[CantidadDanada] [int] NOT NULL,
	[InventarioFisico] [int] NOT NULL,
	[InventarioBuenEstado] [int] NOT NULL,
	[InventarioCongelado] [int] NOT NULL,
	[InventarioTransferenciaCorte] [int] NOT NULL,
	[InventarioDisponibleCruce] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_NumeroUnicoInventario] PRIMARY KEY CLUSTERED 
(
	[NumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_NumeroUnicoInventario_ProyectoID] ON [dbo].[NumeroUnicoInventario] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisicionPinturaDetalle]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisicionPinturaDetalle](
	[RequisicionPinturaDetalleID] [int] IDENTITY(1,1) NOT NULL,
	[WorkstatusSpoolID] [int] NOT NULL,
	[RequisicionPinturaID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_RequisicionPinturaDetalle] PRIMARY KEY CLUSTERED 
(
	[RequisicionPinturaDetalleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisicionNumeroUnicoDetalle]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisicionNumeroUnicoDetalle](
	[RequisicionNumeroUnicoDetalleID] [int] IDENTITY(1,1) NOT NULL,
	[RequisicionNumeroUnicoID] [int] NOT NULL,
	[NumeroUnicoID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_RequisicionNumeroUnicoDetalle] PRIMARY KEY CLUSTERED 
(
	[RequisicionNumeroUnicoDetalleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_RequisicionNumeroUnicoDetalle_RequisicionNumeroUnicoID_NumeroUnicoID] UNIQUE NONCLUSTERED 
(
	[RequisicionNumeroUnicoID] ASC,
	[NumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReporteDimensionalDetalle]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReporteDimensionalDetalle](
	[ReporteDimensionalDetalleID] [int] IDENTITY(1,1) NOT NULL,
	[ReporteDimensionalID] [int] NOT NULL,
	[WorkstatusSpoolID] [int] NOT NULL,
	[Hoja] [int] NULL,
	[FechaLiberacion] [datetime] NULL,
	[Aprobado] [bit] NOT NULL,
	[Observaciones] [nvarchar](500) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ReporteDimensionalDetalle] PRIMARY KEY CLUSTERED 
(
	[ReporteDimensionalDetalleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecepcionNumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecepcionNumeroUnico](
	[RecepcionNumeroUnicoID] [int] IDENTITY(1,1) NOT NULL,
	[RecepcionID] [int] NOT NULL,
	[NumeroUnicoID] [int] NOT NULL,
	[NumeroUnicoMovimientoID] [int] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_RecepcionNumeroUnico] PRIMARY KEY CLUSTERED 
(
	[RecepcionNumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_RecepcionNumeroUnico_NumeroUnicoID] UNIQUE NONCLUSTERED 
(
	[NumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_RecepcionNumeroUnico_RecepcionID] ON [dbo].[RecepcionNumeroUnico] 
(
	[RecepcionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PinturaSpool]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PinturaSpool](
	[PinturaSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[WorkstatusSpoolID] [int] NOT NULL,
	[RequisicionPinturaDetalleID] [int] NULL,
	[FechaSandblast] [datetime] NULL,
	[ReporteSandblast] [nvarchar](50) NULL,
	[FechaPrimarios] [datetime] NULL,
	[ReportePrimarios] [nvarchar](50) NULL,
	[FechaIntermedios] [datetime] NULL,
	[ReporteIntermedios] [nvarchar](50) NULL,
	[FechaAcabadoVisual] [datetime] NULL,
	[ReporteAcabadoVisual] [nvarchar](50) NULL,
	[FechaAdherencia] [datetime] NULL,
	[ReporteAdherencia] [nvarchar](50) NULL,
	[FechaPullOff] [datetime] NULL,
	[ReportePullOff] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_PinturaSpool] PRIMARY KEY CLUSTERED 
(
	[PinturaSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PinturaNumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PinturaNumeroUnico](
	[PinturaNumeroUnicoID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[NumeroUnicoID] [int] NOT NULL,
	[RequisicionNumeroUnicoDetalleID] [int] NOT NULL,
	[FechaPrimarios] [datetime] NULL,
	[ReportePrimarios] [nvarchar](50) NULL,
	[FechaIntermedio] [datetime] NULL,
	[ReporteIntermedio] [nvarchar](50) NULL,
	[Liberado] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_PinturaNumeroUnico] PRIMARY KEY CLUSTERED 
(
	[PinturaNumeroUnicoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NumeroUnicoCorte]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NumeroUnicoCorte](
	[NumeroUnicoCorteID] [int] IDENTITY(1,1) NOT NULL,
	[NumeroUnicoID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[SalidaMovimientoID] [int] NOT NULL,
	[OrdenTrabajoID] [int] NOT NULL,
	[UbicacionFisicaID] [int] NULL,
	[Segmento] [nchar](1) NOT NULL,
	[Longitud] [int] NOT NULL,
	[FechaTraspaso] [datetime] NOT NULL,
	[TieneCorte] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_NumeroUnicoCorte] PRIMARY KEY CLUSTERED 
(
	[NumeroUnicoCorteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Despacho]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Despacho](
	[DespachoID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[OrdenTrabajoSpoolID] [int] NOT NULL,
	[MaterialSpoolID] [int] NOT NULL,
	[NumeroUnicoID] [int] NOT NULL,
	[SalidaInventarioID] [int] NULL,
	[Segmento] [nchar](1) NULL,
	[EsEquivalente] [bit] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Cancelado] [bit] NOT NULL,
	[FechaDespacho] [datetime] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Despacho] PRIMARY KEY CLUSTERED 
(
	[DespachoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Despacho_MaterialSpoolID] ON [dbo].[Despacho] 
(
	[MaterialSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Despacho_OrdenTrabajoSpoolD] ON [dbo].[Despacho] 
(
	[OrdenTrabajoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Despacho_ProyectoID] ON [dbo].[Despacho] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Corte]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Corte](
	[CorteID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[NumeroUnicoCorteID] [int] NOT NULL,
	[Sobrante] [int] NULL,
	[Merma] [int] NULL,
	[MermaMovimientoID] [int] NULL,
	[PreparacionCorteMovimientoID] [int] NULL,
	[Cancelado] [bit] NOT NULL,
	[Rack] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Corte] PRIMARY KEY CLUSTERED 
(
	[CorteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Corte_ProyectoID] ON [dbo].[Corte] 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CorteDetalle]    Script Date: 02/03/2011 16:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CorteDetalle](
	[CorteDetalleID] [int] IDENTITY(1,1) NOT NULL,
	[CorteID] [int] NOT NULL,
	[OrdenTrabajoSpoolID] [int] NOT NULL,
	[MaterialSpoolID] [int] NOT NULL,
	[SalidaInventarioID] [int] NULL,
	[Cantidad] [int] NOT NULL,
	[FechaCorte] [datetime] NULL,
	[MaquinaID] [int] NULL,
	[Cancelado] [bit] NOT NULL,
	[EsAjuste] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_CorteDetalle] PRIMARY KEY CLUSTERED 
(
	[CorteDetalleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CorteDetalle_MaterialSpoolID] ON [dbo].[CorteDetalle] 
(
	[MaterialSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CorteDetalle_OrdenTrabajoSpoolID] ON [dbo].[CorteDetalle] 
(
	[OrdenTrabajoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenTrabajoMaterial]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenTrabajoMaterial](
	[OrdenTrabajoMaterialID] [int] IDENTITY(1,1) NOT NULL,
	[OrdenTrabajoSpoolID] [int] NOT NULL,
	[MaterialSpoolID] [int] NOT NULL,
	[DespachoID] [int] NULL,
	[CorteDetalleID] [int] NULL,
	[TieneInventarioCongelado] [bit] NOT NULL,
	[NumeroUnicoCongeladoID] [int] NULL,
	[SegmentoCongelado] [nchar](1) NULL,
	[CantidadCongelada] [int] NULL,
	[CongeladoEsEquivalente] [bit] NOT NULL,
	[NumeroUnicoSugeridoID] [int] NULL,
	[SegmentoSugerido] [nchar](1) NULL,
	[SugeridoEsEquivalente] [bit] NOT NULL,
	[TieneCorte] [bit] NULL,
	[TieneDespacho] [bit] NOT NULL,
	[DespachoEsEquivalente] [bit] NOT NULL,
	[NumeroUnicoDespachadoID] [int] NULL,
	[SegmentoDespachado] [nchar](1) NULL,
	[CantidadDespachada] [int] NULL,
	[FueReingenieria] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_OrdenTrabajoMaterial] PRIMARY KEY CLUSTERED 
(
	[OrdenTrabajoMaterialID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY],
 CONSTRAINT [UQ_OrdenTrabajoMaterial_MaterialSpoolID] UNIQUE NONCLUSTERED 
(
	[MaterialSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajoMaterial_OrdenTrabajoSpoolID] ON [dbo].[OrdenTrabajoMaterial] 
(
	[OrdenTrabajoSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Solo permite que un material esté una vez en la tabla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrdenTrabajoMaterial', @level2type=N'CONSTRAINT',@level2name=N'UQ_OrdenTrabajoMaterial_MaterialSpoolID'
GO
/****** Object:  Default [DF_Cedula_VerificadoPorCalidad]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Cedula] ADD  CONSTRAINT [DF_Cedula_VerificadoPorCalidad]  DEFAULT ((0)) FOR [VerificadoPorCalidad]
GO
/****** Object:  Default [DF_Corte_Cancelado]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Corte] ADD  CONSTRAINT [DF_Corte_Cancelado]  DEFAULT ((0)) FOR [Cancelado]
GO
/****** Object:  Default [DF_CorteDetalle_Cancelado]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteDetalle] ADD  CONSTRAINT [DF_CorteDetalle_Cancelado]  DEFAULT ((0)) FOR [Cancelado]
GO
/****** Object:  Default [DF_CorteDetalle_EsAjuste]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteDetalle] ADD  CONSTRAINT [DF_CorteDetalle_EsAjuste]  DEFAULT ((0)) FOR [EsAjuste]
GO
/****** Object:  Default [DF_Despacho_EsEquivalente]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Despacho] ADD  CONSTRAINT [DF_Despacho_EsEquivalente]  DEFAULT ((0)) FOR [EsEquivalente]
GO
/****** Object:  Default [DF_DestajoSoldador_Aprobado]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldador] ADD  CONSTRAINT [DF_DestajoSoldador_Aprobado]  DEFAULT ((0)) FOR [Aprobado]
GO
/****** Object:  Default [DF_DestajoSoldadorDetalle_RaizDividida]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle] ADD  CONSTRAINT [DF_DestajoSoldadorDetalle_RaizDividida]  DEFAULT ((0)) FOR [RaizDividida]
GO
/****** Object:  Default [DF_DestajoSoldadorDetalle_RellenoDividido]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle] ADD  CONSTRAINT [DF_DestajoSoldadorDetalle_RellenoDividido]  DEFAULT ((0)) FOR [RellenoDividido]
GO
/****** Object:  Default [DF_DestajoSoldadorDetalle_EsDePeriodoAnterior]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle] ADD  CONSTRAINT [DF_DestajoSoldadorDetalle_EsDePeriodoAnterior]  DEFAULT ((0)) FOR [EsDePeriodoAnterior]
GO
/****** Object:  Default [DF_DestajoSoldadorDetalle_CostoDestajoVacion]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle] ADD  CONSTRAINT [DF_DestajoSoldadorDetalle_CostoDestajoVacion]  DEFAULT ((0)) FOR [CostoRaizVacio]
GO
/****** Object:  Default [DF_DestajoSoldadorDetalle_CostoRellenoVacio]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle] ADD  CONSTRAINT [DF_DestajoSoldadorDetalle_CostoRellenoVacio]  DEFAULT ((0)) FOR [CostoRellenoVacio]
GO
/****** Object:  Default [DF_DestajoTubero_Aprobado]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTubero] ADD  CONSTRAINT [DF_DestajoTubero_Aprobado]  DEFAULT ((0)) FOR [Aprobado]
GO
/****** Object:  Default [DF_DestajoTuberoDetalle_EsDePeriodoAnterior]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTuberoDetalle] ADD  CONSTRAINT [DF_DestajoTuberoDetalle_EsDePeriodoAnterior]  DEFAULT ((0)) FOR [EsDePeriodoAnterior]
GO
/****** Object:  Default [DF_DestajoTuberoDetalle_CostoDestajoVacio]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTuberoDetalle] ADD  CONSTRAINT [DF_DestajoTuberoDetalle_CostoDestajoVacio]  DEFAULT ((0)) FOR [CostoDestajoVacio]
GO
/****** Object:  Default [DF_Diametro_VerificadoPorCalidad]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Diametro] ADD  CONSTRAINT [DF_Diametro_VerificadoPorCalidad]  DEFAULT ((0)) FOR [VerificadoPorCalidad]
GO
/****** Object:  Default [DF_FamiliaAcero_VerificadoPorCalidad]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[FamiliaAcero] ADD  CONSTRAINT [DF_FamiliaAcero_VerificadoPorCalidad]  DEFAULT ((1)) FOR [VerificadoPorCalidad]
GO
/****** Object:  Default [DF_JuntaInspeccionVisual_Aprobado]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaInspeccionVisual] ADD  CONSTRAINT [DF_JuntaInspeccionVisual_Aprobado]  DEFAULT ((0)) FOR [Aprobado]
GO
/****** Object:  Default [DF_JuntaReportePnd_Aprobado]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePnd] ADD  CONSTRAINT [DF_JuntaReportePnd_Aprobado]  DEFAULT ((0)) FOR [Aprobado]
GO
/****** Object:  Default [DF_JuntaReporteTt_Aprobado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaReporteTt] ADD  CONSTRAINT [DF_JuntaReporteTt_Aprobado]  DEFAULT ((0)) FOR [Aprobado]
GO
/****** Object:  Default [DF_JuntaWorkstatus_JuntaFinal]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus] ADD  CONSTRAINT [DF_JuntaWorkstatus_JuntaFinal]  DEFAULT ((1)) FOR [JuntaFinal]
GO
/****** Object:  Default [DF_JuntaWorkstatus_ArmadoPagado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus] ADD  CONSTRAINT [DF_JuntaWorkstatus_ArmadoPagado]  DEFAULT ((0)) FOR [ArmadoPagado]
GO
/****** Object:  Default [DF_JuntaWorkstatus_SoldaduraPagado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus] ADD  CONSTRAINT [DF_JuntaWorkstatus_SoldaduraPagado]  DEFAULT ((0)) FOR [SoldaduraPagada]
GO
/****** Object:  Default [DF_NumeroUnico_Diametro2]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico] ADD  CONSTRAINT [DF_NumeroUnico_Diametro2]  DEFAULT ((0)) FOR [Diametro2]
GO
/****** Object:  Default [DF_NumeroUnico_MarcadoAsme]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico] ADD  CONSTRAINT [DF_NumeroUnico_MarcadoAsme]  DEFAULT ((0)) FOR [MarcadoAsme]
GO
/****** Object:  Default [DF_NumeroUnico_MarcadoGolpe]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico] ADD  CONSTRAINT [DF_NumeroUnico_MarcadoGolpe]  DEFAULT ((0)) FOR [MarcadoGolpe]
GO
/****** Object:  Default [DF_NumeroUnico_MarcadoPintura]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico] ADD  CONSTRAINT [DF_NumeroUnico_MarcadoPintura]  DEFAULT ((0)) FOR [MarcadoPintura]
GO
/****** Object:  Default [DF_NumeroUnico_TieneDano]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico] ADD  CONSTRAINT [DF_NumeroUnico_TieneDano]  DEFAULT ((0)) FOR [TieneDano]
GO
/****** Object:  Default [DF_NumeroUnicoInventario_CantidadRecibida]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario] ADD  CONSTRAINT [DF_NumeroUnicoInventario_CantidadRecibida]  DEFAULT ((0)) FOR [CantidadRecibida]
GO
/****** Object:  Default [DF_NumeroUnicoInventario_CantidadDanada]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario] ADD  CONSTRAINT [DF_NumeroUnicoInventario_CantidadDanada]  DEFAULT ((0)) FOR [CantidadDanada]
GO
/****** Object:  Default [DF_NumeroUnicoInventario_InventarioFisico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario] ADD  CONSTRAINT [DF_NumeroUnicoInventario_InventarioFisico]  DEFAULT ((0)) FOR [InventarioFisico]
GO
/****** Object:  Default [DF_NumeroUnicoInventario_InventarioBuenEstado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario] ADD  CONSTRAINT [DF_NumeroUnicoInventario_InventarioBuenEstado]  DEFAULT ((0)) FOR [InventarioBuenEstado]
GO
/****** Object:  Default [DF_NumeroUnicoInventario_InventarioCongelado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario] ADD  CONSTRAINT [DF_NumeroUnicoInventario_InventarioCongelado]  DEFAULT ((0)) FOR [InventarioCongelado]
GO
/****** Object:  Default [DF_NumeroUnicoInventario_InventarioTransferenciaCorte]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario] ADD  CONSTRAINT [DF_NumeroUnicoInventario_InventarioTransferenciaCorte]  DEFAULT ((0)) FOR [InventarioTransferenciaCorte]
GO
/****** Object:  Default [DF_NumeroUnicoInventario_InventarioDisponibleCruce]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario] ADD  CONSTRAINT [DF_NumeroUnicoInventario_InventarioDisponibleCruce]  DEFAULT ((0)) FOR [InventarioDisponibleCruce]
GO
/****** Object:  Default [DF_NumeroUnicoSegmento_Segmento]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento] ADD  CONSTRAINT [DF_NumeroUnicoSegmento_Segmento]  DEFAULT (N'A') FOR [Segmento]
GO
/****** Object:  Default [DF_NumeroUnicoSegmento_CantidadDanada]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento] ADD  CONSTRAINT [DF_NumeroUnicoSegmento_CantidadDanada]  DEFAULT ((0)) FOR [CantidadDanada]
GO
/****** Object:  Default [DF_NumeroUnicoSegmento_InventarioFisico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento] ADD  CONSTRAINT [DF_NumeroUnicoSegmento_InventarioFisico]  DEFAULT ((0)) FOR [InventarioFisico]
GO
/****** Object:  Default [DF_NumeroUnicoSegmento_InventarioBuenEstado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento] ADD  CONSTRAINT [DF_NumeroUnicoSegmento_InventarioBuenEstado]  DEFAULT ((0)) FOR [InventarioBuenEstado]
GO
/****** Object:  Default [DF_NumeroUnicoSegmento_InventarioCongelado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento] ADD  CONSTRAINT [DF_NumeroUnicoSegmento_InventarioCongelado]  DEFAULT ((0)) FOR [InventarioCongelado]
GO
/****** Object:  Default [DF_NumeroUnicoSegmento_InventarioTransferenciaCorte]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento] ADD  CONSTRAINT [DF_NumeroUnicoSegmento_InventarioTransferenciaCorte]  DEFAULT ((0)) FOR [InventarioTransferenciaCorte]
GO
/****** Object:  Default [DF_NumeroUnicoSegmento_InventarioDisponibleCruce]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento] ADD  CONSTRAINT [DF_NumeroUnicoSegmento_InventarioDisponibleCruce]  DEFAULT ((0)) FOR [InventarioDisponibleCruce]
GO
/****** Object:  Default [DF_OrdenTrabajoJunta_FueReingenieria]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoJunta] ADD  CONSTRAINT [DF_OrdenTrabajoJunta_FueReingenieria]  DEFAULT ((0)) FOR [FueReingenieria]
GO
/****** Object:  Default [DF_OrdenTrabajoMaterial_CongeladoEsEquivalente]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial] ADD  CONSTRAINT [DF_OrdenTrabajoMaterial_CongeladoEsEquivalente]  DEFAULT ((0)) FOR [CongeladoEsEquivalente]
GO
/****** Object:  Default [DF_OrdenTrabajoMaterial_SugeridoEsEquivalente]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial] ADD  CONSTRAINT [DF_OrdenTrabajoMaterial_SugeridoEsEquivalente]  DEFAULT ((0)) FOR [SugeridoEsEquivalente]
GO
/****** Object:  Default [DF_OrdenTrabajoMaterial_TieneCorte]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial] ADD  CONSTRAINT [DF_OrdenTrabajoMaterial_TieneCorte]  DEFAULT ((0)) FOR [TieneCorte]
GO
/****** Object:  Default [DF_OrdenTrabajoMaterial_DespachoEquivalente]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial] ADD  CONSTRAINT [DF_OrdenTrabajoMaterial_DespachoEquivalente]  DEFAULT ((0)) FOR [DespachoEsEquivalente]
GO
/****** Object:  Default [DF_OrdenTrabajoMaterial_FueReingenieria]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial] ADD  CONSTRAINT [DF_OrdenTrabajoMaterial_FueReingenieria]  DEFAULT ((0)) FOR [FueReingenieria]
GO
/****** Object:  Default [DF_PeriodoDestajo_Cerrado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PeriodoDestajo] ADD  CONSTRAINT [DF_PeriodoDestajo_Cerrado]  DEFAULT ((0)) FOR [Aprobado]
GO
/****** Object:  Default [DF_PinturaNumeroUnico_Liberado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaNumeroUnico] ADD  CONSTRAINT [DF_PinturaNumeroUnico_Liberado]  DEFAULT ((0)) FOR [Liberado]
GO
/****** Object:  Default [DF_Proyecto_Activo]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Proyecto] ADD  CONSTRAINT [DF_Proyecto_Activo]  DEFAULT ((1)) FOR [Activo]
GO
/****** Object:  Default [DF_ProyectoConfiguracion_CuadroTubero]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConfiguracion] ADD  CONSTRAINT [DF_ProyectoConfiguracion_CuadroTubero]  DEFAULT ((0)) FOR [CuadroTubero]
GO
/****** Object:  Default [DF_ProyectoConfiguracion_CuadroRaiz]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConfiguracion] ADD  CONSTRAINT [DF_ProyectoConfiguracion_CuadroRaiz]  DEFAULT ((0)) FOR [CuadroRaiz]
GO
/****** Object:  Default [DF_ProyectoConfiguracion_CuadroRelleno]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConfiguracion] ADD  CONSTRAINT [DF_ProyectoConfiguracion_CuadroRelleno]  DEFAULT ((0)) FOR [CuadroRelleno]
GO
/****** Object:  Default [DF_ProyectoConsecutivo_ConsecutivoODT]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConsecutivo] ADD  CONSTRAINT [DF_ProyectoConsecutivo_ConsecutivoODT]  DEFAULT ((0)) FOR [ConsecutivoODT]
GO
/****** Object:  Default [DF_ProyectoConsecutivo_ConsecutivoNumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConsecutivo] ADD  CONSTRAINT [DF_ProyectoConsecutivo_ConsecutivoNumeroUnico]  DEFAULT ((0)) FOR [ConsecutivoNumeroUnico]
GO
/****** Object:  Default [DF_ProyectoDossier_ReporteInspeccionVisual]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReporteInspeccionVisual]  DEFAULT ((0)) FOR [ReporteInspeccionVisual]
GO
/****** Object:  Default [DF_ProyectoDossier_ReporteLiberacionDimensional]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReporteLiberacionDimensional]  DEFAULT ((0)) FOR [ReporteLiberacionDimensional]
GO
/****** Object:  Default [DF_ProyectoDossier_ReporteEspesores]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReporteEspesores]  DEFAULT ((0)) FOR [ReporteEspesores]
GO
/****** Object:  Default [DF_ProyectoDossier_ReporteRT]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReporteRT]  DEFAULT ((0)) FOR [ReporteRT]
GO
/****** Object:  Default [DF_ProyectoDossier_ReportePT]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReportePT]  DEFAULT ((0)) FOR [ReportePT]
GO
/****** Object:  Default [DF_ProyectoDossier_ReportePwht]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReportePwht]  DEFAULT ((0)) FOR [ReportePwht]
GO
/****** Object:  Default [DF_ProyectoDossier_ReporteDurezas]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReporteDurezas]  DEFAULT ((0)) FOR [ReporteDurezas]
GO
/****** Object:  Default [DF_ProyectoDossier_ReporteRTPostTT]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReporteRTPostTT]  DEFAULT ((0)) FOR [ReporteRTPostTT]
GO
/****** Object:  Default [DF_ProyectoDossier_ReportePTPostTT]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReportePTPostTT]  DEFAULT ((0)) FOR [ReportePTPostTT]
GO
/****** Object:  Default [DF_ProyectoDossier_ReportePreheat]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReportePreheat]  DEFAULT ((0)) FOR [ReportePreheat]
GO
/****** Object:  Default [DF_ProyectoDossier_ReporteUT]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReporteUT]  DEFAULT ((0)) FOR [ReporteUT]
GO
/****** Object:  Default [DF_ProyectoDossier_ReportesPintura]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier] ADD  CONSTRAINT [DF_ProyectoDossier_ReportesPintura]  DEFAULT ((0)) FOR [ReportesPintura]
GO
/****** Object:  Default [DF_Soldador_Activo]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Soldador] ADD  CONSTRAINT [DF_Soldador_Activo]  DEFAULT ((1)) FOR [Activo]
GO
/****** Object:  Default [DF_Spool_PendienteDocumental]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Spool] ADD  CONSTRAINT [DF_Spool_PendienteDocumental]  DEFAULT ((0)) FOR [PendienteDocumental]
GO
/****** Object:  Default [DF_SpoolHold_Confinado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[SpoolHold] ADD  CONSTRAINT [DF_SpoolHold_Confinado]  DEFAULT ((0)) FOR [Confinado]
GO
/****** Object:  Default [DF_TipoCorte_VerificadoPorCalidad]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoCorte] ADD  CONSTRAINT [DF_TipoCorte_VerificadoPorCalidad]  DEFAULT ((1)) FOR [VerificadoPorCalidad]
GO
/****** Object:  Default [DF_TipoJunta_VerificadoPorCalidad]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoJunta] ADD  CONSTRAINT [DF_TipoJunta_VerificadoPorCalidad]  DEFAULT ((1)) FOR [VerificadoPorCalidad]
GO
/****** Object:  Default [DF_TipoMovimiento_EsTransferenciaProcesos]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoMovimiento] ADD  CONSTRAINT [DF_TipoMovimiento_EsTransferenciaProcesos]  DEFAULT ((0)) FOR [EsTransferenciaProcesos]
GO
/****** Object:  Default [DF_TipoMovimiento_ApareceEnSaldos]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoMovimiento] ADD  CONSTRAINT [DF_TipoMovimiento_ApareceEnSaldos]  DEFAULT ((0)) FOR [ApareceEnSaldos]
GO
/****** Object:  Default [DF_TipoMovimiento_DisponibleMovimientosUI]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoMovimiento] ADD  CONSTRAINT [DF_TipoMovimiento_DisponibleMovimientosUI]  DEFAULT ((0)) FOR [DisponibleMovimientosUI]
GO
/****** Object:  Default [DF_Tubero_Activo]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Tubero] ADD  CONSTRAINT [DF_Tubero_Activo]  DEFAULT ((1)) FOR [Activo]
GO
/****** Object:  Default [DF_Usuario_EsAdministradorSistema]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_EsAdministradorSistema]  DEFAULT ((0)) FOR [EsAdministradorSistema]
GO
/****** Object:  Default [DF_Wps_RequierePreheat]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wps] ADD  CONSTRAINT [DF_Wps_RequierePreheat]  DEFAULT ((0)) FOR [RequierePreheat]
GO
/****** Object:  Check [CK_CorteSpool_Diametro]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteSpool]  WITH CHECK ADD  CONSTRAINT [CK_CorteSpool_Diametro] CHECK  (([Diametro]>(0)))
GO
ALTER TABLE [dbo].[CorteSpool] CHECK CONSTRAINT [CK_CorteSpool_Diametro]
GO
/****** Object:  Check [CK_CostoArmado_Costo]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoArmado]  WITH CHECK ADD  CONSTRAINT [CK_CostoArmado_Costo] CHECK  (([Costo]>=(0)))
GO
ALTER TABLE [dbo].[CostoArmado] CHECK CONSTRAINT [CK_CostoArmado_Costo]
GO
/****** Object:  Check [CK_CostoProcesoRaiz_Costo]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [CK_CostoProcesoRaiz_Costo] CHECK  (([Costo]>=(0)))
GO
ALTER TABLE [dbo].[CostoProcesoRaiz] CHECK CONSTRAINT [CK_CostoProcesoRaiz_Costo]
GO
/****** Object:  Check [CK_CostoProcesoRelleno_Costo]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [CK_CostoProcesoRelleno_Costo] CHECK  (([Costo]>=(0)))
GO
ALTER TABLE [dbo].[CostoProcesoRelleno] CHECK CONSTRAINT [CK_CostoProcesoRelleno_Costo]
GO
/****** Object:  Check [CK_Espesor_Valor]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Espesor]  WITH CHECK ADD  CONSTRAINT [CK_Espesor_Valor] CHECK  (([Valor]>(0)))
GO
ALTER TABLE [dbo].[Espesor] CHECK CONSTRAINT [CK_Espesor_Valor]
GO
/****** Object:  Check [CK_JuntaSpool_Diametro]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSpool]  WITH CHECK ADD  CONSTRAINT [CK_JuntaSpool_Diametro] CHECK  (([Diametro]>(0)))
GO
ALTER TABLE [dbo].[JuntaSpool] CHECK CONSTRAINT [CK_JuntaSpool_Diametro]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Se asegura que el diámetro de la junta sea mayor a cero' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JuntaSpool', @level2type=N'CONSTRAINT',@level2name=N'CK_JuntaSpool_Diametro'
GO
/****** Object:  Check [CK_KgTeorico_Valor]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[KgTeorico]  WITH CHECK ADD  CONSTRAINT [CK_KgTeorico_Valor] CHECK  (([Valor]>(0)))
GO
ALTER TABLE [dbo].[KgTeorico] CHECK CONSTRAINT [CK_KgTeorico_Valor]
GO
/****** Object:  Check [CK_MaterialSpool_Diametro1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[MaterialSpool]  WITH CHECK ADD  CONSTRAINT [CK_MaterialSpool_Diametro1] CHECK  (([Diametro1]>(0)))
GO
ALTER TABLE [dbo].[MaterialSpool] CHECK CONSTRAINT [CK_MaterialSpool_Diametro1]
GO
/****** Object:  Check [CK_NumeroUnico_Estatus]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnico_Estatus] CHECK  (([Estatus]='R' OR [Estatus]='C' OR [Estatus]='A'))
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [CK_NumeroUnico_Estatus]
GO
/****** Object:  Check [CK_NumeroUnicoInventario_CantidadDanada]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoInventario_CantidadDanada] CHECK  (([CantidadDanada]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [CK_NumeroUnicoInventario_CantidadDanada]
GO
/****** Object:  Check [CK_NumeroUnicoInventario_CantidadRecibida]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoInventario_CantidadRecibida] CHECK  (([CantidadRecibida]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [CK_NumeroUnicoInventario_CantidadRecibida]
GO
/****** Object:  Check [CK_NumeroUnicoInventario_InventarioBuenEstado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoInventario_InventarioBuenEstado] CHECK  (([InventarioBuenEstado]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [CK_NumeroUnicoInventario_InventarioBuenEstado]
GO
/****** Object:  Check [CK_NumeroUnicoInventario_InventarioCongelado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoInventario_InventarioCongelado] CHECK  (([InventarioCongelado]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [CK_NumeroUnicoInventario_InventarioCongelado]
GO
/****** Object:  Check [CK_NumeroUnicoInventario_InventarioFisico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoInventario_InventarioFisico] CHECK  (([InventarioFisico]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [CK_NumeroUnicoInventario_InventarioFisico]
GO
/****** Object:  Check [CK_NumeroUnicoInventario_InventarioTransferenciaCorte]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoInventario_InventarioTransferenciaCorte] CHECK  (([InventarioTransferenciaCorte]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [CK_NumeroUnicoInventario_InventarioTransferenciaCorte]
GO
/****** Object:  Check [CK_NumeroUnicoMovimiento_Estatus]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoMovimiento]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoMovimiento_Estatus] CHECK  (([Estatus]='C' OR [Estatus]='A'))
GO
ALTER TABLE [dbo].[NumeroUnicoMovimiento] CHECK CONSTRAINT [CK_NumeroUnicoMovimiento_Estatus]
GO
/****** Object:  Check [CK_NumeroUnicoSegmento_InventarioBuenEstado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoSegmento_InventarioBuenEstado] CHECK  (([InventarioBuenEstado]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoSegmento] CHECK CONSTRAINT [CK_NumeroUnicoSegmento_InventarioBuenEstado]
GO
/****** Object:  Check [CK_NumeroUnicoSegmento_InventarioCongelado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoSegmento_InventarioCongelado] CHECK  (([InventarioCongelado]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoSegmento] CHECK CONSTRAINT [CK_NumeroUnicoSegmento_InventarioCongelado]
GO
/****** Object:  Check [CK_NumeroUnicoSegmento_InventarioFisico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoSegmento_InventarioFisico] CHECK  (([InventarioFisico]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoSegmento] CHECK CONSTRAINT [CK_NumeroUnicoSegmento_InventarioFisico]
GO
/****** Object:  Check [CK_NumeroUnicoSegmento_InventarioTransferenciaCorte]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento]  WITH CHECK ADD  CONSTRAINT [CK_NumeroUnicoSegmento_InventarioTransferenciaCorte] CHECK  (([InventarioTransferenciaCorte]>=(0)))
GO
ALTER TABLE [dbo].[NumeroUnicoSegmento] CHECK CONSTRAINT [CK_NumeroUnicoSegmento_InventarioTransferenciaCorte]
GO
/****** Object:  Check [CK_Peq_Equivalencia]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Peq]  WITH CHECK ADD  CONSTRAINT [CK_Peq_Equivalencia] CHECK  (([Equivalencia]>(0)))
GO
ALTER TABLE [dbo].[Peq] CHECK CONSTRAINT [CK_Peq_Equivalencia]
GO
/****** Object:  Check [CK_SpoolHoldHistorial]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[SpoolHoldHistorial]  WITH CHECK ADD  CONSTRAINT [CK_SpoolHoldHistorial] CHECK  (([TipoHold]='ING' OR [TipoHold]='CAL' OR [TipoHold]='CON'))
GO
ALTER TABLE [dbo].[SpoolHoldHistorial] CHECK CONSTRAINT [CK_SpoolHoldHistorial]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Revisa que el tipo de hold sea por Calidad (C) o Ingenieria (I)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SpoolHoldHistorial', @level2type=N'CONSTRAINT',@level2name=N'CK_SpoolHoldHistorial'
GO
/****** Object:  Check [CK_Usuario]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [CK_Usuario] CHECK  (([Idioma]='en-us' OR [Idioma]='es-mx'))
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [CK_Usuario]
GO
/****** Object:  ForeignKey [FK_Acero_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Acero]  WITH CHECK ADD  CONSTRAINT [FK_Acero_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Acero] CHECK CONSTRAINT [FK_Acero_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Acero_FamiliaAcero]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Acero]  WITH CHECK ADD  CONSTRAINT [FK_Acero_FamiliaAcero] FOREIGN KEY([FamiliaAceroID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[Acero] CHECK CONSTRAINT [FK_Acero_FamiliaAcero]
GO
/****** Object:  ForeignKey [FK_CampoSeguimientoJunta_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CampoSeguimientoJunta]  WITH CHECK ADD  CONSTRAINT [FK_CampoSeguimientoJunta_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[CampoSeguimientoJunta] CHECK CONSTRAINT [FK_CampoSeguimientoJunta_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_CampoSeguimientoJunta_ModuloSeguimientoJunta]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CampoSeguimientoJunta]  WITH CHECK ADD  CONSTRAINT [FK_CampoSeguimientoJunta_ModuloSeguimientoJunta] FOREIGN KEY([ModuloSeguimientoJuntaID])
REFERENCES [dbo].[ModuloSeguimientoJunta] ([ModuloSeguimientoJuntaID])
GO
ALTER TABLE [dbo].[CampoSeguimientoJunta] CHECK CONSTRAINT [FK_CampoSeguimientoJunta_ModuloSeguimientoJunta]
GO
/****** Object:  ForeignKey [FK_CampoSeguimientoSpool_ModuloSeguimientoSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CampoSeguimientoSpool]  WITH CHECK ADD  CONSTRAINT [FK_CampoSeguimientoSpool_ModuloSeguimientoSpool] FOREIGN KEY([ModuloSeguimientoSpoolID])
REFERENCES [dbo].[ModuloSeguimientoSpool] ([ModuloSeguimientoSpoolID])
GO
ALTER TABLE [dbo].[CampoSeguimientoSpool] CHECK CONSTRAINT [FK_CampoSeguimientoSpool_ModuloSeguimientoSpool]
GO
/****** Object:  ForeignKey [FK_Cedula_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Cedula]  WITH CHECK ADD  CONSTRAINT [FK_Cedula_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Cedula] CHECK CONSTRAINT [FK_Cedula_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Cliente_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Cliente] CHECK CONSTRAINT [FK_Cliente_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Colada_Acero]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Colada]  WITH CHECK ADD  CONSTRAINT [FK_Colada_Acero] FOREIGN KEY([AceroID])
REFERENCES [dbo].[Acero] ([AceroID])
GO
ALTER TABLE [dbo].[Colada] CHECK CONSTRAINT [FK_Colada_Acero]
GO
/****** Object:  ForeignKey [FK_Colada_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Colada]  WITH CHECK ADD  CONSTRAINT [FK_Colada_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Colada] CHECK CONSTRAINT [FK_Colada_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Colada_Fabricante]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Colada]  WITH CHECK ADD  CONSTRAINT [FK_Colada_Fabricante] FOREIGN KEY([FabricanteID])
REFERENCES [dbo].[Fabricante] ([FabricanteID])
GO
ALTER TABLE [dbo].[Colada] CHECK CONSTRAINT [FK_Colada_Fabricante]
GO
/****** Object:  ForeignKey [FK_Colada_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Colada]  WITH CHECK ADD  CONSTRAINT [FK_Colada_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[Colada] CHECK CONSTRAINT [FK_Colada_Proyecto]
GO
/****** Object:  ForeignKey [FK_Color_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Color]  WITH CHECK ADD  CONSTRAINT [FK_Color_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Color] CHECK CONSTRAINT [FK_Color_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ConceptoEstimacion_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ConceptoEstimacion]  WITH CHECK ADD  CONSTRAINT [FK_ConceptoEstimacion_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ConceptoEstimacion] CHECK CONSTRAINT [FK_ConceptoEstimacion_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Consumible_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Consumible]  WITH CHECK ADD  CONSTRAINT [FK_Consumible_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Consumible] CHECK CONSTRAINT [FK_Consumible_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Consumible_Patio]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Consumible]  WITH CHECK ADD  CONSTRAINT [FK_Consumible_Patio] FOREIGN KEY([PatioID])
REFERENCES [dbo].[Patio] ([PatioID])
GO
ALTER TABLE [dbo].[Consumible] CHECK CONSTRAINT [FK_Consumible_Patio]
GO
/****** Object:  ForeignKey [FK_Contacto_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Contacto]  WITH CHECK ADD  CONSTRAINT [FK_Contacto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Contacto] CHECK CONSTRAINT [FK_Contacto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ContactoCliente_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ContactoCliente]  WITH CHECK ADD  CONSTRAINT [FK_ContactoCliente_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ContactoCliente] CHECK CONSTRAINT [FK_ContactoCliente_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ContactoCliente_Cliente]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ContactoCliente]  WITH CHECK ADD  CONSTRAINT [FK_ContactoCliente_Cliente] FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Cliente] ([ClienteID])
GO
ALTER TABLE [dbo].[ContactoCliente] CHECK CONSTRAINT [FK_ContactoCliente_Cliente]
GO
/****** Object:  ForeignKey [FK_Corte_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Corte]  WITH CHECK ADD  CONSTRAINT [FK_Corte_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Corte] CHECK CONSTRAINT [FK_Corte_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Corte_NumeroUnicoCorte]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Corte]  WITH CHECK ADD  CONSTRAINT [FK_Corte_NumeroUnicoCorte] FOREIGN KEY([NumeroUnicoCorteID])
REFERENCES [dbo].[NumeroUnicoCorte] ([NumeroUnicoCorteID])
GO
ALTER TABLE [dbo].[Corte] CHECK CONSTRAINT [FK_Corte_NumeroUnicoCorte]
GO
/****** Object:  ForeignKey [FK_Corte_NumeroUnicoMovimiento_Merma]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Corte]  WITH CHECK ADD  CONSTRAINT [FK_Corte_NumeroUnicoMovimiento_Merma] FOREIGN KEY([MermaMovimientoID])
REFERENCES [dbo].[NumeroUnicoMovimiento] ([NumeroUnicoMovimientoID])
GO
ALTER TABLE [dbo].[Corte] CHECK CONSTRAINT [FK_Corte_NumeroUnicoMovimiento_Merma]
GO
/****** Object:  ForeignKey [FK_Corte_NumeroUnicoMovimiento_Preparacion]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Corte]  WITH CHECK ADD  CONSTRAINT [FK_Corte_NumeroUnicoMovimiento_Preparacion] FOREIGN KEY([PreparacionCorteMovimientoID])
REFERENCES [dbo].[NumeroUnicoMovimiento] ([NumeroUnicoMovimientoID])
GO
ALTER TABLE [dbo].[Corte] CHECK CONSTRAINT [FK_Corte_NumeroUnicoMovimiento_Preparacion]
GO
/****** Object:  ForeignKey [FK_Corte_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Corte]  WITH CHECK ADD  CONSTRAINT [FK_Corte_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[Corte] CHECK CONSTRAINT [FK_Corte_Proyecto]
GO
/****** Object:  ForeignKey [FK_CorteDetalle_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteDetalle]  WITH CHECK ADD  CONSTRAINT [FK_CorteDetalle_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[CorteDetalle] CHECK CONSTRAINT [FK_CorteDetalle_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_CorteDetalle_Corte]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteDetalle]  WITH CHECK ADD  CONSTRAINT [FK_CorteDetalle_Corte] FOREIGN KEY([CorteID])
REFERENCES [dbo].[Corte] ([CorteID])
GO
ALTER TABLE [dbo].[CorteDetalle] CHECK CONSTRAINT [FK_CorteDetalle_Corte]
GO
/****** Object:  ForeignKey [FK_CorteDetalle_Maquina]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteDetalle]  WITH CHECK ADD  CONSTRAINT [FK_CorteDetalle_Maquina] FOREIGN KEY([MaquinaID])
REFERENCES [dbo].[Maquina] ([MaquinaID])
GO
ALTER TABLE [dbo].[CorteDetalle] CHECK CONSTRAINT [FK_CorteDetalle_Maquina]
GO
/****** Object:  ForeignKey [FK_CorteDetalle_MaterialSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteDetalle]  WITH CHECK ADD  CONSTRAINT [FK_CorteDetalle_MaterialSpool] FOREIGN KEY([MaterialSpoolID])
REFERENCES [dbo].[MaterialSpool] ([MaterialSpoolID])
GO
ALTER TABLE [dbo].[CorteDetalle] CHECK CONSTRAINT [FK_CorteDetalle_MaterialSpool]
GO
/****** Object:  ForeignKey [FK_CorteDetalle_NumeroUnicoMovimiento]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteDetalle]  WITH CHECK ADD  CONSTRAINT [FK_CorteDetalle_NumeroUnicoMovimiento] FOREIGN KEY([SalidaInventarioID])
REFERENCES [dbo].[NumeroUnicoMovimiento] ([NumeroUnicoMovimientoID])
GO
ALTER TABLE [dbo].[CorteDetalle] CHECK CONSTRAINT [FK_CorteDetalle_NumeroUnicoMovimiento]
GO
/****** Object:  ForeignKey [FK_CorteDetalle_OrdenTrabajoSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteDetalle]  WITH CHECK ADD  CONSTRAINT [FK_CorteDetalle_OrdenTrabajoSpool] FOREIGN KEY([OrdenTrabajoSpoolID])
REFERENCES [dbo].[OrdenTrabajoSpool] ([OrdenTrabajoSpoolID])
GO
ALTER TABLE [dbo].[CorteDetalle] CHECK CONSTRAINT [FK_CorteDetalle_OrdenTrabajoSpool]
GO
/****** Object:  ForeignKey [FK_CorteSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteSpool]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[CorteSpool] CHECK CONSTRAINT [FK_CorteSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_CorteSpool_ItemCode]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteSpool]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpool_ItemCode] FOREIGN KEY([ItemCodeID])
REFERENCES [dbo].[ItemCode] ([ItemCodeID])
GO
ALTER TABLE [dbo].[CorteSpool] CHECK CONSTRAINT [FK_CorteSpool_ItemCode]
GO
/****** Object:  ForeignKey [FK_CorteSpool_Spool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteSpool]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpool_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO
ALTER TABLE [dbo].[CorteSpool] CHECK CONSTRAINT [FK_CorteSpool_Spool]
GO
/****** Object:  ForeignKey [FK_CorteSpool_TipoCorte_Material1]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteSpool]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpool_TipoCorte_Material1] FOREIGN KEY([TipoCorte1ID])
REFERENCES [dbo].[TipoCorte] ([TipoCorteID])
GO
ALTER TABLE [dbo].[CorteSpool] CHECK CONSTRAINT [FK_CorteSpool_TipoCorte_Material1]
GO
/****** Object:  ForeignKey [FK_CorteSpool_TipoCorte_Material2]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CorteSpool]  WITH CHECK ADD  CONSTRAINT [FK_CorteSpool_TipoCorte_Material2] FOREIGN KEY([TipoCorte2ID])
REFERENCES [dbo].[TipoCorte] ([TipoCorteID])
GO
ALTER TABLE [dbo].[CorteSpool] CHECK CONSTRAINT [FK_CorteSpool_TipoCorte_Material2]
GO
/****** Object:  ForeignKey [FK_CostoArmado_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoArmado]  WITH CHECK ADD  CONSTRAINT [FK_CostoArmado_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[CostoArmado] CHECK CONSTRAINT [FK_CostoArmado_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_CostoArmado_Cedula]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoArmado]  WITH CHECK ADD  CONSTRAINT [FK_CostoArmado_Cedula] FOREIGN KEY([CedulaID])
REFERENCES [dbo].[Cedula] ([CedulaID])
GO
ALTER TABLE [dbo].[CostoArmado] CHECK CONSTRAINT [FK_CostoArmado_Cedula]
GO
/****** Object:  ForeignKey [FK_CostoArmado_Diametro]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoArmado]  WITH CHECK ADD  CONSTRAINT [FK_CostoArmado_Diametro] FOREIGN KEY([DiametroID])
REFERENCES [dbo].[Diametro] ([DiametroID])
GO
ALTER TABLE [dbo].[CostoArmado] CHECK CONSTRAINT [FK_CostoArmado_Diametro]
GO
/****** Object:  ForeignKey [FK_CostoArmado_FamiliaAcero]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoArmado]  WITH CHECK ADD  CONSTRAINT [FK_CostoArmado_FamiliaAcero] FOREIGN KEY([FamiliaAceroID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[CostoArmado] CHECK CONSTRAINT [FK_CostoArmado_FamiliaAcero]
GO
/****** Object:  ForeignKey [FK_CostoArmado_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoArmado]  WITH CHECK ADD  CONSTRAINT [FK_CostoArmado_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[CostoArmado] CHECK CONSTRAINT [FK_CostoArmado_Proyecto]
GO
/****** Object:  ForeignKey [FK_CostoArmado_TipoJunta]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoArmado]  WITH CHECK ADD  CONSTRAINT [FK_CostoArmado_TipoJunta] FOREIGN KEY([TipoJuntaID])
REFERENCES [dbo].[TipoJunta] ([TipoJuntaID])
GO
ALTER TABLE [dbo].[CostoArmado] CHECK CONSTRAINT [FK_CostoArmado_TipoJunta]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRaiz_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRaiz_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[CostoProcesoRaiz] CHECK CONSTRAINT [FK_CostoProcesoRaiz_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRaiz_Cedula]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRaiz_Cedula] FOREIGN KEY([CedulaID])
REFERENCES [dbo].[Cedula] ([CedulaID])
GO
ALTER TABLE [dbo].[CostoProcesoRaiz] CHECK CONSTRAINT [FK_CostoProcesoRaiz_Cedula]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRaiz_Diametro]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRaiz_Diametro] FOREIGN KEY([DiametroID])
REFERENCES [dbo].[Diametro] ([DiametroID])
GO
ALTER TABLE [dbo].[CostoProcesoRaiz] CHECK CONSTRAINT [FK_CostoProcesoRaiz_Diametro]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRaiz_FamiliaAcero]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRaiz_FamiliaAcero] FOREIGN KEY([FamiliaAceroID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[CostoProcesoRaiz] CHECK CONSTRAINT [FK_CostoProcesoRaiz_FamiliaAcero]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRaiz_ProcesoRaiz]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRaiz_ProcesoRaiz] FOREIGN KEY([ProcesoRaizID])
REFERENCES [dbo].[ProcesoRaiz] ([ProcesoRaizID])
GO
ALTER TABLE [dbo].[CostoProcesoRaiz] CHECK CONSTRAINT [FK_CostoProcesoRaiz_ProcesoRaiz]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRaiz_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRaiz_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[CostoProcesoRaiz] CHECK CONSTRAINT [FK_CostoProcesoRaiz_Proyecto]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRaiz_TipoJunta]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRaiz_TipoJunta] FOREIGN KEY([TipoJuntaID])
REFERENCES [dbo].[TipoJunta] ([TipoJuntaID])
GO
ALTER TABLE [dbo].[CostoProcesoRaiz] CHECK CONSTRAINT [FK_CostoProcesoRaiz_TipoJunta]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRelleno_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRelleno_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[CostoProcesoRelleno] CHECK CONSTRAINT [FK_CostoProcesoRelleno_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRelleno_Cedula]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRelleno_Cedula] FOREIGN KEY([CedulaID])
REFERENCES [dbo].[Cedula] ([CedulaID])
GO
ALTER TABLE [dbo].[CostoProcesoRelleno] CHECK CONSTRAINT [FK_CostoProcesoRelleno_Cedula]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRelleno_Diametro]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRelleno_Diametro] FOREIGN KEY([DiametroID])
REFERENCES [dbo].[Diametro] ([DiametroID])
GO
ALTER TABLE [dbo].[CostoProcesoRelleno] CHECK CONSTRAINT [FK_CostoProcesoRelleno_Diametro]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRelleno_FamiliaAcero]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRelleno_FamiliaAcero] FOREIGN KEY([FamiliaAceroID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[CostoProcesoRelleno] CHECK CONSTRAINT [FK_CostoProcesoRelleno_FamiliaAcero]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRelleno_ProcesoRelleno]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRelleno_ProcesoRelleno] FOREIGN KEY([ProcesoRellenoID])
REFERENCES [dbo].[ProcesoRelleno] ([ProcesoRellenoID])
GO
ALTER TABLE [dbo].[CostoProcesoRelleno] CHECK CONSTRAINT [FK_CostoProcesoRelleno_ProcesoRelleno]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRelleno_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRelleno_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[CostoProcesoRelleno] CHECK CONSTRAINT [FK_CostoProcesoRelleno_Proyecto]
GO
/****** Object:  ForeignKey [FK_CostoProcesoRelleno_TipoJunta]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[CostoProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [FK_CostoProcesoRelleno_TipoJunta] FOREIGN KEY([TipoJuntaID])
REFERENCES [dbo].[TipoJunta] ([TipoJuntaID])
GO
ALTER TABLE [dbo].[CostoProcesoRelleno] CHECK CONSTRAINT [FK_CostoProcesoRelleno_TipoJunta]
GO
/****** Object:  ForeignKey [FK_Defecto_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Defecto]  WITH CHECK ADD  CONSTRAINT [FK_Defecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Defecto] CHECK CONSTRAINT [FK_Defecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Defecto_TipoPrueba]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Defecto]  WITH CHECK ADD  CONSTRAINT [FK_Defecto_TipoPrueba] FOREIGN KEY([TipoPruebaID])
REFERENCES [dbo].[TipoPrueba] ([TipoPruebaID])
GO
ALTER TABLE [dbo].[Defecto] CHECK CONSTRAINT [FK_Defecto_TipoPrueba]
GO
/****** Object:  ForeignKey [FK_Despacho_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Despacho]  WITH CHECK ADD  CONSTRAINT [FK_Despacho_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Despacho] CHECK CONSTRAINT [FK_Despacho_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Despacho_MaterialSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Despacho]  WITH CHECK ADD  CONSTRAINT [FK_Despacho_MaterialSpool] FOREIGN KEY([MaterialSpoolID])
REFERENCES [dbo].[MaterialSpool] ([MaterialSpoolID])
GO
ALTER TABLE [dbo].[Despacho] CHECK CONSTRAINT [FK_Despacho_MaterialSpool]
GO
/****** Object:  ForeignKey [FK_Despacho_NumeroUnico]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Despacho]  WITH CHECK ADD  CONSTRAINT [FK_Despacho_NumeroUnico] FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[Despacho] CHECK CONSTRAINT [FK_Despacho_NumeroUnico]
GO
/****** Object:  ForeignKey [FK_Despacho_NumeroUnicoMovimiento]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Despacho]  WITH CHECK ADD  CONSTRAINT [FK_Despacho_NumeroUnicoMovimiento] FOREIGN KEY([SalidaInventarioID])
REFERENCES [dbo].[NumeroUnicoMovimiento] ([NumeroUnicoMovimientoID])
GO
ALTER TABLE [dbo].[Despacho] CHECK CONSTRAINT [FK_Despacho_NumeroUnicoMovimiento]
GO
/****** Object:  ForeignKey [FK_Despacho_OrdenTrabajoSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Despacho]  WITH CHECK ADD  CONSTRAINT [FK_Despacho_OrdenTrabajoSpool] FOREIGN KEY([OrdenTrabajoSpoolID])
REFERENCES [dbo].[OrdenTrabajoSpool] ([OrdenTrabajoSpoolID])
GO
ALTER TABLE [dbo].[Despacho] CHECK CONSTRAINT [FK_Despacho_OrdenTrabajoSpool]
GO
/****** Object:  ForeignKey [FK_Despacho_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Despacho]  WITH CHECK ADD  CONSTRAINT [FK_Despacho_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[Despacho] CHECK CONSTRAINT [FK_Despacho_Proyecto]
GO
/****** Object:  ForeignKey [FK_DestajoSoldador_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldador]  WITH CHECK ADD  CONSTRAINT [FK_DestajoSoldador_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[DestajoSoldador] CHECK CONSTRAINT [FK_DestajoSoldador_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_DestajoSoldador_PeriodoDestajo]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldador]  WITH CHECK ADD  CONSTRAINT [FK_DestajoSoldador_PeriodoDestajo] FOREIGN KEY([PeriodoDestajoID])
REFERENCES [dbo].[PeriodoDestajo] ([PeriodoDestajoID])
GO
ALTER TABLE [dbo].[DestajoSoldador] CHECK CONSTRAINT [FK_DestajoSoldador_PeriodoDestajo]
GO
/****** Object:  ForeignKey [FK_DestajoSoldador_Soldador]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldador]  WITH CHECK ADD  CONSTRAINT [FK_DestajoSoldador_Soldador] FOREIGN KEY([SoldadorID])
REFERENCES [dbo].[Soldador] ([SoldadorID])
GO
ALTER TABLE [dbo].[DestajoSoldador] CHECK CONSTRAINT [FK_DestajoSoldador_Soldador]
GO
/****** Object:  ForeignKey [FK_DestajoSoldadorDetalle_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DestajoSoldadorDetalle_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[DestajoSoldadorDetalle] CHECK CONSTRAINT [FK_DestajoSoldadorDetalle_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_DestajoSoldadorDetalle_DestajoSoldador]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DestajoSoldadorDetalle_DestajoSoldador] FOREIGN KEY([DestajoSoldadorID])
REFERENCES [dbo].[DestajoSoldador] ([DestajoSoldadorID])
GO
ALTER TABLE [dbo].[DestajoSoldadorDetalle] CHECK CONSTRAINT [FK_DestajoSoldadorDetalle_DestajoSoldador]
GO
/****** Object:  ForeignKey [FK_DestajoSoldadorDetalle_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DestajoSoldadorDetalle_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[DestajoSoldadorDetalle] CHECK CONSTRAINT [FK_DestajoSoldadorDetalle_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_DestajoSoldadorDetalle_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoSoldadorDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DestajoSoldadorDetalle_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[DestajoSoldadorDetalle] CHECK CONSTRAINT [FK_DestajoSoldadorDetalle_Proyecto]
GO
/****** Object:  ForeignKey [FK_DestajoTubero_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTubero]  WITH CHECK ADD  CONSTRAINT [FK_DestajoTubero_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[DestajoTubero] CHECK CONSTRAINT [FK_DestajoTubero_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_DestajoTubero_PeriodoDestajo]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTubero]  WITH CHECK ADD  CONSTRAINT [FK_DestajoTubero_PeriodoDestajo] FOREIGN KEY([PeriodoDestajoID])
REFERENCES [dbo].[PeriodoDestajo] ([PeriodoDestajoID])
GO
ALTER TABLE [dbo].[DestajoTubero] CHECK CONSTRAINT [FK_DestajoTubero_PeriodoDestajo]
GO
/****** Object:  ForeignKey [FK_DestajoTubero_Tubero]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTubero]  WITH CHECK ADD  CONSTRAINT [FK_DestajoTubero_Tubero] FOREIGN KEY([TuberoID])
REFERENCES [dbo].[Tubero] ([TuberoID])
GO
ALTER TABLE [dbo].[DestajoTubero] CHECK CONSTRAINT [FK_DestajoTubero_Tubero]
GO
/****** Object:  ForeignKey [FK_DestajoTuberoDetalle_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTuberoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DestajoTuberoDetalle_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[DestajoTuberoDetalle] CHECK CONSTRAINT [FK_DestajoTuberoDetalle_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_DestajoTuberoDetalle_DestajoTubero]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTuberoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DestajoTuberoDetalle_DestajoTubero] FOREIGN KEY([DestajoTuberoID])
REFERENCES [dbo].[DestajoTubero] ([DestajoTuberoID])
GO
ALTER TABLE [dbo].[DestajoTuberoDetalle] CHECK CONSTRAINT [FK_DestajoTuberoDetalle_DestajoTubero]
GO
/****** Object:  ForeignKey [FK_DestajoTuberoDetalle_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTuberoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DestajoTuberoDetalle_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[DestajoTuberoDetalle] CHECK CONSTRAINT [FK_DestajoTuberoDetalle_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_DestajoTuberoDetalle_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DestajoTuberoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_DestajoTuberoDetalle_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[DestajoTuberoDetalle] CHECK CONSTRAINT [FK_DestajoTuberoDetalle_Proyecto]
GO
/****** Object:  ForeignKey [FK_DetallePersonalizacionSeguimientoJunta_CampoSeguimientoJunta]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DetallePersonalizacionSeguimientoJunta]  WITH CHECK ADD  CONSTRAINT [FK_DetallePersonalizacionSeguimientoJunta_CampoSeguimientoJunta] FOREIGN KEY([CampoSeguimientoJuntaID])
REFERENCES [dbo].[CampoSeguimientoJunta] ([CampoSeguimientoJuntaID])
GO
ALTER TABLE [dbo].[DetallePersonalizacionSeguimientoJunta] CHECK CONSTRAINT [FK_DetallePersonalizacionSeguimientoJunta_CampoSeguimientoJunta]
GO
/****** Object:  ForeignKey [FK_DetallePersonalizacionSeguimientoJunta_PersonalizacionSeguimientoJunta]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DetallePersonalizacionSeguimientoJunta]  WITH CHECK ADD  CONSTRAINT [FK_DetallePersonalizacionSeguimientoJunta_PersonalizacionSeguimientoJunta] FOREIGN KEY([PersonalizacionSeguimientoJuntaID])
REFERENCES [dbo].[PersonalizacionSeguimientoJunta] ([PersonalizacionSeguimientoJuntaID])
GO
ALTER TABLE [dbo].[DetallePersonalizacionSeguimientoJunta] CHECK CONSTRAINT [FK_DetallePersonalizacionSeguimientoJunta_PersonalizacionSeguimientoJunta]
GO
/****** Object:  ForeignKey [FK_DetallePersonalizacionSeguimientoSpool_CampoSeguimientoSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DetallePersonalizacionSeguimientoSpool]  WITH CHECK ADD  CONSTRAINT [FK_DetallePersonalizacionSeguimientoSpool_CampoSeguimientoSpool] FOREIGN KEY([CampoSeguimientoSpoolID])
REFERENCES [dbo].[CampoSeguimientoSpool] ([CampoSeguimientoSpoolID])
GO
ALTER TABLE [dbo].[DetallePersonalizacionSeguimientoSpool] CHECK CONSTRAINT [FK_DetallePersonalizacionSeguimientoSpool_CampoSeguimientoSpool]
GO
/****** Object:  ForeignKey [FK_DetallePersonalizacionSeguimientoSpool_PersonalizacionSeguimientoSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[DetallePersonalizacionSeguimientoSpool]  WITH CHECK ADD  CONSTRAINT [FK_DetallePersonalizacionSeguimientoSpool_PersonalizacionSeguimientoSpool] FOREIGN KEY([PersonalizacionSeguimientoSpoolID])
REFERENCES [dbo].[PersonalizacionSeguimientoSpool] ([PersonalizacionSeguimientoSpoolID])
GO
ALTER TABLE [dbo].[DetallePersonalizacionSeguimientoSpool] CHECK CONSTRAINT [FK_DetallePersonalizacionSeguimientoSpool_PersonalizacionSeguimientoSpool]
GO
/****** Object:  ForeignKey [FK_Diametro_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Diametro]  WITH CHECK ADD  CONSTRAINT [FK_Diametro_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Diametro] CHECK CONSTRAINT [FK_Diametro_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Embarque_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Embarque]  WITH CHECK ADD  CONSTRAINT [FK_Embarque_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Embarque] CHECK CONSTRAINT [FK_Embarque_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Embarque_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Embarque]  WITH CHECK ADD  CONSTRAINT [FK_Embarque_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[Embarque] CHECK CONSTRAINT [FK_Embarque_Proyecto]
GO
/****** Object:  ForeignKey [FK_EmbarqueSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EmbarqueSpool]  WITH CHECK ADD  CONSTRAINT [FK_EmbarqueSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[EmbarqueSpool] CHECK CONSTRAINT [FK_EmbarqueSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_EmbarqueSpool_Embarque]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EmbarqueSpool]  WITH CHECK ADD  CONSTRAINT [FK_EmbarqueSpool_Embarque] FOREIGN KEY([EmbarqueID])
REFERENCES [dbo].[Embarque] ([EmbarqueID])
GO
ALTER TABLE [dbo].[EmbarqueSpool] CHECK CONSTRAINT [FK_EmbarqueSpool_Embarque]
GO
/****** Object:  ForeignKey [FK_EmbarqueSpool_WorkstatusSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EmbarqueSpool]  WITH CHECK ADD  CONSTRAINT [FK_EmbarqueSpool_WorkstatusSpool] FOREIGN KEY([WorkstatusSpoolID])
REFERENCES [dbo].[WorkstatusSpool] ([WorkstatusSpoolID])
GO
ALTER TABLE [dbo].[EmbarqueSpool] CHECK CONSTRAINT [FK_EmbarqueSpool_WorkstatusSpool]
GO
/****** Object:  ForeignKey [FK_Espesor_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Espesor]  WITH CHECK ADD  CONSTRAINT [FK_Espesor_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Espesor] CHECK CONSTRAINT [FK_Espesor_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Espesor_Cedula]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Espesor]  WITH CHECK ADD  CONSTRAINT [FK_Espesor_Cedula] FOREIGN KEY([CedulaID])
REFERENCES [dbo].[Cedula] ([CedulaID])
GO
ALTER TABLE [dbo].[Espesor] CHECK CONSTRAINT [FK_Espesor_Cedula]
GO
/****** Object:  ForeignKey [FK_Espesor_Diametro]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Espesor]  WITH CHECK ADD  CONSTRAINT [FK_Espesor_Diametro] FOREIGN KEY([DiametroID])
REFERENCES [dbo].[Diametro] ([DiametroID])
GO
ALTER TABLE [dbo].[Espesor] CHECK CONSTRAINT [FK_Espesor_Diametro]
GO
/****** Object:  ForeignKey [FK_EstatusOrden_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstatusOrden]  WITH CHECK ADD  CONSTRAINT [FK_EstatusOrden_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[EstatusOrden] CHECK CONSTRAINT [FK_EstatusOrden_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Estimacion_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Estimacion]  WITH CHECK ADD  CONSTRAINT [FK_Estimacion_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Estimacion] CHECK CONSTRAINT [FK_Estimacion_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Estimacion_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Estimacion]  WITH CHECK ADD  CONSTRAINT [FK_Estimacion_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[Estimacion] CHECK CONSTRAINT [FK_Estimacion_Proyecto]
GO
/****** Object:  ForeignKey [FK_EstimacionDetalle_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstimacionJunta]  WITH CHECK ADD  CONSTRAINT [FK_EstimacionDetalle_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[EstimacionJunta] CHECK CONSTRAINT [FK_EstimacionDetalle_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_EstimacionDetalle_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstimacionJunta]  WITH CHECK ADD  CONSTRAINT [FK_EstimacionDetalle_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[EstimacionJunta] CHECK CONSTRAINT [FK_EstimacionDetalle_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_EstimacionJunta_ConceptoEstimacion]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstimacionJunta]  WITH CHECK ADD  CONSTRAINT [FK_EstimacionJunta_ConceptoEstimacion] FOREIGN KEY([ConceptoEstimacionID])
REFERENCES [dbo].[ConceptoEstimacion] ([ConceptoEstimacionID])
GO
ALTER TABLE [dbo].[EstimacionJunta] CHECK CONSTRAINT [FK_EstimacionJunta_ConceptoEstimacion]
GO
/****** Object:  ForeignKey [FK_EstimacionJunta_Estimacion]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstimacionJunta]  WITH CHECK ADD  CONSTRAINT [FK_EstimacionJunta_Estimacion] FOREIGN KEY([EstimacionID])
REFERENCES [dbo].[Estimacion] ([EstimacionID])
GO
ALTER TABLE [dbo].[EstimacionJunta] CHECK CONSTRAINT [FK_EstimacionJunta_Estimacion]
GO
/****** Object:  ForeignKey [FK_EstimacionSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstimacionSpool]  WITH CHECK ADD  CONSTRAINT [FK_EstimacionSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[EstimacionSpool] CHECK CONSTRAINT [FK_EstimacionSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_EstimacionSpool_ConceptoEstimacion]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstimacionSpool]  WITH CHECK ADD  CONSTRAINT [FK_EstimacionSpool_ConceptoEstimacion] FOREIGN KEY([ConceptoEstimacionID])
REFERENCES [dbo].[ConceptoEstimacion] ([ConceptoEstimacionID])
GO
ALTER TABLE [dbo].[EstimacionSpool] CHECK CONSTRAINT [FK_EstimacionSpool_ConceptoEstimacion]
GO
/****** Object:  ForeignKey [FK_EstimacionSpool_Estimacion]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstimacionSpool]  WITH CHECK ADD  CONSTRAINT [FK_EstimacionSpool_Estimacion] FOREIGN KEY([EstimacionID])
REFERENCES [dbo].[Estimacion] ([EstimacionID])
GO
ALTER TABLE [dbo].[EstimacionSpool] CHECK CONSTRAINT [FK_EstimacionSpool_Estimacion]
GO
/****** Object:  ForeignKey [FK_EstimacionSpool_WorkstatusSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[EstimacionSpool]  WITH CHECK ADD  CONSTRAINT [FK_EstimacionSpool_WorkstatusSpool] FOREIGN KEY([WorkstatusSpoolID])
REFERENCES [dbo].[WorkstatusSpool] ([WorkstatusSpoolID])
GO
ALTER TABLE [dbo].[EstimacionSpool] CHECK CONSTRAINT [FK_EstimacionSpool_WorkstatusSpool]
GO
/****** Object:  ForeignKey [FK_FabArea_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[FabArea]  WITH CHECK ADD  CONSTRAINT [FK_FabArea_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[FabArea] CHECK CONSTRAINT [FK_FabArea_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Fabricante_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Fabricante]  WITH CHECK ADD  CONSTRAINT [FK_Fabricante_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Fabricante] CHECK CONSTRAINT [FK_Fabricante_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Fabricante_Contacto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[Fabricante]  WITH CHECK ADD  CONSTRAINT [FK_Fabricante_Contacto] FOREIGN KEY([ContactoID])
REFERENCES [dbo].[Contacto] ([ContactoID])
GO
ALTER TABLE [dbo].[Fabricante] CHECK CONSTRAINT [FK_Fabricante_Contacto]
GO
/****** Object:  ForeignKey [FK_FabricanteProyecto_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[FabricanteProyecto]  WITH CHECK ADD  CONSTRAINT [FK_FabricanteProyecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[FabricanteProyecto] CHECK CONSTRAINT [FK_FabricanteProyecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_FabricanteProyecto_Fabricante]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[FabricanteProyecto]  WITH CHECK ADD  CONSTRAINT [FK_FabricanteProyecto_Fabricante] FOREIGN KEY([FabricanteID])
REFERENCES [dbo].[Fabricante] ([FabricanteID])
GO
ALTER TABLE [dbo].[FabricanteProyecto] CHECK CONSTRAINT [FK_FabricanteProyecto_Fabricante]
GO
/****** Object:  ForeignKey [FK_FabricanteProyecto_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[FabricanteProyecto]  WITH CHECK ADD  CONSTRAINT [FK_FabricanteProyecto_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[FabricanteProyecto] CHECK CONSTRAINT [FK_FabricanteProyecto_Proyecto]
GO
/****** Object:  ForeignKey [FK_FamiliaAcero_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[FamiliaAcero]  WITH CHECK ADD  CONSTRAINT [FK_FamiliaAcero_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[FamiliaAcero] CHECK CONSTRAINT [FK_FamiliaAcero_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_FamiliaAcero_FamiliaMaterial]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[FamiliaAcero]  WITH CHECK ADD  CONSTRAINT [FK_FamiliaAcero_FamiliaMaterial] FOREIGN KEY([FamiliaMaterialID])
REFERENCES [dbo].[FamiliaMaterial] ([FamiliaMaterialID])
GO
ALTER TABLE [dbo].[FamiliaAcero] CHECK CONSTRAINT [FK_FamiliaAcero_FamiliaMaterial]
GO
/****** Object:  ForeignKey [FK_FamiliaMaterial_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[FamiliaMaterial]  WITH CHECK ADD  CONSTRAINT [FK_FamiliaMaterial_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[FamiliaMaterial] CHECK CONSTRAINT [FK_FamiliaMaterial_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_InspeccionDimensionalPatio_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[InspeccionDimensionalPatio]  WITH CHECK ADD  CONSTRAINT [FK_InspeccionDimensionalPatio_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[InspeccionDimensionalPatio] CHECK CONSTRAINT [FK_InspeccionDimensionalPatio_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_InspeccionDimensionalPatio_WorkstatusSpool]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[InspeccionDimensionalPatio]  WITH CHECK ADD  CONSTRAINT [FK_InspeccionDimensionalPatio_WorkstatusSpool] FOREIGN KEY([WorkstatusSpoolID])
REFERENCES [dbo].[WorkstatusSpool] ([WorkstatusSpoolID])
GO
ALTER TABLE [dbo].[InspeccionDimensionalPatio] CHECK CONSTRAINT [FK_InspeccionDimensionalPatio_WorkstatusSpool]
GO
/****** Object:  ForeignKey [FK_InspeccionVisual_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[InspeccionVisual]  WITH CHECK ADD  CONSTRAINT [FK_InspeccionVisual_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[InspeccionVisual] CHECK CONSTRAINT [FK_InspeccionVisual_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_InspeccionVisual_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[InspeccionVisual]  WITH CHECK ADD  CONSTRAINT [FK_InspeccionVisual_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[InspeccionVisual] CHECK CONSTRAINT [FK_InspeccionVisual_Proyecto]
GO
/****** Object:  ForeignKey [FK_InspeccionVisualPatio_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[InspeccionVisualPatio]  WITH CHECK ADD  CONSTRAINT [FK_InspeccionVisualPatio_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[InspeccionVisualPatio] CHECK CONSTRAINT [FK_InspeccionVisualPatio_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_InspeccionVisualPatio_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[InspeccionVisualPatio]  WITH CHECK ADD  CONSTRAINT [FK_InspeccionVisualPatio_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[InspeccionVisualPatio] CHECK CONSTRAINT [FK_InspeccionVisualPatio_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_InspeccionVisualPatioDefecto_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[InspeccionVisualPatioDefecto]  WITH CHECK ADD  CONSTRAINT [FK_InspeccionVisualPatioDefecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[InspeccionVisualPatioDefecto] CHECK CONSTRAINT [FK_InspeccionVisualPatioDefecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_InspeccionVisualPatioDefecto_InspeccionVisualPatio]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[InspeccionVisualPatioDefecto]  WITH CHECK ADD  CONSTRAINT [FK_InspeccionVisualPatioDefecto_InspeccionVisualPatio] FOREIGN KEY([InspeccionVisualPatioID])
REFERENCES [dbo].[InspeccionVisualPatio] ([InspeccionVisualPatioID])
GO
ALTER TABLE [dbo].[InspeccionVisualPatioDefecto] CHECK CONSTRAINT [FK_InspeccionVisualPatioDefecto_InspeccionVisualPatio]
GO
/****** Object:  ForeignKey [FK_ItemCode_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ItemCode]  WITH CHECK ADD  CONSTRAINT [FK_ItemCode_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ItemCode] CHECK CONSTRAINT [FK_ItemCode_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ItemCode_Proyecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ItemCode]  WITH CHECK ADD  CONSTRAINT [FK_ItemCode_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ItemCode] CHECK CONSTRAINT [FK_ItemCode_Proyecto]
GO
/****** Object:  ForeignKey [FK_ItemCode_TipoMaterial]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ItemCode]  WITH CHECK ADD  CONSTRAINT [FK_ItemCode_TipoMaterial] FOREIGN KEY([TipoMaterialID])
REFERENCES [dbo].[TipoMaterial] ([TipoMaterialID])
GO
ALTER TABLE [dbo].[ItemCode] CHECK CONSTRAINT [FK_ItemCode_TipoMaterial]
GO
/****** Object:  ForeignKey [FK_ItemCodeEquivalente_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ItemCodeEquivalente]  WITH CHECK ADD  CONSTRAINT [FK_ItemCodeEquivalente_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ItemCodeEquivalente] CHECK CONSTRAINT [FK_ItemCodeEquivalente_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ItemCodeEquivalente_ItemCode_ItemBase]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ItemCodeEquivalente]  WITH CHECK ADD  CONSTRAINT [FK_ItemCodeEquivalente_ItemCode_ItemBase] FOREIGN KEY([ItemCodeID])
REFERENCES [dbo].[ItemCode] ([ItemCodeID])
GO
ALTER TABLE [dbo].[ItemCodeEquivalente] CHECK CONSTRAINT [FK_ItemCodeEquivalente_ItemCode_ItemBase]
GO
/****** Object:  ForeignKey [FK_ItemCodeEquivalente_ItemCode_ItemEquivalente]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[ItemCodeEquivalente]  WITH CHECK ADD  CONSTRAINT [FK_ItemCodeEquivalente_ItemCode_ItemEquivalente] FOREIGN KEY([ItemEquivalenteID])
REFERENCES [dbo].[ItemCode] ([ItemCodeID])
GO
ALTER TABLE [dbo].[ItemCodeEquivalente] CHECK CONSTRAINT [FK_ItemCodeEquivalente_ItemCode_ItemEquivalente]
GO
/****** Object:  ForeignKey [FK_JuntaArmado_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaArmado]  WITH CHECK ADD  CONSTRAINT [FK_JuntaArmado_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaArmado] CHECK CONSTRAINT [FK_JuntaArmado_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaArmado_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaArmado]  WITH CHECK ADD  CONSTRAINT [FK_JuntaArmado_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[JuntaArmado] CHECK CONSTRAINT [FK_JuntaArmado_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_JuntaArmado_NumeroUnico_Numero1]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaArmado]  WITH CHECK ADD  CONSTRAINT [FK_JuntaArmado_NumeroUnico_Numero1] FOREIGN KEY([NumeroUnico1ID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[JuntaArmado] CHECK CONSTRAINT [FK_JuntaArmado_NumeroUnico_Numero1]
GO
/****** Object:  ForeignKey [FK_JuntaArmado_NumeroUnico_Numero2]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaArmado]  WITH CHECK ADD  CONSTRAINT [FK_JuntaArmado_NumeroUnico_Numero2] FOREIGN KEY([NumeroUnico2ID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[JuntaArmado] CHECK CONSTRAINT [FK_JuntaArmado_NumeroUnico_Numero2]
GO
/****** Object:  ForeignKey [FK_JuntaArmado_Taller]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaArmado]  WITH CHECK ADD  CONSTRAINT [FK_JuntaArmado_Taller] FOREIGN KEY([TallerID])
REFERENCES [dbo].[Taller] ([TallerID])
GO
ALTER TABLE [dbo].[JuntaArmado] CHECK CONSTRAINT [FK_JuntaArmado_Taller]
GO
/****** Object:  ForeignKey [FK_JuntaArmado_Tubero]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaArmado]  WITH CHECK ADD  CONSTRAINT [FK_JuntaArmado_Tubero] FOREIGN KEY([TuberoID])
REFERENCES [dbo].[Tubero] ([TuberoID])
GO
ALTER TABLE [dbo].[JuntaArmado] CHECK CONSTRAINT [FK_JuntaArmado_Tubero]
GO
/****** Object:  ForeignKey [FK_JuntaInspeccionVisual_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaInspeccionVisual]  WITH CHECK ADD  CONSTRAINT [FK_JuntaInspeccionVisual_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaInspeccionVisual] CHECK CONSTRAINT [FK_JuntaInspeccionVisual_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaInspeccionVisual_InspeccionVisual]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaInspeccionVisual]  WITH CHECK ADD  CONSTRAINT [FK_JuntaInspeccionVisual_InspeccionVisual] FOREIGN KEY([InspeccionVisualID])
REFERENCES [dbo].[InspeccionVisual] ([InspeccionVisualID])
GO
ALTER TABLE [dbo].[JuntaInspeccionVisual] CHECK CONSTRAINT [FK_JuntaInspeccionVisual_InspeccionVisual]
GO
/****** Object:  ForeignKey [FK_JuntaInspeccionVisual_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaInspeccionVisual]  WITH CHECK ADD  CONSTRAINT [FK_JuntaInspeccionVisual_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[JuntaInspeccionVisual] CHECK CONSTRAINT [FK_JuntaInspeccionVisual_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_JuntaInspeccionVisualDefecto_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaInspeccionVisualDefecto]  WITH CHECK ADD  CONSTRAINT [FK_JuntaInspeccionVisualDefecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaInspeccionVisualDefecto] CHECK CONSTRAINT [FK_JuntaInspeccionVisualDefecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaInspeccionVisualDefecto_Defecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaInspeccionVisualDefecto]  WITH CHECK ADD  CONSTRAINT [FK_JuntaInspeccionVisualDefecto_Defecto] FOREIGN KEY([DefectoID])
REFERENCES [dbo].[Defecto] ([DefectoID])
GO
ALTER TABLE [dbo].[JuntaInspeccionVisualDefecto] CHECK CONSTRAINT [FK_JuntaInspeccionVisualDefecto_Defecto]
GO
/****** Object:  ForeignKey [FK_JuntaInspeccionVisualDefecto_JuntaInspeccionVisual]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaInspeccionVisualDefecto]  WITH CHECK ADD  CONSTRAINT [FK_JuntaInspeccionVisualDefecto_JuntaInspeccionVisual] FOREIGN KEY([JuntaInspeccionVisualID])
REFERENCES [dbo].[JuntaInspeccionVisual] ([JuntaInspeccionVisualID])
GO
ALTER TABLE [dbo].[JuntaInspeccionVisualDefecto] CHECK CONSTRAINT [FK_JuntaInspeccionVisualDefecto_JuntaInspeccionVisual]
GO
/****** Object:  ForeignKey [FK_JuntaReportePnd_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePnd]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePnd_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaReportePnd] CHECK CONSTRAINT [FK_JuntaReportePnd_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaReportePnd_JuntaRequisicion]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePnd]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePnd_JuntaRequisicion] FOREIGN KEY([JuntaRequisicionID])
REFERENCES [dbo].[JuntaRequisicion] ([JuntaRequisicionID])
GO
ALTER TABLE [dbo].[JuntaReportePnd] CHECK CONSTRAINT [FK_JuntaReportePnd_JuntaRequisicion]
GO
/****** Object:  ForeignKey [FK_JuntaReportePnd_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePnd]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePnd_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[JuntaReportePnd] CHECK CONSTRAINT [FK_JuntaReportePnd_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_JuntaReportePnd_ReportePnd]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePnd]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePnd_ReportePnd] FOREIGN KEY([ReportePndID])
REFERENCES [dbo].[ReportePnd] ([ReportePndID])
GO
ALTER TABLE [dbo].[JuntaReportePnd] CHECK CONSTRAINT [FK_JuntaReportePnd_ReportePnd]
GO
/****** Object:  ForeignKey [FK_JuntaReportePnd_TipoRechazo]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePnd]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePnd_TipoRechazo] FOREIGN KEY([TipoRechazoID])
REFERENCES [dbo].[TipoRechazo] ([TipoRechazoID])
GO
ALTER TABLE [dbo].[JuntaReportePnd] CHECK CONSTRAINT [FK_JuntaReportePnd_TipoRechazo]
GO
/****** Object:  ForeignKey [FK_JuntaReportePndCuadrante_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePndCuadrante]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePndCuadrante_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaReportePndCuadrante] CHECK CONSTRAINT [FK_JuntaReportePndCuadrante_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaReportePndCuadrante_Defecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePndCuadrante]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePndCuadrante_Defecto] FOREIGN KEY([DefectoID])
REFERENCES [dbo].[Defecto] ([DefectoID])
GO
ALTER TABLE [dbo].[JuntaReportePndCuadrante] CHECK CONSTRAINT [FK_JuntaReportePndCuadrante_Defecto]
GO
/****** Object:  ForeignKey [FK_JuntaReportePndCuadrante_JuntaReportePnd]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePndCuadrante]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePndCuadrante_JuntaReportePnd] FOREIGN KEY([JuntaReportePndID])
REFERENCES [dbo].[JuntaReportePnd] ([JuntaReportePndID])
GO
ALTER TABLE [dbo].[JuntaReportePndCuadrante] CHECK CONSTRAINT [FK_JuntaReportePndCuadrante_JuntaReportePnd]
GO
/****** Object:  ForeignKey [FK_JuntaReportePndSector_aspnet_Users]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePndSector]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePndSector_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaReportePndSector] CHECK CONSTRAINT [FK_JuntaReportePndSector_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaReportePndSector_Defecto]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePndSector]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePndSector_Defecto] FOREIGN KEY([DefectoID])
REFERENCES [dbo].[Defecto] ([DefectoID])
GO
ALTER TABLE [dbo].[JuntaReportePndSector] CHECK CONSTRAINT [FK_JuntaReportePndSector_Defecto]
GO
/****** Object:  ForeignKey [FK_JuntaReportePndSector_JuntaReportePnd]    Script Date: 02/03/2011 16:09:49 ******/
ALTER TABLE [dbo].[JuntaReportePndSector]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReportePndSector_JuntaReportePnd] FOREIGN KEY([JuntaReportePndID])
REFERENCES [dbo].[JuntaReportePnd] ([JuntaReportePndID])
GO
ALTER TABLE [dbo].[JuntaReportePndSector] CHECK CONSTRAINT [FK_JuntaReportePndSector_JuntaReportePnd]
GO
/****** Object:  ForeignKey [FK_JuntaReporteTt_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaReporteTt]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReporteTt_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaReporteTt] CHECK CONSTRAINT [FK_JuntaReporteTt_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaReporteTt_JuntaRequisicion]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaReporteTt]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReporteTt_JuntaRequisicion] FOREIGN KEY([JuntaRequisicionID])
REFERENCES [dbo].[JuntaRequisicion] ([JuntaRequisicionID])
GO
ALTER TABLE [dbo].[JuntaReporteTt] CHECK CONSTRAINT [FK_JuntaReporteTt_JuntaRequisicion]
GO
/****** Object:  ForeignKey [FK_JuntaReporteTt_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaReporteTt]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReporteTt_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[JuntaReporteTt] CHECK CONSTRAINT [FK_JuntaReporteTt_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_JuntaReporteTt_ReporteTt]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaReporteTt]  WITH CHECK ADD  CONSTRAINT [FK_JuntaReporteTt_ReporteTt] FOREIGN KEY([ReporteTtID])
REFERENCES [dbo].[ReporteTt] ([ReporteTtID])
GO
ALTER TABLE [dbo].[JuntaReporteTt] CHECK CONSTRAINT [FK_JuntaReporteTt_ReporteTt]
GO
/****** Object:  ForeignKey [FK_JuntaRequisicion_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaRequisicion]  WITH CHECK ADD  CONSTRAINT [FK_JuntaRequisicion_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaRequisicion] CHECK CONSTRAINT [FK_JuntaRequisicion_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaRequisicion_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaRequisicion]  WITH CHECK ADD  CONSTRAINT [FK_JuntaRequisicion_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[JuntaRequisicion] CHECK CONSTRAINT [FK_JuntaRequisicion_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_JuntaRequisicion_Requisicion]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaRequisicion]  WITH CHECK ADD  CONSTRAINT [FK_JuntaRequisicion_Requisicion] FOREIGN KEY([RequisicionID])
REFERENCES [dbo].[Requisicion] ([RequisicionID])
GO
ALTER TABLE [dbo].[JuntaRequisicion] CHECK CONSTRAINT [FK_JuntaRequisicion_Requisicion]
GO
/****** Object:  ForeignKey [FK_JuntaSoldadura_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldadura]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldadura_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaSoldadura] CHECK CONSTRAINT [FK_JuntaSoldadura_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaSoldadura_JuntaWorkstatus]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldadura]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldadura_JuntaWorkstatus] FOREIGN KEY([JuntaWorkstatusID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[JuntaSoldadura] CHECK CONSTRAINT [FK_JuntaSoldadura_JuntaWorkstatus]
GO
/****** Object:  ForeignKey [FK_JuntaSoldadura_ProcesoRaiz]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldadura]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldadura_ProcesoRaiz] FOREIGN KEY([ProcesoRaizID])
REFERENCES [dbo].[ProcesoRaiz] ([ProcesoRaizID])
GO
ALTER TABLE [dbo].[JuntaSoldadura] CHECK CONSTRAINT [FK_JuntaSoldadura_ProcesoRaiz]
GO
/****** Object:  ForeignKey [FK_JuntaSoldadura_ProcesoRelleno]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldadura]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldadura_ProcesoRelleno] FOREIGN KEY([ProcesoRellenoID])
REFERENCES [dbo].[ProcesoRelleno] ([ProcesoRellenoID])
GO
ALTER TABLE [dbo].[JuntaSoldadura] CHECK CONSTRAINT [FK_JuntaSoldadura_ProcesoRelleno]
GO
/****** Object:  ForeignKey [FK_JuntaSoldadura_Taller]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldadura]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldadura_Taller] FOREIGN KEY([TallerID])
REFERENCES [dbo].[Taller] ([TallerID])
GO
ALTER TABLE [dbo].[JuntaSoldadura] CHECK CONSTRAINT [FK_JuntaSoldadura_Taller]
GO
/****** Object:  ForeignKey [FK_JuntaSoldadura_Wps]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldadura]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldadura_Wps] FOREIGN KEY([WpsID])
REFERENCES [dbo].[Wps] ([WpsID])
GO
ALTER TABLE [dbo].[JuntaSoldadura] CHECK CONSTRAINT [FK_JuntaSoldadura_Wps]
GO
/****** Object:  ForeignKey [FK_JuntaSoldaduraDetalle_Consumible]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldaduraDetalle]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldaduraDetalle_Consumible] FOREIGN KEY([ConsumibleID])
REFERENCES [dbo].[Consumible] ([ConsumibleID])
GO
ALTER TABLE [dbo].[JuntaSoldaduraDetalle] CHECK CONSTRAINT [FK_JuntaSoldaduraDetalle_Consumible]
GO
/****** Object:  ForeignKey [FK_JuntaSoldaduraDetalle_JuntaSoldadura]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldaduraDetalle]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldaduraDetalle_JuntaSoldadura] FOREIGN KEY([JuntaSoldaduraID])
REFERENCES [dbo].[JuntaSoldadura] ([JuntaSoldaduraID])
GO
ALTER TABLE [dbo].[JuntaSoldaduraDetalle] CHECK CONSTRAINT [FK_JuntaSoldaduraDetalle_JuntaSoldadura]
GO
/****** Object:  ForeignKey [FK_JuntaSoldaduraDetalle_Soldador]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldaduraDetalle]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldaduraDetalle_Soldador] FOREIGN KEY([SoldadorID])
REFERENCES [dbo].[Soldador] ([SoldadorID])
GO
ALTER TABLE [dbo].[JuntaSoldaduraDetalle] CHECK CONSTRAINT [FK_JuntaSoldaduraDetalle_Soldador]
GO
/****** Object:  ForeignKey [FK_JuntaSoldaduraDetalle_TecnicaSoldador]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSoldaduraDetalle]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSoldaduraDetalle_TecnicaSoldador] FOREIGN KEY([TecnicaSoldadorID])
REFERENCES [dbo].[TecnicaSoldador] ([TecnicaSoldadorID])
GO
ALTER TABLE [dbo].[JuntaSoldaduraDetalle] CHECK CONSTRAINT [FK_JuntaSoldaduraDetalle_TecnicaSoldador]
GO
/****** Object:  ForeignKey [FK_JuntaSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSpool]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaSpool] CHECK CONSTRAINT [FK_JuntaSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaSpool_FabArea]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSpool]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpool_FabArea] FOREIGN KEY([FabAreaID])
REFERENCES [dbo].[FabArea] ([FabAreaID])
GO
ALTER TABLE [dbo].[JuntaSpool] CHECK CONSTRAINT [FK_JuntaSpool_FabArea]
GO
/****** Object:  ForeignKey [FK_JuntaSpool_FamiliaAcero_1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSpool]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpool_FamiliaAcero_1] FOREIGN KEY([FamiliaAceroMaterial1ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[JuntaSpool] CHECK CONSTRAINT [FK_JuntaSpool_FamiliaAcero_1]
GO
/****** Object:  ForeignKey [FK_JuntaSpool_FamiliaAcero_2]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSpool]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpool_FamiliaAcero_2] FOREIGN KEY([FamiliaAceroMaterial2ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[JuntaSpool] CHECK CONSTRAINT [FK_JuntaSpool_FamiliaAcero_2]
GO
/****** Object:  ForeignKey [FK_JuntaSpool_Spool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSpool]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpool_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO
ALTER TABLE [dbo].[JuntaSpool] CHECK CONSTRAINT [FK_JuntaSpool_Spool]
GO
/****** Object:  ForeignKey [FK_JuntaSpool_TipoJunta]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaSpool]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpool_TipoJunta] FOREIGN KEY([TipoJuntaID])
REFERENCES [dbo].[TipoJunta] ([TipoJuntaID])
GO
ALTER TABLE [dbo].[JuntaSpool] CHECK CONSTRAINT [FK_JuntaSpool_TipoJunta]
GO
/****** Object:  ForeignKey [FK_JuntaAnterior]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_JuntaAnterior] FOREIGN KEY([JuntaWorkstatusAnteriorID])
REFERENCES [dbo].[JuntaWorkstatus] ([JuntaWorkstatusID])
GO
ALTER TABLE [dbo].[JuntaWorkstatus] CHECK CONSTRAINT [FK_JuntaAnterior]
GO
/****** Object:  ForeignKey [FK_JuntaWorkstatus_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_JuntaWorkstatus_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[JuntaWorkstatus] CHECK CONSTRAINT [FK_JuntaWorkstatus_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_JuntaWorkstatus_JuntaArmado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_JuntaWorkstatus_JuntaArmado] FOREIGN KEY([JuntaArmadoID])
REFERENCES [dbo].[JuntaArmado] ([JuntaArmadoID])
GO
ALTER TABLE [dbo].[JuntaWorkstatus] CHECK CONSTRAINT [FK_JuntaWorkstatus_JuntaArmado]
GO
/****** Object:  ForeignKey [FK_JuntaWorkstatus_JuntaInspeccionVisual]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_JuntaWorkstatus_JuntaInspeccionVisual] FOREIGN KEY([JuntaInspeccionVisualID])
REFERENCES [dbo].[JuntaInspeccionVisual] ([JuntaInspeccionVisualID])
GO
ALTER TABLE [dbo].[JuntaWorkstatus] CHECK CONSTRAINT [FK_JuntaWorkstatus_JuntaInspeccionVisual]
GO
/****** Object:  ForeignKey [FK_JuntaWorkstatus_JuntaSoldadura]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_JuntaWorkstatus_JuntaSoldadura] FOREIGN KEY([JuntaSoldaduraID])
REFERENCES [dbo].[JuntaSoldadura] ([JuntaSoldaduraID])
GO
ALTER TABLE [dbo].[JuntaWorkstatus] CHECK CONSTRAINT [FK_JuntaWorkstatus_JuntaSoldadura]
GO
/****** Object:  ForeignKey [FK_JuntaWorkstatus_JuntaSpool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_JuntaWorkstatus_JuntaSpool] FOREIGN KEY([JuntaSpoolID])
REFERENCES [dbo].[JuntaSpool] ([JuntaSpoolID])
GO
ALTER TABLE [dbo].[JuntaWorkstatus] CHECK CONSTRAINT [FK_JuntaWorkstatus_JuntaSpool]
GO
/****** Object:  ForeignKey [FK_JuntaWorkstatus_OrdenTrabajoSpool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_JuntaWorkstatus_OrdenTrabajoSpool] FOREIGN KEY([OrdenTrabajoSpoolID])
REFERENCES [dbo].[OrdenTrabajoSpool] ([OrdenTrabajoSpoolID])
GO
ALTER TABLE [dbo].[JuntaWorkstatus] CHECK CONSTRAINT [FK_JuntaWorkstatus_OrdenTrabajoSpool]
GO
/****** Object:  ForeignKey [FK_JuntaWorkstatus_UltimoProceso]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[JuntaWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_JuntaWorkstatus_UltimoProceso] FOREIGN KEY([UltimoProcesoID])
REFERENCES [dbo].[UltimoProceso] ([UltimoProcesoID])
GO
ALTER TABLE [dbo].[JuntaWorkstatus] CHECK CONSTRAINT [FK_JuntaWorkstatus_UltimoProceso]
GO
/****** Object:  ForeignKey [FK_KgTeorico_Cedula]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[KgTeorico]  WITH CHECK ADD  CONSTRAINT [FK_KgTeorico_Cedula] FOREIGN KEY([CedulaID])
REFERENCES [dbo].[Cedula] ([CedulaID])
GO
ALTER TABLE [dbo].[KgTeorico] CHECK CONSTRAINT [FK_KgTeorico_Cedula]
GO
/****** Object:  ForeignKey [FK_KgTeorico_Diametro]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[KgTeorico]  WITH CHECK ADD  CONSTRAINT [FK_KgTeorico_Diametro] FOREIGN KEY([DiametroID])
REFERENCES [dbo].[Diametro] ([DiametroID])
GO
ALTER TABLE [dbo].[KgTeorico] CHECK CONSTRAINT [FK_KgTeorico_Diametro]
GO
/****** Object:  ForeignKey [FK_Maquina_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Maquina]  WITH CHECK ADD  CONSTRAINT [FK_Maquina_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Maquina] CHECK CONSTRAINT [FK_Maquina_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Maquina_Patio]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Maquina]  WITH CHECK ADD  CONSTRAINT [FK_Maquina_Patio] FOREIGN KEY([PatioID])
REFERENCES [dbo].[Patio] ([PatioID])
GO
ALTER TABLE [dbo].[Maquina] CHECK CONSTRAINT [FK_Maquina_Patio]
GO
/****** Object:  ForeignKey [FK_MaterialSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[MaterialSpool]  WITH CHECK ADD  CONSTRAINT [FK_MaterialSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[MaterialSpool] CHECK CONSTRAINT [FK_MaterialSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_MaterialSpool_ItemCode]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[MaterialSpool]  WITH CHECK ADD  CONSTRAINT [FK_MaterialSpool_ItemCode] FOREIGN KEY([ItemCodeID])
REFERENCES [dbo].[ItemCode] ([ItemCodeID])
GO
ALTER TABLE [dbo].[MaterialSpool] CHECK CONSTRAINT [FK_MaterialSpool_ItemCode]
GO
/****** Object:  ForeignKey [FK_MaterialSpool_Spool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[MaterialSpool]  WITH CHECK ADD  CONSTRAINT [FK_MaterialSpool_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO
ALTER TABLE [dbo].[MaterialSpool] CHECK CONSTRAINT [FK_MaterialSpool_Spool]
GO
/****** Object:  ForeignKey [FK_Modulo_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Modulo]  WITH CHECK ADD  CONSTRAINT [FK_Modulo_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Modulo] CHECK CONSTRAINT [FK_Modulo_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ModuloSeguimientoJunta_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ModuloSeguimientoJunta]  WITH CHECK ADD  CONSTRAINT [FK_ModuloSeguimientoJunta_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ModuloSeguimientoJunta] CHECK CONSTRAINT [FK_ModuloSeguimientoJunta_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_NumeroUnico_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnico_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [FK_NumeroUnico_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_NumeroUnico_Colada]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnico_Colada] FOREIGN KEY([ColadaID])
REFERENCES [dbo].[Colada] ([ColadaID])
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [FK_NumeroUnico_Colada]
GO
/****** Object:  ForeignKey [FK_NumeroUnico_Fabricante]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnico_Fabricante] FOREIGN KEY([FabricanteID])
REFERENCES [dbo].[Fabricante] ([FabricanteID])
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [FK_NumeroUnico_Fabricante]
GO
/****** Object:  ForeignKey [FK_NumeroUnico_ItemCode]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnico_ItemCode] FOREIGN KEY([ItemCodeID])
REFERENCES [dbo].[ItemCode] ([ItemCodeID])
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [FK_NumeroUnico_ItemCode]
GO
/****** Object:  ForeignKey [FK_NumeroUnico_Proveedor]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnico_Proveedor] FOREIGN KEY([ProveedorID])
REFERENCES [dbo].[Proveedor] ([ProveedorID])
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [FK_NumeroUnico_Proveedor]
GO
/****** Object:  ForeignKey [FK_NumeroUnico_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnico_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [FK_NumeroUnico_Proyecto]
GO
/****** Object:  ForeignKey [FK_NumeroUnico_TipoCorte_1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnico_TipoCorte_1] FOREIGN KEY([TipoCorte1ID])
REFERENCES [dbo].[TipoCorte] ([TipoCorteID])
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [FK_NumeroUnico_TipoCorte_1]
GO
/****** Object:  ForeignKey [FK_NumeroUnico_TipoCorte_2]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnico_TipoCorte_2] FOREIGN KEY([TipoCorte2ID])
REFERENCES [dbo].[TipoCorte] ([TipoCorteID])
GO
ALTER TABLE [dbo].[NumeroUnico] CHECK CONSTRAINT [FK_NumeroUnico_TipoCorte_2]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoCorte_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoCorte]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoCorte_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[NumeroUnicoCorte] CHECK CONSTRAINT [FK_NumeroUnicoCorte_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoCorte_NumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoCorte]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoCorte_NumeroUnico] FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[NumeroUnicoCorte] CHECK CONSTRAINT [FK_NumeroUnicoCorte_NumeroUnico]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoCorte_NumeroUnicoMovimiento]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoCorte]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoCorte_NumeroUnicoMovimiento] FOREIGN KEY([SalidaMovimientoID])
REFERENCES [dbo].[NumeroUnicoMovimiento] ([NumeroUnicoMovimientoID])
GO
ALTER TABLE [dbo].[NumeroUnicoCorte] CHECK CONSTRAINT [FK_NumeroUnicoCorte_NumeroUnicoMovimiento]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoCorte_OrdenTrabajo]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoCorte]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoCorte_OrdenTrabajo] FOREIGN KEY([OrdenTrabajoID])
REFERENCES [dbo].[OrdenTrabajo] ([OrdenTrabajoID])
GO
ALTER TABLE [dbo].[NumeroUnicoCorte] CHECK CONSTRAINT [FK_NumeroUnicoCorte_OrdenTrabajo]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoCorte_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoCorte]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoCorte_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[NumeroUnicoCorte] CHECK CONSTRAINT [FK_NumeroUnicoCorte_Proyecto]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoCorte_UbicacionFisica]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoCorte]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoCorte_UbicacionFisica] FOREIGN KEY([UbicacionFisicaID])
REFERENCES [dbo].[UbicacionFisica] ([UbicacionFisicaID])
GO
ALTER TABLE [dbo].[NumeroUnicoCorte] CHECK CONSTRAINT [FK_NumeroUnicoCorte_UbicacionFisica]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoInventario_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoInventario_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [FK_NumeroUnicoInventario_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoInventario_NumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoInventario_NumeroUnico] FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [FK_NumeroUnicoInventario_NumeroUnico]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoInventario_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoInventario]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoInventario_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[NumeroUnicoInventario] CHECK CONSTRAINT [FK_NumeroUnicoInventario_Proyecto]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoMovimiento_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoMovimiento]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoMovimiento_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[NumeroUnicoMovimiento] CHECK CONSTRAINT [FK_NumeroUnicoMovimiento_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoMovimiento_NumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoMovimiento]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoMovimiento_NumeroUnico] FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[NumeroUnicoMovimiento] CHECK CONSTRAINT [FK_NumeroUnicoMovimiento_NumeroUnico]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoMovimiento_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoMovimiento]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoMovimiento_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[NumeroUnicoMovimiento] CHECK CONSTRAINT [FK_NumeroUnicoMovimiento_Proyecto]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoMovimiento_TipoMovimiento]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoMovimiento]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoMovimiento_TipoMovimiento] FOREIGN KEY([TipoMovimientoID])
REFERENCES [dbo].[TipoMovimiento] ([TipoMovimientoID])
GO
ALTER TABLE [dbo].[NumeroUnicoMovimiento] CHECK CONSTRAINT [FK_NumeroUnicoMovimiento_TipoMovimiento]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoSegmento_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoSegmento_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[NumeroUnicoSegmento] CHECK CONSTRAINT [FK_NumeroUnicoSegmento_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoSegmento_NumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoSegmento_NumeroUnico] FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[NumeroUnicoSegmento] CHECK CONSTRAINT [FK_NumeroUnicoSegmento_NumeroUnico]
GO
/****** Object:  ForeignKey [FK_NumeroUnicoSegmento_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[NumeroUnicoSegmento]  WITH CHECK ADD  CONSTRAINT [FK_NumeroUnicoSegmento_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[NumeroUnicoSegmento] CHECK CONSTRAINT [FK_NumeroUnicoSegmento_Proyecto]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajo_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajo_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[OrdenTrabajo] CHECK CONSTRAINT [FK_OrdenTrabajo_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajo_EstatusOrden]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajo_EstatusOrden] FOREIGN KEY([EstatusOrdenID])
REFERENCES [dbo].[EstatusOrden] ([EstatusOrdenID])
GO
ALTER TABLE [dbo].[OrdenTrabajo] CHECK CONSTRAINT [FK_OrdenTrabajo_EstatusOrden]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajo_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajo_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[OrdenTrabajo] CHECK CONSTRAINT [FK_OrdenTrabajo_Proyecto]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajo_Taller]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajo_Taller] FOREIGN KEY([TallerID])
REFERENCES [dbo].[Taller] ([TallerID])
GO
ALTER TABLE [dbo].[OrdenTrabajo] CHECK CONSTRAINT [FK_OrdenTrabajo_Taller]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoJunta_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoJunta]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoJunta_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[OrdenTrabajoJunta] CHECK CONSTRAINT [FK_OrdenTrabajoJunta_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoJunta_JuntaSpool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoJunta]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoJunta_JuntaSpool] FOREIGN KEY([JuntaSpoolID])
REFERENCES [dbo].[JuntaSpool] ([JuntaSpoolID])
GO
ALTER TABLE [dbo].[OrdenTrabajoJunta] CHECK CONSTRAINT [FK_OrdenTrabajoJunta_JuntaSpool]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoJunta_OrdenTrabajoSpool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoJunta]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoJunta_OrdenTrabajoSpool] FOREIGN KEY([OrdenTrabajoSpoolID])
REFERENCES [dbo].[OrdenTrabajoSpool] ([OrdenTrabajoSpoolID])
GO
ALTER TABLE [dbo].[OrdenTrabajoJunta] CHECK CONSTRAINT [FK_OrdenTrabajoJunta_OrdenTrabajoSpool]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoMaterial_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoMaterial_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] CHECK CONSTRAINT [FK_OrdenTrabajoMaterial_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoMaterial_CorteDetalle]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoMaterial_CorteDetalle] FOREIGN KEY([CorteDetalleID])
REFERENCES [dbo].[CorteDetalle] ([CorteDetalleID])
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] CHECK CONSTRAINT [FK_OrdenTrabajoMaterial_CorteDetalle]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoMaterial_Despacho]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoMaterial_Despacho] FOREIGN KEY([DespachoID])
REFERENCES [dbo].[Despacho] ([DespachoID])
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] CHECK CONSTRAINT [FK_OrdenTrabajoMaterial_Despacho]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoMaterial_MaterialSpool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoMaterial_MaterialSpool] FOREIGN KEY([MaterialSpoolID])
REFERENCES [dbo].[MaterialSpool] ([MaterialSpoolID])
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] CHECK CONSTRAINT [FK_OrdenTrabajoMaterial_MaterialSpool]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoMaterial_NumeroUnico_Congelado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoMaterial_NumeroUnico_Congelado] FOREIGN KEY([NumeroUnicoCongeladoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] CHECK CONSTRAINT [FK_OrdenTrabajoMaterial_NumeroUnico_Congelado]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoMaterial_NumeroUnico_Despachado]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoMaterial_NumeroUnico_Despachado] FOREIGN KEY([NumeroUnicoDespachadoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] CHECK CONSTRAINT [FK_OrdenTrabajoMaterial_NumeroUnico_Despachado]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoMaterial_NumeroUnico_Sugerido]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoMaterial_NumeroUnico_Sugerido] FOREIGN KEY([NumeroUnicoSugeridoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] CHECK CONSTRAINT [FK_OrdenTrabajoMaterial_NumeroUnico_Sugerido]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoMaterial_OrdenTrabajoSpool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoMaterial_OrdenTrabajoSpool] FOREIGN KEY([OrdenTrabajoSpoolID])
REFERENCES [dbo].[OrdenTrabajoSpool] ([OrdenTrabajoSpoolID])
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] CHECK CONSTRAINT [FK_OrdenTrabajoMaterial_OrdenTrabajoSpool]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoSpool]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[OrdenTrabajoSpool] CHECK CONSTRAINT [FK_OrdenTrabajoSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoSpool_OrdenTrabajo]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoSpool]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoSpool_OrdenTrabajo] FOREIGN KEY([OrdenTrabajoID])
REFERENCES [dbo].[OrdenTrabajo] ([OrdenTrabajoID])
GO
ALTER TABLE [dbo].[OrdenTrabajoSpool] CHECK CONSTRAINT [FK_OrdenTrabajoSpool_OrdenTrabajo]
GO
/****** Object:  ForeignKey [FK_OrdenTrabajoSpool_Spool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[OrdenTrabajoSpool]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoSpool_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO
ALTER TABLE [dbo].[OrdenTrabajoSpool] CHECK CONSTRAINT [FK_OrdenTrabajoSpool_Spool]
GO
/****** Object:  ForeignKey [FK_Pagina_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Pagina]  WITH CHECK ADD  CONSTRAINT [FK_Pagina_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Pagina] CHECK CONSTRAINT [FK_Pagina_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Pagina_Permiso]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Pagina]  WITH CHECK ADD  CONSTRAINT [FK_Pagina_Permiso] FOREIGN KEY([PermisoID])
REFERENCES [dbo].[Permiso] ([PermisoID])
GO
ALTER TABLE [dbo].[Pagina] CHECK CONSTRAINT [FK_Pagina_Permiso]
GO
/****** Object:  ForeignKey [FK_Patio_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Patio]  WITH CHECK ADD  CONSTRAINT [FK_Patio_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Patio] CHECK CONSTRAINT [FK_Patio_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Peq_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Peq]  WITH CHECK ADD  CONSTRAINT [FK_Peq_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Peq] CHECK CONSTRAINT [FK_Peq_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Peq_Cedula]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Peq]  WITH CHECK ADD  CONSTRAINT [FK_Peq_Cedula] FOREIGN KEY([CedulaID])
REFERENCES [dbo].[Cedula] ([CedulaID])
GO
ALTER TABLE [dbo].[Peq] CHECK CONSTRAINT [FK_Peq_Cedula]
GO
/****** Object:  ForeignKey [FK_Peq_Diametro]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Peq]  WITH CHECK ADD  CONSTRAINT [FK_Peq_Diametro] FOREIGN KEY([DiametroID])
REFERENCES [dbo].[Diametro] ([DiametroID])
GO
ALTER TABLE [dbo].[Peq] CHECK CONSTRAINT [FK_Peq_Diametro]
GO
/****** Object:  ForeignKey [FK_Peq_FamiliaAcero]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Peq]  WITH CHECK ADD  CONSTRAINT [FK_Peq_FamiliaAcero] FOREIGN KEY([FamiliaAceroID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[Peq] CHECK CONSTRAINT [FK_Peq_FamiliaAcero]
GO
/****** Object:  ForeignKey [FK_Peq_TipoJunta]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Peq]  WITH CHECK ADD  CONSTRAINT [FK_Peq_TipoJunta] FOREIGN KEY([TipoJuntaID])
REFERENCES [dbo].[TipoJunta] ([TipoJuntaID])
GO
ALTER TABLE [dbo].[Peq] CHECK CONSTRAINT [FK_Peq_TipoJunta]
GO
/****** Object:  ForeignKey [FK_Perfil_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Perfil]  WITH CHECK ADD  CONSTRAINT [FK_Perfil_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Perfil] CHECK CONSTRAINT [FK_Perfil_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_PerfilPermiso_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PerfilPermiso]  WITH CHECK ADD  CONSTRAINT [FK_PerfilPermiso_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[PerfilPermiso] CHECK CONSTRAINT [FK_PerfilPermiso_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_PerfilPermiso_Perfil]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PerfilPermiso]  WITH CHECK ADD  CONSTRAINT [FK_PerfilPermiso_Perfil] FOREIGN KEY([PerfilID])
REFERENCES [dbo].[Perfil] ([PerfilID])
GO
ALTER TABLE [dbo].[PerfilPermiso] CHECK CONSTRAINT [FK_PerfilPermiso_Perfil]
GO
/****** Object:  ForeignKey [FK_PerfilPermiso_Permiso]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PerfilPermiso]  WITH CHECK ADD  CONSTRAINT [FK_PerfilPermiso_Permiso] FOREIGN KEY([PermisoID])
REFERENCES [dbo].[Permiso] ([PermisoID])
GO
ALTER TABLE [dbo].[PerfilPermiso] CHECK CONSTRAINT [FK_PerfilPermiso_Permiso]
GO
/****** Object:  ForeignKey [FK_PeriodoDestajo_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PeriodoDestajo]  WITH CHECK ADD  CONSTRAINT [FK_PeriodoDestajo_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[PeriodoDestajo] CHECK CONSTRAINT [FK_PeriodoDestajo_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Permiso_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Permiso_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Permiso] CHECK CONSTRAINT [FK_Permiso_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Permiso_Modulo]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Permiso_Modulo] FOREIGN KEY([ModuloID])
REFERENCES [dbo].[Modulo] ([ModuloID])
GO
ALTER TABLE [dbo].[Permiso] CHECK CONSTRAINT [FK_Permiso_Modulo]
GO
/****** Object:  ForeignKey [FK_PersonalizacionSeguimientoJunta_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PersonalizacionSeguimientoJunta]  WITH CHECK ADD  CONSTRAINT [FK_PersonalizacionSeguimientoJunta_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[PersonalizacionSeguimientoJunta] CHECK CONSTRAINT [FK_PersonalizacionSeguimientoJunta_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_PersonalizacionSeguimientoJunta_Usuario]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PersonalizacionSeguimientoJunta]  WITH CHECK ADD  CONSTRAINT [FK_PersonalizacionSeguimientoJunta_Usuario] FOREIGN KEY([UserId])
REFERENCES [dbo].[Usuario] ([UserId])
GO
ALTER TABLE [dbo].[PersonalizacionSeguimientoJunta] CHECK CONSTRAINT [FK_PersonalizacionSeguimientoJunta_Usuario]
GO
/****** Object:  ForeignKey [FK_PersonalizacionSeguimientoSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PersonalizacionSeguimientoSpool]  WITH CHECK ADD  CONSTRAINT [FK_PersonalizacionSeguimientoSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[PersonalizacionSeguimientoSpool] CHECK CONSTRAINT [FK_PersonalizacionSeguimientoSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_PersonalizacionSeguimientoSpool_Usuario]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PersonalizacionSeguimientoSpool]  WITH CHECK ADD  CONSTRAINT [FK_PersonalizacionSeguimientoSpool_Usuario] FOREIGN KEY([UserId])
REFERENCES [dbo].[Usuario] ([UserId])
GO
ALTER TABLE [dbo].[PersonalizacionSeguimientoSpool] CHECK CONSTRAINT [FK_PersonalizacionSeguimientoSpool_Usuario]
GO
/****** Object:  ForeignKey [FK_PinturaNumeroUnico_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_PinturaNumeroUnico_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[PinturaNumeroUnico] CHECK CONSTRAINT [FK_PinturaNumeroUnico_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_PinturaNumeroUnico_NumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_PinturaNumeroUnico_NumeroUnico] FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[PinturaNumeroUnico] CHECK CONSTRAINT [FK_PinturaNumeroUnico_NumeroUnico]
GO
/****** Object:  ForeignKey [FK_PinturaNumeroUnico_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_PinturaNumeroUnico_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[PinturaNumeroUnico] CHECK CONSTRAINT [FK_PinturaNumeroUnico_Proyecto]
GO
/****** Object:  ForeignKey [FK_PinturaNumeroUnico_RequisicionNumeroUnicoDetalle]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_PinturaNumeroUnico_RequisicionNumeroUnicoDetalle] FOREIGN KEY([RequisicionNumeroUnicoDetalleID])
REFERENCES [dbo].[RequisicionNumeroUnicoDetalle] ([RequisicionNumeroUnicoDetalleID])
GO
ALTER TABLE [dbo].[PinturaNumeroUnico] CHECK CONSTRAINT [FK_PinturaNumeroUnico_RequisicionNumeroUnicoDetalle]
GO
/****** Object:  ForeignKey [FK_PinturaSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaSpool]  WITH CHECK ADD  CONSTRAINT [FK_PinturaSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[PinturaSpool] CHECK CONSTRAINT [FK_PinturaSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_PinturaSpool_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaSpool]  WITH CHECK ADD  CONSTRAINT [FK_PinturaSpool_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[PinturaSpool] CHECK CONSTRAINT [FK_PinturaSpool_Proyecto]
GO
/****** Object:  ForeignKey [FK_PinturaSpool_RequisicionPinturaDetalle]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaSpool]  WITH CHECK ADD  CONSTRAINT [FK_PinturaSpool_RequisicionPinturaDetalle] FOREIGN KEY([RequisicionPinturaDetalleID])
REFERENCES [dbo].[RequisicionPinturaDetalle] ([RequisicionPinturaDetalleID])
GO
ALTER TABLE [dbo].[PinturaSpool] CHECK CONSTRAINT [FK_PinturaSpool_RequisicionPinturaDetalle]
GO
/****** Object:  ForeignKey [FK_PinturaSpool_WorkstatusSpool1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[PinturaSpool]  WITH CHECK ADD  CONSTRAINT [FK_PinturaSpool_WorkstatusSpool1] FOREIGN KEY([WorkstatusSpoolID])
REFERENCES [dbo].[WorkstatusSpool] ([WorkstatusSpoolID])
GO
ALTER TABLE [dbo].[PinturaSpool] CHECK CONSTRAINT [FK_PinturaSpool_WorkstatusSpool1]
GO
/****** Object:  ForeignKey [FK_ProcesoRaiz_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProcesoRaiz]  WITH CHECK ADD  CONSTRAINT [FK_ProcesoRaiz_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ProcesoRaiz] CHECK CONSTRAINT [FK_ProcesoRaiz_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ProcesoRelleno_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProcesoRelleno]  WITH CHECK ADD  CONSTRAINT [FK_ProcesoRelleno_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ProcesoRelleno] CHECK CONSTRAINT [FK_ProcesoRelleno_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Proveedor_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Proveedor]  WITH CHECK ADD  CONSTRAINT [FK_Proveedor_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Proveedor] CHECK CONSTRAINT [FK_Proveedor_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Proveedor_Contacto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Proveedor]  WITH CHECK ADD  CONSTRAINT [FK_Proveedor_Contacto] FOREIGN KEY([ContactoID])
REFERENCES [dbo].[Contacto] ([ContactoID])
GO
ALTER TABLE [dbo].[Proveedor] CHECK CONSTRAINT [FK_Proveedor_Contacto]
GO
/****** Object:  ForeignKey [FK_ProveedorProyecto_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProveedorProyecto]  WITH CHECK ADD  CONSTRAINT [FK_ProveedorProyecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ProveedorProyecto] CHECK CONSTRAINT [FK_ProveedorProyecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ProveedorProyecto_Proveedor]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProveedorProyecto]  WITH CHECK ADD  CONSTRAINT [FK_ProveedorProyecto_Proveedor] FOREIGN KEY([ProveedorID])
REFERENCES [dbo].[Proveedor] ([ProveedorID])
GO
ALTER TABLE [dbo].[ProveedorProyecto] CHECK CONSTRAINT [FK_ProveedorProyecto_Proveedor]
GO
/****** Object:  ForeignKey [FK_ProveedorProyecto_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProveedorProyecto]  WITH CHECK ADD  CONSTRAINT [FK_ProveedorProyecto_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ProveedorProyecto] CHECK CONSTRAINT [FK_ProveedorProyecto_Proyecto]
GO
/****** Object:  ForeignKey [FK_Proyecto_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_Proyecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK_Proyecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Proyecto_Cliente]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_Proyecto_Cliente] FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Cliente] ([ClienteID])
GO
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK_Proyecto_Cliente]
GO
/****** Object:  ForeignKey [FK_Proyecto_Color]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_Proyecto_Color] FOREIGN KEY([ColorID])
REFERENCES [dbo].[Color] ([ColorID])
GO
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK_Proyecto_Color]
GO
/****** Object:  ForeignKey [FK_Proyecto_Contacto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_Proyecto_Contacto] FOREIGN KEY([ContactoID])
REFERENCES [dbo].[Contacto] ([ContactoID])
GO
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK_Proyecto_Contacto]
GO
/****** Object:  ForeignKey [FK_Proyecto_Patio]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_Proyecto_Patio] FOREIGN KEY([PatioID])
REFERENCES [dbo].[Patio] ([PatioID])
GO
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK_Proyecto_Patio]
GO
/****** Object:  ForeignKey [FK_ProyectoConfiguracion_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConfiguracion]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoConfiguracion_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ProyectoConfiguracion] CHECK CONSTRAINT [FK_ProyectoConfiguracion_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ProyectoConfiguracion_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConfiguracion]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoConfiguracion_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ProyectoConfiguracion] CHECK CONSTRAINT [FK_ProyectoConfiguracion_Proyecto]
GO
/****** Object:  ForeignKey [FK_ProyectoConsecutivo_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConsecutivo]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoConsecutivo_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ProyectoConsecutivo] CHECK CONSTRAINT [FK_ProyectoConsecutivo_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ProyectoConsecutivo_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoConsecutivo]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoConsecutivo_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ProyectoConsecutivo] CHECK CONSTRAINT [FK_ProyectoConsecutivo_Proyecto]
GO
/****** Object:  ForeignKey [FK_ProyectoDossier_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoDossier]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoDossier_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ProyectoDossier] CHECK CONSTRAINT [FK_ProyectoDossier_Proyecto]
GO
/****** Object:  ForeignKey [FK_ProyectoNomenclaturaSpool_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoNomenclaturaSpool]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoNomenclaturaSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ProyectoNomenclaturaSpool] CHECK CONSTRAINT [FK_ProyectoNomenclaturaSpool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ProyectoNomenclaturaSpool_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoNomenclaturaSpool]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoNomenclaturaSpool_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ProyectoNomenclaturaSpool] CHECK CONSTRAINT [FK_ProyectoNomenclaturaSpool_Proyecto]
GO
/****** Object:  ForeignKey [FK_ProyectoReporte_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoReporte]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoReporte_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ProyectoReporte] CHECK CONSTRAINT [FK_ProyectoReporte_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ProyectoReporte_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoReporte]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoReporte_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ProyectoReporte] CHECK CONSTRAINT [FK_ProyectoReporte_Proyecto]
GO
/****** Object:  ForeignKey [FK_ProyectoReporte_TipoReporteProyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ProyectoReporte]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoReporte_TipoReporteProyecto] FOREIGN KEY([TipoReporteProyectoID])
REFERENCES [dbo].[TipoReporteProyecto] ([TipoReporteProyectoID])
GO
ALTER TABLE [dbo].[ProyectoReporte] CHECK CONSTRAINT [FK_ProyectoReporte_TipoReporteProyecto]
GO
/****** Object:  ForeignKey [FK_Recepcion_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Recepcion]  WITH CHECK ADD  CONSTRAINT [FK_Recepcion_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Recepcion] CHECK CONSTRAINT [FK_Recepcion_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Recepcion_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Recepcion]  WITH CHECK ADD  CONSTRAINT [FK_Recepcion_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[Recepcion] CHECK CONSTRAINT [FK_Recepcion_Proyecto]
GO
/****** Object:  ForeignKey [FK_Recepcion_Transportista]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Recepcion]  WITH CHECK ADD  CONSTRAINT [FK_Recepcion_Transportista] FOREIGN KEY([TransportistaID])
REFERENCES [dbo].[Transportista] ([TransportistaID])
GO
ALTER TABLE [dbo].[Recepcion] CHECK CONSTRAINT [FK_Recepcion_Transportista]
GO
/****** Object:  ForeignKey [FK_RecepcionNumeroUnico_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RecepcionNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_RecepcionNumeroUnico_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[RecepcionNumeroUnico] CHECK CONSTRAINT [FK_RecepcionNumeroUnico_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_RecepcionNumeroUnico_NumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RecepcionNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_RecepcionNumeroUnico_NumeroUnico] FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[RecepcionNumeroUnico] CHECK CONSTRAINT [FK_RecepcionNumeroUnico_NumeroUnico]
GO
/****** Object:  ForeignKey [FK_RecepcionNumeroUnico_NumeroUnicoMovimiento]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RecepcionNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_RecepcionNumeroUnico_NumeroUnicoMovimiento] FOREIGN KEY([NumeroUnicoMovimientoID])
REFERENCES [dbo].[NumeroUnicoMovimiento] ([NumeroUnicoMovimientoID])
GO
ALTER TABLE [dbo].[RecepcionNumeroUnico] CHECK CONSTRAINT [FK_RecepcionNumeroUnico_NumeroUnicoMovimiento]
GO
/****** Object:  ForeignKey [FK_RecepcionNumeroUnico_Recepcion]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RecepcionNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_RecepcionNumeroUnico_Recepcion] FOREIGN KEY([RecepcionID])
REFERENCES [dbo].[Recepcion] ([RecepcionID])
GO
ALTER TABLE [dbo].[RecepcionNumeroUnico] CHECK CONSTRAINT [FK_RecepcionNumeroUnico_Recepcion]
GO
/****** Object:  ForeignKey [FK_ReporteDimensional_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteDimensional]  WITH CHECK ADD  CONSTRAINT [FK_ReporteDimensional_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ReporteDimensional] CHECK CONSTRAINT [FK_ReporteDimensional_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ReporteDimensional_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteDimensional]  WITH CHECK ADD  CONSTRAINT [FK_ReporteDimensional_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ReporteDimensional] CHECK CONSTRAINT [FK_ReporteDimensional_Proyecto]
GO
/****** Object:  ForeignKey [FK_ReporteDimensional_TipoReporteDimensional]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteDimensional]  WITH CHECK ADD  CONSTRAINT [FK_ReporteDimensional_TipoReporteDimensional] FOREIGN KEY([TipoReporteDimensionalID])
REFERENCES [dbo].[TipoReporteDimensional] ([TipoReporteDimensionalID])
GO
ALTER TABLE [dbo].[ReporteDimensional] CHECK CONSTRAINT [FK_ReporteDimensional_TipoReporteDimensional]
GO
/****** Object:  ForeignKey [FK_ReporteDimensionalDetalle_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteDimensionalDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ReporteDimensionalDetalle_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ReporteDimensionalDetalle] CHECK CONSTRAINT [FK_ReporteDimensionalDetalle_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ReporteDimensionalDetalle_ReporteDimensional]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteDimensionalDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ReporteDimensionalDetalle_ReporteDimensional] FOREIGN KEY([ReporteDimensionalID])
REFERENCES [dbo].[ReporteDimensional] ([ReporteDimensionalID])
GO
ALTER TABLE [dbo].[ReporteDimensionalDetalle] CHECK CONSTRAINT [FK_ReporteDimensionalDetalle_ReporteDimensional]
GO
/****** Object:  ForeignKey [FK_ReporteDimensionalDetalle_WorkstatusSpool1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteDimensionalDetalle]  WITH CHECK ADD  CONSTRAINT [FK_ReporteDimensionalDetalle_WorkstatusSpool1] FOREIGN KEY([WorkstatusSpoolID])
REFERENCES [dbo].[WorkstatusSpool] ([WorkstatusSpoolID])
GO
ALTER TABLE [dbo].[ReporteDimensionalDetalle] CHECK CONSTRAINT [FK_ReporteDimensionalDetalle_WorkstatusSpool1]
GO
/****** Object:  ForeignKey [FK_ReportePnd_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReportePnd]  WITH CHECK ADD  CONSTRAINT [FK_ReportePnd_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ReportePnd] CHECK CONSTRAINT [FK_ReportePnd_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ReportePnd_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReportePnd]  WITH CHECK ADD  CONSTRAINT [FK_ReportePnd_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ReportePnd] CHECK CONSTRAINT [FK_ReportePnd_Proyecto]
GO
/****** Object:  ForeignKey [FK_ReportePnd_TipoPrueba]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReportePnd]  WITH CHECK ADD  CONSTRAINT [FK_ReportePnd_TipoPrueba] FOREIGN KEY([TipoPruebaID])
REFERENCES [dbo].[TipoPrueba] ([TipoPruebaID])
GO
ALTER TABLE [dbo].[ReportePnd] CHECK CONSTRAINT [FK_ReportePnd_TipoPrueba]
GO
/****** Object:  ForeignKey [FK_ReporteTt_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteTt]  WITH CHECK ADD  CONSTRAINT [FK_ReporteTt_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ReporteTt] CHECK CONSTRAINT [FK_ReporteTt_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ReporteTt_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteTt]  WITH CHECK ADD  CONSTRAINT [FK_ReporteTt_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ReporteTt] CHECK CONSTRAINT [FK_ReporteTt_Proyecto]
GO
/****** Object:  ForeignKey [FK_ReporteTt_TipoPrueba]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[ReporteTt]  WITH CHECK ADD  CONSTRAINT [FK_ReporteTt_TipoPrueba] FOREIGN KEY([TipoPruebaID])
REFERENCES [dbo].[TipoPrueba] ([TipoPruebaID])
GO
ALTER TABLE [dbo].[ReporteTt] CHECK CONSTRAINT [FK_ReporteTt_TipoPrueba]
GO
/****** Object:  ForeignKey [FK_Requisicion_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Requisicion]  WITH CHECK ADD  CONSTRAINT [FK_Requisicion_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Requisicion] CHECK CONSTRAINT [FK_Requisicion_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Requisicion_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Requisicion]  WITH CHECK ADD  CONSTRAINT [FK_Requisicion_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[Requisicion] CHECK CONSTRAINT [FK_Requisicion_Proyecto]
GO
/****** Object:  ForeignKey [FK_Requisicion_TipoPrueba]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Requisicion]  WITH CHECK ADD  CONSTRAINT [FK_Requisicion_TipoPrueba] FOREIGN KEY([TipoPruebaID])
REFERENCES [dbo].[TipoPrueba] ([TipoPruebaID])
GO
ALTER TABLE [dbo].[Requisicion] CHECK CONSTRAINT [FK_Requisicion_TipoPrueba]
GO
/****** Object:  ForeignKey [FK_RequisicionNumeroUnico_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionNumeroUnico_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[RequisicionNumeroUnico] CHECK CONSTRAINT [FK_RequisicionNumeroUnico_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_RequisicionNumeroUnico_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionNumeroUnico]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionNumeroUnico_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[RequisicionNumeroUnico] CHECK CONSTRAINT [FK_RequisicionNumeroUnico_Proyecto]
GO
/****** Object:  ForeignKey [FK_RequisicionNumeroUnicoDetalle_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionNumeroUnicoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionNumeroUnicoDetalle_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[RequisicionNumeroUnicoDetalle] CHECK CONSTRAINT [FK_RequisicionNumeroUnicoDetalle_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_RequisicionNumeroUnicoDetalle_NumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionNumeroUnicoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionNumeroUnicoDetalle_NumeroUnico] FOREIGN KEY([NumeroUnicoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
ALTER TABLE [dbo].[RequisicionNumeroUnicoDetalle] CHECK CONSTRAINT [FK_RequisicionNumeroUnicoDetalle_NumeroUnico]
GO
/****** Object:  ForeignKey [FK_RequisicionNumeroUnicoDetalle_RequisicionNumeroUnico]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionNumeroUnicoDetalle]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionNumeroUnicoDetalle_RequisicionNumeroUnico] FOREIGN KEY([RequisicionNumeroUnicoID])
REFERENCES [dbo].[RequisicionNumeroUnico] ([RequisicionNumeroUnicoID])
GO
ALTER TABLE [dbo].[RequisicionNumeroUnicoDetalle] CHECK CONSTRAINT [FK_RequisicionNumeroUnicoDetalle_RequisicionNumeroUnico]
GO
/****** Object:  ForeignKey [FK_RequisicionPintura_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionPintura]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionPintura_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[RequisicionPintura] CHECK CONSTRAINT [FK_RequisicionPintura_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_RequisicionPintura_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionPintura]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionPintura_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[RequisicionPintura] CHECK CONSTRAINT [FK_RequisicionPintura_Proyecto]
GO
/****** Object:  ForeignKey [FK_RequisicionPinturaDetalle_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionPinturaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionPinturaDetalle_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[RequisicionPinturaDetalle] CHECK CONSTRAINT [FK_RequisicionPinturaDetalle_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_RequisicionPinturaDetalle_RequisicionPintura]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionPinturaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionPinturaDetalle_RequisicionPintura] FOREIGN KEY([RequisicionPinturaID])
REFERENCES [dbo].[RequisicionPintura] ([RequisicionPinturaID])
GO
ALTER TABLE [dbo].[RequisicionPinturaDetalle] CHECK CONSTRAINT [FK_RequisicionPinturaDetalle_RequisicionPintura]
GO
/****** Object:  ForeignKey [FK_RequisicionPinturaDetalle_WorkstatusSpool1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[RequisicionPinturaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_RequisicionPinturaDetalle_WorkstatusSpool1] FOREIGN KEY([WorkstatusSpoolID])
REFERENCES [dbo].[WorkstatusSpool] ([WorkstatusSpoolID])
GO
ALTER TABLE [dbo].[RequisicionPinturaDetalle] CHECK CONSTRAINT [FK_RequisicionPinturaDetalle_WorkstatusSpool1]
GO
/****** Object:  ForeignKey [FK_Soldador_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Soldador]  WITH CHECK ADD  CONSTRAINT [FK_Soldador_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Soldador] CHECK CONSTRAINT [FK_Soldador_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Soldador_Patio]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Soldador]  WITH CHECK ADD  CONSTRAINT [FK_Soldador_Patio] FOREIGN KEY([PatioID])
REFERENCES [dbo].[Patio] ([PatioID])
GO
ALTER TABLE [dbo].[Soldador] CHECK CONSTRAINT [FK_Soldador_Patio]
GO
/****** Object:  ForeignKey [FK_Spool_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Spool]  WITH CHECK ADD  CONSTRAINT [FK_Spool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Spool] CHECK CONSTRAINT [FK_Spool_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Spool_FamiliaAcero_1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Spool]  WITH CHECK ADD  CONSTRAINT [FK_Spool_FamiliaAcero_1] FOREIGN KEY([FamiliaAcero1ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[Spool] CHECK CONSTRAINT [FK_Spool_FamiliaAcero_1]
GO
/****** Object:  ForeignKey [FK_Spool_FamiliaAcero_2]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Spool]  WITH CHECK ADD  CONSTRAINT [FK_Spool_FamiliaAcero_2] FOREIGN KEY([FamiliaAcero2ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[Spool] CHECK CONSTRAINT [FK_Spool_FamiliaAcero_2]
GO
/****** Object:  ForeignKey [FK_Spool_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Spool]  WITH CHECK ADD  CONSTRAINT [FK_Spool_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[Spool] CHECK CONSTRAINT [FK_Spool_Proyecto]
GO
/****** Object:  ForeignKey [FK_SpoolHold_Spool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[SpoolHold]  WITH CHECK ADD  CONSTRAINT [FK_SpoolHold_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO
ALTER TABLE [dbo].[SpoolHold] CHECK CONSTRAINT [FK_SpoolHold_Spool]
GO
/****** Object:  ForeignKey [FK_SpoolHoldHistorial_Spool]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[SpoolHoldHistorial]  WITH CHECK ADD  CONSTRAINT [FK_SpoolHoldHistorial_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO
ALTER TABLE [dbo].[SpoolHoldHistorial] CHECK CONSTRAINT [FK_SpoolHoldHistorial_Spool]
GO
/****** Object:  ForeignKey [FK_Taller_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Taller]  WITH CHECK ADD  CONSTRAINT [FK_Taller_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Taller] CHECK CONSTRAINT [FK_Taller_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Taller_Patio]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Taller]  WITH CHECK ADD  CONSTRAINT [FK_Taller_Patio] FOREIGN KEY([PatioID])
REFERENCES [dbo].[Patio] ([PatioID])
GO
ALTER TABLE [dbo].[Taller] CHECK CONSTRAINT [FK_Taller_Patio]
GO
/****** Object:  ForeignKey [FK_TipoCorte_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoCorte]  WITH CHECK ADD  CONSTRAINT [FK_TipoCorte_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TipoCorte] CHECK CONSTRAINT [FK_TipoCorte_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_TipoJunta_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoJunta]  WITH CHECK ADD  CONSTRAINT [FK_TipoJunta_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TipoJunta] CHECK CONSTRAINT [FK_TipoJunta_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_TipoMaterial_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoMaterial]  WITH CHECK ADD  CONSTRAINT [FK_TipoMaterial_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TipoMaterial] CHECK CONSTRAINT [FK_TipoMaterial_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_TipoMovimiento_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoMovimiento]  WITH CHECK ADD  CONSTRAINT [FK_TipoMovimiento_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TipoMovimiento] CHECK CONSTRAINT [FK_TipoMovimiento_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_TipoPrueba_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoPrueba]  WITH CHECK ADD  CONSTRAINT [FK_TipoPrueba_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TipoPrueba] CHECK CONSTRAINT [FK_TipoPrueba_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_TipoRechazo_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoRechazo]  WITH CHECK ADD  CONSTRAINT [FK_TipoRechazo_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TipoRechazo] CHECK CONSTRAINT [FK_TipoRechazo_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_TipoReporteDimensional_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoReporteDimensional]  WITH CHECK ADD  CONSTRAINT [FK_TipoReporteDimensional_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TipoReporteDimensional] CHECK CONSTRAINT [FK_TipoReporteDimensional_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_TipoReporteProyecto_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TipoReporteProyecto]  WITH CHECK ADD  CONSTRAINT [FK_TipoReporteProyecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TipoReporteProyecto] CHECK CONSTRAINT [FK_TipoReporteProyecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Transportista_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Transportista]  WITH CHECK ADD  CONSTRAINT [FK_Transportista_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Transportista] CHECK CONSTRAINT [FK_Transportista_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Transportista_Contacto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Transportista]  WITH CHECK ADD  CONSTRAINT [FK_Transportista_Contacto] FOREIGN KEY([ContactoID])
REFERENCES [dbo].[Contacto] ([ContactoID])
GO
ALTER TABLE [dbo].[Transportista] CHECK CONSTRAINT [FK_Transportista_Contacto]
GO
/****** Object:  ForeignKey [FK_TransportistaProyecto_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TransportistaProyecto]  WITH CHECK ADD  CONSTRAINT [FK_TransportistaProyecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[TransportistaProyecto] CHECK CONSTRAINT [FK_TransportistaProyecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_TransportistaProyecto_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TransportistaProyecto]  WITH CHECK ADD  CONSTRAINT [FK_TransportistaProyecto_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[TransportistaProyecto] CHECK CONSTRAINT [FK_TransportistaProyecto_Proyecto]
GO
/****** Object:  ForeignKey [FK_TransportistaProyecto_Transportista]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[TransportistaProyecto]  WITH CHECK ADD  CONSTRAINT [FK_TransportistaProyecto_Transportista] FOREIGN KEY([TransportistaID])
REFERENCES [dbo].[Transportista] ([TransportistaID])
GO
ALTER TABLE [dbo].[TransportistaProyecto] CHECK CONSTRAINT [FK_TransportistaProyecto_Transportista]
GO
/****** Object:  ForeignKey [FK_Tubero_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Tubero]  WITH CHECK ADD  CONSTRAINT [FK_Tubero_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Tubero] CHECK CONSTRAINT [FK_Tubero_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Tubero_Patio]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Tubero]  WITH CHECK ADD  CONSTRAINT [FK_Tubero_Patio] FOREIGN KEY([PatioID])
REFERENCES [dbo].[Patio] ([PatioID])
GO
ALTER TABLE [dbo].[Tubero] CHECK CONSTRAINT [FK_Tubero_Patio]
GO
/****** Object:  ForeignKey [FK_UbicacionFisica_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[UbicacionFisica]  WITH CHECK ADD  CONSTRAINT [FK_UbicacionFisica_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[UbicacionFisica] CHECK CONSTRAINT [FK_UbicacionFisica_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_UbicacionFisica_Patio]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[UbicacionFisica]  WITH CHECK ADD  CONSTRAINT [FK_UbicacionFisica_Patio] FOREIGN KEY([PatioID])
REFERENCES [dbo].[Patio] ([PatioID])
GO
ALTER TABLE [dbo].[UbicacionFisica] CHECK CONSTRAINT [FK_UbicacionFisica_Patio]
GO
/****** Object:  ForeignKey [FK_UltimoProceso_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[UltimoProceso]  WITH CHECK ADD  CONSTRAINT [FK_UltimoProceso_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[UltimoProceso] CHECK CONSTRAINT [FK_UltimoProceso_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Usuario_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_aspnet_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Usuario_aspnet_Users_UsuarioModifica]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_aspnet_Users_UsuarioModifica] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_aspnet_Users_UsuarioModifica]
GO
/****** Object:  ForeignKey [FK_Usuario_Perfil]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Perfil] FOREIGN KEY([PerfilID])
REFERENCES [dbo].[Perfil] ([PerfilID])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Perfil]
GO
/****** Object:  ForeignKey [FK_UsuarioProyecto_aspnet_UsuarioModifica]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[UsuarioProyecto]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioProyecto_aspnet_UsuarioModifica] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[UsuarioProyecto] CHECK CONSTRAINT [FK_UsuarioProyecto_aspnet_UsuarioModifica]
GO
/****** Object:  ForeignKey [FK_UsuarioProyecto_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[UsuarioProyecto]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioProyecto_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[UsuarioProyecto] CHECK CONSTRAINT [FK_UsuarioProyecto_Proyecto]
GO
/****** Object:  ForeignKey [FK_UsuarioProyecto_Usuario]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[UsuarioProyecto]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioProyecto_Usuario] FOREIGN KEY([UserId])
REFERENCES [dbo].[Usuario] ([UserId])
GO
ALTER TABLE [dbo].[UsuarioProyecto] CHECK CONSTRAINT [FK_UsuarioProyecto_Usuario]
GO
/****** Object:  ForeignKey [FK_WorkstatusSpool_OrdenTrabajoSpool1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[WorkstatusSpool]  WITH CHECK ADD  CONSTRAINT [FK_WorkstatusSpool_OrdenTrabajoSpool1] FOREIGN KEY([OrdenTrabajoSpoolID])
REFERENCES [dbo].[OrdenTrabajoSpool] ([OrdenTrabajoSpoolID])
GO
ALTER TABLE [dbo].[WorkstatusSpool] CHECK CONSTRAINT [FK_WorkstatusSpool_OrdenTrabajoSpool1]
GO
/****** Object:  ForeignKey [FK_WorkstatusSpool_UltimoProceso]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[WorkstatusSpool]  WITH CHECK ADD  CONSTRAINT [FK_WorkstatusSpool_UltimoProceso] FOREIGN KEY([UltimoProcesoID])
REFERENCES [dbo].[UltimoProceso] ([UltimoProcesoID])
GO
ALTER TABLE [dbo].[WorkstatusSpool] CHECK CONSTRAINT [FK_WorkstatusSpool_UltimoProceso]
GO
/****** Object:  ForeignKey [FK_Wpq_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wpq]  WITH CHECK ADD  CONSTRAINT [FK_Wpq_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Wpq] CHECK CONSTRAINT [FK_Wpq_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Wpq_Soldador]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wpq]  WITH CHECK ADD  CONSTRAINT [FK_Wpq_Soldador] FOREIGN KEY([SoldadorID])
REFERENCES [dbo].[Soldador] ([SoldadorID])
GO
ALTER TABLE [dbo].[Wpq] CHECK CONSTRAINT [FK_Wpq_Soldador]
GO
/****** Object:  ForeignKey [FK_Wpq_Wps]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wpq]  WITH CHECK ADD  CONSTRAINT [FK_Wpq_Wps] FOREIGN KEY([WpsID])
REFERENCES [dbo].[Wps] ([WpsID])
GO
ALTER TABLE [dbo].[Wpq] CHECK CONSTRAINT [FK_Wpq_Wps]
GO
/****** Object:  ForeignKey [FK_Wps_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wps]  WITH CHECK ADD  CONSTRAINT [FK_Wps_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Wps] CHECK CONSTRAINT [FK_Wps_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_Wps_FamiliaAcero_Mb1]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wps]  WITH CHECK ADD  CONSTRAINT [FK_Wps_FamiliaAcero_Mb1] FOREIGN KEY([MaterialBase1ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[Wps] CHECK CONSTRAINT [FK_Wps_FamiliaAcero_Mb1]
GO
/****** Object:  ForeignKey [FK_Wps_FamiliaAcero_Mb2]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wps]  WITH CHECK ADD  CONSTRAINT [FK_Wps_FamiliaAcero_Mb2] FOREIGN KEY([MaterialBase2ID])
REFERENCES [dbo].[FamiliaAcero] ([FamiliaAceroID])
GO
ALTER TABLE [dbo].[Wps] CHECK CONSTRAINT [FK_Wps_FamiliaAcero_Mb2]
GO
/****** Object:  ForeignKey [FK_Wps_ProcesoRaiz]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wps]  WITH CHECK ADD  CONSTRAINT [FK_Wps_ProcesoRaiz] FOREIGN KEY([ProcesoRaizID])
REFERENCES [dbo].[ProcesoRaiz] ([ProcesoRaizID])
GO
ALTER TABLE [dbo].[Wps] CHECK CONSTRAINT [FK_Wps_ProcesoRaiz]
GO
/****** Object:  ForeignKey [FK_Wps_ProcesoRelleno]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[Wps]  WITH CHECK ADD  CONSTRAINT [FK_Wps_ProcesoRelleno] FOREIGN KEY([ProcesoRellenoID])
REFERENCES [dbo].[ProcesoRelleno] ([ProcesoRellenoID])
GO
ALTER TABLE [dbo].[Wps] CHECK CONSTRAINT [FK_Wps_ProcesoRelleno]
GO
/****** Object:  ForeignKey [FK_WpsProyecto_aspnet_Users]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[WpsProyecto]  WITH CHECK ADD  CONSTRAINT [FK_WpsProyecto_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[WpsProyecto] CHECK CONSTRAINT [FK_WpsProyecto_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_WpsProyecto_Proyecto]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[WpsProyecto]  WITH CHECK ADD  CONSTRAINT [FK_WpsProyecto_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[WpsProyecto] CHECK CONSTRAINT [FK_WpsProyecto_Proyecto]
GO
/****** Object:  ForeignKey [FK_WpsProyecto_Wps]    Script Date: 02/03/2011 16:09:50 ******/
ALTER TABLE [dbo].[WpsProyecto]  WITH CHECK ADD  CONSTRAINT [FK_WpsProyecto_Wps] FOREIGN KEY([WpsID])
REFERENCES [dbo].[Wps] ([WpsID])
GO
ALTER TABLE [dbo].[WpsProyecto] CHECK CONSTRAINT [FK_WpsProyecto_Wps]
GO
