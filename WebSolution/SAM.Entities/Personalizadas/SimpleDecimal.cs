using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class SimpleDecimal
    {
        [DataMember]
        public decimal ID { get; set; }
        [DataMember]
        public int Valor { get; set; }
    }
}
