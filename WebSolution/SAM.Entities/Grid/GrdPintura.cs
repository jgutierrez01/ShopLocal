using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdPintura
    {
        [DataMember]
        public int OrdenTrabajoID { get; set; }
        
        [DataMember]
        public string OrdenTrabajo { get; set; }
        
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }
        
        [DataMember]
        public string NumeroControl { get; set; }
        
        [DataMember]
        public int? WorkstatusSpoolID { get; set; }
        
        [DataMember]
        public int? RequisicionPinturaDetalleID { get; set; }
        
        [DataMember]
        public string NombreSpool { get; set; }
        
        [DataMember]        
        public string Sistema { get; set; }
        
        [DataMember]                
        public string Color { get; set; }

        [DataMember]        
        public string Codigo { get; set; }

        [DataMember]        
        public DateTime? FechaSandBlast { get; set; }
        
        [DataMember]   
        public string ReporteSandBlast { get; set; }
        
        [DataMember]
        public DateTime? FechaPrimario { get; set; }
        
        [DataMember]
        public string ReportePrimario { get; set; }
        
        [DataMember]
        public DateTime? FechaIntermedio { get; set; }
        
        [DataMember]
        public string ReporteIntermedio { get; set; }
        
        [DataMember]
        public DateTime? FechaAcabadoVisual { get; set; }
        
        [DataMember]
        public string ReporteAcabadoVisual { get; set; }
        
        [DataMember]
        public DateTime? FechaAdherencia { get; set; }
        
        [DataMember]
        public string ReporteAdherencia { get; set; }
        
        [DataMember]
        public DateTime? FechaPullOff { get; set; }
        
        [DataMember]
        public string ReportePullOff { get; set; }
        
        [DataMember]
        public bool? Liberado { get; set; }
        
        [DataMember]
        public bool Hold { get; set; }
    }
}
