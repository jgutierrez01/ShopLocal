using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Catalogos;

namespace SAM.Web.Proyectos
{
    public partial class DossierCalidad : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAProyecto(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar el dossier de calidad de un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_DossierCalidad, EntityID.Value);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.DET_PROYECTO, EntityID.Value);
                cargaInformacion(EntityID.Value);
            }
        }

        public void cargaInformacion(int proyectoID)
        {
            headerProyecto.BindInfo(proyectoID);

            Proyecto proyecto = ProyectoBO.Instance.ObtenerConDossier(proyectoID);
            
            chkDossier.Items[0].Selected = proyecto.ProyectoDossier.Trazabilidad;
            chkDossier.Items[1].Selected = proyecto.ProyectoDossier.WPS;
            chkDossier.Items[2].Selected = proyecto.ProyectoDossier.MTR;
            chkDossier.Items[3].Selected = proyecto.ProyectoDossier.ReporteInspeccionVisual;
            chkDossier.Items[4].Selected = proyecto.ProyectoDossier.ReporteLiberacionDimensional;
            chkDossier.Items[5].Selected = proyecto.ProyectoDossier.ReporteEspesores;
            chkDossier.Items[6].Selected = proyecto.ProyectoDossier.ReporteRT;
            chkDossier.Items[7].Selected = proyecto.ProyectoDossier.ReportePT;
            chkDossier.Items[8].Selected = proyecto.ProyectoDossier.ReportePwht;
            chkDossier.Items[9].Selected = proyecto.ProyectoDossier.ReporteDurezas;
            chkDossier.Items[10].Selected = proyecto.ProyectoDossier.ReporteRTPostTT;
            chkDossier.Items[11].Selected = proyecto.ProyectoDossier.ReportePTPostTT;
            chkDossier.Items[12].Selected = proyecto.ProyectoDossier.ReportePreheat;
            chkDossier.Items[13].Selected = proyecto.ProyectoDossier.ReporteUT;
            chkDossier.Items[14].Selected = proyecto.ProyectoDossier.ReportesPintura;
            chkDossier.Items[15].Selected = proyecto.ProyectoDossier.Embarque;
            chkDossier.Items[16].Selected = proyecto.ProyectoDossier.MTRSoldadura;
            chkDossier.Items[17].Selected = proyecto.ProyectoDossier.Drawing;
            chkDossier.Items[18].Selected = proyecto.ProyectoDossier.ReportePMI;

            rdbMTR.Items[0].Selected = proyecto.ProyectoDossier.MTRCertificado;
            rdbMTR.Items[1].Selected = !proyecto.ProyectoDossier.MTRCertificado;

            rdbLD.Items[0].Selected = proyecto.ProyectoDossier.LDCertificado.Value;
            rdbLD.Items[1].Selected = !proyecto.ProyectoDossier.LDCertificado.Value;
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
                Proyecto proyecto = ProyectoBO.Instance.ObtenerConDossier(EntityID.Value);
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
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        private void unbindCheckBoxList(Proyecto proyecto)
        {
            ProyectoDossier dossier = proyecto.ProyectoDossier;
            dossier.StartTracking();

            dossier.Trazabilidad = chkDossier.Items[0].Selected;
            dossier.WPS = chkDossier.Items[1].Selected;
            dossier.MTR = chkDossier.Items[2].Selected;
            dossier.ReporteInspeccionVisual = chkDossier.Items[3].Selected;
            dossier.ReporteLiberacionDimensional = chkDossier.Items[4].Selected;
            dossier.ReporteEspesores = chkDossier.Items[5].Selected;
            dossier.ReporteRT = chkDossier.Items[6].Selected;
            dossier.ReportePT = chkDossier.Items[7].Selected;
            dossier.ReportePwht = chkDossier.Items[8].Selected;
            dossier.ReporteDurezas = chkDossier.Items[9].Selected;
            dossier.ReporteRTPostTT = chkDossier.Items[10].Selected;
            dossier.ReportePTPostTT = chkDossier.Items[11].Selected;
            dossier.ReportePreheat = chkDossier.Items[12].Selected;
            dossier.ReporteUT = chkDossier.Items[13].Selected;
            dossier.ReportesPintura = chkDossier.Items[14].Selected;
            dossier.Embarque = chkDossier.Items[15].Selected;
            dossier.MTRSoldadura = chkDossier.Items[16].Selected;
            dossier.Drawing = chkDossier.Items[17].Selected;
            dossier.ReportePMI = chkDossier.Items[18].Selected;
            dossier.MTRCertificado = rdbMTR.Items[0].Selected;
            dossier.LDCertificado = rdbLD.Items[0].Selected;            
            
            dossier.UsuarioModifica = SessionFacade.UserId;
            dossier.FechaModificacion = DateTime.Now;

            dossier.StopTracking();
        }


    }
}