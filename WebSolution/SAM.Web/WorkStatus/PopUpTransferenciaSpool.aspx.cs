using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Exceptions;
using System.Globalization;
using Mimo.Framework.Common;
using SAM.Web.Common;
using SAM.BusinessObjects.Produccion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Validations;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using System.Globalization;
using Mimo.Framework.Common;
using SAM.Web.Common;
using System.Web.Script.Serialization;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Catalogos;

using Mimo.Framework.WebControls;
namespace SAM.Web.WorkStatus
{
    public partial class PopUpTransferenciaSpool : SamPaginaPopup
    {

        public int[] TransferenciaSpools
        {
            get
            {
                if (ViewState["TransferenciaSpools"] != null)
                {
                    return (int[])ViewState["TransferenciaSpools"];
                }
                return null;
            }
            set
            {
                ViewState["TransferenciaSpools"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TransferenciaSpools = Request.QueryString["IDs"].Split(',').Select(n => n.SafeIntParse()).ToArray();
                mdpFechaTransferencia.SelectedDate = DateTime.Now;
                cargaCombo();
            }
        }

        protected void btnTransferencia_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    TransferenciaSpoolBO.Instance.GuardaTransferencia(TransferenciaSpools, mdpFechaTransferencia.SelectedDate.Value, txtNumeroTransferencia.Text, ddlDestino.SelectedItem.Value.SafeIntParse(), SessionFacade.UserId);
                    phControles.Visible = false;
                    phMensajeExito.Visible = true;
                }
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
            finally
            {
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }

        }

        private void cargaCombo()
        {
            ddlDestino.BindToEnumerableWithEmptyRow(DestinoBO.Instance.ObtenerDestinos(), "Nombre", "DestinoID", null);
        }

    }
}