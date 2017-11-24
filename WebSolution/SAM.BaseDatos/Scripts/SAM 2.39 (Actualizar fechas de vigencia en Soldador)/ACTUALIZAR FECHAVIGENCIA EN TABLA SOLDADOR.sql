--CURSOR PARA ACTUALIZAR FECHAS DE VIGENCIA PARA LOS SOLDADORES
--
--FECHA CREACIÓN 31/01/2014
--
--!!!!ADVERTENCIA!!!
--EL PROCESO DE ACTUALIZACION CON EL CURSOR CREA BLOQUEOS SOBRE LOS REGISTROS 
--EN LOS QUE ESTA TRABAJANDO, POR LO QUE LAS PETICIONES QUE SE HAGAN DURANTE EL PROCESO
--DE ACTUALIZACIÓN SERAN RECHAZADAS. 
--LA ACTUALIZACION PUEDE TOMAR BASTANTE TIEMPO DEPENDIENDO
--DE LA CANTIDAD DE REGISTROS A ACTUALIZAR. (ESTE PROCESO TARDO 30 SEG. EN ACTUALIZAR 30,000 REGISTROS).

set nocount on
declare @fechaVigencia datetime 
declare @soldador int
declare @temp datetime
declare Soldadores cursor global
for
	select SoldadorID, FechaVigencia from Soldador for update of FechaVigencia 
open Soldadores
	fetch next from Soldadores into @soldador, @fechaVigencia
		while(@@FETCH_STATUS = 0)
		begin
			SELECT @temp = (select max(FechaVigencia) from Wpq where SoldadorID = @soldador) 
			UPDATE Soldador
			SET FechaVigencia = @temp where current of Soldadores 
			fetch next from Soldadores into @soldador, @fechaVigencia
		end
close Soldadores
deallocate Soldadores
go