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
, 'Fecha Ok PND'
, 'Ok PND date'
, 63
, 'litGeneralFechaOkPnd'
, 'GeneralFechaOkPnd'
, 'd'
, ''
, 150 )

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
, 'Usuario Ok Pnd'
, 'Ok Pnd User'
, 63
, 'litGeneralUsuarioOkPnd'
, 'GeneralUsuarioOkPnd'
, ''
, ''
, 150 )
