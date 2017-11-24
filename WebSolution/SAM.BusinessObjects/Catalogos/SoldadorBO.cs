using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using System.Data.Objects;
using SAM.BusinessObjects.Validations;
using SAM.Entities.RadCombo;
using Mimo.Framework.Extensions;


namespace SAM.BusinessObjects.Catalogos
{
    public class SoldadorBO
    {
        public event TableChangedHandler SoldadorCambio;
        private static readonly object _mutex = new object();
        private static SoldadorBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private SoldadorBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase SoldadorBO
        /// </summary>
        /// <returns></returns>
        public static SoldadorBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SoldadorBO();
                    }
                }
                return _instance;
            }
        }

        public Soldador Obtener(int soldadorID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Soldador.Where(x => x.SoldadorID == soldadorID).SingleOrDefault();
            }
        }

        public Soldador ObtenerConWpqs(int soldadorID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Soldador.Include("Wpq")
                                   .Include("Wpq.Wps")
                                   .Where(x => x.SoldadorID == soldadorID).SingleOrDefault();
            }
        }

        public List<Soldador> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Soldador.ToList();
            }
     
        }

        public List<Soldador> ObtenerTodosConPatio()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Soldador.Include("Patio").OrderBy(x => x.Codigo).ToList();
            }
        }

        public void Guarda(Soldador soldador)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesSoldador.CodigoDuplicado(ctx, soldador.Codigo, soldador.SoldadorID, soldador.PatioID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_CodigoDuplicado });
                    }

                    ctx.Soldador.ApplyChanges(soldador);

                    ctx.SaveChanges();

                    ActulizaFechaVigencia(soldador.SoldadorID);
                }

                if (SoldadorCambio != null)
                {
                    SoldadorCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        private void ActulizaFechaVigencia(int _soldadorId)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Soldador soldador = ctx.Soldador.Where(x => x.SoldadorID == _soldadorId).Single();
                    soldador.FechaVigencia = ctx.Wpq.Where(x => x.SoldadorID == _soldadorId).Select(x => x.FechaVigencia).Max();
                    ctx.Soldador.ApplyChanges(soldador);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
 
        }

        public void Borra(int soldadorID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesTransportista.TieneDestajoSoldador(ctx, soldadorID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionDestajoSoldador);
                }

                if (ValidacionesTransportista.TieneJuntaSoldaduraDetalle(ctx, soldadorID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_SoldaduraDetallle);
                }

                if (ValidacionesTransportista.TieneWpq(ctx, soldadorID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionWpq);
                }

                Soldador soldador = ctx.Soldador.Where(x => x.SoldadorID == soldadorID).Single();

                ctx.DeleteObject(soldador);
                ctx.SaveChanges();

                if (SoldadorCambio != null)
                {
                    SoldadorCambio();
                }

            }
        }

        /// <summary>
        /// Obtiene una lista de soldadores relacionados con un proyecto en especifico
        /// a travez del patio en el que se encuentran dados de alta
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns>Lista de Soldadores</returns>
        public List<RadSoldador> ObtenerPorProyectoParaCombo(int proyectoID, string contextText, int skip, int take)
        {
            List<RadSoldador> result = new List<RadSoldador>(take * 2);
            contextText = contextText.ToLower();

            using (SamContext ctx = new SamContext())
            {
                result =
                        (from x in ctx.Soldador.Where(y => y.Patio.Proyecto.Select(z => z.ProyectoID).Contains(proyectoID))
                         select new RadSoldador
                         {
                             SoldadorID = x.SoldadorID,
                             Codigo = x.Codigo,
                             Nombre = x.Nombre,
                             ApPaterno = x.ApPaterno,
                             ApMaterno = x.ApMaterno,
                             AreaTrabajo = x.AreaTrabajo,
                             NombreCompleto = x.Nombre + " " + x.ApPaterno + " " + x.ApMaterno
                         }).ToList();

                return result.Where(x => x.NombreCompleto.ContainsIgnoreCase(contextText)
                                      || x.Codigo.ContainsIgnoreCase(contextText))
                             .OrderBy(x => x.Codigo)
                             .Skip(skip)
                             .Take(take)
                             .ToList();
            }
        }

        public IList<Wps> ObtenerWpsPorPatio(int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<Proyecto> Proyectos = ctx.Proyecto.Where(x => x.PatioID == patioID).AsQueryable();

                IQueryable<WpsProyecto> WpsProyecto =
                    ctx.WpsProyecto.Where(x => Proyectos.Select(y => y.ProyectoID).Contains(x.ProyectoID)).AsQueryable();

                return ctx.Wps.Where(x => WpsProyecto.Select(y => y.WpsID).Contains(x.WpsID)).ToList();
            }
        }
    }
}
