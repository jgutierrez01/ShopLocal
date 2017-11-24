using System.Web.Mvc;
using SAM.Web.Shop.Filters;
using SAM.Web.Shop.Utils;

namespace SAM.Web.Shop.Controllers
{
    [Authorize]
    [SessionChecker]
    public class AuthenticatedController : BaseController
    {
        public AuthenticatedController(INavigationContext navContext) : base(navContext)
        {
        }
    }
}