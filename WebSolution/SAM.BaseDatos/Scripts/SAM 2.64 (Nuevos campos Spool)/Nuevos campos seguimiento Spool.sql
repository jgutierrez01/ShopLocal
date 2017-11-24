DECLARE @moduloID INT
SET @moduloID = (SELECT ModuloSeguimientoSpoolID FROM ModuloSeguimientoSpool where Nombre = 'Agrupadores')

INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Último Proceso'
			, 'Last Process'
			, 63
			, 'ltsSpoolUltimoProceso'
			, 'AgrupadoresSpoolUltimoProceso'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

	INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Área Grupo'
			, 'Area Group'
			, 64
			, 'ltsAreaGrupo'
			, 'AgrupadoresAreaGrupo'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )
	
	INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Kgs Grupo'
			, 'Kgs Group'
			, 65
			, 'ltsKgsGrupo'
			, 'AgrupadoresKgsGrupo'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Diam Grupo'
			, 'Diam Group'
			, 66
			, 'ltsDiamGrupo'
			, 'AgrupadoresDiamGrupo'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'PEQ Grupo'
			, 'PEQ Group'
			, 67
			, 'ltsPeqGrupo'
			, 'AgrupadoresPeqGrupo'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

	INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Sistema Pintura Final'
			, 'Final Paint System'
			, 68
			, 'ltsSistemaPinturaFinal'
			, 'AgrupadoresSistemaPinturaFinal'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

	INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Paint/NoPaint'
			, 'Paint/NoPaint'
			, 69
			, 'ltsPaintNoPaint'
			, 'AgrupadoresPaintNoPaint'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

	INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Diametro Promedio'
			, 'Average Diameter'
			, 70
			, 'ltsDiametroPromedio'
			, 'AgrupadoresDiametroPromedio'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

	INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Paint Line'
			, 'Paint Line'
			, 71
			, 'ltsPaintLine'
			, 'AgrupadoresPaintLine'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

 INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Área EQ'
			, 'EQ Area'
			, 72
			, 'ltsAreaEQ'
			, 'AgrupadoresAreaEQ'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Inox/NoInox'
			, 'Inox/NoInox'
			, 73
			, 'ltsInoxNoInox'
			, 'AgrupadoresInoxNoInox'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )

INSERT INTO [dbo].[CampoSeguimientoSpool]
           ([ModuloSeguimientoSpoolID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[UsuarioModifica]
           ,[FechaModificacion]
           ,[AnchoUI])
     VALUES
           (
			@moduloID
			, 'Clasif Inox'
			, 'Clasif Inox'
			, 74
			, 'ltsClasifInox'
			, 'AgrupadoresClasifInox'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )
GO

