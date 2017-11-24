DECLARE @OrdenUI INT
SET @OrdenUI = (SELECT MAX(OrdenUI) FROM ModuloSeguimientoJunta)

INSERT INTO ModuloSeguimientoJunta (Nombre, NombreIngles, OrdenUI, NombreTemplateColumn) 
VALUES('Agrupadores PND','PND Groupers',(@OrdenUI + 1),'AgrupadoresPnd')

INSERT INTO ModuloSeguimientoJunta (Nombre, NombreIngles, OrdenUI, NombreTemplateColumn) 
VALUES('Agrupadores Soportes','Mounting Groupers',(@OrdenUI + 2),'AgrupadoresSoportes')

INSERT INTO ModuloSeguimientoJunta (Nombre, NombreIngles, OrdenUI, NombreTemplateColumn) 
VALUES('Agrupadores Reparaciones','Repairs Groupers',(@OrdenUI + 3),'AgrupadoresReparaciones')


