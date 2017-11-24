using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Extensions;
using System.Globalization;
using Mimo.Framework.Common;
using SAM.Web.Common;
using System.Threading;

namespace SAM.Web.WorkStatus
{
    public partial class PopupGenerarRequisicion : SamPaginaPopup
    {

        public int[] WorkstatusSpools
        {
            get
            {
                if (ViewState["WorkstatusSpools"] != null)
                {
                    return (int[])ViewState["WorkstatusSpools"];
                }
                return null;
            }
            set
            {
                ViewState["WorkstatusSpools"] = value;
            }
        }

        public int[] OrdenTrabajoSpoolIds
        {
            get
            {
                if (ViewState["OrdenTrabajoSpoolIds"] != null)
                {
                    return (int[])ViewState["OrdenTrabajoSpoolIds"];
                }
                return null;
            }
            set
            {
                ViewState["OrdenTrabajoSpoolIds"] = value;
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
                string ids = Request.QueryString["RPID"];
                WorkstatusSpools = ids.Split(',').Select(n => n.SafeIntParse()).ToArray();
                string otsIDs = Request.QueryString["OTSIDs"];
                OrdenTrabajoSpoolIds = otsIDs.Split(',').Select(n => n.SafeIntParse()).ToArray();

                if (!SeguridadQs.TieneAccesoATodosWorkstatusSpools(WorkstatusSpools))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando generar una requisición para algún workstatus spool {1} al cual no tiene permisos", SessionFacade.UserId, ids);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Fechas = ValidaFechasBO.Instance.ObtenerFechasDimensionalesPorOts(OrdenTrabajoSpoolIds);

                NumerosControl = ValidaFechasBO.Instance.ObtenerNumerosControlPorWorkstatusOtsId(OrdenTrabajoSpoolIds);
             
                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    Proceso = "Dimensional Inspection";
                }
                else
                {
                    Proceso = "Liberación Dimensional";
                }

                //btnRequisitar.OnClientClick = "return Sam.Workstatus.ValidacionFechasPruebas('" + numerosControl + "' ,'" + Fechas + "','2','" + proceso + "')";
            }
        }

        protected void ValidarFechas()
        {
            try
            {
                string errores = "";
                DateTime fechaRequisicion = new DateTime(mdpFechaReq.SelectedDate.Value.Year, mdpFechaReq.SelectedDate.Value.Month, mdpFechaReq.SelectedDate.Value.Day);
                string[] fechas = Fechas.Split(',');
                string[] numerosControl = NumerosControl.Split(',');
                int count = 0;

                for (int i = 0; i < fechas.Length; i++)
                {
                    if (fechas[i] != string.Empty)
                    {
                        DateTime tempFecha = DateTime.ParseExact(fechas[i], "MM/dd/yyyy", null);
                        if (fechaRequisicion < tempFecha)
                        {
                            errores += "<br />" + numerosControl[i];
                            count++;
                        }
                    }
                }

                if (count > 0)
                {
                    errores += "<br />";
                }

                if (count == fechas.Length)
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
                else if (errores.Length > 0 && count < fechas.Length)
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        lblPopup.Text = String.Format(
                            "The control number {0} can't be save because have wrong date, Do you want to save the report for the others control numbers?", errores);
                    }
                    else
                    {
                        lblPopup.Text = String.Format(
                            "Los números de control {0} no se pueden guardar porque tienen fecha incorrecta, ¿Desea guardar el reporte para los demás números de control?", errores);
                    }
                    radwindowPopup.VisibleOnPageLoad = true;
                }
                else
                {
                    Requisitar();
                }
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

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                radwindowPopup.VisibleOnPageLoad = false;
                Requisitar();

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                radwindowPopup.VisibleOnPageLoad = false;
                throw new BaseValidationException("Cancelado");
            }
            catch(BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void Requisitar()
        {
            //ValidarFechas();
            string[] arregloFechas = Fechas.Split(',');
            for (int i = 0; i < arregloFechas.Length; i++)
            {
                if (arregloFechas[i] != string.Empty)
                {
                    DateTime fecha = DateTime.ParseExact(arregloFechas[i], "MM/dd/yyyy", null);

                    if (mdpFechaReq.SelectedDate.Value < fecha)
                    {
                        OrdenTrabajoSpoolIds[i] = -1;
                    }
                }
            }

            RequisicionPinturaBO.Instance.GeneraRequisicion(EntityID.Value, txtNumReq.Text, mdpFechaReq.SelectedDate.Value, OrdenTrabajoSpoolIds, SessionFacade.UserId);
        }

        /// <summary>
        /// Genera la requisicion para los workstatus spools enviados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btnRequisitar_OnClick(object sender, EventArgs e)
        //{
        //    //if (IsValid)
        //    //{
           
        //        try
        //        {
        //            //ValidarFechas();
        //            string[] arregloFechas = Fechas.Split(',');
        //            for (int i = 0; i < arregloFechas.Length; i++)
        //            {
        //                DateTime fecha = DateTime.ParseExact(arregloFechas[i], "MM/dd/yyyy", CultureInfo.InvariantCulture);

        //                if (mdpFechaReq.SelectedDate.Value < fecha)
        //                {
        //                    WorkstatusSpools[i] = -1;
        //                }
        //            }

        //            RequisicionPinturaBO.Instance.GeneraRequisicion(EntityID.Value, txtNumReq.Text, mdpFechaReq.SelectedDate.Value, WorkstatusSpools, SessionFacade.UserId);
        //            JsUtils.RegistraScriptActualizaGridGenerico(this);
        //        }
        //        catch (BaseValidationException ex)
        //        {
        //            RenderErrors(ex);
        //        }
        //    //}
        //}

        protected void btnARequisitar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    ValidarFechas();
                }
                catch (BaseValidationException ex)
                {
                    JsUtils.RegistraScriptActualizaGridGenerico(this);
                    RenderErrors(ex);

                }
                finally
                {
                  
                }
            }
        }
    }
}