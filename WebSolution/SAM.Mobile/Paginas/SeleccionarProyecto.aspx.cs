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
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Ingenieria;

namespace SAM.Mobile.Paginas
{
    public partial class SeleccionarProyecto : PaginaMovilAutenticado
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
                if (Request.QueryString["OrdenTrabajo"] != null)
                {
                    CargaComboPorNumeroDeControl(Request.QueryString["OrdenTrabajo"].SafeStringParse());
                }

                if (Request.QueryString["NombreSpool"] != null)
                {
                    CargaComboPorNombreDeSpool(Request.QueryString["NombreSpool"].SafeStringParse());
                }
            }
        }

        private void CargaComboPorNumeroDeControl(string ordenTrabajo)
        {
            List<DetSpoolMobile> lstOrdenTrabajoSpool = OrdenTrabajoSpoolBO.Instance
                                                                .ObtenerListaOrdenTrabajoSpoolPorNumeroDeControl(ordenTrabajo)
                                                                .Where(x => x.PatioID == SessionFacade.PatioID)
                                                                .Where(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x.ProyectoID)).ToList();
            lstProyectos.DataSource = lstOrdenTrabajoSpool;
            lstProyectos.DataTextField = "Proyecto";
            lstProyectos.DataValueField = "SpoolID";
            lstProyectos.DataBind();

            lstProyectos.Items.Insert(0, String.Empty);
        }

        private void CargaComboPorNombreDeSpool(string nombreSpool)
        {
            List<DetSpoolMobile> lstSpool = SpoolBO.Instance.ObtenerPorNombre(nombreSpool)
                                                           .Where(x => x.PatioID == SessionFacade.PatioID)
                                                           .Where(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x.ProyectoID)).ToList();

            lstProyectos.DataSource = lstSpool;
            lstProyectos.DataTextField = "Proyecto";
            lstProyectos.DataValueField = "SpoolID";
            lstProyectos.DataBind();

            lstProyectos.Items.Insert(0, String.Empty);
        }

        protected void cmdOK_OnClik(object sender, EventArgs e)
        {
            if (lstProyectos.Selection.Value != String.Empty)
            {
                Response.Redirect(SessionFacade.CambioProyectoSigURL + "?ID=" + lstProyectos.Selection.Value.ToString());
            }
            else
            {
                lblError.Visible = true;
            }
        }
    }
}