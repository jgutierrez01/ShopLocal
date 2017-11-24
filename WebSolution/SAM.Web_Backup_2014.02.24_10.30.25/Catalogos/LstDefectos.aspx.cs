using System;
using System.Globalization;
using System.Linq;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Administracion;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Web.Classes;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

namespace SAM.Web.Catalogos
{
    public partial class LstDefecto : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Defectos);
                EstablecerDataSource();
                grdDefectos.DataBind();
            }
        }

        protected void grdDefectos_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdDefectos.DataSource = DefectoBO.Instance.ObtenerTodosConTipoPrueba();
        }

        // Comprobar si es posible eliminar el elemento seleccionado
        protected void grdDefectos_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "borrar")
            {
                int defectoID = e.CommandArgument.SafeIntParse();
                try
                {
                    DefectoBO.Instance.Borra(defectoID);
                    EstablecerDataSource();
                    grdDefectos.DataBind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdDefectos.ResetBind();
            EstablecerDataSource();
            grdDefectos.DataBind();
        }
    }
}