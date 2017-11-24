using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesInspeccionDimensionalPatio
    {
        public static bool ExisteReporteDimensional(SamContext ctx, string numeroEstimacion)
        {

            ReporteDimensional Ins =
                    ctx.ReporteDimensional
                        .Where(x => x.NumeroReporte == numeroEstimacion)
                        .SingleOrDefault();

            return Ins == null ? true : false;
        }
    }
}
