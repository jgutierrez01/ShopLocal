using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Entities.Personalizadas.Shop
{
    [Serializable]
    public class DetailMaterialSummary
    {
        [DataMember]
        public string Material { get; set; }
        [DataMember]
        public string Motion1 { get; set; }
        [DataMember]
        public string Motion2 { get; set; }
        [DataMember]
        public string Bill { get; set; }
        [DataMember]
        public string MTR { get; set; }
        [DataMember]
        public string NumeroUnico { get; set; }
        [DataMember]
        public int Cantidad { get; set; }
        [DataMember]
        public string Codigo { get; set; }
    }
}
