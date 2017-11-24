using System.Web.Mvc;
using SAM.Web.Shop.Utils;

namespace SAM.Web.Shop.Controllers
{
    public class BaseController : Controller
    {
        protected INavigationContext NavContext;

        public BaseController(INavigationContext navContext)
        {
            NavContext = navContext;
        }
    }
}