using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Common;
using SAM.Entities;
using Telerik.Web.UI;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Proyectos
{
    public partial class PendientesAutomaticos : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar item codes de un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_PendientesAutomaticos, EntityID.Value);
                lblTitulo.NavigateUrl = string.Format("~/Proyectos/DetProyecto.aspx?ID={0}", EntityID.Value);

                if (EntityID.Value > 0)
                {
                    cargarDatos();
                }
            }
        }

        private void cargarDatos()
        {
            headerProyecto.BindInfo(EntityID.Value);
            hdnProyectoID.Value = EntityID.Value.ToString();
            repPendientes.DataSource = ProyectoConfiguracionBO.Instance.ObtenerPendientesAutomaticos(EntityID.Value);
            repPendientes.DataBind();
        }

        protected void repPendientes_ItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            if (args.Item.IsItem())
            {
                ProyectoPendiente item = (ProyectoPendiente)args.Item.DataItem;
                HiddenField hdnTipoPendienteID = args.Item.FindControl("hdnTipoPendienteID") as HiddenField;
                RadComboBox radResponsable = args.Item.FindControl("radResponsable") as RadComboBox;
                Label lblPendiente = args.Item.FindControl("lblPendiente") as Label;

                hdnTipoPendienteID.Value = item.TipoPendienteID.ToString();
                lblPendiente.Text = LanguageHelper.CustomCulture == LanguageHelper.ESPANOL ? item.TipoPendiente.Nombre : item.TipoPendiente.NombreIngles;                
                radResponsable.SelectedValue = item.Responsable.ToString();
                radResponsable.Text = item.Usuario.Nombre + " " + item.Usuario.ApPaterno + " " + item.Usuario.ApMaterno;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            List<Simple> ppList = new List<Simple>();

            foreach (RepeaterItem item in repPendientes.Items)
            {
                if (item.IsItem())
                {
                    
                    RadComboBox radResponsable = item.FindControl("radResponsable") as RadComboBox;
                    HiddenField hdnTipoPendienteID = item.FindControl("hdnTipoPendienteID") as HiddenField;
                
                    Simple simple = new Simple();
                    simple.ID = hdnTipoPendienteID.Value.SafeIntParse();
                    simple.Valor = radResponsable.SelectedValue;

                    ppList.Add(simple);
                }
            }

            try
            {
                ProyectoConfiguracionBO.Instance.GuardaPendientesAutomaticos(EntityID.Value, ppList, SessionFacade.UserId);
                Response.Redirect(String.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value));
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }
    }
}