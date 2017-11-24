using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRequisicionPintura
    {
        [DataMember]
        public int? WorkstatusSpoolID { get; set; }

        [DataMember]
        public string NombreSpool { get; set; }

        [DataMember]
        public string EspecificacionSpool { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string Sistema { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public bool Hold { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]//KG
        public decimal? Peso { get; set; }
        
        [DataMember]//M2
        public decimal? Area { get; set; }
        
        [DataMember]//Localizacion
        public string Localizacion { get; set; }

        [DataMember]
        public decimal Peqs { get; set; }
        
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }

        [DataMember]
        public bool TieneRequisicionPintura { get; set; }
        
        [DataMember]
        public int? RequisicionPinturaDetalleID { get; set; }

    }
}
