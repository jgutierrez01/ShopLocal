using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetGrdSeguimientoSpool
    {
        #region Embarque

        [DataMember]
        public DateTime? EmbarqueFechaEmbarque { get; set; }

        [DataMember]
        public string EmbarqueNumeroEmbarque { get; set; }

        [DataMember]
        public string EmbarqueEtiqueta { get; set; }

        [DataMember]
        public DateTime? EmbarqueFechaEtiqueta { get; set; }

        [DataMember]
        public string EmbarqueFolioPreparacion { get; set; }

        #endregion

        #region Rep Dimensional

        [DataMember]
        public List<DetSegSpoolRep> ReportesDimensionales { get; set; }

        #endregion

        #region Rep Espesores

        [DataMember]
        public List<DetSegSpoolRep> ReportesEspesores { get; set; }

        #endregion

        #region Pintura

        [DataMember]
        public string PinturaCodigo { get; set; }

        [DataMember]
        public string PinturaColor { get; set; }

        [DataMember]
        public DateTime? PinturaFechaAcabadoVisual { get; set; }

        [DataMember]
        public DateTime? PinturaFechaIntermedios { get; set; }

        [DataMember]
        public DateTime? PinturaFechaAdherencia { get; set; }

        [DataMember]
        public DateTime? PinturaFechaPrimarios { get; set; }

        [DataMember]
        public DateTime? PinturaFechaPullOff { get; set; }

        [DataMember]
        public string PinturaNumeroRequisicion { get; set; }

        [DataMember]
        public DateTime? PinturaFechaSandBlast { get; set; }

        [DataMember]
        public string PinturaReporteAcabadoVisual { get; set; }

        [DataMember]
        public string PinturaReporteAdherencia { get; set; }

        [DataMember]
        public string PinturaReporteIntermedios { get; set; }

        [DataMember]
        public string PinturaReportePrimarios { get; set; }

        [DataMember]
        public string PinturaReportePullOff { get; set; }

        [DataMember]
        public DateTime? PinturaFechaRequisicion { get; set; }

        [DataMember]
        public string PinturaSistema { get; set; }

        [DataMember]
        public string PinturaReporteSandBlast { get; set; }

        #endregion

        #region General 

        [DataMember]
        public string OrdenDeTrabajo { get; set; }

        [DataMember]
        public string NumeroDeControl { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public string Dibujo { get; set; }

        [DataMember]
        public string Especificacion { get; set; }

        [DataMember]
        public decimal? Area { get; set; }

        [DataMember]
        public decimal? Peso { get; set; }

        [DataMember]
        public string RevisionSteelGo { get; set; }

        [DataMember]
        public int? Prioridad { get; set; }

        [DataMember]
        public decimal? Pdis { get; set; }

        [DataMember]
        public string RevisionCte { get; set; }
        
        [DataMember]
        public int? PorcPnd { get; set; }

        [DataMember]
        public string Material { get; set; }

        [DataMember]
        public string Cedula { get; set; }

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

        [DataMember]
        public bool RequierePWHT { get; set; }

        [DataMember]
        public bool PendienteDocumental { get; set; }

        [DataMember]
        public bool AprobadoCruce { get; set; }

        [DataMember]
        public bool HoldCalidad { get; set; }

        [DataMember]
        public bool HoldIngenieria { get; set; }

        [DataMember]
        public bool Confinado { get; set; }

        [DataMember]
        public decimal? DiametroMayor { get; set; }

        #endregion

        #region Certificado

        [DataMember]
        public bool CertificadoAprobado { get; set; }

        [DataMember]
        public DateTime? CertificadoFecha { get; set; }

        #endregion
    }
}
