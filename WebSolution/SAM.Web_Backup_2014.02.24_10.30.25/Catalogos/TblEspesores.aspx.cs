using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using SAM.BusinessObjects.Administracion;

namespace SAM.Web.Catalogos
{
    public partial class TblEspesores : SamPaginaPrincipal
    {
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

        private List<Espesor> _espesores;
        private List<Espesor> Espesores
        {
            get
            {
                if(_espesores == null)
                {
                    _espesores = EspesorBO.Instance.ObtenerTodos();
                }
                return _espesores;
            }   
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _cedulas = CacheCatalogos.Instance.ObtenerCedulas();
            if(!Page.IsPostBack)
            {
                EstablecerDataSource();
                BindLimpio();
            }
        }

        protected void grdTblEspesores_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            defineGrid();
            grdTblEspesores.DataSource = DiametroBO.Instance.ObtenerTodos().OrderBy(x => x.Valor);
        }
        
        private void BindLimpio()
        {
            grdTblEspesores.ResetBind();
            grdTblEspesores.DataBind();
        }

        protected void defineGrid()
        {
            ColumnasGrid.ForEach(grdTblEspesores.Columns.Remove);
            foreach (Cedula cedula in CedulaBO.Instance.ObtenerTodos().OrderBy(x => x.Orden))
            {
                GridBoundColumn columna = new GridBoundColumn();
                columna.HeaderText = cedula.Codigo;
                columna.UniqueName = cedula.Codigo;
                columna.AllowFiltering = false;
                columna.AllowSorting = false;
                columna.HeaderStyle.Width = new Unit(60);
                grdTblEspesores.MasterTableView.Columns.Add(columna);
            }
            _gridDefinido = true;
        }

        protected void grdTblEspesores_OnItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                Diametro diametro = e.Item.DataItem as Diametro;
                GridDataItem item = e.Item as GridDataItem;

                foreach (var column in ColumnasGrid)
                {
                    string codigoCedula = column.UniqueName;
                    CedulaCache cedula = _cedulas.Single(x => x.Nombre.EqualsIgnoreCase(codigoCedula));
                    Espesor espesor = Espesores.SingleOrDefault(x => x.CedulaID == cedula.ID && x.DiametroID == diametro.DiametroID);

                    if (espesor != null)
                    {
                        item[codigoCedula].Text = String.Format("{0:N3}", espesor.Valor);
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
                return grdTblEspesores.Columns.Cast<GridColumn>().Where(x => !x.UniqueName.EqualsIgnoreCase("Diametro")).ToList();
            }
        }
    }
}