using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Web.Shop.Models
{
    public class DetailSummaryJointModel
    {       
        public string Joint { get; set; }
       
        public string Label { get; set; }
      
        public string TypeJoint { get; set; }
    
        public DateTime? DateWelding { get; set; }
    
        public DateTime? DateVisualInspection { get; set; }
       
        public string ReportePWHT { get; set; }     
   
        public string ClasifPND { get; set; }       
    
        public string RequiredPWHT { get; set; }

        public decimal Diameter { get; set; }
     
        public string ReportePT { get; set; }

        public string ReporteRT { get; set; }

        public string RequiPT { get; set; }

        public string RequiRT { get; set; }

        public string AprobadoPT { get; set; }

        public string AprobadoRT { get; set; }
        
        public bool? AprobadoPWHT { get; set; }

        public string InspectorVisual { get; set; }
        
        public DateTime? FechaReqPT { get; set; }

        public DateTime? FechaReqRT { get; set; }
        public int? JuntaWorkStatusId { get; set; }
        
        public string WeldingDate 
        { 
            get { return DateWelding.HasValue ? DateWelding.Value.ToShortDateString() : string.Empty; } 
        }       
 
        public string VIDate 
        { 
            get { return DateVisualInspection.HasValue ? DateVisualInspection.Value.ToShortDateString() : string.Empty; } 
        }
        public string ReqPTDate
        {
            get { return FechaReqPT.HasValue ? FechaReqPT.Value.ToShortDateString() : string.Empty; }
        }

        public string ReqRTDate
        {
            get { return FechaReqRT.HasValue ? FechaReqRT.Value.ToShortDateString() : string.Empty; }
        }

        public string ResultadoVisual { get; set; }
        public DetailSummaryJointModel() 
        { 

        }    
    }
}