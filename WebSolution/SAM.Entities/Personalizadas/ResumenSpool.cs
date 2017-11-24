using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class ResumenSpool
    {
        [DataMember]
        public int SpoolsTotales { get; set; }

        [DataMember]
        public int SpoolsSinOdt { get; set; }

        [DataMember]
        public int SpoolsConOdt { get; set; }
    }
}
