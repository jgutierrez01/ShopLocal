using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesInspeccionVisualPatio
    {
        public static bool ExisteInspeccionVisual(SamContext ctx, string numeroEstimacion)
        {

            InspeccionVisual Ins =
                    ctx.InspeccionVisual
                        .Where(x => x.NumeroReporte == numeroEstimacion)
                        .SingleOrDefault();

            return Ins == null ? true : false;
        }
    }
}
