using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Workstatus;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Produccion;
using System.Globalization;

namespace SAM.Web.Produccion.JuntasCampo
{
    public partial class PruebasPND : SamPaginaPopup
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

        private int IDCounter
        {
            get
            {
                if (ViewState["IDCunter"] == null)
                {
                    ViewState["IDCunter"] = 0;
                }

                return ViewState["IDCunter"].SafeIntParse();
            }
            set
            {
                ViewState["IDCunter"] = value;
            }
        }

        private List<JuntaCampoReportePNDSector> DefectosSector
        {
            get
            {
                if (ViewState["DefectosSector"] == null)
                {
                    ViewState["DefectosSector"] = new List<JuntaCampoReportePNDSector>();
                }
                return (List<JuntaCampoReportePNDSector>)ViewState["DefectosSector"];
            }
            set
            {
                ViewState["DefectosSector"] = value;
            }
        }

        private List<JuntaCampoReportePNDCuadrante> DefectosCuadrante
        {
            get
            {
                if (ViewState["DefectosCuadrante"] == null)
                {
                    ViewState["DefectosCuadrante"] = new List<JuntaCampoReportePNDCuadrante>();
                }
                return (List<JuntaCampoReportePNDCuadrante>)ViewState["DefectosCuadrante"];
            }
            set
            {
                ViewState["DefectosCuadrante"] = value;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JuntaSpoolID = Request.QueryString["JuntaSpoolID"].SafeIntParse();
                cargaDatos();

            }
        }

        protected void cargaDatos()
        {
            if (JuntaSpoolID > 0)
            {
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

                    hdnCambiaFechas.Value = "0";
                }

                hdnProyectoID.Value = datos.ProyectoID.ToString();

                string parteDerechaEtiqueta = datos.Junta.Substring(datos.EtiquetaIngenieria.Length);
                
                //Significa que es junta de rechazo
                if (parteDerechaEtiqueta.ContainsIgnoreCase("r"))
                {
                    phHistoria.Visible = true;
                    cargaRechazosHistoricos();
                }
                else
                {
                    phHistoria.Visible = false;
                }
            }

            ddlRequisicion.BindToEnumerableWithEmptyRow(JuntaCampoBO.Instance.ObtenerListadoRequisicionesPNDPendientes(JuntaCampoID), "Valor", "ID", -1);

            cargaDatosPruebasPND();
        }

        /// <summary>
        /// 
        /// </summary>
        private void cargaRechazosHistoricos()
        {
            List<GrdDetReporteCampo> ds = JuntaCampoBO.Instance.ObtenerRechazosHistoricos(JuntaCampoID);

            repPruebasHistoricasRechazadas.DataSource = ds;
            repPruebasHistoricasRechazadas.DataBind();
        }

        public void cargaDatosPruebasPND()
        {
            List<GrdDetReporteCampo> datasource = JuntaCampoBO.Instance.ObtenerListadoReportesPND(JuntaCampoID);
            repPruebasPND.DataSource = datasource;
            repPruebasPND.DataBind();
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
                litTipoPrueba.Text = CacheCatalogos.Instance.ObtenerTiposPrueba().Where(x => x.ID == ReqCampo.TipoPruebaID).Select(x => x.Nombre).FirstOrDefault().ToString();

                if (TipoPruebaID == 10)
                {
                    ddlResultado.SelectedIndex = 2;
                }
                else
                {
                    ddlResultado.SelectedIndex = -1;
                }

                ddlDefecto.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.TipoPruebaID == TipoPruebaID));
            }
        }

        protected void ddlResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlResultado.SelectedValue == "1")
            {
                pnlDefecto.Visible = false;
                ddlTipoDefecto.SelectedIndex = -1;
                pnlCuadrante.Visible = false;
                pnlSector.Visible = false;
            }
            else
            {
                if (TipoPruebaID == 10)
                {
                    ddlTipoDefecto.SelectedIndex = 2;
                    ddlDefecto.SelectedIndex = 1;

                }
                else
                {
                    pnlDefecto.Visible = true;
                }
            }
        }

        protected void ddlTipoDefecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoDefecto.SelectedValue.SafeIntParse() == (int)TipoRechazoEnum.Sector)
            {
                pnlSector.Visible = true;
                pnlCuadrante.Visible = false;
            }
            else
            {
                pnlSector.Visible = false;
                pnlCuadrante.Visible = true;
            }
        }

        protected void repDefectos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                JuntaCampoReportePNDCuadrante cuadrante = e.Item.DataItem as JuntaCampoReportePNDCuadrante;
                Literal litNombreDefecto = e.Item.FindControl("litNombreDefecto") as Literal;
                litNombreDefecto.Text = CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.ID == cuadrante.DefectoID).Single().Nombre;
            }
        }

        protected void repDefectoSector_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                JuntaCampoReportePNDSector sector = e.Item.DataItem as JuntaCampoReportePNDSector;
                Literal litNombreDefecto = e.Item.FindControl("litNombreDefecto") as Literal;
                litNombreDefecto.Text = CacheCatalogos.Instance.ObtenerDefectos().Where(x => x.ID == sector.DefectoID).Single().Nombre;
            }
        }

        protected void repDefectos_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                JuntaCampoReportePNDCuadrante defecto = DefectosCuadrante.SingleOrDefault(d => d.JuntaCampoReportePNDCuadranteID == e.CommandArgument.SafeIntParse());

                if (defecto != null)
                {
                    DefectosCuadrante.Remove(defecto);
                }
            }

            repDefectos.DataSource = DefectosCuadrante;
            repDefectos.DataBind();
        }

        protected void repDefectoSector_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                JuntaCampoReportePNDSector defecto = DefectosSector.SingleOrDefault(d => d.JuntaCampoReportePNDSectorID == e.CommandArgument.SafeIntParse());

                if (defecto != null)
                {
                    DefectosSector.Remove(defecto);
                }

            }

            repDefectoSector.DataSource = DefectosSector;
            repDefectoSector.DataBind();
        }

        protected void btnDefectpCuadrante_Click(object sender, EventArgs e)
        {
            try
            {
                JuntaCampoBO.Instance.ValidarDefecto(ddlDefecto.SelectedValue.SafeIntParse());
                IDCounter++;
                JuntaCampoReportePNDCuadrante defecto = new JuntaCampoReportePNDCuadrante
                {
                    JuntaCampoReportePNDCuadranteID = IDCounter,
                    Cuadrante = txtCuadrante.Text,
                    Placa = txtPlaca.Text,
                    DefectoID = ddlDefecto.SelectedValue.SafeIntParse()
                };

                DefectosCuadrante.Add(defecto);

                repDefectos.DataSource = DefectosCuadrante;
                repDefectos.DataBind();
                repDefectos.Visible = true;

                txtCuadrante.Text = string.Empty;
                txtPlaca.Text = string.Empty;
                ddlDefecto.SelectedIndex = -1;
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void btnDefectoSector_OnClick(object sender, EventArgs e)
        {
            try
            {
                JuntaCampoBO.Instance.ValidarDefecto(ddlDefecto.SelectedValue.SafeIntParse());
                IDCounter++;
                JuntaCampoReportePNDSector defecto = new JuntaCampoReportePNDSector
                {
                    JuntaCampoReportePNDSectorID = IDCounter,
                    Sector = txtSector.Text,
                    SectorInicio = txtDe.Text,
                    SectorFin = txtA.Text,
                    DefectoID = ddlDefecto.SelectedValue.SafeIntParse()
                };

                DefectosSector.Add(defecto);

                repDefectoSector.DataSource = DefectosSector;
                repDefectoSector.DataBind();
                repDefectoSector.Visible = true;

                txtSector.Text = string.Empty;
                txtA.Text = string.Empty;
                txtDe.Text = string.Empty;
                ddlDefecto.SelectedIndex = -1;
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void btnGuardarPopUp_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {

                    ReporteCampoPND reporte = new ReporteCampoPND
                    {
                        ProyectoID = hdnProyectoID.Value.SafeIntParse(),
                        TipoPruebaID = TipoPruebaID,
                        NumeroReporte = txtNumeroReporte.Text,
                        FechaReporte = mdpFechaReporte.SelectedDate.Value
                    };

                    if (TipoPruebaID == 10 && ddlResultado.SelectedValue != "1")
                    {
                        JuntaCampoReportePNDCuadrante defecto = new JuntaCampoReportePNDCuadrante
                        {
                            JuntaCampoReportePNDCuadranteID = 1,
                            Cuadrante = "N/A",
                            Placa = "N/A",
                            DefectoID = ddlDefecto.SelectedValue.SafeIntParse()
                        };

                        DefectosCuadrante.Add(defecto);
                    }

                    JuntaCampo junta = JuntaCampoBO.Instance.ObtenerPorID(JuntaCampoID);

                    if (hdnCambiaFechas.Value == "1")
                    {
                        JuntaCampoBO.Instance.CambiaFechasRequisicion(ddlRequisicion.SelectedValue.SafeIntParse(), mdpFechaProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
                    }
                    else if (hdnCambiaFechas.Value == "2")
                    {
                        DateTime fechaReq = DateTime.ParseExact(hdnProcesoAnterior2.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        JuntaCampoBO.Instance.CambiaFechasRequisicion(ddlRequisicion.SelectedValue.SafeIntParse(), fechaReq, SessionFacade.UserId);
                        JuntaCampoBO.Instance.CambiaFechasIV(junta.JuntaCampoSoldaduraID.Value, mdpFechaProcesoAnterior.SelectedDate.Value, mdpFechaReporteProcesoAnterior.SelectedDate.Value, SessionFacade.UserId);
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

                    int? tipoDefecto = null;
                    if (ddlTipoDefecto.SelectedValue.SafeIntParse() > 0)
                    {
                        tipoDefecto = ddlTipoDefecto.SelectedValue.SafeIntParse();
                    }

                    bool aprobado = ddlResultado.SelectedValue == "0" ? false : true;

                    JuntaCampoReportePND juntaReporte = new JuntaCampoReportePND
                    {
                        TipoRechazoID = tipoDefecto,
                        FechaPrueba = mdpFechaPND.SelectedDate.Value,
                        Aprobado = aprobado,
                        Observaciones = txtObservaciones.Text
                    };

                    //si es aprobacion o 1er o 2do rechazo               
                    bool aprobadoORechazo = ReportesBL.Instance.GuardaReporteCampoPND(JuntaCampoID, ddlRequisicion.SelectedValue.SafeIntParse(), reporte, juntaReporte, DefectosSector, DefectosCuadrante, SessionFacade.UserId);

                    if (aprobado)
                    {
                        limpiarDatos();
                        cargaDatos();
                    }
                    else if (!aprobado && aprobadoORechazo)
                    {
                        phPrincipal.Visible = false;
                        phMensaje.Visible = true;
                        litMensajeNuevaJunta.Visible = true;
                    }
                    else if (!aprobado && !aprobadoORechazo)
                    {
                        phPrincipal.Visible = false;
                        phMensaje.Visible = true;
                        litMensajeNuevoCorte.Visible = true;
                        //Mensaje sobre la necesidad de generar una junta de corte
                    }

                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void limpiarDatos()
        {
            ddlRequisicion.SelectedIndex = -1;
            mdpFechaPND.SelectedDate = null;
            mdpFechaReporte.SelectedDate = null;
            txtNumeroReporte.Text = string.Empty;
            ddlResultado.SelectedIndex = -1;
            pnlDefecto.Visible = false;
        }

        protected void repPruebasPND_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int JuntaCampoReportePNDID = e.CommandArgument.SafeIntParse();

                try
                {
                    JuntaCampoBO.Instance.EliminaReporteCampoPnd(JuntaCampoReportePNDID);
                    cargaDatos();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }


        protected void repPruebasPND_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void repPruebasHistoricasRechazadas_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int jcRepPndId = e.CommandArgument.SafeIntParse();

                try
                {
                    JuntaCampoBO.Instance.EliminaReporteDeRechazoHistorico(jcRepPndId, SessionFacade.UserId);
                    cargaDatos();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }
    }
}
