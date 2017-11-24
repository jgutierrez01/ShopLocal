﻿using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Web; 
   
namespace SAM.Web.Shop.Models 
{
    public class DetailSpoolModel 
    { 
        public String Isometric { get; set; } 
        public string Hold { get; set; } 
        public DateTime? DateHold { get; set; } 
        public int Joints { get; set; } 
        public decimal Weight { get; set; } 
        public decimal Area { get; set; } 
        public decimal PDI { get; set; } 
        public decimal? PEQS { get; set; }  
        public string CustomerReview { get; set; }   
        public string Specification { get; set; } 
        public Int32 PercentagePND { get; set; } 
        public string DimensionalLiberation { get; set; }
        public string RequiredPWHT { get; set; }
        public string RequiredTestHydrostatic { get; set; } 
        public string TestHydrostatic { get; set; } 
        public string NumberShipment { get; set; } 
        public DateTime? DateShipment { get; set; } 
        public string Joint { get; set; }  
        public string System { get; set; } 
        public string Color { get; set; }  

        public DateTime? FechaPrimario { get; set; }         
        
        public string ReportePrimario { get; set; } 
        
        public DateTime? FechaIntermedio { get; set; } 
        
        public string ReporteIntermedio { get; set; } 
        
        public DateTime? FechaPullOff { get; set; } 
        
        public string ReportePullOff { get; set; } 
        
        public DateTime? FechaAdherencia { get; set; }         
        
        public string ReporteAdherencia { get; set; } 
        
        public DateTime? FechaAcabadoVisual { get; set; } 
        
        public string ReporteAcabadoVisual { get; set; } 
           
 	    public bool Materiales{get; set;} 
          
 	    public bool Despachos {get; set;} 
          
 	    public bool Armado {get; set;} 
          
 	    public bool Soldadura {get; set;}  
          
 	    public bool InspeccionVisual {get; set;}             
          
 	    public bool TieneLiberacionDimensional {get; set;}         
          
 	    public int PWHT {get; set;} 
          
 	    public int PND {get; set;} 
          
        public int Pintura { get; set; } 
          
 	    public bool OkMateriales {get; set;} 
          
 	    public bool TienePintura {get; set;} 
          
 	    public bool OkCalidad {get; set;}  
          
 	    public bool OkPreparacionEmbarque {get; set;} 
      
        public string HoldDate 
        {  
            get  
            { 
                return DateHold.HasValue ? DateHold.Value.ToShortDateString() : string.Empty;             
            }  
        }  
 
        public string PrimariotDate 
        { 
            get 
            { 
                return FechaPrimario.HasValue ? FechaPrimario.Value.ToShortDateString() : string.Empty; 
            } 
        }  
 
        public string IntermedioDate 
        { 
            get 
            { 
                return FechaIntermedio.HasValue ? FechaIntermedio.Value.ToShortDateString() : string.Empty; 
            } 
        } 

        public string ShipmentDate 
        { 
            get 
            { 
                return DateShipment.HasValue ? DateShipment.Value.ToShortDateString() : string.Empty; 
            } 
        }  
 
        public string PullOffDate 
        { 
            get 
            { 
                return FechaPullOff.HasValue ? FechaPullOff.Value.ToShortDateString() : string.Empty; 
            } 
        }  
 
        public string AdherenciaDate 
        { 
            get 
            { 
                return FechaAdherencia.HasValue ? FechaAdherencia.Value.ToShortDateString() : string.Empty; 
            } 
        }  
 
        public string AcabadoVisualDate 
        { 
            get 
            { 
                return FechaAcabadoVisual.HasValue ? FechaAcabadoVisual.Value.ToShortDateString() : string.Empty; 
            } 
        }         
    } 
} 
 
