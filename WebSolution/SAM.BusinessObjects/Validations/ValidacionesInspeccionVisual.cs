using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesInspeccionVisual
    {
        public static bool TieneJuntaInspeccionVisual(SamContext ctx,int inspeccionVisualID)
        {
            return ctx.JuntaInspeccionVisual.Any(x => x.InspeccionVisualID == inspeccionVisualID);
        }
    }
}
