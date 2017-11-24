declare @moduloGeneralID INT = (select ModuloSeguimientoSpoolID from ModuloSeguimientoSpool where Nombre = 'General');

insert into CampoSeguimientoSpool
( ModuloSeguimientoSpoolID
, Nombre
, NombreIngles
, OrdenUI
, NombreControlUI
, NombreColumnaSp
, DataFormat
, CssColumnaUI
, AnchoUI )
VALUES 
( @moduloGeneralID
, 'Fecha Liberaci�n Material'
, 'Material release date'
, 56
, 'litGeneralFechaLiberacionMaterial'
, 'GeneralFechaLiberacionMaterial'
, 'd'
, ''
, 125 )

insert into CampoSeguimientoSpool
( ModuloSeguimientoSpoolID
, Nombre
, NombreIngles
, OrdenUI
, NombreControlUI
, NombreColumnaSp
, DataFormat
, CssColumnaUI
, AnchoUI )
VALUES 
( @moduloGeneralID
, 'Usuario Liberaci�n Material'
, 'User material release'
, 56
, 'litGeneralUsuarioLiberacionMaterial'
, 'GeneralUsuarioLiberacionMaterial'
, ''
, ''
, 125 )
