using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mimo.Framework.Common;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using System.Data.Objects;

namespace SAM.BusinessObjects.Produccion
{
    public class EstimacionBO
    {
        private static readonly object _mutex = new object();
        private static EstimacionBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private EstimacionBO()
        {
            
        }

        /// <summary>
        /// Obtiene la instancia de la clase EstimacionBO
        /// </summary>
        public static EstimacionBO Instance
        {
             get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EstimacionBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de estimaciones de acuerdo a los filtros
        /// </summary>
        /// <param name="numeroEstimacion">numeroEstimación de la tabla Estimacion</param>
        /// <param name="proyectoID">Proyecto ID de la tabla Estimacion</param>
        /// <param name="fechaDesde">Fecha Inicial de Búsqueda de la tabla Estimacion</param>
        /// <param name="fechaHasta">Fecha Final de Búsqueda de la tabla Estimacion</param>
        /// <returns></returns>
        public List<GrdEstimado> ObtenerConFiltros(int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta, string numeroEstimacion)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Estimacion> iqEstimacion = from e in ctx.Estimacion
                                          where e.ProyectoID == proyectoID
                                          select e;

                if (fechaDesde.HasValue)
                {
                    iqEstimacion = iqEstimacion.Where(y => y.FechaEstimacion >= fechaDesde.Value);
                }
                
                if (fechaHasta.HasValue)
                {
                    iqEstimacion = iqEstimacion.Where(y => y.FechaEstimacion <= fechaHasta.Value);
                }

                if (!String.IsNullOrEmpty(numeroEstimacion))
                {
                    iqEstimacion = iqEstimacion.Where(y => y.NumeroEstimacion == numeroEstimacion);
                }

                return (from r in iqEstimacion
                        select new GrdEstimado
                        {
                            EstimadoID = r.EstimacionID,
                            NumeroEstimacion = r.NumeroEstimacion,
                            FechaEstimacion = r.FechaEstimacion,
                            NumeroJunta = r.EstimacionJunta.Count, 
                            NumeroSpools = r.EstimacionSpool.Count,
                            Proyecto = r.Proyecto.Nombre
                        }).ToList();
            }
        }

        /// <summary>
        /// Borra la estimacion que no tenga ningun movimiento
        /// </summary>
        /// <param name="estimacionID">Id de la estimacion a borrar</param>
        public void Borra(int estimacionID)
        {            
            using (SamContext ctx = new SamContext())
            {
                bool tieneMovimientos = ValidacionesEstimacion.TieneJuntasOSpools(ctx, estimacionID);

                if (!tieneMovimientos)
                {
                    Estimacion estimacion = ctx.Estimacion.Where(x => x.EstimacionID == estimacionID).SingleOrDefault();
                    ctx.DeleteObject(estimacion);
                    ctx.SaveChanges();
                }
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_NumeroUnicoConMovimientos);
                }
            }               
        }

        /// <summary>
        /// Obtiene la información de la estimacion, incluyendo la información del proyecto, de las juntas, y de los spools
        /// </summary>
        /// <param name="estimacionID">int: EstimacionID</param>
        /// <returns></returns>
        public Estimacion ObtenerConProyectoYDetalle(int estimacionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Estimacion
                          .Where(x => x.EstimacionID == estimacionID)
                          .SingleOrDefault();
            }
        }

        /// <summary>
        /// Genera el GrdEstimacionJunta de diferentes tablas
        /// </summary>
        /// <param name="estimacionID">estimacion Id</param>
        /// <returns>Lista de GrdEstimacionJunta</returns>
        public List<GrdEstimacionJunta> ObtenerEstimacionJuntaPorEstimacionID(int estimacionID)
        {
            List<EstimacionJunta> lstEstJta;

            using (SamContext ctx = new SamContext())
            {
                IQueryable<EstimacionJunta> iqEstimacionJunta = 
                    ctx.EstimacionJunta
                       .Where(x => x.EstimacionID == estimacionID);

                IQueryable<JuntaWorkstatus> iqJuntaWorkstatus =
                    ctx.JuntaWorkstatus
                       .Where(x => iqEstimacionJunta.Select(y => y.JuntaWorkstatusID)
                                                    .Contains(x.JuntaWorkstatusID));

                IQueryable<JuntaSpool> iqJuntaSpool = 
                    ctx.JuntaSpool
                       .Where(x => iqJuntaWorkstatus.Select(y => y.JuntaSpoolID)
                                                    .Contains(x.JuntaSpoolID));

                IQueryable<OrdenTrabajoSpool> iqOdtSpool =
                    ctx.OrdenTrabajoSpool
                       .Where(x => iqJuntaWorkstatus.Select(y => y.OrdenTrabajoSpoolID)
                                                    .Contains(x.OrdenTrabajoSpoolID));

                IQueryable<Spool> iqSpool = ctx.Spool.Where(x => iqOdtSpool.Select(y => y.SpoolID).Contains(x.SpoolID));

                //Traer al contexto
                lstEstJta = iqEstimacionJunta.ToList();
                ctx.ConceptoEstimacion.ToList(); 
                iqJuntaWorkstatus.ToList();
                iqJuntaSpool.ToList();
                iqOdtSpool.ToList();
                iqSpool.ToList();
            }

            Dictionary<int, string> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> dicJta = CacheCatalogos.Instance
                                                           .ObtenerTiposJunta()
                                                           .ToDictionary(x => x.ID, y => y.Nombre);
            if (LanguageHelper.CustomCulture == LanguageHelper.ESPANOL)
            {
                //regresa la informacion del grid
                return (from r in lstEstJta
                        let s = r.JuntaWorkstatus.OrdenTrabajoSpool.Spool
                        select new GrdEstimacionJunta
                                   {
                                       EstimacionID = r.EstimacionID,
                                       Concepto = r.ConceptoEstimacion.Nombre,
                                       Diametro = r.JuntaWorkstatus.JuntaSpool.Diametro,
                                       TipoJunta = dicJta[r.JuntaWorkstatus.JuntaSpool.TipoJuntaID],
                                       Etiqueta = r.JuntaWorkstatus.EtiquetaJunta,
                                       EstimadoJuntaID = r.EstimacionJuntaID,
                                       NumeroControl = r.JuntaWorkstatus.OrdenTrabajoSpool.NumeroControl,
                                       NombreSpool = r.JuntaWorkstatus.OrdenTrabajoSpool.Spool.Nombre,
                                       Material = famAcero[s.FamiliaAcero1ID] + (s.FamiliaAcero2ID != null
                                                                                 && s.FamiliaAcero2ID != s.FamiliaAcero1ID
                                                                                 ? "/" + famAcero[s.FamiliaAcero2ID.Value]
                                                                                 : string.Empty),
                                       Cedula = s.Cedula
                                   }).ToList();
            }
            else
            {
                return (from r in lstEstJta
                        let s = r.JuntaWorkstatus.OrdenTrabajoSpool.Spool
                        select new GrdEstimacionJunta
                        {
                            EstimacionID = r.EstimacionID,
                            Concepto = r.ConceptoEstimacion.NombreIngles,
                            Diametro = r.JuntaWorkstatus.JuntaSpool.Diametro,
                            TipoJunta = dicJta[r.JuntaWorkstatus.JuntaSpool.TipoJuntaID],
                            Etiqueta = r.JuntaWorkstatus.EtiquetaJunta,
                            EstimadoJuntaID = r.EstimacionJuntaID,
                            NumeroControl = r.JuntaWorkstatus.OrdenTrabajoSpool.NumeroControl,
                            NombreSpool = r.JuntaWorkstatus.OrdenTrabajoSpool.Spool.Nombre,
                            Material = famAcero[s.FamiliaAcero1ID] + (s.FamiliaAcero2ID != null
                                                                        && s.FamiliaAcero2ID != s.FamiliaAcero1ID
                                                                        ? "/" + famAcero[s.FamiliaAcero2ID.Value]
                                                                        : string.Empty),
                            Cedula = s.Cedula
                        }).OrderBy(x => x.NumeroControl).ThenBy(x => x.Etiqueta).ToList();
            }
        }

        /// <summary>
        /// Genera el GrdEstimacionSpool de diferentes tablas
        /// </summary>
        /// <param name="estimacionID">estimacion Id</param>
        /// <returns>Lista de GrdEstimacionSpool</returns>
        public List<GrdEstimacionSpool> ObtenerEstimacionSpoolPorEstimacionID(int estimacionID)
        {
            List<EstimacionSpool> lstEstSpool;

            using (SamContext ctx = new SamContext())
            {

                IQueryable<EstimacionSpool> iqEstimacionSpool = 
                    ctx.EstimacionSpool
                       .Where(x => x.EstimacionID == estimacionID);

                IQueryable<WorkstatusSpool> iqWorkstatusSpool = 
                    ctx.WorkstatusSpool
                       .Where(x => iqEstimacionSpool.Select(y => y.WorkstatusSpoolID)
                                                    .Contains(x.WorkstatusSpoolID));

                IQueryable<OrdenTrabajoSpool> iqOrdenTrabajoSpool = 
                    ctx.OrdenTrabajoSpool
                       .Where(x => iqWorkstatusSpool.Select(y => y.OrdenTrabajoSpoolID)
                                                    .Contains(x.OrdenTrabajoSpoolID));

                IQueryable<Spool> iqSpool = 
                    ctx.Spool
                        .Where(x => iqOrdenTrabajoSpool.Select(y => y.SpoolID)
                                                        .Contains(x.SpoolID));
               
                //Traer al contexto
                lstEstSpool = iqEstimacionSpool.ToList();
                ctx.ConceptoEstimacion.ToList();
                iqWorkstatusSpool.ToList();
                iqOrdenTrabajoSpool.ToList();
                iqSpool.ToList();

                Dictionary<int, string> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

                if (LanguageHelper.CustomCulture == LanguageHelper.ESPANOL)
                {
                    //regresa la informacion del grid
                    return (from r in lstEstSpool
                            let s = r.WorkstatusSpool.OrdenTrabajoSpool.Spool
                            select new GrdEstimacionSpool
                                       {
                                           EstimacionID = r.EstimacionID,
                                           Concepto = r.ConceptoEstimacion.Nombre,
                                           EstimacionSpoolID = r.EstimacionSpoolID,
                                           NumeroControl = r.WorkstatusSpool.OrdenTrabajoSpool.NumeroControl,
                                           Pdi = r.WorkstatusSpool.OrdenTrabajoSpool.Spool.Pdis,
                                           Spool = r.WorkstatusSpool.OrdenTrabajoSpool.Spool.Nombre,
                                           Material = famAcero[s.FamiliaAcero1ID] + (s.FamiliaAcero2ID != null
                                                                                     && s.FamiliaAcero2ID != s.FamiliaAcero1ID
                                                                                     ? "/" + famAcero[s.FamiliaAcero2ID.Value]
                                                                                     : string.Empty),
                                           Cedula = s.Cedula

                                       }).ToList();
                }
                else
                {
                    return (from r in lstEstSpool
                            let s = r.WorkstatusSpool.OrdenTrabajoSpool.Spool
                            select new GrdEstimacionSpool
                            {
                                EstimacionID = r.EstimacionID,
                                Concepto = r.ConceptoEstimacion.NombreIngles,
                                EstimacionSpoolID = r.EstimacionSpoolID,
                                NumeroControl = r.WorkstatusSpool.OrdenTrabajoSpool.NumeroControl,
                                Pdi = r.WorkstatusSpool.OrdenTrabajoSpool.Spool.Pdis,
                                Spool = r.WorkstatusSpool.OrdenTrabajoSpool.Spool.Nombre,
                                Material = famAcero[s.FamiliaAcero1ID] + (s.FamiliaAcero2ID != null
                                                                            && s.FamiliaAcero2ID != s.FamiliaAcero1ID
                                                                            ? "/" + famAcero[s.FamiliaAcero2ID.Value]
                                                                            : string.Empty),
                                Cedula = s.Cedula

                            }).ToList();
                }
            }
        }

        /// <summary>
        /// borra las estimacionesJunta con los Ids recibidos
        /// </summary>
        /// <param name="junta">arreglo de enteros recibe los Ids de la ErstimacionJunta</param>
        public void BorrarJuntasSeleccionados(int[] junta)
        {
            using (SamContext ctx = new SamContext())
            {
                List<EstimacionJunta> lst =
                    ctx.EstimacionJunta
                       .Where(Expressions.BuildOrExpression<EstimacionJunta, int>(x => x.EstimacionJuntaID, junta))
                       .ToList();

                for (int i = lst.Count - 1; i >= 0; i--)
                {
                    ctx.DeleteObject(lst[i]);
                }

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// borra las estimacionesSpools con los Ids recibidos
        /// </summary>
        /// <param name="Spool">arreglo de enteros recibe los Ids de la ErstimacionSpool</param>
        public void BorrarSpoolsSeleccionados(int[] Spool)
        {
            using (SamContext ctx = new SamContext())
            {

                List<EstimacionSpool> lst =
                   ctx.EstimacionSpool
                      .Where(Expressions.BuildOrExpression<EstimacionSpool, int>(x => x.EstimacionSpoolID, Spool))
                      .ToList();

                for (int i = lst.Count - 1; i >= 0; i--)
                {
                    ctx.DeleteObject(lst[i]);
                }

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Guarda una estimacion a la base de datos
        /// </summary>
        /// <param name="estimacion">objeto estimacion a guardar</param>
        public static void Guarda(Estimacion estimacion)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Estimacion.ApplyChanges(estimacion);

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public List<Estimacion> obtenerEstimaciones(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
               return ctx.Estimacion.Where(x=> x.ProyectoID == proyectoID).ToList();
            }
        }

        public Estimacion obtenerEstimacionPorNumerodeEstimacion(string numeroEstimacion, SamContext ctx)
        {
        
                return ctx.Estimacion.Where(
                                    x => x.NumeroEstimacion == numeroEstimacion).SingleOrDefault();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estimacionID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int estimacionID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoEstimacion(ctx, estimacionID);
            }
        }

        /// <summary>
        /// Versión compilada del query para permisos de estimación
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoEstimacion =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Estimacion
                            .Where(x => x.EstimacionID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
