using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Administracion;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Excepciones;
using SAM.Web.Common;

namespace SAM.Web.Usuarios
{
    public partial class CambiaPregunta : SamPaginaPopup
    {
        protected override void OnLoad(EventArgs e)
        {
            //Todos pueden entrar a esta página, siempre y cuando estén loggeados
            RevisarSeguridad = false;
            IsSecure = true;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<PreguntaSecreta> lst = PreguntaSecretaBO.Instance.ObtenerTodas();
                string columna = Cultura != LanguageHelper.INGLES ? "Pregunta" : "PreguntaIngles";
                ddlPregunta.BindToEnumerableWithEmptyRow(lst, columna, columna, null);
            }
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCambiar_Click(object sender, EventArgs e)
        {
            try
            {
                string pregunta = chkPropia.Checked ? txtPregunta.Text : ddlPregunta.SelectedValue;

                UsuarioBO.Instance.CambiaPreguntaRespuestaSecreta(SessionFacade.UserId,
                                                                  pregunta.Trim(),
                                                                  txtRespuesta.Text.Trim(),
                                                                  txtPassword.Text,
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