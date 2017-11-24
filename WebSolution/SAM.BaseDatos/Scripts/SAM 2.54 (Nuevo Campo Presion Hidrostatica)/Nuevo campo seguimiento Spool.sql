DECLARE @moduloID INT
SET @moduloID = (SELECT ModuloSeguimientoSpoolID FROM ModuloSeguimientoSpool where Nombre = 'Prueba Hidrostática')

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
		   )
GO