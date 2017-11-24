using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Materiales;

namespace SAM.Web.Controles.Materiales
{
    public partial class AltaNumUnicoGeneral : System.Web.UI.UserControl
    {
        public event EventHandler DdlCedulaOrIC_SelectedIndexChanged;

        public int NumeroUnicoActual
        {
            get
            {
                return ViewState["NumeroUnicoActual"].ToString().SafeIntParse();
            }
            set
            {
                ViewState["NumeroUnicoActual"] = value;
            }
        }

        public int NumeroUnicoTotal
        {
            get
            {
                return ViewState["NumeroUnicoTotal"].ToString().SafeIntParse();
            }
            set
            {
                ViewState["NumeroUnicoTotal"] = value;
            }
        }

        public bool Advertencia
        {
            get
            {
                if (ViewState["Advertencia"] != null)
                {
                    return (bool)ViewState["Advertencia"];
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["Advertencia"] = value;
            }
        }

        #region Propiedades Solo Lectura

        public int ItemCodeID
        {
            get
            {
                return rcbItemCode.SelectedValue.SafeIntParse();
            }
        }

        public string ItemCodeDescripcion
        {
            get
            {
                return lblDescripcion.Text;
            }
        }

        public decimal Diametro1
        {
            get
            {
                return txtDiametro1.Text.SafeDecimalParse();
            }
        }

        public decimal Diametro2
        {
            get
            {
                return txtDiametro2.Text.SafeDecimalParse();
            }
        }

        public string Estatus
        {
            get
            {
                return ddlEstatus.SelectedValue;
            }
        }

        public int? Profile1
        {
            get
            {
                if (ddlProfile.SelectedValue.SafeIntParse() > 0)
                {
                    return ddlProfile.SelectedValue.SafeIntParse();
                }
                else
                {
                    return null;
                }
            }
        }

        public int? Profile2
        {
            get
            {
                if (ddlProfile2.SelectedValue.SafeIntParse() > 0)
                {
                    return ddlProfile2.SelectedValue.SafeIntParse();
                }
                else
                {
                    return null;
                }
            }
        }

        public string Cedula
        {
            get
            {
                return ddlCedula.SelectedItem.Text;
            }
        }

        public int Cantidad
        {
            get
            {
                return txtCantidad.Text.SafeIntParse();
            }
        }

        public int CantidadDanada
        {
            get
            {
                return txtCantidadDanada.Text.SafeIntParse() > 0 ?
                       txtCantidadDanada.Text.SafeIntParse() : 0;
            }
        }

        public bool Danada
        {
            get
            {
                return chkDanada.Checked;
            }
        }
        

        public int ColadaID
        {
            get
            {
                return ddlColada.SelectedValue.SafeIntParse();
            }
        }

        public string NumeroUnicoCliente
        {
            get
            {
                return txtNumeroUnicoCliente.Text;
            }
        }

        public string Observaciones
        {
            get
            {
                return txtObservaciones.Text;
            }
        }

        public int CantidadCongelada
        {
            get
            {
                return lblCantidadCongelada.Text.SafeIntParse();
            }
        }

        public int NumeroUnicoID
        {
            get
            {
                return ViewState["NumeroUnicoID"].ToString().SafeIntParse();
            }
            set
            {
                ViewState["NumeroUnicoID"] = value;
            }
        }

        public int CantidadDespachada
        {
            get
            {
                return lblCantidadDespachada.Text.SafeIntParse();
            }
        }

        #endregion


        /// <summary>
        /// Carga la información inicial dependiendo del número único a dar de alta.
        /// </summary>
        /// <param name="numUnico"></param>
        /// <param name="numAnterior"></param>
        /// <param name="puedeEditarDatosBase"></param>
        public void CargaInformacion(NumeroUnico numUnico, NumeroUnico numAnterior, bool puedeEditarDatosBase, bool tienePermisosEditar)
        {
            NumeroUnicoID = numUnico.NumeroUnicoID;
            NumeroUnicoInventario nui = numUnico.NumeroUnicoInventario;
           
            if (nui != null)
            {
                int cantidadDespachada = 0;
                //hacemos visible el panel de cantidades
                pnlInfoCantidades.Visible = true;
                lblCantidadCongelada.Text = nui.InventarioCongelado.ToString();
                if(numUnico.Despacho != null){
                    cantidadDespachada = numUnico.Despacho.Where(x=>!x.Cancelado).Select(x=> x.Cantidad).Sum();
                    lblCantidadDespachada.Text = cantidadDespachada.ToString();
                }

                List<NumeroUnicoMovimiento> numOtrasEntradas;
                List<NumeroUnicoMovimiento> numOtrasSalidas;
                List<NumeroUnicoMovimiento> numEntradasPintura;
                List<NumeroUnicoMovimiento> numSalidasPintura;
                List<NumeroUnicoMovimiento> numMermasCorte;

                if(numUnico.NumeroUnicoMovimiento != null){

                    numOtrasEntradas = numUnico.NumeroUnicoMovimiento.Where(x=> 
                                            x.TipoMovimientoID != (int)TipoMovimientoEnum.EntradaPintura 
                                            && x.TipoMovimiento.DisponibleMovimientosUI 
                                            && x.TipoMovimiento.EsEntrada).ToList();

                    numOtrasSalidas= numUnico.NumeroUnicoMovimiento.Where(x=> 
                                            x.TipoMovimientoID != (int)TipoMovimientoEnum.SalidaPintura
                                            && x.TipoMovimiento.DisponibleMovimientosUI 
                                            && !x.TipoMovimiento.EsEntrada).ToList();

                    numMermasCorte = numUnico.NumeroUnicoMovimiento.Where(x =>
                                            x.TipoMovimientoID == (int)TipoMovimientoEnum.MermaCorte && x.Estatus != EstatusNumeroUnicoMovimiento.CANCELADO).ToList();

                    numEntradasPintura = numUnico.NumeroUnicoMovimiento.Where(x=> 
                                            x.TipoMovimientoID == (int)TipoMovimientoEnum.EntradaPintura
                                            && x.TipoMovimiento.DisponibleMovimientosUI 
                                            && x.TipoMovimiento.EsEntrada).ToList();

                    numSalidasPintura = numUnico.NumeroUnicoMovimiento.Where(x=> 
                                            x.TipoMovimientoID == (int)TipoMovimientoEnum.SalidaPintura
                                            && x.TipoMovimiento.DisponibleMovimientosUI 
                                            && !x.TipoMovimiento.EsEntrada).ToList();
                    
                    lblCantidadOtrasSalidas.Text = numOtrasSalidas.Select(x => x.Cantidad).Sum().ToString();

                    lblCantidadSalidasTemporales.Text = numSalidasPintura.Sum(x => x.Cantidad) - numEntradasPintura.Sum(x => x.Cantidad) + string.Empty;

                    lblMermasCorte.Text = numMermasCorte.Select(x => x.Cantidad).Sum().ToString();
                }
                lblCantidadCongelada.Text = nui.InventarioCongelado.ToString();
            }

            hdnProyectoID.Value = numUnico.ProyectoID.ToString();

            if (numUnico.RecepcionNumeroUnico.Count > 0)
            {
                lblFecha.Text = numUnico.RecepcionNumeroUnico[0].Recepcion.FechaRecepcion.ToString("d");
                lblTranportista.Text = numUnico.RecepcionNumeroUnico[0].Recepcion.Transportista.Nombre;
                lblNumeroActual.Text = NumeroUnicoActual.ToString();
                lblNumerosTotales.Text = NumeroUnicoTotal.ToString();
            }
            else
            {
                plhRecepcion.Visible = false;
            }

            btnAgregarItemCode.OnClientClick = string.Format("return Sam.Materiales.AbrePopupAgregaItemCodes('{0}');", numUnico.ProyectoID);
            btnColada.OnClientClick = string.Format("return Sam.Materiales.AbrePopupAgregaColada('{0}');", numUnico.ProyectoID);
            txtNumeroUnico.Text = numUnico.Codigo;
            txtNumeroUnicoCliente.Text = numUnico.NumeroUnicoCliente;
            txtObservaciones.Text = numUnico.Observaciones;

            cargaCombos(numUnico.ProyectoID);

            //Si se mantienen datos
            if (numAnterior != null)
            {
                cargaDatos(numAnterior);
            }
            else //edicion numerounico
            {
                cargaDatos(numUnico);
            }

            if (!puedeEditarDatosBase)
            {
                if (tienePermisosEditar)
                {
                    Advertencia = true;
                }
                else
                {
                    rcbItemCode.Enabled = false;
                    txtDiametro1.Enabled = false;
                    txtDiametro2.Enabled = false;
                    //txtCantidad.Enabled = false;
                    //txtCantidadDanada.Enabled = false;
                    //chkDanada.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Carga los combos de acuerdo al proyecto al que pertenece el numero unico.
        /// </summary>
        /// <param name="proyectoID"></param>
        private void cargaCombos(int proyectoID)
        {
            ddlProfile.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposCorte(), -1);
            ddlProfile2.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposCorte(), -1);
            ddlCedula.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerCedulas(), -1);
        }

        /// <summary>
        /// Carga los campos con los datos del numero unico recibido
        /// </summary>
        /// <param name="numAnterior"></param>
        public void cargaDatos(NumeroUnico numAnterior)
        {

            ddlProfile.SelectedValue = numAnterior.TipoCorte1ID.HasValue ? numAnterior.TipoCorte1ID.Value.ToString() : string.Empty;
            ddlProfile2.SelectedValue = numAnterior.TipoCorte2ID.HasValue ? numAnterior.TipoCorte2ID.Value.ToString() : string.Empty;
            ddlColada.SelectedValue = numAnterior.ColadaID.HasValue ? numAnterior.ColadaID.Value.ToString() : string.Empty;
            ddlColada.Text = numAnterior.ColadaID.HasValue ? numAnterior.Colada.NumeroColada : string.Empty;
            txtObservaciones.Text = numAnterior.Observaciones != null ? numAnterior.Observaciones : string.Empty;

            if (numAnterior.ColadaID.HasValue)
            {
                cargaDatosColada(numAnterior.ColadaID.Value);
            }

            ddlEstatus.SelectedValue = numAnterior.Estatus;
            
            if (!String.IsNullOrEmpty(numAnterior.Cedula))
            {
                ddlCedula.SelectedValue = ddlCedula.Items.FindByText(numAnterior.Cedula).Value;
            }
            
            rcbItemCode.SelectedValue = numAnterior.ItemCodeID.HasValue ? numAnterior.ItemCodeID.Value.ToString() : string.Empty;
            rcbItemCode.Text = numAnterior.ItemCodeID.HasValue ? numAnterior.ItemCode.Codigo : string.Empty;

            if(numAnterior.ItemCodeID.HasValue)
            {
                cargaDatosItemCode(numAnterior.ItemCodeID.Value);
            }
            txtDiametro1.Text = String.Format("{0:0.000}", numAnterior.Diametro1);
            txtDiametro2.Text = String.Format("{0:0.000}", numAnterior.Diametro2);
            txtCantidad.Text = numAnterior.NumeroUnicoInventario != null ? numAnterior.NumeroUnicoInventario.CantidadRecibida.ToString() : string.Empty;
            chkDanada.Checked = numAnterior.TieneDano;

            if (numAnterior.TieneDano)
            {
                phDanada.Visible = true;
                txtCantidadDanada.Text = numAnterior.NumeroUnicoInventario != null ? numAnterior.NumeroUnicoInventario.CantidadDanada.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Limpia los campos del control
        /// </summary>
        public void LimpiaDatos()
        {
            txtNumeroUnico.Text = string.Empty;
            ddlProfile.SelectedIndex = -1;
            ddlProfile2.SelectedIndex = -1;
            ddlColada.SelectedIndex = -1;
            rcbItemCode.SelectedIndex = -1;
            phColada.Visible = false;
            phItemCode.Visible = false;
            ddlEstatus.SelectedIndex = -1;
            ddlCedula.Text = string.Empty;
            rcbItemCode.Text = string.Empty;
            txtDiametro1.Text = string.Empty;
            txtDiametro2.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtCantidadDanada.Text = string.Empty;
            txtNumeroUnicoCliente.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            chkDanada.Checked = false;
            pnlDanada.Visible = false;
        }

        private void cargaDatosColada(int? coladaID)
        {
            if (coladaID.HasValue)
            {
                Colada colada = ColadasBO.Instance.ObtenerConFabricanteYAcero(coladaID.Value);

                lblCertificado.Text = colada.NumeroCertificado;
                lblEstatusColada.Text = LanguageHelper.CustomCulture == LanguageHelper.INGLES ?
                    colada.HoldCalidad ? "HOLD" : "Approved" :
                    colada.HoldCalidad ? "HOLD" : "Aprobada";
                lblAcero.Text = colada.Acero.Nomenclatura;
                lblAceroFam.Text = colada.Acero.FamiliaAcero.Nombre;
                lblMaterialFam.Text = colada.Acero.FamiliaAcero.FamiliaMaterial.Nombre;

                phColada.Visible = true;
            }
        }

        private void cargaDatosItemCode(int? itemCodeID)
        {
            if (itemCodeID.HasValue)
            {
                ItemCode itemCode = ItemCodeBO.Instance.Obtener(itemCodeID.Value);

                lblDescripcion.Text = itemCode.DescripcionEspanol;
                phItemCode.Visible = true;
            }
        }

        #region Eventos

        /// <summary>
        /// Muestra y habilita el campo de Cantidad Dañada cuando el check se selecciona
        /// Esconde y deshabilita el campo de Cantidad Dañana cuando el check se deselecciona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDanada_CheckedChanged(object sender, EventArgs e)
        {
            phDanada.Visible = chkDanada.Checked;

            if (!chkDanada.Checked)
            {
                txtCantidadDanada.Text = string.Empty;
            }
        }

        protected void ddlColada_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ddlColada.SelectedValue))
            {
                cargaDatosColada(ddlColada.SelectedValue.SafeIntParse());
            }
            else
            {
                phColada.Visible = false;
            }
        }

        protected void rcbItemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(rcbItemCode.SelectedValue))
            {
               cargaDatosItemCode(rcbItemCode.SelectedValue.SafeIntParse());

               if (DdlCedulaOrIC_SelectedIndexChanged != null)
               {
                   DdlCedulaOrIC_SelectedIndexChanged(sender, e);
               }
            }
            else
            {
                phItemCode.Visible = false;
            }
        }

        #endregion

        protected void cusItemCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = rcbItemCode.SelectedValue.SafeIntParse() > 0;
        }

        protected void cusColdada_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlColada.SelectedValue.SafeIntParse() > 0;
        }

        protected void ddlCedula_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlCedulaOrIC_SelectedIndexChanged != null)
            {
                DdlCedulaOrIC_SelectedIndexChanged(sender, e);
            }
        }

        protected void cusEstaEnTr_OnServerValidate(object sender, ServerValidateEventArgs e)
        {
        }

        protected void cusPreSaveValidation_OnServerValidate(object sender, ServerValidateEventArgs e)
        {
            try
            {
                e.IsValid = NumeroUnicoBL.Instance.PuedeEditarCantidades(NumeroUnicoID, txtCantidad.Text.SafeIntParse(0), txtCantidadDanada.Text.SafeIntParse(0));
                if (e.IsValid)
                {
                    e.IsValid = NumeroUnicoBL.Instance.ItemCodeValido(NumeroUnicoID, rcbItemCode.SelectedValue.SafeIntParse());
                }
            }
            catch (BaseValidationException ex)
            {
                cusPreSaveValidation.ErrorMessage = ex.Details[0];
                e.IsValid = false;                
            }
        }
      
    }
}