using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using Resources;

namespace SAM.Web.Classes
{
    [DefaultProperty("ID")]
    [ToolboxData("<{0}:AuthenticatedHyperLink runat=server></{0}:AuthenticatedHyperLink>")]
    public class AuthenticatedHyperLink : HyperLink
    {

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.NavigateUrl.StartsWith("javascript:"))
            {
                string url = Page.ResolveUrl(this.NavigateUrl);

                if (!SeguridadWeb.UsuarioTieneAcceso(url))
                {
                    NavigateUrl = string.Format("javascript:alert('{0}');", MensajesAplicacion.Seguridad_NoTieneAcceso);
                }
            }
        }
    }
}