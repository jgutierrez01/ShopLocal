IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_RequisicionPintura]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Rpt_RequisicionPintura]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[Rpt_RequisicionPintura]
	Funcion:	Reporte de requisicion pintura
	Parametros:	@RequisicionPinturaID int
	Autor:		PEGV
	Modificado:	06/09/2011 PEGV
*****************************************************************************************/
CREATE PROCEDURE [dbo].[Rpt_RequisicionPintura]
(
	@ProyectoID int,
	@RequisicionPinturaID int
)
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @FabAreaID INT 
	SET @FabAreaID = 1 -- FabAreaID = SHOP
	
	SELECT	rp.NumeroRequisicion,
			rp.FechaRequisicion, 
			p.Nombre AS NombreProyecto,
			ots.NumeroControl,
			s.SpoolID, 
			s.Nombre AS NombreSpool, 
			s.RevisionCliente, 
			s.Revision,
			s.Dibujo AS Isometrico, 
			s.Pdis,
			s.Peso,
			s.Area,
			s.SistemaPintura, 
			s.CodigoPintura, 
			s.ColorPintura,
			SUM(ISNULL(js.Peqs,0)) AS Peqs
	FROM RequisicionPintura rp
	INNER JOIN Proyecto p ON rp.ProyectoID = p.ProyectoID
	INNER JOIN RequisicionPinturaDetalle rpd ON rp.RequisicionPinturaID = rpd.RequisicionPinturaID
	INNER JOIN WorkstatusSpool ws ON ws.WorkstatusSpoolID = rpd.WorkstatusSpoolID
	INNER JOIN OrdenTrabajoSpool ots ON ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	INNER JOIN Spool s ON ots.SpoolID = s.SpoolID
	LEFT JOIN JuntaSpool js ON s.SpoolID = js.SpoolID AND js.FabAreaID = @FabAreaID
	WHERE rp.ProyectoID = @ProyectoID AND rp.RequisicionPinturaID = @RequisicionPinturaID
	GROUP BY rp.NumeroRequisicion,
			rp.FechaRequisicion, 
			p.Nombre ,
			ots.NumeroControl,
			s.SpoolID, 
			s.Nombre ,
			s.RevisionCliente, 
			s.Revision,
			s.Dibujo,
			s.Pdis,
			s.Peso,
			s.Area,
			s.SistemaPintura, 
			s.CodigoPintura, 
			s.ColorPintura
	
END