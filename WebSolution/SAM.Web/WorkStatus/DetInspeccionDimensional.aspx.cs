using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;

namespace SAM.Web.WorkStatus
{
    public partial class DetInspeccionDimensional : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAReporteDimensional(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar el reporte dimensional {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }
                titulo.NavigateUrl += "?" + Request.QueryString;
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_InspeccionDimensional);
                cargarDatos();
            }
        }

        public void cargarDatos()
        {
            ReporteDimensional inspeccion = ReporteDimensionalBO.Instance.DetalleInspeccionDimensional(EntityID.Value);

            proyHeader.BindInfo(inspeccion.ProyectoID);
            lblNumeroReporteData.Text = inspeccion.NumeroReporte;
            lblFechaReporteData.Text = DateTime.Parse(inspeccion.FechaReporte.ToString()).ToShortDateString();
            lblTotalSpoolsData.Text = inspeccion.ReporteDimensionalDetalle.Count().ToString();
            lblSpoolsAprobadosData.Text = inspeccion.ReporteDimensionalDetalle.Where(x => x.Aprobado == true).Count().ToString();
            lblSpoolsRechazadosData.Text = inspeccion.ReporteDimensionalDetalle.Where(x => x.Aprobado == false).Count().ToString();
            lblTipoReporteData.Text = inspeccion.TipoReporteDimensional.Nombre;
            grdDimensional.Rebind();
        }

        protected void grdDimensional_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void grdDimensional_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int juntaID = e.CommandArgument.SafeIntParse();
                    ReporteDimensionalDetalleBO.Instance.Borrar(juntaID,SessionFacade.UserId);

                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "");
                }
                finally
                {
                    cargarDatos();
                    grdDimensional.Rebind();
                }
            }
        }

        private void establecerDataSource()
        {
            grdDimensional.DataSource = ReporteDimensionalBO.Instance.ObtenerDetalleInspeccionDimensional(EntityID.Value);
        }
    }
}