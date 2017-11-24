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
		   4
           ,'Inspector'
           ,'Inspector'
           ,7
           ,'ltsInspeccionVisualInspector'
           ,'InspeccionVisualInspector'
           ,''
           ,''
           ,NULL
           ,NULL
           ,125
           ,'System.String'
		   )
GO



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
		   5
           ,'Inspector'
           ,'Inspector'
           ,6
           ,'ltsInspeccionDimensionalInspector'
           ,'InspeccionDimensionalInspector'
           ,''
           ,''
           ,NULL
           ,NULL
           ,125
           ,'System.String'
		   )
GO



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
		   1
           ,'Inspector'
           ,'Inspector'
           ,7
           ,'litInspeccionDimensionalInspector'
		   ,'InspeccionDimensionalInspector'
		   ,''
		   ,''
           ,NULL
           ,NULL
		   ,125       
		   )
GO