using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdCongeladosNumeroUnico
    {
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int MaterialSpoolID { get; set; }
        [DataMember]
        public string NumControl { get; set; }
        [DataMember]
        public string Etiqueta { get; set; }
        [DataMember]
        public int CantCong { get; set; }
        [DataMember]
        public bool Equiv { get; set; }

    }
}
