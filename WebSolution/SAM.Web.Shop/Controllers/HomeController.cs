using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SAM.Web.Common;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Utils;

namespace SAM.Web.Shop.Controllers
{
    public class HomeController : AuthenticatedController
    {
        public HomeController(INavigationContext navContext) : base(navContext)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<YardModel> model = UserScope.MisPatios.OrderBy(y => y.Nombre).ToList().ConvertAll(item => new YardModel
            {
                Name = item.Nombre,
                Owner = item.Propietario,
                YardId = item.ID
            });

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Ping()
        {
            return new EmptyResult();
        }
	}
}