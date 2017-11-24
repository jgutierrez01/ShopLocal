using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRepReqPinturaNumUnico
    {
        [DataMember]
        public int RequisicionNumeroUnicoID { get; set; }
        [DataMember]
        public string NumeroRequisicion { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public int CantidadNumerosUnicos { get; set; }
    }
}
