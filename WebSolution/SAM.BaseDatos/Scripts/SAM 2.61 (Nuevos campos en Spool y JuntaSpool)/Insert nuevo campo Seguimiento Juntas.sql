declare @moduloGeneralID INT = (select ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'General');

INSERT INTO CampoSeguimientoJunta
           (ModuloSeguimientoJuntaID
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
           ,TipoDeDato)
     VALUES
           ( @moduloGeneralID
           ,'Junta Requiere Prueba Neumática'
           ,'Joint Required Neumatic Test'
           ,13
           ,'litJuntaRequierePruebaNeumatica'
           ,'GeneralJuntaRequierePruebaNeumatica'
           ,''
           ,''
           ,NULL
           ,NULL
		   ,125
           ,'System.Boolean')


		   INSERT INTO CampoSeguimientoJunta
           (ModuloSeguimientoJuntaID
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
           ,TipoDeDato)
     VALUES
           ( @moduloGeneralID
           ,'Requiere Prueba Hidrostática'
           ,'Required Hydrostatic Test'
           ,13
           ,'litRequierePruebaHidrostatica'
           ,'GeneralRequierePruebaHidrostatica'
           ,''
           ,''
           ,NULL
           ,NULL
		   ,125
           ,'System.Boolean')
