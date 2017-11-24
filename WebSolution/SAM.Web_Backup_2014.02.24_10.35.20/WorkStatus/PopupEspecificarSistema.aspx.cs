using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Exceptions;

namespace SAM.Web.WorkStatus
{
    public partial class PopupEspecificarSistema : SamPaginaPopup
    {
        #region ViewState
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string ids = Request.QueryString["RPID"];
            Spools = ids.Split(',').Select(n => n.SafeIntParse()).ToArray();

            if (!SeguridadQs.TieneAccesoATodosLosSpools(Spools))
            {
                //Generar error 401 (Unauthorized access)
                string mensaje = string.Format("El usuario {0} está intentando especificar el sistema para algún spool {1} al cual no tiene permisos", SessionFacade.UserId, ids);
                UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
            }
        }

        /// <summary>
        /// Actualiza / Escribe los datos del sistema para los workstatus spool enviados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIncluir_OnClick(object sender, EventArgs e)
        {
            try
            {
                WorkstatusSpoolBO.Instance.EspecificarSistema(Spools, txtSistema.Text, txtColor.Text, txtCodigo.Text);
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}