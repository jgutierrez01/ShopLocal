using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Utilerias;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Grid;
using Mimo.Framework.Exceptions;
using SAM.Entities.Cache;

namespace SAM.Web.Produccion
{
    /// <summary>
    /// Esta página muestra un lista de órdenes de trabajo ya sea dentro de todo un patio o dentro de un proyecto/taller
    /// en particular.
    /// </summary>
    public partial class LstOrdenTrabajo : SamPaginaPrincipal
    {
        /// <summary>
        /// JS para abrir el popup de impresión para una ODT en particular
        /// </summary>
        private const string JS_IMPRESION = "javascript:Sam.Produccion.AbrePopupImpresionOdt('{0}');";
        
        //HSLT -- AGREGO LA LIGA PARA ABRIR EL POPUP DE CONSULTA DE HISTORICOS 
        private const string JS_HISTORICO = "javascript:Sam.Produccion.AbrePopupHistoricoOdt('{0}');";

        /// <summary>
        /// JS para confirmar que se desea eliminar una ODT.
        /// </summary>
        private const string JS_BORRAR = "return Sam.Confirma(2,'{0}');";

        #region Variables de ViewState

        /// <summary>
        /// ID del patio seleccionado en del dropdown
        /// </summary>
        private int PatioID
        {
            get
            {
                if (ViewState["PatioID"] != null)
                {
                    return (int)ViewState["PatioID"];
                }
                return ViewState["PatioID"].SafeIntParse();
            }
            set
            {
                ViewState["PatioID"] = value;
            }
        }

        /// <summary>
        /// ID del proyecto seleccionado en el dropdown
        /// </summary>
        private int ProyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] != null)
                {
                    return (int)ViewState["ProyectoID"];
                }
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }        
        }

        /// <summary>
        /// ID del taller seleccionado en el dropdown
        /// </summary>
        private int TallerID
        {
            get
            {
                if (ViewState["TallerID"] != null)
                {
                    return (int)ViewState["TallerID"];
                }
                return ViewState["TallerID"].SafeIntParse();
            }
            set
            {
                ViewState["TallerID"] = value;
            }        
        }

        #endregion

        /// <summary>
        /// Carga la información para los filtros
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_OrdenesTrabajo);
                cargaCombos();

                if (Request.QueryString["PID"] != null)
                {
                    ProyectoID = Request.QueryString["PID"].SafeIntParse();
                    ddlProyecto.SelectedValue = ProyectoID.ToString();

                    ProyectoCache p = UserScope.MisProyectos.Where(x => x.ID == ProyectoID).Single();
                    ddlPatio.SelectedValue = p.PatioID.ToString();
                    PatioID = p.PatioID;

                    headerProyecto.Visible = true;
                    headerProyecto.BindInfo(ProyectoID);
                    EstablecerDataSource();
                    phLista.Visible = true;
                    grdOrdenTrabajo.DataBind();
                }
            }
        }

        /// <summary>
        /// Valida que se haya seleccionado ya sea el proyecto o el patio para no permitir filtros
        /// en blanco.
        /// </summary>
        protected void cusPatioProyecto_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrEmpty(ddlPatio.SelectedValue) || !string.IsNullOrEmpty(ddlProyecto.SelectedValue);
        }

        /// <summary>
        /// Se dispara cuando el patio seleccionado cambia. Se actualiza la lista disponible de proyectos y talleres.
        /// A su vez se limpia el project header control ya que se debe volver a seleccionar el proyecto deseado.
        /// </summary>
        protected void ddlPatio_SelectedIndexChange(object sender, EventArgs e)
        {
            int patioID = ddlPatio.SelectedValue.SafeIntParse();

            if (patioID <= 0)
            {
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
                ddlTaller.Items.Clear();
            }
            else
            {
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.ProyectosPorPatio(patioID));
                ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorPatio(patioID));
            }

            headerProyecto.Visible = false;
        }

        /// <summary>
        /// Se dispara cuando el proyecto seleccionado cambia.  En caso de seleccionar
        /// un proyecto válido se muestra el project header control con sus datos, de lo contrario
        /// se oculta el control.
        /// </summary>
        protected void ddlProyecto_SelectedIndexChange(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID <= 0)
            {
                headerProyecto.Visible = false;
            }
            else
            {
                headerProyecto.BindInfo(proyectoID);
                headerProyecto.Visible = true;
            }
        }

        /// <summary>
        /// Carga inicial de los combos/filtros de la parte superior de la página.
        /// Básicamente se cargan los proyectos y patios para los cuales el usuario tiene permisos.
        /// </summary>
        private void cargaCombos()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlPatio.BindToEntiesWithEmptyRow(UserScope.MisPatios);
        }

        /// <summary>
        /// Fijar el datasource del grid
        /// </summary>
        public void EstablecerDataSource()
        {
            int[] pids = UserScope.MisProyectos.Select(x => x.ID).ToArray();

            grdOrdenTrabajo.DataSource = OrdenTrabajoBO.Instance
                                                       .ObtenerListaParaGrid(PatioID, ProyectoID, TallerID, pids)
                                                       .OrderBy(x => x.Orden);
        }

        /// <summary>
        /// Se dispara cuando el usuario decide mostrar las órdenes de trabajo en base
        /// a sus filtros especificados.
        /// </summary>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            Validate("vgPatio");

            if (IsValid)
            {
                //Guardar esto en ViewState para los rebinds del grid
                PatioID = ddlPatio.SelectedValue.SafeIntParse();
                ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();
                TallerID = ddlTaller.SelectedValue.SafeIntParse();

                phLista.Visible = true;

                EstablecerDataSource();
                grdOrdenTrabajo.DataBind();
            }
        }

        /// <summary>
        /// Se dispara cada que se hace el binding de una orden de trabajo al grid.
        /// Usamos este método para configurar las acciones que se pueden llevar a cabo sobre
        /// cada elemento en particular del grid: Editar, Borrar e Imprimir.
        /// </summary>
        protected void grdOrdenTrabajo_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                GrdOdt odt = (GrdOdt)item.DataItem;

                HyperLink hlEditar = (HyperLink)item["editar_h"].FindControl("hlEditar");
                ImageButton imgBorrar = (ImageButton)item["borrar_h"].FindControl("imgBorrar");
                HyperLink hlImprimir = (HyperLink)item["imprimir_h"].FindControl("hlImprimir");

                //HSLT -- AGREGO EL HYPERVINCULO PARA EL POPUP DEL HISTORICO 
                HyperLink hlhistorial = (HyperLink)item["historial_h"].FindControl("hlhistorial");


                hlEditar.NavigateUrl = string.Format(WebConstants.ProduccionUrl.DetalleOdt,odt.OrdenDeTrabajoID);
                imgBorrar.CommandArgument = odt.OrdenDeTrabajoID.ToString();
                imgBorrar.OnClientClick = string.Format(JS_BORRAR, odt.NumeroOrden);
                hlImprimir.NavigateUrl = string.Format(JS_IMPRESION, odt.OrdenDeTrabajoID);

                //HSLT -- AGREGO EL URL PARA NAVEGAR 
                hlhistorial.NavigateUrl = string.Format(JS_HISTORICO, odt.OrdenDeTrabajoID);

                //Mostrar un ícono de advertencia en caso que la ODT difiera de lo que dice ingeniería
                if (odt.DifiereDeIngenieria)
                {
                    Image imgAdvertencia = (Image)item["DifiereDeIngenieria"].FindControl("imgAdvertencia");
                    imgAdvertencia.Visible = true;
                }
                else if (odt.FueReingenieria)
                {
                    Image imgFueReingenieria = (Image)item["DifiereDeIngenieria"].FindControl("imgFueReingenieria");
                    imgFueReingenieria.Visible = true;
                }
            }
        }

        /// <summary>
        /// Se dispara cuando desean eliminar una ODT en particular.
        /// </summary>
        protected void grdOrdenTrabajo_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "borrar")
            {
                int ordenTrabajoID = e.CommandArgument.SafeIntParse();
                
                try
                {
                    OrdenTrabajoBO.Instance.Borra(ordenTrabajoID, SessionFacade.UserId);
                    grdOrdenTrabajo.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve, "valLista");
                }
            }
        }

        /// <summary>
        /// Se dispara cuando el grid de Telerik necesita actualizar su fuente de datos.
        /// </summary>
        public void grdOrdenTrabajo_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Limpia los filtros, sortings y paginación y actualiza los datos del grid desde BD.
        /// </summary>
        public void lnkActualizar_Click(object sender, EventArgs e)
        {
            grdOrdenTrabajo.ResetBind();
            grdOrdenTrabajo.Rebind();
        }
    }
}