using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.MobileControls;
using SAM.BusinessLogic;
using SAM.Entities.Cache;
using System.Web.Security;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Mobile.Clases
{
    public static class SeguridadWeb
    {
        public static bool UsuarioTieneAcceso(MobilePage pagina)
        {
            string url = pagina.Request.FilePath;
            return UsuarioTieneAcceso(url);
        }

        public static bool UsuarioTieneAcceso(string url)
        {
            if (SessionFacade.EstaLoggeado)
            {
                if (SessionFacade.EsAdministradorSistema)
                {
                    return true;
                }

                if (SessionFacade.PerfilID.HasValue)
                {
                    return PerfilTieneAcceso(PerfilUsuarioLoggeado, url);
                }
            }

            return false;
        }

        private static PerfilCache PerfilUsuarioLoggeado
        {
            get
            {
                return CacheCatalogos.Instance.ObtenerPerfiles().Where(x => x.ID == SessionFacade.PerfilID.Value).Single();
            }
        }

        private static bool PerfilTieneAcceso(PerfilCache perfil, string url)
        {
            //Todos los permisos de la BD
            List<PermisoCache> lstTodos = CacheCatalogos.Instance.ObtenerPermisos();


            return
                //Solo aquellos permisos del perfil seleccionado
            lstTodos.Where(x => perfil.Permisos.Contains(x.ID))

            //Buscar en todas las páginas de todos los perfiles si contiene el URL pasado
                    .Any(x => x.Paginas.Any(y => y.Equals(url, StringComparison.InvariantCultureIgnoreCase)));
        }

        public static void LogoutImmediately()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Response.Redirect(WebConstants.PublicUrl.LOGIN);
        }
    }
}