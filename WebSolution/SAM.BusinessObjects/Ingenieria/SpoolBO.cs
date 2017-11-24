using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using System.Linq.Expressions;
using System;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using System.Data.Objects;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Validations;
using log4net;
using System.Diagnostics;
using Newtonsoft.Json;

namespace SAM.BusinessObjects.Ingenieria
{
    public class SpoolBO
    {
        private static readonly object _mutex = new object();
        private static SpoolBO _instance;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SpoolBO));
        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private SpoolBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase SpoolBO
        /// </summary>
        /// <returns></returns>
        public static SpoolBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SpoolBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Regresa un objeto de tipo spool en base a su ID.
        /// Este objeto regresa sin relaciones, únicamente las propiedades primitivas
        /// del mismo se llenan.
        /// </summary>
        /// <param name="spoolID">ID del spool que se desea</param>
        /// <returns>Objeto spool para el ID seleccionado</returns>
        public Spool Obtener(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Spool.Include("MaterialSpool").Where(x => x.SpoolID == spoolID).SingleOrDefault();
            }
        }

        public Spool ObtenerConODT(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Spool.Include("OrdenTrabajoSpool").Where(x => x.SpoolID == spoolID).SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolHold"></param>
        /// <param name="sHoldHistorial"></param>
        /// <returns></returns>
        private bool EstaEnHold(SpoolHold spoolHold, SpoolHoldHistorial sHoldHistorial)
        {
            bool enHold = false;

            switch (sHoldHistorial.TipoHold)
            {
                case TipoHoldSpool.INGENIERIA:
                    enHold = spoolHold.TieneHoldIngenieria ? true : false;
                    break;
                case TipoHoldSpool.CALIDAD:
                    enHold = spoolHold.TieneHoldCalidad ? true : false;
                    break;
                case TipoHoldSpool.CONFINADO:
                    enHold = spoolHold.Confinado ? true : false;
                    break;
                default:
                    break;
            }

            return enHold;
        }


        public List<GrdCruce> ObtenerAntesDeCruceRevisionesEspeciales(int proyectoID)
        {
            //List<Spool> spools = null;
            //List<Grupo> cuentaMateriales = null;
            ////List<Grupo> cuentaTubos = null;
            //List<Grupo> cuentaPeqs = null;

            ////Esta separado en dos queries de manera intencional, se supone debe ser más eficiente
            //using (SamContext ctx = new SamContext())
            //{
            //    ctx.Spool.MergeOption = MergeOption.NoTracking;

            //    sw.Start();
            //    _logger.DebugFormat("Query ObtenerSpoolsPorIds");

            //    //Todos los Spools que estan marcados como recision para un proyecto
            //    spools = ctx.Spool.Where(x => x.EsRevision == true
            //                             && (x.UltimaOrdenTrabajoEspecial == null || x.UltimaOrdenTrabajoEspecial == "")
            //                             && x.ProyectoID == proyectoID).ToList();

            //    //Creamos un arreglo solo con los ID de los Spools
            //    int[] spoolIds = (from spls in spools
            //                      select spls.SpoolID).ToArray();

            //    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            //    sw.Restart();
            //    _logger.DebugFormat("cuenta materiales");
            //    cuentaMateriales =
            //        (from matSp in ctx.MaterialSpool.Where(x => spoolIds.Contains(x.SpoolID))
            //         group matSp by matSp.SpoolID
            //             into grp
            //             select new Grupo
            //             {
            //                 ID = grp.Key,
            //                 Cuenta = grp.Where(y => y.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio).Count(),
            //                 Suma = grp.Where(z => z.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio).Sum(x => (int?)x.Cantidad) ?? 0,
            //                 CuentaTubo = grp.Where(y => y.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo).Count(),
            //                 SumaTubo = grp.Where(z => z.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo).Sum(x => (int?)x.Cantidad) ?? 0
            //             }).ToList();

            //    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            //    sw.Restart();
            //    _logger.DebugFormat("cuenta Peqs");
            //    cuentaPeqs =
            //        (from jta in ctx.JuntaSpool.Where(x => spoolIds.Contains(x.SpoolID) && x.FabAreaID == CacheCatalogos.Instance.ShopFabAreaID)
            //         group jta by jta.SpoolID into g
            //         select new Grupo
            //         {
            //             ID = g.Key,
            //             SumaDecimal = g.Sum(x => x.Peqs),
            //             Cuenta = g.Count(),
            //         }).ToList();

            //    Dictionary<int, string> famAceros = CacheCatalogos.Instance
            //                                                  .ObtenerFamiliasAcero()
            //                                                  .ToDictionary(x => x.ID, y => y.Nombre);

            //    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

            //    sw.Restart();
            //    _logger.DebugFormat("return final");
            //    //Regresar objetos del tipo que se necesitan buscando en Cache las familias de acero
            //    return (from sp in spools
            //            let j = cuentaPeqs.SingleOrDefault(x => x.ID == sp.SpoolID)
            //            let a = cuentaMateriales.SingleOrDefault(x => x.ID == sp.SpoolID)
            //            select new GrdCruce
            //            {
            //                Area = sp.Area ?? 0,
            //                Cedula = sp.Cedula,
            //                Dibujo = sp.Dibujo,
            //                FamiliaAcero1 = famAceros[sp.FamiliaAcero1ID],
            //                FamiliaAcero1ID = sp.FamiliaAcero1ID,
            //                FamiliaAcero2 = sp.FamiliaAcero2ID.HasValue ? famAceros[sp.FamiliaAcero2ID.Value] : string.Empty,
            //                FamiliaAcero2ID = sp.FamiliaAcero2ID ?? -1,
            //                Juntas = j != null ? j.Cuenta : 0, //Cuenta de juntas
            //                Nombre = sp.Nombre,
            //                Pdis = sp.Pdis ?? 0,
            //                Peso = sp.Peso ?? 0,
            //                Prioridad = sp.Prioridad ?? 999,
            //                SpoolID = sp.SpoolID,
            //                TotalPeqs = j != null ? (j.SumaDecimal.HasValue ? j.SumaDecimal.Value : 0) : 0,
            //                TotalTubo = a != null ? a.CuentaTubo : 0,
            //                LongitudTubo = a != null ? a.SumaTubo : 0,
            //                TotalAccesorio = a != null ? a.Cuenta : 0,
            //                DiametroPlano = sp.DiametroPlano,
            //                Hold = false,
            //                ObservacionesHold = String.Empty
            //            }).AsParallel().ToList();

            //}
            return null;
        }


        /// <summary>
        /// Regresa la información de varios spools separados por ID con lo que despliega el Grid de cruce.
        ///  
        /// ----> JHT -- 21/03/2014 -- Agrego parametro esRevision, para que este metodo solo traiga información
        /// ---->                      de los Spool que estan marcados como revisión
        /// 
        /// </summary>
        /// <param name="spoolIds">Ids de los spools a mostrar separados por coma</param>
        /// <returns>Spools con la información que se despliega en el grid de cruce</returns>
        public List<GrdCruce> ObtenerDespuesDeCruce(int[] spoolIds, bool incluyeHold)
        {
            List<Spool> spools = null;
            List<Grupo> cuentaMateriales = null;
            //List<Grupo> cuentaTubos = null;
            List<Grupo> cuentaPeqs = null;

            //Esta separado en dos queries de manera intencional, se supone debe ser más eficiente
            using (SamContext ctx = new SamContext())
            {
                ctx.Spool.MergeOption = MergeOption.NoTracking;
                string ids = string.Join(",", spoolIds);

                sw.Start();
                _logger.DebugFormat("Query ObtenerSpoolsPorIds");

                //Los spools que nos piden
                spools = ctx.ObtenerSpoolsPorIds(ids).ToList();
                _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

                sw.Restart();
                _logger.DebugFormat("cuenta materiales");
                cuentaMateriales =
                    (from matSp in ctx.MaterialSpool.Where(x => spoolIds.Contains(x.SpoolID))
                     group matSp by matSp.SpoolID
                         into grp
                         select new Grupo
                         {
                             ID = grp.Key,
                             Cuenta = grp.Where(y => y.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio).Count(),
                             Suma = grp.Where(z => z.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Accessorio).Sum(x => (int?)x.Cantidad) ?? 0,
                             CuentaTubo = grp.Where(y => y.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo).Count(),
                             SumaTubo = grp.Where(z => z.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo).Sum(x => (int?)x.Cantidad) ?? 0
                         }).ToList();

                _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

                sw.Restart();
                _logger.DebugFormat("cuenta Peqs");
                cuentaPeqs =
                    (from jta in ctx.JuntaSpool.Where(x => spoolIds.Contains(x.SpoolID) && x.FabAreaID == CacheCatalogos.Instance.ShopFabAreaID)
                     group jta by jta.SpoolID into g
                     select new Grupo
                     {
                         ID = g.Key,
                         SumaDecimal = g.Sum(x => x.Peqs),
                         Cuenta = g.Count(),
                     }).ToList();

                Dictionary<int, string> famAceros = CacheCatalogos.Instance
                                                              .ObtenerFamiliasAcero()
                                                              .ToDictionary(x => x.ID, y => y.Nombre);

                _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);


                if (incluyeHold)
                {
                    sw.Restart();
                    _logger.DebugFormat("holds");

                    // Se seleccionan los spools en hold junto con sus observaciones más recientes
                    //var spoolHoldHistorial = ctx.SpoolHoldHistorial.Where(x => spoolIds.Contains(x.SpoolID)).ToList();



                    var spoolHolds = ctx.SpoolHold
                                        .Where(x => spoolIds.Contains(x.SpoolID))
                                        .Where(x => x.TieneHoldIngenieria || x.TieneHoldCalidad || x.Confinado).ToList();

                    var spoolObservaciones = (from sh in spoolHolds
                                              let spoolHoldHistorialObservaciones = from shh in ctx.SpoolHoldHistorial.Where(y => y.SpoolID == sh.SpoolID)
                                                                                    select shh.Observaciones
                                              select new
                                              {
                                                  ID = sh.SpoolID,
                                                  Observaciones = String.Join(",", spoolHoldHistorialObservaciones)
                                              }).ToList();



                    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

                    sw.Restart();
                    _logger.DebugFormat("return final");
                    //Regresar objetos del tipo que se necesitan buscando en Cache las familias de acero
                    return (from sp in spools
                            let j = cuentaPeqs.SingleOrDefault(x => x.ID == sp.SpoolID)
                            //let t = cuentaTubos.SingleOrDefault(x => x.ID == sp.SpoolID)
                            let a = cuentaMateriales.SingleOrDefault(x => x.ID == sp.SpoolID)
                            let o = spoolObservaciones.SingleOrDefault(x => x.ID == sp.SpoolID)
                            select new GrdCruce
                            {
                                Area = sp.Area ?? 0,
                                Cedula = sp.Cedula,
                                Dibujo = sp.Dibujo,
                                FamiliaAcero1 = famAceros[sp.FamiliaAcero1ID],
                                FamiliaAcero1ID = sp.FamiliaAcero1ID,
                                FamiliaAcero2 = sp.FamiliaAcero2ID.HasValue ? famAceros[sp.FamiliaAcero2ID.Value] : string.Empty,
                                FamiliaAcero2ID = sp.FamiliaAcero2ID ?? -1,
                                Juntas = j != null ? j.Cuenta : 0, //Cuenta de juntas
                                Nombre = sp.Nombre,
                                Pdis = sp.Pdis ?? 0,
                                Peso = sp.Peso ?? 0,
                                Prioridad = sp.Prioridad ?? 999,
                                SpoolID = sp.SpoolID,
                                TotalPeqs = j != null ? (j.SumaDecimal.HasValue ? j.SumaDecimal.Value : 0) : 0,
                                TotalTubo = a != null ? a.CuentaTubo : 0,
                                LongitudTubo = a != null ? a.SumaTubo : 0,
                                TotalAccesorio = a != null ? a.Cuenta : 0,
                                DiametroPlano = sp.DiametroPlano,
                                Hold = o != null ? true : false,
                                ObservacionesHold = o != null ? o.Observaciones : String.Empty
                            }).AsParallel().ToList();

                }

                else
                {
                    sw.Restart();
                    _logger.DebugFormat("return final");
                    //Regresar objetos del tipo que se necesitan buscando en Cache las familias de acero
                    return (from sp in spools
                            let j = cuentaPeqs.SingleOrDefault(x => x.ID == sp.SpoolID)
                            let a = cuentaMateriales.SingleOrDefault(x => x.ID == sp.SpoolID)
                            select new GrdCruce
                            {
                                Area = sp.Area ?? 0,
                                Cedula = sp.Cedula,
                                Dibujo = sp.Dibujo,
                                FamiliaAcero1 = famAceros[sp.FamiliaAcero1ID],
                                FamiliaAcero1ID = sp.FamiliaAcero1ID,
                                FamiliaAcero2 = sp.FamiliaAcero2ID.HasValue ? famAceros[sp.FamiliaAcero2ID.Value] : string.Empty,
                                FamiliaAcero2ID = sp.FamiliaAcero2ID ?? -1,
                                Juntas = j != null ? j.Cuenta : 0, //Cuenta de juntas
                                Nombre = sp.Nombre,
                                Pdis = sp.Pdis ?? 0,
                                Peso = sp.Peso ?? 0,
                                Prioridad = sp.Prioridad ?? 999,
                                SpoolID = sp.SpoolID,
                                TotalPeqs = j != null ? (j.SumaDecimal.HasValue ? j.SumaDecimal.Value : 0) : 0,
                                TotalTubo = a != null ? a.CuentaTubo : 0,
                                LongitudTubo = a != null ? a.SumaTubo : 0,
                                TotalAccesorio = a != null ? a.Cuenta : 0,
                                DiametroPlano = sp.DiametroPlano,
                                Hold = false,
                                ObservacionesHold = String.Empty
                            }).AsParallel().ToList();
                }
            }
        }

        /// <summary>
        /// Obtiene un listado de objetos de tipo GrdIngSpool que contienen
        /// información "plana" de un spool referente a ingeniería:
        /// - Holds
        /// - Familias de acero
        /// - Número de ODT
        /// 
        /// ----> JHT -- 21/03/2014 -- Agrego parametro esRevision, para que este metodo solo traiga información
        /// ---->                      de los Spool que estan marcados como revisión
        /// 
        /// Este método no regresa las relaciones del spool tales como sus materiales, juntas y cortes.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto del cual se desea obtener los spools</param>
        /// <param name="esRevision">Enviar True si solo se quieren los Spool marcados como revisión</param>
        /// <returns>Lista de objetos tipo GrdIngSpool para el proyecto solicitado</returns>
        public List<GrdIngSpool> ObtenerIngPorProyecto(int proyectoID, bool? esRevision = false)
        {
            List<Spool> spools = null;
            List<SpoolHold> holds = null;
            List<OrdenTrabajoSpool> ots = null;
            List<JuntaSpool> js = null;
            List<SpoolHoldHistorial> sHoldHistorial = null;
            _logger.DebugFormat("Inicio ObtenerIngPorProyecto");
           
                using (SamContext ctx = new SamContext())
                {
                    
                    ctx.Spool.MergeOption = MergeOption.NoTracking;
                    ctx.SpoolHold.MergeOption = MergeOption.NoTracking;
                    ctx.OrdenTrabajoSpool.MergeOption = MergeOption.NoTracking;
                    sw.Start();
                    IQueryable<int> query = ctx.Spool.Where(x => x.ProyectoID == proyectoID).Select(x => x.SpoolID);
                    _logger.DebugFormat("query");
                    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
                    //Traer los registros de spool
                    if (esRevision.HasValue && esRevision.Value == true)
                    {
                        //spools = ctx.Spool.Where(x => x.ProyectoID == proyectoID && x.EsRevision == esRevision).ToList();
                    }
                    else
                    {
                        sw.Restart();
                        spools = ctx.Spool.Where(x => x.ProyectoID == proyectoID).ToList();
                        _logger.DebugFormat("spools");
                        _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
                    }
                    //traer la información de holds al contexto
                    sw.Restart();
                    holds = ctx.SpoolHold.Where(x => query.Contains(x.SpoolID)).ToList();
                    _logger.DebugFormat("holds");
                    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

                    //Traer las ordenes de trabajo en caso que sean necesarias al contexto
                    sw.Restart();
                    ots = ctx.OrdenTrabajoSpool.Where(x => query.Contains(x.SpoolID)).ToList();
                    _logger.DebugFormat("ots");
                    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

                    sw.Restart();
                    js = ctx.JuntaSpool.Where(x => query.Contains(x.SpoolID)).ToList();
                    _logger.DebugFormat("js");
                    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

                    sw.Restart();
                    sHoldHistorial = ctx.SpoolHoldHistorial.Where(x => query.Contains(x.SpoolID)).ToList();
                    _logger.DebugFormat("sHoldHistorial");
                    _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
                }

                sw.Restart();
                var peqs = (from j in js
                            group j by j.SpoolID into g
                            select new
                            {
                                SpoolID = g.Key,
                                TotalPeq = g.Sum(x => x.Peqs)
                            }).ToList();

                _logger.DebugFormat("peqs");
                _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);

                sw.Restart();
                Dictionary<int, string> famAceros = CacheCatalogos.Instance
                                                      .ObtenerFamiliasAcero()
                                                      .ToDictionary(x => x.ID, y => y.Nombre);
                 _logger.DebugFormat("famAceros");
                _logger.DebugFormat("Tiempo: {0}", sw.ElapsedMilliseconds);
                   
                
             List<GrdIngSpool> grdIngSpool = (from sp in spools
                                                 let ot = ots.SingleOrDefault(x => x.SpoolID == sp.SpoolID)
                                                 let hold = holds.SingleOrDefault(x => x.SpoolID == sp.SpoolID)
                                                 let j = peqs.SingleOrDefault(x => x.SpoolID == sp.SpoolID)
                                                 let h = sHoldHistorial.Where(x => x.SpoolID == sp.SpoolID).OrderByDescending(x => x.SpoolHoldHistorialID).FirstOrDefault()

                                                 select new GrdIngSpool
                                                 {
                                                     AprobadoParaCruce = sp.AprobadoParaCruce,
                                                     Area = sp.Area ?? 0,
                                                     Cedula = sp.Cedula,
                                                     Confinado = hold != null ? hold.Confinado : false,
                                                     DiametroPlano = sp.DiametroPlano ?? 0,
                                                     DiametroMayor = sp.DiametroMayor ?? 0,//sp.DiametroMayor ?? 0,
                                                     Dibujo = sp.Dibujo,
                                                     Especificacion = sp.Especificacion,
                                                     FamiliaAcero1 = famAceros[sp.FamiliaAcero1ID],
                                                     FamiliaAcero2 = sp.FamiliaAcero2ID.HasValue ? famAceros[sp.FamiliaAcero2ID.Value] : string.Empty,
                                                     Nombre = sp.Nombre,
                                                     NumeroControl = ot != null ? ot.NumeroControl : string.Empty,
                                                     Pdis = sp.Pdis ?? 0,
                                                     Peso = sp.Peso ?? 0,
                                                     PorcentajePnd = sp.PorcentajePnd ?? 0,
                                                     Prioridad = sp.Prioridad ?? 999,
                                                     ProyectoID = sp.ProyectoID,
                                                     RequierePwht = sp.RequierePwht,
                                                     PendienteDocumental = sp.PendienteDocumental,
                                                     RevisionSteelgo = sp.Revision,
                                                     RevisionCliente = sp.RevisionCliente,
                                                     Segmento1 = sp.Segmento1,
                                                     Segmento2 = sp.Segmento2,
                                                     Segmento3 = sp.Segmento3,
                                                     Segmento4 = sp.Segmento4,
                                                     Segmento5 = sp.Segmento5,
                                                     Segmento6 = sp.Segmento6,
                                                     Segmento7 = sp.Segmento7,
                                                     SpoolID = sp.SpoolID,
                                                     TieneHoldCalidad = hold != null ? hold.TieneHoldCalidad : false,
                                                     TieneHoldIngenieria = hold != null ? hold.TieneHoldIngenieria : false,
                                                     TotalPeq = j != null ? j.TotalPeq : 0,
                                                     ObservacionesHold = h != null ? h.Observaciones : string.Empty,
                                                     FechaHold = h != null ? h.FechaHold.ToString() : string.Empty,
                                                     FechaImportacion = sp.FechaImportacion != null ? sp.FechaImportacion.ToString() : string.Empty,
                                                     //EsRevision = sp != null ? sp.EsRevision : false
                                                 }).AsParallel().ToList();
              
                return (grdIngSpool);
           
        }


        /// <summary>
        /// Obtiene un listado de objetos de tipo GrdIngSpool que contienen
        /// información "plana" de un spool referente a ingeniería:
        /// - Holds
        /// - Familias de acero
        /// - Número de ODT
        /// 
        /// Este método no regresa las relaciones del spool tales como sus materiales, juntas y cortes.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto del cual se desea obtener los spools</param>
        /// <param name="spoolHistoricoID">ID del spool historico proyecto del cual se desea obtener las revisiones realizadas</param>
        /// <returns>Lista de objetos tipo GrdIngSpool para el proyecto solicitado</returns>
        public List<GrdIngSpool> ObtenerHistoricoSpoolPorProyecto(int ProjectoID, string SpoolNombre)
        {
            List<SpoolHistorico> historicoSpool = null;

            using (SamContext ctx = new SamContext())
            {
                ctx.SpoolHistorico.MergeOption = MergeOption.NoTracking;

                //Traer los registros de spool
                historicoSpool = ctx.SpoolHistorico.Where(x => x.ProyectoID == ProjectoID && x.Nombre == SpoolNombre).ToList();
            }

            Dictionary<int, string> famAceros = CacheCatalogos.Instance
                                                  .ObtenerFamiliasAcero()
                                                  .ToDictionary(x => x.ID, y => y.Nombre);

            return (from sp in historicoSpool
                    select new GrdIngSpool
                    {
                        AprobadoParaCruce = sp.AprobadoParaCruce,
                        Area = sp.Area ?? 0,
                        Cedula = sp.Cedula,
                        DiametroPlano = sp.DiametroPlano ?? 0,
                        Dibujo = sp.Dibujo,
                        Especificacion = sp.Especificacion,
                        FamiliaAcero1 = famAceros[sp.FamiliaAcero1ID],
                        FamiliaAcero2 = sp.FamiliaAcero2ID.HasValue ? famAceros[sp.FamiliaAcero2ID.Value] : string.Empty,
                        Nombre = sp.Nombre,
                        Pdis = sp.Pdis ?? 0,
                        Peso = sp.Peso ?? 0,
                        PorcentajePnd = sp.PorcentajePnd ?? 0,
                        Prioridad = sp.Prioridad ?? 999,
                        ProyectoID = sp.ProyectoID,
                        RequierePwht = sp.RequierePwht,
                        PendienteDocumental = sp.PendienteDocumental,
                        RevisionSteelgo = sp.Revision,
                        RevisionCliente = sp.RevisionCliente,
                        Segmento1 = sp.Segmento1,
                        Segmento2 = sp.Segmento2,
                        Segmento3 = sp.Segmento3,
                        Segmento4 = sp.Segmento4,
                        Segmento5 = sp.Segmento5,
                        Segmento6 = sp.Segmento6,
                        Segmento7 = sp.Segmento7,
                        SpoolID = sp.SpoolHistoricoID
                    }).AsParallel().ToList();
        }

        /// <summary>
        /// Obtiene una lista de los spools de un proyecto.
        /// Este método sólo regresa la entidad spool sin ninguna de sus relaciones.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto del cual se desea obtener los spools</param>
        /// <returns>Lista de los spools del proyecto seleccionado</returns>
        public List<Spool> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Spool.Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }

        /// <summary>
        /// Obtiene un objeto DetSpool que contiene la siguiente información del spool:
        /// - Datos de la tabla spool
        /// - Familias de material
        /// - Materiales del spool con sus item codes y familias
        /// - Juntas del spool con su tipo de junta y material
        /// - Cortes del spool con su item code y tipo de corte
        /// - Información de hold del spool
        /// - Historial de holds del spool
        /// </summary>
        /// <param name="spoolID">ID del spool del cual se desea obtener información</param>
        /// <returns>Objeto de tipo DetSpool con casi toda la información de ingeniería</returns>
        public DetSpool ObtenerDetalleCompleto(int spoolID)
        {
            Spool sp = SpoolBO.Instance.ObtenerDetalle(spoolID);
            return Mapper.MapearDesdeSpool(sp);
        }

        /// <summary>
        /// Obtiene un objeto Spool que contiene la siguiente información del spool:
        /// - Datos de la tabla spool (sin familias)
        /// - Materiales del spool con sus item codes (sin familias)
        /// - Juntas del spool (sin familia y sin tipo de junta)
        /// - Cortes del spool (sin item code y sin tipo de corte)
        /// - Información de hold del spool
        /// - Historial de holds del spool
        /// </summary>
        /// <param name="spoolID">ID del spool del cual se desea obtener información</param>
        /// <returns>Objeto de tipo Spool con casi toda la información de ingeniería</returns>        
        public Spool ObtenerDetalle(int spoolID)
        {
            Spool sp = null;

            using (SamContext ctx = new SamContext())
            {
                //Traerme el spool
                sp = ctx.Spool.Where(x => x.SpoolID == spoolID).Single();

                //Llenar sus colecciones
                ctx.LoadProperty<Spool>(sp, x => x.MaterialSpool);
                ctx.LoadProperty<Spool>(sp, x => x.CorteSpool);
                ctx.LoadProperty<Spool>(sp, x => x.JuntaSpool);
                ctx.LoadProperty<Spool>(sp, x => x.SpoolHold);
                ctx.LoadProperty<Spool>(sp, x => x.SpoolHoldHistorial);

                //Preparar para subquery de item codes
                IQueryable<int> icMateriales = ctx.MaterialSpool
                                                  .Where(x => x.SpoolID == spoolID)
                                                  .Select(y => y.ItemCodeID);

                IQueryable<int> icCortes = ctx.CorteSpool
                                              .Where(x => x.SpoolID == spoolID)
                                              .Select(y => y.ItemCodeID);

                //Traer los item codes al contexto, esto va a hacer que el grafo del spool se
                //complete con los item codes que necesita
                ctx.ItemCode.Where(x => icMateriales.Contains(x.ItemCodeID) || icCortes.Contains(x.ItemCodeID)).ToList();

                //Agregamos el tipo de corte para tener la descripcion
                ctx.TipoCorte.ToList();

            }

            return sp;
        }

        public DetSpoolHold ObtenerHolds(int spoolID)
        {
            Spool sp = null;

            using (SamContext ctx = new SamContext())
            {
                //Traerme el spool
                sp = ctx.Spool.Where(x => x.SpoolID == spoolID).Single();

                //Llenar sus colecciones                
                ctx.LoadProperty<Spool>(sp, x => x.SpoolHold);
                ctx.LoadProperty<Spool>(sp, x => x.SpoolHoldHistorial);

            }

            return Mapper.MapearDesdeSpoolHolds(sp);

        }


        /// <summary>
        /// Obtiene un objeto DetSpool que contiene la siguiente información del spool:
        /// - Datos de la tabla spool
        /// - Familias de material
        /// - Materiales del spool con sus item codes y familias
        /// - Juntas del spool con su tipo de junta y material
        /// - Cortes del spool con su item code y tipo de corte
        /// - Información de hold del spool
        /// - Historial de holds del spool
        /// </summary>
        /// <param name="spoolID">ID del spool del cual se desea obtener información</param>
        /// <returns>Objeto de tipo DetSpool con casi toda la información de ingeniería</returns>
        public DetSpoolHistorico ObtenerDetalleCompletoHistorico(int spoolID)
        {
            SpoolHistorico sp = SpoolBO.Instance.ObtenerDetalleHistorico(spoolID);
            return Mapper.MapearDesdeSpoolHistorico(sp);
        }

        /// <summary>
        /// Obtiene un objeto Spool que contiene la siguiente información del spool:
        /// - Datos de la tabla spool (sin familias)
        /// - Materiales del spool con sus item codes (sin familias)
        /// - Juntas del spool (sin familia y sin tipo de junta)
        /// - Cortes del spool (sin item code y sin tipo de corte)
        /// - Información de hold del spool
        /// - Historial de holds del spool
        /// </summary>
        /// <param name="spoolID">ID del spool del cual se desea obtener información</param>
        /// <returns>Objeto de tipo Spool con casi toda la información de ingeniería</returns>        
        public SpoolHistorico ObtenerDetalleHistorico(int spoolID)
        {
            SpoolHistorico sp = null;

            using (SamContext ctx = new SamContext())
            {
                //Traerme el spool
                sp = ctx.SpoolHistorico.Where(x => x.SpoolHistoricoID == spoolID).Single();

                //Llenar sus colecciones
                ctx.LoadProperty<SpoolHistorico>(sp, x => x.MaterialSpoolHistorico);
                ctx.LoadProperty<SpoolHistorico>(sp, x => x.CorteSpoolHistorico);
                ctx.LoadProperty<SpoolHistorico>(sp, x => x.JuntaSpoolHistorico);

                //Preparar para subquery de item codes
                IQueryable<int> icMateriales = ctx.MaterialSpoolHistorico
                                                  .Where(x => x.SpoolHistoricoID == spoolID)
                                                  .Select(y => y.ItemCodeID);

                IQueryable<int> icCortes = ctx.CorteSpoolHistorico
                                              .Where(x => x.SpoolHistoricoID == spoolID)
                                              .Select(y => y.ItemCodeID);

                //Traer los item codes al contexto, esto va a hacer que el grafo del spool se
                //complete con los item codes que necesita
                ctx.ItemCode.Where(x => icMateriales.Contains(x.ItemCodeID) || icCortes.Contains(x.ItemCodeID)).ToList();

                //Agregamos el tipo de corte para tener la descripcion
                ctx.TipoCorte.ToList();

            }

            return sp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID">ID del spool del cual se desea obtener información</param>
        /// <returns>Objeto de tipo Spool con casi toda la información de ingeniería</returns>        
        public Spool ObtenerDetalleHomologacion(int spoolID)
        {
            Spool sp = null;

            using (SamContext ctx = new SamContext())
            {
                //Traerme el spool
                sp = ctx.Spool.Where(x => x.SpoolID == spoolID).Single();

                //Llenar sus colecciones
                ctx.LoadProperty<Spool>(sp, x => x.MaterialSpool);
                ctx.LoadProperty<Spool>(sp, x => x.CorteSpool);
                ctx.LoadProperty<Spool>(sp, x => x.JuntaSpool);
                ctx.LoadProperty<Spool>(sp, x => x.SpoolHold);

                //Preparar para subquery de item codes
                IQueryable<MaterialSpool> iqMateriales = ctx.MaterialSpool
                                                  .Where(x => x.SpoolID == spoolID);

                IQueryable<int> icCortes = ctx.CorteSpool
                                              .Where(x => x.SpoolID == spoolID)
                                              .Select(y => y.ItemCodeID);

                IQueryable<int> icJuntas = ctx.JuntaSpool
                                              .Where(x => x.SpoolID == spoolID)
                                              .Select(y => y.JuntaSpoolID);

                //Traer los item codes al contexto, esto va a hacer que el grafo del spool se
                //complete con los item codes que necesita
                IQueryable<int> itemcode = from item in ctx.ItemCode
                                           where iqMateriales.Select(y => y.ItemCodeID).Contains(item.ItemCodeID) || icCortes.Contains(item.ItemCodeID)
                                           select item.ItemCodeID;
                

                ctx.ItemCode.Where(
                    x =>
                    itemcode.Contains(x.ItemCodeID)).
                    ToList();

                //Agregamos el tipo de corte para tener la descripcion
                ctx.TipoCorte.ToList();
                //Agregamos el tipo de Junta para tener la descripcion
                ctx.TipoJunta.ToList();
                ctx.FabArea.ToList();
                ctx.FamiliaAcero.ToList();
                //para saber si tiene cortes o despacho
                ctx.OrdenTrabajoMaterial.Where(
                    x => iqMateriales.Select(y => y.MaterialSpoolID).Contains(x.MaterialSpoolID)).ToList();
                //para saber si la junta fue armada o soldada
                ctx.JuntaWorkstatus.Where(x => icJuntas.Contains(x.JuntaSpoolID)).ToList();

            }

            return sp;
        }

        /// <summary>
        /// Obtiene un listado de spools por proyecto con sus relaciones a las siguientes tables:
        /// - Juntas
        /// - Cortes
        /// - Materiales
        /// 
        /// Este método no avienta información de catálogos en caso de usarse y necesitarse se debe
        /// mappear desde Cache.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto cuyos spools se desean obtener</param>
        /// <returns>Lista de spools del proyecto con sus materiales, juntas y cortes</returns>
        public List<Spool> ObtenerConJuntaMaterialCortePorProyecto(int proyectoID)
        {
            List<Spool> spools = null;

            using (SamContext ctx = new SamContext())
            {
                IQueryable<int> iqSpoolIds = ctx.Spool.Where(x => x.ProyectoID == proyectoID).Select(x => x.SpoolID);
                IQueryable<int> iqJuntaSpooldIds = ctx.JuntaSpool.Where(x => iqSpoolIds.Contains(x.SpoolID)).Select(x => x.JuntaSpoolID);


                //Traer los registros de spool
                spools = ctx.Spool.Where(x => x.ProyectoID == proyectoID).ToList();

                //Traer juntas, materiales y cortes al contexto para los spools del proyecto únicamente

                ctx.JuntaSpool.Where(x => iqSpoolIds.Contains(x.SpoolID)).ToList();
                ctx.CorteSpool.Where(x => iqSpoolIds.Contains(x.SpoolID)).ToList();
                IQueryable<MaterialSpool> iqMatSpool = ctx.MaterialSpool.Where(x => iqSpoolIds.Contains(x.SpoolID));
                ctx.ItemCode.Where(x => iqMatSpool.Select(y => y.ItemCodeID).Contains(x.ItemCodeID)).ToList();
                iqMatSpool.ToList();

                //traer ordenes de trabajo spool material y junta
                ctx.OrdenTrabajoMaterial.Where(
                    x => iqMatSpool.Select(y => y.MaterialSpoolID).Contains(x.MaterialSpoolID)).ToList();
                ctx.OrdenTrabajoJunta.Where(x => iqJuntaSpooldIds.Contains(x.JuntaSpoolID)).ToList();
                ctx.OrdenTrabajoSpool.Where(x => iqSpoolIds.Contains(x.SpoolID)).ToList();
                ctx.JuntaWorkstatus.Where(x => iqJuntaSpooldIds.Contains(x.JuntaSpoolID)).ToList();
            }

            return spools;
        }


        public void GuardaSpoolEditadoConOdt(Spool spool, List<int> materialIds, List<int> materialesEliminadosIds, List<int> juntasEliminadasIds, Guid userGuid)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {

                    #region Procesa materiales editados y / o eliminados

                    NumeroUnicoInventario nui;
                    NumeroUnicoSegmento nus;

                    if (materialIds.Count() > 0 || materialesEliminadosIds.Count > 0)
                    {
                        IQueryable<OrdenTrabajoMaterial> iqOdtm = ctx.OrdenTrabajoMaterial.Where(x => materialIds.Contains(x.MaterialSpoolID) || materialesEliminadosIds.Contains(x.MaterialSpoolID));

                        #region Despachos Cortes Cancelados
                        //Eliminamos los posibles cortes y/o despachos cancelados de los materiales eliminados.
                        List<Despacho> despachosCancelados = ctx.Despacho.Where(x => materialesEliminadosIds.Contains(x.MaterialSpoolID) && x.Cancelado).ToList();
                        despachosCancelados.ForEach(x => ctx.DeleteObject(x));
                        List<CorteDetalle> cortesCancelados = ctx.CorteDetalle.Where(x => materialesEliminadosIds.Contains(x.MaterialSpoolID) && x.Cancelado).ToList();
                        cortesCancelados.ForEach(x => ctx.DeleteObject(x));
                        #endregion

                        List<OrdenTrabajoMaterial> lstOdtm = iqOdtm.ToList();

                        List<NumeroUnicoInventario> lstNui = ctx.NumeroUnicoInventario
                                                               .Where(x => iqOdtm.Select(y => y.NumeroUnicoCongeladoID)
                                                               .Contains(x.NumeroUnicoID))
                                                               .ToList();


                        List<NumeroUnicoSegmento> lstNus = ctx.NumeroUnicoSegmento
                                                              .Where(x => iqOdtm.Select(y => y.NumeroUnicoCongeladoID)
                                                              .Contains(x.NumeroUnicoID))
                                                              .ToList();

                       
                        lstOdtm.ForEach(x =>
                        {
                            if (x.CantidadCongelada.HasValue)
                            {
                                nui = lstNui.Where(y => y.NumeroUnicoID == x.NumeroUnicoCongeladoID).SingleOrDefault();

                                if (nui != null)
                                {
                                    nui.StartTracking();
                                    nui.InventarioDisponibleCruce += x.CantidadCongelada.Value;
                                    nui.InventarioCongelado -= x.CantidadCongelada.Value;
                                    nui.UsuarioModifica = userGuid;
                                    nui.FechaModificacion = DateTime.Now;
                                    nui.StopTracking();

                                    ctx.NumeroUnicoInventario.ApplyChanges(nui);
                                }

                                if (x.SegmentoCongelado != null)
                                {
                                    nus = lstNus.Where(y => y.NumeroUnicoID == x.NumeroUnicoCongeladoID & y.Segmento == x.SegmentoCongelado).SingleOrDefault();

                                    if (nus != null)
                                    {
                                        nus.StartTracking();
                                        nus.InventarioDisponibleCruce += x.CantidadCongelada.Value;
                                        nus.InventarioCongelado -= x.CantidadCongelada.Value;
                                        nus.UsuarioModifica = userGuid;
                                        nus.FechaModificacion = DateTime.Now;
                                        nus.StartTracking();

                                        ctx.NumeroUnicoSegmento.ApplyChanges(nus);
                                    }
                                }
                            }

                            
                            ctx.DeleteObject(x);
                        });
                        // ActualizardiametroMayor(spool.SpoolID);
                    }
                    #endregion

                    #region Procesa Juntas eliminadas

                    IQueryable<int> iqJuntasIds = juntasEliminadasIds.AsQueryable();

                    //ELIMINAR AGRUPADORES POR JUNTA
                    List<AgrupadoresPorJunta> agrupadores = ctx.AgrupadoresPorJunta.Where(x => iqJuntasIds.Select(ID => ID).Contains(x.JuntaSpoolID)).ToList();
                    agrupadores.ForEach(x => ctx.DeleteObject(x));

                    //Obtenemos las ODTJ de las JuntaSpool que se eliminaron
                    List<OrdenTrabajoJunta> lstOdtj = ctx.OrdenTrabajoJunta
                                                         .Where(x => iqJuntasIds.Select(ID => ID)
                                                         .Contains(x.JuntaSpoolID))
                                                         .ToList();
                    //Borramos las ODTJ
                    lstOdtj.ForEach(x => ctx.DeleteObject(x));

                    //Obtenemos los registros en JuntaWorkstatus de las JuntaSpool que se eliminaron
                    List<JuntaWorkstatus> lstJw = ctx.JuntaWorkstatus
                                                     .Where(x => iqJuntasIds.Select(ID => ID)
                                                     .Contains(x.JuntaSpoolID))
                                                     .ToList();

                    //Borramos los JWS
                    lstJw.ForEach(x => ctx.DeleteObject(x));

                    

                    #endregion

                    #region Procesa Juntas Editadas
                    //Solo en caso de que ya tengo JuntaWorkstatus (puede ser cuando la junta ya haya tenido despachos y luego se hayan cancelado)
                    //en caso que la etiqueta haya sido modificada se modifica en JuntaWorkstatus
                    foreach (JuntaSpool junta in spool.JuntaSpool)
                    {
                        List<JuntaWorkstatus> juntaList = ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == junta.JuntaSpoolID).ToList();
                        if (juntaList.Count() == 1)
                        {
                            JuntaWorkstatus juntaWks = juntaList.Single();
                            if (juntaWks.EtiquetaJunta != junta.Etiqueta)
                            {
                                juntaWks.EtiquetaJunta = junta.Etiqueta;
                                ctx.JuntaWorkstatus.ApplyChanges(juntaWks);
                            }
                        }
                    }
                    #endregion

                    spool.DiametroMayor = spool.MaterialSpool.Select(x => x.Diametro1).Max();// ctx.MaterialSpool.Where(x => x.SpoolID == _spoolId).Select(x => x.Diametro1).Max();

                    ctx.Spool.ApplyChanges(spool);
                    ctx.SaveChanges();


                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Actualiza el diametro mayor en la tabla Spool despues de que se ha hecho alguna modificacion a los materiales
        /// </summary>
        /// <param name="_spoolId"></param>
        /// <returns></returns>
        //public bool ActualizardiametroMayor(Spool spool)
        //{

        //        spool.DiametroMayor = spool.MaterialSpool.Select(x => x.Diametro1).Max();// ctx.MaterialSpool.Where(x => x.SpoolID == _spoolId).Select(x => x.Diametro1).Max();
        //        ctx.Spool.ApplyChanges(spool);
        //        ctx.SaveChanges();
        //        return true;
        //    }

        //}

        /// <summary>
        /// Guarda el spool y los objetos anexados en su grafo en la BD.
        /// </summary>
        /// <param name="spool">Objeto de tipo spool para guardar en la BD</param>
        public void Guarda(Spool spool)
        {

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    spool.DiametroMayor = spool.MaterialSpool.Select(x => x.Diametro1).Max();// ctx.MaterialSpool.Where(x => x.SpoolID == _spoolId).Select(x => x.Diametro1).Max();
                    ctx.Spool.ApplyChanges(spool);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
            catch (UpdateException ue)
            {
                throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_SpoolDuplicado, spool.Nombre));
            }
        }

        public void EliminarAgrupadoresPorJunta(List<int> juntaSpoolIds)
        {
            using (SamContext ctx = new SamContext())
            {
                //recuperamos los agrupadores a eliminar
                List<AgrupadoresPorJunta> lista = (from agj in ctx.AgrupadoresPorJunta
                                                   where juntaSpoolIds.Contains(agj.JuntaSpoolID)
                                                   select agj
                                                  ).ToList();
                if (lista.Count > 0)
                {
                    lista.ForEach(x => ctx.DeleteObject(x));
                    ctx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Borra el spool con sus materiales, cortes y juntas, siempre y cuando no tenga ODT
        /// </summary>
        /// <param name="spoolID"></param>
        public void Borra(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesSpool.TieneODT(ctx, spoolID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneOrdenTrabajo);
                }

                //Obtenermos los Materiales que pertenecen al spool para borrarlos
                List<MaterialSpool> lstMaterialSpool = ctx.MaterialSpool.Where(x => x.SpoolID == spoolID).ToList();
                foreach (MaterialSpool ms in lstMaterialSpool)
                {
                    ctx.DeleteObject(ms);
                }

                //Obtenermos los Cortes que pertenecen al spool para borrarlos
                List<CorteSpool> lstCorteSpool = ctx.CorteSpool.Where(x => x.SpoolID == spoolID).ToList();
                foreach (CorteSpool cs in lstCorteSpool)
                {
                    ctx.DeleteObject(cs);
                }

                //Obtenermos las Juntas que pertenecen al spool para borrarlos
                IQueryable<JuntaSpool> lstJuntaSpool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID).AsQueryable();
                //ANTES DE BORRAR LAS JUNTAS HAY QUE BORRAR LOS REGISTROS DE AGRUPADORES POR JUNTA
                List<AgrupadoresPorJunta> agrupadores = (from a in ctx.AgrupadoresPorJunta
                                                         join j in lstJuntaSpool on a.JuntaSpoolID equals j.JuntaSpoolID
                                                         select a).ToList();
                if (agrupadores.Count > 0)
                {
                    //SI EXISTE ALGUN AGRUPADOR LO ELIMINAMOS
                    foreach (AgrupadoresPorJunta ag in agrupadores)
                    {
                        ctx.DeleteObject(ag);
                    }
                }

                //Eliminar agrupadores Spool PND
                List<AgrupadoresSpoolPND> agrupadoresPND = ctx.AgrupadoresSpoolPND.Where(x => x.SpoolID == spoolID).ToList();
                foreach (AgrupadoresSpoolPND s in agrupadoresPND)
                {
                    ctx.DeleteObject(s);
                }

                //ELIMINAMOS LAS JUNTAS
                foreach (JuntaSpool js in lstJuntaSpool)
                {
                    ctx.DeleteObject(js);
                }

                //Obtenemos los SpoolHolds para borrarlos
                List<SpoolHold> lstSpoolHold = ctx.SpoolHold.Where(x => x.SpoolID == spoolID).ToList();
                foreach (SpoolHold sh in lstSpoolHold)
                {
                    ctx.DeleteObject(sh);
                }

                //Borramos el historial del spool
                List<SpoolHoldHistorial> lstSpoolHoldHistorial = ctx.SpoolHoldHistorial.Where(x => x.SpoolID == spoolID).ToList();
                foreach (SpoolHoldHistorial shh in lstSpoolHoldHistorial)
                {
                    ctx.DeleteObject(shh);
                }

                Spool sp = ctx.Spool.Where(x => x.SpoolID == spoolID).SingleOrDefault();
                ctx.DeleteObject(sp);
                ctx.SaveChanges();
            }

        }

        /// <summary>
        /// Obtiene los spools de un proyecto en particular que ya tienen despacho.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns>Spools del proyecto que ya tienen despacho</returns>
        public List<Spool> ObtenerSpoolsConDespachoOCortePorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {

                return
                    ctx.Spool.Where(
                        x => ctx.OrdenTrabajoSpool.Where(
                            //TODO: comentar
                            y =>
                            y.OrdenTrabajoMaterial.Any(q => q.DespachoID != null || q.CorteDetalleID != null)
                            &&
                            ctx.OrdenTrabajo.Where(
                                z => z.ProyectoID == proyectoID).Select(a => a.OrdenTrabajoID).Contains(y.OrdenTrabajoID)
                                 ).Select(b => b.SpoolID).Contains(x.SpoolID)).ToList();
            }
        }

        /// <summary>
        /// Establece la prioridad al numero deseado para los spools pasados.
        /// El query filtra por proyecto para asegurarse que no haya cambios multi-proyecto
        /// de igual manera filtra solo aquellos spools que no tengan orden de trabajo.
        /// </summary>
        /// <param name="spoolIds">Arreglo que contiene los spools que se deben cambiar</param>
        /// <param name="proyectoID">ID del proyecto al cual pertenecen los spools</param>
        /// <param name="userID">Usuario que indica el cambio</param>
        /// <param name="fechaModificacion">Fecha en la cual se lleva a cabo la modificación</param>
        /// <param name="prioridad">Prioridad a establecer</param>
        public void FijaPrioridad(int[] spoolIds, int proyectoID, Guid userID, DateTime fechaModificacion, int prioridad)
        {
            //Si la prioridad sale de tolerancias se arroja una excepción
            if (prioridad < 0 || prioridad > 999)
            {
                throw new ExcepcionPrioridad(MensajesError.Excepcion_PrioridadInvalida);
            }

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    IQueryable<int> iqSpoolID = spoolIds.AsQueryable();

                    //Solo nos interesan los spools del proyecto que nos hayan pasado y
                    List<Spool> spProyecto = ctx.Spool
                                                .Where(x => x.ProyectoID == proyectoID)
                                                .Where(x => iqSpoolID.Contains(x.SpoolID))
                                                .ToList();

                    //Cambiar los spools que nos trajimos de la BD y aplicar los cambios al contexto
                    spProyecto.ForEach(x =>
                    {
                        x.StartTracking();
                        x.Prioridad = prioridad;
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = fechaModificacion;
                        x.StopTracking();
                        ctx.Spool.ApplyChanges(x);
                    });

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Establece Aprobado para Cruce para los spools pasados.
        /// El query filtra por proyecto para asegurarse que no haya cambios multi-proyecto
        /// de igual manera filtra solo aquellos spools que no tengan orden de trabajo.
        /// </summary>
        /// <param name="spoolIds">Arreglo que contiene los spools que se deben cambiar</param>
        /// <param name="proyectoID">ID del proyecto al cual pertenecen los spools</param>
        /// <param name="userID">Usuario que indica el cambio</param>
        /// <param name="fechaModificacion">Fecha en la cual se lleva a cabo la modificación</param>
        /// <param name="Aprobado">Aprobado para cruce a establecer</param>
        public void FijaAprobadoParaCruce(int[] spoolIds, int proyectoID, Guid userID, DateTime fechaModificacion, bool Aprobado)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    IQueryable<int> iqSpoolID = spoolIds.AsQueryable();

                    //Solo nos interesan los spools del proyecto que nos hayan pasado y que
                    //adicionalmente no tengan ya ODT
                    List<Spool> spProyecto = ctx.Spool
                                                .Where(x => x.ProyectoID == proyectoID)
                                                .Where(x => iqSpoolID.Contains(x.SpoolID))
                                                .Where(x => !ctx.OrdenTrabajoSpool.Any(y => y.SpoolID == x.SpoolID))
                                                .ToList();

                    //Cambiar los spools que nos trajimos de la BD y aplicar los cambios al contexto
                    spProyecto.ForEach(x =>
                    {
                        x.StartTracking();
                        x.AprobadoParaCruce = Aprobado;
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = fechaModificacion;
                        x.StopTracking();
                        ctx.Spool.ApplyChanges(x);
                    });

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Establece Documentación Aprobada para los spools pasados.
        /// </summary>
        /// <param name="spoolIds">Arreglo que contiene los spools que se deben cambiar</param>
        /// <param name="proyectoID">ID del proyecto al cual pertenecen los spools</param>
        /// <param name="userID">Usuario que indica el cambio</param>
        /// <param name="fechaModificacion">Fecha en la cual se lleva a cabo la modificación</param>
        /// <param name="Aprobado">Documentación Aprobada</param>
        public void FijaDocumentacionAprobada(int[] spoolIds, int proyectoID, Guid userID, DateTime fechaModificacion, bool Aprobado)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    IQueryable<int> iqSpoolID = spoolIds.AsQueryable();

                    //Solo nos interesan los spools del proyecto que nos hayan pasado y que
                    //adicionalmente no tengan ya ODT
                    List<Spool> spProyecto = ctx.Spool
                                                .Where(x => x.ProyectoID == proyectoID)
                                                .Where(x => iqSpoolID.Contains(x.SpoolID))
                                                .ToList();

                    //Cambiar los spools que nos trajimos de la BD y aplicar los cambios al contexto
                    spProyecto.ForEach(x =>
                    {
                        x.StartTracking();
                        x.PendienteDocumental = Aprobado;
                        x.UsuarioModifica = userID;
                        x.FechaModificacion = fechaModificacion;
                        x.StopTracking();
                        ctx.Spool.ApplyChanges(x);
                    });

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Obtiene aquellos spools candidatos a incluirse en una nueva orden de trabajo.
        /// Este método se usa para el search as you type de la página de agregar spool a 
        /// ODT, a partir de una llamada a web service.
        /// 
        /// Sólo salen aquellos spools que estén aprobados para el cruce y que no tengan
        /// pendiente documental.  A su vez únicamente los spools que NO están en una ODT
        /// existente son listados y exclusivamente si no están en hold.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto sobre el cual vamos a buscar spools</param>
        /// <param name="nombreSpool">Se utiliza para hacer un filtro de tipo "StartsWith" en el search as you type</param>
        /// <param name="skip">Cantidad de registro a "brincar" cuando se trate de ir por la siguiente página</param>
        /// <param name="take">Cantidad de registros a "tomar" y/o regresar al UI</param>
        public List<Simple> ObtenerSpoolsCandidatosParaOdt(int proyectoID, string nombreSpool, int skip, int take)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Spool.MergeOption = MergeOption.NoTracking;

                IQueryable<Spool> query = from spool in ctx.Spool
                                          let sph = spool.SpoolHold
                                          where spool.ProyectoID == proyectoID
                                                && spool.AprobadoParaCruce
                                                && spool.PendienteDocumental
                                                && !ctx.OrdenTrabajoSpool.Select(y => y.Spool).Contains(spool)
                                                &&
                                                (
                                                    sph == null || (sph != null && !sph.Confinado && !sph.TieneHoldCalidad && !sph.TieneHoldIngenieria)
                                                )
                                          select spool;

                //Intencionalmente hacerlo en la BD
                return query.Where(x => x.Nombre.StartsWith(nombreSpool))
                            .Select(x => new Simple { ID = x.SpoolID, Valor = x.Nombre })
                            .ToList()
                            .OrderBy(x => x.Valor)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
        }

        /// <summary>
        /// Obtiene el SpoolID en base a su nombre
        /// </summary>
        /// <param name="nombre">Nombre del Spool</param>
        /// <returns></returns>
        public int? ObtenerSpoolIDConNombre(string nombre)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Spool.Where(x => x.Nombre == nombre).Select(y => y.SpoolID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene los elementos de JuntaWorkstatus en base a un SpoolID
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <returns></returns>
        public List<JuntaWorkstatus> ObtenerJuntaWorksatusConSpoolID(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<JuntaSpool> juntasSpool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID).ToList();
                List<int> listJuntaSpoolID = juntasSpool.Select(y => y.JuntaSpoolID).ToList();

                return ctx.JuntaWorkstatus.Where(x => listJuntaSpoolID.Contains(x.JuntaSpoolID)).ToList();
            }
        }

        /// <summary>
        /// Obtiene los elementos de JuntaWorkstatus en base a un SpoolID y CodigoFabArea
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <returns></returns>
        public List<JuntaWorkstatus> ObtenerJuntaWorksatusConSpoolIDYCodigoFabArea(int spoolID, int fabAreaID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<JuntaSpool> juntasSpool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID && x.FabAreaID == fabAreaID).ToList();
                List<int> listJuntaSpoolID = juntasSpool.Select(y => y.JuntaSpoolID).ToList();

                return ctx.JuntaWorkstatus.Where(x => listJuntaSpoolID.Contains(x.JuntaSpoolID)).ToList();
            }
        }

        /// <summary>
        /// Obtiene los elementos de JuntaWorkstatus en base a un SpoolID y CodigoFabArea
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <returns></returns>
        public List<JuntaWorkstatus> ObtenerJuntaWorksatusConSpoolIDYCodigoFabAreaYFiltroTiposSoldables(int spoolID, int fabAreaID, int tipoJuntaTHID, int tipoJuntaTWID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<int> eqtp = (new List<int>() { tipoJuntaTHID, tipoJuntaTWID }).AsQueryable();

                List<JuntaSpool> juntasSpool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID && x.FabAreaID == fabAreaID && !eqtp.Contains(x.TipoJuntaID)).ToList();
                List<int> listJuntaSpoolID = juntasSpool.Select(y => y.JuntaSpoolID).ToList();

                return ctx.JuntaWorkstatus.Where(x => listJuntaSpoolID.Contains(x.JuntaSpoolID)).ToList();
            }
        }

        /// <summary>
        /// Obtiene true si el spool tiene hold o esta confinado
        /// </summary>
        /// <param name="spoolID">spoolID</param>
        /// <returns></returns>
        public bool SpoolTieneHold(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.SpoolHold.Where(x => x.SpoolID == spoolID).Select(y => y.TieneHoldCalidad).SingleOrDefault())
                {
                    return true;
                }
                else if (ctx.SpoolHold.Where(x => x.SpoolID == spoolID).Select(y => y.TieneHoldIngenieria).SingleOrDefault())
                {
                    return true;
                }
                else if (ctx.SpoolHold.Where(x => x.SpoolID == spoolID).Select(y => y.Confinado).SingleOrDefault())
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Obtiene para Popup el detalle del seguimiento del spool
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public DetGrdSeguimientoSpool ObtenerDetalleSeguimiento(int spoolID)
        {
            DetGrdSeguimientoSpool ret = new DetGrdSeguimientoSpool();

            List<FamAceroCache> famAceros = CacheCatalogos.Instance.ObtenerFamiliasAcero();

            using (SamContext ctx = new SamContext())
            {

                #region Construimos los queries que enviaremos a BD

                //El spool Solicitado
                IQueryable<Spool> qrySpools =
                    ctx.Spool.Where(x => x.SpoolID == spoolID);

                //Las ordenes de trabajo que involucran a los spools del proyecto
                IQueryable<OrdenTrabajoSpool> iqOrdenTrabajoSpool =
                    ctx.OrdenTrabajoSpool.Where(x => x.SpoolID == spoolID);

                //Las ordenes de trabajo del proyecto
                IQueryable<OrdenTrabajo> iqOrdenTrabajo =
                    ctx.OrdenTrabajo.Where(x => iqOrdenTrabajoSpool.Select(y => y.OrdenTrabajoID).Contains(x.OrdenTrabajoID));

                //Los workstatus que involucran esas ordenes de trabajo
                IQueryable<WorkstatusSpool> iqWorkstatusSpool =
                    ctx.WorkstatusSpool.Where(x => iqOrdenTrabajoSpool.Contains(x.OrdenTrabajoSpool));

                //Los reportes del spool
                IQueryable<ReporteDimensionalDetalle> iqReporteDimensionalDet =
                    ctx.ReporteDimensionalDetalle.Where(
                        x => iqWorkstatusSpool.Select(y => y.WorkstatusSpoolID).Contains(x.WorkstatusSpoolID));
                IQueryable<ReporteDimensional> iqReporteDimensional =
                    ctx.ReporteDimensional.Where(
                        x => iqReporteDimensionalDet.Select(y => y.ReporteDimensionalID).Contains(x.ReporteDimensionalID));

                //Los embarques del spool
                IQueryable<EmbarqueSpool> iqEmbarquesSpools =
                    ctx.EmbarqueSpool.Where(x => iqWorkstatusSpool.Contains(x.WorkstatusSpool));
                IQueryable<Embarque> iqEmbarques = ctx.Embarque.Where(x => iqEmbarquesSpools.Select(y => y.EmbarqueID).Contains(x.EmbarqueID));

                //Las pinturas del proyecto con sus requisiciones y detalles
                IQueryable<PinturaSpool> iqPinturaSpool =
                    ctx.PinturaSpool.Where(x => iqWorkstatusSpool.Contains(x.WorkstatusSpool));
                IQueryable<RequisicionPinturaDetalle> iqPinturaDetalle =
                    ctx.RequisicionPinturaDetalle.Where(x => iqWorkstatusSpool.Contains(x.WorkstatusSpool));
                IQueryable<RequisicionPintura> iqPintura =
                    ctx.RequisicionPintura.Where(
                        x => iqPinturaDetalle.Select(y => y.RequisicionPinturaID).Contains(x.RequisicionPinturaID));

                //Los spools del proyecto que estan en hold
                IQueryable<SpoolHold> iqSpoolHold = ctx.SpoolHold.Where(x => x.SpoolID == spoolID);

                #endregion

                #region  Construimos lookups y diccionarios

                ILookup<int, WorkstatusSpool> lookupWkstatusSpool =
                    iqWorkstatusSpool.ToList().ToLookup(x => x.OrdenTrabajoSpoolID, y => y);

                #endregion

                #region traer objetos al contexto

                qrySpools.ToList();

                iqPinturaDetalle.ToList();
                iqPinturaSpool.ToList();
                iqPintura.ToList();
                iqWorkstatusSpool.ToList();
                iqEmbarques.ToList();
                iqReporteDimensional.ToList();
                iqReporteDimensionalDet.ToList();
                iqOrdenTrabajo.ToList();
                iqOrdenTrabajoSpool.ToList();
                iqEmbarquesSpools.ToList();


                #endregion

                #region ODTSpool

                Spool spool = qrySpools.Single();

                //Un spool solo puede aparecer en una orden de trabajo
                OrdenTrabajoSpool odtSpool = spool.OrdenTrabajoSpool.SingleOrDefault();
                if (odtSpool != null)
                {
                    ret.EmbarqueEtiqueta = spool.NumeroEtiqueta;
                    ret.EmbarqueFechaEtiqueta = spool.FechaEtiqueta;

                    WorkstatusSpool wkstSpool =
                        lookupWkstatusSpool[odtSpool.OrdenTrabajoSpoolID].SingleOrDefault();

                    if (wkstSpool != null)
                    {
                        #region Embarque

                        EmbarqueSpool embSpool = wkstSpool.EmbarqueSpool.SingleOrDefault();
                        if (embSpool != null)
                        {
                            Embarque emb = embSpool.Embarque;
                            if (emb != null)
                            {
                                ret.EmbarqueFechaEmbarque = emb.FechaEmbarque;
                                ret.EmbarqueNumeroEmbarque = emb.NumeroEmbarque;
                            }
                        }

                        int folio = wkstSpool.FolioPreparacion.SafeIntParse();
                        ret.EmbarqueFolioPreparacion = wkstSpool.FechaPreparacion.SafeDateAsStringParse() + "-" + folio.ToString("000");

                        #endregion

                        #region Reporte Dimensional

                        //de los reportes, tomamos los que sean de tipo dimensional, ordenamos por fecha y
                        //los agregamos al objeto de retorno como una lista
                        ret.ReportesDimensionales =
                            wkstSpool.ReporteDimensionalDetalle.OrderByDescending(
                                x => x.ReporteDimensional.FechaReporte).
                                    ThenByDescending(x => x.FechaModificacion).Where(
                                    x =>
                                    x.ReporteDimensional.TipoReporteDimensionalID ==
                                    (int)TipoReporteDimensionalEnum.Dimensional).Select(
                                        det => new DetSegSpoolRep
                                                   {
                                                       Hoja = det.Hoja,
                                                       Fecha = det.FechaLiberacion,
                                                       FechaReporte = det.ReporteDimensional.FechaReporte,
                                                       NumeroReporte = det.ReporteDimensional.NumeroReporte,
                                                       Resultado =
                                                           TraductorEnumeraciones.TextoAprobadoONoAprobado(
                                                               det.Aprobado),
                                                       Observaciones = det.Observaciones
                                                   }).ToList();

                        #endregion

                        #region Reporte Espesores

                        //de los reportes, tomamos los que sean de tipo espesor, ordenamos por fecha y
                        //los agregamos al objeto de retorno como una lista
                        ret.ReportesEspesores =
                           wkstSpool.ReporteDimensionalDetalle.OrderByDescending(
                               x => x.ReporteDimensional.FechaReporte).Where(
                                   x =>
                                   x.ReporteDimensional.TipoReporteDimensionalID ==
                                   (int)TipoReporteDimensionalEnum.Espesores).Select(
                                       det => new DetSegSpoolRep
                                       {
                                           Hoja = det.Hoja,
                                           Fecha = det.FechaLiberacion,
                                           FechaReporte = det.ReporteDimensional.FechaReporte,
                                           NumeroReporte = det.ReporteDimensional.NumeroReporte,
                                           Resultado =
                                               TraductorEnumeraciones.TextoAprobadoONoAprobado(
                                                   det.Aprobado),
                                           Observaciones = det.Observaciones
                                       }).ToList();
                        #endregion

                        #region Pintura

                        PinturaSpool pinturaSpool = wkstSpool.PinturaSpool.SingleOrDefault();
                        if (pinturaSpool != null)
                        {
                            ret.PinturaFechaAcabadoVisual = pinturaSpool.FechaAcabadoVisual;
                            ret.PinturaFechaAdherencia = pinturaSpool.FechaAdherencia;
                            ret.PinturaFechaIntermedios = pinturaSpool.FechaIntermedios;
                            ret.PinturaFechaPrimarios = pinturaSpool.FechaPrimarios;
                            ret.PinturaFechaPullOff = pinturaSpool.FechaPullOff;
                            ret.PinturaFechaRequisicion =
                                pinturaSpool.RequisicionPinturaDetalle.RequisicionPintura.FechaRequisicion;
                            ret.PinturaNumeroRequisicion =
                                pinturaSpool.RequisicionPinturaDetalle.RequisicionPintura.NumeroRequisicion;
                            ret.PinturaFechaSandBlast = pinturaSpool.FechaSandblast;
                            ret.PinturaReporteAcabadoVisual = pinturaSpool.ReporteAcabadoVisual;
                            ret.PinturaReporteAdherencia = pinturaSpool.ReporteAdherencia;
                            ret.PinturaReporteIntermedios = pinturaSpool.ReporteIntermedios;
                            ret.PinturaReportePrimarios = pinturaSpool.ReportePrimarios;
                            ret.PinturaReportePullOff = pinturaSpool.ReportePullOff;
                            ret.PinturaReporteSandBlast = pinturaSpool.ReporteSandblast;
                        }

                        #endregion

                        #region General Dependiente

                        ret.OrdenDeTrabajo = wkstSpool.OrdenTrabajoSpool.OrdenTrabajo.NumeroOrden;
                        ret.NumeroDeControl = wkstSpool.OrdenTrabajoSpool.NumeroControl;

                        #endregion

                        #region Certificado

                        ret.CertificadoAprobado = wkstSpool.Certificado;
                        ret.CertificadoFecha = wkstSpool.FechaCertificacion;

                        #endregion
                    }

                }

                #region General

                ret.Spool = spool.Nombre;
                ret.Dibujo = spool.Dibujo;
                ret.Especificacion = spool.Especificacion;
                ret.Area = spool.Area;
                ret.Peso = spool.Peso;
                ret.RevisionCte = spool.RevisionCliente;
                ret.RevisionSteelGo = spool.Revision;
                ret.Prioridad = spool.Prioridad;
                ret.Pdis = spool.Pdis;
                ret.PorcPnd = spool.PorcentajePnd;
                ret.Material = famAceros.Single(x => x.ID == spool.FamiliaAcero1ID).FamiliaMaterialNombre;
                ret.Cedula = spool.Cedula;
                ret.Segmento1 = spool.Segmento1;
                ret.Segmento2 = spool.Segmento2;
                ret.Segmento3 = spool.Segmento3;
                ret.Segmento4 = spool.Segmento4;
                ret.Segmento5 = spool.Segmento5;
                ret.Segmento6 = spool.Segmento6;
                ret.Segmento7 = spool.Segmento7;
                ret.RequierePWHT = spool.RequierePwht;
                ret.PendienteDocumental = spool.PendienteDocumental;
                ret.AprobadoCruce = spool.AprobadoParaCruce;
                ret.PinturaCodigo = spool.CodigoPintura;
                ret.PinturaColor = spool.ColorPintura;
                ret.PinturaSistema = spool.SistemaPintura;
                ret.DiametroMayor = spool.DiametroMayor;

                SpoolHold hold =
                    iqSpoolHold.SingleOrDefault(x => x.SpoolID == spool.SpoolID);
                if (hold != null)
                {
                    ret.HoldCalidad = hold.TieneHoldCalidad;
                    ret.HoldIngenieria = hold.TieneHoldIngenieria;
                    ret.Confinado = hold.Confinado;
                }

                #endregion


                #endregion

                return ret;
            }

        }

        /// <summary> 
        /// Obtiene una lista para llenar el grid de seguimiento spool
        /// </summary>
        /// <param name="proyectoID">el ID el proyecto</param>
        /// <param name="embarcadas">Filtro para saber si se regresan los spools embarcados</param>
        /// <param name="numeroOrden">Filtro que en caso de no ser blanco o nulo se aplica a Orden de trabajo para solo regresar esos spools</param>
        /// <param name="numControl">Filtro que en caso de no ser blanco o nulo se aplica a Numero de Control para solo regresar esos spools</param>
        /// <param name="spoolID">si se envia este parametro, el metodo retornará solo un registro</param>
        /// <returns></returns>
        /*    public List<GrdSeguimientoSpool> ObtenerParaGridSeguimiento(int proyectoID, bool embarcadas, string numeroOrden, string numControl, int? spoolID)
            {//Si el proyectoID no existe
                if (proyectoID <= 0)
                {
                    return new List<GrdSeguimientoSpool>();
                }

                using (SamContext ctx = new SamContext())
                {
                    Proyecto proyecto = ctx.Proyecto.Single(x => x.ProyectoID == proyectoID);
                    List<GrdSeguimientoSpool> segSpool = new List<GrdSeguimientoSpool>();

                    #region Construimos los queries que enviaremos a BD

                    //Las ordenes de trabajo del proyecto
                    IQueryable<OrdenTrabajo> iqOrdenesTrabajo = ctx.OrdenTrabajo.Where(x => x.ProyectoID == proyectoID);
                    if (!string.IsNullOrEmpty(numeroOrden))
                    {
                        iqOrdenesTrabajo = iqOrdenesTrabajo.Where(x => x.NumeroOrden == numeroOrden);
                    }

                    //Las ordenes de trabajo que involucran a los spools del proyecto
                    IQueryable<OrdenTrabajoSpool> iqOrdenTrabajoSpool =
                        ctx.OrdenTrabajoSpool.Where(x => iqOrdenesTrabajo.Contains(x.OrdenTrabajo));

                    //Todos los spools del proyecto
                    IQueryable<Spool> qrySpools =
                        ctx.Spool.Where(x => x.ProyectoID == proyectoID);

                    IQueryable<int> spoolsIDs = qrySpools.Select(x => x.SpoolID);

                    //Las Juntas de los spools del proyecto
                    IQueryable<JuntaSpool> iqJuntaSpools = ctx.JuntaSpool.Where(x => qrySpools.Contains(x.Spool));
                    IQueryable<JuntaWorkstatus> iqJuntaWorkstatus =
                        ctx.JuntaWorkstatus.Where(x => iqJuntaSpools.Contains(x.JuntaSpool));

                    //si viene el numero de orden solo queremos jws que esten dentro de esas ordenes
                    if (!string.IsNullOrEmpty(numeroOrden))
                    {
                        iqJuntaWorkstatus =
                            iqJuntaWorkstatus.Where(
                                x => iqOrdenTrabajoSpool.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                    }

                    //En caso de solo querer una junta, essto es para mostrar el detalle
                    if (spoolID.HasValue)
                    {
                        iqJuntaWorkstatus = iqJuntaWorkstatus.Where(x => x.JuntaWorkstatusID == spoolID);
                    }




                    //Los reportes del proyecto
                    IQueryable<ReporteDimensional> iqReporteDimensional =
                        ctx.ReporteDimensional.Where(x => x.ProyectoID == proyectoID);
                    IQueryable<ReporteDimensionalDetalle> iqReporteDimensionalDet =
                        ctx.ReporteDimensionalDetalle.Where(
                            x => iqReporteDimensional.Select(y => y.ReporteDimensionalID).Contains(x.ReporteDimensionalID));

                    //Los workstatus que involucran esas ordenes de trabajo
                    IQueryable<WorkstatusSpool> iqWorkstatusSpool =
                        ctx.WorkstatusSpool.Where(x => iqOrdenTrabajoSpool.Contains(x.OrdenTrabajoSpool));

                    //Los embarques del proyecto
                    IQueryable<Embarque> iqEmbarques = ctx.Embarque.Where(x => x.ProyectoID == proyectoID);
                    IQueryable<EmbarqueSpool> iqEmbarquesSpools =
                        ctx.EmbarqueSpool.Where(x => iqEmbarques.Contains(x.Embarque));

                    //Las inspecciones de las juntas de los spools del proyecto
                    IQueryable<InspeccionDimensionalPatio> iqInspInspeccionDimensionalPatio =
                        ctx.InspeccionDimensionalPatio.Where(x => iqWorkstatusSpool.Contains(x.WorkstatusSpool));

                    //Las pinturas del proyecto con sus requisiciones y detalles
                    IQueryable<PinturaSpool> iqPinturaSpool = ctx.PinturaSpool.Where(x => x.ProyectoID == proyectoID);
                    IQueryable<RequisicionPintura> iqPintura = ctx.RequisicionPintura.Where(x => x.ProyectoID == proyectoID);
                    IQueryable<RequisicionPinturaDetalle> iqPinturaDetalle =
                        ctx.RequisicionPinturaDetalle.Where(x => iqWorkstatusSpool.Contains(x.WorkstatusSpool));

                    //Los spools del proyecto que estan en hold
                    IQueryable<SpoolHold> iqSpoolHold = ctx.SpoolHold.Where(x => spoolsIDs.Contains(x.SpoolID));

                    #endregion

                    #region  Construimos lookups y diccionarios

                    ILookup<int, WorkstatusSpool> lookupWkstatusSpool =
                        iqWorkstatusSpool.ToList().ToLookup(x => x.OrdenTrabajoSpoolID, y => y);

                    #endregion

                    #region traer objetos al contexto

                    qrySpools.ToList();
                    iqJuntaSpools.ToList();
                    iqPinturaDetalle.ToList();
                    iqPinturaSpool.ToList();
                    iqPintura.ToList();
                    iqWorkstatusSpool.ToList();
                    iqEmbarques.ToList();
                    iqReporteDimensional.ToList();
                    iqReporteDimensionalDet.ToList();
                    iqOrdenesTrabajo.ToList();
                    iqOrdenTrabajoSpool.ToList();
                    iqEmbarquesSpools.ToList();
                    iqJuntaWorkstatus.ToList();
                    iqInspInspeccionDimensionalPatio.ToList();

                    #endregion

                    //Si es detalle filtramos para solo esa juntaworkstatus
                    if (spoolID.HasValue)
                    {
                        qrySpools = qrySpools.Where(x => x.SpoolID == spoolID.Value);
                    }


                    foreach (Spool spool in qrySpools.ToList())
                    {
                        GrdSeguimientoSpool grdSgSpool = new GrdSeguimientoSpool();

                        #region ODTSpool

                        //Un spool solo puede aparecer en una orden de trabajo
                        OrdenTrabajoSpool odtSpool = spool.OrdenTrabajoSpool.SingleOrDefault();
                        if (odtSpool != null)
                        {

                            WorkstatusSpool wkstSpool =
                                lookupWkstatusSpool[odtSpool.OrdenTrabajoSpoolID].SingleOrDefault();

                            if (wkstSpool != null)
                            {
                                #region Embarque

                                EmbarqueSpool embSpool = wkstSpool.EmbarqueSpool.SingleOrDefault();
                                if (embSpool != null)
                                {
                                    Embarque emb = embSpool.Embarque;
                                    if (emb != null)
                                    {
                                        //como tiene embarque, si el filtro que indica que se deben incluir las juntas embarcadas esta apagado ignoramos esta junta
                                        if (!embarcadas)
                                        {
                                            continue;
                                        }
                                        grdSgSpool.EmbarqueFechaEmbarque = emb.FechaEmbarque;
                                        grdSgSpool.EmbarqueNumeroEmbarque = emb.NumeroEmbarque;
                                    }
                                }

                                grdSgSpool.EmbarqueEtiqueta = spool.NumeroEtiqueta;
                                grdSgSpool.EmbarqueFechaEtiqueta = spool.FechaEtiqueta;
                                grdSgSpool.EmbarqueFechaPreparacion = wkstSpool.FechaPreparacion;

                                #endregion

                                #region Reporte Dimensional

                                ReporteDimensionalDetalle det =
                                    wkstSpool.ReporteDimensionalDetalle.OrderByDescending(x => x.ReporteDimensional.FechaReporte).
                                        ThenByDescending(x => x.FechaModificacion).FirstOrDefault(
                                        x =>
                                        x.ReporteDimensional.TipoReporteDimensionalID ==
                                        (int)TipoReporteDimensionalEnum.Dimensional);
                                grdSgSpool.InspeccionDimensionalNoRechazos =
                                    wkstSpool.ReporteDimensionalDetalle.Where(
                                        x => x.ReporteDimensional.TipoReporteDimensionalID ==
                                             (int)TipoReporteDimensionalEnum.Dimensional && !x.Aprobado).Count();
                                if (det != null)
                                {
                                    grdSgSpool.InspeccionDimensionalHoja = det.Hoja;
                                    grdSgSpool.InspeccionDimensionalFecha = det.FechaLiberacion;
                                    grdSgSpool.InspeccionDimensionalFechaReporte =
                                        det.ReporteDimensional.FechaReporte;
                                    grdSgSpool.InspeccionDimensionalNumeroReporte =
                                        det.ReporteDimensional.NumeroReporte;
                                    grdSgSpool.InspeccionDimensionalResultado =
                                        TraductorEnumeraciones.TextoAprobadoONoAprobado(det.Aprobado);

                                    if (spoolID.HasValue)
                                    {
                                        grdSgSpool.InspeccionDimensionalObservaciones = det.Observaciones;
                                    }
                                }

                                #endregion

                                #region Reporte Espesores

                                det =
                                    wkstSpool.ReporteDimensionalDetalle.OrderByDescending(x => x.ReporteDimensional.FechaReporte)
                                    .ThenByDescending(x => x.FechaModificacion).FirstOrDefault(
                                        x =>
                                        x.ReporteDimensional.TipoReporteDimensionalID ==
                                        (int)TipoReporteDimensionalEnum.Espesores);
                                grdSgSpool.InspeccionEspesoresNoRechazos =
                                    wkstSpool.ReporteDimensionalDetalle.Where(
                                        x => x.ReporteDimensional.TipoReporteDimensionalID ==
                                             (int)TipoReporteDimensionalEnum.Espesores && !x.Aprobado).Count();
                                if (det != null)
                                {
                                    grdSgSpool.InspeccionEspesoresHoja = det.Hoja;
                                    grdSgSpool.InspeccionEspesoresFecha = det.FechaLiberacion;
                                    grdSgSpool.InspeccionEspesoresFechaReporte =
                                        det.ReporteDimensional.FechaReporte;
                                    grdSgSpool.InspeccionEspesoresNumeroReporte =
                                        det.ReporteDimensional.NumeroReporte;
                                    grdSgSpool.InspeccionEspesoresResultado = det.Aprobado;

                                    if (spoolID.HasValue)
                                    {
                                        grdSgSpool.InspeccionEspesoresObservaciones = det.Observaciones;
                                    }
                                }

                                #endregion

                                #region Pintura

                                PinturaSpool pinturaSpool = wkstSpool.PinturaSpool.SingleOrDefault();
                                if (pinturaSpool != null)
                                {
                                    grdSgSpool.PinturaFechaAcabadoVisual = pinturaSpool.FechaAcabadoVisual;
                                    grdSgSpool.PinturaFechaAdherencia = pinturaSpool.FechaAdherencia;
                                    grdSgSpool.PinturaFechaIntermedios = pinturaSpool.FechaIntermedios;
                                    grdSgSpool.PinturaFechaPrimarios = pinturaSpool.FechaPrimarios;
                                    grdSgSpool.PinturaFechaPullOff = pinturaSpool.FechaPullOff;
                                    grdSgSpool.PinturaFechaRequisicion =
                                        pinturaSpool.RequisicionPinturaDetalle.RequisicionPintura.FechaRequisicion;
                                    grdSgSpool.PinturaNumeroRequisicion =
                                        pinturaSpool.RequisicionPinturaDetalle.RequisicionPintura.NumeroRequisicion;
                                    grdSgSpool.PinturaFechaSandBlast = pinturaSpool.FechaSandblast;
                                    grdSgSpool.PinturaReporteAcabadoVisual = pinturaSpool.ReporteAcabadoVisual;
                                    grdSgSpool.PinturaReporteAdherencia = pinturaSpool.ReporteAdherencia;
                                    grdSgSpool.PinturaReporteIntermedios = pinturaSpool.ReporteIntermedios;
                                    grdSgSpool.PinturaReportePrimarios = pinturaSpool.ReportePrimarios;
                                    grdSgSpool.PinturaReportePullOff = pinturaSpool.ReportePullOff;
                                    grdSgSpool.PinturaReporteSandBlast = pinturaSpool.ReporteSandblast;
                                }

                                #endregion

                                #region General Dependiente

                                grdSgSpool.OrdenDeTrabajo = wkstSpool.OrdenTrabajoSpool.OrdenTrabajo.NumeroOrden;
                                grdSgSpool.NumeroDeControl = wkstSpool.OrdenTrabajoSpool.NumeroControl;

                                #endregion
                            }



                            if (grdSgSpool.NumeroDeControl == numControl || string.IsNullOrEmpty(numControl))
                            {
                                segSpool.Add(grdSgSpool);
                            }
                        }

                        #region General

                        grdSgSpool.SpoolID = spool.SpoolID;
                        grdSgSpool.Pdi = spool.Pdis;
                        grdSgSpool.Peso = spool.Peso;
                        grdSgSpool.Area = spool.Area;
                        grdSgSpool.Spool = spool.Nombre;
                        grdSgSpool.Proyecto = proyecto.Nombre;
                        grdSgSpool.Prioridad = spool.Prioridad;
                        grdSgSpool.Segmento1 = spool.Segmento1;
                        grdSgSpool.Segmento2 = spool.Segmento2;
                        grdSgSpool.Segmento3 = spool.Segmento3;
                        grdSgSpool.Segmento4 = spool.Segmento4;
                        grdSgSpool.Segmento5 = spool.Segmento5;
                        grdSgSpool.Segmento6 = spool.Segmento6;
                        grdSgSpool.Segmento7 = spool.Segmento7;
                        grdSgSpool.Especificacion = spool.Especificacion;
                        grdSgSpool.NumeroJuntas = spool.JuntaSpool.Count;
                        grdSgSpool.TieneHold = TraductorEnumeraciones.TextoSiNo(false);
                        grdSgSpool.PinturaCodigo = spool.CodigoPintura;
                        grdSgSpool.PinturaColor = spool.ColorPintura;
                        grdSgSpool.PinturaSistema = spool.SistemaPintura;

                        SpoolHold hold =
                            iqSpoolHold.SingleOrDefault(x => x.SpoolID == spool.SpoolID);
                        if (hold != null)
                        {
                            grdSgSpool.TieneHold =
                                TraductorEnumeraciones.TextoSiNo(hold.TieneHoldCalidad || hold.TieneHoldIngenieria);
                        }

                        if (grdSgSpool.NumeroDeControl == numControl || string.IsNullOrEmpty(numControl))
                        {
                            if (!segSpool.Contains(grdSgSpool))
                            {
                                segSpool.Add(grdSgSpool);
                            }
                        }

                        #endregion


                        #endregion
                    }

                    return segSpool;
                }
            }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="confinado"></param>
        /// <returns></returns>
        public List<GrdConfinarSpool> ObtenerParaConfinarSpool(int proyectoID, int ordenTrabajoID, int ordenTrabajoSpoolID, bool confinado)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Spool> spool = ctx.Spool.Where(x => x.ProyectoID == proyectoID);
                IQueryable<SpoolHold> spoolHold = ctx.SpoolHold.Where(x => spool.Select(y => y.SpoolID).Contains(x.SpoolID));
                IQueryable<OrdenTrabajoSpool> ordenes = ctx.OrdenTrabajoSpool.Where(x => spool.Select(y => y.SpoolID).Contains(x.SpoolID));

                Dictionary<int, string> famAceros = CacheCatalogos.Instance
                                                .ObtenerFamiliasAcero()
                                                .ToDictionary(x => x.ID, y => y.Nombre);


                IEnumerable<GrdConfinarSpool> lst = from s in spool.ToList()
                                                    join sh in spoolHold.ToList() on s.SpoolID equals sh.SpoolID into shDie
                                                    join odts in ordenes.ToList() on s.SpoolID equals odts.SpoolID into odtsDie
                                                    from odt in odtsDie.DefaultIfEmpty()
                                                    from shs in shDie.DefaultIfEmpty()
                                                    select new GrdConfinarSpool
                                                    {
                                                        SpoolID = s.SpoolID,
                                                        Nombre = s.Nombre,
                                                        Prioridad = s.Prioridad,
                                                        NumeroControl = (odt != null) ? odt.NumeroControl : string.Empty,
                                                        RevisionCliente = s.Revision,
                                                        Pdis = s.Pdis,
                                                        Peso = s.Peso,
                                                        Cedula = s.Cedula,
                                                        Area = s.Area,
                                                        FamiliaAcero1 = famAceros[s.FamiliaAcero1ID],
                                                        FamiliaAcero2 = s.FamiliaAcero2ID.HasValue ? famAceros[s.FamiliaAcero2ID.Value] : string.Empty,
                                                        PorcentajePnd = (s.PorcentajePnd != null) ? s.PorcentajePnd : 0,
                                                        RequierePwht = s.RequierePwht,
                                                        DibujoReferencia = s.Dibujo,
                                                        Especificacion = s.Especificacion,
                                                        Confinado = (shs != null) ? shs.Confinado : false,
                                                        OrdenTrabajoID = (odt != null) ? (int?)odt.OrdenTrabajoID : null,
                                                        OrdenTrabajoSpoolID = (odt != null) ? (int?)odt.OrdenTrabajoSpoolID : null
                                                    };
                if (ordenTrabajoID > 0)
                {
                    lst = lst.Where(x => x.OrdenTrabajoID == ordenTrabajoID);
                }

                if (ordenTrabajoSpoolID > 0)
                {
                    lst = lst.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID);
                }

                if (confinado)
                {
                    return lst.Where(x => x.Confinado == confinado).OrderBy(x => x.Nombre).ToList();
                }
                else
                {
                    return lst.OrderBy(x => x.Nombre).ToList();
                }

            }
        }

        /// <summary>
        /// Obtiene la lista de spools por proyecto para un rad combo
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <param name="nombreSpool">Texto a igualar con el nombre del spool</param>
        /// <param name="skip">Cantidad de elementos a ignorar</param>
        /// <param name="take">Cantidad de elementos a obtener</param>
        /// <returns>Lista de Spools (ID, Valor)</returns>
        public List<Simple> ObtenerPorProyectoParaRadCombo(int proyectoID, string nombreSpool, int skip, int take)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Spool.MergeOption = MergeOption.NoTracking;

                List<Simple> spools = (from spool in ctx.Spool
                                       where spool.ProyectoID == proyectoID
                                       select new Simple
                                       {
                                           ID = spool.SpoolID,
                                           Valor = spool.Nombre
                                       }).ToList();

                return spools.Where(x => x.Valor.ContainsIgnoreCase(nombreSpool))
                             .OrderBy(x => x.Valor)
                             .Skip(skip)
                             .Take(take)
                             .ToList();
            }
        }

        /// <summary>
        /// Obtiene la lista de spools historicos por proyecto para un rad combo
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <param name="nombreSpool">Texto a igualar con el nombre del spool</param>
        /// <param name="skip">Cantidad de elementos a ignorar</param>
        /// <param name="take">Cantidad de elementos a obtener</param>
        /// <returns>Lista de Spools (ID, Valor)</returns>
        public List<Simple> ObtenerPorProyectoHistoricoSpoolsParaRadCombo(int proyectoID, string nombreSpool, int skip, int take)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.SpoolHistorico.MergeOption = MergeOption.NoTracking;


                List<Simple> spools = (from spool in ctx.SpoolHistorico
                                       where spool.ProyectoID == proyectoID
                                       group spool by new { spool.Nombre } into groupedSpools
                                       select new Simple
                                       {
                                           ID = groupedSpools.Sum(x => x.SpoolHistoricoID),
                                           Valor = groupedSpools.Key.Nombre
                                       }
                                       ).ToList();


                return spools.Where(x => x.Valor.ContainsIgnoreCase(nombreSpool))
                             .OrderBy(x => x.Valor)
                             .Skip(skip)
                             .Take(take)
                             .ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoSpool(ctx, spoolID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de spool
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoSpool =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Spool
                            .Where(x => x.SpoolID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolIds"></param>
        /// <returns></returns>
        public int[] ObtenerProyectos(int[] spoolIds)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Spool.MergeOption = MergeOption.NoTracking;

                IQueryable<int> spIds = spoolIds.AsQueryable();

                return ctx.Spool
                             .Where(s => spIds.Contains(s.SpoolID))
                             .Select(s => s.ProyectoID)
                             .ToArray()
                             .Distinct()
                             .ToArray();
            }
        }

        public int OrdenTrabajo(string ordenTrabajo)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajo.Where(x => x.NumeroOrden == ordenTrabajo).Select(x => x.OrdenTrabajoID).FirstOrDefault();
            }
        }

        public int NumeroControl(string numeroControl)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Where(x => x.NumeroControl == numeroControl).Select(x => x.OrdenTrabajoSpoolID).FirstOrDefault();
            }
        }


        /// <summary>
        /// Obtiene lista de DetSpool por nombre del spool
        /// </summary>
        /// <param name="nombreSpool"></param>
        /// <returns></returns>
        public List<DetSpoolMobile> ObtenerPorNombre(string nombreSpool)
        {
            using (SamContext ctx = new SamContext())
            {
                List<DetSpoolMobile> lst = (from ots in ctx.OrdenTrabajoSpool
                                            join s in ctx.Spool on ots.SpoolID equals s.SpoolID
                                            join p in ctx.Proyecto on s.ProyectoID equals p.ProyectoID
                                            where s.Nombre == nombreSpool
                                            select new DetSpoolMobile
                                            {
                                                SpoolID = ots.SpoolID,
                                                NumeroControl = ots.NumeroControl,
                                                ProyectoID = s.ProyectoID,
                                                PatioID = p.PatioID,
                                                Proyecto = p.Nombre
                                            }).ToList();
                return lst;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public string[] ObtenerEtiquetasDeMaterial(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return
                    (from ms in ctx.MaterialSpool
                     where ms.SpoolID == spoolID
                     select ms.Etiqueta).ToArray();

            }
        }

        public List<GrdHistWorkstatus> ObtenerHistoricoHomologacionWorkstatus(int proyectoID, int ordentrabajo, int ordenTrabajoSpoolId)
        {
            using (SamContext ctx = new SamContext())
            {
                return (from hws in ctx.HistoricoWorkstatus
                        join odts in ctx.OrdenTrabajoSpool
                        on hws.SpoolID equals odts.SpoolID
                        join spool in ctx.Spool
                        on odts.SpoolID equals spool.SpoolID
                        join odt in ctx.OrdenTrabajo
                        on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                        where ordenTrabajoSpoolId > 0 ? ordenTrabajoSpoolId == odts.OrdenTrabajoSpoolID : ordentrabajo == odts.OrdenTrabajoID
                        && odt.ProyectoID == proyectoID
                        select new GrdHistWorkstatus
                       {
                           HistoricoWorkStatusID = hws.HistoricoWorkstatusID,
                           Spool = spool.Nombre,
                           Odt = odt.NumeroOrden,
                           NumeroControl = odts.NumeroControl,
                           RevCliente = hws.RevisionCliente,
                           RevSteelgo = hws.Revision,
                           FechaHomologacion = hws.FechaHomologacion
                       }).ToList();
            }
        }

        public string ObtenerNombreReporteWorkStatus(int hwsID, bool spool)
        {
            using (SamContext ctx = new SamContext())
            {
                string nombreArchivo = string.Empty;
                if (spool)
                {
                    nombreArchivo = ctx.HistoricoWorkstatus.Where(x => x.HistoricoWorkstatusID == hwsID).Select(x => x.ArchivoSpool).SingleOrDefault();
                }
                else
                {
                    nombreArchivo = ctx.HistoricoWorkstatus.Where(x => x.HistoricoWorkstatusID == hwsID).Select(x => x.ArchivoJuntas).SingleOrDefault();
                }
                return nombreArchivo;
            }

        }

        public bool TieneBastones(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.BastonSpool.Where(x => x.SpoolID == spoolID && x.LetraBaston != "MAN").Any();
            }
        }


        /// <summary>
        /// Borra el spool con sus materiales, cortes y juntas, siempre y cuando no tenga ODT
        /// e inserta los registros en las tablas SpoolDeleted, JustasSpoolDeleted, MaterialesSpoolDeleted
        /// y CorteSpoolDeleted para mantener un registro de los Spool Emiminados
        /// </summary>
        /// <param name="spoolID"></param>
        public List<string> BorrarSpoolsMasivo(int[] spoolIds, int proyectoID, Guid userID, DateTime fechaModificacion)
        {
            string spoolIdsConOT = null;
            string spoolIdsTienenMaterialCongelado = null;
     
            foreach (Int32 spoolID in spoolIds)
            {
                using (SamContext ctx = new SamContext())
                {
                    if (!ValidacionesSpool.TieneODT(ctx, spoolID))
                    {                      
                        //Obtenermos los Materiales que pertenecen al spool para borrarlos e insertarlos en el historial de eliminados 
                        List<MaterialSpool> lstMaterialSpool = ctx.MaterialSpool.Where(x => x.SpoolID == spoolID).ToList();

                        bool msEstaCongeladoParcial = false;
                        foreach (MaterialSpool ms in lstMaterialSpool)
                        {
                            //si se cuenta con material congelado parcial no se eliminara el spool
                            if(!ValidacionesMaterialSpool.EstaCongeladoParcial(ms.MaterialSpoolID))
                            {
                                MaterialSpoolDeleted msd = convertirObjToObj<MaterialSpoolDeleted>(ms);
                                msd.UsuarioModifica = userID;
                                msd.FechaModificacion = fechaModificacion;

                                ctx.MaterialSpoolDeleted.AddObject(msd);
                                ctx.DeleteObject(ms);
                            }
                            else
                            {
                                Spool s = ctx.Spool.Where(x => x.SpoolID == spoolID).SingleOrDefault();
                                spoolIdsTienenMaterialCongelado = s.Nombre + ", ";
                                msEstaCongeladoParcial = true;
                                break;
                            }
                        }

                        if (!msEstaCongeladoParcial)
                        {
                            //Obtenermos los Cortes que pertenecen al spool para borrarlos e insertarlos en el historial de eliminados 
                            List<CorteSpool> lstCorteSpool = ctx.CorteSpool.Where(x => x.SpoolID == spoolID).ToList();
                            foreach (CorteSpool cs in lstCorteSpool)
                            {
                                CorteSpoolDeleted csd = convertirObjToObj<CorteSpoolDeleted>(cs);
                                csd.UsuarioModifica = userID;
                                csd.FechaModificacion = fechaModificacion;

                                ctx.CorteSpoolDeleted.AddObject(csd);
                                ctx.DeleteObject(cs);
                            }

                            //Obtenermos las Juntas que pertenecen al spool para borrarlos e insertarlos en el historial de eliminados 
                            List<JuntaSpool> lstJuntaSpool = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID).ToList();
                            
                            //ANTES DE BORRAR LAS JUNTAS HAY QUE BORRAR LOS REGISTROS DE AGRUPADORES POR JUNTA
                            List<AgrupadoresPorJunta> agrupadores = (from a in ctx.AgrupadoresPorJunta
                                                                     join j in lstJuntaSpool on a.JuntaSpoolID equals j.JuntaSpoolID
                                                                     select a).ToList();
                            if (agrupadores.Count > 0)
                            {
                                //SI EXISTE ALGUN REGISTRO DE AGRUPADORES LOS BORRAMOS
                                foreach (AgrupadoresPorJunta ag in agrupadores)
                                {
                                    ctx.DeleteObject(ag);
                                }
                            }

                            //ELIMINAMOS LAS JUNTAS
                            foreach (JuntaSpool js in lstJuntaSpool)
                            {
                                JuntaSpoolDeleted jsd = convertirObjToObj<JuntaSpoolDeleted>(js);
                                jsd.UsuarioModifica = userID;
                                jsd.FechaModificacion = fechaModificacion;
                                ctx.JuntaSpoolDeleted.AddObject(jsd);
                                ctx.DeleteObject(js);
                            }

                            //Obtenermos las SpoolHold que pertenecen al spool para borrarlos
                            List<SpoolHold> lstSpoolHold = ctx.SpoolHold.Where(x => x.SpoolID == spoolID).ToList();
                            foreach (SpoolHold sh in lstSpoolHold)
                            {
                                ctx.DeleteObject(sh);
                            }

                            //Obtenermos las SpoolHoldHistorial que pertenecen al spool para borrarlos
                            List<SpoolHoldHistorial> lstSpoolHoldH = ctx.SpoolHoldHistorial.Where(x => x.SpoolID == spoolID).ToList();
                            foreach (SpoolHoldHistorial jsh in lstSpoolHoldH)
                            {
                                ctx.DeleteObject(jsh);
                            }

                            //Eliminar Spool 
                            Spool spoolAEliminar = ctx.Spool.Where(x => x.SpoolID == spoolID).SingleOrDefault();
                            SpoolsDeleted sd = convertirObjToObj<SpoolsDeleted>(spoolAEliminar);
                            sd.UsuarioModifica = userID;
                            sd.FechaModificacion = fechaModificacion;
                            ctx.SpoolsDeleted.AddObject(sd);
                            ctx.DeleteObject(spoolAEliminar);

                            ctx.SaveChanges();
                        }
                    }
                    else
                    {                   
                        Spool s = ctx.Spool.Where(x => x.SpoolID == spoolID).SingleOrDefault();
                        spoolIdsConOT += s.Nombre + ", ";                        
                    }                       
                }               
            }

            List<string> mensajes = new List<string>();

            if (!string.IsNullOrEmpty(spoolIdsConOT))
            {
                mensajes.Add(string.Format(MensajesError.Excepcion_TienenOTSpools, spoolIdsConOT));
            }

            if (!string.IsNullOrEmpty(spoolIdsTienenMaterialCongelado))
            {
                mensajes.Add(string.Format(MensajesError.Excepcion_MateriaCongeladoParcial, spoolIdsTienenMaterialCongelado));
            }         

            return mensajes;         
        }

        public T convertirObjToObj<T>(object objeto)
        {
            T p = default(T);
            try
            {
                string json = JsonConvert.SerializeObject(objeto);
                bool nulls = json.Contains("null");

                p = JsonConvert.DeserializeObject<T>(json);
            }
            catch(JsonSerializationException jsE)
            {
                string error =jsE.Message;
            }            

            return p;
        }

        public List<Spool> ObtenerSpoolsConCuadrante(int[] spoolIds)
        {
            using (SamContext ctx = new SamContext())
            {


                List<Spool> items = ctx.Spool.Where(x => spoolIds.Contains(x.SpoolID)).ToList();


                return items;
            }
        }
    }
}
