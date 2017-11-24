INSERT INTO CampoSeguimientoSpool(ModuloSeguimientoSpoolID,Nombre,NombreIngles,OrdenUI,NombreControlUI,NombreColumnaSp,DataFormat
	,CssColumnaUI,UsuarioModifica,FechaModificacion,AnchoUI)
SELECT 7,'Observaciones hold','Hold observations',17,'litObservacionesHold','ObservacionesHold','','',NULL,NULL,125
UNION
SELECT 7,'Fecha hold','Hold date',18,'litFechaHold','FechaHold','','',NULL,NULL,125