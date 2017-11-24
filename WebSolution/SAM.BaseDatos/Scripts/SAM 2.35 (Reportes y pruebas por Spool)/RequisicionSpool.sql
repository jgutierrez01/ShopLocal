CREATE TABLE RequisicionSpool (
RequisicionSpoolID	INT	NOT NULL PRIMARY KEY IDENTITY,
ProyectoID			INT NOT NULL,
TipoPruebaSpoolID	INT NOT NULL,
FechaRequisicion	DATETIME NOT NULL,
NumeroRequisicion	NVARCHAR(50) NOT NULL,
Observaciones		NVARCHAR(500) NULL,	
UsuarioModifica		UNIQUEIDENTIFIER NULL,
FechaModificacion	DATETIME NULL,
VersionRegistro		TIMESTAMP NOT NULL,

CONSTRAINT FK_RequisicionSpool_Proyecto FOREIGN KEY (ProyectoID) REFERENCES Proyecto (ProyectoID),
CONSTRAINT FK_RequisicionSpool_TipoPruebaSpool FOREIGN KEY (TipoPruebaSpoolID) REFERENCES TipoPruebaSpool (TipoPruebaSpoolID),
CONSTRAINT FK_RequisicionSpool_aspnet_Users	FOREIGN KEY (UsuarioModifica) REFERENCES aspnet_Users (UserId),
CONSTRAINT UQ_RequisicionSpool_ProyectoReporteTipoPruebaSpool UNIQUE (ProyectoID, NumeroRequisicion, TipoPruebaSpoolID)
)