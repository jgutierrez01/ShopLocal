using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using Telerik.Web.UI;
using SAM.Entities.Personalizadas;
using SAM.Web.Controles.Spool;
using SAM.Entities.Grid;
using System.Linq.Dynamic;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;

namespace SAM.Web.Produccion
{
    public partial class FijarPrioridad : SamPaginaPrincipal
    {
        /// <summary>
        /// Carga el combo de proyectos con únicamente los proyectos disponibles para el usuario asi como
        /// configurar el radwindow de prioridad para que no muestre todos los controles default de telerik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_Prioridades);
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
                wndFijarPrioridad.Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize;
            }
        }

        /// <summary>
        /// Carga el control del proyecto una vez que se selecciona este
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProyecto.SelectedValue.SafeIntParse() > 0)
            {
                headerProyecto.Visible = true;
                headerProyecto.BindInfo(ddlProyecto.SelectedValue.SafeIntParse());
            }
            else
            {
                headerProyecto.Visible = false;
            }
        }

        /// <summary>
        /// Valida que se haya seleccionado un proyecto y muestra los datos del mismo:
        /// - Información para el project control header
        /// - Carga la información del grid
        /// - Configura las columnas personalizadas del grid
        /// - Muestra los placeholders necesarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            Validate("vgProyecto");

            if (IsValid)
            {
                EntityID = ddlProyecto.SelectedValue.SafeIntParse();                
                configurarColumnasProyecto();
                establecerDataSourceGrid();
                phListado.Visible = true;
                grdSpools.DataBind();
            }
        }

        /// <summary>
        /// Método que muestra las columnas personalizadas del proyecto par la nomenclatura del spool
        /// </summary>
        private void configurarColumnasProyecto()
        {
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == EntityID.Value).Single();

            if (p.ColumnasNomenclatura > 0)
            {
                foreach (NomenclaturaStruct n in p.Nomenclatura)
                {
                    GridColumn col = grdSpools.MasterTableView.Columns.FindByUniqueName("Segmento" + n.Orden);
                    col.HeaderText = n.NombreColumna;
                    col.Visible = true;
                    col.HeaderStyle.Width = Unit.Pixel(100);
                    col.FilterControlWidth = Unit.Pixel(60);
                }
            }
        }

        /// <summary>
        /// Actualiza la información del grid, regresando a la página uno y quitando los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdSpools.ResetBind();
            establecerDataSourceGrid();
            grdSpools.DataBind();
        }

        /// <summary>
        /// Obtiene los spools que están filtrados actualmente en el grid (aunque se extienda a varias páginas)
        /// y les establece la prioridad a lo que el usuario dictó.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFijarPrioridad_Click(object sender, EventArgs e)
        {
            //validar los controles de prioridad
            Page.Validate("vgPrioridad");

            if (IsValid)
            {
                IQueryable<GrdIngSpool> lst = SpoolBO.Instance
                                                     .ObtenerIngPorProyecto(EntityID.Value)
                                                     .AsQueryable();

                //Obtener la expresión de filtrado dinámico del grid de telerik y a traves de LinQ dinámico
                //generar el filtro.
                foreach (GridColumn column in grdSpools.Columns)
                {
                    string filtro = column.EvaluateFilterExpression();
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        lst = lst.Where(filtro);
                    }
                }

                try
                {
                    //Obtiene los ids de los spools a los cuales se le debe fijar la prioridad y manda llamar
                    //el método de la capa de negocios correspondiente
                    SpoolBO.Instance.FijaPrioridad(lst.Select(x => x.SpoolID).ToArray(),
                                                    EntityID.Value,
                                                    SessionFacade.UserId,
                                                    DateTime.Now,
                                                    txtPrioridad.Text.SafeIntParse());

                    //Volver a hacer el binding pero respetando los filtros y paginación
                    grdSpools.Rebind();
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        protected void btnAprobadoParaCruce_Click(object sender, EventArgs e)
        {
            IQueryable<GrdIngSpool> lst = SpoolBO.Instance.ObtenerIngPorProyecto(EntityID.Value).AsQueryable();

            //Obtener la expresión de filtrado dinámico del grid de telerik y a traves de LinQ dinámico
            //generar el filtro.
            foreach (GridColumn column in grdSpools.Columns)
            {
                string filtro = column.EvaluateFilterExpression();
                if (!string.IsNullOrEmpty(filtro))
                {
                    lst = lst.Where(filtro);
                }
            }

            try
            {
                //Obtiene los ids de los spools a los cuales se le debe fijar la prioridad y manda llamar
                //el método de la capa de negocios correspondiente
                SpoolBO.Instance.FijaAprobadoParaCruce(lst.Select(x => x.SpoolID).ToArray(),
                                                       EntityID.Value,
                                                       SessionFacade.UserId,
                                                       DateTime.Now,
                                                       chkAprobadoParaCruce.Checked);

                //Volver a hacer el binding pero respetando los filtros y paginación
                grdSpools.Rebind();
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        /// <summary>
        /// Nos ayuda a configurar los links dinámicos del header del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;

                HyperLink lnkPrioridadSeleccionados = (HyperLink)item.FindControl("lnkPrioridadSeleccionados");
                HyperLink imgPrioridadSeleccionados = (HyperLink)item.FindControl("imgPrioridadSeleccionados");

                string jsLinkSeleccionados = string.Format("javascript:Sam.Produccion.AbrePopupFijarPrioridadSeleccionados('{0}','{1}');",
                                               grdSpools.ClientID, EntityID);

                lnkPrioridadSeleccionados.NavigateUrl = jsLinkSeleccionados;
                imgPrioridadSeleccionados.NavigateUrl = jsLinkSeleccionados;

                HyperLink lnkPrioridad = (HyperLink)item.FindControl("lnkPrioridad");
                HyperLink imgPrioridad = (HyperLink)item.FindControl("imgPrioridad");

                string jsLink = string.Format("javascript:Sam.Produccion.AbrePopupFijarPrioridad('{0}');",
                                               wndFijarPrioridad.ClientID);

                lnkPrioridad.NavigateUrl = jsLink;
                imgPrioridad.NavigateUrl = jsLink;

                HyperLink lnkAprobadoParaCruce = (HyperLink)item.FindControl("lnkAprobadoParaCruce");
                HyperLink imgAprobadoParaCruce = (HyperLink)item.FindControl("imgAprobadoParaCruce");

                string jsLinkCruce = string.Format("javascript:Sam.Ingenieria.AbrePopupAprobadoParaCruce('{0}');",
                                              wndAprobadoParaCruce.ClientID);

                lnkAprobadoParaCruce.NavigateUrl = jsLinkCruce;
                imgAprobadoParaCruce.NavigateUrl = jsLinkCruce;
            }
        }

        /// <summary>
        /// Se dispara cuando el grid determina que necesita volver a obtener su datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSourceGrid();
        }

        /// <summary>
        /// Este método es necesario para poder saber cuando una fila en particular del grid se está expandiendo
        /// y así podernos traer el detalle del spool para mostrarlo en el control inferior.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string cmdName = e.CommandName;

            //Sólo si el comando fue expandir/colapser
            if (cmdName == RadGrid.ExpandCollapseCommandName)
            {
                GridDataItem item = e.Item as GridDataItem;

                //Solo vamos por los datos si el usuario decidió expandir el elemento, si decide colapsarlo
                //no vamos a BD
                if (item != null && !item.Expanded)
                {
                    GridNestedViewItem nestedItem = (GridNestedViewItem)item.ChildItem;

                    RadMultiPage rmpDetalle = nestedItem.FindControl("rmpDetalle") as RadMultiPage;

                    if (rmpDetalle != null)
                    {
                        //Hacer los bindings a los controles hijos con la información del spool
                        int spoolID = item.GetDataKeyValue("SpoolID").SafeIntParse();

                        DetSpool spool = SpoolBO.Instance.ObtenerDetalleCompleto(spoolID);

                        RadPageView rpv = rmpDetalle.FindPageViewByID("rpvMateriales");
                        MaterialRO materiales = (MaterialRO)rpv.FindControl("materiales");
                        materiales.Map(spool.Materiales);

                        rpv = rmpDetalle.FindPageViewByID("rpvJuntas");
                        JuntaRO juntas = (JuntaRO)rpv.FindControl("juntas");
                        juntas.Map(spool.Juntas);

                        rpv = rmpDetalle.FindPageViewByID("rpvCortes");
                        CorteRO cortes = (CorteRO)rpv.FindControl("cortes");
                        cortes.Map(spool.Cortes);
                    }
                }
            }
        }

        /// <summary>
        /// Fija la propiedad datasource del grid, en caso de que se esté haciendo un binding manual es necesario
        /// llamar posteriormente grdSpools.DataBind()
        /// </summary>
        private void establecerDataSourceGrid()
        {
            grdSpools.DataSource = SpoolBO.Instance.ObtenerIngPorProyecto(EntityID.Value);
        }
    }
}