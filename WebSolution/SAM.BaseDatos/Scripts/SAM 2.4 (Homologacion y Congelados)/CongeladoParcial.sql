

/****** Object:  Table [dbo].[CongeladoParcial]    Script Date: 01/11/2012 16:56:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CongeladoParcial](
	[CongeladoParcialID] [int] IDENTITY(1,1) NOT NULL,
	[MaterialSpoolID] [int] NOT NULL,
	[NumeroUnicoCongeladoID] [int] NOT NULL,
	[SegmentoCongelado] [nchar](1) NULL,
	[EsEquivalente] [bit] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_CongeladoParcial] PRIMARY KEY CLUSTERED 
(
	[CongeladoParcialID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CongeladoParcial]  WITH CHECK ADD  CONSTRAINT [FK_CongeladoParcial_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [dbo].[CongeladoParcial] CHECK CONSTRAINT [FK_CongeladoParcial_aspnet_Users]
GO

ALTER TABLE [dbo].[CongeladoParcial]  WITH CHECK ADD  CONSTRAINT [FK_CongeladoParcial_MaterialSpool] FOREIGN KEY([MaterialSpoolID])
REFERENCES [dbo].[MaterialSpool] ([MaterialSpoolID])
GO

ALTER TABLE [dbo].[CongeladoParcial] CHECK CONSTRAINT [FK_CongeladoParcial_MaterialSpool]
GO

ALTER TABLE [dbo].[CongeladoParcial]  WITH CHECK ADD  CONSTRAINT [FK_CongeladoParcial_NumeroUnico] FOREIGN KEY([NumeroUnicoCongeladoID])
REFERENCES [dbo].[NumeroUnico] ([NumeroUnicoID])
GO

ALTER TABLE [dbo].[CongeladoParcial] CHECK CONSTRAINT [FK_CongeladoParcial_NumeroUnico]
GO

ALTER TABLE [dbo].[CongeladoParcial] ADD  CONSTRAINT [DF_CongeladoParcial_EsEquivalente]  DEFAULT ((0)) FOR [EsEquivalente]
GO


insert into [TipoReporteProyecto] (TipoReporteProyectoID,Nombre,NombreIngles,OrdenUI) values (16,'Trazabilidad','Tracking',16)