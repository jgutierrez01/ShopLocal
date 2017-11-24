declare @moduloGeneralID int = (select ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre = 'General')

insert into CampoSeguimientoJunta
( ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato )
VALUES 
( @moduloGeneralID, 'FabClas', 'FabClas', 55, 'litFabClas', 'FabClas', '', '', 150, 'System.String' )

insert into CampoSeguimientoJunta
( ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato )
VALUES 
( @moduloGeneralID, 'Campo junta 2', 'Joint field 2', 56, 'litCampoJunta2', 'CampoJunta2', '', '', 150, 'System.String' )

insert into CampoSeguimientoJunta
( ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato )
VALUES 
( @moduloGeneralID, 'Campo junta 3', 'Joint field 3', 57, 'litCampoJunta3', 'CampoJunta3', '', '', 150, 'System.String' )

insert into CampoSeguimientoJunta
( ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato )
VALUES 
( @moduloGeneralID, 'Campo junta 4', 'Joint field 4', 58, 'litCampoJunta4', 'CampoJunta4', '', '', 150, 'System.String' )

insert into CampoSeguimientoJunta
( ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato )
VALUES 
( @moduloGeneralID, 'Campo junta 5', 'Joint field 5', 59, 'litCampoJunta5', 'CampoJunta5', '', '', 150, 'System.String' )