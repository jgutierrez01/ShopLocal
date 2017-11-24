
/****** Object:  Table [dbo].[Estacion]    Script Date: 05/22/2013 09:30:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Estacion](
	[EstacionID] [int] IDENTITY(1,1) NOT NULL,
	[TallerID] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_Estacion] PRIMARY KEY CLUSTERED 
(
	[EstacionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Estacion]  WITH CHECK ADD  CONSTRAINT [FK_Estacion_Taller] FOREIGN KEY([TallerID])
REFERENCES [dbo].[Taller] ([TallerID])
GO

ALTER TABLE [dbo].[Estacion] CHECK CONSTRAINT [FK_Estacion_Taller]
GO

ALTER TABLE [dbo].[Estacion]  WITH CHECK ADD  CONSTRAINT [FK_Estacion_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO

ALTER TABLE [dbo].[Estacion] CHECK CONSTRAINT [FK_Estacion_Usuario]
GO



/****** Object:  Table [dbo].[BastonSpool]    Script Date: 05/17/2013 12:02:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BastonSpool](
	[BastonSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolID] [int] NOT NULL,
	[LetraBaston] [char](1) NOT NULL,
	[EstacionID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_BastonSpool] PRIMARY KEY CLUSTERED 
(
	[BastonSpoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[BastonSpool]  WITH CHECK ADD  CONSTRAINT [FK_BastonSpool_Estacion] FOREIGN KEY([EstacionID])
REFERENCES [dbo].[Estacion] ([EstacionID])
GO

ALTER TABLE [dbo].[BastonSpool] CHECK CONSTRAINT [FK_BastonSpool_Estacion]
GO

ALTER TABLE [dbo].[BastonSpool]  WITH CHECK ADD  CONSTRAINT [FK_BastonSpool_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO

ALTER TABLE [dbo].[BastonSpool] CHECK CONSTRAINT [FK_BastonSpool_Spool]
GO

ALTER TABLE [dbo].[BastonSpool]  WITH CHECK ADD  CONSTRAINT [FK_BastonSpool_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO

ALTER TABLE [dbo].[BastonSpool] CHECK CONSTRAINT [FK_BastonSpool_Usuario]
GO



/****** Object:  Table [dbo].[BastonSpoolJunta]    Script Date: 05/17/2013 12:02:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BastonSpoolJunta](
	[BastonSpoolJuntaID] [int] IDENTITY(1,1) NOT NULL,
	[BastonSpoolID] [int] NOT NULL,
	[JuntaSpoolID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_BastonSpoolJunta] PRIMARY KEY CLUSTERED 
(
	[BastonSpoolJuntaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BastonSpoolJunta]  WITH CHECK ADD  CONSTRAINT [FK_BastonSpoolJunta_BastonSpool] FOREIGN KEY([BastonSpoolID])
REFERENCES [dbo].[BastonSpool] ([BastonSpoolID])
GO

ALTER TABLE [dbo].[BastonSpoolJunta] CHECK CONSTRAINT [FK_BastonSpoolJunta_BastonSpool]
GO

ALTER TABLE [dbo].[BastonSpoolJunta]  WITH CHECK ADD  CONSTRAINT [FK_BastonSpoolJunta_JuntaSpool] FOREIGN KEY([JuntaSpoolID])
REFERENCES [dbo].[JuntaSpool] ([JuntaSpoolID])
GO

ALTER TABLE [dbo].[BastonSpoolJunta] CHECK CONSTRAINT [FK_BastonSpoolJunta_JuntaSpool]
GO

ALTER TABLE [dbo].[BastonSpoolJunta]  WITH CHECK ADD  CONSTRAINT [FK_BastonSpoolJunta_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO

ALTER TABLE [dbo].[BastonSpoolJunta] CHECK CONSTRAINT [FK_BastonSpoolJunta_Usuario]
GO



-- SE AGREGA NUEVA COLUMNA
alter table JuntaSpool
add EsManual Bit NULL
, EstacionID int NULL
GO

ALTER TABLE [dbo].[JuntaSpool]  WITH CHECK ADD  CONSTRAINT [FK_JuntaSpool_Estacion] FOREIGN KEY([EstacionID])
REFERENCES [dbo].[Estacion] ([EstacionID])
GO

ALTER TABLE [dbo].[JuntaSpool] CHECK CONSTRAINT [FK_JuntaSpool_Estacion]
GO

