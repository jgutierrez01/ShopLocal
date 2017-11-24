using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class PermisoCache : EntidadBase
    {
        [DataMember]
        public int ModuloID { get; set; }

        [DataMember]
        public List<string> Paginas { get; set; }
    }
}
