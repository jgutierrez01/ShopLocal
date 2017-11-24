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
, 'Fecha Liberación Calidad'
, 'Quality release date'
, 55
, 'litGeneralFechaLiberacionCalidad'
, 'GeneralFechaLiberacionCalidad'
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
, 'Usuario Liberación Calidad'
, 'User release date'
, 55
, 'litGeneralUsuarioLiberacionCalidad'
, 'GeneralUsuarioLiberacionCalidad'
, ''
, ''
, 125 )
