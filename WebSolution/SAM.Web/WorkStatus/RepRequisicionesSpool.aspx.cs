using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Produccion;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.Entities.Grid;
using SAM.Entities;
using SAM.BusinessLogic.Workstatus;
using SAM.BusinessObjects.Workstatus;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.WorkStatus
{
    public partial class RepRequisicionesSpool : SamPaginaPrincipal
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
        /// numero de requisicion a buscar
        /// </summary>
        private string _numeroRequisicion
        {
            get
            {
                if (ViewState["numeroRequisicion"] != null)
                {
                    return (string)ViewState["numeroRequisicion"];
                }
                return null;
            }
            set
            {
                ViewState["numeroRequisicion"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int _tipoPruebaSpool
        {
            get
            {
                return (int)ViewState["tipoPrueba"];
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
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_ReporteRequisiciones);
                cargaCombos();

                if (Request.QueryString["RID"] != null)
                {
                    int requisicionSpoolID = Request.QueryString["RID"].SafeIntParse();
                    _proyecto = RequisicionBO.Instance.ObtenerProyectoIDPorRequisicionSpool(requisicionSpoolID);
                    _tipoPruebaSpool = RequisicionBO.Instance.ObtenerReqSpool(requisicionSpoolID).TipoPruebaSpoolID;
                    ddlTipoPrueba.SelectedValue = _tipoPruebaSpool.ToString();
                    ddlProyecto.SelectedValue = _proyecto.ToString();

                    proyEncabezado.Visible = true;
                    proyEncabezado.BindInfo(_proyecto);
                    phGrid.Visible = true;
                    grdRequisicionesSpool.Rebind();
                }
            }
        }

        /// <summary>
        /// metodo para cargar el combo "ddlProyecto" y "ddlTipoPrueba.
        /// </summary>
        private void cargaCombos()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlTipoPrueba.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposPruebaSpool().OrderBy(x => x.Nombre));
        }

        /// <summary>
        /// manda a llamar el rebind del grdRequisiciones para que se generen los renglones
        /// </summary>
        protected void btnMostrar_OnClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                //Guardar en ViewState los filtros
                _proyecto = ddlProyecto.SelectedValue.SafeIntParse();
                _desde = dtpDesde.SelectedDate;
                _hasta = dtpHasta.SelectedDate;
                _numeroRequisicion = txtNumeroRequisicion.Text;
                _tipoPruebaSpool = ddlTipoPrueba.SelectedValue.SafeIntParse();
                phGrid.Visible = true;
                grdRequisicionesSpool.Rebind();
            }
        }

        /// <summary>
        /// trae el DataSource filtrado con sus resultados para mostrar al grdRequisiciones
        /// </summary>
        private void EstablecerDataSource()
        {
            grdRequisicionesSpool.DataSource = RequisicionBO.Instance.ObtenerReporteRequisicionSpool(_proyecto, _desde, _hasta, _numeroRequisicion, _tipoPruebaSpool);
        }

        /// <summary>
        /// desplega el header del proyecto
        /// </summary>
        protected void ddlProyecto_SelectedItemChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                proyEncabezado.BindInfo(proyectoID);
                proyEncabezado.Visible = true;
            }
            else
            {
                proyEncabezado.Visible = false;
                phGrid.Visible = false;
            }
        }

        /// <summary>
        /// llena el grid con la informacion requerida
        /// </summary>
        protected void grdRequisicionesSpool_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// actualiza el grdRequisiciones
        /// </summary>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdRequisicionesSpool.Rebind();
        }

        protected void grdRequisicionesSpool_ItemDataBound(object source, GridItemEventArgs e)
        {
            //Verificar si existe el reporte para mostrar el link de Descarga
            if (e.Item is GridDataItem)
            {
                string numeroReporte = string.Empty;
                GridDataItem item = (GridDataItem)e.Item;
                GrdRepRequisicionSpool reporte = (GrdRepRequisicionSpool)item.DataItem;

                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;
                visor.ProyectoID = _proyecto;
                visor.NombresParametrosReporte = "TipoPruebaID,NumeroRequisicion";
                visor.ValoresParametrosReporte = string.Format("{0},{1}", reporte.TipoPruebaSpoolID, reporte.NumeroRequisicion);

                visor.Tipo = TipoReporteProyectoEnum.RequisicionSpool;

                HyperLink hlDescargar = e.Item.FindControl("hlDescargar") as HyperLink;

                if (ReportesBL.Instance.ReporteExisteEnFileSystem(reporte.RequisicionSpoolID, _proyecto, TipoReporte.RequisicionSpool, reporte.TipoPruebaSpoolID, out numeroReporte))
                {
                    hlDescargar.Visible = true;
                    hlDescargar.NavigateUrl = ConstruyeUrl(reporte.RequisicionSpoolID);
                }

            }
        }

        /// <summary>
        /// Accion dependiendo al botton del templeate seleccionado
        /// Borrar. Borra el registro seleccionado
        /// </summary>
        protected void grdRequisicionesSpool_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int requisicionSpoolID = e.CommandArgument.SafeIntParse();
                    RequisicionBO.Instance.BorraReqSpool(requisicionSpoolID);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "");
                }
                finally
                {
                    EstablecerDataSource();
                    grdRequisicionesSpool.DataBind();
                }
            }
        }

        protected string ConstruyeUrl(int reporteID)
        {
            return "/WorkStatus/DescargaReporte.aspx?ReporteID=" + reporteID + "&ProyectoID=" + _proyecto + "&TipoPrueba=" +
                   _tipoPruebaSpool + "&TipoReporte=22";
        }
    }
}