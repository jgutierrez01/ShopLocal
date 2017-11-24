IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerEstimacionJuntas]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerEstimacionJuntas]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


/****************************************************************************************
	Nombre:		ObtenerEstimacionJuntas
	Funcion:	Obtiene un listado con la estimación de las juntas y posibilidades para estimar
	Parametros:	@ProyectoID int
	Autor:		IHM
	Modificado:	10/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerEstimacionJuntas]
(
	 @ProyectoID int
)
AS
BEGIN

	SET NOCOUNT ON; 

	SELECT	 jw.[JuntaWorkstatusID]
			,jw.[OrdenTrabajoSpoolID]
			,jw.[JuntaSpoolID]
			,jw.[EtiquetaJunta]
			,jw.[ArmadoAprobado]
			,jw.[SoldaduraAprobada]
			,jw.[InspeccionVisualAprobada]
			,js.Diametro
			,js.FamiliaAceroMaterial1ID
			,js.FamiliaAceroMaterial2ID
			,js.TipoJuntaID
			,ots.NumeroControl
			,s.Nombre [NombreSpool]
			,tEst.EstimacionJuntaID
			,tEst.EstimacionID
			,tEst.ConceptoEstimacionID
			,tEst.NumeroEstimacion
			,tRepPnd.JuntaReportePndID
			,tRepPnd.TipoPruebaID [TipoPruebaPndID]
			,tRepPnd.Aprobado [AprobadoPnd]
			,tRepTt.JuntaReporteTtID
			,tRepTt.TipoPruebaID [TipoPruebaTtID]
			,tRepTt.Aprobado [AprobadoTt]
			,tRepD.ReporteDimensionalDetalleID
			,tRepD.ReporteDimensionalID
			,tRepD.Aprobado [AprobadoReporteDimensional]
	FROM [JuntaWorkstatus] jw
	INNER JOIN [OrdenTrabajoSpool] ots on jw.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	INNER JOIN [JuntaSpool] js on js.JuntaSpoolID = jw.JuntaSpoolID
	INNER JOIN
	(
		SELECT SpoolID, Nombre
		FROM Spool
		WHERE ProyectoID = @ProyectoID
	) s on s.SpoolID = ots.SpoolID
	LEFT JOIN
	(
		SELECT	 esj.[EstimacionJuntaID]
				,esj.[ConceptoEstimacionID]
				,esj.[JuntaWorkstatusID]
				,est.[EstimacionID]
				,est.[NumeroEstimacion]
				,est.[FechaEstimacion]
		FROM [EstimacionJunta] esj
		INNER JOIN [Estimacion] est on esj.EstimacionID = est.EstimacionID
		WHERE est.ProyectoID = @ProyectoID	
	) tEst ON jw.JuntaWorkstatusID = tEst.JuntaWorkstatusID
	LEFT JOIN
	(
		SELECT	 jrpnd.[JuntaReportePndID]
				,jrpnd.[JuntaWorkstatusID]
				,jrpnd.Aprobado
				,rpnd.TipoPruebaID
		FROM [JuntaReportePnd] jrpnd
		INNER JOIN [ReportePnd] rpnd on jrpnd.ReportePndID = rpnd.ReportePndID
		WHERE rpnd.ProyectoID = @ProyectoID
	) tRepPnd ON jw.JuntaWorkstatusID = tRepPnd.JuntaWorkstatusID
	LEFT JOIN
	(
		SELECT	 jrtt.[JuntaReporteTtID]
				,jrtt.[JuntaWorkstatusID]
				,jrtt.Aprobado
				,rtt.TipoPruebaID
		FROM [JuntaReporteTt] jrtt
		INNER JOIN [ReporteTt] rtt on jrtt.ReporteTtID = rtt.ReporteTtID
		WHERE rtt.ProyectoID = @ProyectoID
	) tRepTt ON jw.JuntaWorkstatusID = tRepTt.JuntaWorkstatusID
	LEFT JOIN
	(
		SELECT	 rdd.ReporteDimensionalDetalleID
				,rdd.ReporteDimensionalID
				,rdd.Aprobado
				,ws.OrdenTrabajoSpoolID
		FROM [ReporteDimensionalDetalle] rdd
		INNER JOIN [ReporteDimensional] rd ON rdd.ReporteDimensionalID = rd.ReporteDimensionalID
		INNER JOIN [WorkstatusSpool] ws ON rdd.WorkstatusSpoolID = ws.WorkstatusSpoolID
		WHERE	rd.ProyectoID = @ProyectoID
				AND rd.TipoReporteDimensionalID = 1
				AND ws.TieneLiberacionDimensional = 1				
				AND rdd.Aprobado = 1
	) tRepD ON jw.OrdenTrabajoSpoolID = tRepD.OrdenTrabajoSpoolID
	WHERE EXISTS
	(
		SELECT 1
		FROM OrdenTrabajo ot
		WHERE	ot.ProyectoID = @ProyectoID
				AND ots.OrdenTrabajoID = ot.OrdenTrabajoID
	)
	
	SET NOCOUNT OFF;

END
GO

