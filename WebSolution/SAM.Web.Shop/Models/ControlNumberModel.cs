using System.ComponentModel.DataAnnotations;
using Resources.Models;
using SAM.Web.Common;

namespace SAM.Web.Shop.Models
{
    public class ControlNumberModel
    {
        [Display(Name = "ControlNumber", ResourceType = typeof(ControlNumberStrings))]
        public string ControlNumber { get; set; }
        
        public int ControlNumberId { get; set; }
        
        public int SpoolId { get; set; }

        [Display(Name = "Drawing", ResourceType = typeof(ControlNumberStrings))]
        public string Drawing { get; set; }

        [Display(Name = "Spool", ResourceType = typeof(ControlNumberStrings))]
        public string Spool { get; set; }

        public JointModel[] Joints { get; set; }
        public CutModel[] Cuts { get; set; }
        public MaterialModel[] Materials { get; set; }
        public DrawingPage[] Drawings { get; set; }
    }
}