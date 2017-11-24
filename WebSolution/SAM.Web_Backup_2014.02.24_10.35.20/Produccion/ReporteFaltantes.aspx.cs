using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Microsoft.Reporting.WebForms;
using SAM.Entities.Reportes;
using SAM.BusinessLogic.Cruce;
using SAM.Common;
using System.IO;
using Mimo.Framework.Common;

namespace SAM.Web.Produccion
{

    public partial class ReporteFaltantes : SamPaginaBase
    {
        #region Variables privadas de ViewState

        private Guid ReporteUID
        {
            get
            {
                return (Guid)ViewState["ReporteUID"];
            }
            set
            {
                ViewState["ReporteUID"] = value;
            }
        }

        #endregion


        /// <summary>
        /// Toma lo de query string y busca ese reporte en la ruta física esperada,
        /// si no lo encuentra envía un PDF default diciendo que no se ha terminado de generar el reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReporteUID = new Guid(Request.QueryString["ID"]);
                string tipo = Request.QueryString["Type"];

                if (tipo.Equals("pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    enviaPdfFaltantes();
                }
                else if (tipo.Equals("xlsx", StringComparison.InvariantCultureIgnoreCase))
                {
                    enviaExcelFaltantes();
                }
                else
                {
                    throw new Exception("El tipo de reporte solicitado no existe");
                }
            }
        }

        private void enviaExcelFaltantes()
        {
            string directorio = Configuracion.RutaParaAlmacenarArchivos;
            string rutaCompleta = string.Concat(directorio, Path.DirectorySeparatorChar, ReporteUID, ".xlsx");

            //Si por algún motivo el reporte aun no está listo, envíamos un PDF que tiene la leyenda correspondiente
            if (!File.Exists(rutaCompleta))
            {
                rutaCompleta = Cultura == LanguageHelper.INGLES ? Server.MapPath("~/ArchivosAuxiliares/ReportePendiente.en-US.pdf") : Server.MapPath("~/ArchivosAuxiliares/ReportePendiente.pdf");
                UtileriasReportes.EnviaReporteComoPdf(this, File.ReadAllBytes(rutaCompleta), "Faltantes.pdf");
            }
            else
            {
                string nombreAttachment = string.Format("attachment; filename=\"{0}\"", "Faltantes.xlsx");
                Response.Clear();
                Response.ContentType = "binary/octet-stream";
                Response.AddHeader("content-disposition", nombreAttachment);
                Response.WriteFile(rutaCompleta);
            }
        }

        private void enviaPdfFaltantes()
        {
            string directorio = Configuracion.RutaParaAlmacenarArchivos;
            string rutaCompleta = string.Concat(directorio, Path.DirectorySeparatorChar, ReporteUID, ".pdf");

            //Si por algún motivo el reporte aun no está listo, envíamos un PDF que tiene la leyenda correspondiente
            if (!File.Exists(rutaCompleta))
            {
                rutaCompleta = Cultura == LanguageHelper.INGLES ? Server.MapPath("~/ArchivosAuxiliares/ReportePendiente.en-US.pdf") : Server.MapPath("~/ArchivosAuxiliares/ReportePendiente.pdf");
            }

            UtileriasReportes.EnviaReporteComoPdf(this, File.ReadAllBytes(rutaCompleta), "Faltantes.pdf");
        }
    }
}