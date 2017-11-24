using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdSegJuntaDetPNDCuad
    {
        [DataMember]
        public string Cuadrante { get; set; }

        [DataMember]
        public string Placa { get; set; }

        [DataMember]
        public string Defecto { get; set; }
    }
}
