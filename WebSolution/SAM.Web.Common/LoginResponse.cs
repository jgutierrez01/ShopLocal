using SAM.Entities;

namespace SAM.Web.Common
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public Usuario User { get; set; }
    }
}
