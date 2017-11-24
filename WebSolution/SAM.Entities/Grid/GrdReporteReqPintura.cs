using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdReporteReqPintura
    {
        [DataMember]
        public int RequisicionPinturaID { get; set; }

        [DataMember]
        public string NumeroDeRequisicion { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public int Spools { get; set; }
    }
}
