using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdJuntaWorkstatus
    {
        [DataMember]
        public int JuntaWorkstatusID { get; set; }
        [DataMember]
        public string Etiqueta { get; set; }
        [DataMember]
        public string Localizacion
        {
            get
            {
                return EtiquetaMaterial1 + " - " + EtiquetaMaterial2;
            }
        }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public string Material1 { get; set; }
        [DataMember]
        public string Material2 { get; set; }
        [DataMember]
        public string Diametro { get; set; }
        [DataMember]
        public string UltimoProceso { get; set; }
        [DataMember]
        public string EtiquetaMaterial1 { get; set; }
        [DataMember]
        public string EtiquetaMaterial2 { get; set; }
        [DataMember]
        public string TipoJunta { get; set; }
        [DataMember]
        public bool JuntaFinal{ get; set; }
        [DataMember]
        public bool SpoolHold { get; set; }
        [DataMember]
        public bool TieneCorte { get; set; }
    }
}
