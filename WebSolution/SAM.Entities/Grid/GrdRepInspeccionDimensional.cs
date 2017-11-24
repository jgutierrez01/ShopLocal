using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRepInspeccionDimensional
    {
        [DataMember]
        public int ReporteDimensionalID { get; set; }
        [DataMember]
        public int TipoReporteDimensionalID { get; set; }
        [DataMember]
        public string NumeroReporte { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public int SpoolsTotales { get; set; }
        [DataMember]
        public int SpoolsAprobados { get; set; }
        [DataMember]
        public int SpoolsRechazados { get; set; }
        [DataMember]
        public string TipoReporte { get; set; }
        [DataMember]
        public string TipoReporteIngles { get; set; }
        
    }
}
