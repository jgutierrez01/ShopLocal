using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRequisicionSpoolPruebas
    {
        [DataMember]
        public int WorkstatusSpoolID { get; set; }

        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string NombreSpool { get; set; }

        [DataMember]
        public string OrdenTrabajo { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public decimal Area { get; set; }

        [DataMember]
        public decimal PDI { get; set; }

        [DataMember]
        public decimal Peso { get; set; }

        [DataMember]
        public bool Armado { get; set; }

        [DataMember]
        public bool Soldadura { get; set; }

        [DataMember]
        public bool LiberacionDimensional { get; set; }

        [DataMember]
        public bool Hold { get; set; }
    }
}
