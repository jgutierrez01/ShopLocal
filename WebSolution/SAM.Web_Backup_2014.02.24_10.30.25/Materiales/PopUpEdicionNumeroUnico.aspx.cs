using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.BusinessObjects.Materiales;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Common;
using System.Threading;

namespace SAM.Web.Materiales
{
    public partial class PopUpEdicionNumeroUnico : SamPaginaPopup
    {
        private string Permiso = Thread.CurrentThread.CurrentUICulture.Name == LanguageHelper.INGLES ? "Materials Edition" : "Edición de Materiales";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoANumeroUnico(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un número único {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargaDatos();

                //Comportamiento default denuestras ventanas
                wndMgr.Windows[0].Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize | Telerik.Web.UI.WindowBehaviors.Reload;
            }
        }

        #region Loads / Binds

        private void cargaDatos()
        {
            NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConInventarioColadaICProfile(EntityID.Value);
            bool puedeEditar = NumeroUnicoBO.Instance.PuedeModificarDatosBase(numUnico.NumeroUnicoID);

            bool puedeEditarConPermiso = SeguridadWeb.UsuarioPuedeEditar(Permiso);

            VersionRegistro = numUnico.VersionRegistro;

            ctrlGeneral.CargaInformacion(numUnico, null, puedeEditar, puedeEditarConPermiso);
            ctrlProveedor.CargaInformacion(numUnico, null);
            ctrlAdicional.CargaInformacion(numUnico, null);
            ctrlAdicional.NumUnico = numUnico;
            if (numUnico.NumeroUnicoInventario != null)
            {
                int inventarioCongelado = numUnico.NumeroUnicoInventario.InventarioCongelado;
                if (inventarioCongelado > 0)
                {
                    NumeroUnico numUnicoTransferir = NumeroUnicoBO.Instance.ObtenerNumeroUnicoParaTransferirCongelado(numUnico);
                    bool puedeTransferirInventarioCongelado = numUnicoTransferir == null ? false : true;
                    hdnPuedeTransferir.Value = puedeTransferirInventarioCongelado.SafeStringParse();
                    hdnEstatusOriginal.Value = numUnico.Estatus;
                    btnGuardar.OnClientClick = "return Sam.Materiales.CambioEstatusNumeroUnico()";
                }
            }
            lblAdvertencia.Visible = ctrlGeneral.Advertencia;
        }

        /// <summary>
        /// Mapea la información capturada en las entidades correspondientes.
        /// </summary>
        /// <param name="numUnico"></param>
        private void unMap(NumeroUnico numUnico)
        {
            int diferenciaRecepcion = 0;

            // NumeroUnico
            numUnico.ItemCodeID = ctrlGeneral.ItemCodeID;
            numUnico.NumeroUnicoCliente = ctrlGeneral.NumeroUnicoCliente;
            numUnico.ColadaID = ctrlGeneral.ColadaID;
            numUnico.ProveedorID = ctrlProveedor.Proveedor;
            numUnico.FabricanteID = ctrlProveedor.Fabricante;
            numUnico.TipoCorte1ID = ctrlGeneral.Profile1;
            numUnico.TipoCorte2ID = ctrlGeneral.Profile2;
            numUnico.Estatus = ctrlGeneral.Estatus;
            numUnico.Factura = ctrlProveedor.Factura;
            numUnico.PartidaFactura = ctrlProveedor.PartidaFactura;
            numUnico.OrdenDeCompra = ctrlProveedor.OrdenCompra;
            numUnico.PartidaOrdenDeCompra = ctrlProveedor.PartidaOrden;
            numUnico.Diametro1 = ctrlGeneral.Diametro1;
            numUnico.Diametro2 = ctrlGeneral.Diametro2;
            numUnico.Cedula = ctrlGeneral.Cedula;
            numUnico.MarcadoAsme = ctrlAdicional.MarcadoAsme;
            numUnico.MarcadoGolpe = ctrlAdicional.MarcadoGolpe;
            numUnico.MarcadoPintura = ctrlAdicional.MarcadoPintura;
            numUnico.PruebasHidrostaticas = ctrlAdicional.PruebasHidrostaticas;
            numUnico.TieneDano = ctrlGeneral.Danada;
            numUnico.Observaciones = ctrlGeneral.Observaciones;
            #region Campos libre recepción

            if (ctrlAdicional.camposRecepcion[0].Visible)
            {
                numUnico.CampoLibreRecepcion1 = ctrlAdicional.camposRecepcion[0].Text;
            }

            if (ctrlAdicional.camposRecepcion[1].Visible)
            {
                numUnico.CampoLibreRecepcion2 = ctrlAdicional.camposRecepcion[1].Text;
            }

            if (ctrlAdicional.camposRecepcion[2].Visible)
            {
                numUnico.CampoLibreRecepcion3 = ctrlAdicional.camposRecepcion[2].Text;
            }

            if (ctrlAdicional.camposRecepcion[3].Visible)
            {
                numUnico.CampoLibreRecepcion4 = ctrlAdicional.camposRecepcion[3].Text;
            }

            if (ctrlAdicional.camposRecepcion[4].Visible)
            {
                numUnico.CampoLibreRecepcion5 = ctrlAdicional.camposRecepcion[4].Text;
            }

            #endregion
            #region Campos libre número único

            if (ctrlAdicional.camposNumeroUnico[0].Visible)
            {
                numUnico.CampoLibre1 = ctrlAdicional.camposNumeroUnico[0].Text;
            }

            if (ctrlAdicional.camposNumeroUnico[1].Visible)
            {
                numUnico.CampoLibre2 = ctrlAdicional.camposNumeroUnico[1].Text;
            }

            if (ctrlAdicional.camposNumeroUnico[2].Visible)
            {
                numUnico.CampoLibre3 = ctrlAdicional.camposNumeroUnico[2].Text;
            }

            if (ctrlAdicional.camposNumeroUnico[3].Visible)
            {
                numUnico.CampoLibre4 = ctrlAdicional.camposNumeroUnico[3].Text;
            }

            if (ctrlAdicional.camposNumeroUnico[4].Visible)
            {
                numUnico.CampoLibre5 = ctrlAdicional.camposNumeroUnico[4].Text;
            }

            #endregion
            numUnico.UsuarioModifica = SessionFacade.UserId;
            numUnico.FechaModificacion = DateTime.Now;

            ItemCode ic = ItemCodeBO.Instance.ObtenerConTipoMaterial(numUnico.ItemCodeID.Value);
            bool esTubo = ic.TipoMaterialID == (int)TipoMaterialEnum.Tubo;

            //NumeroUnicoInventario
            NumeroUnicoInventario nui;
            if (numUnico.NumeroUnicoInventario == null)
            {
                nui = new NumeroUnicoInventario();
                numUnico.NumeroUnicoInventario = nui;
            }
            else
            {
                nui = numUnico.NumeroUnicoInventario;
            }

            //En teoría si el número único es "no editable" las cantidades no cambian
            bool cantidadesCambiaron = ctrlGeneral.Cantidad != nui.CantidadRecibida || ctrlGeneral.CantidadDanada != nui.CantidadDanada;

            if (cantidadesCambiaron)
            {
                //Calculamos la diferencia de las cantidades de recepciones, si el numero es negativo entonces
                //se esta disminuyendo la cantidad recibida por lo qu ehay que restarlo al inventario actual
                //si es positivo entonces se le esta aumentando por lo que hay que sumarlo al inventario actual
                diferenciaRecepcion = ctrlGeneral.Cantidad - nui.CantidadRecibida;

                nui.StartTracking();
                nui.ProyectoID = numUnico.ProyectoID;
                nui.CantidadRecibida = ctrlGeneral.Cantidad;
                nui.CantidadDanada = ctrlGeneral.CantidadDanada;
                nui.InventarioFisico = nui.InventarioFisico + diferenciaRecepcion;
                nui.InventarioBuenEstado = nui.InventarioFisico - ctrlGeneral.CantidadDanada;
                //Ahora al poder modificar las cantidades el congelado se debe de respetar
                //nui.InventarioCongelado = 0;
                nui.InventarioDisponibleCruce = nui.InventarioBuenEstado - nui.InventarioCongelado;
                nui.UsuarioModifica = SessionFacade.UserId;
                nui.FechaModificacion = DateTime.Now;
            }

            //NumeroUnicoMovimiento - Se crea el movimiento tipo recepcion solo en caso que aun no haya movimientos
            NumeroUnicoMovimiento mov;

            if (numUnico.NumeroUnicoMovimiento.Count == 0)
            {
                mov = new NumeroUnicoMovimiento();
                mov.FechaMovimiento = DateTime.Now;
                numUnico.NumeroUnicoMovimiento.Add(mov);
            }
            else
            {
                mov = numUnico.NumeroUnicoMovimiento
                              .Where(x => x.TipoMovimientoID == (int)TipoMovimientoEnum.Recepcion)
                              .FirstOrDefault();
            }

            if (cantidadesCambiaron && mov != null)
            {
                mov.StartTracking();
                mov.ProyectoID = numUnico.ProyectoID;
                mov.TipoMovimientoID = (int)TipoMovimientoEnum.Recepcion;
                mov.Cantidad = ctrlGeneral.Cantidad - ctrlGeneral.CantidadDanada;
                mov.Referencia = string.Format("Recepcion por RecepcionID = {0}", numUnico.RecepcionNumeroUnico[0].RecepcionID);
                mov.Estatus = "A";
                //La fecha del movimiento ahora no debe cambiar
                //mov.FechaMovimiento = DateTime.Now;
                mov.UsuarioModifica = SessionFacade.UserId;
                mov.FechaModificacion = DateTime.Now;

                if (esTubo)
                {
                    mov.Segmento = "A";
                }

                numUnico.RecepcionNumeroUnico[0].StartTracking();
                numUnico.RecepcionNumeroUnico[0].NumeroUnicoMovimiento = mov;
                numUnico.RecepcionNumeroUnico[0].UsuarioModifica = SessionFacade.UserId;
                numUnico.RecepcionNumeroUnico[0].FechaModificacion = DateTime.Now;
            }

            //Si el tipo de material es Tubo
            if (esTubo)
            {
                if (cantidadesCambiaron)
                {
                    NumeroUnicoSegmento nus;

                    //NumeroUnicoSegmento
                    if (numUnico.NumeroUnicoSegmento.Count == 0)
                    {
                        nus = new NumeroUnicoSegmento();
                        numUnico.NumeroUnicoSegmento.Add(nus);
                    }
                    else
                    {
                        nus = numUnico.NumeroUnicoSegmento
                                      .Where(x => x.Segmento == "A")
                                      .SingleOrDefault();
                    }

                    if (nus != null)
                    {

                        nus.StartTracking();
                        nus.ProyectoID = numUnico.ProyectoID;
                        nus.Segmento = "A";
                        nus.InventarioFisico = nus.InventarioFisico + diferenciaRecepcion;
                        nus.InventarioBuenEstado = nus.InventarioFisico - ctrlGeneral.CantidadDanada;
                        //Ahora al poder modificar las cantidades el congelado se debe de respetar
                        //nui.InventarioCongelado = 0;
                        nus.InventarioDisponibleCruce = nus.InventarioBuenEstado - nus.InventarioCongelado;
                        nus.CantidadDanada = ctrlGeneral.CantidadDanada;
                        nus.UsuarioModifica = SessionFacade.UserId;
                        nus.FechaModificacion = DateTime.Now;
                    }
                }

                List<NumeroUnicoSegmento> nusDb = numUnico.NumeroUnicoSegmento.ToList();
                List<SimpleString> nusUi = ctrlAdicional.Segmentos;
                SimpleString elemento = null;

                if (nusUi != null)
                {
                    foreach (NumeroUnicoSegmento nus in nusDb)
                    {
                        elemento = nusUi.Where(x => x.Valor.EqualsIgnoreCase(nus.Segmento)).FirstOrDefault();

                        if (elemento != null && elemento.ID != null)
                        {
                            if (nus.Rack == string.Empty || nus.Rack != elemento.ID.ToString())
                            {
                                nus.StartTracking();
                                nus.Rack = elemento.ID;
                                nus.UsuarioModifica = SessionFacade.UserId;
                                nus.FechaModificacion = DateTime.Now;
                            }
                        }
                    }
                }
            }
            else
            {
                numUnico.Rack = ctrlAdicional.Rack;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Guarda la información del número único y limpia los campos para el siguiente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                try
                {
                    NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConInventarios(EntityID.Value);
                    string estatus = numUnico.Estatus;
                    if (ctrlGeneral.Estatus != "A" && estatus != ctrlGeneral.Estatus)
                    {
                        //Validamos que el N.U. no tenga despachos
                        NumeroUnicoBO.Instance.ValidaNoTengaDespacho(ctrlGeneral.CantidadDespachada);

                        int inventarioCongelado = ctrlGeneral.CantidadCongelada;
                        if (inventarioCongelado > 0)
                        {
                            NumeroUnico numeroUnicoTransferir = NumeroUnicoBO.Instance.ObtenerNumeroUnicoParaTransferirCongelado(numUnico);

                            NumeroUnicoBO.Instance.ValidaNoTengaCongelados(numeroUnicoTransferir, numUnico);

                            NumeroUnicoBO.Instance.TransferirMaterialCongelado(numUnico, numeroUnicoTransferir, SessionFacade.UserId, DateTime.Now);
                        }
                    }

                    
                    numUnico.VersionRegistro = VersionRegistro;

                    numUnico.StartTracking();
                    unMap(numUnico);
                    numUnico.StopTracking();
                    NumeroUnicoBO.Instance.Guarda(numUnico, SessionFacade.UserId);

                    phEdicion.Visible = false;
                    pnlMensaje.Visible = true;
                    JsUtils.RegistraScriptActualizaGridNumeroUnico(this);

                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        #endregion

        protected void DdlCedulaOrIC_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cedula = ctrlGeneral.Cedula;
            string descripcionIC = ctrlGeneral.ItemCodeDescripcion;

            if (cedula != string.Empty && descripcionIC != string.Empty && !descripcionIC.Contains(cedula))
            {
                btnGuardar.Attributes["OnClick"] = "return Sam.Confirma(20);";
            }
            else
            {
                btnGuardar.Attributes["OnClick"] = string.Empty;
            }
        }
    }
}