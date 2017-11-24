UPDATE TipoReporteProyecto SET TipoReporteProyectoID = TipoReporteProyectoID + 1
WHERE TipoReporteProyectoID > 10 
GO 
UPDATE TipoReporteProyecto SET OrdenUI = OrdenUI + 1
WHERE OrdenUI > 10 
GO
INSERT INTO TipoReporteProyecto(TipoReporteProyectoID,Nombre,NombreIngles,OrdenUI,UsuarioModifica,FechaModificacion)
SELECT 11,'Requisicion Pintura','Paint Requisition',11,NULL,NULL