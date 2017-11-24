declare @consecutivo int
set @consecutivo=(select top 1 TipoReporteProyectoID + 1 from TipoReporteProyecto order by TipoReporteProyectoID desc)


insert into TipoReporteProyecto(TipoReporteProyectoID,
								Nombre,
								NombreIngles,
								OrdenUI,
								UsuarioModifica,
								FechaModificacion
			) values
			(		
					@consecutivo
					,'Transferencia Spool'
					, 'Spool Transfer'
					,@consecutivo
					,null
					,getdate()  
			)