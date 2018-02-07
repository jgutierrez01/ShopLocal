
using System.ComponentModel.DataAnnotations;
using Resources.Models;
using SAM.Web.Shop.Validators;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System;
using SAM.Entities.Busqueda;


namespace SAM.Web.Shop.Models
{
   
    public class SearchControlNumberModel
    {
        [Required(ErrorMessageResourceName = "Project_Required_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        public int ProjectId { get; set; }

        [RequiredIf("SearchType", "cn", ErrorMessageResourceName = "Wo_Required_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        [Display(Name = "Wo_DisplayName", ResourceType = typeof(SearchStrings))]
        public int ? WorkOrderNumber { get; set; }


        [RequiredIf("SearchType", "cn", ErrorMessageResourceName = "ControlNumber_Required_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        [Display(Name = "Wo_DisplayName", ResourceType = typeof(SearchStrings))]
        public int ? ControlNumber { get; set; }


        [RequiredIf("SearchType", "sp", ErrorMessageResourceName = "SpoolName_Required_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        [MinLengthIf(2, "SearchType", "sp", ErrorMessageResourceName = "SpoolName_MinLength_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        [MaxLength(100, ErrorMessageResourceName = "SpoolName_MaxLength_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        [Display(Name = "SpoolName_DisplayName", ResourceType = typeof(SearchStrings))]
        public string SpoolName { get; set; }

        [Required(ErrorMessageResourceName = "SearchType_Required_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        [Display(Name="SearchType_DisplayName", ResourceType = typeof(SearchStrings))]
        public string SearchType { get; set; }
        
        public List<NumeroControlBusqueda> Spools { get; set; }     
        public bool Exit { get; set; }
        public string DateProcess {get; set;}
        public string NumberProcess {get; set;}

        public int TypeReportId { get; set; }         
   
        public int QuadrantId { get; set; }
        public int TypeSearch { get; set; }
        public bool Granel { get; set; }

    }
}

 