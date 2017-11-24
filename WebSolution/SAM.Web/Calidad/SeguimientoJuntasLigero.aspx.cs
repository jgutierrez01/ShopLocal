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
using System.Web.UI;
using System.Collections;

namespace SAM.Web.Calidad
{
    public class DefinicionColumna
    {
        public string NombreColumnaSp { get; set; }
        public string Formato { get; set; }
        public int AnchoColumna { get; set; }
    }

    public partial class SeguimientoJuntasLigero : SamPaginaPrincipal
    {
        private List<ModuloSeguimientoJuntaCache> _modulosSeguimientoJunta;
        private List<CampoSeguimientoJuntaCache> _camposSeguimientoJunta;
        private List<DefinicionColumna> _columnasMostrar;
        private List<DefinicionColumna> _columnasCongeladas;
        private DataRowView _juntaActual;
        private object _columnaActual;
        private readonly string  _ordenDefault = "GeneralSpool ASC, GeneralJunta ASC";
        private readonly string _scriptFiltros = "return Sam.Seguimientos.MostrarFiltros('{0}');";

        private const string COLADA_MATERIAL_SOLDADURA = "SoldaduraConsumiblesRelleno";
        private int proyectoIDEtileno = 16;

        #region valores filtros
        
        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private int OrdenTrabajoSpoolID
        {
            get
            {
                return (int)ViewState["OrdenTrabajoSpoolID"];
            }
            set
            {
                ViewState["OrdenTrabajoSpoolID"] = value;
            }
        }

        private int OrdenTrabajoID
        {
            get
            {
                return (int)ViewState["OrdenTrabajoID"];
            }
            set
            {
                ViewState["OrdenTrabajoID"] = value;
            }
        }

        private bool HistorialRep
        {
            get
            {
                return (bool)ViewState["HistorialRep"];
            }
            set
            {
                ViewState["HistorialRep"] = value;
            }
        }

        private bool Embarcados
        {
            get
            {
                return (bool)ViewState["Embarcados"];
            }
            set
            {
                ViewState["Embarcados"] = value;
            }
        }

        private int [] CamposIDs
        {
            get
            {
                return (int [])ViewState["CamposIDs"];
            }
            set
            {
                ViewState["CamposIDs"] = value;
            }
        }

        #endregion

        private List<string> Ordenamientos
        {
            get
            {
                if (ViewState["Ordenamientos"] == null)
                {
                    ViewState["Ordenamientos"] = new List<string>();
                }
                return (List<string>)ViewState["Ordenamientos"];
            }
            set
            {
                ViewState["Ordenamientos"] = value;
            }
        }

        private List<int> CamposCongelados
        {
            get
            {
                if (ViewState["CamposCongelados"] == null)
                {
                    //Por default congelamos spool y junta
                    ViewState["CamposCongelados"] = (from c in _camposSeguimientoJunta
                                                     where  c.NombreColumnaSp.EqualsIgnoreCase("GeneralSpool") || 
                                                            c.NombreColumnaSp.EqualsIgnoreCase("GeneralJunta") 
                                                     select c.CampoSeguimientoJuntaID).ToList();
                }
                return (List<int>)ViewState["CamposCongelados"];
            }
            set
            {
                ViewState["CamposCongelados"] = value;
            }
        }

        private string ValoresFiltro
        {
            get
            {
                if (ViewState["Filtro"] != null)
                {
                    return (string)ViewState["Filtro"];
                }

                return null;
            }
            set
            {
                ViewState["Filtro"] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //Inicializamos las variables de instancia
            _modulosSeguimientoJunta = CacheCatalogos.Instance.ObtenerModulosSeguimientoJunta().ToList();
            _camposSeguimientoJunta = CacheCatalogos.Instance.ObtenerCamposSeguimientoJunta().ToList();

            configuraControlFiltros(filtro);
        }

        private void configuraControlFiltros(RadFilter filterCtrl)
        {
            int[] ids = Request.QueryString["IDS"].Split(',').ToList().Select(x => x.SafeIntParse()).ToArray();

            var grpEncabezado = (from campo in _camposSeguimientoJunta
                                 join modulo in _modulosSeguimientoJunta on campo.ModuloSeguimientoJuntaID equals modulo.ModuloSeguimientoJuntaID
                                 where ids.Contains(campo.CampoSeguimientoJuntaID)
                                 select new
                                 {
                                     NombreModulo = modulo.Nombre,
                                     NombreCampo = campo.Nombre,
                                     NombreColumnaSp = campo.NombreColumnaSp,
                                     OrdenModulo = modulo.OrdenUI,
                                     OrdenCampo = campo.OrdenUI,
                                     TipoDeDato = campo.TipoDeDato
                                 })
                                 .OrderBy(x => x.OrdenModulo)
                                 .ThenBy(x => x.OrdenCampo)
                                 .ToList();

            grpEncabezado.ForEach(x =>
            {
                switch (x.TipoDeDato)
                {
                    case "System.Boolean":
                        filterCtrl.FieldEditors.Add(new RadFilterBooleanFieldEditor { DataType = typeof(bool), DisplayName = x.NombreModulo + " - " + x.NombreCampo, FieldName = x.NombreColumnaSp });
                        break;
                    case "System.DateTime":
                        filterCtrl.FieldEditors.Add(new RadFilterDateFieldEditor { DataType = typeof(DateTime), DisplayName = x.NombreModulo + " - " + x.NombreCampo, FieldName = x.NombreColumnaSp });
                        break;
                    case "System.Decimal":
                        filterCtrl.FieldEditors.Add(new RadFilterNumericFieldEditor { DataType = typeof(decimal), DisplayName = x.NombreModulo + " - " + x.NombreCampo, FieldName = x.NombreColumnaSp });
                        break;
                    case "System.Int32":
                        filterCtrl.FieldEditors.Add(new RadFilterNumericFieldEditor { DataType = typeof(int), DisplayName = x.NombreModulo + " - " + x.NombreCampo, FieldName = x.NombreColumnaSp });
                        break;
                    case "System.String":
                        filterCtrl.FieldEditors.Add(new RadFilterTextFieldEditor { DataType = typeof(string), DisplayName = x.NombreModulo + " - " + x.NombreCampo, FieldName = x.NombreColumnaSp });
                        break;
                }
            });

            filtro.ApplyExpressions += new EventHandler<RadFilterApplyExpressionsEventArgs>(filtro_ApplyExpressions);
            filtro.Culture = new CultureInfo(LanguageHelper.CustomCulture);
        }

        protected void filtro_ApplyExpressions(object sender, RadFilterApplyExpressionsEventArgs e)
        {
            //Intencionalmente vacia
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ProyectoID = Request.QueryString["PID"].SafeIntParse();
                OrdenTrabajoID = Request.QueryString["OT"].SafeIntParse();
                OrdenTrabajoSpoolID = Request.QueryString["NC"].SafeIntParse();
                HistorialRep = Request.QueryString["HR"].SafeBoolParse();
                Embarcados = Request.QueryString["EM"].SafeBoolParse();
                hdnMenuColumnasID.Value = menuColumnas.ClientID;

                if (Request.QueryString.AllKeys.Contains("IDS"))
                {
                    CamposIDs = Request.QueryString["IDS"].Split(',').ToList().Select(x => x.SafeIntParse()).ToArray();
                }
                else
                {
                    //Default?
                    CamposIDs = new int[] { 3, 4, 8, 40, 42, 53, 55, 102 };
                }

                if (ProyectoID == proyectoIDEtileno)
                {
                    CampoSeguimientoJuntaCache coladaMaterialSold = _camposSeguimientoJunta.SingleOrDefault(x => x.NombreColumnaSp == COLADA_MATERIAL_SOLDADURA);
                    _camposSeguimientoJunta.Remove(coladaMaterialSold);
                }


                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cal_SeguimientoJuntas);
                proyHeader.BindInfo(ProyectoID);
                cargaDatos(true);

                lnkMostrarFiltros.OnClientClick = string.Format(_scriptFiltros, wndFiltros.ClientID);
                btnMostrarFiltros.OnClientClick = string.Format(_scriptFiltros, wndFiltros.ClientID);
                btnFiltrar.OnClientClick = string.Format("Sam.Seguimientos.AplicaFiltros('{0}');", wndFiltros.ClientID);

                string url = string.Format(WebConstants.ProduccionUrl.ExportaExcelJuntaCalidad,
                                            ProyectoID,
                                            (int)TipoArchivoExcel.SeguimientoJuntas, OrdenTrabajoSpoolID, Embarcados, OrdenTrabajoID, HistorialRep, ValoresFiltro, Request.QueryString["IDS"]);

                lnkExportarExcel.NavigateUrl = url;
                btnExportarExcel.NavigateUrl = url;
            }
        }

        private void cargaDatos(bool primeraVez)
        {
            string ordenamiento = Ordenamientos.Any() ? string.Join(", ", Ordenamientos.ToArray()) : _ordenDefault;

            DataSet ds =    SeguimientoJuntaBL.Instance
                                              .ObtenerDataSetParaSeguimientoJunta( ProyectoID,
                                                                                   OrdenTrabajoID,
                                                                                   OrdenTrabajoSpoolID,
                                                                                   null,
                                                                                   HistorialRep);

            DataView resultados = (DataView)ds.Tables[0].DefaultView;
            resultados.Sort = ordenamiento;

            if (ValoresFiltro != null)
            {
                resultados.RowFilter = ValoresFiltro;
            }

            string url = string.Format(WebConstants.ProduccionUrl.ExportaExcelJuntaCalidad,
                                            ProyectoID,
                                            (int)TipoArchivoExcel.SeguimientoJuntas, OrdenTrabajoSpoolID, Embarcados, OrdenTrabajoID, HistorialRep, ValoresFiltro, Request.QueryString["IDS"]);

            lnkExportarExcel.NavigateUrl = url;
            btnExportarExcel.NavigateUrl = url;

            DataTable juntas = obtenerRegistrosFiltradosOrdenados(resultados);

            bindEncabezadoTabla(CamposIDs.Length, primeraVez);

            repJuntas.DataSource = juntas;
            repJuntas.DataBind();

            repJuntasCongeladas.DataSource = juntas;
            repJuntasCongeladas.DataBind();

            pager.Bind(pager.PaginaActual, resultados.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultados"></param>
        /// <returns></returns>
        private DataTable obtenerRegistrosFiltradosOrdenados(DataView resultados)
        {
            int registroInicial = pager.TamanioPagina * pager.PaginaActual;
            int registroFinal = registroInicial + pager.TamanioPagina;
            int index = 0;

            IEnumerator enumerator = resultados.GetEnumerator();
            DataRowView drv;
            DataTable dt = resultados.Table.Clone();
            DataRow row;

            while (enumerator.MoveNext() && dt.Rows.Count < pager.TamanioPagina)
            {
                if (registroInicial <= index++)
                {
                    drv = (DataRowView)enumerator.Current;
                    row = dt.NewRow();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        row[dc.ColumnName] = drv[dc.ColumnName];
                    }

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        protected void pager_PaginaCambio(object sender, ArgumentosPaginador args)
        {
            cargaDatos(false);
        }

        protected void repJuntas_OnItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            RepeaterItem item = args.Item;

            switch (item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    _juntaActual = (DataRowView)item.DataItem;
                    bindRenglon(item);
                    break;

                default:
                    break;
            }
        }

        protected void repJuntasCongeladas_OnItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            RepeaterItem item = args.Item;

            switch (item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    _juntaActual = (DataRowView)item.DataItem;
                    bindRenglonCongelados(item);
                    break;

                case ListItemType.Footer:
                        if (CamposCongelados.Count > 0)
                        {
                            ((Literal)item.FindControl("thFooter")).Text = string.Format("<td colspan=\"{0}\">&nbsp;</td>", CamposCongelados.Count);
                        }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalColumnas"></param>
        private void bindEncabezadoTabla(int totalColumnas, bool primeraVez)
        {
            var joinModulosCampos = (from campo in _camposSeguimientoJunta
                                     join modulo in _modulosSeguimientoJunta on campo.ModuloSeguimientoJuntaID equals modulo.ModuloSeguimientoJuntaID
                                     select new
                                     {
                                         ModuloID = modulo.ModuloSeguimientoJuntaID,
                                         CampoID = campo.CampoSeguimientoJuntaID,
                                         NombreModulo = modulo.Nombre,
                                         NombreCampo = campo.Nombre,
                                         NombreColumnaSp = campo.NombreColumnaSp,
                                         OrdenModulo = modulo.OrdenUI,
                                         OrdenCampo = campo.OrdenUI,
                                         AnchoColumna = campo.AnchoUI ?? 100,
                                         FormatoColumna = campo.DataFormat,
                                         TipoDeDato = campo.TipoDeDato
                                     })
                                     .OrderBy(x => x.OrdenModulo)
                                     .ThenBy(x => x.OrdenCampo)
                                     .ToList();

            var encabezadosVisible = (from cmp in joinModulosCampos
                                      where CamposIDs.Contains(cmp.CampoID)
                                            && !CamposCongelados.Contains(cmp.CampoID)
                                      orderby cmp.OrdenModulo, cmp.OrdenCampo
                                      select cmp).ToList();


            var encabezadosCongelados = (from cmp in joinModulosCampos
                                         where CamposCongelados.Contains(cmp.CampoID)
                                         orderby cmp.OrdenModulo, cmp.OrdenCampo
                                         select cmp).ToList();


            var modulosVisibles = (from cmps in encabezadosVisible
                                   group cmps by new { ID = cmps.ModuloID, Nombre = cmps.NombreModulo, Orden = cmps.OrdenModulo} into grupo
                                   select new
                                   {
                                       ModuloID = grupo.Key.ID,
                                       Nombre = grupo.Key.Nombre,
                                       Orden = grupo.Key.Orden,
                                       Colspan = grupo.Count()
                                   }).OrderBy(x => x.Orden).ToList();

            var modulosCongelados = (from cmps in encabezadosCongelados
                                     group cmps by new { ID = cmps.ModuloID, Nombre = cmps.NombreModulo, Orden = cmps.OrdenModulo} into grupo
                                     select new
                                     {
                                         ModuloID = grupo.Key.ID,
                                         Nombre = grupo.Key.Nombre,
                                         Orden = grupo.Key.Orden,
                                         Colspan = grupo.Count()
                                     }).OrderBy(x => x.Orden).ToList();

            repGruposTitulos.DataSource = modulosVisibles;
            repGruposTitulos.DataBind();

            repTitulos.DataSource = encabezadosVisible;
            repTitulos.DataBind();

            _columnasMostrar = (from grp in encabezadosVisible
                                select new DefinicionColumna
                                {
                                    NombreColumnaSp = grp.NombreColumnaSp,
                                    Formato = grp.FormatoColumna,
                                    AnchoColumna = grp.AnchoColumna
                                }).ToList();

            repGruposTitulosCongelados.DataSource = modulosCongelados;
            repGruposTitulosCongelados.DataBind();

            repTitulosCongelados.DataSource = encabezadosCongelados;
            repTitulosCongelados.DataBind();

            _columnasCongeladas = (from grp in encabezadosCongelados
                                   select new DefinicionColumna
                                   {
                                       NombreColumnaSp = grp.NombreColumnaSp,
                                       Formato = grp.FormatoColumna,
                                       AnchoColumna = grp.AnchoColumna
                                   }).ToList();
        }

        private void bindRenglon(RepeaterItem item)
        {
            Repeater repColumnas = (Repeater)item.FindControl("repColumnas");

            repColumnas.DataSource = _columnasMostrar;
            repColumnas.DataBind();
        }

        private void bindRenglonCongelados(RepeaterItem item)
        {
            Repeater repColumnasCongeladas = (Repeater)item.FindControl("repColumnasCongeladas");

            repColumnasCongeladas.DataSource = _columnasCongeladas;
            repColumnasCongeladas.DataBind();
        }

        protected void repColumnas_OnItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            RepeaterItem item = args.Item;
            bindColumna(item);
        }

        private void bindColumna(RepeaterItem item)
        {
            if (item.IsItem())
            {
                Literal litColumna = (Literal)item.FindControl("litColumna");
                DefinicionColumna colDef = (DefinicionColumna)item.DataItem;

                _columnaActual = _juntaActual[colDef.NombreColumnaSp];

                if (_columnaActual != null && _columnaActual != DBNull.Value)
                {
                    if (!string.IsNullOrWhiteSpace(colDef.Formato))
                    {
                        litColumna.Text = string.Format("{0:" + colDef.Formato + "}", _columnaActual);
                    }
                    else if (_columnaActual.GetType() == typeof(bool))
                    {
                        litColumna.Text += TraductorEnumeraciones.TextoSiNo(_columnaActual.SafeBoolParse());
                    }
                    else
                    {
                        litColumna.Text = _columnaActual.ToString();
                    }
                }
            }
        }

        protected void repColumnasCongeladas_OnItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            RepeaterItem item = args.Item;
            if (item.ItemType == ListItemType.Header)
            {
                ((HyperLink)item.FindControl("hlPopUp")).NavigateUrl = string.Format(   "javascript:Sam.Calidad.AbrePopUpDetalleSeguimientoJunta('{0}','{1}','{2}');",
                                                                                        ProyectoID,
                                                                                        _juntaActual["GeneralJuntaSpoolID"],
                                                                                        _juntaActual["GeneralJuntaCampo"]);
            }
            else
            {
                bindColumna(item);
            }
        }

        protected void menuColumnas_ItemClick(object sender, RadMenuEventArgs e)
        {
            RadMenuItem itemClicked = e.Item;
            string columnaSeleccionada = hdnColumnaSeleccionada.Value;

            switch (itemClicked.Value)
            {
                case "ordAsc":
                    agregaOrden(columnaSeleccionada, "ASC");
                    break;
                case "ordDesc":
                    agregaOrden(columnaSeleccionada, "DESC");
                    break;
                case "remove":
                    quitaOrden(columnaSeleccionada);
                    break;
                case "freeze":
                    congelaDescongelaColumna(columnaSeleccionada);
                    break;
                default:
                    break;
            }

            pager.Reset();
            cargaDatos(false);
        }

        private void congelaDescongelaColumna(string nombreColumnaSp)
        {
            CampoSeguimientoJuntaCache campo = 
                _camposSeguimientoJunta.Where(x => x.NombreColumnaSp.Equals(nombreColumnaSp))
                                       .SingleOrDefault();

            if (campo != null)
            {
                if (!CamposCongelados.Contains(campo.CampoSeguimientoJuntaID))
                {
                    CamposCongelados.Add(campo.CampoSeguimientoJuntaID);
                }
                else
                {
                    CamposCongelados.Remove(campo.CampoSeguimientoJuntaID);
                }
            }
        }

        private void agregaOrden(string nombreColumna, string tipo)
        {
            quitaOrden(nombreColumna);
            Ordenamientos.Add(nombreColumna + " " + tipo);
        }

        private void quitaOrden(string nombreColumna)
        {
            var query = Ordenamientos.Select((v, i) => new { Columna = v.Split(' ')[0], Indice = i })
                                     .Where(v => v.Columna.EqualsIgnoreCase(nombreColumna));

            bool existe = query.Any();

            if (existe)
            {
                Ordenamientos.RemoveAt(query.Select(x => x.Indice).Single());
            }
        }
 
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            //Asegurarse que se procese el árbol de expresiones de UI
            filtro.FireApplyCommand();

            //Obtener la expresión de filtrado a través del provider de SQL y guardar en ViewState
            RadFilterSqlQueryProvider sqlProvider = new RadFilterSqlQueryProvider();
            sqlProvider.ProcessGroup(filtro.RootGroup);
            ValoresFiltro = sqlProvider.Result;

            pager.Reset();
            cargaDatos(false);
        }

        public IEnumerable<T> ControlsOfType<T>(Control parent) where T : class
        {
            foreach (Control control in parent.Controls)
            {
                if (control is T)
                {
                    yield return control as T;
                    continue;
                }

                foreach (T descendant in ControlsOfType<T>(control))
                {
                    yield return descendant;
                }
            }
        }

        protected void filtro_PreRender(object sender, EventArgs e)
        {
            //Quitar el time picker de los campos tipo fecha del filtro
            foreach (RadDateTimePicker picker in ControlsOfType<RadDateTimePicker>(sender as Control))
            {
                picker.TimePopupButton.Visible = false;
                picker.DateInput.DateFormat = "d";
                picker.DateInput.DisplayDateFormat = "d";
            }
        }
    }
}