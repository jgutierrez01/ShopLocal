CREATE TABLE ReporteSpoolPnd(
ReporteSpoolPndID	INT	NOT NULL PRIMARY KEY IDENTITY,
ProyectoID			INT NOT NULL,
TipoPruebaSpoolID	INT NOT NULL,
NumeroReporte		NVARCHAR(50) NOT NULL,
FechaReporte		DATETIME NOT NULL,
UsuarioModifica		UNIQUEIDENTIFIER NULL,
FechaModificacion	DATETIME NULL,
VersionRegistro		TIMESTAMP NOT NULL,

CONSTRAINT FK_ReporteSpoolPnd_Proyecto FOREIGN KEY (ProyectoID) REFERENCES Proyecto (ProyectoID),
CONSTRAINT FK_ReporteSpoolPnd_TipoPruebaSpool FOREIGN KEY (TipoPruebaSpoolID) REFERENCES TipoPruebaSpool (TipoPruebaSpoolID),
CONSTRAINT FK_ReporteSpoolPnd_aspnet_Users	FOREIGN KEY (UsuarioModifica) REFERENCES aspnet_Users (UserId),
CONSTRAINT UQ_ReporteSpoolPnd_ProyectoReporteTipoPruebaSpool UNIQUE (ProyectoID, NumeroReporte, TipoPruebaSpoolID)
)