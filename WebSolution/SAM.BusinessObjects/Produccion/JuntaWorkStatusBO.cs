using System;
using System.Collections.Generic;
using System.Linq;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Cache;
using SAM.Entities.Grid;
using System.Data.Objects;
using Mimo.Framework.Common;

namespace SAM.BusinessObjects.Produccion
{
    public class JuntaWorkstatusBO
    {
        private static readonly object _mutex = new object();
        private static JuntaWorkstatusBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private JuntaWorkstatusBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase JuntaWorkstatusBO
        /// </summary>
        /// <returns></returns>
        public static JuntaWorkstatusBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaWorkstatusBO();
                    }
                }
                return _instance;
            }
        }

        public JuntaWorkstatus Obtener(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == juntaWorkstatusID).SingleOrDefault();
            }
        }

        public JuntaWorkstatus ObtenerPorOts(int otsID, int jsID, string etiqueta)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaWorkstatus.Where(x => x.OrdenTrabajoSpoolID == otsID && x.JuntaSpoolID == jsID && x.EtiquetaJunta == etiqueta).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el listado para mostrar el Reporte Inspeccion Visual
        /// de acuerdo a los filtros seleccionados
        /// </summary>
        /// <param name="_proyecto"></param>
        /// <param name="_desde"></param>
        /// <param name="_hasta"></param>
        /// <param name="_numeroReporte"></param>
        /// <returns></returns>
        public List<GrdRepInspeccionVisual> ObtenerReporteInspeccionVisual(int _proyecto, DateTime? _desde, DateTime? _hasta, string _numeroReporte)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<InspeccionVisual> iqInspeccionVisual = ctx.InspeccionVisual.Where(x => x.ProyectoID == _proyecto);
                IQueryable<JuntaInspeccionVisual> iqJuntaInspeccionVisual = ctx.JuntaInspeccionVisual.Where(x => iqInspeccionVisual.Select(y => y.InspeccionVisualID).Contains(x.InspeccionVisualID));

                List<GrdRepInspeccionVisual> lst = (from iv in iqInspeccionVisual.ToList()
                                                    join jiv in iqJuntaInspeccionVisual.ToList() on iv.InspeccionVisualID equals jiv.InspeccionVisualID into det
                                                    from rpt in det.DefaultIfEmpty()
                                                    group rpt by new { iv.InspeccionVisualID, iv.NumeroReporte, iv.FechaReporte, iv.JuntaInspeccionVisual } into reporte

                                                    select new GrdRepInspeccionVisual
                                                    {
                                                        InspeccionVisualID = reporte.Key.InspeccionVisualID,
                                                        NumeroReporte = reporte.Key.NumeroReporte,
                                                        Fecha = reporte.Key.FechaReporte,
                                                        JuntasTotales = reporte.Key.JuntaInspeccionVisual.Count(),
                                                        JuntasAprobadas = reporte.Key.JuntaInspeccionVisual.Count(x => x.Aprobado == true),
                                                        JuntasRechazadas = reporte.Key.JuntaInspeccionVisual.Count(x => x.Aprobado == false)
                                                    }).ToList();


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
        /// 
        /// </summary>
        /// <param name="juntaworkstatusID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int juntaworkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoJuntaWorkstatus(ctx, juntaworkstatusID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de junta workstatus
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoJuntaWorkstatus =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Spool
                            .Where(s => ctx.JuntaSpool
                                           .Where(y => ctx.JuntaWorkstatus
                                                          .Where(jw => jw.JuntaWorkstatusID == id)
                                                          .Select(jw => jw.JuntaSpoolID)
                                                          .Contains(y.JuntaSpoolID))
                                           .Select(js => js.SpoolID)
                                           .Contains(s.SpoolID))
                            .Select(s => s.ProyectoID)
                            .Single()
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaWorkstatusIds"></param>
        /// <returns></returns>
        public int[] ObtenerProyectos(int[] juntaWorkstatusIds)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Spool.MergeOption = MergeOption.NoTracking;

                IQueryable<int> jwIds = juntaWorkstatusIds.AsQueryable();

                //desafortunadamente este query no lo podemos compilar
                return ctx.Spool
                            .Where(s => ctx.JuntaSpool
                                           .Where(y => ctx.JuntaWorkstatus
                                                          .Where(jw => jwIds.Contains(jw.JuntaWorkstatusID))
                                                          .Select(jw => jw.JuntaSpoolID)
                                                          .Contains(y.JuntaSpoolID))
                                           .Select(js => js.SpoolID)
                                           .Contains(s.SpoolID))
                            .Select(s => s.ProyectoID)
                            .ToArray()
                            .Distinct()
                            .ToArray();
            }
        }

        public JuntaWorkstatus ObtenerJuntaWorkStatusPorID(int JuntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<JuntaWorkstatus> JuntaWs =
                    ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == JuntaWorkstatusID);
                IQueryable<OrdenTrabajoSpool> OrdenTrabajoSpool =
                    ctx.OrdenTrabajoSpool.Where(
                        x => JuntaWs.Select(y => y.OrdenTrabajoSpoolID).Contains(x.OrdenTrabajoSpoolID));
                JuntaWs.ToList();
                OrdenTrabajoSpool.ToList();
                return JuntaWs.FirstOrDefault();
            }
        }

        public int[] ObtenerJuntaWorkStatusIdsConInspeccionVisual(int[] juntaWorkStatusIds)
        {
            string ids = string.Join(",", juntaWorkStatusIds);

            using (SamContext ctx = new SamContext())
            {
                var q = from jiv in ctx.JuntaInspeccionVisual
                        where jiv.Aprobado && juntaWorkStatusIds.Contains(jiv.JuntaWorkstatusID)
                        select jiv.JuntaWorkstatusID;

                return q.ToArray();
            }
        }

      }


}

