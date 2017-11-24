declare @ModuloGeneralID int
select @ModuloGeneralID = ModuloSeguimientoJuntaID
from ModuloSeguimientoJunta
where Nombre = 'General'

insert into CampoSeguimientoJunta
(
	ModuloSeguimientoJuntaID,
	Nombre,
	NombreIngles,
	OrdenUI,
	NombreColumnaSp,
	DataFormat,
	AnchoUI,
	TipoDeDato,
	NombreControlUI,
	CssColumnaUI
)
select	@ModuloGeneralID, 'Fab Area', 'Fab Area', 30, 'GeneralFabArea', '', 125, 'System.String', '', ''
union
select	@ModuloGeneralID, 'Kg Teóricos', 'Theoretical Kg', 31, 'GeneralKgTeoricos', '', 125, 'System.Decimal', '', ''
union
select	@ModuloGeneralID, '% Armado', 'Fitted %', 32, 'PorcentajeArmado', '0.0', 125, 'System.Decimal', '', ''
union
select	@ModuloGeneralID, '% Soldadura', 'Welded %', 33, 'PorcentajeSoldado', '0.0', 125, 'System.Decimal', '', ''
union
select	@ModuloGeneralID, 'Prioridad', 'Priority', 34, 'GeneralPrioridad', '', 125, 'System.Int32', '', ''
union
select	@ModuloGeneralID, 'Isométrico', 'Isometric', 35, 'GeneralIsometrico', '', 125, 'System.String', '', ''
union
select	@ModuloGeneralID, 'Revisión', 'Revision', 36, 'GeneralRevisionSteelgo', '', 125, 'System.String', '', ''
union
select	@ModuloGeneralID, 'Revisión Cliente', 'Client Revision', 37, 'GeneralRevisionCliente', '', 125, 'System.String', '', ''