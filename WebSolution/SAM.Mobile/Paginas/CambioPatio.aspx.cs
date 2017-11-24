using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.MobileControls;
using SAM.Mobile.Clases;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;

namespace SAM.Mobile.Paginas
{
    public partial class CambioPatio : PaginaMovilAutenticado
    {
        protected override void OnLoad(EventArgs e)
        {
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargaCombo();
            }
        }

        private void CargaCombo()
        {
            lstPatios.DataSource = UserScope.MisPatios;
            lstPatios.DataTextField = "Nombre";
            lstPatios.DataValueField = "ID";
            lstPatios.DataBind();

            lstPatios.Items.Insert(0, String.Empty);
        }

        protected void cmdOK_OnClik (object sender, EventArgs e)
        {
            if (lstPatios.Selection.Value != String.Empty)
            {
                SessionFacade.PatioID = lstPatios.Selection.Value.SafeIntParse();
                SessionFacade.PatioNombre = PatioBO.Instance.Obtener(lstPatios.Selection.Value.SafeIntParse()).Nombre;
                Response.Redirect(WebConstants.MobileUrl.DASHBOARD);
            }
            else
            {
                lblError.Visible = true;
            }
        }
        
    }
}