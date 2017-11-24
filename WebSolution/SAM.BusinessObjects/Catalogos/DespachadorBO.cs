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
    public class DespachadorBO
    {
        public event TableChangedHandler DespachadorCambio;
        private static readonly object _mutex = new object();
        private static DespachadorBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private DespachadorBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase DespachadorBO
        /// </summary>
        /// <returns></returns>
        public static DespachadorBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DespachadorBO();
                    }
                }
                return _instance;
            }
        }

        public Despachador Obtener(int despachadorID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Despachador.Where(x => x.DespachadorID == despachadorID).SingleOrDefault();
            }
        }

        public List<Despachador> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Despachador.ToList();
            }

        }

        public List<Despachador> ObtenerTodosConPatio()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Despachador.Include("Patio").Include("Taller").OrderBy(x => x.NumeroEmpleado).ToList();
            }
        }

        /// <summary>
        /// Obtiene todas las maquinas y el nombre del patio al que pertenecen
        /// </summary>
        /// <param name="aceroID"></param>
        /// <returns></returns>
        public List<Despachador> ObtenerTodasConPatioSeleccion()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Despachador
                            .Include("Patio").ToList();
            }
        }

        /// <summary>
        /// Obtiene una lista de despachadores relacionados con un proyecto en especifico
        /// a travez del patio en el que se encuentran dados de alta
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns>Lista de Despachadores</returns>
        public List<RadDespachador> ObtenerPorProyectoParaCombo(int proyectoID, string contextText, int skip, int take)
        {
            List<RadDespachador> result = new List<RadDespachador>(take * 2);
            contextText = contextText.ToLower();

            using (SamContext ctx = new SamContext())
            {
                result =
                        (from x in ctx.Despachador.Where(y => y.Patio.Proyecto.Select(z => z.ProyectoID).Contains(proyectoID))
                         select new RadDespachador
                         {
                             DespachadorID = x.DespachadorID,
                             Nombre = x.Nombre,
                             ApPaterno = x.ApPaterno,
                             ApMaterno = x.ApMaterno,
                             NumeroEmpleado = x.NumeroEmpleado,
                             NombreCompleto = x.Nombre + " " + x.ApPaterno + " " + x.ApMaterno
                         }).ToList();

                result = result.Where(x => x.NombreCompleto.ContainsIgnoreCase(contextText) || x.NumeroEmpleado.ContainsIgnoreCase(contextText))
                             .Skip(skip)
                             .Take(take)
                             .ToList();
                return result;
            }
        }

        public void Guarda(Despachador despachador)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    /*
                    if (ValidacionesDespachador.CodigoDuplicado(ctx, despachador.NumeroEmpleado, despachador.DespachadorID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_CodigoDuplicado });
                    }
                    */
                    ctx.Despachador.ApplyChanges(despachador);

                    ctx.SaveChanges();
                }

                if (DespachadorCambio != null)
                {
                    DespachadorCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int despachadorID)
        {
            using (SamContext ctx = new SamContext())
            {
                /*
                if (ValidacionesDespachador.TieneJuntaArmado(ctx, despachadorID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionJuntaArmado);
                }

                if (ValidacionesDespachador.TieneDestajoDespachador(ctx, despachadorID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionDestajoTubero);
                }
                */
                Despachador despachador = ctx.Despachador.Where(x => x.DespachadorID == despachadorID).SingleOrDefault();

                ctx.DeleteObject(despachador);
                ctx.SaveChanges();

                if (DespachadorCambio != null)
                {
                    DespachadorCambio();
                }

            }
        }

    }
}
