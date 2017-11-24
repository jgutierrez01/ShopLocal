using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Exceptions;
using System.Globalization;
using Mimo.Framework.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpInspeccionDimensional : SamPaginaPopup
    {
        private string SIDs
        {
            get
            {
                if (ViewState["SIDs"] == null)
                {
                    ViewState["SIDs"] = string.Empty;
                }

                return ViewState["SIDs"].ToString();
            }
            set
            {
                ViewState["SIDs"] = value;
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

        private string Fechas
        {
            get
            {
                if (ViewState["Fechas"] == null)
                {
                    ViewState["Fechas"] = string.Empty;
                }

                return ViewState["Fechas"].ToString();
            }
            set
            {
                ViewState["Fechas"] = value;
            }
        }

        private string FechasReporte
        {
            get
            {
                if (ViewState["FechasReporte"] == null)
                {
                    ViewState["FechasReporte"] = string.Empty;
                }

                return ViewState["FechasReporte"].ToString();
            }
            set
            {
                ViewState["FechasReporte"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                SIDs = Request.QueryString["SIDs"];
                TipoReporteID = Request.QueryString["TR"].SafeIntParse();
                
                int [] spoolIds = SIDs.Split(',').Select(n => n.SafeIntParse()).ToArray();

                if (!SeguridadQs.TieneAccesoATodosLosSpools(spoolIds))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando inspeccionar dimensionalmente algún spool {1} al cual no tiene permisos", SessionFacade.UserId, SIDs);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Fechas = ValidaFechasBO.Instance.ObtenerFechasSoldaduraPorSpoolConcatenadas(spoolIds, false);
                FechasReporte = ValidaFechasBO.Instance.ObtenerFechasSoldaduraPorSpoolConcatenadas(spoolIds, true);
                string NumerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorSpoolID(spoolIds);

                btnGenerar.OnClientClick = "return Sam.Workstatus.ValidaFechasInspeccionDimensional('" + NumerosControl + "' ,'" + Fechas + "','" + FechasReporte + "')";
                cargaTipoReporte();
            }
        }

        private void cargaTipoReporte()
        {
            txtTipoReporte.Text = TraductorEnumeraciones.TextoTipoReporteDimensional(TipoReporteID);
        }

        /// <summary>
        /// Genera el reporte de inspeccion visual para las juntas seleccionadas en el grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                ReporteDimensional repDimensional = new ReporteDimensional
                {
                    ProyectoID = EntityID.Value,
                    NumeroReporte = txtNumeroReporte.Text,
                    FechaReporte = rdpFechaReporte.SelectedDate.Value,
                    TipoReporteDimensionalID = TipoReporteID
                };

                ReporteDimensionalDetalle detalle = new ReporteDimensionalDetalle
                {
                    FechaLiberacion = rdpFechaLiberacion.SelectedDate.Value,
                    Aprobado = ddlResultado.SelectedValue.SafeIntParse() == 0 ? false : true,
                    Observaciones = txtObservaciones.Text
                };

                try
                {
                    string[] arregloFechas = Fechas.Split(',');
                    List<string> spools = SIDs.Split(',').ToList();

                    InspeccionDimensionalBO.Instance.GeneraReporte(detalle, repDimensional, SIDs, SessionFacade.UserId);

                    //Actualiza el grid de la ventana padre para que refleje que el reporte ya se generó
                    JsUtils.RegistraScriptActualizaGridGenerico(this);

                    lnkReporte.ProyectoID = EntityID.Value;
                    lnkReporte.NombresParametrosReporte = "NumeroReporte";
                    lnkReporte.ValoresParametrosReporte = txtNumeroReporte.Text;
                    lnkReporte.Tipo = TipoReporteProyectoEnum.LiberacionDimensional;

                    phControles.Visible = false;
                    phMensajeExito.Visible = true;
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }
    }
}