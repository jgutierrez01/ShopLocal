using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesAcero
    {
        public static bool NomenclaturaDuplicada(SamContext ctx, string nomenclatura, int ? aceroID)
        {
            return ctx.Acero.Any(x => x.Nomenclatura == nomenclatura && x.AceroID != aceroID);
        }

        internal static bool TieneColada(SamContext ctx, int aceroID)
        {
            return ctx.Colada.Any(x => x.AceroID == aceroID);
        }
    }
}
