CREATE PROCEDURE [dbo].[Rpt_Transferencia]
(
	@ProyectoID int,
	@NumeroTransferencia nvarchar(50)
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	select p.Nombre as [ProyectoNombre]
		,t.NumeroTransferencia
		,t.FechaTransferencia
		,ots.NumeroControl
		,s.Peso
		,s.Area
		,rtrim(s.Segmento6) [Segmento6]
		,s.Revision
		,s.RevisionCliente
		,s.Nombre
		,s.Dibujo
	from transferencia t
	inner join TransferenciaSpool ts on t.TransferenciaSpoolID=ts.TransferenciaSpoolID
	inner join OrdenTrabajoSpool ots on ots.SpoolID = ts.SpoolID
	inner join Spool s on s.SpoolID = ots.SpoolID
	inner join Proyecto p on p.ProyectoID = s.ProyectoID
	where t.NumeroTransferencia = @NumeroTransferencia
		  and s.ProyectoID = @ProyectoID
	
END








