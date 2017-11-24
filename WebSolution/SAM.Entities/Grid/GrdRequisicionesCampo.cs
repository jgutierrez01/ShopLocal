using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRequisicionesCampo
    {
        [DataMember]
        public int RequisicionID { get; set; }

        [DataMember]
        public string NumeroRequisicion { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; } 

        [DataMember]
        public string TipoPrueba { get; set; }

    }
}
