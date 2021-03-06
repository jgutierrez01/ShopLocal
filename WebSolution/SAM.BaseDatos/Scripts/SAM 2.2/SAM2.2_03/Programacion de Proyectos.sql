/****** Object:  Table [dbo].[ProyectoPrograma]    Script Date: 06/08/2011 14:59:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProyectoPrograma](
	[ProyectoProgramaID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoID] [int] NOT NULL,
	[Rango] [nchar](1) NULL,
	[Unidades] [nchar](1) NULL,
	[IsosPlaneados] [int] NULL,
	[IsosReprogramados] [int] NULL,
	[SpoolsPlaneados] [int] NULL,
	[SpoolsReprogramados] [int] NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_ProyectoPrograma] PRIMARY KEY CLUSTERED 
(
	[ProyectoProgramaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UQ_ProyectoPrograma_ProyectoID] UNIQUE NONCLUSTERED 
(
	[ProyectoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeriodoPrograma]    Script Date: 06/08/2011 14:59:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeriodoPrograma](
	[PeriodoProgramaID] [int] IDENTITY(1,1) NOT NULL,
	[ProyectoProgramaID] [int] NOT NULL,
	[Numero] [int] NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NOT NULL,
	[PorContrato] [decimal](10, 2) NOT NULL,
	[Reprogramaciones] [decimal](10, 2) NOT NULL,
	[UsuarioModifica] [uniqueidentifier] NULL,
	[FechaModificacion] [datetime] NULL,
	[VersionRegistro] [timestamp] NOT NULL,
 CONSTRAINT [PK_PeriodoPrograma] PRIMARY KEY CLUSTERED 
(
	[PeriodoProgramaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UQ_PeriodoPrograma_ProyectoProgramaID_Numero] UNIQUE NONCLUSTERED 
(
	[ProyectoProgramaID] ASC,
	[Numero] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Check [CK_ProyectoPrograma_Rango]    Script Date: 06/08/2011 14:59:55 ******/
ALTER TABLE [dbo].[ProyectoPrograma]  WITH CHECK ADD  CONSTRAINT [CK_ProyectoPrograma_Rango] CHECK  (([Rango]='M' OR [Rango]='S' OR [Rango]='D'))
GO
ALTER TABLE [dbo].[ProyectoPrograma] CHECK CONSTRAINT [CK_ProyectoPrograma_Rango]
GO
/****** Object:  Check [CK_ProyectoPrograma_Unidades]    Script Date: 06/08/2011 14:59:55 ******/
ALTER TABLE [dbo].[ProyectoPrograma]  WITH CHECK ADD  CONSTRAINT [CK_ProyectoPrograma_Unidades] CHECK  (([Unidades]='K' OR [Unidades]='M' OR [Unidades]='P'))
GO
ALTER TABLE [dbo].[ProyectoPrograma] CHECK CONSTRAINT [CK_ProyectoPrograma_Unidades]
GO
/****** Object:  ForeignKey [FK_ProyectoPrograma_aspnet_Users]    Script Date: 06/08/2011 14:59:55 ******/
ALTER TABLE [dbo].[ProyectoPrograma]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoPrograma_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[ProyectoPrograma] CHECK CONSTRAINT [FK_ProyectoPrograma_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_ProyectoPrograma_Proyecto]    Script Date: 06/08/2011 14:59:55 ******/
ALTER TABLE [dbo].[ProyectoPrograma]  WITH CHECK ADD  CONSTRAINT [FK_ProyectoPrograma_Proyecto] FOREIGN KEY([ProyectoID])
REFERENCES [dbo].[Proyecto] ([ProyectoID])
GO
ALTER TABLE [dbo].[ProyectoPrograma] CHECK CONSTRAINT [FK_ProyectoPrograma_Proyecto]
GO
/****** Object:  ForeignKey [FK_PeriodoPrograma_aspnet_Users]    Script Date: 06/08/2011 14:59:55 ******/
ALTER TABLE [dbo].[PeriodoPrograma]  WITH CHECK ADD  CONSTRAINT [FK_PeriodoPrograma_aspnet_Users] FOREIGN KEY([UsuarioModifica])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[PeriodoPrograma] CHECK CONSTRAINT [FK_PeriodoPrograma_aspnet_Users]
GO
/****** Object:  ForeignKey [FK_PeriodoPrograma_ProyectoPrograma]    Script Date: 06/08/2011 14:59:55 ******/
ALTER TABLE [dbo].[PeriodoPrograma]  WITH CHECK ADD  CONSTRAINT [FK_PeriodoPrograma_ProyectoPrograma] FOREIGN KEY([ProyectoProgramaID])
REFERENCES [dbo].[ProyectoPrograma] ([ProyectoProgramaID])
GO
ALTER TABLE [dbo].[PeriodoPrograma] CHECK CONSTRAINT [FK_PeriodoPrograma_ProyectoPrograma]
GO

insert into ProyectoPrograma(ProyectoID)
select	p.ProyectoID
from Proyecto p
GO

