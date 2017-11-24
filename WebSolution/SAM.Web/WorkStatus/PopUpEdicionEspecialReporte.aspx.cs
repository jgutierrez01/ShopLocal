using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Mimo.Framework.WebControls;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpEdicionEspecialReporte : SamPaginaPopup
    {
        #region Variables globales
        private int ReporteID
        {
            get
            {
                if (ViewState["ReporteEdicionID"] == null)
                {
                    ViewState["ReporteEdicionID"] = 0;
                }
                return ViewState["ReporteEdicionID"].SafeIntParse();
            }
            set
            {
                ViewState["ReporteEdicionID"] = value;
            }
        }

        private int TipoReporte
        {
            get
            {
                if (ViewState["TipoReporte"] == null)
                {
                    ViewState["TipoReporte"] = 0;
                }
                return ViewState["TipoReporte"].SafeIntParse();
            }
            set
            {
                ViewState["TipoReporte"] = value;
            }
        }

        private InspeccionVisual inspecVisual
        {
            get
            {
                return (InspeccionVisual)ViewState["inspecVisual"];
            }
            set
            {
                ViewState["inspecVisual"] = value;
            }
        }

        private ReporteDimensional repDimensional
        {
            get
            {
                return (ReporteDimensional)ViewState["repDimensional"];
            }
            set
            {
                ViewState["repDimensional"] = value;
            }
        }

        private ReportePnd repPND
        {
            get
            {
                return (ReportePnd)ViewState["repPND"];
            }
            set
            {
                ViewState["repPND"] = value;
            }
        }

        private ReporteSpoolPnd repSpoolPND
        {
            get
            {
                return (ReporteSpoolPnd)ViewState["repSpoolPND"];
            }
            set
            {
                ViewState["repSpoolPND"] = value;
            }
        }

        private ReporteTt repTT
        {
            get
            {
                return (ReporteTt)ViewState["repTT"];
            }
            set
            {
                ViewState["repTT"] = value;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReporteID = Request.QueryString["ID"].SafeIntParse();
                TipoReporte = Request.QueryString["TP"].SafeIntParse();

                EstablecerTitulo();

                CargarDatos();
            }
        }

        #region Titulos
        private void EstablecerTitulo()
        {
            switch (TipoReporte)
            {
                case 1:
                    litTitulo.Text = CultureInfo.CurrentCulture.Name == "en-US" ? "Edit visual report" : "Edicion de reporte visual";
                    break;
                case 2:
                    litTitulo.Text = CultureInfo.CurrentCulture.Name == "en-US" ? "Edit dimentional report" : "Edicion de reporte dimensional";
                    break;
                case 3:
                    litTitulo.Text = CultureInfo.CurrentCulture.Name == "en-US" ? "Edit Spool test report" : "Edicion pruebas por Spool";
                    break;
                case 4:
                    litTitulo.Text = CultureInfo.CurrentCulture.Name == "en-US" ? "Edit NDT report" : "Edicion de reporte PND";
                    break;
                case 5:
                    litTitulo.Text = CultureInfo.CurrentCulture.Name == "en-US" ? "Edit TT report" : "Edicion de reporte TT";
                    break;
                default:
                    break;
            }

        }
        #endregion

        #region Cargar datos
        private void CargarDatos()
        {
            switch (TipoReporte)
            {
                case 1:
                    inspecVisual = InspeccionVisualBO.Instance.Obtener(ReporteID);
                    txtNumeroReporte.Text = inspecVisual.NumeroReporte;
                    rdpFechaReporte.SelectedDate = inspecVisual.FechaReporte;
                    break;
                case 2:
                    repDimensional = ReporteDimensionalBO.Instance.Obtener(ReporteID);
                    txtNumeroReporte.Text = repDimensional.NumeroReporte;
                    rdpFechaReporte.SelectedDate = repDimensional.FechaReporte;
                    break;
                case 3:
                    repSpoolPND = ReportePndBO.Instance.ObtenerReporteSpool(ReporteID);
                    txtNumeroReporte.Text = repSpoolPND.NumeroReporte;
                    rdpFechaReporte.SelectedDate = repSpoolPND.FechaReporte;
                    break;
                case 4:
                    repPND = ReportePndBO.Instance.Obtener(ReporteID);
                    txtNumeroReporte.Text = repPND.NumeroReporte;
                    rdpFechaReporte.SelectedDate = repPND.FechaReporte;
                    break;
                case 5:
                    repTT = ReporteTtBO.Instance.Obtener(ReporteID);
                    txtNumeroReporte.Text = repTT.NumeroReporte;
                    rdpFechaReporte.SelectedDate = repTT.FechaReporte;
                    break;
                default:
                    break;
            }
        }
        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime nuevaFecha = rdpFechaReporte.SelectedDate.Value;
                switch (TipoReporte)
                {
                    case 1:
                        GuardarInspeccionVisual(nuevaFecha);
                        break;
                    case 2:
                        GuardarInspeccionDimensional(nuevaFecha);
                        break;
                    case 3:
                        GuardarReporteSpoolPND(nuevaFecha);
                        break;
                    case 4:
                        GuardarReportePND(nuevaFecha);
                        break;
                    case 5:
                        GuardarReporteTT(nuevaFecha);
                        break;
                    default:
                        JsUtils.RegistraScriptActualizaGridGenerico(this);
                        break;
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        private void GuardarReporteTT(DateTime nuevaFecha)
        {
            if (nuevaFecha != repTT.FechaReporte || txtNumeroReporte.Text != repTT.NumeroReporte)
            {
                string errores = "";
                int cuenta = 0;
                List<ListaFechaNumeroControl> fechas = ReporteTtBO.Instance.ObtenerFechasRequisicionEdicion(repTT.ReporteTtID);
                if (fechas.Count > 0)
                {
                    foreach (ListaFechaNumeroControl fecha in fechas)
                    {
                        if (fecha.FechaProceso > nuevaFecha)
                        {
                            errores += "<br/>" + fecha.NumeroControl + " . Fecha Soldadura";
                            cuenta++;
                        }
                        if (fecha.FechaReporte > nuevaFecha)
                        {
                            errores += "<br/>" + fecha.NumeroControl + " . Fecha Requisición";
                            cuenta++;
                        }
                    }

                }
                else
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "Unable to validate the date of this report. No dates for welding process or requisition for this report found."
                        : "No se puede validar la fecha de este reporte. No se encontraron fechas de proceso de soldadura o de Requisición");
                }

                if (cuenta > 0)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The reporting date can not be less than the date of weld report or order No. " + errores
                        : "La fecha de reporte no puede ser menor a la fecha del reporte de soldadura de la orden No. " + errores);
                }
                bool existe = ReporteTtBO.Instance.EdicionEspecialValidaNumeroReporteTT(repTT.ReporteTtID, repTT.ProyectoID, txtNumeroReporte.Text);
                if (existe)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The report number is already exists."
                        : "El numero de reporte ya existe.");
                }

                repTT.StartTracking();
                repTT.NumeroReporte = txtNumeroReporte.Text;
                repTT.FechaReporte = nuevaFecha;
                repTT.UsuarioModifica = SessionFacade.UserId;
                repTT.FechaModificacion = DateTime.Now;
                repTT.StopTracking();

                ReporteTtBO.Instance.GuardarEdicionEspecialReporteTT(repTT);

                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            else
            {
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
        }

        private void GuardarReportePND(DateTime nuevaFecha)
        {
            if (nuevaFecha != repPND.FechaReporte || txtNumeroReporte.Text != repPND.NumeroReporte)
            {
                List<ListaFechaNumeroControl> fechas = ReportePndBO.Instance.ObtnerFechasSoldaduraReportePNDEdicion(repPND.ReportePndID);
                string errores = "";
                int cuenta = 0;
                if (fechas.Count > 0)
                {
                    foreach (ListaFechaNumeroControl fecha in fechas)
                    {
                        if (fecha.FechaProceso > nuevaFecha)
                        {
                            errores += "<br/>" + fecha.NumeroControl + " . Fecha Soldadura";
                            cuenta++;
                        }
                        if (fecha.FechaReporte > nuevaFecha)
                        {
                            errores += "<br/>" + fecha.NumeroControl + " . Fecha Requisición";
                            cuenta++;
                        }
                    }
                }
                else
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "Unable to validate the date of this report. No dates for welding process or NDT requisition for this report found."
                        : "No se puede validar la fecha de este reporte. No se encontraron fechas de proceso de soldadura o de Requisición de PND");
                }

                if (cuenta > 0)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The reporting date can not be less than the date of weld report or order No. " + errores
                        : "La fecha de reporte no puede ser menor a la fecha del reporte de soldadura de la orden No. " + errores);
                }
                bool existe = ReportePndBO.Instance.EdicionEspecialValidarNumeroReportePND(repPND.ReportePndID, repPND.ProyectoID, txtNumeroReporte.Text);
                if (existe)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The report number is already exists."
                        : "El numero de reporte ya existe.");
                }

                repPND.StartTracking();
                repPND.FechaReporte = nuevaFecha;
                repPND.NumeroReporte = txtNumeroReporte.Text;
                repPND.UsuarioModifica = SessionFacade.UserId;
                repPND.FechaModificacion = DateTime.Now;
                repPND.StopTracking();

                ReportePndBO.Instance.GuardarEdicionReportePND(repPND);

                JsUtils.RegistraScriptActualizaGridGenerico(this);


            }
            else
            {
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
        }

        private void GuardarReporteSpoolPND(DateTime nuevaFecha)
        {
            if (nuevaFecha != repSpoolPND.FechaReporte || repSpoolPND.NumeroReporte != txtNumeroReporte.Text)
            {
                List<ListaFechaNumeroControl> fechasRequisiciones = ReportePndBO.Instance.ObtenerFehcasRequisicionSpoolPND(repSpoolPND.ReporteSpoolPndID);
                string errores = "";
                int cuenta = 0;
                if (fechasRequisiciones.Count > 0)
                {
                    foreach (ListaFechaNumeroControl fechas in fechasRequisiciones)
                    {
                        if (fechas.FechaProceso > nuevaFecha)
                        {
                            errores += "<br/>" + fechas.NumeroControl;
                            cuenta++;
                        }
                    }
                }
                else
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "Unable to validate the date of this report. No dates for welding process or Test requisition for this report found."
                        : "No se puede validar la fecha de este reporte. No se encontraron fechas de proceso de soldadura o de Requisición de pruebas");
                }

                if (cuenta > 0)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The reporting date can not be less than the date of weld report or order No. " + errores
                        : "La fecha de reporte no puede ser menor a la fecha del reporte de soldadura de la orden No. " + errores);
                }
                bool existe = ReportePndBO.Instance.EdicionEspecialValidarNumeroReporteSpoolPND(repSpoolPND.ReporteSpoolPndID, repSpoolPND.ProyectoID, txtNumeroReporte.Text);
                if (existe)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The report number is already exists."
                        : "El numero de reporte ya existe.");
                }

                repSpoolPND.StartTracking();
                repSpoolPND.FechaReporte = rdpFechaReporte.SelectedDate.Value;
                repSpoolPND.NumeroReporte = txtNumeroReporte.Text;
                repSpoolPND.FechaModificacion = DateTime.Now;
                repSpoolPND.UsuarioModifica = SessionFacade.UserId;
                repSpoolPND.StopTracking();

                ReportePndBO.Instance.GuardarEdicionReporteSpoolPND(repSpoolPND);

                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            else
            {
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
        }

        private void GuardarInspeccionDimensional(DateTime nuevaFecha)
        {
            if (nuevaFecha != repDimensional.FechaReporte || repDimensional.NumeroReporte != txtNumeroReporte.Text)
            {
                List<ListaFechaNumeroControl> fechasSoldadura = ReporteDimensionalBO.Instance.ObtenerFechasReporteSoldaduraEdicionDimensional(repDimensional.ReporteDimensionalID);
                List<DateTime?> fechasDetalle = ReporteDimensionalBO.Instance.ObtenerFechasDetalleDimensional(repDimensional.ReporteDimensionalID);

                string errores = "";
                int cuenta = 0;
                if (fechasSoldadura.Count > 0)
                {
                    foreach (ListaFechaNumeroControl fechas in fechasSoldadura)
                    {
                        if (fechas.FechaReporte > nuevaFecha || fechas.FechaProceso > nuevaFecha)
                        {
                            errores += "<br/>" + fechas.NumeroControl;
                            cuenta++;
                        }
                    }
                }
                else
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "Unable to validate the date of this report. No dates for welding process or requisition for this report found."
                        : "No se puede validar la fecha de este reporte. No se encontraron fechas de proceso de soldadura o de Requisición");
                }

                if (cuenta > 0)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The reporting date can not be less than the date of weld report or order No. " + errores
                        : "La fecha de reporte no puede ser menor a la fecha del reporte de soldadura de la orden No. " + errores);
                }

                if (fechasDetalle.Where(x => x.Value > nuevaFecha).Any())
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The reporting date can not be less than the date of release."
                        : "La fecha de reporte no puede ser menor a la fecha de la liberacion dimensional.");
                }

                bool existe = ReporteDimensionalBO.Instance.ValidarNumeroReporteEdicion(repDimensional.ReporteDimensionalID, repDimensional.ProyectoID, txtNumeroReporte.Text);
                if (existe)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The report number is already exists."
                        : "El numero de reporte ya existe.");
                }

                repDimensional.StartTracking();
                repDimensional.FechaReporte = nuevaFecha;
                repDimensional.NumeroReporte = txtNumeroReporte.Text == repDimensional.NumeroReporte ? repDimensional.NumeroReporte : txtNumeroReporte.Text;
                repDimensional.UsuarioModifica = SessionFacade.UserId;
                repDimensional.FechaModificacion = DateTime.Now;
                repDimensional.StopTracking();

                ReporteDimensionalBO.Instance.GuardarEdicionReporteDimensional(repDimensional);

                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            else
            {
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
        }

        private void GuardarInspeccionVisual(DateTime nuevaFecha)
        {
            if (nuevaFecha != inspecVisual.FechaReporte || txtNumeroReporte.Text != inspecVisual.NumeroReporte)
            {
                List<ListaFechaNumeroControl> fechas = JuntaInspeccionVisualBO.Instance.ObtenerFechasSoldaduraEdicionVisual(inspecVisual.InspeccionVisualID);
                int cuenta = 0;
                string errores = "";
                if (fechas.Count > 0)
                {
                    foreach (ListaFechaNumeroControl resultado in fechas)
                    {
                        if (resultado.FechaProceso > nuevaFecha || resultado.FechaReporte > nuevaFecha)
                        {
                            errores += "<br/>" + resultado.NumeroControl;
                            cuenta++;
                        }
                    }
                }
                throw new BaseValidationException(Cultura == "en-US" ? "Unable to validate the date of this report. No dates for welding process or requisition for this report found."
                        : "No se puede validar la fecha de este reporte. No se encontraron fechas de proceso de soldadura o de Requisición");

                if (cuenta > 0)
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The reporting date can not be less than the date of weld report of order No. " + errores
                        : "La fecha de reporte no puede ser menor a la fecha de reporte de soldadura de la orden No. " + errores);
                }

                if (InspeccionVisualBO.Instance.ValidarNumeroReporteEdicionEspecial(txtNumeroReporte.Text, inspecVisual.ProyectoID, inspecVisual.InspeccionVisualID))
                {
                    throw new BaseValidationException(Cultura == "en-US" ? "The report number es already exists."
                        : "El numero de reporte ya existe.");
                }

                inspecVisual.StartTracking();
                inspecVisual.FechaReporte = nuevaFecha;
                inspecVisual.NumeroReporte = txtNumeroReporte.Text;
                inspecVisual.FechaModificacion = DateTime.Now;
                inspecVisual.UsuarioModifica = SessionFacade.UserId;
                inspecVisual.StopTracking();

                InspeccionVisualBO.Instance.GuardarEdicionInspeccionVisual(inspecVisual);

                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            else
            {
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
        }
    }
}