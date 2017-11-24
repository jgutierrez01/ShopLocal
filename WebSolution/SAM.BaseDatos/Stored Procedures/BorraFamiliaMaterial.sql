IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BorraFamiliaMaterial]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[BorraFamiliaMaterial]
GO

Create PROCEDURE [dbo].[BorraFamiliaMaterial]
(	
	@FamiliaMaterialID int,
	@VersionRegistro timestamp
)
AS
BEGIN
	
	DELETE FROM FamiliaMaterial
	WHERE	FamiliaMaterialID = @FamiliaMaterialID
			AND
			VersionRegistro = @VersionRegistro

END

GO

