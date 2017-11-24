insert into ModuloSeguimientoJunta (Nombre,NombreIngles,OrdenUI,NombreTemplateColumn) values ('Prueba PMI', 'PMI Test', 16, 'PruebaPMI')
update ModuloSeguimientoJunta set OrdenUI = 17 where Nombre = 'Pintura'
update ModuloSeguimientoJunta set OrdenUI = 18 where Nombre = 'Embarque'

insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Fecha Requisición','Requisition Date', 1, 'ltsPruebaPMIFechaRequisicion', 'PruebaPMIFechaRequisicion', 'd', '', 125, 'System.DateTime')
  
  insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Número Requisición','Requisition Number', 2, 'ltsPruebaPMINumeroRequisicion', 'PruebaPMINumeroRequisicion', '', '', 125, 'System.String')
  
  insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Código Requisición','Requisition Code', 3, 'ltsPruebaPMICodigoRequisicion', 'PruebaPMICodigoRequisicion', '', '', 125, 'System.String')
  
  insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Fecha Prueba','Test Date', 4, 'ltsPruebaPMIFechaPrueba', 'PruebaPMIFechaPrueba', 'd', '', 125, 'System.DateTime')
  
  insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Fecha Reporte','Report Date', 5, 'ltsPruebaPMIFechaReporte', 'PruebaPMIFechaReporte', 'd', '', 125, 'System.DateTime')
  
  insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Número Reporte','Report Number', 6, 'ltsPruebaPMINumeroReporte', 'PruebaPMINumeroReporte', '', '', 125, 'System.String')
  
  insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Hoja','Sheet', 7, 'ltsPruebaPMIHoja', 'PruebaPMIHoja', '', '', 125, 'System.Int32')
  
  insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Aprobado','Passed', 8, 'ltsPruebaPMIResultado', 'PruebaPMIResultado', '', '', 125, 'System.Boolean')
  
  insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID, Nombre, NombreIngles, OrdenUI, NombreControlUI, NombreColumnaSp, DataFormat, CssColumnaUI, AnchoUI, TipoDeDato)
  values(19, 'Defecto','Defect', 9, 'ltsPruebaPMIDefecto', 'PruebaPMIDefecto', '', '', 125, 'System.String')