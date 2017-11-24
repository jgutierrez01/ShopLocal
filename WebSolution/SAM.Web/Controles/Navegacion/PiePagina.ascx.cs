using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Common;

namespace SAM.Web.Controles.Navegacion
{
    public partial class PiePagina : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hlAcerca.NavigateUrl = Configuracion.LigaQuienesSomos;
                hlPrivacidad.NavigateUrl = Configuracion.LigaPoliticasPrivacidad;
                hlPoliticasUso.NavigateUrl = Configuracion.LigaPoliticasUso;
                hlContacto.NavigateUrl = Configuracion.LigaContacto;
                hlSteelgo.NavigateUrl = Configuracion.LigaPrincipalSteelgo;
            }
        }
    }
}