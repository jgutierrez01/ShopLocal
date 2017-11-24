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
    public partial class LstWps : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Wps);
                EstablecerDataSource();
                grdWps.DataBind();
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
        protected void grdWps_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// Obtiene la lista de Wps desde WpsBO, incluyendo sus relaciones 
        /// </summary>
        private void EstablecerDataSource()
        {
            grdWps.DataSource = WpsBO.Instance.ObtenerTodosConRelaciones();
        }

        protected void grdWps_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int wpsID = e.CommandArgument.SafeIntParse();
                try
                {
                    WpsBO.Instance.Borra(wpsID);
                    EstablecerDataSource();
                    grdWps.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void grdWps_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.Wps wps = (SAM.Entities.Wps)e.Item.DataItem;
                int idWps = wps.WpsID; //dataItem["WpsID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetWps.aspx?ID={0}", idWps);
            }

        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdWps.ResetBind();
            EstablecerDataSource();
            grdWps.DataBind();
        }
    }
}