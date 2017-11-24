SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE ImportaMateriales
(
	@ProyectoIdSamViejo int,
	@ProyectoId int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @AceroDesconocidoID int
	
	select @AceroDesconocidoID = AceroID
	from SAM..Acero
	where Nomenclatura = 'Desconocido'

	-- Coladas necesarias
	insert into SAM..Colada
	(
		AceroID,
		ProyectoID,
		NumeroColada,
		NumeroCertificado,
		HoldCalidad,
		FechaModificacion
	)
	select	isnull(( select top 1 AceroID
			  from SAM..Acero sac
			  where sac.Nomenclatura in
			  (
				select a.nomenclatura collate SQL_Latin1_General_CP1_CI_AS
				from SAM_OLD..cat_aceros a
				where a.acero = acero
			  )
			),@AceroDesconocidoID) [AceroID],
			@ProyectoId [ProyectoID],
			cast(col.nombre as nvarchar(10)),
			cast(col.certificado as nvarchar(20)),
			0 [HoldCalidad],
			getdate() [FechaModificacion]
	from SAM_OLD..cat_coladas col
	where col.proyecto = @ProyectoIdSamViejo

	declare @RecepcionTbl table
	(
		RowID int identity(1,1),
		RecepcionID int,
		Generacion int
	)

	insert into SAM..Recepcion
	(
		ProyectoID,
		TransportistaID,
		FechaRecepcion, 
		FechaModificacion
	)
	output inserted.RecepcionID, 0 into @RecepcionTbl
	select	@ProyectoId [ProyectoID],
			1 [TransportistaID],
			nu.fecha,
			GETDATE()
	from SAM_OLD..doc_numeros_unicos nu
	where proyecto = @ProyectoIdSamViejo
	order by generacion

	
	UPDATE @RecepcionTbl
	set Generacion = t.Generacion
	from @RecepcionTbl r
	inner join
	(
		select	ROW_NUMBER() over(order by generacion) [RowID],
				generacion [Generacion]
		from SAM_OLD..doc_numeros_unicos nu
		where proyecto = @ProyectoIdSamViejo
	) t on r.RowID = t.RowID
	
	
	declare @Recepciones int
	declare @Row int
	select @Recepciones = COUNT(1) from @RecepcionTbl
	set @Row = 1


	declare @Generacion int
	declare @RecepcionID int
	
	declare @NumsUnicos table
	(
		RowID int identity(1,1),
		NumeroUnicoID int,
		Codigo nvarchar(20)
	)

	declare @MinNumUnico int
	declare @MaxNumUnico int

	while (	@Row <= @Recepciones )
	begin
	
		select	@Generacion = Generacion,
				@RecepcionID = RecepcionID
		from @RecepcionTbl
		where RowID = @Row
		
		insert into SAM..NumeroUnico
		(
			ProyectoID,
			ItemCodeID,
			ColadaID,
			Codigo,
			Diametro1,
			Diametro2
		)
		output inserted.NumeroUnicoID, inserted.Codigo into @NumsUnicos
		select	@ProyectoId [ProyectoID],
				sic.ItemCodeID,
				max(col.ColadaID),
				nud.numero_unico,
				nud.diametro1,
				nud.diametro2
		from SAM_OLD..doc_numeros_unicos_detalle nud
		inner join SAM_OLD..cat_coladas ccol on nud.colada = ccol.colada
		inner join SAM..ItemCode sic on nud.itemcode collate SQL_Latin1_General_CP1_CI_AS = sic.Codigo
		inner join SAM..Colada col on ccol.nombre collate SQL_Latin1_General_CP1_CI_AS = col.NumeroColada
		where	sic.ProyectoID = @ProyectoId
				and nud.generacion = @Generacion
				and ccol.proyecto = @ProyectoIdSamViejo
		group by sic.ItemCodeID, nud.numero_unico, nud.diametro1, nud.diametro2
				
		insert into SAM..NumeroUnicoInventario
		(
			NumeroUnicoID,
			ProyectoID,
			CantidadRecibida,
			CantidadDanada,
			InventarioFisico,
			InventarioBuenEstado,
			InventarioCongelado,
			InventarioDisponibleCruce,
			FechaModificacion
		)
		select	nus.NumeroUnicoID,
				@ProyectoId [ProyectoID],
				nud.cantidad_longitud,
				0,
				nud.cantidad_longitud,
				nud.cantidad_longitud,
				0,
				nud.cantidad_longitud,
				GETDATE()
		from @NumsUnicos nus
		inner join SAM_OLD..doc_numeros_unicos_detalle nud
		on nus.Codigo = nud.numero_unico collate SQL_Latin1_General_CP1_CI_AS 
		where nud.generacion =@Generacion
		
		insert into SAM..NumeroUnicoSegmento
		(
			NumeroUnicoID,
			ProyectoID,
			Segmento,
			CantidadDanada,
			InventarioFisico,
			InventarioBuenEstado,
			InventarioCongelado,
			InventarioDisponibleCruce
		)
		select	nu.NumeroUnicoID,
				@ProyectoId,
				'A',
				nui.CantidadDanada,
				nui.InventarioFisico,
				nui.InventarioBuenEstado,
				nui.InventarioCongelado,
				nui.InventarioDisponibleCruce
		from @NumsUnicos nus
		inner join NumeroUnico nu on nus.NumeroUnicoID = nu.NumeroUnicoID
		inner join NumeroUnicoInventario nui on nu.NumeroUnicoID = nui.NumeroUnicoID
		inner join ItemCode ic on nu.ItemCodeID = ic.ItemCodeID
		where ic.TipoMaterialID = 1 -- tubos
		
		insert into SAM..NumeroUnicoMovimiento
		(
			NumeroUnicoID,
			ProyectoID,
			TipoMovimientoID,
			Cantidad,
			Segmento,
			FechaMovimiento,
			Estatus
		)
		select	nu.NumeroUnicoID,
				@ProyectoId,
				1,
				nui.InventarioFisico,
				case when ic.TipoMaterialID = 1 then 'A' else null end,
				GETDATE(),
				'A'
		from @NumsUnicos nus
		inner join NumeroUnico nu on nus.NumeroUnicoID = nu.NumeroUnicoID
		inner join NumeroUnicoInventario nui on nu.NumeroUnicoID = nui.NumeroUnicoID
		inner join ItemCode ic on nu.ItemCodeID = ic.ItemCodeID
	
		insert into SAM..RecepcionNumeroUnico
		(
			RecepcionID,
			NumeroUnicoID,
			NumeroUnicoMovimientoID
		)
		select	@RecepcionID,
				nus.NumeroUnicoID,
				(
					select top 1 NumeroUnicoMovimientoID
					from NumeroUnicoMovimiento
					where	NumeroUnicoID = nus.NumeroUnicoID
							and TipoMovimientoID = 1
				)
		from @NumsUnicos nus

	
		set @Row = @Row + 1
		
		delete from @NumsUnicos	
	
	end
		
END
GO
