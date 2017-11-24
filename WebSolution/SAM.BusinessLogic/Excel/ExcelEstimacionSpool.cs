using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Grid;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelEstimacionSpool
    {
        private static readonly  object _mutex = new object();
        private static ExcelEstimacionSpool _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private ExcelEstimacionSpool()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase ExcelEstimacionJuntas
        /// </summary>
        public static ExcelEstimacionSpool Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelEstimacionSpool();
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
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgsEstSpool.Nombre_Hoja_Excel);
                    List<GrdEstimacionSpool> lista = EstimacionBO.Instance.ObtenerEstimacionSpoolPorEstimacionID(estimacionID);

                    uint renglon = 1;

                    Row fila = new Row();
                    fila.RowIndex = renglon;

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstSpool.Concepto, renglon, 1));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstSpool.Spool, renglon, 2));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstSpool.Pdis, renglon, 3));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstSpool.Material, renglon, 4));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstSpool.Cedula, renglon, 5));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsEstSpool.NumeroControl, renglon, 6));

                    sheetData.AppendChild(fila);

                    foreach (GrdEstimacionSpool jta in lista)
                    {
                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Concepto, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Spool, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Pdi, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Material, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Cedula, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.NumeroControl, renglon, columna++));

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
