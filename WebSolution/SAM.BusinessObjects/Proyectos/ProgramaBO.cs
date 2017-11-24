using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Proyectos
{
    public class ProgramaBO
    {
        private static readonly object _mutex = new object();
        private static ProgramaBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProgramaBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProgramaBO
        /// </summary>
        /// <returns></returns>
        public static ProgramaBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProgramaBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programa"></param>
        /// <param name="periodos"></param>
        /// <param name="usuarioModifica"></param>
        /// <param name="proyectoID"></param>
        /// <param name="fechaInicio"></param>
        public void Guarda(ProyectoPrograma programa, List<PeriodoPrograma> periodos, Guid usuarioModifica, int proyectoID, DateTime fechaInicio)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Proyecto proyecto = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).Single();

                    proyecto.StartTracking();
                    
                    proyecto.FechaInicio = fechaInicio;
                    proyecto.FechaModificacion = DateTime.Now;
                    proyecto.UsuarioModifica = usuarioModifica;

                    ProyectoPrograma programaDb = ctx.ProyectoPrograma
                                                     .Where(x => x.ProyectoID == proyectoID).Single();

                    ctx.LoadProperty<ProyectoPrograma>(programaDb, x => x.PeriodoPrograma);

                    programaDb.StartTracking();

                    programaDb.FechaModificacion = DateTime.Now;
                    programaDb.IsosPlaneados = programa.IsosPlaneados;
                    programaDb.IsosReprogramados = programa.IsosReprogramados;
                    programaDb.SpoolsPlaneados = programa.SpoolsPlaneados;
                    programaDb.SpoolsReprogramados = programa.SpoolsReprogramados;
                    programaDb.Rango = programa.Rango;
                    programaDb.Unidades = programa.Unidades;
                    programaDb.UsuarioModifica = usuarioModifica;

                    List<PeriodoPrograma> lstDb = programaDb.PeriodoPrograma.ToList();

                    lstDb.ForEach(periodoDb =>
                    {
                        PeriodoPrograma periodo = periodos.Where(x => x.Numero == periodoDb.Numero).SingleOrDefault();
                        periodoDb.StartTracking();

                        if (periodo != null) //update
                        {
                            periodoDb.FechaInicio = periodo.FechaInicio;
                            periodoDb.FechaFin = periodo.FechaFin;
                            periodoDb.UsuarioModifica = usuarioModifica;
                            periodoDb.FechaModificacion = DateTime.Now;
                            periodoDb.PorContrato = periodo.PorContrato;
                            periodoDb.Reprogramaciones = periodo.Reprogramaciones;

                            //quitar de la lista
                            periodos.Remove(periodo);
                        }
                        else
                        {
                            //borrar de la BD
                            ctx.DeleteObject(periodoDb);
                        }

                        periodoDb.StopTracking();
                    });

                    //A estas alturas la lista de periodos ya solo debe tener los nuevos
                    periodos.ForEach(periodo =>
                    {
                        periodo.UsuarioModifica = usuarioModifica;
                        periodo.FechaModificacion = DateTime.Now;
                        programaDb.PeriodoPrograma.Add(periodo);
                    });

                    programaDb.StopTracking();
                    proyecto.StopTracking();

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public ProyectoPrograma ObtenerConPeriodosPorProyectoID(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ProyectoPrograma programa = ctx.ProyectoPrograma.Where(x => x.ProyectoID == proyectoID).Single();
                ctx.LoadProperty<ProyectoPrograma>(programa, x => x.PeriodoPrograma);
                return programa;
            }
        }
    }
}
