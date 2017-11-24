using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopupPrepararSpoolTransferencia : SamPaginaPopup
    {

        public int[] Spools
        {
            get
            {
                if (ViewState["Spools"] != null)
                {
                    return (int[])ViewState["Spools"];
                }
                return null;
            }
            set
            {
                ViewState["Spools"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Spools = Request.QueryString["IDs"].Split(',').Select(n => n.SafeIntParse()).ToArray();
            }
        }

        protected void btnPreparar_Click(object sender, EventArgs e)
        {
            try
            {
                TransferenciaSpoolBO.Instance.PrepararSpools(Spools, mdpFecha.SelectedDate.Value, SessionFacade.UserId);
                JsUtils.RegistraScriptActualizayCierraVentana(this);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

    }
}