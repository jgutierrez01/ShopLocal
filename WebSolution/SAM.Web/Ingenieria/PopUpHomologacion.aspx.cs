using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Ingenieria;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.Web.Common;

namespace SAM.Web.Ingenieria
{
    public partial class PopUpHomologacion : SamPaginaPopup
    {
        private int ProyectoID
        {
            get
            {
                return HttpContext.Current.Request["PID"].SafeIntParse();
            }
        }

        private bool SoloLectura
        {
            get
            {
                return HttpContext.Current.Request["RO"].SafeIntParse() == 1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoASpool(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar a un spool {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargarDatos();
            }
        }

        private void cargarDatos()
        {/*List<SpoolIng> spoolIngs = IngenieriaBL.Instance.ObtenerInfoArchivosSubidos(ProyectoID, SessionFacade.UserId);
            Spool spoolBD = SpoolBO.Instance.ObtenerDetalleHomologacion(EntityID.Value);
            SpoolIng spoolArchivo = spoolIngs.SingleOrDefault(x => x.Nombre.EqualsIgnoreCase(spoolBD.Nombre));
            if(spoolArchivo!=null)
            {
                CorteRO1.MapGeneric(spoolBD, spoolArchivo);
               // MaterialRO1.MapGeneric(spoolBD, spoolArchivo);
               // JuntaRO1.MapGeneric(spoolBD, spoolArchivo);
                SpoolRO1.MapGeneric(spoolBD,spoolArchivo);
            }
            if (!SoloLectura)
            {
                hlAceptar.NavigateUrl = string.Format("javascript:Sam.Ingenieria.HomologacionAcepta({0},true)",
                                                      EntityID.Value);
                imgAceptar.NavigateUrl = string.Format("javascript:Sam.Ingenieria.HomologacionAcepta({0},true)",
                                                     EntityID.Value);
                hlRechazar.NavigateUrl = string.Format("javascript:Sam.Ingenieria.HomologacionAcepta({0},false)",
                                                       EntityID.Value);
                imgRechazar.NavigateUrl = string.Format("javascript:Sam.Ingenieria.HomologacionAcepta({0},false)",
                                                       EntityID.Value);
            }else
            {
                hlAceptar.Visible = false;
                imgAceptar.Visible = false;
                hlRechazar.Visible = false;
                imgRechazar.Visible = false;
            }
            */
        }

        
    }
}