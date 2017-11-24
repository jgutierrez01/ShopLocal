IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Armado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].Rpt_Armado
GO
-- =============================================
-- Author:		Osvaldo Flores
-- Create date: 29/Noviembre/2010
-- DescriptiON:	información del armado.
-- =============================================
CREATE PROCEDURE [dbo].[Rpt_Armado]
(
	@ProyectoID int,
	@FechaInicial datetime,
	@FechaFinal datetime
)
AS 
BEGIN
	SET NOCOUNT ON;
	
	SELECT s.Dibujo AS [Isometrico]
			,s.Revision
			,odt.NumeroOrden AS [OrdenTrabajo]
			,jws.EtiquetaJunta
			,p.Nombre AS [NombreProyecto]
			,p.FechaInicio
			,js.Diametro
			,js.Cedula
			,ja.FechaArmado
			,ja.Observaciones
			,tub.Codigo
			,tub.Nombre AS [NombreTubero]
			,tub.ApPaterno AS [ApPatTubero]
			,tub.ApMaterno AS [ApMatTubero]
			,t.Nombre AS [TallerNombre]
			,nu1.Codigo AS [NumeroUnico1]
			,nu2.Codigo AS [NumeroUnico2]
			FROM JuntaWorkstatus jws
			INNER JOIN OrdenTrabajoSpool ots
			ON ots.OrdenTrabajoSpoolID = jws.OrdenTrabajoSpoolID
			INNER JOIN JuntaSpool js
			ON js.JuntaSpoolID = jws.JuntaSpoolID
			INNER JOIN Spool s
			ON s.SpoolID = ots.SpoolID
			INNER JOIN OrdenTrabajo odt
			ON odt.OrdenTrabajoID = ots.OrdenTrabajoID
			INNER JOIN Proyecto p 	
			ON p.ProyectoID = odt.ProyectoID
			INNER JOIN JuntaArmado ja
			ON ja.JuntaArmadoID = jws.JuntaArmadoID
			INNER JOIN Taller t
			ON t.TallerID = ja.TallerID
			INNER JOIN Tubero tub
			ON tub.TuberoID = ja.TuberoID
			INNER JOIN NumeroUnico nu1
			ON nu1.NumeroUnicoID = ja.NumeroUnico1ID
			INNER JOIN NumeroUnico nu2
			ON nu2.NumeroUnicoID = ja.NumeroUnico2ID
			where p.ProyectoID = @ProyectoID			
			AND jws.ArmadoAprobado = 1
			AND jws.JuntaFinal = 1
			AND ja.FechaArmado >= @FechaInicial AND ja.FechaArmado <= @FechaFinal

END

GO

