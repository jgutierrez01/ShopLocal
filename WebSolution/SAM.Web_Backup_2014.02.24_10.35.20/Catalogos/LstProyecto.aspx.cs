using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Mimo.Framework.Data;
using Telerik.Web.UI;


namespace SAM.Web.Catalogos
{
    public partial class LstProyecto : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Proyectos);
                EstablecerDataSource();
                grdProyectos.DataBind();
            }
        }

        protected void grdProyecto_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdProyectos.DataSource = ProyectoBO.Instance.ObtenerListadoDeProyectos()
                                                         .OrderBy(x => x.Proyecto)
                                                         .ThenBy(x => x.NombreClienteCompleto);
        }

        protected void grdProyecto_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int proyectoID = e.CommandArgument.SafeIntParse();
                try
                {
                    ProyectoBO.Instance.Borra(proyectoID);
                    EstablecerDataSource();
                    grdProyectos.Rebind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdProyectos.ResetBind();
            grdProyectos.Rebind();
        }
    }
}