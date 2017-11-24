using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdHistWorkstatus
    {
        [DataMember]
        public int HistoricoWorkStatusID { get; set; }
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public string Odt { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string RevCliente { get; set; }
        [DataMember]
        public string RevSteelgo { get; set; }
        [DataMember]
        public DateTime FechaHomologacion { get; set; }
    }
}
