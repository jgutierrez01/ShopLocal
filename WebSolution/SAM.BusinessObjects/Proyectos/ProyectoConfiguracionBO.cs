using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Proyectos
{
    public class ProyectoConfiguracionBO
    {
        private static readonly object _mutex = new object();
        private static ProyectoConfiguracionBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProyectoConfiguracionBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProyectoConfiguracionBO
        /// </summary>
        /// <returns></returns>
        public static ProyectoConfiguracionBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProyectoConfiguracionBO();
                    }
                }
                return _instance;
            }
        }

        public ProyectoConfiguracion Obtener(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProyectoConfiguracion.Where(x => x.ProyectoID == proyectoID).Single();
            }
        }

        /// <summary>
        /// Obtiene el listado de pendientes con sus responsables para un proyecto en particular
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns>Listado de pendientes con responsables</returns>
        public List<ProyectoPendiente> ObtenerPendientesAutomaticos(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProyectoPendiente.Include("TipoPendiente").Include("Usuario").Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }

        /// <summary>
        /// Guarda la relacion de pendiente con su responsable.
        /// </summary>
        /// <param name="proyectoID">Relacion pendiente-responsable</param>
        /// <param name="listaPendientes">Relacion pendiente-responsable</param>
        /// <param name="listaPendientes">Relacion pendiente-responsable</param>
        public void GuardaPendientesAutomaticos(int proyectoID, List<Simple> listaPendientes, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                foreach (Simple simple in listaPendientes)
                {
                    if (simple.Valor.SafeGuidParse() == Guid.Empty)
                    {
                        throw new Excepciones.ExcepcionRelaciones(MensajesError.Excepcion_ResponsableRequerido);
                    }

                    ProyectoPendiente proyPendiente = ctx.ProyectoPendiente.Where(x => x.ProyectoID == proyectoID && x.TipoPendienteID == simple.ID).SingleOrDefault();

                    if (proyPendiente.Responsable != simple.Valor.SafeGuidParse())
                    {
                        proyPendiente.Responsable = simple.Valor.SafeGuidParse();
                        proyPendiente.UsuarioModifica = userID;
                        proyPendiente.FechaModificacion = DateTime.Now;
                        ctx.ProyectoPendiente.ApplyChanges(proyPendiente);
                    }
                }

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Guarda la relacion entre los ids de los wps recibidos con el proyecto
        /// </summary>
        /// <param name="ids">Arreglo de IDs de wps</param>
        /// <param name="proyectoID">Id del proyecto</param>
        /// <param name="userID">Usuario logeado</param>
        public void GuardaWPSProyecto(List<int> ids, int proyectoID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<WpsProyecto> wpsExportar = new List<WpsProyecto>();

                foreach (int x in ids)
                {
                    WpsProyecto wpsProy = new WpsProyecto
                    {
                        WpsID = x,
                        ProyectoID = proyectoID,
                        UsuarioModifica = userID,
                        FechaModificacion = DateTime.Now
                    };
                    wpsExportar.Add(wpsProy);
                }

                wpsExportar.ForEach(x => ctx.WpsProyecto.ApplyChanges(x));

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Importa las tablas de destajos de un proyecto a otro, eliminando todos los registros del proyecto 
        /// hacia el que se importaran los nuevos valores
        /// </summary>
        /// <param name="de_proyectoID">Proyecto del que se obtendran las tablas</param>
        /// <param name="a_proyectoID">Proyecto al que se copiarán las tablas</param>
        /// <param name="userID">Id del usuario</param>
        public void ImportaDestajos(int de_proyectoID, int a_proyectoID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<CostoProcesoRelleno> rellenoImportar = ctx.CostoProcesoRelleno.Where(x => x.ProyectoID == de_proyectoID).ToList();
                List<CostoProcesoRaiz> raizImportar = ctx.CostoProcesoRaiz.Where(x => x.ProyectoID == de_proyectoID).ToList();
                List<CostoArmado> armadoImportar = ctx.CostoArmado.Where(x => x.ProyectoID == de_proyectoID).ToList();

                if (rellenoImportar.Count == 0 && raizImportar.Count == 0 && armadoImportar.Count == 0)
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_DestajosSinValores);
                }
                else
                {

                    ctx.CostoProcesoRelleno.Where(x => x.ProyectoID == a_proyectoID).ToList().ForEach(x => ctx.CostoProcesoRelleno.DeleteObject(x));

                    List<CostoProcesoRelleno> rellenoExportar =
                        rellenoImportar.Select(x => new CostoProcesoRelleno
                                                    {
                                                        ProyectoID = a_proyectoID,
                                                        FamiliaAceroID = x.FamiliaAceroID,
                                                        TipoJuntaID = x.TipoJuntaID,
                                                        ProcesoRellenoID = x.ProcesoRellenoID,
                                                        DiametroID = x.DiametroID,
                                                        CedulaID = x.CedulaID,
                                                        Costo = x.Costo,
                                                        UsuarioModifica = userID,
                                                        FechaModificacion = DateTime.Now
                                                    }).ToList();

                    rellenoExportar.ForEach(x => ctx.CostoProcesoRelleno.ApplyChanges(x));


                    ctx.CostoProcesoRaiz.Where(x => x.ProyectoID == a_proyectoID).ToList().ForEach(x => ctx.CostoProcesoRaiz.DeleteObject(x));

                    List<CostoProcesoRaiz> raizExportar =
                        raizImportar.Select(x => new CostoProcesoRaiz
                                                {
                                                    ProyectoID = a_proyectoID,
                                                    FamiliaAceroID = x.FamiliaAceroID,
                                                    TipoJuntaID = x.TipoJuntaID,
                                                    ProcesoRaizID = x.ProcesoRaizID,
                                                    DiametroID = x.DiametroID,
                                                    CedulaID = x.CedulaID,
                                                    Costo = x.Costo,
                                                    UsuarioModifica = userID,
                                                    FechaModificacion = DateTime.Now

                                                }).ToList();
                    raizExportar.ForEach(x => ctx.CostoProcesoRaiz.ApplyChanges(x));


                    ctx.CostoArmado.Where(x => x.ProyectoID == a_proyectoID).ToList().ForEach(x => ctx.CostoArmado.DeleteObject(x));

                    List<CostoArmado> armadoExportar =
                        armadoImportar.Select(x => new CostoArmado
                                                    {
                                                        UsuarioModifica = userID,
                                                        FechaModificacion = DateTime.Now,
                                                        FamiliaAceroID = x.FamiliaAceroID,
                                                        TipoJuntaID = x.TipoJuntaID,
                                                        DiametroID = x.DiametroID,
                                                        CedulaID = x.CedulaID,
                                                        Costo = x.Costo,
                                                        ProyectoID = a_proyectoID
                                                    }).ToList();
                    armadoExportar.ForEach(x => ctx.CostoArmado.ApplyChanges(x));

                    ctx.SaveChanges();
                }
            }
        }
    }
}
