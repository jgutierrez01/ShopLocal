using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using Telerik.Web.UI;
using SAM.Entities.Grid;

namespace SAM.Web.WorkStatus
{
    public partial class LstSpoolPnd : SamPaginaPrincipal
    {
        #region Propiedades Privadas

        private int ProyectoID
        {
            get
            {
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }
        private int OrdenTrabajo
        {
            get
            {
                return ViewState["OrdenTrabajo"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajo"] = value;
            }
        }
        private int OrdenTrabajoSpoolID
        {
            get
            {
                return ViewState["OrdenTrabajoSpoolID"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajoSpoolID"] = value;
            }
        }
        private int TipoPruebaSpoolID
        {
            get
            {
                return ViewState["TipoPruebaSpoolID"].SafeIntParse();
            }
            set
            {
                ViewState["TipoPruebaSpoolID"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_PruebasNoDestructivas);
                cargaCombos();
            }
        }

        /// <summary>
        /// Carga la informacion de tipos de prueba
        /// </summary>
        private void cargaCombos()
        {
            ddlTipoPrueba.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposPruebaSpool().Where(x => x.Categoria == "PND").OrderBy(x => x.Nombre));
        }
        private void establecerDataSource()
        {
            grdSpools.DataSource = ReportePndBO.Instance.ObtenerSpools(ProyectoID, OrdenTrabajo, OrdenTrabajoSpoolID, TipoPruebaSpoolID);
        }

        #region Eventos

        /// <summary>
        /// Carga el grid de las juntas en base a los filtros seleccionados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            OrdenTrabajo = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            OrdenTrabajoSpoolID = filtroGenerico.NumeroControlSelectedValue.SafeIntParse();
            TipoPruebaSpoolID = ddlTipoPrueba.SelectedValue.SafeIntParse();

            establecerDataSource();
            grdSpools.DataBind();
            grdSpools.Visible = true;
        }
        /// <summary>
        /// Carga el link para abrir el pop up de generacion de reporte en el hyperlink del encabezado del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;

                HyperLink hypReporte = (HyperLink)item.FindControl("hypReporte");
                HyperLink imgReporte = (HyperLink)item.FindControl("imgReporte");

                string jsLink = string.Format("javascript:Sam.Workstatus.AbrePopUpPruebasSpoolPND('{0}','{1}','{2}');",
                                              ProyectoID, grdSpools.ClientID, TipoPruebaSpoolID);

                hypReporte.NavigateUrl = jsLink;
                imgReporte.NavigateUrl = jsLink;
            }
        }


        protected void grdSpools_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdRequisicionesSpool item = (GrdRequisicionesSpool)e.Item.DataItem;
                GridDataItem grdItem = (GridDataItem)e.Item;
                //Si la junta se encuentra en hold no deberá poder ser seleccionada.               
                if (item.Hold)
                {
                    grdItem["seleccion_h"].Controls.Clear();
                }

            }
        }

        protected void grdSpools_OnNeedDataSource(object sender, EventArgs e)
        {
            establecerDataSource();
        }
        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            establecerDataSource();
            grdSpools.DataBind();
        }

        #endregion
    }
}