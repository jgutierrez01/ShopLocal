using System.Linq;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using System.Collections.Generic;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesDefecto
    {
        public static bool TieneDefectoRelaciones(SamContext ctx, Defecto defecto)
        {
            bool tieneRelaciones = ctx.JuntaInspeccionVisualDefecto.Any(x => x.DefectoID == defecto.DefectoID);
            tieneRelaciones |= ctx.JuntaReportePndCuadrante.Any(x => x.DefectoID == defecto.DefectoID);
            tieneRelaciones |= ctx.JuntaReportePndSector.Any(x => x.DefectoID == defecto.DefectoID);

            return tieneRelaciones;
        }

        /// <summary>
        /// Valida que el defecto enviado no se encuentre ya dado de alto en la lista recibida
        /// </summary>
        /// <param name="lista">Listado de Defectos</param>
        /// <param name="defecto">Defecto a validar</param>
        /// <returns>Falso si no se encuentra en la lista, si el defecto si se encuentra dentro del listado se lanzará una excepcion</returns>
        public static bool ExistenDuplicados(List<Defecto> lista, Defecto defecto)
        {
            foreach (Defecto def in lista)
            {
                if (def.DefectoID == defecto.DefectoID)
                {
                    throw new Excepciones.ExcepcionDuplicados(MensajesError.Excepcion_DefectoDuplicado);
                }
            }

            return false;
        }
    }
}
