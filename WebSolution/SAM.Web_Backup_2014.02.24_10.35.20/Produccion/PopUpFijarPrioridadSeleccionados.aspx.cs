using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Produccion
{
    public partial class PopUpFijarPrioridadSeleccionados : SamPaginaPopup
    {
        private int[] _spools
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

        private int _proyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] != null)
                {
                    return (int)ViewState["ProyectoID"];
                }

                return -1;
            }
            set 
            {
                ViewState["ProyectoID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _spools = Request.QueryString["IDs"].Split(',').Select(n => n.SafeIntParse()).ToArray();
            _proyectoID = Request.QueryString["PID"].SafeIntParse();
        }

        protected void btnFijarPrioridad_Click(object sender, EventArgs e)
        {
            try
            {
                //Obtiene los ids de los spools a los cuales se le debe fijar la prioridad y manda llamar
                //el método de la capa de negocios correspondiente
                SpoolBO.Instance.FijaPrioridad(_spools,
                                                _proyectoID,
                                                SessionFacade.UserId,
                                                DateTime.Now,
                                                txtPrioridad.Text.SafeIntParse());

               //Manda actualizar grdSpools de la pagina padre
                JsUtils.RegistraScriptActualizayCierraVentana(this);
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }
    }
}