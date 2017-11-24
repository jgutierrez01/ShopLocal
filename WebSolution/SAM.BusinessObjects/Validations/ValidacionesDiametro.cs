using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesDiametro
    {
        public static bool TieneDiametroRelaciones(SamContext ctx, Diametro diametro)
        {            
            bool tieneRelaciones = ctx.JuntaSpool.Any(x => x.Diametro == diametro.Valor);
            tieneRelaciones |= ctx.MaterialSpool.Any(x => x.Diametro1 == diametro.Valor || x.Diametro2 == diametro.Valor);
            tieneRelaciones |= ctx.Spool.Any(x => x.DiametroPlano == diametro.Valor);
            tieneRelaciones |= ctx.CorteSpool.Any(x => x.Diametro == diametro.Valor);
            tieneRelaciones |= ctx.NumeroUnico.Any(x => x.Diametro1 == diametro.Valor || x.Diametro2 == diametro.Valor);
            tieneRelaciones |= ctx.Peq.Any(x => x.DiametroID == diametro.DiametroID);
            tieneRelaciones |= ctx.KgTeorico.Any(x => x.DiametroID == diametro.DiametroID);
            tieneRelaciones |= ctx.Espesor.Any(x => x.DiametroID == diametro.DiametroID);

            return tieneRelaciones;
        }

    }
}
