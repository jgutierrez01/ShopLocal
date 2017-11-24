using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace SAM.Web.Controles.Navegacion
{
    public partial class BarraTituloPagina : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Browsable(true)]
        public string Text
        {
            get
            {
                return lblTitulo.Text;
            }
            set
            {
                lblTitulo.Text = value;
            }
        }

        [Browsable(true)]
        public string NavigateUrl
        {
            get
            {
                return hlRegresar.NavigateUrl;
            }
            set
            {
                hlRegresar.NavigateUrl = value;
                hlRegTxt.NavigateUrl = value;
            }
        }

        [Browsable(true)]
        public bool BotonRegresarVisible
        {
            get
            {
                return hlRegresar.Visible;
            }
            set
            {
                hlRegresar.Visible = value;
                hlRegTxt.Visible = value;
            }
        }
    }
}