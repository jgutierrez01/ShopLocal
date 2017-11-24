
INSERT INTO Permiso(
	ModuloID,Nombre,NombreIngles
)
VALUES(
	(Select ModuloID from Modulo
	 where Nombre = 'Calidad'),
	'Liberaci�n Calidad',
	'Quality release'
)

INSERT INTO Pagina(
	PermisoID,
	Url
)
VALUES(
	(SELECT PermisoID from Permiso
	 where nombre = 'Liberaci�n Calidad'),
	 '/Calidad/LiberacionCalidad.aspx'
)