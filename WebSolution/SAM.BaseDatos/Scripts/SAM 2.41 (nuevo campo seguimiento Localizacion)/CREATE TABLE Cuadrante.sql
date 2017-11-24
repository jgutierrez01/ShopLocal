/****** Object:  Table [dbo].[Cuadrante]    Script Date: 2/10/2014 4:29:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Cuadrante](
	[CuadranteID] [int] IDENTITY(1,1) NOT NULL,
	[PatioID] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CuadranteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Cuadrante]  WITH CHECK ADD FOREIGN KEY([PatioID])
REFERENCES [dbo].[Patio] ([PatioID])
GO


