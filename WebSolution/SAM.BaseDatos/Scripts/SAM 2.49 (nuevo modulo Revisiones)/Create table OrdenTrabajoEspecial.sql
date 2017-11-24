/****** Object:  Table [dbo].[OrdenTrabajoEspecial]    Script Date: 3/20/2014 12:31:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrdenTrabajoEspecial](
	[OrdenTrabajoEspecialID] [int] IDENTITY(1,1) NOT NULL,
	[EstatusOrdenID] [int] NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[TallerID] [int] NOT NULL,
	[NumeroOrden] [nvarchar](50) NULL,
	[FechaOrden] [datetime] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
	[EsAsignado] [bit] NOT NULL,
	[VersionOrden] [int] NULL,
 CONSTRAINT [PK_OrdenTrabajoEspecial] PRIMARY KEY CLUSTERED 
(
	[OrdenTrabajoEspecialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial] ADD  DEFAULT ((0)) FOR [EsAsignado]
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial] ADD  CONSTRAINT [DF_OrdenTrabajoEspecial_VersionOrden]  DEFAULT ((0)) FOR [VersionOrden]
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEspecial_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial] CHECK CONSTRAINT [FK_OrdenTrabajoEspecial_aspnet_Users]
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEspecial_EstatusOrden] FOREIGN KEY([EstatusOrdenID])
REFERENCES [dbo].[EstatusOrden] ([EstatusOrdenID])
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial] CHECK CONSTRAINT [FK_OrdenTrabajoEspecial_EstatusOrden]
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEspecial_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial] CHECK CONSTRAINT [FK_OrdenTrabajoEspecial_Proyecto]
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEspecial_Taller] FOREIGN KEY([TallerID])
REFERENCES [dbo].[Taller] ([TallerID])
GO

ALTER TABLE [dbo].[OrdenTrabajoEspecial] CHECK CONSTRAINT [FK_OrdenTrabajoEspecial_Taller]
GO


