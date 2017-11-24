using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Excepciones;
using System.Data.Objects;
using SAM.Entities.Personalizadas;
using System.Transactions;

namespace SAM.BusinessObjects.Workstatus
{
    public class ReporteDimensionalBO
    {
        private static readonly object _mutex = new object();
        private static ReporteDimensionalBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ReporteDimensionalBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ReporteDimensionalBO
        /// </summary>
        /// <returns></returns>
        public static ReporteDimensionalBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReporteDimensionalBO();
                    }
                }
                return _instance;
            }
        }

        public ReporteDimensional Obtener(int repDimensionalID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ReporteDimensional.Where(x => x.ReporteDimensionalID == repDimensionalID).Single();
            }
        }

        /// <summary>
        /// Obtiene las fechas de reporte de soldadura de las juntas que estan asociadas a un reporte dimencional
        /// </summary>
        /// <param name="reporteDimencionalID"></param>
        /// <returns></returns>
        public List<ListaFechaNumeroControl> ObtenerFechasReporteSoldaduraEdicionDimensional(int reporteDimencionalID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<ListaFechaNumeroControl> fechas = (from soldaduras in ctx.JuntaSoldadura
                                             join juntaWs in ctx.JuntaWorkstatus on soldaduras.JuntaWorkstatusID equals juntaWs.JuntaWorkstatusID
                                             join odts in ctx.OrdenTrabajoSpool on juntaWs.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                             join wsSpool in ctx.WorkstatusSpool on odts.OrdenTrabajoSpoolID equals wsSpool.OrdenTrabajoSpoolID
                                             join detalle in ctx.ReporteDimensionalDetalle on wsSpool.WorkstatusSpoolID equals detalle.WorkstatusSpoolID
                                             join reportes in ctx.ReporteDimensional on detalle.ReporteDimensionalID equals reportes.ReporteDimensionalID
                                             where reportes.ReporteDimensionalID == reporteDimencionalID
                                             && juntaWs.JuntaFinal == true
                                                        select new ListaFechaNumeroControl
                                             {
                                                 FechaReporte = soldaduras.FechaReporte,
                                                 FechaProceso = soldaduras.FechaSoldadura,
                                                 NumeroControl = odts.NumeroControl
                                             }).ToList();
                return fechas;
            }
        }

        /// <summary>
        /// obtiene una lista de fechas de liberacion del detalle de un reporte dimencional
        /// </summary>
        /// <param name="reporteDimencionalID"></param>
        /// <returns></returns>
        public List<DateTime?> ObtenerFechasDetalleDimensional(int reporteDimensionalID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<DateTime?> fechas = (from detalles in ctx.ReporteDimensionalDetalle
                                         where detalles.ReporteDimensionalID == reporteDimensionalID
                                         select detalles.FechaLiberacion).ToList();
                return fechas;
            }
        }

        public void GuardarEdicionReporteDimensional(ReporteDimensional reporte)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {
                    reporte.StartTracking();
                    ctx.ReporteDimensional.ApplyChanges(reporte);
                    ctx.SaveChanges();
                }
                scope.Complete();
            }
        }

        public bool ValidarNumeroReporteEdicion(int reporteDimensionalID, int proyectoID, string numeroReporte)
        {
            using (SamContext ctx = new SamContext())
            {
                bool res = ctx.ReporteDimensional.Where(x => x.ProyectoID == proyectoID && x.NumeroReporte == numeroReporte && x.ReporteDimensionalID != reporteDimensionalID).Any();
                return res;
            }
        }

        public ReporteDimensional DetalleInspeccionDimensional(int reporteDimensionalID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ReporteDimensional.Include("ReporteDimensionalDetalle")
                                             .Include("TipoReporteDimensional")
                                             .Include("Proyecto")
                                             .Where(x => x.ReporteDimensionalID == reporteDimensionalID).Single();
            }
        }

        public List<GrdDetInspeccionDimensional> ObtenerDetalleInspeccionDimensional(int reporteDimensionalID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<ReporteDimensional> iQoRD = ctx.ReporteDimensional.Where(x => x.ReporteDimensionalID == reporteDimensionalID);
                IQueryable<ReporteDimensionalDetalle> iQoRDD = ctx.ReporteDimensionalDetalle.Where(x => iQoRD.Select(y => y.ReporteDimensionalID).Contains(x.ReporteDimensionalID));
                IQueryable<WorkstatusSpool> iQoWS = ctx.WorkstatusSpool.Where(X => iQoRDD.Select(y => y.WorkstatusSpoolID).Contains(X.WorkstatusSpoolID));
                IQueryable<OrdenTrabajoSpool> iQoODTS = ctx.OrdenTrabajoSpool.Where(x => iQoWS.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                IQueryable<OrdenTrabajo> iQoODT = ctx.OrdenTrabajo.Where(x => iQoODTS.Select(y => y.OrdenTrabajoID).Contains(x.OrdenTrabajoID));
                IQueryable<Spool> iQoS = ctx.Spool.Where(x => iQoODTS.Select(y => y.SpoolID).Contains(x.SpoolID));

                List<GrdDetInspeccionDimensional> lst = (from rd in iQoRD.ToList()
                                                         join rdd in iQoRDD.ToList() on rd.ReporteDimensionalID equals rdd.ReporteDimensionalID
                                                         join ws in iQoWS.ToList() on rdd.WorkstatusSpoolID equals ws.WorkstatusSpoolID
                                                         join odts in iQoODTS.ToList() on ws.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                         join odt in iQoODT.ToList() on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                         join s in iQoS.ToList() on odts.SpoolID equals s.SpoolID
                                                         
                                                         select new GrdDetInspeccionDimensional 
                                                         {
                                                             OrdenTrabajo = odt.NumeroOrden,
                                                             NumeroControl = odts.NumeroControl,
                                                             Spool = s.Nombre,
                                                             Hoja = rdd.Hoja,
                                                             Aprobado = rdd.Aprobado,
                                                             FechaLiberacion = rdd.FechaLiberacion,
                                                             Observaciones = rdd.Observaciones,
                                                             ReporteDimensionalDetalleID = rdd.ReporteDimensionalDetalleID
                                                         }).ToList();
                return lst;
            }
        }

        public void Borra(int reporteDimensionalID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesReporteDimensional.TieneReporteDimensionalDetalle(ctx,reporteDimensionalID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneReporteDimensionalDetalle);
                }

                ReporteDimensional rd = ctx.ReporteDimensional.Where(x => x.ReporteDimensionalID == reporteDimensionalID).SingleOrDefault();
                ctx.DeleteObject(rd);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reporteDimensionalID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int reporteDimensionalID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoReporteDimensional(ctx, reporteDimensionalID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de reporte dimensional
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoReporteDimensional =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.ReporteDimensional
                            .Where(x => x.ReporteDimensionalID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
