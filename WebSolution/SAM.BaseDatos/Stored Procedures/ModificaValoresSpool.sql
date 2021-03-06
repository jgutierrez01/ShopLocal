IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ModificaValoresSpool]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ModificaValoresSpool]
GO

/****************************************************************************************
	Nombre:		[[ModificaValoresSpool]] 
	Funcion:					
	Parametros:	@Tabla					NVARCHAR(50)
				@Campo					NVARCHAR(50)
				@SpoolID				INT
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[ModificaValoresSpool]
	-- Add the parameters for the stored procedure here
	@Tabla					NVARCHAR(50)
	,@Campo					NVARCHAR(50)
	,@SpoolID				INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @Tabla = 'SPOOL'
		BEGIN
			IF @Campo = 'CEDULA'
				BEGIN
					UPDATE		A
					SET			A.Cedula = B.Cedula
					FROM		dbo.Spool  A
					INNER JOIN	(
								SELECT		TOP 1 
											BA.SpoolID,BA.Cedula
								FROM		dbo.JuntaSpool BA
								WHERE		BA.SpoolID = @SpoolID
								GROUP BY	BA.SpoolID,Cedula
								ORDER BY	MAX(BA.Diametro) DESC
								) B ON A.SpoolID = B.SpoolID					
				END 
		END 
		
    IF @Tabla = 'SPOOLPENDIENTE'
		BEGIN
			IF @Campo = 'CEDULA'
				BEGIN
					UPDATE		A
					SET			A.Cedula = B.Cedula
					FROM		dbo.SpoolPendiente  A
					INNER JOIN	(
								SELECT		TOP 1 
											BA.SpoolPendienteID,BA.Cedula
								FROM		dbo.JuntaSpoolPendiente BA
								WHERE		BA.SpoolPendienteID = @SpoolID
								GROUP BY	BA.SpoolPendienteID,BA.Cedula
								ORDER BY	MAX(BA.Diametro) DESC
								) B ON A.SpoolPendienteID = B.SpoolPendienteID
				END 					
		END		
END
