using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelLstItemCode
    {
        private static readonly object _mutex = new object();
        private static ExcelLstItemCode _instance;

        //constructor privado para implementar patron singleton
        private ExcelLstItemCode()
        {
        }

        //obtiene la instancia de la clase
        public static ExcelLstItemCode Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelLstItemCode();
                    }
                }
                return _instance;
            }
        }

        public byte[] ObtenerExcelPorIDs(int proyectoId)
        {
            byte[] archivo;

            DataSet ds = ItemCodeBO.Instance.ObtenerLstMatItemCodePorProyecto(proyectoId);
            IEnumerable<DataRow> rows = ds.Tables["ItemCode"].Rows.Cast<DataRow>();

            using(MemoryStream ms = new MemoryStream())
            {
                using(SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgsLstItemCode.Nombre_Hoja_Excel);

                    uint z = 1;
                    uint renglon = 1;

                    Row fila = new Row();
                    fila.RowIndex = renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.ItemCode, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.Descripcion, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.Diametro1, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.Diametro2, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TipoMaterial, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalIngenieria, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.CantidadRecibida, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalEntradaOtrosProcesos, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalCondicionada, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalRechazada, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.CantidadDanada, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.RecibidoNeto, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.CantidadDespachadaEquivalente, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalSalidasTemporales, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalOtrasSalidas, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalMerma, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.CantidadOrdenTrabajo, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.CantidadEnPreparacionEquivalente, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.CantidadEnCorteEquivalente, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.InventarioTransferenciaCorte, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalCorteSinDespacho, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.CantidadDespachadaEquivalente2, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalDespachado, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalDespachadoParaICE, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalPorDespachar, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.InventarioFisico, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.CantidadCongeladaEquivalente, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.InventarioCongelado, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.InventarioDisponibleCruce, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.InventarioDisponibleCruceEquivalente, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstItemCode.TotalDisponibleCruce, renglon, z++));

                    sheetData.AppendChild(fila);
                    foreach(DataRow Ic in rows)
                    {
                        uint columna = 1;
                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(Ic.Field<string>("CodigoItemCode"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(Ic.Field<string>("DescripcionEspanol"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(Ic.Field<decimal>("Diametro1"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(Ic.Field<decimal>("Diametro2"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(Ic.Field<string>("TipoMaterialNombreEspañol"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadIngenieria"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadRecibida"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalEntradaOtrosProcesos"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalCondicionada"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalRechazada"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadDanada"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("RecibidoNeto"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadDespachadaEquivalente"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalSalidasTemporales"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalOtrasSalidas"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalMerma"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadOrdenTrabajo"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadEnPreparacionEquivalente"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadCortadaICE"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("InventarioTransferenciaCorte"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalCorteSinDespacho"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadDespachadaEquivalente"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalDespachado"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalDespachadoParaICE"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalPorDespachar"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("InventarioFisico"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("CantidadCongeladaEquivalente"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("InventarioCongelado"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("InventarioDisponibleCruce"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("InventarioDisponibleCruceEquivalente"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(Ic.Field<int>("TotalDisponibleCruce"), renglon, columna++));

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
