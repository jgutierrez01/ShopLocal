using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using Telerik.Web.UI;

namespace SAM.Web.Calidad
{
    public partial class SeguimientoSpool : SamPaginaPrincipal
    {

        #region valores filtros

        private static int ProyectoID
        {
            get
            {
                if (HttpContext.Current.Request.Params["PID"] != null)
                    return HttpContext.Current.Request.Params["PID"].SafeIntParse();
                return -1;
            }
        }

        private int? OrdenTrabajoSpoolID
        {
            get
            {
                if (HttpContext.Current.Request.Params["NC"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Params["NC"]))
                    return HttpContext.Current.Request.Params["NC"].SafeIntParse();
                return null;
            }
        }

        private int? OrdenTrabajoID
        {
            get
            {
                if (HttpContext.Current.Request.Params["OT"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Params["OT"]))
                    return HttpContext.Current.Request.Params["OT"].SafeIntParse();
                return null;
            }
        }

        private bool Embarcados
        {
            get
            {
                if (HttpContext.Current.Request.Params["EM"] != null)
                    return HttpContext.Current.Request.Params["EM"].SafeBoolParse();
                return false;
            }
        }

        
        private static IEnumerable<int> CamposIDs
        {
            get
            {
                if (HttpContext.Current.Request.Params["IDS"] != null)
                {
                    return HttpContext.Current.Request.Params["IDS"].Split(',').ToList().Select(x => x.SafeIntParse()).ToArray();
                }
                return new[] { 3, 4, 8, 40, 42, 53, 55 };
            }
        }

        #endregion

        #region Propiedades

        private IEnumerable<ModuloSeguimientoSpoolCache> _modulosSeguimientoSpool;
        private IEnumerable<CampoSeguimientoSpoolCache> _camposSeguimientoSpool;

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            //Establecemos la Cultura del Control RadFilter
            RadFilter1.Culture = new CultureInfo(LanguageHelper.CustomCulture);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _modulosSeguimientoSpool = CacheCatalogos.Instance.ObtenerModulosSeguimientoSpool().ToList();
            _camposSeguimientoSpool = CacheCatalogos.Instance.ObtenerCamposSeguimientoSpool().ToList();

            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cal_SeguimientoSpools);
                proyHeader.BindInfo(ProyectoID);
                arreglarOrdenColumnas();

                //agregamos las columnas invisibles que haran que funcione el grid expression filter
                _camposSeguimientoSpool.Where(y => CamposIDs.Contains(y.ID)).ToList().ForEach(
                    x =>
                    grdSegSpool.Columns.Add(new GridBoundColumn
                                                {
                                                    UniqueName = x.NombreColumnaSp + "_h",
                                                    DataField = x.NombreColumnaSp,
                                                    Visible = false
                                                }));

                //Configuramos nomenclatura segun el proyecto
                configurarColumnasProyecto();

                //establecemos el datasource del grid
                grdSegSpool.Rebind();

            }

            //Como perdemos viewstate de las columnas agregadas dinamicamente, establecemos que siempre sean invisibles
            grdSegSpool.Columns.Cast<GridColumn>().Where(x => x.UniqueName.EndsWith("_h")).ToList().ForEach(
                x =>
                    {
                        x.Visible = false;
                        GridBoundColumn gridBoundColumn = x as GridBoundColumn;
                        if(gridBoundColumn != null)
                        {
                            gridBoundColumn.DataField = x.UniqueName.Substring(0, x.UniqueName.LastIndexOf("_"));    
                        }
                        
                    });
            //Establecemos la Cultura del Control RadFilter
            RadFilter1.Culture = new CultureInfo(LanguageHelper.CustomCulture);
        }

        /// <summary>
        /// Establece el orden de las columnas segun Modulo OrdenUI
        /// </summary>
        private void arreglarOrdenColumnas()
        {

            int[] modulosIDs =
                _camposSeguimientoSpool.Where(y => CamposIDs.Contains(y.ID)).Select(y => y.ModuloSeguimientoSpoolID).
                    ToArray();
            List<ModuloSeguimientoSpoolCache> modulos =
                _modulosSeguimientoSpool.Where(
                    x =>
                    modulosIDs.
                        Contains(x.ID)).ToList();

            List<string> nombresColumnas = modulos.Select(x => x.NombreTemplateColumn.ToLower()).ToList();

            int columnasPrevias = grdSegSpool.Columns.Cast<GridColumn>().First(x =>
                                                                               nombresColumnas.Contains(
                                                                                   x.UniqueName.ToLower())).OrderIndex;

            grdSegSpool.Columns.Cast<GridColumn>().Where(x => nombresColumnas.Contains(x.UniqueName.ToLower())).ToList()
                .ForEach(
                    x =>
                        {
                            x.OrderIndex =
                                modulos.First(y => y.NombreTemplateColumn.EqualsIgnoreCase(x.UniqueName)).OrdenUI +
                                columnasPrevias - 1;
                        });
            
        }

        /// <summary>
        /// Metodo que se dispara justo antes de que la el control RadFilter tenga que ser rendereado al cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadFilter1_PreRender(object sender, EventArgs e)
        {
            //Con esto hacemos que el context menu(en este caso son los nombres de las columnas) aparezcan en un div scrollable
            RadContextMenu menu = RadFilter1.FindControl("rfContextMenu") as RadContextMenu;
            menu.DefaultGroupSettings.Height = Unit.Pixel(350);
            menu.EnableAutoScroll = true;
            IEnumerable<RadMenuItem> listaOrdenada = menu.Items.OrderBy(item => item.Text).ToList();
            menu.Items.Clear();
            menu.Items.AddRange(listaOrdenada);
        }
        

        /// <summary>
        /// Metodo que se dispara cuando el usuario presiona el link de actualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdSegSpool.Rebind();
        }

        /// <summary>
        /// Metodo que se dispara cuando el grid esta apunto de hacer un databind, o si se mando ejecutar el rebind del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdSegSpool_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            grdSegSpool.DataSource = SeguimientoSpoolBL.Instance.ObtenerDataSetParaSeguimientoSpool(ProyectoID,
                                                                                                    OrdenTrabajoID,
                                                                                                    OrdenTrabajoSpoolID,
                                                                                                    null,
                                                                                                    "GeneralNumeroDeControl ASC, GeneralNumeroJuntas DESC");
        }



        /// <summary>
        /// Metodo que se dispara para cada item que este asociado al grid cuando se establece el datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSegSpool_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
            
            int[] modulosIDs =
                _camposSeguimientoSpool.Where(y => CamposIDs.Contains(y.ID)).Select(y => y.ModuloSeguimientoSpoolID).
                    ToArray();
            List<ModuloSeguimientoSpoolCache> modulos =
                _modulosSeguimientoSpool.Where(
                    x =>
                    modulosIDs.
                        Contains(x.ID)).ToList();


            if (e.Item.IsItem())
            {
                //Obtenemos el Link y establecemos el URL hacia la funcion que abrirá el PopUp de Seguimiento
                HyperLink lnkDetalle = e.Item.FindControl("hlPopUp") as HyperLink;
                dr  = (DataRowView)e.Item.DataItem;
                string jsLink = string.Format("javascript:Sam.Calidad.AbrePopUpDetalleSeguimientoSpool('{0}');",
                                              dr["GeneralSpoolID"].SafeIntParse());
                lnkDetalle.NavigateUrl = jsLink;

                
                grdSegSpool.Columns.Cast<GridColumn>().Where(
                    x => modulos.Any(z => z.NombreTemplateColumn.EqualsIgnoreCase(x.UniqueName))).ToList().
                    ForEach(y =>
                    {
                        ModuloSeguimientoSpoolCache modulo =
                            modulos.Single(x => y.UniqueName.EqualsIgnoreCase(x.NombreTemplateColumn));

                        Repeater repeater = e.Item.FindControl("repCampo" + y.UniqueName) as Repeater;
                        if (repeater != null)
                        {
                            IEnumerable<CampoSeguimientoSpoolCache> datasource =
                                _camposSeguimientoSpool.Where(
                                    z =>
                                    CamposIDs.Contains(z.ID) &&
                                    z.ModuloSeguimientoSpoolID == modulo.ModuloSeguimientoSpoolID).OrderBy(
                                        z => z.OrdenUI).ToList();
                            repeater.DataSource = datasource;

                            repeater.DataBind();
                        }


                        y.Visible = true;

                    });
                

            }else if (e.Item.IsHeader())
            {
                

                grdSegSpool.Columns.Cast<GridColumn>().Where(
                    x => modulos.Any(z => z.NombreTemplateColumn.EqualsIgnoreCase(x.UniqueName))).ToList().
                    ForEach(y =>
                                {
                                    ModuloSeguimientoSpoolCache modulo =
                                        modulos.Single(x => y.UniqueName.EqualsIgnoreCase(x.NombreTemplateColumn));

                                    Repeater repeater = e.Item.FindControl("repHeader" + y.UniqueName) as Repeater;
                                    if (repeater != null)
                                    {
                                        IEnumerable<CampoSeguimientoSpoolCache> datasource = _camposSeguimientoSpool.
                                            ToList().Where(
                                                x =>
                                                x.ModuloSeguimientoSpoolID == modulo.ModuloSeguimientoSpoolID &&
                                                CamposIDs.Contains(x.CampoSeguimientoSpoolID)).OrderBy(z => z.OrdenUI);
                                        repeater.DataSource = datasource;
                                            
                                        repeater.DataBind();
                                        y.HeaderStyle.Width =
                                            datasource.Sum(x => x.AnchoUI.HasValue ? x.AnchoUI.Value : 0);
                                        y.ItemStyle.Width = datasource.Sum(x => x.AnchoUI.HasValue ? x.AnchoUI.Value : 0);
                                    }


                                    y.Visible = true;

                                });

            }
        }


        /// <summary>
        /// Metodo que se dispara cuando alguno de los items del grid provoca un comando
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdSegSpool_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "FilterRadGrid")
            {
                RadFilter1.FireApplyCommand();
            }
        }
        
        /// <summary>
        /// Método que muestra las columnas personalizadas del proyecto para la nomenclatura del spool
        /// </summary>
        private void configurarColumnasProyecto()
        {
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == ProyectoID).Single();

            if (p.ColumnasNomenclatura > 0)
            {
                foreach (NomenclaturaStruct n in p.Nomenclatura)
                {
                    //GridColumn col = grdSegSpool.MasterTableView.Columns.FindByUniqueName("Segmento" + n.Orden);
                    //col.HeaderText = n.NombreColumna;
                    //col.Visible = true;
                    //col.HeaderStyle.Width = Unit.Pixel(100);
                    //col.FilterControlWidth = Unit.Pixel(60);
                }
            }
        }

        /// <summary>
        /// Usamos este método para configurar dinámicamente los links del header del grid
        /// </summary>
        protected void grdSegSpool_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkExportarExcelSpool = (HyperLink)commandItem.FindControl("hlExportarSpool");
                HyperLink hlExportaImagen = (HyperLink)commandItem.FindControl("hlExportaImagenSpool");
                
                string url = string.Format(WebConstants.ProduccionUrl.ExportaExcelCalidad,
                                           ProyectoID,
                                           (int) TipoArchivoExcel.SeguimientoSpools, OrdenTrabajoSpoolID, Embarcados,
                                           OrdenTrabajoID, HttpContext.Current.Request.Params["IDS"]);

                lnkExportarExcelSpool.NavigateUrl = url;
                hlExportaImagen.NavigateUrl = url;
            }
        }

        private DataRowView dr = null;

        protected void repCampo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                CampoSeguimientoSpoolCache campoSeguimientoSpool = e.Item.DataItem as CampoSeguimientoSpoolCache;

                Literal literal = e.Item.FindControl("litCampo") as Literal;
                object valorCampo = dr[campoSeguimientoSpool.NombreColumnaSp];
                
                if (valorCampo != null)
                {
                    bool esBoleano = dr[campoSeguimientoSpool.NombreColumnaSp].GetType() == typeof(bool);
                    literal.Text = @"<td class=""" + campoSeguimientoSpool.CssColumnaUI + @""" width=""" + campoSeguimientoSpool.AnchoUI + @""" >";
                    if (!esBoleano)
                    {
                        if (!string.IsNullOrEmpty(campoSeguimientoSpool.DataFormat))
                        {
                            #region dataFormat complejo
                            /*
                            //Data format viene  "Aprobado/No Aprobado|Passed/Failed"
                            if (campoSeguimientoSpool.DataFormat.IndexOf("|") != 1 && campoSeguimientoSpool.DataFormat.IndexOf("/") != -1)
                            {
                                if(valorCampo.SafeIntParse() == 1)
                                {
                                    literal.Text += LanguageHelper.CustomCulture == LanguageHelper.INGLES
                                                        ? campoSeguimientoSpool.DataFormat.Split('|')[1].Split('/')[0]
                                                        : campoSeguimientoSpool.DataFormat.Split('|')[0].Split('/')[0];
                                }else
                                {
                                    literal.Text += LanguageHelper.CustomCulture == LanguageHelper.INGLES
                                                        ? campoSeguimientoSpool.DataFormat.Split('|')[1].Split('/')[1]
                                                        : campoSeguimientoSpool.DataFormat.Split('|')[0].Split('/')[1];
                                }
                            }
                            else
                            {
                                literal.Text += string.Format("{0:" + campoSeguimientoSpool.DataFormat + "}", valorCampo);
                            }
                             * */
                            #endregion
                            literal.Text += string.Format("{0:" + campoSeguimientoSpool.DataFormat + "}", valorCampo);
                        }
                        else
                        {
                            

                            if (campoSeguimientoSpool.Nombre == "Tipos de Juntas" || campoSeguimientoSpool.Nombre == "Clasificación PND" || campoSeguimientoSpool.Nombre == "Avance PND")
                            {
                                XmlDocument xmlCampo = new XmlDocument();
                                xmlCampo.LoadXml(valorCampo.ToString().Trim());
                                XmlNodeList lista = xmlCampo.GetElementsByTagName("Junta");
                                int anchoColumna = 50 * lista.Count;
                                //XmlNode nodo = lista[0];
                                string campos = "";
                                
                                    if (campoSeguimientoSpool.Nombre == "Tipos de Juntas")
                                    {
                                        foreach (XmlNode nodo in lista)
                                        {
                                            campos += @"<td style=""width: 50px"">" + nodo.Attributes["Cantidad"].Value + "</td>";
                                        }
                                    }
                                    if (campoSeguimientoSpool.Nombre == "Clasificación PND")
                                    {
                                        foreach (XmlNode nodo in lista)
                                        {
                                            campos += @"<td style=""width: 50px"">" + nodo.Attributes["Clasificacion"].Value + "</td>";
                                        }
                                    }
                                    if (campoSeguimientoSpool.Nombre == "Avance PND")
                                    {
                                        foreach (XmlNode nodo in lista)
                                        {
                                            campos += @"<td style=""width: 50px"">" + nodo.Attributes["Avance"].Value + "</td>";
                                        }
                                    }

                                    literal.Text = @"<td class=""" + campoSeguimientoSpool.CssColumnaUI + @""" style=""width: " + anchoColumna + @"px"" >" +
                                                    @"<table width=""" + anchoColumna + @""">" +
                                                        @"<tr>" +
                                                            campos +
                                                        @"</tr>" +
                                                    @"</table>" +
                                                @"</td>";
                            }
                            else
                            {
                                literal.Text += valorCampo.ToString().Trim();
                            }
                        }
                    }else
                    {
                        literal.Text += TraductorEnumeraciones.TextoSiNo(valorCampo.SafeBoolParse());
                    }
                    literal.Text += @"</td>";
                }

            }
        }

        protected void repHeaderSegmento_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsHeader())
            {
                procesaHeader(sender, e);
            }
            else if (e.Item.IsItem())
            {
                ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == ProyectoID).Single();
                CampoSeguimientoSpoolCache campoSeguimientoSpool = e.Item.DataItem as CampoSeguimientoSpoolCache;
                Literal literal = e.Item.FindControl("litHeaderNombre") as Literal;
                string orden = campoSeguimientoSpool.Nombre.Reverse().ElementAt(0).ToString();
                string nombreColumna = campoSeguimientoSpool.Nombre;


                if(p.Nomenclatura.Any(x => x.Orden.ToString() == orden ))
                {
                    nombreColumna = p.Nomenclatura.Single(x => x.Orden.ToString() == orden).NombreColumna;
                }
                
                literal.Text = @"<td class=""" + campoSeguimientoSpool.CssColumnaUI + @""" style=""width: " +
                               campoSeguimientoSpool.AnchoUI + @"px"" >" +
                              nombreColumna + @"</td>";
            }
        }

        private void procesaHeader(object sender, RepeaterItemEventArgs e)
        {
            IEnumerable<CampoSeguimientoSpoolCache> datasource =
                    ((IEnumerable<CampoSeguimientoSpoolCache>) ((Repeater) sender).DataSource);

                CampoSeguimientoSpoolCache campoSeguimientoSpool = datasource.First();

                Literal literalTitulo = e.Item.FindControl("litHeaderTitulo") as Literal;
                ModuloSeguimientoSpoolCache modulo =
                    _modulosSeguimientoSpool.First(x => x.ID == campoSeguimientoSpool.ModuloSeguimientoSpoolID);
                string nombreHeader = modulo.Nombre;

                if (!string.IsNullOrEmpty(nombreHeader))
                {
                    literalTitulo.Text = @"<th colspan=""" + datasource.Count() + @"""  class=""" +
                                         campoSeguimientoSpool.CssColumnaUI + @""" align=""center"" > " + nombreHeader +
                                         @"</th>";
                }
        }

        protected void repHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {   
            
            if(e.Item.IsHeader())
            {
               procesaHeader(sender, e);
            }
            else if (e.Item.IsItem())
            {
                CampoSeguimientoSpoolCache campoSeguimientoSpool = e.Item.DataItem as CampoSeguimientoSpoolCache;
                Literal literal = e.Item.FindControl("litHeaderNombre") as Literal;


                if (campoSeguimientoSpool.Nombre == "Tipos de Juntas" || campoSeguimientoSpool.Nombre == "Clasificación PND" || campoSeguimientoSpool.Nombre == "Avance PND")
                {
                    List<TipoJunta> tiposJuntas = TipoJuntaBO.Instance.ObtenerTodos();
                    string campos = "";
                    foreach (TipoJunta tipoj in tiposJuntas)
                    {
                        campos += @"<td style=""width: 50px"">" + tipoj.Codigo + "</td>";
                    }

                    int anchoColumna = 50 * tiposJuntas.Count();

                    string tabla = @"<td class=""" + campoSeguimientoSpool.CssColumnaUI + "\" style=\"width: \"" + anchoColumna + "\"px\">" +
                                        @"<table style=""width:" + anchoColumna +@""">" +
                                            @"<tr>" +
                                                @"<th colspan="""+ tiposJuntas.Count() + @""">" + campoSeguimientoSpool.Nombre + "</th>" +
                                            @"</tr>" +
                                            @"<tr>" +
                                                campos +
                                            @"</tr>" +
                                        @"</table>" +
                                    @"</td>";
                    literal.Text = tabla;  
                }
                else
                {
                    literal.Text = @"<td class=""" + campoSeguimientoSpool.CssColumnaUI + @""" style=""width: " +
                               campoSeguimientoSpool.AnchoUI + @"px"" >" + campoSeguimientoSpool.Nombre + @"</td>";
                }

            }

        }

        
    }
}