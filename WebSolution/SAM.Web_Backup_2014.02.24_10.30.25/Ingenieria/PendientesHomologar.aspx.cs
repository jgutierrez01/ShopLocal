using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Ingenieria;
using SAM.Entities;
using SAM.Web.Classes;

namespace SAM.Web.Ingenieria
{
    public partial class PendientesHomologar : SamPaginaConSeguridad
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(EntityID.HasValue)
                {
                    filtroGenerico.CargaProyecto(EntityID.Value);
                    btnMostrarClick(sender, e);
                }
            }
        }

        protected void repHomologacion_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                HyperLink hlNombreSpool = (HyperLink)e.Item.FindControl("hlNombreSpool");
                SpoolPendiente spool = e.Item.DataItem as SpoolPendiente;

                hlNombreSpool.Text = spool.Nombre;
                hlNombreSpool.NavigateUrl = "HomologacionSpool.aspx?ID=" + spool.SpoolPendienteID;
            }
        }

        protected void btnMostrarClick(object sender, EventArgs e)
        {
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            pnlHomologacion.Visible = true;
            repHomologacion.DataSource = IngenieriaBL.Instance.ObtenerPendientesPorHomologar(ProyectoID);
            repHomologacion.DataBind();
            SpoolPendienteHelper.GeneraSpoolsPendientesPorHomologar(ProyectoID);
        }

        protected int ProyectoID
        {
            get
            {
                if (ViewState["ProyectoID"] != null)
                    return ViewState["ProyectoID"].SafeIntParse();
                return -1;
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }


        protected void proyecto_Cambio(object sender, EventArgs e)
        {
            pnlHomologacion.Visible = false;
            ProyectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse();
        }
    }
}