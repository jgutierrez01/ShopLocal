using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetReporteReqPintura
    {
        [DataMember]
        public int RequisicionPinturaDetalleID { get; set; }

        [DataMember]
        public string NumeroDeControl { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public string Sistema { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public int SpoolId { get; set; }

        [DataMember]
        public bool TieneHold { get; set; }
    }
}
