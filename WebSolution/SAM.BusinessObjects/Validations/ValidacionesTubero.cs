using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesTubero
    {
        public static bool CodigoDuplicado(SamContext ctx, string codigo, int? tuberoID)
        {
            return ctx.Tubero.Any(x => x.Codigo == codigo && x.TuberoID != tuberoID);
        }

        public static bool TieneJuntaArmado(SamContext ctx, int tuberoID)
        {
            return ctx.JuntaArmado.Any(x => x.TuberoID == tuberoID);
        }

        public static bool TieneDestajoTubero(SamContext ctx, int tuberoID)
        {
            return ctx.DestajoTubero.Any(x => x.TuberoID == tuberoID);
        }
    }
}
