using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.RadCombo
{
    [Serializable]
    public class RadColada
    {
        [DataMember]
        public int ColadaID { get; set; }
        [DataMember]
        public string Certificado { get; set; }
        [DataMember]
        public string NumeroColada { get; set; }
        [DataMember]
        public string Fabricante { get; set; }
    }
}
