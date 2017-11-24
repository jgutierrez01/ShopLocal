using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM.Web.Controles.Calidad.SegJunta
{
    public partial class RepsPnd : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                pnlDefectosCuadrante.Visible = false;
                pnlDefectosSector.Visible = false;
                if (DatasourceCuad != null)
                {
                    repCuad.DataSource = DatasourceCuad;
                    repCuad.DataBind();
                    pnlDefectosCuadrante.Visible = ((IEnumerable<object>) DatasourceCuad).Count() > 0;
                }
                if (DatasourceSect != null)
                {
                    repSector.DataSource = DatasourceSect;
                    repSector.DataBind();
                    pnlDefectosSector.Visible = ((IEnumerable<object>) DatasourceSect).Count() > 0;
                }
            }
        }

        public object DatasourceCuad { get; set; }
        public object DatasourceSect { get; set; }
    }
}