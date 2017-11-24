using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdLiberacionDimansionalPatio
    {
        [DataMember]
        public int InspeccionDimansionalPatioID { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string NumeroDeControl { get; set; }

        [DataMember]
        public string Spool { get; set; }
    
        [DataMember]
        public string Resultado { get; set; }

        [DataMember]
        public DateTime FechaInspeccion { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        [DataMember]
        public bool Hold { get; set; }
    }
}
