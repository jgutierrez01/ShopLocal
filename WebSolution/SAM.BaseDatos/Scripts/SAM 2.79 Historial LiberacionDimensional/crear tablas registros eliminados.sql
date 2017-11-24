

CREATE TABLE ReporteDimensionalDetalleDeleted(
	ReporteDimensionalDetalleID int NOT NULL,
	ReporteDimensionalID int NOT NULL,
	WorkstatusSpoolID int NOT NULL,
	Hoja int NULL,
	FechaLiberacion datetime NULL,
	Aprobado bit NOT NULL,
	Observaciones nvarchar(500) NULL,
	UsuarioModifica uniqueidentifier NULL,
	FechaModificacion datetime NULL,
	VersionRegistro timestamp NOT NULL
	 CONSTRAINT PK_ReporteDimensionalDetalleDeleted PRIMARY KEY(ReporteDimensionalDetalleID))

