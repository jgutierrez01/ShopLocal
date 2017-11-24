﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
     [Serializable]
    public class GrdReporteTt
    {
        [DataMember]
        public int ReporteTtID { get; set; }

        [DataMember]
        public string NumeroDeReporte { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }
        
        [DataMember]
        public int TipoPruebaID { get; set; }

        [DataMember]
        public string TipoDePrueba { get; set; }

        [DataMember]
        public int JuntasTotales { get; set; }

        [DataMember]
        public int JuntasAprobadas { get; set; }

        [DataMember]
        public int JuntasRechazadas { get; set; }
    }
}
