    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Web.Shop.Models
{
    public class MaterialModel
    {
   
        public string Heat { get; set; }
        public string Certified { get; set; }
        public string ItemCode { get; set; }
        public decimal DiameterOne { get; set; }
        public decimal DiameterTwo { get; set; }
        public int Quantity { get; set; }
        public string Descprition { get; set; }
        public string Manufacturer { get; set; }
        public string Motion { get; set; }
        public string Notes { get; set; }
        public string ItemCodeName { get; set; }
        public string Label { get; set; }
        public string UniqueNumber { get; set; }

        public MaterialModel()
        {
        }
    }
}