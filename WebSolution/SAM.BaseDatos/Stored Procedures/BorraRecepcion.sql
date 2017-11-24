IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BorraRecepcion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[BorraRecepcion]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		BorraRecepcion
	Funcion:	Elimina la recepción y todas sus relaciones			
	Parametros:	@RecepcionID	INT
	Autor:		LMG
	Modificado:	01/10/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[BorraRecepcion]
(
	@RecepcionID	INT
)
AS
BEGIN

	SET NOCOUNT ON;
	
	CREATE TABLE #NumUnico
	( 
		NumeroUnicoID INT
	)
	
	INSERT INTO #NumUnico
	SELECT rnu.NumeroUnicoID 	
	FROM RecepcionNumeroUnico rnu		
	WHERE rnu.RecepcionID = @RecepcionID

    DELETE FROM RecepcionNumeroUnico WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
	
	DELETE FROM NumeroUnicoInventario WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
	
	DELETE FROM NumeroUnicoMovimiento WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
	
	DELETE FROM NumeroUnicoSegmento WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
		
	DELETE FROM NumeroUnico WHERE
	NumeroUnicoID in (SELECT NumeroUnicoID FROM #NumUnico)
	
	DELETE FROM Recepcion WHERE
	RecepcionID = @RecepcionID
	
	DROP TABLE #NumUnico
	
	SELECT CAST(1 as bit)	

	SET NOCOUNT OFF;

END
GO



