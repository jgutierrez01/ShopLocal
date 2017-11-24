IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_TT]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_TT
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 03 / 12 / 2010
-- DescriptiON:	información del tratamiento térmico - PWHT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_TT]
(
	@ProyectoID int,
	@NumeroTratamiento nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.ProyectoID
	,p.Nombre as [NombreProyecto]
	,rt.NumeroReporte
	,rt.FechaReporte
	,jrt.Hoja
	,jrt.Aprobado
	,jrt.FechaTratamiento
	,jrt.Observaciones
	,jws.EtiquetaJunta
	,js.Cedula
	,js.Diametro
	,js.Espesor
	,s.Nombre as [NombreSpool]
	,s.Dibujo
	,s.Revision
	,s.Especificacion
	,fa1.Nombre as [FamiliaMaterial1]
	,fa2.Nombre as [FamiliaMaterial2]
	,ots.NumeroControl
	from ReporteTt rt
	inner join Proyecto p on p.ProyectoID = rt.ProyectoID
	inner join JuntaReporteTt jrt on jrt.ReporteTtID = rt.ReporteTtID
	inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jrt.JuntaWorkstatusID
	inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
	inner join Spool s on s.SpoolID = js.SpoolID
	inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
	left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
	inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
	where rt.ProyectoID = @ProyectoID
	and rt.NumeroReporte = @NumeroTratamiento
	and rt.TipoPruebaID = 3

END


GO


