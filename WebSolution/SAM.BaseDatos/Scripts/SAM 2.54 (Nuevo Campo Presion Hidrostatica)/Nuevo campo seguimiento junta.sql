DECLARE @moduloID INT
SET @moduloID = (SELECT ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'Prueba Hidrostática')

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
			, 'Presion'
			, 'Pressure'
			, 8
			, 'ltsPruebaHidroPresion'
			, 'PruebaHidroPresion'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.String'
		   )
GO