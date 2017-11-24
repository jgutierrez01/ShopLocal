using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesItemCode
    {
        public static bool CodigoDuplicado(SamContext ctx, string codigo, int? itemCodeID, int proyectoID)
        {
            return ctx.ItemCode.Any(x => x.Codigo == codigo && x.ProyectoID == proyectoID && x.ItemCodeID != itemCodeID);
        }

        /// <summary>
        /// Regresa true en caso que el item code pasado tenga relaciones con alguna de las siguientes tablas:
        /// + MaterialSpool
        /// + CorteSpool
        /// + NumeroUnico
        /// </summary>
        /// <param name="ctx">Contexto de la petición</param>
        /// <param name="itemCodeID">ID del item code a borrar</param>
        /// <returns>verdadero si tiene relaciones</returns>
        public static bool TieneRelaciones(SamContext ctx, int itemCodeID)
        {
            return  ctx.MaterialSpool.Any(x => x.ItemCodeID == itemCodeID) ||
                    ctx.CorteSpool.Any(x => x.ItemCodeID == itemCodeID) ||
                    ctx.NumeroUnico.Any(x => x.ItemCodeID == itemCodeID);
        }

    }
}
