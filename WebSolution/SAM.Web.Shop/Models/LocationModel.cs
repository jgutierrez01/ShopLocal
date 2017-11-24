
using Resources.Models;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Busqueda;
using SAM.Entities.Personalizadas;
using SAM.Web.Shop.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SAM.Web.Shop.Models
{
    public class LocationModel
    {
        public string ProcessDate { get; set; }       

        public List<NumeroControlBusqueda> ControlNumberWhitProcess { get; set; }
       
        public List<NumeroControlBusqueda> ControlNumberToProcess { get; set; }
          
        public int ProjectId { get; set; }

        public int QuadrantId { get; set; }      

        public LocationModel()
        {
        }

        public LocationModel(string projectId, string quadrantId, string dateProcess)
        {
            this.ProjectId = string.IsNullOrEmpty(projectId) ? 0 : Convert.ToInt32(projectId);         
           
            this.QuadrantId = string.IsNullOrEmpty(quadrantId) ? 0 : Convert.ToInt32(quadrantId);      

            if (!string.IsNullOrEmpty(dateProcess))
            {
                this.ProcessDate = dateProcess;
            }
            else
            {
                this.ProcessDate = DateTime.Now.ToShortDateString();
            }
           
        }       
    }
}

 
 
