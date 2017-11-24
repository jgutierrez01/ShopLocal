using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesMaterialSpool
    {
        public static bool EstaDespachado(int materialSpoolId)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoMaterial.Any(x => x.MaterialSpoolID == materialSpoolId && x.TieneDespacho);
            }
        }

        public static bool EstaCortado(int materialSpoolId)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoMaterial.Any(x => x.MaterialSpoolID == materialSpoolId && x.TieneCorte.HasValue && x.TieneCorte.Value);
            }
        }


        public static bool EstaCongeladoParcial(int materialSpoolId)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.CongeladoParcial.Any(x => x.MaterialSpoolID == materialSpoolId);
            }
        }
    }
}
