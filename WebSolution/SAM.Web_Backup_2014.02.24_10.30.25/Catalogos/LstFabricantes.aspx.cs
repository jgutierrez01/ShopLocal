using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using Telerik.Web.UI;

namespace SAM.Web.Catalogos
{
    public partial class LstFabricantes : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Fabricantes);
                EstablecerDataSource();
                grdFabricantes.DataBind();
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
        protected void grdFabricantes_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
        /// <summary>
        /// Obtiene la lista de Fabricantes desde FabricantesBO
        /// </summary>
        private void EstablecerDataSource()
        {
            grdFabricantes.DataSource = FabricanteBO.Instance.ObtenerTodos();
        }

        protected void grdFabricantes_ItemDataBound(object source, GridItemEventArgs e)
        {
           
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Fabricante fabricante = (SAM.Entities.Fabricante)e.Item.DataItem;
                int idFabricante = fabricante.FabricanteID;//dataItem["FabricanteID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetFabricante.aspx?ID={0}", idFabricante);
            }

        }

        protected void grdFabricantes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int fabricanteID = e.CommandArgument.SafeIntParse();
                try
                {
                    FabricanteBO.Instance.Borra(fabricanteID);
                    EstablecerDataSource();
                    grdFabricantes.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdFabricantes.ResetBind();
            EstablecerDataSource();
            grdFabricantes.DataBind();
        }
    }
    
}