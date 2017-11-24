using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using Resources;
using SAM.BusinessObjects.Validations;
using SAM.Web.Common;

namespace SAM.Web.Produccion.JuntasCampo
{
    public partial class Armado : SamPaginaPopup
    {

        #region Variables privadas de la página

        private int OrdenTrabajoSpoolID
        {
            get
            {
                if (ViewState["OrdenTrabajoSpoolID"] == null)
                {
                    return -1;
                }

                return ViewState["OrdenTrabajoSpoolID"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajoSpoolID"] = value;
            }
        }

        private int JuntaSpoolID
        {
            get
            {
                if (ViewState["JuntaSpoolID"] == null)
                {
                    return -1;
                }

                return ViewState["JuntaSpoolID"].SafeIntParse();
            }
            set
            {
                ViewState["JuntaSpoolID"] = value;
            }
        }

        private int JuntaCampoArmadoID
        {
            get
            {
                if (ViewState["JuntaCampoArmadoID"] == null)
                {
                    return -1;
                }

                return ViewState["JuntaCampoArmadoID"].SafeIntParse();
            }
            set
            {
                ViewState["JuntaCampoArmadoID"] = value;
            }
        }

        private int JuntaCampoID
        {
            get
            {
                if (ViewState["JuntaCampoID"] == null)
                {
                    return -1;
                }

                return ViewState["JuntaCampoID"].SafeIntParse();
            }
            set
            {
                ViewState["JuntaCampoID"] = value;
            }
        }

        #endregion

        #region Eventos de la página

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JuntaSpoolID = Request.QueryString["JuntaSpoolID"].SafeIntParse();
                cargaDatos();
            }
        }

        #endregion

        #region Carga de información para la pantalla

        /// <summary>
        /// 
        /// </summary>
        private void cargaDatos()
        {
            int jcArmId = -1;
            int jcId = -1;
            string etiquetaProduccion = string.Empty;

            JuntaSpoolProduccion junta = JuntaSpoolBO.Instance.ObtenerConDatosDeProduccion(JuntaSpoolID);
            bool tieneArmado = JuntaCampoArmadoBO.Instance.JuntaSpoolTieneArmadoCampo(JuntaSpoolID, out jcArmId, out jcId, out etiquetaProduccion);

            JuntaCampoArmadoID = jcArmId;
            JuntaCampoID = jcId;

            if (!string.IsNullOrEmpty(etiquetaProduccion))
            {
                junta.Etiqueta = etiquetaProduccion;
            }

            limpiaCampos();
            bindDatosGenericos(junta);

            if (tieneArmado)
            {
                cargaDatosJuntaDeCampo();
                vistaReadOnly(true);
            }
            else
            {
                vistaReadOnly(false);
                cargaDatosParaCaptura(junta);
            }
        }

        private void limpiaCampos()
        {
            mdpFechaArmado.Clear();
            mdpFechaReporte.Clear();
            ddlEtiquetaMaterial.Items.Clear();
            ddlNumeroUnico1.Items.Clear();
            ddlNumeroUnico2.Items.Clear();
            radCmbSpool2.Items.Clear();
            radCmbSpool2.ClearSelection();
            radCmbSpool2.Text = string.Empty;
            rcbTubero.Items.Clear();
            rcbTubero.ClearSelection();
            rcbTubero.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            txtEtiquetaMaterial1.Text = string.Empty;
            ddlEtiquetaMaterial.Items.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="junta"></param>
        private void cargaDatosParaCaptura(JuntaSpoolProduccion junta)
        {
            if (junta.EtiquetaMaterial1 == "*" && junta.EtiquetaMaterial2 == "*")
            {
                //Esto no debe pasar
                RenderErrors(MensajesAplicacion.JuntaCampo_EtiquetaLocalizacionInvalida);
                vistaReadOnlyMenosBoton();
            }
            else if (junta.EtiquetaMaterial1 == "*" || junta.EtiquetaMaterial2 == "*")
            {
                //En este caso solo podremos cargar el lado "izquierdo" con los datos del spool
                //al que pertenece la junta.
                string etiquetaDeMaterial = junta.EtiquetaMaterial1 == "*" ? junta.EtiquetaMaterial2 : junta.EtiquetaMaterial1;
                cargaNusLadoIzquierdo(etiquetaDeMaterial, junta.SpoolID);
                txtEtiquetaMaterial1.Text = etiquetaDeMaterial;
                hdnEtiquetaMaterial1.Value = etiquetaDeMaterial;
                hdnEtiquetaMaterial2.Value = "*";
            }
            else
            {
                string [] materiales = SpoolBO.Instance.ObtenerEtiquetasDeMaterial(junta.SpoolID);

                bool esEtiquetaIzquierda = materialesContieneEtiqueta(materiales, junta.EtiquetaMaterial1);
                bool esEtiquetaDerecha = materialesContieneEtiqueta(materiales, junta.EtiquetaMaterial2);

                if (esEtiquetaIzquierda && esEtiquetaDerecha)
                {
                    //Esto no es válido, se trata de una junta SHOP
                    RenderErrors(MensajesAplicacion.JuntaCampo_AmbosMaterialesEncontrados);
                    vistaReadOnlyMenosBoton();
                }
                else if (esEtiquetaIzquierda)
                {
                    cargaDatosEtiquetas(junta.EtiquetaMaterial1, junta.SpoolID, junta.EtiquetaMaterial2);
                }
                else if (esEtiquetaDerecha)
                {
                    cargaDatosEtiquetas(junta.EtiquetaMaterial2, junta.SpoolID, junta.EtiquetaMaterial1);
                }
                else
                {
                    RenderErrors(MensajesAplicacion.JuntaCampo_NoEncuentraMaterial);
                    vistaReadOnlyMenosBoton();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etiquetaIzquierda"></param>
        /// <param name="spoolID"></param>
        /// <param name="etiquetaDerecha"></param>
        private void cargaDatosEtiquetas(string etiquetaIzquierda, int spoolID, string etiquetaDerecha)
        {
            hdnEtiquetaMaterial1.Value = etiquetaIzquierda;
            hdnEtiquetaMaterial2.Value = etiquetaDerecha;
            cargaNusLadoIzquierdo(etiquetaIzquierda, spoolID);
            txtEtiquetaMaterial1.Text = etiquetaIzquierda;
            ddlEtiquetaMaterial.Items.Add(etiquetaDerecha);
            ddlEtiquetaMaterial.SelectedIndex = 0;
            ddlEtiquetaMaterial.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materiales"></param>
        /// <param name="parteEtiquetaLocalizacion"></param>
        /// <returns></returns>
        private bool materialesContieneEtiqueta(string[] materiales, string parteEtiquetaLocalizacion)
        {
            string etiqueta = parteEtiquetaLocalizacion.PadLeft(15, '0');

            foreach (string etMaterial in materiales)
            {
                if (etMaterial.PadLeft(15, '0') == etiqueta)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void cargaDatosJuntaDeCampo()
        {
            JuntaCampoArmadoInfo jca = JuntaCampoArmadoBO.Instance.DatosJuntaCampoArmado(JuntaCampoArmadoID);
            mdpFechaArmado.SelectedDate = jca.FechaArmado;
            mdpFechaReporte.SelectedDate = jca.FechaReporte;

            radCmbSpool2.Text = jca.Spool2;
            rcbTubero.Text = jca.CodigoTubero;

            txtEtiquetaMaterial1.Text = jca.EtiquetaMaterial1;
            txtSpool1.Text = jca.Spool1;
            litJunta.Text = jca.EtiquetaJunta;

            ddlNumeroUnico1.Items.Add(jca.NumeroUnico1);
            ddlNumeroUnico1.SelectedIndex = 0;

            ddlEtiquetaMaterial.Items.Add(jca.EtiquetaMaterial2);
            ddlEtiquetaMaterial.SelectedIndex = 0;

            ddlNumeroUnico2.Items.Add(jca.NumeroUnico2);
            ddlNumeroUnico2.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etiquetaDeMaterial"></param>
        /// <param name="spoolID"></param>
        /// <param name="ddlNumerosUnicos"></param>
        /// <param name="reqNumeroUnico"></param>
        private void cargaComboNumerosUnicos(string etiquetaDeMaterial, int spoolID, DropDownList ddlNumerosUnicos, RequiredFieldValidator reqNumeroUnico)
        {
            List<Simple> nus = JuntaCampoArmadoBO.Instance
                                                 .ObtenerNumerosUnicosCandidatosParaArmadoCampo(etiquetaDeMaterial, spoolID);

            ddlNumerosUnicos.Enabled = true;
            reqNumeroUnico.Enabled = true;

            if (nus.Count == 1)
            {
                ddlNumerosUnicos.BindToEnumerable(nus, "Valor", "ID", false, null);
                ddlNumerosUnicos.Enabled = false;
                reqNumeroUnico.Enabled = false;
            }
            else if (nus.Count > 1)
            {
                ddlNumerosUnicos.BindToEnumerableWithEmptyRow(nus, "Valor", "ID", null);
            }
            else
            {
                //TODO: No hay candidatos, avisar o mandar mensaje?
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etiquetaDeMaterial"></param>
        /// <param name="spoolID"></param>
        private void cargaNusLadoIzquierdo(string etiquetaDeMaterial, int spoolID)
        {
            cargaComboNumerosUnicos(etiquetaDeMaterial, spoolID, ddlNumeroUnico1, reqNumUnico1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etiquetaDeMaterial"></param>
        /// <param name="spoolID"></param>
        private void cargaNusLadoDerecho(string etiquetaDeMaterial, int spoolID)
        {
            cargaComboNumerosUnicos(etiquetaDeMaterial, spoolID, ddlNumeroUnico2, reqNumUnico2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="junta"></param>
        private void bindDatosGenericos(JuntaSpoolProduccion junta)
        {
            litSpool.Text = junta.Spool;
            hdnSpool1.Value = junta.SpoolID.SafeStringParse();
            hdnProyectoID.Value = junta.ProyectoID.ToString();
            litJunta.Text = junta.Etiqueta;
            litTipoJunta.Text = junta.TipoJunta;
            litNumeroControl.Text = junta.NumeroControl;
            litLocalizacion.Text = junta.EtiquetaMaterial1 + "-" + junta.EtiquetaMaterial2;
            litEspesor.Text = junta.Espesor.SafeStringParse();
            txtSpool1.Text = junta.Spool;
            OrdenTrabajoSpoolID = junta.OrdenTrabajoSpoolID;
        }
        #endregion

        #region Validaciones

        /// <summary>
        /// Valida que se haya seleccionado un tubero válido
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cusRcbTubero_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = rcbTubero.SelectedValue.SafeIntParse() > 0;
        }

        /// <summary>
        /// Se asegura que haya un spool2 seleccionado
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cusSpool2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radCmbSpool2.SelectedValue.SafeIntParse() > 0;
        }

        #endregion

        #region Eventos de controles

        /// <summary>
        /// Cargamos el combo con las etiquetas de material del spool seleccionado (en caso que la localización tenga un *).
        /// Cargamos el combo de números únicos 2 con los candidatos disponibles.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void radCmbSpool_OnSelectedIndexChanged(object source, EventArgs args)
        {
            int spool2ID = radCmbSpool2.SelectedValue.SafeIntParse();

            if (spool2ID > 0)
            {
                //Significa que ambas etiquetas vienen especificadas, por lo que ya no
                //hay que buscar etiquetas candidatas
                if (ddlEtiquetaMaterial.Enabled)
                {
                    ddlEtiquetaMaterial.DataSource = JuntaCampoArmadoBO.Instance.ObtenerEtiquetasDeMaterialArmadas(spool2ID);
                    ddlEtiquetaMaterial.DataBind();
                    ddlEtiquetaMaterial.SelectedIndex = 0;
                }

                if (!string.IsNullOrEmpty(ddlEtiquetaMaterial.SelectedValue))
                {
                    cargaNusLadoDerecho(ddlEtiquetaMaterial.SelectedValue, spool2ID);
                }
            }
            else
            {
                ddlEtiquetaMaterial.Items.Clear();
                ddlEtiquetaMaterial.ClearSelection();
                ddlEtiquetaMaterial.Text = "";
                ddlNumeroUnico2.Items.Clear();
                ddlNumeroUnico2.ClearSelection();
                ddlNumeroUnico2.Text = "";
                ddlNumeroUnico2.Enabled = true;
            }

        }

        /// <summary>
        /// Al seleccionar una etiqueta del spool2 cargar los números únicos candidatos.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void ddlEtiquetaMaterialSelectedIndexChanged(object source, EventArgs args)
        {
            int spool2ID = radCmbSpool2.SelectedValue.SafeIntParse();
            if (spool2ID > 0)
            {
                if (!string.IsNullOrEmpty(ddlEtiquetaMaterial.SelectedValue))
                {
                    cargaNusLadoDerecho(ddlEtiquetaMaterial.SelectedValue, spool2ID);
                }
            }
            else
            {
                ddlNumeroUnico2.Items.Clear();
                ddlNumeroUnico2.ClearSelection();
                ddlNumeroUnico2.Text = "";
                ddlNumeroUnico2.Enabled = true;
            }
        }

        /// <summary>
        /// Elimina el armado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEliminar_OnClick(object sender, EventArgs e)
        {
            try
            {
                JuntaCampoArmadoBO.Instance.BorraJuntaCampoArmado(JuntaCampoArmadoID, SessionFacade.UserId);

                //Recargar UI
                cargaDatos();
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Persiste la información de la forma en la BD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                JuntaCampoArmadoInfo datosArmado = new JuntaCampoArmadoInfo
                {
                    TuberoID = rcbTubero.SelectedValue.SafeIntParse(),
                    EtiquetaMaterial1 = txtEtiquetaMaterial1.Text,
                    EtiquetaMaterial2 = ddlEtiquetaMaterial.SelectedValue,
                    EtiquetaJunta = litJunta.Text,
                    FechaArmado = mdpFechaArmado.SelectedDate.Value.Date,
                    FechaReporte = mdpFechaReporte.SelectedDate.Value.Date,
                    JuntaCampoArmadoID = JuntaCampoArmadoID,
                    JuntaCampoID = JuntaCampoID,
                    JuntaSpoolID = JuntaSpoolID,
                    NumeroUnico1ID = ddlNumeroUnico1.SelectedValue.SafeIntParse(),
                    NumeroUnico2ID = ddlNumeroUnico2.SelectedValue.SafeIntParse(),
                    Observaciones = txtObservaciones.Text,
                    Spool1ID = hdnSpool1.Value.SafeIntParse(),
                    Spool2ID = radCmbSpool2.SelectedValue.SafeIntParse()
                };                

                JuntaSpoolProduccion datosJunta = new JuntaSpoolProduccion
                {
                    Etiqueta = litJunta.Text,
                    OrdenTrabajoSpoolID = OrdenTrabajoSpoolID,
                    SpoolID = hdnSpool1.Value.SafeIntParse()
                };

                try
                {
                    ValidacionesJuntaCampo.ValidaFechasArmado(datosArmado);
                    JuntaCampoArmadoBO.Instance.GuardaArmado(datosArmado, datosJunta, SessionFacade.UserId);
                    //Actualizar UI, aunque sea ir a BD
                    cargaDatos();
                }
                catch (BaseValidationException ble)
                {
                    RenderErrors(ble);
                }
            }
        }

        #endregion

        #region Auxilares

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readOnly"></param>
        private void vistaReadOnly(bool readOnly)
        {
            mdpFechaArmado.Enabled = !readOnly;
            mdpFechaReporte.Enabled = !readOnly;
            rcbTubero.Enabled = !readOnly;
            radCmbSpool2.Enabled = !readOnly;
            ddlEtiquetaMaterial.Enabled = !readOnly;
            txtObservaciones.CssTextBox = readOnly ? "soloLectura" : string.Empty;
            btnGuardar.Visible = !readOnly;
            btnEliminar.Visible = readOnly;
            ddlNumeroUnico1.Enabled = !readOnly;
            ddlNumeroUnico2.Enabled = !readOnly;
        }

        /// <summary>
        /// 
        /// </summary>
        private void vistaReadOnlyMenosBoton()
        {
            vistaReadOnly(true);
            btnEliminar.Visible = false;
        }


        #endregion
    }
}
