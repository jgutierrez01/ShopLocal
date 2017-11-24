IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_InspeccionVisual]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_InspeccionVisual
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 30/Noviembre/2010
-- DescriptiON:	información de la inspección visual.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_InspeccionVisual]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	select p.ProyectoID
	,p.Nombre as [NombreProyecto]
	,iv.NumeroReporte 
	,iv.FechaReporte
	,jiv.Aprobado
	,jiv.Hoja
	,jiv.FechaInspeccion
	,jiv.Observaciones
	,jws.EtiquetaJunta
	,ots.NumeroControl
	,s.Nombre as [NombreSpool]
	,s.Dibujo as [Isometrico]
	,s.Segmento1
	,s.Segmento2
	,s.Segmento3
	,s.Segmento4
	,s.Segmento5
	,s.Segmento6
	,s.Segmento7
	,s.Revision
	,js.Diametro
	,js.Espesor
	,fa1.Nombre as [FamiliaMaterial1]
	,fa2.Nombre as [FamiliaMaterial2]
	,prai.Codigo as [ProcesoRaiz]
	,prel.Codigo as [ProcesoRel]
	from InspeccionVisual iv
	inner join JuntaInspeccionVisual jiv	
	on jiv.InspeccionVisualID = iv.InspeccionVisualID
	inner join Proyecto p
	on p.ProyectoID = iv.ProyectoID
	inner join JuntaWorkstatus jws 
	on jws.JuntaWorkstatusID = jiv.JuntaWorkstatusID
	inner join OrdenTrabajoSpool ots	
	on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
	inner join Spool s
	on s.SpoolID = ots.SpoolID
	inner join JuntaSpool js
	on js.JuntaSpoolID = jws.JuntaSpoolID
	inner join FamiliaAcero fa1
	on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
	left join FamiliaAcero fa2
	on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
	inner join JuntaSoldadura jsol
	on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
	inner join ProcesoRaiz prai
	on prai.ProcesoRaizID = jsol.ProcesoRaizID
	inner join ProcesoRelleno prel
	on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
	where iv.ProyectoID = @ProyectoID
	and iv.NumeroReporte = @NumeroReporte
	
	
END

GO


