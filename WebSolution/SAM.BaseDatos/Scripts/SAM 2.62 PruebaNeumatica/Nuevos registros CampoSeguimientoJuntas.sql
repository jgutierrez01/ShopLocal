
declare @ModuloSeguimientoId int
select @ModuloSeguimientoId = ModuloSeguimientoJuntaId from ModuloSeguimientoJunta 
where NombreTemplateColumn = 'PruebaNeumatica'

insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Fecha Requisición',			
			'Requisition Date',
			1,
			'ltsPruebaNeumaticaFechaRequisicion',
			'PruebaNeumaticaFechaRequisicion',
			'd',
			'',
			125,
			'System.Datetime'
			)

	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Número Requisición',			
			'Requisition Number',
			2,
			'ltsPruebaNeumaticaNumeroRequisicion',
			'PruebaNeumaticaNumeroRequisicion',
			'',
			'',
			125,
			'System.String'
			)
	
	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Fecha Prueba',			
			'Test Date',
			3,
			'ltsPruebaNeumaticaFechaPrueba',
			'PruebaNeumaticaFechaPrueba',
			'd',
			'',
			125,
			'System.DateTime'
			)
	
	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Fecha Reporte',			
			'Report Date',
			4,
			'ltsPruebaNeumaticaFechaReporte',
			'PruebaNeumaticaFechaReporte',
			'd',
			'',
			125,
			'System.DateTime'
			)
	
	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Número Reporte',			
			'Report Number',
			5,
			'ltsPruebaNeumaticaNumeroReporte',
			'PruebaNeumaticaNumeroReporte',
			'',
			'',
			125,
			'System.String'
			)
	
	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Hoja',			
			'Sheet',
			6,
			'ltsPruebaNeumaticaHoja',
			'PruebaNeumaticaHoja',
			'',
			'',
			125,
			'System.String'
			)

	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Aprobado',			
			'Passed',
			7,
			'ltsPruebaNeumaticaResultado',
			'PruebaNeumaticaResultado',
			'',
			'',
			125,
			'System.Boolean'
			)

	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Defecto',			
			'Defect',
			8,
			'ltsPruebaNeumaticaDefecto',
			'PruebaNeumaticaDefecto',
			'',
			'',
			125,
			'System.String'
			)

			
	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Sector',			
			'Sector',
			9,
			'ltsPruebaNeumaticaSector',
			'PruebaNeumaticaSector',
			'',
			'',
			125,
			'System.String'
			)

			
	insert into CampoSeguimientoJunta(
			ModuloSeguimientoJuntaID,
			Nombre,
			NombreIngles,
			OrdenUI,
			NombreControlUI,
			NombreColumnaSp,
			DataFormat,
			CssColumnaUI,
			AnchoUI,
			TipoDeDato
			)
	Values(
			@ModuloSeguimientoId,
			'Cuadrante',			
			'Quadrant',
			10,
			'ltsPruebaNeumaticaCuadrante',
			'PruebaNeumaticaCuadrante',
			'',
			'',
			125,
			'System.String'
			)

