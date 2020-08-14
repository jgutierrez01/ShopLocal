using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using Mimo.Framework.Common;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Calidad;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Common;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using SAM.BusinessLogic.Excepciones;
using SAM.Web.Classes;
using log4net;
using System.Data.SqlClient;
using SAM.Entities.Personalizadas.Shop;


namespace SAM.BusinessLogic.Calidad
{
    public class CertificacionBL
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CertificacionBL));
        private static readonly object _mutex = new object();
        private static CertificacionBL _instance;

        private CertificacionBL()
        {
        }

        /// <summary>
        /// Patron De Singleton
        /// </summary>
        public static CertificacionBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CertificacionBL();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Propiedad de conveniencia para acceder a Cache
        /// </summary>
        private static Cache cache
        {
            get
            {
                return HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// Obtiene la lista de elementos para contruir el Grid de Certificacion
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<GrdCertificacion> ObtenerParaListadoCertificacion(int proyectoID, int ordenTrabajo, int numeroControl, string numeroEmbarque, string[] segmentos)
        {
            List<GrdCertificacion> listado;

            try
            {

                DataSet ds = new DataSet();
                const string nombreProc = "ListadoCertificacion";
                string[] nombreTablas = new[] { "Proyecto", "Spools", "WorkstatusSpool", "JuntaReportes", "JuntaWorkStatus", "Wps", "MTR", "MTRSoldadura" };

                using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
                {
                    IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
                    parameters[0].Value = proyectoID;
                    parameters[1].Value = ordenTrabajo;
                    parameters[2].Value = numeroControl;
                    parameters[3].Value = numeroEmbarque;
                    parameters[4].Value = segmentos[0];
                    parameters[5].Value = segmentos[1];
                    parameters[6].Value = segmentos[2];
                    parameters[7].Value = segmentos[3];
                    parameters[8].Value = segmentos[4];
                    parameters[9].Value = segmentos[5];
                    parameters[10].Value = segmentos[6];

                    ds = DataAccess.ExecuteDataset(connection,
                                                   CommandType.StoredProcedure,
                                                   nombreProc,
                                                   ds,
                                                   nombreTablas,
                                                   parameters);

                    DataTable proyecto = ds.Tables["Proyecto"];
                    DataTable spools = ds.Tables["Spools"];
                    DataTable wkstatusSpool = ds.Tables["WorkstatusSpool"];
                    DataTable reportes = ds.Tables["JuntaReportes"];
                    DataTable wkstatusJunta = ds.Tables["JuntaWorkStatus"];
                    DataTable wps = ds.Tables["Wps"];
                    DataTable mtr = ds.Tables["MTR"];
                    DataTable mtrSold = ds.Tables["MTRSoldadura"];

                    string nombreProyecto = proyecto.Rows[0].Field<string>("Nombre");

                    DirectoryInfo dir = new DirectoryInfo(Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes");

                    int tipoJuntaTH_ID =
                            CacheCatalogos.Instance.ObtenerTiposJunta().Single(x => x.Nombre.EqualsIgnoreCase("TH")).ID;
                    int tipoJuntaTW_ID =
                        CacheCatalogos.Instance.ObtenerTiposJunta().Single(x => x.Nombre.EqualsIgnoreCase("TW")).ID;
                    int shopFabAreaID =
                        CacheCatalogos.Instance.ObtenerFabAreas().Single(x => x.Nombre.EqualsIgnoreCase("shop")).ID;

                    Dictionary<string, IEnumerable<FileInfo>> fileLists = new Dictionary<string, IEnumerable<FileInfo>>();


                    if (dir.Exists)
                    {
                        //todos menos pintura
                        dir.GetDirectories().ToList().ForEach(
                            directorio =>
                            fileLists.Add(dir.Name + "\\" + directorio.Name,
                                           directorio.GetFiles("*.pdf", SearchOption.AllDirectories).AsParallel()));
                        //pintura
                        dir = new DirectoryInfo(Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes\Pintura");
                        dir.GetDirectories().ToList().ForEach(
                            directorio =>
                            fileLists.Add(@"Pintura\" + directorio.Name,
                                           directorio.GetFiles("*.pdf", SearchOption.AllDirectories).AsParallel()));

                    }


                    Decimal numRegistros = spools.Rows.Count;
                    int cores = Configuracion.CoresProcesador;

                    //Dividimos los registros entre el numero de cores del CPU
                    int rate = Math.Ceiling(numRegistros / cores).SafeIntParse();

                    //En esta lista guardaremos todos los threads que nos ayudaran a juntar los registros que ocupamos
                    List<Thread> threads = new List<Thread>();
                    //Esta es una Coleccion ThreadSafe que permite almacenar los registros que se estan obteniendo
                    ConcurrentBag<GrdCertificacion> certBag = new ConcurrentBag<GrdCertificacion>();


#if DEBUG
                    rate = numRegistros.SafeIntParse();
                    cores = 1;
#endif


                    //para cada nucleo del procesador creamos un thread
                    for (int i = 0; i < cores; i++)
                    {
                        IEnumerable<DataRow> spoolParaEsteCore = spools.Rows.Cast<DataRow>().Skip(i * rate).Take(rate);
                        _logger.Info("inicio generar registros: " + DateTime.Now);
                        threads.Add(new Thread(
                                        () =>
                                        generaRegistros(spoolParaEsteCore, wkstatusJunta,
                                                        wkstatusSpool, reportes, wps, mtr, mtrSold, shopFabAreaID,
                                                        tipoJuntaTH_ID, tipoJuntaTW_ID, fileLists, certBag)));
                    }
                    //Arrancamos todos los threads
                    threads.ForEach(x => x.Start());

                    threads.ForEach(x => x.Join());

                    listado = certBag.ToList();
                    _logger.Info("fin obtener Certificacion: " + DateTime.Now);

                    return listado;
                }

            }
            catch (SqlException ex)
            {
                listado = new List<GrdCertificacion>();
                _logger.Info(ex.Message);
                throw new ExcepcionProcesos(MensajesError.Exception_BaseDeDatos);
            }
            catch (Exception ex)
            {
                listado = new List<GrdCertificacion>();
                _logger.Info(ex.Message);
                throw new ExcepcionProcesos(MensajesError.Exception_GeneracionCertificacion);
            }

            // return listado;
        }

        /// <summary>
        /// Obtiene la lista de elementos para contruir el Grid de Certificacion
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<GrdCertificacion> ObtenerParaListadoCertificacion(int proyectoID, string[] segmentos)
        {
            //List<GrdCertificacion> listado = (List<GrdCertificacion>)cache.Get("lstCertificacion_" + proyectoID);
            //if (listado != null)
            //{
            //    return listado;
            //}

            List<GrdCertificacion> listado;
            DataSet ds = new DataSet();
            const string nombreProc = "ListadoCertificacion";
            string[] nombreTablas = new[] { "Proyecto", "Spools", "WorkstatusSpool", "JuntaReportes", "JuntaWorkStatus", "Wps", "Mtr", "MTRSoldadura" };
            using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
            {
                IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
                parameters[0].Value = proyectoID;
                parameters[3].Value = segmentos[0];
                parameters[4].Value = segmentos[1];
                parameters[5].Value = segmentos[2];
                parameters[6].Value = segmentos[3];
                parameters[7].Value = segmentos[4];
                parameters[8].Value = segmentos[5];
                parameters[9].Value = segmentos[6];

                ds = DataAccess.ExecuteDataset(connection,
                                               CommandType.StoredProcedure,
                                               nombreProc,
                                               ds,
                                               nombreTablas,
                                               parameters);

                DataTable proyecto = ds.Tables["Proyecto"];
                DataTable spools = ds.Tables["Spools"];
                DataTable wkstatusSpool = ds.Tables["WorkstatusSpool"];
                DataTable reportes = ds.Tables["JuntaReportes"];
                DataTable wkstatusJunta = ds.Tables["JuntaWorkStatus"];
                DataTable wps = ds.Tables["Wps"];
                DataTable mtr = ds.Tables["Mtr"];
                DataTable mtrSold = ds.Tables["MTRSoldadura"];

                string nombreProyecto = proyecto.Rows[0].Field<string>("Nombre");

                DirectoryInfo dir = new DirectoryInfo(Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes");

                int tipoJuntaTH_ID =
                        CacheCatalogos.Instance.ObtenerTiposJunta().Single(x => x.Nombre.EqualsIgnoreCase("TH")).ID;
                int tipoJuntaTW_ID =
                    CacheCatalogos.Instance.ObtenerTiposJunta().Single(x => x.Nombre.EqualsIgnoreCase("TW")).ID;
                int shopFabAreaID =
                    CacheCatalogos.Instance.ObtenerFabAreas().Single(x => x.Nombre.EqualsIgnoreCase("shop")).ID;

                Dictionary<string, IEnumerable<FileInfo>> fileLists = new Dictionary<string, IEnumerable<FileInfo>>();


                if (dir.Exists)
                {
                    //todos menos pintura
                    dir.GetDirectories().ToList().ForEach(
                        directorio =>
                        fileLists.Add(dir.Name + "\\" + directorio.Name,
                                       directorio.GetFiles("*.pdf", SearchOption.AllDirectories).AsParallel()));
                    //pintura
                    dir = new DirectoryInfo(Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes\Pintura");
                    dir.GetDirectories().ToList().ForEach(
                        directorio =>
                        fileLists.Add(@"Pintura\" + directorio.Name,
                                       directorio.GetFiles("*.pdf", SearchOption.AllDirectories).AsParallel()));

                }


                Decimal numRegistros = spools.Rows.Count;
                int cores = Configuracion.CoresProcesador;

                //Dividimos los registros entre el numero de cores del CPU
                int rate = Math.Ceiling(numRegistros / cores).SafeIntParse();

                //En esta lista guardaremos todos los threads que nos ayudaran a juntar los registros que ocupamos
                List<Thread> threads = new List<Thread>();
                //Esta es una Coleccion ThreadSafe que permite almacenar los registros que se estan obteniendo
                ConcurrentBag<GrdCertificacion> certBag = new ConcurrentBag<GrdCertificacion>();


#if DEBUG
                rate = numRegistros.SafeIntParse();
                cores = 1;
#endif


                //para cada nucleo del procesador creamos un thread
                for (int i = 0; i < cores; i++)
                {
                    IEnumerable<DataRow> spoolParaEsteCore = spools.Rows.Cast<DataRow>().Skip(i * rate).Take(rate);
                    threads.Add(new Thread(
                                    () =>
                                    generaRegistros(spoolParaEsteCore, wkstatusJunta,
                                                    wkstatusSpool, reportes, wps, mtr, mtrSold, shopFabAreaID,
                                                    tipoJuntaTH_ID, tipoJuntaTW_ID, fileLists, certBag)));
                }
                //Arrancamos todos los threads
                threads.ForEach(x => x.Start());

                threads.ForEach(x => x.Join());

                listado = certBag.ToList();
                //cache.Insert("lstCertificacion_" + proyectoID, listado, null,
                //             DateTime.Now.AddMinutes(Configuracion.CacheMuyPocosMinutos), Cache.NoSlidingExpiration);
                return listado;
            }

        }

        /// <summary>
        /// Obtiene la lista de elementos para contruir el Grid de Certificacion
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public List<GrdCertificacion> ObtenerParaListadoCertificacion(int proyectoID, string seg1, string seg2, string seg3, string seg4, string seg5, string seg6, string seg7)
        {
            List<GrdCertificacion> listado = (List<GrdCertificacion>)cache.Get("lstCertificacion_" + proyectoID);
            if (listado != null)
            {
                return listado;
            }

            DataSet ds = new DataSet();
            const string nombreProc = "ListadoCertificacion";
            string[] nombreTablas = new[] { "Proyecto", "Spools", "WorkstatusSpool", "JuntaReportes", "JuntaWorkStatus", "Wps", "Mtr", "MTRSoldadura" };
            using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
            {
                IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
                parameters[0].Value = proyectoID;
                parameters[1].Value = seg1;
                parameters[2].Value = seg2;
                parameters[3].Value = seg3;
                parameters[4].Value = seg4;
                parameters[5].Value = seg5;
                parameters[6].Value = seg6;
                parameters[7].Value = seg7;

                ds = DataAccess.ExecuteDataset(connection,
                                               CommandType.StoredProcedure,
                                               nombreProc,
                                               ds,
                                               nombreTablas,
                                               parameters);

                DataTable proyecto = ds.Tables["Proyecto"];
                DataTable spools = ds.Tables["Spools"];
                DataTable wkstatusSpool = ds.Tables["WorkstatusSpool"];
                DataTable reportes = ds.Tables["JuntaReportes"];
                DataTable wkstatusJunta = ds.Tables["JuntaWorkStatus"];
                DataTable wps = ds.Tables["Wps"];
                DataTable mtr = ds.Tables["Mtr"];
                DataTable mtrSold = ds.Tables["MTRSoldadura"];

                string nombreProyecto = proyecto.Rows[0].Field<string>("Nombre");

                DirectoryInfo dir = new DirectoryInfo(Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes");

                int tipoJuntaTH_ID =
                        CacheCatalogos.Instance.ObtenerTiposJunta().Single(x => x.Nombre.EqualsIgnoreCase("TH")).ID;
                int tipoJuntaTW_ID =
                    CacheCatalogos.Instance.ObtenerTiposJunta().Single(x => x.Nombre.EqualsIgnoreCase("TW")).ID;
                int shopFabAreaID =
                    CacheCatalogos.Instance.ObtenerFabAreas().Single(x => x.Nombre.EqualsIgnoreCase("shop")).ID;

                Dictionary<string, IEnumerable<FileInfo>> fileLists = new Dictionary<string, IEnumerable<FileInfo>>();


                if (dir.Exists)
                {
                    //todos menos pintura
                    dir.GetDirectories().ToList().ForEach(
                        directorio =>
                        fileLists.Add(dir.Name + "\\" + directorio.Name,
                                       directorio.GetFiles("*.pdf", SearchOption.AllDirectories).AsParallel()));
                    //pintura
                    dir = new DirectoryInfo(Configuracion.CalidadRutaDossier + @"\" + nombreProyecto + @"\Reportes\Pintura");
                    dir.GetDirectories().ToList().ForEach(
                        directorio =>
                        fileLists.Add(@"Pintura\" + directorio.Name,
                                       directorio.GetFiles("*.pdf", SearchOption.AllDirectories).AsParallel()));

                }


                Decimal numRegistros = spools.Rows.Count;
                int cores = Configuracion.CoresProcesador;

                //Dividimos los registros entre el numero de cores del CPU
                int rate = Math.Ceiling(numRegistros / cores).SafeIntParse();

                //En esta lista guardaremos todos los threads que nos ayudaran a juntar los registros que ocupamos
                List<Thread> threads = new List<Thread>();
                //Esta es una Coleccion ThreadSafe que permite almacenar los registros que se estan obteniendo
                ConcurrentBag<GrdCertificacion> certBag = new ConcurrentBag<GrdCertificacion>();


#if DEBUG
                rate = numRegistros.SafeIntParse();
                cores = 1;
#endif


                //para cada nucleo del procesador creamos un thread
                for (int i = 0; i < cores; i++)
                {
                    IEnumerable<DataRow> spoolParaEsteCore = spools.Rows.Cast<DataRow>().Skip(i * rate).Take(rate);
                    threads.Add(new Thread(
                                    () =>
                                    generaRegistros(spoolParaEsteCore, wkstatusJunta,
                                                    wkstatusSpool, reportes, wps, mtr, mtrSold, shopFabAreaID,
                                                    tipoJuntaTH_ID, tipoJuntaTW_ID, fileLists, certBag)));
                }
                //Arrancamos todos los threads
                threads.ForEach(x => x.Start());

                threads.ForEach(x => x.Join());

                listado = certBag.ToList();
                cache.Insert("lstCertificacion_" + proyectoID, listado, null,
                             DateTime.Now.AddMinutes(Configuracion.CacheMuyPocosMinutos), Cache.NoSlidingExpiration);
                return listado;
            }
        }

        /// <summary>
        /// Este metodo se encarga de verificar si existe el archivo enviado en el directorio y agrega a una lista los paths de
        /// los archivos en caso de que todos los archivos buscados sean encontrados
        /// <param name="directorio"></param>
        /// <param name="registros"></param>
        /// <param name="fileList"></param>
        /// <param name="rutaFisicaReportes"></param>
        /// <returns></returns></summary>
        private static bool revisaReportes(string directorio, List<DataRow> registros, IDictionary<string, IEnumerable<FileInfo>> fileList, out List<string> rutaFisicaReportes)
        {
            return revisaReportes(directorio, registros, fileList, out rutaFisicaReportes, string.Empty);
        }


        /// <summary>
        /// Este metodo se encarga de verificar si existe el archivo enviado en el directorio y agrega a una lista los paths de
        /// los archivos en caso de que todos los archivos buscados sean encontrados
        /// </summary>
        /// <param name="directorio"></param>
        /// <param name="registros"></param>
        /// <param name="fileList"></param>
        /// <param name="rutaFisicaReportes"></param>
        /// <param name="reportesAcumulados"></param>
        /// <param name="campo"></param>
        /// <returns></returns>
        private static bool revisaReportes(string directorio, List<DataRow> registros, IDictionary<string, IEnumerable<FileInfo>> fileList, out List<string> rutaFisicaReportes, string campo)
        {
            //el campo por default donde buscamos es Numero reporte
            if (string.IsNullOrEmpty(campo))
            {
                campo = "NumeroReporte";
            }
            rutaFisicaReportes = new List<string>();
            if (directorio.StartsWith("\\"))
            {
                directorio = directorio.Substring(1);
            }
            string[] tmp = directorio.Split('\\').Reverse().ToArray();
            directorio = tmp[0];
            string directorioBase = tmp[1];
            if (!fileList.ContainsKey(directorioBase + "\\" + directorio))
            {
                return false;
            }
            IEnumerable<FileInfo> subFileList = fileList[directorioBase + "\\" + directorio];
            bool ret = registros.Count() > 0 &&
                           registros.ToList().TrueForAll(x => subFileList.Any(y => y.Name.EqualsIgnoreCase(x.Field<string>(campo) + ".pdf")));
            if (ret)
            {
                rutaFisicaReportes = registros.Select(x => subFileList.Single(y => y.Name.EqualsIgnoreCase(x.Field<string>(campo) + ".pdf")).FullName).Distinct().ToList();
            }
            return ret;
        }

        private static bool revisaAlmenosUnMTR(string directorio, List<DataRow> registros, IDictionary<string, IEnumerable<FileInfo>> fileList, out List<string> rutaFisicaReportes, string campo)
        {
            rutaFisicaReportes = new List<string>();

            if (directorio.StartsWith("\\"))
            {
                directorio = directorio.Substring(1);
            }

            string[] tmp = directorio.Split('\\').Reverse().ToArray();
            directorio = tmp[0];
            string directorioBase = tmp[1];

            if (!fileList.ContainsKey(directorioBase + "\\" + directorio))
            {
                return false;
            }

            IEnumerable<FileInfo> subFileList = fileList[directorioBase + "\\" + directorio];

            bool almenosUno = false;
            string numCert = string.Empty;

            foreach (DataRow otm in registros)
            {
                string cosa = otm.Field<string>(campo);

                bool ret = subFileList.Any(y => y.Name.EqualsIgnoreCase(otm.Field<string>(campo) + ".pdf"));

                if (ret)
                {
                    almenosUno = true;

                    if (numCert != otm.Field<string>(campo))
                    {
                        rutaFisicaReportes.Add(subFileList.Single(y => y.Name.EqualsIgnoreCase(otm.Field<string>(campo) + ".pdf")).FullName);
                    }

                    numCert = otm.Field<string>(campo);
                }
            }

            return almenosUno;
        }

        /// <summary>
        /// Este metodo se encarga de verificar si existe cada uno de los reportes de pintura
        /// </summary>
        /// <param name="registros"></param>
        /// <param name="fileList"></param>
        /// <param name="rutaFisicaReportes"></param>
        /// <returns></returns>
        private static bool revisaReportesPintura(List<DataRow> registros, IDictionary<string, IEnumerable<FileInfo>> fileList, out List<string> rutaFisicaReportes)
        {
            bool ret = registros.Count() > 0;
            rutaFisicaReportes = new List<string>();
            List<string> listaTmp;

            ret = revisaReportes(DirectorioDossier.Reportes_Pintura_AcabadoVisual, registros, fileList, out listaTmp, "ReporteAcabadoVisual") && ret;
            rutaFisicaReportes.AddRange(listaTmp);
            ret = revisaReportes(DirectorioDossier.Reportes_Pintura_Adherencia, registros, fileList, out listaTmp, "ReporteAdherencia") && ret;
            rutaFisicaReportes.AddRange(listaTmp);
            ret = revisaReportes(DirectorioDossier.Reportes_Pintura_Primarios, registros, fileList, out listaTmp, "ReportePrimarios") && ret;
            rutaFisicaReportes.AddRange(listaTmp);
            ret = revisaReportes(DirectorioDossier.Reportes_Pintura_Intermedios, registros, fileList, out listaTmp, "ReporteIntermedios") && ret;
            rutaFisicaReportes.AddRange(listaTmp);
            ret = revisaReportes(DirectorioDossier.Reportes_Pintura_PullOff, registros, fileList, out listaTmp, "ReportePullOff") && ret;
            rutaFisicaReportes.AddRange(listaTmp);
            ret = revisaReportes(DirectorioDossier.Reportes_Pintura_Sandblast, registros, fileList, out listaTmp, "ReporteSandblast") && ret;
            rutaFisicaReportes.AddRange(listaTmp);

            return ret;
        }

        /// <summary>
        /// Metodo que se mandara llamar desde un thread que se encarga de agregar los registros que le fueron asignados a construir
        /// </summary>
        /// <param name="spools"></param>
        /// <param name="wkstatusJunta"></param>
        /// <param name="wkstatusSpool"></param>
        /// <param name="reportes"></param>
        /// <param name="wps"></param>
        /// <param name="fabAreaID"></param>
        /// <param name="tipoJuntaTH_ID"></param>
        /// <param name="tipoJuntaTW_ID"></param>
        /// <param name="fileList"></param>
        /// <param name="certBag"></param>
        private static void generaRegistros(IEnumerable<DataRow> spools, DataTable wkstatusJunta, DataTable wkstatusSpool, DataTable reportes, DataTable wps, DataTable mtr, DataTable mtrSold, int fabAreaID, int tipoJuntaTH_ID, int tipoJuntaTW_ID, IDictionary<string, IEnumerable<FileInfo>> fileList, ConcurrentBag<GrdCertificacion> certBag)
        {
            GrdCertificacion cert;
            int spoolID;
            List<DataRow> wkstatusJuntaDeEsteSpool;
            List<DataRow> wkstatusSpoolDeEsteSpool;
            List<DataRow> reportesDeJuntasDeEsteSpool;

            List<DataRow> wkstatusJuntas = wkstatusJunta.Select().ToList();
            List<DataRow> wkstatusSpools = wkstatusSpool.Select().ToList();
            List<DataRow> reportesDeJuntas = reportes.Select().ToList();

            foreach (DataRow spool in spools)
            {
                try
                {
                    cert = new GrdCertificacion();
                    spoolID = spool.Field<int>("SpoolID");
                    cert.SpoolID = spoolID;

                    bool wpsCompletos = false;
                    bool mtrCompletos = false;
                    bool mtrAlmenosUno = false;
                    bool mtrSoldCompletos = false;
                    bool mtrSoldAlmenosUno = false;

                    List<string> reportesWps = new List<string>();
                    List<string> reportesMTR = new List<string>();
                    List<string> reportesMTRSold = new List<string>();

                    List<DataRow> wpsXSpool = wps.Select(String.Format("SpoolID={0}", spoolID)).ToList();
                    List<DataRow> mtrXSpool = mtr.Select(String.Format("SpoolID={0}", spoolID)).ToList();
                    List<DataRow> mtrSoldXSpool = mtrSold.Select(String.Format("SpoolID={0}", spoolID)).ToList();

                    wpsCompletos = revisaReportes(DirectorioDossier.Reportes_WPS, wpsXSpool, fileList, out reportesWps, "Nombre");

                    mtrCompletos = revisaReportes(DirectorioDossier.Reportes_MTR, mtrXSpool, fileList, out reportesMTR, "NumCertificado");
                    mtrSoldCompletos = revisaReportes(DirectorioDossier.Reportes_MTRSoldadura, mtrSoldXSpool, fileList, out reportesMTRSold, "Codigo");

                    if (!mtrCompletos)
                    {
                        mtrAlmenosUno = revisaAlmenosUnMTR(DirectorioDossier.Reportes_MTR, mtrXSpool, fileList, out reportesMTR, "NumCertificado");
                    }

                    if (!mtrSoldCompletos)
                    {
                        mtrSoldAlmenosUno = revisaAlmenosUnMTR(DirectorioDossier.Reportes_MTRSoldadura, mtrSoldXSpool, fileList, out reportesMTRSold, "Codigo");
                    }

                    wkstatusJuntaDeEsteSpool = wkstatusJuntas.Where(x => x.Field<int>("SpoolID") == spoolID).ToList();

                    wkstatusSpoolDeEsteSpool = wkstatusSpools.Where(x => x.Field<int>("SpoolID") == spoolID).ToList();

                    reportesDeJuntasDeEsteSpool = reportesDeJuntas.Where(x => wkstatusJuntaDeEsteSpool.Select(y => y.Field<int?>("JuntaWorkStatusID").GetValueOrDefault(0)).Contains(x.Field<int>("JuntaWorkstatusID"))).ToList();

                    cert.NumeroControl = spool.Field<string>("NumeroControl");
                    cert.Spool = spool.Field<string>("Nombre");

                    #region Trazabilidad

                    string[] tmpTrazabilidad = DirectorioDossier.Reportes_Trazabilidad.Split('\\').Reverse().ToArray();
                    string directorioTrazabilidad = tmpTrazabilidad[0];
                    string directorioBaseTrazabilidad = tmpTrazabilidad[1];

                    if (!fileList.ContainsKey(directorioBaseTrazabilidad + "\\" + directorioTrazabilidad))
                    {
                        cert.TrazabilidadEscaneado = false;
                    }
                    else
                    {
                        IEnumerable<FileInfo> subFileList = fileList[directorioBaseTrazabilidad + "\\" + directorioTrazabilidad];
                        cert.TrazabilidadEscaneado = subFileList.Any(y => y.Name.EqualsIgnoreCase(cert.Spool + ".pdf"));
                    }

                    #endregion

                    #region Armado

                    // Verifica que todas las juntas del spool  que tengan fabarea= shop tengan armado 
                    cert.ArmadoCompletas = wkstatusJuntaDeEsteSpool.Count > 0 && !wkstatusJuntaDeEsteSpool.Where(x => x.Field<int>("FabAreaID") == fabAreaID).Any(x => !x.Field<int?>("JuntaArmadoID").HasValue);

                    #endregion

                    #region Soldadura

                    //Verifica que todas las juntas que tengan fabarea = shop y algun tipo junta que no sea th o tw,  tengan soldadura
                    cert.SoladoCompleto = wkstatusJuntaDeEsteSpool.Count > 0 && !wkstatusJuntaDeEsteSpool.Where(x => x.Field<int>("FabAreaID") == fabAreaID && x.Field<int>("TipoJuntaID") == tipoJuntaTH_ID &&
                            x.Field<int>("TipoJuntaID") != tipoJuntaTW_ID).Any(x => !x.Field<int?>("JuntaSoldaduraID").HasValue);

                    #endregion

                    #region Insp Visual

                    //Verifica que todas las juntas tengan inspeccion visual aprobada
                    cert.InspVisAprobada = wkstatusJuntaDeEsteSpool.Count > 0 && !wkstatusJuntaDeEsteSpool.Any(x => !x.Field<bool?>("InspeccionVisualAprobada").GetValueOrDefault(false));

                    //para saber si se puede imprimir, 1.- tiene que estar aprobada  2.- incluido en Dossier  3.-Todas las juntas deberan Tener reporte de inspeccion visual existente
                    cert.InspVisImprimir = cert.InspVisAprobada && revisaReportes(DirectorioDossier.Reportes_InspeccionVisual, wkstatusJuntaDeEsteSpool, fileList, out cert.InspVisReportes);
                    #endregion

                    #region Insp Dimensional

                    //Verifica que el spool tengan inspeccion dimensional aprobada
                    cert.InspDimAprobada =
                        wkstatusSpoolDeEsteSpool.Any(
                            x => x.Field<bool?>("TieneLiberacionDimensional").GetValueOrDefault(false) &&
                                 x.Field<int?>("TipoReporteDimensionalID").GetValueOrDefault(0) == (int)TipoReporteDimensionalEnum.Dimensional &&
                                 x.Field<bool?>("Aprobado").GetValueOrDefault(false));

                    cert.InspDimImprimir = cert.InspDimAprobada &&
                                           revisaReportes(DirectorioDossier.Reportes_LiberacionDimensional,
                                                          wkstatusSpoolDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int?>("TipoReporteDimensionalID").GetValueOrDefault(0) ==
                                                              (int)TipoReporteDimensionalEnum.Dimensional).ToList(), fileList,
                                                          out cert.InspDimReportes);
                    #endregion

                    #region Pintura



                    //Este es un caso especial, aqui vamos a revisar que todos los reportes de pintura que no esten en blanco existan en sus directorios
                    cert.PinturaCompleta = revisaReportesPintura(wkstatusSpoolDeEsteSpool, fileList, out cert.PinturaReportes);

                    //Sabremos si se puede imprimir alguno si es que almenos existe uno
                    cert.PinturaImprimir = cert.PinturaReportes != null && cert.PinturaReportes.Count > 0;


                    #endregion

                    #region Insp Espesores

                    //Verifica que el spool tengan inspeccion dimensional aprobada
                    cert.InspEspesoresAprobada =
                        wkstatusSpoolDeEsteSpool.Any(
                            x =>
                                x.Field<bool?>("TieneLiberacionDimensional").GetValueOrDefault(false) &&
                                x.Field<int?>("TipoReporteDimensionalID").GetValueOrDefault(0) == (int)TipoReporteDimensionalEnum.Espesores &&
                                x.Field<bool?>("Aprobado").GetValueOrDefault(false));

                    cert.InspEspImprimir = cert.InspEspesoresAprobada &&
                                           revisaReportes(DirectorioDossier.Reportes_Espesores,
                                                            wkstatusSpoolDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int?>("TipoReporteDimensionalID").GetValueOrDefault(0) ==
                                                              (int)TipoReporteDimensionalEnum.Espesores).ToList(), fileList, out cert.InspEspReportes);

                    #endregion

                    #region RT

                    //Verifica que el spool tengan todas sus juntas RT aprobadas
                    cert.RtCompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.ReporteRT);

                    cert.RtImprimir = cert.RtCompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_RT,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.ReporteRT).ToList(), fileList, out cert.RtReportes);

                    #endregion

                    #region PT

                    //Verifica que el todas las juntas requisitadas tengan una prueba aprobada del tipo PT
                    cert.PtCompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.ReportePT);

                    cert.PtImprimir = cert.PtCompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_PT,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.ReportePT).ToList(), fileList, out cert.PtReportes);

                    #endregion

                    #region Pwht

                    //Verifica que el todas las juntas requisitadas tengan una prueba aprobada del tipo pwht
                    cert.PwhtCompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.Pwht);

                    cert.PwhtImprimir = cert.PwhtCompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_Pwht,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.Pwht).ToList(), fileList, out cert.PwhtReportes);

                    #endregion

                    #region Durezas

                    //Verifica que el todas las juntas requisitadas tengan una prueba aprobada del tipo Durezas
                    cert.DurezasCompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.Durezas);

                    cert.DurezasImprimir = cert.DurezasCompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_Durezas,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.Durezas).ToList(), fileList, out cert.DurezasReportes);

                    #endregion

                    #region RTPostTT

                    //Verifica que el todas las juntas requisitadas tengan una prueba aprobada del tipo RTPostTT
                    cert.RtPostCompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.RTPostTT);

                    cert.RtPostImprimir = cert.RtPostCompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_RTPostTT,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.RTPostTT).ToList(), fileList, out cert.RtPostReportes);

                    #endregion

                    #region PTPostTT

                    //Verifica que el spool tengan inspeccion dimensional aprobada
                    cert.PtPostCompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.PTPostTT);

                    cert.PtPostImprimir = cert.PtPostCompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_PTPostTT,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.PTPostTT).ToList(), fileList, out cert.PtPostReportes);

                    #endregion

                    #region Preheat

                    //Verifica que el spool tengan inspeccion dimensional aprobada
                    cert.PreheatCompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.Preheat);

                    cert.PreheatImprimir = cert.PreheatCompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_Preheat,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.Preheat).ToList(), fileList, out cert.PreheatReportes);

                    #endregion

                    #region UT

                    //Verifica que el spool tengan inspeccion dimensional aprobada
                    cert.UtCompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.ReporteUT);

                    cert.UtImprimir = cert.UtCompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_UT,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.ReporteUT).ToList(), fileList, out cert.UtReportes);
                    #endregion

                    #region PMI

                    //Verifica que el spool tengan todas sus juntas RT aprobadas
                    cert.PMICompleto = todasAprobadas(reportesDeJuntasDeEsteSpool, TipoPruebaEnum.ReportePMI);

                    cert.PMIImprimir = cert.PMICompleto &&
                                           revisaReportes(DirectorioDossier.Reportes_PMI,
                                                          reportesDeJuntasDeEsteSpool.Where(
                                                              x =>
                                                              x.Field<int>("TipoPruebaID") ==
                                                              (int)TipoPruebaEnum.ReportePMI).ToList(), fileList, out cert.PMIReportes);

                    #endregion

                    #region Prueba Hidrostática

                    //Verifica que el spool tenga prueba hidrostática aprobada
                    cert.PruebaHidroAprobada =
                        wkstatusSpoolDeEsteSpool.Any(
                            x => x.Field<bool?>("TienePruebaHidrostatica").GetValueOrDefault(false));

                    cert.PruebaHidroImprimir = cert.PruebaHidroAprobada &&
                                                revisaReportes(DirectorioDossier.Reportes_PruebasHidrostaticas,
                                                          wkstatusSpoolDeEsteSpool, fileList,
                                                          out cert.PruebaHidroReportes, "NumeroReportePruebaHidrostatica");
                    #endregion

                    #region Embarque


                    if ((wkstatusSpoolDeEsteSpool.FirstOrDefault()) != null)
                    {
                        //Verifica que el spool haya sido embarcado
                        cert.EmbarqueCompleto =
                            wkstatusSpoolDeEsteSpool.First().Field<bool?>("Embarcado").GetValueOrDefault(false);
                        cert.NumeroEmbarque = wkstatusSpoolDeEsteSpool.First().Field<string>("NumeroEmbarque");

                        string[] tmpEmbarque = DirectorioDossier.Reportes_Embarque.Split('\\').Reverse().ToArray();
                        string directorioEmbarque = tmpEmbarque[0];
                        string directorioBaseEmbarque = tmpEmbarque[1];

                        if (!fileList.ContainsKey(directorioBaseEmbarque + "\\" + directorioEmbarque))
                        {
                            cert.EmbarqueEscaneado = false;
                        }
                        else
                        {
                            IEnumerable<FileInfo> subFileList = fileList[directorioBaseEmbarque + "\\" + directorioEmbarque];
                            cert.EmbarqueEscaneado = subFileList.Any(y => y.Name.EqualsIgnoreCase(cert.NumeroEmbarque + ".pdf"));
                        }
                    }

                    #endregion

                    #region Wps

                    cert.WpsCompleto = cert.SoladoCompleto;
                    if (wpsCompletos)
                    {
                        cert.WpsImprimir = true;
                        cert.WpsReportes = reportesWps;
                    }

                    #endregion

                    #region MTR

                    if (mtrCompletos)
                    {
                        cert.MtrCompleto = true;
                        cert.MTRImprimir = true;
                        cert.MTRReportes = reportesMTR;
                    }
                    else if (mtrAlmenosUno)
                    {
                        cert.MtrCompleto = false;
                        cert.MTRImprimir = true;
                        cert.MTRReportes = reportesMTR;
                    }

                    #endregion

                    #region MTRSoldadura
                    if (mtrSoldCompletos)
                    {
                        cert.MtrSoldCompleto = true;
                        cert.MTRSoldImprimir = true;
                        cert.MTRSoldReportes = reportesMTRSold;
                    }
                    else if (mtrSoldAlmenosUno)
                    {
                        cert.MtrSoldCompleto = false;
                        cert.MTRSoldImprimir = true;
                        cert.MTRSoldReportes = reportesMTRSold;
                    }
                    #endregion

                    #region Dibujo
                    string[] tmpDibujo = DirectorioDossier.Reportes_Dibujo.Split('\\').Reverse().ToArray();
                    string directorioDibujo = tmpDibujo[0];
                    string directorioBaseDibujo = tmpDibujo[1];

                    if (!fileList.ContainsKey(directorioBaseDibujo + "\\" + directorioDibujo))
                    {
                        cert.DibujoImprimir = false;
                    }
                    else
                    {
                        IEnumerable<FileInfo> subFileList = fileList[directorioBaseDibujo + "\\" + directorioDibujo];
                        cert.Dibujo = spool.Field<string>("Dibujo");
                        string dibujo = cert.Dibujo.Replace(@"""", "");
                        cert.DibujoImprimir = subFileList.Any(y => y.Name.EqualsIgnoreCase(dibujo + ".pdf"));
                    }
                    #endregion

                    certBag.Add(cert);
                }
                catch (SqlException ex)
                {
                    _logger.Info(ex.Message);
                    throw new ExcepcionProcesos(MensajesError.Exception_BaseDeDatos);
                }
                catch (ArgumentException ex)
                {
                    _logger.Info(ex.Message);
                    throw new ExcepcionProcesos(MensajesError.Exception_GeneracionCertificacion);

                }
                catch (Exception ex)
                {
                    _logger.Info(ex.Message);
                    throw new ExcepcionProcesos(MensajesError.Exception_GeneracionCertificacion);
                }
            }

            _logger.Info("fin generar registros " + DateTime.Now);
        }

        /// <summary>
        /// regresa Verdadero si hay por lo menos un registro y todos los registros en el campo aprobado tienen TRUE
        /// </summary>
        /// <param name="registros"></param>
        /// <param name="tipoPrueba"></param>
        /// <returns></returns>
        private static bool todasAprobadas(IEnumerable<DataRow> registros, TipoPruebaEnum tipoPrueba)
        {
            IEnumerable<DataRow> registrosDeTipo = registros.Where(x => x.Field<int>("TipoPruebaID") == (int)tipoPrueba);

            return registrosDeTipo.Count() > 0 &&
                    registrosDeTipo.ToList().TrueForAll(x => x.Field<bool?>("Aprobado").GetValueOrDefault(false));

        }


        public void DescargaReportes(List<GrdCertificacion> elementos, int? proyectoID, string nombreDefaultReporteEmbarque)
        {
            DataSet ds = obtenInfoBaseDatos(string.Join(",", elementos.Select(x => x.SpoolID)), proyectoID);

            //Esta es una Coleccion ThreadSafe que permite almacenar los registros que se estan obteniendo
            ConcurrentBag<byte[]> pdfFinal = new ConcurrentBag<byte[]>();
            int cores = Configuracion.CoresProcesador;

            //En esta lista guardaremos todos los threads que nos ayudaran a juntar los registros que ocupamos
            List<Thread> threads = new List<Thread>();

            //Dividimos los registros entre el numero de cores del CPU
            int rate = Math.Ceiling(elementos.Count.SafeDecimalParse() / cores).SafeIntParse();

            HttpContext context = HttpContext.Current;

            //para cada nucleo del procesador creamos un thread
            for (int i = 0; i < cores; i++)
            {
                IEnumerable<GrdCertificacion> elementosParaEsteCore = elementos.Skip(i * rate).Take(rate);

                threads.Add(new Thread(
                                () =>
                                ObtenPdfsSpool(elementosParaEsteCore, ds, pdfFinal, context, proyectoID,
                                               nombreDefaultReporteEmbarque)
                                ));
            }

            threads.ForEach(x => x.Start());

            threads.ForEach(x => x.Join());

            if (pdfFinal.Count == 0)
            {
                throw new ExcepcionProcesos(MensajesError.Excepcion_SinReportesParaDescargar);
            }

            byte[] pdf = juntaPDFs(pdfFinal.ToList());
            string nombreArchivo = "Reporte";

            EscribePdfAResponse(nombreArchivo, pdf);
        }

        /// <summary>
        /// Escribe al response el arreglo de bytes como pdf poniendo el nombre de archivo para su descarga
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="pdf"></param>
        private static void EscribePdfAResponse(string nombreArchivo, byte[] pdf)
        {
            HttpResponse response = HttpContext.Current.Response;
            ReportUtils.SendReportAsPDF(response, pdf, nombreArchivo);
        }

        public void ObtenCiertosPdfsSpool(GrdCertificacion elemento, TipoReporte tipoReporte)
        {
            List<string> reportes = new List<string>();
            string nombreArchivo = "Certificacion_";

            switch (tipoReporte)
            {
                case TipoReporte.InspeccionVisual:
                    reportes = elemento.InspVisReportes;
                    nombreArchivo += TipoReporteNombre.InspeccionVisual;
                    break;
                case TipoReporte.ReporteDurezas:
                    reportes = elemento.DurezasReportes;
                    nombreArchivo += TipoReporteNombre.ReporteDurezas;
                    break;
                case TipoReporte.ReporteEspesores:
                    reportes = elemento.InspEspReportes;
                    nombreArchivo += TipoReporteNombre.ReporteEspesores;
                    break;
                case TipoReporte.ReporteLiberacionDimensional:
                    reportes = elemento.InspDimReportes;
                    nombreArchivo += TipoReporteNombre.ReporteLiberacionDimensional;
                    break;
                case TipoReporte.ReportePintura:
                    reportes = elemento.PinturaReportes;
                    nombreArchivo += TipoReporteNombre.ReportePintura;
                    break;
                case TipoReporte.ReportePT:
                    reportes = elemento.PtReportes;
                    nombreArchivo += TipoReporteNombre.ReporteRT;
                    break;
                case TipoReporte.ReportePWHT:
                    reportes = elemento.PwhtReportes;
                    nombreArchivo += TipoReporteNombre.ReportePWHT;
                    break;
                case TipoReporte.ReporteRT:
                    reportes = elemento.RtReportes;
                    nombreArchivo += TipoReporteNombre.ReporteRT;
                    break;
                case TipoReporte.ReportePreheat:
                    reportes = elemento.PreheatReportes;
                    nombreArchivo += TipoReporteNombre.ReportePreheat;
                    break;
                case TipoReporte.ReportePostPT:
                    reportes = elemento.PtPostReportes;
                    nombreArchivo += TipoReporteNombre.ReportePostPT;
                    break;
                case TipoReporte.ReportePostRT:
                    reportes = elemento.RtPostReportes;
                    nombreArchivo += TipoReporteNombre.ReportePostRT;
                    break;
                case TipoReporte.ReporteUT:
                    reportes = elemento.UtReportes;
                    nombreArchivo += TipoReporteNombre.ReporteUT;
                    break;
                case TipoReporte.ReportePMI:
                    reportes = elemento.PMIReportes;
                    nombreArchivo += TipoReporteNombre.ReportePMI;
                    break;
                case TipoReporte.ReporteSpoolPND:
                    reportes = elemento.PruebaHidroReportes;
                    nombreArchivo += TipoReporteNombre.ReportePruebaHidro;
                    break;
                case TipoReporte.ReporteWps:
                    reportes = elemento.WpsReportes;
                    nombreArchivo += TipoReporteNombre.ReporteWPS;
                    break;
                case TipoReporte.ReporteMTR:
                    reportes = elemento.MTRReportes;
                    nombreArchivo += TipoReporteNombre.ReporteMTR;
                    break;
                case TipoReporte.ReporteMTRSoldadura:
                    reportes = elemento.MTRSoldReportes;
                    nombreArchivo += TipoReporteNombre.ReporteMTRSoldadura;
                    break;

            }

            byte[] pdf = juntaPdfYAgregaPortada(reportes, null, null);
            EscribePdfAResponse(nombreArchivo, pdf);
        }

        private static void ObtenPdfsSpool(IEnumerable<GrdCertificacion> elementos, DataSet ds, ConcurrentBag<byte[]> pdfFinal, HttpContext context, int? proyectoID, string nombreDefaultReporte)
        {
            foreach (GrdCertificacion grdCertificacion in elementos)
            {
                List<string> reportesSpool = listaReportes(grdCertificacion);
                byte[] embarque = generaEmbarque(grdCertificacion.SpoolID, ds, context, proyectoID, nombreDefaultReporte);
                //if (reportesSpool.Count != 0 || embarque!= null)
                //{
                byte[] caratula = generaCaratula(grdCertificacion.SpoolID, ds, context, proyectoID);
                byte[] pdfsSpool = creaPdfDeSpoolConCaratula(reportesSpool, caratula, embarque);
                pdfFinal.Add(pdfsSpool);
                //}
            }

        }

        private static byte[] generaEmbarque(int spoolID, DataSet ds, HttpContext context, int? proyectoID, string nombreDefaultReporte)
        {
            //TODO: Generar EmbarqueBL metodo que regrese el reporte en PDF
            DataRow dr =
                ds.Tables["Spool"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID).First();

            if (!proyectoID.HasValue)
            {
                proyectoID = dr.Field<int>("ProyectoID");
            }

            int? workstatusSpoolID = 0;

            workstatusSpoolID = dr.Field<int?>("WorkstatusSpoolID");
            string numeroEmbarque = ds.Tables["Workstatus"].AsEnumerable().Where(
                x => x.Field<int?>("WorkstatusSpoolID") == workstatusSpoolID).Select(x => x.Field<string>("NumeroEmbarque")).FirstOrDefault();

            if (string.IsNullOrEmpty(numeroEmbarque))
            {
                return null;
            }

            ProyectoReporteCache personalizado =
               CacheCatalogos.Instance
                   .ObtenerProyectoReporte()
                   .Where(x => x.ProyectoID == proyectoID && x.TipoReporte == TipoReporteProyectoEnum.Embarque)
                   .SingleOrDefault();

            string nombreReporte;
            if (personalizado != null)
            {
                if (!personalizado.Nombre.StartsWith("/"))
                {
                    nombreReporte = "/" + personalizado.Nombre;
                }
                else
                {
                    nombreReporte = personalizado.Nombre;
                }
            }
            else
            {
                nombreReporte = string.Concat("/", Config.ReportingServicesDefaultFolder, "/", nombreDefaultReporte);
            }

            HttpCookie cookie = context.Request.Cookies["Culture"];
            if (cookie != null)
            {
                if (cookie.Value == LanguageHelper.INGLES)
                {
                    nombreReporte += "-ingles";
                }
            }

            _logger.Debug("Empezando configuracion Reporting Services");

            MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices,
                                                                                   Config.PasswordReportingServices,
                                                                                   Config.DomainReportingServices);
            ReportViewer rptViewer = new ReportViewer();
            rptViewer.Reset();
            rptViewer.ProcessingMode = ProcessingMode.Remote;
            rptViewer.ServerReport.ReportServerCredentials = credenciales;
            rptViewer.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
            rptViewer.ServerReport.ReportPath = nombreReporte;

            _logger.Debug(string.Format("Credentials: {0}", credenciales));
            _logger.Debug(string.Format("ReportServerUrl: {0}", Config.UrlReportingServices));
            _logger.Debug(string.Format("ReportPath: {0}", nombreReporte));
            _logger.Debug(string.Format("NumeroEmbarque: {0}", numeroEmbarque));
            _logger.Debug(string.Format("ProyectoID: {0}", proyectoID));


            List<ReportParameter> lstParametros = new List<ReportParameter>()
                                                      {
                                                          new ReportParameter("NumeroEmbarque", numeroEmbarque,false),
                                                          new ReportParameter("ProyectoID", proyectoID.ToString(),false)
                                                      };

            foreach (ReportParameter param in lstParametros)
            {
                _logger.Debug(string.Format("lstParametros Name: {0}", param.Name));
                foreach (string str in param.Values)
                {
                    _logger.Debug(string.Format("lstParametros Values: {0}", str));
                }
            }



            _logger.Debug("Iniciando SetParameters");

            try
            {

                rptViewer.ServerReport.SetParameters(lstParametros);
            }
            catch (Exception ex)
            {
                _logger.Debug(String.Format("Excepcion: {0}", ex.Message));
                _logger.Debug(String.Format("Inner Ex: {0}", ex.InnerException));
                _logger.Debug(String.Format("Data: {0}", ex.Data));
                _logger.Debug(String.Format("StackTrace: {0}", ex.StackTrace));
            }

            _logger.Debug("Iniciando Refresh");

            rptViewer.ServerReport.Refresh();

            _logger.Debug("Termina Refresh");
            _logger.Debug(rptViewer.ServerReport.Render("PDF").ToString());

            return rptViewer.ServerReport.Render("PDF");
        }

        private static DataSet obtenInfoBaseDatos(string spoolIDs, int? proyectoID)
        {
            const string nombreProc = "[ObtenerInfoCaratulaSpool]";
            DataSet ds = new DataSet();
            string[] nombreTablas = new[] { "Spool", "Workstatus", "Reportes", "Coladas", "Juntas" };
            using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
            {
                IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
                if (!proyectoID.HasValue)
                {
                    parameters[0].Value = spoolIDs;
                }
                else
                {
                    parameters[1].Value = proyectoID;
                }

                ds = DataAccess.ExecuteDataset(connection,
                                               CommandType.StoredProcedure,
                                               nombreProc,
                                               ds,
                                               nombreTablas,
                                               parameters);

                return ds;

            }
        }

        public static byte[] generaCaratula(int spoolID)
        {
            return generaCaratula(spoolID, obtenInfoBaseDatos(spoolID.ToString(), null), HttpContext.Current, null);
        }

        private static byte[] generaCaratula(int spoolID, DataSet ds, HttpContext context, int? proyectoID)
        {
            ProyectoReporteCache personalizado =
               CacheCatalogos.Instance
                   .ObtenerProyectoReporte()
                   .Where(x => x.ProyectoID == proyectoID && x.TipoReporte == TipoReporteProyectoEnum.Trazabilidad)
                   .SingleOrDefault();

            string nombreReporte;
            if (personalizado != null)
            {
                if (!personalizado.Nombre.StartsWith("/"))
                {
                    nombreReporte = "/" + personalizado.Nombre;
                }
                else
                {
                    nombreReporte = personalizado.Nombre;
                }
                DataRow dr =
                    ds.Tables["Spool"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID).FirstOrDefault();

                MyReportServerCredentials credenciales = new MyReportServerCredentials(Config.UsernameReportingServices,
                                                                                   Config.PasswordReportingServices,
                                                                                   Config.DomainReportingServices);

                ReportViewer rptViewer = new ReportViewer();
                rptViewer.Reset();
                rptViewer.ProcessingMode = ProcessingMode.Remote;
                rptViewer.ServerReport.ReportServerCredentials = credenciales;
                rptViewer.ServerReport.ReportServerUrl = new Uri(Config.UrlReportingServices);
                rptViewer.ServerReport.ReportPath = nombreReporte;
                List<ReportParameter> lstParametros = new List<ReportParameter>()
                                                      {
                                                          new ReportParameter("NumeroControl", dr.Field<string>("NumeroControl"),false),
                                                          new ReportParameter("ProyectoID", proyectoID.ToString(),false)
                                                      };
                foreach (ReportParameter param in lstParametros)
                {
                    _logger.Debug(string.Format("lstParametros Name: {0}", param.Name));
                    foreach (string str in param.Values)
                    {
                        _logger.Debug(string.Format("lstParametros Values: {0}", str));
                    }
                }



                _logger.Debug("Iniciando SetParameters");

                try
                {

                    rptViewer.ServerReport.SetParameters(lstParametros);
                }
                catch (Exception ex)
                {
                    _logger.Debug(String.Format("Excepcion: {0}", ex.Message));
                    _logger.Debug(String.Format("Inner Ex: {0}", ex.InnerException));
                    _logger.Debug(String.Format("Data: {0}", ex.Data));
                    _logger.Debug(String.Format("StackTrace: {0}", ex.StackTrace));
                }

                _logger.Debug("Iniciando Refresh");

                rptViewer.ServerReport.Refresh();

                _logger.Debug("Termina Refresh");
                _logger.Debug(rptViewer.ServerReport.Render("PDF").ToString());

                return rptViewer.ServerReport.Render("PDF");
            }
            else
            {


                CaratulaSpool cs = new CaratulaSpool { Fecha = DateTime.Today };

                DataRow dr =
                    ds.Tables["Spool"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID).FirstOrDefault();

                int? workstatusSpoolID = 0;
                if (dr != null)
                {

                    workstatusSpoolID = dr.Field<int?>("WorkstatusSpoolID");
                    cs.Dibujo = dr.Field<string>("Dibujo");
                    cs.Revision = dr.Field<string>("RevisionCliente");
                    cs.NombreSpool = dr.Field<string>("Nombre");
                    cs.NumeroControl = dr.Field<string>("NumeroControl");
                }
                if (workstatusSpoolID.HasValue)
                {
                    dr =
                        ds.Tables["Workstatus"].AsEnumerable().Where(
                            x => x.Field<int?>("WorkstatusSpoolID") == workstatusSpoolID).FirstOrDefault();
                    if (dr != null)
                    {
                        cs.Primario = dr.Field<string>("ReportePrimarios");
                        cs.Adeherencias = dr.Field<string>("ReporteAdherencia");
                        cs.PullOff = dr.Field<string>("ReportePullOff");
                        cs.Acabado = dr.Field<string>("ReporteAcabadoVisual");
                        cs.Enlace = dr.Field<string>("ReporteIntermedios");
                    }

                    dr =
                        ds.Tables["Reportes"].AsEnumerable().Where(
                            x =>
                            x.Field<int>("WorkstatusSpoolID") == workstatusSpoolID &&
                            x.Field<int>("TipoReporteDimensionalID") == (int)TipoReporteDimensionalEnum.Espesores).
                            FirstOrDefault();
                    if (dr != null)
                    {
                        cs.NumReporteEspesores = dr.Field<string>("NumeroReporte");
                    }

                    dr =
                        ds.Tables["Reportes"].AsEnumerable().Where(
                            x =>
                            x.Field<int>("WorkstatusSpoolID") == workstatusSpoolID &&
                            x.Field<int>("TipoReporteDimensionalID") == (int)TipoReporteDimensionalEnum.Dimensional).
                            FirstOrDefault();
                    if (dr != null)
                    {
                        cs.NumReporteDimensional = dr.Field<string>("NumeroReporte");
                    }


                }
                var juntasSpool = ds.Tables["Juntas"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID);
                List<CaratulaJunta> caratulaJuntas = new List<CaratulaJunta>();
                List<CaratulaColada> caratulaColadas = new List<CaratulaColada>();

                IEnumerable<CaratulaJunta> juntasAgrupadas = from j in juntasSpool
                                                             group j by new { SpoolID = j.Field<int>("SpoolID"), EtiquetaJunta = j.Field<string>("EtiquetaJunta") } into g
                                                             let junta = juntasSpool.First(x => x.Field<int>("SpoolID") == g.Key.SpoolID && x.Field<string>("EtiquetaJunta") == g.Key.EtiquetaJunta)
                                                             select new CaratulaJunta
                                                             {
                                                                 MaterialBase1 = obtenerCodigoMaterialBase(junta.Field<int?>("NumeroUnico1ID"), ds),
                                                                 MaterialBase2 = obtenerCodigoMaterialBase(junta.Field<int?>("NumeroUnico2ID"), ds),
                                                                 Etiqueta = junta.Field<string>("EtiquetaJunta"),
                                                                 TipoJunta = junta.Field<string>("TipoJunta"),
                                                                 Diametro = junta.Field<decimal>("Diametro"),
                                                                 Cedula = junta.Field<string>("Cedula"),
                                                                 Armado = junta.Field<DateTime?>("FechaArmado"),
                                                                 Soldadura = junta.Field<DateTime?>("FechaSoldadura"),
                                                                 Wps = junta.Field<string>("WPS"),
                                                                 SoldadorRelleno = string.Join(",", juntasSpool.Where(x => x.Field<int?>("TecnicaSoldadorID").GetValueOrDefault(0) == (int)TecnicaSoldadorEnum.Relleno && junta.Field<string>("EtiquetaJunta") == x.Field<string>("EtiquetaJunta")).Select(y => y.Field<string>("CodigoSoldador")).Distinct().ToList()),
                                                                 SoldadorRaiz = string.Join(",", juntasSpool.Where(x => x.Field<int?>("TecnicaSoldadorID").GetValueOrDefault(0) == (int)TecnicaSoldadorEnum.Raiz && junta.Field<string>("EtiquetaJunta") == x.Field<string>("EtiquetaJunta")).Select(y => y.Field<string>("CodigoSoldador")).Distinct().ToList()),
                                                                 Durezas = obtenTt(juntasSpool, TipoPruebaEnum.Durezas, junta.Field<string>("EtiquetaJunta")),
                                                                 Pt = obtenPnds(juntasSpool, TipoPruebaEnum.ReportePT, junta.Field<string>("EtiquetaJunta")),
                                                                 PostRT = obtenPnds(juntasSpool, TipoPruebaEnum.RTPostTT, junta.Field<string>("EtiquetaJunta")),
                                                                 Rt = obtenPnds(juntasSpool, TipoPruebaEnum.ReporteRT, junta.Field<string>("EtiquetaJunta")),
                                                                 PWHT = obtenTt(juntasSpool, TipoPruebaEnum.Pwht, junta.Field<string>("EtiquetaJunta")),
                                                                 InspeccionVisual = obtenIV(juntasSpool, junta.Field<string>("EtiquetaJunta"))
                                                             };
                caratulaJuntas.AddRange(juntasAgrupadas);

                List<int> numerosUnicos =
                    juntasSpool.Select(x => x.Field<int?>("NumeroUnico1ID")).Distinct().Where(x => x != null).Select(
                        x => x.Value).ToList();
                juntasSpool.Select(x => x.Field<int?>("NumeroUnico2ID")).Distinct().Where(x => x != null).Select(
                    x => x.Value).ToList().ForEach(numerosUnicos.Add);


                if (numerosUnicos.Count == 0)
                {
                    var Colada = ds.Tables["Coladas"].AsEnumerable();
                    if (Colada != null)
                    {
                        List<CaratulaColada> caratColada = (from c in Colada
                                                            select new CaratulaColada
                                                            {
                                                                Descripcion = c.Field<string>(
                                                                  LanguageHelper.CustomCulture == LanguageHelper.INGLES
                                                                      ? "DescripcionIngles"
                                                                      : "DescripcionEspanol"),
                                                                Certificado = c.Field<string>("Certificado"),
                                                                Colada = c.Field<string>("NumeroColada"),
                                                                ItemCode = c.Field<string>("CodigoMaterial")
                                                            }).ToList();
                        foreach (CaratulaColada coladas in caratColada)
                        {
                            caratulaColadas.Add(coladas);
                        }
                    }
                }
                numerosUnicos.Distinct().Select(x => obtenerColada(x, ds)).Where(x => x != null).ToList().ForEach(
                    caratulaColadas.Add);

                cs.Juntas = caratulaJuntas.ToList();
                cs.Coladas = caratulaColadas.ToList();
                return CaratulaBL.Instance.RegresaPdf(cs, cs.Juntas, cs.Coladas, context);
            }
        }

        private static string obtenTt(IEnumerable<DataRow> juntasSpool, TipoPruebaEnum tipoPruebaEnum, string etiquetaJunta)
        {
            return string.Join(",",
                        juntasSpool.Where(x => x.Field<int?>("TtTipoPruebaID").GetValueOrDefault(0) == (int)tipoPruebaEnum && x.Field<string>("EtiquetaJunta") == etiquetaJunta).Select(
                            y => y.Field<string>("ReporteTt")).Distinct().ToList());
        }

        private static string obtenPnds(IEnumerable<DataRow> juntasSpool, TipoPruebaEnum tipoPruebaEnum, string etiquetaJunta)
        {
            return string.Join(",",
                        juntasSpool.Where(x => x.Field<int?>("PndTipoPruebaID").GetValueOrDefault(0) == (int)tipoPruebaEnum && x.Field<string>("EtiquetaJunta") == etiquetaJunta).Select(
                            y => y.Field<string>("ReportePnd")).Distinct().ToList());
        }

        private static string obtenIV(IEnumerable<DataRow> juntasSpool, string etiquetaJunta)
        {
            return string.Join(",",
                        juntasSpool.Where(x => x.Field<string>("EtiquetaJunta") == etiquetaJunta).Select(
                            y => y.Field<string>("ReporteIV")).Distinct().ToList());
        }

        private static string obtenerCodigoMaterialBase(int? numeroUnicoID, DataSet ds)
        {
            if (!numeroUnicoID.HasValue)
            {
                return string.Empty;
            }
            return ds.Tables["Coladas"].AsEnumerable().First(
                           x => x.Field<int>("NumeroUnicoID") == numeroUnicoID.Value).Field<string>("NumeroColada");
        }

        private static CaratulaColada obtenerColada(int? numeroUnicoID, DataSet ds)
        {
            if (!numeroUnicoID.HasValue)
            {
                return null;
            }

            DataRow colada =
                ds.Tables["Coladas"].AsEnumerable().First(x => x.Field<int>("NumeroUnicoID") == numeroUnicoID.Value);

            return new CaratulaColada
            {

                Descripcion = colada.Field<string>(
                               LanguageHelper.CustomCulture == LanguageHelper.INGLES
                                   ? "DescripcionIngles"
                                   : "DescripcionEspanol"),
                Certificado = colada.Field<string>("Certificado"),
                Colada = colada.Field<string>("NumeroColada"),
                ItemCode = colada.Field<string>("CodigoMaterial")

            };
        }


        /// <summary>
        /// Metedo que se encarga de construir una lista con todas las rutas fisicas de los reportes
        /// </summary>
        /// <param name="grdCertificacion"></param>
        /// <returns></returns>
        private static List<string> listaReportes(GrdCertificacion grdCertificacion)
        {
            List<string> retorno = new List<string>();
            if (grdCertificacion.InspDimImprimir)
            {
                retorno.AddRange(grdCertificacion.InspDimReportes);
            }

            if (grdCertificacion.InspVisImprimir)
            {
                retorno.AddRange(grdCertificacion.InspVisReportes);
            }

            if (grdCertificacion.DurezasImprimir)
            {
                retorno.AddRange(grdCertificacion.DurezasReportes);
            }

            if (grdCertificacion.InspEspImprimir)
            {
                retorno.AddRange(grdCertificacion.InspEspReportes);
            }

            if (grdCertificacion.PinturaImprimir)
            {
                retorno.AddRange(grdCertificacion.PinturaReportes);
            }

            if (grdCertificacion.PreheatImprimir)
            {
                retorno.AddRange(grdCertificacion.PreheatReportes);
            }

            if (grdCertificacion.PtImprimir)
            {
                retorno.AddRange(grdCertificacion.PtReportes);
            }

            if (grdCertificacion.PtPostImprimir)
            {
                retorno.AddRange(grdCertificacion.PtPostReportes);
            }

            if (grdCertificacion.PwhtImprimir)
            {
                retorno.AddRange(grdCertificacion.PwhtReportes);
            }

            if (grdCertificacion.RtImprimir)
            {
                retorno.AddRange(grdCertificacion.RtReportes);
            }

            if (grdCertificacion.RtPostImprimir)
            {
                retorno.AddRange(grdCertificacion.RtPostReportes);
            }

            if (grdCertificacion.UtImprimir)
            {
                retorno.AddRange(grdCertificacion.UtReportes);
            }

            if (grdCertificacion.WpsImprimir)
            {
                retorno.AddRange(grdCertificacion.WpsReportes);
            }
            if (grdCertificacion.MTRImprimir)
            {
                retorno.AddRange(grdCertificacion.MTRReportes);
            }
            return retorno;
        }


        /// <summary>
        /// Junta varios pdfs y regresa uno solo conteniendo la union de todos
        /// </summary>
        /// <param name="pdfs"></param>
        /// <returns></returns>
        private static byte[] juntaPDFs(IEnumerable<byte[]> pdfs)
        {
            byte[] array = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {

                Document document = new Document();
                PdfCopy copy = new PdfCopy(document, memoryStream);
                document.Open();

                pdfs.ToList().ForEach(p => agregaPdfDesdeArreglo(p, copy));


                document.Close();
                array = memoryStream.ToArray();
            }
            return array;
        }

        /// <summary>
        /// Antepone el pdf eviado en el segundo parametro a la lista de pdfs enviados en el primer parametro
        /// </summary>
        /// <param name="pdf"></param>
        /// <param name="caratula"></param>
        /// <param name="embarque"></param>
        /// <returns></returns>
        private static byte[] creaPdfDeSpoolConCaratula(IEnumerable<string> pdf, byte[] caratula, byte[] embarque)
        {
            return juntaPdfYAgregaPortada(pdf, caratula, embarque);
        }

        /// <summary>
        /// Antepone el pdf eviado en el segundo parametro a la lista de pdfs enviados en el primer parametro
        /// </summary>
        /// <param name="pdf"></param>
        /// <param name="portada"></param>
        /// <param name="embarque"></param>
        /// <returns></returns>
        private static byte[] juntaPdfYAgregaPortada(IEnumerable<string> pdf, byte[] portada, byte[] embarque)
        {
            byte[] array = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {

                Document document = new Document();
                PdfCopy copia = new PdfCopy(document, memoryStream);
                document.Open();

                //portada
                agregaPdfDesdeArreglo(portada, copia);

                //reportes
                pdf.ToList().ForEach(p => agregaPdfDesdeArchivo(p, copia));

                //reporte Embarque
                agregaPdfDesdeArreglo(embarque, copia);

                document.Close();
                array = memoryStream.ToArray();
            }
            return array;
        }

        private static void agregaPdfDesdeArchivo(string archivo, PdfCopy documento)
        {
            if (!string.IsNullOrEmpty(archivo))
            {
                PdfReader reader = new PdfReader(new RandomAccessFileOrArray(archivo), null);
                agregaPdfDesdeReader(reader, documento);
            }
        }

        private static void agregaPdfDesdeArreglo(byte[] arreglo, PdfCopy documento)
        {
            if (arreglo != null)
            {
                PdfReader reader = new PdfReader(new RandomAccessFileOrArray(arreglo), null);
                agregaPdfDesdeReader(reader, documento);
            }
        }

        private static void agregaPdfDesdeReader(PdfReader reader, PdfCopy documento)
        {
            // loop over document pages
            for (int i = 0; i < reader.NumberOfPages;)
            {
                documento.AddPage(documento.GetImportedPage(reader, ++i));
            }
        }

        public CertificationReport ObtenerReporteCertificacionShop(string spoolID, int proyectoID)
        {
            DataSet ds = new DataSet();
            ds = obtenInfoBaseDatosShop(spoolID, proyectoID);

            CertificationReport cr = new CertificationReport();

            DataRow dr = ds.Tables["Spool"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == Convert.ToInt32(spoolID)).FirstOrDefault();


            if (dr != null)
            {
                cr.DetailSpool.Isometric = dr.Field<string>("Isometrico");
                cr.DetailSpool.Hold = dr.Field<string>("Hold");
                cr.DetailSpool.DateHold = dr.Field<DateTime?>("FechaHold") != null ? dr.Field<DateTime?>("FechaHold") : null;
                cr.DetailSpool.Joints = dr.Field<int>("Juntas");
                cr.DetailSpool.Weight = dr.Field<decimal>("Peso");
                cr.DetailSpool.Area = dr.Field<decimal>("Area");
                cr.DetailSpool.PDI = dr.Field<decimal>("PDI");
                cr.DetailSpool.PEQS = dr.Field<decimal>("PEQS");
                cr.DetailSpool.CustomerReview = dr.Field<string>("RevisionCliente");
                cr.DetailSpool.PercentagePND = dr.Field<int?>("PorcentajePND");
                cr.DetailSpool.Specification = dr.Field<string>("Especificacion");
                cr.DetailSpool.DimensionalLiberation = dr.Field<string>("LiberacionDimensional");
                cr.DetailSpool.RequiredTestHydrostatic = dr.Field<string>("RequierePruebaHidrostatica");
                cr.DetailSpool.TestHydrostatic = dr.Field<string>("ReportePruebaHidrostatica");
                cr.DetailSpool.NumberShipment = dr.Field<string>("NumeroEmbarque");
                cr.DetailSpool.DateShipment = dr.Field<DateTime?>("FechaEmbarque");
                cr.DetailSpool.RequiredPWHT = dr.Field<string>("RequierePWHT");


                cr.DetailSpool.System = dr.Field<string>("Sistema");
                cr.DetailSpool.Color = dr.Field<string>("Color");
                cr.DetailSpool.FechaPrimario = dr.Field<DateTime?>("FechaPrimario");
                cr.DetailSpool.ReportePrimario = dr.Field<string>("ReportePrimario");
                cr.DetailSpool.FechaIntermedio = dr.Field<DateTime?>("FechaIntermedio");
                cr.DetailSpool.ReporteIntermedio = dr.Field<string>("ReporteIntermedio");
                cr.DetailSpool.FechaPullOff = dr.Field<DateTime?>("FechaPullOff");
                cr.DetailSpool.ReportePullOff = dr.Field<string>("ReportePullOff");
                cr.DetailSpool.FechaAcabadoVisual = dr.Field<DateTime?>("FechaAcabadoVisual");
                cr.DetailSpool.ReporteAcabadoVisual = dr.Field<string>("ReporteAcabadoVisual");
                cr.DetailSpool.FechaAdherencia = dr.Field<DateTime?>("FechaAdherencia");
                cr.DetailSpool.ReporteAdherencia = dr.Field<string>("ReporteAdherencia");


                //check list embarque  
                cr.DetailCheckListShipping.Hold = dr.Field<string>("EnHold");
                cr.DetailCheckListShipping.Materials = dr.Field<bool?>("Materiales");
                cr.DetailCheckListShipping.Dispatch = dr.Field<bool?>("Despachos");
                cr.DetailCheckListShipping.Fitting = dr.Field<bool?>("Armado");
                cr.DetailCheckListShipping.Welding = dr.Field<int?>("Soldadura");
                cr.DetailCheckListShipping.VisualInspection = dr.Field<bool?>("InspeccionVisual");
                cr.DetailCheckListShipping.DimensionalInspection = dr.Field<bool?>("TieneLiberacionDimensional");
                cr.DetailCheckListShipping.PWHT = dr.Field<int?>("PWHT");
                cr.DetailCheckListShipping.PND = dr.Field<int?>("PND");
                cr.DetailCheckListShipping.Paint = dr.Field<int?>("Pintura");
                cr.DetailCheckListShipping.SistemaPintura = dr.Field<string>("SistemaPintura");
                cr.DetailCheckListShipping.OKMIL = dr.Field<bool?>("OkMateriales");
                cr.DetailCheckListShipping.OkPaint = dr.Field<bool?>("TienePintura");
                cr.DetailCheckListShipping.OkQuality = dr.Field<bool?>("OKCalidad");
                cr.DetailCheckListShipping.PreparationShipping = dr.Field<bool?>("OkPreparacionEmbarque");

                var juntasSpool = ds.Tables["JuntaSpool"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID.SafeIntParse());
                var reportesJuntas = ds.Tables["ReqRepPNDTT"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID.SafeIntParse());

                IEnumerable<DetailReportSummary> reportes = from r in reportesJuntas
                                                            select new DetailReportSummary
                                                            {
                                                                Aprobado = r.Field<bool>("Aprobado"),
                                                                NumeroReporte = r.Field<string>("NumeroReporte"),
                                                                TipoPruebaId = r.Field<int>("TipoPruebaId"),
                                                                JuntaWorkStatusId = r.Field<int>("JuntaWorkstatusId")
                                                            };

                IEnumerable<DetailJoint> juntas = from j in juntasSpool
                                                  select new DetailJoint
                                                  {
                                                      Joint = j.Field<string>("EtiquetaJunta"),
                                                      TypeJoint = j.Field<string>("TipoJunta"),
                                                      Diameter = j.Field<decimal>("Diametro"),
                                                      Cedula = j.Field<string>("Cedula"),
                                                      DateFitting = j.Field<DateTime?>("FechaArmado"),
                                                      DateWelding = j.Field<DateTime?>("FechaSoldadura"),
                                                      WPS = j.Field<string>("WPS"),
                                                      WelderRoot = j.Field<string>("SoldadorRaiz"),
                                                      WelderFiller = j.Field<string>("SoldadorRelleno"),
                                                      DateVisualInspection = j.Field<DateTime?>("FechaInspeccionVisual"),
                                                      ResultVisualInspection = j.Field<string>("ResultadoInspeccionVisual"),
                                                      UniqueNumberOne = j.Field<string>("NumeroUnicoOne"),
                                                      UniqueNumberTwo = j.Field<string>("NumeroUnicoTwo"),
                                                      JuntaWorkstatusId = j.Field<int?>("JuntaWorkstatusId"),
                                                      RT = reportes.Where(x => x.JuntaWorkStatusId == j.Field<int?>("JuntaWorkstatusId") && x.TipoPrueba == TipoPruebaEnum.ReporteRT && x.Aprobado).Select(x => x.NumeroReporte).FirstOrDefault(),
                                                      PT = reportes.Where(x => x.JuntaWorkStatusId == j.Field<int?>("JuntaWorkstatusId") && x.TipoPrueba == TipoPruebaEnum.ReportePT && x.Aprobado).Select(x => x.NumeroReporte).FirstOrDefault(),
                                                      Hardness = reportes.Where(x => x.JuntaWorkStatusId == j.Field<int?>("JuntaWorkstatusId") && x.TipoPrueba == TipoPruebaEnum.Durezas && x.Aprobado).Select(x => x.NumeroReporte).FirstOrDefault(),
                                                      RequiredPWHT = j.Field<string>("RequierePWHT"),
                                                      PWHT = reportes.Where(x => x.JuntaWorkStatusId == j.Field<int?>("JuntaWorkstatusId") && x.TipoPrueba == TipoPruebaEnum.Pwht && x.Aprobado).Select(x => x.NumeroReporte).FirstOrDefault(),
                                                      PMI = reportes.Where(x => x.JuntaWorkStatusId == j.Field<int?>("JuntaWorkstatusId") && x.TipoPrueba == TipoPruebaEnum.ReportePMI && x.Aprobado).Select(x => x.NumeroReporte).FirstOrDefault(),
                                                      RequiredTestNeumatic = j.Field<string>("RequierePruebaNeumatica"),
                                                      TestNeumatic = reportes.Where(x => x.JuntaWorkStatusId == j.Field<int?>("JuntaWorkstatusId") && x.TipoPrueba == TipoPruebaEnum.Neumatica && x.Aprobado).Select(x => x.NumeroReporte).FirstOrDefault()
                                                  };

                if (juntas != null)
                {
                    cr.DetailJoints.AddRange(juntas);
                }

                var materialesSpool = ds.Tables["Materiales"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID.SafeIntParse());

                IEnumerable<DetailMaterial> materiales = from m in materialesSpool
                                                         select new DetailMaterial
                                                         {
                                                             Label = m.Field<string>("Etiqueta"),
                                                             UniqueNumber = m.Field<string>("NumeroUnico"),
                                                             Heat = m.Field<string>("Colada"),
                                                             Certified = m.Field<string>("Certificado"),
                                                             ItemCode = m.Field<string>("ItemCode"),
                                                             DiameterOne = m.Field<decimal>("Diametro1"),
                                                             DiameterTwo = m.Field<decimal>("Diametro2"),
                                                             Descprition = m.Field<string>("Descripcion"),
                                                             Quantity = m.Field<Int32>("Cantidad"),
                                                             Manufacturer = m.Field<string>("Fabricante"),
                                                             Motion = m.Field<string>("Pedimento"),
                                                             Notes = m.Field<string>("Notas")
                                                         };
                if (materiales != null)
                {
                    cr.DetailMaterials.AddRange(materiales);
                }
            }


            return cr;
        }

        private static DataSet obtenInfoBaseDatosShop(string spoolIDs, int proyectoID)
        {
            const string nombreProc = "[ObtenerInfoCaratulaSpoolShop]";

            DataSet ds = new DataSet();

            string[] nombreTablas = new[] { "Spool", "JuntaSpool", "Materiales", "ReqRepPNDTT" };

            using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
            {
                IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);

                parameters[0].Value = spoolIDs;
                parameters[1].Value = proyectoID;

                ds = DataAccess.ExecuteDataset(connection,
                                            CommandType.StoredProcedure,
                                            nombreProc,
                                            ds,
                                            nombreTablas,
                                            parameters);
                return ds;

            }
        }

        private static DataSet ObtenerInfoSummarySpoolShop(string spoolIDs, int proyectoID)
        {
            const string nombreProc = "[ObtenerInfoSummarySpoolShop]";

            DataSet ds = new DataSet();

            string[] nombreTablas = new[] { "Spool", "JuntaSpool", "Materiales", "ReqRepPNDTT" };

            using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
            {
                IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);

                parameters[0].Value = spoolIDs;
                parameters[1].Value = proyectoID;

                ds = DataAccess.ExecuteDataset(connection,
                                            CommandType.StoredProcedure,
                                            nombreProc,
                                            ds,
                                            nombreTablas,
                                            parameters);
                return ds;

            }
        }

        public SummarySpool ObtenerReporteSummaryShop(string spoolID, int proyectoID)
        {
            DataSet ds = new DataSet();
            ds = ObtenerInfoSummarySpoolShop(spoolID, proyectoID);

            SummarySpool cr = new SummarySpool();

            DataRow dr = ds.Tables["Spool"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == Convert.ToInt32(spoolID)).FirstOrDefault();


            if (dr != null)
            {
                cr.SistemaPintura = dr.Field<string>("Sistema");
                cr.Campo54 = dr.Field<string>("Campo54");
                cr.Cuadrante = dr.Field<string>("Cuadrante");
                cr.FechaLiberacionDimensional = dr.Field<DateTime?>("FechaLiberacionDimensional") != null ? dr.Field<DateTime?>("FechaLiberacionDimensional") : null;
                cr.ReporteLiberacionDimensional = dr.Field<string>("LiberacionDimensional");
                cr.ResultadoLiberacionDimensional = dr.Field<string>("ResultadoDimensional");
                cr.FechaPrimario = dr.Field<DateTime?>("FechaPrimario");
                cr.FechaAcabado = dr.Field<DateTime?>("FechaAcabadoVisual");
                cr.Spool = dr.Field<string>("Spool");
                cr.GrupoAcero = dr.Field<string>("GrupoAcero");
                cr.NumeroEmbarque = dr.Field<string>("NumeroEmbarque");
                cr.RequierePWHT = dr.Field<string>("RequierePWHT");
                cr.PorcentajePND = dr.Field<int>("PorcentajePND");
                cr.Hold = dr.Field<string>("Hold");
                cr.M2 = dr.Field<decimal>("M2");
                cr.Kg = dr.Field<decimal>("Kg");
                cr.KGSGRUPO = dr.Field<string>("KGSGRUPO");
                cr.InspectorLiberacionDimensional = dr.Field<string>("InspectorLiberacionDimensional");
                cr.FechaOkPnd = dr.Field<DateTime?>("FechaOkPnd");
                cr.DiametroMayor = dr.Field<decimal>("DiametroMayor");
                var juntasSpool = ds.Tables["JuntaSpool"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID.SafeIntParse());
                var reportesJuntas = ds.Tables["ReqRepPNDTT"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID.SafeIntParse());
                cr.Prioridad = dr.Field<int>("Prioridad");
                IEnumerable<DetailReport> reportes = from r in reportesJuntas
                                                     select new DetailReport
                                                     {
                                                         NumeroReporteRT = r.Field<string>("NumeroReporteRT"),
                                                         NumeroReportePT = r.Field<string>("NumeroReportePT"),
                                                         NumeroReportePWHT = r.Field<string>("NumeroReportePWHT"),
                                                         NumeroRequiPT = r.Field<string>("NumeroRequiPT"),
                                                         NumeroRequiRT = r.Field<string>("NumeroRequiRT"),
                                                         FechaRequiPT = r.Field<DateTime?>("FechaRequiPT"),
                                                         FechaRequiRT = r.Field<DateTime?>("FechaRequiRT"),
                                                         JuntaWorkstatusId = r.Field<int?>("JuntaWorkstatusId"),
                                                         AprobadoPT = r.Field<bool?>("AprobadoPT"),
                                                         AprobadoRT = r.Field<bool?>("AprobadoRT"),
                                                         AprobadoPWHT = r.Field<bool?>("AprobadoPWHT")
                                                     };

                IEnumerable<DetailSummaryJoint> juntas = from j in juntasSpool
                                                         select new DetailSummaryJoint
                                                         {
                                                             Joint = j.Field<string>("EtiquetaJunta"),
                                                             TypeJoint = j.Field<string>("TipoJunta"),
                                                             DateWelding = j.Field<DateTime?>("FechaSoldadura"),
                                                             DateVisualInspection = j.Field<DateTime?>("FechaInspeccionVisual"),
                                                             ResultadoVisual = j.Field<string>("ResultadoVisual"),
                                                             ClasifPND = j.Field<string>("ClasifPND"),
                                                             Diameter = j.Field<decimal>("Diametro"),
                                                             RequiredPWHT = j.Field<string>("RequierePWHT"),
                                                             JuntaWorkStatusId = j.Field<int?>("JuntaWorkStatusId"),
                                                             RequiPT = reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.NumeroRequiPT).FirstOrDefault(),
                                                             RequiRT = reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.NumeroRequiRT).FirstOrDefault(),
                                                             FechaReqRT = reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.FechaRequiRT).FirstOrDefault(),
                                                             FechaReqPT = reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.FechaRequiPT).FirstOrDefault(),
                                                             AprobadoPT = (reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.AprobadoPT).FirstOrDefault()) == null ? "" : (reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.AprobadoPT).FirstOrDefault()) == true ? "Aprobado" : "Rechazado",
                                                             AprobadoRT = (reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.AprobadoRT).FirstOrDefault()) == null ? "" : (reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.AprobadoRT).FirstOrDefault()) == true ? "Aprobado" : "Rechazado",
                                                             AprobadoPWHT = reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.AprobadoPWHT).FirstOrDefault(),
                                                             ReporteRT = reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.NumeroReporteRT).FirstOrDefault(),
                                                             ReportePT = reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.NumeroReportePT).FirstOrDefault(),
                                                             ReportePWHT = reportes.Where(x => x.JuntaWorkstatusId == j.Field<int?>("JuntaWorkStatusId")).Select(x => x.NumeroReportePWHT).FirstOrDefault(),
                                                             InspectorVisual = j.Field<string>("InspectorVisual")
                                                         };



                if (juntas != null)
                {
                    cr.DetailJoints.AddRange(juntas);
                }

                var materialesSpool = ds.Tables["Materiales"].AsEnumerable().Where(x => x.Field<int>("SpoolID") == spoolID.SafeIntParse());

                IEnumerable<DetailMaterialSummary> materiales = from m in materialesSpool
                                                                select new DetailMaterialSummary
                                                                {
                                                                    Material = m.Field<string>("Descripcion"),
                                                                    Motion1 = m.Field<string>("Pedimento1"),
                                                                    Motion2 = m.Field<string>("Pedimento2"),
                                                                    MTR = m.Field<string>("Certificado"),
                                                                    Bill = m.Field<string>("Factura"),
                                                                    NumeroUnico = m.Field<string>("NumeroUnico"),
                                                                    Cantidad = m.Field<int>("Cantidad"),
                                                                    Codigo = m.Field<string>("Codigo")
                                                                };
                if (materiales != null)
                {
                    cr.DetailMaterials.AddRange(materiales);
                }
            }


            return cr;
        }

        private static string[] obtenTtConFecha(IEnumerable<DataRow> juntasSpool, TipoPruebaEnum tipoPruebaEnum, string etiquetaJunta)
        {
            string[] reporte = new string[3];
            IEnumerable<DataRow> r = juntasSpool.Where(x => x.Field<int?>("TtTipoPruebaID").GetValueOrDefault(0) == (int)tipoPruebaEnum && x.Field<string>("EtiquetaJunta") == etiquetaJunta).ToArray();
            if (r.Count() > 0)
            {
                DataRow dr = r.First<DataRow>();
                reporte[0] = dr.Field<string>("ReporteTt");
                reporte[1] = dr.Field<DateTime>("FechaReporteTt").ToShortDateString();
                reporte[2] = dr.Field<string>("RequiTt");
            }
            return reporte;
        }

        private static string[] obtenPndsConFecha(IEnumerable<DataRow> juntasSpool, TipoPruebaEnum tipoPruebaEnum, string etiquetaJunta)
        {
            string[] reporte = new string[3];
            IEnumerable<DataRow> r = juntasSpool.Where(x => x.Field<int?>("PndTipoPruebaID").GetValueOrDefault(0) == (int)tipoPruebaEnum && x.Field<string>("EtiquetaJunta") == etiquetaJunta).ToArray();
            if (r.Count() > 0)
            {
                DataRow dr = r.First<DataRow>();
                reporte[0] = dr.Field<string>("ReportePND");
                reporte[1] = dr.Field<DateTime>("FechaReportePND").ToShortDateString();
                reporte[2] = dr.Field<string>("RequiPND");
            }
            return reporte;
        }
    }
}









