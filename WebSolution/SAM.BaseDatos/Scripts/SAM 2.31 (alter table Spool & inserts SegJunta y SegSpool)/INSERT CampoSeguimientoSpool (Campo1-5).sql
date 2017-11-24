declare @moduloGeneralID INT = (select ModuloSeguimientoSpoolID from ModuloSeguimientoSpool where Nombre = 'General');

insert into CampoSeguimientoSpool
( ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI )
VALUES 
( @moduloGeneralID, 'Campo 1', 'Field 1', 50, 'litCampo1', 'Campo1', '', '', 150 )

insert into CampoSeguimientoSpool
( ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI )
VALUES 
( @moduloGeneralID, 'Campo 2', 'Field 2', 51, 'litCampo2', 'Campo2', '', '', 150 )

insert into CampoSeguimientoSpool
( ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI)
VALUES 
( @moduloGeneralID, 'Campo 3', 'Field 3', 52, 'litCampo3', 'Campo3', '', '', 150 )

insert into CampoSeguimientoSpool
( ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI )
VALUES 
( @moduloGeneralID, 'Campo 4', 'Field 4', 53, 'litCampo4', 'Campo4', '', '', 150 )

insert into CampoSeguimientoSpool
( ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI )
VALUES 
( @moduloGeneralID, 'Campo 5', 'Field 5', 54, 'litCampo5', 'Campo5', '', '', 150 )