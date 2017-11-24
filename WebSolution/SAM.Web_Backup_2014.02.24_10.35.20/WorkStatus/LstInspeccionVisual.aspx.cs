using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.BusinessObjects.Validations;
using Mimo.Framework.Exceptions;
using SAM.Entities.Grid;
using System.Web.UI.HtmlControls;
using SAM.Entities.Cache;

namespace SAM.Web.WorkStatus
{
    public partial class LstInspeccionVisual : SamPaginaPrincipal
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

        

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_InspeccionVisual);
            }
        }

        private void establecerDataSource()
        {
            grdJuntas.DataSource = InspeccionVisualBO.Instance.ObtenerJuntas(OrdenTrabajo, OrdenTrabajoSpoolID);
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

            establecerDataSource();
            grdJuntas.DataBind();
            grdJuntas.Visible = true;
        }

        /// <summary>
        /// Carga el link para abrir el pop up de generacion de reporte en el hyperlink del encabezado del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdJuntas_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;

                HyperLink hypReporte = (HyperLink)item.FindControl("hypReporte");
                HyperLink imgReporte = (HyperLink)item.FindControl("imgReporte");

                string jsLink = string.Format("javascript:Sam.Workstatus.AbrePopUpReporteInspeccionVisual('{0}','{1}');",
                                              ProyectoID, grdJuntas.ClientID);

                hypReporte.NavigateUrl = jsLink;
                imgReporte.NavigateUrl = jsLink;
            }
        }

        /// <summary>
        /// Recorre todos los registros del grid para editar sus controles en los casos especificos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdJuntas_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdInspeccionVisual item = (GrdInspeccionVisual)e.Item.DataItem;
                GridDataItem grdItem = (GridDataItem)e.Item;
                // Si el tipo de junta es TH / TW no aplica soldadura
                if (item.TipoJunta == TipoJuntas.TH || item.TipoJunta == TipoJuntas.TW)
                {
                    Literal lit = new Literal();
                    lit.Text = "NA";
                    grdItem["hdSoldadura"].Controls.Clear();
                    grdItem["hdSoldadura"].Controls.Add(lit);  
                }

               
                //Si la junta no tiene armado, o no tiene soldadura o se encuentra en hold no deberá poder ser seleccionada.
                if ((item.TipoJunta == TipoJuntas.TW && !item.Armado) || item.TipoJunta == TipoJuntas.TH || item.Hold)
                {
                    grdItem["seleccion_h"].Controls.Clear();
                }
            }
        }

        protected void grdJuntas_OnNeedDataSource(object sender, EventArgs e)
        {
            establecerDataSource();
        }

        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            establecerDataSource();
            grdJuntas.DataBind();
        }
      
        #endregion
    }
}