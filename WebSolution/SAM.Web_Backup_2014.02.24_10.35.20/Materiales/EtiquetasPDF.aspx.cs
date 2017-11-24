using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.BusinessObjects.Materiales;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Mimo.Framework.Common;
using System.IO;

namespace SAM.Web.Materiales
{
    public partial class EtiquetasPDF : SamPaginaConSeguridad
    {
        protected override void OnLoad(EventArgs e)
        {
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SeguridadQs.TieneAccesoARecepcion(EntityID.Value))
            {
                //Generar error 401 (Unauthorized access)
                string mensaje = string.Format("El usuario {0} está intentando accesar una recepción {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
            }
                
            int proyectoID = Request.QueryString["ProyectoID"].SafeIntParse();

            if (UtileriasReportes.ReporteExiste(UtileriasReportes.DameRutaReporte(proyectoID, TipoReporteProyectoEnum.EtiquetaMaterial)))
            {
               
                byte[] reporte = UtileriasReportes.ObtenEtiquetaMaterial(proyectoID, EntityID.Value);

                UtileriasReportes.EnviaReporteComoPdf(this, reporte, "EtiquetasMaterial.pdf");

            }
            else
            {
                string rutaCompleta = Cultura == LanguageHelper.INGLES ? Server.MapPath("~/ArchivosAuxiliares/EtiquetaMaterial.en-US.pdf") : Server.MapPath("~/ArchivosAuxiliares/EtiquetaMaterial.pdf");
                UtileriasReportes.EnviaReporteComoPdf(this, File.ReadAllBytes(rutaCompleta), "EtiquetaMaterial.pdf");
            }


            //ConstructorEtiquetaPDF.CreatePDFForWeb(EntityID.Value);
        }
    }
}