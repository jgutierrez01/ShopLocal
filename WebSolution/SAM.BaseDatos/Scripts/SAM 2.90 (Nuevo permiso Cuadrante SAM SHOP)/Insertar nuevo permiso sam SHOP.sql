--INSERTAR NUEVO PERMISO PARA CUADRANTES EN SAM SHOP

DECLARE @permisoId INT
SET @permisoId = (SELECT PermisoID FROM Permiso WHERE Nombre = 'Localizaci�n F�sica')

INSERT INTO Pagina (PermisoID, Url, FechaModificacion) VALUES(@permisoId, '/Location', GETDATE())