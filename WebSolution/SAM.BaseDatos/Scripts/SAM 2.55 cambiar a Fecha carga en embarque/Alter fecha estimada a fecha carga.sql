--actualizar nombre de columna
update CampoSeguimientoJunta
set Nombre ='Fecha Carga',
	NombreIngles ='Upload Date',
	OrdenUI =3
	where Nombre like 'Fecha Estimada'
	and ModuloSeguimientoJuntaID = 16

--actualizar nombre de columna
update CampoSeguimientoSpool
set Nombre ='Fecha Carga',
	NombreIngles ='Upload Date',
	OrdenUI =3
	where Nombre like 'Fecha Estimada'
	and ModuloSeguimientoSpoolID = 5

--cambiar campo fecha embarque de not null a null
alter table Embarque 
alter column FechaEmbarque datetime null

--eliminar registros relacionados con la columna fecha etiqueta
delete DetallePersonalizacionSeguimientoJunta
where CampoSeguimientoJuntaID in (select CampoSeguimientoJuntaID from CampoSeguimientoJunta
where Nombre like 'Fecha Etiqueta'
and NombreIngles like 'Label Date')

-- eliminar la columna fecha etiqueta
delete from CampoSeguimientoJunta
where Nombre like 'Fecha Etiqueta'
and NombreIngles like 'Label Date'


--eliminar registros relacionados con la columna fecha etiqueta
delete DetallePersonalizacionSeguimientoSpool
where CampoSeguimientoSpoolID in (select CampoSeguimientoSpoolID from CampoSeguimientoSpool
where Nombre like 'Fecha Etiqueta'
and NombreIngles like 'Label Date')

-- eliminar la columna fecha etiqueta
delete from CampoSeguimientoSpool
where Nombre like 'Fecha Etiqueta'
and NombreIngles like 'Label Date'
