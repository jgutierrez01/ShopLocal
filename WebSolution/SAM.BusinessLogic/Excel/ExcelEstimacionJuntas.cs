using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Produccion;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelEstimacionJuntas
    {
        private static readonly  object _mutex = new object();
        private static ExcelEstimacionJuntas _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private ExcelEstimacionJuntas()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase ExcelEstimacionJuntas
        /// </summary>
        public static ExcelEstimacionJuntas Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelEstimacionJuntas();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estimacionID"></param>
        /// <returns></returns>
        public byte[] ObtenerExcelPorEstimacionID(int estimacionID)
        {
            byte[] archivo;

            //Crear el ms sin parámetros para que pueda crecer
            using (MemoryStream ms = new MemoryStream())
            {
                using (SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgsEstJunta.Nombre_Hoja_Excel);
                    List<GrdEstimacionJunta> lista = EstimacionBO.Instance.ObtenerEstimacionJuntaPorEstimacionID(estimacionID);

                    uint renglon = 1;

                    Row fila = new Row();
                    fila.RowIndex = renglon;

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstJunta.NumControl, renglon, 1));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstJunta.NombreSpool, renglon, 2));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstJunta.Etiqueta, renglon, 3));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstJunta.Concepto, renglon, 4));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstJunta.TipoJunta, renglon, 5));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstJunta.Diametro, renglon, 6));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstJunta.Material, renglon, 7));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstJunta.Cedula, renglon, 8));

                    sheetData.AppendChild(fila);

                    foreach (GrdEstimacionJunta jta in lista)
                    {
                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.NumeroControl, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.NombreSpool, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Etiqueta, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Concepto, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.TipoJunta, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Diametro, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Material, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Cedula, renglon, columna++));

                        sheetData.AppendChild(fila);
                    }

                    xlsDoc.Close();
                }

                archivo = ms.ToArray();
                ms.Close();
            }

            return archivo;
        }
    }
}
