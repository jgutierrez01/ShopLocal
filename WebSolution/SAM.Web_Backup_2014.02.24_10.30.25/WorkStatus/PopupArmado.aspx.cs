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

namespace SAM.Web.WorkStatus
{
    public partial class PopupArmado : SamPaginaPopup
    {
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

                bool readOnly = Request.QueryString["RO"].SafeBoolParse();

                string fechaSoldadura = ValidaFechasBO.Instance.ObtenerFechaReporteSoldadura(EntityID.Value);

                btnArmar.OnClientClick = "return Sam.Workstatus.ValidaFechaReporteArmado('" + fechaSoldadura + "')";

                cargaInformacion(readOnly);
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
            OrdenTrabajoJunta odtJ = ArmadoBO.Instance.ObtenerInformacionParaArmado(EntityID.Value);

            NumControl.Text = odtJ.OrdenTrabajoSpool.NumeroControl;

            List<TipoJuntaCache> tj = CacheCatalogos.Instance.ObtenerTiposJunta();
            Junta.Text = odtJ.JuntaSpool.Etiqueta; /**tj.Single(x => x.ID == odtJ.JuntaSpool.TipoJuntaID).NombreJunta;**/
            Localizacion.Text = String.Format("{0} - {1}", odtJ.JuntaSpool.EtiquetaMaterial1, odtJ.JuntaSpool.EtiquetaMaterial2);
            Tipo.Text = tj.Single(x => x.ID == odtJ.JuntaSpool.TipoJuntaID).Nombre;

            Cedula.Text = odtJ.JuntaSpool.Cedula;
            NombreSpool.Text = odtJ.JuntaSpool.Spool.Nombre;

            List<FamAceroCache> tm = CacheCatalogos.Instance.ObtenerFamiliasAcero();

            Material1.Text = tm.Single(x => x.ID == odtJ.JuntaSpool.FamiliaAceroMaterial1ID).Nombre
                + (odtJ.JuntaSpool.FamiliaAceroMaterial2ID == null ? string.Empty : "/" + tm.Single(x => x.ID == odtJ.JuntaSpool.FamiliaAceroMaterial2ID).Nombre);
            hdnProyectoID.Value = odtJ.JuntaSpool.Spool.ProyectoID.ToString();

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
            int spoolID = odtJ.JuntaSpool.SpoolID;
            int odtsID = odtJ.OrdenTrabajoSpool.OrdenTrabajoSpoolID;
            string etiquetaMaterial = odtJ.JuntaSpool.EtiquetaMaterial1;

            ddlNumUnico1.BindToEnumerableWithEmptyRow(ArmadoBO.Instance.ObtenerNumeroUnicoPorEtiquetaMaterial(etiquetaMaterial, odtsID, spoolID), "Valor", "ID", -1);

            etiquetaMaterial = odtJ.JuntaSpool.EtiquetaMaterial2;

            ddlNumUnico2.BindToEnumerableWithEmptyRow(ArmadoBO.Instance.ObtenerNumeroUnicoPorEtiquetaMaterial(etiquetaMaterial, odtsID, spoolID), "Valor", "ID", -1);
            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorProyecto(hdnProyectoID.Value.SafeIntParse()));

            #endregion

            if (readOnly)
            {
                JuntaWorkstatus jws = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(EntityID.Value);

                JuntaArmado armado = ArmadoBO.Instance.ObtenerInformacionArmado(jws.JuntaWorkstatusID);
                ddlNumUnico1.SelectedValue = armado.NumeroUnico1ID.HasValue ? armado.NumeroUnico1ID.ToString() : String.Empty;
                ddlNumUnico2.SelectedValue = armado.NumeroUnico2ID.HasValue ? armado.NumeroUnico2ID.ToString() : String.Empty;
                chbNumUnico1Pendiente.Checked = armado.NumeroUnico1ID.HasValue ? false : true;
                chbNumUnico2Pendiente.Checked = armado.NumeroUnico2ID.HasValue ? false : true;
                ddlTaller.SelectedValue = armado.TallerID.ToString();
                rcbTubero.Text = armado.Tubero.Codigo;
                txtObservaciones.Text = armado.Observaciones;
                mdpFechaArmado.SelectedDate = armado.FechaArmado;
                mdpFechaReporte.SelectedDate = armado.FechaReporte;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chbNumUnicoPendiente_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox numUnicoPendiente = sender as CheckBox;

            if (numUnicoPendiente.ID == "chbNumUnico1Pendiente")
            {
                ddlNumUnico1.Enabled = !numUnicoPendiente.Checked;
                rvNumUnico1.Enabled = ddlNumUnico1.Enabled;

            }
            else if (numUnicoPendiente.ID == "chbNumUnico2Pendiente")
            {
                ddlNumUnico2.Enabled = !numUnicoPendiente.Checked;
                rvNumUnico2.Enabled = ddlNumUnico2.Enabled;
            }
            else
            {
                // Nada
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
                if (IsValid)
                {
                    JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(EntityID.Value);
                    TipoJunta tipoJunta = TipoJuntaBO.Instance.Obtener(juntaSpool.TipoJuntaID);

                    OrdenTrabajoJunta odtJ = ArmadoBO.Instance.ObtenerInformacionParaArmado(EntityID.Value);
                    JuntaWorkstatus jws = ArmadoBO.Instance.ObtenerJuntaWorkstatusPorJuntaSpoolID(EntityID.Value);

                    // creamos la junta armado.
                    JuntaArmado ja = new JuntaArmado();
                    ja.NumeroUnico1ID = ddlNumUnico1.Enabled ? ddlNumUnico1.SelectedValue.SafeIntNullableParse() : null;
                    ja.NumeroUnico2ID = ddlNumUnico2.Enabled ? ddlNumUnico2.SelectedValue.SafeIntNullableParse() : null;
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

                        if (js != null)
                        {
                            ValidaFechasBO.Instance.ValidaFechasArmado(js.FechaSoldadura.Date, ja.FechaArmado.Date);
                        }

                        jws.StartTracking();
                        jws.ArmadoAprobado = true;
                        jws.UltimoProcesoID = (int)UltimoProcesoEnum.Armado;
                        jws.UsuarioModifica = SessionFacade.UserId;
                        jws.FechaModificacion = DateTime.Now;
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
    }
}
