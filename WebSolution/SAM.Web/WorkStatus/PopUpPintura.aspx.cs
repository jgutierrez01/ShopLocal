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
using SAM.Entities;
using System.Globalization;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpPintura : SamPaginaPopup
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

        private string otsIDs
        {
            get
            {
                if (ViewState["OTSIDs"] == null)
                {
                    ViewState["OTSIDs"] = string.Empty;
                }

                return ViewState["OTSIDs"].ToString();
            }
            set
            {
                ViewState["OTSIDs"] = value;
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

        private string Fechas
        {
            get
            {
                if (ViewState["Fecha"] == null)
                {
                    ViewState["Fecha"] = string.Empty;
                }

                return ViewState["Fecha"].ToString();
            }
            set
            {
                ViewState["Fecha"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IDs = Request.QueryString["OTSIDs"];
                RIDs = Request.QueryString["RID"];

                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando pintar spools de un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Fechas = ValidaFechasBO.Instance.ObtenerFechaReqPinturaPorOtsID(RIDs);               
            }
        }

        protected void ValidaFechas()
        {
            DateTime defaultDateTime = new DateTime();            
            DateTime sandBlast = DateTime.ParseExact(rdpFechaSandBlast.SelectedDate.SafeDateAsStringParse() == null ? defaultDateTime.ToShortDateString(): rdpFechaSandBlast.SelectedDate.SafeDateAsStringParse() , "dd/MM/yyyy", null);
            DateTime acabdoVisual = DateTime.ParseExact(rdpAcabadoVisual.SelectedDate.SafeDateAsStringParse() == null ? defaultDateTime.ToShortDateString() : rdpAcabadoVisual.SelectedDate.SafeDateAsStringParse(), "dd/MM/yyyy", null);
            DateTime FechaAdherencia = DateTime.ParseExact(rdpFechaAdherencia.SelectedDate.SafeDateAsStringParse() == null ? defaultDateTime.ToShortDateString(): rdpFechaAdherencia.SelectedDate.SafeDateAsStringParse(), "dd/MM/yyyy", null);
            DateTime FechaIntermedio = DateTime.ParseExact(rdpFechaIntermedio.SelectedDate.SafeDateAsStringParse() == null ? defaultDateTime.ToShortDateString(): rdpFechaIntermedio.SelectedDate.SafeDateAsStringParse(), "dd/MM/yyyy", null);
            DateTime FechaPrimario = DateTime.ParseExact(rdpFechaPrimario.SelectedDate.SafeDateAsStringParse() == null ? defaultDateTime.ToShortDateString() : rdpFechaPrimario.SelectedDate.SafeDateAsStringParse(), "dd/MM/yyyy", null);
            
            int[] otsIDs = IDs.Split(',').Select(x => x.SafeIntParse()).ToArray();
            string[] fechas = Fechas.Split(',');

            for (int i = 0; i < fechas.Length; i++)
            {
                if (!string.IsNullOrEmpty(fechas[i]))
                {
                    DateTime tempFecha = DateTime.ParseExact(fechas[i], "MM/dd/yyyy", null);
                    if (sandBlast != defaultDateTime && sandBlast < tempFecha)
                    {
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            throw new BaseValidationException(String.Format("There's a problem with the {0} date because it can not be lower that paint requisition", sandBlast.ToString()));
                        }
                        else
                        {
                            throw new BaseValidationException(String.Format("Hay un problema con la fecha de {0}, ya que no puede ser menor que la requisición de pintura", sandBlast.ToString()));
                        }
                    }
                    if (acabdoVisual != defaultDateTime && acabdoVisual < tempFecha)
                    {
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            throw new BaseValidationException(String.Format("There's a problem with the {0} date because it can not be lower that paint requisition", acabdoVisual.ToString()));
                        }
                        else
                        {
                            throw new BaseValidationException(String.Format("Hay un problema con la fecha de {0}, ya que no puede ser menor que la requisición de pintura", acabdoVisual.ToString()));
                        }
                    }
                    if (FechaAdherencia != defaultDateTime && FechaAdherencia < tempFecha)
                    {
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            throw new BaseValidationException(String.Format("There's a problem with the {0} date because it can not be lower that paint requisition", FechaAdherencia.ToString()));
                        }
                        else
                        {
                            throw new BaseValidationException(String.Format("Hay un problema con la fecha de {0}, ya que no puede ser menor que la requisición de pintura", FechaAdherencia.ToString()));
                        }
                    }
                    if (FechaIntermedio != defaultDateTime && FechaIntermedio < tempFecha)
                    {
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            throw new BaseValidationException(String.Format("There's a problem with the {0} date because it can not be lower that paint requisition", FechaIntermedio.ToString()));
                        }
                        else
                        {
                            throw new BaseValidationException(String.Format("Hay un problema con la fecha de {0}, ya que no puede ser menor que la requisición de pintura", FechaIntermedio.ToString()));
                        }
                    }
                    if (FechaPrimario != defaultDateTime && FechaPrimario < tempFecha)
                    {
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            throw new BaseValidationException(String.Format("There's a problem with the {0} date because it can not be lower that paint requisition", FechaPrimario.ToString()));
                        }
                        else
                        {
                            throw new BaseValidationException(String.Format("Hay un problema con la fecha de {0}, ya que no puede ser menor que la requisición de pintura", FechaPrimario.ToString()));
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Guarda la información de los reportes de pintura.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

                PinturaSpool spool = new PinturaSpool();
                spool.ProyectoID = EntityID.Value;

                if (rdpFechaSandBlast.SelectedDate.HasValue)
                {
                    spool.FechaSandblast = rdpFechaSandBlast.SelectedDate.Value;
                }

                if (rdpFechaPrimario.SelectedDate.HasValue)
                {
                    spool.FechaPrimarios = rdpFechaPrimario.SelectedDate.Value;
                }

                if (rdpFechaIntermedio.SelectedDate.HasValue)
                {
                    spool.FechaIntermedios = rdpFechaIntermedio.SelectedDate.Value;
                }

                if (rdpAcabadoVisual.SelectedDate.HasValue)
                {
                    spool.FechaAcabadoVisual = rdpAcabadoVisual.SelectedDate.Value;
                }

                if (rdpFechaAdherencia.SelectedDate.HasValue)
                {
                    spool.FechaAdherencia = rdpFechaAdherencia.SelectedDate.Value;
                }

                if (rdpPullOff.SelectedDate.HasValue)
                {
                    spool.FechaPullOff = rdpPullOff.SelectedDate.Value;
                }

                if (!string.IsNullOrEmpty(txtReporteSandBlast.Text))
                {
                    spool.ReporteSandblast = txtReporteSandBlast.Text;
                }

                if (!string.IsNullOrEmpty(txtReportePrimario.Text))
                {
                    spool.ReportePrimarios = txtReportePrimario.Text;
                }

                if (!string.IsNullOrEmpty(txtReporteIntermedio.Text))
                {
                    spool.ReporteIntermedios = txtReporteIntermedio.Text;
                }

                if (!string.IsNullOrEmpty(txtReporteAcabadoVisual.Text))
                {
                    spool.ReporteAcabadoVisual = txtReporteAcabadoVisual.Text;
                }

                if (!string.IsNullOrEmpty(txtReporteAdherencia.Text))
                {
                    spool.ReporteAdherencia = txtReporteAdherencia.Text;
                }

                if (!string.IsNullOrEmpty(txtReportePullOff.Text))
                {
                    spool.ReportePullOff = txtReportePullOff.Text;
                }

                PinturaBO.Instance.GuardaPinturaSpool(spool, IDs, RIDs, chkLiberado.Checked, Fechas, SessionFacade.UserId);

            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
            finally { JsUtils.RegistraScriptActualizaGridGenerico(this); }
        }
    }
}