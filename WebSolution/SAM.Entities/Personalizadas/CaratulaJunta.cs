using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class CaratulaJunta
    {
        [DataMember]
        public string Etiqueta { get; set; }
        [DataMember]
        public string TipoJunta { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public DateTime? Armado { get; set; }
        [DataMember]
        public DateTime? Soldadura { get; set; }
        [DataMember]
        public string Wps { get; set; }
        [DataMember]
        public string SoldadorRaiz { get; set; }
        [DataMember]
        public string SoldadorRelleno { get; set; }
        [DataMember]
        public string InspeccionVisual { get; set; }
        [DataMember]
        public string MaterialBase1 { get; set; }
        [DataMember]
        public string MaterialBase2 { get; set; }
        [DataMember]
        public string Rt { get; set; }
        [DataMember]
        public string Pt { get; set; }
        [DataMember]
        public string PWHT { get; set; }
        [DataMember]
        public string PostRT { get; set; }
        [DataMember]
        public string Durezas { get; set; }
    }
}
