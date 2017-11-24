using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetHoldSpool
    {
        public DetHoldSpool(SpoolHoldHistorial hold)
        {
            FechaHold = hold.FechaHold;
            TipoHold = hold.TipoHold;
            Observaciones = hold.Observaciones;
        }
        [DataMember]
        public DateTime FechaHold { get; set; }
        [DataMember]
        public string TipoHold { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
    }
}
