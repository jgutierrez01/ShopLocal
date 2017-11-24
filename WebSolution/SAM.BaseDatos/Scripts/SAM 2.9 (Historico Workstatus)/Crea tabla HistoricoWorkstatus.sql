
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HistoricoWorkstatus](
	[HistoricoWorkstatusID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolID] [int] NOT NULL,
	[RevisionCliente] [nvarchar](10) NOT NULL,
	[Revision] [nvarchar](10) NOT NULL,
	[FechaHomologacion] [datetime] NOT NULL,
	[ArchivoSpool] [nvarchar](max) NOT NULL,
	[ArchivoJuntas] [nvarchar](max) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NULL,
 CONSTRAINT [PK_HistoricoWorkstatus] PRIMARY KEY CLUSTERED 
(
	[HistoricoWorkstatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[HistoricoWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_HistoricoWorkstatus_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO

ALTER TABLE [dbo].[HistoricoWorkstatus] CHECK CONSTRAINT [FK_HistoricoWorkstatus_Spool]
GO

ALTER TABLE [dbo].[HistoricoWorkstatus]  WITH CHECK ADD  CONSTRAINT [FK_HistoricoWorkstatus_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO

ALTER TABLE [dbo].[HistoricoWorkstatus] CHECK CONSTRAINT [FK_HistoricoWorkstatus_Usuario]
GO


