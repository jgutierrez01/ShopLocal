using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.IO;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Drawing;

namespace SAM.BusinessLogic.Excel
{
    public static class UtileriasExcel
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="nombreHoja"></param>
        /// <returns></returns>
        public static SheetData CreaDocumentoDefault(SpreadsheetDocument excel, string nombreHoja)
        {
            return CreaDocumentoDefault(excel, nombreHoja, 1);
        }

        public static SheetData CreaDocumentoDefault(SpreadsheetDocument excel, string nombreHoja, out Worksheet ws)
        {
            return CreaDocumentoDefault(excel, nombreHoja, 1, out ws);
        }

        public static SheetData CreaDocumentoDefault(SpreadsheetDocument excel, string nombreHoja, uint idHoja)
        {
            Worksheet ws = null;
            return CreaDocumentoDefault(excel, nombreHoja, idHoja, out ws);
        }

        public static SheetData CreaDocumentoDefault(SpreadsheetDocument excel, string nombreHoja, uint idHoja, out Worksheet ws)
        {
            //generar el objeto maestro
            WorkbookPart workbookPart = excel.AddWorkbookPart();

            //Generar los formatos de celdas particularmente usado para las fechas
            WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet styles = CustomStylesheet();
            styles.Save(stylesPart);

            //generar el contenido
            CreaContenidoDeWorkbookPart(workbookPart);
            AgregaHoja(workbookPart, nombreHoja, idHoja);

            //generar el area de trabajo para los worksheets      
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>("rId" + idHoja);
            return GeneraContenidoDelWorksheet(worksheetPart, out ws);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="workbookPart"></param>
        public static void CreaContenidoDeWorkbookPart(WorkbookPart workbookPart)
        {
            Workbook workbook = new Workbook();
            Sheets sheets = new Sheets();
            
            workbook.Append(sheets);
            workbookPart.Workbook = workbook;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="nombreHoja"></param>
        /// <param name="idHoja"></param>
        public static SheetData GeneraNuevaHoja(SpreadsheetDocument excel, string nombreHoja, uint idHoja, out Worksheet worksheet)
        {
            Sheet hoja = AgregaHoja(excel.WorkbookPart, nombreHoja, idHoja);

            //generar el area de trabajo para los worksheets      
            WorksheetPart worksheetPart = excel.WorkbookPart.AddNewPart<WorksheetPart>("rId" + idHoja);
            return GeneraContenidoDelWorksheet(worksheetPart, out worksheet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workbookPart"></param>
        /// <param name="nombreHoja"></param>
        /// <param name="idHoja"></param>
        /// <returns></returns>
        public static Sheet AgregaHoja(WorkbookPart workbookPart, string nombreHoja, uint idHoja)
        {
            Sheet sheet = new Sheet
            {
                Name = nombreHoja,
                SheetId = idHoja,
                Id = "rId" + idHoja
            };

            workbookPart.Workbook.Sheets.Append(sheet);

            return sheet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static SheetData GeneraContenidoDelWorksheet(WorksheetPart worksheetPart, out Worksheet worksheet)
        {
            worksheet = new Worksheet();
            SheetData sheetData = new SheetData();
            worksheet.Append(sheetData);
            worksheetPart.Worksheet = worksheet;
            return sheetData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="renglon"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        public static Cell CreaCeldaTexto(string valor, uint renglon, uint columna)
        {
            Cell c = new Cell();
            c.CellReference = _nombreColumnasExcel[columna - 1] + renglon;
            c.DataType = CellValues.String;


            CellValue v = new CellValue(valor ?? string.Empty);
            
            c.AppendChild(v);
            return c;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="renglon"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        public static Cell CreaCeldaNumeroDecimalDosDecimales(decimal? numero, uint renglon, uint columna)
        {
            Cell c = new Cell();
            c.CellReference = _nombreColumnasExcel[columna - 1] + renglon;

            CellValue v = new CellValue();

            if (numero.HasValue)
            {
                c.DataType = CellValues.Number;
                v.Text = numero.Value.ToString();
                c.StyleIndex = 2;
            }
            else
            {
                c.DataType = CellValues.String;
                v.Text = string.Empty;
            }

            c.AppendChild(v);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="renglon"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        public static Cell CreaCeldaNumeroDecimalCuatroDecimales(decimal? numero, uint renglon, uint columna)
        {
            Cell c = new Cell();
            c.CellReference = _nombreColumnasExcel[columna - 1] + renglon;

            CellValue v = new CellValue();

            if (numero.HasValue)
            {
                c.DataType = CellValues.Number;
                v.Text = numero.Value.ToString();
                c.StyleIndex = 4;
            }
            else
            {
                c.DataType = CellValues.String;
                v.Text = string.Empty;
            }

            c.AppendChild(v);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="?"></param>
        /// <param name="renglon"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        public static Cell CreaCeldaMoneda(decimal? numero, uint renglon, uint columna)
        {
            Cell c = new Cell();
            c.CellReference = _nombreColumnasExcel[columna - 1] + renglon;

            CellValue v = new CellValue();

            if (numero.HasValue)
            {
                c.DataType = CellValues.Number;
                v.Text = numero.Value.ToString();
                c.StyleIndex = 12;
            }
            else
            {
                c.DataType = CellValues.String;
                v.Text = string.Empty;
            }

            c.AppendChild(v);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="renglon"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        public static Cell CreaCeldaEntero(int? numero, uint renglon, uint columna)
        {
            Cell c = new Cell();
            c.CellReference = _nombreColumnasExcel[columna - 1] + renglon;
            c.DataType = CellValues.Number;

            CellValue v = new CellValue();

            if (numero.HasValue)
            {
                v.Text = numero.Value.ToString();
            }

            c.AppendChild(v);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="renglon"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        public static Cell CreaCeldaFecha(DateTime? fecha, uint renglon, uint columna)
        {
            Cell c = new Cell();
            c.CellReference = _nombreColumnasExcel[columna - 1] + renglon;
            CellValue v = new CellValue();

            if (fecha.HasValue)
            {
                v.Text = fecha.Value.ToOADate().ToString();
                c.StyleIndex = 1;
            }

            c.AppendChild(v);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valorTexto"></param>
        /// <param name="renglon"></param>
        /// <param name="columna"></param>
        /// <returns></returns>
        public static Cell CreaCeldaTextoInline(string valorTexto, uint renglon, uint columna)
        {
            Cell c = new Cell();

            c.DataType = CellValues.InlineString;
            c.CellReference = _nombreColumnasExcel[columna - 1] + renglon;

            Text texto = new Text(valorTexto);
            InlineString iString = new InlineString();
            iString.AppendChild(texto);
            c.AppendChild(iString);

            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageFullPath"></param>
        /// <param name="nombreImagen"></param>
        /// <param name="descripcionImagen"></param>
        /// <param name="xl"></param>
        /// <param name="ws"></param>
        public static void InsertaImagen(string imageFullPath, string nombreImagen, string descripcionImagen, SpreadsheetDocument xl, Worksheet ws)
        {
            WorksheetPart wsp = ws.WorksheetPart;
            DrawingsPart dp = wsp.AddNewPart<DrawingsPart>();
            ImagePart imgp = dp.AddImagePart(ImagePartType.Jpeg, wsp.GetIdOfPart(dp));

            using (FileStream fs = new FileStream(imageFullPath, FileMode.Open, FileAccess.Read))
            {
                imgp.FeedData(fs);
            }

            NonVisualDrawingProperties nvdp = new NonVisualDrawingProperties();
            nvdp.Id = 1025;
            nvdp.Name = nombreImagen;
            nvdp.Description = descripcionImagen;
            DocumentFormat.OpenXml.Drawing.PictureLocks picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks();
            picLocks.NoChangeAspect = true;
            picLocks.NoChangeArrowheads = true;
            NonVisualPictureDrawingProperties nvpdp = new NonVisualPictureDrawingProperties();
            nvpdp.PictureLocks = picLocks;
            NonVisualPictureProperties nvpp = new NonVisualPictureProperties();
            nvpp.NonVisualDrawingProperties = nvdp;
            nvpp.NonVisualPictureDrawingProperties = nvpdp;

            DocumentFormat.OpenXml.Drawing.Stretch stretch = new DocumentFormat.OpenXml.Drawing.Stretch();
            stretch.FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle();

            BlipFill blipFill = new BlipFill();
            DocumentFormat.OpenXml.Drawing.Blip blip = new DocumentFormat.OpenXml.Drawing.Blip();
            blip.Embed = dp.GetIdOfPart(imgp);
            blip.CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print;
            blipFill.Blip = blip;
            blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
            blipFill.Append(stretch);

            DocumentFormat.OpenXml.Drawing.Transform2D t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
            DocumentFormat.OpenXml.Drawing.Offset offset = new DocumentFormat.OpenXml.Drawing.Offset();
            offset.X = 10;
            offset.Y = 10;
            t2d.Offset = offset;
            Bitmap bm = new Bitmap(imageFullPath);
            //http://en.wikipedia.org/wiki/English_Metric_Unit#DrawingML
            //http://stackoverflow.com/questions/1341930/pixel-to-centimeter
            //http://stackoverflow.com/questions/139655/how-to-convert-pixels-to-points-px-to-pt-in-net-c
            DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();
            extents.Cx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
            extents.Cy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
            bm.Dispose();
            t2d.Extents = extents;
            ShapeProperties sp = new ShapeProperties();
            sp.BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto;
            sp.Transform2D = t2d;
            DocumentFormat.OpenXml.Drawing.PresetGeometry prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry();
            prstGeom.Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle;
            prstGeom.AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList();
            sp.Append(prstGeom);
            sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

            DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture();
            picture.NonVisualPictureProperties = nvpp;
            picture.BlipFill = blipFill;
            picture.ShapeProperties = sp;

            Position pos = new Position();
            pos.X = 0;
            pos.Y = 0;
            Extent ext = new Extent();
            ext.Cx = extents.Cx;
            ext.Cy = extents.Cy;
            AbsoluteAnchor anchor = new AbsoluteAnchor();
            anchor.Position = pos;
            anchor.Extent = ext;
            anchor.Append(picture);
            anchor.Append(new ClientData());
            WorksheetDrawing wsd = new WorksheetDrawing();
            wsd.Append(anchor);
            Drawing drawing = new Drawing();
            drawing.Id = dp.GetIdOfPart(imgp);

            wsd.Save(dp);
            ws.Append(drawing);
        }


        /// <summary>
        /// 
        /// </summary>
        private static readonly string[] _nombreColumnasExcel = new string[]
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
            "AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ",
            "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
            "CA","CB","CC","CD","CE","CF","CG","CH","CI","CJ","CK","CL","CM","CN","CO","CP","CQ","CR","CS","CT","CU","CV","CW","CX","CY","CZ",
            "DA","DB","DC","DD","DE","DF","DG","DH","DI","DJ","DK","DL","DM","DN","DO","DP","DQ","DR","DS","DT","DU","DV","DW","DX","DY","DZ",
            "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
            "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
            "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ",
            "HA","HB","HC","HD","HE","HF","HG","HH","HI","HJ","HK","HL","HM","HN","HO","HP","HQ","HR","HS","HT","HU","HV","HW","HX","HY","HZ",
            "IA","IB","IC","ID","IE","IF","IG","IH","II","IJ","IK","IL","IM","IN","IO","IP","IQ","IR","IS","IT","IU","IV","IW","IX","IY","IZ",
            "JA","JB","JC","JD","JE","JF","JG","JH","JI","JJ","JK","JL","JM","JN","JO","JP","JQ","JR","JS","JT","JU","JV","JW","JX","JY","JZ",
            "KA","KB","KC","KD","KE","KF","KG","KH","KI","KJ","KK","KL","KM","KN","KO","KP","KQ","KR","KS","KT","KU","KV","KW","KX","KY","KZ",
            "LA","LB","LC","LD","LE","LF","LG","LH","LI","LJ","LK","LL","LM","LN","LO","LP","LQ","LR","LS","LT","LU","LV","LW","LX","LY","LZ",
            "MA","MB","MC","MD","ME","MF","MG","MH","MI","MJ","MK","ML","MM","MN","MO","MP","MQ","MR","MS","MT","MU","MV","MW","MX","MY","MZ",
        };


        /// <summary>
        /// Metodo que crea los estilos para las celdas
        /// </summary>
        /// <returns></returns>
        public static Stylesheet CustomStylesheet()
        {
            Stylesheet stylesheet = new Stylesheet();
            Fonts fonts = new Fonts();
            DocumentFormat.OpenXml.Spreadsheet.Font font = new DocumentFormat.OpenXml.Spreadsheet.Font();
            FontName fontName = new FontName { Val = StringValue.FromString("Arial") };
            FontSize fontSize = new FontSize { Val = DoubleValue.FromDouble(11) };
            font.FontName = fontName;
            font.FontSize = fontSize;
            fonts.Append(font);
            //Font Index 1
            font = new DocumentFormat.OpenXml.Spreadsheet.Font();
            fontName = new FontName {Val = StringValue.FromString("Arial")};
            fontSize = new FontSize {Val = DoubleValue.FromDouble(12)};
            font.FontName = fontName;
            font.FontSize = fontSize;
            font.Bold = new Bold();
            fonts.Append(font);
            fonts.Count = UInt32Value.FromUInt32((uint)fonts.ChildElements.Count);
            Fills fills = new Fills();
            Fill fill = new Fill();
            PatternFill patternFill = new PatternFill { PatternType = PatternValues.None };
            fill.PatternFill = patternFill;
            fills.Append(fill);
            fill = new Fill();
            patternFill = new PatternFill {PatternType = PatternValues.Gray125};
            fill.PatternFill = patternFill;
            fills.Append(fill);
            //Fill index  2
            fill = new Fill();
            patternFill = new PatternFill {PatternType = PatternValues.Solid, 
                                           ForegroundColor = new ForegroundColor()};
            patternFill.ForegroundColor = 
               TranslateForeground(System.Drawing.Color.LightBlue);
            patternFill.BackgroundColor = 
                new BackgroundColor {Rgb = patternFill.ForegroundColor.Rgb};
            fill.PatternFill = patternFill;
            fills.Append(fill);
            //Fill index  3
            fill = new Fill();
            patternFill = new PatternFill {PatternType = PatternValues.Solid, 
                              ForegroundColor = new ForegroundColor()};
            patternFill.ForegroundColor = 
               TranslateForeground(System.Drawing.Color.DodgerBlue);
            patternFill.BackgroundColor = 
               new BackgroundColor {Rgb = patternFill.ForegroundColor.Rgb};
            fill.PatternFill = patternFill;
            fills.Append(fill);
            fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);
            var borders = new Borders();
            var border = new Border
                        {
                            LeftBorder = new LeftBorder(),
                            RightBorder = new RightBorder(),
                            TopBorder = new TopBorder(),
                            BottomBorder = new BottomBorder(),
                            DiagonalBorder = new DiagonalBorder()
                        };
            borders.Append(border);
            //All Boarder Index 1
            border = new Border
                         {
                             LeftBorder = new LeftBorder {Style = BorderStyleValues.Thin},
                             RightBorder = new RightBorder {Style = BorderStyleValues.Thin},
                             TopBorder = new TopBorder {Style = BorderStyleValues.Thin},
                             BottomBorder = new BottomBorder {Style = BorderStyleValues.Thin},
                             DiagonalBorder = new DiagonalBorder()
                         };
            borders.Append(border);
            //Top and Bottom Boarder Index 2
            border = new Border
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder (),
                TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
                BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
                DiagonalBorder = new DiagonalBorder()
            };
            borders.Append(border);
            borders.Count = UInt32Value.FromUInt32((uint)borders.ChildElements.Count);
            CellStyleFormats cellStyleFormats = new CellStyleFormats();
            CellFormat cellFormat = new CellFormat
            {
                NumberFormatId = 0, 
                                 FontId = 0, FillId = 0, BorderId = 0};
            cellStyleFormats.Append(cellFormat);
            cellStyleFormats.Count = 
               UInt32Value.FromUInt32((uint)cellStyleFormats.ChildElements.Count);
            uint iExcelIndex = 164;
            NumberingFormats numberingFormats = new NumberingFormats();
            CellFormats cellFormats = new CellFormats();
            cellFormat = new CellFormat {NumberFormatId = 0, FontId = 0, 
                             FillId = 0, BorderId = 0, FormatId = 0};
            cellFormats.Append(cellFormat);
            NumberingFormat nformatDateTime = new NumberingFormat
                     {
                         NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++),
                         FormatCode = StringValue.FromString("dd/mm/yyyy hh:mm:ss")
                     };
            numberingFormats.Append(nformatDateTime);
            NumberingFormat nformat4Decimal = new NumberingFormat
                     {
                         NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++),
                         FormatCode = StringValue.FromString("#,##0.0000")
                     };
            numberingFormats.Append(nformat4Decimal);
            NumberingFormat nformat2Decimal = new NumberingFormat
                      {
                          NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++),
                          FormatCode = StringValue.FromString("#,##0.00")
                      };
            numberingFormats.Append(nformat2Decimal);
            NumberingFormat nformatForcedText = new NumberingFormat
                       {
                           NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++),
                           FormatCode = StringValue.FromString("@")
                       };
            numberingFormats.Append(nformatForcedText);

            NumberingFormat nformatCurrency = new NumberingFormat
            {
                NumberFormatId = UInt32Value.FromUInt32(iExcelIndex),
                FormatCode = StringValue.FromString("\"$\"#,##0.00")
            };
            numberingFormats.Append(nformatCurrency);

            // index 1
            // Cell Standard Date format 
            cellFormat = new CellFormat
                 {
                     NumberFormatId = 14,
                     FontId = 0,
                     FillId = 0,
                     BorderId = 0,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);
            // Index 2
            // Cell Standard Number format with 2 decimal placing
            cellFormat = new CellFormat
                 {
                     NumberFormatId = 4,
                     FontId = 0,
                     FillId = 0,
                     BorderId = 0,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);
            // Index 3
            // Cell Date time custom format
            cellFormat = new CellFormat
                 {
                     NumberFormatId = nformatDateTime.NumberFormatId,
                     FontId = 0,
                     FillId = 0,
                     BorderId = 0,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);
            // Index 4
            // Cell 4 decimal custom format
            cellFormat = new CellFormat
                 {
                     NumberFormatId = nformat4Decimal.NumberFormatId,
                     FontId = 0,
                     FillId = 0,
                     BorderId = 0,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);
            // Index 5
            // Cell 2 decimal custom format
            cellFormat = new CellFormat
                 {
                     NumberFormatId = nformat2Decimal.NumberFormatId,
                     FontId = 0,
                     FillId = 0,
                     BorderId = 0,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);
            // Index 6
            // Cell forced number text custom format
            cellFormat = new CellFormat
                 {
                     NumberFormatId = nformatForcedText.NumberFormatId,
                     FontId = 0,
                     FillId = 0,
                     BorderId = 0,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);
            // Index 7
            // Cell text with font 12 
            cellFormat = new CellFormat
                 {
                     NumberFormatId = nformatForcedText.NumberFormatId,
                     FontId = 1,
                     FillId = 0,
                     BorderId = 0,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);
            // Index 8
            // Cell text
            cellFormat = new CellFormat
                 {
                     NumberFormatId = nformatForcedText.NumberFormatId,
                     FontId = 0,
                     FillId = 0,
                     BorderId = 1,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);
            // Index 9
            // Coloured 2 decimal cell text
            cellFormat = new CellFormat
                     {
                         NumberFormatId = nformat2Decimal.NumberFormatId,
                         FontId = 0,
                         FillId = 2,
                         BorderId = 2,
                         FormatId = 0,
                         ApplyNumberFormat = BooleanValue.FromBoolean(true)
                     };
            cellFormats.Append(cellFormat);
            // Index 10
            // Coloured cell text
            cellFormat = new CellFormat
                     {
                         NumberFormatId = nformatForcedText.NumberFormatId,
                         FontId = 0,
                         FillId = 2,
                         BorderId = 2,
                         FormatId = 0,
                         ApplyNumberFormat = BooleanValue.FromBoolean(true)
                     };
            cellFormats.Append(cellFormat);
            // Index 11
            // Coloured cell text
            cellFormat = new CellFormat
                 {
                     NumberFormatId = nformatForcedText.NumberFormatId,
                     FontId = 1,
                     FillId = 3,
                     BorderId = 2,
                     FormatId = 0,
                     ApplyNumberFormat = BooleanValue.FromBoolean(true)
                 };
            cellFormats.Append(cellFormat);

            //Index 12
            //Moneda
            cellFormat = new CellFormat
            {
                NumberFormatId = nformatCurrency.NumberFormatId, //currency
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0,
                ApplyNumberFormat = BooleanValue.FromBoolean(true)
            };

            cellFormats.Append(cellFormat);

            numberingFormats.Count = 
              UInt32Value.FromUInt32((uint)numberingFormats.ChildElements.Count);
            cellFormats.Count = UInt32Value.FromUInt32((uint)cellFormats.ChildElements.Count);
            stylesheet.Append(numberingFormats);
            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellStyleFormats);
            stylesheet.Append(cellFormats);
            CellStyles css = new CellStyles();
            CellStyle cs = new CellStyle
            {
                Name = StringValue.FromString("Normal"), 
                                    FormatId = 0, BuiltinId = 0};
            css.Append(cs);
            css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
            stylesheet.Append(css);
            DifferentialFormats dfs = new DifferentialFormats { Count = 0 };
            stylesheet.Append(dfs);
            TableStyles tss = new TableStyles
                  {
                      Count = 0,
                      DefaultTableStyle = StringValue.FromString("TableStyleMedium9"),
                      DefaultPivotStyle = StringValue.FromString("PivotStyleLight16")
                  };
            stylesheet.Append(tss);
            return stylesheet;
        }

        private static ForegroundColor TranslateForeground(System.Drawing.Color fillColor)
        {
           return new ForegroundColor
           {
               Rgb = new HexBinaryValue
                     {
                         Value =
                             System.Drawing.ColorTranslator.ToHtml(
                             System.Drawing.Color.FromArgb(
                                 fillColor.A,
                                 fillColor.R,
                                 fillColor.G,
                                 fillColor.B)).Replace("#", "")
                     }
           };
        }
    }
}
