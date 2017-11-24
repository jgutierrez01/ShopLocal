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


namespace SAM.Web.WorkStatus
{
    public partial class RepInspeccionVisual : SamPaginaPrincipal
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
        /// numero de reporte a buscar en el filtro
        /// </summary>
        private string _numeroReporte
        {
            get
            {
                if (ViewState["numeroReporte"] != null)
                {
                    return (string)ViewState["numeroReporte"];
                }
                return null;
            }
            set
            {
                ViewState["numeroReporte"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_ReporteInspeccionVisual);
                cargaCombo();


                if (Request.Params["PID"] != null || Request.Params["D"] != null || Request.Params["H"] != null || Request.Params["R"] != null)
                {
                    if (Request.Params["PID"] != null)
                    {
                        _proyecto = Request.Params["PID"].SafeIntParse();
                        ddlProyecto.SelectedValue = _proyecto.ToString();
                    }
                    if (!string.IsNullOrEmpty(Request.Params["D"]))
                    {
                        _desde = DateTime.Parse(Request.Params["D"]);
                        dtpDesde.SelectedDate = _desde;
                    }
                    if (!string.IsNullOrEmpty(Request.Params["H"]))
                    {
                        _hasta = DateTime.Parse(Request.Params["H"]);
                        dtpHasta.SelectedDate = _hasta;
                    }
                    if (Request.Params["R"] != null)
                    {
                        _numeroReporte = Request.Params["R"];
                        txtNumeroReporte.Text = _numeroReporte;
                    }

                    proyEncabezado.BindInfo(_proyecto);
                    proyEncabezado.Visible = true;
                    phGrid.Visible = true;
                    grdVisual.Rebind();
                }
                
            }
        }

        //metodo para cargar el combo "ddlProyecto".
        private void cargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        /// <summary>
        /// manda a llamar el rebind del grdVisual para que se generen los renglones
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
                _numeroReporte = txtNumeroReporte.Text;

                phGrid.Visible = true;
                grdVisual.Rebind();
            }
        }

        /// <summary>
        /// trae el DataSource filtrado con sus resultados para mostrar al grdVisual
        /// </summary>
        private void EstablecerDataSource()
        {
            grdVisual.DataSource = JuntaWorkstatusBO.Instance.ObtenerReporteInspeccionVisual(_proyecto, _desde, _hasta, _numeroReporte);
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
        protected void grdVisual_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// actualiza el grdVisual
        /// </summary>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdVisual.Rebind();
        }

        protected void grdVisual_ItemDataBound(object source, GridItemEventArgs e)
        {
            //Verificar si existe el reporte para mostrar el link de Descarga
            if (e.Item is GridDataItem)
            {
                string numeroReporte = string.Empty;
                GridDataItem item = (GridDataItem)e.Item;
                GrdRepInspeccionVisual reporte = (GrdRepInspeccionVisual)item.DataItem;

                LinkVisorReportes visor = e.Item.FindControl("hdReporte") as LinkVisorReportes;
                visor.ProyectoID = _proyecto;
                visor.NombresParametrosReporte = "NumeroReporte";
                visor.ValoresParametrosReporte = reporte.NumeroReporte.ToString();
                visor.Tipo = TipoReporteProyectoEnum.InspeccionVisual;

                HyperLink hlDescargar = e.Item.FindControl("hlDescargar") as HyperLink;

                if (ReportesBL.Instance.ReporteExisteEnFileSystem(reporte.InspeccionVisualID, _proyecto,TipoReporte.InspeccionVisual, 0, out numeroReporte))
                {
                    hlDescargar.Visible = true;
                    hlDescargar.NavigateUrl = ConstruyeUrl(reporte.InspeccionVisualID);
                }

                HyperLink hlDetalle = e.Item.FindControl("hlDetalle") as HyperLink;
                hlDetalle.NavigateUrl += "&H=" + (_hasta != null ? _hasta.Value.ToString("d") : string.Empty) + "&D=" + (_desde != null ? _desde.Value.ToString("d") : string.Empty) + "&R=" + _numeroReporte + "&PID=" + _proyecto;
              
            }      
        }

        /// <summary>
        /// Accion dependiendo al botton del templeate seleccionado
        /// Borrar. Borra el registro seleccionado
        /// </summary>
        protected void grdVisual_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int inspeccionVisualID = e.CommandArgument.SafeIntParse();
                    InspeccionVisualBO.Instance.Borra(inspeccionVisualID);
                   
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex, "");
                }
                finally
                {
                    EstablecerDataSource();
                    grdVisual.DataBind();
                }
            }
        }

        protected string ConstruyeUrl(int reporteID)
        {
            return "/WorkStatus/DescargaReporte.aspx?ReporteID=" + reporteID + "&ProyectoID=" + _proyecto + "&TipoPrueba=0" +
                   "&TipoReporte=0";
        }
    }
}