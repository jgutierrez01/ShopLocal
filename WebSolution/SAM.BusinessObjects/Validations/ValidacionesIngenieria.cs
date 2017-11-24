using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesIngenieria
    {
        public static bool DiametroExiste(decimal diametro)
        {
            return CacheCatalogos.Instance.ObtenerDiametros().Any(x => x.Valor == diametro);
        }

        public static bool CedulaExiste(string codigo)
        {
            return CacheCatalogos.Instance.ObtenerCedulas().Any(x => x.Nombre == codigo);
        }

        public static bool EtiquetaExiste(string etiqueta, int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<JuntaSpool> lstJs = ctx.JuntaSpool.Where(x => x.SpoolID == spoolID).ToList();

                return lstJs.Any(x => x.Etiqueta == etiqueta);
            }
        }
    }
}
