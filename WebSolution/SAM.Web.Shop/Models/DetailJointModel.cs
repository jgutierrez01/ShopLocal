﻿using System; 
using System.Collections.Generic;  
 using System.Linq; 
using System.Web; 
   
namespace SAM.Web.Shop.Models 
{ 
    public class DetailJointModel 
    { 
        public string Joint { get; set; } 
        public string TypeJoint { get; set; } 
        public decimal Diameter { get; set; } 
        public string Cedula { get; set; } 
        public DateTime? DateFitting { get; set; } 
        public DateTime? DateWelding { get; set; } 
        public string WPS { get; set; } 
        public string WelderRoot { get; set; } 
        public string WelderFiller { get; set; } 
        public DateTime? DateVisualInspection { get; set; } 
        public string ResultVisualInspection { get; set; } 
        public string UniqueNumberOne { get; set; } 
        public string UniqueNumberTwo { get; set; }    
        public string RT { get; set; } 
        public string PT { get; set; } 
        public string RequiredTestNeumatic{ get; set; }
        public string TestNeumatic { get; set; } 
        public string PMI { get; set; }
        public string RequiredPWHT { get; set; }
        public string PWHT { get; set; } 
        public string Hardness { get; set; } 
        public string Fitting { get; set; } 
        public string Welding { get; set; }
        public int? JuntaWorkStatusId { get; set; }
 
        public string FittingDate 
        { 
            get { return DateFitting.HasValue ? DateFitting.Value.ToShortDateString(): string.Empty;} 
        } 
          
        public string WeldingDate 
        { 
            get { return DateWelding.HasValue ? DateWelding.Value.ToShortDateString() : string.Empty; } 
        }  
 
        public string VIDate 
        { 
            get { return DateVisualInspection.HasValue ? DateVisualInspection.Value.ToShortDateString() : string.Empty; } 
        } 
 
        public DetailJointModel() 
        { 
        }    
    } 
}
