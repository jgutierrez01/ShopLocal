using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class NumeroUnicoCache : EntidadBase
    {
        [DataMember]
        public int NumeroUnicoID { get; set; }

        [DataMember]
        public string Codigo { get; set; }
    }
}
