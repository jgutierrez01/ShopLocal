Declare @moduloID INT, @permisoID INT
SET @moduloID = (SELECT ModuloID FROM Modulo WHERE Nombre = 'Work Status')

INSERT INTO Permiso (ModuloID,Nombre,NombreIngles) VALUES(@moduloID, 'Localización Física','Physical location')

SET @permisoID = (SELECT PermisoID FROM Permiso WHERE Nombre = 'Localización Física')

INSERT INTO Pagina (PermisoID,Url) VALUES(@permisoID, '/WorkStatus/LstCuadrantes.aspx')