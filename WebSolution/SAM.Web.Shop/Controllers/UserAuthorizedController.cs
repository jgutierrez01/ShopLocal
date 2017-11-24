using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using Resources.Controllers;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Busqueda;
using SAM.Entities.Cache;
using SAM.Web.Common;
using SAM.Web.Shop.Filters;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Utils;

namespace SAM.Web.Shop.Controllers
{
    public class UserAuthorizedController : AuthenticatedController
    {
        public UserAuthorizedController(INavigationContext navContext) : base(navContext) { }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MensajeError = CultureInfo.CurrentCulture.Name == "en-US" ? "Not have the necessary permissions to perform this action." 
                : "No cuentas con los permisos necesarios para llevar a cabo esta acción.";
            return View();
        }
    }
}