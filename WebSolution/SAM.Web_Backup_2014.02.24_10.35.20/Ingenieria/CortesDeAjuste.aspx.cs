using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Produccion;
using Telerik.Web.UI;

namespace SAM.Web.Ingenieria
{
    public partial class CortesDeAjuste : SamPaginaPrincipal
    {

        private int _proyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] == null)
                {
                    ViewState["ProyectoID"] = -1;
                }
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.ing_CortesAjuste);
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
                
            }
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            _proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (_proyectoID > 0)
            {
                proyEncabezado.Visible = true;
                proyEncabezado.BindInfo(_proyectoID);
            }
            else
            {
                proyEncabezado.Visible = false;
                grdDetalle.Visible = false;
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            grdDetalle.Visible = true;            
            grdDetalle.Rebind();
        }

        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            grdDetalle.Rebind();
        }

        protected void grdDetalle_OnNeedDataSource(object sender, EventArgs e)
        {
            grdDetalle.DataSource = CorteDetalleBO.Instance.ObtenerCorteDetalleConAjuste(_proyectoID);
        }

        protected void grdDetalle_OnItemCommand(object sender, EventArgs e)
        { }

        protected void grdDetalle_OnItemCreated(object sender, EventArgs e)
        { }

        protected void grdDetalle_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {

                GrdCorteAjuste dataItem = (GrdCorteAjuste)e.Item.DataItem;
                int matID = dataItem.MaterialSpoolID;


                HyperLink lnkAjustar = e.Item.FindControl("hypAjustar") as HyperLink;
                string jsLink = string.Format("javascript:Sam.Ingenieria.AbrePopupAjustarLongitud('{0}');", matID);
                lnkAjustar.NavigateUrl = jsLink;
            }
        }
      
    }
}