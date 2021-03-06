IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Requisicion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Requisicion
GO


-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 29/Noviembre/2010
-- DescriptiON:	información del armado.

-- Revisado por Jorge Vargas 28/Julio/2011
-- Comentarios: Se agregaron campos a la consulta
-- y se cambio el nombre de los soldadores
-- por el número que tienen asignados.
-- Los cambios estan marcados entre comentarios.
-- Script original tiene terminacion _bk.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Requisicion]
(
	@ProyectoID int,
	@NumeroRequisicion nvarchar(50),
	@TipoPrueba int
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select  distinct p.ProyectoID as [ProyectoID]
		,p.Nombre as [nombreProyecto]
		,r.CodigoAsme
		,r.NumeroRequisicion
		,ots.NumeroControl								---< Campos agreados al
		,s.Nombre										--- reporte por JVargas >
		,s.Dibujo as [Isometrico]
		,s.Revision 
		,s.RevisionCliente
		,s.Cedula
		,s.PorcentajePnd								----< agregado jv
		,s.Segmento1
		,s.Segmento2
		,s.Segmento3
		,s.Segmento4
		,s.Segmento5
		,s.Segmento6
		,s.Segmento7
		,Taller = (select (select nombre from Taller t where t.TallerID = ot.TallerID) from OrdenTrabajo ot where ot.OrdenTrabajoID = ots.OrdenTrabajoID) --< agregado jv
		,jws.EtiquetaJunta
		,js.Diametro
		,js.Espesor
		,Fabarea = ( select codigo from fabarea f where f.FabAreaID = js.fabareaid)  -----< agregado jv
		,Wps = (select w.Nombre from Wps w where w.WpsID = jsol.WpsID)      ------< agregado jv										------< agregado jv
		,r.Observaciones
		,r.FechaRequisicion
		,fa1.Nombre as [FamiliaMaterial1]
		,fa2.Nombre as [FamiliaMaterial2]
		,prai.Codigo as [ProcesoRaiz]
		,prel.Codigo as [ProcesoRel]
		,nS.CodigoSoldadoresRaiz                        --< Se cambiaron nombres de
		,nS1.CodigoSoldadoresRelleno                    -- de los campos>
		from Requisicion r
		inner join JuntaRequisicion jr on jr.RequisicionID = r.RequisicionID
		inner join JuntaWorkstatus jws on jws.JuntaWorkstatusID = jr.JuntaWorkstatusID
		inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID 
		inner join Spool s on s.SpoolID = js.SpoolID 
		----< agregado para reporte por Jorge Vargas
		inner join OrdenTrabajoSpool ots on s.SpoolID = ots.SpoolID 
		---->
		inner join Proyecto p on p.ProyectoID = s.ProyectoID
		inner join FamiliaAcero fa1 on js.FamiliaAceroMaterial1ID = fa1.FamiliaAceroID
		left join FamiliaAcero fa2 on js.FamiliaAceroMaterial2ID = fa2.FamiliaAceroID
		inner join JuntaSoldadura jsol on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
		inner join JuntaSoldaduraDetalle jsd on jsd.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		inner join Soldador sol on sol.SoldadorID = jsd.SoldadorID
		inner join ProcesoRaiz prai on prai.ProcesoRaizID = jsol.ProcesoRaizID
		inner join ProcesoRelleno prel on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + s1.Codigo                --< Se cambio nombre por codigo
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				on s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				and jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS CodigoSoldadoresRaiz
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID		
			) nS on nS.JuntaSoldaduraID = jsol.JuntaSoldaduraID	
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + s1.Codigo				  --< Se cambio nombre por codigo
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				on s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				and jsd.TecnicaSoldadorID = 2
				FOR XML PATH ('')),1,2,'') AS CodigoSoldadoresRelleno
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID		
			) nS1 on nS1.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		WHERE r.ProyectoID = @ProyectoID
		and r.NumeroRequisicion = @NumeroRequisicion
		and r.TipoPruebaID = @TipoPrueba
		
END







