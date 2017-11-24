using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Exceptions;
using SAM.Entities.Grid;
using System.Globalization;
using SAM.BusinessObjects.Produccion;

namespace SAM.Web.Produccion.JuntasCampo
{
    public partial class PruebasTT : SamPaginaPopup
    {

        private int JuntaSpoolID
        {
            get
            {
                return ViewState["JuntaSpoolID"].ToString().SafeIntParse();
            }
            set
            {
                ViewState["JuntaSpoolID"] = value;
            }
        }

        private int JuntaCampoID
        {
            get
            {
                return ViewState["JuntaCampoID"].SafeIntParse();
            }
            set
            {
                ViewState["JuntaCampoID"] = value;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JuntaSpoolID = Request.QueryString["JuntaSpoolID"].SafeIntParse();
                cargaDatos();

            }
        }

        private RequisicionCampo ReqCampo
        {
            get
            {
                if (ViewState["ReqCampo"] == null)
                {
                    ViewState["ReqCampo"] = new RequisicionCampo();
                }
                return (RequisicionCampo)ViewState["ReqCampo"];
            }
            set
            {
                ViewState["ReqCampo"] = value;
            }
        }

        protected void cargaDatos()
        {
            if (JuntaSpoolID > 0)
            {
                limpiarDatos();
                JuntasCampoDTO datos = JuntaCampoBO.Instance.DetalleJunta(JuntaSpoolID);
                litSpool.Text = datos.Spool;
                litJunta.Text = datos.Junta;
                litTipoJunta.Text = datos.TipoJunta;
                litNumeroControl.Text = datos.NumeroControl;
                litLocalizacion.Text = datos.EtiquetaMaterial1 + "-" + datos.EtiquetaMaterial2;
                litEspesor.Text = datos.Espesor.SafeStringParse();

                JuntaCampoID = datos.JuntaCampoID;

                if (JuntaCampoID > -1)
                {
                    string fechasIV = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasIVJuntasCampo(JuntaCampoID);
                    string fechasSoldadura = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasSoldaduraJuntasCampo(JuntaCampoID);
                    string fechasArmado = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasArmadoJuntasCampo(JuntaCampoID);

                    btnGuardarPopUp.OnClientClick = "return Sam.Produccion.ValidaNuevasFechas('3','" + mdpFechaPND.ClientID + "', '" + fechasIV + "','" + fechasSoldadura + "','" + fechasArmado + "')";
                }

                hdnProyectoID.Value = datos.ProyectoID.ToString();
            }

            ddlRequisicion.BindToEnumerableWithEmptyRow(JuntaCampoBO.Instance.ObtenerListadoRequisicionesTTPendientes(JuntaCampoID), "Valor", "ID", -1);

            cargaDatosPruebasTT();
        }

        protected void ddlRequisicion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRequisicion.SelectedValue.SafeIntParse() > 0)
            {
                ReqCampo = JuntaCampoBO.Instance.ObtenerRequisicionCampo(ddlRequisicion.SelectedValue.SafeIntParse());
                
                btnGuardar.OnClientClick = "return Sam.Produccion.CambioFechasPruebas('" + rdwCambiarFechaProcesoAnterior.ClientID + "', '" + ReqCampo.FechaRequisicion.ToString("MM/dd/yyyy") + "')";
                mdpFechaProcesoAnterior.SelectedDate = ReqCampo.FechaRequisicion;
                mdpFechaReporteProcesoAnterior.SelectedDate = ReqCampo.FechaRequisicion;

                TipoPruebaID = ReqCampo.TipoPruebaID;
                litTipoPrueba.Text = CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.ID == ReqCampo.TipoPruebaID).Select(x => x.Nombre).FirstOrDefault();

            }
        }

        protected void limpiarDatos()
        {
            mdpFechaPND.SelectedDate = null;
            litTipoPrueba.Text = string.Empty;
            mdpFechaReporte.SelectedDate = null;
            ddlRequisicion.SelectedIndex = -1;
            txtNumeroReporte.Text = string.Empty;
            txtNumeroGrafica.Text = string.Empty;
            ddlResultado.SelectedIndex = -1;
            txtObservaciones.Text = string.Empty;
        }

        protected void btnGuardarPopUp_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    JuntaCampo junta = JuntaCampoBO.Instance.ObtenerPorID(JuntaCampoID);

                    if (hdnCambiaFechas.Value == "1")
                    {
                        JuntaCampoBO.Instance.CambiaFechasRequisicion(ddlRequisicion.SelectedValue.SafeIntParse(), mdpFechaProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }
                    else if (hdnCambiaFechas.Value == "2")
                    {
                        DateTime fechaReq = DateTime.ParseExact(hdnProcesoAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        JuntaCampoBO.Instance.CambiaFechasRequisicion(ddlRequisicion.SelectedValue.SafeIntParse(), fechaReq, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasIV(junta.JuntaCampoInspeccionVisualID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }
                    else if (hdnCambiaFechas.Value == "3")
                    {
                        DateTime fechaReq = DateTime.ParseExact(hdnProcesoAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        DateTime fechaIV = DateTime.ParseExact(hdnProcesoAnterior3.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime fechaReporteIV = DateTime.ParseExact(hdnProcesoReporteAnterior3.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        JuntaCampoBO.Instance.CambiaFechasRequisicion(ddlRequisicion.SelectedValue.SafeIntParse(), fechaReq, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasIV(junta.JuntaCampoInspeccionVisualID.Value, fechaIV, fechaReporteIV, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoSoldadura(junta.JuntaCampoSoldaduraID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }
                    else if (hdnCambiaFechas.Value == "4")
                    {
                        DateTime fechaReq = DateTime.ParseExact(hdnProcesoAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        DateTime fechaIV = DateTime.ParseExact(hdnProcesoAnterior3.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime fechaReporteIV = DateTime.ParseExact(hdnProcesoReporteAnterior3.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        DateTime fechaSoldadura = DateTime.ParseExact(hdnProcesoAnterior4.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime fechaReporteSoldadura = DateTime.ParseExact(hdnProcesoReporteAnterior4.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        JuntaCampoBO.Instance.CambiaFechasRequisicion(ddlRequisicion.SelectedValue.SafeIntParse(), fechaReq, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasIV(junta.JuntaCampoInspeccionVisualID.Value, fechaIV, fechaReporteIV, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoSoldadura(junta.JuntaCampoSoldaduraID.Value, fechaSoldadura, fechaReporteSoldadura, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoArmado(junta.JuntaCampoArmadoID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }

                    ReporteCampoTT reporte = new ReporteCampoTT
                    {
                        ProyectoID = hdnProyectoID.Value.SafeIntParse(),
                        TipoPruebaID = TipoPruebaID,
                        NumeroReporte = txtNumeroReporte.Text,
                        FechaReporte = mdpFechaReporte.SelectedDate.Value
                    };

                    JuntaCampoReporteTT juntaReporte = new JuntaCampoReporteTT
                    {
                        NumeroGrafica = txtNumeroGrafica.Text,
                        FechaTratamiento = mdpFechaPND.SelectedDate.Value,
                        Aprobado = ddlResultado.SelectedValue == "0" ? false : true,
                        Observaciones = txtObservaciones.Text
                    };

                    JuntaCampoBO.Instance.GuardaReporteTt(reporte, juntaReporte, JuntaCampoID, ddlRequisicion.SelectedValue.SafeIntParse(), SessionFacade.UserId);

                    cargaDatosPruebasTT();
                    limpiarDatos();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }

            }
        }

        public void cargaDatosPruebasTT()
        {
            List<GrdDetReporteCampo> datasource = JuntaCampoBO.Instance.ObtenerListadoReportesTT(JuntaCampoID);
            repPruebasTT.DataSource = datasource;
            repPruebasTT.DataBind();
        }

        protected void repPruebasTT_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int JuntaCampoReporteTTID = e.CommandArgument.SafeIntParse();

                try
                {
                    JuntaCampoBO.Instance.EliminarReporteTT(JuntaCampoReporteTTID);
                    cargaDatosPruebasTT();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        } 

        protected void repPruebasTT_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
    }
}