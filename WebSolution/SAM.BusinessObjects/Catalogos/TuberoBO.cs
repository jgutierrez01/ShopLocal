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
    public class TuberoBO
    {
        public event TableChangedHandler TuberoCambio;
        private static readonly object _mutex = new object();
        private static TuberoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private TuberoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase TuberoBO
        /// </summary>
        /// <returns></returns>
        public static TuberoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new TuberoBO();
                    }
                }
                return _instance;
            }
        }

        public Tubero Obtener(int tuberoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Tubero.Where(x => x.TuberoID == tuberoID).SingleOrDefault();
            }
        }

        public List<Tubero> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Tubero.ToList();
            }

        }

        public List<Tubero> ObtenerTodosConPatio()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Tubero.Include("Patio").OrderBy(x => x.Codigo).ToList();
            }
        }

        /// <summary>
        /// Obtiene una lista de tuberos relacionados con un proyecto en especifico
        /// a travez del patio en el que se encuentran dados de alta
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <returns>Lista de Tuberos</returns>
        public List<RadTubero> ObtenerPorProyectoParaCombo(int proyectoID, string contextText, int skip, int take)
        {
            List<RadTubero> result = new List<RadTubero>(take * 2);
            contextText = contextText.ToLower();

            using (SamContext ctx = new SamContext())
            {
                result =
                        (from x in ctx.Tubero.Where(y => y.Patio.Proyecto.Select(z => z.ProyectoID).Contains(proyectoID))
                         select new RadTubero
                            {
                                TuberoID = x.TuberoID,
                                Codigo = x.Codigo,
                                Nombre = x.Nombre,
                                ApPaterno = x.ApPaterno,
                                ApMaterno = x.ApMaterno,
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

        public void Guarda(Tubero tubero)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesTubero.CodigoDuplicado(ctx, tubero.Codigo, tubero.TuberoID))
                    {
                        throw new ExcepcionDuplicados(new List<string>() { MensajesError.Excepcion_CodigoDuplicado });
                    }

                    ctx.Tubero.ApplyChanges(tubero);

                    ctx.SaveChanges();
                }

                if (TuberoCambio != null)
                {
                    TuberoCambio();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        public void Borra(int tuberoID)
        {
            using (SamContext ctx = new SamContext())
            {

                if (ValidacionesTubero.TieneJuntaArmado(ctx, tuberoID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionJuntaArmado);
                }

                if (ValidacionesTubero.TieneDestajoTubero(ctx, tuberoID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionDestajoTubero);
                }

                Tubero tubero = ctx.Tubero.Where(x => x.TuberoID == tuberoID).SingleOrDefault();
                
                ctx.DeleteObject(tubero);
                ctx.SaveChanges();

                if (TuberoCambio != null)
                {
                    TuberoCambio();
                }

            }
        }
    }
}
