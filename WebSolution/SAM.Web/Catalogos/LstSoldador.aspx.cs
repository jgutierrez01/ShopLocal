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
using SAM.Web.Common;

namespace SAM.Web.Catalogos
{
    public partial class LstSoldador : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionFacade.EsAdministradorSistema)
            {
                
            }
            else
            {
                
            }

            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Soldadores);
                EstablecerDataSource();
                grdSoldadores.DataBind();
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
        protected void grdSoldadores_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
        /// <summary>
        /// Obtiene la lista de soldadores desde SoldadorBO incluyendo los patios
        /// </summary>
        private void EstablecerDataSource()
        {
            grdSoldadores.DataSource = SoldadorBO.Instance.ObtenerTodosConPatio();
        }

        protected void grdSoldadores_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int soldadorID = e.CommandArgument.SafeIntParse();
                try
                {
                    SoldadorBO.Instance.Borra(soldadorID);
                    EstablecerDataSource();
                    grdSoldadores.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdSoldadores_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Soldador soldador = (SAM.Entities.Soldador)e.Item.DataItem;
                int idSoldador = soldador.SoldadorID;//dataItem["SoldadorID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetSoldador.aspx?ID={0}", idSoldador);
            }

        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdSoldadores.ResetBind();
            EstablecerDataSource();
            grdSoldadores.DataBind();
        }
    }
}