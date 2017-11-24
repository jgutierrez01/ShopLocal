using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class ModuloSeguimientoJuntaCache : EntidadBase
    {
        [DataMember]
        public int ModuloSeguimientoJuntaID { get; set; }
      
        [DataMember]
        public int OrdenUI{ get; set; }

        [DataMember]
        public string NombreTemplateColumn{ get; set; }
    }
}
