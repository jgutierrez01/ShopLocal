using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdCorteAjuste
    {
        [DataMember]
        public int MaterialSpoolID { get; set; }
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public string Proyecto { get; set; }
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string EtiquetaMaterial { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public int LongitudIngenieria {get; set;}
        [DataMember]
        public int LongitudCorte { get; set; }
        [DataMember]
        public int Diferencia 
        {
            get
            {
             return  LongitudIngenieria - LongitudCorte;
            }
        }
        [DataMember]
        public int? Tolerancia { get; set; }
    }
}
