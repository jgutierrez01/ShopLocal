-- Eliminamos llaves
alter table JuntaArmado
drop constraint FK_JuntaArmado_NumeroUnico_Numero1
go
alter table JuntaArmado
drop constraint FK_JuntaArmado_NumeroUnico_Numero2
go

-- Hacemos columnas nulas
alter table JuntaArmado
alter column NumeroUnico1ID int null
go
alter table JuntaArmado
alter column NumeroUnico2ID int null
go

-- agregamos llaves
alter table JuntaArmado
add constraint FK_JuntaArmado_NumeroUnico_Numero1 
foreign key (NumeroUnico1ID) references NumeroUnico (NumeroUnicoID)
go

alter table JuntaArmado
add constraint FK_JuntaArmado_NumeroUnico_Numero2
foreign key (NumeroUnico2ID) references NumeroUnico (NumeroUnicoID)
go