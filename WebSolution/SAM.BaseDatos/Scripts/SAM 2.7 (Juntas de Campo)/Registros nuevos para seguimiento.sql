declare @ModuloSeguimientoJuntaID int
select	@ModuloSeguimientoJuntaID = ModuloSeguimientoJuntaID
from	ModuloSeguimientoJunta
where	Nombre = 'Armado'

insert into CampoSeguimientoJunta
(
	ModuloSeguimientoJuntaID,	Nombre,		NombreIngles,	OrdenUI,			NombreControlUI,
	NombreColumnaSp,			DataFormat,	CssColumnaUI,	UsuarioModifica,	FechaModificacion,
	AnchoUI,					TipoDeDato
)
select	@ModuloSeguimientoJuntaID,	'Spool 1',	'Spool 1',	10,		'',
		'ArmadoSpool1',	'',	'',		null,		null,		150,	'System.String'
union
select	@ModuloSeguimientoJuntaID,	'Spool 2',	'Spool 2',	15,		'',
		'ArmadoSpool2',	'',	'',		null,		null,		150,	'System.String'
union
select	@ModuloSeguimientoJuntaID,	'Etiqueta Material 1',	'Material Label 1',	20,		'',
		'ArmadoEtiquetaMaterial1',	'',	'',		null,		null,		150,	'System.String'
union
select	@ModuloSeguimientoJuntaID,	'Etiqueta Material 2',	'Material Label 2',	25,		'',
		'ArmadoEtiquetaMaterial2',	'',	'',		null,		null,		150,	'System.String'
