IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Durezas]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Durezas
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 03 / 12 / 2010
-- DescriptiON:	información del durezas de spool
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Durezas]
(
	@ProyectoID int,
	@NumeroReporte varchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select  rt.ProyectoID
		,rt.FechaReporte
		,p.Nombre as [NombreProyecto]
		,p.Descripcion as [DescripcionProyecto]
		,rt.NumeroReporte
		,jrt.Aprobado
		,jrt.Hoja
		,jrt.Observaciones
		,jrt.FechaTratamiento
		,jws.EtiquetaJunta
		,js.Diametro
		,js.Cedula
		,js.Espesor
		,fa1.Nombre as [FamiliaMaterial1]
		,fa2.Nombre as [FamiliaMaterial2]
		,s.Dibujo as [Isometrico]		
		from  ReporteTt rt
		inner join Proyecto p on p.ProyectoID = rt.ProyectoID		
		inner join JuntaReporteTt jrt on jrt.ReporteTtID = rt.ReporteTtID
		inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jrt.JuntaWorkstatusID
		inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
		inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
		left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
		inner join Spool s on s.SpoolID = ots.SpoolID
		where rt.ProyectoID = @ProyectoID
		and rt.NumeroReporte = @NumeroReporte
		and rt.TipoPruebaID = 4
	
END

GO


