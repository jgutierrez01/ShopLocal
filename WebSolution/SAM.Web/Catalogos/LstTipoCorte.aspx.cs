using System;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class LstTipoCorte : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_TiposCorte);
                EstablecerDataSource();
                BindLimpio();
            }
        }

        protected void grdTipoCorte_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdTipoCorte.DataSource = TipoCorteBO.Instance.ObtenerTodos();
        }

        private void BindLimpio()
        {
            grdTipoCorte.ResetBind();
            grdTipoCorte.DataBind();
        }

        // Comprobar si es posible eliminar el elemento seleccionado
        protected void grdTipoCorte_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "borrar")
            {
                int tipoCorteID = e.CommandArgument.SafeIntParse();
                try
                {
                    TipoCorteBO.Instance.Borra(tipoCorteID);
                    EstablecerDataSource();
                    BindLimpio();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            EstablecerDataSource();
            BindLimpio();
            grdTipoCorte.ResetBind();
            grdTipoCorte.Rebind();
        }
    }
}