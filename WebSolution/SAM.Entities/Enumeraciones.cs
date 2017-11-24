namespace SAM.Entities
{
    /// <summary>
    /// Check constraint en BD
    /// </summary>
    public struct EstatusNumeroUnico
    {
        public const string RECHAZADO = "R";
        public const string CONDICIONADO = "C";
        public const string APROBADO = "A";
    }

    public struct FabAreas
    {
        public const string SHOP = "SHOP";
        public const string FIELD = "FIELD";
        public const string JACKET = "JACKET";
    }

    /// <summary>
    /// Check constraint en la BD
    /// </summary>
    public struct EstatusNumeroUnicoMovimiento
    {
        public const string CANCELADO = "C";
        public const string ACTIVO = "A";
    }

    /// <summary>
    /// Check constraint en la BD
    /// </summary>
    public struct TipoHoldSpool
    {
        public const string INGENIERIA = "ING";
        public const string CALIDAD = "CAL";
        public const string CONFINADO = "CON";
    }

    /// <summary>
    /// Sincronizada con la BD
    /// </summary>
    public enum TipoMaterialEnum
    {
        Tubo = 1,
        Accessorio = 2
    }

    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum TipoPruebaEnum
    {
        ReporteRT = 1,
        ReportePT = 2,
        Pwht = 3,
        Durezas = 4,
        RTPostTT = 5,
        PTPostTT = 6,
        Preheat = 7,
        ReporteUT = 8,
        InspeccionVisual = 9,
        ReportePMI = 10,
        Neumatica = 11

    }

    public enum TipoPruebaSpoolEnum
    {
        Hidrostatica = 1
    }

    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public struct CategoriaTipoPrueba
    {
        public const string PND = "PND";
        public const string TT = "TT";
    }

    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum ModuloEnum
    {
        WorkStatus = 1,
        Materiales = 2,
        Ingenieria = 3,
        Produccion = 4,
        Calidad = 5,
        Proyectos = 6,
        Catalogos = 7,
        Administracion = 8
    }

    /// <summary>
    /// Sincronizado con base de datos
    /// </summary>
    public enum TipoMovimientoEnum
    {
        Recepcion = 1,
        DespachoAccesorio = 2,
        Devolucion = 4,
        Merma = 5,
        SalidaPintura = 6,
        EntradaPintura = 7,
        MermaCorte = 9,
        SalidaOtrosProcesos = 10,
        EntradaOtrosProcesos = 11,
        SalidaSegmentacion = 12,
        EntradasSegmentacion = 13,
        DespachoACorte = 15,
        DespachoACorteCancelado = 16,
        PreparacionCorte = 17,
        Corte = 18,
        CambioItemCode = 19
    }

    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum TipoRechazoEnum
    {
        Sector = 1,
        Cuadrante = 2
    }

    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum TecnicaSoldadorEnum
    {
        Raiz = 1,
        Relleno = 2
    }

    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum TipoReporteDimensionalEnum
    {
        Dimensional = 1,
        Espesores = 2
    }

    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum UltimoProcesoEnum
    {
        Armado = 1,
        Soldado = 2,
        InspeccionVisual = 3,
        InspeccionDimensional = 4,
        RequisicionPND = 5,
        PND = 6,
        TT = 7,
        RequisicionPintura = 8,
        Pintura = 9,
        Certificacion = 10,
        Embarque = 11,
        PruebasporSpool=12
    }

    /// <summary>
    /// Sincronizada con BD
    /// </summary>
    public enum EstatusOrdenDeTrabajo
    {
        Activa = 1,
        Cancelada = 2
    }

    /// <summary>
    /// Se determina de manera lógica
    /// </summary>
    public enum EstatusDespachoOdt
    {
        SinDespacho = 1,
        Parcial = 2,
        Despachada = 3
    }

    /// <summary>
    /// Este estatus se determina de manera lógica por quien llene/utilice el objecto GrdMaterialesDespacho
    /// </summary>
    public enum EstatusMaterialDespacho
    {
        Despachado = 1,
        AccesorioCongelado = 2,
        AccesorioNoCongelado = 3,
        TuboConCorte = 4,
        TuboCongelado = 5,
        TuboNoCongelado = 6,
        SinOdt = 7
    }

    /// <summary>
    /// Para la exportación de los datos a Excel
    /// </summary>
    public enum TipoArchivoExcel
    {
        EstimacionJuntas = 1,
        EstimacionSpools = 2,
        SeguimientoJuntas = 3,
        SeguimientoSpools = 4,
        Destajos = 5,
        LstNumeroUnico = 6,
        LstItemCode = 7,
        ItemCodePeso = 8
    }

    /// <summary>
    /// Tipo de Reporte para el calculo de hoja
    /// </summary>
    public enum TipoReporte
    {
        InspeccionVisual = 0,
        ReporteTT = 1,
        ReportePND = 2,
        ReporteDimensional = 3,
        RequisicionPintura = 4,
        Requisicion = 5,
        ReportePintura = 6,
        ReporteDurezas = 7,
        ReporteEspesores=8,
        ReporteLiberacionDimensional= 9,
        ReportePT = 10,
        ReportePWHT = 11,
        ReporteRT = 12,
        ReportePreheat = 13,
        ReportePostPT =14,
        ReportePostRT =15,
        ReporteUT = 16,
        ReporteWps = 17,
        ReporteMTR = 18,
        ReporteMTRSoldadura = 19,
        ReportePMI = 20,
        ReporteSpoolPND = 21,
        RequisicionSpool = 22
    }

    /// <summary>
    /// Tipo de Reporte para el calculo de hoja
    /// </summary>
    public struct TipoReporteNombre
    {
        public const string InspeccionVisual = "InspeccionVisual";
        public const string ReporteDimensional ="ReporteDimensional";
         public const string RequisicionPintura ="RequisicionPintura";
         public const string Requisicion ="Requisicion";
         public const string ReportePintura ="ReportePintura";
         public const string ReporteDurezas ="ReporteDurezas";
         public const string ReporteEspesores ="ReporteEspesores";
         public const string ReporteLiberacionDimensional ="ReporteLiberacionDimensional";
         public const string ReportePT ="ReportePT";
         public const string ReportePWHT ="ReportePWHT";
         public const string ReporteRT ="ReporteRT";
         public const string ReportePreheat ="ReportePreheat";
         public const string ReportePostPT ="ReportePostPT";
         public const string ReportePostRT ="ReportePostRT";
         public const string ReporteUT = "ReporteUT";
         public const string ReportePMI = "ReportePMI";
         public const string ReporteWPS = "ReportesWPS";
         public const string ReporteMTR = "ReportesMTR";
         public const string ReporteMTRSoldadura = "ReportesMTRSoldadura";
         public const string ReportePruebaHidro = "ReportesPruebaHidrostatica";
    }


    public struct TipoJuntas
    {
        public const string TH = "TH";
        public const string TW = "TW";
        public const string BW = "BW";
    }

    /// <summary>
    /// estatus del armado
    /// </summary>
    public enum EstatusArmado
    {
        SinDespacho = 1,
        Despachado = 2,
        SinODT = 3,
        Armado = 4
    }

    /// <summary>
    /// Estatus de la soldadura
    /// </summary>
    public enum EstatusSoldadura
    {
        SinArmado = 1,
        SinDespacho = 2,
        SinODT = 3,
        Armado = 4,
        Soldado = 5
    }


    /// <summary>
    /// Se usa para el grid de listado de periodos de destajo
    /// </summary>
    public enum EstatusPeriodoDestajo
    {
        Pendiente = 1,
        Cerrado = 2,
        ListoParaCierre = 3
    }


    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum TipoReporteProyectoEnum
    {
        Armado = 1,
        Soldadura = 2,
        InspeccionVisual = 3,
        LiberacionDimensional = 4,
        Espesores = 5,
        Requisicion = 6,
        RT = 7,
        PT = 8,
        PWHT = 9,
        Pintura = 10,
        RequisicionPintura = 11,
        Durezas = 12,
        Embarque = 13,
        EtiquetaMaterial = 14,
        EtiquetaEmbarque = 15,
        Trazabilidad = 16,
        PMI = 17,
        MaterialesODT = 18,
        ResumenMaterialesODT = 19,
        CortesODT = 20,
        CaratulaODT = 21,
        JuntasODT = 22,
        CaratulaDetODT = 23,
        DetJuntasODT = 24,
        DetMaterialesODT = 25,
        CaratulaPorEstacionODT = 26,
        DetJuntasPorEstacionODT = 27,
        MaterialesPorEstacionODT = 28,
        ResumenMatPorEstacionODT = 29,
        CortesPorEstacionODT = 30,
        ResumenMatPorTallerODT = 31,
        CortesPorTallerODT = 32,
        RequisicionSpool = 33,
        Hidrostatica = 34,
        CaratulaODTEspecial = 35,
        CortesODTEspecial = 36,
        JuntasODTEspecial = 37,
        MaterialesODTEspecial = 38,
        ResumenMeterialesODTEspecial = 39,
        Transferencia = 40
    }

    public enum TipoAccionEmbarqueEnum
    {
        Preparacion = 1,
        Etiquetado = 2,
        Embarque = 3
    }

    public enum TipoPinturaEnum
    {
        SandBlast = 1,
        Primario = 2,
        Intermedio = 3,
        AcabadoVisual = 4,
        Adherencia = 5,
        PullOff = 6,
        Digitalizado = 7
    }

    public enum TipoAccionTransferenciaEnum
    {
        Preparacion = 1,
        Transferencia = 2
    }

    #region Enumeraciones para Pendientes
    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum CategoriaPendienteEnum
    {
        Calidad = 1,
        Ingenieria = 2,
        Materiales = 3,
        Produccion = 4,
        Otro = 5
    }

    /// <summary>
    /// Sincronizado con BD
    /// </summary>
    public enum TipoPendienteEnum
    {
        RegresoInvetarioDespachoCancelado = 1,
        SoldaduraSinArmado = 2,
        CorteDeAjuste = 3,
        PendienteManual = 4,
        CortePorRechazoDePrueba = 5
    }

    public struct EstatusPendiente
    {
        public const string Abierto = "A";
        public const string Resuelto = "R";
        public const string Cerrado = "C";
    }
    #endregion

    public struct DirectorioDossier
    {
        public const string Req_RT = @"\Requisiciones\RT";
        public const string Req_PT = @"\Requisiciones\PT";
        public const string Req_Pwht = @"\Requisiciones\PWHT";
        public const string Req_Durezas = @"\Requisiciones\Durezas";
        public const string Req_RTPostTT = @"\Requisiciones\RTPostTT";
        public const string Req_PTPostTT = @"\Requisiciones\PTPostTT";
        public const string Req_Preheat = @"\Requisiciones\Preheat";
        public const string Req_UT = @"\Requisiciones\UT";
        public const string Req_Pintura = @"\Requisiciones\Pintura";
        public const string Req_Hidrostatica = @"\Requisiciones\Hidrostatica";
        public const string Reportes_InspeccionVisual = @"\Reportes\InspeccionVisual";
        public const string Reportes_LiberacionDimensional = @"\Reportes\LiberacionDimensional";
        public const string Reportes_Espesores = @"\Reportes\Espesores";
        public const string Reportes_Montaje = @"\Reportes\Montaje";
        public const string Reportes_RT = @"\Reportes\RT";
        public const string Reportes_PT = @"\Reportes\PT";
        public const string Reportes_Pwht = @"\Reportes\PWHT";
        public const string Reportes_Durezas = @"\Reportes\Durezas";
        public const string Reportes_RTPostTT = @"\Reportes\RTPostTT";
        public const string Reportes_PTPostTT = @"\Reportes\PTPostTT";
        public const string Reportes_Preheat = @"\Reportes\Preheat";
        public const string Reportes_UT = @"\Reportes\UT";
        public const string Reportes_PMI = @"\Reportes\PMI";
        public const string Reportes_Pintura_Sandblast = @"\Reportes\Pintura\Sandblast";
        public const string Reportes_Pintura_Primarios = @"\Reportes\Pintura\Primarios";
        public const string Reportes_Pintura_Intermedios = @"\Reportes\Pintura\Intermedios";
        public const string Reportes_Pintura_AcabadoVisual = @"\Reportes\Pintura\AcabadoVisual";
        public const string Reportes_Pintura_Adherencia = @"\Reportes\Pintura\Adherencia";
        public const string Reportes_Pintura_PullOff = @"\Reportes\Pintura\PullOff";
        public const string Reportes_WPS = @"\Reportes\WPS";
        public const string Reportes_MTR = @"\Reportes\MTR";
        public const string Reportes_Embarque = @"\Reportes\Embarque";
        public const string Reportes_Trazabilidad = @"\Reportes\Trazabilidad";
        public const string Reportes_MTRSoldadura = @"\Reportes\MTRSoldadura";
        public const string Reportes_Dibujo = @"\Reportes\Dibujo";
        public const string Reportes_Hidrostatica = @"Reportes\Hidrostatica";
        public const string Reportes_PruebasHidrostaticas = @"Reportes\Pruebas Hidrostaticas";

        public static readonly string[] NodosLeafRutas = new string[]
        {
            Reportes_MTR,
            Reportes_WPS,
            Req_RT,
            Req_PT, 
            Req_Pwht, 
            Req_Durezas, 
            Req_RTPostTT, 
            Req_PTPostTT, 
            Req_Preheat, 
            Req_UT,
            Req_Pintura,
            Reportes_InspeccionVisual,
            Reportes_LiberacionDimensional ,
            Reportes_Espesores,
            Reportes_Montaje,
            Reportes_RT,
            Reportes_PT,
            Reportes_Pwht,
            Reportes_Durezas,
            Reportes_RTPostTT,
            Reportes_PTPostTT,
            Reportes_Preheat,
            Reportes_UT,
            Reportes_Pintura_Sandblast ,
            Reportes_Pintura_Primarios ,
            Reportes_Pintura_Intermedios,
            Reportes_Pintura_AcabadoVisual,
            Reportes_Pintura_Adherencia,
            Reportes_Pintura_PullOff,
            Reportes_Embarque,
            Reportes_Trazabilidad,
            Reportes_MTRSoldadura,
            Reportes_Dibujo,
            Reportes_PMI,
            Req_Hidrostatica,
            Reportes_Hidrostatica
        };
    }

    public enum AccionesHomologacion
    {
        Eliminar = 1,
        Igual = 2,
        Similar = 3,
        Nuevo = 4
    }

    public enum ResumenSpoolsImportados
    {
        Nuevos = 0,
        SinODT = 1,
        ConODT = 2,
        Descartados = 3
    }

    public enum TipoNumeroControlEnum
    {
        AProcesar = 0,
        Procesado = 1,
        ConProceso = 2,
        FechaInvalida = 3,
        NoCumple = 4 // no cumple con condiciones necesarias para ser procesado
    }
}
