using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using SAM.Web.Classes;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;

namespace SAM.Web.Controles.Login
{
    public partial class PerfilLogin : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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