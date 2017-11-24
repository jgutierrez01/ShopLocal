using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using System.Web.Security;
using Mimo.Framework.Cryptography;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using SAM.Common.Membership;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Common;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Extensions;
using SAM.Web.App_LocalResources;
using Resources;

namespace SAM.Web
{
    public partial class ActivaCuenta : SamPaginaBase
    {
        protected override void OnLoad(EventArgs e)
        {
            IsSecure = true;
            base.OnLoad(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                regPassword.ValidationExpression = Membership.PasswordStrengthRegularExpression;

                List<PreguntaSecreta> lst = PreguntaSecretaBO.Instance.ObtenerTodas();
                string columna = Cultura != LanguageHelper.INGLES ? "Pregunta" : "PreguntaIngles";
                ddlPregunta.BindToEnumerableWithEmptyRow(lst, columna, columna, null);
                litMsgContrasena.Text = MensajesAplicacion.Seguridad_FormatoContrasena;
            }
        }

        /// <summary>
        /// Revisa que el usuario que se está activando en efecto sea el mismo
        /// que se pasó a través del QS.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cusUsername_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            string qsUsername = Crypter.Decrypt(Request.QueryString["UID"]);
            args.IsValid = string.Equals(qsUsername, txtUsername.Text.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Cambia para permitir que el usuario especifique su propia pregunta secreta o
        /// utilice una de las del sistema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkPropia_CheckChanged(object sender, EventArgs e)
        {
            phSistema.Visible = !chkPropia.Checked;
            phPropia.Visible = chkPropia.Checked;
        }

        /// <summary>
        /// Activa la cuenta del usuario si todos los parámetros son correctos y lo redirecciona
        /// a Login al terminar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActivar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                MembershipUser usr = Membership.GetUser(txtUsername.Text);

                if (usr != null)
                {
                    Usuario usu = UsuarioBO.Instance.Obtener((Guid)usr.ProviderUserKey);

                    try
                    {
                        string pregunta = chkPropia.Checked ? txtPregunta.Text : ddlPregunta.SelectedValue;

                        UsuarioBO.Instance.ActivaCuenta(usu,
                                                        txtPassword.Text.Trim(),
                                                        AuxiliarMembershipProvider.RESPUESTA_SECRETA_DEFAULT,
                                                        pregunta.Trim(),
                                                        txtRespuesta.Text.Trim());
                    }
                    catch (BaseValidationException bve)
                    {
                        RenderErrors(bve);
                    }

                    phControles.Visible = false;
                    phMensaje.Visible = true;
                }
                else
                {
                    RenderErrors(MensajesActivaCuenta.Usuario_Invalido);
                }
            }
        }
    }
}