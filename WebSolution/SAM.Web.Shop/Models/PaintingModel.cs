using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SAM.Web.Shop.Models
{
    public class PaintingModel : ModelBase
    {
        public int ProjectID { get; set; }
        public string NumberControl { get; set; }
        public string Spool { get; set; }


        public string NumberRequisition { get; set; }
        public DateTime DateSRequisition { get; set; }
        public int TypeReport { get; set; }
    }
}
