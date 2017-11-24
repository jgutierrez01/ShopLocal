using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Produccion;
using System.Globalization;

namespace SAM.Web.Produccion.JuntasCampo
{
    public partial class Requisiciones : SamPaginaPopup
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

        private int ProyectoID
        {
            get
            {
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private string FechasIV
        {
            get
            {
                if (ViewState["FechasIV"] == null)
                {
                    ViewState["FechasIV"] = string.Empty;
                }

                return ViewState["FechasIV"].ToString();
            }
            set
            {
                ViewState["FechasIV"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JuntaSpoolID = Request.QueryString["JuntaSpoolID"].SafeIntParse();
                cargaDatos(JuntaSpoolID);
            }
        }

        private void cargaDatos(int juntaSpoolID)
        {
            if (juntaSpoolID > 0)
            {
                FechasIV = string.Empty;
                JuntasCampoDTO datos = JuntaCampoBO.Instance.DetalleJunta(juntaSpoolID);
                litSpool.Text = datos.Spool;
                litJunta.Text = datos.Junta;
                litTipoJunta.Text = datos.TipoJunta;
                litNumeroControl.Text = datos.NumeroControl;
                litLocalizacion.Text = datos.EtiquetaMaterial1 + "-" + datos.EtiquetaMaterial2;
                litEspesor.Text = datos.Espesor.SafeStringParse();

                JuntaCampoID = datos.JuntaCampoID;
                if (JuntaCampoID > -1)
                {
                    FechasIV = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasIVJuntasCampo(JuntaCampoID);
                }

                if (FechasIV != string.Empty)
                {
                    mdpFechaProcesoAnterior.SelectedDate = ValidaFechasJuntasCampoBO.Instance.ObtenerFechaIVJuntaCampo(JuntaCampoID);
                    mdpFechaReporteProcesoAnterior.SelectedDate = ValidaFechasJuntasCampoBO.Instance.ObtenerFechaReporteIVJuntaCampo(JuntaCampoID);
                    string fechasSoldadura = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasSoldaduraJuntasCampo(JuntaCampoID);
                    string fechasArmado = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasArmadoJuntasCampo(JuntaCampoID);

                    btnRequisitar.OnClientClick = "return Sam.Produccion.CambioFechasRequisiciones('" + rdwCambiarFechaProcesoAnterior.ClientID + "', '" + FechasIV + "')";
                    btnGuardarPopUp.OnClientClick = "return Sam.Produccion.ValidaNuevasFechas('2','" + mdpFechaRequisicion.ClientID + "','" + fechasSoldadura + "','" + fechasArmado + "')";
                    hdnCambiaFechas.Value = "0";
                }

                ProyectoID = datos.ProyectoID;
            }

            ddlTipoPrueba.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposPrueba());

            cargaRequisicones();
        }


        protected void btnGuardarPopUp_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    JuntaCampo junta = JuntaCampoBO.Instance.ObtenerPorID(JuntaCampoID);
                    ValidacionesJuntaCampo.ValidaInspeccionVisualAprobada(junta);

                    if (hdnCambiaFechas.Value == "1")
                    {
                        JuntaCampoBO.Instance.CambiaFechasIV(junta.JuntaCampoInspeccionVisualID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }
                    else if (hdnCambiaFechas.Value == "2")
                    {
                        DateTime fechaIV = DateTime.ParseExact(hdnProcesoAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime fechaReporteIV = DateTime.ParseExact(hdnProcesoReporteAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        JuntaCampoBO.Instance.CambiaFechasIV(junta.JuntaCampoInspeccionVisualID.Value, fechaIV, fechaReporteIV, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoSoldadura(junta.JuntaCampoSoldaduraID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);                        
                    }
                    else if (hdnCambiaFechas.Value == "3")
                    {
                        DateTime fechaIV = DateTime.ParseExact(hdnProcesoAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime fechaReporteIV = DateTime.ParseExact(hdnProcesoReporteAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        DateTime fechaSoldadura = DateTime.ParseExact(hdnProcesoAnterior3.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime fechaReporteSoldadura = DateTime.ParseExact(hdnProcesoReporteAnterior3.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        JuntaCampoBO.Instance.CambiaFechasIV(junta.JuntaCampoInspeccionVisualID.Value, fechaIV, fechaReporteIV, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoSoldadura(junta.JuntaCampoSoldaduraID.Value, fechaSoldadura, fechaReporteSoldadura, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasJuntaCampoArmado(junta.JuntaCampoArmadoID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }

                    FechasIV = ValidaFechasJuntasCampoBO.Instance.ObtenerFechasIVJuntasCampo(JuntaCampoID);
                    btnRequisitar.OnClientClick = "return Sam.Produccion.CambioFechasRequisiciones('" + rdwCambiarFechaProcesoAnterior.ClientID + "', '" + FechasIV + "')";

                    RequisicionCampo requisicion = new RequisicionCampo
                    {
                        ProyectoID = ProyectoID,
                        TipoPruebaID = ddlTipoPrueba.SelectedValue.SafeIntParse(),
                        FechaRequisicion = mdpFechaRequisicion.SelectedDate.Value,
                        NumeroRequisicion = txtNumeroRequisicion.Text,
                        CodigoAsme = txtCodigo.Text
                    };

                    JuntaCampoBO.Instance.GeneraRequisicion(requisicion, JuntaCampoID, SessionFacade.UserId);

                    cargaRequisicones();

                    limpiarDatos();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        private void limpiarDatos()
        {
            mdpFechaRequisicion.SelectedDate = null;
            ddlTipoPrueba.SelectedIndex = -1;
            txtNumeroRequisicion.Text = string.Empty;
            txtCodigo.Text = string.Empty;
        }

        private void cargaRequisicones()
        {
            List<GrdRequisicionesCampo> lista = JuntaCampoBO.Instance.ObtenerListadoRequisiciones(JuntaCampoID);

            if (lista != null)
            {
                repRequi.Visible = true;
                repRequi.DataSource = lista;
                repRequi.DataBind();
            }
            else
                repRequi.Visible = false;
        }

        protected void repRequi_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Borrar")
                {
                    int RequisicionID = e.CommandArgument.SafeIntParse();

                    JuntaCampoBO.Instance.EliminarRequisiciones(RequisicionID);
                    cargaRequisicones();
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}
