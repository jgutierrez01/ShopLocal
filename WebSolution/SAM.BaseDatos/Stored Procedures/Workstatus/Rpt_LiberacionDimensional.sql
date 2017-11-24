IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_LiberacionDimensional]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_LiberacionDimensional
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 1 / Diciembre / 2010
-- DescriptiON:	información del reporte de liberación Dimensional.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_LiberacionDimensional]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	select rd.ProyectoID
		,rd.FechaReporte
		,p.Nombre as [ProyectoNombre]
		,rd.NumeroReporte
		,rdd.Aprobado
		,rdd.FechaLiberacion
		,rdd.Hoja
		,rdd.Observaciones
		,ots.NumeroControl
		,s.Dibujo
		,s.Especificacion
		,s.Nombre
		from ReporteDimensional rd
		inner join Proyecto p on p.ProyectoID = rd.ProyectoID
		inner join ReporteDimensionalDetalle rdd
		on rdd.ReporteDimensionalID = rd.ReporteDimensionalID
		inner join WorkstatusSpool ws
		on ws.WorkstatusSpoolID = rdd.WorkstatusSpoolID
		inner join OrdenTrabajoSpool ots
		on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		inner join Spool s
		on s.SpoolID = ots.SpoolID
		where rd.ProyectoID = @ProyectoID
		and rd.NumeroReporte = @NumeroReporte
		and rd.TipoReporteDimensionalID = 1
		
END

GO


