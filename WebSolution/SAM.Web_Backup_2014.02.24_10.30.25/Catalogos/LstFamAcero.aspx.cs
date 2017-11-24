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
    public partial class LstFamAcero : SamPaginaPrincipal
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_FamAceros);
                EstablecerDataSource();
                grdFamiliaAcero.DataBind();
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
        protected void grdFamiliaAcero_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }
        /// <summary>
        /// Obtiene la lista de familia de aceros de la entidad de cache FamAcero que incluye el nombre de la familia de material
        /// </summary>
        private void EstablecerDataSource()
        {
            grdFamiliaAcero.DataSource = FamiliaAceroBO.Instance.ObtenerTodasConFamiliaMaterial();   
        }

        protected void grdFamiliaAcero_ItemDataBound(object source, GridItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink edita = e.Item.FindControl("hypEditar") as HyperLink;
                //GridDataItem dataItem = (GridDataItem)e.Item;
                SAM.Entities.FamiliaAcero famAcero = (SAM.Entities.FamiliaAcero)e.Item.DataItem;
                int idFamiliaAcero = famAcero.FamiliaAceroID;//dataItem["FamiliaAceroID"].Text.SafeIntParse();
                edita.NavigateUrl = String.Format("/Catalogos/DetFamAcero.aspx?ID={0}", idFamiliaAcero);
            }

        }

        protected void grdFamiliaAcero_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int familiaAceroID = e.CommandArgument.SafeIntParse();                
                try
                {
                    FamiliaAceroBO.Instance.Borra(familiaAceroID);
                    EstablecerDataSource();
                    grdFamiliaAcero.DataBind();
                }
                catch(BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdFamiliaAcero.ResetBind();
            EstablecerDataSource();
            grdFamiliaAcero.DataBind();
        }
      
    }
}