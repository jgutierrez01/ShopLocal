IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaJuntaSpoolHistorico]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertaJuntaSpoolHistorico]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[InsertaJuntaSpoolHistorico]
	Funcion:	Genera un nuevo registro en la tabla JuntaSpoolHistorico
	Parametros:	@SpoolID				INT
				@SpoolHistoricoID		INT			
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[InsertaJuntaSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@SpoolHistoricoID		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO		dbo.JuntaSpoolHistorico 
					(
					SpoolHistoricoID
					,JuntaSpoolID			 
					,SpoolID
					,TipoJuntaID
					,FabAreaID
					,Etiqueta
					,EtiquetaMaterial1
					,EtiquetaMaterial2
					,Cedula
					,FamiliaAceroMaterial1ID
					,FamiliaAceroMaterial2ID
					,Diametro
					,Espesor
					,KgTeoricos
					,Peqs
					,UsuarioModifica
					,FechaModificacion
					,SpoolNombre
					,TipoJuntaCodigo
					,FabAreaCodigo
					,FamiliaAceroMaterial1Nombre
					,FamiliaAceroMaterial2Nombre
					)
	SELECT	
					@SpoolHistoricoID
					,A.JuntaSpoolID
					,A.SpoolID
					,A.TipoJuntaID
					,A.FabAreaID
					,A.Etiqueta
					,A.EtiquetaMaterial1
					,A.EtiquetaMaterial2
					,A.Cedula
					,A.FamiliaAceroMaterial1ID
					,A.FamiliaAceroMaterial2ID
					,A.Diametro
					,A.Espesor
					,A.KgTeoricos
					,A.Peqs
					,A.UsuarioModifica
					,A.FechaModificacion
					,B.Nombre as SpoolNombre 
					,C.Codigo as TipoJuntaCodigo
					,D.Codigo as FabAreaCodigo
					,E.Nombre as FamiliaAceroMaterial1Nombre 
					,F.Nombre as FamiliaAceroMaterial2Nombre 
	FROM			dbo.JuntaSpool A 
	INNER JOIN		dbo.Spool B ON A.SpoolID = B.SpoolID
	INNER JOIN		dbo.TipoJunta C ON A.TipoJuntaID = C.TipoJuntaID
	INNER JOIN		dbo.FabArea D ON A.FabAreaID = D.FabAreaID
	INNER JOIN		dbo.FamiliaAcero E ON A.FamiliaAceroMaterial1ID = E.FamiliaAceroID
	LEFT JOIN		dbo.FamiliaAcero F ON A.FamiliaAceroMaterial2ID = F.FamiliaAceroID
	LEFT JOIN		dbo.aspnet_Users G ON A.UsuarioModifica = G.UserId
	WHERE			A.SpoolID = @SpoolID
	
END
