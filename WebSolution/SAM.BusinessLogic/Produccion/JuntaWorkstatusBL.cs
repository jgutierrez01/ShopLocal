using System.Collections.Generic;
using System.Linq;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Ingenieria;
using System;


namespace SAM.BusinessLogic.Produccion
{
    public class JuntaWorkstatusBL
    {

         private static readonly  object _mutex = new object();
         private static JuntaWorkstatusBL _instance;

        /// <summary>
        /// constructor privado para implementar patron singleton
        /// </summary>
        private JuntaWorkstatusBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoBL
        /// </summary>
        public static JuntaWorkstatusBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaWorkstatusBL();
                    }
                }
                return _instance;
            }
        }

        public  void Eliminar(int juntaWorkstatusID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaWorkstatus juntaAEliminar = ctx.JuntaWorkstatus.Single(x => x.JuntaWorkstatusID == juntaWorkstatusID);
                JuntaSpool js = ctx.JuntaSpool.Single(x => x.JuntaSpoolID == juntaAEliminar.JuntaSpoolID);
                
                if(juntaAEliminar.JuntaArmadoID != null || juntaAEliminar.JuntaSoldaduraID != null || juntaAEliminar.JuntaInspeccionVisualID != null)
                {
                    throw new ExcepcionRelaciones(MensajesError.ExcepcionRelaciones);
                }

                if (RevisionHoldsBO.Instance.JuntaWorkstatusTieneHold(ctx,juntaWorkstatusID))
                {
                    throw new ExcepcionEnHold(MensajesError.Excepcion_JuntaEnSpoolHold);
                }

                if (!RechazosCortesUtil.EtiquetaTieneCorte(juntaAEliminar.EtiquetaJunta, js.Etiqueta))
                {
                    throw new ExcepcionCorte(MensajesError.ExcepcionEliminarJuntaSinCorte);
                }

                if (juntaAEliminar.JuntaWorkstatusAnteriorID != null)
                {
                    int juntaAnteriorID = juntaAEliminar.JuntaWorkstatusAnteriorID.Value;
                    JuntaWorkstatus juntaAnterior = ctx.JuntaWorkstatus.Single(x => x.JuntaWorkstatusID == juntaAnteriorID);
                    juntaAnterior.StartTracking();
                    juntaAnterior.JuntaFinal = true;
                    juntaAnterior.StopTracking();
                    ctx.JuntaWorkstatus.ApplyChanges(juntaAnterior);
                }
                
                ctx.DeleteObject(juntaAEliminar);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Aplica un corte a la junta 
        /// </summary>
        /// <param name="juntaWorkstatusID"></param>
        /// <param name="userId"></param>
        public void Cortar(int juntaWorkstatusID, Guid userId)
        {
            
            using (SamContext ctx = new SamContext())
            {
                JuntaWorkstatus juntaCortada = JuntaWorkstatusBO.Instance.Obtener(juntaWorkstatusID);
                JuntaSpool juntaSpool = JuntaSpoolBO.Instance.Obtener(juntaCortada.JuntaSpoolID);
                string nuevaEtiqueta = RechazosCortesUtil.ObtenNuevaEtiquetaDeCorte(juntaCortada.EtiquetaJunta, juntaSpool.Etiqueta);

                if (RevisionHoldsBO.Instance.JuntaWorkstatusTieneHold(ctx, juntaWorkstatusID))
                {
                    throw new ExcepcionEnHold(MensajesError.Excepcion_JuntaEnSpoolHold);
                }

                #region Modificar la junta cortada

                juntaCortada.StartTracking();
                juntaCortada.JuntaFinal = false;
                juntaCortada.StopTracking();

                #endregion

                #region modificar el workStatus del spool
                OrdenTrabajoSpool ordenTrabajoSpool =
                    ctx.OrdenTrabajoSpool.Single(x => x.OrdenTrabajoSpoolID == juntaCortada.OrdenTrabajoSpoolID);

                WorkstatusSpool workstatusSpool =
                    ctx.WorkstatusSpool.SingleOrDefault(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpool.OrdenTrabajoSpoolID);

                if (workstatusSpool != null)
                {
                    List<ReporteDimensionalDetalle> rdd = ctx.ReporteDimensionalDetalle.Where(x => x.WorkstatusSpoolID == workstatusSpool.WorkstatusSpoolID).ToList();
                    rdd.ForEach(x => ctx.DeleteObject(x));

                    workstatusSpool.StartTracking();
                    workstatusSpool.UltimoProcesoID = null;
                    workstatusSpool.TieneLiberacionDimensional = false;
                    //workstatusSpool.TieneRequisicionPintura = false;
                    //workstatusSpool.TienePintura = false;
                    //workstatusSpool.LiberadoPintura = false;
                    workstatusSpool.Preparado = false;
                    workstatusSpool.Embarcado = false;
                    workstatusSpool.Certificado = false;
                    workstatusSpool.StopTracking();
                }
                #endregion  

                #region Crear la junta nueva
                JuntaWorkstatus juntaNueva = new JuntaWorkstatus
                {
                    JuntaSpoolID = juntaCortada.JuntaSpoolID,
                    EtiquetaJunta = nuevaEtiqueta,
                    ArmadoAprobado = false,
                    SoldaduraAprobada = false,
                    InspeccionVisualAprobada = false,
                    JuntaArmadoID = null,
                    JuntaSoldaduraID = null,
                    JuntaInspeccionVisualID = null,
                    VersionJunta = juntaCortada.VersionJunta + 1,
                    JuntaFinal = true,
                    UltimoProcesoID = null,
                    JuntaPadre = juntaCortada,
                    OrdenTrabajoSpool = ordenTrabajoSpool,
                    FechaModificacion = DateTime.Now,
                    UsuarioModifica = userId
                };
                #endregion

               
                //junta nueva apunta hacia JuntaCortada que a su vez contiene ordenTrabajoSpool
                ctx.JuntaWorkstatus.ApplyChanges(juntaNueva);

                ctx.SaveChanges();
            }
           
        }

        /// <summary>
        /// Obtiene una lista para el grid de juntaworkstatus recibiendo como parametro el prooyectoID y la ordenTrabajoSpoolID
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <returns></returns>
        public List<GrdJuntaWorkstatus> ObtenerPorOrdenTrabajoSpoolID(int proyectoID, int ordenTrabajoSpoolID)
        {
            List<JuntaWorkstatus> juntas;
            using (SamContext ctx = new SamContext())
            {
                if (ordenTrabajoSpoolID > 0)
                {
                    IEnumerable<OrdenTrabajoSpool> ordenTrabajoSpool =
                        ctx.OrdenTrabajoSpool.Where(x => ordenTrabajoSpoolID == x.OrdenTrabajoSpoolID);

                    juntas =
                        ctx.JuntaWorkstatus.Where(
                            x => ordenTrabajoSpool.Any(y => y.OrdenTrabajoSpoolID == x.OrdenTrabajoSpoolID)).ToList();
                }
                else
                {
                    juntas =
                    ctx.JuntaWorkstatus.Where(
                        x => x.OrdenTrabajoSpool.OrdenTrabajo.ProyectoID == proyectoID).ToList();
                }

                IEnumerable<int> juntasSpoolIds = juntas.Select(x => x.JuntaSpoolID);

                IEnumerable<int> spoolsIds =
                    ctx.JuntaSpool.Where(x => juntasSpoolIds.Contains(x.JuntaSpoolID)).ToList().Select(x => x.SpoolID);

                IEnumerable<int> spoolHoldIds =
                    ctx.SpoolHold.Where(x => spoolsIds.Contains(x.SpoolID) && (x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado)).ToList().Select(x => x.SpoolID);

                Dictionary<int, string> familiasAceros =
                    CacheCatalogos.Instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

                Dictionary<int, string> procesos =
                    CacheCatalogos.Instance.ObtenerUltimoProceso().ToDictionary(x => x.ID, y => y.Nombre);

                Dictionary<int, string> tipoJuntas =
                    CacheCatalogos.Instance.ObtenerTiposJunta().ToDictionary(x => x.ID, y => y.Nombre);

                return (from junta in juntas
                        let js = junta.JuntaSpool
                        where junta.JuntaFinal
                        select new GrdJuntaWorkstatus
                        {
                            JuntaWorkstatusID = junta.JuntaWorkstatusID,
                            Etiqueta = junta.EtiquetaJunta,
                            Cedula = js.Cedula,
                            EtiquetaMaterial1 = js.EtiquetaMaterial1,
                            EtiquetaMaterial2 = js.EtiquetaMaterial2,
                            TipoJunta = tipoJuntas[js.TipoJuntaID],
                            Material1 = familiasAceros[js.FamiliaAceroMaterial1ID],
                            Material2 =
                                js.FamiliaAceroMaterial2ID.HasValue
                                    ? familiasAceros[js.FamiliaAceroMaterial2ID.Value]
                                    : string.Empty,
                            Diametro = js.Diametro.ToString(),
                            UltimoProceso =
                                junta.UltimoProcesoID.HasValue ? procesos[junta.UltimoProcesoID.Value] : string.Empty,
                            JuntaFinal = junta.JuntaFinal,
                            SpoolHold = spoolHoldIds.Contains(js.SpoolID),
                            TieneCorte = RechazosCortesUtil.EtiquetaTieneCorte(junta.EtiquetaJunta, js.Etiqueta)
                        }).ToList();

            }
        }
    }
}
