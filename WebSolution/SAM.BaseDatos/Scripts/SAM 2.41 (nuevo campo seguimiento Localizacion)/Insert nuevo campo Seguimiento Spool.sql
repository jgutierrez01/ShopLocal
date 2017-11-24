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
, 'Ultima Localizacion'
, 'Last Location'
, 10
, 'litUltimaLocalizacion'
, 'GeneralUltimaLocalizacion'
, ''
, ''
, 150 )
