declare @ModuloGeneralID int
select @ModuloGeneralID = ModuloSeguimientoSpoolID
from ModuloSeguimientoSpool
where Nombre = 'General'

insert into CampoSeguimientoSpool
(
	ModuloSeguimientoSpoolID,
	Nombre,
	NombreIngles,
	OrdenUI,
	NombreColumnaSp,
	DataFormat,
	AnchoUI,
	NombreControlUI,
	CssColumnaUI
)
select	@ModuloGeneralID, 'Isométrico', 'Isometric', 20, 'Isometrico', '', 125, '', ''
union
select	@ModuloGeneralID, 'Kg Teóricos', 'Theoretical Kg', 25, 'GeneralKgsTeoricos', '0.0', 125, '', ''
union
select	@ModuloGeneralID, 'Revisión Cliente', 'Client Revision', 30, 'RevisionCte', '', 125, '', ''
union
select	@ModuloGeneralID, 'Revisión', 'Revision', 35, 'RevisionStgo', '', 125, '', ''
union
select	@ModuloGeneralID, '% Armado', 'Fitted %', 40, 'PorcentajeArmado', '0.0', 125, '', ''
union
select	@ModuloGeneralID, '% Soldado', 'Welded %', 45, 'PorcentajeSoldado', '0.0', 125, '', ''

