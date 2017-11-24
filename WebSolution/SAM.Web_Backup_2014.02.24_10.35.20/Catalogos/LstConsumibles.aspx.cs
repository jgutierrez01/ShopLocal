using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using Telerik.Web.UI;

namespace SAM.Web.Catalogos
{
    public partial class LstConsumibles : SamPaginaPrincipal
    {

        private int PatioID
        {
            get
            {
                return ViewState["PatioID"].SafeIntParse();
            }
            set
            {
                ViewState["PatioID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Consumibles);
                PatioID = Request.QueryString["PID"].SafeIntParse();
                cargaPatios();
            }
        }

        private void cargaPatios()
        {
            ddlPatio.BindToEntiesWithEmptyRow(UserScope.MisPatios);

            if (PatioID > 0)
            {
                ddlPatio.SelectedValue = PatioID.ToString();
                EstablecerDataSource();
                grdConsumibles.DataBind();
                grdConsumibles.Visible = true;
            }
        }

        private void EstablecerDataSource()
        {
            grdConsumibles.DataSource = ConsumiblesBO.Instance
                                                     .ObtenerPorPatio(PatioID)
                                                     .OrderBy(x => x.Codigo);
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            PatioID = ddlPatio.SelectedValue.SafeIntParse();
            EstablecerDataSource();
            grdConsumibles.DataBind();
            grdConsumibles.Visible = true;
        }

        protected void grdConsumibles_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdConsumibles_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                HyperLink hlAgregar = commandItem.FindControl("hlAgregar") as HyperLink;
                HyperLink imgAgregar = commandItem.FindControl("imgAgregar") as HyperLink;
                hlAgregar.NavigateUrl = string.Format(WebConstants.CatalogoUrl.DET_CONSUMIBLES, PatioID);
                imgAgregar.NavigateUrl = string.Format(WebConstants.CatalogoUrl.DET_CONSUMIBLES, PatioID);
            }
        }

        protected void grdConsumibles_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int consumibleID = e.CommandArgument.SafeIntParse();
                try
                {
                    ConsumiblesBO.Instance.Borra(consumibleID);
                    EstablecerDataSource();
                    grdConsumibles.DataBind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdConsumibles.ResetBind();
            grdConsumibles.Rebind();
        }

    }
}