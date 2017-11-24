using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Web.Classes;
using Telerik.Web.UI;


namespace SAM.Web.Administracion
{
    public partial class TblDestajos : System.Web.UI.Page
    {
        private int _proyectoId;
        private int _famAceroId;
        private int _tipoJuntaId;
        private int _procesoId;
        private string _procesoValor;

        private List<CedulaCache> _cedulas;
        private readonly List<ProcesoRaizCache> _procesoRaiz = CacheCatalogos.Instance.ObtenerProcesosRaiz();
        private readonly List<ProcesoRellenoCache> _procesoRelleno = CacheCatalogos.Instance.ObtenerProcesosRelleno();
        private readonly List<string> _lstProcesos = new List<string> { "", "A Armado" };

        private List<CostoProcesoRelleno> _costoRelleno;
        protected List<CostoProcesoRelleno> CostoRellenos
        {
            get
            {
                if(_costoRelleno == null)
                {
                    _costoRelleno = CostoProcesoRellenoBO.Instance.ObtenerTodos();
                }
                return _costoRelleno;
            }
        }

        private List<CostoProcesoRaiz> _costoRaiz;
        protected List<CostoProcesoRaiz> CostoRaices
        {
            get
            {
                if (_costoRaiz == null)
                {
                    _costoRaiz = CostoProcesoRaizBO.Instance.ObtenerTodos();
                }
                return _costoRaiz;
            }
        }

        private List<CostoArmado> _costoArmados;
        protected List<CostoArmado> CostoArmados
        {
            get
            {
                if(_costoArmados == null)
                {
                    _costoArmados = CostoArmadoBO.Instance.ObtenerTodos();
                }
                return _costoArmados;
            }
        }

        private bool _gridDefinido
        {
            get
            {
                return ViewState["grdDefinido"] != null;
            }
            set
            {
                ViewState["grdDefinido"] = null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _proyectoId = Request.Params["PTOID"].SafeIntParse();
            _famAceroId = Request.Params["FID"].SafeIntParse();
            _tipoJuntaId = Request.Params["TID"].SafeIntParse();
            _procesoId = Request.Params["PSOID"].SafeIntParse();
            _procesoValor = Request.Params["PSO"];
            _cedulas = CacheCatalogos.Instance.ObtenerCedulas();
            if(!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Destajos);
                cargaCombo();
                EstablecerDataSource();
                BindLimpio();
                grdTblDestajos.Visible = false;
            }
            if(_procesoValor != null && !Page.IsPostBack)
            {
                seleccionaCampos();
            }
        }

        // Refresca la tabla
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            grdTblDestajos.Rebind();
            grdTblDestajos.Visible = true;
        }

        // Crea la lista de los dropdowns
        private void cargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlFamiliaAcero.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
            ddlTipoJunta.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposJunta());
            var praiz = (from pr in _procesoRaiz select ("R" + pr.ID + " " + pr.Nombre)).ToList();
            var prelleno = (from pr in _procesoRelleno select ("RE" + pr.ID + " " + pr.Nombre)).ToList();
            praiz.AddRange(prelleno);
            _lstProcesos.AddRange(praiz);
            ddlProceso.DataTextField = "";
            ddlProceso.DataSource = _lstProcesos;
            ddlProceso.DataBind();
        }

        protected void grdTblDestajos_OnNeedNataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            defineGrid();
            grdTblDestajos.DataSource = DiametroBO.Instance.ObtenerTodos().OrderBy(x => x.Valor);
        }

        private void BindLimpio()
        {
            grdTblDestajos.ResetBind();
            grdTblDestajos.DataBind();
        }

        // Inserta el codigo de cedula en cada cabeza de una columna
        protected void defineGrid()
        {
            ColumnasGrid.ForEach(grdTblDestajos.Columns.Remove);
            foreach (Cedula cedula in CedulaBO.Instance.ObtenerTodos().OrderBy(x => x.Orden))
            {
                GridBoundColumn columna = new GridBoundColumn();
                columna.HeaderText = cedula.Codigo;
                columna.UniqueName = cedula.Codigo;
                columna.AllowFiltering = false;
                columna.AllowSorting = false;
                columna.HeaderStyle.Width = new Unit(60);
                grdTblDestajos.MasterTableView.Columns.Add(columna);
            }
            _gridDefinido = true;
        }

        protected void grdTblDestajos_OnItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                Diametro diametro = e.Item.DataItem as Diametro;
                GridDataItem item = e.Item as GridDataItem;
                string procesoTxt = ddlProceso.SelectedValue;
                int procesoID = obtenerID(procesoTxt);
                int tipoJuntaID = ddlTipoJunta.SelectedValue.SafeIntParse();
                int famAceroID = ddlFamiliaAcero.SelectedValue.SafeIntParse();
                int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

                foreach (var column in ColumnasGrid)
                {
                    string codigoCedula = column.HeaderText;
                    CedulaCache cedula = _cedulas.Single(x => x.Nombre.EqualsIgnoreCase(codigoCedula));

                    if(procesoTxt.StartsWith("A"))
                    {
                        CostoArmado arm = CostoArmados.SingleOrDefault(x => x.CedulaID == cedula.ID &&
                                                                            x.DiametroID == diametro.DiametroID &&
                                                                            x.ProyectoID == proyectoID &&
                                                                            x.FamiliaAceroID == famAceroID &&
                                                                            x.TipoJuntaID == tipoJuntaID);
                        if (arm != null)
                        {
                            item[codigoCedula].Text = arm.Costo.ToString();
                        }
                        else
                        {
                            item[codigoCedula].Text = " ";
                        }
                    }
                    else if(procesoTxt.StartsWith("RE"))
                    {
                        CostoProcesoRelleno rell = CostoRellenos.SingleOrDefault(x => x.CedulaID == cedula.ID &&
                                                                                      x.DiametroID == diametro.DiametroID &&
                                                                                      x.ProyectoID == proyectoID &&
                                                                                      x.FamiliaAceroID == famAceroID &&
                                                                                      x.TipoJuntaID == tipoJuntaID &&
                                                                                      x.ProcesoRellenoID == procesoID);
                        if (rell != null)
                        {
                            item[codigoCedula].Text = rell.Costo.ToString();
                        }
                        else
                        {
                            item[codigoCedula].Text = " ";
                        }
                    }
                    else if (procesoTxt.StartsWith("R"))
                    {
                        CostoProcesoRaiz raiz = CostoRaices.SingleOrDefault(x => x.CedulaID == cedula.ID &&
                                                                                 x.DiametroID == diametro.DiametroID &&
                                                                                 x.ProyectoID == proyectoID &&
                                                                                 x.FamiliaAceroID == famAceroID &&
                                                                                 x.TipoJuntaID == tipoJuntaID &&
                                                                                 x.ProcesoRaizID == procesoID);
                        if (raiz != null)
                        {
                            item[codigoCedula].Text = raiz.Costo.ToString();
                        }
                        else
                        {
                            item[codigoCedula].Text = " ";
                        }
                    }
                    else
                    {
                        item[codigoCedula].Text = " ";
                    }

                }
            }
        }

        protected List<GridColumn> ColumnasGrid
        {
            get
            {
                return grdTblDestajos.Columns.Cast<GridColumn>().Where(x => !x.UniqueName.EqualsIgnoreCase("Diametro")).ToList();
            }
        }

        // redireccion para insertar un nuevo destajo
        protected void grdTblDestajos_OnItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "Agregar")
            {
                int procesoID = obtenerID(ddlProceso.SelectedValue);
                string url = string.Format(WebConstants.AdminUrl.ImportaDestajos,
                                            ddlProyecto.SelectedValue,
                                            ddlFamiliaAcero.SelectedValue,
                                            ddlTipoJunta.SelectedValue,
                                            ddlProceso.SelectedItem.Text,
                                            procesoID);

                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Carga la informacion del proyecto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                proyEncabezado.BindInfo(ddlProyecto.SelectedValue.SafeIntParse());
                proyEncabezado.Visible = true;
            }
            catch
            {
                // Captura error si el dllProyecto se encuentra vacio
                proyEncabezado.Visible = false;
            }
        }

        /// <summary>
        /// Usamos este método para configurar dinámicamente los links del header del grid
        /// </summary>
        protected void grdTblDestajos_OnItemCreated(object sender, GridItemEventArgs e)
        {
            GridCommandItem commandItem = e.Item as GridCommandItem;

            if(commandItem != null)
            {
                int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();
                int tipoJunta = ddlTipoJunta.SelectedValue.SafeIntParse();
                int famAcero = ddlFamiliaAcero.SelectedValue.SafeIntParse();
                int procesoID = obtenerID(ddlProceso.Text);
                string procesoValor = ddlProceso.SelectedItem.Text;
                string proyectoValor = ddlProyecto.SelectedItem.Text;

                HyperLink lnkExportarDestajos = (HyperLink) commandItem.FindControl("lnkExportar");
                HyperLink lnkExportarImagen = (HyperLink) commandItem.FindControl("imgExportar");

                string url = string.Format(WebConstants.ProduccionUrl.ExportaExcelDestajos,
                                           proyectoID,
                                           (int)TipoArchivoExcel.Destajos, tipoJunta, famAcero, procesoID, procesoValor, proyectoValor
                                           );
                lnkExportarDestajos.NavigateUrl = url;
                lnkExportarImagen.NavigateUrl = url;
            }
        }

        /// <summary>
        /// Obtiene un id de una cadena como "R23 Proceso Relleno Tal"
        /// Regresaria 23
        /// </summary>
        /// <param name="procesoTxt"></param>
        /// <returns>La primera serie de digitos encontrada en una cadena</returns>
        private static int obtenerID(string procesoTxt)
        {
            string id = string.Empty;
            foreach (char c in procesoTxt)
            {
                //si es un caracter entre 0 y 9
                if (c >= 48 && c <= 57)
                {
                    id += c;
                }
                //si es un espacio terminamos
                if (c == 32)
                {
                    break;
                }
            }

            return id.SafeIntParse();
        }

        protected void seleccionaCampos()
        {
            string proceso;
            ProcesoRellenoCache proRelleno;
            ProcesoRaizCache proRaiz;
            FamAceroCache famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().SingleOrDefault(x => x.ID == _famAceroId);
            TipoJuntaCache tipoJunta = CacheCatalogos.Instance.ObtenerTiposJunta().SingleOrDefault(x => x.ID == _tipoJuntaId);

            if (_procesoValor.StartsWith("RE"))
            {
                proRelleno = CacheCatalogos.Instance.ObtenerProcesosRelleno().SingleOrDefault(x => x.ID == _procesoId);
                proceso = "RE" + proRelleno.ID + " " + proRelleno.Codigo;
            }
            else if (_procesoValor.StartsWith("R"))
            {
                proRaiz = CacheCatalogos.Instance.ObtenerProcesosRaiz().SingleOrDefault(x => x.ID == _procesoId);
                proceso = "R" + proRaiz.ID + " " + proRaiz.Codigo;
            }
            else
            {
                proceso = "A Armado";
            }

            ddlProyecto.SelectedValue = _proyectoId.ToString();
            proyEncabezado.BindInfo(ddlProyecto.SelectedValue.SafeIntParse());
            proyEncabezado.Visible = true;
            ddlTipoJunta.SelectedValue = tipoJunta.ID.SafeStringParse();
            ddlFamiliaAcero.SelectedValue = famAcero.ID.SafeStringParse();
            ddlProceso.SelectedValue = proceso;
        }
    }
}