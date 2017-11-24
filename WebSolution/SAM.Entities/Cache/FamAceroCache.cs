using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class FamAceroCache : EntidadBase
    {
        [DataMember]
        public int FamiliaMaterialID { get; set; }

        [DataMember]
        public string FamiliaMaterialNombre { get; set; }

        [DataMember]
        public string Descripcion { get; set; }
    }
}
