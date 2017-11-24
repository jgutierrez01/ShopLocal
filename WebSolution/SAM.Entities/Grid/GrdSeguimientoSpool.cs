using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdSeguimientoSpool
    {
        
        #region General

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public decimal? Area { get; set; }

        [DataMember]
        public decimal? Peso { get; set; }

        [DataMember]
        public decimal? Pdi { get; set; }

        [DataMember]
        public string Proyecto { get; set; }

        [DataMember]
        public string OrdenDeTrabajo { get; set; }

        [DataMember]
        public string NumeroDeControl { get; set; }

        [DataMember]
        public string Spool { get; set; }
        
        [DataMember]
        public int NumeroJuntas { get; set; }
        
        [DataMember]
        public string  TieneHold { get; set; }

        [DataMember]
        public string Especificacion { get; set; }

        [DataMember]
        public int? Prioridad { get; set; }

        [DataMember]
        public string RequierePWHT { get; set; }

        #region Nomenclatura

        [DataMember]
        public string Segmento1 { get; set; }
        [DataMember]
        public string Segmento2 { get; set; }
        [DataMember]
        public string Segmento3 { get; set; }
        [DataMember]
        public string Segmento4 { get; set; }
        [DataMember]
        public string Segmento5 { get; set; }
        [DataMember]
        public string Segmento6 { get; set; }
        [DataMember]
        public string Segmento7 { get; set; }
        #endregion

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
        public int InspeccionDimensionalNoRechazos { get; set; }

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
        public bool?  InspeccionEspesoresResultado { get; set; }

        [DataMember]
        public string InspeccionEspesoresObservaciones { get; set; }

        [DataMember]
        public int InspeccionEspesoresNoRechazos { get; set; }

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
        public DateTime? EmbarqueFechaPreparacion { get; set; }
        
        [DataMember]
        public DateTime? EmbarqueFechaEmbarque { get; set; }
        
        [DataMember]
        public string EmbarqueNumeroEmbarque { get; set; }

        #endregion

        #region Certificado

        [DataMember]
        public string CertificadoCodigo { get; set; }

        [DataMember]
        public DateTime? CertificadoFecha { get; set; }

        #endregion


      
    }
}
