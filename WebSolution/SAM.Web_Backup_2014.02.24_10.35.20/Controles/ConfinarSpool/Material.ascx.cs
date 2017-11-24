using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM.Web.Controles.ConfinarSpool
{
    public partial class Material : System.Web.UI.UserControl
    {
        internal void Map(object entity)
        {
            repDetMaterial.DataSource = entity;
            repDetMaterial.DataBind();
        }
    }
}