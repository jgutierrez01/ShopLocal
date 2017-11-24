/*
Run this script on:

        MIMOSS01.SAM    -  This database will be modified

to synchronize it with:

        MIMOSS01.SAM_21

You are recommended to back up your database before running this script

Script created by SQL Compare version 8.1.0 from Red Gate Software Ltd at 2/4/2011 10:17:27 AM

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Creating [dbo].[Pendiente]'
GO
CREATE TABLE [dbo].[Pendiente]
(
[PendienteID] [int] NOT NULL IDENTITY(1, 1),
[CategoriaPendienteID] [int] NOT NULL,
[TipoPendienteID] [int] NOT NULL,
[ProyectoID] [int] NOT NULL,
[Estatus] [nchar] (1) NOT NULL,
[Titulo] [nvarchar] (150) NOT NULL,
[Descripcion] [nvarchar] (max) NULL,
[FechaApertura] [datetime] NOT NULL,
[GeneradoPor] [uniqueidentifier] NOT NULL,
[AsignadoA] [uniqueidentifier] NOT NULL,
[UsuarioModifica] [uniqueidentifier] NULL,
[FechaModificacion] [datetime] NULL,
[VersionRegistro] [timestamp] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Pendiente] on [dbo].[Pendiente]'
GO
ALTER TABLE [dbo].[Pendiente] ADD CONSTRAINT [PK_Pendiente] PRIMARY KEY CLUSTERED  ([PendienteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[CategoriaPendiente]'
GO
CREATE TABLE [dbo].[CategoriaPendiente]
(
[CategoriaPendienteID] [int] NOT NULL,
[Nombre] [nvarchar] (150) NOT NULL,
[NombreIngles] [nvarchar] (150) NULL,
[UsuarioModifica] [uniqueidentifier] NULL,
[FechaModificacion] [datetime] NULL,
[VersionRegistro] [timestamp] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_CategoriaPendiente] on [dbo].[CategoriaPendiente]'
GO
ALTER TABLE [dbo].[CategoriaPendiente] ADD CONSTRAINT [PK_CategoriaPendiente] PRIMARY KEY CLUSTERED  ([CategoriaPendienteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[TipoPendiente]'
GO
CREATE TABLE [dbo].[TipoPendiente]
(
[TipoPendienteID] [int] NOT NULL,
[EsAutomatico] [bit] NULL,
[Nombre] [nvarchar] (150) NOT NULL,
[NombreIngles] [nvarchar] (150) NULL,
[UsuarioModifica] [uniqueidentifier] NULL,
[FechaModificacion] [datetime] NULL,
[VersionRegistro] [timestamp] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_TipoPendiente] on [dbo].[TipoPendiente]'
GO
ALTER TABLE [dbo].[TipoPendiente] ADD CONSTRAINT [PK_TipoPendiente] PRIMARY KEY CLUSTERED  ([TipoPendienteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[PendienteDetalle]'
GO
CREATE TABLE [dbo].[PendienteDetalle]
(
[PendienteDetalleID] [int] NOT NULL IDENTITY(1, 1),
[PendienteID] [int] NOT NULL,
[CategoriaPendienteID] [int] NOT NULL,
[EsAlta] [bit] NOT NULL,
[Responsable] [uniqueidentifier] NOT NULL,
[Estatus] [nchar] (1) NOT NULL,
[Observaciones] [varchar] (max) NULL,
[UsuarioModifica] [uniqueidentifier] NULL,
[FechaModificacion] [datetime] NULL,
[VersionRegistro] [timestamp] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_PendienteDetalle] on [dbo].[PendienteDetalle]'
GO
ALTER TABLE [dbo].[PendienteDetalle] ADD CONSTRAINT [PK_PendienteDetalle] PRIMARY KEY CLUSTERED  ([PendienteDetalleID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ProyectoPendiente]'
GO
CREATE TABLE [dbo].[ProyectoPendiente]
(
[ProyectoPendienteID] [int] NOT NULL IDENTITY(1, 1),
[ProyectoID] [int] NOT NULL,
[TipoPendienteID] [int] NOT NULL,
[Responsable] [uniqueidentifier] NOT NULL,
[UsuarioModifica] [uniqueidentifier] NULL,
[FechaModificacion] [datetime] NULL,
[VersionRegistro] [timestamp] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ProyectoPendiente] on [dbo].[ProyectoPendiente]'
GO
ALTER TABLE [dbo].[ProyectoPendiente] ADD CONSTRAINT [PK_ProyectoPendiente] PRIMARY KEY CLUSTERED  ([ProyectoPendienteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[OrdenTrabajoSpool]'
GO
ALTER TABLE [dbo].[OrdenTrabajoSpool] ADD
[EsAsignado] [bit] NOT NULL CONSTRAINT [DF_OrdenTrabajoSpool_EsAsignado] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[OrdenTrabajo]'
GO
ALTER TABLE [dbo].[OrdenTrabajo] ADD
[EsAsignado] [bit] NOT NULL CONSTRAINT [DF_OrdenTrabajo_EsAsignado] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[OrdenTrabajoMaterial]'
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] ADD
[EsAsignado] [bit] NOT NULL CONSTRAINT [DF_OrdenTrabajoMaterial_EsAsignado] DEFAULT ((0)),
[NumeroUnicoAsignadoID] [int] NULL,
[SegmentoAsignado] [nchar] (1) NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[Pendiente]'
GO
ALTER TABLE [dbo].[Pendiente] ADD CONSTRAINT [CK_Pendiente_Estatus] CHECK (([Estatus]='C' OR [Estatus]='A' OR [Estatus]='R'))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[OrdenTrabajoMaterial]'
GO
ALTER TABLE [dbo].[OrdenTrabajoMaterial] ADD
CONSTRAINT [FK_OrdenTrabajoMaterial_NumeroUnico_Asignado] FOREIGN KEY ([NumeroUnicoAsignadoID]) REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Pendiente]'
GO
ALTER TABLE [dbo].[Pendiente] ADD
CONSTRAINT [FK_Pendiente_CategoriaPendiente] FOREIGN KEY ([CategoriaPendienteID]) REFERENCES [dbo].[CategoriaPendiente] ([CategoriaPendienteID]),
CONSTRAINT [FK_Pendiente_TipoPendiente] FOREIGN KEY ([TipoPendienteID]) REFERENCES [dbo].[TipoPendiente] ([TipoPendienteID]),
CONSTRAINT [FK_Pendiente_Proyecto] FOREIGN KEY ([ProyectoID]) REFERENCES [dbo].[Proyecto] ([ProyectoID]),
CONSTRAINT [FK_Pendiente_Usuario_GeneradoPor] FOREIGN KEY ([GeneradoPor]) REFERENCES [dbo].[Usuario] ([UserId]),
CONSTRAINT [FK_Pendiente_Usuario_AsignadoA] FOREIGN KEY ([AsignadoA]) REFERENCES [dbo].[Usuario] ([UserId]),
CONSTRAINT [FK_Pendiente_aspnet_Users] FOREIGN KEY ([UsuarioModifica]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[PendienteDetalle]'
GO
ALTER TABLE [dbo].[PendienteDetalle] ADD
CONSTRAINT [FK_PendienteDetalle_CategoriaPendiente] FOREIGN KEY ([CategoriaPendienteID]) REFERENCES [dbo].[CategoriaPendiente] ([CategoriaPendienteID]),
CONSTRAINT [FK_PendienteDetalle_Pendiente] FOREIGN KEY ([PendienteID]) REFERENCES [dbo].[Pendiente] ([PendienteID]),
CONSTRAINT [FK_PendienteDetalle_Usuario] FOREIGN KEY ([Responsable]) REFERENCES [dbo].[Usuario] ([UserId]),
CONSTRAINT [FK_PendienteDetalle_aspnet_Users] FOREIGN KEY ([UsuarioModifica]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[CategoriaPendiente]'
GO
ALTER TABLE [dbo].[CategoriaPendiente] ADD
CONSTRAINT [FK_CategoriaPendiente_aspnet_Users] FOREIGN KEY ([UsuarioModifica]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ProyectoPendiente]'
GO
ALTER TABLE [dbo].[ProyectoPendiente] ADD
CONSTRAINT [FK_ProyectoPendiente_Proyecto] FOREIGN KEY ([ProyectoID]) REFERENCES [dbo].[Proyecto] ([ProyectoID]),
CONSTRAINT [FK_ProyectoPendiente_TipoPendiente] FOREIGN KEY ([TipoPendienteID]) REFERENCES [dbo].[TipoPendiente] ([TipoPendienteID]),
CONSTRAINT [FK_ProyectoPendiente_Usuario] FOREIGN KEY ([Responsable]) REFERENCES [dbo].[Usuario] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[TipoPendiente]'
GO
ALTER TABLE [dbo].[TipoPendiente] ADD
CONSTRAINT [FK_TipoPendiente_aspnet_Users] FOREIGN KEY ([UsuarioModifica]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'MS_Description', N'A = Abierto, C = Cerrado, R = Resuelto', 'SCHEMA', N'dbo', 'TABLE', N'Pendiente', 'CONSTRAINT', N'CK_Pendiente_Estatus'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
