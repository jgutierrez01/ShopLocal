using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class ProyectoReporteCache : EntidadBase
    {
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public TipoReporteProyectoEnum TipoReporte { get; set; }
    }
}
