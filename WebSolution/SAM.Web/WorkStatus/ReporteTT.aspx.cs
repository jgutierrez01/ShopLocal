using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using SAM.Common;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class ReporteTT : SamPaginaPrincipal
    {
        #region ViewState de los filtros

        private const string JS_POPUP_EDICIONESPECIAL = "javascript:Sam.Workstatus.AbrePopUpEdicionReporte({0},'{1}');";

        /// <summary>
        /// ID del proyecto seleccionado en el dropdown
        /// </summary>
        private int _proyecto
        {
            get
            {
                return (int)ViewState["proyecto"];
            }
            set
            {
                ViewState["proyecto"] = value;
            }
        }

        Dictionary<string, byte[]> reporte = new Dictionary<string, byte[]>();

        /// <summary>
        /// fecha de inicio en la cual se hara el filtro
        /// </summary>
        private DateTime? _desde
        {
            get
            {
                if (ViewState["desde"] != null)
                {
                    return (DateTime)ViewState["desde"];
                }
                return null;
            }
            set
            {
                ViewState["desde"] = value;
            }
        }

        /// <summary>
        ///  fecha final en la cual se hara el filtro
        /// </summary>
        private DateTime? _hasta
        {
            get
            {
                if (ViewState["hasta"] != null)
                {
                    return (DateTime)ViewState["hasta"];
                }
                return null;
            }
            set
            {
                ViewState["hasta"] = value;
            }
        }

        /// <summary>
        /// numero de estimacion a buscar en el filtro
        /// </summary>
        private string _numeroDeReporte
        {
            get
            {
                if (ViewState["numeroDeReporte"] != null)
                {
                    return (string)ViewState["numeroDeReporte"];
                }
                return null;
            }
            set
            {
                ViewState["numeroDeReporte"] = value;
            }
        }

        private int? _tipoPrueba
        {
            get
            {
                if (ViewState["tipoPrueba"] != null)
                {
                    return (int)ViewState["tipoPrueba"];
                }
                return null;
            }
            set
            {
                ViewState["tipoPrueba"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_ReporteTt);
                CargaCombo();

                if (Request.QueryString["RID"] != null)
                {
                    int reporteID = Request.QueryString["RID"].SafeIntParse();
                    _proyecto = ReporteTtBO.Instance.ObtenerProyectoID(reporteID);
                    _tipoPrueba = ReporteTtBO.Instance.Obtener(reporteID).TipoPruebaID;
                    ddlTipoDePrueba.SelectedValue = _tipoPrueba.ToString();
                    ddlProyecto.SelectedValue = _proyecto.ToString();

                    proyHeader.Visible = true;
                    proyHeader.BindInfo(_proyecto);
                    phGrid.Visible = true;
                    grdReporteTt.Rebind();
                }
            }
        }

        /// <summary>
        /// desplega el header del proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedItemChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                proyHeader.BindInfo(proyectoID);
                proyHeader.Visible = true;
            }
            else
            {
                proyHeader.Visible = false;
            }
        }

        /// <summary>
        /// guarda las variables en viewState y genera el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_OnClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                //Guardar en ViewState los filtros
                _proyecto = ddlProyecto.SelectedValue.SafeIntParse();
                _desde = dtpDesde.SelectedDate;
                _hasta = dtpHasta.SelectedDate;
                _numeroDeReporte = txtNumeroDeReporte.Text;
                _tipoPrueba = ddlTipoDePrueba.SelectedValue.SafeIntParse();

                phGrid.Visible = true;
                grdReporteTt.Rebind();
            }
        }

        /// <summary>
        /// llama al metodo Establecer datasource para generar el grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdReporteTt_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// genera la funsionalidad de los bottones del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdReporteTt_ItemCommand(object source, GridCommandEventArgs e)
        {
                if (e.CommandName == "Borrar")
                {
                    try
                    {
                        int reporteTt = e.CommandArgument.SafeIntParse();
                        ReporteTtBO.Instance.Borra(reporteTt);
                        EstablecerDataSource();
                        grdReporteTt.DataBind();
                    }
                    catch (BaseValidationException ex)
                    {
                        RenderErrors(ex);
                    }
                }
        }
        /// <summary>
        /// genera dinamicamente el link de descargar para saber si existe el archivo o no,
        ///  si existe le genera la liga a la pagina de descarga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdReporteTt_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                
                GridDataItem item = (GridDataItem)e.Item;
                GrdReporteTt RepTt = (GrdReporteTt)item.DataItem;
                HyperLink hlDescargar = (HyperLink)item["hlDescargar_h"].FindControl("hlDescargar");
                HyperLink hlEditar = (HyperLink)item["editarRegistro"].FindControl("hlEditarTT");

                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;
                visor.ProyectoID = _proyecto;
                visor.NombresParametrosReporte = "NumeroReporte";
                visor.ValoresParametrosReporte = RepTt.NumeroDeReporte;


                switch ((TipoPruebaEnum)RepTt.TipoPruebaID)
                {
                    case TipoPruebaEnum.Pwht:
                        visor.Visible = true;
                        visor.Tipo = TipoReporteProyectoEnum.PWHT;
                        break;
                    case TipoPruebaEnum.Durezas:
                        visor.Visible = true;
                        visor.Tipo = TipoReporteProyectoEnum.Durezas;
                        break;
                    default:
                        break;
                }

                if (hlDescargar != null)
                {
                    if (existeReporte(RepTt.NumeroDeReporte, RepTt.TipoPruebaID))
                    {
                        hlDescargar.NavigateUrl = ConstruyeUrl(RepTt.ReporteTtID, RepTt.TipoPruebaID);
                    }
                    else
                    {
                        hlDescargar.Visible = false;
                    }
                }
                if (SessionFacade.PermisoEdicionesEspeciales)
                {
                    hlEditar.NavigateUrl = string.Format(JS_POPUP_EDICIONESPECIAL, RepTt.ReporteTtID, 5);
                    hlEditar.Visible = true;
                }
            }      
        }

        /// <summary>
        /// llena los ddl
        /// </summary>
        private void CargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlTipoDePrueba.BindToEnumerableWithEmptyRow(ReporteTtBO.Instance.ObtenerTipoPrueba(), "Nombre", "TipoPruebaID", null);
        }

        /// <summary>
        /// Establece el datasource filtrado al grid
        /// </summary>
        private void EstablecerDataSource()
        {
            //llama a llenar el grid con los filtros
            grdReporteTt.DataSource = ReporteTtBO.Instance.ObtenerConFiltros(_proyecto,
                                                                                _desde,
                                                                                _hasta,
                                                                                _numeroDeReporte,
                                                                                _tipoPrueba
                                                                               );
        }

        /// <summary>
        /// verifica si existe el archivo en su carpeta correspondiente
        /// </summary>
        private bool existeReporte(string numeroReporte, int tipoPruebaID)
        {
                return ReportesBL.Instance
                                 .ReporteExisteEnFileSystem(numeroReporte, _proyecto, TipoReporte.ReporteTT, tipoPruebaID);
        }

        /// <summary>
        /// construye la liga a la pagina de descargarReporte
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected string ConstruyeUrl(int x, int tipoPruebaID)
        {
            return "/WorkStatus/DescargaReporte.aspx?ReporteID=" + x + "&ProyectoID=" + _proyecto + "&TipoPrueba=" +
                   tipoPruebaID + "&TipoReporte= 1";
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdReporteTt.ResetBind();
            grdReporteTt.Rebind();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            EstablecerDataSource();
            grdReporteTt.DataBind();
        }
    }
}