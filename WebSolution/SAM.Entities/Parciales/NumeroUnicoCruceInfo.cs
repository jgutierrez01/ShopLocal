using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Parciales
{
    [Serializable]
    public class NumeroUnicoCruceInfo
    {
        [DataMember]
        public bool SeUsoEnCruce { get; set; }
    }
}
