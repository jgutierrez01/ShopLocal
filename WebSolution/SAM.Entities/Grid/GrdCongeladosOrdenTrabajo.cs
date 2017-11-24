using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdCongeladosOrdenTrabajo
    {
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int MaterialSpoolID { get; set; }
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public string Etiqueta { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public decimal D1 { get; set; }
        [DataMember]
        public decimal D2 { get; set; }
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public int NumeroUnico { get; set; }
        [DataMember]
        public int Cantidad { get; set; }
        [DataMember]
        public string Estatus { get; set; }
    }
}
