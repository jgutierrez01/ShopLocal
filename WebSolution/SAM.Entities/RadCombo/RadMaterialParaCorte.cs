using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.RadCombo
{
    [Serializable]
    public class RadMaterialParaCorte
    {
        [DataMember]
        public int MaterialSpoolID { get; set; }

        [DataMember]
        public string EtiquetaSeccion { get; set; }

        [DataMember]
        public string EtiquetaMaterial { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string EsEquivalente { get; set; }

    }
}
