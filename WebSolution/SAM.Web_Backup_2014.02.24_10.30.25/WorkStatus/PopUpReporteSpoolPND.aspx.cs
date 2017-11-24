using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using System.Globalization;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Workstatus;
using SAM.BusinessLogic.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpReporteSpoolPND : SamPaginaPopup
    {

        private int IDCounter
        {
            get
            {
                return ViewState["IDCunter"].SafeIntParse();
            }
            set
            {
                ViewState["IDCunter"] = value;
            }
        }
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

        private string NumerosControl
        {
            get { return ViewState["NumerosControl"].ToString(); }
            set { ViewState["NumerosControl"] = value; }
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

                int[] workstatusSpoolsIds = IDs.Split(',').Select(n => n.SafeIntParse()).ToArray();
                Fechas = ValidaFechasBO.Instance.ObtenerFechasRequisicionesSpool(workstatusSpoolsIds, RIDs);
                NumerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorWorkstatusSpoolID(workstatusSpoolsIds);

                //btnGenerar.OnClientClick = "return Sam.Workstatus.ValidacionFechasReqReporteSpool('" + numerosControl + "', '" + Fechas + "')";

            }
        }

        protected void ValidaFechas()
        {
            string errores = "";
            string[] arregloFechas = Fechas.Split(',');
            string[] numControl = NumerosControl.Split(',');
            DateTime fechaReporte = new DateTime(rdpFechaReporte.SelectedDate.Value.Year, rdpFechaReporte.SelectedDate.Value.Month, rdpFechaReporte.SelectedDate.Value.Day);
            DateTime fechaPrueba = new DateTime(rdpFechaPrueba.SelectedDate.Value.Year, rdpFechaPrueba.SelectedDate.Value.Month, rdpFechaPrueba.SelectedDate.Value.Day);
            //Validamos que la fecha del reporte no sea menor a la fecha de prueba
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

            //Verificar que las fechas de prueba no sean menores a las pruebas de requisicion
            for (int i = 0; i < arregloFechas.Length; i++)
            {
                if (fechaPrueba < DateTime.ParseExact(arregloFechas[i], "MM/dd/yyyy", null))
                {
                    errores += "<br />" + numControl[i];
                }
            }
            //Si alguna de las fechas de prueba es menor a la prueba de requisicion se envia un mensaje de error
            if (errores.Length > 0)
            {
                if (CultureInfo.CurrentCulture.Name == "en-US")
                {
                    throw new BaseValidationException(String.Format("The test date must be greater or equal to the requisition process of the following control numbers: {0}", errores));
                }
                else
                {
                    throw new BaseValidationException(String.Format("La fecha de la prueba debe ser mayor o igual a la del proceso de requisición de los siguientes números de control: {0}", errores));
                }
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    ValidaFechas();
                    string[] arregloFechas = Fechas.Split(',');
                    List<string> spools = IDs.Split(',').ToList();
                    List<string> reportes = RIDs.Split(',').ToList();

                    for (int i = 0; i < arregloFechas.Length; i++)
                    {
                        DateTime fecha = DateTime.ParseExact(arregloFechas[i], "MM/dd/yyyy", CultureInfo.InvariantCulture);

                        if (rdpFechaPrueba.SelectedDate.Value < fecha || rdpFechaReporte.SelectedDate.Value < fecha)
                        {
                            spools[i] = string.Empty;
                            reportes[i] = string.Empty;
                        }
                    }

                    spools.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));
                    reportes.RemoveAll(new System.Predicate<string>(delegate(string val) { return (val == ""); }));

                    IDs = string.Join(",", spools);
                    RIDs = string.Join(",", reportes);

                    ReporteSpoolPnd reporte = new ReporteSpoolPnd
                    {
                        ProyectoID = EntityID.Value,
                        TipoPruebaSpoolID = TipoPruebaID,
                        NumeroReporte = txtNumeroReporte.Text,
                        FechaReporte = rdpFechaReporte.SelectedDate.Value
                    };

                    bool aprobado = !(ddlResultado.SelectedValue == "0");
                    SpoolReportePnd spoolReporte = new SpoolReportePnd
                    {
                        FechaPrueba = rdpFechaPrueba.SelectedDate.Value,
                        Aprobado = aprobado,
                        Observaciones = txtObservaciones.Text
                    };

                    lnkReporte.ProyectoID = EntityID.Value;
                    lnkReporte.NombresParametrosReporte = "NumeroReporte";
                    lnkReporte.ValoresParametrosReporte = txtNumeroReporte.Text;

                    ReportesBL.Instance.GuardaReporteSpoolPND(reporte, spoolReporte, IDs, RIDs, SessionFacade.UserId);


                    JsUtils.RegistraScriptActualizaGridGenerico(this);
                    phControles.Visible = false;
                    phMensaje.Visible = true;

                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "vgGenerar");
                }
            }
        }


    }
}