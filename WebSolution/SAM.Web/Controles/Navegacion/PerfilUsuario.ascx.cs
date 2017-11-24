using System;
using System.Web;
using Mimo.Framework.Common;
using SAM.Web.Classes;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using SAM.Web.Common;

namespace SAM.Web.Controles.Navegacion
{
    public partial class PerfilUsuario : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUsuario.Text = SessionFacade.NombreCompleto;
                perfil_hlSalir.NavigateUrl = string.Concat(    WebConstants.PublicUrl.LOGOUT,
                                                        "?Rnd=",
                                                        Guid.NewGuid().ToString().Replace("-", ""));
                
                lblUsuario.Attributes["onclick"] = string.Format("Sam.Usuarios.AbreContextMenu(event,'{0}');", menuUsuario.ClientID);
                hlAyuda.Attributes["onclick"] = string.Format("Sam.Usuarios.AbreContextMenu(event,'{0}');", menuAyuda.ClientID);
                menuUsuario.Items[0].NavigateUrl = "javascript:Sam.Usuarios.AbrePopupCambiarPassword();";
                menuUsuario.Items[1].NavigateUrl = "javascript:Sam.Usuarios.AbrePopupCambiarPregunta();";

                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    menuAyuda.Items[0].NavigateUrl = "~/Ayuda/en-US/WebHelp/index.htm";
                    menuAyuda.Items[1].NavigateUrl = "~/Ayuda/en-US/DeskHelp/help.chm";
                }
                else
                {
                    menuAyuda.Items[0].NavigateUrl = "~/Ayuda/es-MX/WebHelp/index.htm";
                    menuAyuda.Items[1].NavigateUrl = "~/Ayuda/es-MX/DeskHelp/help.chm";
                }
                
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
        /// Cambia el idioma a español en caso de ser necesario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Espanol_Click(object sender, EventArgs e)
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
                Usuario usu = UsuarioBO.Instance.Obtener(SessionFacade.UserId);
                usu.UsuarioModifica = SessionFacade.UserId;
                usu.FechaModificacion = DateTime.Now;
                UsuarioBO.Instance.Guarda(usu);
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString());
            }
        }

        
    }
}