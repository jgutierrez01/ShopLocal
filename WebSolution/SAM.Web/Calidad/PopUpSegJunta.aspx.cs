using System;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities.Grid;
using SAM.Web.Classes;
using SAM.Web.Common;
using SAM.Web.Controles.Calidad.SegJunta;
using System.Web;

namespace SAM.Web.Calidad
{   
    public partial class PopUpSegJunta : SamPaginaPopup
    {
        private static int ProyectoID
        {
            get
            {
                return HttpContext.Current.Request.Params["PID"].SafeIntParse();
            }
        }

        private static int JuntaSpoolID
        {
            get
            {
                return HttpContext.Current.Request.Params["JSID"].SafeIntParse();
            }
        }

        private static bool EsJuntaCampo
        {
            get
            {
                return HttpContext.Current.Request.Params["EsJuntaCampo"].SafeBoolParse();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAJuntaSpool(JuntaSpoolID))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando accesar la junta {1} a la cual no tiene permisos", SessionFacade.UserId, JuntaSpoolID);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                GrdSeguimientoJunta det = SeguimientoJuntaBL.Instance.ObtenerDetalleSeguimientoJunta(ProyectoID, JuntaSpoolID, EsJuntaCampo);
                
                Controls.IterateRecursively(c =>
                {
                    if (c is ControlSegJunta)
                    {
                        ((ControlSegJunta)c).MapGeneric(det);
                    }
                });
            }
        }
    }
}