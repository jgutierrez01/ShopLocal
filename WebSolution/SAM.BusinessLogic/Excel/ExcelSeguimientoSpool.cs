using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Administracion;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Utilerias;
using SAM.Common;
using SAM.Entities;
using SAM.Entities.Grid;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelSeguimientoSpool
    {
        private static readonly object _mutex = new object();
        private static ExcelSeguimientoSpool _instance;
       

        //constructor privado para implementar patron singleton
        private ExcelSeguimientoSpool()
        {
        }

        //obtiene la instancia de la clase ExcelEstimacionJuntas
        public static ExcelSeguimientoSpool Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelSeguimientoSpool();
                    }
                }
                return _instance;
            }
        }

        public byte[] ObtenerExcelPorIDs(int proyectoID, string ordenTrabajo, string numeroControl, bool embarcados, string filtros)
        {
            byte[] archivo;
            int? OrdenTrabajoID;
            int? NumeroControlID;
            List<string> NombreCampoSeguimientoSpool = null;

            if (filtros != null)
            {
                    int[] FiltrosID = filtros.Split(',').ToList().Select(x => x.SafeIntParse()).ToArray();
                    NombreCampoSeguimientoSpool = SeguimientoSpoolBL.Instance.ObtenerNombreCampoSeguimientoSpool(FiltrosID);              
            }
            //Crear el ms sin parámetros para que pueda crecer
            using (MemoryStream ms = new MemoryStream())
            {
                using (SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    SheetData sheetData = UtileriasExcel.CreaDocumentoDefault(xlsDoc, MsgsSegSpool.Nombre_Hoja_Excel);

                    OrdenTrabajoID = ordenTrabajo.SafeIntNullableParse();

                    NumeroControlID = numeroControl.SafeIntNullableParse();

                    DataRow[] rows = SeguimientoSpoolBL.Instance.ObtenerDataSetParaSeguimientoSpool(proyectoID,
                                                                                    OrdenTrabajoID,
                                                                                    NumeroControlID,
                                                                                    null,
                                                                                    "GeneralNumeroDeControl ASC, GeneralNumeroJuntas DESC");


                    uint z = 1;
                    uint renglon = 1;

                    Row fila = new Row();
                    fila.RowIndex = renglon;

                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralProyecto")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Proyecto, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralOrdenDeTrabajo")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.OrdenDeTrabajo, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralNumeroDeControl")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.NumeroControl, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaEmision")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.FechaEmision, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralSpool")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Spool, renglon, z++));
                    //if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralOrdenTrabajoEspecial")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralOrdenTrabajoEspecial, renglon, z++));
                    //if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralNumeroControlEspecial")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralNumeroControlEspecial, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralNumeroJuntas")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.NumeroJuntas, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralPrioridad")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Prioridad, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralPeqs")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Peqs, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFamiliaAcero1")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.FamiliaDeAcero1, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFamiliaMaterial1")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.FamiliaDeMaterial1, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFamiliaAcero2")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.FamiliaDeAcero2, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFamiliaMaterial2")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.FamiliaDeMaterial2, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralUltimaLocalizacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.UltimaLocalizacion, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaLocalizacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralFechaLocalizacion, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralPorcentajePnd")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PorcentajePND, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Generalpdi")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Pdi, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("DiametroPlano")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.DiametroPlano, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralDiametroMayor")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralDiametroMayor, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaLiberacionCalidad")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralFechaliberacionCalidad, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralUsuarioLiberacionCalidad")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralUsuarioLiberacionCalidad, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaLiberacionMaterial")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralFechaLiberacionMaterial, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralUsuarioLiberacionMaterial")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralUsuarioLiberacionMaterial, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaOkPnd")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralFechaOkPnd, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralUsuarioOkPnd")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralUsuarioOkPnd, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Generalpeso")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Peso, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralArea")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Area, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralEspecificacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Especificacion, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralTieneHold")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.TieneHold, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("ObservacionesHold")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.ObservacionesHold, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("FechaHold")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.FechaHold, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralPWHT")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.RequierePWHT, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralRequierePruebaHidrostatica")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralRequierePruebaHidrostatica, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Isometrico")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Isometrico, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralKgsTeoricos")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.KgTeoricos, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("RevisionCte")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.RevisionCliente, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("RevisionStgo")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Revision, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PorcentajeArmado")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PorcentajeArmado, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PorcentajeSoldado")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PorcentajeSoldado, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralMaterialPendiente")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.MaterialPendiente, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralEtiquetaSegmentos")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.EtiquetaSegmentos, renglon, z++));
                    //if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralEsRevision")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.GeneralEsRevision, renglon, z++));


                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo1")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Campo1, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo2")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Campo2, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo3")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Campo3, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo4")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Campo4, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo5")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Campo5, renglon, z++));


                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento1")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Segmento1, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento2")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Segmento2, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento3")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Segmento3, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento4")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Segmento4, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento5")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Segmento5, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento6")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Segmento6, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento7")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Segmento7, renglon, z++));

                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalFecha")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionDimensionalFecha, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalFechaReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionDimensionalFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalNumeroReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionDimensionalNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalHoja")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionDimensionalHoja, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalResultado")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionDimensionalResultado, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalNumeroRechazos")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionDimensionalNumeroRechazos, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalInspector")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionDimensionalInspector, renglon, z++));

                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresFecha")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionEspesoresFecha, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresFechaReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionEspesoresFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresNumeroReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionEspesoresNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresHoja")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionEspesoresHoja, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresResultado")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionEspesoresResultado, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresNumeroRechazos")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.InspeccionEspesoresNumeroRechazos, renglon, z++));

                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaNumeroRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaSistema")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaSistema, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaColor")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaColor, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaCodigo")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaCodigo, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaSendBlast")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaFechaSandBlast, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReporteSendBlast")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaReporteSandBlast, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaPrimarios")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaFechaPrimarios, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReportePrimarios")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaReportePrimarios, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaIntermedios")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaFechaIntermedios, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReporteIntermedios")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaReporteIntermedios, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaAcabadoVisual")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaFechaAcabadoVisual, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReporteAcabadoVisual")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaReporteAcabadoVisual, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaAdherencia")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaFechaAdherencia, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReporteAdherencia")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaReporteAdherencia, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaPullOff")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaFechaPullOff, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReportePullOff")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaReportePullOff, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaLiberado")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PinturaLiberado, renglon, z++));

                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueEtiqueta")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.EmbarqueEtiqueta, renglon, z++));
                    //if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueFechaEtiqueta")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.EmbarqueFechaEtiqueta, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueFolioPreparacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.EmbarqueFolioPreparacion, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueFechaEstimada")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.EmbarqueFechaCarga, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueFechaEmbarque")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.EmbarqueFechaEmbarque, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueNumeroEmbarque")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.EmbarqueNumeroEmbarque, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota1")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Nota1, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota2")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Nota2, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota3")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Nota3, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota4")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Nota4, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota5")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Nota5, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueVigenciaAduana")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.EmbarqueVigenciaAduana, renglon, z++));

                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroFechaRequisicion")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PruebaHidroFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroNumeroRequisicion")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PruebaHidroNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroFechaPrueba")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PruebaHidroFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroFechaReporte")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PruebaHidroFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroNumeroReporte")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PruebaHidroNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroHoja")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PruebaHidroHoja, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroAprobado")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PruebaHidroAprobado, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroPresion")) fila.Append(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PruebaHidroPresion, renglon, z++));

                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolUltimoProceso")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresSpoolUltimoProceso, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresAreaGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresAreaGrupo, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresKgsGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresKgsGrupo, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresDiamGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresDiamGrupo, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresPeqGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresPeqGrupo, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSistemaPinturaFinal")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresSistemaPinturaFinal, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresPaintNoPaint")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresPaintNoPaint, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresDiametroPromedio")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresDiametroPromedio, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresPaintLine")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresPaintLine, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresAreaEQ")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresAreaEQ, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresInoxNoInox")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresInoxNoInox, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresClasifInox")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresClasifInox, renglon, z++));


                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("NumeroTransferencia")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.NumeroTransferencia, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PreparacionTransferencia")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.PreparacionTransferencia, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Transferencia")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.Transferencia, renglon, z++));

                    //------------------------------AGRUPADORES PND ----------------------------------------------------------
                    XmlDocument datosXML = new XmlDocument();
                    

                    //Cantidad de Juntas por tipo
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDTiposJuntas"))
                    {
                        string xml = rows.Select(x => x.Field<string>("AgrupadoresSpoolPNDTiposJuntas")).SingleOrDefault();
                        datosXML.LoadXml(xml);
                        XmlNodeList lista = datosXML.GetElementsByTagName("Junta");
                        //XmlNode nodo = lista[0];
                        //Orden de origen BW   ,SW   ,TH   ,TW   ,INS  ,SOP  
                        // el orden que se requiere es BW, INS, SW, TW, SOP, TH
                        List<string> columnas = new List<string>();
                        string nombreColumna = "";
                        foreach (XmlNode nodo in lista)
                        {
                            columnas.Add(nodo.Attributes["Codigo"].Value.Trim());
                        }
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Quantity " + columnas[columnas.LastIndexOf("BW")]
                                : "Cantidad " + columnas[columnas.LastIndexOf("BW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Quantity " + columnas[columnas.LastIndexOf("INS")]
                                : "Cantidad " + columnas[columnas.LastIndexOf("INS")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Quantity " + columnas[columnas.LastIndexOf("SW")]
                                : "Cantidad " + columnas[columnas.LastIndexOf("SW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Quantity " + columnas[columnas.LastIndexOf("TW")]
                                : "Cantidad " + columnas[columnas.LastIndexOf("TW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Quantity " + columnas[columnas.LastIndexOf("SOP")]
                                : "Cantidad " + columnas[columnas.LastIndexOf("SOP")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Quantity " + columnas[columnas.LastIndexOf("TH")]
                                : "Cantidad " + columnas[columnas.LastIndexOf("TH")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                    }
                    //CALSIFICACION
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDClasificacion"))
                    {
                        string xml = rows.Select(x => x.Field<string>("AgrupadoresSpoolPNDTiposJuntas")).SingleOrDefault();
                        datosXML.LoadXml(xml);
                        XmlNodeList lista = datosXML.GetElementsByTagName("Junta");
                        //XmlNode nodo = lista[0];
                        //Orden de origen BW   ,SW   ,TH   ,TW   ,INS  ,SOP  
                        // el orden que se requiere es BW, INS, SW, TW, SOP, TH
                        List<string> columnas = new List<string>();
                        string nombreColumna = "";
                        foreach (XmlNode nodo in lista)
                        {
                            columnas.Add(nodo.Attributes["Codigo"].Value.Trim());
                        }
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Classification " + columnas[columnas.LastIndexOf("BW")]
                                : "Clasificación " + columnas[columnas.LastIndexOf("BW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Classification " + columnas[columnas.LastIndexOf("INS")]
                                : "Clasificación " + columnas[columnas.LastIndexOf("INS")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Classification " + columnas[columnas.LastIndexOf("SW")]
                                : "Clasificación " + columnas[columnas.LastIndexOf("SW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Classification " + columnas[columnas.LastIndexOf("TW")]
                                : "Clasificación " + columnas[columnas.LastIndexOf("TW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Classification " + columnas[columnas.LastIndexOf("SOP")]
                                : "Clasificación " + columnas[columnas.LastIndexOf("SOP")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Classification " + columnas[columnas.LastIndexOf("TH")]
                                : "Clasificación " + columnas[columnas.LastIndexOf("TH")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                    }
                    //AVANCE DE JUNTAS
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDAvance"))
                    {
                        string xml = rows.Select(x => x.Field<string>("AgrupadoresSpoolPNDTiposJuntas")).SingleOrDefault();
                        datosXML.LoadXml(xml);
                        XmlNodeList lista = datosXML.GetElementsByTagName("Junta");
                        //XmlNode nodo = lista[0];
                        //Orden de origen BW   ,SW   ,TH   ,TW   ,INS  ,SOP  
                        // el orden que se requiere es BW, INS, SW, TW, SOP, TH
                        List<string> columnas = new List<string>();
                        string nombreColumna = "";
                        foreach (XmlNode nodo in lista)
                        {
                            columnas.Add(nodo.Attributes["Codigo"].Value.Trim());
                        }
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Advance " + columnas[columnas.LastIndexOf("BW")]
                                : "Avance " + columnas[columnas.LastIndexOf("BW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Advance " + columnas[columnas.LastIndexOf("INS")]
                                : "Avance " + columnas[columnas.LastIndexOf("INS")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Advance " + columnas[columnas.LastIndexOf("SW")]
                                : "Avance " + columnas[columnas.LastIndexOf("SW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Advance " + columnas[columnas.LastIndexOf("TW")]
                                : "Avance " + columnas[columnas.LastIndexOf("TW")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Advance " + columnas[columnas.LastIndexOf("SOP")]
                                : "Avance " + columnas[columnas.LastIndexOf("SOP")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                        nombreColumna = CultureInfo.CurrentCulture.Name == "en-US" ? "Advance " + columnas[columnas.LastIndexOf("TH")]
                                : "Avance " + columnas[columnas.LastIndexOf("TH")];
                        fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(nombreColumna, renglon, z++));
                    }

                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDReparaciones")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresSpoolPNDReparaciones, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDPendientes")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresSpoolPNDPendientes, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDReportes")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresSpoolPNDReportes, renglon, z++));
                    if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDJuntasSeguimiento")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(MsgsSegSpool.AgrupadoresSpoolPNDJuntasSeguimiento, renglon, z++));

                    //------------------------------AGRUPADORES PND ----------------------------------------------------------



                    sheetData.AppendChild(fila);
                    foreach (DataRow jta in rows)
                    {
                        jta.ItemArray[0].ToString();
                        uint columna = 1;
                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralProyecto")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralProyecto"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralOrdenDeTrabajo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralOrdenDeTrabajo"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralNumeroDeControl")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralNumeroDeControl"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaEmision")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("GeneralFechaEmision"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralSpool")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralSpool"), renglon, columna++));
                        //if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralOrdenTrabajoEspecial")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralOrdenTrabajoespecial"), renglon, columna++));
                        //if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralNumeroControlEspecial")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralNumeroControlEspecial"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralNumeroJuntas")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("GeneralNumeroJuntas"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralPrioridad")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("GeneralPrioridad"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralPeqs")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralPeqs"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFamiliaAcero1")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFamiliaAcero1"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFamiliaMaterial1")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFamiliaMaterial1"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFamiliaAcero2")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFamiliaAcero2"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFamiliaMaterial2")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFamiliaMaterial2"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralUltimaLocalizacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTextoInline(jta.Field<string>("GeneralUltimalocalizacion"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaLocalizacion")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("GeneralFechaLocalizacion"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralPorcentajePnd")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("GeneralPorcentajePnd"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Generalpdi")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal>("GeneralPdi"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("DiametroPlano")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(jta.Field<decimal?>("DiametroPlano"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralDiametroMayor")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalCuatroDecimales(jta.Field<decimal?>("GeneralDiametroMayor"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaLiberacionCalidad")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("GeneralFechaLiberacionCalidad"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralUsuarioLiberacionCalidad")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralUsuarioLiberacionCalidad"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaLiberacionMaterial")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("GeneralFechaLiberacionMaterial"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralUsuarioLiberacionMaterial")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralUsuarioLiberacionMaterial"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralFechaOkPnd")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("GeneralFechaOkPnd"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralUsuarioOkPnd")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralUsuarioOkPnd"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Generalpeso")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralPeso"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralArea")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralArea"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralEspecificacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralEspecificacion"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralTieneHold")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralTieneHold")), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("ObservacionesHold")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ObservacionesHold"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("FechaHold")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("FechaHold"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralPWHT")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralPWHT")), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralRequierePruebaHidrostatica")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<bool?>("GeneralRequierePruebaHidrostatica") != null ?
                            TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("GeneralRequierePruebaHidrostatica").SafeBoolParse()) : "", renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Isometrico")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Isometrico"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralKgsTeoricos")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralKgsTeoricos"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("RevisionCte")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("RevisionCte"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("RevisionStgo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("RevisionStgo"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PorcentajeArmado")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal>("PorcentajeArmado"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PorcentajeSoldado")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal>("PorcentajeSoldado"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralMaterialPendiente")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralMaterialPendiente")), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralEtiquetaSegmentos")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralEtiquetaSegmentos"), renglon, columna++));
                        //if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("GeneralEsRevision")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralEsRevision")), renglon, columna++));


                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo1")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo1"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo2")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo2"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo3")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo3"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo4")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo4"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Campo5")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo5"), renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento1")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Segmento1"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento2")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Segmento2"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento3")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Segmento3"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento4")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Segmento4"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento5")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Segmento5"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento6")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Segmento6"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Segmento7")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Segmento7"), renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalFecha")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionDimensionalFecha"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalFechaReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionDimensionalFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalNumeroReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionDimensionalNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalHoja")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("InspeccionDimensionalHoja"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalResultado")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("InspeccionDimensionalResultado") == null ? false : jta.Field<bool>("InspeccionDimensionalResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalNumeroRechazos")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("InspeccionDimensionalNumeroRechazos"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionDimensionalInspector")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionDimensionalInspector"), renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresFecha")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionEspesoresFecha"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresFechaReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionEspesoresFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresNumeroReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionEspesoresNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresHoja")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("InspeccionEspesoresHoja"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresResultado")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("InspeccionEspesoresResultado") == null ? false : jta.Field<bool>("InspeccionEspesoresResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("InspeccionEspesoresNumeroRechazos")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("InspeccionEspesoresNumeroRechazos"), renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaNumeroRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaSistema")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaSistema"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaColor")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaColor"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaCodigo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaCodigo"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaSendBlast")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaSendBlast"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReporteSendBlast")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReporteSendBlast"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaPrimarios")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaPrimarios"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReportePrimarios")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReportePrimarios"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaIntermedios")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaIntermedios"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReporteIntermedios")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReporteIntermedios"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaAcabadoVisual")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaAcabadoVisual"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReporteAcabadoVisual")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReporteAcabadoVisual"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaAdherencia")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaAdherencia"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReporteAdherencia")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReporteAdherencia"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaFechaPullOff")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaPullOff"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaReportePullOff")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReportePullOff"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PinturaLiberado")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PinturaLiberado") == null ? false : jta.Field<bool>("PinturaLiberado")), renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueEtiqueta")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("EmbarqueEtiqueta"), renglon, columna++));
                        //if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueFechaEtiqueta")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("EmbarqueFechaEtiqueta"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueFolioPreparacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("EmbarqueFolioPreparacion"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueFechaEstimada")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("EmbarqueFechaEstimada"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueFechaEmbarque")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("EmbarqueFechaEmbarque"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueNumeroEmbarque")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("EmbarqueNumeroEmbarque"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota1")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Nota1"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota2")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Nota2"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota3")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Nota3"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota4")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Nota4"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Nota5")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Nota5"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("EmbarqueVigenciaAduana")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("EmbarqueVigenciaAduana"), renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroFechaRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaHidroFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroNumeroRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaHidroNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroFechaPrueba")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaHidroFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroFechaReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaHidroFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroNumeroReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaHidroNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroHoja")) fila.AppendChild(UtileriasExcel.CreaCeldaEntero(jta.Field<int?>("PruebaHidroHoja"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroAprobado")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaHidroAprobado") == null ? false : jta.Field<bool>("PruebaHidroAprobado")), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PruebaHidroPresion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaHidroPresion"), renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolUltimoProceso")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresSpoolUltimoProceso"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresAreaGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresAreaGrupo"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresKgsGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresKgsGrupo"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresDiamGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresDiamGrupo"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresPeqGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresPeqGrupo"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSistemaPinturaFinal")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresSistemaPinturaFinal"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresPaintNoPaint")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresPaintNoPaint"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresDiametroPromedio")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresDiametroPromedio"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresPaintLine")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresPaintLine"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresAreaEQ")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresAreaEQ"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresInoxNoInox")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresInoxNoInox"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresClasifInox")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresClasifInox"), renglon, columna++));

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("NumeroTransferencia")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("NumeroTransferencia"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("PreparacionTransferencia")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PreparacionTransferencia"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("Transferencia")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("Transferencia"), renglon, columna++));
                    
                       

          
                        //-----------------------------------------------AGRUPADORES PND ----------------------------------
                        XmlDocument documento = new XmlDocument();
                        
                        //TIPOS DE JUNTAS
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDTiposJuntas"))
                        {
                            documento.LoadXml(jta.Field<string>("AgrupadoresSpoolPNDTiposJuntas"));
                            XmlNodeList lista = documento.GetElementsByTagName("Junta");
                            //Orden de origen BW   ,SW   ,TH   ,TW   ,INS  ,SOP  
                            // el orden que se requiere es BW, INS, SW, TW, SOP, TH
                            int[] valores = new int[6];
                            foreach (XmlNode nodo in lista)
                            {
                                string codigo = nodo.Attributes["Codigo"].Value.Trim();
                                switch (codigo)
                                {
                                    case "BW":
                                        valores[0] = nodo.Attributes["Cantidad"].Value.SafeIntParse();
                                        break;
                                    case "INS":
                                        valores[1] = nodo.Attributes["Cantidad"].Value.SafeIntParse();
                                        break;
                                    case "SW":
                                        valores[2] = nodo.Attributes["Cantidad"].Value.SafeIntParse();
                                        break;
                                    case "TW":
                                        valores[3] = nodo.Attributes["Cantidad"].Value.SafeIntParse();
                                        break;
                                    case "SOP":
                                        valores[4] = nodo.Attributes["Cantidad"].Value.SafeIntParse();
                                        break;
                                    case "TH":
                                        valores[5] = nodo.Attributes["Cantidad"].Value.SafeIntParse();
                                        break;
                                }
                            }

                            for (int i = 0; i < lista.Count; i++)
                            {
                                fila.AppendChild(UtileriasExcel.CreaCeldaEntero(valores[i], renglon, columna++));
                            }
                        }

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDClasificacion"))
                        {
                            documento.LoadXml(jta.Field<string>("AgrupadoresSpoolPNDClasificacion"));
                            XmlNodeList lista = documento.GetElementsByTagName("Junta");
                            //Orden de origen BW   ,SW   ,TH   ,TW   ,INS  ,SOP  
                            // el orden que se requiere es BW, INS, SW, TW, SOP, TH
                            int[] valores = new int[6];
                            foreach (XmlNode nodo in lista)
                            {
                                string codigo = nodo.Attributes["Codigo"].Value.Trim();
                                switch (codigo)
                                {
                                    case "BW":
                                        valores[0] = nodo.Attributes["Clasificacion"].Value.SafeIntParse();
                                        break;
                                    case "INS":
                                        valores[1] = nodo.Attributes["Clasificacion"].Value.SafeIntParse();
                                        break;
                                    case "SW":
                                        valores[2] = nodo.Attributes["Clasificacion"].Value.SafeIntParse();
                                        break;
                                    case "TW":
                                        valores[3] = nodo.Attributes["Clasificacion"].Value.SafeIntParse();
                                        break;
                                    case "SOP":
                                        valores[4] = nodo.Attributes["Clasificacion"].Value.SafeIntParse();
                                        break;
                                    case "TH":
                                        valores[5] = nodo.Attributes["Clasificacion"].Value.SafeIntParse();
                                        break;
                                }
                            }

                            for (int i = 0; i < lista.Count; i++)
                            {
                                fila.AppendChild(UtileriasExcel.CreaCeldaEntero(valores[i], renglon, columna++));
                            }
                        }

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDAvance"))
                        {
                            documento.LoadXml(jta.Field<string>("AgrupadoresSpoolPNDAvance"));
                            XmlNodeList lista = documento.GetElementsByTagName("Junta");
                            //Orden de origen BW   ,SW   ,TH   ,TW   ,INS  ,SOP  
                            // el orden que se requiere es BW, INS, SW, TW, SOP, TH
                            int[] valores = new int[6];
                            foreach (XmlNode nodo in lista)
                            {
                                string codigo = nodo.Attributes["Codigo"].Value.Trim();
                                switch (codigo)
                                {
                                    case "BW":
                                        valores[0] = nodo.Attributes["Avance"].Value.SafeIntParse();
                                        break;
                                    case "INs":
                                        valores[1] = nodo.Attributes["Avance"].Value.SafeIntParse();
                                        break;
                                    case "SW":
                                        valores[2] = nodo.Attributes["Avance"].Value.SafeIntParse();
                                        break;
                                    case "TW":
                                        valores[3] = nodo.Attributes["Avance"].Value.SafeIntParse();
                                        break;
                                    case "SOP":
                                        valores[4] = nodo.Attributes["Avance"].Value.SafeIntParse();
                                        break;
                                    case "TH":
                                        valores[5] = nodo.Attributes["Avance"].Value.SafeIntParse();
                                        break;
                                }
                            }

                            for (int i = 0; i < lista.Count; i++)
                            {
                                fila.AppendChild(UtileriasExcel.CreaCeldaEntero(valores[i], renglon, columna++));
                            }
                        }

                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDReparaciones")) fila.AppendChild(UtileriasExcel.CreaCeldaEntero(jta.Field<int>("AgrupadoresSpoolPNDReparaciones"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDPendientes")) fila.AppendChild(UtileriasExcel.CreaCeldaEntero(jta.Field<int>("AgrupadoresSpoolPNDPendientes"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDReportes")) fila.AppendChild(UtileriasExcel.CreaCeldaEntero(jta.Field<int>("AgrupadoresSpoolPNDReportes"), renglon, columna++));
                        if (NombreCampoSeguimientoSpool == null || NombreCampoSeguimientoSpool.Contains("AgrupadoresSpoolPNDJuntasSeguimiento")) fila.AppendChild(UtileriasExcel.CreaCeldaEntero(jta.Field<int>("AgrupadoresSpoolPNDJuntasSeguimiento"), renglon, columna++));
                        
                        //-----------------------------------------------AGRUPADORES PND ----------------------------------


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
