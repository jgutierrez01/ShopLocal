using System;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class LstTipoJunta : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_TiposJunta);
                EstablecerDataSource();
                BindLimpio();
            }
        }

        protected void grdTipoJunta_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdTipoJunta.DataSource = TipoJuntaBO.Instance.ObtenerTodos();
        }

        private void BindLimpio()
        {
            grdTipoJunta.ResetBind();
            grdTipoJunta.DataBind();
        }

        // Comprobar si es posible eliminar el elemento seleccionado
        protected void grdTipoJunta_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "borrar")
            {
                int tipoJuntaID = e.CommandArgument.SafeIntParse();
                try
                {
                    TipoJuntaBO.Instance.Borra(tipoJuntaID);
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
            grdTipoJunta.ResetBind();
            grdTipoJunta.Rebind();
        }
    }
}