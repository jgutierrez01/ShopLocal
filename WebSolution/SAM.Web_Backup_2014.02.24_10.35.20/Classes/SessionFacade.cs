using System;
using System.Web;
using System.Web.SessionState;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using System.Linq;

namespace SAM.Web.Classes
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
                //return String.Empty;
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
                //return String.Empty;
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
            EsAdministradorSistema = usuario.EsAdministradorSistema;

            if (!usuario.EsAdministradorSistema)
            {
                ProyectosConPermiso = (from p in ProyectoBO.Instance.ObtenerPorUsuario(usuario.UserId)
                                       select p.ProyectoID).ToArray();
            }
        }
    }
}
