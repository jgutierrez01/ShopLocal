using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WebForms;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Web.Classes;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using System.Web.UI;
using SAM.Common;


namespace SAM.Web.Calidad
{
    public partial class ImpresionEmbarque : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            int proyectoID = Request.QueryString["ProyectoID"].SafeIntParse();

            string numeroEmbarque = Request.QueryString["NumeroEmbarque"].ToString();

            if (Request.QueryString["Escaneado"] != null && Request.QueryString["Escaneado"].SafeIntParse() == 1)
            {
                string nombreProyecto = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == proyectoID).Select(x => x.Nombre).SingleOrDefault();

                string directorio = Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes\Embarque" + @"\" + numeroEmbarque + ".pdf";

                if (System.IO.File.Exists(directorio))//----------------
                {
                    string nombreAttachment = string.Format("attachment; filename=\"{0}\"", numeroEmbarque + ".pdf");


                    Response.Clear();
                    Response.ContentType = "binary/octet-stream";
                    Response.AddHeader("content-disposition", nombreAttachment);
                    Response.WriteFile(directorio);
                }
                else
                {
                    ReporteEmbarque(proyectoID);
                }
            }
            else
                ReporteEmbarque(proyectoID);
        }

        public void ReporteEmbarque(int proyectoID)
        {
            string rutaEmbarque = UtileriasReportes.DameRutaReporte(proyectoID, TipoReporteProyectoEnum.Embarque);

            MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices,
                                                                                   Config.PasswordReportingServices,
                                                                                   Config.DomainReportingServices);
            ReportViewer rptViewer = new ReportViewer();
            rptViewer.Reset();
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            rptViewer.ServerReport.ReportServerCredentials = credenciales;
            rptViewer.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
            rptViewer.ServerReport.ReportPath = rutaEmbarque;

            var qsKeys = from qs in Request.QueryString.AllKeys
                         where !qs.EqualsIgnoreCase("EsCustom")
                               && !qs.EqualsIgnoreCase("Tipo")
                               && !qs.EqualsIgnoreCase("ID")
                         select qs;


            List<ReportParameter> lstParametros = new List<ReportParameter>();

            foreach (string llaveQs in qsKeys)
            {
                if(llaveQs != "Escaneado")
                    lstParametros.Add(new ReportParameter(llaveQs, Request.QueryString[llaveQs], false));
            }
            rptViewer.ServerReport.SetParameters(lstParametros);
            rptViewer.ServerReport.Refresh();
            UtileriasReportes.EnviaReporteComoPdf(this, rptViewer.ServerReport.Render("PDF"), "Embarque.pdf");
        }
    }
}