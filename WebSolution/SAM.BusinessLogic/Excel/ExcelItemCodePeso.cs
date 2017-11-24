using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Grid;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelItemCodePeso
    {
       private static readonly object _mutex = new object();
        private static ExcelItemCodePeso _instance;

        //constructor privado para implementar patron singleton
        private ExcelItemCodePeso()
        {
        }

        //obtiene la instancia de la clase
        public static ExcelItemCodePeso Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelItemCodePeso();
                    }
                }
                return _instance;
            }
        }

        public byte[] ObtenerExcelPorIDs(int proyectoId)
        {
            byte[] archivo;

            List<GrdItemCode> ds = ItemCodeBO.Instance.ObtenerListaPorProyecto(proyectoId).OrderBy(x => x.Codigo).ToList();
           
            using(MemoryStream ms = new MemoryStream())
            {
                using(SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgsItemCodePeso.Nombre_Hoja_Excel);

                    uint z = 1;
                    uint renglon = 1;

                    Row fila = new Row();
                    fila.RowIndex = renglon;

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsItemCodePeso.ItemCode, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsItemCodePeso.Peso, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsItemCodePeso.DescripcionInterna, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsItemCodePeso.Diametro1, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsItemCodePeso.Diametro2, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsItemCodePeso.FamiliaAcero, renglon, z++));
                    sheetData.AppendChild(fila);

                    foreach(GrdItemCode Ic in ds)
                    {
                        uint columna = 1;
                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(Ic.Codigo, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(Ic.Peso, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(Ic.DescripcionInterna, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(Ic.Diametro1, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(Ic.Diametro2, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(Ic.FamiliaAcero != null ? Ic.FamiliaAcero.Nombre : string.Empty, renglon, columna++));

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
