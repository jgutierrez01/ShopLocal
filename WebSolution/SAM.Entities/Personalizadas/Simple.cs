using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class Simple
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Valor { get; set; }
    }
}
