CREATE NONCLUSTERED INDEX [IX_CorteDetalle_CorteID_MaterialSpoolID_Cancelado] ON [dbo].[CorteDetalle] 
(
	[CorteID] ASC,
	[MaterialSpoolID] ASC,
	[Cancelado] ASC
)
INCLUDE ( [Cantidad]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IX_Corte_Cancelado_NumeroUnicoCorteID_CorteID] ON [dbo].[Corte] 
(
	[Cancelado] ASC,
	[NumeroUnicoCorteID] ASC,
	[CorteID] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go


