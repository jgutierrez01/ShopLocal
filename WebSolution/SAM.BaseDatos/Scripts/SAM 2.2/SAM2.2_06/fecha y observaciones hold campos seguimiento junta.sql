INSERT INTO CampoSeguimientoJunta(ModuloSeguimientoJuntaID,Nombre,NombreIngles,OrdenUI,NombreControlUI,NombreColumnaSp,DataFormat
	,CssColumnaUI,UsuarioModifica,FechaModificacion,AnchoUI,TipoDeDato)
SELECT 17,'Observaciones hold','Hold observations',12,'litObservacionesHold','ObservacionesHold','','',NULL,NULL,125,'System.String'
UNION
SELECT 17,'Fecha hold','Hold date',12,'litFechaHold','FechaHold','','',NULL,NULL,125,'System.DateTime'