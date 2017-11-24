--Juntas de Campo
insert into Permiso (ModuloID,Nombre,NombreIngles) values (4, 'Juntas de Campo', 'Field Welds')
insert into Pagina (PermisoID, Url) values ((select PermisoID from Permiso where Nombre = 'Juntas de Campo'), '/Produccion/JuntasCampo.aspx')
--Congelado Parcial
insert into Permiso (ModuloID,Nombre,NombreIngles) values (4, 'Congelado Parcial', 'Partial Allocation')
insert into Pagina (PermisoID, Url) values ((select PermisoID from Permiso where Nombre = 'Congelado Parcial'), '/Produccion/CongeladoParcial.aspx')
--Administracion Congelados
insert into Permiso (ModuloID,Nombre,NombreIngles) values (4, 'Administracion de Congelados', 'Allocation Administration')
insert into Pagina (PermisoID, Url) values ((select PermisoID from Permiso where Nombre = 'Administracion de Congelados'), '/Produccion/CongeladosNumeroUnico.aspx')
insert into Pagina (PermisoID, Url) values ((select PermisoID from Permiso where Nombre = 'Administracion de Congelados'), '/Produccion/CongeladosOrdenTrabajo.aspx')