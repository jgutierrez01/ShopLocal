declare @moduloGeneralID INT = (select ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'General');

INSERT INTO CampoSeguimientoJunta
( ModuloSeguimientoJuntaID,
 Nombre
 , NombreIngles
 , OrdenUI
 , NombreControlUI
 , NombreColumnaSp
 , DataFormat
 , CssColumnaUI
 , AnchoUI
 , TipoDeDato )
VALUES 
( @moduloGeneralID
, 'Es Revisión'
, 'Is review'
, 60
, 'litGeneralEsRevision'
, 'GeneralEsRevision'
, ''
, ''
, 125
, 'System.Boolean' )

INSERT INTO CampoSeguimientoJunta
( ModuloSeguimientoJuntaID,
 Nombre
 , NombreIngles
 , OrdenUI
 , NombreControlUI
 , NombreColumnaSp
 , DataFormat
 , CssColumnaUI
 , AnchoUI
 , TipoDeDato )
VALUES 
( @moduloGeneralID
, 'Orden trabajo Especial'
, 'Special Work Order'
, 5
, 'litGeneralOrdenTrabajoEspecial'
, 'GeneralOrdenTrabajoEspecial'
, ''
, ''
, 125
, 'System.String' )

INSERT INTO CampoSeguimientoJunta
( ModuloSeguimientoJuntaID,
 Nombre
 , NombreIngles
 , OrdenUI
 , NombreControlUI
 , NombreColumnaSp
 , DataFormat
 , CssColumnaUI
 , AnchoUI
 , TipoDeDato )
VALUES 
( @moduloGeneralID
, 'Número de control especial'
, 'Special control number'
, 5
, 'litGeneralNumeroControlEspecial'
, 'GeneralNumeroControlEspecial'
, ''
, ''
, 125
, 'System.String' )