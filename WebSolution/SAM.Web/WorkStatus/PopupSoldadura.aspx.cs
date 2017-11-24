using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Mimo.Framework.WebControls;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using SAM.BusinessObjects.Produccion;
using System.Globalization;

namespace SAM.Web.WorkStatus
{
    public partial class PopupSoldadura : SamPaginaPopup
    {
        #region variables globales
        public decimal? EspesorJunta
        {
            get
            {
                if (ViewState["EspesorJunta"] != null)
                {
                    return ViewState["EspesorJunta"].SafeDecimalParse();
                }
                return 0;
            }
            set
            {
                ViewState["EspesorJunta"] = value;
            }
        }

        private string FechaArmado
        {
            get
            {
                if (ViewState["FechasArmado"] == null)
                {
                    ViewState["FechasArmado"] = string.Empty;
                }

                return ViewState["FechasArmado"].ToString();
            }
            set
            {
                ViewState["FechasArmado"] = value;
            }
        }

        private string FechaProcArmado
        {
            get
            {
                if (ViewState["FechaProcArmado"] == null)
                {
                    ViewState["FechaProcArmado"] = string.Empty;
                }

                return ViewState["FechaProcArmado"].ToString();
            }
            set
            {
                ViewState["FechaProcArmado"] = value;
            }
        }

        private string FechaIV
        {
            get
            {
                if (ViewState["FechaIV"] == null)
                {
                    ViewState["FechaIV"] = string.Empty;
                }

                return ViewState["FechaIV"].ToString();
            }
            set
            {
                ViewState["FechaIV"] = value;
            }
        }

        public List<GrdSoldadorProceso> SoldadoresRaiz
        {
            get
            {
                if (ViewState["SoldadoresRaiz"] == null)
                {
                    ViewState["SoldadoresRaiz"] = new List<GrdSoldadorProceso>();
                }
                return (List<GrdSoldadorProceso>)ViewState["SoldadoresRaiz"];
            }
            set
            {
                ViewState["SoldadoresRaiz"] = value;
            }
        }

        private bool EdicionEspecial
        {
            get
            {
                return ViewState["EdicionEspecialSoldadora"].SafeBoolParse();
            }
            set
            {
                ViewState["EdicionEspecialSoldadora"] = value;
            }
        }

        private int JuntaSoldaduraId
        {
            get
            {
                if (ViewState["juntaSoldaduraIdEdicion"] == null)
                {
                    ViewState["juntaSoldaduraIdEdicion"] = 0;
                }
                return ViewState["juntaSoldaduraIdEdicion"].SafeIntParse();
            }
            set
            {
                ViewState["juntaSoldaduraIdEdicion"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAJuntaSpool(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando soldar una junta spool {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                EdicionEspecial = Request.QueryString["EDES"].SafeBoolParse();
                if (EdicionEspecial)
                {
                    divGuardar.Visible = false;
                    divBotonEdicionEspecial.Visible = true;
                }

                cargaInformacion(Request.QueryString["RO"].SafeBoolParse());
            }
        }

        private void cargaInformacion(bool readOnly)
        {
            OrdenTrabajoJunta otj = ctrlInfo.CargaInformacion(EntityID.Value, readOnly);
            EspesorJunta = otj.JuntaSpool.Espesor;
            Proyecto proyecto = ProyectoBO.Instance.Obtener(otj.JuntaSpool.Spool.ProyectoID);
            ctrlRaiz.CargaInformacion(proyecto.PatioID, proyecto.ProyectoID);
            ctrlRell.CargaInformacion(proyecto.PatioID, proyecto.ProyectoID);
            JuntaWorkstatus jwks = SoldaduraBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(EntityID.Value);

            if (jwks != null)
            {
                JuntaSoldaduraId = jwks.JuntaSoldaduraID.SafeIntParse();
                FechaIV = ValidaFechasBO.Instance.ObtenerFechaIV(jwks.JuntaWorkstatusID);
                hfFechaIV.Text = FechaIV;

                if (FechaIV == string.Empty)
                {
                    cvFechaIV.Visible = false;
                }
            }

            if (jwks != null && jwks.JuntaArmadoID != null)
            {
                FechaArmado = ValidaFechasBO.Instance.ObtenerFechaReporteArmado(jwks.JuntaWorkstatusID);
                FechaProcArmado = ValidaFechasBO.Instance.ObtenerFechaProcesoArmado(jwks.JuntaWorkstatusID);

                if (FechaArmado != string.Empty)
                {
                    mdpFechaReporteSoldadura.SelectedDate = Convert.ToDateTime(FechaArmado);
                    hfFechaArmado.Text = FechaArmado;
                    hdnCambiaFechas.Value = "0";
                }
            }
            else
            {
                //cvFechas.Visible = false;
                hdnCambiaFechas.Value = "0";
            }

            JuntaSoldadura js = jwks != null ? SoldaduraBO.Instance.ObtenerInformacionSoldadura(jwks.JuntaWorkstatusID) : null;

            if (js != null)
            {
                ctrlInfo.CargaInformacion(jwks, js, readOnly, EdicionEspecial);
                ctrlRaiz.CargaInformacion(jwks, js, readOnly, ctrlInfo.wpsItem, EdicionEspecial);
                ctrlRell.CargaInformacion(jwks, js, readOnly, ctrlInfo.wpsRellenoItem, EdicionEspecial);
            }

            if (readOnly)
            {
                divGuardar.Visible = false;
            }
            else
            {
                if (EspesorJunta.GetValueOrDefault(0) == 0)
                {
                    divGuardar.Visible = false;
                    pnlEspesor.Visible = true;
                }
            }
        }

        protected void EnviarSoldadores(Object sender, EventArgs e)
        {
            ctrlRell.SoldadoresList = ctrlRaiz.SoldadoresList;
            ctrlRell.SoldarConRaiz = ctrlInfo.TerminadoConRaiz;
            ctrlRell.AgregaSoldadorRaiz();
        }

        protected void procesoSeleccionado(object sender, EventArgs e)
        {
            if (ctrlInfo.TerminadoConRaiz)
            {
                ctrlInfo.CargaWPSTerminacionRaiz(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, EspesorJunta, ctrlRaiz.ProcesoRaizSelectedItem);
                ctrlRaiz.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta);
                ctrlRell.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, ctrlRaiz.ProcesoRaizSelectedItem);
                ctrlRell.ProcesoRellenoSelectedItem = ctrlRaiz.ProcesoRaizSelectedItem;
                ctrlInfo.CargaWPSRellenoTerminacion(EntityID.Value, ctrlRaiz.ProcesoRaiz, EspesorJunta, ctrlRaiz.ProcesoRaizSelectedItem);
                ctrlInfo.cargaWpsDefault();
                ctrlRell.ProcesorellenoEnabled = false;
                ctrlRell.WpsRellenoEnabled = false;
                ctrlRell.btnAgregarEnabled = false;
                ctrlRell.ListadoSoldadoresEnabled = false;
                ctrlRell.ColadaEnabled = false;
                ctrlRaiz.SoldarConRaiz = ctrlInfo.TerminadoConRaiz;
            }
            else
            {
                ctrlInfo.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, EspesorJunta);
                ctrlRaiz.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, !ctrlInfo.WpsIguales);

                if (ctrlInfo.WpsIguales)
                {
                    ctrlRell.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, !ctrlInfo.WpsIguales);
                    ctrlInfo.cargaWpsDefault();
                }
            }
        }

        protected void procesoRellenoSeleccionado(object sender, EventArgs e)
        {
            if (ctrlInfo.TerminadoConRaiz == false)
            {
                ctrlInfo.CargaWPSRelleno(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, EspesorJunta);
                ctrlRell.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, !ctrlInfo.WpsIguales);

                if (ctrlInfo.WpsIguales)
                {
                    ctrlRaiz.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, !ctrlInfo.WpsIguales);
                    ctrlInfo.cargaWpsDefault();
                }
            }
        }

        protected void wpsRellenoSeleccionado(object sender, EventArgs e)
        {
            MappableDropDown ddlWpsRelleno = (MappableDropDown)sender;
            Label lblNoPreheat = (Label)ctrlInfo.FindControl("lblNoPreheat");
            Label lblSiPreheat = (Label)ctrlInfo.FindControl("lblSIPreheat");
            Label lblNoPwht = (Label)ctrlInfo.FindControl("lblNoPwht");
            Label lblSiPwht = (Label)ctrlInfo.FindControl("lblSiPwht");

            string wpsItem = ddlWpsRelleno.SelectedValue;

            if (ddlWpsRelleno.SelectedValue.SafeIntParse() < 0)
            {
                lblNoPreheat.Visible = false;
                lblSiPreheat.Visible = false;
                lblNoPwht.Visible = false;
                lblSiPwht.Visible = false;
            }
            else
            {
                WpsCache wps = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == ddlWpsRelleno.SelectedValue.SafeIntParse()).SingleOrDefault();

                if (wps != null)
                {
                    //chkPreheat.Checked = wps.RequierePreheat;
                    //chkPWHT.Checked = wps.RequierePwht;

                    if (wps.RequierePreheat)
                    {
                        lblNoPreheat.Visible = false;
                        lblSiPreheat.Visible = true;
                    }
                    else
                    {
                        lblNoPreheat.Visible = true;
                        lblSiPreheat.Visible = false;
                    }

                    if (wps.RequierePwht)
                    {
                        lblNoPwht.Visible = false;
                        lblSiPwht.Visible = true;
                    }
                    else
                    {
                        lblNoPwht.Visible = true;
                        lblSiPwht.Visible = false;
                    }
                }
            }

            if (ctrlInfo.WpsIguales)
            {
                if (ctrlRaiz.wpsItem != wpsItem)
                {
                    ctrlRaiz.wpsItem = wpsItem;
                }

                if (ctrlInfo.wpsItem != wpsItem)
                {
                    ctrlInfo.wpsItem = wpsItem;
                }

                if (ctrlInfo.wpsRellenoItem != wpsItem)
                {
                    ctrlInfo.wpsRellenoItem = wpsItem;
                }
            }

            else
            {
                if (ctrlInfo.wpsRellenoItem != wpsItem)
                {
                    ctrlInfo.wpsRellenoItem = wpsItem;
                }
            }


        }

        protected void wpsRellenoInfoSeleccionado(object sender, EventArgs e)
        {
            MappableDropDown ddlWpsRelleno = (MappableDropDown)sender;
            Label lblNoPreheat = (Label)ctrlInfo.FindControl("lblNoPreheat");
            Label lblSiPreheat = (Label)ctrlInfo.FindControl("lblSIPreheat");
            Label lblNoPwht = (Label)ctrlInfo.FindControl("lblNoPwht");
            Label lblSiPwht = (Label)ctrlInfo.FindControl("lblSiPwht");

            //ctrlInfo.ddlWpsRelleno.Items.Clear();
            //foreach (ListItem item in ddlWpsRelleno.Items)
            //{
            //    ctrlInfo.ddlWpsRelleno.Items.Add(item);
            //};

            string wpsItem = ddlWpsRelleno.SelectedValue;

            if (ddlWpsRelleno.SelectedValue.SafeIntParse() < 0)
            {
                lblNoPreheat.Visible = false;
                lblSiPreheat.Visible = false;
                lblNoPwht.Visible = false;
                lblSiPwht.Visible = false;
            }
            else
            {
                WpsCache wps = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == ddlWpsRelleno.SelectedValue.SafeIntParse()).SingleOrDefault();

                if (wps != null)
                {
                    //chkPreheat.Checked = wps.RequierePreheat;
                    //chkPWHT.Checked = wps.RequierePwht;

                    if (wps.RequierePreheat)
                    {
                        lblNoPreheat.Visible = false;
                        lblSiPreheat.Visible = true;
                    }
                    else
                    {
                        lblNoPreheat.Visible = true;
                        lblSiPreheat.Visible = false;
                    }

                    if (wps.RequierePwht)
                    {
                        lblNoPwht.Visible = false;
                        lblSiPwht.Visible = true;
                    }
                    else
                    {
                        lblNoPwht.Visible = true;
                        lblSiPwht.Visible = false;
                    }
                }
            }

            if (ctrlInfo.WpsIguales)
            {
                if (ctrlInfo.wpsItem != wpsItem)
                {
                    ctrlInfo.wpsItem = wpsItem;
                }

                if (ctrlRaiz.wpsItem != wpsItem)
                {
                    ctrlRaiz.wpsItem = wpsItem;
                }

                if (ctrlRell.wpsItem != wpsItem)
                {
                    ctrlRell.wpsItem = wpsItem;
                }
            }
            else
            {
                if (ctrlRell.wpsItem != wpsItem)
                {
                    ctrlRell.wpsItem = wpsItem;
                }
            }
        }


        protected void WpsDiferenteCambio(object sender, EventArgs e)
        {
            //ctrlInfo.wpsRellenoItem = string.Empty;
            //ctrlInfo.wpsItem = string.Empty;
            //ctrlRaiz.wpsItem = string.Empty;
            //ctrlRell.wpsItem = string.Empty;

            ctrlRaiz.LimpiaCombos();
            ctrlRell.LimpiaCombos();

        }

        protected void wpsSeleccionado(object sender, EventArgs e)
        {
            MappableDropDown ddlWps = (MappableDropDown)sender;
            Label lblNoPreheat = (Label)ctrlInfo.FindControl("lblNoPreheat");
            Label lblSiPreheat = (Label)ctrlInfo.FindControl("lblSIPreheat");
            Label lblNoPwht = (Label)ctrlInfo.FindControl("lblNoPwht");
            Label lblSiPwht = (Label)ctrlInfo.FindControl("lblSiPwht");

            string wpsItem = ddlWps.SelectedValue;
            ctrlInfo.ddlWps.Items.Clear();
            //*******************************
            if (ctrlInfo.TerminadoConRaiz)
            {
                ctrlRell.WpsRellenoEnabled = false;
                ctrlInfo.ddlWpsRelleno.Items.Clear();
            }
            //*******************************
            foreach (ListItem item in ddlWps.Items)
            {
                ctrlInfo.ddlWps.Items.Add(item);
                if (ctrlInfo.TerminadoConRaiz)
                {
                    ctrlInfo.ddlWpsRelleno.Items.Add(item);
                }
            };

            if (ddlWps.SelectedValue.SafeIntParse() < 0)
            {
                lblNoPreheat.Visible = false;
                lblSiPreheat.Visible = false;
                lblNoPwht.Visible = false;
                lblSiPwht.Visible = false;
            }
            else
            {
                WpsCache wps = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == ddlWps.SelectedValue.SafeIntParse()).SingleOrDefault();

                if (wps != null)
                {
                    //chkPreheat.Checked = wps.RequierePreheat;
                    //chkPWHT.Checked = wps.RequierePwht;

                    if (wps.RequierePreheat)
                    {
                        lblNoPreheat.Visible = false;
                        lblSiPreheat.Visible = true;
                    }
                    else
                    {
                        lblNoPreheat.Visible = true;
                        lblSiPreheat.Visible = false;
                    }

                    if (wps.RequierePwht)
                    {
                        lblNoPwht.Visible = false;
                        lblSiPwht.Visible = true;
                    }
                    else
                    {
                        lblNoPwht.Visible = true;
                        lblSiPwht.Visible = false;
                    }
                }
            }



            if (ctrlInfo.WpsIguales)
            {
                if (ctrlInfo.wpsRellenoItem != wpsItem)
                {
                    ctrlInfo.wpsRellenoItem = wpsItem;
                }
                if (ctrlRell.wpsItem != wpsItem)
                {
                    ctrlRell.wpsItem = wpsItem;
                }
            }

            if (ctrlInfo.TerminadoConRaiz)
            {
                if (ctrlInfo.wpsRellenoItem != wpsItem)
                {
                    ctrlInfo.wpsRellenoItem = wpsItem;
                }
            }


            if (ctrlInfo.wpsItem != wpsItem)
            {
                ctrlInfo.wpsItem = wpsItem;
            }
        }

        protected void wpsInfoSeleccionado(object sender, EventArgs e)
        {
            MappableDropDown ddlWps = (MappableDropDown)sender;
            Label lblNoPreheat = (Label)ctrlInfo.FindControl("lblNoPreheat");
            Label lblSiPreheat = (Label)ctrlInfo.FindControl("lblSIPreheat");
            Label lblNoPwht = (Label)ctrlInfo.FindControl("lblNoPwht");
            Label lblSiPwht = (Label)ctrlInfo.FindControl("lblSiPwht");

            string wpsItem = ddlWps.SelectedValue;

            if (ddlWps.SelectedValue.SafeIntParse() < 0)
            {
                lblNoPreheat.Visible = false;
                lblSiPreheat.Visible = false;
                lblNoPwht.Visible = false;
                lblSiPwht.Visible = false;
            }
            else
            {
                WpsCache wps = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == ddlWps.SelectedValue.SafeIntParse()).SingleOrDefault();

                if (wps != null)
                {
                    //chkPreheat.Checked = wps.RequierePreheat;
                    //chkPWHT.Checked = wps.RequierePwht;

                    if (wps.RequierePreheat)
                    {
                        lblNoPreheat.Visible = false;
                        lblSiPreheat.Visible = true;
                    }
                    else
                    {
                        lblNoPreheat.Visible = true;
                        lblSiPreheat.Visible = false;
                    }

                    if (wps.RequierePwht)
                    {
                        lblNoPwht.Visible = false;
                        lblSiPwht.Visible = true;
                    }
                    else
                    {
                        lblNoPwht.Visible = true;
                        lblSiPwht.Visible = false;
                    }
                }
            }

            if (ctrlInfo.WpsIguales)
            {
                if (ctrlInfo.wpsRellenoItem != wpsItem)
                {
                    ctrlInfo.wpsRellenoItem = wpsItem;
                }

                if (ctrlRell.wpsItem != wpsItem)
                {
                    ctrlRell.wpsItem = wpsItem;
                }

                if (ctrlRaiz.wpsItem != wpsItem)
                {
                    ctrlRaiz.wpsItem = wpsItem;
                }
            }

            if (ctrlInfo.TerminadoConRaiz)
            {
                if (ctrlRell.wpsItem != wpsItem)
                {
                    ctrlRell.wpsItem = wpsItem;
                }
            }

            else if (ctrlRaiz.wpsItem != wpsItem)
            {
                ctrlRaiz.wpsItem = wpsItem;
            }
        }

        protected void btnGuardarPopUp_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ctrlInfo.FechaSoldadura.HasValue)
                {
                    ValidaFechasBO.Instance.ValidaAnioFecha(ctrlInfo.FechaSoldadura);
                }

                if (ctrlInfo.FechaSoldadura.HasValue)
                {
                    ValidaFechasBO.Instance.ValidaAnioFecha(ctrlInfo.FechaReporteSoldadura);
                }

                if (!ctrlInfo.TerminadoConRaiz)
                {
                    int tipoValidacion = 1;
                    ValidaSoldador(tipoValidacion);
                }
                else
                {
                    int tipoValidacion = 2;
                    ValidaSoldador(tipoValidacion);
                }

                if (IsValid)
                {
                    JuntaWorkstatus jws = SoldadorProcesoBO.Instance.ObtenerWorkStatus(EntityID.Value);

                    JuntaSoldadura js = null;
                    //Junta Workstatus
                    if (jws == null)
                    {
                        OrdenTrabajoJunta odtJ = ArmadoBO.Instance.ObtenerInformacionParaArmado(EntityID.Value);

                        jws = new JuntaWorkstatus();
                        jws.EtiquetaJunta = odtJ.JuntaSpool.Etiqueta;
                        jws.JuntaSpoolID = odtJ.JuntaSpoolID;
                        jws.OrdenTrabajoSpoolID = odtJ.OrdenTrabajoSpoolID;
                        jws.SoldaduraAprobada = false; //vamos a tomar como que el proceso no está completo hasta que se compruebe lo contrario
                        jws.VersionJunta = 1;
                        jws.JuntaFinal = true;
                        jws.UltimoProcesoID = (int)UltimoProcesoEnum.Soldado;
                        jws.UsuarioModifica = SessionFacade.UserId;
                        jws.FechaModificacion = DateTime.Now;

                        js = new JuntaSoldadura();
                    }
                    else
                    {
                        jws.StartTracking();
                        jws.SoldaduraAprobada = false; //vamos a tomar como que el proceso no está completo hasta que se compruebe lo contrario
                        jws.UltimoProcesoID = (int)UltimoProcesoEnum.Soldado;
                        jws.UsuarioModifica = SessionFacade.UserId;
                        jws.FechaModificacion = DateTime.Now;

                        if (jws.JuntaSoldaduraID != null)
                        {
                            js = SoldaduraBO.Instance.ObtenerJuntaSoldadura(jws);
                        }
                        else
                        {
                            js = new JuntaSoldadura();
                        }
                    }

                    List<int> EliminarDetalle = new List<int>();

                    js.StartTracking();
                    ctrlInfo.UnBindInformacion(js);
                    ctrlRell.UnBindInformacion(js.JuntaSoldaduraDetalle, EliminarDetalle);
                    ctrlRaiz.UnBindInformacion(js.JuntaSoldaduraDetalle, EliminarDetalle);

                    WorkstatusSpool ws = WorkstatusSpoolBO.Instance.ObtenerPorJuntaSpool(EntityID.Value);

                    DateTime fechaSoldadura = new DateTime();

                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        DateTime.TryParse(js.FechaSoldadura.ToString("MM/dd/yyyy"), out fechaSoldadura);
                    }
                    else
                    {
                        DateTime.TryParse(js.FechaSoldadura.ToString("dd/MM/yyyy"), out fechaSoldadura);
                    }

                    // Si la junta no es de reparación validamos que la fecha de soldadura sea menor a la de liberación dimensional si es que lo tiene             
                    if (jws.VersionJunta == 1 && ws != null && InspeccionDimensionalBO.Instance.TieneLiberacionDimensional(ws))
                    {
                        string fechaLiberacion = ValidaFechasBO.Instance.ObtenerFechaLiberacionDimensional(ws.WorkstatusSpoolID);

                        DateTime tempFechaLiberacion = new DateTime();
                        if (CultureInfo.CurrentCulture.Name != "en-US")
                        {
                            string[] splitFechaLiberacion = fechaLiberacion.Split('/');
                            string newFechaLiberacion = splitFechaLiberacion[1] + "/" + splitFechaLiberacion[0] + "/" + splitFechaLiberacion[2];

                            DateTime.TryParse(newFechaLiberacion, out tempFechaLiberacion);
                        }
                        else
                        {
                            DateTime.TryParse(fechaLiberacion, out tempFechaLiberacion);
                        }

                        ValidaFechasBO.Instance.ValidaFechasLiberacion(fechaSoldadura, tempFechaLiberacion);
                    }

                    if (jws.VersionJunta > 1)
                    {
                        ValidaFechasBO.Instance.ValidaFechaReportePND(fechaSoldadura, jws.JuntaWorkstatusAnteriorID.SafeIntParse());
                    }
                    else
                    {
                        //validacion cuando ya se tiene avances de pintura solo para las juntas que no son rechazos                    
                        SoldaduraBO.Instance.ValidaFechasSoldaduraFechaRequiPintura(js.FechaSoldadura, jws.OrdenTrabajoSpoolID);
                    }


                    // validamos que la fecha de soldadura sea mayor a la de armado
                    if (FechaProcArmado != string.Empty)
                    {
                        ValidaFechasBO.Instance.ValidaFechaSoldadura(js.FechaSoldadura, Convert.ToDateTime(FechaProcArmado).Date);
                    }

                    if (hdnCambiaFechas.Value == "1")
                    {
                        ArmadoBO.Instance.CambiaFechaReporteArmado(jws.JuntaArmadoID.Value, mdpFechaSoldadura.SelectedDate.Value, mdpFechaReporteSoldadura.SelectedDate.Value, SessionFacade.UserId);
                    }

                    js.ProcesoRaizID = ctrlRaiz.ProcesoRaiz != -1 ? ctrlRaiz.ProcesoRaiz : (int?)null;

                    if (ctrlInfo.TerminadoConRaiz)
                    {
                        js.ProcesoRellenoID = (int)Session["SoldadorProcesoRellenoID"] != -1 ? (int)Session["SoldadorProcesoRellenoID"] : (int?)null;
                    }
                    else
                    {
                        js.ProcesoRellenoID = ctrlRell.ProcesoRelleno != -1 ? ctrlRell.ProcesoRelleno : (int?)null;
                    }

                    ValidacionesSoldador.ValidaWpq(js.JuntaSoldaduraDetalle.Where(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Raiz), js.WpsID.HasValue ? js.WpsID.Value : (int?)null, js.FechaSoldadura);
                    ValidacionesSoldador.ValidaWpq(js.JuntaSoldaduraDetalle.Where(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Relleno), js.WpsRellenoID.HasValue ? js.WpsRellenoID.Value : (int?)null, js.FechaSoldadura);

                    js.StopTracking();

                    SoldaduraBO.Instance.GuardaJuntaWorkstatus(jws, js, EliminarDetalle);
                    JsUtils.RegistraScriptActualizaGridGenerico(this);
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "valGuardar");
            }
        }


        private void ValidaSoldador(int tipo)
        {
            bool soldadorRaiz = ctrlRaiz.validaGridSoldadores();
            bool soldadorRelleno = ctrlRell.validaGridSoldadores();
            bool procesoRelleno = ctrlRell.validaProcesoRelleno();
            List<string> errors = new List<string>();

            if (tipo == 1)
            {
                if (!procesoRelleno)
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        errors.Add(
                            String.Format("The filling process is required."));
                    }
                    else
                    {
                        errors.Add(
                            String.Format("El proceso relleno es requerido"));
                    }
                }

                if (!soldadorRaiz || !soldadorRelleno)
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        errors.Add(
                            String.Format("It is necessary that at least add a welder to the root process and  filling process."));
                    }
                    else
                    {
                        errors.Add(
                            String.Format("Es necesario que por lo menos se agregue un soldador para el proceso raíz y un soldador para el proceso relleno."));
                    }
                }
            }
            else
            {
                if (!soldadorRaiz)
                {
                    if (CultureInfo.CurrentCulture.Name == "en-US")
                    {
                        errors.Add(
                            String.Format("It is necessary that at least add a welder to the root process."));
                    }
                    else
                    {
                        errors.Add(
                            String.Format("Es necesario que por lo menos se agregue un soldador para el proceso raíz."));
                    }
                }
            }
            if (errors.Count > 0)
            {
                throw new BaseValidationException(errors);
            }
        }


        protected void ctrlInfo_LimpiarSoldaduraConRaiz(Object sender, EventArgs e)
        {
            ctrlRell.WpsRellenoEnabled = true;
            ctrlRell.ProcesorellenoEnabled = true;
            ctrlRell.ListadoSoldadoresEnabled = true;
            ctrlRell.ColadaEnabled = true;
            ctrlRell.LimpiaCombos();
            ctrlRaiz.LimpiaCombos();
            ctrlRaiz.LimpiarGridSoldadores();
            ctrlRell.LimpiarGridSoldadores();
        }

        protected void btnGuardarEdicionEspecial_Click(object sender, EventArgs e)
        {
            try
            {
                if (ctrlInfo.FechaSoldadura.HasValue)
                {
                    ValidaFechasBO.Instance.ValidaAnioFecha(ctrlInfo.FechaSoldadura);
                }

                if (ctrlInfo.FechaSoldadura.HasValue)
                {
                    ValidaFechasBO.Instance.ValidaAnioFecha(ctrlInfo.FechaReporteSoldadura);
                }

                if (!ctrlInfo.TerminadoConRaiz)
                {
                    int tipoValidacion = 1;
                    ValidaSoldador(tipoValidacion);
                }
                else
                {
                    int tipoValidacion = 2;
                    ValidaSoldador(tipoValidacion);
                }

                JuntaSoldadura juntaSoldadura = SoldaduraBO.Instance.ObtnerJuntaSoldaduraPorID(JuntaSoldaduraId);
                List<int> EliminarDetalle = new List<int>();

                juntaSoldadura.StartTracking();

                ctrlInfo.UnBindInformacion(juntaSoldadura);
                ctrlRaiz.UnBindInformacion(juntaSoldadura.JuntaSoldaduraDetalle, EliminarDetalle);
                ctrlRell.UnBindInformacion(juntaSoldadura.JuntaSoldaduraDetalle, EliminarDetalle);

                // validamos que la fecha de soldadura sea mayor a la de armado
                if (FechaProcArmado != string.Empty)
                {
                    ValidaFechasBO.Instance.ValidaFechasArmado(juntaSoldadura.FechaSoldadura, Convert.ToDateTime(FechaProcArmado).Date);
                }

                juntaSoldadura.ProcesoRaizID = ctrlRaiz.ProcesoRaiz != -1 ? ctrlRaiz.ProcesoRaiz : (int?)null;

                if (ctrlInfo.TerminadoConRaiz)
                {
                    juntaSoldadura.ProcesoRellenoID = (int)Session["SoldadorProcesoRellenoID"] != -1 ? (int)Session["SoldadorProcesoRellenoID"] : (int?)null;
                }
                else
                {
                    juntaSoldadura.ProcesoRellenoID = ctrlRell.ProcesoRelleno != -1 ? ctrlRell.ProcesoRelleno : (int?)null;
                }


                ValidacionesSoldador.ValidaWpq(juntaSoldadura.JuntaSoldaduraDetalle.Where(x => x.TecnicaSoldadorID ==
                    (int)TecnicaSoldadorEnum.Raiz), juntaSoldadura.WpsID.HasValue ? juntaSoldadura.WpsID.Value : (int?)null, juntaSoldadura.FechaSoldadura);
                ValidacionesSoldador.ValidaWpq(juntaSoldadura.JuntaSoldaduraDetalle.Where(x => x.TecnicaSoldadorID ==
                    (int)TecnicaSoldadorEnum.Relleno), juntaSoldadura.WpsRellenoID.HasValue ? juntaSoldadura.WpsRellenoID.Value : (int?)null, juntaSoldadura.FechaSoldadura);

                juntaSoldadura.StopTracking();

                SoldaduraBO.Instance.GuardarEdicionEspecialJuntaSoldadura(juntaSoldadura, EliminarDetalle, EntityID.Value);

                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "valGuardar");
            }

        }
    }
}