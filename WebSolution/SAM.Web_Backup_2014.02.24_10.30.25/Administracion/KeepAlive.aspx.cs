using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;

namespace SAM.Web.Administracion
{
    public partial class KeepAlive : SamPaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Session.KeepAlive"] = DateTime.Now;
        }
    }
}