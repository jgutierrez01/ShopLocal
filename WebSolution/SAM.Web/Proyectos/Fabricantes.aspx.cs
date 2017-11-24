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
    public partial class Fabricantes : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar fabricantes para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_Fabricantes, EntityID.Value);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                cargaInformacion(EntityID.Value);
            }
        }

        public void cargaInformacion(int proyectoID)
        {
            headerProyecto.BindInfo(proyectoID);

            chkFabricantes.DataSource = CacheCatalogos.Instance.ObtenerFabricantes().OrderBy(x => x.Nombre);
            chkFabricantes.DataTextField = "Nombre";
            chkFabricantes.DataValueField = "ID";
            chkFabricantes.DataBind();

            List<Fabricante> fabricante = FabricanteBO.Instance.ObtenerPorProyecto(proyectoID);
            int fabricanteID;
            int i;

            for (i = chkFabricantes.Items.Count - 1; i >= 0; i--)
            {
                fabricanteID = chkFabricantes.Items[i].Value.SafeIntParse();

                if (fabricante.Any(x => x.FabricanteID == fabricanteID))
                {
                    chkFabricantes.Items[i].Selected = true;
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
                Proyecto proyecto = ProyectoBO.Instance.ObtenerConFabricantes(EntityID.Value);
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
            int fabricanteID;
            FabricanteProyecto fabricante;

            foreach (ListItem item in chkFabricantes.Items)
            {
                fabricanteID = item.Value.SafeIntParse();

                fabricante = proyecto.FabricanteProyecto.Where(x => x.FabricanteID == fabricanteID).SingleOrDefault();

                //si ya existe en la base de datos;
                if (fabricante != null)
                {
                    fabricante.StartTracking();

                    // si ya no está seleccionado, se borra.
                    if (!item.Selected)
                    {
                        fabricante.MarkAsDeleted();
                    }
                    else //si sigue seleccionado, solo se actualiza.
                    {
                        fabricante.UsuarioModifica = SessionFacade.UserId;
                        fabricante.FechaModificacion = DateTime.Now;
                    }

                    fabricante.StopTracking();
                }
                // si no está en BD pero si está seleccionado, se debe agregar.
                else if (item.Selected)
                {
                    fabricante = new FabricanteProyecto();
                    fabricante.FabricanteID = fabricanteID;
                    fabricante.ProyectoID = proyecto.ProyectoID;
                    fabricante.FechaModificacion = DateTime.Now;
                    fabricante.UsuarioModifica = SessionFacade.UserId;

                    proyecto.FabricanteProyecto.Add(fabricante);
                }
            }
        }

    }
}