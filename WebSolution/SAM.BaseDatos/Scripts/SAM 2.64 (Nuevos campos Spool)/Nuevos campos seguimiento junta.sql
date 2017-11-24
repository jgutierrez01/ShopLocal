DECLARE @moduloID INT
SET @moduloID = (SELECT ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'Agrupadores')

INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Último Proceso'
			, 'Last Process'
			, 64
			, 'ltsSpoolUltimoProceso'
			, 'AgrupadoresSpoolUltimoProceso'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )


 INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Área Grupo'
			, 'Area Group'
			, 65
			, 'ltsAreaGrupo'
			, 'AgrupadoresAreaGrupo'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )


 INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Kgs Grupo'
			, 'Kgs Group'
			, 66
			, 'ltsKgsGrupo'
			, 'AgrupadoresKgsGrupo'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )


 INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Diam Grupo'
			, 'Diam Group'
			, 67
			, 'ltsDiamGrupo'
			, 'AgrupadoresDiamGrupo'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )

INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'PEQ Grupo'
			, 'PEQ Group'
			, 68
			, 'ltsPeqGrupo'
			, 'AgrupadoresPeqGrupo'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )

INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Sistema Pintura Final'
			, 'Final Paint System'
			, 69
			, 'ltsSistemaPinturaFinal'
			, 'AgrupadoresSistemaPinturaFinal'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )


 INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Paint/NoPaint'
			, 'Paint/NoPaint'
			, 70
			, 'ltsPaintNoPaint'
			, 'AgrupadoresPaintNoPaint'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )


 INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Diametro Promedio'
			, 'Average Diameter'
			, 71
			, 'ltsDiametroPromedio'
			, 'AgrupadoresDiametroPromedio'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )
		   

   INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'PaintLine'
			, 'PaintLine'
			, 72
			, 'ltsPaintLine'
			, 'AgrupadoresPaintLine'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )


INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Área EQ'
			, 'EQ Area'
			, 73
			, 'ltsAreaEQ'
			, 'AgrupadoresAreaEQ'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )

 INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Inox/NoInox'
			, 'Inox/NoInox'
			, 74
			, 'ltsInoxNoInox'
			, 'AgrupadoresInoxNoInox'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )

INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (
			@moduloID
			, 'Clasif Inox'
			, 'Clasif Inox'
			, 75
			, 'ltsClasifInox'
			, 'AgrupadoresClasifInox'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )

GO

