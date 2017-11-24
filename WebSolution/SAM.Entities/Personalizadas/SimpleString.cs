using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class SimpleString
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Valor { get; set; }
    }
}
