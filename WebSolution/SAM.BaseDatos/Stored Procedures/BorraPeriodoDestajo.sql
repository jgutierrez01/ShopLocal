IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BorraPeriodoDestajo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[BorraPeriodoDestajo]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		BorraPeriodoDestajo
	Funcion:	Elimina un periodo de destajo y todas sus relaciones
	Parametros:	@PeriodoDestajoID	INT
	Autor:		IHM
	Modificado:	23/11/2010
*****************************************************************************************/
CREATE PROCEDURE [dbo].[BorraPeriodoDestajo]
(
	@PeriodoDestajoID	INT
)
AS
BEGIN

	SET NOCOUNT ON;
	
	delete from DestajoSoldadorDetalle
	where DestajoSoldadorID in
	(
		select DestajoSoldadorID
		from DestajoSoldador
		where PeriodoDestajoID = @PeriodoDestajoID
	)

	delete from DestajoTuberoDetalle
	where DestajoTuberoID in
	(
		select DestajoTuberoID
		from DestajoTubero
		where PeriodoDestajoID = @PeriodoDestajoID
	)
	
	delete from DestajoSoldador where PeriodoDestajoID = @PeriodoDestajoID
	delete from DestajoTubero where PeriodoDestajoID = @PeriodoDestajoID
	
	delete from PeriodoDestajo where PeriodoDestajoID = @PeriodoDestajoID

	select CAST(1 as bit) [Borrado]

	SET NOCOUNT OFF;

END
GO



