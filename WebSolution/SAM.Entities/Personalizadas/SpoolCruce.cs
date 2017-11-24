using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class SpoolCruce
    {
        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public byte[] VersionRegistro { get; set; }

        [DataMember]
        public bool UsoEquivalencias { get; set; }
    }
}
