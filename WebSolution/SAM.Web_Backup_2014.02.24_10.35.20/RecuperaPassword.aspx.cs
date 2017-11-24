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
using SAM.BusinessLogic.Utilerias;
using SAM.Web.App_LocalResources;

namespace SAM.Web
{
    public partial class RecuperaPassword : SamPaginaBase
    {
        protected override void OnLoad(EventArgs e)
        {
            IsSecure = true;
            base.OnLoad(e);
        }


        /// <summary>
        /// Verifica el nombre de usuario, en caso de ser válido
        /// prosigue al paso dos en el cual se le pide al usuario que
        /// conteste su respuesta secreta para poder entrar al sistema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                MembershipUser user = Membership.GetUser(txtUsuario.Text);

                if (user != null)
                {
                    Usuario usu = UsuarioBO.Instance.Obtener((Guid)user.ProviderUserKey);

                    if (usu.BloqueadoPorAdministrador)
                    {
                        RenderErrors(MensajesRecuperaPassword.Cuenta_EstaBloqueada);
                    }
                    else if (user.IsLockedOut)
                    {
                        RenderErrors(MensajesRecuperaPassword.Cuenta_HaSidoBloqueada);
                    }
                    else if (!user.IsApproved)
                    {
                        RenderErrors(MensajesRecuperaPassword.Cuenta_NoActivada);
                    }
                    else
                    {
                        txtRespuesta.Label = user.PasswordQuestion;
                        btnRecupera.Visible = phSecreto.Visible = true;
                        btnSiguiente.Visible = txtUsuario.Enabled = false;
                    }
                }
                else
                {
                    RenderErrors(MensajesRecuperaPassword.Usuario_NoExiste);
                }
            }
        }

        /// <summary>
        /// Una vez que se validó el nombre de usuario este método revisa que
        /// se conteste correctamente la pregunta secreta.
        /// En caso de ser así, se envía el correo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecupera_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                MembershipUser user = Membership.GetUser(txtUsuario.Text);

                if (user.IsLockedOut)
                {
                    RenderErrors(MensajesRecuperaPassword.Cuenta_HaSidoBloqueada);
                    return;
                }

                try
                {
                    string password = user.GetPassword(txtRespuesta.Text);

                    Usuario usu = UsuarioBO.Instance.Obtener((Guid)user.ProviderUserKey);
                    EnvioCorreos.Instance.EnviaCorreoOlvidoPassword(usu, password);
                    phControles.Visible = false;
                    phMensaje.Visible = true;

                }
                catch (MembershipPasswordException)
                {
                    RenderErrors(MensajesRecuperaPassword.RespuestaIncorrecta_SeBloquearaAlTercerIntento);
                }
            }
        }
    }
}