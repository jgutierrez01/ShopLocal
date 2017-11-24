using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetInspeccionVisual
    {
        [DataMember]
        public string OrdenTrabajo { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public string Junta { get; set; }
        [DataMember]
        public string Localizacion { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public string Material1 { get; set; }
        [DataMember]
        public string Material2 { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
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
        public DateTime? FechaInspeccion { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public int JuntaInspeccionVisualID { get; set; }
    }
}
