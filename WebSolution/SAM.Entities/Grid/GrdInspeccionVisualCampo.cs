using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdInspeccionVisualCampo
    {
        [DataMember]
        public int JuntaCampoInspeccionVisualID { get; set; }
        [DataMember]
        public string NumeroReporte { get; set; }
        [DataMember]
        public DateTime FechaReporte { get; set; }
        [DataMember]
        public DateTime ? FechaInspeccion { get; set; }
        [DataMember]
        public bool Aprobado { get; set; }
        [DataMember]
        public string Resultado { get; set; }
    }

}
