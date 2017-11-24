using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using SAM.BusinessLogic;
using SAM.Entities.Cache;
using System.Web.Security;
using SAM.BusinessObjects.Utilerias;
using SAM.Web.Common;

namespace SAM.Web.Classes
{
    public static class SeguridadWeb
    {
        public static bool UsuarioTieneAcceso(Page pagina)
        {
            string url = pagina.Request.FilePath;
            return UsuarioTieneAcceso(url);
        }

        public static bool UsuarioTieneAcceso(string url)
        {
            if (SessionFacade.EstaLoggeado)
            {
                //No toma en cuenta la totalidad de los casos posibles de Urls, pero no tiene caso
                //hacer parsing de urls, para efectos del SAM esto nos basta
                if (url.Contains("?"))
                {
                    url = url.Substring(0, url.IndexOf('?'));
                }

                string llave = string.Format("UsuarioTieneAcceso_{0}", url);

                if (HttpContext.Current.Items[llave] == null)
                {
                    if (SessionFacade.EsAdministradorSistema)
                    {
                        HttpContext.Current.Items[llave] = true;
                    }
                    else if (SessionFacade.PerfilID.HasValue)
                    {
                        HttpContext.Current.Items[llave] = PerfilTieneAcceso(PerfilUsuarioLoggeado, url);
                    }
                }

                return (bool)HttpContext.Current.Items[llave];
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
            if (CacheCatalogos.Instance
                              .ObtenerPaginas()
                              .Any(y => y.Nombre.Equals(url, StringComparison.InvariantCultureIgnoreCase)))
            {
                //Todos los permisos de la BD
                List<PermisoCache> lstTodos = CacheCatalogos.Instance.ObtenerPermisos();

                return
                    //Solo aquellos permisos del perfil seleccionado
                lstTodos.Where(x => perfil.Permisos.Contains(x.ID))
                    //Buscar en todas las páginas de todos los perfiles si contiene el URL pasado
                        .Any(x => x.Paginas.Any(y => y.Equals(url, StringComparison.InvariantCultureIgnoreCase)));
            }

            //Si la página no se encuentra dada de alta en la BD, regresamos que el usuario si tiene permisos,
            //significa que la página como tal no tiene relevancia en lo que se refiere a permisos
            return true;
        }

        public static bool UsuarioPuedeEditar(string modulo)
        {
            if (!SessionFacade.EsAdministradorSistema)
            {
                PerfilCache perfiles = CacheCatalogos.Instance.ObtenerPerfiles().Where(x => x.ID == SessionFacade.PerfilID.Value).SingleOrDefault();

                List<PermisoCache> lstTodos = CacheCatalogos.Instance.ObtenerPermisos().Where(x => perfiles.Permisos.Contains(x.ID)).ToList();

                return lstTodos.Any(x => x.Nombre == modulo);
            }
            else
            {
                return true;
            }
        }

        public static void LogoutImmediately()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Response.Redirect(WebConstants.PublicUrl.LOGIN);
        }
    }
}