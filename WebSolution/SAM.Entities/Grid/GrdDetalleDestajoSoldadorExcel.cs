using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetalleDestajoSoldadorExcel : GrdDetalleDestajoSoldador
    {
        [DataMember]
        public string Isometrico { get; set; }
        [DataMember]
        public int ProcesoRaizID { get; set; }
        [DataMember]
        public int ProcesoRellenoID { get; set; }
        [DataMember]
        public DateTime FechaSoldadura { get; set; }
        [DataMember]
        public string ProcesoRaiz { get; set; }
        [DataMember]
        public string ProcesoRelleno { get; set; }
        [DataMember]
        public int SoldadorID { get; set; }
        [DataMember]
        public string Cedula { get; set; }
    }
}
