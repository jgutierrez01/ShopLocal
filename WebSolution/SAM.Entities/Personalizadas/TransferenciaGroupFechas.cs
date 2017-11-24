using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{

    [Serializable]
    public class MaxTransferenciaSpool
    {
        [DataMember]
        public int TransferenciaSpoolID { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

    }

    [Serializable]
    public class ColeccionFechasTransferencia
    {
        [DataMember]
        public int TransferenciaSpoolID { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public DateTime? FechaPreparacion { get; set; }

        [DataMember]
        public DateTime? FechaTransferencia { get; set; }

        [DataMember]
        public string NumeroTransferencia { get; set; }

        [DataMember]
        public bool Transferidos { get; set; }

    }

}
