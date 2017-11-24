using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdWps
    {
        [DataMember]
        public int WpsID { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public FamiliaAcero FamiliaAcero { get; set; }

        [DataMember]
        public FamiliaAcero FamiliaAcero1 { get; set; }

        [DataMember]
        public ProcesoRaiz ProcesoRaiz { get; set; }

        [DataMember]
        public ProcesoRelleno ProcesoRelleno { get; set; }

        [DataMember]
        public decimal? EspesorRaizMaximo { get; set; }

        [DataMember]
        public decimal? EspesorRellenoMaximo { get; set; }

        [DataMember]
        public bool RequierePwht { get; set; }

        [DataMember]
        public bool RequierePreheat { get; set; }
    }
}
