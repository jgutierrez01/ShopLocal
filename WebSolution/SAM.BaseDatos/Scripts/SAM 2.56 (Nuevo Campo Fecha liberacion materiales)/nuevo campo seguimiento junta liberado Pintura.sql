declare @moduloPinturaID INT = (select ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'Pintura');

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
( @moduloPinturaID
, 'Liberado Pintura'
, 'Paint Release'
, 18
, 'litPinturaLiberado'
, 'PinturaLiberado'
, ''
, ''
, 125
, 'System.Boolean' )
