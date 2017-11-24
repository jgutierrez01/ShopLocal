using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;

namespace SAM.Web.Catalogos
{
    public partial class LstCliente : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Clientes);
                EstablecerDataSource();
                grdClientes.DataBind();
            }
        }

         protected void grdClientes_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdClientes.DataSource = ClienteBO.Instance.ObtenerTodosConContactoCliente();   
        }

        protected void grdClientes_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int clienteID = e.CommandArgument.SafeIntParse();
                
                try
                {
                    ClienteBO.Instance.Borra(clienteID);
                    EstablecerDataSource();
                    grdClientes.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdClientes.ResetBind();
            EstablecerDataSource();
            grdClientes.DataBind();
        }

        protected void grdClientes_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Grid.GrdCliente cliente = (SAM.Entities.Grid.GrdCliente)e.Item.DataItem;
                int idCliente = cliente.ClienteID;//dataItem["ClienteID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetCliente.aspx?ID={0}", idCliente);
            }

        }
    }
}