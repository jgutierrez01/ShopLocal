-- Eliminamos llave
alter table JuntaSoldaduraDetalle
drop constraint FK_JuntaSoldaduraDetalle_Consumible
go

-- Hacemos columna nula
alter table JuntaSoldaduraDetalle
alter column ConsumibleID int null
go

-- agregamos llave
alter table JuntaSoldaduraDetalle
add constraint FK_JuntaSoldaduraDetalle_Consumible
foreign key (ConsumibleID) references Consumible (ConsumibleID)
go