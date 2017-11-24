using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;

namespace SAM.BusinessObjects.Workstatus
{
    public class LiberacionDimensionalPatioBO
    {
         private static readonly object _mutex = new object();
        private static LiberacionDimensionalPatioBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private LiberacionDimensionalPatioBO()
        {
            
        }

        /// <summary>
        /// Obtiene la instancia de la clase LiberacionVisualPatioBO
        /// </summary>
        public static LiberacionDimensionalPatioBO Instance
        {
             get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new LiberacionDimensionalPatioBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de estimaciones de acuerdo a los filtros
        /// </summary>
        /// <param name="proyectoID">Proyecto ID de la tabla Estimacion</param>
        /// <param name="fechaDesde">Fecha Inicial de Búsqueda de la tabla Estimacion</param>
        /// <param name="fechaHasta">Fecha Final de Búsqueda de la tabla Estimacion</param>
        /// <returns></returns>
        public List<GrdLiberacionDimansionalPatio> ObtenerConFiltros(int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Spool> iqSpool= from e in ctx.Spool
                                                      where e.ProyectoID == proyectoID
                                                      select e;

                IQueryable<OrdenTrabajoSpool> iqOrdenTrabajoSpool = ctx.OrdenTrabajoSpool
                                                                       .Where(x => iqSpool.Select(y => y.SpoolID)
                                                                       .Contains(x.SpoolID));

                IQueryable<WorkstatusSpool> iqWorkstatusSpool = ctx.WorkstatusSpool
                                                                   .Where(x => iqOrdenTrabajoSpool.Select(y=> y.OrdenTrabajoSpoolID)
                                                                   .Contains(x.OrdenTrabajoSpoolID));

                 IQueryable<InspeccionDimensionalPatio> iqInspeccionDimencionalPatio = ctx.InspeccionDimensionalPatio
                                                                              .Where(x => iqWorkstatusSpool.Select(y => y.WorkstatusSpoolID)
                                                                              .Contains(x.WorkstatusSpoolID));


                IQueryable<SpoolHold> iqSpoolHold = ctx.SpoolHold.Where(x => iqSpool.Select(y => y.SpoolID).Contains(x.SpoolID));

                if (fechaDesde.HasValue)
                {
                    iqInspeccionDimencionalPatio = iqInspeccionDimencionalPatio.Where(y => y.FechaInspeccion >= fechaDesde.Value);
                }

                if (fechaHasta.HasValue)
                {
                    iqInspeccionDimencionalPatio = iqInspeccionDimencionalPatio.Where(y => y.FechaInspeccion <= fechaHasta.Value);
                }

                iqSpool.ToList();
                iqOrdenTrabajoSpool.ToList();
                iqWorkstatusSpool.ToList();
                iqInspeccionDimencionalPatio.ToList();

                return (from r in iqInspeccionDimencionalPatio
                        join spoolHold in iqSpoolHold on r.WorkstatusSpool.OrdenTrabajoSpool.SpoolID equals spoolHold.SpoolID into leftJoinned
                        from sph in leftJoinned.DefaultIfEmpty()
                        select new GrdLiberacionDimansionalPatio
                        {
                            InspeccionDimansionalPatioID = r.InspeccionDimensionalPatioID,
                            SpoolID = r.WorkstatusSpool.OrdenTrabajoSpool.SpoolID,
                            NumeroDeControl = r.WorkstatusSpool.OrdenTrabajoSpool.NumeroControl,
                            Spool = r.WorkstatusSpool.OrdenTrabajoSpool.Spool.Nombre,
                            Resultado = r.Aprobado == true ? "Aprobado" : "No Aprobado",
                            FechaInspeccion = r.FechaInspeccion,
                            Observaciones = r.Observaciones,
                            Hold = sph != null && (sph.TieneHoldCalidad || sph.TieneHoldIngenieria || sph.Confinado)
                        }).ToList();
            }
        }

        public InspeccionDimensionalPatio Obtener(int inspeccionDimencionalPatioID)
        {
            using(SamContext ctx = new SamContext())
            {
                return ctx.InspeccionDimensionalPatio.SingleOrDefault(x => x.InspeccionDimensionalPatioID == inspeccionDimencionalPatioID);
            }
        }

        public static void Guarda(ReporteDimensional reporteDimensional)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.ReporteDimensional.ApplyChanges(reporteDimensional);

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Verifica que la estimacion a agregar no exista en la base de datos,
        /// si es falso manda un mensaje de error
        /// </summary>
        /// <param name="ctx">contexto</param>
        /// <param name="numeroEstimacion">numero de estimacion a evaluar</param>
        /// <returns>verdadero o falso</returns>
        public static bool ExisteInspeccionDimensional(SamContext ctx, string numeroEstimacion)
        {
            if (ValidacionesInspeccionDimensionalPatio.ExisteReporteDimensional(ctx, numeroEstimacion))
            {
                return true;
            }
            else
            {
                throw new ExcepcionInspeccionDimensionalPatio(new List<string>() { MensajesError.Excepcion_ExisteInspeccionDimensional });
            }
        }

        public static int ObtenerPorNumeroReporte(SamContext ctx, string numeroReporte)
        {
            return ctx.ReporteDimensional.Where(x => x.NumeroReporte == numeroReporte).FirstOrDefault().ReporteDimensionalID;
        }

        public static bool TieneLiberacionDimensional(WorkstatusSpool wks)
        {
             using (SamContext ctx = new SamContext())
             {
                 // Solo si la liberacion Dimensional tiene estatus de  aprobada existe
                 ReporteDimensionalDetalle rdd = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).OrderByDescending(x => x.ReporteDimensionalDetalleID).FirstOrDefault();
                 if (rdd != null)
                 {
                     if (rdd.Aprobado)
                     {
                         return true;
                     }
                 }

                 return false;
             }
         }

         public static void GenerarReporteDimensionalDetalle(int ReporteDimencionalID, int[] ids, string InspDimensionalIDs, Guid UserUID)
         {
             using (SamContext ctx = new SamContext())
             {
                 string WksStatusCVS = "";
                 ReporteDimensional repDim = ctx.ReporteDimensional.SingleOrDefault(x => x.ReporteDimensionalID == ReporteDimencionalID);
                 repDim.StartTracking();
                 InspeccionDimensionalPatio Inspect;
                 string spoolsLiberacionFechaMayor = string.Empty;
                 string spoolsReqPinturaFechaMenor = string.Empty;
                 List<string> erroresLiberacionDimensional = new List<string>();                 
                 string idsEnReporte = string.Empty;

                 foreach (var id in ids)
                 {
                     Inspect = ctx.InspeccionDimensionalPatio.SingleOrDefault(x => x.InspeccionDimensionalPatioID == id);
                 
                     WorkstatusSpool WksStatus = ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == Inspect.WorkstatusSpoolID).Single();
                    
                     if (Inspect.FechaInspeccion > repDim.FechaReporte)
                     {                        
                         OrdenTrabajoSpool ots = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == WksStatus.OrdenTrabajoSpoolID).FirstOrDefault();
                         string nombreSpool = ctx.Spool.Where(x => x.SpoolID == ots.SpoolID).FirstOrDefault().Nombre;
                         spoolsLiberacionFechaMayor += nombreSpool + ", ";
                         continue;
                     }

                     if (WksStatus.TieneRequisicionPintura)
                     {
                         RequisicionPinturaDetalle rpd = ctx.RequisicionPinturaDetalle.Where(x => x.WorkstatusSpoolID == WksStatus.WorkstatusSpoolID).FirstOrDefault();
                         if (rpd != null)
                         {
                             RequisicionPintura rp = ctx.RequisicionPintura.Where(x => x.RequisicionPinturaID == rpd.RequisicionPinturaID).FirstOrDefault();

                             if (repDim.FechaReporte < rp.FechaRequisicion)
                             {
                                 OrdenTrabajoSpool ots = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == WksStatus.OrdenTrabajoSpoolID).FirstOrDefault();
                                 string nombreSpool = ctx.Spool.Where(x => x.SpoolID == ots.SpoolID).FirstOrDefault().Nombre;
                                 spoolsReqPinturaFechaMenor+= nombreSpool + ", ";
                                 continue;
                             }
                         }

                     }
                    
                     if (TieneLiberacionDimensional(WksStatus))
                     {
                         ReporteDimensionalDetalle reporteDimensionalDetalleA = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == WksStatus.WorkstatusSpoolID && x.Aprobado).OrderByDescending(x => x.ReporteDimensionalDetalleID).FirstOrDefault();                     
                
                         string numeroReporte = ctx.ReporteDimensional.Where(x => x.ReporteDimensionalID == reporteDimensionalDetalleA.ReporteDimensionalID).FirstOrDefault().NumeroReporte;
                         OrdenTrabajoSpool ots = ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == WksStatus.OrdenTrabajoSpoolID).FirstOrDefault();
                         string nombreSpool = ctx.Spool.Where(x => x.SpoolID == ots.SpoolID).FirstOrDefault().Nombre;
                         string error = string.Format(MensajesError.Excepcion_SpoolConReporte, nombreSpool, MensajesError.EstatusReporteAprobado, numeroReporte);
                         erroresLiberacionDimensional.Add(error);
                         continue;
                     }


                    ReporteDimensionalDetalle reporteDimensionalDetalle = new ReporteDimensionalDetalle();                       
                    reporteDimensionalDetalle.WorkstatusSpoolID = Inspect.WorkstatusSpoolID;
                    reporteDimensionalDetalle.FechaLiberacion = Inspect.FechaInspeccion;
                    reporteDimensionalDetalle.Aprobado = Inspect.Aprobado;
                    reporteDimensionalDetalle.UsuarioModifica = UserUID;
                    reporteDimensionalDetalle.FechaModificacion = DateTime.Now;
                    //ctx.ReporteDimensionalDetalle.ApplyChanges(reporteDimensionalDetalle);

                    //Si el reporte es aprobado se actualiza el registro de WorkStatus
                    if (Inspect.Aprobado)
                    {
                        WksStatus.StartTracking();
                        WksStatus.TieneLiberacionDimensional = true;
                        WksStatus.UsuarioModifica = UserUID;
                        WksStatus.FechaModificacion = DateTime.Now;
                        WksStatus.StopTracking();
                        WksStatusCVS += WksStatus.WorkstatusSpoolID + ",";
                        ctx.WorkstatusSpool.ApplyChanges(WksStatus);
                    }

                    repDim.ReporteDimensionalDetalle.Add(reporteDimensionalDetalle);                   
                     
                    BorrarInspeccionDimensionalPatio(ctx, Inspect);
                 }

                 repDim.StopTracking();

                 ctx.ReporteDimensional.ApplyChanges(repDim);
                 ctx.SaveChanges();

                 ctx.CalculaHojaParaReporte((int)TipoReporte.ReporteDimensional, repDim.ProyectoID, repDim.NumeroReporte, WksStatusCVS);

                 if (erroresLiberacionDimensional.Count > 0 || !string.IsNullOrEmpty(spoolsLiberacionFechaMayor) || !string.IsNullOrEmpty(spoolsReqPinturaFechaMenor))
                 {
                     List<string> errores = new List<string>();
                     
                     if(!string.IsNullOrEmpty(spoolsLiberacionFechaMayor))
                     {
                         errores.Add(string.Format(MensajesError.Excepcion_FechaInspDimesionalMayorFechaReporte, spoolsLiberacionFechaMayor, repDim.FechaReporte.ToShortDateString()));                    
                     }
                      if(!string.IsNullOrEmpty(spoolsReqPinturaFechaMenor))
                     {
                         errores.Add(string.Format(MensajesError.Exception_FechaInspDimensionalMenorFechaReqPintura, spoolsReqPinturaFechaMenor));
                     }
                        
                     if(erroresLiberacionDimensional.Count < ids.Length && erroresLiberacionDimensional.Count > 0)
                     {
                         erroresLiberacionDimensional.Add(string.Format(MensajesError.SpoolsReporteGenerado,  repDim.NumeroReporte));
                         errores.AddRange(erroresLiberacionDimensional.ToArray());
                     }                   

                     throw new ExcepcionInspeccionDimensionalPatio(errores);
                 }
             }
         }

         public static void BorrarInspeccionDimensionalPatio(SamContext ctx, InspeccionDimensionalPatio Inspect)
         {
             ctx.DeleteObject(Inspect);
             ctx.SaveChanges();
         }

         public static DateTime ObtenerFechaReporteDimensional(int reporteID)
         {
             using (SamContext ctx = new SamContext())
             {
                 return ctx.ReporteDimensional.SingleOrDefault(x => x.ReporteDimensionalID == reporteID).FechaReporte;
             }
         }
    }
}
