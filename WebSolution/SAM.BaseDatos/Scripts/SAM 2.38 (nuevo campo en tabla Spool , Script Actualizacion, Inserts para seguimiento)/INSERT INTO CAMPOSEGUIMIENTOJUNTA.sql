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
, 'Diametro Mayor'
, 'Major Diameter'
, 7
, 'litDiametroMayor'
, 'GeneralDiametroMayor'
, '0.###'
, ''
, 150
, 'System.Decimal' )