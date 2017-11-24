/****** Object:  Table [dbo].[TransferenciaSpool]    Script Date: 9/8/2014 7:50:51 AM ******/
CREATE TABLE [dbo].[TransferenciaSpool](
	[TransferenciaSpoolID] [int] IDENTITY(1,1) NOT NULL,
	[SpoolID] [int] NOT NULL,
	[SpoolPreparado] [bit] NOT NULL,
	[FechaPreparacion] [datetime] NOT NULL,	
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransferenciaSpoolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TransferenciaSpool] ADD  CONSTRAINT [DF_TransferenciaSpool_SpoolPreparado]  DEFAULT ((0)) FOR [SpoolPreparado]
GO

ALTER TABLE [dbo].[TransferenciaSpool]  WITH CHECK ADD  CONSTRAINT [FK_TransferenciaSpool_Spool] FOREIGN KEY([SpoolID])
REFERENCES [dbo].[Spool] ([SpoolID])
GO

ALTER TABLE [dbo].[TransferenciaSpool] CHECK CONSTRAINT [FK_TransferenciaSpool_Spool]
GO

ALTER TABLE [dbo].[TransferenciaSpool]  WITH CHECK ADD  CONSTRAINT [FK_TransferenciaSpool_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO

ALTER TABLE [dbo].[TransferenciaSpool] CHECK CONSTRAINT [FK_TransferenciaSpool_Usuario]
GO




/****** Object:  Table [dbo].[Transferencia]    Script Date: 9/8/2014 7:50:51 AM ******/
CREATE TABLE [dbo].[Transferencia](
	[TransferenciaID] [int] IDENTITY(1,1) NOT NULL,
	[TransferenciaSpoolID][int] NOT NULL,
	[NumeroTransferencia] [nvarchar](100) NOT NULL,
	[FechaTransferencia] [datetime] NOT NULL,
	[DestinoID] [int] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransferenciaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Transferencia]  WITH CHECK ADD  CONSTRAINT [FK_Transferencia_TransferenciaSpoolID] FOREIGN KEY([TransferenciaSpoolID])
REFERENCES [dbo].[TransferenciaSpool] ([TransferenciaSpoolID])
GO

ALTER TABLE [dbo].[Transferencia] CHECK CONSTRAINT [FK_Transferencia_TransferenciaSpoolID]
GO

ALTER TABLE [dbo].[Transferencia]  WITH CHECK ADD  CONSTRAINT [FK_Transferencia_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO

ALTER TABLE [dbo].[Transferencia] CHECK CONSTRAINT [FK_Transferencia_Usuario]
GO

/****** Object:  Table [dbo].[Destino]    Script Date: 9/8/2014 7:50:51 AM ******/
CREATE TABLE [dbo].[Destino](
	[DestinoID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[CuadranteID] [int] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DestinoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Destino]  WITH CHECK ADD  CONSTRAINT [FK_Destino_CuadranteID] FOREIGN KEY([CuadranteID])
REFERENCES [dbo].[Cuadrante] ([CuadranteID])
GO

ALTER TABLE [dbo].[Destino] CHECK CONSTRAINT [FK_Destino_CuadranteID]
GO

ALTER TABLE [dbo].[Destino]  WITH CHECK ADD  CONSTRAINT [FK_Destino_Usuario] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[Usuario] ([UserId])
GO

ALTER TABLE [dbo].[Destino] CHECK CONSTRAINT [FK_Destino_Usuario]
GO
