using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessLogic.Materiales;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Materiales
{
    public partial class AltaNumeroUnico : SamPaginaPrincipal
    {
        #region Propiedades

        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }
        private int RecepcionID
        {
            get
            {
                return (int)ViewState["RecepcionID"];
            }
            set
            {
                ViewState["RecepcionID"] = value;
            }
        }
        private int NumeroUnicoActual
        {
            get
            {
                return (int)ViewState["NumeroUnicoActual"];
            }
            set
            {
                ViewState["NumeroUnicoActual"] = value;
            }
        }
        private int NumeroUnicoTotal
        {
            get
            {
                return (int)ViewState["NumeroUnicoTotal"];
            }
            set
            {
                ViewState["NumeroUnicoTotal"] = value;
            }
        }

        #endregion

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

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.mat_NumUnicos);
                NumeroUnicoTotal = Request.QueryString["NT"].ToString().SafeIntParse();
                NumeroUnicoActual = Request.QueryString["NA"].ToString().SafeIntParse();
                ProyectoID = Request.QueryString["PID"].ToString().SafeIntParse();
                int mantenerDatos = Request.QueryString["MD"].ToString().SafeIntParse();
                cargaDatos(mantenerDatos);

                string materiales = Request.QueryString["MSID"].ToString();
                int numeroUnicoID = Request.QueryString["NID"].ToString().SafeIntParse();

                if (!String.IsNullOrEmpty(materiales))
                {
                    ClientScript.RegisterStartupScript(typeof(AltaNumeroUnico), "NumUnicoPopup", "$(function () { Sam.Materiales.AbrePopupOdtReqMaterial(" + numeroUnicoID + ", '" + materiales + "' ); });", true);
                }
            }
        }

        #region Loads / Binds

        private void cargaDatos(int mantenerDatos)
        {
            proyEncabezado.BindInfo(ProyectoID);
            NumeroUnico numAnterior = null;

            if (mantenerDatos > 0)
            {
                numAnterior = NumeroUnicoBO.Instance.ObtenerConInventarioColadaICProfile(EntityID.Value - 1);
            }

            NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConRecepcion(EntityID.Value);

            VersionRegistro = numUnico.VersionRegistro;

            ctrlGeneral.NumeroUnicoActual = NumeroUnicoActual;
            ctrlGeneral.NumeroUnicoTotal = NumeroUnicoTotal;
            ctrlGeneral.CargaInformacion(numUnico, numAnterior, true, true);
            ctrlProveedor.CargaInformacion(numUnico, numAnterior);
            ctrlAdicional.CargaInformacion(numUnico, numAnterior);
            RecepcionID = numUnico.RecepcionNumeroUnico[0].RecepcionID;
            ctrlAdicional.RecepcionID = RecepcionID;

            btnRecepcion.NavigateUrl = string.Format(WebConstants.MaterialesUrl.DetalleRecepcion, RecepcionID);
            lblCantidadNumUnicosGenerados.Text = NumeroUnicoTotal.ToString();
            btnGenerarNumUnico.NavigateUrl = string.Format("javascript:Sam.Materiales.AbrePopupAgregaNumUnicos('{0}');", RecepcionID);
            btnEtiquetas.NavigateUrl = string.Format("~/Materiales/EtiquetasPDF.aspx?ID={0}", RecepcionID);
        }

        /// <summary>
        /// Mapea la información capturada en las entidades correspondientes.
        /// </summary>
        /// <param name="numUnico"></param>
        private void unMap(NumeroUnico numUnico)
        {
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
            numUnico.NumeroUnicoInventario = new NumeroUnicoInventario();
            numUnico.NumeroUnicoInventario.ProyectoID = numUnico.ProyectoID;
            numUnico.NumeroUnicoInventario.CantidadRecibida = ctrlGeneral.Cantidad;
            numUnico.NumeroUnicoInventario.CantidadDanada = ctrlGeneral.CantidadDanada;
            numUnico.NumeroUnicoInventario.InventarioFisico = ctrlGeneral.Cantidad;
            numUnico.NumeroUnicoInventario.InventarioBuenEstado = ctrlGeneral.Cantidad - ctrlGeneral.CantidadDanada;
            numUnico.NumeroUnicoInventario.InventarioCongelado = 0;
            numUnico.NumeroUnicoInventario.InventarioTransferenciaCorte = 0;
            numUnico.NumeroUnicoInventario.InventarioDisponibleCruce = numUnico.NumeroUnicoInventario.InventarioBuenEstado;
            numUnico.NumeroUnicoInventario.UsuarioModifica = SessionFacade.UserId;
            numUnico.NumeroUnicoInventario.FechaModificacion = DateTime.Now;

            //NumeroUnicoMovimiento - Se crea el movimiento tipo recepcion
            NumeroUnicoMovimiento mov = new NumeroUnicoMovimiento();
            mov.ProyectoID = numUnico.ProyectoID;
            mov.TipoMovimientoID = TipoMovimientoEnum.Recepcion.SafeIntParse();
            mov.Cantidad = ctrlGeneral.Cantidad - ctrlGeneral.CantidadDanada;
            mov.Referencia = String.Format("Recepcion por RecepcionID = {0}", numUnico.RecepcionNumeroUnico[0].RecepcionID);
            mov.Estatus = "A";
            mov.FechaMovimiento = DateTime.Now;
            mov.UsuarioModifica = SessionFacade.UserId;
            mov.FechaModificacion = DateTime.Now;

            if (esTubo)
            {
                mov.Segmento = "A";
            }

            numUnico.NumeroUnicoMovimiento.Add(mov);

            numUnico.RecepcionNumeroUnico[0].StartTracking();
            numUnico.RecepcionNumeroUnico[0].NumeroUnicoMovimiento = mov;
            numUnico.RecepcionNumeroUnico[0].UsuarioModifica = SessionFacade.UserId;
            numUnico.RecepcionNumeroUnico[0].FechaModificacion = DateTime.Now;
            numUnico.RecepcionNumeroUnico[0].StopTracking();


            //Si el tipo de material es Tubo
            if (esTubo)
            {
                NumeroUnicoSegmento nus = new NumeroUnicoSegmento();

                //NumeroUnicoSegmento
                nus.ProyectoID = numUnico.ProyectoID;
                nus.Segmento = "A";
                nus.InventarioFisico = ctrlGeneral.Cantidad;
                nus.CantidadDanada = ctrlGeneral.CantidadDanada;
                nus.InventarioBuenEstado = ctrlGeneral.Cantidad - ctrlGeneral.CantidadDanada;
                nus.InventarioCongelado = 0;
                nus.InventarioDisponibleCruce = nus.InventarioBuenEstado;
                nus.InventarioTransferenciaCorte = 0;
                nus.UsuarioModifica = SessionFacade.UserId;
                nus.FechaModificacion = DateTime.Now;
                nus.Rack = ctrlAdicional.Rack;
                numUnico.NumeroUnicoSegmento.Add(nus);
            }
            else
            {
                numUnico.Rack = ctrlAdicional.Rack;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Guarda la información del número único y mantiene los datos para el siguiente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarMantener_OnClick(object sender, EventArgs e)
        {
            int[] matSpool = null;
            int numeroUnicoID;
            string materiales = string.Empty;
            if (guardaNumeroUnico(out matSpool, out numeroUnicoID))
            {

                //Si es el último número unico ir a Paso 2, si no regresar con siguiente.
                if (NumeroUnicoTotal == NumeroUnicoActual)
                {
                    pnlAlta.Visible = false;
                    pnlAcciones.Visible = true;

                    if (matSpool != null)
                    {
                        materiales = string.Join(",", matSpool.Select(x => x.ToString()).ToArray());
                        ClientScript.RegisterStartupScript(typeof(AltaNumeroUnico), "NumUnicoPopup", "$(function () { Sam.Materiales.AbrePopupOdtReqMaterial(" + numeroUnicoID + ", '" + materiales + "' ); });", true);
                    }
                }
                else
                {
                    if (matSpool != null)
                    {
                        materiales = string.Join(",", matSpool.Select(x => x.ToString()).ToArray());
                    }

                    Response.Redirect(string.Format(WebConstants.MaterialesUrl.AltaNumeroUnico, EntityID.Value + 1, NumeroUnicoTotal, NumeroUnicoActual + 1, ProyectoID, 1, numeroUnicoID, materiales));
                }


            }
        }

        /// <summary>
        /// Guarda la información del número único y limpia los campos para el siguiente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            int[] matSpool = null;
            int numeroUnicoID;
            string materiales = string.Empty;
            if (guardaNumeroUnico(out matSpool, out numeroUnicoID))
            {
                //Si es el último número unico ir a Paso 2, si no regresar con siguiente.
                if (NumeroUnicoTotal == NumeroUnicoActual)
                {
                    pnlAlta.Visible = false;
                    pnlAcciones.Visible = true;

                    if (matSpool != null)
                    {
                        materiales = string.Join(",", matSpool.Select(x => x.ToString()).ToArray());
                        ClientScript.RegisterStartupScript(typeof(AltaNumeroUnico), "NumUnicoPopup", "$(function () { Sam.Materiales.AbrePopupOdtReqMaterial(" + numeroUnicoID + ", '" + materiales + "' ); });", true);
                    }
                }
                else
                {
                    limpiaDatos();
                    
                    if (matSpool != null)
                    {
                        materiales = string.Join(",", matSpool.Select(x => x.ToString()).ToArray());
                    }

                    Response.Redirect(string.Format(WebConstants.MaterialesUrl.AltaNumeroUnico, EntityID.Value + 1, NumeroUnicoTotal, NumeroUnicoActual + 1, ProyectoID, 0, numeroUnicoID, materiales));
                }

            }
        }

        /// <summary>
        /// Evento lanzado al generar más números unicos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRedirectAltaNumUnicos_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format(WebConstants.MaterialesUrl.AltaNumeroUnico, hdnNumeroUnicoID.Value.SafeIntParse(), hdnCantidadNumUnicos.Value, 1, ProyectoID, 0, - 1, string.Empty));
        }

        //Evento lanzado por el dropdown de cedula del control de informacion general para verificar si el nombre de la 
        //cedula está contenido dentro de la descripcion de item code
        protected void DdlCedulaOrIC_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cedula = ctrlGeneral.Cedula;
            string descripcionIC = ctrlGeneral.ItemCodeDescripcion;

            if (cedula != string.Empty && descripcionIC != string.Empty && !descripcionIC.Contains(cedula))
            {
                btnGuardar.Attributes["OnClick"] = "return Sam.Confirma(20);";
                btnGuardarMantener.Attributes["OnClick"] = "return Sam.Confirma(20);";
            }
            else
            {
                btnGuardar.Attributes["OnClick"] = string.Empty;
                btnGuardarMantener.Attributes["OnClick"] = string.Empty;
            }
        }

        #endregion

        #region Utilerias

        /// <summary>
        /// Guarda toda la información del número único.
        /// </summary>
        private bool guardaNumeroUnico(out int[] matSpool, out int numeroUnicoID)
        {

            matSpool = null;
            numeroUnicoID = -1;
            try
            {
                NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConRecepcion(EntityID.Value);
                numUnico.VersionRegistro = VersionRegistro;

                numUnico.StartTracking();
                unMap(numUnico);
                numUnico.StopTracking();
                NumeroUnicoBO.Instance.Guarda(numUnico, SessionFacade.UserId);

                matSpool = NumeroUnicoBO.Instance.OdtEsperaMaterial(ProyectoID, numUnico);
                numeroUnicoID = numUnico.NumeroUnicoID;
                //if (matSpool != null)
                //{
                //    string materiales = string.Join(",", matSpool.Select(x => x.ToString()).ToArray());
                //    ClientScript.RegisterStartupScript(typeof(AltaNumeroUnico), "NumUnicoPopup", "$(function () { Sam.Materiales.AbrePopupOdtReqMaterial(" + numUnico.NumeroUnicoID + ", '" + materiales + "' ); });", true);

                //}

                return true;
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }

            return false;
        }

        /// <summary>
        /// Limpia los campos de la forma
        /// </summary>
        private void limpiaDatos()
        {
            ctrlAdicional.LimpiaDatos();
            ctrlGeneral.LimpiaDatos();
            ctrlProveedor.LimpiaDatos();
        }


        #endregion
    }
}