using System;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Catalogos
{
    public partial class LstCedula : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Cedulas);
                EstablecerDataSource();
                BindLimpio();
            }
        }

        protected void grdCedula_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdCedula.DataSource = CedulaBO.Instance.ObtenerTodos();
        }

        private void BindLimpio()
        {
            grdCedula.ResetBind();
            grdCedula.DataBind();
        }

        // Comprobar si es posible eliminar el elemento seleccionado
        protected void grdCedula_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "borrar")
            {
                int cedulaID = e.CommandArgument.SafeIntParse();
                try
                {
                    CedulaBO.Instance.Borra(cedulaID);
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
            grdCedula.ResetBind();
            grdCedula.Rebind();
        }
    }
}