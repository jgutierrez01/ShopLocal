using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Grid;
using System.Data;
using System.Data.Objects;
using Mimo.Framework.Common;

namespace SAM.BusinessObjects.Materiales
{
    public class RequisicionNumeroUnicoBO
    {
        private static readonly object _mutex = new object();
        private static RequisicionNumeroUnicoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private RequisicionNumeroUnicoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase RequisicionNumeroUnicoBO
        /// </summary>
        /// <returns></returns>
        public static RequisicionNumeroUnicoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RequisicionNumeroUnicoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene la informacion de RequisicionNumeroUnico
        /// </summary>
        /// <param name="reqDetalleID">RequisicionNumeroUnicoID</param>
        /// <returns></returns>
        public RequisicionNumeroUnico Obtener(int reqNumUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.RequisicionNumeroUnico.Where(x => x.RequisicionNumeroUnicoID == reqNumUnicoID).SingleOrDefault();
            }
        }
        /// <summary>
        /// Obtiene la informacion de RequisicionNumeroUnico por número de Requisición
        /// </summary>
        /// <param name="numRequisicion">NumeroRequisicion</param>
        /// <returns></returns>
        public RequisicionNumeroUnico ObtenerPorNumeroRequisicion(string numRequisicion)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.RequisicionNumeroUnico.Where(x => x.NumeroRequisicion == numRequisicion).Single();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="numRequisicion"></param>
        /// <returns></returns>
        public List<GrdRepReqPinturaNumUnico> ObtenerReportePinturaNumUnicoFiltrado(int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta, string numRequisicion)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.RequisicionNumeroUnico.MergeOption = MergeOption.NoTracking;
                ctx.RequisicionNumeroUnicoDetalle.MergeOption = MergeOption.NoTracking;

                IQueryable<RequisicionNumeroUnico> iqRequisicion = ctx.RequisicionNumeroUnico.Where(x => x.ProyectoID == proyectoID);

                List<GrdRepReqPinturaNumUnico> lst = (from rnu in iqRequisicion
                                                      join rnud in ctx.RequisicionNumeroUnicoDetalle on rnu.RequisicionNumeroUnicoID equals rnud.RequisicionNumeroUnicoID into j1
                                                      from q1 in j1.DefaultIfEmpty()
                                                      select new GrdRepReqPinturaNumUnico
                                                      {
                                                          RequisicionNumeroUnicoID = rnu.RequisicionNumeroUnicoID,
                                                          NumeroRequisicion = rnu.NumeroRequisicion,
                                                          Fecha = (DateTime)rnu.FechaRequisicion,
                                                          CantidadNumerosUnicos = rnu.RequisicionNumeroUnicoDetalle.Count()
                                                      }).Distinct().ToList();
                if (fechaDesde.HasValue)
                {
                    lst = lst.Where(x => x.Fecha >= fechaDesde).ToList();
                }

                if (fechaHasta.HasValue)
                {
                    lst = lst.Where(x => x.Fecha <= fechaHasta).ToList();
                }

                if (numRequisicion != string.Empty)
                {
                    lst = lst.Where(x => x.NumeroRequisicion == numRequisicion).ToList();
                }

                return lst;                 

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nuIds"></param>
        /// <param name="proyectoID"></param>
        /// <param name="numRequisicion"></param>
        /// <param name="fechaRequisicion"></param>
        /// <param name="userID"></param>
        public void GenerarRequisicion(int[] nuIds, int proyectoID, string numRequisicion, DateTime? fechaRequisicion, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                RequisicionNumeroUnico rnu = ctx.RequisicionNumeroUnico
                                                        .Where(x => x.ProyectoID == proyectoID && x.NumeroRequisicion == numRequisicion)
                                                        .SingleOrDefault();

                if (rnu != null)
                {
                    //validafechas
                    ctx.LoadProperty<RequisicionNumeroUnico>(rnu, x => x.RequisicionNumeroUnicoDetalle);
                    rnu.StartTracking();

                    if (rnu.NumeroRequisicion != numRequisicion || rnu.FechaRequisicion != fechaRequisicion)
                    {
                        throw new ExcepcionConcordancia(MensajesError.Excepcion_FechaInconsistente);
                    }

                }
                else
                {
                    rnu = new RequisicionNumeroUnico()
                     {
                        ProyectoID = proyectoID,
                        NumeroRequisicion = numRequisicion,
                        FechaRequisicion = fechaRequisicion,
                        FechaModificacion = DateTime.Now,
                        UsuarioModifica = userID,
                     };
                }

                List<NumeroUnico> lstNus = ctx.NumeroUnico.Where(Expressions.BuildOrExpression<NumeroUnico, int>(x => x.NumeroUnicoID, nuIds)).ToList();

                NumeroUnico nu;

                foreach (int numeroUnicoID in nuIds)
                {
                    nu = lstNus.Where(x => x.NumeroUnicoID == numeroUnicoID).Single();

                    ctx.NumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoID).Single();

                    if (!rnu.RequisicionNumeroUnicoDetalle.Any(x => x.NumeroUnicoID == numeroUnicoID))
                    {
                        rnu.RequisicionNumeroUnicoDetalle.Add(
                            new RequisicionNumeroUnicoDetalle
                            {
                                NumeroUnicoID = numeroUnicoID,
                                FechaModificacion = DateTime.Now,
                                UsuarioModifica = userID,
                                NumeroUnico = nu
                            });
                    }
                }

                ctx.RequisicionNumeroUnico.ApplyChanges(rnu);
                ctx.SaveChanges();

            }
        }


        public RequisicionNumeroUnico DetalleRequisicionPinturaNumUnico(int requisicionNumUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.RequisicionNumeroUnico.Include("Proyecto")
                                                 .Include("RequisicionNumeroUnicoDetalle")
                                                 .Where(x => x.RequisicionNumeroUnicoID == requisicionNumUnicoID).Single();
            }
        }

        public void Borra(int requisicionNumeroUnicoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesRequisicionNumeroUnico.TieneRelacionesDetalle(ctx,requisicionNumeroUnicoID))
                    {
                        throw new ExcepcionRelaciones(new List<string>() { MensajesError.Excepcion_RequisicionNumeroUnicoRelacion });
                    }

                    RequisicionNumeroUnico numUnico = ctx.RequisicionNumeroUnico.Where(x => x.RequisicionNumeroUnicoID == requisicionNumeroUnicoID).Single();
                    ctx.RequisicionNumeroUnico.DeleteObject(numUnico);

                    ctx.SaveChanges();
                }

            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requisicionNumeroUnicoID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int requisicionNumeroUnicoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoRequisicionNumeroUnico(ctx, requisicionNumeroUnicoID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de recepción
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoRequisicionNumeroUnico =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.RequisicionNumeroUnico
                            .Where(x => x.RequisicionNumeroUnicoID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
