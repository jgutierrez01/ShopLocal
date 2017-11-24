using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.RadCombo
{
    [Serializable]
    public class RadSpool
    {
        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

    }
}
