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
, 'Es Revisión'
, 'Is Review'
, 56
, 'litGeneralEsRevision'
, 'GeneralEsRevision'
, ''
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
, 'Orden Trabajo Especial'
, 'Special Work Order'
, 5
, 'litGeneralOrdenTrabajoEspecial'
, 'GeneralOrdenTrabajoEspecial'
, ''
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
, 'Número control especial'
, 'Special control number'
, 5
, 'litGeneralNumeroControlEspecial'
, 'GeneralNumeroControlEspecial'
, ''
, ''
, 125 )

