using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdSegJuntaDetSoldadura
    {
        [DataMember]
        public string Proceso { get; set; }

        [DataMember]
        public string CodigoSoldador { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Consumible { get; set; }
    }
}
