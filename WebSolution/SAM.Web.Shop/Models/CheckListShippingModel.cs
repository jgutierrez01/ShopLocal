using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Web; 
 
 
 namespace SAM.Web.Shop.Models 
 { 
     public class CheckListShippingModel 
     {
        public int? SpoolId { get; set; }   
        public string Hold { get; set; }  
        public bool? Materials { get; set; }  
        public bool? Dispatch { get; set; }  
        public bool? Fitting { get; set; }  
        public int? Welding { get; set; } 
        public bool? VisualInspection { get; set; }  
        public bool? DimensionalInspection { get; set; } 
        public int? PND { get; set; } 
        public int? PWHT { get; set; } 
        public int? Paint { get; set; } 
        public string SistemaPintura { get; set; } 
        public bool? OKMIL { get; set; }  
        public bool? OkPaint { get; set; }                
        public bool? OkQuality { get; set; }  
        public bool? PreparationShipping { get; set; }   
 
        public CheckListShippingModel() 
        { 
        } 
 
     } 
 } 
