using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Workstatus;
using System.Globalization;
using Mimo.Framework.Common;
using SAM.Web.Common;
using SAM.BusinessObjects.Produccion;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpReporteTT : SamPaginaPopup
    {

        private string IDs
        {
            get
            {
                if (ViewState["IDs"] == null)
                {
                    ViewState["IDs"] = string.Empty;
                }

                return ViewState["IDs"].ToString();
            }
            set
            {
                ViewState["IDs"] = value;
            }
        }

        private string RIDs
        {
            get
            {
                if (ViewState["RIDs"] == null)
                {
                    ViewState["RIDs"] = string.Empty;
                }

                return ViewState["RIDs"].ToString();
            }
            set
            {
                ViewState["RIDs"] = value;
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
        private string Juntas
        {
            get
            {
                if (ViewState["Juntas"] == null)
                {
                    ViewState["Juntas"] = string.Empty;
                }
                return ViewState["Juntas"].ToString();
            }
            set { ViewState["Juntas"] = value; }
        }

        private string NumerosControl
        {
            get
            {
                if (ViewState["NumerosControl"] == null)
                {
                    ViewState["NumerosControl"] = string.Empty;
                }
                return ViewState["NumerosControl"].ToString();
            }
            set
            {
                ViewState["NumerosControl"] = value;
            }
        }

        private List<PinturaSpool> PinturaSpools
        {
            get
            {
                if (ViewState["PinturaSpools"] == null)
                {
                    ViewState["PinturaSpools"] = new List<JuntaReportePndCuadrante>();
                }
                return (List<PinturaSpool>)ViewState["PinturaSpools"];
            }
            set
            {
                ViewState["PinturaSpools"] = value;
            }
        }

        private string Proceso
        {
            get
            {
                if (ViewState["Proceso"] == null)
                {
                    ViewState["Proceso"] = string.Empty;
                }
                return ViewState["Proceso"].ToString();
            }
            set
            {
                ViewState["Proceso"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                IDs = Request.QueryString["IDs"];
                TipoPruebaID = Request.QueryString["TP"].SafeIntParse();
                RIDs = Request.QueryString["RID"];

                int[] juntaWorkstatusIds = IDs.Split(',').Select(n => n.SafeIntParse()).ToArray();

                Fechas = ValidaFechasBO.Instance.ObtenerFechasRequisiciones(juntaWorkstatusIds, RIDs);
                Juntas = ValidaFechasBO.Instance.ObtenerJuntasPorJuntaWorkstatusIds(juntaWorkstatusIds);
                NumerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorJuntaWorkStatusIds(juntaWorkstatusIds);
                PinturaSpools = PinturaBO.Instance.ObtenerListadoPinturas(juntaWorkstatusIds);

                string proceso;
                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    Proceso = "HT Requisition";
                }
                else
                {
                    Proceso = "Requisiciones TT";
                }

                //btnGenerar.OnClientClick = "return Sam.Workstatus.ValidacionFechas('" + numerosControl + "','" + juntas + "' ,'" + Fechas + "','" + Fechas + "','3','" + proceso + "')";
            }
        }

        /// <summary>
        /// REALIZA LAS VALIDACIONES NECESARIOS PARA LAS FECHAS
        /// SUSTITUYE A LAS VALIDACIONES EN JAVASCRIPT
        /// </summary>
        protected void ValidaFechas()
        {

            string errores = "";
            string juntasErrorFechaPintura = string.Empty;
            List<string> procesosPintura = new List<string>();
            DateTime fechaReporte = new DateTime(rdpFechaReporte.SelectedDate.Value.Year, rdpFechaReporte.SelectedDate.Value.Month, rdpFechaReporte.SelectedDate.Value.Day);
            DateTime fechaPrueba = new DateTime(rdpFechaPrueba.SelectedDate.Value.Year, rdpFechaPrueba.SelectedDate.Value.Month, rdpFechaPrueba.SelectedDate.Value.Day);
            string[] juntas = Juntas.Split(',');
            string[] juntasWs = IDs.Split(',');
            int[] juntasWorkstatus = juntasWs.Select(x => x.SafeIntParse()).ToArray();
            string[] numerosControl = NumerosControl.Split(',');
            string[] fechas = Fechas.Split(',');

            //Primer validacion de Fechas
            if (fechaReporte < fechaPrueba)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    throw new BaseValidationException("The report date must be greater than process date");
                }
                else
                {
                    throw new BaseValidationException("La fecha del reporte debe ser mayor o igual a la del proceso");
                }
            }

            //Se valida que ninguna de las fechas seleccionadas sea menor a la fecha del proceso
            int cuenta = 0;
            for (int i = 0; i < fechas.Length; i++)
            {
                DateTime tempfecha = DateTime.ParseExact(fechas[i], "MM/dd/yyyy", null);
                if (fechaReporte < tempfecha || fechaPrueba < tempfecha)
                {
                    errores += "<br />" + numerosControl[i] + ", " + juntas[i];
                    cuenta++;
                }

                if (PinturaSpools[i] != null)
                {
                    string m = ValidaFechasPintura(juntasWorkstatus[i], fechaPrueba);
                    if (!string.IsNullOrEmpty(m))
                    {
                        juntasErrorFechaPintura += "<br />" + numerosControl[i] + ", " + juntas[i];
                        juntasWorkstatus[i] = (juntasWorkstatus[i] * -1);
                    }
                }

            }

            if (cuenta > 0)
            {
                errores += "<br />";
            }

            if (cuenta == fechas.Length)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    throw new BaseValidationException(
                        String.Format("The report can not be saved because there is an error with respect to the dates of {0} dates", Proceso));
                }
                else
                {
                    throw new BaseValidationException(
                        String.Format("El reporte no se puede guardar ya que hay un error con las fechas respecto a las de {0}", Proceso));
                }
            }

            else if (errores.Length > 0 && cuenta < fechas.Length  && string.IsNullOrEmpty(juntasErrorFechaPintura))
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    lblPopup.Text = String.Format("The control number and joint{0}\n can't be save because have wrong date, Do you want to save the report for the others joints?", errores);
                }
                else
                {
                    lblPopup.Text = String.Format("Los números de control y juntas{0}\n no se pueden guardar porque tiene fecha incorrecta, ¿Desea guardar el reporte para las demás juntas?", errores);
                }
                radwindowPopup.VisibleOnPageLoad = true;
            }

            if(!string.IsNullOrEmpty(juntasErrorFechaPintura))
            {
                int negativos = juntasWorkstatus.Where(x => x < 0).Count();

                if (negativos == juntasWorkstatus.Count())
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        throw new BaseValidationException(
                            String.Format("The report can not be saved because the trial date is more than dates painting processes"));
                    }
                    else
                    {
                        throw new BaseValidationException(
                            String.Format("El reporte no se puede guardar ya que la fecha del proceso es mayor a las fechas de los procesos de pintura"));
                    }
                }
                else
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        throw new BaseValidationException(  String.Format("The control number and joint{0}\n can't be save because the date of the process is greater than the date of the reports paint.", juntasErrorFechaPintura));
                    }
                    else
                    {
                        throw new BaseValidationException(  String.Format("Los números de control y juntas{0}\n no se pueden guardar porque la fecha del proceso es mayor a la fecha de los reportes de pintura.", juntasErrorFechaPintura));
                    }                    
                }
            }
       
            Generar();          
        }

        private string ValidaFechasPintura(int jwsId, DateTime fechaPrueba)
        {
            JuntaWorkstatus jws = JuntaWorkstatusBO.Instance.ObtenerJuntaWorkStatusPorID(jwsId);
            string s = string.Empty;

            if (jws.VersionJunta == 1)
            {
                s = ValidaFechasBO.Instance.ValidaFechasProcesoFechaRequiPinturas(fechaPrueba, jws.OrdenTrabajoSpoolID);
            }

            return s;
        }              

        protected void btnOk_Click(object sender, EventArgs e)
        {
            radwindowPopup.VisibleOnPageLoad = false;
            Generar();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                radwindowPopup.VisibleOnPageLoad = false;
                throw new BaseValidationException("Cancelado");
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void Generar()
        {
            //if (IsValid)
            //{
            try
            {
                //ValidaFechas();
                string[] arregloFechas = Fechas.Split(',');
                List<string> juntas = IDs.Split(',').ToList();
                List<string> reportes = RIDs.Split(',').ToList();
                for (int i = 0; i < arregloFechas.Length; i++)
                {
                    DateTime fecha = DateTime.ParseExact(arregloFechas[i], "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    if (rdpFechaPrueba.SelectedDate.Value < fecha || rdpFechaReporte.SelectedDate.Value < fecha)
                    {
                        juntas[i] = "";
                        reportes[i] = "";
                    }
                }

                juntas.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));
                reportes.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));

                IDs = string.Join(",", juntas);
                RIDs = string.Join(",", reportes);


                ReporteTt reporte = new ReporteTt
                {
                    ProyectoID = EntityID.Value,
                    TipoPruebaID = TipoPruebaID,
                    NumeroReporte = txtNumeroReporte.Text,
                    FechaReporte = rdpFechaReporte.SelectedDate.Value
                };

                JuntaReporteTt juntaReporte = new JuntaReporteTt
                {
                    NumeroGrafica = txtNumeroGrafica.Text,
                    FechaTratamiento = rdpFechaPrueba.SelectedDate.Value,
                    Aprobado = ddlResultado.SelectedValue == "0" ? false : true,
                    Observaciones = txtObservaciones.Text
                };

                ReporteTtBO.Instance.GuardaReporteTt(reporte, juntaReporte, IDs, RIDs, SessionFacade.UserId);

                JsUtils.RegistraScriptActualizaGridGenerico(this);

                lnkReporte.ProyectoID = EntityID.Value;
                lnkReporte.NombresParametrosReporte = "NumeroReporte";
                lnkReporte.ValoresParametrosReporte = txtNumeroReporte.Text;

                switch ((TipoPruebaEnum)TipoPruebaID)
                {
                    case TipoPruebaEnum.Pwht:
                        lblMensajeExito.Visible = false;
                        lblMensajeExitoReporte.Visible = true;
                        lnkReporte.Visible = true;
                        lnkReporte.Tipo = TipoReporteProyectoEnum.PWHT;
                        break;
                    case TipoPruebaEnum.Durezas:
                        lblMensajeExito.Visible = false;
                        lblMensajeExitoReporte.Visible = true;
                        lnkReporte.Visible = true;
                        lnkReporte.Tipo = TipoReporteProyectoEnum.Durezas;
                        break;
                    default:
                        lnkReporte.Visible = false;
                        break;
                }

                phControles.Visible = false;
                phMensajeExito.Visible = true;

            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void btnAGenerar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    ValidaFechas();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }
    }
}