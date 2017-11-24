using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class ListaFechaNumeroControl
    {
        [DataMember]
        public DateTime FechaProceso { get; set; }

        [DataMember]
        public DateTime FechaReporte { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }
    }
}
