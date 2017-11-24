using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesFamiliaMaterial
    {
        public static bool TieneFamiliaAcero(SamContext ctx, int familiaMaterialID)
        {
            int registro = ctx.FamiliaAcero.Where(x => x.FamiliaMaterialID == familiaMaterialID).Count();
            return registro > 0 ? true : false;
        }

        public static bool NombreDuplicado(SamContext ctx, string nombre, int? familiaMaterialID)
        {
            return ctx.FamiliaMaterial.Any(x => x.Nombre == nombre && x.FamiliaMaterialID != familiaMaterialID);
        }
    }
}
