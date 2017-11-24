using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdPeqNoEncontrados
    {
        
        [DataMember]
        public string TipoJunta { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
        [DataMember]
        public string FamiliaAcero { get; set; }
    }
}
