using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Ingenieria;

namespace SAM.BusinessObjects.Produccion
{
    public class EstimacionSpoolBO
    {
        private static readonly object _mutex = new object();
        private static EstimacionSpoolBO _instance;
        /// <summary>
        /// Obtiene la instancia de la clase EstimacionBO
        /// </summary>
        public static EstimacionSpoolBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EstimacionSpoolBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera el grid de EstimacionSpool de acuerdo al proyecto
        /// </summary>
        /// <param name="proyectoID">int: ProyectoID</param>
        /// <returns></returns>
        public List<GrdEstimacionSpoolCompleta> ObtenerEstimacionSpoolPorProyectoID(int proyectoID)
        {
            List<EstimacionSpoolCompuesto> lst;
            using (SamContext ctx = new SamContext())
            {
                lst = ctx.ObtenerEstimacionSpools(proyectoID).ToList();
            }

            Dictionary<string, Spool> lstSpools = SpoolBO.Instance.ObtenerPorProyecto(proyectoID).ToDictionary(x => x.Nombre, y => y);
            Dictionary<int, string> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

            //regresa los datos del grid
            return (from spool in lst
                    let s = lstSpools[spool.Nombre]
                    select new GrdEstimacionSpoolCompleta
                                {
                                    EstimacionId = spool.EstimacionID,
                                    WorkStatusSpoolID = spool.WorkstatusSpoolID,
                                    Embarcado = spool.Embarcado,
                                    InspecciónDimensional = spool.TieneLiberacionDimensional,
                                    NumeroControl = spool.NumeroControl,
                                    PDI = spool.Pdis,
                                    Material = famAcero[s.FamiliaAcero1ID] + (s.FamiliaAcero2ID != null
                                                                                && s.FamiliaAcero2ID != s.FamiliaAcero1ID
                                                                                ?
                                                                                "/" + famAcero[s.FamiliaAcero2ID.Value]
                                                                                :
                                                                                string.Empty),
                                    Cedula = s.Cedula,
                                    Pintura = spool.LiberadoPintura,
                                    Spool = spool.Nombre,
                                    ConceptoEstimacionID = spool.ConceptoEstimacionID,
                                    EstimacionSpoolID = spool.EstimacionSpoolID,
                                    NumeroEstimacion = spool.NumeroEstimacion,
                                    OrdenTrabajoSpoolID = spool.OrdenTrabajoSpoolID
                                }).ToList();

            //using (SamContext ctx = new SamContext())
            //{
            //    //Datos Basicos
            //    IQueryable<Estimacion> iqEstimacion =
            //        ctx.Estimacion
            //            .Where(x => x.ProyectoID == proyectoID);

            //    IQueryable<EstimacionSpool> iqEstimacionSpool =
            //        ctx.EstimacionSpool
            //            .Where(x => iqEstimacion.Select(y => y.EstimacionID)
            //                            .Contains(x.EstimacionID));

            //    IQueryable<ConceptoEstimacion> iqConceptoEst =
            //        ctx.ConceptoEstimacion
            //            .Where(x => iqEstimacionSpool.Select(y => y.ConceptoEstimacionID)
            //                            .Contains(x.ConceptoEstimacionID));

            //    IQueryable<WorkstatusSpool> iqWorkstatusSpool =
            //        ctx.WorkstatusSpool
            //            .Where(x => iqEstimacionSpool.Select(y => y.WorkstatusSpoolID)
            //                            .Contains(x.WorkstatusSpoolID));

            //    IQueryable<OrdenTrabajoSpool> iqOrdenTrabajoSpool =
            //        ctx.OrdenTrabajoSpool
            //            .Where(x => iqWorkstatusSpool.Select(y => y.OrdenTrabajoSpoolID)
            //                            .Contains(x.OrdenTrabajoSpoolID));

            //    IQueryable<Spool> iqSpool =
            //        ctx.Spool
            //            .Where(x => iqOrdenTrabajoSpool.Select(y => y.SpoolID)
            //                            .Contains(x.SpoolID));

            //    //Traigo los datos basicos para desplegar en el grid
            //    lstEstJta = iqEstimacionSpool.ToList();
            //    iqWorkstatusSpool.ToList();
            //    iqEstimacion.ToList();
            //    iqSpool.ToList();
            //    iqOrdenTrabajoSpool.ToList();
            //    iqConceptoEst.ToList();


            //    TipoPruebaEnum prueba = new TipoPruebaEnum();

            //    //regresa los datos del grid
            //    return (from junta in lstEstJta
            //            select new GrdEstimacionSpoolCompleta
            //                       {
            //                           EstimacionId = junta.EstimacionID,//
            //                           WorkStatusSpoolID = junta.WorkstatusSpoolID,//
            //                           Embarcado = junta.WorkstatusSpool.Embarcado,//
            //                           InspecciónDimensional = junta.WorkstatusSpool.TieneLiberacionDimensional,//
            //                           NumeroControl = junta.WorkstatusSpool.OrdenTrabajoSpool.NumeroControl,//
            //                           PDI = junta.WorkstatusSpool.OrdenTrabajoSpool.Spool.Pdis,//
            //                           Pintura = junta.WorkstatusSpool.LiberadoPintura,//
            //                           Spool = junta.WorkstatusSpool.OrdenTrabajoSpool.Spool.Nombre,//
            //                           ConceptoEstimacionID = junta.ConceptoEstimacionID//
            //                       }).ToList();
            //}
        }

        /// <summary>
        /// Guarda una EstimacionSpool a la base de datos
        /// </summary>
        public static void Guarda(int[] ids, string numeroEstimacion, List<int> estimaciones)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Estimacion estimacion =
                              EstimacionBO.Instance.obtenerEstimacionPorNumerodeEstimacion(numeroEstimacion, ctx);

                    foreach (var id in ids)
                    {
                        foreach (var ConceptoEstimacion in estimaciones)
                        {
                            EstimacionSpool estimacionSpool = new EstimacionSpool();
                            estimacionSpool.StartTracking();
                            estimacionSpool.EstimacionID = estimacion.EstimacionID;
                            estimacionSpool.ConceptoEstimacionID = ConceptoEstimacion;
                            estimacionSpool.WorkstatusSpoolID = id;
                            ctx.EstimacionSpool.ApplyChanges(estimacionSpool);
                            ctx.SaveChanges();
                        }
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// verifica si los ids de los workstatusSpools no existen en los conceptos seleccionados
        /// si es falso manda un error
        /// </summary>
        /// <returns>verdadero o falso</returns>
        public void WorkStatusSpoolSinConcepto(List<string> conceptos)
        {
            if (conceptos.Count > 0)
            {
                throw new ExcepcionEstimacionSpool(conceptos);
            }
        }

        /// <summary>
        /// Verifica que la estimacion a agregar no exista en la base de datos,
        /// si es falso manda un mensaje de error
        /// </summary>
        /// <param name="ctx">contexto</param>
        /// <param name="numeroEstimacion">numero de estimacion a evaluar</param>
        /// <param name="proyectoID">proyecto al que pertenece</param>
        /// <returns>verdadero o falso</returns>
        public static bool ExisteEstimacion(SamContext ctx, string numeroEstimacion,int proyectoID)
        {
            if (ValidacionesEstimacionSpool.ExisteEstimacion(ctx, numeroEstimacion, proyectoID))
            {
                return true;
            }
            throw new ExcepcionEstimacionSpool(new List<string> { MensajesError.Excepcion_ExisteEstimacion });
            
        }

        /// <summary>
        /// verifica que al menos un checkbox este seleccionado ,
        /// si es falso manda un  mensaje de error
        /// </summary>
        /// <param name="chkEstimaciones"></param>
        /// <returns></returns>
        public void chkBoxSeleccionados(bool chkEstimaciones)
        {
            if (!chkEstimaciones)
            {
                throw new ExcepcionEstimacionSpool(new List<string> { MensajesError.Excepcion_EstimacionSpoolConceptos });
            }
        }

        public EstimacionSpool TraerEstimacionSpool(int id, int estimacion)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.EstimacionSpool
                    .Where(x => x.WorkstatusSpoolID == id && x.ConceptoEstimacionID == estimacion)
                    .SingleOrDefault();
            }
        }
    }
}
