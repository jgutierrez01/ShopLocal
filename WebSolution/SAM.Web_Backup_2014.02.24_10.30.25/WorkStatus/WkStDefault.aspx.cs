using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using SAM.Web.Classes;

namespace SAM.Web.WorkStatus
{
    public partial class WkStDefault : SamPaginaConSeguridad
    {
        protected override void OnLoad(EventArgs e)
        {
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            foreach (HotSpot hs in imgMap.HotSpots)
            {
                if (!hs.NavigateUrl.StartsWith("javascript:"))
                {
                    string url = Page.ResolveUrl(hs.NavigateUrl);

                    if (!SeguridadWeb.UsuarioTieneAcceso(url))
                    {
                        hs.NavigateUrl = string.Format("javascript:alert('{0}');", MensajesAplicacion.Seguridad_NoTieneAcceso);
                    }
                }
            }
        }
    }
}