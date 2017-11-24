using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Web.Classes;
using Telerik.Web.UI;

namespace SAM.Web.Calidad
{
    public partial class SeguimientoJuntas : SamPaginaPrincipal
    {
        private IEnumerable<ModuloSeguimientoJuntaCache> _modulosSeguimientoJunta;
        private IEnumerable<CampoSeguimientoJuntaCache> _camposSeguimientoJunta;

        #region valores filtros
        
        private int ProyectoID
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

        private bool HistorialRep
        {
            get
            {
                if (HttpContext.Current.Request.Params["HR"] != null)
                    return HttpContext.Current.Request.Params["HR"].SafeBoolParse();
                return false;
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
                return new[] { 3, 4, 8, 40, 42, 53, 55, 102 };
            }
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            //Establecemos la Cultura del Control RadFilter
            RadFilter1.Culture = new CultureInfo(LanguageHelper.CustomCulture);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Inicializamos las variables de instancia
            _modulosSeguimientoJunta = CacheCatalogos.Instance.ObtenerModulosSeguimientoJunta().ToList();
            _camposSeguimientoJunta = CacheCatalogos.Instance.ObtenerCamposSeguimientoJunta().ToList();

            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cal_SeguimientoJuntas);

                proyHeader.BindInfo(ProyectoID);
                arreglarOrdenColumnas();

                //agregamos las columnas invisibles que haran que funcione el grid expression filter
                _camposSeguimientoJunta.Where(y => CamposIDs.Contains(y.ID)).ToList().ForEach(
                    x =>
                    grdSegJunta.Columns.Add(new GridBoundColumn
                    {
                        UniqueName = x.NombreColumnaSp + "_h",
                        DataField = x.NombreColumnaSp,
                        Visible = false
                    }));
            }

            //Como perdemos viewstate de las columnas agregadas dinamicamente, establecemos que siempre sean invisibles
            grdSegJunta.Columns.Cast<GridColumn>().Where(x => x.UniqueName.EndsWith("_h")).ToList().ForEach(
                 x =>
                 {
                     x.Visible = false;
                     //Con esto las columnas que agregamos establecemos su datafield, ya que tmb por viewstate los perdimos 
                     GridBoundColumn gridBoundColumn = x as GridBoundColumn;
                     if (gridBoundColumn != null)
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
                _camposSeguimientoJunta.Where(y => CamposIDs.Contains(y.ID)).Select(y => y.ModuloSeguimientoJuntaID).
                    ToArray();
            List<ModuloSeguimientoJuntaCache> modulos =
                _modulosSeguimientoJunta.Where(
                    x =>
                    modulosIDs.
                        Contains(x.ID)).ToList();

            List<string> nombresColumnas = modulos.Select(x => x.NombreTemplateColumn.ToLower()).ToList();

            int columnasPrevias = grdSegJunta.Columns.Cast<GridColumn>().First(x =>
                                                                               nombresColumnas.Contains(
                                                                                   x.UniqueName.ToLower())).OrderIndex;

            grdSegJunta.Columns.Cast<GridColumn>().Where(x => nombresColumnas.Contains(x.UniqueName.ToLower())).ToList()
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
            IEnumerable<RadMenuItem> listaOrdenada = menu.Items.OrderBy(item => item.Text).ToList();
            menu.Items.Clear();
            menu.Items.AddRange(listaOrdenada);
            menu.EnableAutoScroll = true;
        }

        /// <summary>
        /// Metodo que se dispara cuando el usuario presiona el link de actualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdSegJunta.Rebind();
        }

        /// <summary>
        /// Metodo que se dispara cuando el grid esta apunto de hacer un databind, o si se mando ejecutar el rebind del grid
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdSegJunta_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            DataSet ds = SeguimientoJuntaBL.Instance.ObtenerDataSetParaSeguimientoJunta(ProyectoID,
                                                                                        OrdenTrabajoID,
                                                                                        OrdenTrabajoSpoolID,
                                                                                        null,
                                                                                        HistorialRep);

            DataRow [] rows = ds.Tables[0].Select(string.Empty, "GeneralNumeroDeControl ASC, GeneralSpool ASC, GeneralJunta ASC");

            grdSegJunta.DataSource = rows;
        }

        /// <summary>
        /// Metodo que se dispara para cada item que este asociado al grid cuando se establece el datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSegJunta_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //Obtenemos los Ids de los modulos de seguimiento que debemos mostrar
            int[] modulosIDs =
                _camposSeguimientoJunta.Where(y => CamposIDs.Contains(y.ID)).Select(y => y.ModuloSeguimientoJuntaID).
                    ToArray();

            //obtenemos los modulos que debemos mostrar
            List<ModuloSeguimientoJuntaCache> modulos =
                _modulosSeguimientoJunta.Where(x => modulosIDs.Contains(x.ID)).ToList();


            //Si es un item
            if (e.Item.IsItem())
            {
                HyperLink lnkDetalle = e.Item.FindControl("hlPopUp") as HyperLink;
                DataRowView item = (DataRowView)e.Item.DataItem;
                string jsLink = string.Format("javascript:Sam.Calidad.AbrePopUpDetalleSeguimientoJunta('{0}','{1}');",
                                              item["GeneralJuntaWorkstatusID"], ProyectoID);
                lnkDetalle.NavigateUrl = jsLink;
                dr = (DataRowView)e.Item.DataItem;

                //Para cada columna del Grid que este dentro de los modulos que se deben mostrar
                grdSegJunta.Columns.Cast<GridColumn>().Where(
                    x => modulos.Any(z => z.NombreTemplateColumn.EqualsIgnoreCase(x.UniqueName))).ToList().
                    ForEach(y =>
                    {
                        //Obtenemos el modulo que debemos mostrar
                        ModuloSeguimientoJuntaCache modulo =
                            modulos.Single(x => y.UniqueName.EqualsIgnoreCase(x.NombreTemplateColumn));

                        //Obtenemos el repeater para generar los TDs en este renglon, de este modulo 
                        Repeater repeater = e.Item.FindControl("repCampo" + y.UniqueName) as Repeater;
                        if (repeater != null)
                        {
                            //Establecemos el datasource del repeater a los campos seleccionados que pertenezcan a este modulo
                            IEnumerable<CampoSeguimientoJuntaCache> datasource =
                                _camposSeguimientoJunta.Where(
                                    z =>
                                    CamposIDs.Contains(z.ID) &&
                                    z.ModuloSeguimientoJuntaID == modulo.ModuloSeguimientoJuntaID).OrderBy(
                                        z => z.OrdenUI).ToList();
                            repeater.DataSource = datasource;

                            repeater.DataBind();
                        }

                        //Hacemos esta columna visible
                        y.Visible = true;

                    });
            }
            else if (e.Item.IsHeader())
            {
                //si son los headers

                //
                grdSegJunta.Columns.Cast<GridColumn>().Where(
                    x => modulos.Any(z => z.NombreTemplateColumn.EqualsIgnoreCase(x.UniqueName))).ToList().
                    ForEach(y =>
                    {
                        ModuloSeguimientoJuntaCache modulo =
                            modulos.Single(x => y.UniqueName.EqualsIgnoreCase(x.NombreTemplateColumn));

                        Repeater repeater = e.Item.FindControl("repHeader" + y.UniqueName) as Repeater;
                        if (repeater != null)
                        {
                            IEnumerable<CampoSeguimientoJuntaCache> datasource = _camposSeguimientoJunta.
                                ToList().Where(
                                    x =>
                                    x.ModuloSeguimientoJuntaID == modulo.ModuloSeguimientoJuntaID &&
                                    CamposIDs.Contains(x.CampoSeguimientoJuntaID)).OrderBy(z => z.OrdenUI);
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
        protected void grdSegJunta_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "FilterRadGrid")
            {
                RadFilter1.FireApplyCommand();
            } 
        }
        
        /// <summary>
        /// Usamos este método para configurar dinámicamente los links del header del grid
        /// </summary>
        protected void grdSegJunta_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkExportarExcelJunta = (HyperLink)commandItem.FindControl("hlExportarJunta");
                HyperLink hlExportaImagen = (HyperLink)commandItem.FindControl("hlExportaImagenJunta");

                string url = string.Format(WebConstants.ProduccionUrl.ExportaExcelJuntaCalidad,
                                            ProyectoID,
                                            (int)TipoArchivoExcel.SeguimientoJuntas, OrdenTrabajoSpoolID, Embarcados, OrdenTrabajoID, HistorialRep);

                if (lnkExportarExcelJunta != null)
                {
                    lnkExportarExcelJunta.NavigateUrl = url;
                    hlExportaImagen.NavigateUrl = url;
                }
            }
        }

        private DataRowView dr = null;

        protected void repCampo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                CampoSeguimientoJuntaCache campoSeguimientoJunta = e.Item.DataItem as CampoSeguimientoJuntaCache;
                Literal literal = e.Item.FindControl("litCampo") as Literal;

                object valorCampo = dr[campoSeguimientoJunta.NombreColumnaSp];

                if (valorCampo != null)
                {
                    bool esBoleano = dr[campoSeguimientoJunta.NombreColumnaSp].GetType() == typeof(bool);
                    string td = @"<td class=""{0}"" style=""width:{1}px; max-width:{1}px; min-width:{1}px"">";
                    literal.Text =  string.Format(td, campoSeguimientoJunta.CssColumnaUI , campoSeguimientoJunta.AnchoUI);
                    if (!esBoleano)
                    {
                        if (!string.IsNullOrEmpty(campoSeguimientoJunta.DataFormat))
                        {
                            literal.Text += string.Format("{0:" + campoSeguimientoJunta.DataFormat + "}", valorCampo);
                        }else
                        {
                            literal.Text += valorCampo.ToString();
                        }
                    }
                    else
                    {
                        literal.Text += TraductorEnumeraciones.TextoSiNo(valorCampo.SafeBoolParse());
                    }
                    literal.Text += @"</td>";
                }

            }
        }

        protected void repHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsHeader())
            {
                IEnumerable<CampoSeguimientoJuntaCache> datasource =
                    ((IEnumerable<CampoSeguimientoJuntaCache>)((Repeater)sender).DataSource);

                CampoSeguimientoJuntaCache campoSeguimientoJunta = datasource.First();

                Literal literalTitulo = e.Item.FindControl("litHeaderTitulo") as Literal;
                ModuloSeguimientoJuntaCache modulo =
                    _modulosSeguimientoJunta.First(x => x.ID == campoSeguimientoJunta.ModuloSeguimientoJuntaID);
                string nombreHeader = modulo.Nombre;

                if (!string.IsNullOrEmpty(nombreHeader))
                {
                    literalTitulo.Text = @"<th colspan=""" + datasource.Count() + @"""  class=""" +
                                         campoSeguimientoJunta.CssColumnaUI + @""" align=""center"" > " + nombreHeader +
                                         @"</th>";
                }

            }
            else if (e.Item.IsItem())
            {
                Repeater rep = (Repeater)sender;
                CampoSeguimientoJuntaCache campoSeguimientoJunta = e.Item.DataItem as CampoSeguimientoJuntaCache;
                Literal literal = e.Item.FindControl("litHeaderNombre") as Literal;

                string td = @"<td class=""{0}"" style=""width:{1}px; max-width:{1}px; min-width:{1}px"">{2}</td>";
                literal.Text = string.Format(td, campoSeguimientoJunta.CssColumnaUI, campoSeguimientoJunta.AnchoUI, campoSeguimientoJunta.Nombre);

               }

        }

        
    }
}
