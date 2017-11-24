using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mobile;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using SAM.Mobile.Clases;
using System.Resources;
using System.Globalization;
using System.Web.Security;
using SAM.Entities;
using SAM.BusinessObjects.Administracion;
using Resources;



namespace SAM.Mobile
{
    public partial class Login : PaginaMovil
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblError.Text = String.Empty;
                //txtPassword.Attributes.Add("onkeypress", "javascript:myFunction()");
            }
            
        }

        /// <summary>
        /// Revisa las credenciales del usuario, para darle o no acceso a la aplicación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_OnClick(object sender, EventArgs e)
        {
            lblError.Text = String.Empty;
            string username = txtUsuario.Text;
            string password = txtPassword.Text;

            if (username != String.Empty && password != String.Empty)
            {
                //Obtener el usuario
                MembershipUser user = Membership.GetUser(username);

                if (user == null)
                {
                    lblError.Text = ErroresMobile.Error_UsuarioPasswordInvalido;
                    return;
                }

                //Validar password vs usuario para ver si lo vamos a dejar entrar
                bool passwordValido = Membership.ValidateUser(username, password);

                //Revisar si el usuario está bloqueado por muchos intentos de password
                if (user.IsLockedOut)
                {
                    lblError.Text = ErroresMobile.Error_CuentaDesactivadaPorFallos;
                    return;
                }

                //Revisar password
                if (!passwordValido)
                {
                    lblError.Text = ErroresMobile.Error_UsuarioPasswordInvalido;
                    return;
                }

                //Revisar si la cuenta ya fue activada
                if (!user.IsApproved)
                {
                    Usuario usu = UsuarioBO.Instance.Obtener((Guid)user.ProviderUserKey);

                    if (usu.BloqueadoPorAdministrador)
                    {
                        lblError.Text = ErroresMobile.Error_CuentaDesactivada;
                        return;
                    }

                    lblError.Text = ErroresMobile.Error_CuentaNoActivada;
                    return;
                }

                //Usuario Aprobado
                Usuario usuario = UsuarioBO.Instance.Obtener((Guid)user.ProviderUserKey);

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

                FormsAuthentication.SetAuthCookie(usuario.Username, false);
                MobileFormsAuthentication.RedirectFromLoginPage(usuario.Username, false);
            }
            else
            {
                lblError.Text = ErroresMobile.Error_UsuarioPasswordRequerido;
            }
        }



        /// <summary>
        /// Cambia el idioma a inglés en caso de ser necesario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ingles_Click(object sender, EventArgs e)
        {
            cambiaIdioma(LanguageHelper.INGLES);
        }

        /// <summary>
        /// Cambia el idioma a inglés en caso de ser necesario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CambioIngles(object sender, EventArgs e)
        {
            cambiaIdioma(LanguageHelper.INGLES);
        }

        /// <summary>
        /// Cambia el idioma a español en caso de ser necesario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Espanol_Click(object sender, EventArgs e)
        {
            cambiaIdioma(LanguageHelper.ESPANOL);
        }

        /// <summary>
        /// Cambia el idioma a español en caso de ser necesario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CambioEspanol(object sender, EventArgs e)
        {
            cambiaIdioma(LanguageHelper.ESPANOL);
        }

        /// <summary>
        /// Si el usuario decide cambiar al idioma de la aplicación, actualizamos la BD
        /// para que ese quede como se idioma default.
        /// </summary>
        /// <param name="nuevoIdioma">Nuevo idioma a utilizarse de ahora en adelante hasta que sea cambiado nuevamente</param>
        private void cambiaIdioma(string nuevoIdioma)
        {
            string idiomaActual = LanguageHelper.CustomCulture;
            LanguageHelper.CustomCulture = nuevoIdioma;

            if (idiomaActual != nuevoIdioma)
            {
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString());
            }
        }
    }
}