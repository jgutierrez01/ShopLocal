using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessLogic.Cruce;
using SAM.BusinessObjects.Ingenieria;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Reportes;
using System.Threading;
using Mimo.Framework.Common;
using SAM.Entities.Grid;
using SAM.BusinessLogic.Excel;
using Mimo.Framework.Data;
using log4net;
using System.Diagnostics;

namespace SAM.Web.Produccion
{
    public partial class CrucePorProyecto : SamPaginaPrincipal
    {
        private string sortOrder = string.Empty;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CrucePorProyecto));
        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Cargar el combo de proyectos con aquellos proyectos a los cuales tiene permiso/acceso el usuario
        /// loggeado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_CrucePrioridad);
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            }
        }

        /// <summary>
        /// Usamos este método para configurar dinámicamente los links del header del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if (commandItem != null)
            {
                HyperLink lnkOdt = (HyperLink)commandItem.FindControl("lnkOdt");
                HyperLink imgOdt = (HyperLink)commandItem.FindControl("imgOdt");
                HyperLink hlFaltantes = (HyperLink)commandItem.FindControl("hlFaltantes");
                HyperLink hlFaltantesExcel = (HyperLink)commandItem.FindControl("hlFaltantesExcel");
                HyperLink imgFaltantes = (HyperLink)commandItem.FindControl("imgFaltantes");
                HyperLink imgFaltantesExcel = (HyperLink)commandItem.FindControl("imgFaltantesExcel");

                List<ObjectSetOrder> order = null;

                order = grdSpools.GetCurrentSortings();

                if (order != null)
                {
                    order.ForEach(x => sortOrder += x.GetESqlSort().Substring(3) + ",");
                    sortOrder = sortOrder.Substring(0, sortOrder.Length - 1);
                }
                else
                {
                    sortOrder = string.Empty;
                }

                string jsLink = string.Format("javascript:Sam.Produccion.AbrePopupCruce('{0}','{1}','{2}');",
                                                grdSpools.ClientID,
                                                ddlProyecto.ClientID,
                                                sortOrder);

                hlFaltantes.NavigateUrl = string.Format(WebConstants.ProduccionUrl.ReporteFaltantes, NombreArchivo, "pdf");
                imgFaltantes.NavigateUrl = hlFaltantes.NavigateUrl;

                hlFaltantesExcel.NavigateUrl = string.Format(WebConstants.ProduccionUrl.ReporteFaltantes, NombreArchivo, "xlsx");
                imgFaltantesExcel.NavigateUrl = hlFaltantesExcel.NavigateUrl;

                lnkOdt.NavigateUrl = jsLink;
                imgOdt.NavigateUrl = jsLink;
                sortOrder = string.Empty;
            }
        }

        /// <summary>
        /// Usamos este método para deshabilitar los spools que se encuentran en hold
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                var itemSeleccion = dataItem["chk_h"];
                var itemHold = dataItem["Hold"];

                if (itemSeleccion != null && itemHold != null)
                {
                    CheckBox chckHold = (CheckBox)itemHold.Controls[0];

                    if (chckHold.Checked)
                    {
                        itemSeleccion.Text = String.Empty;
                    }
                }
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
        /// Se dispara cuando el usuario da click en el botón mostrar.
        /// Al suceder lo anterior se muestra un resumen sobre los spools
        /// de ese proyecto en total y aquellos que aún no tienen ODT.
        /// 
        /// En el panel que se muestra el usuario puede dar click para llevar a cabo el cruce o bien
        /// dirigirse a la página de fijar prioridad para priorizar los spools.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            Validate("vgProyecto");

            if (IsValid)
            {
                EntityID = ddlProyecto.SelectedValue.SafeIntParse();
                ResumenSpool resumen = ProyectoBO.Instance.ObtenResumenSpoolPorProyecto(EntityID.Value);

                lblSinOdtValor.Text = String.Format("{0:N0}", resumen.SpoolsSinOdt);
                lblTotalValor.Text = String.Format("{0:N0}", resumen.SpoolsTotales);
                phLabels.Visible = true;
                phSpools.Visible = false;
                phTotalizador.Visible = false;
            }
        }

        /// <summary>
        /// Lleva a cabo un cruce de todos los spools del proyecto contra sus inventarios/números únicos.
        /// El resultado del cruce se muestra en pantalla permitiendo al usuario determinar que spools quiere
        /// incluir en una orden de trabajo.
        /// 
        /// A su vez se manda a un background thread la generación del reporte de faltantes el cual se
        /// va a generar a través de reporting services y guardarse en file system para que
        /// después se pueda recuperar rápido.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCruzar_Click(object sender, EventArgs e)
        {
            CruceProyecto cruce = new CruceProyecto(EntityID.Value);

            List<FaltanteCruce> faltantes;
            List<CondensadoItemCode> condensado;

            //Lleva a cabo el proceso de cruce, guarda en ViewState los ids de aquellos spools
            //que si se pueden cruzar. Posteriormente del Grid utiliza este arreglo para determinar que spools
            //se muestran en el grid.
            bool spoolHoldIncluidos = chkSpoolHold.Checked;
            bool crucePorIsometrico = chkCrucePorIsometrico.Checked;

            SpoolsFabricables = cruce.Procesa(out faltantes, out condensado, spoolHoldIncluidos, crucePorIsometrico)
                                     .Select(x => x.SpoolID)
                                     .ToArray();

            DateTime hoy = DateTime.Now;

            //Generar el PDF de manera asíncrona, mientras tanto llevar a cabo el binding, es altamente probable
            //que para cuando el usuario quiera descargar el reporte el PDF ya se haya terminado de generar
            NombreArchivo = Guid.NewGuid();
            string pathReporte = Server.MapPath("~/Produccion/Reportes/RptFaltantes.rdlc");
            string idioma = LanguageHelper.CustomCulture;

            sw.Start();
            _logger.DebugFormat("Crea pdf de faltantes");
            Thread t = new Thread(() => UtileriasReportes.GeneraPdfReporteFaltantes(faltantes, condensado, NombreArchivo, pathReporte, idioma, hoy.ToShortDateString(), hoy.ToShortTimeString()));
            t.Start();
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            string imgPath = Server.MapPath("~/Imagenes/Logos/sam_powered_by_small_small.jpg");
            sw.Start();
            _logger.DebugFormat("Crea excel de faltantes");
            Thread texcel = new Thread(() => ExcelReporteFaltantes.Instance.GeneraExcelReporteFaltantes(faltantes, condensado, NombreArchivo, hoy.ToShortDateString(), hoy.ToShortTimeString(), imgPath, idioma));
            texcel.Start();
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            //Mostrar el panel que contiene el grid con los spools y el totalizador
            phSpools.Visible = true;
            phTotalizador.Visible = true;

            //en base al spec este grid debe mostrar por default 100 filas a la vez
            grdSpools.MasterTableView.PageSize = 100;
            sw.Restart();
            _logger.DebugFormat("Pinta grid");
            EstablecerDataSource(spoolHoldIncluidos);
            grdSpools.DataBind();
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// Actualiza el grid con los spools regresando a la página 1 y quitando los filtros.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_OnClick(object sender, EventArgs e)
        {
            EstablecerDataSource(chkSpoolHold.Checked);
            grdSpools.ResetBind();
            grdSpools.DataBind();
        }

        /// <summary>
        /// Este botón no está visible desde el UI.  Se dispara a través del clásico truco de JS donde una ventana modal
        /// da click sobre un botón en particular de la ventana padre.
        /// 
        /// Al hacer lo anterior se obtienen aquellos spools que el usuario seleccionó los cuales son los mismos que
        /// se utilizaron en el popup para generar la ODT.
        /// 
        /// La variable de ViewState "SpoolsFabricables" se actualiza para quitar los spools que se acaban de enviar a ODT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuitaSeleccionados_Click(object sender, EventArgs e)
        {
            //Traer los elementos seleccionados a través del API de telerik
            GridDataItem [] items = grdSpools.MasterTableView.GetSelectedItems();
            
            //Obtener los ids de los spools seleccionados
            int [] ids = items.Select(x => x.GetDataKeyValue("SpoolID").SafeIntParse()).ToArray();

            //quita los spools que ya se enviaron a ODT
            SpoolsFabricables = SpoolsFabricables.Except(SpoolsFabricables.Where(x => ids.Contains(x))).ToArray();

            //Actualiza la información del grid
            EstablecerDataSource(chkSpoolHold.Checked);
            grdSpools.ResetBind();
            grdSpools.DataBind();
        }

        /// <summary>
        /// Variable de ViewState que contiene un arreglo con los IDs de los spools que se
        /// pueden fabricar, inicialmente esto lo determina el proceso de cruce, conforme
        /// se van generando ODTs los IDs de los spools que ya se fueron a ODT se van
        /// quitando de esta lista.
        /// </summary>
        private int[] SpoolsFabricables
        {
            get
            {
                return (int[])ViewState["SpoolsFabricables"];
            }
            set
            {
                ViewState["SpoolsFabricables"] = value;
            }
        }

        /// <summary>
        /// Nombre (Guid) del reporte de faltantes para el cruce que se acaba de efectuar
        /// </summary>
        private Guid NombreArchivo
        {
            get
            {
                return (Guid)ViewState["NombreArchivo"];
            }
            set
            {
                ViewState["NombreArchivo"] = value;
            }
        }

        /// <summary>
        /// Se dispara cuando el grid necesita actualizar su datasource, por ejemplo cuando
        /// hay paginación, sorting o filtros.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSpools_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource(chkSpoolHold.Checked);
        }

        /// <summary>
        /// Establece la propiedad DataSource del grid con aquellos spools que deben aparecer ene l grid.
        /// Este método no hace el DataBind.
        /// </summary>
        private void EstablecerDataSource(bool incluyeHold)
        {
            sw.Restart();
            _logger.DebugFormat("query para pintar grid");
            List<GrdCruce> DataSource = SpoolBO.Instance.ObtenerDespuesDeCruce(SpoolsFabricables, incluyeHold);
            _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
            grdSpools.DataSource = DataSource;

            litSpoolsTotales.Text = (from s in DataSource
                                     select s.SpoolID).Count().SafeStringParse();

            litJuntasTotales.Text = (from j in DataSource
                                     select j.Juntas).Sum().SafeStringParse();

            litAccesoriosTotales.Text = (from a in DataSource
                                         select a.TotalAccesorio).Sum().SafeStringParse();

            litTubosTotales.Text = (from j in DataSource
                                    select j.TotalTubo).Sum().SafeStringParse();

            litLongitudTotales.Text = (from l in DataSource
                                       select l.LongitudTubo).Sum().SafeStringParse();

            litPdiTotales.Text = string.Format("{0:#0.00}", (from pdi in DataSource
                                  select pdi.Pdis).Sum());

            litKgsTotales.Text = string.Format("{0:#0.00}", (from kg in DataSource
                                                select kg.Peso).Sum());

            litAreaTotales.Text = string.Format("{0:#0.00}", (from a in DataSource
                                                 select a.Area).Sum());

            litPeqsTotales.Text = string.Format("{0:#0.00}", (from p in DataSource
                                                 select p.TotalPeqs).Sum());
            
        }
    }
}