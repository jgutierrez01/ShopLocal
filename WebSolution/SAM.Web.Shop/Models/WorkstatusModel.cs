using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAM.Entities.Cache;
using System.ComponentModel.DataAnnotations;
using SAM.Web.Shop.Validators;
using System.Web.Script.Serialization;
using SAM.Entities.Busqueda;
using SAM.Web.Shop.Resources.Views.WorkStatus;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using System.Web.Mvc;
using System.Globalization;

namespace SAM.Web.Shop.Models
{
    public class WorkstatusModel
    {
        public string ProcessNumber { get; set; }

        public string ProcessDate { get; set; }

        public List<NumeroControlBusqueda> ControlNumberInvalidDate { get; set; }   

        public List<NumeroControlBusqueda> ControlNumberNotConditions { get; set; }

        public List<NumeroControlBusqueda> ControlNumberToProcess { get; set; }

        public List<NumeroControlBusqueda> ControlNumberWhitProcess { get; set; }

        public int ProjectId { get; set; }

        public int TypeReportId { get; set; }

        public WorkstatusModel()
        {
        }

        public WorkstatusModel(string numberProcess, string dateProcess, string projectId, string typeReportId)
        {
            this.ProcessNumber = numberProcess;
            this.ProjectId = string.IsNullOrEmpty(projectId) ? 0 : Convert.ToInt32(projectId);

            if (!string.IsNullOrEmpty(dateProcess))
            {
                this.ProcessDate = dateProcess;
            }
            else
            {
                this.ProcessDate = DateTime.Now.ToShortDateString();
            }

            this.TypeReportId = string.IsNullOrEmpty(typeReportId) ? 0 : Convert.ToInt32(typeReportId);
        }
    }
}
