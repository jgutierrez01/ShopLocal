using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Text; 
 using System.Threading.Tasks; 
 using System.Runtime.Serialization;


namespace SAM.Entities.Personalizadas.Shop
{
    [Serializable]
    public class DetailReport
    {
       
        [DataMember]
        public string NumeroReportePT { get; set; }
        [DataMember]
        public string NumeroReporteRT { get; set; }
        [DataMember]
        public string NumeroReportePWHT { get; set; }

        [DataMember]
        public string NumeroRequiPT { get; set; }
        [DataMember]
        public string NumeroRequiRT { get; set; }
       
        [DataMember]
        public DateTime? FechaRequiPT { get; set; }
        [DataMember]
        public DateTime? FechaRequiRT { get; set; }
      
        [DataMember]
        public bool? AprobadoPT { get; set; }
        [DataMember]
        public bool? AprobadoRT { get; set; }
        [DataMember]
        public bool? AprobadoPWHT { get; set; }
        [DataMember]
        public int? JuntaWorkstatusId { get; set; }

    }
}
