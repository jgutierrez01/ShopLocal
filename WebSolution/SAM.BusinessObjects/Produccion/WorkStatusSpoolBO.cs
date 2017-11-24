using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Globalization;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using Mimo.Framework.Common;

namespace SAM.BusinessObjects.Produccion
{
    public class WorkstatusSpoolBO
    {
        private static readonly object _mutex = new object();
        private static WorkstatusSpoolBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private WorkstatusSpoolBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase WorkStatusSpoolBO
        /// </summary>
        /// <returns></returns>
        public static WorkstatusSpoolBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new WorkstatusSpoolBO();
                    }
                }
                return _instance;
            }
        }

        public WorkstatusSpool Obtener(int workStatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.WorkstatusSpool.Where(x => x.WorkstatusSpoolID == workStatusSpoolID).SingleOrDefault();
            }
        }

        public List<WorkstatusSpool> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.WorkstatusSpool.ToList();
            }
        }

        public WorkstatusSpool ObtenerPorOrdenTrabajoSpool(int ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID).FirstOrDefault();
            }
        }

        public WorkstatusSpool ObtenerPorJuntaSpool(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaSpool js = ctx.JuntaSpool.Single(x => x.JuntaSpoolID == juntaSpoolID);

                return (from s in ctx.Spool
                        join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID
                        join ws in ctx.WorkstatusSpool on ots.OrdenTrabajoSpoolID equals ws.OrdenTrabajoSpoolID
                        where s.SpoolID == js.SpoolID
                        select ws
                        ).SingleOrDefault();
            }
        }


        public OrdenTrabajoSpool ObtenerODTSpoolPorJuntaSpool(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaSpool js = ctx.JuntaSpool.Single(x => x.JuntaSpoolID == juntaSpoolID);

                return (from s in ctx.Spool
                        join ots in ctx.OrdenTrabajoSpool on s.SpoolID equals ots.SpoolID
                        where s.SpoolID == js.SpoolID
                        select ots).SingleOrDefault(); 
            }
        }

        /// <summary>
        /// Obtiene el listado para mostrar el Reporte Inspeccion Dimensional
        /// de acuerdo a los filtros seleccionados
        /// </summary>
        /// <param name="_proyecto"></param>
        /// <param name="_desde"></param>
        /// <param name="_hasta"></param>
        /// <param name="_numeroReporte"></param>
        /// <returns></returns>
        public List<GrdRepInspeccionDimensional> ObtenerReporteInspeccionDimensional(int _proyecto, DateTime? _desde, DateTime? _hasta, string _numeroReporte)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<ReporteDimensional> iqReporteDimensional = ctx.ReporteDimensional.Where(x => x.ProyectoID == _proyecto);
                IQueryable<ReporteDimensionalDetalle> iqReporteDimensionalDetalle = ctx.ReporteDimensionalDetalle.Where(x => iqReporteDimensional.Select(y => y.ReporteDimensionalID).Contains(x.ReporteDimensionalID));
                IQueryable<TipoReporteDimensional> iqTipoReporteDimensionalDetalle = ctx.TipoReporteDimensional.Where(x => iqReporteDimensional.Select(y => y.TipoReporteDimensionalID).Contains(x.TipoReporteDimensionalID));
                iqReporteDimensional.ToList();
                iqReporteDimensionalDetalle.ToList();
                iqTipoReporteDimensionalDetalle.ToList();
                List<GrdRepInspeccionDimensional> lst = (from rd in iqReporteDimensional
                                                         join rdd in iqReporteDimensionalDetalle on rd.ReporteDimensionalID equals rdd.ReporteDimensionalID into j1
                                                         join trd in iqTipoReporteDimensionalDetalle on rd.TipoReporteDimensionalID equals trd.TipoReporteDimensionalID into j2

                                                         from q1 in j1.DefaultIfEmpty()
                                                         from q2 in j2.DefaultIfEmpty()

                                                         select new GrdRepInspeccionDimensional
                                                         {
                                                             ReporteDimensionalID = rd.ReporteDimensionalID,
                                                             TipoReporteDimensionalID = rd.TipoReporteDimensionalID,
                                                             NumeroReporte = rd.NumeroReporte,
                                                             Fecha = rd.FechaReporte,
                                                             Tipo = (q2 != null) ? q2.Nombre : string.Empty,
                                                             SpoolsTotales = rd.ReporteDimensionalDetalle.Count(),
                                                             SpoolsAprobados = rd.ReporteDimensionalDetalle.Count(x => x.Aprobado == true),
                                                             SpoolsRechazados = rd.ReporteDimensionalDetalle.Count(x => x.Aprobado == false),
                                                             TipoReporte = rd.TipoReporteDimensional.Nombre,
                                                             TipoReporteIngles = rd.TipoReporteDimensional.NombreIngles
                                                         }).Distinct().ToList();

                //lst = (from p in lst
                //       group p by new { p.NumeroReporte } into g
                //       select g).ToList();


                if (_desde.HasValue)
                {
                    lst = lst.Where(x => x.Fecha >= _desde).ToList();
                }

                if (_hasta.HasValue)
                {
                    lst = lst.Where(x => x.Fecha <= _hasta).ToList();
                }

                if (_numeroReporte != string.Empty)
                {
                    lst = lst.Where(x => x.NumeroReporte == _numeroReporte).ToList();
                }

                return lst;
            }
        }

        /// <summary>
        /// Especifica el sistema para la pintura del spool
        /// </summary>
        /// <param name="ids">Arreglo de enteros con los SpoolIDs a especificar sistema</param>
        /// <param name="sistema"></param>
        /// <param name="color"></param>
        /// <param name="codigo"></param>
        public void EspecificarSistema(int[] ids, string sistema, string color, string codigo)
        {
            using (SamContext ctx = new SamContext())
            {
                List<Spool> s = ctx.Spool.Where(x => ids.Contains(x.SpoolID)).ToList();
                s.ForEach(x => cambiaSistema(ctx, x, sistema, color, codigo));

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Especifica el sistema para un objeto en particular
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="wks"></param>
        /// <param name="sistema"></param>
        /// <param name="color"></param>
        /// <param name="codigo"></param>
        private void cambiaSistema(SamContext ctx, Spool s, string sistema, string color, string codigo)
        {
            s.StartTracking();
            s.SistemaPintura = sistema;
            s.ColorPintura = color;
            s.CodigoPintura = codigo;
            s.StopTracking();

            ctx.Spool.ApplyChanges(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workstatusSpoolIds"></param>
        /// <returns></returns>
        public int[] ObtenerProyectos(int[] workstatusSpoolIds)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.OrdenTrabajoSpool.MergeOption = MergeOption.NoTracking;

                IQueryable<int> wsSpIds = workstatusSpoolIds.AsQueryable();

                return ctx.Spool
                            .Where(s => ctx.OrdenTrabajoSpool
                                           .Where(odts => ctx.WorkstatusSpool
                                                              .Where(ws => wsSpIds.Contains(ws.WorkstatusSpoolID))
                                                              .Select(ws => ws.OrdenTrabajoSpoolID)
                                                              .Contains(odts.OrdenTrabajoSpoolID))
                                           .Select(odts => odts.SpoolID)
                                           .Contains(s.SpoolID))
                            .Select(s => s.ProyectoID)
                            .ToArray()
                            .Distinct()
                            .ToArray();
            }
        }

        public WorkstatusSpool ObtenerConPintura(int workStatusSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.WorkstatusSpool.Include("OrdenTrabajoSpool").Include("Spool").Include("PinturaSpool").Where(x => x.WorkstatusSpoolID == workStatusSpoolID).SingleOrDefault();
            }
        }

        public OrdenTrabajoSpool ObtenerConPinturaOts(int otsId) 
        { 
            using (SamContext ctx = new SamContext()) 
            { 
               return ctx.OrdenTrabajoSpool.Include("Spool").Where(x => x.OrdenTrabajoSpoolID == otsId).SingleOrDefault(); 
            } 
        } 

 
        public WorkstatusSpool ObtenerConPinturaWSS(int wssId) 
        { 
            using (SamContext ctx = new SamContext()) 
            {
                return ctx.WorkstatusSpool.Include("PinturaSpool").Where(x => x.OrdenTrabajoSpoolID == wssId).SingleOrDefault(); 
            } 
        }

        public WorkstatusSpool ObtenerConEmbarque(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.WorkstatusSpool.Include("OrdenTrabajoSpool").Include("EmbarqueSpool").Include("EmbarqueSpool.Embarque").Where(x => x.OrdenTrabajoSpool.SpoolID == spoolID).SingleOrDefault();
            }
        }

        public void GuardarFechaLiberacionCalidad(DateTime fechaLiberacion, int ordenTrabajoSpoolID, Guid usuarioID)
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wksSpool = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID.Equals(ordenTrabajoSpoolID)).Single();
                wksSpool.StartTracking();
                wksSpool.FechaLiberacionCalidad = fechaLiberacion;
                wksSpool.FechaModificacion = DateTime.Now.Date;
                wksSpool.UsuarioLiberacionCalidad = usuarioID;
                wksSpool.UsuarioModifica = usuarioID;
                wksSpool.StopTracking();
                ctx.WorkstatusSpool.ApplyChanges(wksSpool);
                ctx.SaveChanges();
            }
        }

        public void BorrarFechaLiberacionCalidad(DateTime fechaLiberacion, int ordenTrabajoSpoolID, Guid usuarioID) 
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wksSpool = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID.Equals(ordenTrabajoSpoolID)).Single();
                wksSpool.StartTracking();
                wksSpool.FechaLiberacionCalidad =null;
                wksSpool.FechaModificacion = DateTime.Now.Date;
                wksSpool.UsuarioLiberacionCalidad = null;
                wksSpool.UsuarioModifica = usuarioID;
                wksSpool.StopTracking();
                ctx.WorkstatusSpool.ApplyChanges(wksSpool);
                ctx.SaveChanges();
            }
        }
        public void GuardarFechasLiberacionCalidad(DateTime fechaLiberacion, int[] ordenTrabajoSpoolIDs, Guid usuarioID)
        {
            foreach (int ots in ordenTrabajoSpoolIDs)
            {
                WorkstatusSpoolBO.Instance.GuardarFechaLiberacionCalidad(fechaLiberacion, ots, usuarioID);
            }
        }

        public void GuardarFechaLiberacionMaterial(DateTime fechaLiberacion, int ordenTrabajoSpoolID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                WorkstatusSpool wksSpool = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID.Equals(ordenTrabajoSpoolID)).Single();
                wksSpool.StartTracking();
                wksSpool.FechaLiberacionMateriales = fechaLiberacion;
                wksSpool.FechaModificacion = DateTime.Now;
                wksSpool.UsuarioLiberacionMateriales = userID;
                wksSpool.UsuarioModifica = userID;
                wksSpool.StopTracking();
                ctx.WorkstatusSpool.ApplyChanges(wksSpool);
                ctx.SaveChanges();
            }
        }

        public DateTime? ObtenerFechaLiberacionCalidad(int? ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ordenTrabajoSpoolID.HasValue)
                {
                    return ctx.WorkstatusSpool.Where(
                        x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                        .Select(x => x.FechaLiberacionCalidad).SingleOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime? ObtnerFechaLiberacionMateriales(int? ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ordenTrabajoSpoolID.HasValue)
                {
                    return ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                              .Select(x => x.FechaLiberacionMateriales).SingleOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public List<WorkstatusSpool> ObtenerWorkstatus(int[] ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<int> otsIds = ordenTrabajoSpoolID.AsQueryable();

                return ctx.WorkstatusSpool.Where(ws => otsIds.Contains(ws.OrdenTrabajoSpoolID)).ToList();

            }
        }    
        
        public int[] ObtenerOtsSpoolSistemaEmpty(int[] ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<int> otsIds = ordenTrabajoSpoolID.AsQueryable();


                List<int> lstSpoolIds =  ctx.OrdenTrabajoSpool.Where(ws => otsIds.Contains(ws.OrdenTrabajoSpoolID)).Select(x => x.SpoolID).ToList();


                List<int> spoolsIds = ctx.Spool.Where(x => lstSpoolIds.Contains(x.SpoolID) && string.IsNullOrEmpty(x.SistemaPintura)).Select(x => x.SpoolID).ToList();


                return ctx.OrdenTrabajoSpool.Where(x => spoolsIds.Contains(x.SpoolID)).Select(x => x.OrdenTrabajoSpoolID).ToArray();;
            }
        }


        public bool TienePermisoBorrarLiberacionCalidad(int PerfilID, string cultura, string nombrepermiso, bool esAdministradorSistema)
        {
            using (SamContext ctx = new SamContext())
            {
                Permiso permiso;
                if (!esAdministradorSistema)
                {
                    if (cultura != LanguageHelper.INGLES)
                        permiso = ctx.Permiso.Where(x => x.Nombre == nombrepermiso).SingleOrDefault();
                    else
                        permiso = ctx.Permiso.Where(x => x.NombreIngles == nombrepermiso).SingleOrDefault();

                    if (permiso != null)
                        return ctx.PerfilPermiso.Where(x => x.PerfilID == PerfilID && x.PermisoID == permiso.PermisoID).Any();
                    else
                        return false;
                }
                else
                {
                    return true;
                }

            }
        }
        public void GuardarWorkstatus(WorkstatusSpool wss)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.WorkstatusSpool.AddObject(wss);
                ctx.SaveChanges();
            }
        }
    }
}

