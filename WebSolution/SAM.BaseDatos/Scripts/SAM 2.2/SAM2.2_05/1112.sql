
declare @modulo int
select @modulo = ModuloSeguimientoJuntaID from ModuloSeguimientoJunta where Nombre ='Soldadura'
update CampoSeguimientoJunta set Nombre ='Wps Raíz', NombreIngles='Root Wps' where Nombre='Wps' and ModuloSeguimientoJuntaID = @modulo
update CampoSeguimientoJunta set OrdenUI = OrdenUI + 1 where OrdenUI >=5 and ModuloSeguimientoJuntaID = @modulo
insert into CampoSeguimientoJunta (ModuloSeguimientoJuntaID,Nombre,NombreIngles,OrdenUI,NombreControlUI,NombreColumnaSp,AnchoUI,DataFormat,CssColumnaUI,TipoDeDato) 
values(@modulo,'Wps Relleno', 'Fill Wps', 5,'litSoldadorWpsRelleno','SoldaduraWpsRelleno',125,'','','System.String')