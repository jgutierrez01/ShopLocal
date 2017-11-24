using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdTransferenciaSpool
    {
        [DataMember]
        public int TransferenciaSpoolID { get; set; }

        [DataMember]
        public string NumeroTransferencia { get; set; }
        
        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string OrdenTrabajo { get; set; }

        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string NombreSpool { get; set; }

        [DataMember]
        public decimal Area { get; set; }

        [DataMember]
        public decimal PDI { get; set; }

        [DataMember]
        public decimal Peso { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }       

        [DataMember]
        public bool Preparado { get; set; }

        [DataMember]
        public DateTime? FechaPreparacion { get; set; }

        [DataMember]
        public DateTime? FechaTransferencia { get; set; }

        [DataMember]
        public bool Transferencia { get; set; }
     
        [DataMember]
        public bool Hold { get; set; }
    }
}
