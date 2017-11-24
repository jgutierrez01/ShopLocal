using System;
using System.Collections.Generic;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using System.Transactions;
using Mimo.Framework.Extensions;
using System.Data.Objects;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Administracion;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Ingenieria;
using Newtonsoft.Json;
using SAM.BusinessObjects.Produccion;

namespace SAM.BusinessObjects.Workstatus
{
    public class ReportePndBO
    {
        private static readonly object _mutex = new object();
        private static ReportePndBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private ReportePndBO()
        {

        }

        /// <summary>
        /// Obtiene la instancia de la clase ReportePndBO
        /// </summary>
        public static ReportePndBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReportePndBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de estimaciones de acuerdo a los filtros
        /// </summary>
        /// <param name="numeroDeReporte">numeroEstimación de la tabla Estimacion</param>
        /// <param name="proyectoID">Proyecto ID de la tabla Estimacion</param>
        /// <param name="fechaDesde">Fecha Inicial de Búsqueda de la tabla Estimacion</param>
        /// <param name="fechaHasta">Fecha Final de Búsqueda de la tabla Estimacion</param>
        /// <returns></returns>
        public List<GrdReportePnd> ObtenerConFiltros(int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta, string numeroDeReporte, int? TipoPrueba)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<ReportePnd> iqReportePnd = from e in ctx.ReportePnd
                                                      where e.ProyectoID == proyectoID
                                                      select e;

                if (fechaDesde.HasValue)
                {
                    iqReportePnd = iqReportePnd.Where(y => y.FechaReporte >= fechaDesde.Value);
                }

                if (fechaHasta.HasValue)
                {
                    iqReportePnd = iqReportePnd.Where(y => y.FechaReporte <= fechaHasta.Value);
                }

                if (!String.IsNullOrEmpty(numeroDeReporte))
                {
                    iqReportePnd = iqReportePnd.Where(y => y.NumeroReporte == numeroDeReporte);
                }

                if (TipoPrueba != -1)
                {
                    iqReportePnd = iqReportePnd.Where(y => y.TipoPruebaID == TipoPrueba);
                }
                iqReportePnd.ToList();
                //  ctx.JuntaReportePnd.Where(x => iqReportePnd.Select(y => y.ReportePndID).Contains(x.ReportePndID));

                return (from r in iqReportePnd
                        let JTPnd = r.JuntaReportePnd
                        select new GrdReportePnd
                        {
                            ReportePndID = r.ReportePndID,
                            NumeroDeReporte = r.NumeroReporte,
                            Fecha = r.FechaReporte,
                            TipoDePrueba = r.TipoPrueba.Nombre,
                            TipoPruebaID = r.TipoPruebaID,
                            JuntasTotales = JTPnd.Count,
                            JuntasAprobadas = JTPnd.Where(x => x.Aprobado).Count(),
                            JuntasRechazadas = JTPnd.Where(x => !x.Aprobado).Count()
                        }).ToList();
            }
        }

        /// <summary>
        /// Obtiene el listado de estimaciones de acuerdo a los filtros
        /// </summary>
        /// <param name="numeroDeReporte">numeroEstimación de la tabla Estimacion</param>
        /// <param name="proyectoID">Proyecto ID de la tabla Estimacion</param>
        /// <param name="fechaDesde">Fecha Inicial de Búsqueda de la tabla Estimacion</param>
        /// <param name="fechaHasta">Fecha Final de Búsqueda de la tabla Estimacion</param>
        /// <returns></returns>
        public List<GrdReporteSpoolPnd> ObtenerReportesSpoolConFiltros(int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta, string numeroDeReporte, int? tipoPruebaSpool)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<ReporteSpoolPnd> iqReporteSpoolPnd = from e in ctx.ReporteSpoolPnd
                                                                where e.ProyectoID == proyectoID
                                                                select e;

                if (fechaDesde.HasValue)
                {
                    iqReporteSpoolPnd = iqReporteSpoolPnd.Where(y => y.FechaReporte >= fechaDesde.Value);
                }

                if (fechaHasta.HasValue)
                {
                    iqReporteSpoolPnd = iqReporteSpoolPnd.Where(y => y.FechaReporte <= fechaHasta.Value);
                }

                if (!String.IsNullOrEmpty(numeroDeReporte))
                {
                    iqReporteSpoolPnd = iqReporteSpoolPnd.Where(y => y.NumeroReporte == numeroDeReporte);
                }

                if (tipoPruebaSpool != -1)
                {
                    iqReporteSpoolPnd = iqReporteSpoolPnd.Where(y => y.TipoPruebaSpoolID == tipoPruebaSpool.Value);
                }
                iqReporteSpoolPnd.ToList();

                return (from r in iqReporteSpoolPnd
                        let SRPnd = r.SpoolReportePnd
                        select new GrdReporteSpoolPnd
                        {
                            ReporteSpoolPndID = r.ReporteSpoolPndID,
                            NumeroDeReporte = r.NumeroReporte,
                            Fecha = r.FechaReporte,
                            TipoDePruebaSpool = r.TipoPruebaSpool.Nombre,
                            TipoPruebaSpoolID = r.TipoPruebaSpoolID,
                            SpoolsTotales = SRPnd.Count,
                            SpoolsAprobados = SRPnd.Where(x => x.Aprobado).Count(),
                            SpoolsRechazados = SRPnd.Where(x => !x.Aprobado).Count()
                        }).ToList();
            }
        }

        public ReportePnd Obtener(int reportePndID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ReportePnd.SingleOrDefault(x => x.ReportePndID == reportePndID);
            }
        }

        public ReporteSpoolPnd ObtenerReporteSpool(int reporteSpoolPndID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ReporteSpoolPnd.SingleOrDefault(x => x.ReporteSpoolPndID == reporteSpoolPndID);
            }
        }

        /// <summary>
        /// Obtiene las fechas de reporte
        /// </summary>
        /// <param name="reporteSpoolPndID"></param>
        /// <returns></returns>
        public List<ListaFechaNumeroControl> ObtenerFehcasRequisicionSpoolPND(int reporteSpoolPndID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<ListaFechaNumeroControl> encontrados = (from reportes in ctx.ReporteSpoolPnd
                                                             join spoolReporte in ctx.SpoolReportePnd on reportes.ReporteSpoolPndID equals spoolReporte.ReporteSpoolPndID
                                                             join wsSpool in ctx.WorkstatusSpool on spoolReporte.WorkstatusSpoolID equals wsSpool.WorkstatusSpoolID
                                                             join odts in ctx.OrdenTrabajoSpool on wsSpool.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                             join spoolRequisicion in ctx.SpoolRequisicion on wsSpool.WorkstatusSpoolID equals spoolRequisicion.WorkstatusSpoolID
                                                             join requisisiones in ctx.RequisicionSpool on spoolRequisicion.RequisicionSpoolID equals requisisiones.RequisicionSpoolID
                                                             where reportes.ReporteSpoolPndID == reporteSpoolPndID
                                                             select new ListaFechaNumeroControl
                                                             {
                                                                 FechaProceso = requisisiones.FechaRequisicion,
                                                                 FechaReporte = reportes.FechaReporte,
                                                                 NumeroControl = odts.NumeroControl
                                                             }).ToList();
                return encontrados;
            }
        }

        public List<ListaFechaNumeroControl> ObtnerFechasSoldaduraReportePNDEdicion(int reportePndID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<ListaFechaNumeroControl> encontrados = (from juntaReporte in ctx.JuntaReportePnd
                                                       join jr in ctx.JuntaRequisicion on juntaReporte.JuntaRequisicionID equals jr.JuntaRequisicionID
                                                       join r in ctx.Requisicion on jr.RequisicionID equals r.RequisicionID
                                                       join jws in ctx.JuntaWorkstatus on juntaReporte.JuntaWorkstatusID equals jws.JuntaWorkstatusID
                                                       join js in ctx.JuntaSoldadura on jws.JuntaWorkstatusID equals js.JuntaWorkstatusID
                                                       join odts in ctx.OrdenTrabajoSpool on jws.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                       where juntaReporte.ReportePndID == reportePndID
                                                       select new ListaFechaNumeroControl
                                                       {
                                                           FechaProceso = js.FechaSoldadura,
                                                           FechaReporte = r.FechaRequisicion,
                                                           NumeroControl = odts.NumeroControl
                                                       }).ToList();
                return encontrados;
            }
        }


        /// <summary>
        /// Verifica si ya existe un numero de reporte en la tabla reporte Spool PND
        /// </summary>
        /// <param name="proyectoId"></param>
        /// <param name="numeroReorte"></param>
        /// <returns></returns>
        public bool EdicionEspecialValidarNumeroReporteSpoolPND(int reporteId, int proyectoId, string numeroReorte)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ReporteSpoolPnd.Where(x => x.NumeroReporte == numeroReorte 
                                                    && x.ProyectoID == proyectoId
                                                    && x.ReporteSpoolPndID != reporteId).Any();
            }
        }

        public bool EdicionEspecialValidarNumeroReportePND(int reportePndId, int proyectoId, string numeroReporte)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ReportePnd.Where(x => x.NumeroReporte == numeroReporte
                                                && x.ProyectoID == proyectoId
                                                && x.ReportePndID != reportePndId).Any();
            }
        }

        public void GuardarEdicionReporteSpoolPND(ReporteSpoolPnd reporteSpool)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    reporteSpool.StartTracking();
                    ctx.ReporteSpoolPnd.ApplyChanges(reporteSpool);
                    ctx.SaveChanges();
                }
                scope.Complete();
            }
        }

        public void GuardarEdicionReportePND(ReportePnd reportePnd)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    reportePnd.StartTracking();
                    ctx.ReportePnd.ApplyChanges(reportePnd);
                    ctx.SaveChanges();
                }
                scope.Complete();
            }
        }

        public void Borra(int reportePndID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool tieneMovimientos = ValidacionesReportePnd.TieneJuntaReporte(ctx, reportePndID);

                if (!tieneMovimientos)
                {
                    ReportePnd reportePnd = ctx.ReportePnd.Where(x => x.ReportePndID == reportePndID).SingleOrDefault();
                    ctx.DeleteObject(reportePnd);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneJuntasReporte);
                }
            }
        }

        public void BorraReporteSpool(int reporteSpoolPndID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool tieneMovimientos = ValidacionesReportePnd.TieneSpoolReporte(ctx, reporteSpoolPndID);

                if (!tieneMovimientos)
                {
                    ReporteSpoolPnd reporteSpoolPnd = ctx.ReporteSpoolPnd.Where(x => x.ReporteSpoolPndID == reporteSpoolPndID)
                                                                         .SingleOrDefault();
                    ctx.DeleteObject(reporteSpoolPnd);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneSpoolsReporte);
                }
            }
        }

        public ReportePnd ObtenerNumeroJuntasDetalleReportePnd(int reportePndID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<ReportePnd> iqReportePnd = ctx.ReportePnd.Where(x => x.ReportePndID == reportePndID);
                IQueryable<JuntaReportePnd> iqJuntaReportePnd =
                    ctx.JuntaReportePnd.Where(x => iqReportePnd.Select(y => y.ReportePndID).Contains(x.ReportePndID));
                iqReportePnd.ToList();
                iqJuntaReportePnd.ToList();
                return iqReportePnd.FirstOrDefault();
            }

        }

        public List<TipoPrueba> ObtenerTipoPrueba()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoPrueba.Where(x => x.Categoria == CategoriaTipoPrueba.PND).ToList();
            }
        }

        public List<TipoPruebaSpool> ObtenerTipoPruebaSpool()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.TipoPruebaSpool.Where(x => x.Categoria == CategoriaTipoPrueba.PND).ToList();
            }
        }

        /// <summary>
        /// Obtenemos las juntas con requisicion a prueba PND
        /// </summary>
        /// <param name="ordenTrabajoID">Id de la orden de trabajo</param>
        /// <param name="ordenTrabajoSpoolID">ID de la ordentrabajospool</param>
        /// <returns></returns>
        public List<GrdRequisiciones> ObtenerJuntas(int ordenTrabajoID, int ordenTrabajoSpoolID, int tipoPruebaID)
        {
            List<GrdRequisiciones> lista = new List<GrdRequisiciones>();
            List<JuntaSpool> juntaSpool;
            List<OrdenTrabajoJunta> ordenTrabajoJunta;
            IQueryable<OrdenTrabajoSpool> ordenTrabajoSpool;
            List<OrdenTrabajoSpool> ordenTrabajoSpoolList;
            List<OrdenTrabajo> ordenTrabajo;
            IQueryable<JuntaWorkstatus> juntaWorkstatus;
            IQueryable<JuntaRequisicion> juntaRequisicion;
            List<JuntaWorkstatus> juntaWorkstatusList;
            List<JuntaRequisicion> juntaRequisicionList;
            List<Requisicion> requisicion;
            List<FamAceroCache> famAcero;
            List<SpoolHold> spoolHold;
            List<TipoJuntaCache> tipoJunta;

            using (SamContext ctx = new SamContext())
            {
                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajo.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoJunta.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Requisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaRequisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.SpoolHold.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                //Obtengo los IDs de las juntas que existen en la tabla JuntaWorkstatus que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal

                IQueryable<JuntaWorkstatus> juntas = juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                                                            && (ordenTrabajoSpoolID == -1
                                                                                            || x.OrdenTrabajoSpool.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                                                            && x.JuntaFinal);

                //Obtengo las requisiciones de las juntas que caen en los filtros
                IQueryable<int> juntasConRequisicion = (from jtaReq in ctx.JuntaRequisicion
                                                        join jtaWks in juntas on jtaReq.JuntaWorkstatusID equals jtaWks.JuntaWorkstatusID
                                                        join jtaRep in ctx.JuntaReportePnd on jtaReq.JuntaRequisicionID equals jtaRep.JuntaRequisicionID into Reportes
                                                        from rep in Reportes.DefaultIfEmpty()
                                                        where jtaReq.Requisicion.TipoPruebaID == tipoPruebaID && rep == null
                                                        select jtaWks.JuntaSpoolID);

                //Obtengo los registros de las juntas obtenidas en el paso anterior y que además son parte de un spool con liberacion dimensional
                //IQueryable<JuntaSpool> query = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();

                juntaSpool = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).ToList();
                ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).ToList();
                ordenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoID == ordenTrabajoID).AsQueryable();
                ordenTrabajo = ctx.OrdenTrabajo.Where(x => x.OrdenTrabajoID == ordenTrabajoID).ToList();
                juntaWorkstatus = ctx.JuntaWorkstatus.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();

                juntaRequisicion = ctx.JuntaRequisicion.Where(x => juntaWorkstatus.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID)).AsQueryable();
                requisicion = ctx.Requisicion.Where(x => juntaRequisicion.Select(y => y.RequisicionID).Contains(x.RequisicionID)).ToList();
                spoolHold = ctx.SpoolHold.Where(x => ordenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID)).ToList();
                famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero();
                tipoJunta = CacheCatalogos.Instance.ObtenerTiposJunta();
                ordenTrabajoSpoolList = ordenTrabajoSpool.ToList();
                juntaWorkstatusList = juntaWorkstatus.ToList();
                juntaRequisicionList = juntaRequisicion.ToList();
            }

            lista = (from js in juntaSpool
                     join otj in ordenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                     join ots in ordenTrabajoSpoolList on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                     join ot in ordenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                     join jw in juntaWorkstatusList on js.JuntaSpoolID equals jw.JuntaSpoolID
                     join jReq in juntaRequisicionList on jw.JuntaWorkstatusID equals jReq.JuntaWorkstatusID
                     join req in requisicion on jReq.RequisicionID equals req.RequisicionID
                     join sh in spoolHold on ots.SpoolID equals sh.SpoolID into Holds
                     from hld in Holds.DefaultIfEmpty()
                     where req.TipoPruebaID == tipoPruebaID
                     where jw.JuntaFinal
                     select new GrdRequisiciones
                     {
                         JuntaWorkstatusID = jw.JuntaWorkstatusID,
                         JuntaSpoolID = js.JuntaSpoolID,
                         RequisicionID = req.RequisicionID,
                         FechaRequisicion = req.FechaRequisicion,
                         NumeroRequisicion = req.NumeroRequisicion,
                         OrdenTrabajo = ot.NumeroOrden,
                         NumeroControl = ots.NumeroControl,
                         EtiquetaJunta = jw.EtiquetaJunta,
                         EtiquetaMaterial1 = js.EtiquetaMaterial1,
                         EtiquetaMaterial2 = js.EtiquetaMaterial2,
                         TipoJuntaID = js.TipoJuntaID,
                         TipoJunta = tipoJunta.Where(x => x.ID == js.TipoJuntaID).Select(x => x.Nombre).FirstOrDefault(),
                         Cedula = js.Cedula,
                         FamiliaAceroMaterial1 = famAcero.Where(x => x.ID == js.FamiliaAceroMaterial1ID).Select(x => x.Nombre).FirstOrDefault(),
                         FamiliaAceroMaterial2 = famAcero.Where(x => x.ID == js.FamiliaAceroMaterial2ID).Select(x => x.Nombre).FirstOrDefault(),
                         Diametro = js.Diametro,
                         Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                     }).ToList();


            return lista;

        }

        /// <summary>
        /// Obtenemos las juntas con requisicion a prueba PND
        /// </summary>
        /// <param name="proyectoID">Id del proyecto</param>
        /// <returns></returns>
        public List<GrdRequisiciones> ObtenerJuntas(int proyectoID, int tipoPruebaID)
        {
            List<GrdRequisiciones> lista = new List<GrdRequisiciones>();
            List<JuntaSpool> juntaSpool;
            List<OrdenTrabajoJunta> ordenTrabajoJunta;
            IQueryable<OrdenTrabajoSpool> ordenTrabajoSpool;
            List<OrdenTrabajoSpool> ordenTrabajoSpoolList;
            List<OrdenTrabajo> ordenTrabajo;
            IQueryable<JuntaWorkstatus> juntaWorkstatus;
            IQueryable<JuntaRequisicion> juntaRequisicion;
            List<JuntaWorkstatus> juntaWorkstatusList;
            List<JuntaRequisicion> juntaRequisicionList;
            List<Requisicion> requisicion;
            List<FamAceroCache> famAcero;
            List<SpoolHold> spoolHold;
            List<TipoJuntaCache> tipoJunta;

            using (SamContext ctx = new SamContext())
            {
                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajo.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoJunta.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.Requisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaRequisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.SpoolHold.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                //Obtengo los IDs de las juntas que existen en la tabla JuntaWorkstatus que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal

                IQueryable<JuntaWorkstatus> juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID
                                                                                    && x.JuntaFinal);

                //Obtengo las requisiciones de las juntas que caen en los filtros
                IQueryable<int> juntasConRequisicion = (from jtaReq in ctx.JuntaRequisicion
                                                        join jtaWks in juntas on jtaReq.JuntaWorkstatusID equals jtaWks.JuntaWorkstatusID
                                                        join jtaRep in ctx.JuntaReportePnd on jtaReq.JuntaRequisicionID equals jtaRep.JuntaRequisicionID into Reportes
                                                        from rep in Reportes.DefaultIfEmpty()
                                                        where jtaReq.Requisicion.TipoPruebaID == tipoPruebaID && rep == null
                                                        select jtaWks.JuntaSpoolID);

                //Obtengo los registros de las juntas obtenidas en el paso anterior y que además son parte de un spool con liberacion dimensional
                //IQueryable<JuntaSpool> query = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();

                juntaSpool = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).ToList();
                ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).ToList();
                ordenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajo.ProyectoID == proyectoID).AsQueryable();
                ordenTrabajo = ctx.OrdenTrabajo.Where(x => x.ProyectoID == proyectoID).ToList();
                juntaWorkstatus = ctx.JuntaWorkstatus.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();

                juntaRequisicion = ctx.JuntaRequisicion.Where(x => juntaWorkstatus.Select(y => y.JuntaWorkstatusID).Contains(x.JuntaWorkstatusID)).AsQueryable();
                requisicion = ctx.Requisicion.Where(x => juntaRequisicion.Select(y => y.RequisicionID).Contains(x.RequisicionID)).ToList();
                spoolHold = ctx.SpoolHold.Where(x => ordenTrabajoSpool.Select(y => y.SpoolID).Contains(x.SpoolID)).ToList();
                famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero();
                tipoJunta = CacheCatalogos.Instance.ObtenerTiposJunta();
                ordenTrabajoSpoolList = ordenTrabajoSpool.ToList();
                juntaWorkstatusList = juntaWorkstatus.ToList();
                juntaRequisicionList = juntaRequisicion.ToList();
            }

            lista = (from js in juntaSpool
                     join otj in ordenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                     join ots in ordenTrabajoSpoolList on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                     join ot in ordenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                     join jw in juntaWorkstatusList on js.JuntaSpoolID equals jw.JuntaSpoolID
                     join jReq in juntaRequisicionList on jw.JuntaWorkstatusID equals jReq.JuntaWorkstatusID
                     join req in requisicion on jReq.RequisicionID equals req.RequisicionID
                     join sh in spoolHold on ots.SpoolID equals sh.SpoolID into Holds
                     from hld in Holds.DefaultIfEmpty()
                     where req.TipoPruebaID == tipoPruebaID
                     where jw.JuntaFinal
                     select new GrdRequisiciones
                     {
                         JuntaWorkstatusID = jw.JuntaWorkstatusID,
                         JuntaSpoolID = js.JuntaSpoolID,
                         RequisicionID = req.RequisicionID,
                         FechaRequisicion = req.FechaRequisicion,
                         NumeroRequisicion = req.NumeroRequisicion,
                         OrdenTrabajo = ot.NumeroOrden,
                         NumeroControl = ots.NumeroControl,
                         EtiquetaJunta = jw.EtiquetaJunta,
                         EtiquetaMaterial1 = js.EtiquetaMaterial1,
                         EtiquetaMaterial2 = js.EtiquetaMaterial2,
                         TipoJuntaID = js.TipoJuntaID,
                         TipoJunta = tipoJunta.Where(x => x.ID == js.TipoJuntaID).Select(x => x.Nombre).FirstOrDefault(),
                         Cedula = js.Cedula,
                         FamiliaAceroMaterial1 = famAcero.Where(x => x.ID == js.FamiliaAceroMaterial1ID).Select(x => x.Nombre).FirstOrDefault(),
                         FamiliaAceroMaterial2 = famAcero.Where(x => x.ID == js.FamiliaAceroMaterial2ID).Select(x => x.Nombre).FirstOrDefault(),
                         Diametro = js.Diametro,
                         Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                     }).ToList();


            return lista;

        }

        /// <summary>
        /// Obtenemos las juntas con requisicion a prueba PND
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <param name="tipoPruebaID"></param>
        /// <returns></returns>
        public List<GrdRequisicionesSpool> ObtenerSpools(int proyectoID, int ordenTrabajoID, int ordenTrabajoSpoolID, int tipoPruebaID)
        {
            List<Spool> spool;
            List<GrdRequisicionesSpool> lista = new List<GrdRequisicionesSpool>();
            IQueryable<OrdenTrabajoSpool> ordenTrabajoSpool;
            List<OrdenTrabajoSpool> ordenTrabajoSpoolList;
            List<OrdenTrabajo> ordenTrabajo;
            IQueryable<WorkstatusSpool> workstatusSpool;
            IQueryable<SpoolRequisicion> spoolRequisicion;
            List<WorkstatusSpool> workstatusSpoolList;
            List<SpoolRequisicion> spoolRequisicionList;
            List<RequisicionSpool> requisicionSpool;
            List<FamAceroCache> famAcero;

            List<JuntaWorkstatus> jwks = null;
            List<JuntaSpool> js = null;
            List<OrdenTrabajoJunta> otj = null;
            List<TipoJuntaCache> tp = CacheCatalogos.Instance.ObtenerTiposJunta();
            int fabArea = CacheCatalogos.Instance.ObtenerFabAreas().Where(x => x.Nombre == FabAreas.SHOP).Select(x => x.ID).SingleOrDefault();
            int jTH = tp.Where(y => y.Nombre == TipoJuntas.TH).Select(y => y.ID).SingleOrDefault();
            int jTW = tp.Where(y => y.Nombre == TipoJuntas.TW).Select(y => y.ID).SingleOrDefault();

            using (SamContext ctx = new SamContext())
            {
                ctx.WorkstatusSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajo.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.RequisicionSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.SpoolRequisicion.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                //Obtengo los IDs de las juntas que existen en la tabla WorkstatusSpool que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal

                IQueryable<WorkstatusSpool> spools = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpool.Spool.ProyectoID == proyectoID && 
                                                                                    (ordenTrabajoID == -1 || 
                                                                                    x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                                                    )&& 
                                                                                    (ordenTrabajoSpoolID == -1 ||
                                                                                    x.OrdenTrabajoSpool.OrdenTrabajoSpoolID == ordenTrabajoSpoolID));

                
                //Obtengo las requisiciones de las juntas que caen en los filtros
                IQueryable<int> juntasConRequisicion = (from jtaReq in ctx.SpoolRequisicion
                                                        join jtaWks in spools on jtaReq.WorkstatusSpoolID equals jtaWks.WorkstatusSpoolID
                                                        join jtaRep in ctx.SpoolReportePnd on jtaReq.SpoolRequisicionID equals jtaRep.SpoolRequisicionID into Reportes
                                                        from rep in Reportes.DefaultIfEmpty()
                                                        where jtaReq.RequisicionSpool.TipoPruebaSpoolID == tipoPruebaID && rep == null
                                                        select jtaWks.OrdenTrabajoSpool.SpoolID);



                //Se obtienen los spool requisicion que tienen reporte
                IQueryable<int> ReqConReporte = (from jtaReq in ctx.SpoolRequisicion
                                                        join jtaWks in spools on jtaReq.WorkstatusSpoolID equals jtaWks.WorkstatusSpoolID
                                                        join jtaRep in ctx.SpoolReportePnd on jtaReq.SpoolRequisicionID equals jtaRep.SpoolRequisicionID into Reportes
                                                        from rep in Reportes.DefaultIfEmpty()
                                                        where jtaReq.RequisicionSpool.TipoPruebaSpoolID == tipoPruebaID && rep != null
                                                        select jtaReq.SpoolRequisicionID);


                spool = ctx.Spool.Where(x => juntasConRequisicion.Contains(x.SpoolID)).ToList();
                ordenTrabajoSpool = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID || ordenTrabajoSpoolID == -1).AsQueryable();
                ordenTrabajo = ctx.OrdenTrabajo.Where(x => x.OrdenTrabajoID == ordenTrabajoID || ordenTrabajoID == -1).ToList();
                workstatusSpool = ctx.WorkstatusSpool.Where(x => juntasConRequisicion.Contains(x.OrdenTrabajoSpool.SpoolID)).AsQueryable();

                spoolRequisicion = ctx.SpoolRequisicion.Where(x => workstatusSpool.Select(y => y.WorkstatusSpoolID).Contains(x.WorkstatusSpoolID)).AsQueryable();
                requisicionSpool = ctx.RequisicionSpool.Where(x => spoolRequisicion.Select(y => y.RequisicionSpoolID).Contains(x.RequisicionSpoolID)).ToList();
                famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero();
                ordenTrabajoSpoolList = ordenTrabajoSpool.ToList();
                workstatusSpoolList = workstatusSpool.ToList();
                spoolRequisicionList = spoolRequisicion.ToList();

                lista = (from s in spool
                         join ots in ordenTrabajoSpoolList on s.SpoolID equals ots.SpoolID
                         join ot in ordenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                         join jw in workstatusSpoolList on ots.OrdenTrabajoSpoolID equals jw.OrdenTrabajoSpoolID
                         join jReq in spoolRequisicionList on jw.WorkstatusSpoolID equals jReq.WorkstatusSpoolID
                         join req in requisicionSpool on jReq.RequisicionSpoolID equals req.RequisicionSpoolID
                         join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                         from hld in Holds.DefaultIfEmpty()
                         where !ReqConReporte.Contains(jReq.SpoolRequisicionID)
                         select new GrdRequisicionesSpool
                         {
                             WorkstatusSpoolID = jw.WorkstatusSpoolID,
                             RequisicionSpoolID = req.RequisicionSpoolID,
                             FechaRequisicion = req.FechaRequisicion,
                             NumeroRequisicion = req.NumeroRequisicion,
                             OrdenTrabajo = ot.NumeroOrden,
                             NumeroControl = ots.NumeroControl,
                             NombreSpool = s.Nombre,
                             LiberacionDimensional = jw != null ? jw.TieneLiberacionDimensional : false,
                             OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                             SpoolID = s.SpoolID,
                             Area = s.Area != null ? (decimal)s.Area : 0,
                             PDI = s.Pdis != null ? (decimal)s.Pdis : 0,
                             Peso = s.Peso != null ? (decimal)s.Peso : 0,
                             Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                         }).ToList();

                IQueryable<int> listaODTS = lista.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();

                ctx.JuntaWorkstatus.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                ctx.JuntaSpool.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                jwks = ctx.JuntaWorkstatus.Where(x => listaODTS.Contains(x.OrdenTrabajoSpoolID) && x.JuntaFinal).ToList();
                IQueryable<OrdenTrabajoJunta> ordenTrabajoJunta = ctx.OrdenTrabajoJunta.Where(y => listaODTS.Contains(y.OrdenTrabajoSpoolID)).AsQueryable();
                js = ctx.JuntaSpool.Where(x => ordenTrabajoJunta.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID)).ToList();
                otj = ordenTrabajoJunta.ToList();

                lista.ForEach(x => calculaEstatus(x, jwks, js, otj, fabArea, jTH, jTW));

                return lista;
            }
        }

        /// <summary>
        /// Calcula el estatus de cada spool en base al estatus de todas sus juntas.
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="juntaWks"></param>
        /// <param name="juntaSpool"></param>
        /// <param name="otj"></param>
        /// <param name="fabAreaID"></param>
        /// <param name="jTH"></param>
        /// <param name="jTW"></param>
        private void calculaEstatus(GrdRequisicionesSpool grd, List<JuntaWorkstatus> juntaWks, List<JuntaSpool> juntaSpool, List<OrdenTrabajoJunta> otj, int fabAreaID, int jTH, int jTW)
        {
            IEnumerable<JuntaWorkstatus> jwks = juntaWks.Where(y => y.OrdenTrabajoSpoolID == grd.OrdenTrabajoSpoolID);
            IEnumerable<OrdenTrabajoJunta> otjQuery = otj.Where(x => x.OrdenTrabajoSpoolID == grd.OrdenTrabajoSpoolID);
            IEnumerable<JuntaSpool> js = juntaSpool.Where(x => otjQuery.Select(y => y.JuntaSpoolID).Contains(x.JuntaSpoolID));
            int numJuntas = js.Where(x => x.FabAreaID == fabAreaID).Count();
            int numSoldaduras = js.Where(x => x.FabAreaID == fabAreaID && x.TipoJuntaID != jTH && x.TipoJuntaID != jTW).Count();
            grd.Armado = jwks.Where(x => x.ArmadoAprobado).Count() == numJuntas;
            grd.Soldadura = jwks.Where(x => x.SoldaduraAprobada
                                            && js.Where(y => y.JuntaSpoolID == x.JuntaSpoolID).Select(y => y.TipoJuntaID).FirstOrDefault() != jTH
                                            && js.Where(y => y.JuntaSpoolID == x.JuntaSpoolID).Select(y => y.TipoJuntaID).FirstOrDefault() != jTW).Count()
                                        == numSoldaduras;
        }

        /// <summary>
        /// Obtenemos las juntas con requisicion a prueba PND
        /// </summary>
        /// <param name="ordenTrabajoID">Id de la orden de trabajo</param>
        /// <param name="ordenTrabajoSpoolID">ID de la ordentrabajospool</param>
        /// <returns></returns>
        public List<GrdRequisiciones> ObtenerJuntasUniQuery(int ordenTrabajoID, int ordenTrabajoSpoolID, int tipoPruebaID)
        {
            List<GrdRequisiciones> lista = new List<GrdRequisiciones>();

            using (SamContext ctx = new SamContext())
            {
                //Obtengo los IDs de las juntas que existen en la tabla JuntaWorkstatus que son parte de la orden de trabajo
                //y del numero de control (en caso de especificarse)
                //Donde la junta es JuntaFinal
                IQueryable<JuntaWorkstatus> juntas = ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpool.OrdenTrabajoID == ordenTrabajoID
                                                                      && (ordenTrabajoSpoolID == -1
                                                                      || x.OrdenTrabajoSpool.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                                                      && x.JuntaFinal);

                //Obtengo las requisiciones de las juntas que caen en los filtros
                IQueryable<int> juntasConRequisicion = (from jtaReq in ctx.JuntaRequisicion
                                                        join jtaWks in juntas on jtaReq.JuntaWorkstatusID equals jtaWks.JuntaWorkstatusID
                                                        join jtaRep in ctx.JuntaReportePnd on jtaReq.JuntaRequisicionID equals jtaRep.JuntaRequisicionID into Reportes
                                                        from rep in Reportes.DefaultIfEmpty()
                                                        where jtaReq.Requisicion.TipoPruebaID == tipoPruebaID && rep == null
                                                        select jtaWks.JuntaSpoolID);

                //Obtengo los registros de las juntas obtenidas en el paso anterior y que además son parte de un spool con liberacion dimensional
                IQueryable<JuntaSpool> query = ctx.JuntaSpool.Where(x => juntasConRequisicion.Contains(x.JuntaSpoolID)).AsQueryable();
                
                lista = (from js in query
                         join otj in ctx.OrdenTrabajoJunta on js.JuntaSpoolID equals otj.JuntaSpoolID
                         join ots in ctx.OrdenTrabajoSpool on otj.OrdenTrabajoSpoolID equals ots.OrdenTrabajoSpoolID
                         join ot in ctx.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                         join jw in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID
                         join jReq in ctx.JuntaRequisicion on jw.JuntaWorkstatusID equals jReq.JuntaWorkstatusID
                         join req in ctx.Requisicion on jReq.RequisicionID equals req.RequisicionID
                         join sh in ctx.SpoolHold on ots.SpoolID equals sh.SpoolID into Holds
                         from hld in Holds.DefaultIfEmpty()
                         where req.TipoPruebaID == tipoPruebaID
                         select new GrdRequisiciones
                         {
                             JuntaWorkstatusID = jw.JuntaWorkstatusID,
                             JuntaSpoolID = js.JuntaSpoolID,
                             RequisicionID = req.RequisicionID,
                             FechaRequisicion = req.FechaRequisicion,
                             NumeroRequisicion = req.NumeroRequisicion,
                             OrdenTrabajo = ot.NumeroOrden,
                             NumeroControl = ots.NumeroControl,
                             EtiquetaJunta = js.Etiqueta,
                             EtiquetaMaterial1 = js.EtiquetaMaterial1,
                             EtiquetaMaterial2 = js.EtiquetaMaterial2,
                             TipoJuntaID = js.TipoJuntaID,
                             TipoJunta = js.TipoJunta.Codigo,
                             Cedula = js.Cedula,
                             FamiliaAceroMaterial1 = js.FamiliaAcero.Nombre,
                             FamiliaAceroMaterial2 = js.FamiliaAcero1.Nombre,
                             Diametro = js.Diametro,
                             Hold = (hld == null) ? false : hld.TieneHoldIngenieria || hld.TieneHoldCalidad || hld.Confinado
                         }).ToList();
            }

            return lista;

        }

        public bool GuardaReporteSpoolPND(ReporteSpoolPnd reporteSpool, SpoolReportePnd spoolReporte, string IDs, string RIDs, Guid UserUID)
        {
            bool resultado = true;

            using (SamContext ctx = new SamContext())
            {
                //Validar si el numero de reporte ya existe en la base de datos
                ReporteSpoolPnd reporteExistente = ctx.ReporteSpoolPnd.Where(x => x.NumeroReporte == reporteSpool.NumeroReporte &&
                                                                                  x.ProyectoID == reporteSpool.ProyectoID).SingleOrDefault();
                if (reporteExistente != null)
                {
                    //Validando que las fechas concuerden
                    if (reporteExistente.FechaReporte != reporteSpool.FechaReporte)
                    {
                        throw new ExcepcionReportes(MensajesError.Excepcion_ReporteExistenteConFechaDiferente);
                    }
                    else
                    {
                        reporteSpool = reporteExistente;

                        reporteSpool.StartTracking();
                        reporteSpool.UsuarioModifica = UserUID;
                        reporteSpool.FechaModificacion = DateTime.Now;
                    }
                }
                else
                {
                    reporteSpool.UsuarioModifica = UserUID;
                    reporteSpool.FechaModificacion = DateTime.Now;
                }

                string[] spoolArreglo = IDs.Split(',');
                string[] requisicionArreglo = RIDs.Split(',');
                int contador = 0;

                foreach (string spoolID in spoolArreglo)
                {
                    int sID = spoolID.SafeIntParse();
                    int reqID = requisicionArreglo[contador].SafeIntParse();

                    //Verifico que no exista ya un detalle para este spool en especifico en el reporte 
                     if (ctx.SpoolReportePnd.Where(x => x.ReporteSpoolPndID == reporteSpool.ReporteSpoolPndID && x.WorkstatusSpoolID == sID).Any()) 
                     { 
                         throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicado)); 
                     }  
 

                     int spoolReq = ctx.SpoolRequisicion.Where(x => x.RequisicionSpoolID == reqID && x.WorkstatusSpoolID == sID).Select(x => x.SpoolRequisicionID).SingleOrDefault(); 

                     SpoolReportePnd spool = new SpoolReportePnd
                     {
                        WorkstatusSpoolID = sID,
                        SpoolRequisicionID = spoolReq,
                        FechaPrueba = spoolReporte.FechaPrueba,
                        Aprobado = spoolReporte.Aprobado,
                        Observaciones = spoolReporte.Observaciones,
                        UsuarioModifica = UserUID,
                        FechaModificacion = DateTime.Now
                    };

                    WorkstatusSpool wksSpool = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == sID).Single();

                    //Si el reporte es aprobado se actualiza el registro de WorkstatusSpool
                    if (spoolReporte.Aprobado)
                    {
                        wksSpool.StartTracking();
                        wksSpool.UltimoProcesoID = (int)UltimoProcesoEnum.PND;
                        wksSpool.UsuarioModifica = UserUID;
                        wksSpool.FechaModificacion = DateTime.Now;
                        wksSpool.StopTracking();
                        ctx.WorkstatusSpool.ApplyChanges(wksSpool);
                    }

                    reporteSpool.SpoolReportePnd.Add(spool);

                    //Contador de las requisiciones
                    contador++;
                }

                if (reporteExistente != null)
                {
                    reporteSpool.StopTracking();
                }

                ctx.ReporteSpoolPnd.ApplyChanges(reporteSpool);
                ctx.SaveChanges();

                ctx.CalculaHojaParaReporte((int)TipoReporte.ReporteSpoolPND, reporteSpool.ProyectoID, reporteSpool.NumeroReporte, IDs);
            }

            return resultado;
        }

        /// <summary>
        /// Genera el reporte de PND
        /// </summary>
        /// <param name="reporte">Objeto ReportePnd que tiene los datos generales a guardar</param>
        /// <param name="juntaReporte">Objeto JuntaReportePnd que tiene los datos generales de cada junta a guardar</param>
        /// <param name="sectores">Listado de defectos en sectores</param>
        /// <param name="cuadrantes">Listado de defectos en cuadrantes</param>
        /// <param name="IDs">IDs de las juntas en el reporte</param>
        /// <returns>True si fue aprobado o fue rechazo que genero nueva etiqueta / False si fue el tercer rechazo o mayor</returns>
        public bool GuardaReportePND(ReportePnd reporte, JuntaReportePnd juntaReporte, List<JuntaReportePndSector> sectores, List<JuntaReportePndCuadrante> cuadrantes, string IDs, string RIDs, Guid UserUID, out Guid responsable, out string proyectoNombre, out string pendiente, out string detalle)
        {
            bool resultado = true;
            responsable = Guid.Parse("dddddddddddddddddddddddddddddddd");
            proyectoNombre = string.Empty;
            pendiente = string.Empty;
            detalle = string.Empty;

                using (SamContext ctx = new SamContext())
                {
                    //Si la prueba es rechazada se validara que por lo menos se haya dado de alta un defecto
                    if (!juntaReporte.Aprobado && (juntaReporte.TipoRechazoID == (int)TipoRechazoEnum.Sector && sectores.Count() == 0)
                        || (juntaReporte.TipoRechazoID == (int)TipoRechazoEnum.Cuadrante && cuadrantes.Count() == 0))
                    {
                        throw new ExcepcionReportes(MensajesError.Excepcion_RechazadoSinDefectos);
                    }

                    //Validar si el numero de reporte ya existe en la base de datos
                    ReportePnd reporteExistente = ctx.ReportePnd.Where(x => x.NumeroReporte == reporte.NumeroReporte && x.ProyectoID == reporte.ProyectoID).SingleOrDefault();
                    if (reporteExistente != null)
                    {
                        //Validando que las fechas concuerden
                        if (reporteExistente.FechaReporte != reporte.FechaReporte)
                        {
                            throw new ExcepcionReportes(string.Format( MensajesError.Excepcion_ReporteExistenteConFechaDiferente, reporteExistente.FechaReporte.ToShortDateString()));
                        }
                        else
                        {
                            reporte = reporteExistente;

                            reporte.StartTracking();
                            reporte.UsuarioModifica = UserUID;
                            reporte.FechaModificacion = DateTime.Now;
                        }
                    }
                    else
                    {
                        reporte.UsuarioModifica = UserUID;
                        reporte.FechaModificacion = DateTime.Now;
                    }

                    string[] juntasArreglo = IDs.Split(',');
                    string[] requisicionArreglo = RIDs.Split(',');
                    int contador = 0;
               
                    foreach (string juntaID in juntasArreglo)
                    {
                        int jID = juntaID.SafeIntParse();
                        int reqID = requisicionArreglo[contador].SafeIntParse();
                     
                        //Verifico que no exista ya un detalle para este spool en especifico en el reporte
                        JuntaReportePnd jrpnd = ctx.JuntaReportePnd.Where(x => x.ReportePndID == reporte.ReportePndID && x.JuntaWorkstatusID == jID).FirstOrDefault();

                        if (jrpnd != null)
                        {
                            throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicado));
                        }

                        int jtaReq = ctx.JuntaRequisicion.Where(x => x.RequisicionID == reqID && x.JuntaWorkstatusID == jID).Select(x => x.JuntaRequisicionID).SingleOrDefault();

                        JuntaReportePnd junta = new JuntaReportePnd
                        {
                            JuntaWorkstatusID = jID,
                            JuntaRequisicionID = jtaReq,
                            TipoRechazoID = juntaReporte.TipoRechazoID,
                            FechaPrueba = juntaReporte.FechaPrueba,
                            Aprobado = juntaReporte.Aprobado,
                            Observaciones = juntaReporte.Observaciones,
                            UsuarioModifica = UserUID,
                            FechaModificacion = DateTime.Now,
                            JuntaSeguimientoID1 = juntaReporte.JuntaSeguimientoID1,
                            JuntaSeguimientoID2 = juntaReporte.JuntaSeguimientoID2
                        };

                        JuntaWorkstatus juntaWks = ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == jID).FirstOrDefault();     
 
                        if(juntaWks != null && juntaWks.VersionJunta > 1)
                        {
                            JuntaSoldadura jsold = ctx.JuntaSoldadura.Where(x => x.JuntaSoldaduraID == juntaWks.JuntaSoldaduraID).FirstOrDefault();

                            if( reporte.FechaReporte < jsold.FechaSoldadura )
                            {
                                throw new ExcepcionFechas(string.Format(MensajesError.Excepcion_FechaReporteMayorFechaProceso, Procesos.Soldadura, jsold.FechaSoldadura.ToShortDateString() ));
                            }
                        }
                       
                        //Si el reporte es aprobado se actualiza el registro de JuntaWorkstatus
                        if (juntaReporte.Aprobado)
                        {
                            juntaWks.StartTracking();
                            juntaWks.UltimoProcesoID = (int)UltimoProcesoEnum.PND;
                            juntaWks.UsuarioModifica = UserUID;
                            juntaWks.FechaModificacion = DateTime.Now;
                            juntaWks.StopTracking();
                            ctx.JuntaWorkstatus.ApplyChanges(juntaWks);
                            ctx.SaveChanges();
                        }
                        else //Si el reporte es rechazado se generan los registros de defectos y ademas se genera una nuevo junta con una nueva etiqueta
                        {
                            if (juntaReporte.TipoRechazoID == (int)TipoRechazoEnum.Sector)
                            {
                                foreach (JuntaReportePndSector sector in sectores)
                                {
                                    sector.FechaModificacion = DateTime.Now;
                                    sector.UsuarioModifica = UserUID;
                                    junta.JuntaReportePndSector.Add(sector);
                                }
                            }
                            else
                            {
                                foreach (JuntaReportePndCuadrante cuadrante in cuadrantes)
                                {
                                    cuadrante.FechaModificacion = DateTime.Now;
                                    cuadrante.UsuarioModifica = UserUID;
                                    junta.JuntaReportePndCuadrante.Add(cuadrante);
                                }
                            }

                            int siguienteNumRechazo = RechazosCortesUtil.ObtenerSiguienteRechazo(juntaWks.EtiquetaJunta);
                            //Si la version de la junta aún no es la tercera se genera un nuevo registro en juntaworkstatus y se envia a resoldar (por rechazo)

                            if (siguienteNumRechazo < 3)
                            {
                                JuntaWorkstatus nuevaJuntaWks = new JuntaWorkstatus
                                {
                                    OrdenTrabajoSpoolID = juntaWks.OrdenTrabajoSpoolID,
                                    JuntaSpoolID = juntaWks.JuntaSpoolID,
                                    EtiquetaJunta = RechazosCortesUtil.ObtenerNuevaEtiquetaDeRechazo(siguienteNumRechazo, juntaWks.EtiquetaJunta),
                                    ArmadoAprobado = true,
                                    SoldaduraAprobada = false,
                                    InspeccionVisualAprobada = false,
                                    VersionJunta = juntaWks.VersionJunta + 1,
                                    JuntaWorkstatusAnteriorID = juntaWks.JuntaWorkstatusID,
                                    JuntaFinal = true,
                                    UltimoProcesoID = (int)UltimoProcesoEnum.Armado,
                                    UsuarioModifica = UserUID,
                                    FechaModificacion = DateTime.Now
                                };

                                JuntaArmado armado = ctx.JuntaArmado.Where(x => x.JuntaArmadoID == juntaWks.JuntaArmadoID).SingleOrDefault();

                                JuntaArmado nuevoArmado = new JuntaArmado
                                {
                                    NumeroUnico1ID = armado.NumeroUnico1ID,
                                    NumeroUnico2ID = armado.NumeroUnico2ID,
                                    TallerID = armado.TallerID,
                                    TuberoID = armado.TuberoID,
                                    FechaArmado = armado.FechaArmado,
                                    FechaReporte = armado.FechaReporte,
                                    Observaciones = armado.Observaciones,
                                    UsuarioModifica = UserUID,
                                    FechaModificacion = DateTime.Now
                                };
                                
                                WorkstatusSpool spoolWks = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == juntaWks.OrdenTrabajoSpoolID).FirstOrDefault();
                       
                                if (spoolWks != null)
                                {
                                    if(spoolWks.TieneLiberacionDimensional)
                                    {
                                        ReporteDimensionalDetalleDeleted rddDeleted = null;                                       

                                        ReporteDimensionalDetalle rdd = ctx.ReporteDimensionalDetalle.Include("ReporteDimensional").Where(x => x.WorkstatusSpoolID == spoolWks.WorkstatusSpoolID).FirstOrDefault();
                                        
                                        if(rdd != null)
                                        {
                                            rddDeleted = convertirObjToObj<ReporteDimensionalDetalleDeleted, ReporteDimensionalDetalle>(rdd);
                                            ReporteDimensional rd = ctx.ReporteDimensional.Where(x => x.ReporteDimensionalID == rdd.ReporteDimensionalID).FirstOrDefault();

                                            if (rd != null)
                                            {

                                                rddDeleted = convertirObjToObj<ReporteDimensionalDetalleDeleted, ReporteDimensionalDetalle>(rdd);

                                                rdd.FechaModificacion = DateTime.Now;
                                                rdd.UsuarioModifica = UserUID;
                                                ctx.ReporteDimensionalDetalleDeleted.AddObject(rddDeleted);
                                                ctx.DeleteObject(rdd);

                                            }                                        
                                        }
                                    }
                                }        

                                if (spoolWks != null)
                                {
                                    spoolWks.StartTracking();
                                    spoolWks.UltimoProcesoID = null;
                                    spoolWks.TieneLiberacionDimensional = false;
                                    //spoolWks.TieneRequisicionPintura = false;
                                    //spoolWks.TienePintura = false;
                                    //spoolWks.LiberadoPintura = false;
                                    spoolWks.Preparado = false;
                                    spoolWks.Embarcado = false;
                                    spoolWks.Certificado = false;
                                    spoolWks.FechaPreparacion = null;
                                    spoolWks.FechaCertificacion = null;
                                    spoolWks.UsuarioModifica = UserUID;
                                    spoolWks.FechaModificacion = DateTime.Now;
                                    spoolWks.StopTracking();
                                    ctx.WorkstatusSpool.ApplyChanges(spoolWks);
                                }

                                juntaWks.StartTracking();
                                juntaWks.JuntaFinal = false;
                                juntaWks.UsuarioModifica = UserUID;
                                juntaWks.FechaModificacion = DateTime.Now;
                                juntaWks.StopTracking();
                                ctx.JuntaWorkstatus.ApplyChanges(juntaWks);

                                //Guardar Junta Workstatus
                                ctx.JuntaWorkstatus.ApplyChanges(nuevaJuntaWks);
                                ctx.SaveChanges();

                                //Guardar Junta Armado
                                nuevoArmado.JuntaWorkstatusID = nuevaJuntaWks.JuntaWorkstatusID;
                                ctx.JuntaArmado.ApplyChanges(nuevoArmado);
                                ctx.SaveChanges();


                                //Re guardar Junta Workstatus añadiendo Junta Armado
                                nuevaJuntaWks.StartTracking();
                                nuevaJuntaWks.JuntaArmadoID = nuevoArmado.JuntaArmadoID;
                                ctx.JuntaWorkstatus.ApplyChanges(nuevaJuntaWks);
                                ctx.SaveChanges();

                            }
                            else
                            {
                                resultado = false;    
                          
                                //TODO:
                                //Generar pendiente automatico de Corte en junta.
                                //En la descripción del pendiente incluir el siguiente texto:
                                //Etiqueta de la junta: *juntaWks.EtiquetaJunta

                                int categoriaPendienteID = (int)CategoriaPendienteEnum.Ingenieria;
                                int tipoPendienteID = (int)TipoPendienteEnum.CortePorRechazoDePrueba;
                                string nombreProyecto = ctx.Proyecto.Where(x => x.ProyectoID == reporte.ProyectoID).Single().Nombre;
                                string idiomaUsuario = ctx.Usuario.Where(x => x.UserId == UserUID).Single().Idioma;
                                string numeroControl = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == juntaWks.OrdenTrabajoSpoolID).Single().NumeroControl;
                                Pendiente p = new Pendiente();

                                p.StartTracking();
                                p.ProyectoID = reporte.ProyectoID;
                                p.TipoPendienteID = tipoPendienteID;
                                p.Estatus = EstatusPendiente.Abierto;
                                p.FechaApertura = DateTime.Now;
                                p.GeneradoPor = UserUID;
                                p.FechaModificacion = DateTime.Now;

                                //Obtenemos el usuario responsable
                                ProyectoPendiente pp = ctx.ProyectoPendiente
                                                          .Where(x => x.ProyectoID == reporte.ProyectoID && x.TipoPendienteID == p.TipoPendienteID)
                                                          .SingleOrDefault();
                                
                                responsable = pp.Responsable;
                                proyectoNombre = nombreProyecto;

                                if (pp != null)
                                {
                                    p.AsignadoA = pp.Responsable;
                                    p.CategoriaPendienteID = categoriaPendienteID;

                                    TipoPendiente tipo = ctx.TipoPendiente
                                                            .Where(x => x.TipoPendienteID == tipoPendienteID)
                                                            .Single();

                                    p.Descripcion = LanguageHelper.INGLES == idiomaUsuario ? "Label: " + juntaWks.EtiquetaJunta + "<br > Control Number: " + numeroControl : "Etiqueta de la junta: " + juntaWks.EtiquetaJunta + "<br > Número de Control: " + numeroControl;
                                    p.Titulo = LanguageHelper.INGLES == idiomaUsuario ? tipo.NombreIngles : tipo.Nombre;
                                }

                                pendiente = p.Titulo;
                                detalle = p.Descripcion;

                                PendienteDetalle pd = new PendienteDetalle();

                                pd.CategoriaPendienteID = categoriaPendienteID;
                                pd.EsAlta = true;
                                pd.Responsable = pp.Responsable;
                                pd.Estatus = EstatusPendiente.Abierto;
                                pd.UsuarioModifica = UserUID;
                                pd.FechaModificacion = DateTime.Now;

                                p.StopTracking();
                                p.PendienteDetalle.Add(pd);

                                ctx.Pendiente.ApplyChanges(p);
                                ctx.SaveChanges();
                            }
                        }

                        reporte.JuntaReportePnd.Add(junta);

                        //Contador de las requisiciones
                        contador++;
                    }

                    if (reporteExistente != null)
                    {
                        reporte.StopTracking();
                    }

                    ctx.ReportePnd.ApplyChanges(reporte);
                    ctx.SaveChanges();

                    ctx.CalculaHojaParaReporte((int)TipoReporte.ReportePND, reporte.ProyectoID, reporte.NumeroReporte, IDs);
                }

            //    ts.Complete();
            //}

            return resultado;
        } 

        public T convertirObjToObj<T, U>(object objeto)
        {
            T p = default(T);
            try
            {
                string json = JsonConvert.SerializeObject(objeto);
                bool nulls = json.Contains("null");

                p = JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonSerializationException jsE)
            {
                string error = jsE.Message;
            }

            return p;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportePndID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int reportePndID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoReportePnd(ctx, reportePndID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportePndID"></param>
        /// <returns></returns>
        public int ObtenerProyectoIDPorReporteSpool(int reporteSpoolPndID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoReporteSpoolPnd(ctx, reporteSpoolPndID);
            }
        }

        /// <summary>
        /// Versión compilada del query para permisos de reportes pnd
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoReportePnd =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.ReportePnd
                            .Where(x => x.ReportePndID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );

        /// <summary>
        /// Versión compilada del query para permisos de reportes pnd
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoReporteSpoolPnd =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.ReporteSpoolPnd
                            .Where(x => x.ReporteSpoolPndID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
