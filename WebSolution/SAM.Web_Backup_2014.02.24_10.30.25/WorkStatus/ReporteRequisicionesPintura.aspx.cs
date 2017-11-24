using System;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using Telerik.Web.UI;

namespace SAM.Web.WorkStatus
{
    public partial class ReporteRequisicionesPintura : SamPaginaPrincipal
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
        /// numero de Requisicion a buscar en el filtro
        /// </summary>
        private string _numeroDeRequisicion
        {
            get
            {
                if (ViewState["numeroDeRequisicion"] != null)
                {
                    return (string)ViewState["numeroDeRequisicion"];
                }
                return null;
            }
            set
            {
                ViewState["numeroDeRequisicion"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargaCombo();
                
                if (Request.QueryString["RID"] != null)
                {
                    int requisicionID = Request.QueryString["RID"].SafeIntParse();
                    _proyecto = RequisicionPinturaBO.Instance.ObtenerProyectoID(requisicionID);
                    //_numeroDeRequisicion = RequisicionPinturaBO.Instance.ObtenerRequisicionPintura(requisicionID).NumeroRequisicion;
                    //txtNumeroDeRequisicion.Text = _numeroDeRequisicion;
                    ddlProyecto.SelectedValue = _proyecto.ToString();

                    proyHeader.Visible = true;
                    proyHeader.BindInfo(_proyecto);
                    EstablecerDataSource();
                    phGrid.Visible = true;
                    grdReporteReqPintura.DataBind();
                }
            }
        }

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

        protected void btnMostrar_OnClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                //Guardar en ViewState los filtros
                _proyecto = ddlProyecto.SelectedValue.SafeIntParse();
                _desde = dtpDesde.SelectedDate;
                _hasta = dtpHasta.SelectedDate;
                _numeroDeRequisicion = txtNumeroDeRequisicion.Text;

                phGrid.Visible = true;
                grdReporteReqPintura.Rebind();
            }
        }

        protected void grdReporteReqPintura_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdReporteReqPintura_ItemCommand(object source, GridCommandEventArgs e)
        {
           if (e.CommandName == "Borrar")
            {
                try
                {
                    int reportePnd = e.CommandArgument.SafeIntParse();
                    ReporteRequisicionPinturaBO.Instance.Borra(reportePnd);
                    EstablecerDataSource();
                    grdReporteReqPintura.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdReporteReqPintura_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdReporteReqPintura RepReqPintura = (GrdReporteReqPintura)item.DataItem;

                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;
                visor.ProyectoID = _proyecto;
                visor.NombresParametrosReporte = "RequisicionPinturaID";
                visor.ValoresParametrosReporte = RepReqPintura.RequisicionPinturaID.ToString();
                visor.Tipo = TipoReporteProyectoEnum.RequisicionPintura;

                HyperLink hlDescargar = (HyperLink)item["hlDescargar_h"].FindControl("hlDescargar");

                if (hlDescargar != null)
                {
                    if (existeReporte(RepReqPintura.NumeroDeRequisicion))
                    {
                        hlDescargar.NavigateUrl = ConstruyeUrl(RepReqPintura.RequisicionPinturaID);
                    }
                    else
                    {
                        hlDescargar.Visible = false;
                    }
                }

            }      
        }

        /// <summary>
        /// verifica si existe el archivo en su carpeta correspondiente
        /// </summary>
        /// <param name="numeroreporte"></param>
        /// <returns></returns>
        private bool existeReporte(string numeroreporte)
        {
            return ReportesBL.Instance
                               .ReporteExisteEnFileSystem(numeroreporte, _proyecto, TipoReporte.RequisicionPintura, 0);
        }

        private void EstablecerDataSource()
        {
            //llama a llenar el grid con los filtros
            grdReporteReqPintura.DataSource = ReporteRequisicionPinturaBO.Instance.ObtenerConFiltros(_proyecto,
                                                                                _desde,
                                                                                _hasta,
                                                                                _numeroDeRequisicion
                                                                               );
        }

        private void CargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        /// <summary>
        /// manda a la pagina de DescargaReporte con sus respectivos parametros
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected string ConstruyeUrl(int x)
        {
            return "/WorkStatus/DescargaReporte.aspx?ReporteID=" + x + "&ProyectoID=" + _proyecto + "&TipoPrueba=" +
                  0 + "&TipoReporte= 4";
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdReporteReqPintura.ResetBind();
            grdReporteReqPintura.Rebind();
        }
    }
}