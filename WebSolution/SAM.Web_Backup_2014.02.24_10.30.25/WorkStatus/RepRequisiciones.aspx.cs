using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Produccion;
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
    public partial class RepRequisiciones : SamPaginaPrincipal
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

        private int _tipoPrueba
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
                    int requisicionID = Request.QueryString["RID"].SafeIntParse();
                    _proyecto = RequisicionBO.Instance.ObtenerProyectoID(requisicionID);
                    _tipoPrueba = RequisicionBO.Instance.Obtener(requisicionID).TipoPruebaID.Value;
                    ddlTipoPrueba.SelectedValue = _tipoPrueba.ToString();
                    ddlProyecto.SelectedValue = _proyecto.ToString();

                    proyEncabezado.Visible = true;
                    proyEncabezado.BindInfo(_proyecto);
                    phGrid.Visible = true;
                    grdRequisiciones.Rebind();
                }
            }
        }

        //metodo para cargar el combo "ddlProyecto" y "ddlTipoPrueba.
        private void cargaCombos()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlTipoPrueba.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposPrueba());
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
                _tipoPrueba = ddlTipoPrueba.SelectedValue.SafeIntParse();
                phGrid.Visible = true;
                grdRequisiciones.Rebind();
            }
        }

        /// <summary>
        /// trae el DataSource filtrado con sus resultados para mostrar al grdRequisiciones
        /// </summary>
        private void EstablecerDataSource()
        {
            grdRequisiciones.DataSource = RequisicionBO.Instance.ObtenerReporteRequisicion(_proyecto, _desde, _hasta, _numeroRequisicion, _tipoPrueba);  
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
        protected void grdRequisiciones_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// actualiza el grdRequisiciones
        /// </summary>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdRequisiciones.Rebind();
        }

        protected void grdRequisiciones_ItemDataBound(object source, GridItemEventArgs e)
        {
            //Verificar si existe el reporte para mostrar el link de Descarga
            if (e.Item is GridDataItem)
            {

               

                string numeroReporte = string.Empty;
                GridDataItem item = (GridDataItem)e.Item;
                GrdRepRequisicion reporte = (GrdRepRequisicion)item.DataItem;

                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;
                visor.ProyectoID = _proyecto;
                visor.NombresParametrosReporte = "TipoPruebaID,NumeroRequisicion";
                visor.ValoresParametrosReporte = string.Format("{0},{1}", reporte.TipoPruebaID,reporte.NumeroRequisicion);
                
                visor.Tipo = TipoReporteProyectoEnum.Requisicion;

                HyperLink hlDescargar = e.Item.FindControl("hlDescargar") as HyperLink;

                if (ReportesBL.Instance.ReporteExisteEnFileSystem(reporte.RequisicionID, _proyecto, TipoReporte.Requisicion,reporte.TipoPruebaID, out numeroReporte))
                {
                    hlDescargar.Visible = true;
                    hlDescargar.NavigateUrl = ConstruyeUrl(reporte.RequisicionID);
                }

            }
        }

        /// <summary>
        /// Accion dependiendo al botton del templeate seleccionado
        /// Borrar. Borra el registro seleccionado
        /// </summary>
        protected void grdRequisiciones_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int requisicionID = e.CommandArgument.SafeIntParse();
                    RequisicionBO.Instance.Borra(requisicionID);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "");
                }
                finally
                {
                    EstablecerDataSource();
                    grdRequisiciones.DataBind();
                }
            }
        }

        protected string ConstruyeUrl(int reporteID)
        {
            return "/WorkStatus/DescargaReporte.aspx?ReporteID=" + reporteID + "&ProyectoID=" + _proyecto + "&TipoPrueba=" +
                   _tipoPrueba + "&TipoReporte=5";
        }
    }
}