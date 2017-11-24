using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Administracion;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Administracion
{
    public partial class LstPerfiles : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Perfiles);
                EstablecerDataSource();
                grdPerfiles.DataBind();
            }
        }

        protected void grdPerfiles_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        private void EstablecerDataSource()
        {
            grdPerfiles.DataSource = PerfilBO.Instance.ObtenerTodos();
        }

        protected void grdPerfiles_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "borrar")
            {
                int perfilID = e.CommandArgument.SafeIntParse();
                try
                {
                    PerfilBO.Instance.Borra(perfilID);
                    grdPerfiles.Rebind();
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdPerfiles.ResetBind();
            grdPerfiles.Rebind();
        }
    }
}