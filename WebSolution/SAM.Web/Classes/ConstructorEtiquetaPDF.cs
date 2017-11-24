using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using SAM.Entities;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Common;
using Resources;
using log4net;

namespace SAM.Web.Classes
{
    public static class ConstructorEtiquetaPDF
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ConstructorEtiquetaPDF));

        public static Font _font = FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));
        public static Font _font1 = FontFactory.GetFont(FontFactory.HELVETICA, 6, Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));
        public static Font _font2 = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));
        public static Font _font3 = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));
        public static Font _font4 = FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));

        /// <summary>
        /// Assumes path for images already exists
        /// </summary>
        /// <param name="operacion"></param>
        /// <param name="solicitud"></param>
        public static void CreatePDFForWeb(int recepcionID)
        {
            HttpContext context = HttpContext.Current;

            byte[] arr = CrearPDF(recepcionID);

            context.Response.Clear();
            context.Response.ContentType = "application/pdf";
            context.Response.BinaryWrite(arr);


        }

        public static void CreatePDFEmbarque(int[] workstatusSpoolIDs)
        {
            HttpContext context = HttpContext.Current;
            string nombreAttachment = string.Format("attachment; filename=\"{0}\"", LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Labels.pdf" : "Etiquetas.pdf");

            byte[] arr = CrearPDFEmbarque(workstatusSpoolIDs);

            context.Response.Clear();
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("content-disposition", nombreAttachment);
            context.Response.BinaryWrite(arr);
        }

        public static byte[] CrearPDF(int recepcionID)
        {
            byte[] array = null;
            HttpContext context = HttpContext.Current;

            using (MemoryStream memoria = new MemoryStream())
            {
                Document pdf = new Document(new Rectangle(288, 144), 10f, 0f, 0f, 0f);

                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdf, memoria);

                    pdf.Open();
                    PdfContentByte content = writer.DirectContent;

                    List<NumeroUnico> lstNumUnico = NumeroUnicoBO.Instance.ObtenerPorRecepcionIDEtiquetas(recepcionID);
                    string logoSteelgo = context.Server.MapPath(@"/Imagenes/Logos/sam_control_bw.jpg");
                    Image logo = Image.GetInstance(logoSteelgo);
                    logo.ScaleToFit(130f, 30f);

                    foreach (NumeroUnico numUnico in lstNumUnico)
                    {

                        PdfPTable tPrincipal = creaTabla(3);
                        tPrincipal.TotalWidth = 288f;
                        tPrincipal.LockedWidth = true;
                        tPrincipal.SetWidthPercentage(new[] { 8f, 252f, 28f }, new Rectangle(288, 144));

                        PdfPTable tVertical = creaTabla(1);
                        PdfPTable tVerticalProy = creaTabla(1);

                        // Tablas para 80% de tabla principal
                        PdfPTable tDerecha = creaTabla(1);                        
                        PdfPTable tPrimerRenglon = creaTabla(2);
                        PdfPTable tSegundoRenglon = creaTabla(3);
                        PdfPTable tTercerRenglon = creaTabla(3);
                        PdfPTable tCuartoRenglon = creaTabla(2);

                        #region Primer Renglon

                        Phrase pCodigo = new Phrase();
                        Chunk numUnicoT = new Chunk(MensajesAplicacion.Etiqueta_NumeroUnico + " ", _font);

                        Chunk numeroUnico = new Chunk(numUnico.Codigo, _font4);
                        pCodigo.Add(numUnicoT);
                        pCodigo.Add(numeroUnico);
                        PdfPCell cCodigo = new PdfPCell(pCodigo);
                        cCodigo.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cCodigo.Border = 0;

                        //Logo                        
                        PdfPCell cLogo = new PdfPCell(logo);
                        cLogo.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cLogo.Border = 0;                       

                        //Tabla para 20% 
                        Chunk materialID = new Chunk(MensajesAplicacion.Etiqueta_MaterialID + " ", _font1);
                        Chunk material_ID = new Chunk(numUnico.Codigo, _font);
                        Phrase pMaterial = new Phrase();
                        pMaterial.Add(materialID);
                        pMaterial.Add(material_ID);
                        PdfPCell cRotada = new PdfPCell(pMaterial);
                        cRotada.Border = 0;
                        cRotada.Padding = 0;                       
                        cRotada.Rotation = 90;
                        cRotada.HorizontalAlignment = Element.ALIGN_CENTER;
                        cRotada.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tVertical.AddCell(cRotada);

                        Chunk proyecto = new Chunk(numUnico.Proyecto.Nombre, _font);
                        Phrase pProyecto = new Phrase();
                        pProyecto.Add(proyecto);
                        PdfPCell cRotada1 = new PdfPCell(pProyecto);
                        cRotada1.Border = 0;
                        cRotada1.Padding = 0;
                        cRotada1.PaddingRight = 2f;
                        cRotada1.Rotation = 90;
                        cRotada1.HorizontalAlignment = Element.ALIGN_CENTER;
                        cRotada1.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tVerticalProy.AddCell(cRotada1);

                        tPrimerRenglon.SetWidthPercentage(new[] { 173f, 87f }, new Rectangle(260f, 10f));
                        tPrimerRenglon.AddCell(cCodigo);
                        tPrimerRenglon.AddCell(cLogo);

                        tDerecha.AddCell(tPrimerRenglon);
                        #endregion

                        #region Segundo Renglon
                        tSegundoRenglon.SetWidthPercentage(new[] { 130f, 65f, 65f }, new Rectangle(260f, 10f));
                        PdfPCell itemCode = creaCeldaDosTextos("Item Code", numUnico.ItemCodeID.HasValue ? numUnico.ItemCode.Codigo : string.Empty, _font, _font2);
                        PdfPCell diam1 = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Diametro1, numUnico.Diametro1.ToString(), _font, _font2);
                        PdfPCell diam2 = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Diametro2, numUnico.Diametro2.ToString(), _font, _font2);
                        tSegundoRenglon.AddCell(itemCode);
                        tSegundoRenglon.AddCell(diam1);
                        tSegundoRenglon.AddCell(diam2);

                        tDerecha.AddCell(tSegundoRenglon);
                        #endregion

                        #region Tercer Renglon
                        tTercerRenglon.SetWidthPercentage(new[] { 65f, 130f, 65f }, new Rectangle(260f, 10f));
                        PdfPCell colada = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Colada, numUnico.ColadaID.HasValue ? numUnico.Colada.NumeroColada : string.Empty, _font, _font2);
                        PdfPCell certificado = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Certificado, numUnico.ColadaID.HasValue ? numUnico.Colada.NumeroCertificado : string.Empty, _font, _font2);
                        PdfPCell cedula1  = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Cedula1, numUnico.Cedula, _font, _font2);
                        tTercerRenglon.AddCell(colada);
                        tTercerRenglon.AddCell(certificado);
                        tTercerRenglon.AddCell(cedula1);
                        
                        tDerecha.AddCell(tTercerRenglon);
                        #endregion

                        #region Cuarto Renglon
                        {
                            tCuartoRenglon.SetWidthPercentage(new[] { 40f, 220f }, new Rectangle(260f, 20f));
                            Chunk descripcionT = new Chunk(MensajesAplicacion.Etiqueta_Descripcion + " ", _font);
                            Chunk descripcion = new Chunk(numUnico.ItemCodeID.HasValue ? numUnico.ItemCode.DescripcionEspanol : string.Empty, _font2);
                            Phrase pDescripcionT = new Phrase();
                            Phrase pDescripcion = new Phrase();
                            pDescripcionT.Add(descripcionT);
                            pDescripcion.Add(descripcion);
                            PdfPCell cDescripcion = new PdfPCell(pDescripcionT);                            
                            PdfPCell dDescripcion = new PdfPCell(pDescripcion);
                            cDescripcion.Border = 0;
                            dDescripcion.Border = 0;
                            tCuartoRenglon.AddCell(cDescripcion);
                            tCuartoRenglon.AddCell(dDescripcion);
                            tDerecha.AddCell(tCuartoRenglon);
                        }

                        #endregion

                        #region Codigo Barras                       
                       
                        _logger.Debug("Iniciando codigo de barras");
                        Barcode128 bcode = new Barcode128();
                        bcode.CodeType = BarcodeEAN.EAN13;
                        _logger.Debug(string.Format("Codigo:{0}", numUnico.Codigo));
                        bcode.Code = numUnico.Codigo;
                        bcode.StartStopText = false;
                        bcode.GenerateChecksum = false;
                        bcode.Size = 1f;
                        bcode.BarHeight = 30f;
                        _logger.Debug("Creando Imagen");
                        iTextSharp.text.Image bcodeimg = bcode.CreateImageWithBarcode(content, null, null);
                        Phrase pbarcode = new Phrase(new Chunk(bcodeimg, 0, 0));
                        PdfPCell cBarcode = new PdfPCell(pbarcode);
                        cBarcode.Border = 0;
                        cBarcode.HorizontalAlignment = Element.ALIGN_CENTER;
                        cBarcode.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tDerecha.AddCell(cBarcode);
                        #endregion

                        tPrincipal.AddCell(tVerticalProy);
                        tPrincipal.AddCell(tDerecha);
                        tPrincipal.AddCell(tVertical);
                        pdf.Add(tPrincipal);
                    }


                    PdfGState estate = new PdfGState();
                    estate.FillOpacity = .3f;
                    estate.StrokeOpacity = .3f;
                    content.SaveState();
                    content.SetGState(estate);
                    content.RestoreState();
                }

                catch (DocumentException ex)
                {
                    throw (ex);
                }
                catch (Exception e)
                {
                    _logger.Error("Error al crear etiqueta de recepción", e);
                }
                finally
                {
                    pdf.Close();
                }

                array = memoria.ToArray();
            }

            return array;
        }

        public static byte[] CrearPDFEmbarque(int[] workstatusSpoolIDs)
        {
            byte[] array = null;
            HttpContext context = HttpContext.Current;

            using (MemoryStream memoria = new MemoryStream())
            {
                Document pdf = new Document(new Rectangle(288, 144), 10f, 0f, 0f, 0f);

                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdf, memoria);

                    pdf.Open();
                    PdfContentByte content = writer.DirectContent;

                    List<Spool> lstSpool = EmbarqueBO.Instance.ObtenSpoolsImpresion(workstatusSpoolIDs);
                    string logoSteelgo = context.Server.MapPath(@"/Imagenes/Logos/steelgo.jpg");
                    Image logo = Image.GetInstance(logoSteelgo);
                    logo.ScaleToFit(130f, 30f);

                    foreach (Spool spool in lstSpool)
                    {

                        PdfPTable tPrincipal = creaTabla(1);
                        tPrincipal.TotalWidth = 288f;
                        tPrincipal.LockedWidth = true;
                       
                        PdfPTable tDerecha = creaTabla(1);
                        PdfPTable tPrimerRenglon = creaTabla(2);
                        PdfPTable tSegundoRenglon = creaTabla(2);
                        PdfPTable tTercerRenglon = creaTabla(3);
                        PdfPTable tCuartoRenglon = creaTabla(1);

                        #region Primer Renglon

                        Phrase pNumeroControl = new Phrase();
                        Chunk numControlT = new Chunk(MensajesAplicacion.Etiqueta_NumeroControl + " ", _font);

                        Chunk numeroControl = new Chunk(spool.OrdenTrabajoSpool.Select(x => x.NumeroControl).FirstOrDefault(), _font3);
                        pNumeroControl.Add(numControlT);
                        pNumeroControl.Add(numeroControl);
                        PdfPCell cCodigo = new PdfPCell(pNumeroControl);
                        cCodigo.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cCodigo.Border = 0;

                        //Logo                        
                        PdfPCell cLogo = new PdfPCell(logo);
                        cLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cLogo.Border = 0;                       
                        
                        tPrimerRenglon.AddCell(cCodigo);
                        tPrimerRenglon.AddCell(cLogo);

                        tDerecha.AddCell(tPrimerRenglon);
                        #endregion

                        #region Segundo Renglon
                        tSegundoRenglon.SetWidthPercentage(new[] { 130f, 130f }, new Rectangle(260f, 10f));
                        PdfPCell isometrico = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Isometrico, spool.Dibujo, _font, _font2);
                        PdfPCell rev = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Revision, spool.Revision, _font, _font2);
                        tSegundoRenglon.AddCell(isometrico);
                        tSegundoRenglon.AddCell(rev);

                        tDerecha.AddCell(tSegundoRenglon);
                        #endregion

                        #region Tercer Renglon
                        tTercerRenglon.SetWidthPercentage(new[] { 65f, 65f, 130f }, new Rectangle(244f, 10f));
                        PdfPCell peso = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Peso, spool.Peso.ToString(), _font, _font2);
                        PdfPCell pdi = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_PDI, spool.Pdis.ToString(), _font, _font2);
                        PdfPCell spec = creaCeldaDosTextos(MensajesAplicacion.Etiqueta_Especificacion, spool.Especificacion, _font, _font2);
                        tTercerRenglon.AddCell(peso);
                        tTercerRenglon.AddCell(pdi);
                        tTercerRenglon.AddCell(spec);

                        tDerecha.AddCell(tTercerRenglon);
                        #endregion

                        #region Codigo Barras
                        Barcode39 bcode = new Barcode39();
                        bcode.CodeType = BarcodeEAN.EAN13;
                        bcode.Code = spool.Nombre;
                        bcode.StartStopText = false;
                        bcode.GenerateChecksum = false;
                        Image bcodeimg = bcode.CreateImageWithBarcode(content, null, null);
                        Phrase barcode = new Phrase(new Chunk(bcodeimg, 0, 0));
                        PdfPCell cBarcode = new PdfPCell(barcode);
                        cBarcode.Border = 0;
                        cBarcode.HorizontalAlignment = Element.ALIGN_CENTER;
                        cBarcode.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tDerecha.AddCell(cBarcode);
                        #endregion

                        #region Cuarto Renglon
                        {
                            Phrase nombre = new Phrase("www.steelgo.com", _font);
                            PdfPCell cWebPage = new PdfPCell(nombre);
                            cWebPage.HorizontalAlignment = 2; //derecha
                            cWebPage.PaddingRight = 10f;
                            cWebPage.Border = 0;
                            tCuartoRenglon.AddCell(cWebPage);

                            tDerecha.AddCell(tCuartoRenglon);
                        }
                        #endregion

                        tPrincipal.AddCell(tDerecha);

                        pdf.Add(tPrincipal);
                    }


                    PdfGState estate = new PdfGState();
                    estate.FillOpacity = .3f;
                    estate.StrokeOpacity = .3f;
                    content.SaveState();
                    content.SetGState(estate);
                    content.RestoreState();
                }

                catch (DocumentException ex)
                {
                    throw (ex);
                }
                catch (Exception)
                {
                }
                finally
                {
                    pdf.Close();
                }

                array = memoria.ToArray();
            }

            return array;
        }

        /// <summary>
        /// Crea una celda con dos textos con break entre ellos.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="texto"></param>
        /// <param name="font1"></param>
        /// <param name="font2"></param>
        /// <returns></returns>
        private static PdfPCell creaCeldaDosTextos(string titulo, string texto, Font font1, Font font2)
        {
            Chunk chk = new Chunk(titulo, font1);
            Chunk chk2 = new Chunk(texto, font2);
            Phrase p = new Phrase();
            p.Add(chk);
            p.Add("\n");
            p.Add(chk2);
            PdfPCell celda = new PdfPCell(p);
            celda.Border = 0;
            celda.PaddingBottom = 4;
            celda.VerticalAlignment = Element.ALIGN_MIDDLE;
            celda.HorizontalAlignment = Element.ALIGN_LEFT;

            return celda;
        }


        public static PdfPTable creaTabla(int columnas)
        {
            PdfPTable tabla = new PdfPTable(columnas);
            tabla.WidthPercentage = 100;
            tabla.DefaultCell.Border = 0;
            tabla.DefaultCell.Padding = 0;

            return tabla;
        }

    }
}