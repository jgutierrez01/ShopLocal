using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using System.Web.Security;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using Mimo.Framework.Common;
using SAM.Web.App_LocalResources;

namespace SAM.Web
{
    public partial class Login : SamPaginaBase
    {
        protected override void OnLoad(EventArgs e)
        {
            IsSecure = true;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((TextBox)login.FindControl("UserName")).Focus();
        }

        /// <summary>
        /// Inicializar variables de sesión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void login_LoggedIn(object sender, EventArgs e)
        {
            string username = ((TextBox)login.FindControl("UserName")).Text;

            MembershipUser mUser = Membership.GetUser(username);

            Usuario usuario = UsuarioBO.Instance.Obtener((Guid)mUser.ProviderUserKey);

            string culturaCookie = LanguageHelper.CustomCulture;

            //Si no existe el cookie lo creamos con el default del usuario
            if (string.IsNullOrEmpty(culturaCookie))
            {
                LanguageHelper.CustomCulture = usuario.Idioma;
            }
            //Si el cookie trae algo distinto a lo que el usuario tiene, manda el cookie y actualizamos el registro del usuario
            else if (culturaCookie != usuario.Idioma)
            {
                usuario.Idioma = culturaCookie;
                usuario.FechaModificacion = DateTime.Now;
                usuario.UsuarioModifica = usuario.UserId;
                UsuarioBO.Instance.Guarda(usuario);
            }

            SessionFacade.Inicializa(usuario);

            Response.Redirect(WebConstants.WorkstatusUrl.DEFAULT);
        }

        /// <summary>
        /// Revisar manualmente para mandar mensajes de error más adhoc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void login_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string username = ((TextBox)login.FindControl("UserName")).Text;
            string password = ((TextBox)login.FindControl("Password")).Text;

            //Asumir que no vamos a poder loguearnos
            e.Authenticated = false;

            //Obtener el usuario
            MembershipUser user = Membership.GetUser(username);

            if (user == null)
            {
                RenderErrors(MensajesLogin.Usuario_Password_Invalido,"login");
                return;
            }

            //Validar password vs usuario para ver si lo vamos a dejar entrar
            bool passwordValido = Membership.ValidateUser(username, password);

            //Revisar si el usuario está bloqueado por muchos intentos de password
            if (user.IsLockedOut)
            {
                RenderErrors(MensajesLogin.Password_Reintentos_Excedidos,"login");
                return;
            }

            //Revisar password
            if (!passwordValido)
            {
                RenderErrors(MensajesLogin.Usuario_Password_Invalido,"login");
                return;
            }

            //Revisar si la cuenta ya fue activada
            if (!user.IsApproved)
            {
                Usuario usu = UsuarioBO.Instance.Obtener((Guid)user.ProviderUserKey);

                if (usu.BloqueadoPorAdministrador)
                {
                    RenderErrors(MensajesLogin.Cuenta_Bloqueada,"login");
                    return;
                }

                RenderErrors(MensajesLogin.Cuenta_Falta_Activacion,"login");
                return;
            }

            e.Authenticated = true;
        }
    }
}