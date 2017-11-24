using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdEstimacionSpoolCompleta
    {
        [DataMember]
        public int? EstimacionId { get; set; }

        [DataMember]
        public int WorkStatusSpoolID { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public decimal? PDI { get; set; }

        [DataMember]
        public string Material { get; set; }

        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public bool InspecciónDimensional { get; set; }

        [DataMember]
        public bool Pintura { get; set; }

        [DataMember]
        public bool Embarcado { get; set; }

        [DataMember]
        public int? OrdenTrabajoSpoolID { get; set; }

        [DataMember]
        public int? EstimacionSpoolID { get; set; }

        [DataMember]
        public string NumeroEstimacion { get; set; }

        [DataMember]
        public int? ConceptoEstimacionID { get; set; }

    }
}
