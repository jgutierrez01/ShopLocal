using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesTipoCorte
    {
        public static bool TieneTipoCorteRelaciones(SamContext ctx, TipoCorte tipoCorte)
        {
            bool tieneRelaciones = ctx.CorteSpool.Any(x => x.TipoCorte1ID == tipoCorte.TipoCorteID || x.TipoCorte2ID == tipoCorte.TipoCorteID);
            return tieneRelaciones;
        }
    }
}
