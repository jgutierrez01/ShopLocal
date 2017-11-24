
CREATE TABLE dbo.JuntaSpoolDeleted(
	JuntaSpoolID int NOT NULL,
	SpoolID int NOT NULL,
	TipoJuntaId int NOT NULL,
	FabAreaID int NOT NULL,
	Etiqueta nvarchar(10) NULL,
	EtiquetaMaterial1 nvarchar(10) NULL,
	EtiquetaMaterial2 nvarchar(10) NULL,
	Cedula nvarchar(10) NULL,
	FamiliaAceroMaterial1ID int NOT NULL,
	FamiliaAceroMaterial2ID int NULL,
	Diametro decimal(7, 4) NOT NULL,
	Espesor decimal(10, 4) NULL,
	KgTeoricos decimal(12, 4) NULL,
	Peqs decimal(10, 4) NULL,
	UsuarioModifica uniqueidentifier NULL,
	FechaModificacion datetime NULL,
	VersionRegistro timestamp NOT NULL,
	EsManual bit NULL,
	EstacionID int NULL,
	FabClas nvarchar(150) NULL,
	Campo2 nvarchar(150) NULL,
	Campo3 nvarchar(150) NULL,
	Campo4 nvarchar(150) NULL,
	Campo5 nvarchar(150) NULL,
    CONSTRAINT PK_JuntaSpoolDeleted PRIMARY KEY (JuntaSpoolID)

 )


