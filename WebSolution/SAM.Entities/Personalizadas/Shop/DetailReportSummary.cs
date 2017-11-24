using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Text; 
 using System.Threading.Tasks; 
 using System.Runtime.Serialization;


namespace SAM.Entities.Personalizadas.Shop
{
    [Serializable]
    public class DetailReportSummary
    {
       
        [DataMember]
        public string NumeroReporte { get; set; }
        [DataMember]
        public bool Aprobado { get; set; }
        [DataMember]
        public int TipoPruebaId { get; set; }
        [DataMember]
        public int JuntaWorkStatusId { get; set; }

        public TipoPruebaEnum TipoPrueba
        {
            get
            {
                TipoPruebaEnum tipo = new TipoPruebaEnum();

                switch(TipoPruebaId)
                {
                    case 1:
                        tipo = TipoPruebaEnum.ReporteRT;
                        break;
                    case 2:
                        tipo = TipoPruebaEnum.ReportePT;
                        break;
                    case 3:
                        tipo = TipoPruebaEnum.Pwht;
                        break;
                    case 4:
                        tipo = TipoPruebaEnum.Durezas;
                        break;
                    case 10:
                        tipo = TipoPruebaEnum.ReportePMI;
                        break;
                    case 11:
                        tipo = TipoPruebaEnum.Neumatica;
                        break;                    
                }

                return tipo;  
            }
        }       
    }
}
