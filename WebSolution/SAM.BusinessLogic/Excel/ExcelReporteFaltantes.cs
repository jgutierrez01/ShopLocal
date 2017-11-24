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

namespace SAM.BusinessLogic.Excel
{
    public class ExcelReporteFaltantes
    {
        private static readonly object _mutex = new object();
        private static ExcelReporteFaltantes _instance;

        //constructor privado para implementar patron singleton
        private ExcelReporteFaltantes()
        {
        }

        //obtiene la instancia de la clase ExcelReporteFaltantes
        public static ExcelReporteFaltantes Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelReporteFaltantes();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="faltantes"></param>
        /// <param name="condensado"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="fechaReporte"></param>
        /// <param name="horaReporte"></param>
        /// <param name="imgReporteFullPath"></param>
        public void GeneraExcelReporteFaltantes(List<FaltanteCruce> faltantes, List<CondensadoItemCode> condensado, Guid nombreArchivo, string fechaReporte, string horaReporte, string imgReporteFullPath, string idioma)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(idioma);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(idioma);
            #region Generar estructuras de datos necesarias

            var lstIcIntegrados = (from icInt in condensado
                                   group icInt by new { icInt.ItemCodeID, icInt.D1, icInt.D2, icInt.DescripcionItemCode, icInt.CodigoItemCode, icInt.DisponibleCruceOriginal, icInt.CantidadRecibidaOriginal, icInt.CantidadRechazadosOriginal, icInt.CantidadCondicionadosOriginal, icInt.DisponiblePorEquivalencia, icInt.RequeridaParaFabricacion, icInt.CongeladoEnEsteCruceOriginal, icInt.CongeladoEnEsteCruceEquivalencia, icInt.RequeridaTotalIngenieria }
                                       into icGrp
                                       orderby icGrp.Key.CodigoItemCode, icGrp.Key.D1
                                       select new
                                       {
                                           ItemCodeID = icGrp.Key.ItemCodeID,
                                           CodigoItemCode = icGrp.Key.CodigoItemCode,
                                           DescripcionItemCode = icGrp.Key.DescripcionItemCode,
                                           D1 = icGrp.Key.D1,
                                           D2 = icGrp.Key.D2,
                                           DisponibleCruce = icGrp.Key.DisponibleCruceOriginal,
                                           CantidadRecibidaOriginal = icGrp.Key.CantidadRecibidaOriginal,
                                           CantidadRechazadaOriginal = icGrp.Key.CantidadRechazadosOriginal,
                                           CantidadCondicionadosOriginal = icGrp.Key.CantidadCondicionadosOriginal,
                                           DisponibleEquivalentes = icGrp.Key.DisponiblePorEquivalencia,
                                           RequeridaFabricacion = icGrp.Key.RequeridaParaFabricacion,
                                           CongeladoCruceOriginal = icGrp.Key.CongeladoEnEsteCruceOriginal,
                                           CongeladoCruceEquivalencia = icGrp.Key.CongeladoEnEsteCruceEquivalencia,
                                           TotalFaltante = icGrp.Sum(x => x.SumaPrioridad),
                                           RequeridaTotalIng = icGrp.Key.RequeridaTotalIngenieria
                                       }).ToList();

            List<int> lstPrioridad = (from icInt in condensado
                                      where icInt.Prioridad > 0
                                      select icInt.Prioridad)
                                      .Distinct()
                                      .OrderBy(x => x)
                                      .ToList();

            var lstResumen = (from flt in faltantes
                              group flt by new { flt.SpoolID, flt.Prioridad } into spools
                              select new
                              {
                                  SpoolID = spools.Key.SpoolID,
                                  Prioridad = spools.Key.Prioridad,
                                  TotalMateriales = spools.Count(),
                                  TotalCongelado = spools.Count(x => x.Congelado)
                              }).ToList();


            var lstOrdenada = (from flt in faltantes
                               join res in lstResumen on flt.SpoolID equals res.SpoolID
                               select new
                               {
                                   Prioridad = flt.Prioridad,
                                   NombreSpool = flt.NombreSpool,
                                   Isometrico = flt.Isometrico,
                                   CodigoItemCode = flt.CodigoItemCode,
                                   DescripcionItemCode = flt.DescripcionItemCode,
                                   Diametro1 = flt.Diametro1,
                                   Diametro2 = flt.Diametro2,
                                   Etiqueta = flt.Etiqueta,
                                   Cantidad = flt.Cantidad,
                                   MaterialCongelado = flt.Congelado,
                                   SpoolCompleto = res.TotalMateriales == res.TotalCongelado,
                                   MaterialEquivalente = flt.MaterialEquivalente,
                                   PDI = flt.Pdis,
                                   Peso = flt.Peso,
                                   FamiliasAcero = flt.FamiliaAcero,
                                   SpoolEnHold = flt.SpoolEnHold,
                                   ObservacionesSpool = flt.ObservacionesSpoolHold,
                                   IsometricoCompleto = flt.IsometricoCompleto,
                                   Campo1 = flt.Campo1,
                                   Campo2 = flt.Campo2,
                                   DiametroMayor = flt.DiametroMayor                                  
                               })
                               .OrderBy(x => x.SpoolCompleto ? 0 : 1)
                               .ThenBy(x => x.Prioridad)
                               .ThenBy(x => x.NombreSpool)
                               .ThenBy(x => x.DescripcionItemCode)
                               .ThenBy(x => x.Diametro1)
                               .ThenBy(x => x.Diametro2)
                               .ToList();

            var dicPrioridad = condensado.ToDictionary(k => new { ID = k.ItemCodeID, D1= k.D1, D2= k.D2, Pri = k.Prioridad }, v => v.SumaPrioridad);

            #endregion

            byte[] archivo = null;

            //Crear el ms sin parámetros para que pueda crecer
            using(MemoryStream ms = new MemoryStream())
            {
                using (SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    Worksheet ws = null;
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgsFaltantes.Sheet_Faltantes_IC, out ws);

                    Row fila = new Row();

                    #region Encabezado

                    fila = new Row();
                    fila.RowIndex = 2;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.InfoCruce, 2, 9));
                    sheetData.AppendChild(fila);

                    fila = new Row();
                    fila.RowIndex = 3;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(fechaReporte, 3, 9));
                    sheetData.AppendChild(fila);

                    //titulo del reporte de item codes
                    fila = new Row();
                    fila.RowIndex = 4;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.ResumenItemCodes, 4, 5));
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

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Item_Code, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Descripcion, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.D1, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.D2, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntIngenieria, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntFabricacion, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntDispCruce, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntRecibida, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntRechazada, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntCondicionada, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntDispEquiv, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntAsignadaCruceOriginal, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntAsignadaCruceEquivalencia, renglon, col++));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.CntTotalFaltante, renglon, col++));                    

                    foreach (int prioridad in lstPrioridad)
                    {
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(string.Format(MsgsFaltantes.PrioridadFaltante, prioridad), renglon, col++));
                    }

                    sheetData.AppendChild(fila);

                    #endregion

                    lstIcIntegrados.ForEach(itemCode =>
                    {
                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;
                       
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(itemCode.CodigoItemCode, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(itemCode.DescripcionItemCode, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(itemCode.D1, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(itemCode.D2, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.RequeridaTotalIng, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.RequeridaFabricacion, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.DisponibleCruce, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.CantidadRecibidaOriginal + itemCode.CantidadRechazadaOriginal + itemCode.CantidadCondicionadosOriginal, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.CantidadRechazadaOriginal, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.CantidadCondicionadosOriginal, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.DisponibleEquivalentes, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.CongeladoCruceOriginal, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.CongeladoCruceEquivalencia, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(itemCode.TotalFaltante > 0 ? itemCode.TotalFaltante : (int ?)null, renglon, columna++));
                        

                        foreach (int prioridad in lstPrioridad)
                        {
                            var llaveDic = new { ID = itemCode.ItemCodeID, D1 = itemCode.D1, D2 = itemCode.D2, Pri = prioridad };

                            if (dicPrioridad.ContainsKey(llaveDic))
                            {
                                fila.AppendChild(UtileriasExcel.CreaCeldaEntero(dicPrioridad[llaveDic], renglon, columna++));
                            }
                            else
                            {
                                fila.AppendChild(UtileriasExcel.CreaCeldaEntero(null, renglon, columna++));
                            }
                        }

                        sheetData.AppendChild(fila);                        
                    });

                    
                    SheetData hoja2 = UtileriasExcel.GeneraNuevaHoja(xlsDoc, MsgsFaltantes.Sheet_Faltantes_Spool, idHoja++, out ws);
                    UtileriasExcel.InsertaImagen(imgReporteFullPath, "Sam", "Sam", xlsDoc, ws);

                    #region Encabezado reporte

                    fila = new Row();
                    fila.RowIndex = 2;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.InfoCruce, 2, 11));
                    hoja2.AppendChild(fila);

                    fila = new Row();
                    fila.RowIndex = 3;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(fechaReporte, 3, 11));
                    hoja2.AppendChild(fila);

                    fila = new Row();
                    fila.RowIndex = 4;
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.ResumenSpools, 4, 5));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(horaReporte, 4, 11));
                    hoja2.AppendChild(fila);

                    #endregion


                    renglon = 6;
                    fila = new Row();
                    fila.RowIndex = renglon;

                    #region Encabezado columnas

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Prioridad, renglon, 1));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Spool, renglon, 2));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Isometrico, renglon, 3));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Item_Code, renglon, 4));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Descripcion, renglon, 5));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.D1, renglon, 6));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.D2, renglon, 7));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Etiqueta, renglon, 8));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Cantidad, renglon, 9));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.EstatusMaterial, renglon, 10));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.EstatusSpool, renglon, 11));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.EsEquivalencia, renglon, 12));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.PDI, renglon, 13));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Peso, renglon, 14));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.FamAcero, renglon, 15));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Hold, renglon, 16));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.ObservacionesHold, renglon, 17));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.EstatusIsometrico, renglon, 18));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Campo2, renglon, 19));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.DiametroMayor, renglon, 20));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsFaltantes.Campo1, renglon, 21));

                    #endregion

                    hoja2.AppendChild(fila);

                    lstOrdenada.ForEach(material =>
                    {
                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(material.Prioridad, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.NombreSpool, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.Isometrico, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.CodigoItemCode, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.DescripcionItemCode, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(material.Diametro1, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(material.Diametro2, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.Etiqueta, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaEntero(material.Cantidad, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.MaterialCongelado ? MsgsFaltantes.OK : MsgsFaltantes.Falta, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.SpoolCompleto ? MsgsFaltantes.Completo : MsgsFaltantes.Incompleto, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.MaterialEquivalente ? MsgsFaltantes.Si : MsgsFaltantes.No, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(material.PDI, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(material.Peso, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.FamiliasAcero, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.SpoolEnHold ? MsgsFaltantes.Si : MsgsFaltantes.No, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.ObservacionesSpool, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.IsometricoCompleto ? MsgsFaltantes.Completo : MsgsFaltantes.Incompleto, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.Campo2, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(material.DiametroMayor, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(material.Campo1, renglon, columna));

                        hoja2.AppendChild(fila);    
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
            }
        }
    }
}
