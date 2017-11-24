declare @moduloID INT = (select ModuloSeguimientoSpoolID from ModuloSeguimientoSpool where Nombre = 'Pintura');

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
( @moduloID
, 'Liberado Pintura'
, 'Paint Release'
, 18
, 'litPinturaLiberado'
, 'PinturaLiberado'
, ''
, ''
, 125 )
