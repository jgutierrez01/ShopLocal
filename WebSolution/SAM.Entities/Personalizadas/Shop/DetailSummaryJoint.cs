using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Text; 
 using System.Threading.Tasks; 
 using System.Runtime.Serialization;
namespace SAM.Entities.Personalizadas.Shop
{
    [Serializable]
    public class DetailSummaryJoint
    {
        [DataMember]
        public string Joint { get; set; }
        [DataMember]
        public decimal Diameter { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public string TypeJoint { get; set; } 
      
        [DataMember]
        public DateTime? DateWelding { get; set; }

        [DataMember]       
        public DateTime? DateVisualInspection { get; set; }

        [DataMember]
        public string ReportePWHT { get; set; }

        [DataMember]
        public string ClasifPND { get; set; }

        [DataMember]
        public string ReportePT { get; set; }

        [DataMember]
        public string ReporteRT { get; set; }

        [DataMember]
        public string RequiPT { get; set; }

        [DataMember]
        public string RequiRT { get; set; }

        [DataMember]
        public DateTime? FechaReqPT { get; set; }

        [DataMember]
        public DateTime? FechaReqRT { get; set; }

        [DataMember]
        public string RequiredPWHT { get; set; }

        [DataMember]
        public int? JuntaWorkStatusId { get; set; }

        [DataMember]
        public string AprobadoPT { get; set; }

        [DataMember]
        public string AprobadoRT { get; set; }

        [DataMember]
        public bool? AprobadoPWHT { get; set; }

        [DataMember]
        public string InspectorVisual { get; set; }
        [DataMember]
        public string ResultadoVisual { get; set; }
    }
}
