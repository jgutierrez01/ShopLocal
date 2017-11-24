using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesProcesoRelleno
    {
        public static bool TieneWps(SamContext ctx, int procesoRellenoID)
        {
            int registro = ctx.Wps.Where(x => x.ProcesoRellenoID == procesoRellenoID).Count();
            return registro > 0 ? true : false;
        }

        public static bool CodigoDuplicado(SamContext ctx, string codigoProcesoRelleno, int? procesoRellenoID)
        {
            return ctx.ProcesoRelleno.Any(x => x.Codigo == codigoProcesoRelleno && x.ProcesoRellenoID != procesoRellenoID);
        }

        public static bool TieneCostoProcesoRelleno(SamContext ctx, int procesoRellenoID)
        {
            return ctx.CostoProcesoRelleno.Any(x => x.ProcesoRellenoID == procesoRellenoID);
        }

        public static bool TieneJuntaSoldadura(SamContext ctx, int procesoRellenoID)
        {
            return ctx.JuntaSoldadura.Any(x => x.ProcesoRellenoID == procesoRellenoID);
        }
    }
}
