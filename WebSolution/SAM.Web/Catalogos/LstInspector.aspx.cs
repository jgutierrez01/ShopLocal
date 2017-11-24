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
    public partial class LstInspector : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Inspectores);
                EstablecerDataSource();
                grdInspectores.DataBind();
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
        protected void grdInspectores_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
        /// <summary>
        /// Obtiene la lista de inspectores desde InspectorBO
        /// </summary>
        private void EstablecerDataSource()
        {
            grdInspectores.DataSource = InspectorBO.Instance.ObtenerTodosConPatioYTaller();
        }

        protected void grdInspectores_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int inspectorID = e.CommandArgument.SafeIntParse();
                try
                {
                    InspectorBO.Instance.Borra(inspectorID);
                    EstablecerDataSource();
                    grdInspectores.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdInspectores_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Inspector inspector = (SAM.Entities.Inspector)e.Item.DataItem;
                int idInspector = inspector.InspectorID;//dataItem["SoldadorID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetInspector.aspx?ID={0}", idInspector);
            }

        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdInspectores.ResetBind();
            EstablecerDataSource();
            grdInspectores.DataBind();
        }
    }
}