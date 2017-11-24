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
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.WorkStatus
{
    public partial class LstRequisicionesPruebas : SamPaginaPrincipal
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

        private int TipoPruebaID
        {
            get
            {
                return ViewState["TipoPruebaID"].SafeIntParse();
            }
            set
            {
                ViewState["TipoPruebaID"] = value;
            }
        }



        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_RequisicionesPruebas);
                cargaCombos();
            }
        }

        /// <summary>
        /// Carga la informacion de tipos de prueba
        /// </summary>
        private void cargaCombos()
        {
            ddlTipoPrueba.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.Categoria == "PND" || x.Categoria == "TT").OrderBy(y => y.Nombre));
        }


        private void establecerDataSource()
        {
            grdJuntas.DataSource = RequisicionPruebasBO.Instance.ObtenerJuntas(OrdenTrabajo, OrdenTrabajoSpoolID, TipoPruebaID);
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
            TipoPruebaID = ddlTipoPrueba.SelectedValue.SafeIntParse();

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

                HyperLink hypRequisicion = (HyperLink)item.FindControl("hypRequisicion");
                HyperLink imgRequisicion = (HyperLink)item.FindControl("imgRequisicion");

                string jsLink = string.Format("javascript:Sam.Workstatus.AbrePopUpRequisicionPruebas('{0}','{1}','{2}');",
                                              ProyectoID, grdJuntas.ClientID, TipoPruebaID);

                hypRequisicion.NavigateUrl = jsLink;
                imgRequisicion.NavigateUrl = jsLink;
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
                GrdRequisicionPruebas item = (GrdRequisicionPruebas)e.Item.DataItem;
                GridDataItem grdItem = (GridDataItem)e.Item;
                // Si el tipo de junta es TH / TW no aplica soldadura
                if (item.TipoJunta == TipoJuntas.TH || item.TipoJunta == TipoJuntas.TW)
                {
                    Literal lit = new Literal();
                    lit.Text = "NA";
                    grdItem["hdSoldadura"].Controls.Clear();
                    grdItem["hdSoldadura"].Controls.Add(lit);
                }

                //Si la junta se encuentra en hold no deberá poder ser seleccionada.
                if (item.Hold)
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