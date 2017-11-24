using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class KgTeoricoCache : EntidadBase
    {
        [DataMember]
        public decimal Valor { get; set; }

        [DataMember]
        public int CedulaID { get; set; }

        [DataMember]
        public int DiametroID { get; set; }
    }
}
