using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SAM.Web.Classes;
using SAM.BusinessLogic;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class LstProcesoRelleno : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_ProcesoRelleno);
                EstablecerDataSource();
                grdProcesoRelleno.DataBind();
            }
        }

        protected void grdProcesoRelleno_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdProcesoRelleno.DataSource = ProcesoRellenoBO.Instance.ObtenerTodos();
        }

        protected void grdProcesoRelleno_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int procesoRellenoID = e.CommandArgument.SafeIntParse();
                try
                {
                    ProcesoRellenoBO.Instance.Borra(procesoRellenoID);
                    EstablecerDataSource();
                    grdProcesoRelleno.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdProcesoRelleno.ResetBind();
            EstablecerDataSource();
            grdProcesoRelleno.DataBind();
        }

        protected void grdProcesoRelleno_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.ProcesoRelleno relleno = (SAM.Entities.ProcesoRelleno)e.Item.DataItem;
                int idProceso = relleno.ProcesoRellenoID; //dataItem["ProcesoRellenoID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetProcesoRelleno.aspx?ID={0}", idProceso);
            }

        }
    }
}