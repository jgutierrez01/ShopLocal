IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Pintura]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_Pintura]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[Rpt_Pintura]
	Funcion:	Reporte de pintura
	Parametros:	@ProyectoID int,
				@WorkstatusSpoolID int
	Autor:		OF
	Modificado:	31/08/2011 PEGV
*****************************************************************************************/
CREATE PROCEDURE [dbo].[Rpt_Pintura]
(
	@ProyectoID int,
	@WorkstatusSpoolID int
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.Nombre as [NombreProyecto]
		,p.ProyectoID
		,s.Dibujo as [Isometrico]
		,s.Especificacion
		,s.Nombre as [NombreSpool]
		,s.Area
		,ots.NumeroControl
		,s.SistemaPintura
		,s.ColorPintura
		,s.CodigoPintura
		,ps.FechaAdherencia
		,ps.ReporteAdherencia
		,ps.FechaIntermedios
		,ps.ReporteIntermedios
		,ps.FechaAcabadoVisual
		,ps.ReporteAcabadoVisual
		,ps.FechaSandblast
		,ps.ReporteSandblast
		,ps.FechaPrimarios
		,ps.ReportePrimarios
		,ps.FechaPullOff
		,ps.ReportePullOff
		from WorkstatusSpool wks
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = wks.OrdenTrabajoSpoolID
		inner join Spool s on s.SpoolID = ots.SpoolID
		inner join Proyecto p on p.ProyectoID = s.ProyectoID
		left join PinturaSpool ps on wks.WorkstatusSpoolID = ps.WorkstatusSpoolID		
		where p.ProyectoID = @ProyectoID
		and wks.WorkstatusSpoolID = @WorkstatusSpoolID		
	
	END
	
