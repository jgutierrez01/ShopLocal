BEGIN TRAN

UPDATE Spool SET SistemaPintura = WorkstatusSpool.SistemaPintura, ColorPintura = WorkstatusSpool.ColorPintura, 
CodigoPintura = WorkstatusSpool.CodigoPintura
FROM Spool 
INNER JOIN OrdenTrabajoSpool ON Spool.SpoolID = OrdenTrabajoSpool.SpoolID
INNER JOIN WorkstatusSpool ON WorkstatusSpool.OrdenTrabajoSpoolID = OrdenTrabajoSpool.OrdenTrabajoSpoolID

ALTER TABLE WorkstatusSpool DROP COLUMN SistemaPintura
ALTER TABLE WorkstatusSpool DROP COLUMN ColorPintura
ALTER TABLE WorkstatusSpool DROP COLUMN CodigoPintura

COMMIT TRAN