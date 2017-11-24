using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using System.Globalization;
using Mimo.Framework.Common;
using Telerik.Web.UI;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Grid;
using SAM.BusinessLogic.Workstatus;
using SAM.Entities;

namespace SAM.Web.WorkStatus
{
    public partial class LstPintura : SamPaginaPrincipal
    {
        #region valores filtros

        private int ProyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] != null)
                    return ViewState["ProyectoID"].SafeIntParse();
                return -1;
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private int OrdenTrabajoID
        {
            get
            {
                return ViewState["OrdenTrabajoID"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajoID"] = value;
            }
        }

        private int RequisicionID
        {
            get
            {
                return ViewState["RequisicionID"].SafeIntParse();
            }
            set
            {
                ViewState["RequisicionID"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_PinturaSpool);
               
            }
        }

        /// <summary>
        /// Carga el combo de requisiciones
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="ordenTrabajoID"></param>
        private void cargaCombos(int proyectoID, int ordenTrabajoID)
        {
            ddlRequisicion.BindToEnumerableWithEmptyRow(RequisicionPinturaBO.Instance.ObtenerListadoRequisicion(proyectoID, ordenTrabajoID), "Valor", "ID", -1);
        }

        /// <summary>
        /// Evento que se dispara al cambiar la seleccion ya sea del proyecto o de la orden de trabajo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void filtro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse() > 0)
            {
                reqStyle.Visible = false;
                valRequisicion.Enabled = false;
            }
            else
            {
                reqStyle.Visible = true;
                valRequisicion.Enabled = true;
            }

            cargaCombos(filtroGenerico.ProyectoSelectedValue.SafeIntParse(), filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse());
        }

        /// <summary>
        /// Metodo que se dispara cuando el usuario presiona el boton Mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            //Ponemos en ViewState lo filtros con los que se esta ejecutando la consulta
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            OrdenTrabajoID = filtroGenerico.OrdenTrabajoSelectedValue.SafeIntParse();
            RequisicionID = ddlRequisicion.SelectedValue.SafeIntParse();
            //establecemos el datasource del grid
            grdSpools.Rebind();
            grdSpools.Visible = true;
        }

        /// <summary>
        /// Metodo que se dispara cuando el usuario presiona el link de actualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            grdSpools.Rebind();
        }

        /// <summary>
        /// Metodo que se dispara cuando el grid esta apunto de hacer un databind, o si se mando ejecutar el rebind del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            grdSpools.DataSource = PinturaBO.Instance.ObtenerListadoPintura(ProyectoID, OrdenTrabajoID, RequisicionID);
        }

        /// <summary>
        /// Metodo que se dispara para cada item que este asociado al grid cuando se establece el datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                GrdPintura item = (GrdPintura)e.Item.DataItem;
                GridDataItem grdItem = (GridDataItem)e.Item;

                HyperLink hlDescargar = (HyperLink)grdItem["hlDescargar_h"].FindControl("hlDescargar");

                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;
                visor.ProyectoID = ProyectoID;
                visor.NombresParametrosReporte ="WorkstatusSpoolID";
                visor.ValoresParametrosReporte = item.WorkstatusSpoolID.ToString();
                visor.Tipo = TipoReporteProyectoEnum.Pintura;

                HyperLink hlVer = e.Item.FindControl("hlVer") as HyperLink;
                hlVer.NavigateUrl = string.Format("javascript:Sam.Workstatus.AbrePopupReportePintura('{0}','{1}')", item.WorkstatusSpoolID, item.OrdenTrabajoSpoolID);
               

                HyperLink hypSandBlast = e.Item.FindControl("hypSandBlast") as HyperLink;
                Literal litSandBlast = e.Item.FindControl("litSandBlast") as Literal;
                HyperLink hypPrimario = e.Item.FindControl("hypPrimario") as HyperLink;
                Literal litPrimario = e.Item.FindControl("litPrimario") as Literal;
                HyperLink hypIntermedio = e.Item.FindControl("hypIntermedio") as HyperLink;
                Literal litIntermedio = e.Item.FindControl("litIntermedio") as Literal;
                HyperLink hypAcabadoVisual = e.Item.FindControl("hypAcabadoVisual") as HyperLink;
                Literal litAcabadoVisual = e.Item.FindControl("litAcabadoVisual") as Literal;
                HyperLink hypAdherencia = e.Item.FindControl("hypAdherencia") as HyperLink;
                Literal litAdherencia = e.Item.FindControl("litAdherencia") as Literal;
                HyperLink hypPullOff = e.Item.FindControl("hypPullOff") as HyperLink;
                Literal litPullOff = e.Item.FindControl("litPullOff") as Literal;

                if (existeReporte(item.ReporteSandBlast, (int)TipoPinturaEnum.SandBlast))
                {
                    litSandBlast.Visible = false;
                    hypSandBlast.NavigateUrl = ConstruyeUrl(item.ReporteSandBlast, (int)TipoPinturaEnum.SandBlast);
                }
                else
                {
                    hypSandBlast.Visible = false;
                }

                if (existeReporte(item.ReportePrimario, (int)TipoPinturaEnum.Primario))
                {
                    litPrimario.Visible = false;
                    hypPrimario.NavigateUrl = ConstruyeUrl(item.ReportePrimario, (int)TipoPinturaEnum.Primario);
                }
                else
                {
                    hypPrimario.Visible = false;
                }

                if (existeReporte(item.ReporteIntermedio, (int)TipoPinturaEnum.Intermedio))
                {
                    litIntermedio.Visible = false;
                    hypIntermedio.NavigateUrl = ConstruyeUrl(item.ReporteIntermedio, (int)TipoPinturaEnum.Intermedio);
                }
                else
                {
                    hypIntermedio.Visible = false;
                }

                if (existeReporte(item.ReporteAcabadoVisual, (int)TipoPinturaEnum.AcabadoVisual))
                {
                    litAcabadoVisual.Visible = false;
                    hypAcabadoVisual.NavigateUrl = ConstruyeUrl(item.ReporteAcabadoVisual, (int)TipoPinturaEnum.AcabadoVisual);
                }
                else
                {
                    hypAcabadoVisual.Visible = false;
                }

                if (existeReporte(item.ReporteAdherencia, (int)TipoPinturaEnum.Adherencia))
                {
                    litAdherencia.Visible = false;
                    hypAdherencia.NavigateUrl = ConstruyeUrl(item.ReporteAdherencia, (int)TipoPinturaEnum.Adherencia);
                }
                else
                {
                    hypAdherencia.Visible = false;
                }

                if (existeReporte(item.ReportePullOff, (int)TipoPinturaEnum.PullOff))
                {
                    litPullOff.Visible = false;
                    hypPullOff.NavigateUrl = ConstruyeUrl(item.ReportePullOff, (int)TipoPinturaEnum.PullOff);
                }
                else
                {
                    hypPullOff.Visible = false;
                }

                // Reporte Pintura
                if (hlDescargar != null)
                {
                    if (existeReporte(item.NumeroControl, (int)TipoPinturaEnum.Digitalizado))
                    {
                        hlDescargar.NavigateUrl = ConstruyeUrl(item.NumeroControl, (int)TipoPinturaEnum.Digitalizado);
                    }
                    else
                    {
                        hlDescargar.Visible = false;
                    }
                }

                //Si la junta se encuentra en hold no deberá poder ser seleccionada.
                if (item.Hold)
                {
                    grdItem["seleccion_h"].Controls.Clear();
                }                
            }
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

                string jsLink = string.Format("javascript:Sam.Workstatus.AbrePopUpPintura('{0}','{1}');",
                                              ProyectoID, grdSpools.ClientID);

                hypReporte.NavigateUrl = jsLink;
                imgReporte.NavigateUrl = jsLink;
            }
        }


        private bool existeReporte(string numeroReporte, int tipoPintura)
        {
            return ReportesBL.Instance.ReportePinturaExisteEnFileSystem(numeroReporte, ProyectoID, tipoPintura);
        }

        protected string ConstruyeUrl(string numeroReporte, int tipoPintura)
        {
            return "/WorkStatus/DescargaReporte.aspx?NR=" + numeroReporte + "&ProyectoID=" + ProyectoID + "&TipoPintura=" +
                   tipoPintura + "&TipoReporte=" + (int)TipoReporte.ReportePintura;
        }
    }
}
