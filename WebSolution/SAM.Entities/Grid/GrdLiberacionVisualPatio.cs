using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdLiberacionVisualPatio
    {
        [DataMember]
        public int InspeccionVisualPatioID { get; set; }

        [DataMember]
        public string NumeroDeControl{ get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public string EtiquetaJunta { get; set; }

        [DataMember]
        public string TipoJunta { get; set; }

        [DataMember]
        public string Resultado { get; set; }

        [DataMember]
        public DateTime FechaInspeccion { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        [DataMember]
        public bool Hold { get; set; }
    }
}
