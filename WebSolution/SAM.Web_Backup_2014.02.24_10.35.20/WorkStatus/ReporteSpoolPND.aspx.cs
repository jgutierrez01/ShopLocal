using System;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using Telerik.Web.UI;
using SAM.BusinessLogic.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class ReporteSpoolPND : SamPaginaPrincipal
    {
        #region ViewState de los filtros

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

        /// <summary>
        /// 
        /// </summary>
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
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_ReportePND);
                CargaCombo();

                if (Request.QueryString["RID"] != null)
                {
                    int reporteSpoolID = Request.QueryString["RID"].SafeIntParse();
                    _proyecto = ReportePndBO.Instance.ObtenerProyectoIDPorReporteSpool(reporteSpoolID);
                    _tipoPrueba = ReportePndBO.Instance.ObtenerReporteSpool(reporteSpoolID).TipoPruebaSpoolID;
                    ddlTipoDePrueba.SelectedValue = _tipoPrueba.ToString();
                    ddlProyecto.SelectedValue = _proyecto.ToString();

                    proyHeader.Visible = true;
                    proyHeader.BindInfo(_proyecto);
                    phGrid.Visible = true;
                    grdReporteSpoolPND.Rebind();
                }
            }
        }

        /// <summary>
        /// desplega el header al seleccionar el proyecto
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
        /// guarda los filtros en viewState para despues generar el grid
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
                grdReporteSpoolPND.Rebind();
            }
        }

        /// <summary>
        /// manda a llamar el metodo de establecerDataSource para llenar el grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdReporteSpoolPND_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Genera la funsionalidad de los botones del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdReporteSpoolPND_ItemCommand(object source, GridCommandEventArgs e)
        {
            int reporteSpoolPnd = e.CommandArgument.SafeIntParse();
            if (e.CommandName == "Borrar")
            {
                try
                {

                    ReportePndBO.Instance.BorraReporteSpool(reporteSpoolPnd);
                    EstablecerDataSource();
                    grdReporteSpoolPND.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        /// <summary>
        /// verifica si existe el archivo en su carpeta correspondiente
        /// </summary>
        /// <param name="reportePndID"></param>
        /// <returns></returns>
        private bool existeReporte(string numeroReporte, int tipoPruebaID)
        {
            return ReportesBL.Instance
                            .ReporteExisteEnFileSystem(numeroReporte, _proyecto, TipoReporte.ReporteSpoolPND, tipoPruebaID);
        }

        /// <summary>
        /// establece el datasource filtrado en el grid
        /// </summary>
        private void EstablecerDataSource()
        {
            //llama a llenar el grid con los filtros
            grdReporteSpoolPND.DataSource = ReportePndBO.Instance.ObtenerReportesSpoolConFiltros(_proyecto,
                                                                                            _desde,
                                                                                            _hasta,
                                                                                            _numeroDeReporte,
                                                                                            _tipoPrueba
                                                                                            );
        }

        /// <summary>
        /// llena los ddl con sus datos correspondientes
        /// </summary>
        private void CargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlTipoDePrueba.BindToEnumerableWithEmptyRow(ReportePndBO.Instance.ObtenerTipoPruebaSpool(), "Nombre", "TipoPruebaSpoolID", null);
        }

        /// <summary>
        /// llena dinamicamente el link de descargar para verificar si existe, si no existe lo oculta
        /// y si existe le genera su liga correspondiente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdReporteSpoolPND_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdReporteSpoolPnd repPnd = (GrdReporteSpoolPnd)item.DataItem;

                HyperLink hlDescargar = (HyperLink)item["hlDescargar_h"].FindControl("hlDescargar");

                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;
                visor.ProyectoID = _proyecto;
                visor.NombresParametrosReporte = "TipoPrueba,NumeroReporte";
                visor.ValoresParametrosReporte = string.Format("{0},{1}", repPnd.TipoPruebaSpoolID, repPnd.NumeroDeReporte);

                switch ((TipoPruebaSpoolEnum)repPnd.TipoPruebaSpoolID)
                {
                    case TipoPruebaSpoolEnum.Hidrostatica:
                        visor.Visible = true;
                        visor.Tipo = TipoReporteProyectoEnum.Hidrostatica;
                        break;
                    default:
                        break;
                }

                if (hlDescargar != null)
                {
                    if (existeReporte(repPnd.NumeroDeReporte, repPnd.TipoPruebaSpoolID))
                    {
                        hlDescargar.NavigateUrl = ConstruyeUrl(repPnd.ReporteSpoolPndID, repPnd.TipoPruebaSpoolID);
                    }
                    else
                    {
                        hlDescargar.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// manda a la pagina de DescargaReporte con sus respectivos parametros
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected string ConstruyeUrl(int x, int tipoPruebaSpoolID)
        {
            ReporteSpoolPnd reporte = ReportePndBO.Instance.ObtenerReporteSpool(x);

            return "/WorkStatus/DescargaReporte.aspx?ReporteID=" + x + "&ProyectoID=" + _proyecto + "&TipoPrueba=" +
                   tipoPruebaSpoolID + "&TipoReporte=21";
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdReporteSpoolPND.ResetBind();
            grdReporteSpoolPND.Rebind();
        }
    }
}