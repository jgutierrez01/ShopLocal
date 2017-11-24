declare @moduloGeneralID INT = (select ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'Armado');

insert into CampoSeguimientoJunta
( ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato )
VALUES 
( @moduloGeneralID, 'Número Único 1 pendiente', 'Unique Number 1 pending', 5, 'litNumeroUnico1Pendiente', 'NumeroUnico1Pendiente', '', '', 125, 'System.Boolean' )

insert into CampoSeguimientoJunta
( ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato )
VALUES 
( @moduloGeneralID, 'Número Único 2 pendiente', 'Unique Number 2 pending', 6, 'litNumeroUnico2Pendiente', 'NumeroUnico2Pendiente', '', '', 125, 'System.Boolean' )