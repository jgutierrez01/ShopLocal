using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Parciales
{
    [Serializable]
    public class OrdenTrabajoMaterialCruceInfo
    {
        [DataMember]
        public bool CambioNuCongelado { get; set; }
    }
}
