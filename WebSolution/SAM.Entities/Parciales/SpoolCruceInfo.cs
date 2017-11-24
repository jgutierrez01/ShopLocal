using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Parciales
{
    [Serializable]
    public class SpoolCruceInfo
    {
        [DataMember]
        public bool CruceExitoso { get; set; }

        [DataMember]
        public bool UsoEquivalencia { get; set; }
    }
}
