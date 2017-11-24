using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class EspesorCache : EntidadBase
    {
        [DataMember]
        public decimal Valor { get; set; }

        [DataMember]
        public int DiametroID { get; set; }

        [DataMember]
        public int CedulaID { get; set; }
    }
}