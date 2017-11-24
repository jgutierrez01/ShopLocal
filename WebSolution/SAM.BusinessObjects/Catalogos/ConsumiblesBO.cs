using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Personalizadas;
using System;

namespace SAM.BusinessObjects.Catalogos
{
    public class ConsumiblesBO
    {
        private static readonly object _mutex = new object();
        private static ConsumiblesBO _instance;

        /// <summary>
        /// constructor privado para implementar el singleton
        /// </summary>
        private ConsumiblesBO()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase coladasbo
        /// </summary>
        public static ConsumiblesBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ConsumiblesBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumible"></param>
        public void Guarda(Consumible consumible)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesConsumible.CodigoExiste(ctx, consumible.ConsumibleID, consumible.Codigo, consumible.PatioID))
                {
                    throw new ExcepcionDuplicados(MensajesError.Excepcion_CodigoDuplicado);
                }

                ctx.Consumible.ApplyChanges(consumible);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumibleID"></param>
        /// <returns></returns>
        public Consumible Obtener(int consumibleID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Consumible
                          .Include("Patio")
                          .Where(x => x.ConsumibleID == consumibleID)
                          .SingleOrDefault();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="patioID"></param>
        /// <returns></returns>
        public List<Consumible> ObtenerPorPatio(int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Consumible
                          .Where(x => x.PatioID == patioID)
                          .ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumibleID"></param>
        public void Borra(int consumibleID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ValidacionesConsumible.TieneRelaciones(ctx, consumibleID))
                {
                    throw new ExcepcionRelaciones(MensajesError.Excepcion_ConsumibleTieneRelaciones);
                }

                Consumible cons = ctx.Consumible
                                     .Where(x => x.ConsumibleID == consumibleID)
                                     .Single();

                ctx.DeleteObject(cons);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene la lista de consumibles por patio para un rad combo
        /// </summary>
        /// <param name="patioID">ID del patio</param>
        /// <param name="codigo">Texto a igualar con el codigo del consumible</param>
        /// <param name="skip">Cantidad de elementos a ignorar</param>
        /// <param name="take">Cantidad de elementos a obtener</param>
        /// <returns>Lista de Spools (ID, Valor)</returns>
        public List<Simple> ObtenerPorPatioParaRadCombo(int patioID, string codigo, int skip, int take)
        {
            using (SamContext ctx = new SamContext())
            {
                List<Simple> consumibles = (from consumible in ctx.Consumible
                                            where consumible.PatioID == patioID
                                            select new Simple
                                            {
                                                ID = consumible.ConsumibleID,
                                                Valor = consumible.Codigo
                                            }).ToList();

                return consumibles.Where(x => x.Valor.StartsWith(codigo, StringComparison.InvariantCultureIgnoreCase))
                                  .OrderBy(x => x.Valor)
                                  .Skip(skip)
                                  .Take(take)
                                  .ToList();
            }
        }

    }
}
