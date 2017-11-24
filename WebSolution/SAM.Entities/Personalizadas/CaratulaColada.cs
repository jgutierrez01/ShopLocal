using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class CaratulaColada
    {
        [DataMember]
        public string Colada { get; set; }
        [DataMember]
        public string Certificado { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
    }
}
