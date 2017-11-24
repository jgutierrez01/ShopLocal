using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdSegJuntaDetPNDSector
    {
        [DataMember]
        public string Sector { get; set; }

        [DataMember]
        public string De { get; set; }

        [DataMember]
        public string A { get; set; }

        [DataMember]
        public string Defecto { get; set; }
    }
}
