IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaSpoolHistorico]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertaSpoolHistorico]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
/****************************************************************************************
	Nombre:		[InsertaSpoolHistorico]
	Funcion:	Genera un nuevo registro en la tabla Spool
	Parametros:	@SpoolID				INT
				@SpoolHistoricoID		INT OUTPUT
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[InsertaSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@SpoolHistoricoID		INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	INSERT INTO	dbo.SpoolHistorico 
					(
					SpoolID
					,ProyectoID
					,FamiliaAcero1ID
					,FamiliaAcero2ID
					,Nombre
					,Dibujo
					,Especificacion
					,Cedula
					,Pdis
					,DiametroPlano
					,Peso
					,A.Area
					,A.PorcentajePnd
					,A.RequierePwht
					,A.PendienteDocumental
					,A.AprobadoParaCruce
					,A.Prioridad
					,A.Revision
					,A.RevisionCliente
					,A.Segmento1
					,A.Segmento2
					,A.Segmento3
					,A.Segmento4
					,A.Segmento5
					,A.Segmento6
					,A.Segmento7
					,A.UsuarioModifica
					,A.FechaModificacion
					,A.SistemaPintura
					,A.ColorPintura
					,A.CodigoPintura
					,ProyectoNombre
					,FamiliaAcero1Nombre
					,FamiliaAcero2Nombre 
					,TieneHoldIngenieria
					,TieneHoldCalidad
					,Confinado				
					)
	SELECT 
					A.SpoolID
					,A.ProyectoID
					,A.FamiliaAcero1ID
					,A.FamiliaAcero2ID
					,A.Nombre
					,A.Dibujo
					,A.Especificacion
					,A.Cedula
					,A.Pdis
					,A.DiametroPlano
					,A.Peso
					,A.Area
					,A.PorcentajePnd
					,A.RequierePwht
					,A.PendienteDocumental
					,A.AprobadoParaCruce
					,A.Prioridad
					,A.Revision
					,A.RevisionCliente
					,A.Segmento1
					,A.Segmento2
					,A.Segmento3
					,A.Segmento4
					,A.Segmento5
					,A.Segmento6
					,A.Segmento7
					,A.UsuarioModifica
					,A.FechaModificacion
					,A.SistemaPintura
					,A.ColorPintura
					,A.CodigoPintura
					,B.Nombre as ProyectoNombre
					,C.Nombre as FamiliaAcero1Nombre
					,D.Nombre as FamiliaAcero2Nombre
					,F.TieneHoldIngenieria
					,F.TieneHoldCalidad
					,F.Confinado 
	FROM			dbo.Spool A
	INNER JOIN		dbo.Proyecto B ON A.ProyectoID = B.ProyectoID
	LEFT JOIN		dbo.FamiliaAcero C ON A.FamiliaAcero1ID = C.FamiliaAceroID
	LEFT JOIN		dbo.FamiliaAcero D ON A.FamiliaAcero2ID = D.FamiliaAceroID
	LEFT JOIN		dbo.aspnet_Users E ON A.UsuarioModifica = E.UserId
	LEFT JOIN		dbo.SpoolHold F ON A.SpoolID = F.SpoolID
	WHERE			A.SpoolID = @SpoolID
	
	SET @SpoolHistoricoID = SCOPE_IDENTITY() 

END
