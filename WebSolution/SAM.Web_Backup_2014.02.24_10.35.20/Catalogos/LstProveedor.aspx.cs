using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Data;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessLogic;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;
using Telerik.Web.UI;

namespace SAM.Web.Catalogos
{
    public partial class LstProveedor : SamPaginaPrincipal
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Proveedores);
                EstablecerDataSource();
                grdProveedores.DataBind();
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
        protected void grdProveedores_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
        /// <summary>
        /// Obtiene la lista de Proveedores desde ProveedorBO
        /// </summary>
        private void EstablecerDataSource()
        {
            grdProveedores.DataSource = ProveedorBO.Instance.ObtenerTodos();
        }

        protected void grdProveedores_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int proveedorID = e.CommandArgument.SafeIntParse();
                try
                {
                    ProveedorBO.Instance.Borra(proveedorID);
                    EstablecerDataSource();
                    grdProveedores.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdProveedores_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Proveedor proveedor = (SAM.Entities.Proveedor)e.Item.DataItem;
                int idProveedor = proveedor.ProveedorID; //dataItem["ProveedorID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetProveedor.aspx?ID={0}", idProveedor);
            }

        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdProveedores.ResetBind();
            EstablecerDataSource();
            grdProveedores.DataBind();
        }
    }
    
}