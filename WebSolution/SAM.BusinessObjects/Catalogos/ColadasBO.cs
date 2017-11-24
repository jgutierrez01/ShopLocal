using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Validations;
using System.Data.Objects;
using SAM.Entities.Personalizadas;
using System;
using SAM.Entities.RadCombo;
using Mimo.Framework.Extensions;

namespace SAM.BusinessObjects.Catalogos
{
    public class ColadasBO
    {
        private static readonly object _mutex = new object();
        private static ColadasBO _instance;

        /// <summary>
        /// constructor privado para implementar el singleton
        /// </summary>
        private ColadasBO()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase coladasbo
        /// </summary>
        public static ColadasBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ColadasBO();
                    }
                }
                return _instance;
            }
        }

        public Colada Obtener(int coladaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Colada.Where(x => x.ColadaID == coladaID).SingleOrDefault();
            }
        }

        public List<Colada> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Colada
                        .Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }

        public Colada ObtenerConFabricanteYAcero(int coladaID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Colada.MergeOption = MergeOption.NoTracking;
                ctx.Acero.MergeOption = MergeOption.NoTracking;
                ctx.FamiliaAcero.MergeOption = MergeOption.NoTracking;
                ctx.FamiliaMaterial.MergeOption = MergeOption.NoTracking;

                return ctx.Colada
                          .Include("Fabricante")
                          .Include("Acero")
                          .Include("Acero.FamiliaAcero")
                          .Include("Acero.FamiliaAcero.FamiliaMaterial")
                          .Where(x => x.ColadaID == coladaID)
                          .SingleOrDefault();
            }
        }

        public List<Colada> ObtenerConFabricanteYAceroPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Colada.MergeOption = MergeOption.NoTracking;
                ctx.Fabricante.MergeOption = MergeOption.NoTracking;
                ctx.Acero.MergeOption = MergeOption.NoTracking;

                return ctx.Colada
                          .Include("Fabricante")
                          .Include("Acero")
                          .Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }

        public void Guarda(Colada colada)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesColada.ColadaDuplicada(ctx, colada.NumeroColada, colada.FabricanteID, colada.ProyectoID, colada.NumeroCertificado, colada.ColadaID))
                    {
                        throw new ExcepcionDuplicados(MensajesError.Excepcion_ColadaDuplicada);
                    }
                    else
                    {
                        ctx.Colada.ApplyChanges(colada);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void Borra(int coladaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesColada.TieneRelaciones(ctx, coladaID))
                    {
                        throw new ExcepcionRelaciones(MensajesError.Excepcion_ColadaTieneRelaciones);
                    }

                    Colada colada = ctx.Colada.Where(x => x.ColadaID == coladaID).Single();
                    ctx.Colada.DeleteObject(colada);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }


        /// <summary>
        /// Obtiene la lista de coladas por proyecto para un rad combo
        /// </summary>
        /// <param name="proyectoID">ID del proyecto</param>
        /// <param name="numeroColada">Texto a igualar con numero de colada</param>
        /// <param name="skip">Cantidad de elementos a ignorar</param>
        /// <param name="take">Cantidad de elementos a obtener</param>
        /// <returns>Lista de Coladas (ID, Valor)</returns>
        public List<RadColada> ObtenerPorProyectoParaRadCombo(int proyectoID, string numeroColada, int skip, int take)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.Colada.MergeOption = MergeOption.NoTracking;

                List<RadColada> coladas = (from colada in ctx.Colada
                                           where colada.ProyectoID == proyectoID
                                           select new RadColada
                                           {
                                               ColadaID = colada.ColadaID,
                                               Certificado = colada.NumeroCertificado,
                                               NumeroColada = colada.NumeroColada,
                                               Fabricante = colada.Fabricante != null ? colada.Fabricante.Nombre : string.Empty
                                           }).ToList();

                return coladas.Where(x => x.NumeroColada.ContainsIgnoreCase(numeroColada))
                              .OrderBy(x => x.NumeroColada)
                              .ThenBy(x => x.Certificado)
                              .Skip(skip)
                              .Take(take)
                              .ToList();
            }
        }

        /// <summary>
        /// Regresa el ID del proyecto al cual pertenece la colada.
        /// </summary>
        /// <param name="coladaID">ID de la colada</param>
        /// <returns>Id del proyecto al cual pertenece la colada</returns>
        public int ObtenerProyectoID(int coladaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoColada(ctx, coladaID);
            }
        }


        /// <summary>
        /// Versión compilada del query para permisos de colada
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoColada =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.Colada
                            .Where(x => x.ColadaID == id)
                            .Select(x => x.ProyectoID)
                            .Single()
        );
    }
}
