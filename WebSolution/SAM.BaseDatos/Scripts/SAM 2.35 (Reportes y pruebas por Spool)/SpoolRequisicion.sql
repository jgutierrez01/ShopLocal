CREATE TABLE SpoolRequisicion(
SpoolRequisicionID	INT	NOT NULL PRIMARY KEY IDENTITY,
RequisicionSpoolID	INT	NOT NULL,
WorkstatusSpoolID	INT NOT NULL,
UsuarioModifica		UNIQUEIDENTIFIER NULL,
FechaModificacion	DATETIME NULL,
VersionRegistro		TIMESTAMP NOT NULL,

CONSTRAINT FK_SpoolRequisicion_RequisicionSpool FOREIGN KEY (RequisicionSpoolID) REFERENCES RequisicionSpool (RequisicionSpoolID),
CONSTRAINT FK_SpoolRequisicion_WorkstatusSpool FOREIGN KEY (WorkstatusSpoolID) REFERENCES WorkstatusSpool (WorkstatusSpoolID),
CONSTRAINT FK_SpoolRequisicion_aspnet_Users	FOREIGN KEY (UsuarioModifica) REFERENCES aspnet_Users (UserId)
)