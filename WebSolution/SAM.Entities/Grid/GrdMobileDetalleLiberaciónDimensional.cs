using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdMobileDetalleLiberaciónDimensional
    {
        [DataMember]
        public DateTime? FechaLiberacion { get; set; }

        [DataMember]
        public DateTime FechaReporte { get; set; }

        [DataMember]
        public string NumeroReporte { get; set; }
    }
}
