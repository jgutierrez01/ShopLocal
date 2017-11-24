using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Parciales
{
    [Serializable]
    public class NumeroUnicoInventarioCruceInfo
    {
        [DataMember]
        public int InventarioCongeladoTemporal { get; set; }

        [DataMember]
        public int InventarioDisponibleCruceTemporal { get; set; }

        [DataMember]
        public int InventarioDisponibleCruceOriginal { get; set; }

        [DataMember]
        public int InventarioCantidadRecibidaOriginal { get; set; }

        [DataMember]
        public int InventarioCantidadDanadaOriginal { get; set; }

        [DataMember]
        public int InventarioCongeladoOriginal { get; set; }

        [DataMember]
        public int InventarioCongeladoParaFabricacion { get; set; }
    }
}
