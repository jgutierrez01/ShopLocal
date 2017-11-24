DECLARE @ModuloRT INT, @CuentaOrdenUI INT
SET @ModuloRT = (SELECT ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'Prueba RT')
SET @CuentaOrdenUI = (SELECT COUNT(CampoSeguimientoJuntaID) from CampoSeguimientoJunta where ModuloSeguimientoJuntaID = @ModuloRT)

--select * from CampoSeguimientoJunta where ModuloSeguimientoJuntaID = 7

INSERT INTO [dbo].[CampoSeguimientoJunta]
           ([ModuloSeguimientoJuntaID]
           ,[Nombre]
           ,[NombreIngles]
           ,[OrdenUI]
           ,[NombreControlUI]
           ,[NombreColumnaSp]
           ,[DataFormat]
           ,[CssColumnaUI]
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (@ModuloRT
		   , 'Junta Seguimiento 1'
		   , 'Joint Tracing 1'
		   , (@CuentaOrdenUI + 1)
		   , 'ltsPruebaRTJuntaSeguimiento1'
		   , 'PruebaRTJuntaSeguimiento1'
		   , ''
		   , ''
		   , 150
		   , 'System.Int32'
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
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (@ModuloRT
		   , 'Junta Seguimiento 2'
		   , 'Joint Tracing 2'
		   , (@CuentaOrdenUI + 2)
		   , 'ltsPruebaRTJuntaSeguimiento2'
		   , 'PruebaRTJuntaSeguimiento2'
		   , ''
		   , ''
		   , 150
		   , 'System.Int32'
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
           ,[AnchoUI]
           ,[TipoDeDato])
     VALUES
           (@ModuloRT
		   , 'Junta Original Seguimiento'
		   , 'Original tracing joint'
		   , (@CuentaOrdenUI + 1)
		   , 'ltsPruebaRTReferenciaSeguimiento'
		   , 'PruebaRTReferenciaSeguimiento'
		   , ''
		   , ''
		   , 150
		   , 'System.String'
		   )

		   