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
				   'Numero Transferencia',
				   'Transfer Number',
				   @OrdenUI,
				   'litNumeroTransferencia',
				   'NumeroTransferencia',
				   '',
				   '',
				   null,
				   null,
				   125
			)
