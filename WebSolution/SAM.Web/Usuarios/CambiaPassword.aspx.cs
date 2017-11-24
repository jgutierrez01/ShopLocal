using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using System.Web.Security;
using SAM.BusinessObjects.Administracion;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Excepciones;
using Resources;
using SAM.Web.Common;

namespace SAM.Web.Usuarios
{
    public partial class CambiaPassword : SamPaginaPopup
    {
        protected override void OnLoad(EventArgs e)
        {
            //todos pueden entrar a esta página
            IsSecure = true;
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                regPassword.ValidationExpression = Membership.PasswordStrengthRegularExpression;
                litMsgContrasena.Text = MensajesAplicacion.Seguridad_FormatoContrasena;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCambiar_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBO.Instance.CambiarContraseña(SessionFacade.UserId,
                                                     txtPasswordActual.Text,
                                                     txtPasswordNuevo.Text,
                                                     SessionFacade.UserId);

                phControles.Visible = false;
                phMensaje.Visible = true;
            }
            catch (UsuarioBloqueadoException)
            {
                JsUtils.RegistraScriptLogoutDesdePopup(this);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }
    }
}