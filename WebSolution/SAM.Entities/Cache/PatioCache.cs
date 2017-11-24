using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    public class PatioCache : EntidadBase
    {
        [DataMember]
        public string Propietario { get; set; }

        [DataMember]
        public List<TallerCache> Talleres { get; set; }
    }
}
