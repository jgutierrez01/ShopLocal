using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class ConceptoEstimacionCache : EntidadBase
    {
        [DataMember]
        public bool AplicaParaJunta{get;set;}
        [DataMember]
        public bool AplicaParaSpool{ get; set; }
    }
}
