DECLARE @OrdenUI INT
SET @OrdenUI = (SELECT MAX(OrdenUI) FROM ModuloSeguimientoSpool)

INSERT INTO ModuloSeguimientoSpool (Nombre, NombreIngles, OrdenUI, NombreTemplateColumn) 
VALUES('Agrupadores Spool PND','Spool PND Groupers',(@OrdenUI + 1),'AgrupadoresSpoolPnd')

