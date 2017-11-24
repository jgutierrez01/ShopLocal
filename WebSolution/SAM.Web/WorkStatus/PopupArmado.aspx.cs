using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.WebControls;
using SAM.Entities;
using SAM.BusinessObjects.Workstatus;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Catalogos;
using SAM.Web.Common;
using System.Web.Script.Serialization;
using SAM.BusinessObjects.Produccion;
using System.Globalization;

namespace SAM.Web.WorkStatus
{
    public partial class PopupArmado : SamPaginaPopup
    {
    #region ViewStates
        private string Fecha
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

        private bool EdicionEspecial
        {
            get
            {
                return ViewState["EdicionEspecialArmado"].SafeBoolParse();
            }
            set 
            {
                ViewState["EdicionEspecialArmado"] = value;
            }
        }

        private int? JuntaSpoolIDEdicion
        {
            get
            {
                return ViewState["JuntaSpoolIDEdicion"].SafeIntNullableParse();
            }
            set
            {
                ViewState["JuntaSpoolIDEdicion"] = value;
            }
        }

        private int? JuntaArmadoIDEdicion
        {
            get
            {
                return ViewState["JuntaArmadoIDEdicion"].SafeIntNullableParse();
            }
            set
            {
                ViewState["JuntaArmadoIDEdicion"] = value;
            }
        }

        private int JuntaWsId
        {
            get
            {
                if (ViewState["JuntaWsId"] == null)
                {
                    ViewState["JuntaWsId"] = 0;
                }
                return ViewState["JuntaWsId"].SafeIntParse();
            }
            set
            {
                ViewState["JuntaWsId"] = value;
            }
        }

        private JuntaArmado Armado
        {
            get
            {
                return (JuntaArmado)ViewState["ArmadoEdicion"];
            }
            set
            {
                ViewState["ArmadoEdicion"] = value;
            }
        }

        private OrdenTrabajoJunta odtj
        {
            get
            {
                return (OrdenTrabajoJunta)ViewState["ODTJunta"];
            }
            set
            {
                ViewState["ODTJunta"] = value;
            }
        }
        private bool ReadOnly
        {
            get
            {
                return ViewState["ReadOnly"].SafeBoolParse();
            }
            set
            {
                ViewState["ReadOnly"] = value;
            }
        }

    #endregion

        /// <summary>
        /// carga la información de la junta seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAJuntaSpool(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando armar una junta spool {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                ReadOnly = Request.QueryString["RO"].SafeBoolParse();
                //--------------EdicionEspecial
                EdicionEspecial = Request.QueryString["EDES"].SafeBoolParse();
                JuntaArmadoIDEdicion = Request.QueryString["IDJA"].SafeIntNullableParse();
                JuntaSpoolIDEdicion = EntityID.Value;
                //--------------
                hdnJuntaSpoolID.Value = EntityID.Value.SafeStringParse();

                Fecha = ValidaFechasBO.Instance.ObtenerFechaReporteSoldadura(EntityID.Value);
                JavaScriptSerializer js = new JavaScriptSerializer();

                string jLink = string.Format("return Sam.Workstatus.ValidaFechaReporteArmado({0}, {1}, {2});", js.Serialize(Fecha), mdpFechaArmado.ClientID, mdpFechaReporte.ClientID);


                if (EdicionEspecial)
                {
                    btnArmar.Visible = false;
                    btnGuardarEdicion.Visible = true;
                    btnGuardarEdicion.OnClientClick = jLink;
                    ReadOnly = false;
                    cargaInformacion(ReadOnly);
                }
                else
                {
                    btnArmar.OnClientClick = jLink;
                    cargaInformacion(ReadOnly);
                }
            }
        }

        /// <summary>
        /// carga la informacion en los controles del popup, recibe la información
        /// a través de query string. Hace una nueva consulta a base de datos para recuperar
        /// la información necesaria para el despliegue.
        /// </summary>
        private void cargaInformacion(bool readOnly)
        {
            #region informacion de textos
             odtj = ArmadoBO.Instance.ObtenerInformacionParaArmado(EntityID.Value);

            NumControl.Text = odtj.OrdenTrabajoSpool.NumeroControl;

            List<TipoJuntaCache> tj = CacheCatalogos.Instance.ObtenerTiposJunta();
            Junta.Text = odtj.JuntaSpool.Etiqueta; /**tj.Single(x => x.ID == odtJ.JuntaSpool.TipoJuntaID).NombreJunta;**/
            Localizacion.Text = String.Format("{0} - {1}", odtj.JuntaSpool.EtiquetaMaterial1, odtj.JuntaSpool.EtiquetaMaterial2);
            Tipo.Text = tj.Single(x => x.ID == odtj.JuntaSpool.TipoJuntaID).Nombre;

            Cedula.Text = odtj.JuntaSpool.Cedula;
            NombreSpool.Text = odtj.JuntaSpool.Spool.Nombre;

            List<FamAceroCache> tm = CacheCatalogos.Instance.ObtenerFamiliasAcero();

            Material1.Text = tm.Single(x => x.ID == odtj.JuntaSpool.FamiliaAceroMaterial1ID).Nombre
                + (odtj.JuntaSpool.FamiliaAceroMaterial2ID == null ? string.Empty : "/" + tm.Single(x => x.ID == odtj.JuntaSpool.FamiliaAceroMaterial2ID).Nombre);
            hdnProyectoID.Value = odtj.JuntaSpool.Spool.ProyectoID.ToString();

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                mdpFechaArmado.SelectedDate = DateTime.Now.AddDays(-2);
                mdpFechaReporte.SelectedDate = DateTime.Now.AddDays(-2);
            }
            else
            {
                mdpFechaArmado.SelectedDate = DateTime.Now.AddDays(-1);
                mdpFechaReporte.SelectedDate = DateTime.Now.AddDays(-1);
            }
            

            #endregion

            #region carga combos
            //combos de numero unico
            JuntaWorkstatus jws;
            jws = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(EntityID.Value);
            if (jws != null)
            {
                Armado = ArmadoBO.Instance.ObtenerInformacionArmado(jws.JuntaWorkstatusID);
                if (Armado == null)
                {
                    Armado = new JuntaArmado();
                }
            }
            else
            {
                Armado = new JuntaArmado();
            }
            int spoolID = odtj.JuntaSpool.SpoolID;
            int odtsID = odtj.OrdenTrabajoSpool.OrdenTrabajoSpoolID;
            string etiquetaMaterial = odtj.JuntaSpool.EtiquetaMaterial1;

            ddlNumUnico1.BindToEnumerableWithEmptyRow(
                ArmadoBO.Instance.ObtenerNumeroUnicoPorEtiquetaMaterial(etiquetaMaterial, odtsID, spoolID, Armado.NumeroUnico1ID.SafeIntParse())
                , "Valor", "ID", -1);

            etiquetaMaterial = odtj.JuntaSpool.EtiquetaMaterial2;

            ddlNumUnico2.BindToEnumerableWithEmptyRow(
                ArmadoBO.Instance.ObtenerNumeroUnicoPorEtiquetaMaterial(etiquetaMaterial, odtsID, spoolID, Armado.NumeroUnico2ID.SafeIntParse())
                , "Valor", "ID", -1);

            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorProyecto(hdnProyectoID.Value.SafeIntParse()));

            #endregion

            
            //JuntaArmado armado;

            if (EdicionEspecial)
            {
                //jws = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(EntityID.Value);
                JuntaWsId = jws.JuntaWorkstatusID;
                //Armado = ArmadoBO.Instance.ObtenerInformacionArmado(jws.JuntaWorkstatusID);
                ddlNumUnico1.SelectedValue = Armado.NumeroUnico1ID.HasValue ? Armado.NumeroUnico1ID.ToString() : String.Empty;
                ddlNumUnico2.SelectedValue = Armado.NumeroUnico2ID.HasValue ? Armado.NumeroUnico2ID.ToString() : String.Empty;
                chbNumUnico1Pendiente.Checked = Armado.NumeroUnico1ID.HasValue ? false : true;
                chbNumUnico2Pendiente.Checked = Armado.NumeroUnico2ID.HasValue ? false : true;
                ddlTaller.SelectedValue = Armado.TallerID.ToString();
                rcbTubero.Text = Armado.Tubero.Codigo;
                txtObservaciones.Text = Armado.Observaciones;
                mdpFechaArmado.SelectedDate = Armado.FechaArmado;
                mdpFechaReporte.SelectedDate = Armado.FechaReporte;
            }

            if (readOnly)
            {
                //jws = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(EntityID.Value);
               // Armado = ArmadoBO.Instance.ObtenerInformacionArmado(jws.JuntaWorkstatusID);
                ddlNumUnico1.SelectedValue = Armado.NumeroUnico1ID.HasValue ? Armado.NumeroUnico1ID.ToString() : String.Empty;
                ddlNumUnico2.SelectedValue = Armado.NumeroUnico2ID.HasValue ? Armado.NumeroUnico2ID.ToString() : String.Empty;
                chbNumUnico1Pendiente.Checked = Armado.NumeroUnico1ID.HasValue ? false : true;
                chbNumUnico2Pendiente.Checked = Armado.NumeroUnico2ID.HasValue ? false : true;
                ddlTaller.SelectedValue = Armado.TallerID.ToString();
                rcbTubero.Text = Armado.Tubero.Codigo;
                txtObservaciones.Text = Armado.Observaciones;
                mdpFechaArmado.SelectedDate = Armado.FechaArmado;
                mdpFechaReporte.SelectedDate = Armado.FechaReporte;

                ddlNumUnico1.Enabled = false;
                ddlNumUnico2.Enabled = false;
                chbNumUnico1Pendiente.Enabled = false;
                chbNumUnico2Pendiente.Enabled = false;
                ddlTaller.Enabled = false;
                txtObservaciones.ReadOnly = true;
                rcbTubero.Enabled = false;
                mdpFechaReporte.Enabled = false;
                mdpFechaArmado.Enabled = false;

                btnArmar.Visible = false;
            }
        }

        protected void chbNumUnico1Pendiente_CheckedChanged(object sender, EventArgs e)
        {
            if (chbNumUnico1Pendiente.Checked)
            {
                rcbNumeroUnico.Visible = true;
                ddlNumUnico1.Visible = false;
            }
            else
            {
                rcbNumeroUnico.Visible = false;
                ddlNumUnico1.Visible = true;
                ddlNumUnico1.SelectedValue = Armado.NumeroUnico1ID.HasValue ? Armado.NumeroUnico1ID.ToString() : String.Empty;
                ddlNumUnico1.Enabled = ReadOnly == false ? true : false;

            }
        }

        protected void chbNumUnico2Pendiente_CheckedChanged(object sender, EventArgs e)
        {
            if (chbNumUnico2Pendiente.Checked)
            {
                ddlNumUnico2.Visible = false;
                rcbNumeroUnico2.Visible = true;
            }
            else
            {
                rcbNumeroUnico2.Visible = false;
                ddlNumUnico2.Visible = true;
                ddlNumUnico2.SelectedValue = Armado.NumeroUnico2ID.HasValue ? Armado.NumeroUnico2ID.ToString() : String.Empty;
                ddlNumUnico2.Enabled = ReadOnly == false ? true : false;
            } 
        }

        protected void ValidaCombos()
        {
            string errores = "";
            string salto = "<br />";

            //if (rcbNumeroUnico.Visible == true && (rcbNumeroUnico.SelectedValue == string.Empty || rcbNumeroUnico.SelectedValue.SafeIntParse() < 0))
            //{
            //    errores += (Cultura == "en-US" ? "The unique numbre 1 is required" : "El número único 1 es requerido") + salto;
            //}
            
            //if (rcbNumeroUnico2.Visible == true && (rcbNumeroUnico2.SelectedValue == string.Empty || rcbNumeroUnico2.SelectedValue.SafeIntParse() < 0))
            //{
            //    errores += (Cultura == "en-US" ? "The unique number 2 is required" : "El número único 2 es requerido") + salto;
            //}

            if (ddlNumUnico1.Visible == true && (ddlNumUnico1.SelectedValue == string.Empty || ddlNumUnico1.SelectedValue.SafeIntParse() < 0))
            {
                errores += (Cultura == "en-US" ? "The unique numbre 1 is required" : "El número único 1 es requerido") + salto;
            }
            
            if (ddlNumUnico2.Visible == true && (ddlNumUnico2.SelectedValue == string.Empty || ddlNumUnico2.SelectedValue.SafeIntParse() < 0))
            {
                errores += (Cultura == "en-US" ? "The unique number 2 is required" : "El número único 2 es requerido") + salto;
            }
            
            if (rcbTubero.SelectedValue == string.Empty || rcbTubero.SelectedValue.SafeIntParse() < 0)
            {
                errores += (Cultura == "en-US" ? "Fitter is required" : "El tubero es requerido") + salto;
            }
            
            if (ddlTaller.SelectedValue == string.Empty || ddlTaller.SelectedValue.SafeIntParse() < 0)
            {
                errores += (Cultura == "en-US" ? "The workshop is required" : "El taller es requerido") + salto;
            }

            if (errores != string.Empty)
            {
                throw new BaseValidationException(errores);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnArmar_OnClick(object sender, EventArgs e)
        {
            try
            {

                ValidaCombos();

                if (IsValid)
                {
                    JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(EntityID.Value);
                    TipoJunta tipoJunta = TipoJuntaBO.Instance.Obtener(juntaSpool.TipoJuntaID);

                    OrdenTrabajoJunta odtJ = ArmadoBO.Instance.ObtenerInformacionParaArmado(EntityID.Value);
                    JuntaWorkstatus jws = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(EntityID.Value);

                    // creamos la junta armado.
                    JuntaArmado ja = new JuntaArmado();
                    ja.NumeroUnico1ID = chbNumUnico1Pendiente.Checked == false ? ddlNumUnico1.SelectedValue.SafeIntNullableParse() : rcbNumeroUnico.SelectedValue.SafeIntNullableParse();
                    ja.NumeroUnico2ID = chbNumUnico2Pendiente.Checked == false ? ddlNumUnico2.SelectedValue.SafeIntNullableParse() : rcbNumeroUnico2.SelectedValue.SafeIntNullableParse();
                    ja.TallerID = ddlTaller.SelectedValue.SafeIntParse();
                    ja.TuberoID = rcbTubero.SelectedValue.SafeIntParse();
                    ja.FechaArmado = mdpFechaArmado.SelectedDate.Value;
                    ja.FechaReporte = mdpFechaReporte.SelectedDate.Value;
                    ja.Observaciones = txtObservaciones.Text;
                    ja.UsuarioModifica = SessionFacade.UserId;
                    ja.FechaModificacion = DateTime.Now;

                    //Junta Workstatus
                    if (jws == null)
                    {
                        jws = new JuntaWorkstatus();
                        jws.EtiquetaJunta = odtJ.JuntaSpool.Etiqueta;
                        jws.JuntaSpoolID = odtJ.JuntaSpoolID;
                        jws.OrdenTrabajoSpoolID = odtJ.OrdenTrabajoSpoolID;
                        jws.ArmadoAprobado = true;
                        jws.SoldaduraAprobada = false;
                        jws.InspeccionVisualAprobada = false;
                        jws.VersionJunta = 1;
                        jws.JuntaFinal = true;
                        jws.UltimoProcesoID = (int)UltimoProcesoEnum.Armado;
                        jws.UsuarioModifica = SessionFacade.UserId;
                        jws.FechaModificacion = DateTime.Now;
                    }
                    else
                    {
                        JuntaSoldadura js = SoldaduraBO.Instance.ObtenerInformacionSoldadura(jws.JuntaWorkstatusID);

                        //if (js != null)
                        //{
                        //    //ValidaFechasBO.Instance.ValidaFechasArmado(js.FechaSoldadura.Date, ja.FechaArmado.Date);                        
                        //}

                        jws.StartTracking();
                        jws.ArmadoAprobado = true;
                        jws.UltimoProcesoID = (int)UltimoProcesoEnum.Armado;
                        jws.UsuarioModifica = SessionFacade.UserId;
                        jws.FechaModificacion = DateTime.Now;
                    }

                    // Si la junta no es de reparación validamos que la fecha de armado sea menor a la de liberación dimensional si es que lo tiene
                    WorkstatusSpool ws = WorkstatusSpoolBO.Instance.ObtenerPorJuntaSpool(EntityID.Value);
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

                        DateTime fechaArmado = new DateTime();
                        if (CultureInfo.CurrentCulture.Name == "en-US")
                        {
                            DateTime.TryParse(ja.FechaArmado.ToString("MM/dd/yyyy"), out fechaArmado);
                        }
                        else
                        {
                            DateTime.TryParse(ja.FechaArmado.ToString("dd/MM/yyyy"), out fechaArmado);
                        }

                        ValidaFechasBO.Instance.ValidaFechasLiberacion(fechaArmado, tempFechaLiberacion);
                    }
                
                    ArmadoBO.Instance.GuardaJuntaWorkstatus(jws, ja);

                    // soldamos junta si la misma es de tipo TW
                    if (tipoJunta.Codigo == TipoJuntas.TW && jws.JuntaSoldaduraID == null)
                    {
                        JuntaWorkstatus juntaWorkstatus = SoldadorProcesoBO.Instance.ObtenerWorkStatus(EntityID.Value);
                        JuntaSoldadura js = new JuntaSoldadura();
                        js.FechaSoldadura = mdpFechaArmado.SelectedDate.Value;
                        js.FechaReporte = mdpFechaReporte.SelectedDate.Value;
                        js.TallerID = ddlTaller.SelectedValue.SafeIntParse();
                        js.UsuarioModifica = SessionFacade.UserId;
                        js.FechaModificacion = DateTime.Now;

                        juntaWorkstatus.StartTracking();
                        juntaWorkstatus.SoldaduraAprobada = true;

                        SoldaduraBO.Instance.GuardaJuntaWorkstatus(juntaWorkstatus, js, new List<int>());
                    }

                    JsUtils.RegistraScriptActualizaGridGenerico(this);
                }

            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        protected void cusRcbTubero_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = rcbTubero.SelectedValue.SafeIntParse() > 0;
        }

        protected void btnGuardarEdicion_Click(object sender, EventArgs e)
        {
            JuntaArmado juntaArmado = new JuntaArmado();

            juntaArmado.JuntaArmadoID = JuntaArmadoIDEdicion.SafeIntParse();
            juntaArmado.JuntaWorkstatusID = JuntaWsId;
            juntaArmado.NumeroUnico1ID = chbNumUnico1Pendiente.Checked == false ? ddlNumUnico1.SelectedValue.SafeIntNullableParse() : rcbNumeroUnico.SelectedValue.SafeIntNullableParse();
            juntaArmado.NumeroUnico2ID = chbNumUnico2Pendiente.Checked == false ? ddlNumUnico2.SelectedValue.SafeIntNullableParse() : rcbNumeroUnico2.SelectedValue.SafeIntNullableParse();
            juntaArmado.TallerID = ddlTaller.SelectedValue.SafeIntParse();
            juntaArmado.TuberoID = rcbTubero.SelectedValue.SafeIntParse() > 0 ? rcbTubero.SelectedValue.SafeIntParse() : Armado.TuberoID;
            juntaArmado.FechaArmado = mdpFechaArmado.SelectedDate.Value;
            juntaArmado.FechaReporte = mdpFechaReporte.SelectedDate.Value;
            juntaArmado.Observaciones = txtObservaciones.Text;
            juntaArmado.UsuarioModifica = SessionFacade.UserId;
            juntaArmado.FechaModificacion = DateTime.Now;

            ArmadoBO.Instance.GuardarEdicionEspecialArmado(juntaArmado);

            JsUtils.RegistraScriptActualizaGridGenerico(this);
        }

        protected void cusCombo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = rcbNumeroUnico2.SelectedValue != string.Empty || rcbNumeroUnico.SelectedValue != null ? true : false;
        }

        protected void ValidaNumUnico2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = rcbNumeroUnico2.SelectedValue != string.Empty || rcbNumeroUnico2.SelectedValue != null ? true : false;
        }

        
    }
}
