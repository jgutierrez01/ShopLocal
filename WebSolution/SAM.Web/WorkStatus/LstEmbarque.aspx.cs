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
using SAM.BusinessLogic.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class LstEmbarque : SamPaginaPrincipal
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
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_Embarque);               
            }
        }

        

        private void establecerDataSource()
        {
            grdSpools.DataSource = EmbarqueBO.Instance.ObtenListadoParaEmbarque(ProyectoID,OrdenTrabajo, Accion);
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
            Accion = ddlAccion.SelectedValue.SafeIntParse();

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

                HyperLink hypImprimir = (HyperLink)item.FindControl("hypImprimir");
                HyperLink imgImprimir = (HyperLink)item.FindControl("imgImprimir");
                HyperLink hypEtiquetar = (HyperLink)item.FindControl("hypEtiquetar");
                HyperLink imgEtiquetar = (HyperLink)item.FindControl("imgEtiquetar");
                HyperLink hypPreparar = (HyperLink)item.FindControl("hypPreparar");
                HyperLink imgPreparar = (HyperLink)item.FindControl("imgPreparar");
                HyperLink hypEmbarcar = (HyperLink)item.FindControl("hypEmbarcar");
                HyperLink imgEmbarcar = (HyperLink)item.FindControl("imgEmbarcar");
                                
                string jsEtiquetar = string.Format("javascript:Sam.Workstatus.AbrePopUpEtiquetar('{0}','{1}');",
                                              ProyectoID, grdSpools.ClientID);
                string jsPreparar = string.Format("javascript:Sam.Workstatus.AbrePopUpPreparar('{0}','{1}');",
                                              ProyectoID, grdSpools.ClientID);

                string jsEmbarcar = string.Format("javascript:Sam.Workstatus.AbrePopUpEmbarcar('{0}','{1}');",
                                              ProyectoID, grdSpools.ClientID);

                string jsImprimir = string.Format("javascript:Sam.Workstatus.AbrePopUpImprimeEtiquetas('{0}','{1}');",
                                              ProyectoID, grdSpools.ClientID);

                hypEtiquetar.NavigateUrl = jsEtiquetar;
                imgEtiquetar.NavigateUrl = jsEtiquetar;
                hypPreparar.NavigateUrl = jsPreparar;
                imgPreparar.NavigateUrl = jsPreparar;
                hypEmbarcar.NavigateUrl = jsEmbarcar;
                imgEmbarcar.NavigateUrl = jsEmbarcar;
                hypImprimir.NavigateUrl = jsImprimir;
                imgImprimir.NavigateUrl = jsImprimir;


                if (Accion == 0)
                {
                    hypPreparar.Visible = true;
                    imgPreparar.Visible = true;
                }
                else if (Accion == 1)
                {
                    hypEtiquetar.Visible = true;
                    imgEtiquetar.Visible = true;                    
                }
                else if (Accion == 2)
                {
                    hypImprimir.Visible = true;
                    imgImprimir.Visible = true;
                }
                else if (Accion == 3)
                {
                    hypEmbarcar.Visible = true;
                    imgEmbarcar.Visible = true;
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
                GrdEmbarque item = (GrdEmbarque)e.Item.DataItem;
                GridDataItem grdItem = (GridDataItem)e.Item;

                //Si se encuentra en hold no deberá poder ser seleccionada.
                if (item.Hold)
                {
                    grdItem["seleccion_h"].Controls.Clear();
                }

                HyperLink hlVer = e.Item.FindControl("hlVer") as HyperLink;
                hlVer.NavigateUrl = string.Format("javascript:Sam.Workstatus.AbrePopupReporteEmbarque('{0}')", item.SpoolID);
               
                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;                    
                if (item.Embarque != string.Empty)
                {
                   visor.ProyectoID = ProyectoID;
                    visor.NombresParametrosReporte = "NumeroEmbarque";
                    visor.ValoresParametrosReporte = item.Embarque;
                    visor.Tipo = TipoReporteProyectoEnum.Embarque;
                }
                else
                {
                    visor.Visible = false;
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