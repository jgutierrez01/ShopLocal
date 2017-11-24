using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;

namespace SAM.BusinessObjects.Workstatus
{
    public class ReporteRequisicionPinturaBO
    {
        private static readonly object _mutex = new object();
        private static ReporteRequisicionPinturaBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private ReporteRequisicionPinturaBO()
        {
            
        }

        /// <summary>
        /// Obtiene la instancia de la clase 
        /// </summary>
        public static ReporteRequisicionPinturaBO Instance
        {
             get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReporteRequisicionPinturaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de estimaciones de acuerdo a los filtros
        /// </summary>
        /// <param name="numeroDeRequisicion">numeroEstimación de la tabla Estimacion</param>
        /// <param name="proyectoID">Proyecto ID de la tabla Estimacion</param>
        /// <param name="fechaDesde">Fecha Inicial de Búsqueda de la tabla Estimacion</param>
        /// <param name="fechaHasta">Fecha Final de Búsqueda de la tabla Estimacion</param>
        /// <returns></returns>
        public List<GrdReporteReqPintura> ObtenerConFiltros(int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta, string numeroDeRequisicion)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<RequisicionPintura> iqReporteRequisicionPintura = from e in ctx.RequisicionPintura
                                                                      where e.ProyectoID == proyectoID
                                                                      select e;

                if (fechaDesde.HasValue)
                {
                    iqReporteRequisicionPintura = iqReporteRequisicionPintura.Where(y => y.FechaRequisicion >= fechaDesde.Value);
                }

                if (fechaHasta.HasValue)
                {
                    iqReporteRequisicionPintura = iqReporteRequisicionPintura.Where(y => y.FechaRequisicion <= fechaHasta.Value);
                }

                if (!String.IsNullOrEmpty(numeroDeRequisicion))
                {
                    iqReporteRequisicionPintura = iqReporteRequisicionPintura.Where(y => y.NumeroRequisicion == numeroDeRequisicion);
                }

                iqReporteRequisicionPintura.ToList();

                return (from r in iqReporteRequisicionPintura
                        select new GrdReporteReqPintura()
                                   {
                                       RequisicionPinturaID = r.RequisicionPinturaID,
                                       NumeroDeRequisicion = r.NumeroRequisicion,
                                       Fecha = r.FechaRequisicion,
                                       Spools = r.RequisicionPinturaDetalle.Count()
                        }).ToList();
            }
        }

        public RequisicionPintura Obtener(int requisicionPinturaID)
        {
            using(SamContext ctx = new SamContext())
            {
                return ctx.RequisicionPintura.SingleOrDefault(x => x.RequisicionPinturaID == requisicionPinturaID);
            }
        }

        public void Borra(int requisicionPinturaID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool tieneMovimientos = ValidacionesRequisicionPintura.TienePinturaDetalle(ctx, requisicionPinturaID);

                if (!tieneMovimientos)
                {
                    RequisicionPintura requisicionPintura = ctx.RequisicionPintura.Where(x => x.RequisicionPinturaID == requisicionPinturaID).SingleOrDefault();
                    ctx.DeleteObject(requisicionPintura);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneDetallePintura);
                }
            }
        }


        public RequisicionPintura ObtenerDetalleRequisicionPintura(int requisicionPinturaID)
            {
                using (SamContext ctx = new SamContext())
                {
                    return (from rep in ctx.RequisicionPintura.ToList()
                            join proy in ctx.Proyecto.ToList()
                                      on rep.ProyectoID
                                      equals proy.ProyectoID
                            join rpdet in ctx.RequisicionPinturaDetalle.ToList()
                                       on rep.RequisicionPinturaID
                                       equals rpdet.RequisicionPinturaID
                            where rep.RequisicionPinturaID == requisicionPinturaID
                            select rep).FirstOrDefault();
                }
            }

        public RequisicionPintura ObtenerNumeroSpoolsRequisicionPintura(int requisicionPinturaID)
            {
                using (SamContext ctx = new SamContext())
                {
                    IQueryable<RequisicionPintura> iqRequisicionPintura = ctx.RequisicionPintura.Where(x => x.RequisicionPinturaID == requisicionPinturaID);
                    IQueryable<RequisicionPinturaDetalle> iqRequisicionPinturaDetalle =
                        ctx.RequisicionPinturaDetalle.Where(x => iqRequisicionPintura.Select(y => y.RequisicionPinturaID).Contains(x.RequisicionPinturaID));
                    iqRequisicionPintura.ToList();
                    iqRequisicionPinturaDetalle.ToList();
                    return iqRequisicionPintura.FirstOrDefault();
                }

            }
    }
}
