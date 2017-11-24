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
using SAM.BusinessLogic;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Produccion;
using SAM.Web.Controles.Spool;
using System.Linq.Dynamic;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using SAM.BusinessLogic.Ingenieria;

namespace SAM.Web.Ingenieria
{
    public partial class IngenieriaDeProyecto : SamPaginaPrincipal
    {
        private int _proyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] == null)
                {
                    ViewState["ProyectoID"] = -1;
                }
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        /// <summary>
        /// JS para abrir el popup con el detalle del spool en modo sólo lectura
        /// </summary>
        private const string JS_POPUP_SPOOL = "javascript:Sam.Produccion.AbrePopupSpoolRO('{0}');";

        protected void Page_Load(object sender, EventArgs e)
        {
                if (!IsPostBack)
                {                    
                    Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.ing_IngenieriaProyecto);
                    ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
                    wndFijarPrioridad.Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize;
                    //wndAprobadoParaCruce.Behaviors = Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize;
                    if (Request.QueryString["PID"] != null)
                    {
                        _proyectoID = Request.QueryString["PID"].SafeIntParse();
                        ddlProyecto.SelectedValue = _proyectoID.SafeStringParse();
                        proyEncabezado.Visible = true;
                        proyEncabezado.BindInfo(_proyectoID);
                        MuestraGrid();
                    }
                }
        }

        protected void grdSpools_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            _proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (_proyectoID > 0)
            {
                proyEncabezado.Visible = true;
                proyEncabezado.BindInfo(_proyectoID);
            }
            else
            {
                proyEncabezado.Visible = false;
                
            }

        }
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            MuestraGrid();
        }

        protected void MuestraGrid()
        {
            configurarColumnasProyecto();
            establecerDataSource();
            grdSpools.Visible = true;
            grdSpools.DataBind();
        }

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
                        
                       // EntityID = spoolID;
                        
                        SAM.Entities.Personalizadas.DetSpool spool = SpoolBO.Instance.ObtenerDetalleCompleto(spoolID);

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
            

            if (e.CommandName == "Borrar")
            {
                int spoolID = e.CommandArgument.SafeIntParse();

                try
                {
                    SpoolBO.Instance.Borra(spoolID);
                    establecerDataSource();
                    grdSpools.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }

            if (e.CommandName == "ActualizarDatos")
            {
                List<JuntaSpool> peqNoEncontrados = new List<JuntaSpool>();
                List<JuntaSpool> kgtNoEncontrados = new List<JuntaSpool>();
                List<JuntaSpool> espNoEncontrados = new List<JuntaSpool>();

                //Se mandan actualizar los datos Peq, KgT, Esp
                IngenieriaBL.Instance.ActualizarPeqKgtEsp(_proyectoID, out peqNoEncontrados, out kgtNoEncontrados, out espNoEncontrados);
                
                //Se necesita hacer un rebind
                grdSpools.Rebind();

                if (peqNoEncontrados.Count() > 0 || kgtNoEncontrados.Count() > 0 || espNoEncontrados.Count() > 0)
                {
                    Session["Session.PeqsNoEncontrados"] = peqNoEncontrados;
                    Session["Session.KgtNoEncontrados"] = kgtNoEncontrados;
                    Session["Session.EspNoEncontrados"] = espNoEncontrados;

                    //Abrimos el popUp para mostrar el detalle de los datos no encontrados
                    ajaxMgr.ResponseScripts.Add("Sam.Ingenieria.AbrePopUpDatosNoEncontrados();");
                    
                    
                }
            }
        }

        protected void grdSpools_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                //HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                GridDataItem dataItem = (GridDataItem)e.Item;
                GrdIngSpool ingSpool = (GrdIngSpool)e.Item.DataItem;

                int idSpool = ingSpool.SpoolID;//dataItem["SpoolID"].Text.SafeIntParse();
                string nombreSpool = ingSpool.Nombre;//dataItem["Spool"].Text;
                //edita.NavigateUrl = String.Format("/Ingenieria/DetSpool.aspx?ID={0}", idSpool);
                HyperLink lnkPonerHold = e.Item.FindControl("hypPonerHold") as HyperLink;
                HyperLink lnkQuitarHold = e.Item.FindControl("hypQuitarHold") as HyperLink;
                string jsLink = string.Format("javascript:Sam.Ingenieria.AbrePopupDetalleHold('{0}','{1}');", idSpool, TipoHoldSpool.INGENIERIA);
                //CheckBox chkTieneHold = (CheckBox)dataItem["TieneHoldIngenieria"].Controls[0];

                //Configurar la liga para ver del detalle del spool
                HyperLink hlSpool = (HyperLink)dataItem["hypDetalle"].FindControl("hyDetalle");
                hlSpool.Text = nombreSpool;
                hlSpool.NavigateUrl = string.Format(JS_POPUP_SPOOL, idSpool);

                if (ingSpool.TieneHold)
                {
                    lnkPonerHold.Visible = false;
                    lnkQuitarHold.Visible = true;
                    lnkQuitarHold.NavigateUrl = jsLink;
                }
                else
                {
                    lnkQuitarHold.Visible = false;
                    lnkPonerHold.Visible = true;
                    lnkPonerHold.NavigateUrl = jsLink;
                }
            }
        }

        /// <summary>
        /// Método que muestra las columnas personalizadas del proyecto par la nomenclatura del spool
        /// </summary>
        private void configurarColumnasProyecto()
        {
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == _proyectoID).Single();

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

        private void establecerDataSource()
        {
            grdSpools.DataSource = SpoolBO.Instance.ObtenerIngPorProyecto(_proyectoID);
        }

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdSpools.ResetBind();
            establecerDataSource();
            grdSpools.DataBind();
        }

        protected void grdSpools_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;

                HyperLink lnkPrioridadSeleccionados = (HyperLink)item.FindControl("lnkPrioridadSeleccionados");
                HyperLink imgPrioridadSeleccionados = (HyperLink)item.FindControl("imgPrioridadSeleccionados");

                string jsLinkSeleccionados = string.Format("javascript:Sam.Produccion.AbrePopupFijarPrioridadSeleccionados('{0}','{1}');",
                                               grdSpools.ClientID, _proyectoID);

                lnkPrioridadSeleccionados.NavigateUrl = jsLinkSeleccionados;
                imgPrioridadSeleccionados.NavigateUrl = jsLinkSeleccionados;

                HyperLink lnkPrioridad = (HyperLink)item.FindControl("lnkPrioridad");
                HyperLink imgPrioridad = (HyperLink)item.FindControl("imgPrioridad");

                string jsLinkPrioridad = string.Format("javascript:Sam.Ingenieria.AbrePopupFijarPrioridad('{0}');",
                                              wndFijarPrioridad.ClientID);

                lnkPrioridad.NavigateUrl = jsLinkPrioridad;
                imgPrioridad.NavigateUrl = jsLinkPrioridad;

                HyperLink lnkAprobadoParaCruce = (HyperLink)item.FindControl("lnkAprobadoParaCruce");
                HyperLink imgAprobadoParaCruce = (HyperLink)item.FindControl("imgAprobadoParaCruce");

                string jsLinkCruce = string.Format("javascript:Sam.Ingenieria.AbrePopupAprobadoParaCruce('{0}');",
                                              wndAprobadoParaCruce.ClientID);

                lnkAprobadoParaCruce.NavigateUrl = jsLinkCruce;
                imgAprobadoParaCruce.NavigateUrl = jsLinkCruce;

                HyperLink lnkDocumentoAprobado = (HyperLink)item.FindControl("lnkDocumentoAprobado");
                HyperLink imgDocumentoAprobado = (HyperLink)item.FindControl("imgDocumentoAprobado");

                string jsLinkDoc = string.Format("javascript:Sam.Ingenieria.AbrePopupDocumentoAprobado('{0}','{1}');",
                                              grdSpools.ClientID, _proyectoID);

                lnkDocumentoAprobado.NavigateUrl = jsLinkDoc;
                imgDocumentoAprobado.NavigateUrl = jsLinkDoc;

                HyperLink lnkActualizaSpool = (HyperLink)item.FindControl("lnkActualizaSpool");
                HyperLink imgActualizaSpool = (HyperLink)item.FindControl("imgActualizaSpool");

                lnkActualizaSpool.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.CAMPO_SPOOL, _proyectoID);
                imgActualizaSpool.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.CAMPO_SPOOL, _proyectoID);

                HyperLink lnkPlanchadoPrioridades = (HyperLink)item.FindControl("lnkPlanchadoPrioridades");
                HyperLink imgPlanchadoPrioridades = (HyperLink)item.FindControl("imgPlanchadoPrioridades");

                lnkPlanchadoPrioridades.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.PRIORIDADES_SPOOL, _proyectoID);
                imgPlanchadoPrioridades.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.PRIORIDADES_SPOOL, _proyectoID);
            }
        }
        
        protected void btnFijarPrioridad_Click(object sender, EventArgs e)
        {
            //validar los controles de prioridad
            Page.Validate("vgPrioridad");

            if (IsValid)
            {
                IQueryable<GrdIngSpool> lst = SpoolBO.Instance.ObtenerIngPorProyecto(_proyectoID).AsQueryable();

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
                                                    _proyectoID,
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

        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            establecerDataSource();
            grdSpools.DataBind();
        }        

        protected void btnAprobadoParaCruce_Click(object sender, EventArgs e)
        {
            IQueryable<GrdIngSpool> lst = SpoolBO.Instance.ObtenerIngPorProyecto(_proyectoID).AsQueryable();

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
                                                       _proyectoID,
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
    }
}