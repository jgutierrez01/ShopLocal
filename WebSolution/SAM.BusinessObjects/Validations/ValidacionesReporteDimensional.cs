using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesReporteDimensional
    {
        public static bool TieneReporteDimensionalDetalle(SamContext ctx, int reporteDimensionalID)
        {
            return ctx.ReporteDimensionalDetalle.Any(x => x.ReporteDimensionalID == reporteDimensionalID);
        }
    }
}
