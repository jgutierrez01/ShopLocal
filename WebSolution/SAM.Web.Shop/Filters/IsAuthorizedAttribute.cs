using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAM.Common;
using SAM.BusinessLogic.Utilerias;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Administracion;
using SAM.Entities.Cache;
using SAM.Entities;
using SAM.Web.Common;
using System.Security.Authentication;
using System.Globalization;
using System.Web.Routing;

namespace SAM.Web.Shop.Filters
{
    public class IsAuthorizedAttribute : ActionFilterAttribute
    {
        #region Atributos
        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext.HttpContext.Request.Path;
            bool tienePermiso = false;
            if (SessionFacade.EsAdministradorSistema)
            {
                tienePermiso = true;
            }
            else
            {
                tienePermiso = PermisoBO.Instance.TienePermisoUrl(url, SessionFacade.UserId);
            }

            if (!tienePermiso)
            {
                filterContext.Result = new RedirectToRouteResult(new
                                                RouteValueDictionary(new { controller = "UserAuthorized", action = "Index" }));
            }
        }
    }
}