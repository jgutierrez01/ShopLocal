using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesFabricante
    {
        public static bool NombreDuplicado(SamContext ctx, string nombreFabricante, int? fabricanteID)
        {
            return ctx.Fabricante.Any(x => x.Nombre == nombreFabricante && x.FabricanteID != fabricanteID);
        }

        public static bool TieneFabricanteProyecto(SamContext ctx, int fabricanteID)
        {
            return ctx.FabricanteProyecto.Any(x => x.FabricanteID == fabricanteID);
        }

        public static bool TieneColada(SamContext ctx, int fabricanteID)
        {
            return ctx.Colada.Any(x => x.FabricanteID == fabricanteID);
        }

        public static bool TieneNumeroUniso(SamContext ctx, int fabricanteID)
        {
            return ctx.NumeroUnico.Any(x => x.FabricanteID == fabricanteID);
        }
    }
}
