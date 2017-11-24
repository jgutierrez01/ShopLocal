﻿using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Web; 
   
namespace SAM.Web.Shop.Models 
{ 
    public class CertificationReportModel 
    { 
        public DetailSpoolModel DetailSpool { get; set; } 
        public List<DetailJointModel> DetailJoints { get; set; } 
        public List<MaterialModel> DetailMaterials{ get; set; }
        public CheckListShippingModel DetailCheckListShipping { get; set; }
        public string Yard { get; set; }
        public string NumberControl {get; set;}
        public string Project { get; set; }

        public CertificationReportModel() 
        { 
        } 
    } 
}
