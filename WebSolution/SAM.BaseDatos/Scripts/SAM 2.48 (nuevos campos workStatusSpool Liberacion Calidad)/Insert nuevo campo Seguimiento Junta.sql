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
, 'Fecha Liberacion Calidad'
, 'Quality release date'
, 59
, 'litGeneralFechaLiberacionCalidad'
, 'GeneralFechaLiberacionCalidad'
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
, 'Usuario Liberacion Calidad'
, 'User Quality release'
, 59
, 'litGeneralUsuarioLiberacionCalidad'
, 'GeneralUsuarioLiberacionCalidad'
, ''
, ''
, 125
, 'System.String' )