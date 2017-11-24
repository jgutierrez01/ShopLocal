using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Materiales;

namespace SAM.Web.Materiales
{
    public partial class PopUpPinturaNumeroUnico : SamPaginaPopup
    {

        private int[] _IDs
        {
            get
            {
                if (ViewState["IDs"] != null)
                {
                    return (int[])ViewState["IDs"];
                }
                return null;
            }
            set
            {
                ViewState["IDs"] = value;
            }
        }

        /// <summary>
        /// ID del proyecto seleccionado en el dropdown
        /// </summary>
        private int _proyecto
        {
            get
            {
                return (int)ViewState["proyecto"];
            }
            set
            {
                ViewState["proyecto"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string ids = Request.QueryString["IDs"];
            _IDs = ids.Split(',').Select(n => n.SafeIntParse()).ToArray();
            _proyecto = int.Parse(Request.QueryString["proyID"]);


        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (txtNumReportePrimarios.Text != null | txtNumReporteIntermedio.Text != null | dtpFechaIntermedios.SelectedDate != null | dtpFechaPrimarios.SelectedDate != null)
                    {

                        PinturaNumeroUnicoBO.Instance.GuardaRequisicionPintura(_IDs,
                                                                               _proyecto,
                                                                               txtNumReportePrimarios.Text,
                                                                               dtpFechaPrimarios.SelectedDate,
                                                                               txtNumReporteIntermedio.Text,
                                                                               dtpFechaIntermedios.SelectedDate,
                                                                               chkLiberado.Checked,
                                                                               SessionFacade.UserId
                                                                               );
                   }

                    JsUtils.RegistraScriptActualizaPinturaNumUnico(this);
                    JsUtils.RegistraScriptActualizaRequisicionPinturaNumUnico(this);
                }

                catch (BaseValidationException bve)
                {
                    RenderErrors(bve, "vgRequisita");
                }
            }
        }
    }
}