using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetalleDestajoTuberoExcel : GrdDetalleDestajoTubero
    {
        [DataMember]
        public string Isometrico { get; set; }
        [DataMember]
        public DateTime FechaArmado { get; set; }
        [DataMember]
        public int TuberoID { get; set; }
        [DataMember]
        public string Cedula { get; set; }
    }
}
