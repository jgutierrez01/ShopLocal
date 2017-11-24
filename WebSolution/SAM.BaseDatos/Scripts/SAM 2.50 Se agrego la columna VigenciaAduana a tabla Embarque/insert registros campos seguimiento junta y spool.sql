

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
           (16
           ,'Vigencia Aduana'
           ,'Validity Customs'
           ,4
           ,'litEmbarqueVigenciaAduana'
           ,'EmbarqueVigenciaAduana'
           ,'d'
           ,''
           ,NULL
           ,NULL
		   ,125
           ,'System.DateTime')


INSERT INTO dbo.CampoSeguimientoSpool
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
           ,AnchoUI)
     VALUES
           (5
           ,'Vigencia Aduana'
           ,'Validity Customs'
           ,4
           ,'litEmbarqueVigenciaAduana'
           ,'EmbarqueVigenciaAduana'
           ,'d'
           ,''
           ,NULL
           ,NULL
           ,125)

		