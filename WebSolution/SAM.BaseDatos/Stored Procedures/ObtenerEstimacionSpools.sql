IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerEstimacionSpools]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerEstimacionSpools]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


/****************************************************************************************
	Nombre:		ObtenerEstimacionSpools
	Funcion:	Obtiene un listado con la estimación de los spools y posibilidades para estimar
	Parametros:	@ProyectoID int
	Autor:		IHM
	Modificado:	10/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerEstimacionSpools]
(
	 @ProyectoID int
)
AS
BEGIN

	SET NOCOUNT ON; 

	SELECT	 ws.[WorkstatusSpoolID]
			,ws.[OrdenTrabajoSpoolID]
			,ws.[Embarcado]
			,ws.[TieneLiberacionDimensional]
			,ws.[LiberadoPintura]
			,sp.Pdis
			,sp.Nombre
			,ots.NumeroControl
			,tEst.EstimacionSpoolID
			,tEst.EstimacionID
			,tEst.ConceptoEstimacionID
			,tEst.NumeroEstimacion
	FROM WorkstatusSpool ws
	INNER JOIN [OrdenTrabajoSpool] ots on ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
	INNER JOIN [Spool] sp on sp.SpoolID = ots.SpoolID
	LEFT JOIN
	(
		SELECT	 ess.[EstimacionSpoolID]
				,ess.[ConceptoEstimacionID]
				,ess.[WorkstatusSpoolID]
				,est.[EstimacionID]
				,est.[NumeroEstimacion]
				,est.[FechaEstimacion]
		FROM [EstimacionSpool] ess
		INNER JOIN [Estimacion] est on ess.EstimacionID = est.EstimacionID
		WHERE est.ProyectoID = @ProyectoID	
	) tEst ON ws.WorkstatusSpoolID = tEst.WorkstatusSpoolID
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

