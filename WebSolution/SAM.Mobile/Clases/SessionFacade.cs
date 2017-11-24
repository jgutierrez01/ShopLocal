using System;
using System.Web;
using System.Web.SessionState;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using System.Linq;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;

namespace SAM.Mobile.Clases
{
    public class SessionFacade
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
                return (int?)Session["Session.PerfilID"];
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
                return string.Format("{0}, {1}", ApellidoPaterno, Nombre);
            }
        }

        public static String PatioNombre
        {
            get
            {
                if (Session["Session.PatioNombre"] != null)
                    return Session["Session.PatioNombre"].ToString();
                return null;
            }
            set
            {
                Session["Session.PatioNombre"] = value;
            }
        }

        public static int? PatioID
        {
            get
            {
                if (Session["Session.PatioID"] != null)
                    return Session["Session.PatioID"].SafeIntParse();
                return -1;
            }
            set
            {
                Session["Session.PatioID"] = value;
            }
        }

        public static String CambioProyectoSigURL
        {
            get
            {
                if (Session["Session.SiguienteURL"] != null)
                    return Session["Session.SiguienteURL"].ToString();
                return null;
            }
            set
            {
                Session["Session.SiguienteURL"] = value;
            }
        }

        public static int[] ProyectosConPermiso
        {
            get
            {
                return (int[])Session["Session.ProyectosConPermiso"];
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