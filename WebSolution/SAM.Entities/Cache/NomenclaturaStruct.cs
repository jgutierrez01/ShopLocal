using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public struct NomenclaturaStruct
    {
        [DataMember]
        public int Orden { get; set; }
        [DataMember]
        public string NombreColumna { get; set; }
    }
}
