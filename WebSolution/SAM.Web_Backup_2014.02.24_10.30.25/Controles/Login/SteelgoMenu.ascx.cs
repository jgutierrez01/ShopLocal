using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Common;

namespace SAM.Web.Controles.Login
{
    public partial class SteelgoMenu : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hlContacto.NavigateUrl = Configuracion.LigaContacto;
                hlQuienesSomos.NavigateUrl = Configuracion.LigaQuienesSomos;
                hlSteelgo.NavigateUrl = Configuracion.LigaPrincipalSteelgo;
                hlServicios.NavigateUrl = Configuracion.LigaServicios;
            }
        }
    }
}