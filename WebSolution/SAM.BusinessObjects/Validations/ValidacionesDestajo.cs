using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesDestajo
    {
        /// <summary>
        /// Regresa verdadero si el periodo de destajo se puede cerrar.
        /// Un periodo de detajo puede ser cerrado cuando cada uno de los soldadores y tuberos hayan sido aprobados.
        /// </summary>
        /// <param name="ctx">Contexto listo para usarse de BD</param>
        /// <param name="periodoDestajoID">ID del periodo de destajo que se quiere aprobar</param>
        /// <returns>Verdadero en caso de poderlo cerrar, falso de lo contrario</returns>
        public static bool PuedeCerrarDestajo(SamContext ctx, int periodoDestajoID)
        {
            bool tieneDestajosSoldadorSinAprobar = ctx.DestajoSoldador.Any(x => x.PeriodoDestajoID == periodoDestajoID && !x.Aprobado);
            bool tieneDestajosTuberoSinAprobar = ctx.DestajoTubero.Any(x => x.PeriodoDestajoID == periodoDestajoID && !x.Aprobado);

            return !tieneDestajosSoldadorSinAprobar && !tieneDestajosTuberoSinAprobar;
        }

        /// <summary>
        /// Regresar verdadero si el periodo de destajo se puede eliminar de la BD.
        /// Un periodo de destajo se puede eliminar siempre y cuando no se haya cerrado.
        /// </summary>
        /// <param name="ctx">Contexto listo para usarse de BD</param>
        /// <param name="periodoDestajoID">ID del periodo que se desea eliminar</param>
        /// <returns>Verdadero si se puede eliminar el periodo, falso de lo contrario</returns>
        public static bool PuedeEliminarPeriodo(SamContext ctx, int periodoDestajoID)
        {
            return !ctx.PeriodoDestajo.Where(x => x.PeriodoDestajoID == periodoDestajoID).Select(x => x.Aprobado).Single();
        }
    }
}
