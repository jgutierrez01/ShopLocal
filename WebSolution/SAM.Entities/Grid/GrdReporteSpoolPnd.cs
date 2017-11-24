using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdReporteSpoolPnd
    {
        [DataMember]
        public int ReporteSpoolPndID { get; set; }

        [DataMember]
        public string NumeroDeReporte { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string TipoDePruebaSpool { get; set; }

        [DataMember]
        public int TipoPruebaSpoolID { get; set; }

        [DataMember]
        public int SpoolsTotales { get; set; }

        [DataMember]
        public int SpoolsAprobados { get; set; }

        [DataMember]
        public int SpoolsRechazados { get; set; }
    }
}
