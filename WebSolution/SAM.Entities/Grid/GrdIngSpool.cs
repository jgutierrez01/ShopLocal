using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdIngSpool
    {
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int Prioridad { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Dibujo { get; set; }
        [DataMember]
        public decimal Pdis { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public decimal Peso { get; set; }
        [DataMember]
        public decimal Area { get; set; }
        [DataMember]
        public string FamiliaAcero1 { get; set; }
        [DataMember]
        public string FamiliaAcero2 { get; set; }
        [DataMember]
        public string FamiliasAcero
        {
            get
            {
                if (!string.IsNullOrEmpty(FamiliaAcero2))
                {
                    return string.Concat(FamiliaAcero1, "/", FamiliaAcero2);
                }
                else
                {
                    return FamiliaAcero1;
                }
            }
        }
        [DataMember]
        public bool TieneHoldIngenieria{ get; set;}
        [DataMember]
        public bool TieneHoldCalidad { get; set; }
        [DataMember]
        public bool Confinado { get; set; }
        [DataMember]
        public bool TieneHold
        {
            get
            {
                return TieneHoldIngenieria || TieneHoldCalidad;
            }
        }
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
        public string RevisionCliente { get; set; }
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public bool RequierePwht { get; set; }
        [DataMember]
        public string Especificacion { get; set; }
        [DataMember]
        public int PorcentajePnd { get; set; }
        [DataMember]
        public string RevisionSteelgo { get; set; }
        [DataMember]
        public bool PendienteDocumental { get; set; }
        [DataMember]
        public bool AprobadoParaCruce { get; set; }
        [DataMember]
        public decimal DiametroPlano { get; set; }
        [DataMember]
        public decimal DiametroMayor { get; set; }
        [DataMember]
        public decimal? TotalPeq { get; set; }
        [DataMember]
        public string ObservacionesHold { get; set; }
        [DataMember]
        public string FechaHold { get; set; }
        [DataMember]
        public string FechaImportacion { get; set; }
        [DataMember]
        public bool? EsRevision { get; set; }
    }
}
