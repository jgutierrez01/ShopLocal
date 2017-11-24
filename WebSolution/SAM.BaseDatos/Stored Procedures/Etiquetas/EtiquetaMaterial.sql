IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtiquetaMaterial]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[EtiquetaMaterial]
GO

-- =============================================
-- Author:		Lilian Mendoza
-- Modificado por Jorge Vargas 17 feb 2011
-- Create date: 19-mayo-2011
-- Description:	Información para las etiquetas de materiales.
-- =============================================
CREATE PROCEDURE [dbo].[EtiquetaMaterial]
(
	 @RecepcionID int
	,@Idioma int
)
AS 
BEGIN
	SET NOCOUNT ON;		
	
	DECLARE @SI VARCHAR(3) 
	SET @SI = CASE WHEN @Idioma = 1 THEN 'Si' ELSE 'Yes' END	
	
	CREATE TABLE #TempNU
	(
		NumeroUnicoID int
	)
	
--Creamos una tabla temporal y le insertamos el número de registros igual al máximo de cantidad recibida de la recepción en cuestión y que sean accesorios. 
--El número máximo de copias va a ser igual al número de registros que tenga la tabla de NumeroUnicoInventario
INSERT INTO #TempNU
SELECT TOP (
  select ISNULL(MAX(CantidadRecibida),0) from NumeroUnicoInventario nui
  join NumeroUnico nu on nui.NumeroUnicoID = nu.NumeroUnicoID
  join ItemCode ic on nu.ItemCodeID = ic.ItemCodeID
  join RecepcionNumeroUnico r on r.NumeroUnicoID = nu.NumeroUnicoID
  where r.RecepcionID = @RecepcionID
  and ic.TipoMaterialID = 2) rn = ROW_NUMBER() 
  OVER (ORDER BY [numerounicoid]) 
  FROM dbo.NumeroUnicoInventario 
  ORDER BY [numerounicoid]

INSERT INTO #TempNU
SELECT nui.NumeroUnicoID FROM #TempNU
CROSS JOIN NumeroUnicoInventario AS nui
join NumeroUnico nu on nui.NumeroUnicoID = nu.NumeroUnicoID
  join ItemCode ic on nu.ItemCodeID = ic.ItemCodeID
  join RecepcionNumeroUnico r on r.NumeroUnicoID = nu.NumeroUnicoID
WHERE #TempNU.NumeroUnicoID <= nui.CantidadRecibida
AND r.RecepcionID = @RecepcionID
and ic.TipoMaterialID = 2
order by nui.NumeroUnicoID
	
	SELECT
			 p.ProyectoID
			,p.Nombre
			,nu.NumeroUnicoID
			,nu.Codigo
			,nu.Diametro1
			,nu.Diametro2
			,nu.Cedula
			,CantRecibida = (select CantidadRecibida from NumeroUnicoInventario nui where nui.NumeroUnicoID = nu.NumeroUnicoID)
			,ic.Codigo [ItemCode]
			,ic.DescripcionEspanol [DescripcionItemCode]
			,c.NumeroColada
			,c.NumeroCertificado
			,pr.Nombre [Proveedor]
			,f.Nombre [Fabricante]
			,tc.Codigo [TipoCorte]
			,tc2.Codigo [TipoCorte2]
			,nu.NumeroUnicoCliente
			,nu.Factura
			,nu.PartidaFactura
			,nu.OrdenDeCompra
			,nu.PartidaOrdenDeCompra			
			,case when nu.MarcadoAsme = 1 
				then @SI else 'No' end [MarcadoAsme]
			,case when nu.MarcadoGolpe = 1 
				then @SI else 'No' end [MarcadoGolpe]
			,case when nu.MarcadoPintura = 1 
				then @SI else 'No' end [MarcadoPintura]
			,case when nu.PruebasHidrostaticas = 1 
				then @SI else 'No' end [PruebasHidrostaticas]
			,case when nu.TieneDano = 1 
				then @SI else 'No' end [TieneDano]
	FROM NumeroUnico nu
	INNER JOIN RecepcionNumeroUnico r on r.NumeroUnicoID = nu.NumeroUnicoID
	INNER JOIN Proyecto p on p.ProyectoID = nu.ProyectoID
	LEFT JOIN ItemCode ic on ic.ItemCodeID = nu.ItemCodeID
	LEFT JOIN Colada c on c.ColadaID = nu.ColadaID
	LEFT JOIN Proveedor pr on pr.ProveedorID = nu.ProveedorID
	LEFT JOIN Fabricante f on f.FabricanteID = nu.FabricanteID
	LEFT JOIN TipoCorte tc on tc.TipoCorteID = nu.TipoCorte1ID
	LEFT JOIN TipoCorte tc2 on tc2.TipoCorteID = nu.TipoCorte2ID
	WHERE r.RecepcionID = @RecepcionID and ic.TipoMaterialID = 1
	UNION ALL
	(SELECT
			 p.ProyectoID
			,p.Nombre
			,nu.NumeroUnicoID
			,nu.Codigo
			,nu.Diametro1
			,nu.Diametro2
			,nu.Cedula
			,nui.CantidadRecibida as CantRecibida
			,ic.Codigo [ItemCode]
			,ic.DescripcionEspanol [DescripcionItemCode]
			,c.NumeroColada
			,c.NumeroCertificado
			,pr.Nombre [Proveedor]
			,f.Nombre [Fabricante]
			,tc.Codigo [TipoCorte]
			,tc2.Codigo [TipoCorte2]
			,nu.NumeroUnicoCliente
			,nu.Factura
			,nu.PartidaFactura
			,nu.OrdenDeCompra
			,nu.PartidaOrdenDeCompra			
			,case when nu.MarcadoAsme = 1 
				then @SI else 'No' end [MarcadoAsme]
			,case when nu.MarcadoGolpe = 1 
				then @SI else 'No' end [MarcadoGolpe]
			,case when nu.MarcadoPintura = 1 
				then @SI else 'No' end [MarcadoPintura]
			,case when nu.PruebasHidrostaticas = 1 
				then @SI else 'No' end [PruebasHidrostaticas]
			,case when nu.TieneDano = 1 
				then @SI else 'No' end [TieneDano]
	FROM NumeroUnico nu
	INNER JOIN RecepcionNumeroUnico r on r.NumeroUnicoID = nu.NumeroUnicoID
	INNER JOIN Proyecto p on p.ProyectoID = nu.ProyectoID
	INNER JOIN NumeroUnicoInventario nui on nu.NumeroUnicoID = nui.NumeroUnicoID	
	LEFT JOIN #TempNU temp on temp.NumeroUnicoID = nu.NumeroUnicoID
	LEFT JOIN ItemCode ic on ic.ItemCodeID = nu.ItemCodeID
	LEFT JOIN Colada c on c.ColadaID = nu.ColadaID
	LEFT JOIN Proveedor pr on pr.ProveedorID = nu.ProveedorID
	LEFT JOIN Fabricante f on f.FabricanteID = nu.FabricanteID
	LEFT JOIN TipoCorte tc on tc.TipoCorteID = nu.TipoCorte1ID
	LEFT JOIN TipoCorte tc2 on tc2.TipoCorteID = nu.TipoCorte2ID	
	WHERE r.RecepcionID = @RecepcionID) 	
	ORDER BY 3
	

END

