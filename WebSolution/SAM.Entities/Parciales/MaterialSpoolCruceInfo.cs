using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Parciales
{
    [Serializable]
    public class MaterialSpoolCruceInfo
    {
        [DataMember]
        public bool EsEquivalente { get; set; }

        [DataMember]
        public bool EsSugerido { get; set; }

        [DataMember]
        public int NumeroUnicoID { get; set; }

        [DataMember]
        public string Segmento { get; set; }        
    }
}
