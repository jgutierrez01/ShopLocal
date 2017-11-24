/****** Object:  StoredProcedure [dbo].[EliminaSpool]    Script Date: 02/02/2012 16:02:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[EliminaSpool]
	-- Add the parameters for the stored procedure here
	@SpoolID				INT
	,@Tabla					NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF @Tabla = 'SPOOL'  
		BEGIN
		
			/*
			TIPOS DE ACCESORIOS 
			Tubo = 1, Accessorio = 2 
			*/
		
			DECLARE	@SpoolHistoricoID INT

			EXEC dbo.InsertaSpoolHistorico @SpoolID,@SpoolHistoricoID = @SpoolHistoricoID OUTPUT
			EXEC dbo.InsertaMaterialSpoolHistorico @SpoolID, @SpoolHistoricoID
			EXEC dbo.InsertaJuntaSpoolHistorico @SpoolID, @SpoolHistoricoID
			EXEC dbo.InsertaCorteSpoolHistorico @SpoolID, @SpoolHistoricoID
			
			DELETE FROM dbo.CorteSpool WHERE SpoolID = @SpoolID
			DELETE FROM dbo.JuntaSpool WHERE SpoolID = @SpoolID
			
			UPDATE		A 
			SET			A.InventarioDisponibleCruce = A.InventarioBuenEstado - (A.InventarioCongelado - B.Cantidad)
						,A.InventarioCongelado = A.InventarioCongelado - B.Cantidad 
			FROM		dbo.NumeroUnicoInventario A
			INNER JOIN	(
							SELECT		BA.NumeroUnicoCongeladoID,SUM(BC.Cantidad) AS Cantidad
							FROM		dbo.CongeladoParcial BA
							INNER JOIN	dbo.MaterialSpool BC ON BA.MaterialSpoolID = BC.MaterialSpoolID AND BC.SpoolID = @SpoolID
							INNER JOIN	dbo.ItemCode BD ON BC.ItemCodeID = BD.ItemCodeID 
							GROUP BY	BA.NumeroUnicoCongeladoID
						) B ON A.NumeroUnicoID = B.NumeroUnicoCongeladoID
			
			UPDATE		A 
			SET			A.InventarioDisponibleCruce = A.InventarioBuenEstado - (A.InventarioCongelado - B.Cantidad)
						,A.InventarioCongelado = A.InventarioCongelado - B.Cantidad 
			FROM		dbo.NumeroUnicoSegmento A 
			INNER JOIN	(
							SELECT		BA.NumeroUnicoCongeladoID,BA.SegmentoCongelado,SUM(BC.Cantidad) AS Cantidad			
							FROM		dbo.CongeladoParcial BA
							INNER JOIN	dbo.MaterialSpool BC ON BA.MaterialSpoolID = BC.MaterialSpoolID AND BC.SpoolID = @SpoolID
							INNER JOIN	dbo.ItemCode BD ON BC.ItemCodeID = BD.ItemCodeID AND BD.TipoMaterialID = 1 -- APLICA SOLO PARA TUBOS
							GROUP BY	BA.NumeroUnicoCongeladoID, BA.SegmentoCongelado
						) B ON A.NumeroUnicoID = B.NumeroUnicoCongeladoID AND A.Segmento = B.SegmentoCongelado
			
			DELETE		A
			FROM		dbo.CongeladoParcial A 
			INNER JOIN	dbo.MaterialSpool B ON A.MaterialSpoolID = B.MaterialSpoolID AND B.SpoolID = @SpoolID
			
			DELETE FROM dbo.MaterialSpool WHERE SpoolID = @SpoolID
			DELETE FROM dbo.Spool WHERE SpoolID = @SpoolID

		END
	IF @Tabla = 'SPOOLPENDIENTE'
		BEGIN
		
			DELETE FROM dbo.CorteSpoolPendiente WHERE SpoolPendienteID = @SpoolID
			DELETE FROM dbo.JuntaSpoolPendiente WHERE SpoolPendienteID = @SpoolID
			DELETE FROM dbo.MaterialSpoolPendiente WHERE SpoolPendienteID = @SpoolID
			DELETE FROM dbo.SpoolPendiente WHERE SpoolPendienteID = @SpoolID
		
		END 
	
END

GO


