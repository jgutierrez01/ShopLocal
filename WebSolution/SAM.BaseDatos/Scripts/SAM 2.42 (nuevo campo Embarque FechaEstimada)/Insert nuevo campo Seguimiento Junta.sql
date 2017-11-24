declare @moduloGeneralID INT = (select ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'Embarque');

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
, 'Fecha Estimada'
, 'Estimated Date'
, 4
, 'litEmbarqueFechaEstimada'
, 'EmbarqueFechaEstimada'
, 'd'
, ''
, 125
, 'System.DateTime' )