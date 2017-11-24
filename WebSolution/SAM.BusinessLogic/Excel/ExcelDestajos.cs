using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelDestajos
    {
        private static readonly object _mutex = new object();
        private static ExcelDestajos _instance;

        //constructor privado para implementar patron singleton
        private ExcelDestajos()
        {
        }

        //obtiene la instancia de la clase ExcelEstimacionJuntas
        public static ExcelDestajos Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelDestajos();
                    }
                }
                return _instance;
            }
        }

        public byte[] ObtenerExcelPorIDs(int proyectoId, int tipoJunta, int famAcero, int procesoId, string procesoValor, string proyectoValor)
        {
            byte[] archivo;
            Dictionary<int, string> cedulas = CacheCatalogos.Instance.ObtenerCedulas().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, decimal> diametros = CacheCatalogos.Instance.ObtenerDiametros().ToDictionary(x => x.ID, y => y.Valor);
            Dictionary<int, string> famAceros = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> tipoJuntas = CacheCatalogos.Instance.ObtenerTiposJunta().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> procesoRellenos = CacheCatalogos.Instance.ObtenerProcesosRelleno().ToDictionary(x => x.ID, y => y.Codigo);
            Dictionary<int, string> procesoRaices = CacheCatalogos.Instance.ObtenerProcesosRaiz().ToDictionary(x => x.ID, y => y.Codigo);

            string pestana = string.Format("{0}-{1}-{2}-{3}", procesoValor, proyectoValor, famAceros[famAcero], tipoJuntas[tipoJunta]);
            //Crear el ms sin parámetros para que pueda crecer
            using(MemoryStream ms = new MemoryStream())
            {
                using (SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, pestana);
                 
                    uint z = 1;
                    uint renglon = 1;

                    Row fila = new Row();
                    fila.RowIndex = renglon;

                    if(procesoValor.StartsWith("RE"))
                    {
                        List<CostoProcesoRelleno> listaRelleno = CostoProcesoRellenoBO.Instance.ObtenerTodosPorId(proyectoId, famAcero, tipoJunta, procesoId);

                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.CostoProcesoRellenoID, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Proyecto, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.FamiliaAcero, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.TipoJunta, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.ProcesoRelleno, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Diametro, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Cedula, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Costo, renglon, z++));
                        sheetData.AppendChild(fila);
                        #region Exportacion ProcesoRelleno
                        foreach (CostoProcesoRelleno dfto in listaRelleno)
                        {
                            uint columna = 1;
                            fila = new Row();
                            fila.RowIndex = ++renglon;

                            //fila.AppendChild(UtileriasExcel.CreaCeldaEntero(dfto.CostoProcesoRellenoID, renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(proyectoValor, renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(famAceros[dfto.FamiliaAceroID], renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(tipoJuntas[dfto.TipoJuntaID], renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(procesoRellenos[dfto.ProcesoRellenoID], renglon, columna++));
							fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(diametros[dfto.DiametroID], renglon, columna++));
                            fila.AppendChild(UtileriasExcel.CreaCeldaTexto(cedulas[dfto.CedulaID], renglon, columna++));
                            fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(dfto.Costo, renglon, columna++));

                            sheetData.AppendChild(fila);
                        }
                        #endregion               
                    }
                    else if(procesoValor.StartsWith("R"))
                    {
                        List<CostoProcesoRaiz> listaRaiz = CostoProcesoRaizBO.Instance.ObtenerTodosPorId(proyectoId, famAcero, tipoJunta, procesoId);

                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.CostoProcesoRaizID, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Proyecto, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.FamiliaAcero, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.TipoJunta, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.ProcesoRaiz, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Diametro, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Cedula, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Costo, renglon, z++));
                        sheetData.AppendChild(fila);
                        #region Exportacion ProcesoRaiz
                        foreach (CostoProcesoRaiz dfto in listaRaiz)
                        {
                            uint columna = 1;
                            fila = new Row();
                            fila.RowIndex = ++renglon;

                            //fila.AppendChild(UtileriasExcel.CreaCeldaEntero(dfto.CostoProcesoRaizID, renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(proyectoValor, renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(famAceros[dfto.FamiliaAceroID], renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(tipoJuntas[dfto.TipoJuntaID], renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(procesoRaices[dfto.ProcesoRaizID], renglon, columna++));
                            fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(diametros[dfto.DiametroID], renglon, columna++));
                            fila.AppendChild(UtileriasExcel.CreaCeldaTexto(cedulas[dfto.CedulaID], renglon, columna++));
                            fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(dfto.Costo, renglon, columna++));

                            sheetData.AppendChild(fila);
                        }
                        #endregion
                    }
                    else if(procesoValor.StartsWith("A"))
                    {
                        List<CostoArmado> listaArmado = CostoArmadoBO.Instance.ObtenerTodosPorId(proyectoId, famAcero, tipoJunta);

                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.CostoArmadoID, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Proyecto, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.FamiliaAcero, renglon, z++));
                        //fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.TipoJunta, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Diametro, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Cedula, renglon, z++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsDestajos.Costo, renglon, z++));
                        sheetData.AppendChild(fila);
                        #region Exportacion Armado
                        foreach (CostoArmado dfto in listaArmado)
                        {
                            uint columna = 1;
                            fila = new Row();
                            fila.RowIndex = ++renglon;

                            //fila.AppendChild(UtileriasExcel.CreaCeldaEntero(dfto.CostoArmadoID, renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(proyectoValor, renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(famAceros[dfto.FamiliaAceroID], renglon, columna++));
                            //fila.AppendChild(UtileriasExcel.CreaCeldaTexto(tipoJuntas[dfto.TipoJuntaID], renglon, columna++));
                            fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(diametros[dfto.DiametroID], renglon, columna++));
                            fila.AppendChild(UtileriasExcel.CreaCeldaTexto(cedulas[dfto.CedulaID], renglon, columna++));
                            fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(dfto.Costo, renglon, columna++));

                            sheetData.AppendChild(fila);
                        }
                        #endregion
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
