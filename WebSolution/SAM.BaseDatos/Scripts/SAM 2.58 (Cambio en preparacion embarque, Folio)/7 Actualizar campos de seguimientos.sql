UPDATE CampoSeguimientoJunta 
	SET Nombre = 'Folio Preparaci�n', 
		NombreIngles = 'Preparation Folio',
		NombreControlUI = 'litEmbarqueFolioPreparacion',
		NombreColumnaSp = 'EmbarqueFolioPreparacion',
		DataFormat = '',
		TipoDeDato = 'System.String'
WHERE Nombre = 'Fecha Preparaci�n'


UPDATE CampoSeguimientoSpool
	SET Nombre = 'Folio Preparaci�n',
		NombreIngles = 'Preparation Folio',
		NombreControlUI = 'litEmbarqueFolioPreparacion',
		NombreColumnaSp = 'EmbarqueFolioPreparacion',
		AnchoUI = 150
WHERE Nombre = 'Fecha Preparaci�n'