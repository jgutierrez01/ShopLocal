using System;

namespace SAM.Web.Controles.ToolTips
{
    public partial class ToolTipGrids : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                repCuad.DataSource = DatasourceCuad;
                repCuad.DataBind();
                repSector.DataSource = DatasourceSect;
                repSector.DataBind();
            }
        }

        public object DatasourceCuad { get; set; }
        public object DatasourceSect { get; set; }
    }
}