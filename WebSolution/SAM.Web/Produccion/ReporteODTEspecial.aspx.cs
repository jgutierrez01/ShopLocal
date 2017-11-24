using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Web.Common;


namespace SAM.Web.Produccion
{
    public partial class ReporteODTEspecial : SamPaginaPrincipal
    {
        protected override void OnLoad(EventArgs e)
        {
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAOrdenDeTrabajo(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando descargar el reporte de una ODT {1} para la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                byte[] reporte = UtileriasReportes.ObtenReporteOdtEspecial(EntityID.Value, true, true, true, true, true);
                UtileriasReportes.EnviaReporteComoPdf(this, reporte, string.Format("ODT #{0}.pdf", EntityID.Value));
            }
        }

    }
}