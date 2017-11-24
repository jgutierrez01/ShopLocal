DECLARE @agrPNDID INT
SET @agrPNDID = (SELECT ModuloSeguimientoSpoolID FROM ModuloSeguimientoSpool WHERE Nombre = 'Agrupadores Spool PND')

--INSERT AGRUPADORES PND
INSERT INTO CampoSeguimientoSpool(ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agrPNDID, 'Tipos de Juntas', 'Joint types', 1, 'litAgrupadoresSpoolPNDTiposJuntas', 'AgrupadoresSpoolPNDTiposJuntas', 300 , '', '')
INSERT INTO CampoSeguimientoSpool (ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agrPNDID, 'Clasificación PND', 'PND Classification', 2, 'litAgrupadoresSpoolPNDClasificacion', 'AgrupadoresSpoolPNDClasificacion', 300, '', '')
INSERT INTO CampoSeguimientoSpool (ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agrPNDID, 'Avance PND', 'PND advance', 3, 'litAgrupadoresSpoolPNDAvance', 'AgrupadoresSpoolPNDAvance', 300, '', '')
INSERT INTO CampoSeguimientoSpool (ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agrPNDID, 'Reparaciones', 'Repairs', 4, 'litAgrupadoresSpoolPNDReparaciones', 'AgrupadoresSpoolPNDReparaciones', 125, '', '')
INSERT INTO CampoSeguimientoSpool (ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agrPNDID, 'Reparaciones Pend. Soldadura', 'Outstanding welding repairs', 5, 'litAgrupadoresSpoolPNDPendientes', 'AgrupadoresSpoolPNDPendientes', 125, '', '')
INSERT INTO CampoSeguimientoSpool (ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agrPNDID, 'PND Reparaciones', 'PND repairs', 6, 'litAgrupadoresSpoolPNDReportes', 'AgrupadoresSpoolPNDReportes', 125, '', '')
	INSERT INTO CampoSeguimientoSpool (ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp,AnchoUI, DataFormat, CssColumnaUI)
	VALUES(@agrPNDID, 'PND Juntas Seguimiento', 'PND tracing Joints', 7, 'litAgrupadoresSpoolPNDJuntasSeguimiento', 'AgrupadoresSpoolPNDJuntasSeguimiento', 125, '', '')