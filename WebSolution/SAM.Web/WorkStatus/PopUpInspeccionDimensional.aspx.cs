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
using SAM.Web.Common;
using System.Web.Script.Serialization;

using SAM.Entities.Grid;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessLogic.Workstatus;
using Mimo.Framework.WebControls;

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

        private string Patio
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
                JavaScriptSerializer js = new JavaScriptSerializer();

                string jsLink = string.Format("return Sam.Workstatus.ValidaFechasInspeccionDimensional({0}, {1}, {2}, {3}, {4});", js.Serialize(NumerosControl), rdpFechaLiberacion.ClientID, rdpFechaReporte.ClientID, js.Serialize(Fechas),  js.Serialize(FechasReporte));
                btnGenerar.OnClientClick = jsLink;
                cargaTipoReporte();

                CargaTalleres();
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
                    Observaciones = txtObservaciones.Text,
                    TallerID = ddlTaller.SelectedValue.SafeIntParse(),
                    InspectorID = rcbInspector.SelectedValue.SafeIntParse()

                };

                try
                {
                    string[] arregloFechas = Fechas.Split(',');
         
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
                finally
                {
                    JsUtils.RegistraScriptActualizaGridGenerico(this);
                }
            }
        }

        /// <summary>
        /// metodo para la carga dinamica de los talleres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTaller_SelectedIndexChanged(object sender, EventArgs e)
        {
            int taller = ddlTaller.SelectedValue.SafeIntParse();
        }

        /// <summary>
        /// Cargara los talleres del patio en el que se esta trabajando
        /// </summary>
        private void CargaTalleres()
        {
            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorPatio(SAM.Web.Controles.Proyecto.Header.patioID));
        }

    }
}