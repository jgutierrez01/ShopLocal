
declare @ModuloSeguimientoJuntaID int, @OrdenUI INT
select @ModuloSeguimientoJuntaID=ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre like 'Embarque'



select TOP 1 @OrdenUI=(OrdenUI + 1)  from CampoSeguimientoJunta where ModuloSeguimientoJuntaID=@ModuloSeguimientoJuntaID order by OrdenUI desc

insert into CampoSeguimientoJunta(							
							ModuloSeguimientoJuntaID,
							Nombre,
							NombreIngles,
							OrdenUI,
							NombreControlUI,
							NombreColumnaSp,
							DataFormat,
							CssColumnaUI,
							UsuarioModifica,
							FechaModificacion,							
							AnchoUI,
							TipoDeDato)
					values(@ModuloSeguimientoJuntaID,
						'Fecha Transferencia',
						'Transfer Date',
						@OrdenUI,
						'litFechaTransferencia',
						'Transferencia',
						'd',
						'',
						null,
						null,
						125,
						'System.DateTime'
					)

set @OrdenUI =@OrdenUI + 1
insert into CampoSeguimientoJunta(							
							ModuloSeguimientoJuntaID,
							Nombre,
							NombreIngles,
							OrdenUI,
							NombreControlUI,
							NombreColumnaSp,
							DataFormat,
							CssColumnaUI,
							UsuarioModifica,
							FechaModificacion,							
							AnchoUI,
							TipoDeDato)
					values(@ModuloSeguimientoJuntaID,
						'Fecha Preparación',
						'Preparation Date',
						@OrdenUI,
						'litFechaPreparacion',
						'PreparacionTransferencia',
						'd',
						'',
						null,
						null,
						125,
						'System.DateTime'
					)
set @OrdenUI=@OrdenUI +1
insert into CampoSeguimientoJunta(							
							ModuloSeguimientoJuntaID,
							Nombre,
							NombreIngles,
							OrdenUI,
							NombreControlUI,
							NombreColumnaSp,
							DataFormat,
							CssColumnaUI,
							UsuarioModifica,
							FechaModificacion,							
							AnchoUI,
							TipoDeDato)
					values(@ModuloSeguimientoJuntaID,
						'Numero Transferencia',
						'Transfer Number',
						@OrdenUI,
						'litNumeroTransferencia',
						'NumeroTransferencia',
						'',
						'',
						null,
						null,
						125,
						'System.String'
					)
set @OrdenUI=@OrdenUI +1