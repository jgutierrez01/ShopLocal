using System;
using SAM.BusinessLogic.Calidad;
using System.Linq;
using SAM.Web.Classes;
using SAM.Entities;
using Mimo.Framework.Extensions;
using Microsoft.Reporting.WebForms;
using Mimo.Framework.Common;
using System.Collections.Generic;
using SAM.BusinessObjects.Utilerias;
using SAM.Common;

namespace SAM.Web.Calidad
{
    public partial class ImpresionCaratula : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (EntityID.HasValue)
            {
                if (SeguridadQs.TieneAccesoASpool(EntityID.Value))
                {
                    int proyectoID = Request.QueryString["ProyectoID"].SafeIntParse();

                    if (Request.QueryString["Spool"] != null)
                    {

                        string spool = Request.QueryString["Spool"];

                        string nombreProyecto = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == proyectoID).Select(x => x.Nombre).SingleOrDefault();

                        string directorio = Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes\Trazabilidad" + @"\" + spool + ".pdf";

                        if (System.IO.File.Exists(directorio))//----------------
                        {
                            string nombreAttachment = string.Format("attachment; filename=\"{0}\"", spool + ".pdf");

                            Response.Clear();
                            Response.ContentType = "binary/octet-stream";
                            Response.AddHeader("content-disposition", nombreAttachment);
                            Response.WriteFile(directorio);
                        }
                        else
                        {
                            ReporteTrazabilidad(proyectoID);
                        }
                    }
                    else
                    {
                        ReporteTrazabilidad(proyectoID);
                    }
                }
            }
        }
        public void ReporteTrazabilidad(int proyectoID)
        {
            string rutaTrazabilidad = UtileriasReportes.DameRutaReporte(proyectoID, TipoReporteProyectoEnum.Trazabilidad);

            if (rutaTrazabilidad.ContainsIgnoreCase(Config.ReportingServicesDefaultFolder))
            {
                UtileriasReportes.EnviaReporteComoPdf(this, CertificacionBL.generaCaratula(EntityID.Value), "Caratula.pdf");
            }
            else
            {
                MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices,
                                                                           Config.PasswordReportingServices,
                                                                           Config.DomainReportingServices);

                ReportViewer rptViewer = new ReportViewer();
                rptViewer.Reset();
                rptViewer.ProcessingMode = ProcessingMode.Remote;
                rptViewer.ServerReport.ReportServerCredentials = credenciales;
                rptViewer.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
                rptViewer.ServerReport.ReportPath = rutaTrazabilidad;

                var qsKeys = from qs in Request.QueryString.AllKeys
                             where !qs.EqualsIgnoreCase("EsCustom")
                                   && !qs.EqualsIgnoreCase("Tipo")
                                   && !qs.EqualsIgnoreCase("ID")
                                   && !qs.EqualsIgnoreCase("Spool")
                             select qs;

                List<ReportParameter> lstParametros = new List<ReportParameter>();

                foreach (string llaveQs in qsKeys)
                {
                    lstParametros.Add(new ReportParameter(llaveQs, Request.QueryString[llaveQs], false));
                }
                rptViewer.ServerReport.SetParameters(lstParametros);
                rptViewer.ServerReport.Refresh();

                UtileriasReportes.EnviaReporteComoPdf(this, rptViewer.ServerReport.Render("PDF"),
                                                  "Caratula.pdf");
            }
        }
    }
}