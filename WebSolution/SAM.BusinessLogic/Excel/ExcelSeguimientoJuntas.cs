using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Grid;
using Mimo.Framework.Extensions;
using ue = SAM.BusinessLogic.Excel.UtileriasExcel;
using msgs = SAM.BusinessLogic.Excel.MsgsSegJunta;
using log4net;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Administracion;
using SAM.Entities;

namespace SAM.BusinessLogic.Excel
{
    public class ExcelSeguimientoJuntas
    {
        private int proyectoIDEtileno = 16;

        private static readonly object _mutex = new object();
        private static ExcelSeguimientoJuntas _instance;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ExcelSeguimientoJuntas));
        public const string todaslasColumnas = "Todas";
        
        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private ExcelSeguimientoJuntas()
        {
        }


        /// <summary>
        /// obtiene la instancia de la clase ExcelEstimacionJuntas
        /// </summary>
        public static ExcelSeguimientoJuntas Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ExcelSeguimientoJuntas();
                    }
                }
                return _instance;
            }
        }

        public byte[] ObtenerExcelPorIDs(int proyectoID, string ordenTrabajo, string numeroControl, bool historialRep, bool embarcados, string columnas, string filtros)
        {
            byte[] archivo = null;
            int? OrdenTrabajoID;
            int? NumeroControlID;
            List<string> NombreCampoSeguimientoJunta = null;

            if (columnas != null)
            {               
                    int[] FiltrosID = columnas.Split(',').ToList().Select(x => x.SafeIntParse()).ToArray();

                    NombreCampoSeguimientoJunta = SeguimientoJuntaBL.Instance.ObtenerNombreCampoSeguimientoJunta(FiltrosID);
                
            }

           

            //Crear el ms sin parámetros para que pueda crecer
            using (MemoryStream ms = new MemoryStream())
            {
                using (
                    SpreadsheetDocument xlsDoc = SpreadsheetDocument.Create(ms,
                                                                            DocumentFormat.OpenXml.
                                                                                SpreadsheetDocumentType.Workbook,
                                                                            true))
                {
                    SheetData sheetData = ue.CreaDocumentoDefault(xlsDoc, msgs.Nombre_Hoja_Excel);
                    List<GrdSeguimientoJunta> lista = new List<GrdSeguimientoJunta>(); 

                    if (!string.IsNullOrEmpty(ordenTrabajo))
                    {
                        OrdenTrabajoID = ordenTrabajo.SafeIntParse();
                    }
                    else
                    {
                        OrdenTrabajoID = null;
                    }
                    if (!string.IsNullOrEmpty(numeroControl))
                    {
                        NumeroControlID = numeroControl.SafeIntParse();
                    }
                    else
                    {
                        NumeroControlID = null;
                    }
                    _logger.Info("inicia query");
                    DataSet ds = SeguimientoJuntaBL.Instance
                                                   .ObtenerDataSetParaSeguimientoJunta( proyectoID,
                                                                                        OrdenTrabajoID,
                                                                                        NumeroControlID,
                                                                                        null,
                                                                                        historialRep);
                    _logger.Info("finaliza query, generando excel");
                    DataView resultados = (DataView)ds.Tables[0].DefaultView;

                    if (filtros != null && filtros != string.Empty)
                    {

                        resultados.RowFilter = filtros;
                    }

                    uint renglon = 1;

                    Row fila = new Row();
                    fila.RowIndex = renglon;

                    uint z = 1;

                    #region encabezados de fila

                    fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Spool, renglon, z++));
                    fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Junta, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralProyecto")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Proyecto, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralOrdenDeTrabajo")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.OrdenDeTrabajo, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralNumeroDeControl")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.NumeroControl, renglon, z++));
                    
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralOrdenTrabajoEspecial")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralOrdenTrabajoEspecial, renglon, z++));
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralNumeroControlEspecial")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralNumeroControlEspecial, renglon, z++));
                    
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralBaston")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Baston, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEstacion")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Estacion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralSegundaFabricacion")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SegundaFabricacion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralTipoJunta")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TipoJunta, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiametro")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Diametro, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DiametroPlano")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.DiametroPlano, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralCedula") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Cedula, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspesor") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Espesor, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralLocalizacion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Localizacion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUltimoProceso") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.UltimoProceso, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralTieneHold") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TieneHold, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesHold") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ObservacionesHold, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("FechaHold") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.FechaHold, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPWHT") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.RequierePWHT, renglon, z++));                    
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CodigoItemCodeMaterial1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ICMat1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DescripcionItemCodeMaterial1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.DescMat1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CodigoItemCodeMaterial2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ICMat2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DescripcionItemCodeMaterial2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.DescMat2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPeqs") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Peqs, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaAcero1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.FamiliaDeAcero1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaMaterial1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.FamiliaDeMaterial1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaAcero2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.FamiliaDeAcero2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaMaterial2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.FamiliaDeMaterial2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPorcentajePnd") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PorcentajePnd, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspecificacion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Especificacion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiamMat1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.DiamMat1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspMat1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.EspMat1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiamMat2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.DiamMat2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspMat2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.EspMat2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFabArea") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.FabArea, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralKgTeoricos") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.KgTeoricos, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PorcentajeArmado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PorcentajeArmado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PorcentajeSoldado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PorcentajeSoldado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPrioridad") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Prioridad, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralIsometrico") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Isometrico, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRevisionSteelgo") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Revision, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRevisionCliente") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.RevisionCliente, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralMaterialPendienteSpool") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.MaterialPendiente, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralMaterialPendienteJunta") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.JuntaMaterialPendiente, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachado1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Despachado1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachado2")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Despachado2, renglon, z++));                    
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachador")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralDespachador, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralCortador")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralCortador, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo1")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Campo1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo2")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Campo2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo3")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Campo3, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo4")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Campo4, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo5")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.Campo5, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionCalidad")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralFechaliberacionCalidad, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionCalidad")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralUsuarioLiberacionCalidad, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionMaterial")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralFechaLiberacionMaterial, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionMaterial")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralUsuarioLiberacionMaterial, renglon, z++));

                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEsRevision")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralEsRevision,renglon,z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionMaterial")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralFechaLiberacionMaterial, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionMaterial")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.GeneralUsuarioLiberacionMaterial, renglon, z++));
                    
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoFecha") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoFecha, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoTaller") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoTaller, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoTubero") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoTubero, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoNumeroUnico1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoNumeroUnico1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroUnico1Pendiente")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.NumeroUnico1Pendiente, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoNumeroUnico2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoNumeroUnico2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroUnico2Pendiente")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.NumeroUnico2Pendiente, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoCedMat1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoCedMat1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoCedMat2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoCedMat2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoSpool1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoSpool1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoSpool2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoSpool2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoEtiquetaMaterial1") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoEtiquetaMaterial1, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoEtiquetaMaterial2") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ArmadoEtiquetaMaterial2, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesArmado")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ObservacionesArmado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoTubero")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AreaTrabajoTubero, renglon, z++));


                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraFecha") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraFecha, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraTaller") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraTaller, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraWps") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraWPS, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraWpsRelleno") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraWPSRelleno, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraProcesoRelleno") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraProcesoRelleno, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraSoldadorRelleno") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraSoldadorRelleno, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraProcesoRaiz") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraProcesoRaiz, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraSoldadorRaiz") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraSoldadorRaiz, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraConsumiblesRelleno") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.SoldaduraConsumiblesRelleno, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesSoldadura")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.ObservacionesSoldadura, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoRaiz")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AreaTrabajoRaiz, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoRelleno")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AreaTrabajoRelleno, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualFecha") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionVisualFecha, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionVisualFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionVisualNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionVisualHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionVisualResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualDefecto")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionVisualDefecto, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualInspector")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionVisualInspector, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalFecha") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionDimensionalFecha, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionDimensionalFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionDimensionalNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionDimensionalHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalResultado")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionDimensionalResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalInspector")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionDimensionalInspector, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresFecha") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionEspesoresFecha, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionEspesoresFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionEspesoresNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionEspesoresHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.InspeccionEspesoresResultado, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaRequisicion")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaHidroFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroNumeroRequisicion")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaHidroNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaPrueba")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaHidroFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaReporte")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaHidroFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroNumeroReporte")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaHidroNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroHoja")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaHidroHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroAprobado")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaHidroAprobado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroPresion")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaHidroPresion, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTCodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTCodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaPrueba") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTDefecto") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTDefecto, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTSector")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTSector, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTCuadrante")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTCuadrante, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTCodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTCodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaPrueba") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTDefecto") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTDefecto, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtCodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTCodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaTratamiento") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTFechaTratamiento, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtGrafica") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTGrafica, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPWHTResultado, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasCodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasCodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaTratamiento") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasFechaTratamiento, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasGrafica") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasGrafica, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoDurezasResultado, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTCodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTCodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaPrueba") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTDefecto") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaRTPostTTDefecto, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTCodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTCodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaPrueba") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTDefecto") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPTPostTTDefecto, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatCodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatCodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaTratamiento") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatFechaTratamiento, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatGrafica") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatGrafica, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.TratamientoPreheatResultado, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTCodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTCodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaPrueba") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTNumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTDefecto") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaUTDefecto, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMIFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMINumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMINumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMICodigoRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMICodigoRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaPrueba") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMIFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMIFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMINumeroReporte") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMINumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIHoja") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMIHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIResultado") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMIResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIDefecto") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaPMIDefecto, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaNumeroRequisicion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaSistema") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaSistema, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaColor") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaColor, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaCodigo") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaCodigo, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaSendBlast") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaFechaSandBlast, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteSendBlast") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaReporteSandBlast, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaPrimarios") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaFechaPrimarios, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReportePrimarios") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaReportePrimarios, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaIntermedios") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaFechaIntermedios, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteIntermedios") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaReporteIntermedios, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaAcabadoVisual") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaFechaAcabadoVisual, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteAcabadoVisual") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaReporteAcabadoVisual, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaAdherencia") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaFechaAdherencia, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteAdherencia") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaReporteAdherencia, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaPullOff") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaFechaPullOff, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReportePullOff") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaReportePullOff, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaLiberado")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PinturaLiberado,renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueEtiqueta") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.EmbarqueEtiqueta, renglon, z++));
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEtiqueta") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.EmbarqueFechaEtiqueta, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaPreparacion") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.EmbarqueFolioPreparacion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEmbarque") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.EmbarqueFechaEmbarque, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaCarga")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.EmbarqueFechaCarga, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueNumeroEmbarque") ) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.EmbarqueNumeroEmbarque, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaRequisicion")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaFechaRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaNumeroRequisicion")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaNumeroRequisicion, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaPrueba")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaFechaPrueba, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaReporte")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaFechaReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaNumeroReporte")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaNumeroReporte, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaHoja")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaHoja, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaResultado")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaResultado, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaDefecto")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaDefecto, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaSector")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaSector, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaCuadrante")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.PruebaNeumaticaCuadrante, renglon, z++));

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSpoolUltimoProceso")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresSpoolUltimoProceso, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresAreaGrupo")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresAreaGrupo, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresKgsGrupo")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresKgsGrupo, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresDiamGrupo")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresDiamGrupo, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPeqGrupo")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresPeqGrupo, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSistemaPinturaFinal")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresSistemaPinturaFinal, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintNoPaint")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresPaintNoPaint, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintNoPaint")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresPaintNoPaint, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintLine")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresPaintLine, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresAreaEQ")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresAreaEQ, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresInoxNoInox")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresInoxNoInox, renglon, z++));
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresClasifInox")) fila.AppendChild(ue.CreaCeldaTextoInline(msgs.AgrupadoresClasifInox, renglon, z++));
                   

                    #endregion

                    sheetData.AppendChild(fila);

                    foreach (DataRowView dvr in resultados)
                    {
                        DataRow jta = dvr.Row;

                        uint columna = 1;

                        fila = new Row();
                        fila.RowIndex = ++renglon;

                        #region datos de fila

                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralSpool"), renglon, columna++));
                        fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralJunta"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralProyecto") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralProyecto"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralOrdenDeTrabajo") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralOrdenDeTrabajo"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralNumeroDeControl") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralNumeroDeControl"), renglon, columna++));
                        
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralOrdenTrabajoEspecial")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralOrdenTrabajoEspecial"), renglon, columna++));
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralNumeroControlEspecial")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralNumeroControlEspecial"), renglon, columna++));
                        
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralBaston")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralBaston"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEstacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralEstacion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralSegundaFabricacion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralSegundaFabricacion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralTipoJunta") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralTipoJunta"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiametro") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralDiametro"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DiametroPlano") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("DiametroPlano"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralCedula") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralCedula"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspesor") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralEspesor"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralLocalizacion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralLocalizacion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUltimoProceso") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralUltimoProceso"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralTieneHold") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralTieneHold")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesHold") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ObservacionesHold"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("FechaHold") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("FechaHold"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPWHT") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralPWHT")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CodigoItemCodeMaterial1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("CodigoItemCodeMaterial1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DescripcionItemCodeMaterial1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("DescripcionItemCodeMaterial1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CodigoItemCodeMaterial2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("CodigoItemCodeMaterial2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DescripcionItemCodeMaterial2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("DescripcionItemCodeMaterial2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPeqs") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralPeqs"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaAcero1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFamiliaAcero1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaMaterial1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFamiliaMaterial1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaAcero2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFamiliaAcero2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaMaterial2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFamiliaMaterial2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspecificacion") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("GeneralPorcentajePnd"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspecificacion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralEspecificacion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiamMat1") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralDiamMat1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspMat1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralEspMat1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiamMat2") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralDiamMat2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspMat2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralEspMat2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFabArea") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralFabArea"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralKgTeoricos") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("GeneralKgTeoricos"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PorcentajeArmado") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("PorcentajeArmado"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PorcentajeSoldado") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<decimal?>("PorcentajeSoldado"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPrioridad") ) fila.AppendChild(UtileriasExcel.CreaCeldaEntero(jta.Field<int>("GeneralPrioridad"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralIsometrico") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralIsometrico"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRevisionSteelgo") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralRevisionSteelgo"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRevisionCliente") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralRevisionCliente"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralMaterialPendienteSpool") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralMaterialPendienteSpool")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralMaterialPendienteJunta") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralMaterialPendienteJunta")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachado1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralDespachado1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachado2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralDespachado2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo1")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo2")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo3")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo3"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo4")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo4"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo5")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("Campo5"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionCalidad")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("GeneralFechaLiberacionCalidad"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionCalidad")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralUsuarioLiberacionCalidad"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionMaterial")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("GeneralFechaLiberacionMaterial"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionMaterial")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralUsuarioLiberacionMaterial"), renglon, columna++));

                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEsRevision")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralEsRevision")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionMaterial")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("GeneralFechaLiberacionMaterial"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionMaterial")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("GeneralUsuarioLiberacionMaterial"), renglon, columna++));

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoFecha") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("ArmadoFecha"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("ArmadoFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoTaller") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoTaller"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoTubero") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoTubero"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoNumeroUnico1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoNumeroUnico1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroUnico1Pendiente")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("NumeroUnico1Pendiente")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoNumeroUnico2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoNumeroUnico2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroUnico2Pendiente")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("NumeroUnico2Pendiente")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoCedMat1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoCedMat1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoCedMat2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoCedMat2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoSpool1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoSpool1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoSpool2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoSpool2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoEtiquetaMaterial1") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoEtiquetaMaterial1"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoEtiquetaMaterial2") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ArmadoEtiquetaMaterial2"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesArmado")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ObservacionesArmado"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoTubero")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AreaTrabajoTubero"), renglon, columna++));


                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraFecha") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("SoldaduraFecha"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("SoldaduraFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraTaller") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("SoldaduraTaller"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraWps") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("SoldaduraWPS"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraWpsRelleno") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("SoldaduraWPSRelleno"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraProcesoRelleno") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("SoldaduraProcesoRelleno"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraSoldadorRelleno") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("SoldaduraSoldadorRelleno"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraProcesoRaiz") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("SoldaduraProcesoRaiz"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraSoldadorRaiz") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("SoldaduraSoldadorRaiz"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraConsumiblesRelleno") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("SoldaduraConsumiblesRelleno"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesSoldadura")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("ObservacionesSoldadura"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoRaiz")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AreaTrabajoRaiz"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoRelleno")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AreaTrabajoRelleno"), renglon, columna++));

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualFecha") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionVisualFecha"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionVisualFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionVisualNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("InspeccionVisualHoja"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("InspeccionVisualResultado") == null ? false : jta.Field<bool>("InspeccionVisualResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualDefecto")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionVisualDefecto"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualInspector")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionVisualInspector"), renglon, columna++));

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalFecha") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionDimensionalFecha"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionDimensionalFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionDimensionalNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalHoja")) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("InspeccionDimensionalHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalInspector")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionDimensionalInspector")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("InspeccionDimensionalResultado") == null ? false : jta.Field<bool>("InspeccionDimensionalResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresFecha") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionEspesoresFecha"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("InspeccionEspesoresFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("InspeccionEspesoresNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("InspeccionEspesoresHoja")/* == null ? 0 : jta.InspeccionEspesoresHoja.Value*/, renglon, columna++));
                        
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaHidroFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroNumeroRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaHidroNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaPrueba")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaHidroFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaHidroFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroNumeroReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaHidroNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroHoja")) fila.AppendChild(UtileriasExcel.CreaCeldaEntero(jta.Field<int?>("PruebaHidroHoja"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroAprobado")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaHidroAprobado"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroPresion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaHidroPresion"), renglon, columna++));
                        
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("InspeccionEspesoresResultado") == null ? false : jta.Field<bool>("InspeccionEspesoresResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaRTFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTCodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTCodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaPrueba") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaRTFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaRTFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("PruebaRTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaRTResultado") == null ? false : jta.Field<bool>("PruebaRTResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTDefecto") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTDefecto"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTSector")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTSector"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTCuadrante")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTCuadrante"), renglon, columna++));

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPTFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPTNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTCodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPTCodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaPrueba") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPTFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPTFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPTNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("PruebaPTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaPTResultado") == null ? false : jta.Field<bool>("PruebaPTResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTDefecto") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPTDefecto"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientoPwhtFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientoPwhtNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtCodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientoPwhtCodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaTratamiento") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientoPwhtFechaTratamiento"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientoPwhtFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientoPwhtNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("TratamientoPwhtHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtGrafica") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientoPwhtGrafica"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("TratamientoPwhtResultado") == null ? false : jta.Field<bool>("TratamientoPwhtResultado")), renglon, columna++));

                        #region 2

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientoDurezasFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientoDurezasNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasCodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientoDurezasCodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaTratamiento") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientoDurezasFechaTratamiento"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientoDurezasFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientoDurezasNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("TratamientoDurezasHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasGrafica") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientoDurezasGrafica"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("TratamientoDurezasResultado") == null ? false : jta.Field<bool>("TratamientoDurezasResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaRTPostTTFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTPostTTNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTCodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTPostTTCodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaPrueba") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaRTPostTTFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaRTPostTTFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTPostTTNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("PruebaRTPostTTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaRTPostTTResultado") == null ? false : jta.Field<bool>("PruebaRTPostTTResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTDefecto") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaRTPostTTDefecto"), renglon, columna++));

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPTPostTTFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPTPostTTNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTCodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPTPostTTCodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaPrueba") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPTPostTTFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPTPostTTFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPTPostTTNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("PruebaPTPostTTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                       //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaPTPostTTResultado") == null ? false : jta.Field<bool>("PruebaPTPostTTResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTDefecto") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPTPostTTDefecto"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientopreheatFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientopreheatNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatCodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientopreheatCodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaTratamiento") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientopreheatFechaTratamiento"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("TratamientopreheatFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientopreheatNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("TratamientopreheatHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatGrafica") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("TratamientopreheatGrafica"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("TratamientopreheatResultado") == null ? false : jta.Field<bool>("TratamientopreheatResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaUTFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaUTNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTCodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaUTCodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaPrueba") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaUTFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaUTFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTNumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaUTNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("PruebaUTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaUTResultado") == null ? false : jta.Field<bool>("PruebaUTResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTDefecto") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaUTDefecto"), renglon, columna++));

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPMIFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMINumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPMINumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMICodigoRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPMICodigoRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaPrueba") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPMIFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaPMIFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMINumeroReporte") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPMINumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIHoja") ) fila.AppendChild(UtileriasExcel.CreaCeldaNumeroDecimalDosDecimales(jta.Field<int?>("PruebaPMIHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, renglon, columna++));
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIResultado") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaPMIResultado") == null ? false : jta.Field<bool>("PruebaPMIResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIDefecto") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaPMIDefecto"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaNumeroRequisicion") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaSistema") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaSistema"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaColor") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaColor"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaCodigo") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaCodigo"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaSendBlast") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaSendBlast"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteSendBlast") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReporteSendBlast"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaPrimarios") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaPrimarios"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReportePrimarios") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReportePrimarios"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaIntermedios") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaIntermedios"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteIntermedios") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReporteIntermedios"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaAcabadoVisual") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaAcabadoVisual"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteAcabadoVisual") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReporteAcabadoVisual"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaAdherencia") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaAdherencia"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteAdherencia") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReporteAdherencia"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaPullOff") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PinturaFechaPullOff"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReportePullOff") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PinturaReportePullOff"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaLiberado")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PinturaLiberado") == null ? false : jta.Field<bool>("PinturaLiberado")), renglon, columna++));
                        
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueEtiqueta") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("EmbarqueEtiqueta"), renglon, columna++));
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEtiqueta") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("EmbarqueFechaEtiqueta"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaPreparacion") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("EmbarqueFechaPreparacion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEstimada")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("EmbarqueFechaEstimada"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEmbarque") ) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("EmbarqueFechaEmbarque"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueNumeroEmbarque") ) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("EmbarqueNumeroEmbarque"), renglon, columna++));

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaNeumaticaFechaRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaNumeroRequisicion")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaNumeroRequisicion"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaPrueba")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaNeumaticaFechaPrueba"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaFecha(jta.Field<DateTime?>("PruebaNeumaticaFechaReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaNumeroReporte")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaNeumaticaNumeroReporte"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaHoja")) fila.AppendChild(UtileriasExcel.CreaCeldaEntero(jta.Field<int?>("PruebaNeumaticaHoja"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaResultado")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaNeumaticaResultado") == null ? false : jta.Field<bool>("PruebaNeumaticaResultado")), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaDefecto")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaNeumaticaDefecto"), renglon, columna ++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaSector")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaNeumaticaSector"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaCuadrante")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("PruebaNeumaticaCuadrante"), renglon, columna));

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSpoolUltimoProceso")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresSpoolUltimoProceso"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresAreaGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresAreaGrupo"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresKgsGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresKgsGrupo"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresDiamGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresDiamGrupo"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPeqGrupo")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresPeqGrupo"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSistemaPinturaFinal")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresSistemaPinturaFinal"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintNoPaint")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresPaintNoPaint"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresDiametroPromedio")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresDiametroPromedio"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintLine")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresPaintLine"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresAreaEQ")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresAreaEQ"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresInoxNoInox")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresInoxNoInox"), renglon, columna++));
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresClasifInox")) fila.AppendChild(UtileriasExcel.CreaCeldaTexto(jta.Field<string>("AgrupadoresClasifInox"), renglon, columna++));

      

                        #endregion

                        #endregion

                        sheetData.AppendChild(fila);

                    }

                    xlsDoc.Close();
                }

                archivo = ms.ToArray();
                ms.Close();
            }
            _logger.Info("excel generado");
            return archivo;

        }

        public byte[] ObtenerCsvPorIDs(int proyectoID, string ordenTrabajo, string numeroControl, bool historialRep, bool embarcados, string columnas, string filtros)
        {
            byte[] archivo = null;

            int? OrdenTrabajoID;
            int? NumeroControlID;
            List<string> NombreCampoSeguimientoJunta = null;

            if (columnas != null)
            {               
                    int[] FiltrosID = columnas.Split(',').ToList().Select(x => x.SafeIntParse()).ToArray();
                    NombreCampoSeguimientoJunta = SeguimientoJuntaBL.Instance.ObtenerNombreCampoSeguimientoJunta(FiltrosID);                           
            }

            //Crear el ms sin parámetros para que pueda crecer
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter csvDoc = new StreamWriter(ms, Encoding.Default))
                {
                    List<GrdSeguimientoJunta> lista = new List<GrdSeguimientoJunta>();

                    if (!string.IsNullOrEmpty(ordenTrabajo))
                    {
                        OrdenTrabajoID = ordenTrabajo.SafeIntParse();
                    }
                    else
                    {
                        OrdenTrabajoID = null;
                    }
                    if (!string.IsNullOrEmpty(numeroControl))
                    {
                        NumeroControlID = numeroControl.SafeIntParse();
                    }
                    else
                    {
                        NumeroControlID = null;
                    }

                    _logger.Info("inicia query");
                    DataSet ds = SeguimientoJuntaBL.Instance
                                                   .ObtenerDataSetParaSeguimientoJunta(proyectoID,
                                                                                       OrdenTrabajoID,
                                                                                       NumeroControlID,
                                                                                       null,
                                                                                       historialRep);
                    _logger.Info("finaliza query, generando csv");

                    DataView resultados = (DataView)ds.Tables[0].DefaultView;
                    if (filtros != null && filtros != string.Empty)
                    {

                        resultados.RowFilter = filtros;
                    }

                    StringBuilder fila = new StringBuilder();
                    #region encabezados de fila

                    UtileriasCsv.InsertaComa(msgs.Spool, fila);
                    UtileriasCsv.InsertaComa(msgs.Junta, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralProyecto")) UtileriasCsv.InsertaComa(msgs.Proyecto, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralOrdenDeTrabajo")) UtileriasCsv.InsertaComa(msgs.OrdenDeTrabajo, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralNumeroDeControl")) UtileriasCsv.InsertaComa(msgs.NumeroControl, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaEmision")) UtileriasCsv.InsertaComa(msgs.FechaEmision, fila);

                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralOrdenTrabajoEspecial")) UtileriasCsv.InsertaComa(msgs.GeneralOrdenTrabajoEspecial, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralNumeroControlEspecial")) UtileriasCsv.InsertaComa(msgs.GeneralNumeroControlEspecial, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralBaston")) UtileriasCsv.InsertaComa(msgs.Baston, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEstacion")) UtileriasCsv.InsertaComa(msgs.Estacion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralSegundaFabricacion")) UtileriasCsv.InsertaComa(msgs.SegundaFabricacion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralTipoJunta")) UtileriasCsv.InsertaComa(msgs.TipoJunta, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiametro")) UtileriasCsv.InsertaComa(msgs.Diametro, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiametroMayor")) UtileriasCsv.InsertaComa(msgs.DiametroMayor, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DiametroPlano")) UtileriasCsv.InsertaComa(msgs.DiametroPlano, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralCedula")) UtileriasCsv.InsertaComa(msgs.Cedula, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspesor")) UtileriasCsv.InsertaComa(msgs.Espesor, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralLocalizacion")) UtileriasCsv.InsertaComa(msgs.Localizacion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUltimaLocalizacion")) UtileriasCsv.InsertaComa(msgs.UltimaLocalizacion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLocalizacion")) UtileriasCsv.InsertaComa(msgs.GeneralFechaLocalizacion, fila);  
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUltimoProceso")) UtileriasCsv.InsertaComa(msgs.UltimoProceso, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralTieneHold")) UtileriasCsv.InsertaComa(msgs.TieneHold, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesHold")) UtileriasCsv.InsertaComa(msgs.ObservacionesHold, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("FechaHold")) UtileriasCsv.InsertaComa(msgs.FechaHold, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPWHT")) UtileriasCsv.InsertaComa(msgs.RequierePWHT, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRequierePruebaHidrostatica")) UtileriasCsv.InsertaComa(msgs.GeneralRequierePruebaHidrostatica, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralJuntaRequierePwht")) UtileriasCsv.InsertaComa(msgs.GeneralJuntaRequirePwht, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralJuntaRequierePruebaNeumatica")) UtileriasCsv.InsertaComa(msgs.GeneralJuntaRequirePruebaNeumatica, fila);
                    
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CodigoItemCodeMaterial1")) UtileriasCsv.InsertaComa(msgs.ICMat1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DescripcionItemCodeMaterial1")) UtileriasCsv.InsertaComa(msgs.DescMat1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CodigoItemCodeMaterial2")) UtileriasCsv.InsertaComa(msgs.ICMat2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DescripcionItemCodeMaterial2")) UtileriasCsv.InsertaComa(msgs.DescMat2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPeqs")) UtileriasCsv.InsertaComa(msgs.Peqs, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaAcero1")) UtileriasCsv.InsertaComa(msgs.FamiliaDeAcero1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaMaterial1")) UtileriasCsv.InsertaComa(msgs.FamiliaDeMaterial1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaAcero2")) UtileriasCsv.InsertaComa(msgs.FamiliaDeAcero2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaMaterial2")) UtileriasCsv.InsertaComa(msgs.FamiliaDeMaterial2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPorcentajePnd")) UtileriasCsv.InsertaComa(msgs.PorcentajePnd, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspecificacion")) UtileriasCsv.InsertaComa(msgs.Especificacion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiamMat1")) UtileriasCsv.InsertaComa(msgs.DiamMat1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspMat1")) UtileriasCsv.InsertaComa(msgs.EspMat1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiamMat2")) UtileriasCsv.InsertaComa(msgs.DiamMat2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspMat2")) UtileriasCsv.InsertaComa(msgs.EspMat2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFabArea")) UtileriasCsv.InsertaComa(msgs.FabArea, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralKgTeoricos")) UtileriasCsv.InsertaComa(msgs.KgTeoricos, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PorcentajeArmado")) UtileriasCsv.InsertaComa(msgs.PorcentajeArmado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PorcentajeSoldado")) UtileriasCsv.InsertaComa(msgs.PorcentajeSoldado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPrioridad")) UtileriasCsv.InsertaComa(msgs.Prioridad, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralIsometrico")) UtileriasCsv.InsertaComa(msgs.Isometrico, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRevisionSteelgo")) UtileriasCsv.InsertaComa(msgs.Revision, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRevisionCliente")) UtileriasCsv.InsertaComa(msgs.RevisionCliente, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralMaterialPendienteSpool")) UtileriasCsv.InsertaComa(msgs.MaterialPendiente, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralMaterialPendienteJunta")) UtileriasCsv.InsertaComa(msgs.JuntaMaterialPendiente, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachado1")) UtileriasCsv.InsertaComa(msgs.Despachado1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachado2")) UtileriasCsv.InsertaComa(msgs.Despachado2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachador")) UtileriasCsv.InsertaComa(msgs.GeneralDespachador, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralCortador")) UtileriasCsv.InsertaComa(msgs.GeneralCortador, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo1")) UtileriasCsv.InsertaComa(msgs.Campo1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo2")) UtileriasCsv.InsertaComa(msgs.Campo2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo3")) UtileriasCsv.InsertaComa(msgs.Campo3, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo4")) UtileriasCsv.InsertaComa(msgs.Campo4, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo5")) UtileriasCsv.InsertaComa(msgs.Campo5, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("FabClas")) UtileriasCsv.InsertaComa(msgs.FabClas, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CampoJunta2")) UtileriasCsv.InsertaComa(msgs.CampoJunta2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CampoJunta3")) UtileriasCsv.InsertaComa(msgs.CampoJunta3, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CampoJunta4")) UtileriasCsv.InsertaComa(msgs.CampoJunta4, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CampoJunta5")) UtileriasCsv.InsertaComa(msgs.CampoJunta5, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionCalidad")) UtileriasCsv.InsertaComa(msgs.GeneralFechaliberacionCalidad, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionCalidad")) UtileriasCsv.InsertaComa(msgs.GeneralUsuarioLiberacionCalidad, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionMaterial")) UtileriasCsv.InsertaComa(msgs.GeneralFechaLiberacionMaterial, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionMaterial")) UtileriasCsv.InsertaComa(msgs.GeneralUsuarioLiberacionMaterial, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEsRevision")) UtileriasCsv.InsertaComa(msgs.GeneralEsRevision, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaOkPnd")) UtileriasCsv.InsertaComa(msgs.GeneralFechaokPnd, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioOkPnd")) UtileriasCsv.InsertaComa(msgs.GeneralUsuarioOkPnd, fila);
                    

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoFecha")) UtileriasCsv.InsertaComa(msgs.ArmadoFecha, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoFechaReporte")) UtileriasCsv.InsertaComa(msgs.ArmadoFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoTaller")) UtileriasCsv.InsertaComa(msgs.ArmadoTaller, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoTubero")) UtileriasCsv.InsertaComa(msgs.ArmadoTubero, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoNumeroUnico1")) UtileriasCsv.InsertaComa(msgs.ArmadoNumeroUnico1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroUnico1Pendiente")) UtileriasCsv.InsertaComa(msgs.NumeroUnico1Pendiente, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoNumeroUnico2")) UtileriasCsv.InsertaComa(msgs.ArmadoNumeroUnico2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroUnico2Pendiente")) UtileriasCsv.InsertaComa(msgs.NumeroUnico2Pendiente, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoCedMat1")) UtileriasCsv.InsertaComa(msgs.ArmadoCedMat1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoCedMat2")) UtileriasCsv.InsertaComa(msgs.ArmadoCedMat2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoSpool1")) UtileriasCsv.InsertaComa(msgs.ArmadoSpool1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoSpool2")) UtileriasCsv.InsertaComa(msgs.ArmadoSpool2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoEtiquetaMaterial1")) UtileriasCsv.InsertaComa(msgs.ArmadoEtiquetaMaterial1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoEtiquetaMaterial2")) UtileriasCsv.InsertaComa(msgs.ArmadoEtiquetaMaterial2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesArmado")) UtileriasCsv.InsertaComa(msgs.ObservacionesArmado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoTubero")) UtileriasCsv.InsertaComa(msgs.AreaTrabajoTubero, fila);


                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraFecha")) UtileriasCsv.InsertaComa(msgs.SoldaduraFecha, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraFechaReporte")) UtileriasCsv.InsertaComa(msgs.SoldaduraFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraTaller")) UtileriasCsv.InsertaComa(msgs.SoldaduraTaller, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraWps")) UtileriasCsv.InsertaComa(msgs.SoldaduraWPS, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraWpsRelleno")) UtileriasCsv.InsertaComa(msgs.SoldaduraWPSRelleno, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraProcesoRelleno")) UtileriasCsv.InsertaComa(msgs.SoldaduraProcesoRelleno, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraSoldadorRelleno")) UtileriasCsv.InsertaComa(msgs.SoldaduraSoldadorRelleno, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraProcesoRaiz")) UtileriasCsv.InsertaComa(msgs.SoldaduraProcesoRaiz, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraSoldadorRaiz")) UtileriasCsv.InsertaComa(msgs.SoldaduraSoldadorRaiz, fila);
                    if (proyectoID != proyectoIDEtileno) { if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraConsumiblesRelleno")) UtileriasCsv.InsertaComa(msgs.SoldaduraConsumiblesRelleno, fila); }
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesSoldadura")) UtileriasCsv.InsertaComa(msgs.ObservacionesSoldadura, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoRaiz")) UtileriasCsv.InsertaComa(msgs.AreaTrabajoRaiz, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoRelleno")) UtileriasCsv.InsertaComa(msgs.AreaTrabajoRelleno, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualFecha")) UtileriasCsv.InsertaComa(msgs.InspeccionVisualFecha, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualFechaReporte")) UtileriasCsv.InsertaComa(msgs.InspeccionVisualFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualNumeroReporte")) UtileriasCsv.InsertaComa(msgs.InspeccionVisualNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualHoja")) UtileriasCsv.InsertaComa(msgs.InspeccionVisualHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualResultado")) UtileriasCsv.InsertaComa(msgs.InspeccionVisualResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualDefecto")) UtileriasCsv.InsertaComa(msgs.InspeccionVisualDefecto, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualInspector")) UtileriasCsv.InsertaComa(msgs.InspeccionVisualInspector, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalFecha")) UtileriasCsv.InsertaComa(msgs.InspeccionDimensionalFecha, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalFechaReporte")) UtileriasCsv.InsertaComa(msgs.InspeccionDimensionalFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalNumeroReporte")) UtileriasCsv.InsertaComa(msgs.InspeccionDimensionalNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalHoja")) UtileriasCsv.InsertaComa(msgs.InspeccionDimensionalHoja, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalResultado")) UtileriasCsv.InsertaComa(msgs.InspeccionDimensionalResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalInspector")) UtileriasCsv.InsertaComaTextoPlano(msgs.InspeccionDimensionalInspector, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresFecha")) UtileriasCsv.InsertaComa(msgs.InspeccionEspesoresFecha, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresFechaReporte")) UtileriasCsv.InsertaComa(msgs.InspeccionEspesoresFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresNumeroReporte")) UtileriasCsv.InsertaComa(msgs.InspeccionEspesoresNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresHoja")) UtileriasCsv.InsertaComa(msgs.InspeccionEspesoresHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresResultado")) UtileriasCsv.InsertaComa(msgs.InspeccionEspesoresResultado, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaHidroFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaHidroNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaPrueba")) UtileriasCsv.InsertaComa(msgs.PruebaHidroFechaPrueba, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaReporte")) UtileriasCsv.InsertaComa(msgs.PruebaHidroFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroNumeroReporte")) UtileriasCsv.InsertaComa(msgs.PruebaHidroNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroHoja")) UtileriasCsv.InsertaComa(msgs.PruebaHidroHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroAprobado")) UtileriasCsv.InsertaComa(msgs.PruebaHidroAprobado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroPresion")) UtileriasCsv.InsertaComa(msgs.PruebaHidroPresion, fila);
                                    
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaRTFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaRTNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTCodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaRTCodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaPrueba")) UtileriasCsv.InsertaComa(msgs.PruebaRTFechaPrueba, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaReporte")) UtileriasCsv.InsertaComa(msgs.PruebaRTFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTNumeroReporte")) UtileriasCsv.InsertaComa(msgs.PruebaRTNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTHoja")) UtileriasCsv.InsertaComa(msgs.PruebaRTHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTResultado")) UtileriasCsv.InsertaComa(msgs.PruebaRTResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTDefecto")) UtileriasCsv.InsertaComa(msgs.PruebaRTDefecto, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTSector")) UtileriasCsv.InsertaComa(msgs.PruebaRTSector, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTCuadrante")) UtileriasCsv.InsertaComa(msgs.PruebaRTCuadrante, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTJuntaSeguimiento1")) UtileriasCsv.InsertaComa(msgs.PruebaRTJuntaSeguimiento1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTJuntaSeguimiento2")) UtileriasCsv.InsertaComa(msgs.PruebaRTJuntaSeguimiento2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTReferenciaSeguimiento")) UtileriasCsv.InsertaComa(msgs.PruebaRTReferenciaSeguimiento, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPTFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPTNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTCodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPTCodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaPrueba")) UtileriasCsv.InsertaComa(msgs.PruebaPTFechaPrueba, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaReporte")) UtileriasCsv.InsertaComa(msgs.PruebaPTFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTNumeroReporte")) UtileriasCsv.InsertaComa(msgs.PruebaPTNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTHoja")) UtileriasCsv.InsertaComa(msgs.PruebaPTHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTResultado")) UtileriasCsv.InsertaComa(msgs.PruebaPTResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTDefecto")) UtileriasCsv.InsertaComa(msgs.PruebaPTDefecto, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtCodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTCodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaTratamiento")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTFechaTratamiento, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaReporte")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtNumeroReporte")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtHoja")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtGrafica")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTGrafica, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtResultado")) UtileriasCsv.InsertaComa(msgs.TratamientoPWHTResultado, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasCodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasCodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaTratamiento")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasFechaTratamiento, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaReporte")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasNumeroReporte")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasHoja")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasGrafica")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasGrafica, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasResultado")) UtileriasCsv.InsertaComa(msgs.TratamientoDurezasResultado, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTCodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTCodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaPrueba")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTFechaPrueba, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaReporte")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTNumeroReporte")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTHoja")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTResultado")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTDefecto")) UtileriasCsv.InsertaComa(msgs.PruebaRTPostTTDefecto, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTCodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTCodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaPrueba")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTFechaPrueba, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaReporte")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTNumeroReporte")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTHoja")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTResultado")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTDefecto")) UtileriasCsv.InsertaComa(msgs.PruebaPTPostTTDefecto, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatCodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatCodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaTratamiento")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatFechaTratamiento, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaReporte")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatNumeroReporte")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatHoja")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatGrafica")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatGrafica, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatResultado")) UtileriasCsv.InsertaComa(msgs.TratamientoPreheatResultado, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaUTFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaUTNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTCodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaUTCodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaPrueba")) UtileriasCsv.InsertaComa(msgs.PruebaUTFechaPrueba, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaReporte")) UtileriasCsv.InsertaComa(msgs.PruebaUTFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTNumeroReporte")) UtileriasCsv.InsertaComa(msgs.PruebaUTNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTHoja")) UtileriasCsv.InsertaComa(msgs.PruebaUTHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTResultado")) UtileriasCsv.InsertaComa(msgs.PruebaUTResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTDefecto")) UtileriasCsv.InsertaComa(msgs.PruebaUTDefecto, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPMIFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMINumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPMINumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMICodigoRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaPMICodigoRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaPrueba")) UtileriasCsv.InsertaComa(msgs.PruebaPMIFechaPrueba, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaReporte")) UtileriasCsv.InsertaComa(msgs.PruebaPMIFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMINumeroReporte")) UtileriasCsv.InsertaComa(msgs.PruebaPMINumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIHoja")) UtileriasCsv.InsertaComa(msgs.PruebaPMIHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIResultado")) UtileriasCsv.InsertaComa(msgs.PruebaPMIResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIDefecto")) UtileriasCsv.InsertaComa(msgs.PruebaPMIDefecto, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PinturaFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PinturaNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaSistema")) UtileriasCsv.InsertaComa(msgs.PinturaSistema, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaColor")) UtileriasCsv.InsertaComa(msgs.PinturaColor, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaCodigo")) UtileriasCsv.InsertaComa(msgs.PinturaCodigo, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaSendBlast")) UtileriasCsv.InsertaComa(msgs.PinturaFechaSandBlast, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteSendBlast")) UtileriasCsv.InsertaComa(msgs.PinturaReporteSandBlast, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaPrimarios")) UtileriasCsv.InsertaComa(msgs.PinturaFechaPrimarios, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReportePrimarios")) UtileriasCsv.InsertaComa(msgs.PinturaReportePrimarios, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaIntermedios")) UtileriasCsv.InsertaComa(msgs.PinturaFechaIntermedios, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteIntermedios")) UtileriasCsv.InsertaComa(msgs.PinturaReporteIntermedios, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaAcabadoVisual")) UtileriasCsv.InsertaComa(msgs.PinturaFechaAcabadoVisual, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteAcabadoVisual")) UtileriasCsv.InsertaComa(msgs.PinturaReporteAcabadoVisual, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaAdherencia")) UtileriasCsv.InsertaComa(msgs.PinturaFechaAdherencia, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteAdherencia")) UtileriasCsv.InsertaComa(msgs.PinturaReporteAdherencia, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaPullOff")) UtileriasCsv.InsertaComa(msgs.PinturaFechaPullOff, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReportePullOff")) UtileriasCsv.InsertaComa(msgs.PinturaReportePullOff, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaLiberado")) UtileriasCsv.InsertaComa(msgs.PinturaLiberado, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueEtiqueta")) UtileriasCsv.InsertaComa(msgs.EmbarqueEtiqueta, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEtiqueta")) UtileriasCsv.InsertaComa(msgs.EmbarqueFechaEtiqueta, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFolioPreparacion")) UtileriasCsv.InsertaComaTextoPlano(msgs.EmbarqueFolioPreparacion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEstimada")) UtileriasCsv.InsertaComa(msgs.EmbarqueFechaCarga, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEmbarque")) UtileriasCsv.InsertaComa(msgs.EmbarqueFechaEmbarque, fila);                   

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueNumeroEmbarque")) UtileriasCsv.InsertaComa(msgs.EmbarqueNumeroEmbarque, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota1")) UtileriasCsv.InsertaComa(msgs.Nota1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota2")) UtileriasCsv.InsertaComa(msgs.Nota2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota3")) UtileriasCsv.InsertaComa(msgs.Nota3, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota4")) UtileriasCsv.InsertaComa(msgs.Nota4, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota5")) UtileriasCsv.InsertaComa(msgs.Nota5, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueVigenciaAduana")) UtileriasCsv.InsertaComa(msgs.EmbarqueVigenciaAduana, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaFechaRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaNumeroRequisicion")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaNumeroRequisicion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaPrueba")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaFechaPrueba, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaReporte")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaFechaReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaNumeroReporte")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaNumeroReporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaHoja")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaHoja, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaResultado")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaResultado, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaDefecto")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaDefecto, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaSector")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaSector, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaCuadrante")) UtileriasCsv.InsertaComa(msgs.PruebaNeumaticaCuadrante, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSpoolUltimoProceso")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSpoolUltimoProceso, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresAreaGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresAreaGrupo, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresKgsGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresKgsGrupo, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresDiamGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresDiamGrupo, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPeqGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPeqGrupo, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSistemaPinturaFinal")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSistemaPinturaFinal, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintNoPaint")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPaintNoPaint, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresDiametroPromedio")) UtileriasCsv.InsertaComa(msgs.AgrupadoresDiametroPromedio, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintLine")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPaintLine, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresAreaEQ")) UtileriasCsv.InsertaComa(msgs.AgrupadoresAreaEQ, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresInoxNoInox")) UtileriasCsv.InsertaComa(msgs.AgrupadoresInoxNoInox, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresClasifInox")) UtileriasCsv.InsertaComa(msgs.AgrupadoresClasifInox, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaClasifPND")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPorJuntaClasifPND, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaClasifReparacion")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPorJuntaClasifReparacion, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaClasifSoporte")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPorJuntaClasifSoporte, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo1")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPorJuntaGrupo1, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo2")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPorJuntaGrupo2, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo3")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPorJuntaGrupo3, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo4")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPorJuntaGrupo4, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo5")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPorJuntaGrupo5, fila);

                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroTransferencia")) UtileriasCsv.InsertaComa(msgs.NumeroTransferencia, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PreparacionTransferencia")) UtileriasCsv.InsertaComa(msgs.PreparacionTransferencia, fila);
                    if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Transferencia")) UtileriasCsv.InsertaComa(msgs.Transferencia, fila);

                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDUltimoProceso")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDUltimoProceso, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDAreaGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDAreaGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDKgsGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDKgsGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDDiamGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDDiamGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDPEQGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDPEQGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDSistemaPinturaFinal")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDSistemaPinturaFinal, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDPaintNoPaint")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDPaintNoPaint, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDDiametroPromedio")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDDiametroPromedio, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDPaintLine")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDPaintLine, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDAreaEQ")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDAreaEQ, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDInoxNoInox")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDInoxNoInox, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDClasifInox")) UtileriasCsv.InsertaComa(msgs.AgrupadoresPNDClasifInox, fila);

                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopUltimoProceso")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopUltimoProceso, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopAreaGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopAreaGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopKgsGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopKgsGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopDiamGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopDiamGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopPEQGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopPEQGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopSistemaPinturaFinal")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopSistemaPinturaFinal, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopPaintNoPaint")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopPaintNoPaint, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopDiametroPromedio")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopDiametroPromedio, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopPaintLine")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopPaintLine, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopAreaEQ")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopAreaEQ, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopInoxNoInox")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopInoxNoInox, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopClasifInox")) UtileriasCsv.InsertaComa(msgs.AgrupadoresSopClasifInox, fila);

                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepUltimoProceso")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepUltimoProceso, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepAreaGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepAreaGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepKgsGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepKgsGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepDiamGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepDiamGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepPEQGrupo")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepPEQGrupo, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepSistemaPinturaFinal")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepSistemaPinturaFinal, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepPaintNoPaint")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepPaintNoPaint, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepDiametroPromedio")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepDiametroPromedio, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepPaintLine")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepPaintLine, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepAreaEQ")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepAreaEQ, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepInoxNoInox")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepInoxNoInox, fila);
                    //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepClasifInox")) UtileriasCsv.InsertaComa(msgs.AgrupadoresRepClasifInox, fila);
                    
                    


                    #endregion

                    csvDoc.WriteLine(fila.ToString());

                    // se supone que el for es más rápido que el foreach
                    for (int i = 0; i < resultados.Count; i++)
                    {
                        DataRow jta = resultados[i].Row;

                        fila = new StringBuilder();

                        #region datos de fila
                        UtileriasCsv.InsertaComa(jta.Field<string>("GeneralSpool"), fila);
                        UtileriasCsv.InsertaComa(jta.Field<string>("GeneralJunta"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralProyecto")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralProyecto"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralOrdenDeTrabajo")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralOrdenDeTrabajo"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralNumeroDeControl")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralNumeroDeControl"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaEmision")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("GeneralFechaEmision"), fila);


                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralOrdenTrabajoEspecial")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralOrdenTrabajoEspecial"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralNumeroControlEspecial")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralNumeroControlEspecial"), fila);


                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralBaston")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralBaston"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEstacion")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralEstacion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralSegundaFabricacion")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralSegundaFabricacion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralTipoJunta")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralTipoJunta"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiametro")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("GeneralDiametro"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiametroMayor")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("GeneralDiametroMayor"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DiametroPlano")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("DiametroPlano"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralCedula")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralCedula"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspesor")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("GeneralEspesor"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralLocalizacion")) UtileriasCsv.InsertaComaTextoPlano(jta.Field<string>("GeneralLocalizacion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUltimaLocalizacion")) UtileriasCsv.InsertaComaTextoPlano(jta.Field<string>("GeneralUltimaLocalizacion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLocalizacion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("GeneralFechaLocalizacion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUltimoProceso")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralUltimoProceso"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralTieneHold")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralTieneHold")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesHold")) UtileriasCsv.InsertaComa(jta.Field<string>("ObservacionesHold"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("FechaHold")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("FechaHold"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPWHT")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralPWHT")), fila);
                        
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRequierePruebaHidrostatica")) UtileriasCsv.InsertaComa(jta.Field<bool?>("GeneralRequierePruebaHidrostatica") != null ?
                            TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("GeneralRequierePruebaHidrostatica").SafeBoolParse()) : "", fila);
                        
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralJuntaRequierePwht")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralJuntaRequierePwht"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralJuntaRequierePruebaNeumatica")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralJuntaRequierePruebaNeumatica"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CodigoItemCodeMaterial1")) UtileriasCsv.InsertaComa(jta.Field<string>("CodigoItemCodeMaterial1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DescripcionItemCodeMaterial1")) UtileriasCsv.InsertaComa(jta.Field<string>("DescripcionItemCodeMaterial1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CodigoItemCodeMaterial2")) UtileriasCsv.InsertaComa(jta.Field<string>("CodigoItemCodeMaterial2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("DescripcionItemCodeMaterial2")) UtileriasCsv.InsertaComa(jta.Field<string>("DescripcionItemCodeMaterial2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPeqs")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("GeneralPeqs"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaAcero1")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralFamiliaAcero1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaMaterial1")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralFamiliaMaterial1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaAcero2")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralFamiliaAcero2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFamiliaMaterial2")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralFamiliaMaterial2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPorcentajePnd")) UtileriasCsv.InsertaComa(jta.Field<int?>("GeneralPorcentajePnd"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspecificacion")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralEspecificacion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiamMat1")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("GeneralDiamMat1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspMat1")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralEspMat1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDiamMat2")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("GeneralDiamMat2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralEspMat2")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralEspMat2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFabArea")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralFabArea"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralKgTeoricos")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("GeneralKgTeoricos"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PorcentajeArmado")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("PorcentajeArmado"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PorcentajeSoldado")) UtileriasCsv.InsertaComa(jta.Field<decimal?>("PorcentajeSoldado"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralPrioridad")) UtileriasCsv.InsertaComa(jta.Field<int>("GeneralPrioridad").ToString(), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralIsometrico")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralIsometrico"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRevisionSteelgo")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralRevisionSteelgo"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralRevisionCliente")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralRevisionCliente"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralMaterialPendienteSpool")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralMaterialPendienteSpool")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralMaterialPendienteJunta")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("GeneralMaterialPendienteJunta")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachado1")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralDespachado1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachado2")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralDespachado2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralDespachador")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralDespachador"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralCortador")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralCortador"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo1")) UtileriasCsv.InsertaComa(jta.Field<string>("Campo1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo2")) UtileriasCsv.InsertaComa(jta.Field<string>("Campo2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo3")) UtileriasCsv.InsertaComa(jta.Field<string>("Campo3"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo4")) UtileriasCsv.InsertaComa(jta.Field<string>("Campo4"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Campo5")) UtileriasCsv.InsertaComa(jta.Field<string>("Campo5"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("FabClas")) UtileriasCsv.InsertaComa(jta.Field<string>("FabClas"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CampoJunta2")) UtileriasCsv.InsertaComa(jta.Field<string>("CampoJunta2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CampoJunta3")) UtileriasCsv.InsertaComa(jta.Field<string>("CampoJunta3"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CampoJunta4")) UtileriasCsv.InsertaComa(jta.Field<string>("CampoJunta4"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("CampoJunta5")) UtileriasCsv.InsertaComa(jta.Field<string>("CampoJunta5"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionCalidad")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("GeneralFechaLiberacionCalidad"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionCalidad")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralUsuarioLiberacionCalidad"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaLiberacionMaterial")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("GeneralFechaLiberacionMaterial"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioLiberacionMaterial")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralUsuarioLiberacionMaterial"), fila);               
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralFechaOkPnd")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("GeneralFechaOkPnd"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("GeneralUsuarioOkPnd")) UtileriasCsv.InsertaComa(jta.Field<string>("GeneralUsuarioOkPnd"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoFecha")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("ArmadoFecha"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("ArmadoFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoTaller")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoTaller"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoTubero")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoTubero"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoNumeroUnico1")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoNumeroUnico1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroUnico1Pendiente")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("NumeroUnico1Pendiente")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoNumeroUnico2")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoNumeroUnico2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroUnico2Pendiente")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool>("NumeroUnico2Pendiente")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoCedMat1")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoCedMat1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoCedMat2")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoCedMat2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoSpool1")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoSpool1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoSpool2")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoSpool2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoEtiquetaMaterial1")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoEtiquetaMaterial1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ArmadoEtiquetaMaterial2")) UtileriasCsv.InsertaComa(jta.Field<string>("ArmadoEtiquetaMaterial2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesArmado")) UtileriasCsv.InsertaComa(jta.Field<string>("ObservacionesArmado"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoTubero")) UtileriasCsv.InsertaComa(jta.Field<string>("AreaTrabajoTubero"), fila);


                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraFecha")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("SoldaduraFecha"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("SoldaduraFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraTaller")) UtileriasCsv.InsertaComa(jta.Field<string>("SoldaduraTaller"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraWps")) UtileriasCsv.InsertaComa(jta.Field<string>("SoldaduraWPS"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraWpsRelleno")) UtileriasCsv.InsertaComa(jta.Field<string>("SoldaduraWPSRelleno"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraProcesoRelleno")) UtileriasCsv.InsertaComa(jta.Field<string>("SoldaduraProcesoRelleno"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraSoldadorRelleno")) UtileriasCsv.InsertaComa(jta.Field<string>("SoldaduraSoldadorRelleno"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraProcesoRaiz")) UtileriasCsv.InsertaComa(jta.Field<string>("SoldaduraProcesoRaiz"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraSoldadorRaiz")) UtileriasCsv.InsertaComa(jta.Field<string>("SoldaduraSoldadorRaiz"), fila);
                        if (proyectoID != proyectoIDEtileno) { if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("SoldaduraConsumiblesRelleno")) UtileriasCsv.InsertaComa(jta.Field<string>("SoldaduraConsumiblesRelleno"), fila); }
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("ObservacionesSoldadura")) UtileriasCsv.InsertaComa(jta.Field<string>("ObservacionesSoldadura"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoRaiz")) UtileriasCsv.InsertaComa(jta.Field<string>("AreaTrabajoRaiz"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AreaTrabajoRelleno")) UtileriasCsv.InsertaComa(jta.Field<string>("AreaTrabajoRelleno"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualFecha")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("InspeccionVisualFecha"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("InspeccionVisualFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("InspeccionVisualNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("InspeccionVisualHoja"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("InspeccionVisualResultado") == null ? false : jta.Field<bool>("InspeccionVisualResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualDefecto")) UtileriasCsv.InsertaComa(jta.Field<string>("InspeccionVisualDefecto"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionVisualInspector")) UtileriasCsv.InsertaComa(jta.Field<string>("InspeccionVisualInspector"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalFecha")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("InspeccionDimensionalFecha"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("InspeccionDimensionalFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("InspeccionDimensionalNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("InspeccionDimensionalHoja"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalInspector")) UtileriasCsv.InsertaComaTextoPlano(jta.Field<string>("InspeccionDimensionalInspector"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionDimensionalResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("InspeccionDimensionalResultado") == null ? false : jta.Field<bool>("InspeccionDimensionalResultado")), fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresFecha")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("InspeccionEspesoresFecha"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("InspeccionEspesoresFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("InspeccionEspesoresNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("InspeccionEspesoresHoja")/* == null ? 0 : jta.InspeccionEspesoresHoja.Value*/, fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaHidroFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaHidroNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaPrueba")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaHidroFechaPrueba"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaHidroFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaHidroNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("PruebaHidroHoja"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroAprobado")) UtileriasCsv.InsertaComa(jta.Field<bool?>("PruebaHidroAprobado"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaHidroPresion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaHidroPresion"), fila);
                        
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("InspeccionEspesoresResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("InspeccionEspesoresResultado") == null ? false : jta.Field<bool>("InspeccionEspesoresResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaRTFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTCodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTCodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaPrueba")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaRTFechaPrueba"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaRTFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("PruebaRTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaRTResultado") == null ? false : jta.Field<bool>("PruebaRTResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTDefecto")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTDefecto"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTSector")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTSector"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTCuadrante")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTCuadrante"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTJuntaSeguimiento1")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTJuntaSeguimiento1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTJuntaSeguimiento2")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTJuntaSeguimiento2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTReferenciaSeguimiento")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTReferenciaSeguimiento"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPTFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPTNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTCodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPTCodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaPrueba")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPTFechaPrueba"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPTFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPTNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("PruebaPTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaPTResultado") == null ? false : jta.Field<bool>("PruebaPTResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTDefecto")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPTDefecto"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientoPwhtFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientoPwhtNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtCodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientoPwhtCodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaTratamiento")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientoPwhtFechaTratamiento"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientoPwhtFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientoPwhtNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("TratamientoPwhtHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtGrafica")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientoPwhtGrafica"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPwhtResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("TratamientoPwhtResultado") == null ? false : jta.Field<bool>("TratamientoPwhtResultado")), fila);

                        #region 2

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientoDurezasFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientoDurezasNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasCodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientoDurezasCodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaTratamiento")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientoDurezasFechaTratamiento"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientoDurezasFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientoDurezasNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("TratamientoDurezasHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasGrafica")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientoDurezasGrafica"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoDurezasResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("TratamientoDurezasResultado") == null ? false : jta.Field<bool>("TratamientoDurezasResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaRTPostTTFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTPostTTNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTCodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTPostTTCodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaPrueba")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaRTPostTTFechaPrueba"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaRTPostTTFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTPostTTNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("PruebaRTPostTTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaRTPostTTResultado") == null ? false : jta.Field<bool>("PruebaRTPostTTResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaRTPostTTDefecto")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaRTPostTTDefecto"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPTPostTTFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPTPostTTNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTCodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPTPostTTCodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaPrueba")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPTPostTTFechaPrueba"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPTPostTTFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPTPostTTNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("PruebaPTPostTTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaPTPostTTResultado") == null ? false : jta.Field<bool>("PruebaPTPostTTResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPTPostTTDefecto")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPTPostTTDefecto"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientopreheatFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientopreheatNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatCodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientopreheatCodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaTratamiento")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientopreheatFechaTratamiento"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("TratamientopreheatFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientopreheatNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("TratamientopreheatHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatGrafica")) UtileriasCsv.InsertaComa(jta.Field<string>("TratamientopreheatGrafica"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("TratamientoPreHeatResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("TratamientopreheatResultado") == null ? false : jta.Field<bool>("TratamientopreheatResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaUTFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaUTNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTCodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaUTCodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaPrueba")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaUTFechaPrueba"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaUTFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaUTNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("PruebaUTHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaUTResultado") == null ? false : jta.Field<bool>("PruebaUTResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaUTDefecto")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaUTDefecto"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPMIFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMINumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPMINumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMICodigoRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPMICodigoRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaPrueba")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPMIFechaPrueba"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaPMIFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMINumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPMINumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("PruebaPMIHoja")/* == null ? 0 : jta.InspeccionDimensionalHoja.Value*/, fila);
                        //verificar
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaPMIResultado") == null ? false : jta.Field<bool>("PruebaPMIResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaPMIDefecto")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaPMIDefecto"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PinturaFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaSistema")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaSistema"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaColor")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaColor"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaCodigo")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaCodigo"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaSendBlast")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PinturaFechaSendBlast"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteSendBlast")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaReporteSendBlast"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaPrimarios")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PinturaFechaPrimarios"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReportePrimarios")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaReportePrimarios"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaIntermedios")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PinturaFechaIntermedios"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteIntermedios")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaReporteIntermedios"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaAcabadoVisual")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PinturaFechaAcabadoVisual"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteAcabadoVisual")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaReporteAcabadoVisual"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaAdherencia")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PinturaFechaAdherencia"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReporteAdherencia")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaReporteAdherencia"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaFechaPullOff")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PinturaFechaPullOff"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaReportePullOff")) UtileriasCsv.InsertaComa(jta.Field<string>("PinturaReportePullOff"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PinturaLiberado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PinturaLiberado") == null ? false : jta.Field<bool>("PinturaLiberado")), fila);
                        
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueEtiqueta")) UtileriasCsv.InsertaComa(jta.Field<string>("EmbarqueEtiqueta"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEtiqueta")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("EmbarqueFechaEtiqueta"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFolioPreparacion")) UtileriasCsv.InsertaComaTextoPlano(jta.Field<string>("EmbarqueFolioPreparacion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEstimada")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("EmbarqueFechaEstimada"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueFechaEmbarque")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("EmbarqueFechaEmbarque"), fila); 
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueNumeroEmbarque")) UtileriasCsv.InsertaComa(jta.Field<string>("EmbarqueNumeroEmbarque"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota1")) UtileriasCsv.InsertaComa(jta.Field<string>("Nota1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota2")) UtileriasCsv.InsertaComa(jta.Field<string>("Nota2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota3")) UtileriasCsv.InsertaComa(jta.Field<string>("Nota3"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota4")) UtileriasCsv.InsertaComa(jta.Field<string>("Nota4"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Nota5")) UtileriasCsv.InsertaComa(jta.Field<string>("Nota5"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("EmbarqueVigenciaAduana")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("EmbarqueVigenciaAduana"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaRequisicion")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaNeumaticaFechaRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaNumeroRequisicion")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaNeumaticaNumeroRequisicion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaPrueba")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaNeumaticaFechaPrueba"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaFechaReporte")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PruebaNeumaticaFechaReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaNumeroReporte")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaNeumaticaNumeroReporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaHoja")) UtileriasCsv.InsertaComa(jta.Field<int?>("PruebaNeumaticaHoja"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaResultado")) UtileriasCsv.InsertaComa(TraductorEnumeraciones.TextoSiNo(jta.Field<bool?>("PruebaNeumaticaResultado") == null ? false : jta.Field<bool>("PruebaNeumaticaResultado")), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaDefecto")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaNeumaticaDefecto"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaSector")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaNeumaticaSector"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PruebaNeumaticaCuadrante")) UtileriasCsv.InsertaComa(jta.Field<string>("PruebaNeumaticaCuadrante"), fila);


                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSpoolUltimoProceso")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSpoolUltimoProceso"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresAreaGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresAreaGrupo"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresKgsGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresKgsGrupo"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresDiamGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresDiamGrupo"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPeqGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPeqGrupo"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSistemaPinturaFinal")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSistemaPinturaFinal"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintNoPaint")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPaintNoPaint"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresDiametroPromedio")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresDiametroPromedio"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPaintLine")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPaintLine"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresAreaEQ")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresAreaEQ"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresInoxNoInox")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresInoxNoInox"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresClasifInox")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresClasifInox"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaClasifPND")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPorJuntaClasifPND"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaClasifReparacion")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPorJuntaClasifReparacion"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaClasifSoporte")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPorJuntaClasifSoporte"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo1")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPorJuntaGrupo1"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo2")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPorJuntaGrupo2"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo3")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPorJuntaGrupo3"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo4")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPorJuntaGrupo4"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPorJuntaGrupo5")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPorJuntaGrupo5"), fila);

                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("NumeroTransferencia")) UtileriasCsv.InsertaComa(jta.Field<string>("NumeroTransferencia"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("PreparacionTransferencia")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("PreparacionTransferencia"), fila);
                        if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("Transferencia")) UtileriasCsv.InsertaComaFecha(jta.Field<DateTime?>("Transferencia"), fila);

                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDUltimoProceso")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDUltimoProceso"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDAreaGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDAreaGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDKgsGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDKgsGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDDiamGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDDiamGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDPEQGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDPEQGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDSistemaPinturaFinal")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDSistemaPinturaFinal"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDPaintNoPaint")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDPaintNoPaint"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDDiametroPromedio")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDDiametroPromedio"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDPaintLine")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDPaintLine"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDAreaEQ")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDAreaEQ"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDInoxNoInox")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDInoxNoInox"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresPNDClasifInox")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresPNDClasifInox"), fila);

                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopUltimoProceso")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopUltimoProceso"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopAreaGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopAreaGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopKgsGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopKgsGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopDiamGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopDiamGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopPEQGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopPEQGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopSistemaPinturaFinal")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopSistemaPinturaFinal"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopPaintNoPaint")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopPaintNoPaint"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopDiametroPromedio")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopDiametroPromedio"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopPaintLine")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopPaintLine"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopAreaEQ")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopAreaEQ"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopInoxNoInox")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopInoxNoInox"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresSopClasifInox")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresSopClasifInox"), fila);

                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepUltimoProceso")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepUltimoProceso"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepAreaGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepAreaGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepKgsGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepKgsGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepDiamGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepDiamGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepPEQGrupo")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepPEQGrupo"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepSistemaPinturaFinal")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepSistemaPinturaFinal"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepPaintNoPaint")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepPaintNoPaint"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepDiametroPromedio")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepDiametroPromedio"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepPaintLine")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepPaintLine"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepAreaEQ")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepAreaEQ"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepInoxNoInox")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepInoxNoInox"), fila);
                        //if (NombreCampoSeguimientoJunta == null || NombreCampoSeguimientoJunta.Contains("AgrupadoresRepClasifInox")) UtileriasCsv.InsertaComa(jta.Field<string>("AgrupadoresRepClasifInox"), fila);



            
                                            
                        #endregion

                        #endregion

                        csvDoc.WriteLine(fila.ToString());
                    }

                    csvDoc.Close();
                }
             
                archivo = GeneraZip(ms.ToArray());

                ms.Close();
            }

            return archivo;
        }

        private byte[] GeneraZip(byte[] archivo)
        {
             string nombreArchivo = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "WorkstatusWeld.csv" : "SeguimientoJuntas.csv"; 
             string rutaCsvGenerado = System.Configuration.ConfigurationManager.AppSettings["Sam.BusinessLogic.Csv"]; 
             string rutaDirectorioUnico = rutaCsvGenerado + Guid.NewGuid().ToString() + @"\"; 


            #region Generamos Csv
            byte[] archivoCsv;

            // Creamos directorio único 
             DirectoryInfo di = Directory.CreateDirectory(rutaDirectorioUnico); 
             string rutaCompleta = rutaDirectorioUnico + nombreArchivo; 


            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(rutaCompleta))
            {
                // Use FileStream object to write to the specified file
                fileStream.Write(archivo, 0, archivo.Length);

                archivoCsv = new byte[fileStream.Length];
                fileStream.Read(archivoCsv, 0, archivoCsv.Length);
            }
            #endregion

            #region Generamos Zip
            string nombreArchivoZip = LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "WorkstatusWeld.zip" : "SeguimientoJuntas.zip"; 
 
            string rutaCompletaCsv = rutaDirectorioUnico + nombreArchivo; 
            string rutaCompletaZip = rutaDirectorioUnico + nombreArchivoZip; 

            FileStream fileStreamIn = new FileStream(rutaCompletaCsv, FileMode.Open, FileAccess.Read);
            FileStream fileStreamOut = new FileStream(rutaCompletaZip, FileMode.Create, FileAccess.ReadWrite);

            ZipOutputStream zipOutStream = new ZipOutputStream(fileStreamOut);
            zipOutStream.SetLevel(1);
            byte[] buffer = new byte[archivoCsv.Length];

            ZipEntry entry = new ZipEntry(Path.GetFileName(rutaCompletaCsv));
            zipOutStream.PutNextEntry(entry);
            int size;
            do
            {
                size = fileStreamIn.Read(buffer, 0, buffer.Length);
                zipOutStream.Write(buffer, 0, size);
            } while (size > 0);

            zipOutStream.Close();
            fileStreamOut.Close();
            fileStreamIn.Close();

            FileStream fileZipStream = new FileStream(rutaCompletaZip, FileMode.Open, FileAccess.Read);
            byte[] archivoZip = new byte[fileZipStream.Length];
            fileZipStream.Read(archivoZip, 0, archivoZip.Length);
            fileZipStream.Close();
            #endregion

            return archivoZip;
        }


    }
}
