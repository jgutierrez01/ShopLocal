using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using Telerik.Web.UI;
using Resources;

namespace SAM.Web.Catalogos
{
    public partial class LstFamiliaMaterial : SamPaginaPrincipal
    {
        private readonly List<ObjectSetOrder> ORDEN_DEFAULT = new List<ObjectSetOrder>(new[] { new ObjectSetOrder { ColumnName = "Nombre", Order = SortOrder.Ascending } });

        /// <summary>
        /// Carga el listado en la primera página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_FamMateriales);
                EstablecerDataSource(grdFamilias.SelectedPageSize, 0);
                grdFamilias.DataBind();
            }
        }


        /// <summary>
        /// Se dispara cuando el grid debe vover a recalcular su contenido debido a eventos como los siguientes:
        /// + Paginación
        /// + Ordenamiento
        /// + Filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdFamilias_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource(grdFamilias.SelectedPageSize, grdFamilias.MasterTableView.CurrentPageIndex);
        }

        /// <summary>
        /// Va a la capa de negocios para obtener la información requerida y la asigna al grid
        /// </summary>
        /// <param name="tamanoPagina">Tamaño de página seleccionado por el grid</param>
        /// <param name="indicePagina">Página que debe recuperarse</param>
        private void EstablecerDataSource(int tamanoPagina, int indicePagina)
        {
            int totalFilas = 0;

            List<ObjectSetOrder> order = grdFamilias.GetCurrentSortings();
            order = order == null ? ORDEN_DEFAULT : order;

            grdFamilias.DataSource = FamiliaMaterialBO.Instance.ObtenerPaginado(    tamanoPagina,
                                                                                    indicePagina,
                                                                                    grdFamilias.GetCurrentFilters(),
                                                                                    order,
                                                                                    ref totalFilas);

            grdFamilias.VirtualItemCount = totalFilas;
        }

        protected void grdFamilias_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.FamiliaMaterial famMaterial = (SAM.Entities.FamiliaMaterial)e.Item.DataItem;
                int idFamilia = famMaterial.FamiliaMaterialID;//dataItem["FamiliaMaterialID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetFamiliaMaterial.aspx?ID={0}", idFamilia);
            }

        }

        protected void grdFamilias_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int familiaMaterialID = e.CommandArgument.SafeIntParse();
                try
                {
                    FamiliaMaterialBO.Instance.Borra(familiaMaterialID);
                    EstablecerDataSource(grdFamilias.SelectedPageSize, grdFamilias.MasterTableView.CurrentPageIndex);
                    grdFamilias.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            EstablecerDataSource(grdFamilias.SelectedPageSize, grdFamilias.MasterTableView.CurrentPageIndex);
        }
    }
}