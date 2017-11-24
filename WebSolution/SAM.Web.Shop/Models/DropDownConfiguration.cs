using System.Web.Mvc;

namespace SAM.Web.Shop.Models
{    
    public class DropDownConfiguration
    {
        public string Name { get; set; }
        public string LabelText { get; set; }
        public bool IsRequired { get; set; }
        public string RequiredErrorMessage { get; set; }
        public string SelectedId { get; set; }
        public bool IncludeEmptyOption { get; set; }
        public bool PreselectIfOnlyOne { get; set; }
        public bool HasErrors { get; set; }
        public ModelErrorCollection Errors { get; set; }
    }
}