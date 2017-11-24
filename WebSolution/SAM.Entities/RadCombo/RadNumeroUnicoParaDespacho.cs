using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.RadCombo
{
    [Serializable]
    public class RadNumeroUnicoParaDespacho
    {
        [DataMember]
        public int ItemCodeID { get; set; }
        [DataMember]
        public string CodigoItemCode { get; set; }
        [DataMember]
        public string DescripcionItemCode { get; set; }
        [DataMember]
        public string IndicadorEsEquivalente { get; set; }
        [DataMember]
        public int NumeroUnicoID { get; set; }
        [DataMember]
        public string CodigoNumeroUnico { get; set; }
        [DataMember]
        public decimal Diametro1 { get; set; }
        [DataMember]
        public decimal Diametro2 { get; set; }
        [DataMember]
        public bool EsEquivalente { get; set; }
        [DataMember]
        public int InventarioBuenEstado { get; set; }
        [DataMember]
        public string Segmento { get; set; }
        [DataMember]
        public bool EsSugeridoPorCruce { get; set; }
    }
}
