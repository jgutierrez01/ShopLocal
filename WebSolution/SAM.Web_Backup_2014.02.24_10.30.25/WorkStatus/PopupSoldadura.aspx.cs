using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities.Grid;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Classes;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Validations;
using Mimo.Framework.WebControls;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessLogic.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class PopupSoldadura : SamPaginaPopup
    {
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
                    mdpFechaReporteArmado.SelectedDate = Convert.ToDateTime(FechaArmado);
                    hfFechaArmado.Text = FechaArmado;
                    hdnCambiaFechas.Value = "0";
                }
            }
            else 
            {
                cvFechas.Visible = false;
                hdnCambiaFechas.Value = "0";
            }

            JuntaSoldadura js = jwks != null ? SoldaduraBO.Instance.ObtenerInformacionSoldadura(jwks.JuntaWorkstatusID) : null;

            if (js != null)
            {
                ctrlInfo.CargaInformacion(jwks, js, readOnly);
                ctrlRaiz.CargaInformacion(jwks, js, readOnly);
                ctrlRell.CargaInformacion(jwks, js, readOnly);
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

        protected void procesoSeleccionado(object sender, EventArgs e)
        {

            ctrlInfo.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, EspesorJunta);
            ctrlRaiz.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, !ctrlInfo.WpsIguales);

            if (ctrlInfo.WpsIguales)
            {
                ctrlRell.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, !ctrlInfo.WpsIguales);
                ctrlInfo.cargaWpsDefault();
            }
        }

        protected void procesoRellenoSeleccionado(object sender, EventArgs e)
        {
            ctrlInfo.CargaWPSRelleno(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, EspesorJunta);
            ctrlRell.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, !ctrlInfo.WpsIguales);

            if (ctrlInfo.WpsIguales)
            {
                ctrlRaiz.CargaWPS(EntityID.Value, ctrlRaiz.ProcesoRaiz, ctrlRell.ProcesoRelleno, ctrlInfo.material1, ctrlInfo.material2, EspesorJunta, !ctrlInfo.WpsIguales);
                ctrlInfo.cargaWpsDefault();
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
            foreach (ListItem item in ddlWps.Items)
            {
                ctrlInfo.ddlWps.Items.Add(item);
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


            else if (ctrlRaiz.wpsItem != wpsItem)
            {
                ctrlRaiz.wpsItem = wpsItem;
            }
        }

        protected void btnGuardarPopUp_OnClick(object sender, EventArgs e)
        {
            try
            {
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


                    // validamos que la fecha de soldadura sea mayor a la de armado
                    if (FechaProcArmado != string.Empty)
                    {
                        ValidaFechasBO.Instance.ValidaFechasArmado(js.FechaSoldadura, Convert.ToDateTime(FechaProcArmado).Date);
                    }

                    if (hdnCambiaFechas.Value == "1")
                    {
                        ArmadoBO.Instance.CambiaFechaReporteArmado(jws.JuntaArmadoID.Value, mdpFechaArmado.SelectedDate.Value, mdpFechaReporteArmado.SelectedDate.Value, SessionFacade.UserId);
                    }

                    js.ProcesoRaizID = ctrlRaiz.ProcesoRaiz != -1 ? ctrlRaiz.ProcesoRaiz : (int?)null;
                    js.ProcesoRellenoID = ctrlRell.ProcesoRelleno != -1 ? ctrlRell.ProcesoRelleno : (int?)null;


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
    }
}