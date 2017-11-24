using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdBaston
    {
        [DataMember]
        public int BastonSpoolID { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public string LetraBaston { get; set; }
        [DataMember]
        public string Estacion { get; set; }
        [DataMember]
        public string Etiquetas { get; set; }
        [DataMember]
        public decimal PDI { get; set; }
    }
}
