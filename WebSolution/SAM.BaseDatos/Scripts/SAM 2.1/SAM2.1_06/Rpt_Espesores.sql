IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Espesores]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Espesores
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 1 / Diciembre / 2010
-- DescriptiON:	información de la liberacion dimensional de espesores.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Espesores]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	select p.ProyectoID
		,p.Nombre as [NombreProyecto]
		,rd.NumeroReporte
		,rd.FechaReporte
		,rdd.Observaciones
		,rdd.Aprobado
		,rdd.Hoja
		,ots.NumeroControl
		,s.Dibujo
		,s.Nombre as [NombreSpool]
		,s.Especificacion
		from ReporteDimensional rd		
		inner join ReporteDimensionalDetalle rdd on rd.ReporteDimensionalID = rdd.ReporteDimensionalID
		inner join WorkstatusSpool ws on ws.WorkstatusSpoolID = rdd.WorkstatusSpoolID
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		inner join Spool s	on s.SpoolID = ots.SpoolID
		inner join Proyecto p on p.ProyectoID = s.ProyectoID
		where rd.ProyectoID = @ProyectoID
		and rd.NumeroReporte = @NumeroReporte
		and rd.TipoReporteDimensionalID = 2
	
	END
GO


