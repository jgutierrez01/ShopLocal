using System;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class LstDiametro : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Diametro);
                EstablecerDataSource();
                BindLimpio();
            }
        }

        protected void grdDiametro_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdDiametro.DataSource = DiametroBO.Instance.ObtenerTodos();
        }

        private void BindLimpio()
        {
            grdDiametro.ResetBind();
            grdDiametro.DataBind();
        }


        // Comprobar si es posible eliminar el elemento seleccionado
        protected void grdDiametro_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if(e.CommandName == "borrar")
            {
                int diametroID = e.CommandArgument.SafeIntParse();
                try
                {
                    DiametroBO.Instance.Borra(diametroID);
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
            grdDiametro.ResetBind();
            grdDiametro.Rebind();
        }
    }
}