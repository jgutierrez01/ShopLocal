using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class ResumenIsometrico
    {
        [DataMember]
        public string Dibujo { get; set; }

        [DataMember]
        public int TotalSpools { get; set; }

        [DataMember]
        public int SpoolsConODT { get; set; }

        [DataMember]
        public int SpoolsSinODT { get; set; }
    }
}
