using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.RadCombo
{
    [Serializable]
    public class RadNumeroUnico
    {
        [DataMember]
        public int NumeroUnicoID { get; set; }
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Segmento { get; set; }
        [DataMember]
        public string CodigoSegmento { get { return Segmento != string.Empty ? Codigo + "-" + Segmento : Codigo; } }
        [DataMember]
        public int InventarioBuenEstado { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public decimal Diametro1 { get; set; }
        [DataMember]
        public decimal Diametro2 { get; set; }
    }
}
