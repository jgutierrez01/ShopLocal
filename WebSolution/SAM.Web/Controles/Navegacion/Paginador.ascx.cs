using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using Resources;

namespace SAM.Web.Controles.Navegacion
{
    public partial class Paginador : System.Web.UI.UserControl
    {
        public event PaginaCambioHandler PaginaCambio;

        #region Defaults

        private readonly int _tamanioPaginaDefault = 20;
        private readonly int _totalRegistrosDefault = 0;
        private readonly int _paginaActualDefault = 0;
        private readonly bool _muestraPanel = false;
        private readonly string _scriptProcesando = "Sam.Utilerias.PostbackProcesando(false,'');";

        #endregion

        #region Propiedades en ViewState

        public bool MuestraPanelCargando
        {
            get
            {
                if (ViewState["MuestraPanelCargando"] != null)
                {
                    return (bool)ViewState["MuestraPanelCargando"];
                }

                return _muestraPanel;
            }
            set
            {
                ViewState["MuestraPanelCargando"] = value;
            }        
        }

        public int TamanioPagina
        {
            get
            {
                if (ViewState["TamanioPagina"] != null)
                {
                    return (int)ViewState["TamanioPagina"];
                }

                return _tamanioPaginaDefault;
            }
            set
            {
                if (value < 1 || value > 500)
                {
                    throw new ArgumentOutOfRangeException("El tamaño de página debe estar entre 1 y 500");
                }

                ViewState["TamanioPagina"] = value;
            }
        }

        private int TotalRegistros
        {
            get
            {
                if (ViewState["TotalRegistros"] != null)
                {
                    return (int)ViewState["TotalRegistros"];
                }

                return _totalRegistrosDefault;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("El total de registros debe ser mayor o igual a cero");
                }

                ViewState["TotalRegistros"] = value;
            }
        }

        public int PaginaActual
        {
            get
            {
                if (ViewState["PaginaActual"] != null)
                {
                    return (int)ViewState["PaginaActual"];
                }

                return _paginaActualDefault;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La página actual debe ser mayor o igual a cero");
                }

                ViewState["PaginaActual"] = value;
            }
        }

        #endregion

        private int calculaTotalPaginas()
        {
            int numeroPaginasTotales = TotalRegistros / TamanioPagina;

            //Si no es divisible hay una página más
            if (TotalRegistros % TamanioPagina != 0)
            {
                numeroPaginasTotales++;
            }

            return numeroPaginasTotales;
        }

        private int calculaPaginaInicialBloqueAnterior()
        {
            if (PaginaActual < 5)
            {
                return 0;
            }

            int bloquePrev = PaginaActual - 5;
            return bloquePrev - (bloquePrev % 5);
        }

        private int calculaPaginaInicialBloqueSiguiente()
        {
            int sigBloque = PaginaActual + 5;
            return sigBloque - (sigBloque % 5);
        }

        public void Bind(int paginaActual, int totalRegistros)
        {
            PaginaActual = paginaActual;
            TotalRegistros = totalRegistros;
            int numeroPaginasTotales = calculaTotalPaginas();
            int textoInicial = PaginaActual - (PaginaActual % 5);

            foreach (Control ctrl in phHolder.Controls)
            {
                LinkButton lnk = (LinkButton)ctrl;

                lnk.CssClass = "pag";
                lnk.CommandArgument = textoInicial.ToString();

                if (textoInicial == PaginaActual)
                {
                    lnk.CssClass = "pagActiva";
                }

                lnk.Text = (++textoInicial).ToString();
                lnk.Visible = textoInicial <= numeroPaginasTotales;
            }

            lnkPrevios.Visible = paginaActual > 4;
            lnkSiguientes.Visible = textoInicial < numeroPaginasTotales;

            if (totalRegistros <= 0)
            {
                ltLeyendaPaginador.Text = string.Format(MensajesAplicacion.Pager_PaginaXdeY,0,0,0,0,0);
            }
            else
            {
                string formatoCustom = "#,###,###,##0";
                int registroInicial = (PaginaActual*TamanioPagina)+1;
                int registroFinal = Math.Min(totalRegistros, registroInicial+TamanioPagina - 1);

                ltLeyendaPaginador.Text = string.Format(    MensajesAplicacion.Pager_PaginaXdeY,
                                                            (PaginaActual+1).ToString(formatoCustom),
                                                            numeroPaginasTotales.ToString(formatoCustom), 
                                                            registroInicial.ToString(formatoCustom), 
                                                            registroFinal.ToString(formatoCustom), 
                                                            totalRegistros.ToString(formatoCustom));
            }
        }

        public void Reset()
        {
            TotalRegistros = _totalRegistrosDefault;
            PaginaActual = _paginaActualDefault;
        }

        protected void lnk_Comando(object sender, CommandEventArgs args)
        {
            switch (args.CommandName.ToLowerInvariant().Trim())
            {
                case "primera":
                    PaginaActual = 0;
                    break;
                case "anterior":
                    PaginaActual = Math.Max(PaginaActual - 1, 0);
                    break;
                case "bloqueprevio":
                    PaginaActual = calculaPaginaInicialBloqueAnterior();
                    break;
                case "bloquesiguiente":
                    PaginaActual = calculaPaginaInicialBloqueSiguiente();
                    break;
                case "siguiente":
                    PaginaActual = Math.Min(PaginaActual + 1, calculaTotalPaginas() - 1);
                    break;
                case "ultima":
                    PaginaActual = calculaTotalPaginas() - 1;
                    break;
                case "pos":
                    PaginaActual = args.CommandArgument.SafeIntParse();
                    break;
                default:
                    throw new NotSupportedException("El comando enviado no es soportado");
            }

            if (PaginaCambio != null)
            {
                PaginaCambio(this, new ArgumentosPaginador { PaginaActual = PaginaActual });
            }
            else
            {
                Bind(PaginaActual, TotalRegistros);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string onClientClick = MuestraPanelCargando ? _scriptProcesando : string.Empty;

            foreach (Control ctrl in phHolder.Controls)
            {
                LinkButton lnk = (LinkButton)ctrl;
                lnk.OnClientClick = onClientClick;
            }

            lnkPrevios.OnClientClick = onClientClick;
            lnkSiguientes.OnClientClick = onClientClick;
            btnPaginaAnterior.OnClientClick = onClientClick;
            btnPaginaSiguiente.OnClientClick = onClientClick;
            btnPrimerPagina.OnClientClick = onClientClick;
            btnUltimaPagina.OnClientClick = onClientClick;
        }
    }
}