using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.UI;
using Resources.Scripts;
using SAM.Web.Shop.Utils;

namespace SAM.Web.Shop.Controllers
{
    public class LocalesController : BaseController
    {
        public LocalesController(INavigationContext navContext) : base(navContext){}

        [HttpGet]
        [OutputCache(Duration = 2592000, VaryByParam = "*", Location = OutputCacheLocation.Any)]
        public ActionResult Index(string lng, string ns)
        {
            string cultureName = lng.StartsWith("en", StringComparison.OrdinalIgnoreCase) ? "en-US" : "es-MX";
            CultureInfo culture = new CultureInfo(cultureName);

            return Json(new
            {
                Messages = new
                {
                    PleaseWait = CommonStrings.ResourceManager.GetString("PleaseWait", culture)
                }
            }, JsonRequestBehavior.AllowGet);
        }
	}
}