/****** Object:  StoredProcedure [dbo].[PlanchaSoldadura]    Script Date: 02/26/2014 10:45:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PlanchaSoldadura]
	-- Add the parameters for the stored procedure here
	@NombreProyecto		   nvarchar(50),
	@NumeroControl		   nvarchar(50),
	@EtiquetaJunta		   nvarchar(10),
	@SoldaduraFecha		   nvarchar(10),
	@SoldaduraFechaReporte nvarchar(10),
	@SoldaduraTaller       nvarchar(150), 

	@SoldaduraProcesoRaiz  nvarchar(50),
	@SoldaduraWPSRaiz      nvarchar(50),
	@SoldaduraSoldadorRaiz nvarchar(20),
	@ColadaRaiz			   nvarchar(50),

	@SoldaduraProcesoRelleno nvarchar(50),
	@SoldaduraWPSRelleno   nvarchar(50),
	@SoldaduraSoldadorRelleno nvarchar(20),
	@ColadaRelleno			   nvarchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	declare @ultimoProcesoID int = (select UltimoProcesoID from UltimoProceso where Nombre = 'Soldado')

	declare @ordenTrabajoSpoolID int = (select OrdenTrabajoSpoolID from OrdenTrabajoSpool where NumeroControl = @NumeroControl)
	declare @juntaWorkstatusID int = (select JuntaWorkstatusID from JuntaWorkstatus where OrdenTrabajoSpoolID = @ordenTrabajoSpoolID and EtiquetaJunta = @EtiquetaJunta)

	declare @patioID int = (select PatioID from Proyecto where Nombre = @NombreProyecto)
	declare @tallerID int = (select TallerID from Taller where PatioID = @patioID and Nombre = @SoldaduraTaller)

	declare @procesoRellenoID int = (select ProcesoRellenoID from ProcesoRelleno where Codigo = @SoldaduraProcesoRelleno)
	declare @procesoRaizID int = (select ProcesoRaizID from ProcesoRaiz where Codigo = @SoldaduraProcesoRaiz)

	declare @wpsRaizID int = (select WpsID from Wps where Nombre = @SoldaduraWPSRaiz and ProcesoRaizID = @procesoRaizID and ProcesoRellenoID = @procesoRellenoID)
	declare @wpsRellenoID int = (select WpsID from Wps where Nombre = @SoldaduraWPSRelleno and ProcesoRaizID = @procesoRaizID and ProcesoRellenoID = @procesoRellenoID)

	declare @soldadorRaizID int = (select SoldadorID from Soldador where Codigo = @SoldaduraSoldadorRaiz)
	declare @soldadorRellenoID int = (select SoldadorID from Soldador where Codigo = @SoldaduraSoldadorRelleno)

	declare @coladaRaizID int = (select ConsumibleID from Consumible where Codigo = @ColadaRaiz)
	declare @coladaRellenoID int = (select ConsumibleID from Consumible where Codigo = @ColadaRelleno)

	declare @tecnicaRaizID int = (select TecnicaSoldadorID from TecnicaSoldador where Nombre = 'Raiz')
	declare @tecnicaRellenoID int = (select TecnicaSoldadorID from TecnicaSoldador where Nombre = 'Relleno')


	IF(@juntaWorkstatusID IS NULL)
	BEGIN
	
		declare @spoolID int = (select SpoolID from OrdenTrabajoSpool where OrdenTrabajoSpoolID = @ordenTrabajoSpoolID)
		declare @juntaSpoolID int = (select JuntaSpoolID from JuntaSpool where SpoolID = @spoolID and Etiqueta = @EtiquetaJunta)
		
		INSERT INTO [SAM].[dbo].[JuntaWorkstatus]
           ([OrdenTrabajoSpoolID]
           ,[JuntaSpoolID]
           ,[EtiquetaJunta]
           ,[ArmadoAprobado]
           ,[SoldaduraAprobada]
           ,[InspeccionVisualAprobada]
           ,[VersionJunta]
           ,[JuntaFinal]
           ,[UltimoProcesoID]
           ,[ArmadoPagado]
           ,[SoldaduraPagada])
     VALUES
           (@ordenTrabajoSpoolID,
           @juntaSpoolID,
           @EtiquetaJunta,
           0,
           0,
           0,
           1,
           1,
           @ultimoProcesoID,
           0,
           0)
           
        set @juntaWorkstatusID = SCOPE_IDENTITY()
	END


	-- INSERTAMOS JUNTA SOLDADURA
	INSERT INTO JuntaSoldadura
           ([JuntaWorkstatusID]
           ,[FechaSoldadura]
           ,[FechaReporte]
           ,[TallerID]
           ,[ProcesoRellenoID]
           ,[ProcesoRaizID]
           ,[WpsID]
           ,[WpsRellenoID])
     VALUES
           (@juntaWorkstatusID,
		    @SoldaduraFecha,
			@SoldaduraFechaReporte,
			@tallerID,
			@procesoRellenoID,
			@procesoRaizID,
			@wpsRaizID,
			@wpsRellenoID)

	declare @juntaSoldaduraID int = SCOPE_IDENTITY()


	-- INSERTAMOS JUNTA SOLDADURA DETALLE PARA PROCESO RAIZ
	INSERT INTO JuntaSoldaduraDetalle
           ([JuntaSoldaduraID]
           ,[SoldadorID]
           ,[ConsumibleID]
           ,[TecnicaSoldadorID])
     VALUES
           (@juntaSoldaduraID,
            @soldadorRaizID,
            @coladaRaizID,
			@tecnicaRaizID)

	-- INSERTAMOS JUNTA SOLDADURA DETALLE PARA PROCESO RELLENO
	INSERT INTO JuntaSoldaduraDetalle
           ([JuntaSoldaduraID]
           ,[SoldadorID]
           ,[ConsumibleID]
           ,[TecnicaSoldadorID])
     VALUES
           (@juntaSoldaduraID,
            @soldadorRellenoID,
            @coladaRellenoID,
			@tecnicaRellenoID)
	
	
	-- ACTUALIZAMOS JUNTA WORKSTATUS
	UPDATE JuntaWorkstatus
	SET UltimoProcesoID = @ultimoProcesoID,
		SoldaduraAprobada = 1,
		JuntaSoldaduraID = @juntaSoldaduraID
	WHERE JuntaWorkstatusID = @juntaWorkstatusID
END

GO


