using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class MaquinaCache : EntidadBase
    {
        [DataMember]
        public string PatioNombre { get; set; }

        [DataMember]
        public int PatioID { get; set; }

    }
}
