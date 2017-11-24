using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Reportes;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAM.Common;
using System.Web;
using System.Threading;
using SAM.Entities;
using SAM.BusinessObjects.Ingenieria;
using Mimo.Framework.Mail;
using SAM.BusinessLogic.Utilerias;
using log4net;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelPeqKgtEspNoEncontrados
    {
        private static readonly object _mutex = new object();
        private static ExcelPeqKgtEspNoEncontrados _instance;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ExcelPeqKgtEspNoEncontrados));
        //constructor privado para implementar patron singleton
        private ExcelPeqKgtEspNoEncontrados()
        {
        }

        //obtiene la instancia de la clase ExcelReporteFaltantes
        public static ExcelPeqKgtEspNoEncontrados Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelPeqKgtEspNoEncontrados();
                    }
                }
                return _instance;
            }
        }


        public void GeneraExcelPeqKgtEspNoEncontrados(List<JuntaSpool> peqNoEncontrados, List<JuntaSpool> kgtNoEncontrados, List<JuntaSpool> espNoEncontrados, Guid nombreArchivo, string fechaReporte, string horaReporte, string imgReporteFullPath, string idioma, int proyectoid,Guid usuarioID)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(idioma);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(idioma);
            #region Generar estructuras de datos necesarias

            var lstpeqNoEncontrados = JuntaSpoolBO.Instance.ObtenerPeqsNoEncontrados(peqNoEncontrados);
            var lstespNoEncontrados = JuntaSpoolBO.Instance.ObtenerEspNoencontrados(espNoEncontrados);
            var lstkgtNoEncontrados = JuntaSpoolBO.Instance.ObtenerKgtNoencontrados(kgtNoEncontrados);

            #endregion

            byte[] archivo = null;

            //Crear el ms sin parámetros para que pueda crecer
            using (MemoryStream ms = new MemoryStream())
            {
                using (SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    Worksheet ws = null;
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgPeqKgtEspNoEncontrados.SheetPeq, out ws);

                    Row fila = new Row();

                    #region Encabezado

                    fila = new Row();
                    fila.RowIndex = 2;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.infoDatosNoEncontrados, 2, 9));
                    sheetData.AppendChild(fila);

                    fila = new Row();
                    fila.RowIndex = 3;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(fechaReporte, 3, 9));
                    sheetData.AppendChild(fila);

                    //titulo del reporte de item codes
                    fila = new Row();
                    fila.RowIndex = 4;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.ResumenDatosNoEncontrados, 4, 5));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(horaReporte, 4, 9));
                    sheetData.AppendChild(fila);

                    #endregion

                    UtileriasExcel.InsertaImagen(imgReporteFullPath, "Sam", "Sam", xlsDoc, ws);

                    #region Encabezado columnas

                    uint renglon = 6;
                    uint idHoja = 2;
                    uint col = 1;

                    fila = new Row();
                    fila.RowIndex = renglon;

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.TipoJunta, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.FamiliaAcero, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.Cedula, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.Diametro, renglon, col++));


                    sheetData.AppendChild(fila);

                    #endregion

                    lstpeqNoEncontrados.ForEach(x =>
                    {
                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(x.TipoJunta, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(x.FamiliaAcero, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(x.Cedula, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(x.Diametro, renglon, columna++));

                        sheetData.AppendChild(fila);
                    });



                    SheetData hoja2 = UtileriasExcel.GeneraNuevaHoja(xlsDoc, MsgPeqKgtEspNoEncontrados.SheetEsp, idHoja++, out ws);
                    UtileriasExcel.InsertaImagen(imgReporteFullPath, "Sam", "Sam", xlsDoc, ws);

                    #region Encabezado

                    fila = new Row();
                    fila.RowIndex = 2;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.infoDatosNoEncontrados, 2, 9));
                    hoja2.AppendChild(fila);

                    fila = new Row();
                    fila.RowIndex = 3;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(fechaReporte, 3, 9));
                    hoja2.AppendChild(fila);

                    //titulo del reporte de item codes
                    fila = new Row();
                    fila.RowIndex = 4;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.ResumenDatosNoEncontrados, 4, 5));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(horaReporte, 4, 9));
                    hoja2.AppendChild(fila);

                    #endregion



                    #region Encabezado columnas
                    col = 1;
                    renglon = 6;
                    fila = new Row();
                    fila.RowIndex = renglon;

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.Cedula, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.Diametro, renglon, col++));
                    hoja2.AppendChild(fila);
                    #endregion

                    lstespNoEncontrados.ForEach(x =>
                    {
                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(x.Cedula, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(x.Diametro, renglon, columna++));

                        hoja2.AppendChild(fila);
                    });



                    SheetData hoja3 = UtileriasExcel.GeneraNuevaHoja(xlsDoc, MsgPeqKgtEspNoEncontrados.SheetKgt, idHoja++, out ws);
                    UtileriasExcel.InsertaImagen(imgReporteFullPath, "Sam", "Sam", xlsDoc, ws);

                    #region Encabezado

                    fila = new Row();
                    fila.RowIndex = 2;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.infoDatosNoEncontrados, 2, 9));
                    hoja3.AppendChild(fila);

                    fila = new Row();
                    fila.RowIndex = 3;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(fechaReporte, 3, 9));
                    hoja3.AppendChild(fila);

                    //titulo del reporte de item codes
                    fila = new Row();
                    fila.RowIndex = 4;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.ResumenDatosNoEncontrados, 4, 5));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(horaReporte, 4, 9));
                    hoja3.AppendChild(fila);
                    #endregion

                    #region Encabezado columnas
                    col = 1;
                    renglon = 6;
                    fila = new Row();
                    fila.RowIndex = renglon;

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.Cedula, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgPeqKgtEspNoEncontrados.Diametro, renglon, col++));
                    hoja3.AppendChild(fila);
                    #endregion


                    lstkgtNoEncontrados.ForEach(x =>
                    {
                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(x.Cedula, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(x.Diametro, renglon, columna++));

                        hoja3.AppendChild(fila);
                    });

                    xlsDoc.Close();
                }

                archivo = ms.ToArray();
                ms.Close();
            }

            if (archivo != null)
            {
                string directorio = Configuracion.RutaParaAlmacenarArchivos;
                string rutaCompleta = string.Concat(directorio, Path.DirectorySeparatorChar, nombreArchivo, ".xlsx");

                using (FileStream fs = new FileStream(rutaCompleta, FileMode.Create))
                {
                    using (BinaryWriter wr = new BinaryWriter(fs))
                    {
                        wr.Write(archivo);
                        wr.Flush();
                        wr.Close();
                    }
                    fs.Close();
                }


                _logger.DebugFormat("EnviaCorreoDatosNoEncontradosPeqKgtEsp");
                EnvioCorreos.Instance.EnviaCorreoDatosNoEncontradosPeqKgtEsp(usuarioID,Path.GetFullPath(rutaCompleta), proyectoid);

            }


        }







    }
}
