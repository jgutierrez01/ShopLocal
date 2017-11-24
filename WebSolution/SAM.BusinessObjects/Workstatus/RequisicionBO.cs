using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;
using System.Data.Objects;

namespace SAM.BusinessObjects.Workstatus
{
    public class RequisicionBO
    {
        
        //variables de instancia
        private static readonly object _mutex = new object();
        private static RequisicionBO _instance;

        /// <summary>
        /// constructor para implementar el patrón Singleton.
        /// </summary>
        private RequisicionBO()
        {
        }

        /// <summary>
        /// permite la creación de una instancia de la clase
        /// </summary>
        public static RequisicionBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RequisicionBO();                            
                    }
                }
                return _instance;
            }
        }

        public List<GrdRepRequisicionSpool> ObtenerReporteRequisicionSpool(int proyectoID, DateTime? desde, DateTime? hasta, string numeroRequisicion, int tipoPruebaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<RequisicionSpool> iqRequisicionSpool = ctx.RequisicionSpool.Where(x => x.ProyectoID == proyectoID);
                IQueryable<SpoolRequisicion> iqSpoolRequisicion = ctx.SpoolRequisicion.Where(x => iqRequisicionSpool.Select(y => y.RequisicionSpoolID).Contains(x.RequisicionSpoolID));
                IQueryable<TipoPruebaSpool> iqTipoPruebaSpool = ctx.TipoPruebaSpool.Where(x => iqRequisicionSpool.Select(y => y.TipoPruebaSpoolID).Contains(x.TipoPruebaSpoolID));

                List<GrdRepRequisicionSpool> lst = (from r in iqRequisicionSpool.ToList()
                                                    join sr in iqSpoolRequisicion.ToList() on r.RequisicionSpoolID equals sr.RequisicionSpoolID into sReq
                                                    from jreq in sReq.DefaultIfEmpty()
                                                    join tp in iqTipoPruebaSpool.ToList() on r.TipoPruebaSpoolID equals tp.TipoPruebaSpoolID into det
                                                    from rep in det.DefaultIfEmpty()
                                                    group rep by new { r.RequisicionSpoolID, r.NumeroRequisicion, r.FechaRequisicion, rep.Nombre, r.SpoolRequisicion, rep.TipoPruebaSpoolID } into reporte
                                                    select new GrdRepRequisicionSpool
                                                    {
                                                        RequisicionSpoolID = reporte.Key.RequisicionSpoolID,
                                                        NumeroRequisicion = reporte.Key.NumeroRequisicion,
                                                        Fecha = reporte.Key.FechaRequisicion,
                                                        Tipo = reporte.Key.Nombre,
                                                        SpoolsTotales = reporte.Key.SpoolRequisicion.Count(),
                                                        TipoPruebaSpoolID = reporte.Key.TipoPruebaSpoolID
                                                    }).ToList();
                if (desde.HasValue)
                {
                    lst = lst.Where(x => x.Fecha >= desde).ToList();
                }

                if (hasta.HasValue)
                {
                    lst = lst.Where(x => x.Fecha <= hasta).ToList();
                }

                if (!string.IsNullOrEmpty(numeroRequisicion))
                {
                    lst = lst.Where(x => x.NumeroRequisicion == numeroRequisicion).ToList();
                }

                if (tipoPruebaSpoolID > 0)
                {
                    lst = lst.Where(x => x.TipoPruebaSpoolID == tipoPruebaSpoolID).ToList();
                }

                return lst;
            }
        }

        public List<GrdRepRequisicion> ObtenerReporteRequisicion(int proyectoID, DateTime? desde, DateTime? hasta, string numeroRequisicion, int tipoPruebaID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Requisicion> iqRequisicion = ctx.Requisicion.Where(x => x.ProyectoID == proyectoID);
                IQueryable<JuntaRequisicion> iqJuntaRequisicion = ctx.JuntaRequisicion.Where(x => iqRequisicion.Select(y => y.RequisicionID).Contains(x.RequisicionID));
                IQueryable<TipoPrueba> iqTipoPrueba = ctx.TipoPrueba.Where(x => iqRequisicion.Select(y => y.TipoPruebaID).Contains(x.TipoPruebaID));

                List<GrdRepRequisicion> lst = (from r in iqRequisicion.ToList()
                                               join jr in iqJuntaRequisicion.ToList() on r.RequisicionID equals jr.RequisicionID into jtaReq
                                               from jreq in jtaReq.DefaultIfEmpty()
                                               join tp in iqTipoPrueba.ToList() on r.TipoPruebaID equals tp.TipoPruebaID into det
                                               from rep in det.DefaultIfEmpty()
                                               group rep by new { r.RequisicionID, r.NumeroRequisicion, r.FechaRequisicion, rep.Nombre, r.JuntaRequisicion,rep.TipoPruebaID } into reporte
                                               select new GrdRepRequisicion 
                                               {
                                                   RequisicionID = reporte.Key.RequisicionID,
                                                   NumeroRequisicion = reporte.Key.NumeroRequisicion,
                                                   Fecha = reporte.Key.FechaRequisicion,
                                                   Tipo = reporte.Key.Nombre,
                                                   JuntasTotales = reporte.Key.JuntaRequisicion.Count(),
                                                   TipoPruebaID = reporte.Key.TipoPruebaID
                                               }).ToList();

                if (desde.HasValue)
                {
                    lst = lst.Where(x => x.Fecha >= desde).ToList();
                }

                if (hasta.HasValue)
                {
                    lst = lst.Where(x => x.Fecha <= hasta).ToList();
                }

                if (!string.IsNullOrEmpty(numeroRequisicion))
                {
                    lst = lst.Where(x => x.NumeroRequisicion == numeroRequisicion).ToList();
                }

                if (tipoPruebaID > 0)
                {
                    lst = lst.Where(x => x.TipoPruebaID == tipoPruebaID).ToList();
                }

                return lst;
            }
        }

        public Requisicion DetalleRequisicion(int requisicionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Requisicion.Include("Proyecto")
                                      .Include("TipoPrueba")
                                      .Include("JuntaRequisicion")
                                      .Where(x => x.RequisicionID == requisicionID).Single();
            }
        }

        public RequisicionSpool DetalleRequisicionSpool(int requisicionSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.RequisicionSpool.Include("Proyecto")
                                           .Include("TipoPruebaSpool")
                                           .Include("SpoolRequisicion")
                                           .Where(x => x.RequisicionSpoolID == requisicionSpoolID).Single();
            }
        }

        public void Borra(int requisicionID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (Validations.ValidacionesRequisicion.TieneJuntaRequisicion(ctx, requisicionID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneJuntaRequisicion);
                }

                Requisicion r = ctx.Requisicion.Where(x => x.RequisicionID == requisicionID).Single();
                
                ctx.DeleteObject(r);
                ctx.SaveChanges();
            }
        }

        public void BorraReqSpool(int requisicionSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (Validations.ValidacionesRequisicion.TieneSpoolRequisicion(ctx, requisicionSpoolID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneSpoolRequisicion);
                }

                RequisicionSpool r = ctx.RequisicionSpool.Where(x => x.RequisicionSpoolID == requisicionSpoolID).Single();

                ctx.DeleteObject(r);
                ctx.SaveChanges();
            }
        }

        public Requisicion Obtener(int requisicionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Requisicion.Where(x => x.RequisicionID == requisicionID).Single();
            }
        }

        public RequisicionSpool ObtenerReqSpool(int requisicionSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.RequisicionSpool.Where(x => x.RequisicionSpoolID == requisicionSpoolID).Single();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisicionID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int requisicionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoRequisicion(ctx, requisicionID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisicionSpoolID"></param>
        /// <returns></returns>
        public int ObtenerProyectoIDPorRequisicionSpool(int requisicionSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoRequisicionSpool(ctx, requisicionSpoolID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de requisiciones
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoRequisicion =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Requisicion
                            .Where(x => x.RequisicionID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );

        /// <summary>
        /// Versión compilada del query para permisos de requisiciones
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoRequisicionSpool =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.RequisicionSpool
                            .Where(x => x.RequisicionSpoolID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
