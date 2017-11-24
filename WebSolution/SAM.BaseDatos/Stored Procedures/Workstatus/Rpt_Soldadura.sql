IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Soldadura]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Soldadura
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 29/Noviembre/2010
-- DescriptiON:	información del soldadura.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Soldadura]
(
	@ProyectoID int,
	@FechaInicial datetime,
	@FechaFinal datetime
)
AS 
BEGIN
	SET NOCOUNT ON;
	select  p.Nombre as [NombreProyecto]
		,s.Dibujo as [Isometrico]
		,s.Revision as [RevisionSpool]
		,s.RevisionCliente as [RevisionCliente]
		,ot.NumeroOrden as [OrdenTrabajo]
		,ots.NumeroControl as [NumeroControl]
		,jws.EtiquetaJunta
		,jws.JuntaWorkstatusID
		,jsol.JuntaSoldaduraID
		,tj.Codigo [TipoJunta]
		,t.Nombre [NombreTaller]
		,js.Diametro
		,js.Cedula
		,fa1.Nombre [FamiliaMaterial1]
		,fa2.Nombre [FamiliaMaterial2]
		,jsol.FechaSoldadura 
		,jsol.Observaciones
		,pr.Codigo as [ProcesoRaiz]
		,prel.Codigo as [ProcesoRelleno]
		,nS.NombreSoldadoresRaiz
		,nS1.NombreSoldadoresRelleno
		,nS2.CodigosConsumibles as [ConsumiblesRaiz]
		,nS3.CodigosConsumibles as [ConsumiblesRelleno]
		from JuntaWorkstatus jws
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
		inner join JuntaArmado ja on ja.JuntaWorkstatusID = jws.JuntaWorkstatusID
		inner join JuntaSpool js on js.JuntaSpoolID = jws.JuntaSpoolID
		inner join TipoJunta tj on js.TipoJuntaID = tj.TipoJuntaID
		inner join JuntaSoldadura jsol on jsol.JuntaSoldaduraID = jws.JuntaSoldaduraID
		inner join Spool s on s.SpoolID = ots.SpoolID
		inner join OrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
		inner join Proyecto p on p.ProyectoID = s.ProyectoID
		inner join Taller t on t.TallerID = ja.TallerID
		inner join FamiliaAcero fa1 on fa1.FamiliaAceroID = js.FamiliaAceroMaterial1ID
		left join FamiliaAcero fa2 on fa2.FamiliaAceroID = js.FamiliaAceroMaterial2ID
		inner join ProcesoRaiz pr on pr.ProcesoRaizID = jsol.ProcesoRaizID
		inner join ProcesoRelleno prel on prel.ProcesoRellenoID = jsol.ProcesoRellenoID
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(c1.Codigo + ' ','')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Consumible c1 
				ON c1.ConsumibleID = jsd.ConsumibleID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				AND jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS CodigosConsumibles
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Consumible c
				ON c.ConsumibleID = jsd1.ConsumibleID
				GROUP BY jsd1.JuntaSoldaduraID
			) nS2 on nS2.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT ', ' + COALESCE(c1.Codigo + ' ','')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Consumible c1 
				ON c1.ConsumibleID = jsd.ConsumibleID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				AND jsd.TecnicaSoldadorID = 2
				FOR XML PATH ('')),1,2,'') AS CodigosConsumibles
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Consumible c
				ON c.ConsumibleID = jsd1.ConsumibleID
				GROUP BY jsd1.JuntaSoldaduraID
			) nS3 on nS3.JuntaSoldaduraID = jsol.JuntaSoldaduraID		
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT DISTINCT ', ' + COALESCE(s1.Nombre + ' ','') 
								   + COALESCE(s1.ApPaterno + ' ','') 
								   + COALESCE(s1.ApMaterno,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				ON s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				AND jsd.TecnicaSoldadorID = 1
				FOR XML PATH ('')),1,2,'') AS NombreSoldadoresRaiz
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID
			) nS on nS.JuntaSoldaduraID = jsol.JuntaSoldaduraID	
		LEFT JOIN(
				SELECT  jsd1.JuntaSoldaduraID, 
				STUFF((SELECT DISTINCT ', ' + COALESCE(s1.Nombre + ' ','') 
								   + COALESCE(s1.ApPaterno + ' ','') 
								   + COALESCE(s1.ApMaterno,'')
				FROM JuntaSoldaduraDetalle jsd
				INNER JOIN  Soldador s1 
				ON s1.SoldadorID = jsd.SoldadorID
				WHERE jsd1.JuntaSoldaduraID = jsd.JuntaSoldaduraID 
				AND jsd.TecnicaSoldadorID = 2
				FOR XML PATH ('')),1,2,'') AS NombreSoldadoresRelleno
				FROM JuntaSoldaduraDetalle jsd1
				INNER JOIN Soldador s
				ON s.SoldadorID = jsd1.SoldadorID
				GROUP BY jsd1.JuntaSoldaduraID
			) nS1 on nS1.JuntaSoldaduraID = jsol.JuntaSoldaduraID
		where p.ProyectoID = @ProyectoID
		and jsol.FechaSoldadura >= @FechaInicial 
		and jsol.FechaSoldadura <= @FechaFinal
		and jws.JuntaFinal = 1
		and jws.SoldaduraAprobada = 1

END

GO


