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
    public partial class LstPatio : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Patios);
                EstablecerDataSource();
                grdPatio.DataBind();
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
        protected void grdPatio_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
        /// <summary>
        /// Obtiene la lista de Patios desde PatioBO
        /// </summary>
        private void EstablecerDataSource()
        {
            
            grdPatio.DataSource = PatioBO.Instance.ObtenerConMaquinaTaller();
            
        }

        protected void grdPatio_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int patioID = e.CommandArgument.SafeIntParse();
                try
                {
                    PatioBO.Instance.Borra(patioID);
                    EstablecerDataSource();
                    grdPatio.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdPatio.ResetBind();
            EstablecerDataSource();
            grdPatio.DataBind();
        }

        protected void grdPatio_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Patio patio = (SAM.Entities.Patio)e.Item.DataItem;
                int idPatio = patio.PatioID;//dataItem["PatioID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetPatio.aspx?ID={0}", idPatio);
            }

        }
    }
}