using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesColada
    {
        public static bool ColadaDuplicada(SamContext ctx, string numeroColada, int? fabricanteID, int proyectoID, string numeroCertificado, int coladaID)
        {
            return ctx.Colada.Any(x => x.NumeroColada == numeroColada && (fabricanteID == -1 || fabricanteID == null || x.FabricanteID == fabricanteID) && x.ProyectoID == proyectoID && x.NumeroCertificado == numeroCertificado && x.ColadaID != coladaID);
        }

        public static bool TieneRelaciones(SamContext ctx, int coladaID)
        {
            return ctx.NumeroUnico.Any(x => x.ColadaID == coladaID);
        }
    }
}
