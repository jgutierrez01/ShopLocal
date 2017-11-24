using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesTipoPrueba
    {

        /// <summary>
        /// Regresa verdadero si el nombre del tipo de prueba es duplicado
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="nombre"></param>
        /// <param name="tipoPruebaID"></param>
        /// <returns></returns>
        public static bool NombreDuplicado(SamContext ctx, string nombre, int? tipoPruebaID)
        {
            return ctx.TipoPrueba.Any(x => x.Nombre == nombre && x.TipoPruebaID != tipoPruebaID);
        }
    }
}
