using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Excepciones;
using System.Transactions;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessObjects.Workstatus
{
    public class JuntaInspeccionVisualBO
    {
        private static readonly object _mutex = new object();
        private static JuntaInspeccionVisualBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private JuntaInspeccionVisualBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase JuntaInspeccionVisualBO
        /// </summary>
        /// <returns></returns>
        public static JuntaInspeccionVisualBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaInspeccionVisualBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Borra JuntaInspeccionVisual y sus relaciones con JuntaInspeccionVisualDefecto
        /// </summary>
        /// <param name="juntaInspeccionVisualID">JuntaInspeccionVisualID</param>
        /// <param name="userId"></param>
        public void Borra(int juntaInspeccionVisualID, Guid userId)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaInspeccionVisual jiv = ctx.JuntaInspeccionVisual.Where(x => x.JuntaInspeccionVisualID == juntaInspeccionVisualID).Single();
                JuntaWorkstatus jw = ctx.JuntaWorkstatus.Where(x => x.JuntaInspeccionVisualID == juntaInspeccionVisualID).SingleOrDefault();

                if (!ctx.JuntaRequisicion.Where(x => x.JuntaWorkstatusID == jiv.JuntaWorkstatusID).Any())
                {
                    WorkstatusSpool ws = ctx.WorkstatusSpool.Where(x => x.OrdenTrabajoSpoolID == jw.OrdenTrabajoSpoolID).FirstOrDefault();

                    if (ws != null)
                    {
                        if (ws.TieneRequisicionPintura || ws.TienePintura)
                        {
                            throw new ExcepcionRelaciones(MensajesError.Excepcion_TieneRequiPintura);
                        }
                    }
                   
                    List<JuntaInspeccionVisualDefecto> juntaInspeccionVisualDefecto = ctx.JuntaInspeccionVisualDefecto.Where(x => x.JuntaInspeccionVisualID == juntaInspeccionVisualID).ToList();
                    juntaInspeccionVisualDefecto.ForEach(ctx.DeleteObject);

                    if (jw != null)
                    {
                        jw.StartTracking();
                        jw.InspeccionVisualAprobada = false;
                        jw.JuntaInspeccionVisualID = null;
                        jw.UsuarioModifica = userId;
                        jw.FechaModificacion = DateTime.Now;
                        jw.StopTracking();
                    }

                    ctx.DeleteObject(jiv);
                    ctx.SaveChanges();
                }                
                else
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_IVRequisicion);
                }
            }
        }

        public List<ListaFechaNumeroControl> ObtenerFechasSoldaduraEdicionVisual(int inspeccionVisualID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<ListaFechaNumeroControl> listado = (from soldaduras in ctx.JuntaSoldadura
                                                         join juntaWs in ctx.JuntaWorkstatus on soldaduras.JuntaWorkstatusID equals juntaWs.JuntaWorkstatusID
                                                         join odts in ctx.OrdenTrabajoSpool on juntaWs.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                         join juntaInspeccion in ctx.JuntaInspeccionVisual on juntaWs.JuntaWorkstatusID equals juntaInspeccion.JuntaWorkstatusID
                                                         join inspecciones in ctx.InspeccionVisual on juntaInspeccion.InspeccionVisualID equals inspecciones.InspeccionVisualID
                                                         where inspecciones.InspeccionVisualID == inspeccionVisualID
                                                         && juntaWs.JuntaFinal == true
                                                         select new ListaFechaNumeroControl
                                                         {
                                                             FechaReporte = soldaduras.FechaReporte,
                                                             FechaProceso = soldaduras.FechaSoldadura,
                                                             NumeroControl = odts.NumeroControl
                                                         }).ToList();
                return listado;
            }
        }
    }
}
