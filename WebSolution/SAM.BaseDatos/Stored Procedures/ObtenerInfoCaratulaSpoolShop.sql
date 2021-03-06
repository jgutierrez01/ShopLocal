
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ObtenerInfoCaratulaSpoolShop') AND type in (N'P', N'PC'))
        DROP PROCEDURE [dbo].[ObtenerSeguimientoDeSpools]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/****************************************************************************************
	Nombre:		ObtenerInfoCaratulaSpoolShop
	Funcion:	Obtiene la info para la caratula del los spools enviados
	Parametros:	@SpoolIDs nvarchar(max)
	Autor:		SCB
	Modificado:	
*****************************************************************************************/
ALTER PROCEDURE [dbo].[ObtenerInfoCaratulaSpoolShop]
(
	@SpoolID NVARCHAR(MAX),
	@ProyectoID INT = NULL
)
AS 
BEGIN

	SET NOCOUNT ON;

	DECLARE @SumPeqs INT,
			@SumJuntas INT,
			@CantMaterialesPendientes INT,
			@CantDespachosPendientes INT,
			@CantArmadosPendientes INT,
			@CantSoldadurasPendientes INT,
			@CantInsVisualPendientes INT,			
			@CantPWHTPendientes INT,
			@CantRequierePWHT INT, 
			@PendientesPND INT, 
			@PendienteDimensional BIT,
			@ReportePruebaHidrostatica VARCHAR(100),
			@ReportePruebaNeumatica VARCHAR(100),
			@PorcentajePND INT

	CREATE TABLE #Spool
	(
		SpoolID INT NOT NULL, 
		ProyectoID INT, 		
		Isometrico VARCHAR(50),
		Hold VARCHAR(10), 
		FechaHold DATETIME NULL, 
		RevisionCliente VARCHAR(10),
		Especificacion VARCHAR(100),
		PorcentajePND INT,
		Juntas INT NULL,
		Peso DECIMAL NULL,
		Area DECIMAL NULL, 
		PDI DECIMAL NULL,
		PEQS DECIMAL NULL,
		LiberacionDimensional VARCHAR(50), 
		RequierePWHT VARCHAR(50),
		RequierePruebaHidrostatica VARCHAR(10)  NULL,
		ReportePruebaHidrostatica VARCHAR(50) NULL,
		NumeroEmbarque VARCHAR(50),
		FechaEmbarque DATETIME,
		Sistema VARCHAR(50),
		Color VARCHAR(50),
		FechaPrimario DATETIME,
		ReportePrimario VARCHAR(50),
		FechaIntermedio DATETIME,
		ReporteIntermedio VARCHAR(50),
		FechaPullOff DATETIME,
		ReportePullOff VARCHAR(50),
		FechaAdherencia DATETIME,
		ReporteAdherencia VARCHAR(50),
		FechaAcabadoVisual DATETIME,
		ReporteAcabadoVisual VARCHAR(50),
		Materiales BIT,
	    Despachos BIT,
	    Armado BIT,
	    Soldadura BIT, 
	    InspeccionVisual BIT,
	    TieneLiberacionDimensional BIT,
	    PWHT INT,
	    PND INT, -- No requiere PND =0, Requiere no tienen todas las juntas =1, Requiere y las juntas ya lo tienen = 2
	    Pintura INT,
	    OkMateriales BIT,
	    TienePintura BIT,
	    OkCalidad BIT, 
	    OkPreparacionEmbarque BIT
	)
	
	CREATE TABLE #JuntaSpool(
		JuntaSpoolID INT, 
		SpoolID INT,		
		EtiquetaJunta NVARCHAR(50),
		TipoJuntaID INT,
		TipoJunta VARCHAR(50),
		Diametro DECIMAL(7, 4),
		Cedula NVARCHAR(10),
		FechaArmado DATETIME,
		JuntaArmadoID INT,
		FechaSoldadura DATETIME,
		WPS VARCHAR(50),
		SoldadorRaiz VARCHAR(50),
		SoldadorRelleno VARCHAR(50),
		FechaInspeccionVisual DATETIME,
		ResultadoInspeccionVisual VARCHAR(50),
		NumeroReporteInspeccionVisual VARCHAR(50),
		NumeroUnico1 INT,
		NumeroUnico2 INT,
		NumeroUnicoOne VARCHAR(50),
		NumeroUnicoTwo VARCHAR(50),		
		RequierePWHT VARCHAR(50),
		ReportePWHT VARCHAR(50),
		JuntaWorkstatusID INT, 
		RequierePruebaNeumatica VARCHAR(50),
		ReportePruebaNeumatica VARCHAR(100),
		Peqs DECIMAL

	)

	CREATE TABLE #ReqRepPNDTT
	(
		SpoolId INT,
		JuntaWorkstatusId INT,
		RequiId INT NULL,		
		NumeroReporte VARCHAR(50) NULL,
		Aprobado BIT NULL,
		TipoPruebaID INT	
		
	)

	CREATE TABLE #Materiales
	(
		NumeroUnicoID INT,
		SpoolID INT,	
		Etiqueta VARCHAR(50),		
		NumeroUnico VARCHAR(50),
		ColadaID INT,
		Colada VARCHAR(50),
		Certificado VARCHAR(50),
		ItemCode VARCHAR(150),
		Diametro1 DECIMAL(7,4),
		Diametro2 DECIMAL(7,4),
		Descripcion VARCHAR(500),
		Cantidad INT,
		FabricanteId INT,
		Fabricante VARCHAR(150),
		Pedimento VARCHAR(50),
		Notas VARCHAR(500),
		MaterialSpoolID INT 
	)				

	SELECT @CantMaterialesPendientes = COUNT(ms.materialspoolID) 
		FROM MaterialSpool ms
			INNER JOIN Spool s 
			ON ms.SpoolID = s.SpoolID	
		left join OrdenTrabajoMaterial otm
			on ms.MaterialSpoolID = otm.MaterialSpoolID
		LEFT JOIN ItemCode ic
				ON ic.ItemCodeID = ms.ItemCodeID							
		LEFT JOIN NumeroUnico nu
			ON nu.ItemCodeID =	 ms.ItemCodeID	
			and otm.NumeroUnicoDespachadoID = nu.NumeroUnicoID
		left join Despacho d
			on d.DespachoID = otm.DespachoID
		LEFT JOIN Colada col 
			ON col.ColadaID = nu.ColadaID			
		WHERE s.SpoolID = @SpoolID
			AND (col.NumeroCertificado IN ('','ET20-2013','FALTA CERTIFICADO')
			OR col.NumeroCertificado IS NULL)


	SELECT @CantDespachosPendientes = COUNT(MaterialesSinDespacho.MaterialSpoolID) FROM (
		SELECT	ms.MaterialSpoolID FROM Spool s
			INNER JOIN JuntaSpool js 
				ON js.SpoolID = s.SpoolID
			LEFT JOIN JuntaWorkstatus jw 
				ON jw.JuntaSpoolID = js.JuntaSpoolID
			LEFT JOIN OrdenTrabajoSpool ots 
				ON ots.SpoolID = s.SpoolID
			LEFT JOIN MaterialSpool ms 
				ON ms.SpoolID = s.SpoolID	
				AND ms.Etiqueta = js.EtiquetaMaterial1
			LEFT JOIN Despacho ds 
				ON ds.Cancelado= 0 
				AND ds.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID 
				AND ds.MaterialSpoolID = ms.MaterialSpoolID
			WHERE s.SpoolID = @SpoolID
				AND js.FabAreaID = 1
				AND ds.NumeroUnicoID IS NULL
	
		UNION ALL
	
		SELECT	ms.MaterialSpoolID FROM Spool s
			INNER JOIN JuntaSpool js 
				ON js.SpoolID = s.SpoolID
			LEFT JOIN JuntaWorkstatus jw 
				ON jw.JuntaSpoolID = js.JuntaSpoolID
			LEFT JOIN OrdenTrabajoSpool ots 
				ON ots.SpoolID = s.SpoolID
			LEFT JOIN MaterialSpool ms 
				ON ms.SpoolID = s.SpoolID	
				AND ms.Etiqueta = js.EtiquetaMaterial2
			LEFT JOIN Despacho ds 
				ON ds.Cancelado= 0 
				AND ds.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID 
				AND ds.MaterialSpoolID = ms.MaterialSpoolID
			WHERE s.SpoolID = @SpoolID
				AND js.FabAreaID=1
				AND ds.NumeroUnicoID IS NULL ) MaterialesSinDespacho


	SELECT @CantArmadosPendientes = COUNT(DISTINCT js.JuntaSpoolID)
		FROM Spool s
			INNER JOIN JuntaSpool js 
				ON js.SpoolID = s.SpoolID
			LEFT JOIN OrdenTrabajoSpool ots 
				ON ots.SpoolID = s.SpoolID		
			LEFT JOIN JuntaWorkstatus jw 
				ON jw.JuntaSpoolID = js.JuntaSpoolID
			LEFT JOIN JuntaArmado ja 
				ON ja.JuntaArmadoID = jw.JuntaArmadoID		
			WHERE s.SpoolID = @SpoolID
			AND js.FabAreaID = 1
			AND  (ja.FechaArmado IS NULL	
					OR ja.TuberoID IS NULL )
					
	SELECT @CantSoldadurasPendientes = Count(DISTINCT JuntaSpoolID) FROM (
		SELECT	s.SpoolID, js.JuntaSpoolID, ots.NumeroControl, Spool = s.Nombre
				,Junta = ISNULL (jw.EtiquetaJunta,js.Etiqueta) 
				,jsol.FechaSoldadura	
				,WPS = (SELECT Wps.Nombre FROM Wps WHERE Wps.WpsID=  jsol.WpsID)
				,SoldRaiz = (SELECT Soldador.Codigo FROM Soldador WHERE Soldador.SoldadorID= jsd1.SoldadorID)
				,ProcRaiz  = (SELECT ProcesoRaiz.Nombre FROM ProcesoRaiz WHERE ProcesoRaiz.ProcesoRaizID = jsol.ProcesoRaizID )
				,SoldRelleno = (SELECT Soldador.Codigo FROM Soldador WHERE Soldador.SoldadorID = jsd2.SoldadorID )
				,ProcRelleno = (SELECT ProcesoRelleno.Nombre FROM ProcesoRelleno WHERE ProcesoRelleno.ProcesoRellenoID = jsol.ProcesoRellenoID )
		FROM Spool	s
			INNER JOIN JuntaSpool js ON js.SpoolID = s.SpoolID
			LEFT JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID
			LEFT JOIN JuntaWorkstatus jw ON jw.JuntaSpoolID = js.JuntaSpoolID
			LEFT JOIN JuntaSoldadura jsol ON jsol.JuntaSoldaduraID = jw.JuntaSoldaduraID
			LEFT JOIN JuntaSoldaduraDetalle jsd1 ON jsd1.JuntaSoldaduraID = jw.JuntaSoldaduraID AND jsd1.TecnicaSoldadorID = 1
			LEFT JOIN JuntaSoldaduraDetalle jsd2 ON jsd2.JuntaSoldaduraID = jw.JuntaSoldaduraID AND jsd2.TecnicaSoldadorID = 2
		WHERE	s.SpoolID = @SpoolID
			AND js.FabAreaID = 1
			AND js.TipoJuntaID <> 2
		 )qry 	
		WHERE qry.FechaSoldadura IS NULL 
			OR qry.WPS IS NULL 
			OR qry.SoldRaiz IS NULL 
			OR qry.ProcRaiz IS NULL 
			OR qry.SoldRelleno IS NULL 
			OR qry.ProcRelleno IS NULL 

	SELECT @CantInsVisualPendientes =	COUNT(DISTINCT js.JuntaSpoolID)	FROM Spool s
			INNER JOIN JuntaSpool js ON js.SpoolID = s.SpoolID
			LEFT JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID
			LEFT JOIN JuntaWorkstatus jw ON jw.JuntaSpoolID = js.JuntaSpoolID
			LEFT JOIN JuntaSoldadura jsol ON jsol.JuntaSoldaduraID = jw.JuntaSoldaduraID
			LEFT JOIN JuntaInspeccionVisual jiv ON jiv.JuntaInspeccionVisualID = jw.JuntaInspeccionVisualID
			LEFT JOIN InspeccionVisual iv ON iv.InspeccionVisualID = jiv.InspeccionVisualID		
			where s.SpoolID = @SpoolID
			AND js.FabAreaID = 1
			AND (jiv.Aprobado IS NULL OR (jiv.FechaInspeccion IS NULL OR iv.NumeroReporte IS NULL))

	SELECT @PendienteDimensional = CASE WHEN @SpoolID IN(
		SELECT s.SpoolID
		FROM Spool s
			LEFT JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID
			LEFT JOIN WorkstatusSpool ws ON ws.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID
			LEFT JOIN ReporteDimensionalDetalle rdd ON rdd.WorkstatusSpoolID= ws.WorkstatusSpoolID 
			LEFT JOIN ReporteDimensional rd ON rd.ReporteDimensionalID = rdd.ReporteDimensionalID
			where s.SpoolID = @SpoolID
				AND (rd.ReporteDimensionalID IS NULL 
						OR rdd.Aprobado = 0
						OR (rdd.FechaLiberacion is null OR rd.NumeroReporte is null))
			) THEN 0 ELSE 1 END

					
	SELECT @CantRequierePWHT =  COUNT(DISTINCT js.JuntaSpoolID)	FROM Spool s
			INNER JOIN JuntaSpool js ON js.SpoolID = s.SpoolID
			LEFT JOIN JuntaWorkstatus jw ON jw.JuntaSpoolID = js.JuntaSpoolID
			LEFT JOIN OrdenTrabajoSpool ots ON ots.SpoolID = s.SpoolID			
					where	s.SpoolID = @SpoolID
					AND js.FabAreaID = 1	
					AND js.RequierePwht = 1


	IF(@CantRequierePWHT > 0)
	BEGIN			
		SELECT @CantPWHTPendientes = COUNT(DISTINCT js.JuntaSpoolID) FROM Spool s
			INNER JOIN JuntaSpool js ON js.SpoolID = s.SpoolID
			LEFT JOIN JuntaWorkstatus jw ON jw.JuntaSpoolID = js.JuntaSpoolID
			LEFT JOIN OrdenTrabajoSpool ots ON ots.SpoolID=s.SpoolID
			LEFT JOIN (			SELECT 		jreq.JuntaWorkstatusID
									,req.FechaRequisicion
									,req.NumeroRequisicion
									,jrpnd.FechaPrueba
									,jrpnd.Aprobado
									,rpnd.NumeroReporte
									FROM JuntaRequisicion jreq
										INNER JOIN Requisicion req ON req.RequisicionID =jreq.RequisicionID 
										LEFT JOIN JuntaReportePnd jrpnd ON jrpnd.JuntaWorkstatusID =jreq.JuntaWorkstatusID
										LEFT JOIN ReportePnd rpnd ON rpnd.ReportePndID =jrpnd.ReportePndID AND rpnd.TipoPruebaID = req.TipoPruebaID
										where req.TipoPruebaID = (select TipoPruebaID from TipoPrueba where Nombre LIKE 'PWHT')
										) req ON req.JuntaWorkstatusID = jw.JuntaWorkstatusID
					where	s.SpoolID = @SpoolID
					AND js.FabAreaID = 1					
					AND req.Aprobado = 1		
		END
		ELSE
			SELECT @CantPWHTPendientes = 0


	SELECT @ReportePruebaHidrostatica = rspnd.NumeroReporte
	FROM OrdenTrabajoSpool ots		
	LEFT JOIN WorkstatusSpool ws ON ots.OrdenTrabajoSpoolID = ws.OrdenTrabajoSpoolID
	LEFT JOIN SpoolReportePnd srpnd ON srpnd.WorkstatusSpoolID = ws.WorkstatusSpoolID
	LEFT JOIN ReporteSpoolPnd rspnd on rspnd.ReporteSpoolPndID = srpnd.ReporteSpoolPndID
	LEFT JOIN (SELECT srpnd1.WorkstatusSpoolID, MAX(rspnd1.FechaReporte ) AS FechaReporte, MAX(rspnd1.FechaModificacion) AS FechaModificacion	 FROM SpoolReportePnd srpnd1 
									LEFT JOIN ReporteSpoolPnd rspnd1 ON rspnd1.ReporteSpoolPndID = srpnd1.ReporteSpoolPndID
									where  rspnd1.TipoPruebaSpoolID in (1) 	and srpnd1.Aprobado = 1 AND srpnd1.SpoolReportePndID IS NOT NULL
											GROUP BY srpnd1.WorkstatusSpoolID) AS pnd ON ws.WorkstatusSpoolID = pnd.WorkstatusSpoolID 									
	WHERE srpnd.SpoolReportePndID IS NOT NULL
	and  ots.SpoolID = @SpoolID
	AND rspnd.TipoPruebaSpoolID = 1


	SELECT @PorcentajePND = PorcentajePND from #Spool
	WHERE SpoolID = @SpoolID

	EXEC ObtenerEstatusPNDPorSpool @SpoolID, @ProyectoID, @PendientesPND out, @PorcentajePND

	INSERT INTO #Spool
		SELECT	s.SpoolID, 
				s.ProyectoID,
				s.Dibujo,
				CASE WHEN sh.TieneHoldCalidad = 1 THEN 'Si'
					WHEN sh.TieneHoldIngenieria = 1 THEN 'Si'
					ELSE 'No' END,
				sh.FechaModificacion,
				s.RevisionCliente,
				s.Especificacion,
				s.PorcentajePnd,
				0,--juntas #
				s.Peso,
				s.Area,
				s.Pdis,
				0,--# Peqs						
				CASE WHEN rd.NumeroReporte IS NOT NULL THEN rd.NumeroReporte
					ELSE '' END,			 -- lib dim
				CASE WHEN s.RequierePWHT IS NULL THEN ''
					WHEN s.RequierePWHT =1 THEN 'Si'
					WHEN s.RequierePWHT = 0 THEN 'No'
					END,
				CASE WHEN s.RequierePruebaHidrostatica IS NULL THEN ''
					 WHEN s.RequierePruebaHidrostatica = 1 THEN 'Si'
					 WHEN s.RequierePruebaHidrostatica = 0 THEN 'No'									
				END,	
				CASE WHEN @ReportePruebaHidrostatica IS NULL THEN '' ELSE @ReportePruebaHidrostatica END,--reporte Hidro
				CASE WHEN e.NumeroEmbarque is not null THEN e.NumeroEmbarque 
					ELSE '' END,		   		 
				e.FechaEmbarque,
				s.SistemaPintura,
				s.ColorPintura,
				ps.FechaPrimarios,
				ps.ReportePrimarios,
				ps.FechaIntermedios,
				ps.ReporteIntermedios,
				ps.FechaPullOff,
				ps.ReportePullOff,
				ps.FechaAdherencia,
				ps.ReporteAdherencia,
				ps.FechaAcabadoVisual,
				ps.ReporteAcabadoVisual,
				CASE WHEN @CantMaterialesPendientes > 0 THEN 0 ELSE 1 END,-- materiales
				CASE WHEN @CantDespachosPendientes > 0 THEN 0 ELSE 1 END,-- despachos
				CASE WHEN @CantArmadosPendientes > 0 THEN 0 ELSE 1 END,-- armado
				CASE WHEN @CantSoldadurasPendientes > 0 THEN 0 ELSE 1 END,-- soldadura 
				CASE WHEN @CantInsVisualPendientes > 0 THEN 0 ELSE 1 END,-- ins visual
				@PendienteDimensional, -- lib dimensional
				CASE WHEN @CantRequierePWHT = 0 THEN 0 
					 WHEN @CantPWHTPendientes > 0 THEN 1 
					 ELSE 2 END,-- pwht  0 = no requiere ninguna junta PWHT, 1 = juntas con PWHT pendiente, 2 = PWHT OK
				@PendientesPND,
				CASE WHEN s.SistemaPintura  IN ('NOPAINT' ,'NOPAINT', 'N/A','NO','UNDEFINED') THEN 0 -- 0 no aplica 
					 WHEN ps.FechaPrimarios IS NOT NULL AND ps.FechaIntermedios IS NOT NULL AND ps.FechaAcabadoVisual IS NOT NULL									
					 THEN 2		-- 2 Tiene Pintura			
					ELSE 1      -- 1 no tiene pintura
				END,
				CASE WHEN wss.FechaLiberacionMateriales IS NOT NULL AND wss.UsuarioLiberacionMateriales IS NOT NULL THEN 1  -- OK calidad
				ELSE 0 END,	-- ok materiales 
				CASE WHEN wss.TienePintura IS NULL THEN 0 ELSE wss.TienePintura END,-- ok pintura
				CASE WHEN wss.FechaLiberacionCalidad IS NOT NULL AND wss.UsuarioLiberacionCalidad IS NOT NULL THEN 1  -- OK calidad
				ELSE 0 END, -- ok calidad	
				CASE WHEN wss.FechaPreparacion IS NOT NULL THEN 1
				ELSE 0 END --embarque			
			FROM Spool  s
			LEFT JOIN SpoolHold sh
				ON s.SpoolID = sh.SpoolID
			LEFT JOIN OrdenTrabajoSpool ots
				ON ots.SpoolID = s.spoolID
			LEFT JOIN WorkstatusSpool wss
				ON wss.OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID		
			LEFT JOIN ReporteDimensionalDetalle rdd
				ON rdd.WorkstatusSpoolID = wss.WorkstatusSpoolID				
			LEFT JOIN ReporteDimensional rd
				ON rdd.ReporteDimensionalID = rd.ReporteDimensionalID	
				AND rdd.Aprobado = 1 --solo el reporte aprobado
				AND rd.TipoReporteDimensionalID = 1 --Réporte tipo Liberacion Dimensional
			LEFT JOIN EmbarqueSpool es
				ON es.WorkstatusSpoolID = wss.WorkstatusSpoolID
			LEFT JOIN Embarque e
				ON e.EmbarqueID = es.EmbarqueID	
			LEFT JOIN PinturaSpool ps 
				ON wss.WorkstatusSpoolID = ps.WorkstatusSpoolID
			WHERE s.ProyectoID = @ProyectoID
				AND s.SpoolID = @SpoolID		
	

	INSERT INTO #JuntaSpool	
		SELECT  js.JuntaSpoolID,
				js.SpoolID,
				CASE WHEN jws.EtiquetaJunta IS NULL THEN js.Etiqueta
					ELSE jws.EtiquetaJunta END AS EtiquetaJunta,
				js.TipoJuntaID,
				tj.Codigo,
				js.Diametro,
				js.Cedula,
				ja.FechaArmado, 
				ja.JuntaArmadoID,
				jSold.FechaSoldadura,
				Wps.Nombre,
				'',--sold raiz
				'',--sold relleno
				null,--fechains visual
				'',--result insp visual
				'',--# rep ins visual
				ja.NumeroUnico1ID,
				ja.NumeroUnico2ID,
				'',-- num uni one
				'',-- num unico two				
				CASE WHEN js.RequierePwht IS NULL THEN '' 
					  WHEN js.RequierePwht = 0 THEN 'No'
					  WHEN js.RequierePwht = 1 THEN 'Si'
					  END,
				'',--reportePWHT
				jws.JuntaWorkstatusID,
				CASE WHEN js.RequierePruebaNeumatica = 0 THEN 'No'
					 WHEN js.RequierePruebaNeumatica = 1 THEN 'Si'
				ELSE '' END,
				CASE WHEN @ReportePruebaNeumatica IS NULL THEN '' 
					  ELSE @ReportePruebaNeumatica  END,
				js.Peqs
		FROM JuntaSpool js 
		LEFT JOIN JuntaWorkstatus jws
			ON jws.JuntaSpoolID = js.JuntaSpoolID
		LEFT JOIN JuntaArmado ja
			ON ja.JuntaWorkstatusID = jws.JuntaWorkstatusID
		LEFT JOIN JuntaSoldadura jSold
			ON jsold.JuntaWorkstatusID = jws.JuntaWorkstatusID
		LEFT JOIN Wps 
			ON Wps.WpsID = jSold.WpsID
		LEFT JOIN TipoJunta tj
			ON tj.TipoJuntaID = js.TipoJuntaID	
		WHERE js.SpoolID IN (SELECT SpoolID FROM #Spool)			
		AND js.FabAreaID = (SELECT FabAreaID FROM FabArea where Codigo LIKE 'SHOP')		
		order BY js.Etiqueta 

	SELECT @SumPeqs = SUM(Peqs),
			@SumJuntas = count(JuntaSpoolID)
		FROM #JuntaSpool js

	UPDATE #Spool 
		SET PEQS = @SumPeqs,
			Juntas = @SumJuntas
		
	UPDATE #JuntaSpool 
		SET NumeroUnicoOne = nu.Codigo
		FROM NumeroUnico nu		
		 WHERE nu.NumeroUnicoID = #JuntaSpool.NumeroUnico1 
	
	UPDATE #JuntaSpool 
		SET NumeroUnicoTwo = nu.Codigo
		FROM NumeroUnico nu		
		 WHERE nu.NumeroUnicoID = #JuntaSpool.NumeroUnico2 

	UPDATE #JuntaSpool 
	SET SoldadorRaiz = sol.Codigo
	FROM JuntaWorkstatus jws
					INNER JOIN #juntaspool js
					ON js.JuntaSpoolID = jws.JuntaSpoolID
					LEFT JOIN JuntaSoldadura jsold
						ON jsold.JuntaSoldaduraID = jws.JuntaSoldaduraID	
						LEFT JOIN JuntaSoldaduraDetalle jsd
				ON jsd.JuntaSoldaduraID = jsold.JuntaSoldaduraID
				AND jsd.TecnicaSoldadorID = 1
				LEFT JOIN soldador sol
				ON sol.SoldadorID = jsd.SoldadorID		

	UPDATE #JuntaSpool 
	SET SoldadorRelleno = sol.Codigo
		FROM JuntaWorkstatus jws
					INNER JOIN #juntaspool js
					ON js.JuntaSpoolID = jws.JuntaSpoolID
					LEFT JOIN JuntaSoldadura jsold
						ON jsold.JuntaSoldaduraID = jws.JuntaSoldaduraID	
						LEFT JOIN JuntaSoldaduraDetalle jsd
				ON jsd.JuntaSoldaduraID = jsold.JuntaSoldaduraID
				AND jsd.TecnicaSoldadorID = 2
				LEFT JOIN soldador sol
				ON sol.SoldadorID = jsd.SoldadorID						
						

	UPDATE #JuntaSpool 
		SET FechaInspeccionVisual = jiv.FechaInspeccion,
			ResultadoInspeccionVisual = CASE WHEN jiv.Aprobado = 0 THEN 'RECHAZADO'
											WHEN jiv.Aprobado = 1 THEN 'APROBADO'
											ELSE ''
											END,
			NumeroReporteInspeccionVisual = iv.NumeroReporte
		FROM JuntaInspeccionVisual jiv
		LEFT JOIN JuntaWorkstatus jws
			ON jiv.JuntaWorkstatusID = jws.JuntaWorkstatusID 	
			AND jiv.JuntaInspeccionVisualID = jws.JuntaInspeccionVisualID
		LEFT JOIN InspeccionVisual iv 
			ON iv.InspeccionVisualID = jiv.InspeccionVisualID
		WHERE iv.ProyectoID = ProyectoID
		AND jws.JuntaSpoolID = #JuntaSpool.JuntaSpoolID


	INSERT INTO #ReqRepPNDTT
	SELECT @SpoolID,
			sqry.JuntaWorkstatusID,
			r.RequisicionId,
			rpnd.numeroReporte,
			jrpnd.Aprobado,
			rpnd.TipoPruebaId
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				INNER join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'Reporte de PT' and Categoria like 'PND')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		INNER JOIN JuntareportePND jrpnd
			ON jrpnd.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrpnd.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		INNER JOIN ReportePND rpnd 
			ON rpnd.ReportePNDId = jrpnd.ReportePNDId			
	where  sqry.JuntaWorkstatusID in (Select JuntaWorkStatusId from #JuntaSpool)

	INSERT INTO #ReqRepPNDTT
	SELECT @SpoolID,
			sqry.JuntaWorkstatusID,
			r.RequisicionId,
			rpnd.numeroReporte,
			jrpnd.Aprobado,
			rpnd.TipoPruebaId
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				INNER join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'Reporte de RT' and Categoria like 'PND')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		INNER JOIN JuntareportePND jrpnd
			ON jrpnd.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrpnd.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		INNER JOIN ReportePND rpnd 
			ON rpnd.ReportePNDId = jrpnd.ReportePNDId			
	where  sqry.JuntaWorkstatusID in (Select JuntaWorkStatusId from #JuntaSpool)

	INSERT INTO #ReqRepPNDTT
	SELECT @SpoolID,
			sqry.JuntaWorkstatusID,
			r.RequisicionId,
			rtt.numeroReporte,
			jrtt.Aprobado,
			rtt.TipoPruebaId
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				INNER join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'PWHT' and Categoria like 'TT')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		INNER JOIN JuntaReporteTt jrTT
			ON jrtt.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrtt.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		INNER JOIN ReporteTt rtt
			ON rtt.ReporteTtID = jrtt.ReportettId			
	where  sqry.JuntaWorkstatusID in (Select JuntaWorkStatusId from #JuntaSpool)	

	INSERT INTO #ReqRepPNDTT
	SELECT @SpoolID,
			sqry.JuntaWorkstatusID,
			r.RequisicionId,
			rtt.numeroReporte,
			jrtt.Aprobado,
			rtt.TipoPruebaId
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				INNER join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'Durezas' and Categoria like 'TT')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		INNER JOIN JuntareporteTt jrtt
			ON jrtt.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrtt.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		INNER JOIN ReporteTt rtt 
			ON rtt.ReporteTtId = jrtt.ReporteTtId			
	where  sqry.JuntaWorkstatusID in (Select JuntaWorkStatusId from #JuntaSpool)	

	INSERT INTO #ReqRepPNDTT
	SELECT @SpoolID,
			sqry.JuntaWorkstatusID,
			r.RequisicionId,
			rpnd.numeroReporte,
			jrpnd.Aprobado,
			rpnd.TipoPruebaId
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				INNER join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'Reporte PMI' and Categoria like 'PND')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		INNER JOIN JuntareportePND jrpnd
			ON jrpnd.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrpnd.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		INNER JOIN ReportePND rpnd 
			ON rpnd.ReportePNDId = jrpnd.ReportePNDId			
	where  sqry.JuntaWorkstatusID in (Select JuntaWorkStatusId from #JuntaSpool)

	INSERT INTO #ReqRepPNDTT
	SELECT @SpoolID,
			sqry.JuntaWorkstatusID,
			r.RequisicionId,
			rpnd.numeroReporte,
			jrpnd.Aprobado,
			rpnd.TipoPruebaId
		FROM Requisicion r
		INNER JOIN (		
			SELECT jr.JuntaWorkstatusID, MAX (r.RequisicionID) as RequisicionID, MAX (jr.JuntaRequisicionID) as JuntaRequisicionID from JuntaRequisicion jr
				INNER join requisicion r
				on r.requisicionid = jr.requisicionid
				where r.tipopruebaid in ( SELECT TIpoPruebaID  FROM TipoPrueba WHERE Nombre like 'Neumática' and Categoria like 'PND')				
				group by jr.JuntaWorkstatusid) sqry
		ON sqry.RequisicionId = r.RequisicionId		
		INNER JOIN JuntareportePND jrpnd
			ON jrpnd.JuntaRequisicionID = sqry.JuntaRequisicionID
			AND jrpnd.JuntaWorkstatusID = sqry.JuntaWorkstatusID
		INNER JOIN ReportePND rpnd 
			ON rpnd.ReportePNDId = jrpnd.ReportePNDId			
	where  sqry.JuntaWorkstatusID in (Select JuntaWorkStatusId from #JuntaSpool)


INSERT INTO #Materiales
		SELECT nu.NumeroUnicoId, 
			ms.SpoolID,	
			ms.Etiqueta,
			nu.Codigo,
			nu.ColadaID,
			c.NumeroColada,
			c.NumeroCertificado,
			ic.Codigo,
			ms.Diametro1,
			ms.Diametro2,
			ic.DescripcionEspanol,
			ms.Cantidad,
			c.FabricanteID,
			f.Nombre, 
			nu.OrdenDeCompra,
			nu.Observaciones, 
			ms.MaterialSpoolID
			FROM MaterialSpool ms
			INNER JOIN Spool s 		
			on s.SpoolID = ms.SpoolID	
			left join OrdenTrabajoMaterial otm
			on ms.MaterialSpoolID = otm.MaterialSpoolID
			LEFT JOIN ItemCode ic
				ON ic.ItemCodeID = ms.ItemCodeID							
			LEFT JOIN NumeroUnico nu
				ON nu.ItemCodeID =	 ms.ItemCodeID	
			and otm.NumeroUnicoDespachadoID = nu.NumeroUnicoID
			left join Despacho d
			on d.DespachoID = otm.DespachoID
			LEFT JOIN RecepcionNumeroUnico rnu
			ON rnu.NumeroUnicoID = nu.NumeroUnicoID
			LEFT JOIN Recepcion r
			ON r.RecepcionID = rnu.RecepcionID
			LEFT JOIN Colada c
				ON c.ColadaID = nu.ColadaID
				and c.ProyectoID= s.ProyectoId
				LEFT JOIN Fabricante f
				ON f.FabricanteID = c.FabricanteID
				Where s.SpoolID = @SpoolID
				order by ms.MaterialSpoolID



	SELECT * FROM #Spool
	SELECT * FROM #JuntaSpool 
	SELECT * FROM #Materiales
	SELECT * FROM #ReqRepPNDTT

	DROP TABLE #Spool
	DROP TABLE #JuntaSpool
	DROP TABLE #Materiales	
	DROP TABLE #ReqRepPNDTT

SET NOCOUNT OFF;

END


GO

