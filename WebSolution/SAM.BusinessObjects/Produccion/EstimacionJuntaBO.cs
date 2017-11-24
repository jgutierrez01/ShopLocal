using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;
using System.Data.Objects;
using SAM.BusinessObjects.Ingenieria;

namespace SAM.BusinessObjects.Produccion
{
    public class EstimacionJuntaBO
    {
        private static readonly object _mutex = new object();
        private static EstimacionJuntaBO _instance;
        /// <summary>
        /// Obtiene la instancia de la clase EstimacionBO
        /// </summary>
        public static EstimacionJuntaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EstimacionJuntaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Genera el GrdEstimacionJunta de diferentes tablas
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns>Lista de GrdEstimacionJuntaCompleta</returns>
        public List<GrdEstimacionJuntaCompleta> ObtenerEstimacionJuntaPorProyectoID(int proyectoID)
        {
            List<EstimacionJuntaCompuesto> lst;
         
            using (SamContext ctx = new SamContext())
            {
                lst = ctx.ObtenerEstimacionJuntas(proyectoID).ToList();
            }

            Dictionary<int, string> lstJuntaSpools = JuntaSpoolBO.Instance.ObtenerPorProyecto(proyectoID).ToDictionary(x => x.JuntaSpoolID, y => y.Cedula);
            Dictionary<int, string> tipoJta = CacheCatalogos.Instance.ObtenerTiposJunta().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

            var grp = from est in lst
                      group est by new
                      {
                          est.JuntaWorkstatusID,
                          est.JuntaSpoolID,
                          est.NumeroControl,
                          est.NombreSpool,
                          est.OrdenTrabajoSpoolID,
                          est.Diametro,
                          est.EtiquetaJunta,
                          est.FamiliaAceroMaterial1ID,
                          est.FamiliaAceroMaterial2ID,
                          est.TipoJuntaID,
                          est.ArmadoAprobado,
                          est.SoldaduraAprobada,
                          est.InspeccionVisualAprobada,
                          est.ConceptoEstimacionID,
                          est.EstimacionID,
                          est.NumeroEstimacion,
                          est.AprobadoReporteDimensional
                      }
                          into grupo
                          select new
                          {
                              grupo.Key.JuntaWorkstatusID,
                              RtAprobado = grupo.Where(x => x.TipoPruebaPndID.HasValue && x.TipoPruebaPndID.Value == (int)TipoPruebaEnum.ReporteRT && x.AprobadoPnd.Value).Any(),
                              PtAprobado = grupo.Where(x => x.TipoPruebaPndID.HasValue && x.TipoPruebaPndID.Value == (int)TipoPruebaEnum.ReportePT && x.AprobadoPnd.Value).Any(),
                              PwhtAprobado = grupo.Where(x => x.TipoPruebaTtID.HasValue && x.TipoPruebaTtID.Value == (int)TipoPruebaEnum.Pwht && x.AprobadoTt.Value).Any(),
                              DurezasAprobado = grupo.Where(x => x.TipoPruebaTtID.HasValue && x.TipoPruebaTtID.Value == (int)TipoPruebaEnum.Durezas && x.AprobadoTt.Value).Any(),
                              RTPostTTAprobado = grupo.Where(x => x.TipoPruebaPndID.HasValue && x.TipoPruebaPndID.Value == (int)TipoPruebaEnum.RTPostTT && x.AprobadoPnd.Value).Any(),
                              PTPostTTAprobado = grupo.Where(x => x.TipoPruebaPndID.HasValue && x.TipoPruebaPndID.Value == (int)TipoPruebaEnum.PTPostTT && x.AprobadoPnd.Value).Any(),
                              PreheatAprobado = grupo.Where(x => x.TipoPruebaTtID.HasValue && x.TipoPruebaTtID.Value == (int)TipoPruebaEnum.Preheat && x.AprobadoTt.Value).Any(),
                              UTAprobado = grupo.Where(x => x.TipoPruebaPndID.HasValue && x.TipoPruebaPndID.Value == (int)TipoPruebaEnum.ReporteUT && x.AprobadoPnd.Value).Any(),
                              Armada = grupo.Key.ArmadoAprobado,
                              grupo.Key.NumeroControl,
                              grupo.Key.NombreSpool,
                              grupo.Key.OrdenTrabajoSpoolID,
                              grupo.Key.SoldaduraAprobada,
                              grupo.Key.TipoJuntaID,
                              grupo.Key.Diametro,
                              grupo.Key.JuntaSpoolID,
                              grupo.Key.InspeccionVisualAprobada,
                              grupo.Key.FamiliaAceroMaterial1ID,
                              grupo.Key.FamiliaAceroMaterial2ID,
                              grupo.Key.EtiquetaJunta,
                              grupo.Key.ConceptoEstimacionID,
                              grupo.Key.EstimacionID,
                              grupo.Key.NumeroEstimacion,
                              grupo.Key.AprobadoReporteDimensional
                          };

            return (from est in grp
                    select new GrdEstimacionJuntaCompleta
                    {
                        JuntaWorkStatusID = est.JuntaWorkstatusID,
                        NumeroControl = est.NumeroControl,
                        NombreSpool = est.NombreSpool,
                        Etiqueta = est.EtiquetaJunta,
                        TipoDeJunta = tipoJta[est.TipoJuntaID],
                        Diametro = est.Diametro,
                        Material = famAcero[est.FamiliaAceroMaterial1ID] + (    est.FamiliaAceroMaterial2ID != null
                                                                                && est.FamiliaAceroMaterial2ID != est.FamiliaAceroMaterial1ID
                                                                                ? 
                                                                                "/" + famAcero[est.FamiliaAceroMaterial2ID.Value]
                                                                                : 
                                                                                string.Empty),
                        Cedula = lstJuntaSpools[est.JuntaSpoolID],
                        Armada = est.Armada,
                        Soldada = est.SoldaduraAprobada,
                        InspeccionVisual = est.InspeccionVisualAprobada,
                        RtAprobado = est.RtAprobado,
                        InspeccionDimensional = est.AprobadoReporteDimensional ?? false,
                        PtAprobado = est.PtAprobado,
                        DurezasAprobado = est.DurezasAprobado,
                        RtPostTtAprobado = est.RTPostTTAprobado,
                        PtPostTtAprobado = est.PTPostTTAprobado,
                        PreHeatAprobado = est.PreheatAprobado,
                        UtAprobado = est.UTAprobado,
                        PwhtAprobado = est.PwhtAprobado,
                        ConceptoEstimacionID = est.ConceptoEstimacionID,
                        EstimacionId = est.EstimacionID,
                        NumeroEstimacion = est.NumeroEstimacion
                    })
                    .AsParallel()
                    .OrderBy(x => x.NumeroControl)
                    .ToList();


        }

        /// <summary>
        /// Guarda una estimacionJunta a la base de datos
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
                            EstimacionJunta estimacionJunta = new EstimacionJunta();
                            estimacionJunta.StartTracking();
                            estimacionJunta.EstimacionID = estimacion.EstimacionID;
                            estimacionJunta.ConceptoEstimacionID = ConceptoEstimacion;
                            estimacionJunta.JuntaWorkstatusID = id;
                            ctx.EstimacionJunta.ApplyChanges(estimacionJunta);
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
        /// verifica si los ids de las  Juntaworkstatus no existen en los conceptos seleccionados
        /// si es falso manda un error
        /// </summary>
        /// <returns>verdadero o falso</returns>
        public void JuntaWorkStatusSinConcepto(List<string> conceptos)
        {
            if (conceptos.Count > 0)
            {
                throw new ExcepcionEstimacionJunta(conceptos);
            }
        }

        /// <summary>
        /// Verifica que la estimacion a agregar no exista en la base de datos,
        /// si es falso manda un mensaje de error
        /// </summary>
        /// <param name="numeroEstimacion">numero de estimacion a evaluar</param>
        /// <param name="proyectoID">Proyecto a evaluar</param>
        /// <returns>verdadero o falso</returns>
        public void ExisteEstimacion(string numeroEstimacion, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (!ValidacionesEstimacionJunta.ExisteEstimacion(ctx, numeroEstimacion, proyectoID))
                {
                    throw new ExcepcionEstimacionJunta(new List<string> { MensajesError.Excepcion_ExisteEstimacion });
                }
            }
        }

        /// <summary>
        /// verifica que al menos un checkbox este seleccionado ,
        /// si es falso manda un  mensaje de error
        /// </summary>
        /// <param name="chk"></param>
        /// <returns></returns>
        public void SinchkBoxSeleccionados(bool chk)
        {
            if (!chk)
            {
                throw new ExcepcionEstimacionJunta(new List<string> { MensajesError.Excepcion_JuntaEstimacionConceptos });
            }
        }

        public EstimacionJunta TraerEstimacionJunta(int id, int estimacion)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.EstimacionJunta
                    .Where(x => x.JuntaWorkstatusID == id && x.ConceptoEstimacionID == estimacion)
                    .SingleOrDefault();
            }
        }

    }
}