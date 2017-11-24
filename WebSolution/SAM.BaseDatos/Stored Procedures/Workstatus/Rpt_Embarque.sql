IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Embarque]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Embarque
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 06 / 12 / 2010
-- DescriptiON:	información del embarque
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Embarque]
(
	@ProyectoID int,
	@NumeroEmbarque nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.Nombre as [ProyectoNombre]
		,e.NumeroEmbarque
		,e.FechaEmbarque
		,ots.NumeroControl
		,s.Peso
		,s.Area
		,s.Revision
		,s.Nombre
		,s.Dibujo
		,e.Nota1
		,e.Nota2
		,e.Nota3
		,e.Nota4
		,e.Nota5
		from Embarque e
		inner join Proyecto p on p.ProyectoID = e.ProyectoID
		inner join EmbarqueSpool es on es.EmbarqueID = e.EmbarqueID
		inner join WorkstatusSpool ws on ws.WorkstatusSpoolID = es.WorkstatusSpoolID
		inner join OrdenTrabajoSpool ots on ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
		inner join Spool s on s.SpoolID = ots.SpoolID
		where e.NumeroEmbarque = @NumeroEmbarque
		and e.ProyectoID = @ProyectoID
	
END

GO


