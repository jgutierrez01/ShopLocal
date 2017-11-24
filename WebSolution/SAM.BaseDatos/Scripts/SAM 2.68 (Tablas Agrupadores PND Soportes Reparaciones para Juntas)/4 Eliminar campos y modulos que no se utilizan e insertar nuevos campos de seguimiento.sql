
DECLARE @agSopId INT, @agRepId INT, @agId INT
SET @agId = (SELECT ModuloSeguimientoJuntaID FROM ModuloSeguimientoJunta WHERE Nombre = 'Agrupadores PND')
SET @agSopId = (SELECT ModuloSeguimientoJuntaID FROM ModuloSeguimientoJunta WHERE Nombre = 'Agrupadores Soportes')
SET @agRepId = (SELECT ModuloSeguimientoJuntaID FROM ModuloSeguimientoJunta WHERE Nombre = 'Agrupadores Reparaciones')

UPDATE ModuloSeguimientoJunta SET Nombre = 'Agrupadores por Junta',  NombreIngles = 'Joint Groupers', NombreTemplateColumn = 'AgrupadoresPorJunta' where ModuloSeguimientoJuntaID = @agId

--Eliminar los campos que no se van a usar para los seguimientos
DELETE FROM CampoSeguimientoJunta WHERE ModuloSeguimientoJuntaID = @agRepId OR ModuloSeguimientoJuntaID = @agSopId or ModuloSeguimientoJuntaID = @agId

--Insertamos los nuevos campos para el seguimiento
INSERT INTO CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agId, 'Clasificación PND', 'PND Classification', 1, 'litAgrupadoresPorJuntaClasifPND', 'AgrupadoresPorJuntaClasifPND', 150 , '', '')
INSERT INTO CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agId, 'Clasificación Reparación', 'Repair Classification', 2, 'litAgrupadoresPorJuntaClasifReparacion', 'AgrupadoresPorJuntaClasifReparacion', 150, '', '')
INSERT INTO CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agId, 'Clasificación Soporte', 'Mounting Classification', 3, 'litAgrupadoresPorJuntaClasifSoporte', 'AgrupadoresPorJuntaClasifSoporte', 150, '', '')
INSERT INTO CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agId, 'Grupo 1', 'Group 1', 4, 'litAgrupadoresPorJuntaGrupo1', 'AgrupadoresPorJuntaGrupo1', 150, '', '')
INSERT INTO CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agId, 'Grupo 2', 'Group 2', 5, 'litAgrupadoresPorJuntaGrupo2', 'AgrupadoresPorJuntaGrupo2', 150, '', '')
INSERT INTO CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agId, 'Grupo 3', 'Group 3', 6, 'litAgrupadoresPorJuntaGrupo3', 'AgrupadoresPorJuntaGrupo3', 150, '', '')
INSERT INTO CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agId, 'Grupo 4', 'Group 4', 7, 'litAgrupadoresPorJuntaGrupo4', 'AgrupadoresPorJuntaGrupo4', 150, '', '')
INSERT INTO CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agId, 'Grupo 5', 'Group 5', 8, 'litAgrupadoresPorJuntaGrupo5', 'AgrupadoresPorJuntaGrupo5', 150, '', '')

--Borramos los modulos que no se van a utilizar

DELETE FROM ModuloSeguimientoJunta WHERE ModuloSeguimientoJuntaID = @agRepId OR ModuloSeguimientoJuntaID = @agSopId

--Nueva tabla para agrupadores
CREATE TABLE AgrupadoresPorJunta(
	AgrupadorJuntaID INT NOT NULL PRIMARY KEY IDENTITY,
	JuntaSpoolID INT NOT NULL,
	ClasificacionPND VARCHAR(MAX) NULL,
	ClasificacionReparacion VARCHAR(MAX) NULL,
	ClasificaiconSoporte VARCHAR(MAX) NULL,
	Grupo1 VARCHAR(MAX) NULL,
	Grupo2 VARCHAR(MAX) NULL,
	Grupo3 VARCHAR(MAX) NULL,
	Grupo4 VARCHAR(MAX) NULL,
	Grupo5 VARCHAR(MAX) NULL,
	FOREIGN KEY (JuntaSpoolID) REFERENCES JuntaSpool(JuntaSpoolID)
)