using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRepInspeccionVisual
    {
        [DataMember]
        public int InspeccionVisualID { get; set; }
        [DataMember]
        public string NumeroReporte { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public int JuntasTotales { get; set; }
        [DataMember]
        public int JuntasAprobadas { get; set; }
        [DataMember]
        public int JuntasRechazadas { get; set; }
    }
}
