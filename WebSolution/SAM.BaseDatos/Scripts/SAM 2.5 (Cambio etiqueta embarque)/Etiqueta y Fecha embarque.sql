ALTER TABLE Spool
ADD FechaEtiqueta datetime
GO

ALTER TABLE Spool
ADD NumeroEtiqueta nvarchar(20)
GO

update Spool 
set Spool.FechaEtiqueta = (select fechaetiqueta from WorkstatusSpool ws 
							inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
							where ots.SpoolID = Spool.SpoolID),
	Spool.NumeroEtiqueta = (select NumeroEtiqueta from WorkstatusSpool ws 
							inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
							where ots.SpoolID = Spool.SpoolID)


select * from spool where FechaEtiqueta is not null

select * from WorkstatusSpool where FechaEtiqueta is not null