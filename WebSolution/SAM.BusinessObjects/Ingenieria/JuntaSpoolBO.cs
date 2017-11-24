using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using Mimo.Framework.Extensions;
using System.Data.Objects;
using System;
using SAM.Entities.Personalizadas;
using log4net;

namespace SAM.BusinessObjects.Ingenieria
{
    public class JuntaSpoolBO
    {
        private static readonly object _mutex = new object();
        private static JuntaSpoolBO _instance;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(JuntaSpoolBO));
        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private JuntaSpoolBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase JuntaSpoolBO
        /// </summary>
        /// <returns></returns>
        public static JuntaSpoolBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaSpoolBO();
                    }
                }
                return _instance;
            }
        }

        public JuntaSpool Obtener(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaSpool.Where(x => x.JuntaSpoolID == juntaSpoolID).SingleOrDefault();
            }
        }

        public void Guarda(JuntaSpool juntaSpool)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.JuntaSpool.ApplyChanges(juntaSpool);

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void GuardaJuntas(List<JuntaSpool> juntasSpool)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    juntasSpool.ForEach(x => ctx.JuntaSpool.ApplyChanges(x));

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }


        /// <summary>
        /// Obtiene la cantidad de Juntas en Base a un SpoolID
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <returns></returns>
        public int ObtenerNumeroDeJuntasPorSpoolID(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaSpool.Where(x => x.SpoolID == spoolID).Count();
            }
        }

        /// <summary>
        /// Obtiene la cantidad de Juntas en Base a un SpoolID y un codigo de Fab Area
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <param name="codigoFabArea">Codigo Fab Area</param>
        /// <returns></returns>
        public int ObtenerNumeroDeJuntasPorSpoolIDyCodigoFabArea(int spoolID, int fabAreaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaSpool.Where(x => x.SpoolID == spoolID && x.FabAreaID == fabAreaID).Count();
            }
        }

        /// <summary>
        /// Obtiene la cantidad de Juntas en Base a un SpoolID, FabArea y Seleccion de Tipo de Juntas
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <param name="codigoFabArea">Codigo Fab Area</param>
        /// <returns></returns>
        public int ObtenerNumeroDeJuntasPorSpoolIDyCodigoFabAreaFiltroTiposSoldables(int spoolID, int fabAreaID, int tipoJuntaTHID, int tipoJuntaTWID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<int> eqtp = (new List<int>() { tipoJuntaTHID, tipoJuntaTWID }).AsQueryable();

                return ctx.JuntaSpool.Where(x => x.SpoolID == spoolID && x.FabAreaID == fabAreaID && !eqtp.Contains(x.TipoJuntaID)).Count();
            }
        }

        /// <summary>
        /// Obtiene las Juntas en Base a un SpoolID
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <returns></returns>
        public List<JuntaSpool> ObtenerJuntasPorSpoolID(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaSpool.Include("FabArea").Where(x => x.SpoolID == spoolID).ToList();
            }
        }

        /// <summary>
        /// Obtiene las Juntas en Base a un SpoolID y Codigo FabArea
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <returns></returns>
        public List<Simple> ObtenerJuntasPorSpoolIDYCodigoFabArea(int spoolID, string codigoFabArea)
        {
            using (SamContext ctx = new SamContext())
            {
                return (from j in ctx.JuntaSpool
                        let jw = j.JuntaWorkstatus.Where(x => x.JuntaFinal).FirstOrDefault()
                        where j.SpoolID == spoolID
                        && j.FabArea.Codigo == codigoFabArea
                        select new Simple
                        {
                            ID = j.JuntaSpoolID,
                            Valor = (jw == null) ? j.Etiqueta : jw.EtiquetaJunta
                        }).ToList();

            }
        }

        /// <summary>
        /// Obtiene las Juntas en Base a un SpoolID y Codigo FabArea menos los tipos de juntas TW y TH
        /// </summary>
        /// <param name="spoolID">Spool ID</param>
        /// <returns></returns>
        public List<Simple> ObtenerJuntasPorSpoolIDYCodigoFabAreaFiltroTiposSoldables(int spoolID, int fabAreaID, int tipoJuntaTHID, int tipoJuntaTWID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<int> eqtp = (new List<int>() { tipoJuntaTHID, tipoJuntaTWID }).AsQueryable();

                return (from j in ctx.JuntaSpool
                        let jw = j.JuntaWorkstatus.Where(x => x.JuntaFinal).FirstOrDefault()
                        where j.SpoolID == spoolID
                        && j.FabAreaID == fabAreaID
                        && !eqtp.Contains(j.TipoJuntaID)
                        select new Simple
                        {
                            ID = j.JuntaSpoolID,
                            Valor = (jw == null) ? j.Etiqueta : jw.EtiquetaJunta
                        }).ToList();
            }
        }

        private static void ObservacionesYDetallesPruebas(out string observacionesReporte,
                                                          out string observacionesRequisicion,
                                                          out List<GrdSegJuntaDetPNDSector> sector,
                                                          out List<GrdSegJuntaDetPNDCuad> cuadrante,
                                                          JuntaReportePnd juntaPnd,
                                                          IDictionary<int, string> diccDefectos)
        {
            observacionesReporte = juntaPnd.Observaciones;
            observacionesRequisicion =
                juntaPnd.JuntaRequisicion.Requisicion.Observaciones;
            sector =
                (from junta in juntaPnd.JuntaReportePndSector
                 select new GrdSegJuntaDetPNDSector
                            {
                                A = junta.SectorFin,
                                De = junta.SectorInicio,
                                Defecto = diccDefectos.SafeTryGetValue(junta.DefectoID),
                                Sector = junta.Sector
                            }).ToList();
            cuadrante = (from junta in juntaPnd.JuntaReportePndCuadrante
                         select new GrdSegJuntaDetPNDCuad
                                    {
                                        Cuadrante = junta.Cuadrante,
                                        Defecto = diccDefectos.SafeTryGetValue(junta.DefectoID.Value),
                                        Placa = junta.Placa
                                    }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoJuntaSpool(ctx, juntaSpoolID);
            }
        }


        public WorkstatusSpool ObtenerWorkStatusspoolPorID(int workstatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<WorkstatusSpool> Spool =   ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == workstatusSpoolID);

                IQueryable<OrdenTrabajoSpool> OrdenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => Spool.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                Spool.ToList();
                OrdenTrabajoSpool.ToList();
                return Spool.FirstOrDefault();
            }
        }

        /// <summary>
        /// Versión compilada del query para permisos de junta spool
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoJuntaSpool =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Spool
                            .Where(s => ctx.JuntaSpool
                                           .Where(js => js.JuntaSpoolID == id)
                                           .Select(js => js.SpoolID)
                                           .Contains(s.SpoolID))
                            .Select(s => s.ProyectoID)
                            .Single()
        );

        public int? ObtenerOrdenTrabajo(string ordenTrabajo)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajo.Where(x => x.NumeroOrden == ordenTrabajo).Select(x => x.OrdenTrabajoID).FirstOrDefault();
            }
        }

        public int? NumeroControl(string numeroControl)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Where(x => x.NumeroControl == numeroControl).Select(x => x.OrdenTrabajoSpoolID).FirstOrDefault();
            }
        }

        public List<JuntaSpool> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.JuntaSpool.MergeOption = MergeOption.NoTracking;
                ctx.Spool.MergeOption = MergeOption.NoTracking;

                List<JuntaSpool> lst = (from s in ctx.Spool
                                        join js in ctx.JuntaSpool on s.SpoolID equals js.SpoolID
                                        where s.ProyectoID == proyectoID
                                        select js).ToList();

                return lst;
            }
        }


        public List<Simple> ObtenerJuntasPorProyectoYNumeroJunta(int proyectoId, string busqueda, int skip, int take, string jwsId)
        {
            using (SamContext ctx = new SamContext())
            {
                string[] ids = jwsId.Split(',');
                int wsId = 0;

                if (ids.Count() == 1)
                {
                    wsId = ids[0].SafeIntParse();
                }

                List<int?> idsJuntaSeg1 = (from r in ctx.JuntaReportePnd
                                           where r.JuntaSeguimientoID1 > 0
                                           select r.JuntaSeguimientoID1).ToList();

                List<int?> idsJuntaSeg2 = (from r in ctx.JuntaReportePnd
                                           where r.JuntaSeguimientoID2 > 0
                                           select r.JuntaSeguimientoID2).ToList();

                //List<int> idJuntasRefEnReporte = (from jws in ctx.JuntaWorkstatus
                //                                join jr in ctx.JuntaReportePnd on jws.JuntaWorkstatusID equals jr.JuntaWorkstatusID
                //                                where idsJuntaSeg1.Contains(jr.JuntaSeguimientoID1) || idsJuntaSeg2.Contains(jr.JuntaSeguimientoID2)
                //                                select jws.JuntaSpoolID).ToList();

                //List<int> idJuntasConReporte = (from jws in ctx.JuntaWorkstatus
                //                                join jr in ctx.JuntaReportePnd on jws.JuntaWorkstatusID equals jr.JuntaWorkstatusID
                //                                select jws.JuntaSpoolID).ToList();

                int tipoJunta = (from jw in ctx.JuntaWorkstatus
                                 join js in ctx.JuntaSpool on jw.JuntaSpoolID equals js.JuntaSpoolID
                                 where jw.JuntaWorkstatusID == wsId
                                 select js.TipoJuntaID).SingleOrDefault();
                                                    

                List<Simple> juntas = (from junta in ctx.JuntaSpool
                                       join jws in ctx.JuntaWorkstatus on junta.JuntaSpoolID equals jws.JuntaSpoolID
                                       join spool in ctx.Spool on junta.SpoolID equals spool.SpoolID
                                       join odts in ctx.OrdenTrabajoSpool on spool.SpoolID equals odts.SpoolID
                                       where spool.ProyectoID == proyectoId 
                                       && !jws.EtiquetaJunta.Contains("R")
                                       && junta.TipoJuntaID == tipoJunta
                                       && jws.JuntaFinal
                                       select new Simple
                                       {
                                           ID = jws.JuntaSpoolID,
                                           Valor = odts.NumeroControl + "-" + jws.EtiquetaJunta
                                       }).ToList();

                juntas = juntas.Where(x => !idsJuntaSeg1.Contains(x.ID)).ToList();
                juntas = juntas.Where(x => !idsJuntaSeg2.Contains(x.ID)).ToList();
                //juntas = juntas.Where(x => !idJuntasRefEnReporte.Contains(x.ID)).ToList();
                //juntas = juntas.Where(x => !idJuntasConReporte.Contains(x.ID)).ToList();

                return juntas.Where(x => x.Valor.ContainsIgnoreCase(busqueda))
                        .OrderBy(x => x.Valor)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }

        public List<JuntaSpool> ObtenerConSpoolsPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<JuntaSpool> lst = ctx.JuntaSpool.Include("Spool").Where(x => x.Spool.ProyectoID == proyectoID).ToList();

                return lst;
            }
        }

        public void Guarda(List<JuntaSpool> JuntaSpoolsEditados)
        {
            using (SamContext ctx = new SamContext())
            {
                foreach (JuntaSpool js in JuntaSpoolsEditados)
                {

                    ctx.JuntaSpool.ApplyChanges(js);
                }

                ctx.SaveChanges();
            }
        }

        public List<GrdPeqNoEncontrados> ObtenerPeqsNoEncontrados(List<JuntaSpool> _PeqsNoEncontrados)
        { 
            _logger.DebugFormat("inicio ObtenerPeqsNoEncontrados popup");
            List<TipoJuntaCache> tipuJunta = CacheCatalogos.Instance.ObtenerTiposJunta();
            _logger.DebugFormat("tipuJunta");
            List<FamAceroCache> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero();
            _logger.DebugFormat("famAcero");

            var lista = (from js in _PeqsNoEncontrados
                         join tj in tipuJunta on js.TipoJuntaID equals tj.ID
                         join fa in famAcero on js.FamiliaAceroMaterial1ID equals fa.ID
                         select new
                         {
                             TipoJunta = tj.Nombre,
                             FamiliaAcero = fa.Nombre,
                             Cedula = js.Cedula,
                             Diametro = js.Diametro
                         }).Distinct().ToList();
            _logger.DebugFormat("lista _PeqsNoEncontrados");

            List<GrdPeqNoEncontrados> peqs = new List<GrdPeqNoEncontrados>(lista.Count * 2);
            lista.ForEach(x => peqs.Add(new GrdPeqNoEncontrados
            {
                TipoJunta = x.TipoJunta,
                FamiliaAcero = x.FamiliaAcero,
                Cedula = x.Cedula,
                Diametro = x.Diametro
            }));

            _logger.DebugFormat("lista ForEach _PeqsNoEncontrados");

            return peqs;

        }

        public List<GrdPeqNoEncontrados> ObtenerKgtNoencontrados(List<JuntaSpool> _KgNoEncontrados)
        {
            _logger.DebugFormat("inicio ObtenerKgtNoencontrados popup");
            var lista = (from js in _KgNoEncontrados                         
                         select new
                         {
                             Cedula = js.Cedula,
                             Diametro = js.Diametro
                         }).Distinct().ToList();
            _logger.DebugFormat("lista _KgNoEncontrados");
            List<GrdPeqNoEncontrados> peqs = new List<GrdPeqNoEncontrados>(lista.Count * 2);
            lista.ForEach(x => peqs.Add(new GrdPeqNoEncontrados
            {
                Cedula = x.Cedula,
                Diametro = x.Diametro
            }));

            _logger.DebugFormat("lista foreach _KgNoEncontrados");

            return peqs;

        }

        public List<GrdPeqNoEncontrados> ObtenerEspNoencontrados(List<JuntaSpool> _EspNoEncontrados)
        {
            _logger.DebugFormat("inicio ObtenerEspNoencontrados popup");
            var lista = (from js in _EspNoEncontrados
                         select new
                         {
                             Cedula = js.Cedula,
                             Diametro = js.Diametro
                         }).Distinct().ToList();

            _logger.DebugFormat("lista _EspNoEncontrados");
            List<GrdPeqNoEncontrados> peqs = new List<GrdPeqNoEncontrados>(lista.Count * 2);
            lista.ForEach(x => peqs.Add(new GrdPeqNoEncontrados
            {
                Cedula = x.Cedula,
                Diametro = x.Diametro
            }));
            _logger.DebugFormat("lista foreach _EspNoEncontrados");
            return peqs;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <returns></returns>
        public JuntaSpoolProduccion ObtenerConDatosDeProduccion(int juntaSpoolID)
        {
            JuntaSpoolProduccion junta = null;

            using (SamContext ctx = new SamContext())
            {
                junta = (from j in ctx.JuntaSpool
                         join s in ctx.Spool on j.SpoolID equals s.SpoolID
                         join odts in ctx.OrdenTrabajoSpool on s.SpoolID equals odts.SpoolID
                         where j.JuntaSpoolID == juntaSpoolID
                         select new JuntaSpoolProduccion
                         {
                             Espesor = j.Espesor,
                             Etiqueta = j.Etiqueta,
                             EtiquetaMaterial1 = j.EtiquetaMaterial1,
                             EtiquetaMaterial2 = j.EtiquetaMaterial2,
                             NumeroControl = odts.NumeroControl,
                             ProyectoID = s.ProyectoID,
                             Spool = s.Nombre,
                             SpoolID = s.SpoolID,
                             TipoJunta = j.TipoJunta.Codigo,
                             OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID,
                             FabArea = j.FabArea.Codigo
                         }).SingleOrDefault();
            }

            return junta;
        }

        /// <summary>
        /// Método para saber si ya existe una junta en la tabla JuntaSpool con la misma localización de la junta a guardar
        /// </summary>
        /// <param name="junta">Junta a insertar o modificar</param>
        /// <returns>TRUE si ya existe una junta con la misma localización</returns>
        public bool ExisteJuntaConLocalizacion(JuntaSpool junta)
        {
            using (SamContext ctx = new SamContext())
            {
                bool hayMasDeUno = ctx.JuntaSpool.Where(x => x.EtiquetaMaterial1 == junta.EtiquetaMaterial1 && x.EtiquetaMaterial2 == junta.EtiquetaMaterial2 && x.SpoolID == junta.SpoolID).Any();

                return hayMasDeUno;
            }
        }
    }
}
