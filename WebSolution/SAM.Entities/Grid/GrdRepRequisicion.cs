using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRepRequisicion
    {
        [DataMember]
        public int RequisicionID { get; set; }
        [DataMember]
        public string NumeroRequisicion { get; set; }
        [DataMember]
        public DateTime? Fecha { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public int JuntasTotales { get; set; }
        [DataMember]
        public int TipoPruebaID { get; set; }
    }
}
