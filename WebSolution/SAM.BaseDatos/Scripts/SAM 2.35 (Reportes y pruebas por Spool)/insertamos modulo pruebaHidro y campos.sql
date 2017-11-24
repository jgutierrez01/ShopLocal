
-- INSERTAMOS Prueba hidrostática en módulo
insert into ModuloSeguimientoSpool
(Nombre, NombreIngles, OrdenUI, NombreTemplateColumn)
values
('Prueba Hidrostática', 'Hydrostatic Test', 7, 'PruebaHidrostatica')
GO


-- INSERTAMOS los campos para el módulo de Prueba hidrostática
declare @pruebaHidroID int = (select ModuloSeguimientoSpoolID from ModuloSeguimientoSpool where Nombre = 'Prueba Hidrostática')

insert into CampoSeguimientoSpool
(ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI)
values
(@pruebaHidroID, 'Fecha Requisición', 'Requisition Date', 1, 'ltsPruebaHidroFechaRequisicion', 'PruebaHidroFechaRequisicion', 'd', '', 125)

insert into CampoSeguimientoSpool
(ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI)
values
(@pruebaHidroID, 'Número Requisición', 'Requisition Number', 2, 'ltsPruebaHidroNumeroRequisicion', 'PruebaHidroNumeroRequisicion', '', '', 125)

insert into CampoSeguimientoSpool
(ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI)
values
(@pruebaHidroID, 'Fecha Prueba', 'Test Date', 3, 'ltsPruebaHidroFechaPrueba', 'PruebaHidroFechaPrueba', 'd', '', 125)

insert into CampoSeguimientoSpool
(ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI)
values
(@pruebaHidroID, 'Fecha Reporte', 'Report Date', 4, 'ltsPruebaHidroFechaReporte', 'PruebaHidroFechaReporte', 'd', '', 125)

insert into CampoSeguimientoSpool
(ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI)
values
(@pruebaHidroID, 'Número Reporte', 'Report Number', 5, 'ltsPruebaHidroNumeroReporte', 'PruebaHidroNumeroReporte', 'd', '', 125)

insert into CampoSeguimientoSpool
(ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI)
values
(@pruebaHidroID, 'Hoja', 'Sheet', 6, 'ltsPruebaHidroHoja', 'PruebaHidroHoja', '', '', 125)

insert into CampoSeguimientoSpool
(ModuloSeguimientoSpoolID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI)
values
(@pruebaHidroID, 'Aprobado', 'Passed', 7, 'ltsPruebaHidroAprobado', 'PruebaHidroAprobado', '', '', 125)