/****** Object:  Table [dbo].[CuadranteHistorico]    Script Date: 02/03/2011 16:09:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CuadranteHistorico](
	[CuadranteHistoricoID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolID] [int] NOT NULL,
	[CuadranteID] [int] NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_CuadranteHistorico] PRIMARY KEY CLUSTERED 
(
	[CuadranteHistoricoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[CuadranteHistorico]  WITH CHECK ADD CONSTRAINT [FK_CuadranteHistorico_Cuadrante] FOREIGN KEY([CuadranteID])
REFERENCES [dbo].[Cuadrante] ([CuadranteID])
GO

ALTER TABLE [dbo].[CuadranteHistorico]  WITH CHECK ADD CONSTRAINT [FK_CuadranteHistorico_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO


ALTER TABLE [dbo].[CuadranteHistorico]  WITH CHECK ADD  CONSTRAINT [FK_CuadranteHistorico_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO