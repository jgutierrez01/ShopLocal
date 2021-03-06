CREATE NONCLUSTERED INDEX [IX_NumeroUnico_ProyectoID_NumeroUnicoID_ItemCodeID_ColadaID_Codigo] ON [dbo].[NumeroUnico] 
(
	[ProyectoID] ASC,
	[NumeroUnicoID] ASC,
	[ItemCodeID] ASC,
	[ColadaID] ASC,
	[Codigo] ASC
)
INCLUDE ( [ProveedorID],
[FabricanteID],
[TipoCorte1ID],
[TipoCorte2ID],
[Estatus],
[Factura],
[PartidaFactura],
[OrdenDeCompra],
[PartidaOrdenDeCompra],
[Diametro1],
[Diametro2],
[Cedula],
[MarcadoAsme],
[MarcadoGolpe],
[MarcadoPintura],
[PruebasHidrostaticas]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_NumeroUnicoMovimiento_TipoMovimientoID_Estatus_NumeroUnicoID_NumeroUnicoMovimientoID] ON [dbo].[NumeroUnicoMovimiento] 
(
	[TipoMovimientoID] ASC,
	[Estatus] ASC,
	[NumeroUnicoID] ASC,
	[NumeroUnicoMovimientoID] ASC
)
INCLUDE ( [Cantidad]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_NumeroUnicoMovimiento_NumeroUnicoID_NumeroUnicoMovimientoID_TipoMovimientoID_Estatus] ON [dbo].[NumeroUnicoMovimiento] 
(
	[NumeroUnicoID] ASC,
	[NumeroUnicoMovimientoID] ASC,
	[TipoMovimientoID] ASC,
	[Estatus] ASC
)
INCLUDE ( [Cantidad]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_ItemCode_Codigo_ItemCodeID] ON [dbo].[ItemCode] 
(
	[Codigo] ASC,
	[ItemCodeID] ASC
)
INCLUDE ( [TipoMaterialID],
[DescripcionEspanol]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_NumeroUnicoInventario_NumeroUnicoID] ON [dbo].[NumeroUnicoInventario] 
(
	[NumeroUnicoID] ASC
)
INCLUDE ( [CantidadRecibida],
[CantidadDanada],
[InventarioFisico],
[InventarioBuenEstado],
[InventarioCongelado],
[InventarioTransferenciaCorte],
[InventarioDisponibleCruce]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_Colada_ColadaID_NumeroColada] ON [dbo].[Colada] 
(
	[ColadaID] ASC,
	[NumeroColada] ASC
)
INCLUDE ( [AceroID],
[NumeroCertificado]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_Despacho_Cancelado_EsEquivalente_NumeroUnicoID] ON [dbo].[Despacho] 
(
	[Cancelado] ASC,
	[EsEquivalente] ASC,
	[NumeroUnicoID] ASC
)
INCLUDE ( [Cantidad]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_Despacho_NumeroUnicoOD_Cancelado] ON [dbo].[Despacho] 
(
	[NumeroUnicoID] ASC,
	[Cancelado] ASC
)
INCLUDE ( [Cantidad]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_RecepcionNumeroUnico_NumeroUnicoID_RecepcionID] ON [dbo].[RecepcionNumeroUnico] 
(
	[NumeroUnicoID] ASC,
	[RecepcionID] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_Recepcion_RecepcionID] ON [dbo].[Recepcion] 
(
	[RecepcionID] ASC
)
INCLUDE ( [TransportistaID]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

