using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM.Web.Controles.ConfinarSpool
{
    public partial class Junta : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        internal void Map(object entity)
        {
            repDetJuntas.DataSource = entity;
            repDetJuntas.DataBind();
        }
    }
}