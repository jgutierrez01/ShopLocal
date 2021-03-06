INSERT [dbo].[CategoriaPendiente] ([CategoriaPendienteID], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (1, N'Calidad', N'Quality', NULL, NULL)
INSERT [dbo].[CategoriaPendiente] ([CategoriaPendienteID], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (2, N'Ingeniería', N'Engineering', NULL, NULL)
INSERT [dbo].[CategoriaPendiente] ([CategoriaPendienteID], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (3, N'Materiales', N'Material', NULL, NULL)
INSERT [dbo].[CategoriaPendiente] ([CategoriaPendienteID], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (4, N'Producción', N'Production', NULL, NULL)
INSERT [dbo].[CategoriaPendiente] ([CategoriaPendienteID], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (5, N'Otro', N'Other', NULL, NULL)

INSERT [dbo].[TipoPendiente] ([TipoPendienteID], [EsAutomatico], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (1, 1, N'Regreso a inventario por despacho cancelado', N'Return to inventory by canceled dispatch', NULL, NULL)
INSERT [dbo].[TipoPendiente] ([TipoPendienteID], [EsAutomatico], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (2, 1, N'Soldadura sin Armado (HH)', N'Welding without Fitting (HH)', NULL, NULL)
INSERT [dbo].[TipoPendiente] ([TipoPendienteID], [EsAutomatico], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (3, 1, N'Corte de Ajuste', N'Adjustment Cut', NULL, NULL)
INSERT [dbo].[TipoPendiente] ([TipoPendienteID], [EsAutomatico], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (4, 0, N'Pendiente Manual', N'Manual pending', NULL, NULL)
INSERT [dbo].[TipoPendiente] ([TipoPendienteID], [EsAutomatico], [Nombre], [NombreIngles], [UsuarioModifica], [FechaModificacion]) VALUES (5, 1, N'Corte por Rechazo de Prueba', N'Cut by test rejection', NULL, NULL)

insert into Permiso(ModuloID, Nombre,NombreIngles) values(8,'Listado de pendientes','Pending items listing')
insert into Permiso(ModuloID, Nombre,NombreIngles) values(8,'Detalle de pendientes','Pending items detail')
insert into Permiso(ModuloID, Nombre,NombreIngles) values(7,'Importar configuraciones de proyecto','Import project configurations')
insert into Permiso(ModuloID, Nombre,NombreIngles) values(4,'Agregar spool con asignación','Add spool with assigned material allocation')
insert into Permiso(ModuloID, Nombre,NombreIngles) values(6,'Asignación de pendientes por proyecto','Pending items assignment per project')

insert into Pagina(PermisoID, Url) select (select top 1 PermisoID from Permiso where ModuloID=8 and Nombre='Listado de pendientes'), '/Administracion/LstPendientes.aspx'
insert into Pagina(PermisoID, Url) select (select top 1 PermisoID from Permiso where ModuloID=8 and Nombre='Detalle de pendientes'), '/Administracion/PopUpEdicionPendiente.aspx'
insert into Pagina(PermisoID, Url) select (select top 1 PermisoID from Permiso where ModuloID=8 and Nombre='Detalle de pendientes'), '/Administracion/PopUpNuevoPendiente.aspx'
insert into Pagina(PermisoID, Url) select (select top 1 PermisoID from Permiso where ModuloID=7 and Nombre='Importar configuraciones de proyecto'), '/Catalogos/ImportConfigProyecto.aspx'
insert into Pagina(PermisoID, Url) select (select top 1 PermisoID from Permiso where ModuloID=3 and Nombre='Importar ingenierías'), '/Ingenieria/PopUpHomologacion.aspx'
insert into Pagina(PermisoID, Url) select (select top 1 PermisoID from Permiso where ModuloID=4 and Nombre='Agregar spool con asignación'), '/Produccion/AgregarSpoolOdtAsignado.aspx'
insert into Pagina(PermisoID, Url) select (select top 1 PermisoID from Permiso where ModuloID=6 and Nombre='Asignación de pendientes por proyecto'), '/Proyectos/PendientesAutomaticos.aspx'


INSERT INTO ProyectoPendiente
(
	ProyectoID,
	TipoPendienteID,
	Responsable,
	UsuarioModifica,
	FechaModificacion
)
SELECT	p.ProyectoID,
		t.TipoPendienteID,
		p.UsuarioModifica,
		null,
		null
FROM	Proyecto p
CROSS JOIN TipoPendiente t


