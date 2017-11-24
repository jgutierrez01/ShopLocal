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
    public partial class LstTubero : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionFacade.EsAdministradorSistema)
            {
                
                //btnGuardar.Enabled = false;
            }

            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Tuberos);
                EstablecerDataSource();
                grdTuberos.DataBind();
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
        protected void grdTuberos_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
        /// <summary>
        /// Obtiene la lista de tuberos desde TuberoBO incluyendo los patios
        /// </summary>
        private void EstablecerDataSource()
        {
            grdTuberos.DataSource = TuberoBO.Instance.ObtenerTodosConPatio();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdTuberos_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int tuberoID = e.CommandArgument.SafeIntParse();
                try
                {
                    TuberoBO.Instance.Borra(tuberoID);
                    EstablecerDataSource();
                    grdTuberos.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void grdTuberos_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Tubero tubero = (SAM.Entities.Tubero)e.Item.DataItem;
                int idTubero = tubero.TuberoID;//dataItem["TuberoID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetTubero.aspx?ID={0}", idTubero);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdTuberos.ResetBind();
            EstablecerDataSource();
            grdTuberos.DataBind();
        }
      
    }
}