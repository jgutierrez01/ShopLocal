using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SAM.Entities.Cache;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class NumeroUnicoEdicionEspecial : EntidadBase
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public int Value { get; set; }
    }
}
