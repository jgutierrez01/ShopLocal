using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class ProcesoRaizCache : EntidadBase
    {
        [DataMember]
        public string Codigo { get; set; }
    }
}
