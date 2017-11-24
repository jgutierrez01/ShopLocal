using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class LstInspeccionDimensional : SamPaginaPrincipal
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

        private int TipoReporteID
        {
            get
            {
                return ViewState["TipoReporteID"].SafeIntParse();
            }
            set
            {
                ViewState["TipoReporteID"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_LiberacionDimensionalPatio);
                cargaCombos();
            }
        }

        /// <summary>
        /// Carga los combos de tipo de reporte
        /// </summary>
        private void cargaCombos()
        {
            ddlTipoReporte.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTipoReporteDimensional());
        }

        private void establecerDataSource()
        {
            grdSpools.DataSource = InspeccionDimensionalBO.Instance.ObtenerSpools(OrdenTrabajo, TipoReporteID);
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
            TipoReporteID = ddlTipoReporte.SelectedValue.SafeIntParse();

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

                string jsLink = string.Format("javascript:Sam.Workstatus.AbrePopUpReporteInspeccionDimensional('{0}','{1}','{2}');",
                                              ProyectoID, grdSpools.ClientID, ddlTipoReporte.SelectedValue);

                hypReporte.NavigateUrl = jsLink;
                imgReporte.NavigateUrl = jsLink;
            }
        }

        /// <summary>
        /// Recorre todos los registros del grid para editar sus controles en los casos especificos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdInspeccionDimensional item = (GrdInspeccionDimensional)e.Item.DataItem;
                GridDataItem grdItem = (GridDataItem)e.Item;                

                //Si la junta no tiene armado, o no tiene soldadura o se encuentra en hold no deberá poder ser seleccionada.
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