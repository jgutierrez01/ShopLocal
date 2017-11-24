IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActualizaUsername]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ActualizaUsername]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ActualizaUsername
	Funcion:	Cambia el username en la tabla del membership provider				
	Parametros:	@UserID				UNIQUEIDENTIFIER
				@UserName			NVARCHAR(256)
	Autor:		IHM
	Modificado:	16/09/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ActualizaUsername]
(
	 @UserID			UNIQUEIDENTIFIER
	,@UserName			NVARCHAR(256)
)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE	aspnet_Users
	SET		 UserName = @UserName
			,LoweredUserName = LOWER(@UserName)
	WHERE	UserId = @UserID
	
	SELECT CAST(1 as bit)

	SET NOCOUNT OFF;

END
GO



