/****** Script for SelectTopNRows command from SSMS  ******/
INSERT INTO ProyectoCamposRecepcion 
(ProyectoID, CantidadCamposRecepcion, CantidadCamposNumeroUnico, UsuarioModifica, FechaModificacion)
SELECT p.ProyectoID,
       0,
       0,
       (SELECT UserId FROM Usuario WHERE Nombre = 'Sam' AND ApPaterno = 'Admin'),
       GETDATE()
FROM Proyecto p
WHERE p.ProyectoID NOT IN (SELECT pcr.ProyectoID FROM ProyectoCamposRecepcion pcr)