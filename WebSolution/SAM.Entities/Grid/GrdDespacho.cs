using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDespacho
    {
        [DataMember]
        public int DespachoID { get; set; }

        [DataMember]
        public DateTime FechaDespacho { get; set; }

        [DataMember]
        public string NumeroOrden { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public bool EsEquivalente { get; set; }

        [DataMember]
        public string NumeroUnico { get; set; }

        [DataMember]
        public int Cantidad { get; set; }

        [DataMember]
        public bool Cancelado { get; set; }

        [DataMember]
        public string Estatus { get; set; }
        
    }
}
