using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesRecepcion
    {
        /// <summary>
        /// Verifica si algun numero unico perteneciente a la recepción ya ha sido utilizado dentro del sistema.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="recepcionID"></param>
        /// <returns></returns>
        public static bool NumeroUnicoConMovimientos(SamContext ctx, int recepcionID)
        {
            //Obtiene cualquier registro que de la tabla NumeroUnicoMovimiento su tipo de movimiento sea diferente a Recepcion
            // y cuya recepcion coincida con el ID que se recibe
            bool tieneMovimientos = (from num in ctx.NumeroUnicoMovimiento
                                     where num.TipoMovimientoID != (int)TipoMovimientoEnum.Recepcion
                                           && (from rnu in num.NumeroUnico.RecepcionNumeroUnico
                                               where rnu.RecepcionID == recepcionID
                                               select rnu).Any()
                                     select num).Any();



            return tieneMovimientos;
        }

        /// <summary>
        /// Verifica si la recepcion cuenta con algún numero unico que haya sido congelado en cruce
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="recepcionID">ID de la recepcion</param>
        /// <returns>True si sí hay numero unico congelado - False de lo contrario</returns>
        public static bool NumeroUnicoCongelado(SamContext ctx, int recepcionID)
        {
            //Obtiene cualquier registro de la tabla OrdenTrabajoMaterial en el que el numero unico congelado sea parte de la recepcion
            bool numeroUnicoCongelado = (from num in ctx.OrdenTrabajoMaterial
                                         join rnu in ctx.RecepcionNumeroUnico on num.NumeroUnicoCongeladoID equals rnu.NumeroUnicoID
                                         where rnu.RecepcionID == recepcionID
                                         select num).Any();



            return numeroUnicoCongelado;
        }
    }
}
