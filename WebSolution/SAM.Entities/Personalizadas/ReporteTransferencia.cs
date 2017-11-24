using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Cache;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class ReporteTransferencia
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
        public string Etiqueta { get; set; }

        [DataMember]
        public string NumeroTransferencia { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public DateTime? FechaEtiqueta { get; set; }

    }
}
