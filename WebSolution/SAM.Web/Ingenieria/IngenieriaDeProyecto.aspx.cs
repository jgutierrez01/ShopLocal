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
using SAM.Web.Common;
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
using System.Web.Services;
using System.Web.Script.Serialization;
using SAM.BusinessObjects.Excepciones;
using Resources;
using log4net;
using System.Threading;
using System.Diagnostics;


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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(IngenieriaDeProyecto));
        private Stopwatch sw = new Stopwatch();

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
                        _logger.DebugFormat("inicio Page load MuestraGrid");
                        MuestraGrid();
                        _logger.DebugFormat("fin Page load MuestraGrid");
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
            _logger.DebugFormat("INICIO MuestraGrid");
            MuestraGrid();
            _logger.DebugFormat("FIN MuestraGrid");
        }

        protected void MuestraGrid()
        {
            try { 
                configurarColumnasProyecto();
                establecerDataSource();
            }
            catch (Exception ex) {
                _logger.Fatal("Error inesperado MuestraGrid", ex);
            }
            grdSpools.Visible = true;
            grdSpools.DataBind();
        }

        protected void grdSpools_OnItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string cmdName = e.CommandName;
            _logger.DebugFormat("ComandName {0}", cmdName);
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

            if (e.CommandName == "EliminacionMasiva")
            {
                try
                {
                    List<int> listItemSelected = getSpoolIDsSelected(source, e);

                    if (listItemSelected.Count > 0)
                    {
                        //Obtiene los ids de los spools a los cuales se eliminaran
                        List<string> mensajes = SpoolBO.Instance.BorrarSpoolsMasivo(listItemSelected.ToArray(),
                                                       _proyectoID,
                                                       SessionFacade.UserId,
                                                       DateTime.Now);    
            
                        //Volver a hacer el binding pero respetando los filtros y paginación
                        grdSpools.Rebind();   
                                               
                        if (mensajes.Count > 0)
                        {
                            throw new ExcepcionRelaciones(mensajes);
                        }                       
                    }
                }                
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }

            if (e.CommandName == "ActualizarDatos")
            {
                   try
                {
                    List<JuntaSpool> peqNoEncontrados = new List<JuntaSpool>();
                    List<JuntaSpool> kgtNoEncontrados = new List<JuntaSpool>();
                    List<JuntaSpool> espNoEncontrados = new List<JuntaSpool>();

                    Guid userid = SessionFacade.UserId;
                    //Se mandan actualizar los datos Peq, KgT, Esp
                    _logger.DebugFormat("Proceso asyncrono AsyncActualizarPeks");
                    sw.Start();
                    Thread tactualizarPeqKgtEsp = new Thread(() =>
                        AsyncActualizarPeks(_proyectoID, userid));                   
                    tactualizarPeqKgtEsp.Start();
              
                }
                catch (Exception ex)
                {
                    _logger.Fatal("Error no esperado ActualizarDatos", ex);
                }
                              
            }
            if (e.CommandName == "PrioridadSeleccionados")
            { 
                List<int> spoolSelected = getSpoolIDsSelected(source, e);
            
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(spoolSelected);
                string parametros = string.Format("{0},{1}", json, _proyectoID);
                string jsl = string.Format("Sam.Ingenieria.AbrePopupFijarPrioridadSeleccionados({0});", parametros);
                ajaxMgr.ResponseScripts.Add(jsl);   
            }
            if (e.CommandName == "DocumentoAprobado")
            {
                List<int> spoolSelected = getSpoolIDsSelected(source, e);

                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(spoolSelected);
                string parametros = string.Format("{0},{1}", json, _proyectoID);
                string jsl = string.Format("Sam.Ingenieria.AbrePopupDocumentoAprobado ({0});", parametros);
                ajaxMgr.ResponseScripts.Add(jsl);
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
            _logger.DebugFormat("Inicio configurarColumnasProyecto");
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == _proyectoID).Single();
            _logger.DebugFormat("ObtenerProyectos");
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
            _logger.DebugFormat("Fin configurarColumnasProyecto");
        }


        private void AsyncActualizarPeks(int proyectoID, Guid userid)
        {

            _logger.DebugFormat("Inicio ActualizarPeqKgtEsp");
            string imgPath = Server.MapPath("~/Imagenes/Logos/sam_powered_by_small_small.jpg");
            IngenieriaBL.Instance.ActualizarPeqKgtEsp(proyectoID,imgPath, _proyectoID, userid);
        }

       

        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            grdSpools.ResetBind();
            establecerDataSource();
            grdSpools.DataBind();
        }

        private void establecerDataSource()
        {
            _logger.DebugFormat("Inicio establecerDataSource");
            try
            {
                grdSpools.DataSource = SpoolBO.Instance.ObtenerIngPorProyecto(_proyectoID);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Error Inesperado establecerDataSource", ex);
            }

            _logger.DebugFormat("fin establecerDataSource");
        }

        protected void grdSpools_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem item = (GridCommandItem)e.Item;               

                HyperLink lnkPrioridad = (HyperLink)item.FindControl("lnkPrioridad");
                HyperLink imgPrioridad = (HyperLink)item.FindControl("imgPrioridad");

                string jsLinkPrioridad = string.Format("javascript: Sam.Ingenieria.AbrePopupFijarPrioridad('{0}', '{1}');",
                                              wndFijarPrioridad.ClientID, grdSpools.ClientID);

                lnkPrioridad.NavigateUrl = jsLinkPrioridad;
                imgPrioridad.NavigateUrl = jsLinkPrioridad;

                HyperLink lnkAprobadoParaCruce = (HyperLink)item.FindControl("lnkAprobadoParaCruce");
                HyperLink imgAprobadoParaCruce = (HyperLink)item.FindControl("imgAprobadoParaCruce");

                string jsLinkCruce = string.Format("javascript:Sam.Ingenieria.AbrePopupAprobadoParaCruce('{0}');",
                                              wndAprobadoParaCruce.ClientID);

                lnkAprobadoParaCruce.NavigateUrl = jsLinkCruce;
                imgAprobadoParaCruce.NavigateUrl = jsLinkCruce;           

                HyperLink lnkActualizaSpool = (HyperLink)item.FindControl("lnkActualizaSpool");
                HyperLink imgActualizaSpool = (HyperLink)item.FindControl("imgActualizaSpool");

                lnkActualizaSpool.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.CAMPO_SPOOL, _proyectoID);
                imgActualizaSpool.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.CAMPO_SPOOL, _proyectoID);

                HyperLink lnkPlanchadoPrioridades = (HyperLink)item.FindControl("lnkPlanchadoPrioridades");
                HyperLink imgPlanchadoPrioridades = (HyperLink)item.FindControl("imgPlanchadoPrioridades");

                lnkPlanchadoPrioridades.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.PRIORIDADES_SPOOL, _proyectoID);
                imgPlanchadoPrioridades.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.PRIORIDADES_SPOOL, _proyectoID);
 
                LinkButton lnkEliminacionMasiva = (LinkButton)item.FindControl("lnkEliminacionMasiva");
                ImageButton imgEliminacionMasiva = (ImageButton)item.FindControl("imgEliminacionMasiva");

                string jsLinkEliminacion = string.Format("javascript: return Sam.Ingenieria.Validaciones.CheckItemsSelected('{0}');",
                                              grdSpools.ClientID);

                lnkEliminacionMasiva.OnClientClick = jsLinkEliminacion;
                imgEliminacionMasiva.OnClientClick = jsLinkEliminacion;
            }           
        }
           
        protected void btnFijarPrioridad_Click(object sender, EventArgs e)
        {
            //validar los controles de prioridad
            Page.Validate("vgPrioridad");

            if (IsValid)
            {                     
                try
                {
                    List<int> listItemSelected = getSpoolIDsSelected(sender, e);

                    if (listItemSelected.Count > 0)
                    {
                        //Obtiene los ids de los spools a los cuales se le debe fijar la prioridad y manda llamar
                        //el método de la capa de negocios correspondiente
                        SpoolBO.Instance.FijaPrioridad(listItemSelected.ToArray(),
                                                        _proyectoID,
                                                        SessionFacade.UserId,
                                                        DateTime.Now,
                                                        txtPrioridad.Text.SafeIntParse());

                        //Volver a hacer el binding pero respetando los filtros y paginación
                        grdSpools.Rebind();
                    }
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        protected void grdSpools_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
        {
           // grdSpools.PageIndex= e.NewPageIndex;
            grdSpools.Rebind();
        }  

        protected void btnActualiza_Click(object sender, EventArgs e)
        {
            establecerDataSource();
            grdSpools.DataBind();
        }        

        protected void btnAprobadoParaCruce_Click(object sender, EventArgs e)
        { 
            //Obtener listado de spools seleccionados para cruce      
            List<int> lstSpoolsSelected = getSpoolIDsSelected(sender, e);           

            try
            {
                if (lstSpoolsSelected.Count > 0)
                {
                    //Obtiene los ids de los spools a los cuales se le debe fijar la prioridad y manda llamar
                    //el método de la capa de negocios correspondiente
                    SpoolBO.Instance.FijaAprobadoParaCruce(lstSpoolsSelected.ToArray(),
                                                           _proyectoID,
                                                           SessionFacade.UserId,
                                                           DateTime.Now,
                                                           chkAprobadoParaCruce.Checked);

                    //Volver a hacer el binding pero respetando los filtros y paginación
                    grdSpools.Rebind();
                }
                else
                {
                    ajaxMgr.ResponseScripts.Add("Sam.Alerta(7)");
                }
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }     

        protected List<int> getSpoolIDsSelected(object source, EventArgs e)
        {
                List<int> listSelected = new List<int>();

            
                GridDataItem[] items = grdSpools.MasterTableView.GetSelectedItems();

                foreach (GridDataItem di in items)
                {
                    listSelected.Add(Convert.ToInt32(di.GetDataKeyValue("SpoolID").ToString()));
                }

                return listSelected;
        }
 
        protected void ToggleRowSelection(object sender, EventArgs e)
        {
            ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;           
        }

        protected void ToggleSelectedState(object sender, EventArgs e)
        {
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (GridDataItem dataItem in grdSpools.MasterTableView.Items)
            {
                (dataItem.FindControl("rowChkBox") as CheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked;
            }
        }               
    }
}