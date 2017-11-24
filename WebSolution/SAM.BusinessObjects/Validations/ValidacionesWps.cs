using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesWps
    {
        public static bool TieneWpq(SamContext ctx, int wpsID)
        {
            int registro = ctx.Wpq.Where(x => x.WpsID == wpsID).Count();
            return registro > 0 ? true : false;
        }


        public static bool NombreDuplicado(SamContext ctx, string nombreWps, int? MaterialBase1, int? MaterialBase2, int? wpsID)
        {
            return ctx.Wps.Any(x => x.Nombre == nombreWps && x.MaterialBase1ID == MaterialBase1.Value && x.MaterialBase2ID == MaterialBase2.Value && x.WpsID != wpsID);
        }

        public static bool TieneJuntaSoldadura(SamContext ctx, int wpsID)
        {
            return ctx.JuntaSoldadura.Any(x => x.WpsID == wpsID);
        }

        public static bool TieneWpsProyecto(SamContext ctx, int wpsID)
        {
            return ctx.WpsProyecto.Any(x => x.WpsID == wpsID);
        }

        public static bool VerificaPWHT(SamContext ctx, int juntaSpoolID, int wpsRaizID, int wpsRellenoID)
        {
            int spoolID = ctx.JuntaSpool.Where(x => x.JuntaSpoolID == juntaSpoolID).Select(x => x.SpoolID).SingleOrDefault();
            bool spoolPWHT = ctx.Spool.Where(x => x.SpoolID == spoolID).Select(x => x.RequierePwht).SingleOrDefault();
            bool wpsRaizPWHT = ctx.Wps.Where(x => x.WpsID == wpsRaizID).Select(x => x.RequierePwht).SingleOrDefault();
            bool wpsRellenoPWHT = ctx.Wps.Where(x => x.WpsID == wpsRellenoID).Select(x => x.RequierePwht).SingleOrDefault();
            if ((spoolPWHT && wpsRaizPWHT && wpsRellenoPWHT) || (!spoolPWHT && !wpsRaizPWHT && !wpsRellenoPWHT))
            { return true; }
            else
            {
                throw new ExcepcionSoldadura(MensajesError.Excepcion_PWHTDiferente);
            }
        }
    }
}
