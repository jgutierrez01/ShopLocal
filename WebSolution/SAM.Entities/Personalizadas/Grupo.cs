using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class Grupo
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int Cuenta { get; set; }
        [DataMember]
        public int Suma { get; set; }
        [DataMember]
        public int CuentaTubo { get; set; }
        [DataMember]
        public int SumaTubo { get; set; }
        [DataMember]
        public decimal ? SumaDecimal { get; set; }
    }
}
