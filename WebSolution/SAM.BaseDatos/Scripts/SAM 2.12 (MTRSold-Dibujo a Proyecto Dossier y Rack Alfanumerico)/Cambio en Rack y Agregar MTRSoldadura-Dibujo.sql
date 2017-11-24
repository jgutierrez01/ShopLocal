alter table NumeroUnicoSegmento alter column Rack varchar(50) null
alter table NumeroUnico alter column Rack varchar(50) null

alter table ProyectoDossier add MTRSoldadura bit not null default 0
alter table ProyectoDossier add Drawing bit not null default 0
