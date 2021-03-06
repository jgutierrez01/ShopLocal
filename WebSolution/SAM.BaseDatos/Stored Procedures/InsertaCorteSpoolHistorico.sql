IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertaCorteSpoolHistorico]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertaCorteSpoolHistorico]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[InsertaCorteSpoolHistorico]
	Funcion:	Genera un nuevo registro en la tabla CorteSpoolHistorico
	Parametros:	@SpoolID				INT
				@SpoolHistoricoID		INT			
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[InsertaCorteSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@SpoolHistoricoID		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	INSERT INTO		dbo.CorteSpoolHistorico
					(
					SpoolHistoricoID
					,CorteSpoolID
					,SpoolID
					,ItemCodeID
					,TipoCorte1ID
					,TipoCorte2ID
					,EtiquetaMaterial
					,EtiquetaSeccion
					,Diametro
					,InicioFin
					,Cantidad
					,Observaciones
					,UsuarioModifica
					,FechaModificacion
					,SpoolNombre
					,ItemCodeCodigo
					,ItemCodeDescripcionEspanol
					,ItemCodeDescripcionIngles
					,TipoCorte1Codigo
					,TipoCorte2Codigo
					)
	SELECT			@SpoolHistoricoID
					,A.CorteSpoolID
					,A.SpoolID
					,A.ItemCodeID
					,A.TipoCorte1ID
					,A.TipoCorte2ID
					,A.EtiquetaMaterial
					,A.EtiquetaSeccion
					,A.Diametro
					,A.InicioFin
					,A.Cantidad
					,A.Observaciones
					,A.UsuarioModifica
					,A.FechaModificacion
					,B.Nombre as SpoolNombre
					,C.Codigo as ItemCodeCodigo
					,C.DescripcionEspanol as ItemCodeDescripcionEspanol
					,C.DescripcionIngles as ItemCodeDescripcionIngles
					,D.Codigo as TipoCorte1Codigo
					,E.Codigo as TipoCorte2Codigo
	FROM			dbo.CorteSpool A 
	INNER JOIN		dbo.Spool B ON A.SpoolID = B.SpoolID
	INNER JOIN		dbo.ItemCode C ON A.ItemCodeID = C.ItemCodeID
	INNER JOIN		dbo.TipoCorte D ON A.TipoCorte1ID = D.TipoCorteID
	INNER JOIN		dbo.TipoCorte E ON A.TipoCorte2ID = E.TipoCorteID
	LEFT JOIN		dbo.aspnet_Users F ON A.UsuarioModifica = F.UserId
	WHERE			A.SpoolID = @SpoolID

END
