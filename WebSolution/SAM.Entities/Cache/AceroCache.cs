using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class AceroCache : EntidadBase
    {
        [DataMember]
        public int FamiliaAceroID { get; set; }

        [DataMember]
        public string FamiliaAceroNombre { get; set; }

        [DataMember]
        public string FamiliaMaterialNombre { get; set; }

        [DataMember]
        public bool VerificadoPorCalidad { get; set; }
    }
}
