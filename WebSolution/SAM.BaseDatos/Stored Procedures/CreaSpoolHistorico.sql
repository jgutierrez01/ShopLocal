IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreaSpoolHistorico]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[CreaSpoolHistorico]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		[CreaSpoolHistorico]
	Funcion:	Copia toda la informacion de un spool y lo copia en las tablas de historicos
	Parametros:	@SpoolID INT				
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[CreaSpoolHistorico]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	DECLARE	@SpoolHistoricoID INT

	EXEC dbo.InsertaSpoolHistorico @SpoolID,@SpoolHistoricoID = @SpoolHistoricoID OUTPUT
	EXEC dbo.InsertaMaterialSpoolHistorico @SpoolID, @SpoolHistoricoID
	EXEC dbo.InsertaJuntaSpoolHistorico @SpoolID, @SpoolHistoricoID
	EXEC dbo.InsertaCorteSpoolHistorico @SpoolID, @SpoolHistoricoID
	
	SELECT 	@SpoolHistoricoID
	
END



