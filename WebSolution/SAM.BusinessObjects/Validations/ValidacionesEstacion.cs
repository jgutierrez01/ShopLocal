using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesEstacion
    {
        public static bool TieneEstacionRelaciones(SamContext ctx, Estacion estacion)
        {
            bool tieneRelaciones = ctx.JuntaSpool.Any(x => x.EstacionID == estacion.EstacionID);
            tieneRelaciones |= ctx.BastonSpool.Any(x => x.EstacionID == estacion.EstacionID);

            return tieneRelaciones;
        }
    }
}
