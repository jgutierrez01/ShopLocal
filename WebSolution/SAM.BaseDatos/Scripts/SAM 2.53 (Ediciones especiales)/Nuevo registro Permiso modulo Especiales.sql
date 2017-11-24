DECLARE @ModuloID INT

SET @ModuloID = (SELECT ModuloID FROM Modulo WHERE Nombre = 'Especiales') 

INSERT INTO [dbo].[Permiso]
           ([ModuloID]
           ,[Nombre]
           ,[NombreIngles]
           ,[Descripcion]
           ,[UsuarioModifica]
           ,[FechaModificacion])
     VALUES
           (@ModuloID,
		   'Ediciones Especiales',
		   'Special Edition',
		   'Permisos especiales para edicion',
		   null,
		   GETDATE()
		   )
GO


