begin transaction

select s.spoolid, s.DiametroMayor, MAX(diametro1) from Spool s
inner join MaterialSpool ms on ms.SpoolID = s.SpoolID 
group by s.spoolid, s.DiametroMayor


UPDATE Spool
SET DiametroMayor =(SELECT MAX(Diametro1) FROM MaterialSpool where Spool.SpoolID = MaterialSpool.SpoolID  ) 
FROM Spool

select s.spoolid, s.DiametroMayor, MAX(diametro1) from Spool s
inner join MaterialSpool ms on ms.SpoolID = s.SpoolID 
group by s.spoolid, s.DiametroMayor
order by s.SpoolID

rollback