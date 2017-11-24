using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Catalogos;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesFamiliaAcero
    {
        public static bool TieneAceros(SamContext ctx, int familiaAceroID)
        {
            int registro = ctx.Acero.Where(x => x.FamiliaAceroID == familiaAceroID).Count();
            return registro > 0 ? true : false;
        }

        public static bool TieneCostoArmado(SamContext ctx, int familiaAceroID)
        {
            return ctx.CostoArmado.Any(x => x.FamiliaAceroID == familiaAceroID);
        }

        public static bool TieneCostoProcesoRaiz(SamContext ctx, int familiaAceroID)
        {
            return ctx.CostoProcesoRaiz.Any(x => x.FamiliaAceroID == familiaAceroID);
        }

        public static bool TieneCostoProcesoRelleno(SamContext ctx, int familiaAceroID)
        {
            return ctx.CostoProcesoRelleno.Any(x => x.FamiliaAceroID == familiaAceroID);
        }

        public static bool TieneJuntaSpool(SamContext ctx, int familiaAceroID)
        {
            return ctx.JuntaSpool.Any(x => x.FamiliaAceroMaterial1ID == familiaAceroID);
        }

        public static bool TienePeq(SamContext ctx, int familiaAceroID)
        {
            return ctx.Peq.Any(x => x.FamiliaAceroID == familiaAceroID);
        }

        public static bool TieneSpool(SamContext ctx, int familiaAceroID)
        {
            return ctx.Spool.Any(x => x.FamiliaAcero1ID == familiaAceroID || x.FamiliaAcero2ID == familiaAceroID);
        }

        public static bool TieneWps(SamContext ctx, int familiaAceroID)
        {
            return ctx.Wps.Any(x => x.MaterialBase1ID == familiaAceroID || x.MaterialBase2ID == familiaAceroID);
        }

        public static bool NombreDuplicado(SamContext ctx, string nombreFamiliaAcero, int? familiaAceroID)
        {
            return ctx.FamiliaAcero.Any(x => x.Nombre == nombreFamiliaAcero && x.FamiliaAceroID != familiaAceroID);
        }
    }
}
