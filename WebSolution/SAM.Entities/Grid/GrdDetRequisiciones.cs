using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetRequisiciones
    {
        [DataMember]
        public string OrdenTrabajo { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public string Junta { get; set; }
        [DataMember]
        public string Localizacion { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public string Material1 { get; set; }
        [DataMember]
        public string Material2 { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
        [DataMember]
        public int JuntaRequisicionID { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public bool TieneHold { get; set; }
    }
}
