using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Utilerias;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Materiales;
using SAM.Web.Produccion.App_LocalResources;
using Resources;
using Mimo.Framework.Common;
using SAM.Entities.Personalizadas;
using SAM.Entities.Cache;
using SAM.BusinessLogic.Materiales;

namespace SAM.Web.Materiales
{
    public partial class MovimientosInventario : SamPaginaPrincipal
    {

        #region Propiedades privadas en ViewState
        
        private int _proyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] == null)
                {
                    ViewState["ProyectoID"] = -1;
                }

                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private int _numeroUnicoID
        {
            get
            {
                if (ViewState["NumeroUnicoID"] == null)
                {
                    ViewState["NumeroUnicoID"] = -1;
                }

                return (int)ViewState["NumeroUnicoID"];
            }

            set
            {
                ViewState["NumeroUnicoID"] = value;
            }
        }

        private int _tipoMaterialID
        {
            get
            {
                if (ViewState["TipoMaterialID"] == null)
                {
                    ViewState["TipoMaterialID"] = -1;
                }

                return (int)ViewState["TipoMaterialID"];
            }

            set
            {
                ViewState["TipoMaterialID"] = value;
            }
        }

        private int _totalSegmentos
        {
            get
            {
                return (int)ViewState["TotalSegmentos"];
            }

            set
            {
                ViewState["TotalSegmentos"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_MovimientosInventario);
                TiposMovimiento = CacheCatalogos.Instance.ObtenerTiposMovimiento();
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            try
            {
                NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerParaMovimientoInventarios(filtroGenerico.NumeroUnicoSelectedValue.SafeIntParse());

                pnlInformacion.Visible = true;
                phGuardar.Visible = true;
                rbEntrada.Checked = false;
                rbSalida.Checked = false;

               
                cargaDatos(numUnico);
                //Asignar los valores de los filtros a las propiedades del ViewState
                _proyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
                _numeroUnicoID = filtroGenerico.NumeroUnicoSelectedValue.SafeIntParse();
                _tipoMaterialID = numUnico.ItemCode.TipoMaterialID;

                if (numUnico.NumeroUnicoSegmento != null)
                {
                    _totalSegmentos = numUnico.NumeroUnicoSegmento.Count();
                }

                lblItemCodeData.Text = numUnico.ItemCode.Codigo;
                lblDescripcionData.Text = numUnico.ItemCode.DescripcionEspanol;
                lblDiametro1Data.Text = String.Format("{0:N3}", numUnico.Diametro1);
                lblDiametro2Data.Text = String.Format("{0:N3}", numUnico.Diametro2);

                lblInvFisicoData.Text = numUnico.NumeroUnicoInventario != null ? String.Format("{0:N3}", numUnico.NumeroUnicoInventario.InventarioFisico) : "0";
                lblInvCongeladoData.Text = numUnico.NumeroUnicoInventario != null ? String.Format("{0:N3}", numUnico.NumeroUnicoInventario.InventarioCongelado) : "0";
                lblInvDanadoData.Text = numUnico.NumeroUnicoInventario != null ? String.Format("{0:N3}", numUnico.NumeroUnicoInventario.CantidadDanada) : "0";
                lblInvDisponibleData.Text = numUnico.NumeroUnicoInventario != null ? String.Format("{0:N3}", numUnico.NumeroUnicoInventario.InventarioDisponibleCruce) : "0";

                ocultarPaneles();
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected void rbEntrada_CheckedChanged(object sender, EventArgs e)
        {
            ocultarPaneles();
            ddlTipo.Items.Clear();
            ddlTipo.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposMovimiento().Where(x => x.EsEntrada == true && x.EsTransferenciaProcesos == false && x.DisponibleMovimientosUI).OrderBy(x => x.Nombre));
            mostrarCapturaDinamica();
        }

        protected void rbSalida_CheckedChanged(object sender, EventArgs e)
        {
            ocultarPaneles();
            ddlTipo.Items.Clear();
            ddlTipo.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposMovimiento().Where(x => x.EsEntrada == false && x.EsTransferenciaProcesos == false && x.DisponibleMovimientosUI).OrderBy(x => x.Nombre));
            mostrarCapturaDinamica();
        }

        protected void rbNuevo_CheckedChanged(object sender, EventArgs e)
        {
            phSegmento.Visible = false;
            phTxtSegmento.Visible = true;
        }

        protected void rbExistente_CheckedChanged(object sender, EventArgs e)
        {
            phSegmento.Visible = true;
            phTxtSegmento.Visible = false;
            int numeroUnicoID = filtroGenerico.NumeroUnicoSelectedValue.SafeIntParse();

            List<NumeroUnicoSegmento> lstNumUnicoSeg = NumeroUnicoSegmentoBO.Instance
                                                                            .ObtenerPorNumeroUnico(numeroUnicoID)
                                                                            .OrderBy(x => x.Segmento)
                                                                            .ToList();

            ddlSegmento.BindToEnumerableWithEmptyRow(lstNumUnicoSeg, "Segmento", "NumeroUnicoSegmentoID", -1);
        }

        private void mostrarCapturaDinamica()
        {

            if (_tipoMaterialID == (int)TipoMaterialEnum.Accessorio)
            {
                limpiarControles(1);
                pnlAccesorio.Visible = true;
            }

            if (_tipoMaterialID == (int)TipoMaterialEnum.Tubo && _totalSegmentos == 1 && rbSalida.Checked)
            {
                limpiarControles(1);
                pnlAccesorio.Visible = true;
            }

            if (_tipoMaterialID == (int)TipoMaterialEnum.Tubo && rbEntrada.Checked)
            {
                limpiarControles(3);
                pnlEntrada.Visible = true;
            }

            if (_tipoMaterialID == (int)TipoMaterialEnum.Tubo && rbSalida.Checked)
            {
                if (_totalSegmentos == 1)
                {
                    limpiarControles(1);
                    pnlAccesorio.Visible = true;
                }
                else if (_totalSegmentos > 1)
                {
                    limpiarControles(2);
                    pnlSalida.Visible = true;
                    grdSegmentos.ResetBind();
                    grdSegmentos.DataSource = NumeroUnicoSegmentoBO.Instance.ObtenerPorNumeroUnico(_numeroUnicoID);
                    grdSegmentos.DataBind();
                }

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
           

                Validate();
                if (!IsValid) return;
                
            
            try
            {
                NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerParaMovimientoInventarios(_numeroUnicoID);
                NumeroUnicoSegmento numUnicoSeg = null;
                NumeroUnicoMovimiento numUnicoMov = new NumeroUnicoMovimiento();

                numUnico.StartTracking();
                numUnico.NumeroUnicoInventario.StartTracking();

                #region Panel Accesorio(Entrada/Salida) y Tubo Salida (Con un segmento)

                if (pnlAccesorio.Visible == true)
                {
                    if (rbEntrada.Checked)
                    {
                        //Es accesorio entrada
                        numUnicoMov.Cantidad = txtCantidadA.Text.SafeIntParse();
                        numUnicoMov.Segmento = null;
                        numUnicoMov.Referencia = txtReferenciaA.Text;

                        numUnico.NumeroUnicoInventario.InventarioFisico = numUnico.NumeroUnicoInventario.InventarioFisico + txtCantidadA.Text.SafeIntParse();
                        numUnico.NumeroUnicoInventario.InventarioBuenEstado = numUnico.NumeroUnicoInventario.InventarioBuenEstado + txtCantidadA.Text.SafeIntParse();
                        numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                        
                    }
                    else
                    {
                        //Es salida

                        if (numUnico.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                        {
                            if (txtCantidadA.Text.SafeIntParse() > numUnico.NumeroUnicoInventario.InventarioFisico)
                            {

                                throw new BaseValidationException(MensajesErrorWeb.Exception_SalidaMayorAInventarioFisico);
                            }

                            //Es accesorio salida
                            numUnicoMov.Cantidad = txtCantidadA.Text.SafeIntParse();
                            numUnicoMov.Segmento = null;
                            numUnicoMov.Referencia = txtReferenciaA.Text;

                            numUnico.NumeroUnicoInventario.InventarioFisico = numUnico.NumeroUnicoInventario.InventarioFisico - txtCantidadA.Text.SafeIntParse();
                            numUnico.NumeroUnicoInventario.InventarioBuenEstado = numUnico.NumeroUnicoInventario.InventarioBuenEstado - txtCantidadA.Text.SafeIntParse();
                            numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                        }
                        else
                        {
                            //Es un tubo salida con un segmento 
                            if (txtCantidadA.Text.SafeIntParse() > numUnico.NumeroUnicoInventario.InventarioFisico)
                            {

                                throw new BaseValidationException(MensajesErrorWeb.Exception_SalidaMayorAInventarioFisico);
                            }

                            numUnico.NumeroUnicoSegmento[0].StartTracking();
                            numUnico.NumeroUnicoSegmento[0].InventarioFisico = numUnico.NumeroUnicoSegmento[0].InventarioFisico - txtCantidadA.Text.SafeIntParse();
                            numUnico.NumeroUnicoSegmento[0].InventarioBuenEstado = numUnico.NumeroUnicoSegmento[0].InventarioBuenEstado - txtCantidadA.Text.SafeIntParse();
                            numUnico.NumeroUnicoSegmento[0].InventarioDisponibleCruce = numUnico.NumeroUnicoSegmento[0].InventarioBuenEstado - numUnico.NumeroUnicoSegmento[0].InventarioCongelado;
                            numUnico.NumeroUnicoSegmento[0].UsuarioModifica = SessionFacade.UserId;
                            numUnico.NumeroUnicoSegmento[0].FechaModificacion = DateTime.Now;
                            numUnico.NumeroUnicoSegmento[0].StopTracking();

                            numUnicoMov.Cantidad = txtCantidadA.Text.SafeIntParse();
                            numUnicoMov.Segmento = numUnico.NumeroUnicoSegmento[0].Segmento;
                            numUnicoMov.Referencia = txtReferenciaA.Text;

                            numUnico.NumeroUnicoInventario.InventarioFisico = numUnico.NumeroUnicoInventario.InventarioFisico - txtCantidadA.Text.SafeIntParse();
                            numUnico.NumeroUnicoInventario.InventarioBuenEstado = numUnico.NumeroUnicoInventario.InventarioBuenEstado - txtCantidadA.Text.SafeIntParse();
                            numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                        }
                    }
                }

                #endregion

                #region Panel Entrada
                if (pnlEntrada.Visible == true)
                {
                    //Es un tubo entrada con 1 o varios segmentos
                    if (rbNuevo.Checked)
                    {
                        numUnicoMov.Segmento = txtSegmento.Text;
                        NumeroUnicoSegmento segmento = new NumeroUnicoSegmento();
                        segmento.ProyectoID = _proyectoID;
                        segmento.NumeroUnicoID = _numeroUnicoID;
                        segmento.Segmento = txtSegmento.Text;
                        segmento.InventarioFisico = txtCantidadE.Text.SafeIntParse();
                        segmento.InventarioBuenEstado = txtCantidadE.Text.SafeIntParse();
                        segmento.InventarioCongelado = 0;
                        segmento.CantidadDanada = 0;
                        segmento.InventarioDisponibleCruce = segmento.InventarioBuenEstado - segmento.InventarioCongelado;
                        segmento.UsuarioModifica = SessionFacade.UserId;
                        segmento.FechaModificacion = DateTime.Now;

                        //Agregamos ya que es un nuevo segmento
                        if (numUnico.NumeroUnicoSegmento.Any(x => x.Segmento == segmento.Segmento))
                        {
                            throw new BaseValidationException(MensajesErrorWeb.Excepcion_SegmentoExiste);
                        }
                        else
                        {
                            numUnico.NumeroUnicoSegmento.Add(segmento);
                        }
                    }
                    else
                    {
                        numUnicoMov.Segmento = ddlSegmento.SelectedItem.Text;

                        numUnicoSeg = numUnico.NumeroUnicoSegmento.Where(x => x.NumeroUnicoSegmentoID == ddlSegmento.SelectedValue.SafeIntParse()).Single();
                        numUnicoSeg.StartTracking();
                        numUnicoSeg.InventarioFisico = numUnicoSeg.InventarioFisico + txtCantidadE.Text.SafeIntParse();
                        numUnicoSeg.InventarioBuenEstado = numUnicoSeg.InventarioBuenEstado + txtCantidadE.Text.SafeIntParse();
                        numUnicoSeg.InventarioDisponibleCruce = numUnicoSeg.InventarioBuenEstado - numUnicoSeg.InventarioCongelado;
                        numUnicoSeg.UsuarioModifica = SessionFacade.UserId;
                        numUnicoSeg.FechaModificacion = DateTime.Now;
                        numUnicoSeg.StopTracking();
                    }

                    numUnicoMov.Cantidad = txtCantidadE.Text.SafeIntParse();
                    numUnicoMov.Referencia = txtReferenciaE.Text;

                    numUnico.NumeroUnicoInventario.InventarioFisico = numUnico.NumeroUnicoInventario.InventarioFisico + txtCantidadE.Text.SafeIntParse();
                    numUnico.NumeroUnicoInventario.InventarioBuenEstado = numUnico.NumeroUnicoInventario.InventarioBuenEstado + txtCantidadE.Text.SafeIntParse();
                    numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                }
                #endregion

                #region Panel Salida
                if (pnlSalida.Visible == true)
                {
                    //Es un tubo salida mas de un Segmento
                    foreach (GridDataItem item in grdSegmentos.Items)
                    {
                        CheckBox chkSelected = (CheckBox)item["chkSelect_h"].Controls[0];
                        if (chkSelected.Checked)
                        {
                            int idNumUnicoSeg = item.GetDataKeyValue("NumeroUnicoSegmentoID").SafeIntParse();
                            numUnicoSeg = numUnico.NumeroUnicoSegmento.Where(x => x.NumeroUnicoSegmentoID == idNumUnicoSeg).Single();

                            if (txtCantidadS.Text.SafeIntParse() > numUnicoSeg.InventarioFisico)
                            {
                                throw new BaseValidationException(MensajesErrorWeb.Exception_SalidaMayorAInventarioFisico);
                            }

                            numUnicoSeg.StartTracking();
                            numUnicoSeg.InventarioFisico = numUnicoSeg.InventarioFisico - txtCantidadS.Text.SafeIntParse();
                            numUnicoSeg.InventarioBuenEstado = numUnicoSeg.InventarioBuenEstado - txtCantidadS.Text.SafeIntParse();
                            numUnicoSeg.InventarioDisponibleCruce = numUnicoSeg.InventarioBuenEstado - numUnicoSeg.InventarioCongelado;
                            numUnicoSeg.UsuarioModifica = SessionFacade.UserId;
                            numUnicoSeg.FechaModificacion = DateTime.Now;
                            numUnicoSeg.StopTracking();

                            numUnicoMov.Cantidad = txtCantidadS.Text.SafeIntParse();
                            numUnicoMov.Segmento = item["Segmento"].Text;
                            numUnicoMov.Referencia = txtReferenciaS.Text;

                            numUnico.NumeroUnicoInventario.InventarioFisico = numUnico.NumeroUnicoInventario.InventarioFisico - txtCantidadS.Text.SafeIntParse();
                            numUnico.NumeroUnicoInventario.InventarioBuenEstado = numUnico.NumeroUnicoInventario.InventarioBuenEstado - txtCantidadS.Text.SafeIntParse();
                            numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado - numUnico.NumeroUnicoInventario.InventarioCongelado;
                            break;
                        }
                    }
                }
                #endregion

                numUnicoMov.NumeroUnicoID = _numeroUnicoID;
                numUnicoMov.ProyectoID = _proyectoID;
                numUnicoMov.TipoMovimientoID = ddlTipo.SelectedValue.SafeIntParse();
                numUnicoMov.FechaMovimiento = DateTime.Now;
                numUnicoMov.Estatus = "A";
                numUnicoMov.UsuarioModifica = SessionFacade.UserId;
                numUnicoMov.FechaModificacion = DateTime.Now;
                numUnico.NumeroUnicoMovimiento.Add(numUnicoMov);


                numUnico.NumeroUnicoInventario.UsuarioModifica = SessionFacade.UserId;
                numUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                numUnico.NumeroUnicoInventario.StopTracking();

              
                    NumeroUnicoBO.Instance.GuardaMovimientosInventario(numUnico, numUnicoSeg);
                    //ocultarPaneles();

                    UtileriaRedireccion.RedireccionaExitoProduccion(MensajesProduccion.TituloMovimientosInventario, MensajesProduccion.DetalleMovimiento,
                                                                    new List<LigaMensaje>()
                                                                 {
                                                                    new LigaMensaje
                                                                    {
                                                                        Texto = MensajesProduccion.NuevoMovimiento, 
                                                                        Url = WebConstants.MaterialesUrl.MovimientoInventario
                                                                    },

                                                                    new LigaMensaje
                                                                    {
                                                                        Texto = MensajesProduccion.ConsultaNumerosUnicos,

                                                                        Url = WebConstants.MaterialesUrl.LST_NUMEROSUNICOS
                                                                    }
                                                                });
                }
            
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgMovimientos");
            }
            
        }

        protected void cvSelectedRow_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = false;

            foreach (GridDataItem item in grdSegmentos.Items)
            {
                CheckBox chkSelected = (CheckBox)item["chkSelect_h"].Controls[0];
                if (chkSelected.Checked)
                {
                    e.IsValid = true;
                }
            }
            
        }

        protected void cvSegmento_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = false;

            if (rbNuevo.Checked)
            {
                if (txtSegmento.Text != null)
                {
                    e.IsValid = true;
                }
                
            }
            else
            {
                if (ddlSegmento.SelectedValue.SafeIntParse() > 0)
                {
                    e.IsValid = true;
                }
            }
        }

        //protected void cvCantidades_ServerValidate(object sender, ServerValidateEventArgs e)
        //{
        //}

        private void ocultarPaneles()
        {
            pnlAccesorio.Visible = false;
            pnlEntrada.Visible = false;
            pnlSalida.Visible = false;           
        }

        private void limpiarControles(int tipoPanel)
        {
            switch (tipoPanel)
            {
                case 1:
                    txtCantidadA.Text = string.Empty;
                    txtReferenciaA.Text = string.Empty;
                    break;
                case 2:
                    txtSegmento.Text = string.Empty;
                    txtCantidadS.Text = string.Empty;
                    txtReferenciaS.Text = string.Empty;
                    break;
                case 3:
                    txtCantidadE.Text = string.Empty;
                    txtReferenciaE.Text = string.Empty;
                    break;
                default:
                    break;
            }
        }



        private List<TipoMovimientoCache> TiposMovimiento
        {
            get
            {
                return ViewState["TiposMovimiento"] as List<TipoMovimientoCache>;
            }
            set
            {
                ViewState["TiposMovimiento"] = value;
            }
        }


        private int saldo;
        private int saldoTotalTubo = 0;
        private int entradaGeneralTubo = 0;
        private int salidaGeneralTubo = 0;
        private string segmento;
        private int contadorSalida = 0;
        private int contadorEntrada = 0;
        private int recibidoInicial = 0;

        private List<NumeroUnicoSegmento> saldoSegmentos;
        private List<Simple> entradaSegmentos;
        private List<Simple> salidaSegmentos;
        private List<Simple> salidaTemporalSegmentos;

        /// <summary>
        /// Carga los datos del numero unico en los campos correspondientes
        /// </summary>
        /// <param name="numUnico"></param>
        private void cargaDatos(NumeroUnico numUnico)
        {
            lblItemCode.Text = numUnico.ItemCodeID.HasValue ? numUnico.ItemCode.Codigo + " - " + numUnico.ItemCode.DescripcionEspanol : string.Empty;
            
            if (numUnico.NumeroUnicoInventario != null)
            {

                NumeroUnico movimientos = NumeroUnicoBO.Instance.ObtenerMovimientosInventarios(numUnico.NumeroUnicoID);

                if (numUnico.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                {
                    cargaAccesorio(movimientos);
                    phTubo.Visible = false;
                    phAccesorio.Visible = true;
                }
                else
                {


                    List<int> salidas = TipoMovimientoBO.Instance.ObtenerSalidasPorSegmento();
                    List<int> salidasGenerales = TipoMovimientoBO.Instance.ObtenerSalidasGenerales();
                    salidaSegmentos = NumeroUnicoMovimientoBO.Instance.ObtenerSalidasPorSegmento(numUnico.NumeroUnicoID, salidas);
                    salidaTemporalSegmentos = NumeroUnicoMovimientoBO.Instance.ObtenerSalidasTemporalesPorSegmento(numUnico.NumeroUnicoID);
                    salidaGeneralTubo = NumeroUnicoMovimientoBO.Instance.ObtenerSalidasGenerales(numUnico.NumeroUnicoID, salidasGenerales);
                    saldoSegmentos = NumeroUnicoSegmentoBO.Instance.ObtenerPorNumeroUnico(numUnico.NumeroUnicoID);
                    saldoTotalTubo = NumeroUnicoInventarioBO.Instance.ObtenerSaldoPorNumeroUnico(numUnico.NumeroUnicoID);
                    entradaSegmentos = NumeroUnicoMovimientoBO.Instance.ObtenerEntradasPorSegmento(numUnico.NumeroUnicoID);
                    entradaGeneralTubo = NumeroUnicoMovimientoBO.Instance.ObtenerEntradasGenerales(numUnico.NumeroUnicoID);
                    recibidoInicial = numUnico.NumeroUnicoInventario.CantidadRecibida;
                    cargaTubo(movimientos);
                    phAccesorio.Visible = false;
                    phTubo.Visible = true;
               

                }
            }
        }

        private void cargaAccesorio(NumeroUnico movimientos)
        {
            repAccesorio.DataSource = movimientos.NumeroUnicoMovimiento.Where(x => x.Estatus == EstatusNumeroUnicoMovimiento.ACTIVO).OrderBy(x => x.Segmento).ThenBy(x => x.FechaMovimiento);
            repAccesorio.DataBind();
        }

        private void cargaTubo(NumeroUnico movimientos)
        {
            repTubo.DataSource = movimientos.NumeroUnicoMovimiento.Where(x => x.Estatus == EstatusNumeroUnicoMovimiento.ACTIVO).OrderBy(x => x.Segmento).ThenBy(x => x.FechaMovimiento);
            repTubo.DataBind();

            repSegmentos.DataSource = movimientos.NumeroUnicoSegmento.OrderBy(x => x.Segmento);
            repSegmentos.DataBind();
        }

        protected void repTubo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                NumeroUnicoMovimiento item = e.Item.DataItem as NumeroUnicoMovimiento;
                Literal litMov = e.Item.FindControl("litMov") as Literal;
                litMov.Text = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? item.TipoMovimiento.NombreIngles : item.TipoMovimiento.Nombre;
                Literal ltSaldo = e.Item.FindControl("ltSaldo") as Literal;
                ImageButton btnEliminarMovimiento = e.Item.FindControl("btnEliminarMovimiento") as ImageButton;
                HiddenField hdnNumMovimientoID = e.Item.FindControl("hdnNumMovimientoID") as HiddenField;
                hdnNumMovimientoID.Value = item.NumeroUnicoMovimientoID.ToString();

                btnEliminarMovimiento.Visible = NumeroUnicoBL.Instance.EsTipoMovimientoEliminable(item.TipoMovimientoID);
                if (TiposMovimiento.Where(x => x.ID == item.TipoMovimientoID).Select(x => x.EsEntrada).Single())
                {
                    Literal ltEntrada = e.Item.FindControl("ltEntrada") as Literal;
                    ltEntrada.Text = item.Cantidad.ToString();

                    if (item.Segmento != segmento)
                    {
                        saldo = item.Cantidad;
                        segmento = item.Segmento;
                    }
                    else
                    {
                        saldo = saldo + item.Cantidad;
                    }

                    if (item.TipoMovimientoID == (int)TipoMovimientoEnum.EntradaPintura)
                    {
                        ltEntrada.Text = ltEntrada.Text + " **";
                    }
                }
                else
                {
                    Literal ltSalida = e.Item.FindControl("ltSalida") as Literal;
                    ltSalida.Text = String.Format("{0:N3}", item.Cantidad);
                    saldo = saldo - item.Cantidad;

                    if (item.TipoMovimientoID == (int)TipoMovimientoEnum.SalidaPintura)
                    {
                        ltSalida.Text = ltSalida.Text + " *";
                    }
                }

                if (!TiposMovimiento.Where(x => x.ID == item.TipoMovimientoID).Select(x => x.ApareceEnSaldos).SingleOrDefault())
                {
                    e.Item.Visible = false;
                }


                ltSaldo.Text = String.Format("{0:N3}", saldo);
            }
        }

        protected void repSegmentos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                NumeroUnicoSegmento item = e.Item.DataItem as NumeroUnicoSegmento;
                Literal litRecibido = e.Item.FindControl("litRecibido") as Literal;
                Literal litTotalSalidas = e.Item.FindControl("litTotalSalidas") as Literal;
                Literal litTotalSalidasTemporales = e.Item.FindControl("litTotalSalidasTemporales") as Literal;
                Literal litTotalEntradas = e.Item.FindControl("litTotalEntradas") as Literal;
                Literal litTotalSaldos = e.Item.FindControl("litTotalSaldos") as Literal;

                //if (item.InventarioDisponibleCruce < 0)
                //{
                //    litDisponible.Text = "0";
                //}
                //else
                //{
                //    litDisponible.Text = item.InventarioDisponibleCruce.ToString();
                //}

                if (item.Segmento == "A")
                {
                    litRecibido.Text = String.Format("{0:N0}", recibidoInicial);
                }
                else
                {
                    litRecibido.Text = "0";
                }

                if (salidaSegmentos.Count == 0)
                {
                    litTotalSalidas.Text = "0";
                }
                else
                {
                    Simple salida = salidaSegmentos.Where(x => x.Valor == item.Segmento).FirstOrDefault();
                    litTotalSalidas.Text = salida != null ? String.Format("{0:N0}", salida.ID) : "0";
                }

                if (salidaTemporalSegmentos.Count == 0)
                {
                    litTotalSalidasTemporales.Text = "0";
                }
                else
                {
                    Simple salidaTemp = salidaTemporalSegmentos.Where(x => x.Valor == item.Segmento).FirstOrDefault();
                    litTotalSalidasTemporales.Text = salidaTemp != null ? String.Format("{0:N0}", salidaTemp.ID) : "0";
                }

                if (entradaSegmentos.Count == 0)
                {
                    litTotalEntradas.Text = "0";
                }
                else
                {
                    Simple entrada = entradaSegmentos.Where(x => x.Valor == item.Segmento).FirstOrDefault();
                    litTotalEntradas.Text = entrada != null ? String.Format("{0:N0}", entrada.ID) : "0";
                }

                if (saldoSegmentos.Count == 0)
                {
                    litTotalSaldos.Text = "0";
                }
                else
                {
                    NumeroUnicoSegmento saldo = saldoSegmentos.Where(x => x.Segmento == item.Segmento).FirstOrDefault();
                    litTotalSaldos.Text = saldo != null ? String.Format("{0:N0}", saldo.InventarioBuenEstado) : "0";
                }
            }
        }

        protected void repAccesorio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                NumeroUnicoMovimiento item = e.Item.DataItem as NumeroUnicoMovimiento;
                Literal litMov = e.Item.FindControl("litMov") as Literal;
                litMov.Text = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? item.TipoMovimiento.NombreIngles : item.TipoMovimiento.Nombre;
                Literal ltSaldo = e.Item.FindControl("ltSaldo") as Literal;
                ImageButton btnEliminarMovimiento = e.Item.FindControl("btnEliminarMovimiento") as ImageButton;
                HiddenField hdnNumMovimientoID = e.Item.FindControl("hdnNumMovimientoID") as HiddenField;
                hdnNumMovimientoID.Value = item.NumeroUnicoMovimientoID.ToString();

                btnEliminarMovimiento.Visible = NumeroUnicoBL.Instance.EsTipoMovimientoEliminable(item.TipoMovimientoID);
                if (CacheCatalogos.Instance.ObtenerTiposMovimiento().Where(x => x.ID == item.TipoMovimientoID).Select(x => x.EsEntrada).Single())
                {
                    Literal ltEntrada = e.Item.FindControl("ltEntrada") as Literal;
                    ltEntrada.Text = item.Cantidad.ToString();
                    contadorEntrada += item.Cantidad;

                    if (item.TipoMovimientoID == (int)TipoMovimientoEnum.Recepcion)
                    {
                        saldo = item.Cantidad;
                    }
                    else
                    {
                        saldo = saldo + item.Cantidad;
                    }

                    if (item.TipoMovimientoID == (int)TipoMovimientoEnum.EntradaPintura)
                    {
                        ltEntrada.Text = ltEntrada.Text + " **";
                    }



                }
                else
                {
                    Literal ltSalida = e.Item.FindControl("ltSalida") as Literal;
                    contadorSalida += item.Cantidad;
                    ltSalida.Text = item.Cantidad.ToString();
                    saldo = saldo - item.Cantidad;

                    if (item.TipoMovimientoID == (int)TipoMovimientoEnum.SalidaPintura)
                    {
                        ltSalida.Text = ltSalida.Text + " *";
                    }

                }

                ltSaldo.Text = saldo.ToString();
            }
        }

        protected void btnEliminarMovimiento_OnClick(object sender, EventArgs args)
        {
            try
            {
                int numMovimientoID = int.Parse(((sender as ImageButton).Parent.FindControl("hdnNumMovimientoID") as HiddenField).Value);

                if (NumeroUnicoBL.Instance.EsMovimientoEliminable(numMovimientoID))
                {
                    NumeroUnicoBL.Instance.EliminaMovimientoInventario(numMovimientoID, SessionFacade.UserId);
                    btnMostrar_Click(sender, args);
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}
