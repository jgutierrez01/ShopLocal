using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdPersonaDestajoExcel : GrdPersonaDestajo
    {
        [DataMember]
        public string Comentarios { get; set; }
    }
}
