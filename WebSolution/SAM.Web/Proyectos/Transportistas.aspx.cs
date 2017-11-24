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
using SAM.Web.Common;

namespace SAM.Web.Proyectos
{
    public partial class Transportistas : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar transportistas para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_Transportistas, EntityID.Value);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                cargaInformacion(EntityID.Value);
            }
        }

        /// <summary>
        /// carga la informacion en el encabezado del proyecto
        /// así como en la lista de checkboxes de transportistas.
        /// Obtiene todo los transportistas para ese proyecto y selecciona el checkbox
        /// correspondiente.
        /// </summary>
        /// <param name="proyectoID"></param>
        public void cargaInformacion(int proyectoID)
        {
            headerProyecto.BindInfo(proyectoID);

            chkTransportistas.DataSource = CacheCatalogos.Instance.ObtenerTransportistas().OrderBy(x => x.Nombre);
            chkTransportistas.DataTextField = "Nombre";
            chkTransportistas.DataValueField = "ID";
            chkTransportistas.DataBind();

            List<Transportista> transportista = TransportistaBO.Instance.ObtenerPorProyecto(proyectoID);
            int transportistaID;
            int i;

            for (i = chkTransportistas.Items.Count - 1; i >= 0; i--)
            {
                transportistaID = chkTransportistas.Items[i].Value.SafeIntParse();

                if (transportista.Any(x => x.TransportistaID == transportistaID))
                {
                    chkTransportistas.Items[i].Selected = true;
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
                Proyecto proyecto = ProyectoBO.Instance.ObtenerConTransportistas(EntityID.Value);
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

        /// <summary>
        /// recibe un indice de proyecto con el cuál valida si existe un fabricante de proyecto para el item en 
        /// el que se encuentra.
        /// si ya existe y sigue seleccionado, lo guarda
        /// si ya existe y no esta seleccionado, lo borra
        /// si no existe y esta seleccionado, lo agruega
        /// si no existe y no está seleccionado, no hace nada.
        /// </summary>
        /// <param name="proyecto"></param>
        private void unbindCheckBoxList(Proyecto proyecto)
        {
            int transportistaID;
            TransportistaProyecto transportista;

            foreach (ListItem item in chkTransportistas.Items)
            {
                transportistaID = item.Value.SafeIntParse();

                transportista = proyecto.TransportistaProyecto.Where(x => x.TransportistaID == transportistaID).SingleOrDefault();

                //si ya existe en la base de datos;
                if (transportista != null)
                {
                    transportista.StartTracking();

                    // si ya no está seleccionado, se borra.
                    if (!item.Selected)
                    {
                        transportista.MarkAsDeleted();
                    }
                    else //si sigue seleccionado, solo se actualiza.
                    {
                        transportista.UsuarioModifica = SessionFacade.UserId;
                        transportista.FechaModificacion = DateTime.Now;
                    }

                    transportista.StopTracking();
                }
                // si no está en BD pero si está seleccionado, se debe agregar.
                else if (item.Selected)
                {
                    transportista = new TransportistaProyecto();
                    transportista.TransportistaID = transportistaID;
                    transportista.ProyectoID = proyecto.ProyectoID;
                    transportista.FechaModificacion = DateTime.Now;
                    transportista.UsuarioModifica = SessionFacade.UserId;

                    proyecto.TransportistaProyecto.Add(transportista);
                }
            }
        }


    }
}