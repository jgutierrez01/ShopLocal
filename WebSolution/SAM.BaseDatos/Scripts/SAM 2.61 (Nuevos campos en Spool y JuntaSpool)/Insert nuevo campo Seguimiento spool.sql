declare @moduloGeneralID INT = (select ModuloSeguimientoSpoolID from ModuloSeguimientoSpool where Nombre = 'General');

INSERT INTO CampoSeguimientoSpool
           (ModuloSeguimientoSpoolID
           ,Nombre
           ,NombreIngles
           ,OrdenUI
           ,NombreControlUI
           ,NombreColumnaSp
           ,DataFormat
           ,CssColumnaUI
           ,UsuarioModifica
           ,FechaModificacion
           ,AnchoUI
           )
     VALUES
           ( @moduloGeneralID
           ,'Requiere Prueba Hidrostática'
           ,'Required Hydrostatic Test'
           ,19
           ,'litRequierePruebaHidrostatica'
           ,'GeneralRequierePruebaHidrostatica'
           ,''
           ,''
           ,NULL
           ,NULL
		   ,125)
