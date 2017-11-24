using System;
using System.Security;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using SAM.Web.Common;

namespace SAM.Web.Shop.Filters
{
    public class SessionCheckerAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            IPrincipal principal = filterContext.Principal;

            if (principal != null && principal.Identity.IsAuthenticated && !SessionFacade.EstaLoggeado)
            {
                MembershipUser mUser = Membership.GetUser(principal.Identity.Name);

                if (mUser == null)
                {
                    throw new SecurityException(string.Format("User {0} does not exist!", principal.Identity.Name));
                }

                Usuario user = UsuarioBO.Instance.Obtener((Guid)mUser.ProviderUserKey);

                SessionFacade.Inicializa(user);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}