using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdSpoolOdt
    {
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int Partida { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string NombreSpool { get; set; }
        [DataMember]
        public decimal Pdis { get; set; }
        [DataMember]
        public bool DifiereDeIngenieria { get; set; }
        [DataMember]
        public EstatusDespachoOdt EstatusDespacho { get; set; }
        [DataMember]
        public bool FueReingenieria { get; set; }
        [DataMember]
        public string EstatusDespachoTexto { get; set; }
        [DataMember]
        public bool DifiereOReingenieria
        {
            get
            {
                return DifiereDeIngenieria || FueReingenieria;
            }
        }
    }
}
