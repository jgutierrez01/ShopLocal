begin transaction

update NumeroUnicoSegmento set 
InventarioBuenEstado = InventarioBuenEstado + InventarioTransferenciaCorte,
InventarioDisponibleCruce = InventarioBuenEstado + InventarioTransferenciaCorte - InventarioCongelado

update NumeroUnicoInventario set 
InventarioBuenEstado = InventarioBuenEstado + InventarioTransferenciaCorte,
InventarioDisponibleCruce = InventarioBuenEstado + InventarioTransferenciaCorte - InventarioCongelado

rollback



