using System.Web.Mvc;
using System.Web.Security;
using SAM.Web.Common;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Utils;


namespace SAM.Web.Shop.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(INavigationContext navContext) : base(navContext) { }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                LoginResponse response = LoginExtensions.TryLogin(model.Username, model.Password);


                if (response.Success)
                {
                    SessionFacade.Inicializa(response.User);
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);


                    return RedirectToAction("Index", "Home");
                }


                ModelState.AddModelError(string.Empty, response.ErrorMessage);
            }


            return View();
        }


        [HttpGet]
        public ActionResult Logout()
        {
            return View("Index");
        }
    }
}
