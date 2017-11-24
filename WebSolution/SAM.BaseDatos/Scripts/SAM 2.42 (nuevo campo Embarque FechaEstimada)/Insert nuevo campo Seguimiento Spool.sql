declare @moduloGeneralID INT = (select ModuloSeguimientoSpoolID from ModuloSeguimientoSpool where Nombre = 'Embarque');

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
, 'Fecha Estimada'
, 'Estimated Date'
, 4
, 'litEmbarqueFechaEstimada'
, 'EmbarqueFechaEstimada'
, 'd'
, ''
, 125 )
