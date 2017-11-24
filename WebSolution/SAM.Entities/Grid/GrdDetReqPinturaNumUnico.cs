using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetReqPinturaNumUnico
    {
        [DataMember]
        public int RequisicionNumeroUnicoDetalleID { get; set; }
        [DataMember]
        public string NumeroUnico { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public decimal Diametro1 { get; set; }
        [DataMember]
        public decimal Diametro2 { get; set; }
    }
}
