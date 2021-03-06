IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_RequisicionSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_RequisicionSpool]
GO


-- =============================================
-- Author:		LMG
-- Create date: 6/Enero/2013
-- DescriptiON:	Requisicion de Spool

-- =============================================
CREATE PROCEDURE [dbo].[Rpt_RequisicionSpool]
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
		,r.NumeroRequisicion
		,ots.NumeroControl								
		,s.Nombre										
		,s.Dibujo as [Isometrico]
		,s.Revision 
		,s.RevisionCliente
		,s.Cedula
		,s.PorcentajePnd								
		,s.Segmento1
		,s.Segmento2
		,s.Segmento3
		,s.Segmento4
		,s.Segmento5
		,s.Segmento6
		,s.Segmento7
		,r.Observaciones
		,r.FechaRequisicion
		from RequisicionSpool r
		inner join SpoolRequisicion sr on sr.RequisicionSpoolID = r.RequisicionSpoolID
		inner join WorkstatusSpool ws on ws.WorkstatusSpoolID = sr.WorkstatusSpoolID
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		inner join Spool s on s.SpoolID = ots.SpoolID
		inner join Proyecto p on p.ProyectoID = s.ProyectoID
		WHERE r.ProyectoID = @ProyectoID
		and r.NumeroRequisicion = @NumeroRequisicion
		and r.TipoPruebaSpoolID = @TipoPrueba
		
END







