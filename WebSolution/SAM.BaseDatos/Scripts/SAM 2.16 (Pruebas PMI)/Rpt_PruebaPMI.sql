IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_PruebaPMI]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_PruebaPMI
GO
-- =============================================
-- Author:		Misael Espinoza
-- Create date: 8 / Agosto / 2012
-- DescriptiON:	información del reporte prueba PMI
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_PruebaPMI]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.ProyectoID
		,p.Nombre as [NombreProyecto]
		,rpnd.NumeroReporte
		,rpnd.FechaReporte
		,jrp.Hoja
		,jrp.Observaciones
		,jrp.FechaPrueba
		,jrp.Aprobado
		,jws.EtiquetaJunta
		,js.Diametro
		,js.Cedula
		,js.Espesor
		,fa1.Nombre as [FamiliaMaterial1]
		,fa2.Nombre as [FamiliaMaterial2]
		,ots.NumeroControl
		,ot.NumeroOrden
		,s.Nombre as [NombreSpool]
		,s.Dibujo as [Isometrico]
		,s.Revision
		,prai.Codigo as [ProcesoRaiz]
		,prel.Codigo as [ProcesoRelleno]
		from ReportePnd rpnd
		inner join Proyecto p on p.ProyectoID = rpnd.ProyectoID
		inner join JuntaReportePnd jrp on jrp.ReportePndID = rpnd.ReportePndID
		inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jrp.JuntaWorkstatusID
		inner join JuntaSpool js
		on js.JuntaSpoolID = jws.JuntaSpoolID
		inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
		left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
		inner join JuntaSoldadura jsol
		on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
		inner join ProcesoRaiz prai on prai.ProcesoRaizID = jsol.ProcesoRaizID
		inner join ProcesoRelleno prel
		on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
		inner join OrdenTrabajoSpool ots
		on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
		inner join OrdenTrabajo ot
		on ot.OrdenTrabajoID = ots.OrdenTrabajoID
		inner join Spool s
		on s.SpoolID = ots.SpoolID
		where rpnd.ProyectoID = @ProyectoID
		and rpnd.NumeroReporte = @NumeroReporte
		and rpnd.TipoPruebaID = 10
	
	END
GO


