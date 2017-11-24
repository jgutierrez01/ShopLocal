using System.IO;
using System.Linq;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using SAM.Common;
using SAM.Entities;
using SAM.Entities.Cache;
using System.Collections.Generic;
using System;
using System.Transactions;
using SAM.BusinessLogic.Utilerias;
using SAM.BusinessObjects.Catalogos;


namespace SAM.BusinessLogic.Workstatus
{
    public class ReportesBL
    {
        private static readonly object _mutex = new object();
        private static ReportesBL _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private ReportesBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase ReportesBL
        /// </summary>
        public static ReportesBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReportesBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// verifica si existe el archivo
        /// </summary>
        /// <param name="proyectoID">obtiene la carpeta del proyecto</param>
        /// <param name="tipoReporte">verifica que tipo de reporte es</param>
        /// <param name="reporteID">obtiene el nombre del archivo</param>
        /// <param name="tipoPrueba">Obtiene la carpeta donde estara el archivo</param>
        /// <param name="numeroReporte"> regresa el nombre del archivo</param>
        /// <returns>true si encuentra el archivo en su ruta correspondiente</returns>
        public bool ReporteExisteEnFileSystem(int reporteID, int proyectoID, TipoReporte? tipoReporte, int? tipoPrueba, out string numeroReporte)
        {
            string rutaBase;

            if (tipoReporte.Value == TipoReporte.RequisicionSpool)
            {
                rutaBase = obtenerRutaFisica(proyectoID, tipoReporte, reporteID, ObtenerPruebaSpool(tipoPrueba), out numeroReporte);
            }
            else
            {
                rutaBase = obtenerRutaFisica(proyectoID, tipoReporte, reporteID, ObtenerPrueba(tipoPrueba), out numeroReporte);
            }

            return File.Exists(rutaBase);
        }

        public bool ReporteExisteEnFileSystem(string numeroReporte, int proyectoID, TipoReporte? tipoReporte, int? tipoPrueba)
        {
            string rutaBase;

            if (tipoReporte.Value == TipoReporte.ReporteSpoolPND)
            {
                rutaBase = obtenerRutaFisica(proyectoID, tipoReporte, numeroReporte, ObtenerPruebaSpool(tipoPrueba));
            }
            else
            {
                rutaBase = obtenerRutaFisica(proyectoID, tipoReporte, numeroReporte, ObtenerPrueba(tipoPrueba));
            }

            return File.Exists(rutaBase);
        }

        public bool ReportePinturaExisteEnFileSystem(string numeroReporte, int proyectoID, int tipoPintura)
        {
            string rutaBase = obtenerRutaFisica(proyectoID, numeroReporte, (TipoPinturaEnum)tipoPintura);
            return File.Exists(rutaBase);
        }

        /// <summary>
        /// Obtiene la ruta donde esta el archivo a buscar
        /// </summary>
        /// <param name="proyectoID">obtiene la carpeta del proyecto</param>
        /// <param name="tipoReporte">verifica que tipo de reporte es</param>
        /// <param name="reporteID">obtiene el nombre del archivo</param>
        /// <param name="tipo">Obtiene la carpeta donde estara el archivo</param>
        /// <param name="numeroReporte"> regresa el nombre del archivo</param>
        /// <returns>string con la ruta del archivo</returns>
        private static string obtenerRutaFisica(int proyectoID, TipoReporte? tipoReporte, int reporteID, TipoPruebaSpoolEnum? tipo, out string numeroReporte)
        {
            numeroReporte = string.Empty;

            switch (tipoReporte)
            {
                case TipoReporte.RequisicionSpool:
                    RequisicionSpool reporteRequisicionSpool = RequisicionBO.Instance.ObtenerReqSpool(reporteID);
                    numeroReporte = reporteRequisicionSpool.NumeroRequisicion;
                    break;
                case TipoReporte.ReporteSpoolPND:
                    ReporteSpoolPnd reporteSpoolPnd = ReportePndBO.Instance.ObtenerReporteSpool(reporteID);
                    numeroReporte = reporteSpoolPnd.NumeroReporte;
                    break;

                default:
                    break;
            }
            if (!numeroReporte.EndsWith(".pdf"))
            {
                numeroReporte += ".pdf";
            }
            return obtenerRutaFisica(proyectoID, tipoReporte, numeroReporte, tipo);
        }

        /// <summary>
        /// Obtiene la ruta donde esta el archivo a buscar
        /// </summary>
        /// <param name="proyectoID">obtiene la carpeta del proyecto</param>
        /// <param name="tipoReporte">verifica que tipo de reporte es</param>
        /// <param name="reporteID">obtiene el nombre del archivo</param>
        /// <param name="tipo">Obtiene la carpeta donde estara el archivo</param>
        /// <param name="numeroReporte"> regresa el nombre del archivo</param>
        /// <returns>string con la ruta del archivo</returns>
        private static string obtenerRutaFisica(int proyectoID, TipoReporte? tipoReporte, int reporteID, TipoPruebaEnum? tipo, out string numeroReporte)
        {
            numeroReporte = string.Empty;

            switch (tipoReporte)
            {
                case TipoReporte.ReportePND:
                    ReportePnd reportePnd = ReportePndBO.Instance.Obtener(reporteID);
                    numeroReporte = reportePnd.NumeroReporte;
                    break;

                case TipoReporte.ReporteTT:
                    ReporteTt reporteTt = ReporteTtBO.Instance.Obtener(reporteID);
                    numeroReporte = reporteTt.NumeroReporte;
                    break;

                case TipoReporte.RequisicionPintura:
                    RequisicionPintura reporteReqPintura = ReporteRequisicionPinturaBO.Instance.Obtener(reporteID);
                    numeroReporte = reporteReqPintura.NumeroRequisicion;
                    break;

                case TipoReporte.InspeccionVisual:
                    InspeccionVisual reporteInspeccion = InspeccionVisualBO.Instance.Obtener(reporteID);
                    numeroReporte = reporteInspeccion.NumeroReporte;
                    break;

                case TipoReporte.ReporteDimensional:
                    ReporteDimensional reporteDimensional = ReporteDimensionalBO.Instance.Obtener(reporteID);
                    numeroReporte = reporteDimensional.NumeroReporte;
                    break;

                case TipoReporte.ReporteEspesores:
                    ReporteDimensional reporteEspesores = ReporteDimensionalBO.Instance.Obtener(reporteID);
                    numeroReporte = reporteEspesores.NumeroReporte;
                    break;

                case TipoReporte.Requisicion:
                    Requisicion reporteRequisicion = RequisicionBO.Instance.Obtener(reporteID);
                    numeroReporte = reporteRequisicion.NumeroRequisicion;
                    break;
                case TipoReporte.RequisicionSpool:
                    RequisicionSpool reporteRequisicionSpool = RequisicionBO.Instance.ObtenerReqSpool(reporteID);
                    numeroReporte = reporteRequisicionSpool.NumeroRequisicion;
                    break;
                case TipoReporte.ReporteSpoolPND:
                    ReporteSpoolPnd reporteSpoolPnd = ReportePndBO.Instance.ObtenerReporteSpool(reporteID);
                    numeroReporte = reporteSpoolPnd.NumeroReporte;
                    break;

                default:
                    break;
            }
            if(!numeroReporte.EndsWith(".pdf"))
            {
                numeroReporte += ".pdf";
            }
            return obtenerRutaFisica(proyectoID, tipoReporte, numeroReporte, tipo);
        }



        private static string obtenerRutaFisica(int proyectoID, TipoReporte? tipoReporte, string numeroReporte, TipoPruebaSpoolEnum? tipo)
        {
            string rutaBase = Configuracion.CalidadRutaDossier;
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == proyectoID).Single();

            if (!numeroReporte.EndsWith(".pdf"))
            {
                numeroReporte += ".pdf";
            }
            switch (tipoReporte)
            {
                case TipoReporte.RequisicionSpool:
                    switch (tipo)
                    {
                        case TipoPruebaSpoolEnum.Hidrostatica:
                            rutaBase += p.Nombre + "/Requisiciones/Hidrostatica/" + numeroReporte;
                            break;
                        default:
                            break;
                    }
                    break;
                case TipoReporte.ReporteSpoolPND:
                    switch (tipo)
                    {
                        case TipoPruebaSpoolEnum.Hidrostatica:
                            rutaBase += p.Nombre + "/Reportes/Hidrostatica/" + numeroReporte;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return rutaBase;
        }

        private static string obtenerRutaFisica(int proyectoID, TipoReporte? tipoReporte, string numeroReporte, TipoPruebaEnum? tipo)
        {
            string rutaBase = Configuracion.CalidadRutaDossier;
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == proyectoID).Single();
            //string s = "";
            if (!numeroReporte.EndsWith(".pdf"))
            {
                numeroReporte += ".pdf";
            }
            switch (tipoReporte)
            {
                case TipoReporte.ReportePND:
                    switch (tipo)
                    {
                        case TipoPruebaEnum.PTPostTT:
                            rutaBase += p.Nombre + "/Reportes/PTPostTT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.ReporteUT:
                            rutaBase += p.Nombre + "/Reportes/UT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.RTPostTT:
                            rutaBase += p.Nombre + "/Reportes/RTPostTT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.ReportePT:
                            rutaBase += p.Nombre + "/Reportes/PT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.ReporteRT:
                            rutaBase += p.Nombre + "/Reportes/RT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.ReportePMI:
                            rutaBase += p.Nombre + "/Reportes/PMI/" + numeroReporte;
                            break;
                        default:
                            break;
                    }
                    break;

                case TipoReporte.ReporteTT:


                    switch (tipo)
                    {
                        case TipoPruebaEnum.Durezas:
                            rutaBase += p.Nombre + "/Reportes/Durezas/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.Preheat:
                            rutaBase += p.Nombre + "/Reportes/Preheat/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.Pwht:
                            rutaBase += p.Nombre + "/Reportes/PWHT/" + numeroReporte;
                            break;
                        default:
                            break;
                    }
                    break;

                case TipoReporte.RequisicionPintura:
                    rutaBase += p.Nombre + "/Requisiciones/Pintura/" + numeroReporte;
                    break;

                case TipoReporte.InspeccionVisual:
                    rutaBase += p.Nombre + "/Reportes/InspeccionVisual/" + numeroReporte;
                    break;

                case TipoReporte.ReporteDimensional:
                    rutaBase += p.Nombre + "/Reportes/LiberacionDimensional/" + numeroReporte;
                    break;

                case TipoReporte.ReporteEspesores:
                    rutaBase += p.Nombre + "/Reportes/Espesores/" + numeroReporte;
                    break;

                case TipoReporte.Requisicion:
                    switch (tipo)
                    {
                        case TipoPruebaEnum.PTPostTT:
                            //s = "PTPostTT/";
                            rutaBase += p.Nombre + "/Requisiciones/PTPostTT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.ReporteUT:
                            //s = "ReporteUT/";
                            rutaBase += p.Nombre + "/Requisiciones/UT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.RTPostTT:
                            rutaBase += p.Nombre + "/Requisiciones/RTPostTT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.ReportePT:
                            rutaBase += p.Nombre + "/Requisiciones/PT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.ReporteRT:
                            rutaBase += p.Nombre + "/Requisiciones/RT/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.Durezas:
                            rutaBase += p.Nombre + "/Requisiciones/Durezas/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.Preheat:
                            rutaBase += p.Nombre + "/Requisiciones/Preheat/" + numeroReporte;
                            break;
                        case TipoPruebaEnum.Pwht:
                            rutaBase += p.Nombre + "/Requisiciones/PWHT/" + numeroReporte;
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            return rutaBase;
        }

        private static string obtenerRutaFisica(int proyectoID, string numeroReporte, TipoPinturaEnum tipo)
        {
            string rutaBase = Configuracion.CalidadRutaDossier;
            ProyectoCache p = CacheCatalogos.Instance.ObtenerProyectos().Where(x => x.ID == proyectoID).Single();
            //string s = "";
            if (numeroReporte != null && !numeroReporte.EndsWith(".pdf"))
            {
                numeroReporte += ".pdf";
            }
            switch (tipo)
            {

                case TipoPinturaEnum.SandBlast:
                    rutaBase += p.Nombre + "\\Reportes\\Pintura\\Sandblast\\" + numeroReporte ;
                    break;
                case TipoPinturaEnum.Primario:
                    rutaBase += p.Nombre + "\\Reportes\\Pintura\\Primarios\\" + numeroReporte ;
                    break;
                case TipoPinturaEnum.Intermedio:
                    rutaBase += p.Nombre + "\\Reportes\\Pintura\\Intermedios\\" + numeroReporte ;
                    break;
                case TipoPinturaEnum.AcabadoVisual:
                    rutaBase += p.Nombre + "\\Reportes\\Pintura\\AcabadoVisual\\" + numeroReporte ;
                    break;
                case TipoPinturaEnum.Adherencia:
                    rutaBase += p.Nombre + "\\Reportes\\Pintura\\Adherencia\\" + numeroReporte ;
                    break;
                case TipoPinturaEnum.PullOff:
                    rutaBase += p.Nombre + "\\Reportes\\Pintura\\PullOff\\" + numeroReporte ;
                    break;
                case TipoPinturaEnum.Digitalizado:
                    rutaBase += p.Nombre + "\\Reportes\\Pintura\\" + numeroReporte;
                    break;

                default:
                    break;
            }

            return rutaBase;
        }


        /// <summary>
        /// manda a llamar el metodo obtenerRutaFisica que genera la ruta del archivo y lo regresa en binario
        /// </summary>
        /// <param name="proyectoID">obtiene la carpeta del proyecto</param>
        /// <param name="tipoReporte">verifica que tipo de reporte es</param>
        /// <param name="reporteID">obtiene el nombre del archivo</param>
        /// <param name="tipoPrueba">Obtiene la carpeta donde estara el archivo</param>
        /// <param name="numeroReporte"> regresa el nombre del archivo</param>
        /// <returns>Byte: regresa una cadena de bytes donde se encuentra el archivo</returns>
        public byte[] ObtenReporteDeFileSystem(int proyectoID, TipoReporte? tipoReporte, int reporteID, int? tipoPrueba, out string numeroReporte)
        {
            string rutaBase = string.Empty;

            if (tipoReporte.Value == TipoReporte.ReporteSpoolPND || tipoReporte.Value == TipoReporte.RequisicionSpool)
            {
                rutaBase = obtenerRutaFisica(proyectoID, tipoReporte, reporteID, ObtenerPruebaSpool(tipoPrueba), out numeroReporte);
            }
            else
            {
                rutaBase = obtenerRutaFisica(proyectoID, tipoReporte, reporteID, ObtenerPrueba(tipoPrueba), out numeroReporte);
            }

            return File.ReadAllBytes(rutaBase);
        }

        public byte[] ObtenReportePinturaDeFileSystem(int proyectoID, TipoPinturaEnum tipoPintura, string numeroReporte)
        {
            string rutaBase = string.Empty;

            rutaBase = obtenerRutaFisica(proyectoID, numeroReporte, tipoPintura);

            return File.ReadAllBytes(rutaBase);
        }


        /// <summary>
        /// trae el tipo de prueba correspondiente de acuerdo a su ID
        /// </summary>
        /// <param name="tipoPrueba">ID del tipo prueba</param>
        /// <returns>el tipo prueba correspondiente</returns>
        public TipoPruebaEnum? ObtenerPrueba(int? tipoPrueba)
        {
            if ((int)TipoPruebaEnum.Durezas == tipoPrueba)
            {
                return TipoPruebaEnum.Durezas;
            }
            else if ((int)TipoPruebaEnum.Preheat == tipoPrueba)
            {
                return TipoPruebaEnum.Preheat;
            }
            else if ((int)TipoPruebaEnum.PTPostTT == tipoPrueba)
            {
                return TipoPruebaEnum.PTPostTT;
            }
            else if ((int)TipoPruebaEnum.Pwht == tipoPrueba)
            {
                return TipoPruebaEnum.Pwht;
            }
            else if ((int)TipoPruebaEnum.ReportePT == tipoPrueba)
            {
                return TipoPruebaEnum.ReportePT;
            }
            else if ((int)TipoPruebaEnum.ReporteRT == tipoPrueba)
            {
                return TipoPruebaEnum.ReporteRT;
            }
            else if ((int)TipoPruebaEnum.ReporteUT == tipoPrueba)
            {
                return TipoPruebaEnum.ReporteUT;
            }
            else if ((int)TipoPruebaEnum.RTPostTT == tipoPrueba)
            {
                return TipoPruebaEnum.RTPostTT;
            }
            else if ((int)TipoPruebaEnum.ReportePMI == tipoPrueba)
            {
                return TipoPruebaEnum.ReportePMI;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// trae el tipo de prueba correspondiente de acuerdo a su ID
        /// </summary>
        /// <param name="tipoPrueba">ID del tipo prueba</param>
        /// <returns>el tipo prueba correspondiente</returns>
        public TipoPruebaSpoolEnum? ObtenerPruebaSpool(int? tipoPruebaSpool)
        {
            if ((int)TipoPruebaSpoolEnum.Hidrostatica == tipoPruebaSpool)
            {
                return TipoPruebaSpoolEnum.Hidrostatica;
            }
            else
            {
                return null;
            }
        }

        public bool GuardaReportePND(ReportePnd reporte, JuntaReportePnd juntaReporte, List<JuntaReportePndSector> sectores, List<JuntaReportePndCuadrante> cuadrantes, string IDs, string RIDs, Guid UserUID)
        {
            bool resultado = true;
            
            using (TransactionScope ts = new TransactionScope())
            {
                Guid responsable;
                string nombreProyecto = string.Empty;
                string pendiente = string.Empty;
                string detalle = string.Empty;

                resultado = ReportePndBO.Instance.GuardaReportePND(reporte, juntaReporte, sectores, cuadrantes, IDs, RIDs, UserUID, out responsable, out nombreProyecto, out pendiente, out detalle);
                
                if (nombreProyecto != string.Empty)
                {
                    EnvioCorreos.Instance.EnviaNotificacionDePendientes(responsable, nombreProyecto, pendiente, detalle);
                }

                ts.Complete();
            }

            return resultado;
        }

        public bool GuardaReporteSpoolPND(ReporteSpoolPnd reporteSpool, SpoolReportePnd spoolReporte, string IDs, string RIDs, Guid UserUID)
        {
            bool resultado = true;

            using (TransactionScope ts = new TransactionScope())
            {
                resultado = ReportePndBO.Instance.GuardaReporteSpoolPND(reporteSpool, spoolReporte, IDs, RIDs, UserUID);

                ts.Complete();
            }

            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaCampoID"></param>
        /// <param name="requisicionCampoID"></param>
        /// <param name="reporte"></param>
        /// <param name="juntaReporte"></param>
        /// <param name="sectores"></param>
        /// <param name="cuadrantes"></param>
        /// <param name="UserUID"></param>
        /// <returns></returns>
        public bool GuardaReporteCampoPND(int juntaCampoID, int requisicionCampoID, ReporteCampoPND reporte, JuntaCampoReportePND juntaReporte, List<JuntaCampoReportePNDSector> sectores, List<JuntaCampoReportePNDCuadrante> cuadrantes, Guid UserUID)
        {
            bool resultado = true;

            using (TransactionScope ts = new TransactionScope())
            {
                Guid responsable;
                string nombreProyecto = string.Empty;
                string pendiente = string.Empty;
                string detalle = string.Empty;

                resultado = JuntaCampoBO.Instance.GuardaReportePND(juntaCampoID, requisicionCampoID, reporte, juntaReporte, sectores, cuadrantes, UserUID, out responsable, out nombreProyecto, out pendiente, out detalle);

                if (nombreProyecto != string.Empty)
                {
                    EnvioCorreos.Instance.EnviaNotificacionDePendientes(responsable, nombreProyecto, pendiente, detalle);
                }

                ts.Complete();
            }

            return resultado;
        }
    }
}
