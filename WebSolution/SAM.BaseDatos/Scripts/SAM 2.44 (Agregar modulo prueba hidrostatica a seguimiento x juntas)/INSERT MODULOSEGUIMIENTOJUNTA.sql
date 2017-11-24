INSERT INTO ModuloSeguimientoJunta (
			Nombre
			, NombreIngles
			, OrdenUI
			, NombreTemplateColumn)
			SELECT Nombre
					, NombreIngles
					, '19'
					, NombreTemplateColumn
			FROM ModuloSeguimientoSpool
			WHERE ModuloSeguimientoSpoolID = 9