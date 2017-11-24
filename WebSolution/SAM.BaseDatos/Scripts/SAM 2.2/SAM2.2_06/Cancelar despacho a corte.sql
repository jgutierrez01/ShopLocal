

UPDATE NumeroUnicoMovimiento SET Estatus = 'C'
WHERE TipoMovimientoID = 15 AND NumeroUnicoMovimientoID NOT IN (SELECT SalidaMovimientoID FROM NumeroUnicoCorte)

DELETE FROM NumeroUnicoMovimiento WHERE TipoMovimientoID = 16