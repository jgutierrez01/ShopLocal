ALTER TABLE [AgrupadoresPorJunta]
ADD [UsuarioModifica] UNIQUEIDENTIFIER NULL

ALTER TABLE [AgrupadoresPorJunta]
ADD [FechaModificacion]  DATETIME NULL

ALTER TABLE [AgrupadoresPorJunta]
ADD [VersionRegistro]  timestamp NOT NULL
