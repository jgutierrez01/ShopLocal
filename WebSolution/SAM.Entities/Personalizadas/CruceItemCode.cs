using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class CruceItemCode
    {
        [DataMember]
        public int ItemCodeID { get; set; }

        [DataMember]
        public decimal Diametro1 { get; set; }

        [DataMember]
        public decimal Diametro2 { get; set; }

        [DataMember]
        public decimal InventarioFisico { get; set; }

        [DataMember]
        public decimal InventarioBuenEstado { get; set; }

        [DataMember]
        public decimal InventarioCongelado { get; set; }

        [DataMember]
        public decimal InventarioDisponibleCruce { get; set; }

        [DataMember]
        public decimal InventarioCongeladoTemporal { get; set; }

        [DataMember]
        public decimal InventarioDisponibleCruceTemporal { get; set; }
    }
}
