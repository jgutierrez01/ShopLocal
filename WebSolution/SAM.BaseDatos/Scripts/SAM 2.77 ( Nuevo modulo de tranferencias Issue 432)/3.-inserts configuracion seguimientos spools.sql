declare @OrdenUI int,
		@ModuloSeguimientoSpoolID int

		select @ModuloSeguimientoSpoolID=moduloSeguimientoSpoolID from ModuloSeguimientoSpool where nombre='Embarque'

set @OrdenUI=(select max(OrdenUI) from CampoSeguimientoSpool where ModuloSeguimientoSpoolID=@ModuloSeguimientoSpoolID) + 1


insert into CampoSeguimientoSpool (ModuloSeguimientoSpoolID,
									Nombre,
									NombreIngles,
									OrdenUI,
									NombreControlUI,
									NombreColumnaSp,
									DataFormat,
									CssColumnaUI,
									UsuarioModifica,
									FechaModificacion,
									AnchoUI)
			VALUES(@ModuloSeguimientoSpoolID,
				   'Fecha Transferencia',
				   'Transfer Date',
				   @OrdenUI,
				   'litFechaTransferencia',
				   'Transferencia',
				   'd',
				   '',
				   null,
				   null,
				   125
			)


insert into CampoSeguimientoSpool (ModuloSeguimientoSpoolID,
									Nombre,
									NombreIngles,
									OrdenUI,
									NombreControlUI,
									NombreColumnaSp,
									DataFormat,
									CssColumnaUI,
									UsuarioModifica,
									FechaModificacion,
									AnchoUI)
			VALUES(@ModuloSeguimientoSpoolID,
				   'Fecha Preparación',
				   'Preparation Date',
				   @OrdenUI + 1,
				   'litFechaPreparacion',
				   'PreparacionTransferencia',
				   'd',
				   '',
				   null,
				   null,
				   125
			)
