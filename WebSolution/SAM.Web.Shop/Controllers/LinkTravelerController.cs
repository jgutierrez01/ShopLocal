using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SAM.BusinessObjects.LinkTraveler;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.LinkTraveler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace SAM.Web.Shop.Controllers
{
    public class LinkTravelerController : Controller
    {
        // GET: LinkTraveler
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet, ValidateInput(false)]      
        public FileResult ObtenerPDFTraveler(string NumeroControl, int ProyectoID)
        {
            DataTable dtSpools = new DataTable();            
            dtSpools = LinkTravelerBO.Instance.ObtieneSpools(NumeroControl, ProyectoID);
            ProyectoModel Proyecto = LinkTravelerBO.Instance.GetInfoProyecto(ProyectoID);                        
            if(dtSpools.Rows.Count > 0)
            {
                if ((LinkTravelerBO.Instance.ExisteNumeroOrdenPath(Proyecto.pathPDF, dtSpools.Rows[0][0].ToString())))
                {                    
                    //Existe el pdf en el servidor se agrega logica de negocio para retornar el traveler
                    string ruta = Path.Combine(Proyecto.pathPDF, "ODT " + dtSpools.Rows[0][0].ToString() + ".pdf");
                    int numeroPaginasPDF = SplitPDF.Instance.CantidadDePaginas(ruta) - int.Parse(dtSpools.Rows[0][2].ToString());

                    if(numeroPaginasPDF > 0)
                    {
                        Document doc = new Document();
                        string nombreTemporal = "\\ODT_" + NumeroControl + ".pdf";
                        MemoryStream stream = new MemoryStream();
                        Document document = new Document();
                        PdfCopy writer = new PdfCopy(document, stream);
                        writer.CloseStream = false;
                        PdfImportedPage page = null;
                        document.Open();
                        PdfReader reader = new PdfReader(ruta);
                        page = writer.GetImportedPage(reader, numeroPaginasPDF);
                        writer.AddPage(page);
                        document.Close();
                        reader.Close();

                        Response.Clear();
                        //Response.AddHeader("Content-Disposition", "attachment;filename=ODT_" + NumeroControl + ".pdf");
                        Response.AddHeader("Pragma", "no-cache");
                        Response.ContentType = "application/force-download";
                        Response.BinaryWrite(stream.ToArray());
                        return File(stream.ToArray(), "application/pdf");
                    }
                    else
                    {
                        MemoryStream workStream = new MemoryStream();
                        Document document = new Document();
                        PdfWriter.GetInstance(document, workStream).CloseStream = false;
                        document.Open();
                        document.Add(new Paragraph("El Archivo PDF De Origen Está Incompleto Ó No Coincide Con El Número De Páginas."));
                        document.Close();
                        byte[] byteInfo = workStream.ToArray();
                        workStream.Write(byteInfo, 0, byteInfo.Length);
                        workStream.Position = 0;
                        return new FileStreamResult(workStream, "application/pdf");
                    }                                                        
                }
                else
                {
                    MemoryStream workStream = new MemoryStream();
                    Document document = new Document();
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;
                    document.Open();
                    document.Add(new Paragraph("No se Encontro Traveler"));
                    document.Close();
                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;
                    return new FileStreamResult(workStream, "application/pdf");
                }                                  
            }
            else
            {
                MemoryStream workStream = new MemoryStream();
                Document document = new Document();
                PdfWriter.GetInstance(document, workStream).CloseStream = false;
                document.Open();
                document.Add(new Paragraph("No Se Encontró Spool"));                
                document.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                return new FileStreamResult(workStream, "application/pdf");
            }                                             
        }
        
        [HttpGet]
        public JsonResult ValidaGranel(string NumeroControl, int ProyectoID)
        {
            string Respuesta = LinkTravelerBO.Instance.ValidarGranel(NumeroControl, ProyectoID);
            var myData = new[] { new { result = Respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

    }
}