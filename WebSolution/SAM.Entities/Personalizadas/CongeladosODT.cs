using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class CongeladosOdt
    {
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public decimal Diametro1 { get; set; }
        [DataMember]
        public decimal Diametro2 { get; set; }
        [DataMember]
        public int InvFisico { get; set; }
        [DataMember]
        public int InvDañado { get; set; }
        [DataMember]
        public int InvCongelado { get; set; }
        [DataMember]
        public int InvDisponible { get; set; }
    }
}
