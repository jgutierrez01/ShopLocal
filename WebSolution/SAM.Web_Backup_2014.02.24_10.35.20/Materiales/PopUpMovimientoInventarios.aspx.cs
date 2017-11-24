using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Produccion;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Materiales;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Materiales
{
    public partial class PopUpMovimientoInventarios : SamPaginaPopup
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoANumeroUnico(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un número único {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    //Inicializo el segmento como A para poder inicializar el saldo cuando cambie.
                    segmento = "A";
                    saldo = 0;
                }

                TiposMovimiento = CacheCatalogos.Instance.ObtenerTiposMovimiento();
                NumeroUnico numUnico = NumeroUnicoBO.Instance.ObtenerConInventarioColadaICProfile(EntityID.Value);
                cargaDatos(numUnico);
            }
        }

        /// <summary>
        /// Carga los datos del numero unico en los campos correspondientes
        /// </summary>
        /// <param name="numUnico"></param>
        private void cargaDatos(NumeroUnico numUnico)
        {
            lblNumeroUnico.Text = numUnico.Codigo;
            lblDiametro1.Text = String.Format("{0:N3}", numUnico.Diametro1);
            lblDiametro2.Text = String.Format("{0:N3}", numUnico.Diametro2);
            lblProfile1.Text = numUnico.TipoCorte1ID.HasValue ? numUnico.TipoCorte.Nombre : string.Empty;
            lblProfile2.Text = numUnico.TipoCorte2ID.HasValue ? numUnico.TipoCorte1.Nombre : string.Empty;
            lblItemCode.Text = numUnico.ItemCodeID.HasValue ? numUnico.ItemCode.Codigo + " - " + numUnico.ItemCode.DescripcionEspanol : string.Empty;
            lblCedula.Text = numUnico.Cedula;
            if (numUnico.NumeroUnicoInventario != null)
            {
                
                NumeroUnico movimientos = NumeroUnicoBO.Instance.ObtenerMovimientosInventarios(numUnico.NumeroUnicoID);

                if (numUnico.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio)
                {
                    cargaAccesorio(movimientos);
                    phTubo.Visible = false;
                    string unidades = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? " units" : " piezas";
                    lblTotalRecibido.Text = String.Format("{0:N0}", numUnico.NumeroUnicoInventario.CantidadRecibida) + unidades;
                    lblTotalEntradas.Text = String.Format("{0:N0}", contadorEntrada) + unidades;
                    lblTotalDanado.Text = String.Format("{0:N0}", numUnico.NumeroUnicoInventario.CantidadDanada) + unidades;
                    lblSaldoActual.Text = String.Format("{0:N0}", numUnico.NumeroUnicoInventario.InventarioBuenEstado) + unidades;
                    lblTotalSalidas.Text = String.Format("{0:N0}", contadorSalida) + unidades;
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

                    lblTotalRecibido.Text = String.Format("{0:N0}", numUnico.NumeroUnicoInventario.CantidadRecibida) + " mm";
                    lblTotalEntradas.Text = String.Format("{0:N0}", contadorEntrada) + " mm";
                    lblTotalDanado.Text = String.Format("{0:N0}", numUnico.NumeroUnicoInventario.CantidadDanada) + " mm";
                    lblSaldoActual.Text = String.Format("{0:N0}", numUnico.NumeroUnicoInventario.InventarioBuenEstado) + " mm";
                    lblTotalSalidas.Text = salidaGeneralTubo.ToString() + " mm";
                   
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
                else if (!TiposMovimiento.Where(x => x.ID == item.TipoMovimientoID).Select(x => x.Nombre == "CambioItemCode").SingleOrDefault())
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

                if(salidaSegmentos.Count == 0)
                {
                    litTotalSalidas.Text = "0";
                }
                else
                {
                    Simple salida = salidaSegmentos.Where(x => x.Valor == item.Segmento).FirstOrDefault();
                    litTotalSalidas.Text = salida != null ? String.Format("{0:N0}", salida.ID) : "0";
                }

                if(salidaTemporalSegmentos.Count == 0)
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
                else if (!TiposMovimiento.Where(x => x.ID == item.TipoMovimientoID).Select(x => x.Nombre == "CambioItemCode").SingleOrDefault())
                {
                    Literal ltSalida = e.Item.FindControl("ltSalida") as Literal;
                    contadorSalida += item.Cantidad;
                    ltSalida.Text = item.Cantidad.ToString();
                    saldo = saldo - item.Cantidad;

                    if(item.TipoMovimientoID == (int)TipoMovimientoEnum.SalidaPintura)
                    {
                        ltSalida.Text = ltSalida.Text + " *";
                    }

                }

                if (!TiposMovimiento.Where(x => x.ID == item.TipoMovimientoID).Select(x => x.ApareceEnSaldos).SingleOrDefault())
                {
                    e.Item.Visible = false;
                }

                ltSaldo.Text = saldo.ToString();
            }
        }
    }
}