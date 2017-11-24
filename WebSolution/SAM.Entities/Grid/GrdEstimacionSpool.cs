using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdEstimacionSpool
    {
        [DataMember]
        public int EstimacionID { get; set; }

        [DataMember]
        public int EstimacionSpoolID { get; set; }

        [DataMember]
        public string Concepto { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public decimal? Pdi { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string Material { get; set; }

        [DataMember]
        public string Cedula { get; set; }
    }
}
