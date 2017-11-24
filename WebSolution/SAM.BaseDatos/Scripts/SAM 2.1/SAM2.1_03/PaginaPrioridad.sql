

declare @PermisoID int
select @PermisoID = PermisoID from Permiso where Nombre = 'Fijar Prioridades'

Insert into Pagina (permisoid, url)
values
(@PermisoID,'/Produccion/PopUpFijarPrioridadSeleccionados.aspx')

select * from Pagina
