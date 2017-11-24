using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetSegSpoolRep
    {
        [DataMember]
        public int? Hoja{ get; set;}

        [DataMember]
        public DateTime? Fecha { get; set; }

        [DataMember]
        public DateTime? FechaReporte { get; set; }

        [DataMember]
        public string NumeroReporte { get; set; }

        [DataMember]
        public string Resultado { get; set; }

        [DataMember]
        public string Observaciones { get; set; }
    }
}
