using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;

namespace SAM.Web.Produccion
{
    public partial class MensajeExitoProd : SamPaginaConSeguridad
    {
        protected override void OnLoad(EventArgs e)
        {
            AllowBothProtocols = true;
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MensajeExitoUI mensaje = (MensajeExitoUI)HttpContext.Current.Items[WebConstants.Contexto.CTX_MENSAJE];

                lblTitulo.Text = mensaje.Titulo;
                lblCuerpo.Text = mensaje.CuerpoMensaje;
                repLigas.DataSource = mensaje.Ligas;
                repLigas.DataBind();
            }
        }
    }
}