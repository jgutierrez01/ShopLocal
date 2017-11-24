IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_PruebaSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_PruebaSpool
GO
-- =============================================
-- Author:		LMG
-- Create date: 6 / Enero / 2014
-- DescriptiON:	información del reporte prueba por spool
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_PruebaSpool]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50),
	@TipoPrueba int
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.ProyectoID
		,p.Nombre as [NombreProyecto]
		,rpnd.NumeroReporte
		,rpnd.FechaReporte
		,srp.Hoja
		,srp.Observaciones
		,srp.FechaPrueba
		,srp.Aprobado
		,ots.NumeroControl
		,ot.NumeroOrden
		,s.Nombre as [NombreSpool]
		,s.Dibujo as [Isometrico]
		,s.Revision
		from ReporteSpoolPnd rpnd
		inner join Proyecto p on p.ProyectoID = rpnd.ProyectoID
		inner join SpoolReportePnd srp on srp.ReporteSpoolPndID = rpnd.ReporteSpoolPndID
		inner join WorkstatusSpool ws on ws.WorkstatusSpoolID = srp.WorkstatusSpoolID
		inner join OrdenTrabajoSpool ots
		on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		inner join OrdenTrabajo ot
		on ot.OrdenTrabajoID = ots.OrdenTrabajoID
		inner join Spool s
		on s.SpoolID = ots.SpoolID
		where rpnd.ProyectoID = @ProyectoID
		and rpnd.NumeroReporte = @NumeroReporte
		and rpnd.TipoPruebaSpoolID = @TipoPrueba
	
	END
GO


