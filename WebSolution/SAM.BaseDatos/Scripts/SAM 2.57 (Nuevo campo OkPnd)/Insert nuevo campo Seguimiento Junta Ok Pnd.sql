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
, 'Fecha Ok PND'
, 'OK PND Date'
, 63
, 'litGeneralFechaOkPnd'
, 'GeneralFechaOkPnd'
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
, 'Usuario Ok PND'
, 'User Ok PND'
, 63
, 'litGeneralUsuarioOkPnd'
, 'GeneralUsuarioOkPnd'
, ''
, ''
, 125
, 'System.String' )