using System;
using System.Linq;
using Telerik.Web.UI;
using SAM.Entities;
using SAM.Entities.Cache;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using System.Collections.Generic;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Utilerias;
using SAM.Web.Classes;

namespace SAM.Web.Catalogos
{
    public partial class KgTeoricos : System.Web.UI.Page
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

        private List<KgTeorico> _kgTeoricos;
        private List<KgTeorico> KgTeoricoss
        {
            get
            {
                if (_kgTeoricos == null)
                {
                    _kgTeoricos = KgTeoricoBO.Instance.ObtenerTodos();
                }
                return _kgTeoricos;
            }  
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _cedulas = CacheCatalogos.Instance.ObtenerCedulas();
            if(!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_KgTeoricos);
                EstablecerDataSource();
                BindLimpio();
            }

        }

        protected void grdKgTeoricos_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();            
        }

        private void EstablecerDataSource()
        {
            defineGrid();
            grdKgTeoricos.DataSource = DiametroBO.Instance.ObtenerTodos().OrderBy(x => x.Valor);
        }

        private void BindLimpio()
        {
            grdKgTeoricos.ResetBind();
            grdKgTeoricos.DataBind();
        }

        protected void defineGrid()
        {
            ColumnasGrid.ForEach(grdKgTeoricos.Columns.Remove);
            foreach (Cedula cedula in CedulaBO.Instance.ObtenerTodos().OrderBy(x => x.Orden))
            {
                GridBoundColumn columna = new GridBoundColumn();
                columna.HeaderText = cedula.Codigo;
                columna.UniqueName = cedula.Codigo;
                columna.AllowFiltering = false;
                columna.AllowSorting = false;
                columna.HeaderStyle.Width = new Unit(60);
                grdKgTeoricos.MasterTableView.Columns.Add(columna);
            }
            _gridDefinido = true;
        }

        protected void grdKgTeoricos_OnItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                Diametro diametro = e.Item.DataItem as Diametro;
                GridDataItem item = e.Item as GridDataItem;

                foreach (var column in ColumnasGrid)
                {
                    string codigoCedula = column.HeaderText;
                    CedulaCache cedula = _cedulas.Single(x => x.Nombre.EqualsIgnoreCase(codigoCedula));
                    KgTeorico kgteorico =
                        KgTeoricoss.SingleOrDefault(x => x.CedulaID == cedula.ID && x.DiametroID == diametro.DiametroID);

                    if (kgteorico != null)
                    {
                        item[codigoCedula].Text = String.Format("{0:N3}", kgteorico.Valor);
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
                return grdKgTeoricos.Columns.Cast<GridColumn>().Where(x => !x.UniqueName.EqualsIgnoreCase("Diametro")).ToList();
            }
        }
    }
}
