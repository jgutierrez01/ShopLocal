using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Web.Common;

namespace SAM.Web.Proyectos
{
    public partial class ProyDefault : SamPaginaConSeguridad
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
                Master.QuitaLigasSubmenuProyecto();
                establecerDataSource();
                grdProyectos.DataBind();
            }
        }

        protected void grdProyecto_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        private void establecerDataSource()
        {
            grdProyectos.DataSource = UserScope.MisProyectos.OrderBy(x => x.Nombre);
        }
    }
}