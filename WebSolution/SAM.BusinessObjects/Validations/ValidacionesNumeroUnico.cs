using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesNumeroUnico
    {
        /// <summary>
        /// Valida que el corte a realizar sea dentro de la tolerancia configurada en las propiedades del proyecto.
        /// </summary>
        /// <param name="cantidadReal">Cantidad a cortar</param>
        /// <param name="cantidadRequerida">Cantidad especificada por ingenieria como la necesaria para el armado del spool</param>
        /// <param name="tolerancia">Tolerancia configurada en el proyecto</param>
        /// <returns></returns>
        public static bool CorteDentroDeTolerancia(int cantidadReal, int cantidadRequerida, int tolerancia)
        {
            if (cantidadReal >= cantidadRequerida - tolerancia
                     && cantidadReal <= cantidadRequerida + tolerancia)
            {
                return true;
            }
            else
            {
                throw new Excepciones.ExcepcionRelaciones(MensajesError.Excepcion_CorteFueraDeTolerancia);
            }
        }

        public static bool CorteDentroDeInventario(int cantidadReal, int cantidadInventario)
        {
            if (cantidadReal <= cantidadInventario)
            {
                return true;
            }
            else
            {
                throw new Excepciones.ExcepcionRelaciones(MensajesError.Excepcion_CorteFueraInventario);
            }
        }
    }
}
