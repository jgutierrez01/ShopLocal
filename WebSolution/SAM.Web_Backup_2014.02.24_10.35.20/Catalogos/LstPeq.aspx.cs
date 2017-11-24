using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using Telerik.Web.UI;
using SAM.BusinessObjects.Administracion;
using SAM.Web.Classes;

namespace SAM.Web.Catalogos
{
    public partial class LstPeq : System.Web.UI.Page
    {
        private string _tipoJuntaId;
        private string _famAcerId;

        private List<CedulaCache> _cedulas;
        private bool _gridDefinido
        {
            get
            {
                return ViewState["grdDefinido"] != null;
            }
            set
            {
                ViewState["grdDefinido"] = value;
            }
        }

        private List<Peq> _peqs;
        protected List<Peq> Peqs
        {
            get
            {
                if(_peqs == null)
                {
                    _peqs = PeqBO.Instance.ObtenerPorProyecto(ProyectoID);
                }
                return _peqs;
            }
        }

        private int ProyectoID
        {
            get
            {
                return ViewState["ProyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _tipoJuntaId = Request.Params["TID"];
            _famAcerId = Request.Params["FID"];
            ProyectoID = Request.Params["PID"].SafeIntParse();
            _cedulas = CacheCatalogos.Instance.ObtenerCedulas();
            
            if(!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_PulgadasEquivalentes);

                cargaCombo();                

                if (_tipoJuntaId != null && _famAcerId != null)
                {
                    ddlTipoJunta.SelectedValue = _tipoJuntaId;
                    ddlFamiliaAcero.SelectedValue = _famAcerId;
                    ddlProyecto.SelectedValue = ProyectoID.ToString();
                    proyHeader.BindInfo(ddlProyecto.SelectedValue.SafeIntParse());
                    proyHeader.Visible = true;
                    EstablecerDataSource();
                    BindLimpio();
                    grdLstPeq.Visible = true;
                }
                
            } 
          
        }

        protected void ddlProyecto_SelectedChanged(object sender, EventArgs e)
        {
            if (ddlProyecto.SelectedValue.SafeIntParse() > 0)
            {
                proyHeader.BindInfo(ddlProyecto.SelectedValue.SafeIntParse());
                proyHeader.Visible = true;
            }
            else
            {
                proyHeader.Visible = false;
            }
        }

        // Refresca la tabla
        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            ProyectoID = ddlProyecto.SelectedValue.SafeIntParse();
            grdLstPeq.Rebind();
            grdLstPeq.Visible = true;
        }

        // Crea la lista de los dropdowns
        private void cargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlTipoJunta.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTiposJunta());
            ddlFamiliaAcero.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
        }

        protected void grdLstPeq_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            defineGrid();
            grdLstPeq.DataSource = DiametroBO.Instance.ObtenerTodos().OrderBy(x => x.Valor);
        }

        private void BindLimpio()
        {
            grdLstPeq.ResetBind();
            grdLstPeq.DataBind();
        }

        // Inserta el codigo de cedula en cada cabeza de una columna
        protected void defineGrid()
        {
            //if (_gridDefinido) return;
            ColumnasGrid.ForEach(grdLstPeq.Columns.Remove);
            foreach (Cedula cedula in CedulaBO.Instance.ObtenerTodos().OrderBy(x => x.Orden))
            {
                GridBoundColumn columna = new GridBoundColumn();
                columna.HeaderText = cedula.Codigo;
                columna.UniqueName = cedula.Codigo;
                columna.AllowFiltering = false;
                columna.AllowSorting = false;
                columna.HeaderStyle.Width = new Unit(60);
                grdLstPeq.MasterTableView.Columns.Add(columna);
            }
            _gridDefinido = true;
        }


        // Inserta el peq correspondiente en cada celda, inserta 0 si no existe
        protected void grdLstPeq_OnItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                Diametro diametro = e.Item.DataItem as Diametro;
                GridDataItem item = e.Item as GridDataItem;

                foreach (var column in ColumnasGrid)
                {
                    string codigoCedula = column.HeaderText;
                    CedulaCache cedula = _cedulas.Single(x => x.Nombre.EqualsIgnoreCase(codigoCedula));
                    int tipoJuntaID = ddlTipoJunta.SelectedValue.SafeIntParse();
                    int famAceroID = ddlFamiliaAcero.SelectedValue.SafeIntParse();
                    int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

                    Peq peq =
                        Peqs.SingleOrDefault(
                            x =>
                            x.CedulaID == cedula.ID && x.DiametroID == diametro.DiametroID &&
                            x.FamiliaAceroID == famAceroID && x.TipoJuntaID == tipoJuntaID);

                    if (peq != null)
                    {
                        item[codigoCedula].Text = String.Format("{0:N3}", peq.Equivalencia);
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
                return grdLstPeq.Columns.Cast<GridColumn>().Where(x => !x.UniqueName.EqualsIgnoreCase("Diametro")).ToList();
            }
        }

        
        // redireccion para insertar un nuevo peq
        protected void grdLstPeq_ItemCommand(object source, GridCommandEventArgs e)
        {
            if(e.CommandName == "Agregar")
            {
                string url = string.Format(WebConstants.CatalogoUrl.ImportaPeq, ddlTipoJunta.SelectedValue.SafeIntParse(), ddlFamiliaAcero.SelectedValue.SafeIntParse(), ddlProyecto.SelectedValue.SafeIntParse());

                Response.Redirect(url);
            }
        }
    }
}
