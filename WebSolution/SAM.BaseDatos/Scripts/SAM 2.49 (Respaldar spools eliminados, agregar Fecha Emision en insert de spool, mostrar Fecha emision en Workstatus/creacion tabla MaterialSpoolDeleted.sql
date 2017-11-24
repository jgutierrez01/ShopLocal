
CREATE TABLE dbo.MaterialSpoolDeleted(
	MaterialSpoolID int NOT NULL,
	SpoolID int NOT NULL,
	ItemCodeID int NOT NULL,
	Diametro1  decimal(7, 4) NOT NULL,
	Diametro2  decimal(7, 4) NOT NULL,
	Etiqueta nvarchar(10) NULL,
	Cantidad int NOT NULL,
	Peso decimal(7, 2) NULL,
	Area decimal(7, 2) NULL,
	Especificacion nvarchar(10) NULL,
	Grupo nvarchar(150) NULL,
	UsuarioModifica uniqueidentifier NULL,
	FechaModificacion datetime NULL,
	VersionRegistro timestamp NOT NULL,
	DescripcionMaterial nvarchar(150) NULL
	CONSTRAINT PK_MaterialSpoolDeleted PRIMARY KEY (MaterialSpoolID)
	)


