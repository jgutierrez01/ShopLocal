/****** Object:  Table [dbo].[OrdenTrabajoEspecialSpool]    Script Date: 3/20/2014 12:43:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrdenTrabajoEspecialSpool](
	[OrdenTrabajoEspecialSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[OrdenTrabajoEspecialID] [int] NOT NULL,
	[SpoolID] [int] NOT NULL,
	[Partida] [int] NOT NULL,
	[NumeroControl] [nvarchar](50) NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[EsAsignado] [bit] NOT NULL,
 CONSTRAINT [PK_OrdenTrabajoEspecialSpool] PRIMARY KEY CLUSTERED 
(
	[OrdenTrabajoEspecialSpoolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrdenTrabajoEspecialSpool] ADD  DEFAULT ((0)) FOR [EsAsignado]
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecialSpool]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEspecialSpool_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecialSpool] CHECK CONSTRAINT [FK_OrdenTrabajoEspecialSpool_aspnet_Users]
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecialSpool]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEspecialSpool_OrdenTrabajo] FOREIGN KEY([OrdenTrabajoEspecialID])
REFERENCES [dbo].[OrdenTrabajoEspecial] ([OrdenTrabajoEspecialID])
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecialSpool] CHECK CONSTRAINT [FK_OrdenTrabajoEspecialSpool_OrdenTrabajo]
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecialSpool]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEspecialSpool_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecialSpool] CHECK CONSTRAINT [FK_OrdenTrabajoEspecialSpool_Spool]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Solo permite que el spool esté en una ODT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrdenTrabajoEspecialSpool', @level2type=N'CONSTRAINT',@level2name=N'UQ_OrdenTrabajoEspecialSpool_SpoolID'
GO


