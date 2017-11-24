--select * from Modulo
DECLARE @moduloID INT
SET @moduloID = (SELECT ModuloID FROM Modulo WHERE Nombre = 'Catálogos')

--Select * from Permiso where ModuloID = 7

INSERT INTO Permiso (ModuloID,Nombre, NombreIngles) VALUES(@moduloID,'Listado de Cortadores', 'Cutters List')
INSERT INTO Permiso (ModuloID,Nombre, NombreIngles) VALUES(@moduloID,'Detalle de Cortadores', 'Cutters Detail')

INSERT INTO Permiso (ModuloID,Nombre, NombreIngles) VALUES(@moduloID,'Listado de Despachadores', 'dispatchers List')
INSERT INTO Permiso (ModuloID,Nombre, NombreIngles) VALUES(@moduloID,'Detalle de Despachadores', 'dispatchers Detail') 

--select * from pagina

INSERT INTO Pagina (PermisoID,Url) VALUES((SELECT PermisoID FROM Permiso WHERE Nombre = 'Listado de Cortadores'),'/Catalogos/LstCortador.aspx')
INSERT INTO Pagina (PermisoID,Url) VALUES((SELECT PermisoID FROM Permiso WHERE Nombre = 'Detalle de Cortadores'),'/Catalogos/DetCortador.aspx')

INSERT INTO Pagina (PermisoID,Url) VALUES((SELECT PermisoID FROM Permiso WHERE Nombre = 'Listado de Despachadores'),'/Catalogos/LstDespachador.aspx')
INSERT INTO Pagina (PermisoID,Url) VALUES((SELECT PermisoID FROM Permiso WHERE Nombre = 'Detalle de Despachadores'),'/Catalogos/DetDespachador.aspx')