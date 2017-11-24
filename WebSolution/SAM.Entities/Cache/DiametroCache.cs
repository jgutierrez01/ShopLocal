using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class DiametroCache : EntidadBase
    {
        [DataMember]
        public decimal Valor { get; set; }
    }
}
