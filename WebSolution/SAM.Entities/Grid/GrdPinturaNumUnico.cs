using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdPinturaNumUnico
    {
        [DataMember]
        public int NumeroUnicoID { get; set; }
        [DataMember]
        public string NumeroRequisicion { get; set; }
        [DataMember]
        public DateTime? FechaRequisicion { get; set; }
        [DataMember]
        public string NumeroUnico { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Liberado { get; set; }
        [DataMember]
        public DateTime? FechaPrimarios { get; set; }
        [DataMember]
        public string ReportePrimarios { get; set; }
        [DataMember]
        public DateTime? FechaIntermedio { get; set; }
        [DataMember]
        public string ReporteIntermedio { get; set; }
    }
}
