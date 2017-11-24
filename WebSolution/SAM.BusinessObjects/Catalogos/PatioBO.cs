using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Validations;

namespace SAM.BusinessObjects.Catalogos
{
    public class PatioBO
    {
        public event TableChangedHandler PatioCambio;
        private static readonly object _mutex = new object();
        private static PatioBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private PatioBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase PatioBO
        /// </summary>
        /// <returns></returns>
        public static PatioBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PatioBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patioID"></param>
        /// <returns></returns>
        public Patio Obtener(int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Patio.Where(x => x.PatioID == patioID).SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Patio> ObtenerConMaquinaTaller()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Patio
                          .Include("Taller.Estacion")
                          .Include("Maquina")
                          .OrderBy(x => x.Nombre)
                          .ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patioID"></param>
        /// <returns></returns>
        public Patio ObtenerConMaquinaTallerUbicacion(int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Patio.Include("Taller.Estacion")
                                .Include("Maquina")
                                .Include("UbicacionFisica")
                                .Include("Destino")
                                .Include("Cuadrante")
                                .Where(x => x.PatioID == patioID)
                                .SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Patio> ObtenerTodosConTaller()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Patio.Include("Taller").ToList();
            }
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Patio> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Patio.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patio"></param>
        public void Guarda(Patio patio)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesPatio.NomenclaturaDuplicada(ctx, patio.Nombre, patio.PatioID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_NombreDuplicado });
                    }

                    ctx.Patio.ApplyChanges(patio);
                    ctx.SaveChanges();

                    List<Entities.Destino> Destino = (List<Entities.Destino>)((TrackableCollection<Entities.Destino>)patio.Destino).ToList();

                    foreach (var row in Destino)
                    {                                              
                        Destino destino = ctx.Destino.Where(x => x.DestinoID == row.DestinoID).FirstOrDefault();
                        if (destino.CuadranteID != 0 && destino.CuadranteID != null) {
                            Cuadrante cuadrante = ctx.Cuadrante.Where(x => x.CuadranteID==destino.CuadranteID).FirstOrDefault();
                            cuadrante.Nombre = row.Nombre;
                            cuadrante.UsuarioModifica = row.UsuarioModifica;
                            cuadrante.FechaModificacion = row.FechaModificacion;
                            ctx.Cuadrante.ApplyChanges(cuadrante);
                        }
                        else
                        {
                            Cuadrante cuadrante = ctx.Cuadrante.Where(x => x.PatioID == patio.PatioID && x.Nombre.ToUpper() == row.Nombre.ToUpper()).FirstOrDefault();
                            if (cuadrante != null)
                            {
                                destino.CuadranteID = cuadrante.CuadranteID;
                                ctx.Destino.ApplyChanges(destino);
                            }
                            else
                            {
                                cuadrante = new Cuadrante();
                                cuadrante.Nombre = row.Nombre;
                                cuadrante.PatioID = row.PatioID;
                                cuadrante.UsuarioModifica = row.UsuarioModifica;
                                cuadrante.FechaModificacion = row.FechaModificacion;
                                ctx.Cuadrante.ApplyChanges(cuadrante);
                                ctx.SaveChanges();
                                
                                destino.CuadranteID = cuadrante.CuadranteID;
                                ctx.Destino.ApplyChanges(destino);
                            }
                        }
                    }
                    

                    ctx.SaveChanges();
                }

                if ( PatioCambio != null )
                {
                    PatioCambio();
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
        /// <param name="patioID"></param>
        public void Borra(int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesPatio.TieneTaller(ctx, patioID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionPatioTaller);
                }
                else if (ValidacionesPatio.TieneMaquina(ctx, patioID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionPatioMaquina);
                }
                else if (ValidacionesPatio.TieneSoldador(ctx, patioID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionPatioSoldador);
                }
                else if (ValidacionesPatio.TieneProyecto(ctx, patioID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionProyecto);
                }
                else if (ValidacionesPatio.TieneUbicacionFisica(ctx, patioID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionUbicacionFisica);
                }
                else if (ValidacionesPatio.TieneTubero(ctx, patioID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionTubero);
                }
                else if (ValidacionesPatio.TieneConsumible(ctx, patioID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionConsumible);
                }

                Patio patio = ctx.Patio.Where(x => x.PatioID == patioID).SingleOrDefault();
                ctx.DeleteObject(patio);
                ctx.SaveChanges();

                if (PatioCambio != null)
                {
                    PatioCambio();
                }
            }
        }
    }
}
