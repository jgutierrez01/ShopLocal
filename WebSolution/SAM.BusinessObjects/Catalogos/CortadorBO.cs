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
    public class CortadorBO
    {
        public event TableChangedHandler CortadorCambio;
        private static readonly object _mutex = new object();
        private static CortadorBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private CortadorBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase DespachadorBO
        /// </summary>
        /// <returns></returns>
        public static CortadorBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CortadorBO();
                    }
                }
                return _instance;
            }
        }

        public Cortador Obtener(int cortadorID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cortador.Where(x => x.CortadorID == cortadorID).SingleOrDefault();
            }
        }

        public List<Cortador> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cortador.ToList();
            }

        }

        public List<Cortador> ObtenerTodosConPatio()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cortador.Include("Patio").Include("Taller").OrderBy(x => x.NumeroEmpleado).ToList();
            }
        }

        /// <summary>
        /// Obtiene todas las maquinas y el nombre del patio al que pertenecen
        /// </summary>
        /// <param name="aceroID"></param>
        /// <returns></returns>
        public List<Cortador> ObtenerTodasConPatioSeleccion()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Cortador
                            .Include("Patio").ToList();
            }
        }

        /// <summary>
        /// Obtiene una lista de despachadores relacionados con un proyecto en especifico
        /// a travez del patio en el que se encuentran dados de alta
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns>Lista de Despachadores</returns>
        public List<RadCortador> ObtenerPorProyectoParaCombo(int proyectoID, string contextText, int skip, int take)
        {
            List<RadCortador> result = new List<RadCortador>(take * 2);
            contextText = contextText.ToLower();

            using (SamContext ctx = new SamContext())
            {
                result =
                        (from x in ctx.Cortador.Where(y => y.Patio.Proyecto.Select(z => z.ProyectoID).Contains(proyectoID))
                         select new RadCortador
                         {
                             CortadorID = x.CortadorID,
                             Nombre = x.Nombre,
                             ApPaterno = x.ApPaterno,
                             ApMaterno = x.ApMaterno,
                             NumeroEmpleado = x.NumeroEmpleado,
                             NombreCompleto = x.Nombre + " " + x.ApPaterno + " " + x.ApMaterno
                         }).ToList();

                return result.Where(x => x.NombreCompleto.ContainsIgnoreCase(contextText) || x.NumeroEmpleado.ContainsIgnoreCase(contextText))
                             .Skip(skip)
                             .Take(take)
                             .ToList();
            }
        }

        public void Guarda(Cortador cortador)
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
                    ctx.Cortador.ApplyChanges(cortador);

                    ctx.SaveChanges();
                }

                if (CortadorCambio != null)
                {
                    CortadorCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int cortadorID)
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
                Cortador cortador = ctx.Cortador.Where(x => x.CortadorID == cortadorID).SingleOrDefault();

                ctx.DeleteObject(cortador);
                ctx.SaveChanges();

                if (CortadorCambio != null)
                {
                    CortadorCambio();
                }

            }
        }

    }
}
