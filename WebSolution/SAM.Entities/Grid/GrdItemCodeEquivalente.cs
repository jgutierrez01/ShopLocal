using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdItemCodeEquivalente
    {
        [DataMember]
        public int ItemCodeEquivalenteID { get; set; }
        [DataMember]
        public int ItemCodeID { get; set; }
        [DataMember]
        public string CodigoIC { get; set; }
        [DataMember]
        public string DescripcionIC { get; set; }
        [DataMember]
        public decimal D1 { get; set; }
        [DataMember]
        public decimal D2 { get; set; }
        [DataMember]
        public int ItemEquivalenteID { get; set; }
        [DataMember]
        public string CodigoEq { get; set; }
        [DataMember]
        public string DescripcionEq { get; set; }
        [DataMember]
        public decimal D1Eq { get; set; }
        [DataMember]
        public decimal D2Eq { get; set; }
        [DataMember]
        public int ProyectoID { get; set; }
    }
}
