using System.ComponentModel.DataAnnotations;
using Resources.Models;

namespace SAM.Web.Shop.Models
{
    public class LoginModel
    {
        [Display(Name = "Username_DisplayName", ResourceType = typeof(LoginStrings))]
        [Required(ErrorMessageResourceName = "Username_Required_ErrorMessage", ErrorMessageResourceType = typeof(LoginStrings))]
        [StringLength(200, ErrorMessageResourceName = "Username_MaxLength_ErrorMessage", ErrorMessageResourceType = typeof(LoginStrings))]
        public string Username { get; set; }

        [Display(Name = "Password_DisplayName", ResourceType = typeof(LoginStrings))]
        [Required(ErrorMessageResourceName = "Password_Required_ErrorMessage", ErrorMessageResourceType = typeof(LoginStrings))]
        [StringLength(200, ErrorMessageResourceName = "Password_MaxLength_ErrorMessage", ErrorMessageResourceType = typeof(LoginStrings))]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}