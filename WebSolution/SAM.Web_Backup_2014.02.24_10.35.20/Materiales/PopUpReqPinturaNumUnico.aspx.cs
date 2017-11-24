using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Materiales;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Materiales
{
    public partial class PopUpReqPinturaNumUnico : SamPaginaPopup
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

            if (!IsPostBack)
            {
                string ids = Request.QueryString["IDs"];
                _IDs = ids.Split(',').Select(n => n.SafeIntParse()).ToArray();
                _proyecto = int.Parse(Request.QueryString["proyID"]);

                if (!SeguridadQs.TieneAccesoAProyecto(_proyecto))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando generar una requisición de pintura para números únicos de un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, _proyecto);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }
            }
        }

        protected void btnRequisitar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    RequisicionNumeroUnicoBO.Instance.GenerarRequisicion(_IDs, _proyecto, txtNumRequisicion.Text, dtpFechaRequisicion.SelectedDate, SessionFacade.UserId);
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