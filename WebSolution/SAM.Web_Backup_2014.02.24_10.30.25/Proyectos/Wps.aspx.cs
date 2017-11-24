using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Proyectos
{
    public partial class Wps : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando wps de un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_Wps, EntityID.Value);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                cargaInformacion(EntityID.Value);
            }
        }


        /// <summary>
        /// carga la información al header control y hace el bind de todos los 
        /// WPS disponibles a la lista de checkboxes
        /// </summary>
        /// <param name="proyectoID"></param>
        public void cargaInformacion(int proyectoID)
        {
            headerProyecto.BindInfo(proyectoID);

            chkWps.DataSource = CacheCatalogos.Instance.ObtenerWps().OrderBy(x => x.Nombre);
            chkWps.DataTextField = "Nombre";
            chkWps.DataValueField = "ID";
            chkWps.DataBind();

            List<Entities.Wps> wps = WpsBO.Instance.ObtenerPorProyecto(proyectoID);
            int wpsID;
            int i;

            for (i = chkWps.Items.Count - 1; i >= 0; i--)
            {
                wpsID = chkWps.Items[i].Value.SafeIntParse();

                if (wps.Any(x => x.WpsID == wpsID))
                {
                    chkWps.Items[i].Selected = true;
                }
            }
        }

        /// <summary>
        /// crea una entidad proyecto y manda llamar el método unbindcheckboxlist
        /// el cual guardará los cambios en la entidad proyecto.
        /// Esta entidad se guarda en la base de datos en la llamada a Guarda();
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Proyecto proyecto = ProyectoBO.Instance.ObtenerConWps(EntityID.Value);
                proyecto.StartTracking();
                unbindCheckBoxList(proyecto);
                proyecto.StopTracking();

                try
                {
                    ProyectoBO.Instance.Guarda(proyecto);
                    Response.Redirect(String.Format(WebConstants.ProyectoUrl.DET_PROYECTO, proyecto.ProyectoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        private void unbindCheckBoxList(Proyecto proyecto)
        {
            int wpsID;
            WpsProyecto wps;

            foreach (ListItem item in chkWps.Items)
            {
                wpsID = item.Value.SafeIntParse();

                wps = proyecto.WpsProyecto.Where(x => x.WpsID == wpsID).SingleOrDefault();

                //si ya existe en la base de datos;
                if (wps != null)
                {
                    wps.StartTracking();

                    // si ya no está seleccionado, se borra.
                    if (!item.Selected)
                    {
                        wps.MarkAsDeleted();
                    }
                    else //si sigue seleccionado, solo se actualiza.
                    {
                        wps.UsuarioModifica = SessionFacade.UserId;
                        wps.FechaModificacion = DateTime.Now;
                    }

                    wps.StopTracking();
                }
                // si no está en BD pero si está seleccionado, se debe agregar.
                else if (item.Selected)
                {
                    wps = new WpsProyecto();
                    wps.WpsID = wpsID;
                    wps.ProyectoID = proyecto.ProyectoID;
                    wps.FechaModificacion = DateTime.Now;
                    wps.UsuarioModifica = SessionFacade.UserId;

                    proyecto.WpsProyecto.Add(wps);
                }
            }
        }
    }
}