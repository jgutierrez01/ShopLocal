using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SAM.Entities.Personalizadas;
using System.Web;
using System.IO;
using System.Data;

namespace SAM.BusinessLogic.Calidad
{
    public class CaratulaBL
    {
        private static readonly object _mutex = new object();
        private static CaratulaBL _instance;
        public static Font _fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new iTextSharp.text.BaseColor(103, 99,99));
        public static Font _fontTituloRight = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, new iTextSharp.text.BaseColor(103, 99, 99));
        public static Font _fontTituloDetalle = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));
        public static Font _fontHeaderText = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));
        public static Font _fontHeaderText2 = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));
        public static Font _font = FontFactory.GetFont(FontFactory.HELVETICA, 5, Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));
        /// <summary>
        /// constructor del patrón singleton
        /// </summary>
        private CaratulaBL()
        {
        }

        /// <summary>
        /// obtiene la instancia para la clase.
        /// </summary>
        public static CaratulaBL Instance
        {
            get
            {
                lock(_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CaratulaBL();
                    }
                }
                return _instance;
            }            
        }

        /// <summary>
        /// regresa un PdfDocument para el reporte del Dossier de Calidad.
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="cj"></param>
        /// <param name="cs"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public byte[] RegresaPdf(CaratulaSpool cs, List<CaratulaJunta> cj, List<CaratulaColada> cc, HttpContext context)
        {
            byte[] array = null;
            
            string imgSam = context.Server.MapPath(@"/Imagenes/Logos/logo.png");

            using (MemoryStream memory = new MemoryStream())
            {
                Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 5, 5, 5, 5);
                
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, memory);

                    doc.Open();
                    doc.NewPage();

                    PdfContentByte cb = writer.DirectContent;
                    Image image = Image.GetInstance(imgSam);
                    image.ScalePercent(50f);
                    image.SetAbsolutePosition(50f, 750f);
                    doc.Add(image);

                    PdfPTable tabla = new PdfPTable(1);
                    tabla.WidthPercentage = 100;
                    tabla.TotalWidth = 800;
                    tabla.LockedWidth = true;
                    tabla.SplitLate = false;
                    tabla.SplitRows = true;
                    tabla.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    tabla.DefaultCell.BorderWidth = 0;
                    tabla.DefaultCell.Padding = 0;
                    tabla.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    tabla.AddCell(GeneraHeader(cs));
                    AgregarRenglonVacio(tabla, 5);
                    tabla.AddCell(GeneraTablaSpool(cs));
                    AgregarRenglonVacio(tabla, 5);
                    tabla.AddCell(GeneraTablaJuntas(cj));
                    AgregarRenglonVacio(tabla, 5);
                    tabla.AddCell(GeneraTablaColadas(cc));
                    AgregarRenglonVacio(tabla, 5);

                    doc.Add(tabla);
                    
                }
                catch(DocumentException ex)
                {
                    throw (ex);
                }
                finally
                {
                    doc.Close();
                }

                array = memory.ToArray();
                return array;
            }

            //PdfDocument pdf = null;
            //return pdf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablaPDF"></param>
        /// <param name="padding"></param>
        private void AgregarRenglonVacio(PdfPTable tablaPDF, float padding)
        {
            for (int i = 0; i < tablaPDF.NumberOfColumns; i++)
            {
                PdfPCell celda = new PdfPCell();

                celda.Border = 0;
                celda.Padding = padding;

                tablaPDF.AddCell(celda);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        private PdfPCell GeneraHeader(CaratulaSpool cs)
        {
            PdfPCell header = new PdfPCell();
            
            //espacio vacio
            PdfPTable tabla = new PdfPTable(3);
            //tablaColadas.DefaultCell.BackgroundColor = new Color(255, 255, 255);
            tabla.SetWidths(new[] { .33f, .34f, .33f });
            tabla.TotalWidth = 750;
            tabla.DefaultCell.BorderWidth = 0;
            tabla.DefaultCell.Padding = 0;
            tabla.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            Phrase text = new Phrase("");
            tabla.AddCell(text);

            text = new Phrase(String.Format("REPORTE DE CERTIFICACIÓN DE SPOOL\n(Spool Certification Report)\n" + cs.NombreSpool), _fontTitulo);
            tabla.AddCell(text);

            text = new Phrase(String.Format("No. de Control\n(Control Number)\n" + cs.NumeroControl + "\n" + cs.Fecha.ToShortDateString()), _fontTituloRight);
            tabla.AddCell(text);

            header.AddElement(tabla);
            return header;
        }

        /// <summary>
        /// recibe un objeto del tipo CaratulaSpool con el cuál crea la información de la tabla de Detalle Spool
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        private PdfPTable GeneraTablaSpool(CaratulaSpool cs)
        {
            PdfPTable tabla = new PdfPTable(1);
            tabla.WidthPercentage = 100; /***/
            tabla.TotalWidth = 750; /***/
            tabla.DefaultCell.BorderWidth = 0;
            tabla.DefaultCell.Padding = 0;
            tabla.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabla.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

            //  1er renglon
            Phrase text = new Phrase("DETALLE DE SPOOL (Spool Detail)", _fontTituloDetalle);
            tabla.AddCell(text);

            //  2do renglon
            PdfPTable tablaUtil = new PdfPTable(2);
            tablaUtil.SetWidths(new[] { .50f, .50f });
            tablaUtil.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablaUtil.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            tablaUtil.DefaultCell.BorderWidth = 0;
            tablaUtil.DefaultCell.Padding = 0;
            text = new Phrase("");
            tablaUtil.AddCell(text);
           
            //*** y lo agrega :) **/
            tabla.AddCell(tablaUtil);

            // y la info
            tablaUtil = new PdfPTable(9);
            tablaUtil.SetWidths(new[] { .11f, .11f, .12f, .11f, .11f, .11f, .11f, .11f, .11f });
            tabla.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabla.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            tablaUtil.DefaultCell.BorderWidth = 0;
            tablaUtil.DefaultCell.Padding = 0;

            text = new Phrase("Detalle de Recubrimiento (Coating)", _fontHeaderText);
            PdfPCell xCell = new PdfPCell(text);
            xCell.Colspan = 9;
            xCell.BorderWidth = 0;
            xCell.BorderColor = new BaseColor(255, 255, 255);
            tablaUtil.AddCell(xCell);
            // HEADER TEXT
            text = new Phrase("ISOMETRICO\n(Isometric)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("REVISION\n(Rev.)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("LIBERACION\nDIMENSIONAL\n(Dimensional)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("ESPESORES\n(Thickness)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("PRIMARIO\n(Primer)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("ENLACE\n(Link)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("ACABADO\n(Final Coat)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("ADHERENCIA", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("PULL OFF", _fontHeaderText);
            tablaUtil.AddCell(text);
            // DATA
            text = new Phrase(cs.Dibujo, _font);
            tablaUtil.AddCell(text);
            text = new Phrase(cs.Revision, _font);
            tablaUtil.AddCell(text);
            text = new Phrase(cs.NumReporteDimensional, _font);
            tablaUtil.AddCell(text);
            text = new Phrase(cs.NumReporteEspesores, _font);
            tablaUtil.AddCell(text);
            text = new Phrase(cs.Primario, _font);
            tablaUtil.AddCell(text);
            text = new Phrase(cs.Enlace, _font);
            tablaUtil.AddCell(text);
            text = new Phrase(cs.Acabado, _font);
            tablaUtil.AddCell(text);
            text = new Phrase(cs.Adeherencias, _font);
            tablaUtil.AddCell(text);
            text = new Phrase(cs.PullOff, _font);
            tablaUtil.AddCell(text);
            //*** y lo agrega :) **/
            tabla.AddCell(tablaUtil);

            return tabla;
        }

        /// <summary>
        /// recibe un listado de CaratulaJuntas con la cuál genera el reporte.
        /// </summary>
        /// <param name="cj"></param>
        /// <returns></returns>
        private PdfPTable GeneraTablaJuntas(List<CaratulaJunta> cj)
        {
            PdfPTable tabla = new PdfPTable(1);
            tabla.WidthPercentage = 100; /***/
            tabla.TotalWidth = 750; /***/
            tabla.DefaultCell.BorderWidth = 0;
            tabla.DefaultCell.Padding = 0;
            tabla.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabla.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            
            //  1er renglon
           
            PdfPTable tablaUtil = new PdfPTable(17);
            tablaUtil.SetWidths(new[] { .04389f, .04389f, .04389f, .04389f, .05889f, .05889f, .07889f, .07889f, 
                                        .07889f, .05889f, .05889f, .05889f, .05889f, .05889f, .05889f, .05889f, .05889f, });
            tablaUtil.DefaultCell.BorderWidth = 0;
            tablaUtil.DefaultCell.Padding = 2;
            tablaUtil.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablaUtil.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Phrase text = new Phrase("DETALLE DE JUNTAS (Weld Detail)", _fontTituloDetalle);
            PdfPCell xCell = new PdfPCell(text);
            xCell.Colspan = 17;
            xCell.BorderWidth = 0;
            xCell.BorderColor = new BaseColor(255,255,255);
            tablaUtil.AddCell(xCell);
            //  HEADER TEXT
            text = new Phrase("JUNTA\n(Weld)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("TIPO\n(Type)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("DIAM.\n(M.B)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("CED.\n(Sch.)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("ARMADO\n(Fitup)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("SOLDADURA\n(Welding)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("WPS", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("SOLDADOR\nRAIZ\n(Root Welder)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("SOLDADOR\nRELLENO\n(Filler Welder)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("INSP.VISUAL\n(Visual Insp.)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("COLADA\nMATL.BASE1\n(Base Heat no.1)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("COLADA\nMATL.BASE2\n(Base Heat no. 2)", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("RT", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("PT", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("PWHT", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("Post-RT", _fontHeaderText2);
            tablaUtil.AddCell(text);
            text = new Phrase("DUREZAS\n(Hardness Test)", _fontHeaderText2);
            tablaUtil.AddCell(text);


            tablaUtil.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //   INFO
            foreach(CaratulaJunta c in cj)
            {
                text = new Phrase(c.Etiqueta, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.TipoJunta, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Diametro.ToString(), _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Cedula, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Armado.HasValue ? c.Armado.Value.ToShortDateString(): string.Empty, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Soldadura.HasValue ? c.Soldadura.Value.ToShortDateString() : string.Empty, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Wps, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.SoldadorRaiz, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.SoldadorRelleno, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.InspeccionVisual, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.MaterialBase1, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.MaterialBase2, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Rt, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Pt, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.PWHT, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.PostRT, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Durezas, _font); 
                tablaUtil.AddCell(text);                
            }


            //**  Y LO AGREGA :) **/
            tabla.AddCell(tablaUtil);

            return tabla;
        }

        /// <summary>
        ///  recibe un listado de CaratulaColadas con la cuál genera el reporte.
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        private PdfPTable GeneraTablaColadas(List<CaratulaColada> cc)
        {
            PdfPTable tabla = new PdfPTable(1);
            tabla.WidthPercentage = 100; /***/
            tabla.TotalWidth = 750; /***/
            tabla.DefaultCell.BorderWidth = 0;
            tabla.DefaultCell.Padding = 0;
            tabla.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tabla.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
           
        //  1er renglon
            
            //  HEADER TEXT
            PdfPTable tablaUtil = new PdfPTable(4);
            tablaUtil.SetWidths(new[] { .165f, .165f, .165f, .50f });
            tablaUtil.DefaultCell.BorderWidth = 0;
            tablaUtil.DefaultCell.Padding = 3;
            tablaUtil.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablaUtil.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            Phrase text = new Phrase("COLADAS Y MATERIAL BASE(Heat Number and bae material)", _fontTituloDetalle);
            PdfPCell xCell = new PdfPCell(text);
            xCell.Colspan = 4;
            xCell.BorderWidth = 0;
            xCell.BorderColor = new BaseColor(255, 255, 255);
            tablaUtil.AddCell(xCell);
            //  HEADER TEXT
            text = new Phrase("COLADA\n(Heat Number)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("CERTIFICADO\n(MTR)", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("ITEM CODE", _fontHeaderText);
            tablaUtil.AddCell(text);
            text = new Phrase("DESCRIPCION.\n(Description)", _fontHeaderText);
            tablaUtil.AddCell(text);


            tablaUtil.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //   INFO
            foreach (CaratulaColada c in cc)
            {
                text = new Phrase(c.Colada, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Certificado, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.ItemCode, _font);
                tablaUtil.AddCell(text);
                text = new Phrase(c.Descripcion, _font);
                tablaUtil.AddCell(text);
            }


            //**  Y LO AGREGA :) **/
            tabla.AddCell(tablaUtil);

            return tabla;
        }
    }
}
