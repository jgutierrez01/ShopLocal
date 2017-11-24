

update JuntaSpool 
set Peqs = 
(select p.Equivalencia from Peq p
where p.FamiliaAceroID = JuntaSpool.FamiliaAceroMaterial1ID
and p.TipoJuntaID = JuntaSpool.TipoJuntaID
and p.CedulaID = (select c.CedulaID from Cedula c where c.Codigo = JuntaSpool.Cedula)
and p.DiametroID = (select d.DiametroID from Diametro d where d.Valor = JuntaSpool.Diametro)
)

update JuntaSpool 
set Espesor = 
(select p.Valor from Espesor p
where p.CedulaID = (select c.CedulaID from Cedula c where c.Codigo = JuntaSpool.Cedula)
and p.DiametroID = (select d.DiametroID from Diametro d where d.Valor = JuntaSpool.Diametro)
)

update JuntaSpool 
set KgTeoricos = 
(select p.Valor from KgTeorico p
where p.CedulaID = (select c.CedulaID from Cedula c where c.Codigo = JuntaSpool.Cedula)
and p.DiametroID = (select d.DiametroID from Diametro d where d.Valor = JuntaSpool.Diametro)
)

select * from JuntaSpool


select * from KgTeorico