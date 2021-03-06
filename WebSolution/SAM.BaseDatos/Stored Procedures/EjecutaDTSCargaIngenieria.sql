IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EjecutaDTSCargaIngenieria]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[EjecutaDTSCargaIngenieria]
GO

/****************************************************************************************
	Nombre:		[EjecutaDTSCargaIngenieria] 
	Funcion:					
	Parametros:	@CommandString AS VARCHAR(4000)
	Autor:		HL
	Modificado:	
*****************************************************************************************/

CREATE PROCEDURE [dbo].[EjecutaDTSCargaIngenieria]
	-- Add the parameters for the stored procedure here
	@CommandString AS VARCHAR(4000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	 
	/*
	@ReturnCode recibe el resultado de la ejecución del DTEXEC
	Value  Description  
	0  The package executed successfully.  
	1  The package failed.  
	3  The package was cancelled by the user.  
	4  The utility was unable to locate the requested package. The package could not be found.  
	5  The utility was unable to load the requested package. The package could not be loaded.  
	6  The utility encountered an internal error of syntactic or semantic errors in the command line. 	
	*/

	DECLARE		@DTSExecStartTime	DATETIME
	DECLARE		@DTSExecEndTime		DATETIME 
	DECLARE		@ReturnCode			INT
	
	SET			@DTSExecStartTime = GETDATE() 
	
	EXEC		@ReturnCode = XP_CMDSHELL @CommandString,NO_OUTPUT


	SET			@DTSExecEndTime = GETDATE() 	

	INSERT INTO	dbo.DTSExecLog 
				(DTSExecStartTime,DTSExecEndTime,DTSExecCommandString,DTSExecReturnCode,UsuarioModifica,FechaModificacion) 
	VALUES		(@DTSExecStartTime,@DTSExecEndTime,@CommandString,@ReturnCode,NULL,GETDATE())

	SELECT		@ReturnCode

END
