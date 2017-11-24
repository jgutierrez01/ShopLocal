using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdIcEquivalenteAgrupado
    {
        [DataMember]
        public int ItemCodeID { get; set; }

        [DataMember]
        public decimal Diametro1 { get; set; }

        [DataMember]
        public decimal Diametro2 { get; set; }

        [DataMember]
        public int NumEquivalencias { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public int MinItemCodeEquivalenteID { get; set; }
    }
}
