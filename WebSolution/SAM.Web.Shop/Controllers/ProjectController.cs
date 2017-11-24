using System.Web.Mvc;
using SAM.Web.Shop.Utils;

namespace SAM.Web.Shop.Controllers
{
    public class ProjectController : AuthenticatedController
    {
      
        public ProjectController(INavigationContext navContext) : base(navContext)
        {
        }

        [HttpGet]
        public ActionResult Index(int projectId)
        {
            return View();
        }
	}
}