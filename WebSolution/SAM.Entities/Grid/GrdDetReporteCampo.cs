using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetReporteCampo
    {
        [DataMember]
        public int JuntaCampoReporteID { get; set; }

        [DataMember]
        public string NumeroReporte { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string TipoPrueba { get; set; }

        [DataMember]
        public string Resultado { get; set; }
    }
}
