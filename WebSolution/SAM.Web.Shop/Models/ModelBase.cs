using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Web.Shop.Models
{
    public class ModelBase
    {
        public Guid? UserModifies { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}