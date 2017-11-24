using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SAM.Web.Classes;
using Mimo.Framework.Data;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;

namespace SAM.Web.Catalogos
{
    public partial class LstProcesoRaiz : SamPaginaPrincipal
    {
        private readonly List<ObjectSetOrder> ORDEN_DEFAULT = new List<ObjectSetOrder>(new[] { new ObjectSetOrder { ColumnName = "Nombre", Order = SortOrder.Ascending } });

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_ProcesoRaiz);
                EstablecerDataSource();
                grdProcesoRaiz.DataBind();
            }
        }

        protected void grdProcesoRaiz_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdProcesoRaiz.DataSource = ProcesoRaizBO.Instance.ObtenerTodos();
        }

        protected void grdProcesoRaiz_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int procesoRaizID = e.CommandArgument.SafeIntParse();
                try
                {
                    
                    ProcesoRaizBO.Instance.Borra(procesoRaizID);
                    EstablecerDataSource();
                    grdProcesoRaiz.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdProcesoRaiz_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.ProcesoRaiz proceso = (SAM.Entities.ProcesoRaiz)e.Item.DataItem;
                int idProcesoRaiz = proceso.ProcesoRaizID;//dataItem["ProcesoRaizID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetProcesoRaiz.aspx?ID={0}", idProcesoRaiz);
            }

        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdProcesoRaiz.ResetBind();
            EstablecerDataSource();
            grdProcesoRaiz.DataBind();
        }
    }
}