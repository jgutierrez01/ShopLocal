CREATE TABLE CorteSpoolDeleted(
CorteSpoolID int NOT NULL,
SpoolID int NOT NULL,
ItemCodeID int NOT NULL,
TipoCorte1ID int NOT NULL,
TipoCorte2ID int NOT NULL,
EtiquetaMaterial nvarchar(10) NULL,
EtiquetaSeccion nvarchar(15) NULL,
Diametro decimal(7,4) NOT NULL,
InicioFin nvarchar(500) NULL,
Cantidad int NULL,
Observaciones nvarchar(500) NULL,
UsuarioModifica uniqueidentifier NULL,
FechaModificacion datetime NULL,
VersionRegistro timestamp NOT NULL
CONSTRAINT PK_CorteSpoolsDeleted PRIMARY KEY(CorteSpoolID)
)