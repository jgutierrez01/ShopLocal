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

namespace SAM.Web.WorkStatus
{
    public partial class LstTransferenciaSpool : SamPaginaPrincipal
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

        private int Accion
        {
            get
            {
                return ViewState["Accion"].SafeIntParse();
            }
            set
            {
                ViewState["Accion"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_TransferenciaSpool);               
            }
        }

        /// <summary>
        /// Carga el grid de las juntas en base a los filtros seleccionados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {

            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            OrdenTrabajo = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            Accion = ddlAccion.SelectedValue.SafeIntParse();

            establecerDataSource();
            grdSpools.DataBind();
            grdSpools.Visible = true;

        }

        private void establecerDataSource()
        {
            grdSpools.DataSource =TransferenciaSpoolBO.Instance.ObtenListadoParaTransferencia(ProyectoID, OrdenTrabajo, Accion).OrderBy(x=>x.NumeroControl);
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

                HyperLink hypPreparar = (HyperLink)item.FindControl("hypPreparar");
                HyperLink imgPreparar = (HyperLink)item.FindControl("imgPreparar");
                HyperLink hypTransferencia = (HyperLink)item.FindControl("hypTransferencia");
                HyperLink imgTransferencia = (HyperLink)item.FindControl("imgTransferencia");


                string jsPreparar = string.Format("javascript:Sam.Workstatus.AbrePopUpPrepararSpoolTransferencia('{0}','{1}');",
                                              ProyectoID, grdSpools.ClientID);

                string jsTransferencia = string.Format("javascript:Sam.Workstatus.AbrePopUpTransferenciaSpool('{0}','{1}');",
                                              ProyectoID, grdSpools.ClientID);


                hypPreparar.NavigateUrl = jsPreparar;
                imgPreparar.NavigateUrl = jsPreparar;
                hypTransferencia.NavigateUrl = jsTransferencia;
                imgTransferencia.NavigateUrl = jsTransferencia;

                if (Accion == 0)
                {
                    hypPreparar.Visible = true;
                    imgPreparar.Visible = true;
                }
                else if (Accion == 1)
                {
                    hypTransferencia.Visible = true;
                    imgTransferencia.Visible = true;
                }

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
                GrdTransferenciaSpool item = (GrdTransferenciaSpool)e.Item.DataItem;
                GridDataItem grdItem = (GridDataItem)e.Item;


                HyperLink hlVer = e.Item.FindControl("hlVer") as HyperLink;
                hlVer.NavigateUrl = string.Format("javascript:Sam.Workstatus.AbrePopupReporteTransferencia('{0}')", item.SpoolID);

                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;

                if (!string.IsNullOrEmpty(item.NumeroTransferencia))
                {
                    visor.ProyectoID = ProyectoID;
                    visor.NombresParametrosReporte = "NumeroTransferencia";
                    visor.ValoresParametrosReporte = item.NumeroTransferencia;
                    visor.Tipo = TipoReporteProyectoEnum.Transferencia;
                }
                else
                {
                    visor.Visible = false;
                }

                //Si se encuentra en hold no deberá poder ser seleccionada.
                if (item.Hold)
                {
                    grdItem["seleccion_h"].Controls.Clear();
                }
                //else { 
                //    if (item.Preparado && !item.Transferencia)
                //    {
                //        grdItem["seleccion_h"].Controls.Clear();
                //    }
                //}              

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

    }
}