IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_PruebaPT]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_PruebaPT
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 2 / Diciembre / 2010
-- DescriptiON:	información del reporte prueba PT
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_PruebaPT]
(
	@ProyectoID int,
	@NumeroReporte nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select distinct p.ProyectoID
	,p.Nombre as [NombreProyecto]
	,rp.NumeroReporte
	,rp.FechaReporte
	,jrp.Aprobado
	,jrp.Hoja
	,jrp.FechaPrueba
	,jrp.Observaciones
	,jws.EtiquetaJunta
	,js.Espesor
	,js.Cedula
	,js.Diametro
	,fa1.Nombre as [FamiliaMaterial1]
	,fa2.Nombre as [FamiliaMaterial2]
	,s.Nombre as [SpoolNombre]
	,s.Dibujo
	,s.Revision
	,ots.NumeroControl
	,ot.NumeroOrden
	--,prai.Codigo as [ProcesoRaiz]
	--,prel.Codigo as [ProcesoRelleno]
	--,nS1.CodigoSoldadorRelleno
	--,nS.CodigoSoldadorRaiz
	from ReportePnd rp
	inner join Proyecto p on p.ProyectoID = rp.ProyectoID
	inner join JuntaReportePnd jrp on jrp.ReportePndID = rp.ReportePndID
	inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jrp.JuntaWorkstatusID
	inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
	inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
	left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
	inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
	inner join OrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
	inner join Spool s on s.SpoolID = ots.SpoolID
	inner join JuntaSoldadura jsol on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
	inner join JuntaSoldaduraDetalle jsd on jsd.JuntaSoldaduraID = jsol.JuntaSoldaduraID
	inner join ProcesoRaiz prai on prai.ProcesoRaizID = jsol.ProcesoRaizID
	inner join ProcesoRelleno prel on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
	LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(s1.Codigo,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				on s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				and jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS CodigoSoldadorRaiz
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID		
			) nS on nS.JuntaSoldaduraID = jsol.JuntaSoldaduraID	
	LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(s1.Codigo,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				on s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				and jsd.TecnicaSoldadorID = 2
				FOR XML PATH ('')),1,2,'') AS CodigoSoldadorRelleno
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID		
			) nS1 on nS1.JuntaSoldaduraID = jsol.JuntaSoldaduraID
	where rp.ProyectoID = @ProyectoID
	and rp.NumeroReporte = @NumeroReporte
	and rp.TipoPruebaID = 2

	
	END
GO


