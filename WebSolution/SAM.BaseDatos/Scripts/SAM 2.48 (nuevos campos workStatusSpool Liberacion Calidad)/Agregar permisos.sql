
INSERT INTO Permiso(
	ModuloID,Nombre,NombreIngles
)
VALUES(
	(Select ModuloID from Modulo
	 where Nombre = 'Calidad'),
	'Liberación Calidad',
	'Quality release'
)

INSERT INTO Pagina(
	PermisoID,
	Url
)
VALUES(
	(SELECT PermisoID from Permiso
	 where nombre = 'Liberación Calidad'),
	 '/Calidad/LiberacionCalidad.aspx'
)