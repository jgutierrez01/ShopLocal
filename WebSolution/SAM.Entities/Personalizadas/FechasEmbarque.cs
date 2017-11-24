using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class FechasEmbarque
    {
        [DataMember]
        public DateTime? FechaEstimada { get; set; }

        [DataMember]
        public DateTime? FechaEmbarque { get; set; }
    }
}
