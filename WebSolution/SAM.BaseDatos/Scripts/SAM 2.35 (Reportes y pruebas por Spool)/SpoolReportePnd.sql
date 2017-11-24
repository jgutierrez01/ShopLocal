CREATE TABLE SpoolReportePnd(
SpoolReportePndID	INT NOT NULL PRIMARY KEY IDENTITY,
ReporteSpoolPndID	INT NOT NULL,
WorkstatusSpoolID	INT NOT NULL,
SpoolRequisicionID	INT NOT NULL,
FechaPrueba			DATETIME NOT NULL,
Aprobado			BIT NOT NULL DEFAULT 0,
Observaciones		NVARCHAR(500) NULL,
Hoja			INT NULL,
UsuarioModifica		UNIQUEIDENTIFIER NULL,
FechaModificacion	DATETIME NULL,
VersionRegistro		TIMESTAMP NOT NULL,

CONSTRAINT FK_SpoolReportePnd_ReporteSpoolPnd FOREIGN KEY (ReporteSpoolPndID) REFERENCES ReporteSpoolPnd (ReporteSpoolPndID),
CONSTRAINT FK_SpoolReportePnd_WorkstatusSpool FOREIGN KEY (WorkstatusSpoolID) REFERENCES WorkstatusSpool (WorkstatusSpoolID),
CONSTRAINT FK_SpoolReportePnd_SpoolRequisicion FOREIGN KEY (SpoolRequisicionID) REFERENCES SpoolRequisicion (SpoolRequisicionID),
CONSTRAINT FK_SpoolReportePnd_aspnet_Users	FOREIGN KEY (UsuarioModifica) REFERENCES aspnet_Users (UserId)
)