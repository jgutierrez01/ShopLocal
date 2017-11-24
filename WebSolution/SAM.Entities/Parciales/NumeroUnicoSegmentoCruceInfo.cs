using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Parciales
{
    [Serializable]
    public class NumeroUnicoSegmentoCruceInfo
    {
        [DataMember]
        public int InventarioCongeladoTemporal { get; set; }

        [DataMember]
        public int InventarioDisponibleCruceTemporal { get; set; }
    }
}
