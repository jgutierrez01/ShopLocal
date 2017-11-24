using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdSeguimientoJunta
    {
        
        #region General

        [DataMember]
        public int ? JuntaWorkstatusID { get; set; }

        [DataMember]
        public string FabArea { get; set; }

        [DataMember]
        public decimal? KgTeoricos { get; set; }

        [DataMember]
        public decimal? Peqs { get; set; }

        [DataMember]
        public string FamAcero1 { get; set; }

        [DataMember]
        public string FamAcero2 { get; set; }

        [DataMember]
        public string Proyecto { get; set; }

        [DataMember]
        public string OrdenDeTrabajo { get; set; }

        [DataMember]
        public string NumeroDeControl { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public string Junta { get; set; }

        [DataMember]
        public string TipoJunta { get; set; }

        [DataMember]
        public decimal? Diametro { get; set; }

        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public decimal? Espesor { get; set; }

        [DataMember]
        public string Localizacion { get; set; }

        [DataMember]
        public string UltimoProceso { get; set; }

        [DataMember]
        public string  TieneHold { get; set; }

        [DataMember]
        public string RequierePWHT { get; set; }

        #endregion

        #region Armado
        [DataMember]
        public DateTime? ArmadoFecha { get; set; }

        [DataMember]
        public DateTime? ArmadoFechaReporte { get; set; }

        [DataMember]
        public string ArmadoTaller { get; set; }

        [DataMember]
        public string ArmadoTubero { get; set; }

        [DataMember]
        public string ArmadoNumeroUnico1 { get; set; }

        [DataMember]
        public string ArmadoNumeroUnico2 { get; set; }
        
        [DataMember]
        public string ArmadoEtiquetaMaterial1 { get; set; }
        
        [DataMember]
        public string ArmadoEtiquetaMaterial2 { get; set; }
        
        [DataMember]
        public string ArmadoSpool1 { get; set; }
        
        [DataMember]
        public string ArmadoSpool2 { get; set; }

        [DataMember]
        public string AreaTrabajoTubero { get; set; }

        #endregion
        
        #region Soldadura

        [DataMember]
        public List<GrdSegJuntaDetSoldadura> SoldaduraDetalle { get; set; }

        [DataMember]
        public string SoldaduraMaterialBase2 { get; set; }

        [DataMember]
        public string SoldaduraMaterialBase1 { get; set; }

        [DataMember]
        public DateTime? SoldaduraFecha { get; set; }

        [DataMember]
        public DateTime? SoldaduraFechaReporte { get; set; }

        [DataMember]
        public string SoldaduraTaller { get; set; }

        [DataMember]
        public string SoldaduraWPS { get; set; }
        
        [DataMember]
        public string SoldaduraWPSRelleno { get; set; }

        [DataMember]
        public string SoldaduraProcesoRelleno { get; set; }

        [DataMember]
        public string SoldaduraSoldadorRelleno { get; set; }

        [DataMember]
        public string SoldaduraConsumiblesRelleno { get; set; }

        [DataMember]
        public string SoldaduraProcesoRaiz { get; set; }
        
        [DataMember]
        public string SoldaduraSoldadorRaiz { get; set; }
        
        [DataMember]
        public string ItemCode1 { get; set; }

        [DataMember]
        public string ItemCode2 { get; set; }
        
        [DataMember]
        public string DescItemCode1 { get; set; }
        
        [DataMember]
        public string DescItemCode2 { get; set; }

        [DataMember]
        public string AreaTrabajoRaiz { get; set; }

        [DataMember]
        public string AreaTrabajoRelleno { get; set; }
        



        #endregion

        #region InspeccionVisual

        [DataMember]
        public DateTime? InspeccionVisualFecha { get; set; }

        [DataMember]
        public DateTime? InspeccionVisualFechaReporte { get; set; }

        [DataMember]
        public string InspeccionVisualNumeroReporte { get; set; }

        [DataMember]
        public string InspeccionVisualObservaciones{ get; set; }

        [DataMember]
        public int? InspeccionVisualHoja { get; set; }

        [DataMember]
        public string InspeccionVisualResultado { get; set; }

        [DataMember]
        public string InspeccionVisualDefecto { get; set; }

        [DataMember]
        public string InspeccionVisualInspector { get; set; }

        #endregion

        #region 	InspeccionDimensional

        [DataMember]
        public DateTime? InspeccionDimensionalFecha { get; set; }

        [DataMember]
        public DateTime? InspeccionDimensionalFechaReporte { get; set; }

        [DataMember]
        public string InspeccionDimensionalNumeroReporte { get; set; }

        [DataMember]
        public int? InspeccionDimensionalHoja { get; set; }

        [DataMember]
        public string  InspeccionDimensionalResultado { get; set; }

        [DataMember]
        public string InspeccionDimensionalObservaciones { get; set; }

        [DataMember]
        public string InspeccionDimensionalInspector { get; set; }

        #endregion
        
        #region	InspeccionEspesores

        [DataMember]
        public DateTime? InspeccionEspesoresFecha { get; set; }

        [DataMember]
        public DateTime? InspeccionEspesoresFechaReporte { get; set; }

        [DataMember]
        public string InspeccionEspesoresNumeroReporte { get; set; }

        [DataMember]
        public int? InspeccionEspesoresHoja { get; set; }

        [DataMember]
        public string InspeccionEspesoresResultado { get; set; }

        [DataMember]
        public string InspeccionEspesoresObservaciones { get; set; }
        
        #endregion
        
        #region 	PruebaRT
        [DataMember]
        public DateTime? PruebaRTFechaRequisicion { get; set; }

        [DataMember]
        public string PruebaRTNumeroRequisicion { get; set; }

        [DataMember]
        public string PruebaRTCodigoRequisicion { get; set; }

        [DataMember]
        public DateTime? PruebaRTFechaPrueba { get; set; }

        [DataMember]
        public DateTime? PruebaRTFechaReporte { get; set; }

        [DataMember]
        public string PruebaRTNumeroReporte { get; set; }

        [DataMember]
        public int? PruebaRTHoja { get; set; }

        [DataMember]
        public string PruebaRTResultado { get; set; }

        [DataMember]
        public string PruebaRTDefecto { get; set; }

        [DataMember]
        public string PruebaRTObservacionesRequisicion{ get; set; }

        [DataMember]
        public string PruebaRTObservacionesReporte{ get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDSector> PruebaRTPndSector { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDCuad> PruebaRTPndCuad { get; set; }
        #endregion

        #region 	PruebaPMI
        [DataMember]
        public DateTime? PruebaPMIFechaRequisicion { get; set; }

        [DataMember]
        public string PruebaPMINumeroRequisicion { get; set; }

        [DataMember]
        public string PruebaPMICodigoRequisicion { get; set; }

        [DataMember]
        public DateTime? PruebaPMIFechaPrueba { get; set; }

        [DataMember]
        public DateTime? PruebaPMIFechaReporte { get; set; }

        [DataMember]
        public string PruebaPMINumeroReporte { get; set; }

        [DataMember]
        public int? PruebaPMIHoja { get; set; }

        [DataMember]
        public string PruebaPMIResultado { get; set; }

        [DataMember]
        public string PruebaPMIDefecto { get; set; }

        [DataMember]
        public string PruebaPMIObservacionesRequisicion { get; set; }

        [DataMember]
        public string PruebaPMIObservacionesReporte { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDSector> PruebaPMIPndSector { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDCuad> PruebaPMIPndCuad { get; set; }
        #endregion

        #region	PruebaPT

        [DataMember]
        public DateTime? PruebaPTFechaRequisicion { get; set; }

        [DataMember]
        public string PruebaPTNumeroRequisicion { get; set; }

        [DataMember]
        public string PruebaPTCodigoRequisicion { get; set; }

        [DataMember]
        public DateTime? PruebaPTFechaPrueba { get; set; }

        [DataMember]
        public DateTime? PruebaPTFechaReporte { get; set; }

        [DataMember]
        public string PruebaPTNumeroReporte { get; set; }

        [DataMember]
        public int? PruebaPTHoja { get; set; }

        [DataMember]
        public string PruebaPTResultado { get; set; }

        [DataMember]
        public string PruebaPTDefecto { get; set; }

        [DataMember]
        public string PruebaPTObservacionesRequisicion { get; set; }

        [DataMember]
        public string PruebaPTObservacionesReporte { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDSector> PruebaPTPndSector { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDCuad> PruebaPTPndCuad { get; set; }

        #endregion
        
        #region	TratamientoPWHT
        
        [DataMember]
        public DateTime? TratamientoPWHTFechaRequisicion { get; set; }

        [DataMember]
        public string TratamientoPWHTNumeroRequisicion { get; set; }

        [DataMember]
        public string TratamientoPWHTCodigoRequisicion { get; set; }

        [DataMember]
        public DateTime? TratamientoPWHTFechaTratamiento { get; set; }

        [DataMember]
        public DateTime? TratamientoPWHTFechaReporte { get; set; }

        [DataMember]
        public string TratamientoPWHTNumeroReporte { get; set; }

        [DataMember]
        public int? TratamientoPWHTHoja { get; set; }

        [DataMember]
        public string TratamientoPWHTGrafica { get; set; }

        [DataMember]
        public string TratamientoPWHTResultado { get; set; }

        [DataMember]
        public string TratamientoPWHTObservacionesReporte { get; set; }

        [DataMember]
        public string TratamientoPWHTObservacionesRequisicion { get; set; }

        #endregion

        #region	TratamientoDurezas

        [DataMember]
        public DateTime? TratamientoDurezasFechaRequisicion { get; set; }

        [DataMember]
        public string TratamientoDurezasNumeroRequisicion { get; set; }

        [DataMember]
        public string TratamientoDurezasCodigoRequisicion { get; set; }

        [DataMember]
        public DateTime? TratamientoDurezasFechaTratamiento { get; set; }

        [DataMember]
        public DateTime? TratamientoDurezasFechaReporte { get; set; }

        [DataMember]
        public string TratamientoDurezasNumeroReporte { get; set; }

        [DataMember]
        public int? TratamientoDurezasHoja { get; set; }

        [DataMember]
        public string TratamientoDurezasGrafica { get; set; }

        [DataMember]
        public string TratamientoDurezasResultado { get; set; }

        [DataMember]
        public string TratamientoDurezasObservacionesReporte { get; set; }

        [DataMember]
        public string TratamientoDurezasObservacionesRequisicion { get; set; }

        
        
        #endregion
        
        #region 	PruebaRTPostTT

        [DataMember]
        public DateTime? PruebaRTPostTTFechaRequisicion { get; set; }

        [DataMember]
        public string PruebaRTPostTTNumeroRequisicion { get; set; }

        [DataMember]
        public string PruebaRTPostTTCodigoRequisicion { get; set; }

        [DataMember]
        public DateTime? PruebaRTPostTTFechaPrueba { get; set; }

        [DataMember]
        public DateTime? PruebaRTPostTTFechaReporte { get; set; }

        [DataMember]
        public string PruebaRTPostTTNumeroReporte { get; set; }

        [DataMember]
        public int? PruebaRTPostTTHoja { get; set; }

        [DataMember]
        public string PruebaRTPostTTResultado { get; set; }

        [DataMember]
        public string PruebaRTPostTTDefecto { get; set; }

        [DataMember]
        public string PruebaRTPostTTObservacionesRequisicion { get; set; }

        [DataMember]
        public string PruebaRTPostTTObservacionesReporte { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDSector> PruebaRTPostTTPndSector { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDCuad> PruebaRTPostTTPndCuad { get; set; }

        #endregion

        #region	PruebaPTPostTT

        [DataMember]
        public DateTime? PruebaPTPostTTFechaRequisicion { get; set; }
        
        [DataMember]
        public string PruebaPTPostTTNumeroRequisicion { get; set; }
        
        [DataMember]
        public string PruebaPTPostTTCodigoRequisicion { get; set; }
        
        [DataMember]
        public DateTime? PruebaPTPostTTFechaPrueba { get; set; }
        
        [DataMember]
        public DateTime? PruebaPTPostTTFechaReporte { get; set; }
        
        [DataMember]
        public string PruebaPTPostTTNumeroReporte { get; set; }
        
        [DataMember]
        public int? PruebaPTPostTTHoja { get; set; }
        
        [DataMember]
        public string PruebaPTPostTTResultado { get; set; }
        
        [DataMember]
        public string PruebaPTPostTTDefecto { get; set; }

        [DataMember]
        public string PruebaPTPostTTObservacionesRequisicion { get; set; }

        [DataMember]
        public string PruebaPTPostTTObservacionesReporte { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDSector> PruebaPTPostTTPndSector { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDCuad> PruebaPTPostTTPndCuad { get; set; }
        
        #endregion
        
        #region	TratamientoPreheat
        
        [DataMember]
        public DateTime? TratamientoPreheatFechaRequisicion { get; set; }
        
        [DataMember]
        public string TratamientoPreheatNumeroRequisicion { get; set; }
        
        [DataMember]
        public string TratamientoPreheatCodigoRequisicion { get; set; }
        
        [DataMember]
        public DateTime? TratamientoPreheatFechaTratamiento { get; set; }
        
        [DataMember]
        public DateTime? TratamientoPreheatFechaReporte { get; set; }
        
        [DataMember]
        public string TratamientoPreheatNumeroReporte { get; set; }
        
        [DataMember]
        public int? TratamientoPreheatHoja { get; set; }
        
        [DataMember]
        public string TratamientoPreheatGrafica { get; set; }
        
        [DataMember]
        public string TratamientoPreheatResultado { get; set; }

        [DataMember]
        public string TratamientoPreheatObservacionesReporte { get; set; }

        [DataMember]
        public string TratamientoPreheatObservacionesRequisicion { get; set; }

        #endregion

        #region	PruebaUT
        
        [DataMember]
        public DateTime? PruebaUTFechaRequisicion { get; set; }
        
        [DataMember]
        public string PruebaUTNumeroRequisicion { get; set; }
        
        [DataMember]
        public string PruebaUTCodigoRequisicion { get; set; }
        
        [DataMember]
        public DateTime? PruebaUTFechaPrueba { get; set; }
        
        [DataMember]
        public DateTime? PruebaUTFechaReporte { get; set; }
        
        [DataMember]
        public string PruebaUTNumeroReporte { get; set; }
        
        [DataMember]
        public int? PruebaUTHoja { get; set; }
        
        [DataMember]
        public string PruebaUTResultado { get; set; }
        
        [DataMember]
        public string PruebaUTDefecto { get; set; }

        [DataMember]
        public string PruebaUTObservacionesRequisicion { get; set; }

        [DataMember]
        public string PruebaUTObservacionesReporte { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDSector> PruebaUTPndSector { get; set; }

        [DataMember]
        public List<GrdSegJuntaDetPNDCuad> PruebaUTPndCuad { get; set; }
        
        #endregion

        #region 	Pintura
        
        [DataMember]
        public DateTime? PinturaFechaRequisicion { get; set; }
        
        [DataMember]
        public string PinturaNumeroRequisicion { get; set; }
        
        [DataMember]
        public string PinturaSistema { get; set; }
        
        [DataMember]
        public string PinturaColor { get; set; }
        
        [DataMember]
        public string PinturaCodigo { get; set; }
        
        [DataMember]
        public DateTime? PinturaFechaSandBlast { get; set; }
        
        [DataMember]
        public string PinturaReporteSandBlast { get; set; }
        
        [DataMember]
        public DateTime? PinturaFechaPrimarios { get; set; }
        
        [DataMember]
        public string PinturaReportePrimarios { get; set; }
        
        [DataMember]
        public DateTime? PinturaFechaIntermedios { get; set; }
        
        [DataMember]
        public string PinturaReporteIntermedios { get; set; }
        
        [DataMember]
        public DateTime? PinturaFechaAcabadoVisual { get; set; }
        
        [DataMember]
        public string PinturaReporteAcabadoVisual { get; set; }
        
        [DataMember]
        public DateTime? PinturaFechaAdherencia { get; set; }
        
        [DataMember]
        public string PinturaReporteAdherencia { get; set; }
        
        [DataMember]
        public DateTime? PinturaFechaPullOff { get; set; }
        
        [DataMember]
        public string PinturaReportePullOff { get; set; }

        #endregion

        #region	Embarque
        
        [DataMember]
        public string EmbarqueEtiqueta { get; set; }
        
        [DataMember]
        public DateTime? EmbarqueFechaEtiqueta { get; set; }
        
        [DataMember]
        public string EmbarqueFolioPreparacion { get; set; }
        
        [DataMember]
        public DateTime? EmbarqueFechaEmbarque { get; set; }
        
        [DataMember]
        public string EmbarqueNumeroEmbarque { get; set; }

        #endregion
    }
}
