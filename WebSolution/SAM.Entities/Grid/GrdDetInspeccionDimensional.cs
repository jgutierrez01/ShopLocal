using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetInspeccionDimensional
    {
        [DataMember]
        public string OrdenTrabajo { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public int? Hoja { get; set; }
        [DataMember]
        public bool Aprobado { get; set; }
        [DataMember]
        public string Resultado
        {
            get
            {
                if (Aprobado == true)
                {
                    return "Aprobado";
                }
                else
                {
                    return "Rechazado";
                }
            }
        }
        [DataMember]
        public DateTime? FechaLiberacion { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public int ReporteDimensionalDetalleID { get; set; }
    }
}
