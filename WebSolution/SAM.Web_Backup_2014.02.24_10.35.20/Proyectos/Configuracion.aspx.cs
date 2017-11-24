using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Proyectos;

namespace SAM.Web.Proyectos
{
    public partial class Configuracion : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                ctrlInformacion.CargaCombos();
                cargaInformacion(EntityID.Value);
            }
        }

        public void cargaInformacion(int proyectoID)
        {
            Proyecto proyectoCargado = ProyectoBO.Instance.ObtenerProyectoConfiguracion(proyectoID);

            headerProyecto.BindInfo(proyectoID);
            ctrlInformacion.Map(proyectoCargado);
            ctrlContacto.Map(proyectoCargado.Contacto);
            ctrlConfiguracion.Map(proyectoCargado);
            ctrlConfiguracionLibre.Map(proyectoCargado);
        }

        public void btnGuardar_GuardaConfiguracion(object sender, EventArgs e)
        {

            if (IsValid)
            {
                Proyecto proyecto = ProyectoBO.Instance.ObtenerProyectoConfiguracion(EntityID.Value);
                string nombreOriginal = proyecto.Nombre;

                ctrlInformacion.Unmap(proyecto);
                ctrlContacto.Unmap(proyecto.Contacto);
                ctrlConfiguracion.Unmap(proyecto);
                ctrlConfiguracionLibre.Unmap(proyecto);

                try
                {
                    ProyectoBL.Instance.GuardaProyecto(proyecto, nombreOriginal);
                    Response.Redirect(String.Format(WebConstants.ProyectoUrl.DET_PROYECTO, proyecto.ProyectoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}