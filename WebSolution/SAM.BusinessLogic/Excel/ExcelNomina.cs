using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAM.BusinessObjects.Administracion;
using SAM.Entities.Grid;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelNomina
    {
        private static readonly object _mutex = new object();
        private static ExcelNomina _instance;

        /// <summary>
        /// Constructor privado para implementar patron singleton
        /// </summary>
        private ExcelNomina()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ExcelNomina
        /// </summary>
        public static ExcelNomina Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelNomina();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Méotodo principal que se encarga de generar en memoria un archivo de Excel con la información del destajo.
        /// El archivo de Excel consta de lo siguiente:
        /// + Hoja con resumen de destajo por personas
        /// + Una hoja por cada tubero con el detalle de las juntas de ese tubero
        /// + Una hoja por cada soldador con el detalle de las juntas de ese soldador
        /// </summary>
        /// <param name="periodoDestajoID">ID del periodo de destajo que se desea exportar a Excel</param>
        /// <returns>Arreglo de bytes con el archivo de Excel</returns>
        public byte[] ObtenerNominaPorPeriodo(int periodoDestajoID)
        {
            byte[] archivo;

            //Traerme las personas, ordenar por tuberos primero
            IEnumerable<GrdPersonaDestajoExcel> personas = DestajoBO.Instance
                                                                    .ObtenerPersonasPorPeriodoParaExcel(periodoDestajoID)
                                                                    .OrderBy(x => x.EsTubero ? 0 : 1)
                                                                    .ThenBy(x => x.Codigo);
            
            //Traer todas las juntas de todos los tuberos dentro del periodo
            IEnumerable<GrdDetalleDestajoTuberoExcel> tuberos = DestajoBO.Instance
                                                                         .ObtenerDetalleDestajoTuberoPorPeriodo(periodoDestajoID);

            //Traer todas las juntas de todos los soldadores dentro del periodo
            IEnumerable<GrdDetalleDestajoSoldadorExcel> soldadores = DestajoBO.Instance
                                                                              .ObtenerDetalleDestajoSoldadorPorPeriodo(periodoDestajoID);

            //Crear el ms sin parámetros para que pueda crecer
            using (MemoryStream ms = new MemoryStream())
            {
                using (SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgsNomina.Caratula_NombreHoja);

                    uint renglon = 1;
                    uint idHoja = 2;

                    Row fila = new Row();
                    fila.RowIndex = renglon;

                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Caratula_Nombre, renglon, 1));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Caratula_Clave, renglon, 2));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Caratula_Categoria, renglon, 3));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Caratula_Total, renglon, 4));
                    fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Caratula_Observaciones, renglon, 5));

                    sheetData.AppendChild(fila);

                    IEnumerable<GrdDetalleDestajoTuberoExcel> jtasArmadas;
                    IEnumerable<GrdDetalleDestajoSoldadorExcel> jtasSoldadas;

                    foreach (GrdPersonaDestajoExcel persona in personas)
                    {
                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(persona.NombreCompleto, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(persona.Codigo, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(persona.CategoriaPuestoTexto, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaMoneda(persona.TotalAPagar, renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(persona.Comentarios, renglon, columna++));

                        sheetData.AppendChild(fila);

                        if (persona.EsTubero)
                        {
                            jtasArmadas = tuberos.Where(x => x.TuberoID == persona.IdPersona);
                            GeneraHojaDetalleTubero(xlsDoc, jtasArmadas, persona, idHoja++);
                        }
                        else
                        {
                            jtasSoldadas = soldadores.Where(x => x.SoldadorID == persona.IdPersona);
                            GeneraHojaDetalleSoldador(xlsDoc, jtasSoldadas, persona, idHoja++);
                        }
                    }

                    xlsDoc.Close();
                }

                archivo = ms.ToArray();
                ms.Close();
            }

            return archivo;
        }

        /// <summary>
        /// Genera una nueva hoja en el archivo de Excel con el detalle del destajo para un soldador en particular.
        /// </summary>
        /// <param name="xlsDoc">Documento de Excel al cual se agregará la hoja</param>
        /// <param name="jtasSoldadas">Detalle de las juntas soldadas únicamente por ese soldador</param>
        /// <param name="persona">Datos de la persona</param>
        /// <param name="idHoja">ID consecutivo de la hoja de Excel que sigue</param>
        private void GeneraHojaDetalleSoldador(SpreadsheetDocument xlsDoc, IEnumerable<GrdDetalleDestajoSoldadorExcel> jtasSoldadas, GrdPersonaDestajoExcel persona, uint idHoja)
        {
            string nombreHoja = persona.Codigo;

            Worksheet ws = null;
            SheetData sheetData = UtileriasExcel.GeneraNuevaHoja(xlsDoc, nombreHoja, idHoja, out ws);

            uint renglon = 1;

            Row fila = new Row();
            fila.RowIndex = renglon;

            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Isometrico, renglon, 1));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Spool, renglon, 2));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_NumeroControl, renglon, 3));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Junta, renglon, 4));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_TipoJunta, renglon, 5));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Diametro, renglon, 6));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Cedula, renglon, 7));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Soldador_FamiliaAcero, renglon, 8));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Soldador_FechaSoldadura, renglon, 9));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Caratula_Clave, renglon, 10));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Soldador_ProcesoRaiz, renglon, 11));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Soldador_ProcesoRelleno, renglon, 12));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Soldador_NumFondeadores, renglon, 13));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Soldador_NumRellenadores, renglon, 14));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Soldador_ObservacionesSoldadura, renglon, 15));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_ObservacionesDestajo, renglon, 16));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Total, renglon, 17));


            sheetData.AppendChild(fila);

            foreach (GrdDetalleDestajoSoldadorExcel junta in jtasSoldadas.OrderBy(x => x.EtiquetaJunta))
            {
                uint columna = 1;

                fila = new Row();
                fila.RowIndex = ++renglon;

                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.Isometrico, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.Spool, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.NumeroControl, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.EtiquetaJunta, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.TipoJunta, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(junta.Diametro, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.Cedula, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.FamiliaAcero, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaFecha(junta.FechaSoldadura, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(persona.Codigo, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.ProcesoRaiz, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.ProcesoRelleno, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaEntero(junta.NumeroFondeadores, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaEntero(junta.NumeroRellenadores, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.ComentariosSoldadura, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.ComentariosDestajo, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaMoneda(junta.Total, renglon, columna++));

                sheetData.AppendChild(fila);
            }
        }


        /// <summary>
        /// Genera una nueva hoja en el archivo de Excel con el detalle del destajo para un tubero en particular.
        /// </summary>
        /// <param name="xlsDoc">Documento de Excel al cual se agregará la hoja</param>
        /// <param name="jtasArmadas">Detalle de las juntas armadas únicamente por ese tubero</param>
        /// <param name="persona">Datos de la persona</param>
        /// <param name="idHoja">ID consecutivo de la hoja de Excel que sigue</param>
        private void GeneraHojaDetalleTubero(SpreadsheetDocument xlsDoc, IEnumerable<GrdDetalleDestajoTuberoExcel> jtasArmadas, GrdPersonaDestajoExcel persona, uint idHoja)
        {
            string nombreHoja = persona.Codigo;

            Worksheet ws = null;
            SheetData sheetData = UtileriasExcel.GeneraNuevaHoja(xlsDoc, nombreHoja, idHoja, out ws);

            uint renglon = 1;

            Row fila = new Row();
            fila.RowIndex = renglon;

            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Isometrico, renglon, 1));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Spool, renglon, 2));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_NumeroControl, renglon, 3));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Junta, renglon, 4));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_TipoJunta, renglon, 5));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Diametro, renglon, 6));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Cedula, renglon, 7));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Tubero_FechaArmado, renglon, 8));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Caratula_Clave, renglon, 9));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.Tubero_ObservacionesArmado, renglon, 10));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_ObservacionesDestajo, renglon, 11));
            fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsNomina.General_Total, renglon, 12));

            sheetData.AppendChild(fila);

            foreach (GrdDetalleDestajoTuberoExcel junta in jtasArmadas.OrderBy(x => x.EtiquetaJunta))
            {
                uint columna = 1;

                fila = new Row();
                fila.RowIndex = ++renglon;

                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.Isometrico, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.Spool, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.NumeroControl, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.EtiquetaJunta, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.TipoJunta, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(junta.Diametro, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.Cedula, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaFecha(junta.FechaArmado, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(persona.Codigo, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.ComentariosArmado, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaTexto(junta.ComentariosDestajo, renglon, columna++));
                fila.AppendChild(UtileriasExcel.CreaCeldaMoneda(junta.Total, renglon, columna++));

                sheetData.AppendChild(fila);
            }
        }
    }
}