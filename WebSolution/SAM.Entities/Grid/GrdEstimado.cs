using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdEstimado
    {
        [DataMember]
        public int EstimadoID { get; set; }

        [DataMember]
        public string NumeroEstimacion { get; set; }

        [DataMember]
        public string Proyecto { get; set; }

        [DataMember]
        public int NumeroSpools { get; set; }

        [DataMember]
        public DateTime FechaEstimacion { get; set; }

        [DataMember]
        public int NumeroJunta { get; set; }
       
    }
}
