ALTER TABLE ItemCode
ADD Peso DECIMAL(7,2),
 DescripcionInterna NVARCHAR(200) NULL
 
 
declare @permisoid int
select @permisoid = permisoid from Permiso where Nombre ='Detalle de item codes'
insert into Pagina (PermisoID, Url)
values(@permisoid , '/Proyectos/PesoItemCode.aspx')