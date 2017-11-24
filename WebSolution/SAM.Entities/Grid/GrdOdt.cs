using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdOdt
    {
        [DataMember]
        public int OrdenDeTrabajoID { get; set; }
        [DataMember]
        public int EstatusOrdenID { get; set; }
        [DataMember]
        public string Estatus { get; set; }
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public int PatioID { get; set; }
        [DataMember]
        public int TallerID { get; set; }
        [DataMember]
        public string Patio { get; set; }
        [DataMember]
        public string Proyecto { get; set; }
        [DataMember]
        public string Taller { get; set; }
        [DataMember]
        public string NumeroOrden { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public int Version { get; set; }
        [DataMember]
        public EstatusDespachoOdt EstatusDespacho { get; set; }
        [DataMember]
        public string EstatusDespachoTexto { get; set; }
        [DataMember]
        public int CantidadSpools { get; set; }
        [DataMember]
        public bool DifiereDeIngenieria { get; set; }
        [DataMember]
        public bool FueReingenieria { get; set; }
        [DataMember]
        public bool DifiereOReingenieria
        {
            get
            {
                return DifiereDeIngenieria || FueReingenieria;
            }
        }

        [DataMember]
        public int Orden { get; set; }

    }
}