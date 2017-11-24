using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesProcesoRaiz
    {
        public static bool TieneWps(SamContext ctx, int procesoRaizID)
        {
            return ctx.Wps.Any(x => x.ProcesoRaizID == procesoRaizID);
        }

        public static bool CodigoDuplicado(SamContext ctx, string codigoProcesoRaiz, int? procesoRaizID)
        {
            return ctx.ProcesoRaiz.Any(x => x.Codigo == codigoProcesoRaiz && x.ProcesoRaizID != procesoRaizID);
        }

        public static bool TieneCostoProcesoRaiz(SamContext ctx, int procesoRaizID)
        {
            return ctx.CostoProcesoRaiz.Any(x => x.ProcesoRaizID == procesoRaizID);
        }

        public static bool TieneJuntaSoldadura(SamContext ctx, int procesoRaizID)
        {
            return ctx.JuntaSoldadura.Any(x => x.ProcesoRaizID == procesoRaizID);
        }
    }
}
