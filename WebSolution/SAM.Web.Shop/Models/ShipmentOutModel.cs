using System.ComponentModel.DataAnnotations;


using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAM.Web.Shop.Resources.Models.Workstatus;
using SAM.BusinessObjects.Workstatus;


namespace SAM.Web.Shop.Models
{
    public class ShipmentOutModel
    {
        //[Range(1, Int32.MaxValue, ErrorMessageResourceName = "Yard_Required_ErrorMessage",ErrorMessageResourceType = typeof(WorkstatusString))]
        public int YardId { get; set; }

        //[Range(1, Int32.MaxValue, ErrorMessageResourceName = "Shipment_Required_ErrorMessage", ErrorMessageResourceType = typeof(WorkstatusString))]       
        public int ShipmentId { get; set; }


        public DateTime DateReal
        {
            get
            {
                return DateTime.Now;
            }
        }


        public List<Simple> Yards
        {
            get
            {
                List<Simple> Yards = new List<Simple>();
                Yards.Add(new Simple() { ID = 0, Valor = "Select-One" });
                Yards.AddRange(UserScope.MisPatios.OrderBy(y => y.Nombre).ToList().ConvertAll(item => new Simple
                {
                    ID = item.ID,
                    Valor = item.Nombre
                }));
                return Yards;
            }
        }
        public bool Exit { get; set; }
    }
}
