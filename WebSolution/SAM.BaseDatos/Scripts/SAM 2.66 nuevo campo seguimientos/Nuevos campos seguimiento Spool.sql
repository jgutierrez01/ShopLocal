DECLARE @moduloID INT
SET @moduloID = (SELECT ModuloSeguimientoSpoolID FROM ModuloSeguimientoSpool where Nombre = 'General')

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
			, 'Fecha Cuadrante'
			, 'Quadrant Date'
			, 10
			, 'ltsGeneralFechaLocalizacion'
			, 'GeneralFechaLocalizacion'
			, ''
			, ''
			, NULL
			, NULL
			, 125
		   )
GO
