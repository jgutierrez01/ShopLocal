using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdOdtReqMateial
    {
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public int OrdenTrabajoSpool { get; set; }
        [DataMember]
        public int MaterialSpoolID { get; set; }
        [DataMember]
        public string EtiquetaMaterial { get; set; }
        [DataMember]
        public int Cantidad { get; set; }
    }
}
