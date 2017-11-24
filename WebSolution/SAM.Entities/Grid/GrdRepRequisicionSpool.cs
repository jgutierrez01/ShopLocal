using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRepRequisicionSpool
    {
        [DataMember]
        public int RequisicionSpoolID { get; set; }
        [DataMember]
        public string NumeroRequisicion { get; set; }
        [DataMember]
        public DateTime? Fecha { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public int SpoolsTotales { get; set; }
        [DataMember]
        public int TipoPruebaSpoolID { get; set; }
    }
}
