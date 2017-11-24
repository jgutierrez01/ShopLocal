using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesEstimacion
    {
        /// <summary>
        /// Verifica si algun numero unico perteneciente a la entimacón ya ha sido utilizado dentro del sistema.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="EstimacionID"></param>
        /// <returns></returns>
        public static bool TieneJuntasOSpools(SamContext ctx, int estimacionID)
        {
            //Obtiene cualquier registro que de la tabla EstimacionJunta y EstimacionSpool su tipo de movimiento sea diferente a Estimacion
            // y cuya estimacion coincida con el ID que se recibe
            bool tieneMovimientos = ctx.EstimacionJunta.Any(y => y.EstimacionID == estimacionID) || ctx.EstimacionSpool.Any(y => y.EstimacionID == estimacionID);

            return tieneMovimientos;
        }
    }
}
