create table TipoPruebaSpool(
TipoPruebaSpoolID	INT	NOT NULL PRIMARY KEY IDENTITY,
Nombre				NVARCHAR(50) NOT NULL,
NombreIngles		NVARCHAR(100) NULL,
Categoria 		NVARCHAR(10) NULL,
UsuarioModifica		UNIQUEIDENTIFIER NULL,
FechaModificacion	DATETIME NULL,
VersionRegistro		TIMESTAMP NOT NULL,

CONSTRAINT FK_TipoPruebaSpool_aspnet_Users	FOREIGN KEY (UsuarioModifica) REFERENCES aspnet_Users (UserId)
)