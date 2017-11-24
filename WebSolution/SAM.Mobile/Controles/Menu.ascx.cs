using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using SAM.Mobile.Clases;
using System.Web.Security;

namespace SAM.Mobile.Controles
{
    public partial class Menu : MobileUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hypMenu.NavigateUrl = string.Concat(WebConstants.PublicUrl.LOGOUT,
                                                        "?Rnd=",
                                                        Guid.NewGuid().ToString().Replace("-", ""));

                hypMenu.NavigateUrl = WebConstants.MobileUrl.DASHBOARD;

                if (SessionFacade.PatioID != null || SessionFacade.PatioID > 0)
                {
                    lblPatio.Text = SessionFacade.PatioNombre;
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
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
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString());
            }
        }
    }
}