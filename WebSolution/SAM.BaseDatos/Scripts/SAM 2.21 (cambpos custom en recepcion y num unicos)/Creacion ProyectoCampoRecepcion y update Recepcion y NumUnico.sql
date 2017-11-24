

CREATE TABLE [dbo].[ProyectoCamposRecepcion](
	[ProyectoID] [int] NOT NULL,
	[CantidadCamposRecepcion] [int] NOT NULL,
	[CampoRecepcion1] [nvarchar](100) NULL,
	[CampoRecepcion2] [nvarchar](100) NULL,
	[CampoRecepcion3] [nvarchar](100) NULL,
	[CampoRecepcion4] [nvarchar](100) NULL,
	[CampoRecepcion5] [nvarchar](100) NULL,
	[CantidadCamposNumeroUnico] [int] NOT NULL,
	[CampoNumeroUnico1] [nvarchar](100) NULL,
	[CampoNumeroUnico2] [nvarchar](100) NULL,
	[CampoNumeroUnico3] [nvarchar](100) NULL,
	[CampoNumeroUnico4] [nvarchar](100) NULL,
	[CampoNumeroUnico5] [nvarchar](100) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProyectoCamposRecepcion] PRIMARY KEY CLUSTERED 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ProyectoCamposRecepcion]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoCamposRecepcion_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO

ALTER TABLE [dbo].[ProyectoCamposRecepcion] CHECK CONSTRAINT [FK_ProyectoCamposRecepcion_Proyecto]
GO

ALTER TABLE [dbo].[ProyectoCamposRecepcion]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoCamposRecepcion_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO

ALTER TABLE [dbo].[ProyectoCamposRecepcion] CHECK CONSTRAINT [FK_ProyectoCamposRecepcion_Usuario]
GO

ALTER TABLE [dbo].[Recepcion] ADD [CampoLibre1] [nvarchar](100) NULL 
ALTER TABLE [dbo].[Recepcion] ADD [CampoLibre2] [nvarchar](100) NULL 
ALTER TABLE [dbo].[Recepcion] ADD [CampoLibre3] [nvarchar](100) NULL 
ALTER TABLE [dbo].[Recepcion] ADD [CampoLibre4] [nvarchar](100) NULL 
ALTER TABLE [dbo].[Recepcion] ADD [CampoLibre5] [nvarchar](100) NULL 



ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibreRecepcion1] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibreRecepcion2] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibreRecepcion3] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibreRecepcion4] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibreRecepcion5] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibre1] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibre2] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibre3] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibre4] [nvarchar](100) NULL 
ALTER TABLE [dbo].[NumeroUnico] ADD	[CampoLibre5] [nvarchar](100) NULL 

