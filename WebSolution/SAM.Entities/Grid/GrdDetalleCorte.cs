using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetalleCorte
    {
        [DataMember]
        public int CorteDetalleID { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string Maquina { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public int CantidadRequerida { get; set; }

        [DataMember]
        public int CantidadReal { get; set; }

        [DataMember]
        public bool Cancelado { get; set; }

        [DataMember]
        public string Estatus { get; set; }
    }
}
