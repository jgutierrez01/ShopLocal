IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtiquetaEmbarque]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].EtiquetaEmbarque
GO
-- =============================================
-- Author:		Lilian Mendoza
-- Create date: 19-mayo-2011
-- Description:	Información para las etiquetas de materiales.
-- =============================================
CREATE PROCEDURE [dbo].[EtiquetaEmbarque]
(
	 @IDs nvarchar(MAX)
	 ,@NumeroEtiqueta nvarchar(100)
	 ,@NumeroControl nvarchar(100)
	 ,@OrdenTrabajo nvarchar(100)
)
AS 
BEGIN
	SET NOCOUNT ON;
		
		IF(@NumeroEtiqueta = '' AND @NumeroControl = '' AND @OrdenTrabajo = '')
		BEGIN
		
	SELECT
			 p.Nombre [NombreProyecto]
			,ot.NumeroOrden
			,ot.FechaOrden
			,ots.NumeroControl
			,ots.Partida
			,s.SpoolID
			,s.Nombre
			,s.Dibujo
			,s.Especificacion
			,s.Cedula
			,s.Pdis
			,s.DiametroPlano
			,s.Peso
			,s.Area
			,s.Revision
			,s.RevisionCliente
			,s.Segmento1
			,s.Segmento2
			,s.Segmento3
			,s.Segmento4
			,s.Segmento5
			,s.Segmento6
			,s.Segmento7
			,e.FechaEmbarque
			,e.NumeroEmbarque
			,s.FechaEtiqueta
			,wk.FechaPreparacion
			,s.NumeroEtiqueta
	FROM Spool s
	INNER JOIN Proyecto p on p.ProyectoID = s.ProyectoID
	INNER JOIN OrdenTrabajoSpool ots on ots.SpoolID = s.SpoolID
	INNER JOIN OrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
	LEFT JOIN WorkstatusSpool wk on wk.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	LEFT JOIN EmbarqueSpool es on es.WorkstatusSpoolID = wk.WorkstatusSpoolID
	LEFT JOIN Embarque e on e.EmbarqueID = es.EmbarqueID
	WHERE s.SpoolID IN
	(
		SELECT CAST([Value] AS INT)
		FROM dbo.SplitCVSToTable(@IDs,',')
	)
	
	END
	ELSE
	BEGIN
	SELECT
			 p.Nombre [NombreProyecto]
			,ot.NumeroOrden
			,ot.FechaOrden
			,ots.NumeroControl
			,ots.Partida
			,s.SpoolID
			,s.Nombre
			,s.Dibujo
			,s.Especificacion
			,s.Cedula
			,s.Pdis
			,s.DiametroPlano
			,s.Peso
			,s.Area
			,s.Revision
			,s.RevisionCliente
			,s.Segmento1
			,s.Segmento2
			,s.Segmento3
			,s.Segmento4
			,s.Segmento5
			,s.Segmento6
			,s.Segmento7
			,e.FechaEmbarque
			,e.NumeroEmbarque
			,s.FechaEtiqueta
			,wk.FechaPreparacion
			,s.NumeroEtiqueta
	FROM Spool s
	INNER JOIN Proyecto p on p.ProyectoID = s.ProyectoID
	INNER JOIN OrdenTrabajoSpool ots on ots.SpoolID = s.SpoolID
	INNER JOIN OrdenTrabajo ot on ot.OrdenTrabajoID = ots.OrdenTrabajoID
	INNER JOIN WorkstatusSpool wk on wk.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	LEFT JOIN EmbarqueSpool es on es.WorkstatusSpoolID = wk.WorkstatusSpoolID
	LEFT JOIN Embarque e on e.EmbarqueID = es.EmbarqueID
	WHERE(@NumeroEtiqueta = '' OR s.NumeroEtiqueta = @NumeroEtiqueta)
	AND (@NumeroControl = '' OR ots.NumeroControl = @NumeroControl)
	AND (@OrdenTrabajo = '' OR ot.NumeroOrden = @OrdenTrabajo)
	END
END

GO

/*

select * from workstatusSpool
exec EtiquetaEmbarque '6,7,8','988-89','',''
select * from recepcion
select * from numerounico where TieneDano=1
select * from recepcionnumerounico where numerounicoid=21191
*/