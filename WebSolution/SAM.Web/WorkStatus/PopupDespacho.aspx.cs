using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using SAM.Web.Classes;
using SAM.Web.Common;
using SAM.Entities.Cache;


namespace SAM.Web.WorkStatus
{
    /// <summary>
    /// Esta página es la que solicita los datos de captura para efectuar el despacho
    /// de un material en particular.
    /// 
    /// El comportamiento de este popup cambia un poco en base a si el material a despachar
    /// es un tubo o un accesorio.
    /// </summary>
    public partial class PopupDespacho : SamPaginaPopup
    {
        private string EtiquetaMaterial
        {
            get { return (string)ViewState["EtiquetaMaterialDespacho"]; }
            set { ViewState["EtiquetaMaterialDespacho"] = value; }
        }

        private int NumUnico
        {
            get { return ViewState["NumUnico"].SafeIntParse(); }
            set { ViewState["NumUnico"] = value; }
        }


        /// <summary>
        /// Carga la información de material seleccionado para el despacho.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAOrdenTrabajoMaterial(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando despachar una orden de trabajo material {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargaInformacion();
            }
            else
            {
                if (!string.IsNullOrEmpty(hdnNuSeleccionado.Value))
                {
                    //Este hidden se settea por JS entonces hay que recalcularlo en el postback
                    string[] split = hdnNuSeleccionado.Value.Split('|');
                    lblCantidad.Text = split[0];
                    lblIc.Text = split[1];
                    lblIcDesc.Text = split[2];
                    lblEquiv.Text = split[3];
                }
            }
        }



        /// <summary>
        /// Asume que en el QS viene el ID del registro en la tabla OrdenTrabajoMaterial y en base a ese ID hace
        /// las consultas necesarias para cargar la información en pantalla que le ayude al usuario a llevar a cabo el despacho.
        /// </summary>
        private void cargaInformacion()
        {
            OrdenTrabajoMaterial odtM = OrdenTrabajoMaterialBO.Instance.ObtenerInformacionParaDespacho(EntityID.Value);

            VersionRegistro = odtM.VersionRegistro;

            txtCantRequerida.Text = odtM.MaterialSpool.Cantidad.ToString();
            txtD1.Text = odtM.MaterialSpool.Diametro1.ToString();
            txtD2.Text = odtM.MaterialSpool.Diametro2.ToString();
            txtItemCode.Text = odtM.MaterialSpool.ItemCode.Codigo;
            txtDescIc.Text = odtM.MaterialSpool.ItemCode.DescripcionEspanol;
            txtEtiqueta.Text = odtM.MaterialSpool.Etiqueta;
            hdnMatSpoolID.Value = odtM.MaterialSpoolID.ToString();
            hdnProyectoID.Value = odtM.MaterialSpool.ItemCode.ProyectoID.ToString();

            if (odtM.MaterialSpool.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo)
            {
                //Debe de venir de corte
                phControlesTubo.Visible = true;
                phControlesAccesorio.Visible = false;
                ProyectoConfiguracion config = ProyectoConfiguracionBO.Instance.Obtener(odtM.MaterialSpool.ItemCode.ProyectoID);
                int mmTolerancia = config.ToleranciaCortes.HasValue ? config.ToleranciaCortes.Value : 0;
                int cantidadRequerida = odtM.MaterialSpool.Cantidad;

                //Solo se puede despachar si la cantidad está dentro de la tolerancia del proyecto
                rngCantidad.MinimumValue = Math.Max(cantidadRequerida - mmTolerancia, 0).ToString();
                rngCantidad.MaximumValue = (cantidadRequerida + mmTolerancia).ToString();
                rngCantidad.Enabled = true;

                //En teoria para llegar aquí siempre debe tener corte asi que podemos asumir que el objeto
                //nunca viene nulo
                InfoCorteDespacho corte = CorteDetalleBO.Instance.ObtenerInformacionDeCorteParaDespacho(odtM.CorteDetalleID);
                lblCantidad.Text = corte.LongitudDelCorte.ToString();
                lblIc.Text = corte.CodigoItemCode;
                lblIcDesc.Text = corte.DescripcionItemCode;

                //La cantidad a despachar para un tubo debe ser idéntica a la longitud del corte
                cmpTubo.Enabled = true;
                cmpTubo.ValueToCompare = lblCantidad.Text;

                //El número único ya viene fijo por el corte
                txtNumUnico.Text = corte.CodigoNumeroUnico;
                NumUnico = corte.NumeroUnicoID;

                bool esEquivalente = odtM.MaterialSpool.ItemCodeID != corte.ItemCodeID
                                        || odtM.MaterialSpool.Diametro1 != corte.Diametro1
                                        || odtM.MaterialSpool.Diametro2 != corte.Diametro2;

                lblEquiv.Text = TraductorEnumeraciones.TextoSiNo(esEquivalente);

                //Por default llenar la cantidad a despachar con la longitud del corte
                txtCantidad.Text = corte.LongitudDelCorte.ToString();

                //Deshabilitar los validadores de accesorio
                cmpCantidad.Enabled = false;
                //cusCombo.Enabled = false;
            }
            else
            {
                //Se trata de un accesorio hay que ir a inventarios
                phControlesAccesorio.Visible = true;
                phControlesTubo.Visible = false;

                //Validar que la cantidad sea exactamente lo mismo que pide ingeniería
                cmpCantidad.Enabled = true;
                //cusCombo.Enabled = true;

                //Por default la cantidad requerida debe ser igual a la cantidad solicitada por ingeniería
                txtCantidad.Text = txtCantRequerida.Text;

                //Deshabiliatar los validadores de tubo
                rngCantidad.Enabled = false;
                cmpTubo.Enabled = false;
            }
        }

        /// <summary>
        /// Se dispara cuando las validaciones en el cliente pasaron y se desea despachar un material en particular.
        /// 
        /// El tipo de despacho depende de si se trata de un tubo o de un accesorio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDespachar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    string errores = "";
                    string salto = "<br />";

                    if (phControlesAccesorio.Visible == true && (rcbNumeroUnico.SelectedValue == null || rcbNumeroUnico.SelectedValue.SafeIntParse() < 0))
                    {
                        errores += (Cultura == "en-US" ? "The unique number in required" : "El número único es requerido") + salto;
                    }

                    //if (rcbDespachadores.SelectedValue.SafeIntParse() < 0 || rcbDespachadores.SelectedValue == null)
                    //{
                    //    errores += (Cultura == "en-US" ? "You must select a dispatcher" : "Debe seleccionar un despachador") + salto; ;
                    //}

                    if (errores != string.Empty)
                    {
                        throw new BaseValidationException(errores);
                    }

                    //Ir nuevamente a base de datos por la misma información que fuimos al principio
                    OrdenTrabajoMaterial odtM = OrdenTrabajoMaterialBO.Instance.ObtenerInformacionParaDespacho(EntityID.Value);
                    List<string> mensaje = new List<string>();
                    if (NumUnico > 0)
                    {

                        mensaje = ArmadoBO.Instance.ObtenerNumeroUnicoEnArmadoParaDespacho(NumUnico, odtM);

                    }
                    else
                    {
                        mensaje = ArmadoBO.Instance.ObtenerNumeroUnicoEnArmadoParaDespacho(rcbNumeroUnico.SelectedValue.SafeIntParse(), odtM);
                    }

                    if (mensaje.Count > 0)
                    {
                        foreach (string msg in mensaje)
                        {
                            string[] partesMensaje = msg.Split('/');
                            lblMensajeConfirma.Text += Cultura == "en-US" ?
                                "The board already has registered related the following unique number:<br/>Code: "
                                + partesMensaje[0] + "<br/>Certificate: " + partesMensaje[1]
                                + "<br/><br/>Diameter 1: " + partesMensaje[2] + "<br/>Diameter 2: " + partesMensaje[3]
                                + "<br/>If Save changes the unique number registered in a different process was modified.<br/>" :
                                "La junta relacionada ya tiene registrado el siguiente numero unico:"
                                + "<br/><br/>Codigo: " + partesMensaje[0] + "<br/>Cédula: " + partesMensaje[1]
                                + "<br/>Diametro 1: " + partesMensaje[2] + "<br/>Diametro 2: " + partesMensaje[3]
                                + "<br/>Si Guarda los cambios se modificara el numero unico registrado en un proceso diferente.<br/>";
                            EtiquetaMaterial += "/" + partesMensaje[4];
                        }
                        PopUpConfirma.VisibleOnPageLoad = true;
                    }
                    else
                    {
                        EtiquetaMaterial = string.Empty;
                        GenerarDespacho();
                    }
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
        /// <summary>
        /// Obtiene el ID de orden trabajo material que puedo despachar a continuación, si no hay ningún match, regresa nulo.
        /// </summary>
        /// <param name="odtM">Material despachado recientemente</param>
        /// <returns>ID del siguiente despachable o nulo</returns>
        private static int? obtenSiguienteMaterialDespachable(OrdenTrabajoMaterial odtM)
        {
            int? siguienteID = null;

            //Traerme los materiales del número de control en particular
            //Sólo aquellos que se puedan despachar
            List<GrdMaterialesDespacho> materiales =
                OrdenTrabajoSpoolBO.Instance
                                   .ObtenerMaterialesParaDespacho(odtM.OrdenTrabajoSpoolID)
                                   .Where(x => x.PerteneceAOdt)
                                   .Where(x => (x.EsTubo && x.TieneCorte) || !x.EsTubo)
                                   .OrderBy(x => x.EtiquetaMaterial.SafeIntParse(99))
                                   .ToList();

            int indice = 0;

            //Con este for se hace lo siguiente:
            // 1. Buscar el índice del material que acabo de despachar
            // 2. Una vez encontrado busco hacia adelante el siguiente que podría despachar
            // 3. Si no lo encuentro busco desde el índice cero hasta el índice de mi material reciente
            // 4. Si puedo despachar algo la variable siguienteID contiene el valor del siguiente despachable
            foreach (GrdMaterialesDespacho mat in materiales)
            {
                if (mat.OrdenTrabajoMaterialID == odtM.OrdenTrabajoMaterialID)
                {
                    for (int i = indice + 1; i < materiales.Count; i++)
                    {
                        if (!materiales[i].TieneDespacho)
                        {
                            siguienteID = materiales[i].OrdenTrabajoMaterialID;
                            break;
                        }
                    }

                    if (!siguienteID.HasValue)
                    {
                        for (int i = 0; i < indice; i++)
                        {
                            if (!materiales[i].TieneDespacho)
                            {
                                siguienteID = materiales[i].OrdenTrabajoMaterialID;
                                break;
                            }
                        }
                    }

                    break;
                }

                indice++;
            }

            return siguienteID;
        }

        /// <summary>
        /// Por algún motivo los combos de Telerik no se validan bien con un RequiredFieldValidator por lo que
        /// lo hacemos a través de un custom validator.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cusCombo_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = rcbNumeroUnico.SelectedValue.SafeIntParse() > 0;
        }

        protected void btnConfirma_Click(object sender, EventArgs e)
        {
            GenerarDespacho();
            PopUpConfirma.VisibleOnPageLoad = false;
        }

        protected void GenerarDespacho()
        {
            try
            {
                OrdenTrabajoMaterial odtM = OrdenTrabajoMaterialBO.Instance.ObtenerInformacionParaDespacho(EntityID.Value);
                odtM.VersionRegistro = VersionRegistro;

                if (odtM.MaterialSpool.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo)
                {
                    OrdenTrabajoMaterialBO.Instance.DespachaTubo(odtM,
                                                                    txtCantidad.Text.SafeIntParse(),
                                                                    SessionFacade.UserId, EtiquetaMaterial, rcbDespachadores.SelectedValue.SafeIntParse());
                }
                else
                {
                    OrdenTrabajoMaterialBO.Instance.DespachaAccesorio(odtM,
                                                                        txtCantidad.Text.SafeIntParse(),
                                                                        rcbNumeroUnico.SelectedValue.SafeIntParse(),
                                                                        SessionFacade.UserId, EtiquetaMaterial,
                                                                        rcbDespachadores.SelectedValue.SafeIntParse());
                }


                int? odtMID = obtenSiguienteMaterialDespachable(odtM);

                if (!odtMID.HasValue)
                {
                    //Muestra el mensaje de éxito
                    phControles.Visible = false;
                    phMensaje.Visible = true;
                }
                else
                {
                    EntityID = odtMID;
                    hdnNuSeleccionado.Value = string.Empty;
                    rcbNumeroUnico.SelectedValue = string.Empty;
                    rcbNumeroUnico.Text = string.Empty;
                    lblCantidad.Text = string.Empty;
                    lblIc.Text = string.Empty;
                    lblIcDesc.Text = string.Empty;
                    lblEquiv.Text = string.Empty;
                    rcbDespachadores.Items.Clear();
                    rcbDespachadores.SelectedIndex = -1;
                    cargaInformacion();
                }

                //Actualiza el grid de la ventana padre para que refleje que el material ya se despachó
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "ActualizaGridDespacho", "Sam.Workstatus.ActualizaGridDespacho();", true);
                
                //JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            PopUpConfirma.VisibleOnPageLoad = false;
        }

        protected void valDespachadores_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rcbDespachadores.SelectedValue.SafeIntParse() > 0)
            {
                args.IsValid = true;
            }
        }
    }
}