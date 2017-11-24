using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Grid;
using System.Transactions;
using System.Data.Objects;

namespace SAM.BusinessObjects.Materiales
{
    public class RecepcionBO
    {
        private static readonly object _mutex = new object();
        private static RecepcionBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private RecepcionBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase RecepcionBO
        /// </summary>
        /// <returns></returns>
        public static RecepcionBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RecepcionBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene la información básica de la recepción de materiales
        /// </summary>
        /// <param name="recepcionID">ID de la Recepción</param>
        /// <returns></returns>
        public Recepcion Obtener(int recepcionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Recepcion.Where(x => x.RecepcionID == recepcionID).SingleOrDefault();
            }
        }


        /// <summary>
        /// Obtiene el listado de recepciones de acuerdo a los filtros
        /// </summary>
        /// <param name="patioID">Patio ID</param>
        /// <param name="proyectoID">Proyecto ID</param>
        /// <param name="fechaDesde">Fecha Inicial de Búsqueda</param>
        /// <param name="fechaHasta">Fecha Final de Búsqueda</param>
        /// <param name="pids">Proyectos a los cuales tiene permiso el usuario</param>
        /// <returns></returns>
        public List<GrdRecepcion> ObtenerConFiltros(int patioID, int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta, int[] pids)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Recepcion.MergeOption = MergeOption.NoTracking;

                IQueryable<Recepcion> iqRecepciones =   from r in ctx.Recepcion
                                                        where r.Proyecto.PatioID == patioID
                                                        select r;

                if (proyectoID > 0)
                {
                    iqRecepciones = iqRecepciones.Where(y => y.ProyectoID == proyectoID);
                }
                else
                {
                    IQueryable<int> iqPids = pids.AsQueryable();
                    iqRecepciones = iqRecepciones.Where(y => iqPids.Contains(y.ProyectoID));
                }

                if (fechaDesde.HasValue)
                {
                    iqRecepciones = iqRecepciones.Where(y => y.FechaRecepcion >= fechaDesde.Value);
                }

                if (fechaHasta.HasValue)
                {
                    iqRecepciones = iqRecepciones.Where(y => y.FechaRecepcion <= fechaHasta.Value);
                }

                return (from r in iqRecepciones
                        select new GrdRecepcion
                        {
                            RecepcionID = r.RecepcionID,
                            CantidadNumerosUnicos = r.RecepcionNumeroUnico.Count,
                            FechaRecepcion = r.FechaRecepcion,
                            Proyecto = r.Proyecto.Nombre,
                            Transportista = r.Transportista.Nombre
                        }).ToList();
            }
        }

        /// <summary>
        /// Elimina la recepción y todas los registros relacionados a menos que algún numero unico ya haya tenido movimientos en inventario o bien haya sido congelado en algún cruce
        /// </summary>
        /// <param name="recepcionID">ID de la recepción</param>
        public void BorraRecepcion(int recepcionID)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (SamContext ctx = new SamContext())
                {                    
                    if (Validations.ValidacionesRecepcion.NumeroUnicoConMovimientos(ctx, recepcionID))
                    {
                        throw new ExcepcionRelaciones(MensajesError.Excepcion_NumeroUnicoConMovimientos);                        
                    }

                    if (Validations.ValidacionesRecepcion.NumeroUnicoCongelado(ctx, recepcionID))
                    {
                        throw new ExcepcionRelaciones(MensajesError.Excepcion_RecepcionConCongelado);
                    }
                    
                    ctx.BorraRecepcion(recepcionID);
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// Recibe entidad y la guarda / actualiza
        /// </summary>
        /// <param name="recepcion"></param>
        public void Guarda(SamContext ctx, Recepcion recepcion)
        {
            try
            {
                ctx.Recepcion.ApplyChanges(recepcion);                
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        /// <summary>
        /// Recibe entidad y la guarda / actualiza
        /// </summary>
        /// <param name="recepcion"></param>
        public void Guarda(Recepcion recepcion)
        {
            using (SamContext ctx = new SamContext())
            {
                Guarda(ctx, recepcion);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene la recepción y los datos del proyecto al que pertenece.
        /// </summary>
        /// <param name="recepcionID"></param>
        /// <returns></returns>
        public Recepcion ObtenerConProyectoConfiguracion(int recepcionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Recepcion
                          .Include("Proyecto")
                          .Include("Proyecto.ProyectoConfiguracion")
                          .Where(x => x.RecepcionID == recepcionID)
                          .SingleOrDefault();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="recepcionID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int recepcionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoRecepcion(ctx, recepcionID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de recepción
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoRecepcion =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Recepcion
                            .Where(x => x.RecepcionID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
