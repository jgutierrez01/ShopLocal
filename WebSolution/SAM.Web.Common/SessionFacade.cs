using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using System.Collections.Generic;

namespace SAM.Web.Common
{
    public static class SessionFacade
    {

        /// <summary>
        /// 
        /// </summary>
        private static HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }


        #region Security


        public static bool EstaLoggeado
        {
            get
            {
                return Session["Session.UserId"] != null;
            }
        }

        public static Guid UserId
        {
            get
            {
                return (Guid)Session["Session.UserId"];
            }
            set
            {
                Session["Session.UserId"] = value;
            }
        }

        public static string Username
        {
            get
            {
                return Session["Session.Username"].ToString();
            }
            set
            {
                Session["Session.Username"] = value;
            }
        }

        public static int? PerfilID
        {
            get
            {
                return (int ?)Session["Session.PerfilID"];
            }
            set
            {
                Session["Session.PerfilID"] = value;
            }        
        }

        public static bool EsAdministradorSistema
        {
            get
            {
                return (bool)Session["Session.EsAdministradorSistema"];
            }
            set
            {
                Session["Session.EsAdministradorSistema"] = value;
            }
        }

        public static string Nombre
        {
            get
            {
                return Session["Session.Nombre"].ToString();
            }
            set
            {
                Session["Session.Nombre"] = value;
            }
        }

        public static string ApellidoPaterno
        {
            get
            {
                return Session["Session.ApellidoPaterno"].ToString();
            }
            set
            {
                Session["Session.ApellidoPaterno"] = value;
            }
        }

        public static string NombreCompleto
        {
            get
            {
                return string.Format("{0} {1}", Nombre, ApellidoPaterno);
            }
        }

        public static int[] ProyectosConPermiso
        {
            get
            {
                return (int [])Session["Session.ProyectosConPermiso"];
            }
            set
            {
                Session["Session.ProyectosConPermiso"] = value;
            }
        }

        public static bool PermisoEdicionesEspeciales
        {
            get
            {
                return (bool)Session["Session.PermisoEdicionesEspeciales"];
            }
            set
            {
                Session["Session.PermisoEdicionesEspeciales"] = value;
            }
        }

        public static bool PermisoEdicionesLimitadaTubero
        {
            get
            {
                return (bool)Session["Session.PermisoEdicionesLimitadaTubero"];
            }
            set
            {
                Session["Session.PermisoEdicionesLimitadaTubero"] = value;
            }
        }

        public static bool PermisoEdicionesLimitadaSoldador
        {
            get
            {
                return (bool)Session["Session.PermisoEdicionesLimitadaSoldador"];
            }
            set
            {
                Session["Session.PermisoEdicionesLimitadaSoldador"] = value;
            }
        }

        #endregion  

        /// <summary>
        /// Inicializa las variables de sesión requeridas para trabajar en el sistema.
        /// </summary>
        /// <param name="usuario">Objeto de usuario de base de datos</param>
        public static void Inicializa(Usuario usuario)
        {
            UserId = usuario.UserId;
            Username = usuario.Username;
            Nombre = usuario.Nombre;
            ApellidoPaterno = usuario.ApPaterno;
            PerfilID = usuario.PerfilID;
            Session["PerfilID"] = PerfilID == null ? "0" : PerfilID.ToString();
            EsAdministradorSistema = usuario.EsAdministradorSistema;

            if (!usuario.EsAdministradorSistema)
            {
                List<Proyecto> proyectos = ProyectoBO.Instance.ObtenerPorUsuario(usuario.UserId);
                ProyectosConPermiso = (from p in proyectos
                                       select p.ProyectoID).ToArray();

                PermisoEdicionesEspeciales = UsuarioBO.Instance.ObtenerPermisosEdicionesEspeciales(PerfilID);
                PermisoEdicionesLimitadaTubero = UsuarioBO.Instance.ObtenerPermisosEdicionesLimitadaTubero(PerfilID);
                PermisoEdicionesLimitadaSoldador = UsuarioBO.Instance.ObtenerPermisosEdicionesLimitadaSoldador(PerfilID);
            }
            else
            {
                PermisoEdicionesEspeciales = true;
                PermisoEdicionesLimitadaTubero = true;
                PermisoEdicionesLimitadaSoldador = true;

            }

        }
    }
}
