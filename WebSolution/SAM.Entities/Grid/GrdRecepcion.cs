using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRecepcion
    {
        [DataMember]
        public int RecepcionID { get; set; }

        [DataMember]
        public string Proyecto { get; set; }

        [DataMember]
        public string Transportista { get; set; }

        [DataMember]
        public DateTime FechaRecepcion { get; set; }

        [DataMember]
        public int CantidadNumerosUnicos { get; set; }
    }
}
