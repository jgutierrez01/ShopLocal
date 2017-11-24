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
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic;

namespace SAM.Web.Catalogos
{
    public partial class LstAcero : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Aceros);
                EstablecerDataSource();
                grdAceros.DataBind();
            }
        }

        protected void grdAceros_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdAceros.DataSource = AceroBO.Instance.ObtenerTodosConFamilias();
        }

        protected void grdAceros_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int aceroID = e.CommandArgument.SafeIntParse();
                try
                {
                    AceroBO.Instance.Borra(aceroID);
                    EstablecerDataSource();
                    grdAceros.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdAceros.ResetBind();
            EstablecerDataSource();
            grdAceros.DataBind();
        }

        protected void grdSpools_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Acero acero = (SAM.Entities.Acero)e.Item.DataItem;
                int idAcero = acero.AceroID;//dataItem["AceroID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetAcero.aspx?ID={0}", idAcero);              
            }

        }
    }
}