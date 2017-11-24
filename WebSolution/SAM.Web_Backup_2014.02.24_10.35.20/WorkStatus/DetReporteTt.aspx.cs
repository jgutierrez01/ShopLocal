using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using Telerik.Web.UI;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.WorkStatus
{
    public partial class DetReporteTt : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAReporteTT(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un reporte TT {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_ReporteTt);
                carga(EntityID.Value);
            }
        }

        /// <summary>
        /// Carga los labels necesarios para mostrar el reporte seleccionada
        /// </summary>
        /// <param name="reporteTtID"></param>
        private void carga(int reporteTtID)
        {
            ReporteTt reporteTt = ReporteTtBO.Instance.Obtener(reporteTtID);
            lblNumeroDeReporte.Text = reporteTt.NumeroReporte;
            lblFechaDeReporte.Text = reporteTt.FechaReporte.ToString("d");
            lblTipoPrueba.Text = CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.ID == reporteTt.TipoPruebaID).Select(x => x.Nombre).Single();
            proyHeader.BindInfo(reporteTt.ProyectoID);
            EstablecerDataSource(reporteTtID);
            grdReporteTt.DataBind();
            titulo.NavigateUrl += "?RID=" + EntityID.Value;
        }

        /// <summary>
        /// manda a llamar el metodo EstablecerDataSource con el id del reporteTt
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdReportePnd_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource(EntityID.Value);
        }

        protected void grdReportePnd_ItemCreated(object sender, GridItemEventArgs e)
        {
           // throw new NotImplementedException();
        }

        /// <summary>
        /// establece el datasource con el reporte establecido
        /// </summary>
        /// <param name="reporteTt"></param>
        private void EstablecerDataSource(int reporteTt)
        {
            List<GrdDetReporteTt> NumeroJuntaReporteTt = JuntaReporteTtBO.Instance.ObtenerJuntaReporteTt(reporteTt);
            grdReporteTt.DataSource = NumeroJuntaReporteTt;
            //ReporteTt NumeroJuntaReporteTt = ReporteTtBO.Instance.ObtenerNumeroJuntasDetalleReporteTt(reporteTt);
            lblTotalJuntas.Text = NumeroJuntaReporteTt.Count().ToString();
            lblJuntasAprobadas.Text = NumeroJuntaReporteTt.Where(x => x.Aprobado).Count().ToString();
            lblJuntasRechazadas.Text = NumeroJuntaReporteTt.Where(x => !x.Aprobado).Count().ToString();
        }

        /// <summary>
        /// Genera la funcionaldad de los bottones del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdReportePnd_OnItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int juntaReporteTt = e.CommandArgument.SafeIntParse();

                        JuntaReporteTtBO.Instance.Borra(juntaReporteTt);
                        EstablecerDataSource(EntityID.Value);
                        grdReporteTt.DataBind();
                   
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }
    }
}