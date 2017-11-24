using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesCedula
    {
        public static bool TieneCedulaRelaciones(SamContext ctx, Cedula cedula)
        {
            bool tieneRelaciones = ctx.JuntaSpool.Any(x => x.Cedula == cedula.Codigo);
            tieneRelaciones |= ctx.Spool.Any(x => x.Cedula == cedula.Codigo);
            tieneRelaciones |= ctx.NumeroUnico.Any(x => x.Cedula == cedula.Codigo);
            tieneRelaciones |= ctx.KgTeorico.Any(x => x.CedulaID == cedula.CedulaID);
            tieneRelaciones |= ctx.Peq.Any(x => x.CedulaID == cedula.CedulaID);
            tieneRelaciones |= ctx.Espesor.Any(x => x.CedulaID == cedula.CedulaID);

            return tieneRelaciones;
        }
    }
}
