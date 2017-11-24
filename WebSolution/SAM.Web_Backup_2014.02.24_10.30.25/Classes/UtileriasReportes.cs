using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAM.Entities.Reportes;
using Microsoft.Reporting.WebForms;
using System.IO;
using SAM.Common;
using Mimo.Framework.Common;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SAM.Web.ReportService2005;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Exceptions;
using System.Net;
using SAM.BusinessObjects.Produccion;

namespace SAM.Web.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public static class UtileriasReportes
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="faltantes"></param>
        /// <param name="condensado"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="pathReporte"></param>
        /// <param name="idioma"></param>
        /// <param name="fechaReporte"></param>
        /// <param name="horaReporte"></param>
        public static void GeneraPdfReporteFaltantes(List<FaltanteCruce> faltantes, List<CondensadoItemCode> condensado, Guid nombreArchivo, string pathReporte, string idioma, string fechaReporte, string horaReporte)
        {
            ReportViewer rptViewer = new ReportViewer();
            ReportDataSource rdsFlt = new ReportDataSource("Faltantes", faltantes);
            ReportDataSource rdsIc = new ReportDataSource("ItemCodes", condensado);

            rptViewer.Reset();
            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.LocalReport.ReportPath = pathReporte;
            rptViewer.LocalReport.DataSources.Clear();
            rptViewer.LocalReport.DataSources.Add(rdsFlt);
            rptViewer.LocalReport.DataSources.Add(rdsIc);
            rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("idioma", idioma));
            rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("fecha", fechaReporte));
            rptViewer.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("hora", horaReporte));
            rptViewer.LocalReport.Refresh();

            byte [] pdf = rptViewer.LocalReport.Render("PDF");

            string directorio = Configuracion.RutaParaAlmacenarArchivos;
            string rutaCompleta = string.Concat(directorio, Path.DirectorySeparatorChar, nombreArchivo, ".pdf");

            using (FileStream fs = new FileStream(rutaCompleta, FileMode.Create))
            {
                using (BinaryWriter wr = new BinaryWriter(fs))
                {
                    wr.Write(pdf);
                    wr.Flush();
                    wr.Close();
                }
                fs.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="reporte"></param>
        /// <param name="nombreReporte"></param>
        public static void EnviaReporteComoPdf(Page pagina, byte[] reporte, string nombreReporte)
        {
            ReportUtils.SendReportAsPDF(pagina.Response,reporte, nombreReporte); 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="caratula"></param>
        /// <param name="juntas"></param>
        /// <param name="cortes"></param>
        /// <param name="materiales"></param>
        /// <param name="resumenMateriales"></param>
        /// <param name="caratulaEstacion"></param>
        /// <param name="juntasDet"></param>
        /// <param name="juntasDetEstacion"></param>
        /// <param name="materialesDet"></param>
        /// <param name="materialesDetEstacion"></param>
        /// <param name="resumenMaterialesEstacion"></param>
        /// <returns></returns>
        public static byte[] ObtenReporteOdt(int ordenTrabajoID, bool caratula, bool juntas, bool cortes, bool materiales, bool resumenMateriales, bool caratulaEstacion, bool juntasDet, bool juntasDetEstacion, bool materialesDet, bool materialesDetEstacion, bool resumenMaterialesEstacion, bool caratulaDetallada, bool resumenMaterialesTaller, bool corteTaller, bool corteEstacion)
        {
            List<PdfWrapper> listaFinal = new List<PdfWrapper>(8);
            MyReportServerCredentials c = new MyReportServerCredentials(Config.UsernameReportingServices, Config.PasswordReportingServices, Config.DomainReportingServices);

            if (caratula)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_Caratula, TipoReporteProyectoEnum.CaratulaODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF"), OrientacionPagina.Horizontal));
            }
            if (juntas)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_Juntas, TipoReporteProyectoEnum.JuntasODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (materiales)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_Materiales, TipoReporteProyectoEnum.MaterialesODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (resumenMateriales)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_ResumenMateriales, TipoReporteProyectoEnum.ResumenMaterialesODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (cortes)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_Cortes, TipoReporteProyectoEnum.CortesODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (caratulaDetallada)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_CaratulaDetallada, TipoReporteProyectoEnum.CaratulaDetODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF"), OrientacionPagina.Horizontal));
            }
            if (juntasDet)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_JuntasDetalle, TipoReporteProyectoEnum.DetJuntasODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (materialesDet)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_MaterialesDetalle, TipoReporteProyectoEnum.DetMaterialesODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (caratulaEstacion)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_CaratulaEstacion, TipoReporteProyectoEnum.CaratulaPorEstacionODT);
            }
           
            if (juntasDetEstacion)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_JuntasDetalleEstacion, TipoReporteProyectoEnum.DetJuntasPorEstacionODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
           
            if (materialesDetEstacion)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_MaterialesDetalleEstacion, TipoReporteProyectoEnum.MaterialesPorEstacionODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (resumenMaterialesEstacion)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_ResumenMaterialesEstacion, TipoReporteProyectoEnum.ResumenMatPorEstacionODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (corteEstacion)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_CorteEstacion, TipoReporteProyectoEnum.CortesPorEstacionODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (resumenMaterialesTaller)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_ResumenMaterialesTaller, TipoReporteProyectoEnum.ResumenMatPorTallerODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }
            if (corteTaller)
            {
                ReportViewer rptViewer = configuraReporteOdt(ordenTrabajoID, c, WebConstants.ReportesAplicacion.Odt_CorteTaller, TipoReporteProyectoEnum.CortesPorTallerODT);
                listaFinal.Add(new PdfWrapper(rptViewer.ServerReport.Render("PDF")));
            }

            byte[] byteFinal = concatAndAddContent(listaFinal);

            return byteFinal;
        }

        /// <summary>
        /// Regresa el arreglo de bytes con el reporte para las etiquetas de material
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="recepcionID"></param>
        /// <returns></returns>
        public static byte[] ObtenEtiquetaMaterial(int proyectoID, int recepcionID)
        {
            int idioma = 1;
            if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
            {
                idioma = 2;
            }
            string rutaReporte = UtileriasReportes.DameRutaReporte(proyectoID, TipoReporteProyectoEnum.EtiquetaMaterial);

            MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices,
                                                                                   Config.PasswordReportingServices,
                                                                                   Config.DomainReportingServices);
            ReportViewer rptViewer = new ReportViewer();
            rptViewer.Reset();
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            rptViewer.ServerReport.ReportServerCredentials = credenciales;
            rptViewer.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
            rptViewer.ServerReport.ReportPath = rutaReporte;


            List<Microsoft.Reporting.WebForms.ReportParameter> lstParametros = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            lstParametros.Add(new Microsoft.Reporting.WebForms.ReportParameter("RecepcionID", recepcionID.ToString(), false));
            lstParametros.Add(new Microsoft.Reporting.WebForms.ReportParameter("Idioma", idioma.ToString(), false));
            
            rptViewer.ServerReport.SetParameters(lstParametros);
            rptViewer.ServerReport.Refresh();
           
            return rptViewer.ServerReport.Render("PDF");
        }

        /// <summary>
        /// Regresa el arreglo de bytes con el reporte para las etiquetas de embarque
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="recepcionID"></param>
        /// <returns></returns>
        public static byte[] ObtenEtiquetaEmbarque(string ids, string numeroEtiqueta, string numeroControl, string ordenTrabajo, int proyectoID)
        {
            try
            {
                string rutaReporte = UtileriasReportes.DameRutaReporte(proyectoID, TipoReporteProyectoEnum.EtiquetaEmbarque);

                MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices,
                                                                                       Config.PasswordReportingServices,
                                                                                       Config.DomainReportingServices);
                ReportViewer rptViewer = new ReportViewer();
                rptViewer.Reset();
                rptViewer.ProcessingMode = ProcessingMode.Remote;
                rptViewer.ServerReport.ReportServerCredentials = credenciales;
                rptViewer.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
                rptViewer.ServerReport.ReportPath = rutaReporte;


                List<Microsoft.Reporting.WebForms.ReportParameter> lstParametros = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                lstParametros.Add(new Microsoft.Reporting.WebForms.ReportParameter("IDs", ids, false));
                lstParametros.Add(new Microsoft.Reporting.WebForms.ReportParameter("NumeroEtiqueta", numeroEtiqueta, false));
                lstParametros.Add(new Microsoft.Reporting.WebForms.ReportParameter("NumeroControl", numeroControl, false));
                lstParametros.Add(new Microsoft.Reporting.WebForms.ReportParameter("OrdenTrabajo", ordenTrabajo, false));

                rptViewer.ServerReport.SetParameters(lstParametros);
                rptViewer.ServerReport.Refresh();

                return rptViewer.ServerReport.Render("PDF");
            }
            catch (ReportServerException rse)
            {
                if (rse.Message.Contains("cannot be found") || rse.Message.Contains("No se encuentra"))
                {
                    return null;
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
        /// <param name="ordenTrabajoID"></param>
        /// <param name="credenciales"></param>
        /// <param name="nombreReporte"></param>
        /// <returns></returns>
        private static ReportViewer configuraReporteOdt(int ordenTrabajoID, MyReportServerCredentials credenciales, string nombreReporte, TipoReporteProyectoEnum? tipoRep)
        {
            string nombreReportePersonalizado = String.Empty;

            if (tipoRep != null)
            {
                int proyectoID = OrdenTrabajoBO.Instance.Obtener(ordenTrabajoID).ProyectoID;

                ProyectoReporteCache personalizado =
                CacheCatalogos.Instance
                              .ObtenerProyectoReporte()
                              .Where(x => x.ProyectoID == proyectoID && x.TipoReporte == tipoRep)
                              .SingleOrDefault();

                if (personalizado != null)
                {
                    if (!personalizado.Nombre.StartsWith("/"))
                    {
                        nombreReportePersonalizado = "/" + personalizado.Nombre;
                    }
                    else
                    {
                        nombreReportePersonalizado = personalizado.Nombre;
                    }
                }
            }

            if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
            {
                nombreReporte += "-ingles";
            }

            ReportViewer rptViewer = new ReportViewer();
            rptViewer.Reset();
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            rptViewer.ServerReport.ReportServerCredentials = credenciales;
            rptViewer.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
            rptViewer.ServerReport.ReportPath = nombreReportePersonalizado != string.Empty ? nombreReportePersonalizado : string.Concat("/", Config.ReportingServicesDefaultFolder, "/", nombreReporte);
            rptViewer.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("OrdenTrabajoID", ordenTrabajoID.ToString()));
            rptViewer.ServerReport.Refresh();
            return rptViewer;
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdfWrapper"></param>
        /// <returns></returns>
        public static byte[] concatAndAddContent(List<PdfWrapper> pdfWrapper)
        {
            byte [] todos;

            using(MemoryStream ms = new MemoryStream())
            {
                int rotation = 0;
                Document doc = new Document();
                PdfImportedPage page;
                PdfReader reader;
                Rectangle rectangulo;

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.SetPageSize(PageSize.LETTER);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;

                foreach(PdfWrapper wrapper in pdfWrapper)
                {
                    reader = new PdfReader(wrapper.Contenido);
                    int pages = reader.NumberOfPages;

                    // loop over document pages
                    for (int i = 1; i <= pages; i++)
                    {
                        rectangulo = reader.GetPageSizeWithRotation(i);
                        doc.SetPageSize(rectangulo);
                        doc.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        rotation = reader.GetPageRotation(i);

                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, rectangulo.Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 0, 0);
                        }
                    }
                }

                doc.Close();
                todos = ms.ToArray();
                ms.Flush();
                ms.Dispose();
            }

            return todos;
        }

        /// <summary>
        /// Método que verifica si el reporte recibido existe fisicamente en reporting services
        /// </summary>
        /// <param name="nombreReporte"></param>
        /// <returns></returns>
        public static bool ReporteExiste(string nombreReporte)
        {
            bool existe = false;

            try
            {
                ReportingService2005 servicio = new ReportingService2005();
                servicio.Credentials = new NetworkCredential(Config.UsernameReportingServices, Config.PasswordReportingServices);
                servicio.Url = Config.UrlReportingServicesWebService;

                string[] ruta = nombreReporte.Split('/');

                string rutaFolder = string.Empty;
                for (int str = 0; str < ruta.Length - 1; str++)
                {
                    rutaFolder += ruta[str] + "/";
                }
                rutaFolder = rutaFolder.TrimEnd('/');
                CatalogItem[] reportes = servicio.ListChildren(rutaFolder, false);

                foreach (CatalogItem item in reportes)
                {
                    if (item.Name == ruta[ruta.Length - 1])
                    {
                        existe = true;
                        break;
                    }
                }

                return existe;

            }
            catch
            {
                return false;
            }            

        }


        /// <summary>
        /// Método que regresa la ruta configurada para el reporte en el proyecto en especifico
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <param name="tipoRep">Tipo de reporte a obtener</param>
        /// <returns></returns>
        public static string DameRutaReporte(int proyectoID, TipoReporteProyectoEnum tipoRep)
        {
            ProyectoReporteCache personalizado =
                CacheCatalogos.Instance
                              .ObtenerProyectoReporte()
                              .Where(x => x.ProyectoID == proyectoID && x.TipoReporte == tipoRep)
                              .SingleOrDefault();

            if (personalizado != null)
            {
                if (!personalizado.Nombre.StartsWith("/"))
                {
                    return "/" + personalizado.Nombre;
                }
                else
                {
                    return personalizado.Nombre;
                }
            }

            string reporteDefault = string.Empty;

            switch (tipoRep)
            {
                case TipoReporteProyectoEnum.Armado:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Armado;
                    break;
                case TipoReporteProyectoEnum.Soldadura:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Soldadura;
                    break;
                case TipoReporteProyectoEnum.Requisicion:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Requisicion;
                    break;
                case TipoReporteProyectoEnum.LiberacionDimensional:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_LiberacionDimensional;
                    break;
                case TipoReporteProyectoEnum.InspeccionVisual:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_InspeccionVisual;
                    break;
                case TipoReporteProyectoEnum.RT:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_RT;
                    break;
                case TipoReporteProyectoEnum.PMI:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_PMI;
                    break;
                case TipoReporteProyectoEnum.PT:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_PT;
                    break;
                case TipoReporteProyectoEnum.PWHT:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Pwht;
                    break;
                case TipoReporteProyectoEnum.Pintura:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Pintura;
                    break;
                case TipoReporteProyectoEnum.Durezas:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Durezas;
                    break;
                case TipoReporteProyectoEnum.Embarque:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Embarque;
                    break;
                case TipoReporteProyectoEnum.Espesores:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_Espesores;
                    break;
                case TipoReporteProyectoEnum.EtiquetaMaterial:
                    reporteDefault = WebConstants.ReportesAplicacion.Etiqueta_Material;
                    break;
                case TipoReporteProyectoEnum.EtiquetaEmbarque:
                    reporteDefault = WebConstants.ReportesAplicacion.Etiqueta_Embarque;
                    break;
                case TipoReporteProyectoEnum.RequisicionPintura:
                    reporteDefault = WebConstants.ReportesAplicacion.RequisicionPintura;
                    break;
                case TipoReporteProyectoEnum.RequisicionSpool:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_ReqSpool;
                    break;
                case TipoReporteProyectoEnum.Hidrostatica:
                    reporteDefault = WebConstants.ReportesAplicacion.Wkst_PNDSpool;
                    break;
            }

            if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
            {
                reporteDefault += "-ingles";
            }

            return string.Concat("/", Config.ReportingServicesDefaultFolder, "/", reporteDefault);
        }
    }
}