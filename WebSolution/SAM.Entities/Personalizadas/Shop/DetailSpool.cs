using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Text; 
 using System.Threading.Tasks; 
 using System.Runtime.Serialization; 
 
 
 namespace SAM.Entities.Personalizadas.Shop 
 { 
     [Serializable] 
     public class DetailSpool 
     { 
         [DataMember] 
         public String Isometric { get; set; } 
         
         [DataMember] 
         public string Hold { get; set; } 
         
         [DataMember] 
         public DateTime? DateHold { get; set; } 
          
         [DataMember] 
         public string CustomerReview { get; set; } 
  
         [DataMember] 
         public string Specification { get; set; } 
 
         [DataMember] 
         public int? PercentagePND { get; set; }  
 
         [DataMember] 
         public int Joints { get; set; } 
         
         [DataMember] 
         public decimal Weight { get; set; } 
         
         [DataMember] 
         public decimal Area { get; set; } 
         
         [DataMember] 
         public decimal PDI { get; set; } 
        
         [DataMember] 
         public decimal PEQS { get; set; } 
        
         [DataMember] 
         public string DimensionalLiberation { get; set; } 
          
         [DataMember] 
         public string RequiredTestNeumatic { get; set; }

         [DataMember]
         public string RequiredPWHT { get; set; } 
          
         [DataMember] 
         public string TestNeumatic { get; set; } 
          
         [DataMember] 
         public string RequiredTestHydrostatic { get; set; } 
          
         [DataMember] 
         public string TestHydrostatic { get; set; } 
          
         [DataMember] 
         public string NumberShipment { get; set; } 
          
         [DataMember] 
         public DateTime? DateShipment { get; set; } 
          
         [DataMember] 
         public string System { get; set; } 
  
         [DataMember] 
         public string Color { get; set; }      
      
         [DataMember] 
         public DateTime? FechaPrimario { get; set; } 
          
         [DataMember] 
         public string ReportePrimario { get; set; }  
 
         [DataMember] 
         public DateTime? FechaIntermedio { get; set; } 
 
         [DataMember] 
         public string ReporteIntermedio { get; set; }  
 
         [DataMember] 
         public DateTime? FechaPullOff { get; set; }  
 
         [DataMember] 
         public string ReportePullOff { get; set; }  
 
         [DataMember] 
         public DateTime? FechaAdherencia { get; set; } 
          
         [DataMember] 
         public string ReporteAdherencia { get; set; }  
 
         [DataMember] 
         public DateTime? FechaAcabadoVisual { get; set; }  
 
         [DataMember] 
         public string ReporteAcabadoVisual { get; set; } 
 
         [DataMember] 
         public bool Materiales{get; set;} 
 
         [DataMember] 
 	     public bool Despachos {get; set;} 
  
         [DataMember] 
 	     public bool Armado {get; set;} 
  
         [DataMember] 
 	     public bool Soldadura {get; set;}   
 
         [DataMember] 
 	     public bool InspeccionVisual {get; set;} 
              
         [DataMember] 
 	     public bool TieneLiberacionDimensional {get; set;} 
          
         [DataMember] 
 	     public int PWHT {get; set;}  
 
         [DataMember] 
 	     public int PND {get; set;}  
 
         [DataMember] 
         public int Pintura { get; set; }  
 
         [DataMember] 
 	     public bool OkMateriales {get; set;}  
 
         [DataMember] 
 	     public bool TienePintura {get; set;}  
 
         [DataMember] 
 	     public bool OkCalidad {get; set;}   
 
         [DataMember] 
 	     public bool OkPreparacionEmbarque {get; set;} 
     } 
 } 
