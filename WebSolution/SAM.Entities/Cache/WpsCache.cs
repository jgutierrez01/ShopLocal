using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class WpsCache : EntidadBase
    {
        [DataMember]
        public int MaterialBaseID { get; set; }

        [DataMember]
        public string FamiliaAcero1 { get; set; }

        [DataMember]
        public int MaterialBase2ID { get; set; }

        [DataMember]
        public string FamiliaAcero2 { get; set; }

        [DataMember]
        public int ProcesoRaizID { get; set; }

        [DataMember]
        public string ProcesoRaizNombre { get; set; }

        [DataMember]
        public int ProcesoRellenoID { get; set; }

        [DataMember]
        public string ProcesoRellenoNombre { get; set; }

        [DataMember]
        public decimal ? EspesorRaizMaximo { get; set; }

        [DataMember]
        public decimal ? EspesorRellenoMaximo { get; set; }

        [DataMember]
        public bool RequierePwht { get; set; }

        [DataMember]
        public bool RequierePreheat { get; set; }
    }
}
