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
    public partial class LstTransportista : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Transportistas);
                EstablecerDataSource();
                grdTransportistas.DataBind();
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
        protected void grdTransportistas_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
        /// <summary>
        /// Obtiene la lista de Transportistas desde TransportistasBO
        /// </summary>
        private void EstablecerDataSource()
        {
            grdTransportistas.DataSource = TransportistaBO.Instance.ObtenerTodos();
        }

        protected void grdTransportistas_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int transportistaID = e.CommandArgument.SafeIntParse();
                try
                {
                    TransportistaBO.Instance.Borra(transportistaID);
                    EstablecerDataSource();
                    grdTransportistas.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdTransportistas_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Transportista transportista = (SAM.Entities.Transportista)e.Item.DataItem;
                int idTransportista = transportista.TransportistaID;//dataItem["TransportistaID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetTransportista.aspx?ID={0}", idTransportista);
            }

        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdTransportistas.ResetBind();
            EstablecerDataSource();
            grdTransportistas.DataBind();
        }
    }
    
}