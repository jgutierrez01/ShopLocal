IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerConsultaDeNumerosUnicos]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ObtenerConsultaDeNumerosUnicos]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerConsultaDeNumerosUnicos
	Funcion:	Obtiene la cuanta de las juntas para una serie de spools
	Parametros:	@ProyectoID int
	Autor:		IHM
	Modificado:	29/10/2010, 06/09/2011 PEGV
*****************************************************************************************/
CREATE PROCEDURE [dbo].[ObtenerConsultaDeNumerosUnicos]
(
	 @ProyectoID int,
	 @NumeroColada nvarchar(10),
	 @CodigoItemCode nvarchar(50),
	 @CodigoNumeroUnicoInicial nvarchar(20),
	 @CodigoNumeroUnicoFinal nvarchar(20)
)
AS
BEGIN

	SET NOCOUNT ON; 

	select	nu.NumeroUnicoID,
			nu.Codigo,
			nu.Diametro1,
			nu.Diametro2,
			nu.Rack,
			nu.Factura,
			nu.PartidaFactura,
			nu.OrdenDeCompra,
			nu.PartidaOrdenDeCompra,
			nu.Cedula,
			c.FabricanteID,
			nu.ProveedorID,
			nu.MarcadoAsme,
			nu.MarcadoGolpe,
			nu.MarcadoPintura,
			nu.PruebasHidrostaticas,
			nu.Estatus, 
			nu.TipoCorte1ID,
			nu.TipoCorte2ID,
			ic.ItemCodeID,
			ic.DescripcionEspanol,
			ic.Codigo [CodigoItemCode],
			ic.TipoMaterialID,
			c.NumeroColada,
			c.NumeroCertificado,
			c.AceroID,
			nui.CantidadDanada,
			nui.CantidadRecibida,
			nui.InventarioBuenEstado,
			nui.InventarioCongelado,
			nui.InventarioDisponibleCruce,
			nui.InventarioFisico,		
			nui.InventarioTransferenciaCorte,		
			r.TransportistaID,		
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (11)
						and num.Estatus = 'A'
			) [TotalEntradaOtrosProcesos],	
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (6)
						and num.Estatus = 'A'
			) [TotalSalidasTemporales],
			(
			select isnull(SUM(num.Cantidad),0) 
				from NumeroUnicoMovimiento num				
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (7) --Entrada de Pintura
						and num.Estatus = 'A'	
			) [TotalEntradasTemporales],
			(
				select isnull(SUM(d.Cantidad),0)
				from Despacho d 
				where d.NumeroUnicoID = nu.NumeroUnicoID
				and d.Cancelado = 0
			)	[TotalDespachado],
			(
				select isnull(SUM(d.Cantidad),0)
				from Despacho d 
				where d.NumeroUnicoID = nu.NumeroUnicoID
				and d.Cancelado = 0 and d.EsEquivalente = 1
			)	[TotalDespachadoParaICE],
			(				
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (18)
						and num.Estatus = 'A'
			) [TotalCorte],
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID in (5,9)
						and num.Estatus = 'A'
			) [TotalMerma],
			(
				select isnull(SUM(num.Cantidad),0)
				from NumeroUnicoMovimiento num
				where	num.NumeroUnicoID = nu.NumeroUnicoID
						and num.TipoMovimientoID not in (2,18,5,9,6,19)
						and num.Estatus = 'A'
						and exists
						(
							select 1
							from TipoMovimiento tp
							where tp.EsEntrada = 0
								  and tp.EsTransferenciaProcesos = 0
								  and tp.TipoMovimientoID = num.TipoMovimientoID
						)
			) [TotalOtrasSalidas],
			r.FechaRecepcion,
			nu.Observaciones
					
	from NumeroUnico nu
	inner join NumeroUnicoInventario nui on nu.NumeroUnicoID = nui.NumeroUnicoID
	inner join RecepcionNumeroUnico rnu on rnu.NumeroUnicoID = nu.NumeroUnicoID
	inner join Recepcion r on rnu.RecepcionID = r.RecepcionID
	inner join ProyectoConfiguracion pc on pc.ProyectoID = nu.proyectoID
	left join Colada c on nu.ColadaID = c.ColadaID
	left join ItemCode ic on nu.ItemCodeID = ic.ItemCodeID
	where nu.ProyectoID = @ProyectoID
		  and (ISNULL(@NumeroColada,'') = '' or c.NumeroColada = @NumeroColada)
		  and (ISNULL(@CodigoItemCode,'') = '' or ic.Codigo = @CodigoItemCode)
		  and (ISNULL(@CodigoNumeroUnicoInicial,'') = '' or CAST(Replace(nu.Codigo,pc.PrefijoNumeroUnico+'-','') AS INT) >= CAST(Replace(@CodigoNumeroUnicoInicial,pc.PrefijoNumeroUnico+'-','')AS INT))
		  and (ISNULL(@CodigoNumeroUnicoFinal,'') = '' or CAST(Replace(nu.Codigo,pc.PrefijoNumeroUnico+'-','') AS INT) <= CAST(Replace(@CodigoNumeroUnicoFinal,pc.PrefijoNumeroUnico+'-','')AS INT))
	

	SET NOCOUNT OFF;

END
GO

