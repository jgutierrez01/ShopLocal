UPDATE CampoSeguimientoJunta 
	SET Nombre = 'Folio Preparación', 
		NombreIngles = 'Preparation Folio',
		NombreControlUI = 'litEmbarqueFolioPreparacion',
		NombreColumnaSp = 'EmbarqueFolioPreparacion',
		DataFormat = '',
		TipoDeDato = 'System.String'
WHERE Nombre = 'Fecha Preparación'


UPDATE CampoSeguimientoSpool
	SET Nombre = 'Folio Preparación',
		NombreIngles = 'Preparation Folio',
		NombreControlUI = 'litEmbarqueFolioPreparacion',
		NombreColumnaSp = 'EmbarqueFolioPreparacion',
		AnchoUI = 150
WHERE Nombre = 'Fecha Preparación'