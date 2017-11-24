using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesConsumible
    {

        /// <summary>
        /// Regresa true si el consumible tiene alguna relación con la tabla JuntaSoldaduraDetalle
        /// </summary>
        /// <param name="ctx">Contexto de SAM</param>
        /// <param name="consumibleID">ID del consumible</param>
        /// <returns>true si tiene relaciones</returns>
        public static bool TieneRelaciones(SamContext ctx, int consumibleID)
        {
            return ctx.JuntaSoldaduraDetalle.Any(x => x.ConsumibleID == consumibleID);
        }

        /// <summary>
        /// Regresa verdadero si el codigo ya existe en algun otro consumible del patio
        /// </summary>
        /// <param name="ctx">Contexto SAM</param>
        /// <param name="consumibleID">ID del consumible</param>
        /// <param name="codigo">Codigo a verificar</param>
        /// <param name="patioID">ID del patio</param>
        /// <returns>True si el codigo ya existe</returns>
        public static bool CodigoExiste(SamContext ctx, int consumibleID, string codigo, int patioID)
        {
            return ctx.Consumible.Where(x => x.Codigo == codigo && x.ConsumibleID != consumibleID && x.PatioID == patioID).Any(); 
        }
    }
}
