IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EtiquetaMaterial]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].EtiquetaMaterial
GO
-- =============================================
-- Author:		Lilian Mendoza
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
	
	SELECT
			 p.ProyectoID
			,p.Nombre
			,nu.NumeroUnicoID
			,nu.Codigo
			,nu.Diametro1
			,nu.Diametro2
			,nu.Cedula
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
	LEFT JOIN TipoCorte tc2 on tc.TipoCorteID = nu.TipoCorte2ID
	WHERE r.RecepcionID = @RecepcionID

END

GO

/*
exec EtiquetaMaterial 29566,1

select * from recepcion
select * from numerounico where TieneDano=1
select * from recepcionnumerounico where numerounicoid=21191
*/