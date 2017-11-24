ALTER TABLE ProyectoConfiguracion 
ADD ActualizaLocalizacion BIT NULL

update ProyectoConfiguracion
set ActualizaLocalizacion = 0
where ActualizaLocalizacion is Null


alter table ProyectoConfiguracion 
alter column ActualizaLocalizacion BIT NOT NULL