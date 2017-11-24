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
, 'Fecha Liberacion Material'
, 'Mterial release date'
, 60
, 'litGeneralFechaLiberacionMaterial'
, 'GeneralFechaLiberacionMaterial'
, 'd'
, ''
, 125
, 'System.DateTime' )

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
, 'Usuario Liberacion Material'
, 'User Material release'
, 60
, 'litGeneralUsuarioLiberacionMaterial'
, 'GeneralUsuarioLiberacionMaterial'
, ''
, ''
, 125
, 'System.String' )