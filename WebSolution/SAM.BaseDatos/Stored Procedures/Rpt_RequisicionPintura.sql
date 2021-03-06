USE [SAMPro]
GO
/****** Object:  StoredProcedure [dbo].[Rpt_RequisicionPintura]    Script Date: 6/25/2014 6:23:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[Rpt_RequisicionPintura]
	Funcion:	Reporte de requisicion pintura
	Parametros:	@RequisicionPinturaID int
	Autor:		PEGV
	Modificado:	06/09/2011 PEGV
				25/02/2014 GTG se agrego columna Localizacion
*****************************************************************************************/
ALTER PROCEDURE [dbo].[Rpt_RequisicionPintura]
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
			SUM(ISNULL(js.Peqs,0)) AS Peqs,
			c.Nombre as Localizacion
	FROM RequisicionPintura rp
	INNER JOIN Proyecto p ON rp.ProyectoID = p.ProyectoID
	INNER JOIN RequisicionPinturaDetalle rpd ON rp.RequisicionPinturaID = rpd.RequisicionPinturaID
	INNER JOIN WorkstatusSpool ws ON ws.WorkstatusSpoolID = rpd.WorkstatusSpoolID
	INNER JOIN OrdenTrabajoSpool ots ON ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	INNER JOIN Spool s ON ots.SpoolID = s.SpoolID
	LEFT JOIN JuntaSpool js ON s.SpoolID = js.SpoolID AND js.FabAreaID = @FabAreaID
	LEFT JOIN Cuadrante c on c.CuadranteID = s.CuadranteID
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
			s.ColorPintura,
			c.Nombre
	
END