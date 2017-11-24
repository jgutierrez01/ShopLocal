using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesTipoJunta
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="tipoJunta"></param>
        /// <returns></returns>
        public static bool TieneTipoJuntaRelaciones(SamContext ctx, TipoJunta tipoJunta)
        {
            bool tieneRelaciones = ctx.JuntaSpool.Any(x => x.TipoJuntaID == tipoJunta.TipoJuntaID);
            tieneRelaciones |= ctx.Peq.Any(x => x.TipoJuntaID == tipoJunta.TipoJuntaID);
            tieneRelaciones |= ctx.CostoArmado.Any(x => x.TipoJuntaID == tipoJunta.TipoJuntaID);
            tieneRelaciones |= ctx.CostoProcesoRaiz.Any(x => x.TipoJuntaID == tipoJunta.TipoJuntaID);
            tieneRelaciones |= ctx.CostoProcesoRelleno.Any(x => x.TipoJuntaID == tipoJunta.TipoJuntaID);

            return tieneRelaciones;
        }
    }
}
