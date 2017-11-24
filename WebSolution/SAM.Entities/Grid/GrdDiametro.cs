using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDiametro
    {
        [DataMember]
        public int DiametroID { get; set; }

        [DataMember]
        public bool VerificadoPorCalidad { get; set; }

    }
}
