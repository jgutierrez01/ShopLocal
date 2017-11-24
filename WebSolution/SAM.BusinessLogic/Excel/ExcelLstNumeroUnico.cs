using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Grid;
using System.Text.RegularExpressions;
using SAM.BusinessObjects.Proyectos;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelLstNumeroUnico
    {
        private static readonly object _mutex = new object();
        private static ExcelLstNumeroUnico _instance;

        //constructor privado para implementar patron singleton
        private ExcelLstNumeroUnico()
        {
        }

        //obtiene la instancia de la clase
        public static ExcelLstNumeroUnico Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelLstNumeroUnico();
                    }
                }
                return _instance;
            }
        }

        public byte[] ObtenerExcelPorIDs(int proyectoId, string colada, string itemCode, string numUnicoInicial, string numUnicoFinal)
        {
            byte[] archivo;
            List<GrdNumerosUnicosCompleto> numUnico;
            Dictionary<int, string> trans = CacheCatalogos.Instance.ObtenerTransportistas().ToDictionary(x => x.ID, y => y.Nombre);
            Proyecto proyecto = ProyectoBO.Instance.ObtenerConCamposRecepcion(proyectoId);

            using (SamContext ctx = new SamContext())
            {
                numUnico = NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyecto(proyectoId,
                                                                colada,
                                                                itemCode,
                                                                numUnicoInicial,
                                                                numUnicoFinal).ToList();
            }

            using(MemoryStream ms = new MemoryStream())
            {
                using(SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgsLstNumeroUnico.Nombre_Hoja_Excel);

                    uint z = 1;
                    uint renglon = 1;

                    Row fila = new Row();
                    fila.RowIndex = renglon;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.FechaRecepcion, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.NumeroUnico, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TipoMaterial, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.ItemCode, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Descripcion, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Diametro1, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Diametro2, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Rack, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Factura, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.PartidaFactura, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.OrdenCompra, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.PartidaOrden, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.NumeroColada, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Certificado, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.AceroNomenclatura, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Cedula, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Profile1, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Profile2, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Proveedor, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Transportista, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Fabricante, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalRecibida, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalOtrasEntradas, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalCondicionada, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalRechazada, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalDanada, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalRecibida, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalOtrasSalidas, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalOtrasSalidas2, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalMerma, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalEnTransferencia, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalCorteSinDespachada, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalDespachada, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalDespachada2, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.TotalInventarioActual, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.InventarioCongelado, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.MarcadoAsme, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.MarcadoGolpe, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.MarcadoPintura, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.PruebasHidrostaticas, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Estatus, renglon, z++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsLstNumeroUnico.Observaciones, renglon, z++));
                    #region Agregar campos personalizados
                    if (proyecto != null)
                    {
                        ProyectoCamposRecepcion campos = proyecto.ProyectoCamposRecepcion;

                        if (!string.IsNullOrEmpty(campos.CampoRecepcion1))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoRecepcion1, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoRecepcion2))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoRecepcion2, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoRecepcion3))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoRecepcion3, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoRecepcion4))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoRecepcion4, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoRecepcion5))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoRecepcion5, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoNumeroUnico1))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoNumeroUnico1, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoNumeroUnico2))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoNumeroUnico2, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoNumeroUnico3))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoNumeroUnico3, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoNumeroUnico4))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoNumeroUnico4, renglon, z++));

                        if (!string.IsNullOrEmpty(campos.CampoNumeroUnico5))
                            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(campos.CampoNumeroUnico5, renglon, z++));
                    }
                    #endregion

                    sheetData.AppendChild(fila);
                    foreach (GrdNumerosUnicosCompleto num in numUnico)
                    {
                        uint columna = 1;
                        fila = new Row();
                        fila.RowIndex = ++renglon;
                        
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.FechaRecepcion.ToShortDateString(), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.NumeroUnico, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.TipoMaterial, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.ItemCode, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Descripcion, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(num.Diametro1, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(num.Diametro2, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.RackDisplay, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Factura, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.PartidaFactura, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.OrdenCompra, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.PartidaOrden, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.NumeroColada, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Certificado, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto((num.AceroNomenclatura ?? string.Empty), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Cedula, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Profile1, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Profile2, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Proveedor, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Transportista, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Fabricante, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalRecibida, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalOtrasEntradas, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalCondicionada, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalRechazada, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalDanada, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalRecibidoNeto, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalOtrasSalidas, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalOtrasSalidas, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalMerma, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalEnTransferencia, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalCorteSinDespachada, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalDespachada, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalDespachadaICE, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.TotalInventarioActual, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero((int)num.InventarioCongelado, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(num.MarcadoAsme), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(num.MarcadoGolpe), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(num.MarcadoPintura), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.PruebasHidrostaticas, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Estatus, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.Observaciones, renglon, columna++));
                        #region Asignar campos personalizados
                        if (proyecto != null)
                        {
                            ProyectoCamposRecepcion campos = proyecto.ProyectoCamposRecepcion;

                            if (!string.IsNullOrEmpty(campos.CampoRecepcion1))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre1, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoRecepcion2))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre2, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoRecepcion3))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre3, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoRecepcion4))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre4, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoRecepcion5))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre5, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoNumeroUnico1))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre6, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoNumeroUnico2))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre7, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoNumeroUnico3))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre8, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoNumeroUnico4))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre9, renglon, columna++));

                            if (!string.IsNullOrEmpty(campos.CampoNumeroUnico5))
                                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(num.CampoLibre10, renglon, columna));
                        }
                        #endregion

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
