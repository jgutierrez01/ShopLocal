using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Extensions;

namespace SAM.BusinessObjects.Utilerias
{
    public static class RechazosCortesUtil
    {
        /// <summary>
        /// Regresa la nueva etiqueta despues de haber aplicado un corte
        /// </summary>
        /// <param name="etiquetaProduccionActual">La etiqueta de la junta actual en producción (JuntaWorkstatus o JuntaCampo)</param>
        /// <param name="etiquetaIngenieria">La etiqueta como se tiene en ingeniería</param>
        /// <returns></returns>
        public static string ObtenNuevaEtiquetaDeCorte(string etiquetaProduccionActual, string etiquetaIngenieria)
        {
            int numCorteNuevo = 1;
            if (EtiquetaTieneCorte(etiquetaProduccionActual, etiquetaIngenieria))
            {
                numCorteNuevo = etiquetaProduccionActual.Substring(etiquetaProduccionActual.LastIndexOf('C') + 1, 1).SafeIntParse() + 1;
            }
            return etiquetaIngenieria + "C" + numCorteNuevo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etiquetaProduccionActual"></param>
        /// <param name="etiquetaIngenieria"></param>
        /// <returns></returns>
        public static bool EtiquetaTieneCorte(string etiquetaProduccionActual, string etiquetaIngenieria)
        {
            string etiquetaSinOriginal = etiquetaProduccionActual.Remove(0, etiquetaIngenieria.Count());

            if (string.IsNullOrEmpty(etiquetaSinOriginal))
            {
                return false;
            }
            else
            {
                if (etiquetaSinOriginal.ToCharArray()[0] == 'C')
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etiquetaJunta"></param>
        /// <returns></returns>
        public static bool EtiquetaTieneRechazo(IEnumerable<char> etiquetaJunta)
        {
            char? ultimaLetra = null;
            //este ciclo itera en reversa cada caracter en la etiqueta y deja en ultimaLetra el ultimo caracter no numerico encontrado
            foreach (char caracter in etiquetaJunta.Reverse())
            {
                //es necesario hacer el cast a string, por que los chars son compatibles con int!!!
                if (caracter.ToString().SafeIntParse() == -1)
                {
                    ultimaLetra = caracter;
                    break;
                }
            }

            //Si la etiqueta tuvo almenos una letra
            if (ultimaLetra.HasValue)
            {
                return ultimaLetra.Value == 'R';
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etiquetaJunta"></param>
        /// <returns></returns>
        public static int ObtenerSiguienteRechazo(string etiquetaJunta)
        {
            int numRechazoNuevo = 1;
            if (EtiquetaTieneRechazo(etiquetaJunta))
            {
                numRechazoNuevo = etiquetaJunta.Substring(etiquetaJunta.LastIndexOf('R') + 1).SafeIntParse() + 1;
            }
            return numRechazoNuevo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="numRechazoNuevo"></param>
        /// <param name="etiquetaJunta"></param>
        /// <returns></returns>
        public static  string ObtenerNuevaEtiquetaDeRechazo(int numRechazoNuevo, string etiquetaJunta)
        {
            if (numRechazoNuevo > 1)
            {
                etiquetaJunta = etiquetaJunta.Substring(0, etiquetaJunta.LastIndexOf('R'));
            }
            return etiquetaJunta + "R" + numRechazoNuevo;
        }
    }
}
