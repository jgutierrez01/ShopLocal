using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.Web.Common;
using SAM.Entities;
using SAM.BusinessLogic.Produccion;
using SAM.Web.Classes;


namespace SAM.Web.Produccion
{
    public partial class PopupTaller : SamPaginaPopup
    {
        #region variables Privadas
        /// <summary>
        /// ID del proyecto en cuestión
        /// </summary>
        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private string[] SpoolsIDs
        {
            get
            {
                return (string[])ViewState["SpoolsIds"];
            }
            set
            {
                ViewState["SpoolsIds"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string temp = Request.QueryString["SPIDS"];
                SpoolsIDs = temp.Split(',');
                ProyectoID = Request.QueryString["PID"].SafeIntParse();
                carga();
            }
        }

        private void carga()
        {
            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorProyecto(ProyectoID));
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                List<int> spools = new List<int>();
                foreach(string sp in SpoolsIDs)
                {
                    spools.Add(sp.SafeIntParse());
                }

                //SAM.Entities.OrdenTrabajoEspecial odte = OrdenTrabajoEspecialBL.Instance.GeneraNueva(ProyectoID, spools, ddlTaller.SelectedValue.SafeIntParse(),
                //    SessionFacade.UserId, false);

                //EnviarMensaje(odte);
            }
        }

        //protected void EnviarMensaje(SAM.Entities.OrdenTrabajoEspecial odte)
        //{
        //    phControles.Visible = false;
        //    phMensaje.Visible = true;
        //    litNumOdt.Text = odte.NumeroOrden;

        //    hlReporteOdt.NavigateUrl = WebConstants.ProduccionUrl.ReporteODTEspecial + string.Format("?ID={0}&PDF=true", odte.OrdenTrabajoEspecialID);

        //    JsUtils.RegistraScriptActualizaGridCruceRevisiones(this);
        //}
    }
}