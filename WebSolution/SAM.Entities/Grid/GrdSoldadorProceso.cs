using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdSoldadorProceso
    {
        [DataMember]
        public int SoldadorID { get; set; }
        [DataMember]
        public string CodigoSoldador { get; set; }
        [DataMember]
        public Nullable<int> ConsumibleID { get; set; }
        [DataMember]
        public string CodigoConsumible { get; set; }
        [DataMember]
        public int TipoProceso { get; set; }
        [DataMember]
        public string Proceso { get; set; }
        [DataMember]
        public string NombreCompleto { get; set; }
        [DataMember]
        public int? JuntaSoldaduraDetalleID { get; set; }
    }
}
