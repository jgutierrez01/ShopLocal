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

namespace SAM.Web.WorkStatus
{
    public partial class PopUpPreparar : SamPaginaPopup
    {
        public int[] WorkstatusSpools
        {
            get
            {
                if (ViewState["WorkstatusSpools"] != null)
                {
                    return (int[])ViewState["WorkstatusSpools"];
                }
                return null;
            }
            set
            {
                ViewState["WorkstatusSpools"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WorkstatusSpools = Request.QueryString["IDs"].Split(',').Select(n => n.SafeIntParse()).ToArray();
            }
        }

        /// <summary>
        /// Evento que etiqueta los spools enviados por query string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPreparar_Click(object sender, EventArgs e)
        {
            try
            {
                EmbarqueBO.Instance.PrepararSpools(WorkstatusSpools, mdpFecha.SelectedDate.Value, SessionFacade.UserId);
                JsUtils.RegistraScriptActualizayCierraVentana(this);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}