using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Common;
using Microsoft.Reporting.WebForms;
using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;

namespace SAM.Web
{
    public partial class VisorReporte : SamPaginaConSeguridad
    {
        protected override void OnLoad(EventArgs e)
        {
            RevisarSeguridad = false;
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    rptViewer.Visible = true;
                    //siempre debe venir
                    bool esReporteCustom = Convert.ToBoolean(Request.QueryString["EsCustom"]);
                    string rutaReporte = string.Empty;

                    if (!esReporteCustom)
                    {
                        int proyectoID = Request.QueryString["ProyectoID"].SafeIntParse();
                        TipoReporteProyectoEnum tipoRep = (TipoReporteProyectoEnum)Request.QueryString["Tipo"].SafeIntParse();
                        rutaReporte = UtileriasReportes.DameRutaReporte(proyectoID, tipoRep);
                    }
                    else
                    {
                        rutaReporte = Request.QueryString["NombreReporte"];
                    }

                    MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices, Config.PasswordReportingServices, Config.DomainReportingServices);

                    rptViewer.Reset();
                    rptViewer.ProcessingMode = ProcessingMode.Remote;
                    rptViewer.ServerReport.ReportServerCredentials = credenciales;
                    rptViewer.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
                    rptViewer.ServerReport.ReportPath = rutaReporte;

                    var qsKeys = from qs in Request.QueryString.AllKeys
                                 where !qs.EqualsIgnoreCase("EsCustom")
                                       && !qs.EqualsIgnoreCase("Tipo")
                                 select qs;

                    if (esReporteCustom)
                    {
                        qsKeys = qsKeys.Where(x => !x.EqualsIgnoreCase("NombreReporte"));
                    }

                    List<ReportParameter> lstParametros = new List<ReportParameter>();

                    foreach (string llaveQs in qsKeys)
                    {
                        lstParametros.Add(new ReportParameter(llaveQs, Request.QueryString[llaveQs], false));
                    }

                    rptViewer.ServerReport.SetParameters(lstParametros);
                    rptViewer.ServerReport.Refresh();
                }
                litReporteNoEncontrado.Visible = false;
            }
            catch (ReportServerException rse)
            {
                if (rse.Message.Contains("cannot be found") || rse.Message.Contains("No se encuentra"))
                {
                    litReporteNoEncontrado.Visible = true;
                    rptViewer.Visible = false;
                }
                else
                {
                    throw rse;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="tipoRep"></param>
        /// <returns></returns>
        //private string dameRutaReporte(int proyectoID, TipoReporteProyectoEnum tipoRep)
        //{
        //    ProyectoReporteCache personalizado =
        //        CacheCatalogos.Instance
        //                      .ObtenerProyectoReporte()
        //                      .Where(x => x.ProyectoID == proyectoID && x.TipoReporte == tipoRep)
        //                      .SingleOrDefault();

        //    if (personalizado != null)
        //    {
        //        if (!personalizado.Nombre.StartsWith("/"))
        //        {
        //            return "/" + personalizado.Nombre;
        //        }
        //        else
        //        {
        //            return personalizado.Nombre;
        //        }
        //    }

        //    string reporteDefault = string.Empty;

        //    switch (tipoRep)
        //    {
        //        case TipoReporteProyectoEnum.Armado:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_Armado;
        //            break;
        //        case TipoReporteProyectoEnum.Soldadura:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_Soldadura;
        //            break;
        //        case TipoReporteProyectoEnum.Requisicion:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_Requisicion;
        //            break;
        //        case TipoReporteProyectoEnum.LiberacionDimensional:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_LiberacionDimensional;
        //            break;
        //        case TipoReporteProyectoEnum.InspeccionVisual:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_InspeccionVisual;
        //            break;
        //        case TipoReporteProyectoEnum.RT:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_RT;
        //            break;
        //        case TipoReporteProyectoEnum.PT:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_PT;
        //            break;
        //        case TipoReporteProyectoEnum.PWHT:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_Pwht;
        //            break;
        //        case TipoReporteProyectoEnum.Pintura:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_Pintura;
        //            break;
        //        case TipoReporteProyectoEnum.Durezas:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_Durezas;
        //            break;
        //        case TipoReporteProyectoEnum.Embarque:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_Embarque;
        //            break;
        //        case TipoReporteProyectoEnum.Espesores:
        //            reporteDefault = WebConstants.ReportesAplicacion.Wkst_Espesores;
        //            break;
        //    }

        //    if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
        //    {
        //        reporteDefault += "-ingles";
        //    }

        //    return string.Concat("/", Config.ReportingServicesDefaultFolder, "/", reporteDefault);
        //}
    }
}