using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Validations;
using SAM.Entities;
using SAM.Entities.Grid;

namespace SAM.BusinessObjects.Workstatus
{
    public class LiberacionVisualPatioBO
    {
        private static readonly object _mutex = new object();
        private static LiberacionVisualPatioBO _instance;

        /// <summary>
        /// Constructor privado
        /// </summary>
        private LiberacionVisualPatioBO()
        {

        }

        /// <summary>
        /// Obtiene la instancia de la clase LiberacionVisualPatioBO
        /// </summary>
        public static LiberacionVisualPatioBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new LiberacionVisualPatioBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el listado de estimaciones de acuerdo a los filtros
        /// </summary>
        /// <param name="proyectoID">Proyecto ID de la tabla Estimacion</param>
        /// <param name="fechaDesde">Fecha Inicial de Búsqueda de la tabla Estimacion</param>
        /// <param name="fechaHasta">Fecha Final de Búsqueda de la tabla Estimacion</param>
        /// <returns></returns>
        public List<GrdLiberacionVisualPatio> ObtenerConFiltros(int proyectoID, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<GrdLiberacionVisualPatio> inspVisual =
                    (from ivp in ctx.InspeccionVisualPatio
                     let jw = ivp.JuntaWorkstatus
                     let ots = jw.OrdenTrabajoSpool
                     let sp = ots.Spool
                     let js = jw.JuntaSpool
                     let tj = js.TipoJunta
                     let sph = sp.SpoolHold
                     where sp.ProyectoID == proyectoID
                     select new GrdLiberacionVisualPatio
                     {
                         InspeccionVisualPatioID = ivp.InspeccionVisualPatioID,
                         NumeroDeControl = ots.NumeroControl,
                         Spool = sp.Nombre,
                         EtiquetaJunta = jw.EtiquetaJunta,
                         TipoJunta = tj.Nombre,
                         Resultado = ivp.Aprobado == true ? "Aprobado" : "No Aprobado",
                         FechaInspeccion = ivp.FechaInspeccion,
                         Observaciones = ivp.Observaciones,
                         Hold = sph != null && (sph.TieneHoldCalidad || sph.TieneHoldIngenieria || sph.Confinado)
                     }).AsQueryable();

                if (fechaDesde.HasValue)
                {
                    inspVisual = inspVisual.Where(iv => iv.FechaInspeccion >= fechaDesde);
                }

                if (fechaHasta.HasValue)
                {
                    inspVisual = inspVisual.Where(iv => iv.FechaInspeccion <= fechaDesde);
                }

                return inspVisual.ToList();
            }
        }

        public InspeccionVisualPatio Obtener(int inspeccionVisualPatioID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.InspeccionVisualPatio.SingleOrDefault(x => x.InspeccionVisualPatioID == inspeccionVisualPatioID);
            }
        }

        public static void Guarda(InspeccionVisual inspeccionVisual)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.InspeccionVisual.ApplyChanges(inspeccionVisual);

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// Verifica que la estimacion a agregar no exista en la base de datos,
        /// si es falso manda un mensaje de error
        /// </summary>
        /// <param name="ctx">contexto</param>
        /// <param name="numeroReporte">numero de estimacion a evaluar</param>
        /// <returns>verdadero o falso</returns>
        public static bool ExisteInspeccionVisual(SamContext ctx, string numeroReporte)
        {
            if (ValidacionesInspeccionVisualPatio.ExisteInspeccionVisual(ctx, numeroReporte))
            {
                return true;
            }
            else
            {
                throw new ExcepcionInspeccionVisualPatio(new List<string>() { MensajesError.Excepcion_ExisteInspeccionVisual });
            }
        }

        public static InspeccionVisual ObtenerPorNumeroReporte(int proyectoID, string numeroReporte)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.InspeccionVisual.SingleOrDefault(x => x.ProyectoID == proyectoID && x.NumeroReporte == numeroReporte);
            }
        }

        public static int ObtenerPorNumeroReporte(SamContext ctx, string numeroReporte)
        {
            return ctx.InspeccionVisual.Where(x => x.NumeroReporte == numeroReporte).FirstOrDefault().InspeccionVisualID;
        }

        public static void GenerarJuntaInspeccionVisual(int inspeccionVisualID, int[] ids, string LiberacionVisualIds, Guid UserUID)
        {
            using (SamContext ctx = new SamContext())
            {
                InspeccionVisual inspVisual =
                    ctx.InspeccionVisual.SingleOrDefault(x => x.InspeccionVisualID == inspeccionVisualID);
                inspVisual.StartTracking();
                InspeccionVisualPatio Inspect;

                foreach (var id in ids)
                {
                    if (id > -1)
                    {
                        Inspect = ctx.InspeccionVisualPatio.SingleOrDefault(x => x.InspeccionVisualPatioID == id);
                      
                        JuntaWorkstatus juntaWks = ctx.JuntaWorkstatus.Where(x => x.JuntaWorkstatusID == Inspect.JuntaWorkstatusID).Single();
                           
                        JuntaInspeccionVisual juntaInspeccionVisual = null;
                        IQueryable<JuntaInspeccionVisualDefecto> defectosExistentes = null;

                        juntaInspeccionVisual = ctx.JuntaInspeccionVisual.Where(x => x.JuntaWorkstatusID == juntaWks.JuntaWorkstatusID).OrderByDescending(x => x.JuntaWorkstatusID).FirstOrDefault();

                        if (juntaInspeccionVisual == null)
                        {
                            juntaInspeccionVisual= new  JuntaInspeccionVisual();
                        }

                         if (juntaInspeccionVisual != null )
                        {                            
                            if(juntaInspeccionVisual.Aprobado)
                            {      
                                if (juntaInspeccionVisual.InspeccionVisualID == inspVisual.InspeccionVisualID)
                                {
                                    throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicado));
                                }
                                else
                                {
                                    string numeroReporte = ctx.InspeccionVisual.Where(x => x.InspeccionVisualID == juntaInspeccionVisual.InspeccionVisualID).Select(x => x.NumeroReporte).FirstOrDefault();
                                    throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_ReporteVisualConDetalleDuplicadoOtroReporte, numeroReporte ));
                                }
                            }
                            else
                            {                               
                                defectosExistentes = ctx.JuntaInspeccionVisualDefecto.Where(x => x.JuntaInspeccionVisualID == juntaInspeccionVisual.JuntaInspeccionVisualID);                       
                            }
                        }         

                        juntaInspeccionVisual.InspeccionVisualID = inspeccionVisualID;
                        juntaInspeccionVisual.JuntaWorkstatusID = Inspect.JuntaWorkstatusID;
                        juntaInspeccionVisual.FechaInspeccion = Inspect.FechaInspeccion;
                        juntaInspeccionVisual.Aprobado = Inspect.Aprobado;
                        juntaInspeccionVisual.UsuarioModifica = UserUID;
                        juntaInspeccionVisual.FechaModificacion = DateTime.Now;
                        ctx.JuntaInspeccionVisual.ApplyChanges(juntaInspeccionVisual);

                        //Si el reporte es aprobado se actualiza el registro de JuntaWorkstatus
                        if (Inspect.Aprobado)
                        {
                            juntaWks.StartTracking();
                            juntaWks.InspeccionVisualAprobada = true;
                            juntaWks.InspeccionVisual = juntaInspeccionVisual;
                            juntaWks.UsuarioModifica = UserUID;
                            juntaWks.FechaModificacion = DateTime.Now;
                            juntaWks.StopTracking();

                            ctx.JuntaWorkstatus.ApplyChanges(juntaWks);
                        }
                        else //Si el reporte es rechazado se generan los registros de defectos
                        {
                            List<int> defectos =
                                ctx.InspeccionVisualPatioDefecto.Where(x => x.InspeccionVisualPatioID == id).Select(x => x.DefectoID).ToList();

                            foreach (int defectoID in defectos)
                            {
                                JuntaInspeccionVisualDefecto juntaDefecto = null;
                                if (defectosExistentes != null)
                                {
                                    juntaDefecto = defectosExistentes.Where(x => x.DefectoID == defectoID).OrderByDescending(X => X.JuntaInspeccionVisualDefectoID).FirstOrDefault();
                                }
                                 
                                if(juntaDefecto == null)                                                   
                                {
                                    juntaDefecto  = new JuntaInspeccionVisualDefecto();
                                }
                               
                                juntaDefecto.DefectoID = defectoID;
                                juntaDefecto.UsuarioModifica = UserUID;
                                juntaDefecto.FechaModificacion = DateTime.Now;                                
                              
                                juntaDefecto.JuntaInspeccionVisualID = juntaInspeccionVisual.InspeccionVisualID;

                                if (juntaInspeccionVisual.JuntaInspeccionVisualDefecto.Contains(juntaDefecto))
                                {
                                    ctx.JuntaInspeccionVisualDefecto.ApplyChanges(juntaDefecto);
                                }
                                else
                                {
                                    juntaInspeccionVisual.JuntaInspeccionVisualDefecto.Add(juntaDefecto);
                                }                               
                            }
                            if (defectosExistentes != null)
                            {
                                foreach (JuntaInspeccionVisualDefecto jd in defectosExistentes)
                                {
                                    if (!defectos.Contains(jd.DefectoID))
                                    {
                                        ctx.DeleteObject(jd);
                                    }
                                }
                            }  
                        }
                        inspVisual.JuntaInspeccionVisual.Add(juntaInspeccionVisual);
                        BorrarInspeccionVisualPatioDefecto(ctx, Inspect.InspeccionVisualPatioID);
                        BorrarInspeccionVisualPatio(ctx, Inspect);
                    }
                }
                inspVisual.StopTracking();
                
                ctx.InspeccionVisual.ApplyChanges(inspVisual);
                ctx.SaveChanges();

                ctx.CalculaHojaParaReporte((int)TipoReporte.InspeccionVisual, inspVisual.ProyectoID, inspVisual.NumeroReporte, LiberacionVisualIds);
            }
        }

        public static void BorrarInspeccionVisualPatioDefecto(SamContext ctx, int inspeccionVisualPatioID)
        {
            List<InspeccionVisualPatioDefecto> defectos =
                            ctx.InspeccionVisualPatioDefecto.Where(x => x.InspeccionVisualPatioID == inspeccionVisualPatioID).ToList();
            
            foreach(InspeccionVisualPatioDefecto defecto in defectos)
            {
                ctx.DeleteObject(defecto);
            }

            ctx.SaveChanges();
        }

        public static void BorrarInspeccionVisualPatio(SamContext ctx, InspeccionVisualPatio Inspect)
        {
            
            ctx.DeleteObject(Inspect);

            ctx.SaveChanges();
        }
    }
}

