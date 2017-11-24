DECLARE @moduloID INT
SET @moduloID = (SELECT ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'General')

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
			, 'Fecha Cuadrante'
			, 'Quadrant Dates'
			, 10
			, 'ltsGeneralFechaLocalizacion'
			, 'GeneralFechaLocalizacion'
			, ''
			, ''
			, NULL
			, NULL
			, 125
			, 'System.DateTime'
		   )
GO



