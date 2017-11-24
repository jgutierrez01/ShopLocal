using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using System.Web;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.WorkStatus
{
    public partial class DetReporteSpoolPnd : SamPaginaPrincipal
    {
        /// <summary>
        /// toma el QueryString del reporteSpoolPnd Id y lo manda al metodo carga
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAReporteSpoolPnd(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un reporte PND {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_ReportePND);
                carga(EntityID.Value);
            }
        }

        /// <summary>
        /// Carga los labels necesarios para mostrar el reporte seleccionada
        /// </summary>
        /// <param name="reportePndID"></param>
        private void carga(int reporteSpoolPndID)
        {
            ReporteSpoolPnd reporteSpoolPnd = ReportePndBO.Instance.ObtenerReporteSpool(reporteSpoolPndID);
            lblNumeroDeReporte.Text = reporteSpoolPnd.NumeroReporte;
            lblFechaDeReporte.Text = reporteSpoolPnd.FechaReporte.ToString("d");
            lblTipoPrueba.Text = CacheCatalogos.Instance.ObtenerTiposPruebaSpool().Where(x => x.ID == reporteSpoolPnd.TipoPruebaSpoolID).Select(x => x.Nombre).Single();
            proyHeader.BindInfo(reporteSpoolPnd.ProyectoID);
            EstablecerDataSource(reporteSpoolPndID);
            grdReportePnd.DataBind();

            titulo.NavigateUrl += "?RID=" + EntityID.Value;
        }

        /// <summary>
        /// manda a llamar al metodo de establecerDataSource con el ID del reportePND
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdReportePnd_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource(EntityID.Value);
        }

        /// <summary>
        /// genera el grid con el ID del reporte indicado asi como establece la cantidad de spools aprobados, rechazados y totales 
        /// </summary>
        /// <param name="reporteSpoolPnd"></param>
        private void EstablecerDataSource(int reporteSpoolPnd)
        {
            List<GrdDetReporteSpoolPnd> NumeroJuntaReportePnd = SpoolReportePndBO.Instance.ObtenerSpoolReportePnd(reporteSpoolPnd);
            grdReportePnd.DataSource = NumeroJuntaReportePnd;
            lblTotalSpools.Text = NumeroJuntaReportePnd.Count().ToString();
            lblSpoolsAprobados.Text = NumeroJuntaReportePnd.Where(x => x.Aprobado).Count().ToString();
            lblSpoolsRechazados.Text = NumeroJuntaReportePnd.Where(x => !x.Aprobado).Count().ToString();
        }

        /// <summary>
        /// Genera la funcionalidad de los bottones del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdReportePnd_OnItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int spoolReportePnd = e.CommandArgument.SafeIntParse();
                    SpoolReportePndBO.Instance.Borra(spoolReportePnd);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
                EstablecerDataSource(EntityID.Value);
                grdReportePnd.DataBind();
            }
            else
            {
                ToolTip.TargetControls.Clear();
            }
        }

        /// <summary>
        /// establece el proceso necesario para el link de detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdReportePnd_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem || e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdDetReporteSpoolPnd DetRepPnd = (GrdDetReporteSpoolPnd)item.DataItem;

                Control target = e.Item.FindControl("targetControl");
                if (!Object.Equals(target, null) && !Object.Equals(this.ToolTip, null))
                {
                    this.ToolTip.TargetControls.Add(target.ClientID, item.GetDataKeyValue("SpoolReportePndID").ToString(), true);
                }

                HyperLink hltargetControl = (HyperLink)item["hlDetalle_h"].FindControl("targetControl");

                if (DetRepPnd.Aprobado)
                {
                    hltargetControl.Visible = false;
                }

                if (DetRepPnd.TieneHold)
                {
                    ImageButton btnBorrar = e.Item.FindControl("btnBorrar") as ImageButton;
                    btnBorrar.Visible = false;
                }
            }
        }
    }
}