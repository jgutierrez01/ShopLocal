using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using SAM.Web.Classes;

namespace SAM.Web.Proyectos
{
    public partial class DetProyecto : SamPaginaPrincipal
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

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.wks_Soldadura, EntityID.Value);
                cargaInformacion(EntityID.Value);
            }
        }

        public void cargaInformacion(int proyectoID)
        {
            Proyecto proyectoCargado = new Proyecto();

            headerProyecto.BindInfo(proyectoID);

            proyectoCargado = ProyectoBO.Instance.ObtenerInfoDashboard(proyectoID);
            //div izquierdo
            hlEditar.NavigateUrl = string.Format(WebConstants.ProyectoUrl.CONFIGURACION, proyectoID);
            imgEditar.NavigateUrl = string.Format(WebConstants.ProyectoUrl.CONFIGURACION, proyectoID);
            lblCliente.Text = proyectoCargado.Cliente.Nombre;
            lblFechaInicio.Text = proyectoCargado.FechaInicio.Value.Date.ToString();
            lblPatio.Text = proyectoCargado.Patio.Nombre;
            lblColor.Text = proyectoCargado.Color.Nombre;
            lblCodigoNumUnico.Text = proyectoCargado.ProyectoConfiguracion.PrefijoNumeroUnico;
            lblCodigoOdt.Text = proyectoCargado.ProyectoConfiguracion.PrefijoOrdenTrabajo;
            lblDigitoNumUnico.Text = proyectoCargado.ProyectoConfiguracion.DigitosNumeroUnico.ToString();
            lblDigitoOdt.Text = proyectoCargado.ProyectoConfiguracion.DigitosOrdenTrabajo.ToString();

            //divcentral
            lblNombre.Text = proyectoCargado.Contacto.Nombre;
            lblApellidos.Text = proyectoCargado.Contacto.ApPaterno + " " + proyectoCargado.Contacto.ApMaterno;
            lblTelefono.Text = proyectoCargado.Contacto.TelefonoParticular;
            lblTelefonoOficina.Text = proyectoCargado.Contacto.TelefonoOficina;
            lblTelefonoCelular.Text = proyectoCargado.Contacto.TelefonoCelular;
            lblCorreo.Text = proyectoCargado.Contacto.CorreoElectronico;



            //divIzquierdo
            hlProveedores.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_PROVEEDORES, proyectoID);
            hlFabricantes.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_FABRICANTES, proyectoID);
            hlTransportistas.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_TRANSPORTISTAS, proyectoID);
            hlItemCodes.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_ITEM_CODES, proyectoID);
            hlIcEquivalentes.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_ITEM_CODES_EQUIVALENTES, proyectoID);
            hlColadas.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_COLADAS, proyectoID);
            hlWps.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_WPS, proyectoID);
            hlDossierCalidad.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_DOSSIER, proyectoID);
            hlTipoReporteProyecto.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_TIPOS_REPORTES, proyectoID);
            hlPendientesAutomaticos.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PENDIENTES_AUTOMATICOS, proyectoID);
            hlProgramacion.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROGRAMACION, proyectoID);
        }
    }
}
